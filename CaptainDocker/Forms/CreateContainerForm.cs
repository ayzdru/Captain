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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class CreateContainerForm : BaseForm
    {
        public class ImageSelectListItem
        {
            public ImageSelectListItem(string imageId)
            {
                ImageId = imageId;
                RegistryUrl =Constants.Application.DefaultRegistry;
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
        public CreateContainerForm(Guid dockerConnectionId, string imageId, string imageName)
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
                if (_isFirstDockerConnectionSelect && comboBoxImage.Items.Count>0)
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
      
        private async void buttonFinish_Click(object sender, EventArgs e)
        {
            if (comboBoxDockerEngine.SelectedItem != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {

                        var exposedPorts = dataGridViewExposedPorts.Rows.OfType<DataGridViewRow>().Select(s =>
                            new {Key = $"{s.Cells[0].Value.ToString()}/{s.Cells[1].Value.ToString()}"}).ToDictionary(t => t.Key, t => new EmptyStruct()); ;
                        var portBindings = dataGridViewExposedPorts.Rows.OfType<DataGridViewRow>().Select(s =>
                            new { Key = $"{s.Cells[0].Value.ToString()}" , Value = new List<PortBinding>(){ new PortBinding(){ HostIP = s.Cells[2].Value.ToString(), HostPort = s.Cells[3].Value.ToString() } } }).ToDictionary(t => t.Key, t => (IList<PortBinding>)t.Value); ;

                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                       await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters() { Image = comboBoxImage.SelectedText, Name = textBoxName.Text, Entrypoint = textBoxEntrypoint.Text.Split(' ').ToList(), Env = textBoxEnvironment.Text.Split(' ').ToList(),AttachStderr = checkBoxAttachToStderr.Checked, AttachStdin = checkBoxAttachToStdin.Checked, AttachStdout =  checkBoxAttachToStdout.Checked, Cmd = textBoxCommand.Text.Split(' ').ToList(),
                           ExposedPorts = exposedPorts,
                           HostConfig = new HostConfig()
                           {
                               PublishAllPorts = checkBoxPublishAllPorts.Checked,
                               PortBindings = portBindings
                           }
                           
                       });
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
                        
                    }
                }
              
            }
        }
    }
}
