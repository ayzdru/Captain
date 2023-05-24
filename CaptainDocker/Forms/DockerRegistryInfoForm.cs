using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.Registry.DotNet;
using Docker.Registry.DotNet.Authentication;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class DockerRegistryInfoForm : BaseForm
    {
        private class Repository
        {
            public string Name { get; set; }
            public List<string> Tags { get; set; } = new List<string>();
        }
        private class Catalog
        {
            public List<Repository> Repositories { get; set; } = new List<Repository> { };
        }
        public Guid DockerRegistryId { get; set; }
        private readonly DockerRegistry _dockerRegistry;
        public DockerRegistryInfoForm(Guid dockerRegistryId)
        {
            DockerRegistryId = dockerRegistryId;        
            InitializeComponent();
            using (var dbContext = new ApplicationDbContext())
            {
                _dockerRegistry = dbContext.DockerRegistries.GetById(DockerRegistryId).SingleOrDefault();
                if (_dockerRegistry != null)
                {
                    try
                    {
                        labelHeader.Text = _dockerRegistry.Name;
                        labelDescription.Text = _dockerRegistry.Address;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, _dockerRegistry.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Docker Registry is not exist.", "Docker Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private async Task Info()
        {
            try
            {
                var configuration = new RegistryClientConfiguration(_dockerRegistry.Address);
                var catalogDto = new Catalog();
                using (var client = string.IsNullOrEmpty(_dockerRegistry.Username) == false || string.IsNullOrEmpty(_dockerRegistry.Password) == false ? configuration.CreateClient(new BasicAuthenticationProvider(_dockerRegistry.Username, _dockerRegistry.Password)) : configuration.CreateClient())
                {
                    var catalog =  await client.Catalog.GetCatalogAsync();
                    var repositories = catalog.Repositories;
                    foreach (var repository in repositories)
                    {
                        var repositoryDto = new Repository();
                        repositoryDto.Name = repository;
                        var tags = await client.Tags.ListImageTagsAsync(repository);
                        foreach (var tag in tags.Tags)
                        {
                            repositoryDto.Tags.Add(tag);
                        }
                        catalogDto.Repositories.Add(repositoryDto);
                    }
                }
                
                var options = new JsonSerializerOptions { WriteIndented = true };
                options.Converters.Add(new JsonStringEnumConverter());
                richTextBoxInfo.Text = JsonSerializer.Serialize(catalogDto, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerRegistry.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void DockerRegistryInfoForm_Load(object sender, EventArgs e)
        {
            await Info();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
