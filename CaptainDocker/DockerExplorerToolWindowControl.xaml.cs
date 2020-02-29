namespace CaptainDocker
{
    using CaptainDocker.Data;
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

        private async void RefreshButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnections = dbContext.DockerConnections.ToList();
                List<DockerTreeViewItem> dockerTreeViewItems = new List<DockerTreeViewItem>();
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

                    List<ITreeNode> dockerContainerTreeViewItems = new List<ITreeNode>();
                    foreach (var container in containers)
                    {
                        dockerContainerTreeViewItems.Add(new DockerContainerTreeViewItem() { Name = $"{container.ID} - {container.Image}" });
                    }
                    var images = await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true });
                    List<ITreeNode> dockerImageTreeViewItems = new List<ITreeNode>();
                    foreach (var image in images)
                    {
                        dockerImageTreeViewItems.Add(new DockerImageTreeViewItem() { Name = string.Join(",", (image.RepoTags != null ? image.RepoTags : new string[] { "" })) });
                    }
                    var nodes = new List<ITreeNode>
        {
            new DockerContainerTreeViewItem { Name = "Containers", ChildNodes = dockerContainerTreeViewItems },
            new DockerImageTitleTreeViewItem { Name = "Images", ChildNodes = dockerImageTreeViewItems }
        };
                    DockerTreeViewItem dockerTreeViewItem = new DockerTreeViewItem() { Name = dockerConnection.Name, EngineApiUrl = dockerConnection.EngineApiUrl, ChildNodes = nodes };

                    dockerTreeViewItems.Add(dockerTreeViewItem);
                }
                dockerExplorerTreeView.ItemsSource = dockerTreeViewItems;

            }
        }

       
        private async void ImagePushContextMenuButton_Click(object sender, RoutedEventArgs e)
        {
            DockerClient dockerClient = new DockerClientConfiguration(
 new Uri("http://kubernetemaster:2375/"))
  .CreateClient();
            var p = new Progress<JSONMessage>(status =>
            {
                Console.WriteLine(status.Status);
            });
            await dockerClient.Images.PushImageAsync("kubernetemaster:5000/webapplication4:v1",
                new ImagePushParameters()
                {

                },
                new AuthConfig()
                {
                    ServerAddress = "http://kubernetemaster:5000",
                    Username = "username",
                    Password = "password"
                },
                p);

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
            new BuildImageForm().ShowDialog();
        }

        private void PushImageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new PushImageForm().ShowDialog();
        }
      
    }
}