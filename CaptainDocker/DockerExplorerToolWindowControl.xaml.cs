namespace CaptainDocker
{
    using CaptainDocker.Forms;
    using CaptainDocker.Interfaces;
    using CaptainDocker.ValueObjects;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
            DockerClient dockerClient = new DockerClientConfiguration(
 new Uri("http://kubernetemaster:2375/"))
 .CreateClient();
            IList<ContainerListResponse> containers =await dockerClient.Containers.ListContainersAsync(
      new ContainersListParameters()
      {
          Limit = 10,
      });

            List<DockerTreeViewItem> dockerTreeViewItems = new List<DockerTreeViewItem>();
            List<ITreeNode> dockerContainerTreeViewItems = new List<ITreeNode>();
            foreach (var container in containers)
            {
                dockerContainerTreeViewItems.Add(new DockerContainerTreeViewItem() { Name = $"{container.ID} - {container.Image}" });
            }
            var images = await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true });
            List<ITreeNode>  dockerImageTreeViewItems = new List<ITreeNode>();
            foreach (var image in images)
            {
                dockerImageTreeViewItems.Add(new DockerImageTreeViewItem() { Name = string.Join(",", image.RepoTags) });
            }
            var nodes = new List<ITreeNode>
        {
            new DockerContainerTreeViewItem { Name = "Containers", ChildNodes = dockerContainerTreeViewItems },
            new DockerImageTreeViewItem { Name = "Images", ChildNodes = dockerImageTreeViewItems }
        };
            DockerTreeViewItem dockerTreeViewItem = new DockerTreeViewItem() { Name = dockerClient.Configuration.EndpointBaseUri.Host, ChildNodes = nodes };

            dockerTreeViewItems.Add(dockerTreeViewItem);
            dockerExplorerTreeView.ItemsSource = dockerTreeViewItems;
        }

        private void BuildImageButton_Click(object sender, RoutedEventArgs e)
        {
            new BuildImageForm().ShowDialog();
        }
    }
}