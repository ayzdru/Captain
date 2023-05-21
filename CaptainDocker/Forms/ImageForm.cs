using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.Registry.DotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class ImageForm : BaseForm
    {
        public Guid DockerConnectionId { get; set; }
        public string ImageId { get; set; }
        private readonly DockerConnection _dockerConnection;
        private readonly DockerClient _dockerClient;
        public ImageForm(Guid dockerConnectionId, string imageId)
        {
            DockerConnectionId = dockerConnectionId;
            ImageId = imageId;
            InitializeComponent();
            using (var dbContext = new ApplicationDbContext())
            {
                _dockerConnection = dbContext.DockerConnections.GetById(DockerConnectionId).SingleOrDefault();
                if (_dockerConnection != null)
                {
                    try
                    {
                        _dockerClient = _dockerConnection.GetDockerClientConfiguration().CreateClient();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async Task InspectImage()
        {
            try
            {
                var image = await _dockerClient.Images.InspectImageAsync(ImageId);
                labelHeader.Text = string.Join(",", image.RepoTags);
                labelDescription.Text = image.ID;
                var options = new JsonSerializerOptions { WriteIndented = true };
                richTextBoxInspect.Text = JsonSerializer.Serialize(image, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void ImageForm_Load(object sender, EventArgs e)
        {
            await InspectImage();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
