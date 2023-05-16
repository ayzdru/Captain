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
        private async void IsSolutionOpen()
        {
            if (CaptainDockerPackage._dte.Solution.IsOpen == true && dockerExplorerToolbar.IsEnabled == false)
            {
                dockerExplorerToolbar.IsEnabled = true;
                openProjectSolutionLabel.Visibility = Visibility.Hidden;
                var databaseConnection = Path.GetDirectoryName(CaptainDockerPackage._dte.Solution.FullName);
                Constants.Application.DatabaseConnection = databaseConnection;
                try
                {
                    if (!string.IsNullOrEmpty(Constants.Application.DatabaseConnection))
                    {
                        using (var dbContext = new ApplicationDbContext())
                        {
                            dbContext.Database.EnsureCreated();
                        }
                        RefreshAsync();
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
        private void SolutionEvents_Opened()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            IsSolutionOpen();
        }

        public DockerExplorerToolWindowControl()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _solutionEvents = ((Events2)CaptainDockerPackage._dte.Events).SolutionEvents;
            _solutionEvents.Opened += SolutionEvents_Opened;
            _solutionEvents.BeforeClosing += SolutionEvents_BeforeClosing;
            this.InitializeComponent();
            IsSolutionOpen();
        }

        public ObservableCollection<DockerTreeViewItem> DockerTreeViewItems { get; set; } = new ObservableCollection<DockerTreeViewItem>();
        private async Task<DockerTreeViewItem> GetDockerTreeViewItem(Guid dockerConnectionId, string dockerConnectionName, string dockerConnectionEngineApiUrl)
        {
            var dockerConnectionChildNodes = new ObservableCollection<ITreeNode>();
            try
            {
                DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnectionEngineApiUrl)).CreateClient();
                var containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { Limit = long.MaxValue });
                var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }));
                dockerConnectionChildNodes.Add(new DockerContainerTitleTreeViewItem { DockerConnectionId = dockerConnectionId, Name = "Containers", ChildNodes = await GetDockerContainerTreeViewItems(dockerConnectionId, containers) });
                dockerConnectionChildNodes.Add(new DockerImageTitleTreeViewItem { Name = "Images", DockerConnectionId = dockerConnectionId, ChildNodes = await GetDockerImageTreeViewItems(dockerConnectionId, images) });

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, dockerConnectionName, MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return new DockerTreeViewItem() { DockerConnectionId = dockerConnectionId, Name = dockerConnectionName, EngineApiUrl = dockerConnectionEngineApiUrl, ChildNodes = dockerConnectionChildNodes };

        }
        private async Task<ObservableCollection<ITreeNode>> GetDockerContainerTreeViewItems(Guid dockerConnectionId, IList<ContainerListResponse> containers)
        {
            ObservableCollection<ITreeNode> dockerContainerTreeViewItems = new ObservableCollection<ITreeNode>();
            foreach (var container in containers)
            {
                dockerContainerTreeViewItems.Add(new DockerContainerTreeViewItem() { DockerConnectionId = dockerConnectionId, ContainerId = container.ID, Name = $"[{container.Status}] {container.Image} - {container.ID}" });
            }
            return dockerContainerTreeViewItems;
        }
        private async Task<ObservableCollection<ITreeNode>> GetDockerImageTreeViewItems(Guid dockerConnectionId, IList<ImagesListResponse> images)
        {
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
        private async void RefreshAsync()
        {
            DockerTreeViewItems.Clear();
            refreshButton.IsEnabled = false;
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnections = dbContext.DockerConnections.ToList();
                foreach (var dockerConnection in dockerConnections)
                {
                    try
                    {
                        DockerTreeViewItems.Add(await GetDockerTreeViewItem(dockerConnection.Id, dockerConnection.Name, dockerConnection.EngineApiUrl));
                    }
                    catch (Exception)
                    {

                    }

                }
                dockerExplorerTreeView.ItemsSource = DockerTreeViewItems;

            }
            refreshButton.IsEnabled = true;

        }
        private void CreateContainerMenuItem_Click(object sender, RoutedEventArgs e)
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
            else if (menuItem.Tag is DockerContainerTreeViewItem)
            {
                var tag = menuItem.Tag as DockerContainerTreeViewItem;
                dockerConnectionId = tag.DockerConnectionId;
            }
            new CreateContainerForm(dockerConnectionId, imageId, imageName).ShowDialog();
        }
        private async void RefreshButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            RefreshAsync();

        }


        private void NewDockerConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            new NewDockerConnectionForm().ShowDialog();
            RefreshButton_ClickAsync(null, null);
        }

        private void ManageDockerRegistryButton_Click(object sender, RoutedEventArgs e)
        {
            new ManageDockerRegistryForm().ShowDialog();
        }


        private void BuildImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerImageTitleTreeViewItem;
            new BuildImageForm(tag.DockerConnectionId).ShowDialog();
        }

        private void PushImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerImageTreeViewItem;
            new PushImageForm(tag.DockerConnectionId, tag.ImageId, tag.Name).ShowDialog();
        }

        private async void RemoveImageMenuItem_Click(object sender, RoutedEventArgs e)
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

                        try
                        {
                            DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                            var result = await dockerClient.Images.DeleteImageAsync(tag.Name, new ImageDeleteParameters()
                            {
                                Force = true
                            });
                            var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                            var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[1].ChildNodes.Cast<DockerImageTreeViewItem>().Where(d => d.Name == tag.Name && d.ImageId == tag.ImageId).SingleOrDefault();
                            removed = dockerTreeViewItem.ChildNodes[1].ChildNodes.Remove(dockerImageTreeViewItem);
                        }
                        catch (Exception exception)
                        {

                        }

                        if (!removed)
                        {
                            MessageBox.Show($"{tag.Name} image could not be deleted.", "Image", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
            }
        }

        private void AddImageTagMenuItem_Click(object sender, RoutedEventArgs e)
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

        private void PullImageMenuItem_Click(object sender, RoutedEventArgs e)
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
                    try
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                        RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        private async void RefreshImagesChildNodes(Guid dockerConnectionId, DockerClient dockerClient)
        {
            var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == dockerConnectionId).SingleOrDefault();
            for (int i = dockerTreeViewItem.ChildNodes[1].ChildNodes.Count - 1; i >= 0; i--)
            {
                dockerTreeViewItem.ChildNodes[1].ChildNodes.RemoveAt(i);
            }

            var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }));
            var dockerImageTreeViewItems = await GetDockerImageTreeViewItems(dockerConnectionId, images);
            foreach (var dockerImageTreeViewItem in dockerImageTreeViewItems)
            {
                dockerTreeViewItem.ChildNodes[1].ChildNodes.Add(dockerImageTreeViewItem);
            }
        }
        private async void RemoveDanglingImagesMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Dangling Images will be removed?\nAre you sure?", "Remove Dangling Images", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var menuItem = sender as System.Windows.Controls.MenuItem;
                var tag = menuItem.Tag as DockerImageTitleTreeViewItem;

                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        try
                        {
                            DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
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
                            RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                        }
                        catch (Exception exception)
                        {

                        }
                    }
                }
            }
        }
        private async void RefreshImageMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerImageTitleTreeViewItem;


            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                if (dockerConnection != null)
                {
                    try
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                        RefreshImagesChildNodes(dockerConnection.Id, dockerClient);
                    }
                    catch (Exception exception)
                    {

                    }

                }
            }
        }
        private async void RefreshDockerConnectionMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerTreeViewItem;
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                if (dockerConnection != null)
                {
                    var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                    var newDockerTreeViewItem = await GetDockerTreeViewItem(dockerConnection.Id, dockerConnection.Name, dockerConnection.EngineApiUrl);
                    dockerTreeViewItem.Name = newDockerTreeViewItem.Name;
                    dockerTreeViewItem.EngineApiUrl = newDockerTreeViewItem.EngineApiUrl;
                    dockerTreeViewItem.DockerConnectionId = newDockerTreeViewItem.DockerConnectionId;
                    dockerTreeViewItem.ChildNodes = newDockerTreeViewItem.ChildNodes;
                }
            }
        }
        private void RemoveDockerConnectionMenuItem_Click(object sender, RoutedEventArgs e)
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
        private async void RefreshContainerChildNodes(Guid dockerConnectionId, DockerClient dockerClient)
        {
            var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == dockerConnectionId).SingleOrDefault();
            for (int i = dockerTreeViewItem.ChildNodes[0].ChildNodes.Count - 1; i >= 0; i--)
            {
                dockerTreeViewItem.ChildNodes[0].ChildNodes.RemoveAt(i);
            }

            var containers = await dockerClient.Containers.ListContainersAsync(new ContainersListParameters() { Limit = long.MaxValue });
            var dockerContainerTreeViewItems = await GetDockerContainerTreeViewItems(dockerConnectionId, containers);
            foreach (var dockerContainerTreeViewItem in dockerContainerTreeViewItems)
            {
                dockerTreeViewItem.ChildNodes[0].ChildNodes.Add(dockerContainerTreeViewItem);
            }
        }
        private async void RefreshContainerMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerContainerTitleTreeViewItem;


            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                if (dockerConnection != null)
                {
                    try
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                        RefreshContainerChildNodes(dockerConnection.Id, dockerClient);
                    }
                    catch (Exception exception)
                    {

                    }

                }
            }
        }
        private async void RemoveContainerMenuItem_ClickAsync(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerContainerTreeViewItem;
            if (MessageBox.Show($"{tag.Name} will be deleted?\nAre you sure?", "Docker Container", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                    await dockerClient.Containers.RemoveContainerAsync(tag.ContainerId, new ContainerRemoveParameters()
                    {
                        Force = true
                    });
                    var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                    var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[0].ChildNodes.Cast<DockerContainerTreeViewItem>().Where(d => d.Name == tag.Name && d.ContainerId == tag.ContainerId).SingleOrDefault();
                    dockerTreeViewItem.ChildNodes[0].ChildNodes.Remove(dockerImageTreeViewItem);
                }
            }
        }
    }
}