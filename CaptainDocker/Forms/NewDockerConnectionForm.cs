using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Settings;
using Microsoft.VisualStudio.Settings;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Settings;
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
    public partial class NewDockerConnectionForm : BaseForm
    {
        public NewDockerConnectionForm()
        {
            InitializeComponent();
        }       
        private void ButtonSave_Click(object sender, EventArgs e)
        {            
            using (var dbContext = new ApplicationDbContext())
            {
                dbContext.DockerConnections.Add(new DockerConnection(textBoxName.Text, textBoxEngineApiUrl.Text));
                dbContext.SaveChanges();
            }
        }

        private void CheckBoxEnableAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAuthentication.Enabled = checkBoxEnableAuthentication.Enabled;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
