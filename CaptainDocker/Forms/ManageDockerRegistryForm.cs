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
    public partial class ManageDockerRegistryForm : BaseForm
    {
        public ManageDockerRegistryForm()
        {
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {

        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {

        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            new DockerRegistryForm().ShowDialog();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {

        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
