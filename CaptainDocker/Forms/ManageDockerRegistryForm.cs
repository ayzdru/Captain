using CaptainDocker.Data;
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
    public partial class ManageDockerRegistryForm : BaseForm
    {
        public ManageDockerRegistryForm()
        {
            InitializeComponent();
        
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            var dockerRegistryForm = new DockerRegistryForm(null);
            dockerRegistryForm.ShowDialog();
            dataGridViewDockerRegistries.Rows.Add(dockerRegistryForm.DockerRegistryId.Value, dockerRegistryForm.textBoxName.Text, dockerRegistryForm.textBoxAddress.Text, dockerRegistryForm.textBoxUsername.Text);
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            var selectedRow = dataGridViewDockerRegistries.SelectedRows[0];
            var selectedRegistryName = selectedRow.Cells[1].Value.ToString();
            if (MessageBox.Show($"{selectedRegistryName} will be deleted?\nAre you sure?", "Registry Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var selectedRegistryId = Guid.Parse(selectedRow.Cells[0].Value.ToString());
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerRegistry = dbContext.DockerRegistries.GetById(selectedRegistryId).SingleOrDefault();
                    dbContext.DockerRegistries.Remove(dockerRegistry);
                    dbContext.SaveChanges();
                    dataGridViewDockerRegistries.Rows.Remove(selectedRow);
                }
            }
        }

        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            var selectedRow = dataGridViewDockerRegistries.SelectedRows[0];
            var selectedRegistryId = Guid.Parse(selectedRow.Cells[0].Value.ToString());
            var dockerRegistryForm = new DockerRegistryForm(selectedRegistryId);
            dockerRegistryForm.ShowDialog();
            if (dockerRegistryForm.DockerRegistryId.HasValue)
            {
                selectedRow.Cells[1].Value = dockerRegistryForm.textBoxName.Text;
                selectedRow.Cells[2].Value = dockerRegistryForm.textBoxAddress.Text;
                if (dockerRegistryForm.checkBoxLoginRequired.Checked)
                {
                    selectedRow.Cells[3].Value = dockerRegistryForm.textBoxUsername.Text;
                }
                else
                {
                    selectedRow.Cells[3].Value = string.Empty;
                }
            }
        }

        private void ManageDockerRegistryForm_Load(object sender, EventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerRegistiries = dbContext.DockerRegistries.ToList();
                foreach (var dockerRegistry in dockerRegistiries)
                {
                    dataGridViewDockerRegistries.Rows.Add(dockerRegistry.Id, dockerRegistry.Name, dockerRegistry.Address, dockerRegistry.Username);
                }
            }
        }

        private void dataGridViewDockerRegistries_SelectionChanged(object sender, EventArgs e)
        {
            buttonRemove.Enabled = buttonEdit.Enabled = dataGridViewDockerRegistries.SelectedRows.Count > 0;
        }
    }
}
