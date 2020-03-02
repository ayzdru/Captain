using CaptainDocker.Data;
using CaptainDocker.Entities;
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
        public Guid DockerRegistryId { get; set; }
    
        public DockerRegistryForm(Guid dockerRegistryId)
        {
            DockerRegistryId = dockerRegistryId;
            InitializeComponent();
        }
        public DockerRegistryForm()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.DockerRegistries.Add(new DockerRegistry(textBoxName.Text, textBoxAddress.Text, textBoxUsername.Text, textBoxPassword.Text));
                dbContext.SaveChanges();
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private void CheckBoxLoginRequired_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxLogin.Enabled = checkBoxLoginRequired.Checked;
        }

    }
}
