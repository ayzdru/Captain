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
        public class CreateImageResponse
        {
            public CreateImageResponse(string repository, string tag)
            {
                Repository = repository;
                Tag = tag;
            }

            public string Repository { get; set; }
            public string Tag { get; set; }
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
        public CreateImageResponse CreateImage { get; set; } = null;
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
        private async void ButtonOk_Click(object sender, EventArgs e)
        {
            buttonOk.Enabled = false;
            if (comboBoxDockerEngine.SelectedItem != null && comboBoxRegistry.SelectedItem != null)
            {                
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        string id = "";
                        var p = new Progress<JSONMessage>(status =>
                        {
                            buttonOk.Text = status.Status;
                            if(status.Status.Contains("Digest: "))
                            {
                                id = status.Status.Replace("Digest: ", "");
                            }                            
                        });
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                       
                        try
                        {
                            var dockerRegistryItem = comboBoxRegistry.SelectedItem as SelectListItem;
                            var dockerRegistry = dbContext.DockerRegistries.GetById(dockerRegistryItem.Value).SingleOrDefault();
                            await dockerClient.Images.CreateImageAsync(new ImagesCreateParameters() { FromImage= $"{textBoxRepository.Text}:{textBoxTag.Text}"}, dockerRegistry.Address.Contains(Constants.Application.DefaultRegistry) ?  new AuthConfig() : new AuthConfig() { ServerAddress = dockerRegistry.Address, Username = dockerRegistry.Username, Password = dockerRegistry.Password }, p);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this.Text, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        this.CreateImage = new  CreateImageResponse(textBoxRepository.Text, textBoxTag.Text);
                        this.Close();
                    }
                }
            }
            buttonOk.Enabled = true;
        }
        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
