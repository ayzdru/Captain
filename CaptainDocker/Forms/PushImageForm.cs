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
    public partial class PushImageForm : BaseForm
    {
        public Guid DockerConnectionId { get; set; }
        public string Image { get; set; }
        public PushImageForm(Guid dockerConnectionId)
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
            }
        }
        private async void ComboBoxDockerEngine_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDockerEngine.SelectedItem != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if(dockerConnection!=null)
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                        var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true })).Where(q=> q.RepoTags!= null && !q.RepoTags.Any(r => r.Contains("<none>"))).SelectMany(q=> q.RepoTags).Select(q=> {
                            var value = "";
                            if(q.Contains("/"))
                            {
                             value =  q.Split('/')[0];                              
                            }       
                            else
                            {
                                value = "registry.hub.docker.com";
                            }
                            return new SelectListItem<string>() { Text = q, Value = value };
                        }).ToList();
                        comboBoxImage.DataSource = images;
                        
                    }
                }
            }
        }
        private DockerRegistry DockerRegistry { get; set; }
        private async void ComboBoxRegistry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRegistry.SelectedItem != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerRegistryItem = comboBoxRegistry.SelectedItem as SelectListItem;
                    var dockerRegistry = dbContext.DockerRegistries.GetById(dockerRegistryItem.Value).SingleOrDefault();
                    DockerRegistry = dockerRegistry;                   
                }
            }
        }
        private async void buttonFinish_Click(object sender, EventArgs e)
        {
            if (comboBoxDockerEngine.SelectedItem != null && DockerRegistry != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();

                        var p = new Progress<JSONMessage>(status =>
                        {
                            buttonFinish.Text = status.Status;
                        });
                        await dockerClient.Images.PushImageAsync(
                            comboBoxImage.Text,
                            new ImagePushParameters()
                            {
                                 
                            },
                            new AuthConfig()
                            {
                                ServerAddress = DockerRegistry.Address,
                                Username = DockerRegistry.Username,
                                Password = DockerRegistry.Password
                            },
                            p
                            );
                    }
                }
            }
           
        }

        private void ComboBoxImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxImage.SelectedItem != null)
            {
                using (var dbContext= new  ApplicationDbContext())
                {
                    var imageItem = comboBoxImage.SelectedItem as SelectListItem<string>;
                    var dockerRegistries = dbContext.DockerRegistries.Where(q => q.Address.Contains(imageItem.Value)).GetComboBoxItems().ToList();
                    comboBoxRegistry.DataSource = dockerRegistries;
                    if (comboBoxRegistry.Items.Count > 0)
                    {
                        comboBoxRegistry.SelectedIndex = 0;
                    }
                }
              
            }
        }
    }
}
