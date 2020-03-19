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
    public partial class SettingsForm : BaseForm
    {
        public const string CaptainDockerCollectionName = "CaptainDocker";
        public const string CaptainDockerDatabaseConnection = "CaptainDockerDatabaseConnection";
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            if (!userSettingsStore.CollectionExists(CaptainDockerCollectionName))
            {
                userSettingsStore.CreateCollection(CaptainDockerCollectionName);
            }
            userSettingsStore.SetString(CaptainDockerCollectionName, CaptainDockerDatabaseConnection, textBoxDatabaseConnection.Text);
            this.Close();
        }
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            SettingsStore configurationSettingsStore = settingsManager.GetReadOnlySettingsStore(SettingsScope.UserSettings);
            textBoxDatabaseConnection.Text = configurationSettingsStore.GetString(CaptainDockerCollectionName, CaptainDockerDatabaseConnection);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
