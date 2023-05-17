using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
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
    public partial class DockerConnectionForm : BaseForm
    {
        public Guid? DockerConnectionId { get; set; }
        public DockerConnectionForm(Guid? dockerConnectionId = null)
        {
            DockerConnectionId = dockerConnectionId;
            InitializeComponent();
            if (!DockerConnectionId.HasValue)
            {
                this.Text = "Add New Docker Connection";
                labelHeader.Text = this.Text;
                labelDescription.Text = "Add new docker connection.";             
            }
            else
            {
                this.Text = "Edit Docker Connection";
                labelHeader.Text = this.Text;
                labelDescription.Text = "Edit current docker connection data.";
            }
        
        }       
        private void ButtonSave_Click(object sender, EventArgs e)
        {            
            using (var dbContext = new ApplicationDbContext())
            {
                
                DockerConnection.AuthenticationTypes? authenticationType = null;
                if(checkBoxEnableAuthentication.Checked == true)
                {
                    if(radioButtonAuthenticationLocal.Checked == true)
                    {
                        authenticationType = DockerConnection.AuthenticationTypes.LocalDocker;
                    }
                    else if(radioButtonAuthenticationCertificate.Checked == true)
                    {
                        authenticationType = DockerConnection.AuthenticationTypes.Certificate;
                    }
                    else if(radioButtonAuthenticationBasicAuth.Checked == true)
                    {
                        authenticationType = DockerConnection.AuthenticationTypes.BasicAuth;
                    }
                }
                if (DockerConnectionId.HasValue == false)
                {
                    dbContext.DockerConnections.Add(new DockerConnection(textBoxName.Text, textBoxEngineApiUrl.Text, authenticationType, textBoxBasicAuthenticationUsername.Text, textBoxBasicAuthenticationPassword.Text, textBoxCertificateFilePath.Text, textBoxCertificatePassword.Text));
                }
                else
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(DockerConnectionId.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        dockerConnection.Name = textBoxName.Text;
                        dockerConnection.EngineApiUrl = textBoxEngineApiUrl.Text;
                        dockerConnection.AuthenticationType = authenticationType;
                        dockerConnection.BasicAuthCredentialUsername = textBoxBasicAuthenticationUsername.Text;
                        dockerConnection.BasicAuthCredentialPassword = textBoxBasicAuthenticationPassword.Text;
                        dockerConnection.CertificateCredentialFilePath = textBoxCertificateFilePath.Text;
                        dockerConnection.CertificateCredentialPassword = textBoxCertificatePassword.Text;
                        dbContext.DockerConnections.Update(dockerConnection);
                    }
                    else
                    {
                        MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                dbContext.SaveChanges();
            }
            this.Close();
        }

        private void CheckBoxEnableAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxAuthentication.Enabled = checkBoxEnableAuthentication.Checked;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCertificateBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialogCertificateFile.ShowDialog() == DialogResult.OK)
            {
                textBoxCertificateFilePath.Text = openFileDialogCertificateFile.FileName;
            }
        }

        private void DockerConnectionForm_Load(object sender, EventArgs e)
        {
            if (DockerConnectionId.HasValue)
            {
                using (var dbContext = new ApplicationDbContext())
                {
                    var dockerConnection = dbContext.DockerConnections.GetById(DockerConnectionId.Value).SingleOrDefault();
                    if (dockerConnection != null)
                    {
                        textBoxName.Text = dockerConnection.Name;
                        textBoxEngineApiUrl.Text = dockerConnection.EngineApiUrl;
                        textBoxBasicAuthenticationUsername.Text = dockerConnection.BasicAuthCredentialUsername;
                        textBoxBasicAuthenticationPassword.Text = dockerConnection.BasicAuthCredentialPassword;
                        textBoxCertificateFilePath.Text = dockerConnection.CertificateCredentialFilePath;
                        textBoxCertificatePassword.Text = dockerConnection.CertificateCredentialPassword;
                        checkBoxEnableAuthentication.Checked = dockerConnection.AuthenticationType.HasValue;
                        if(dockerConnection.AuthenticationType.HasValue == true)
                        {
                            switch(dockerConnection.AuthenticationType.Value)
                            {
                                case DockerConnection.AuthenticationTypes.LocalDocker:
                                    radioButtonAuthenticationLocal.Checked = true;
                                    break;
                                case DockerConnection.AuthenticationTypes.Certificate:
                                     radioButtonAuthenticationCertificate.Checked = true; 
                                    break;
                                case DockerConnection.AuthenticationTypes.BasicAuth:
                                     radioButtonAuthenticationBasicAuth.Checked = true;
                                     break;
                            }
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
}
