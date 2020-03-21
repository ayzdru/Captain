using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
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
    public partial class DockerRegistryForm : BaseForm
    {
        public Guid? DockerRegistryId { get; set; }
        public DockerRegistryForm(Guid? dockerRegistryId)
        {
            DockerRegistryId = dockerRegistryId;
            InitializeComponent();          
            if(!DockerRegistryId.HasValue)
            {
                this.Text = "Add Docker Registry";
                labelHeader.Text = this.Text;
                labelDescription.Text = "Add new docker registry.";
                textBoxAddress.Text = Constants.Application.DefaultRegistry;
            }
            else
            {
                this.Text = "Edit Docker Registry";
                labelHeader.Text = this.Text;
                labelDescription.Text = "Edit current docker registry data.";
            }
        }
        public DockerRegistryForm()
        {
            InitializeComponent();
            textBoxAddress.Text = Constants.Application.DefaultRegistry;
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                if (!DockerRegistryId.HasValue)
                {
                    var newDockerRegistry = new DockerRegistry(textBoxName.Text, textBoxAddress.Text, textBoxUsername.Text, textBoxPassword.Text);
                    dbContext.DockerRegistries.Add(newDockerRegistry);
                    dbContext.SaveChanges();
                    DockerRegistryId = newDockerRegistry.Id;
                }
                else
                {                   
                    var dockerRegistry = dbContext.DockerRegistries.GetById(DockerRegistryId.Value).SingleOrDefault();
                    dockerRegistry.Name = textBoxName.Text;
                    dockerRegistry.Address = textBoxAddress.Text;
                    if (checkBoxLoginRequired.Checked)
                    {
                        dockerRegistry.Username = textBoxUsername.Text;
                        dockerRegistry.Password = textBoxPassword.Text;
                    }
                    dbContext.DockerRegistries.Update(dockerRegistry);
                    dbContext.SaveChanges();
                }
               
            }          
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DockerRegistryId = null;
            this.Close();
        }

        private void CheckBoxLoginRequired_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxLogin.Enabled = checkBoxLoginRequired.Checked;
        }

        private void DockerRegistryForm_Load(object sender, EventArgs e)
        {
            if (DockerRegistryId.HasValue)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerRegistry = dbContext.DockerRegistries.GetById(DockerRegistryId.Value).SingleOrDefault();
                    textBoxName.Text = dockerRegistry.Name;
                    textBoxAddress.Text = dockerRegistry.Address;
                    textBoxUsername.Text = dockerRegistry.Username;
                    textBoxPassword.Text = dockerRegistry.Password;
                    if (!string.IsNullOrEmpty(dockerRegistry.Username) || !string.IsNullOrEmpty(dockerRegistry.Password))
                    {
                        checkBoxLoginRequired.Checked = true;
                    }
                }
            }
        }
    }
}
