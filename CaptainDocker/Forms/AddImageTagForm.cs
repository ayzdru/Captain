using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.Registry.DotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class AddImageTagForm : BaseForm
    {
        public class ImageSelectListItem
        {
            public ImageSelectListItem(string imageId)
            {
                ImageId = imageId;
                RegistryUrl = "registry.hub.docker.com";
            }
            public ImageSelectListItem(string registryUrl, string imageId)
            {
                RegistryUrl = registryUrl;
                ImageId = imageId;
            }

            public string RegistryUrl { get; set; }
            public string ImageId { get; set; }
        }
        public Guid DockerConnectionId { get; set; }
        public string ImageId { get; set; }
        public string ImageName { get; set; }
        public bool _isFirstDockerConnectionSelect = true;
        public AddImageTagForm(Guid dockerConnectionId, string imageId, string imageName)
        {
            DockerConnectionId = dockerConnectionId;
            ImageId = imageId;
            ImageName = imageName;
            InitializeComponent();
        }       
    }
}
