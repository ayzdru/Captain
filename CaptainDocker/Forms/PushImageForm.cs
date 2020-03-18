﻿using CaptainDocker.Data;
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
        public PushImageForm(Guid dockerConnectionId, string imageId, string imageName)
        {
            DockerConnectionId = dockerConnectionId;
            ImageId = imageId;
            ImageName = imageName;
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
                        var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true })).Where(q => q.RepoDigests != null && !q.RepoDigests.Any(r => r.Contains("<none>"))).ToList();
                        List<SelectListItem<ImageSelectListItem>> imageSelectListItems = new List<SelectListItem<ImageSelectListItem>>();
                        foreach (var image in images)
                        {
                            if (image.RepoTags != null)
                            {
                                foreach (var repoTag in image.RepoTags)
                                {
                                    ImageSelectListItem value = null;
                                    if (repoTag.Contains("/"))
                                    {
                                        value = new ImageSelectListItem(repoTag.Split('/')[0], image.ID);
                                    }
                                    else
                                    {
                                        value = new ImageSelectListItem(image.ID);
                                    }
                                    imageSelectListItems.Add(new SelectListItem<ImageSelectListItem>() { Text = repoTag, Value = value });
                                }
                            }
                            else
                            {
                                foreach (var repoDigest in image.RepoDigests)
                                {
                                    imageSelectListItems.Add(new SelectListItem<ImageSelectListItem>() { Text = repoDigest, Value = new ImageSelectListItem(image.ID) });
                                }
                            }                            
                        }
                        comboBoxImage.DataSource = imageSelectListItems;                        
                    }
                }
                if (_isFirstDockerConnectionSelect && comboBoxImage.Items.Count>0)
                {
                    _isFirstDockerConnectionSelect = false;
                    try
                    {
                        var selectedImage = comboBoxImage.Items.Cast<SelectListItem<ImageSelectListItem>>().Select((s, i) => new { Item = s, Index = i }).Where(s => s.Item.Value.ImageId == ImageId && s.Item.Text == ImageName).SingleOrDefault();
                        if (selectedImage != null)
                        {
                            comboBoxImage.SelectedIndex = selectedImage.Index;
                        }
                    }
                    catch (Exception ex)
                    {

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
                    var imageItem = comboBoxImage.SelectedItem as SelectListItem<ImageSelectListItem>;
                    if (imageItem != null)
                    {
                        var dockerRegistries = dbContext.DockerRegistries.Where(q => q.Address.Contains(imageItem.Value.RegistryUrl)).GetComboBoxItems().ToList();
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
}
