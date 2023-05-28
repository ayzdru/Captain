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
    public partial class PullImageForm : BaseForm
    {
        private System.Threading.CancellationTokenSource _cts;
        public System.Threading.CancellationTokenSource Cts
        {
            get
            {
                if (_cts == null)
                {
                    _cts = new System.Threading.CancellationTokenSource();
                }
                return _cts;
            }
            set
            {
                _cts = value;
            }
        }
        public class ImageSelectListItem
        {
            public ImageSelectListItem(string imageId)
            {
                ImageId = imageId;
                RegistryUrl = Constants.Application.DefaultRegistry;
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
        public PullImageForm(Guid dockerConnectionId)
        {
            DockerConnectionId = dockerConnectionId;
            InitializeComponent();
        }

        private void PushImageForm_Load(object sender, EventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnections = dbContext.DockerConnections.GetComboBoxItems().ToList();
                comboBoxDockerEngine.DataSource = dockerConnections;
                comboBoxDockerEngine.SelectById(DockerConnectionId);
                var dockerRegistries = dbContext.DockerRegistries.GetComboBoxItems().ToList();
                comboBoxRegistry.DataSource = dockerRegistries;
            }
        }
        private void ProgressAppendText(string text)
        {
            if (text != null)
            {
                richTextBoxProgress.AppendText(text);
            }
        }
        private async void ButtonPull_Click(object sender, EventArgs e)
        {
            buttonPull.Enabled = false;
            if (comboBoxDockerEngine.SelectedItem != null && comboBoxRegistry.SelectedItem != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        try
                        {
                            var progress = new Progress<JSONMessage>(status =>
                            {
                                ProgressAppendText(status.Status);
                                ProgressAppendText(status.Stream);
                                ProgressAppendText(status.ProgressMessage);
                                ProgressAppendText(status.ErrorMessage);
                            });
                            using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                            {
                                var image = $"{textBoxRepository.Text}:{textBoxTag.Text}";
                                var dockerRegistryItem = comboBoxRegistry.SelectedItem as SelectListItem;
                                var dockerRegistry = dbContext.DockerRegistries.GetById(dockerRegistryItem.Value).SingleOrDefault();
                                await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters() { FromImage = image }, dockerRegistry.Address.Contains(Constants.Application.DefaultRegistry) ? new AuthConfig() : new AuthConfig() { ServerAddress = dockerRegistry.Address, Username = dockerRegistry.Username, Password = dockerRegistry.Password }, progress, Cts.Token);
                                MessageBox.Show("Pull Image process completed.\nPlease review the progress logs.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        richTextBoxProgress.ScrollToCaret();
                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            buttonPull.Enabled = true;
        }
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            Cts.Dispose();
            Cts = null;
            buttonPull.Enabled = true;
        }

        private void comboBoxDockerEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDockerEngine.SelectedItem != null)
            {
                var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                //todo: Response olarak göndermedim.
                DockerConnectionId = dockerEngineItem.Value;
            }
        }
    }
}
