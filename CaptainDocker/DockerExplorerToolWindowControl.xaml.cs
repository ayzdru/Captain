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
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Settings;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
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
        public DockerExplorerToolWindowControl()
        {
            this.InitializeComponent();
         
        }
        public ObservableCollection<DockerTreeViewItem> DockerTreeViewItems { get; set; }

        private async void RefreshButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            DockerTreeViewItems = new ObservableCollection<DockerTreeViewItem>();
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnections = dbContext.DockerConnections.ToList();
                foreach (var dockerConnection in dockerConnections)
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
                    var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true })).Where(q => q.RepoDigests != null && !q.RepoDigests.Any(r=> r.Contains("<none>")));
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
                    var nodes = new ObservableCollection<ITreeNode>
        {
            new DockerContainerTitleTreeViewItem { Name = "Containers", ChildNodes = dockerContainerTreeViewItems },
            new DockerImageTitleTreeViewItem { Name = "Images", DockerConnectionId = dockerConnection.Id, ChildNodes = dockerImageTreeViewItems }
        };
                    DockerTreeViewItem dockerTreeViewItem = new DockerTreeViewItem() {DockerConnectionId = dockerConnection.Id, Name = dockerConnection.Name, EngineApiUrl = dockerConnection.EngineApiUrl, ChildNodes = nodes };

                    DockerTreeViewItems.Add(dockerTreeViewItem);
                }
                dockerExplorerTreeView.ItemsSource = DockerTreeViewItems;

            }
        }


        private void NewDockerConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            new NewDockerConnectionForm().ShowDialog();
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
            var dockerConnectionId = Guid.Parse(menuItem.Tag.ToString());
            new PushImageForm(dockerConnectionId).ShowDialog();
        }

        private async void RemoveImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as System.Windows.Controls.MenuItem;
            var tag = menuItem.Tag as DockerImageTreeViewItem;
            if (MessageBox.Show($"{tag.Name} will be deleted?\nAre you sure?", "Image Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(tag.DockerConnectionId).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                        var result =  await dockerClient.Images.DeleteImageAsync(tag.Name, new ImageDeleteParameters()
                        {
                            Force = true
                        });
                        var dockerTreeViewItem =  DockerTreeViewItems.Where(d => d.DockerConnectionId == tag.DockerConnectionId).SingleOrDefault();
                        var dockerImageTreeViewItem = dockerTreeViewItem.ChildNodes[1].ChildNodes.Cast<DockerImageTreeViewItem>().Where(d => d.Name == tag.Name && d.ImageId == tag.ImageId).SingleOrDefault();
                        var removed = dockerTreeViewItem.ChildNodes[1].ChildNodes.Remove(dockerImageTreeViewItem);



                    }
                }
            }
        }
    }
}