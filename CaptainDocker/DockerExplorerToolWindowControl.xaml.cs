namespace CaptainDocker
{
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
            dockerExplorerTreeView.Items.Clear();
            DockerClient dockerClient = new DockerClientConfiguration(
 new Uri("http://kubernetemaster:2375/"))
 .CreateClient();
            IList<ContainerListResponse> containers =await dockerClient.Containers.ListContainersAsync(
      new ContainersListParameters()
      {
          Limit = 10,
      });

            DockerTreeViewItem root = new DockerTreeViewItem() { Title = dockerClient.Configuration.EndpointBaseUri.Host };
            DockerTreeViewItem childItem1 = new DockerTreeViewItem() { Title = "Containers" };
            foreach (var container in containers)
            {
                childItem1.Items.Add(new DockerTreeViewItem() { Title = $"{container.ID} - {container.Image}" });
            }
            root.Items.Add(childItem1);
            var images = await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true }); 
            DockerTreeViewItem childItem2 = new DockerTreeViewItem() { Title = "Images" };
            foreach (var image in images)
            {
                childItem2.Items.Add(new DockerTreeViewItem() { Title = string.Join(",", image.RepoTags) });
            }
            root.Items.Add(childItem2);
            dockerExplorerTreeView.Items.Add(root);
        }
    }
}