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
        public class ImageTagResponse
        {
            public ImageTagResponse(string repository, string tag)
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
        public string ImageId { get; set; }
        public string ImageName { get; set; }
        public bool _isFirstDockerConnectionSelect = true;
        public ImageTagResponse ImageTag { get; set; } = null;
        public AddImageTagForm(Guid dockerConnectionId, string imageId, string imageName)
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
                    DockerConnectionId = dockerEngineItem.Value;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        try
                        {
                            using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                            {
                                var images = (await dockerClient.Images.ListImagesAsync(new ImagesListParameters() { All = true })).ToList();
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
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (_isFirstDockerConnectionSelect && comboBoxImage.Items.Count > 0)
                {
                    _isFirstDockerConnectionSelect = false;
                    try
                    {
                        var selectedImage = comboBoxImage.Items.Cast<SelectListItem<ImageSelectListItem>>().Select((item, index) => new { Item = item, Index = index }).Where(s => s.Item.Value.ImageId == ImageId && s.Item.Text == ImageName).SingleOrDefault();
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
        private async void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (comboBoxImage.SelectedItem != null)
            {
                var imageItem = comboBoxImage.SelectedItem as SelectListItem<ImageSelectListItem>;
                if (imageItem != null)
                {
                    using (var dbContext = new ApplicationDbContext())
                    {
                        var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                        var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                        if (dockerConnection != null)
                        {
                            try
                            {
                                using (var dockerClient = dockerConnection.GetDockerClientConfiguration().CreateClient())
                                {
                                    await dockerClient.Images.TagImageAsync(imageItem.Text, new ImageTagParameters() { RepositoryName = textBoxRepository.Text, Tag = textBoxTag.Text });
                                    this.ImageTag = new ImageTagResponse(textBoxRepository.Text, textBoxTag.Text);
                                    MessageBox.Show($"{textBoxRepository.Text}-{textBoxTag.Text} image tag added.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
