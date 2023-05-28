using CaptainDocker.Entities;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;

namespace CaptainDocker
{
    using CaptainDocker.Data;
    using CaptainDocker.Extensions;
    using CaptainDocker.Forms;
    using CaptainDocker.Interfaces;
    using CaptainDocker.Settings;
    using CaptainDocker.ValueObjects;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Settings;
    using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for DockerExplorerToolWindowControl.
    /// </summary>
    public partial class DockerExplorerToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DockerExplorerToolWindowControl"/> class.
        /// </summary>
        SolutionEvents _solutionEvents;
        private void SolutionEvents_BeforeClosing()
        {
            dockerExplorerToolbar.IsEnabled = false;
            DockerTreeViewItems.Clear();
            openProjectSolutionLabel.Visibility = Visibility.Visible;
            Constants.Application.DatabaseConnection = null;
        }
        private async void SolutionEvents_Opened()
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            if (CaptainDockerPackage._dte.Solution.IsOpen == true && dockerExplorerToolbar.IsEnabled == false)
            {
                try
                {
                    dockerExplorerToolbar.IsEnabled = true;
                    openProjectSolutionLabel.Visibility = Visibility.Hidden;
                    var databaseConnection = Path.GetDirectoryName(CaptainDockerPackage._dte.Solution.FullName);
                    Constants.Application.DatabaseConnection = databaseConnection;

                    if (!string.IsNullOrEmpty(Constants.Application.DatabaseConnection))
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            dbContext.Database.EnsureCreated();
                        }
                        await RefreshAsync();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Database", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public DockerExplorerToolWindowControl()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _solutionEvents = ((Events2)CaptainDockerPackage._dte.Events).SolutionEvents;
            _solutionEvents.Opened += SolutionEvents_Opened;
            _solutionEvents.BeforeClosing += SolutionEvents_BeforeClosing;
            this.InitializeComponent();
            SolutionEvents_Opened();
        }

        public ObservableCollection<DockerTreeViewItem> DockerTreeViewItems { get; set; } = new ObservableCollection<DockerTreeViewItem>();
        private async Task<DockerTreeViewItem> GetDockerTreeViewItemAsync(DockerConnection dockerConnection)
        {
            try
            {
                var dockerConnectionChildNodes = new ObservableCollection<ITreeNode>();
                using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                {
                    dockerConnectionChildNodes.Add(new DockerContainerTitleTreeViewItem { DockerConnectionId = dockerConnection.Id, Name = "Containers", ChildNodes = await GetDockerContainerTreeViewItemsAsync(dockerConnection.Id, dockerClient) });
                    dockerConnectionChildNodes.Add(new DockerImageTitleTreeViewItem { Name = "Images", DockerConnectionId = dockerConnection.Id, ChildNodes = await GetDockerImageTreeViewItemsAsync(dockerConnection.Id, dockerClient) });
                }
                return new DockerTreeViewItem() { DockerConnectionId = dockerConnection.Id, Name = dockerConnection.Name, EngineApiUrl = dockerConnection.EngineApiUrl, ChildNodes = dockerConnectionChildNodes };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, dockerConnection.Name, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
        private async Task<ObservableCollection<ITreeNode>> GetDockerContainerTreeViewItemsAsync(Guid dockerConnectionId, DockerClient dockerClient)
        {
            var containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { Limit = long.MaxValue });
            ObservableCollection<ITreeNode> dockerContainerTreeViewItems = new ObservableCollection<ITreeNode>();
            foreach (var container in containers)
            {
                dockerContainerTreeViewItems.Add(new DockerContainerTreeViewItem() { DockerConnectionId = dockerConnectionId, ContainerId = container.ID, Name = $"[{container.Status}] {string.Join(" ", container.Names)} - {container.Image} - {container.ID}" });
            }
            return dockerContainerTreeViewItems;
        }
        private async Task<ObservableCollection<ITreeNode>> GetDockerImageTreeViewItemsAsync(Guid dockerConnectionId, DockerClient dockerClient)
        {
            var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters()
            {
                All = true,
                Filters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        {
                            "dangling",
                            new Dictionary<string, bool>
                            {
                                { "false", true }
                            }
                        }
                    }
            }));
            ObservableCollection<ITreeNode> dockerImageTreeViewItems = new ObservableCollection<ITreeNode>();
            foreach (var image in images)
            {
                if (image.RepoTags != null)
                {
                    foreach (var repoTag in image.RepoTags)
                    {
                        dockerImageTreeViewItems.Add(new DockerImageTreeViewItem() { Name = repoTag, DockerConnectionId = dockerConnectionId, ImageId = image.ID });
                    }
                }
                else
                {
                    foreach (var repoDigest in image.RepoDigests)
                    {
                        dockerImageTreeViewItems.Add(new DockerImageTreeViewItem() { Name = repoDigest, DockerConnectionId = dockerConnectionId, ImageId = image.ID });
                    }
                }
            }
            return dockerImageTreeViewItems;
        }
        private async Task RefreshAsync()
        {
            DockerTreeViewItems.Clear();
            refreshButton.IsEnabled = false;
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnections = dbContext.DockerConnections.ToList();
                foreach (var dockerConnection in dockerConnections)
                {
                    var dockerTreeViewItem = await GetDockerTreeViewItemAsync(dockerConnection);
                    if (dockerTreeViewItem != null)
                    {
                        DockerTreeViewItems.Add(dockerTreeViewItem);
                    }
                }
                dockerExplorerTreeView.ItemsSource = DockerTreeViewItems;

            }
            refreshButton.IsEnabled = true;

        }
        private async void CreateContainerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                Guid dockerConnectionId = Guid.Empty;
                string imageId = null;
                string imageName = null;
                if (menuItem.Tag is DockerImageTreeViewItem)
                {
                    var tag = menuItem.Tag as DockerImageTreeViewItem;
                    dockerConnectionId = tag.DockerConnectionId;
                    imageId = tag.ImageId;
                    imageName = tag.Name;
                }
                else if (menuItem.Tag is DockerContainerTitleTreeViewItem)
                {
                    var tag = menuItem.Tag as DockerContainerTitleTreeViewItem;
                    dockerConnectionId = tag.DockerConnectionId;
                }
                new CreateContainerForm(dockerConnectionId, imageId, imageName).ShowDialog();
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {

                        using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                        {
                           await RefreshContainerChildNodes(dockerConnection.Id, dockerClient);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Container", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void RefreshButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await RefreshAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private async void NewDockerConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            new DockerConnectionForm().ShowDialog();
            try
            {
                await RefreshAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "New Docker Connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageDockerRegistryButton_Click(object sender, RoutedEventArgs e)
        {
            new ManageDockerRegistryForm().ShowDialog();
        }


        private async void BuildImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTitleTreeViewItem;
                new BuildImageForm(tag.DockerConnectionId).ShowDialog();
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {

                        using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                        {
                            await RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Build Image", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PushImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTreeViewItem;
                new PushImageForm(tag.DockerConnectionId, tag.ImageId, tag.Name).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Push Image", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async void RemoveImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTreeViewItem;
                if (MessageBox.Show($"{tag.Name} will be deleted?\nAre you sure?", "Image", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                        if (dockerConnection != null)
                        {
                            var removed = false;


                            using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                            {
                                var result = await dockerClient.Images.DeleteImageAsync(tag.Name, new ImageDeleteParameters()
                                {
                                    Force = true
                                });
                                var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                                var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[1].ChildNodes.Cast<DockerImageTreeViewItem>().Where(d => d.Name == tag.Name && d.ImageId == tag.ImageId).SingleOrDefault();
                                removed = dockerTreeViewItem.ChildNodes[1].ChildNodes.Remove(dockerImageTreeViewItem);
                            }


                            if (!removed)
                            {
                                MessageBox.Show($"{tag.Name} image could not be deleted.", "Image", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Remove Image", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddImageTagMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTreeViewItem;
                var addImageTagForm = new AddImageTagForm(tag.DockerConnectionId, tag.ImageId, tag.Name);
                addImageTagForm.ShowDialog();
                if (addImageTagForm.ImageTag != null)
                {
                    var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                    var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[1].ChildNodes.Cast<DockerImageTreeViewItem>().Select((item, index) => new { Item = item, Index = index }).Where(d => d.Item.Name == tag.Name && d.Item.ImageId == tag.ImageId).SingleOrDefault();
                    dockerTreeViewItem.ChildNodes[1].ChildNodes.Insert(dockerImageTreeViewItem.Index, new DockerImageTreeViewItem() { DockerConnectionId = addImageTagForm.DockerConnectionId, ImageId = addImageTagForm.ImageId, Name = $"{addImageTagForm.ImageTag.Repository}:{addImageTagForm.ImageTag.Tag}" });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Image Tag", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void PullImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTitleTreeViewItem;
                var pullImageForm = new PullImageForm(tag.DockerConnectionId);
                pullImageForm.ShowDialog();
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {

                        using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                        {
                           await RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Pull Image", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task RefreshImagesChildNodes(Guid dockerConnectionId, DockerClient dockerClient)
        {
            var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == dockerConnectionId).SingleOrDefault();
            for (int i = dockerTreeViewItem.ChildNodes[1].ChildNodes.Count - 1; i >= 0; i--)
            {
                dockerTreeViewItem.ChildNodes[1].ChildNodes.RemoveAt(i);
            }
            var dockerImageTreeViewItems = await GetDockerImageTreeViewItemsAsync(dockerConnectionId, dockerClient);
            foreach (var dockerImageTreeViewItem in dockerImageTreeViewItems)
            {
                dockerTreeViewItem.ChildNodes[1].ChildNodes.Add(dockerImageTreeViewItem);
            }
        }
        private async void RemoveDanglingImagesMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Dangling Images will be removed?\nAre you sure?", "Remove Dangling Images", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var menuItem = sender as System.Windows.Controls.MenuItem;
                    var tag = menuItem.Tag as DockerImageTitleTreeViewItem;

                    using (var dbContext = new ApplicationDbContext())
                    {
                        var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                        if (dockerConnection != null)
                        {
                            using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                            {
                                var result = await dockerClient.Images.PruneImagesAsync(new ImagesPruneParameters()
                                {
                                    Filters = new Dictionary<string, IDictionary<string, bool>>
                {
                    {
                        "dangling",
                        new Dictionary<string, bool>
                        {
                            { "1", true}
                        }
                    }
                }
                                });
                                await RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Remove Dangling Images", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void RefreshImageMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTitleTreeViewItem;


                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {

                        using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                        {
                            await RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                        }


                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh Images", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void RefreshDockerConnectionMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerTreeViewItem;
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                        var newDockerTreeViewItem = await GetDockerTreeViewItemAsync(dockerConnection);
                        if (newDockerTreeViewItem != null)
                        {
                            dockerTreeViewItem.Name = newDockerTreeViewItem.Name;
                            dockerTreeViewItem.EngineApiUrl = newDockerTreeViewItem.EngineApiUrl;
                            dockerTreeViewItem.DockerConnectionId = newDockerTreeViewItem.DockerConnectionId;
                            dockerTreeViewItem.ChildNodes = newDockerTreeViewItem.ChildNodes;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private async void EditDockerConnectionMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerTreeViewItem;
                new DockerConnectionForm(tag.DockerConnectionId).ShowDialog();
                await RefreshAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Edit Docker Connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RemoveDockerConnectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerTreeViewItem;
                if (MessageBox.Show($"{tag.Name} will be deleted?\nAre you sure?", "Docker Connection", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                        dbContext.DockerConnections.Remove(dockerConnection);
                        dbContext.SaveChanges();
                        var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                        DockerTreeViewItems.Remove(dockerTreeViewItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Remove Docker Connection", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task RefreshContainerChildNodes(Guid dockerConnectionId, DockerClient dockerClient)
        {
            try
            {
                var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == dockerConnectionId).SingleOrDefault();
                for (int i = dockerTreeViewItem.ChildNodes[0].ChildNodes.Count - 1; i >= 0; i--)
                {
                    dockerTreeViewItem.ChildNodes[0].ChildNodes.RemoveAt(i);
                }
                var dockerContainerTreeViewItems = await GetDockerContainerTreeViewItemsAsync(dockerConnectionId, dockerClient);
                foreach (var dockerContainerTreeViewItem in dockerContainerTreeViewItems)
                {
                    dockerTreeViewItem.ChildNodes[0].ChildNodes.Add(dockerContainerTreeViewItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh Container", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private async void RefreshContainerMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerContainerTitleTreeViewItem;
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                        {
                           await RefreshContainerChildNodes(dockerConnection.Id, dockerClient);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh Container", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async void RemoveContainerMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerContainerTreeViewItem;
                if (MessageBox.Show($"{tag.Name} will be deleted?\nAre you sure?", "Docker Container", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                        if (dockerConnection != null)
                        {

                            using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                            {
                                await dockerClient.Containers.RemoveContainerAsync(tag.ContainerId, new ContainerRemoveParameters()
                                {
                                    Force = true
                                });
                                var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                                var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[0].ChildNodes.Cast<DockerContainerTreeViewItem>().Where(d => d.Name == tag.Name && d.ContainerId == tag.ContainerId).SingleOrDefault();
                                dockerTreeViewItem.ChildNodes[0].ChildNodes.Remove(dockerImageTreeViewItem);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Remove Container", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void OnItemMouseDoubleClick(object sender, MouseButtonEventArgs args)
        {
            try
            {
                if (sender is TreeViewItem)
                {
                    if (!((TreeViewItem)sender).IsSelected)
                    {
                        return;
                    }
                    var header = (sender as TreeViewItem).Header;
                    if (header != null)
                    {
                        if (header is DockerContainerTreeViewItem)
                        {
                            var containerTreeViewItem = (DockerContainerTreeViewItem)header;
                            new ContainerForm(containerTreeViewItem.DockerConnectionId, containerTreeViewItem.ContainerId).ShowDialog();
                        }
                        else if (header is DockerImageTreeViewItem)
                        {
                            var imageTreeViewItem = (DockerImageTreeViewItem)header;
                            new ImageForm(imageTreeViewItem.DockerConnectionId, imageTreeViewItem.ImageId).ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Captain Docker", MessageBoxButton.OK, MessageBoxImage.Error);
            }            
        }
    }
}