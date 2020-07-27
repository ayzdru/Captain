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
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
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
            DockerTreeViewItems = new ObservableCollection<DockerTreeViewItem>();
            openProjectSolutionLabel.Visibility = Visibility.Visible;
            Constants.Application.DatabaseConnection = null;
        }

        private void SolutionEvents_Opened()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            dockerExplorerToolbar.IsEnabled = true;
            openProjectSolutionLabel.Visibility = Visibility.Hidden;
            if (CaptainDockerPackage._dte.Solution.IsOpen)
            {
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
                    }

                }
                catch (Exception ex)
                {

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
            //SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            //WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            //if (!userSettingsStore.CollectionExists(SettingsForm.CaptainDockerCollectionName) && !userSettingsStore.PropertyExists(SettingsForm.CaptainDockerCollectionName, SettingsForm.CaptainDockerDatabaseConnection))
            //{
            //    userSettingsStore.CreateCollection(SettingsForm.CaptainDockerCollectionName);

            //    userSettingsStore.SetString(SettingsForm.CaptainDockerCollectionName, SettingsForm.CaptainDockerDatabaseConnection, databaseConnection);

            //}
            //else
            //{
            //    userSettingsStore.DeleteCollection(SettingsForm.CaptainDockerCollectionName);
            //    Constants.Application.DatabaseConnection = userSettingsStore.GetString(SettingsForm.CaptainDockerCollectionName, SettingsForm.CaptainDockerDatabaseConnection);
            //}            
            

        }
        public ObservableCollection<DockerTreeViewItem> DockerTreeViewItems { get; set; }

        private async void RefreshButton_ClickAsync(object sender, RoutedEventArgs e)
        {           
            refreshButton.IsEnabled = false;
                DockerTreeViewItems = new ObservableCollection<DockerTreeViewItem>();
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnections = dbContext.DockerConnections.ToList();
                    foreach (var dockerConnection in dockerConnections)
                    {
                    var nodes = new ObservableCollection<ITreeNode>();
                    try
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(
        new Uri(dockerConnection.EngineApiUrl))
        .CreateClient();
                        IList<ContainerListResponse> containers = await dockerClient.Containers.ListContainersAsync(
                  new ContainersListParameters()
                  {
                      Limit = 10,
                  });

                        ObservableCollection<ITreeNode> dockerContainerTreeViewItems = new ObservableCollection<ITreeNode>();
                        foreach (var container in containers)
                        {
                            dockerContainerTreeViewItems.Add(new DockerContainerTreeViewItem() { Name = $"{container.ID} - {container.Image}" });
                        }
                        var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }));
                        ObservableCollection<ITreeNode> dockerImageTreeViewItems = new ObservableCollection<ITreeNode>();
                        foreach (var image in images)
                        {
                            if (image.RepoTags != null)
                            {
                                foreach (var repoTag in image.RepoTags)
                                {
                                    dockerImageTreeViewItems.Add(new DockerImageTreeViewItem() { Name = repoTag, DockerConnectionId = dockerConnection.Id, ImageId = image.ID });
                                }
                            }
                            else
                            {
                                foreach (var repoDigest in image.RepoDigests)
                                {
                                    dockerImageTreeViewItems.Add(new DockerImageTreeViewItem() { Name = repoDigest, DockerConnectionId = dockerConnection.Id, ImageId = image.ID });
                                }
                            }
                        }


                        nodes.Add(new DockerContainerTitleTreeViewItem { Name = "Containers", ChildNodes = dockerContainerTreeViewItems });
                        nodes.Add(new DockerImageTitleTreeViewItem { Name = "Images", DockerConnectionId = dockerConnection.Id, ChildNodes = dockerImageTreeViewItems });
       
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, dockerConnection.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    
                        DockerTreeViewItem dockerTreeViewItem = new DockerTreeViewItem() { DockerConnectionId = dockerConnection.Id, Name = dockerConnection.Name, EngineApiUrl = dockerConnection.EngineApiUrl, ChildNodes = nodes };

                        DockerTreeViewItems.Add(dockerTreeViewItem);
                    }
                    dockerExplorerTreeView.ItemsSource = DockerTreeViewItems;

                }
                refreshButton.IsEnabled = true;
            
            
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
            var dockerConnectionId = Guid.Parse(menuItem.Tag.ToString());
            new BuildImageForm(dockerConnectionId).ShowDialog();
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
                      
                        if(!removed)
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
            if(addImageTagForm.ImageTag!=null)
            {
                var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[1].ChildNodes.Cast<DockerImageTreeViewItem>().Select((item, index) => new { Item = item, Index = index }).Where(d => d.Item.Name == tag.Name && d.Item.ImageId == tag.ImageId).SingleOrDefault();
                dockerTreeViewItem.ChildNodes[1].ChildNodes.Insert(dockerImageTreeViewItem.Index, new DockerImageTreeViewItem() { DockerConnectionId = addImageTagForm.DockerConnectionId, ImageId = addImageTagForm.ImageId, Name = $"{addImageTagForm.ImageTag.Repository}:{addImageTagForm.ImageTag.Tag}" });
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            new SettingsForm().ShowDialog();
        }

        private void PullImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerImageTitleTreeViewItem;
            var pullImageForm = new PullImageForm(tag.DockerConnectionId);
            pullImageForm.ShowDialog();
            if (pullImageForm.CreateImage != null)
            {
                var dockerTreeViewItem = DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                dockerTreeViewItem.ChildNodes[1].ChildNodes.Insert(0, new DockerImageTreeViewItem() { DockerConnectionId = pullImageForm.DockerConnectionId, ImageId =Guid.NewGuid().ToString(), Name = $"{pullImageForm.CreateImage.Repository}:{pullImageForm.CreateImage.Tag}" });
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
    }
}