using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.Registry.DotNet;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
        public CreateContainerForm(Guid dockerConnectionId, string imageId, string imageName)
        {
            DockerConnectionId = dockerConnectionId;
            ImageId = imageId;
            ImageName = imageName;
            InitializeComponent();
        }

        private void CreateContainerForm_Load(object sender, EventArgs e)
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
                    if (dockerConnection != null)
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
                if (_isFirstDockerConnectionSelect && comboBoxImage.Items.Count > 0 && string.IsNullOrEmpty(ImageId) == false && string.IsNullOrEmpty(ImageName) == false)
                {
                    _isFirstDockerConnectionSelect = false;
                    var selectedImage = comboBoxImage.Items.Cast<SelectListItem<ImageSelectListItem>>().Select((item, index) => new { Item = item, Index = index }).Where(s => s.Item.Value.ImageId == ImageId && s.Item.Text == ImageName).SingleOrDefault();
                    if (selectedImage != null)
                    {
                        comboBoxImage.SelectedIndex = selectedImage.Index;
                    }
                }
            }
        }

        private async void buttonCreate_Click(object sender, EventArgs e)
        {
            if (comboBoxDockerEngine.SelectedItem != null)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                    var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        var exposedPorts = new Dictionary<string, EmptyStruct>();
                        var portBindings = new Dictionary<string, IList<PortBinding>>();
                        var ports = dataGridViewExposedPorts.Rows.OfType<DataGridViewRow>().Where(q => q.Cells[0].Value != null && q.Cells[1].Value != null);
                        var bindings = dataGridViewExposedPorts.Rows.OfType<DataGridViewRow>().Where(q => q.Cells[0].Value != null && q.Cells[2].Value != null && q.Cells[3].Value != null);
                        if (ports.Count() > 0 && bindings.Count() > 0)
                        {
                            exposedPorts = ports.Select(s =>
                            new { Key = $"{s.Cells[0].Value.ToString()}/{s.Cells[1].Value.ToString()}" }).ToDictionary(t => t.Key, t => new EmptyStruct()); ;
                            portBindings = bindings.Select(s =>
                                new { Key = $"{s.Cells[0].Value.ToString()}", Value = new List<PortBinding>() { new PortBinding() { HostIP = s.Cells[2].Value.ToString(), HostPort = s.Cells[3].Value.ToString() } } }).ToDictionary(t => t.Key, t => (IList<PortBinding>)t.Value); ;

                        }

                        var env = textBoxEnvironment.Text.TrimStart().TrimEnd().Split(new char[] { ',' },
    StringSplitOptions.RemoveEmptyEntries).ToList();
                        var entrypoint = textBoxEntrypoint.Text.TrimStart().TrimEnd().Split(new char[] { ',' },
    StringSplitOptions.RemoveEmptyEntries).ToList();
                        var cmd = textBoxCommand.Text.TrimStart().TrimEnd().Split(',').ToList();

                        if (env.Count == 0)
                        {
                            env = null;
                        }
                        if (entrypoint.Count == 0) 
                        { 
                            entrypoint = null; 
                        }                       
                        DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                        await dockerClient.Containers.CreateContainerAsync(new CreateContainerParameters()
                        {
                            Image = comboBoxImage.Text,
                            Name = textBoxName.Text,
                            Entrypoint = entrypoint,
                            Env = env,
                            AttachStderr = checkBoxAttachToStderr.Checked,
                            AttachStdin = checkBoxAttachToStdin.Checked,
                            AttachStdout = checkBoxAttachToStdout.Checked,
                            Cmd = cmd,
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
                using (var dbContext = new ApplicationDbContext())
                {
                    var imageItem = comboBoxImage.SelectedItem as SelectListItem<ImageSelectListItem>;
                    if (imageItem != null)
                    {

                    }
                }

            }
        }

        private void checkBoxPublishAllPorts_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxSpecifyExposedPorts.Enabled = !checkBoxPublishAllPorts.Checked;
        }

        private void removeSpecifyExposedPortsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewExposedPorts.SelectedRows.Count > 0)
            {
                dataGridViewExposedPorts.Rows.Remove(dataGridViewExposedPorts.SelectedRows[0]);
            }
        }
    }
}
