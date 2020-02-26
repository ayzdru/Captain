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
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class NewDockerConnectionForm : Form
    {
        public NewDockerConnectionForm()
        {
            InitializeComponent();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SettingsManager settingsManager = new ShellSettingsManager(ServiceProvider.GlobalProvider);
            WritableSettingsStore userSettingsStore = settingsManager.GetWritableSettingsStore(SettingsScope.UserSettings);
            userSettingsStore.CreateCollection("Docker");
            List<DockerConnectionSetting> dockerConnectionSettings;
            if(userSettingsStore.PropertyExists("Docker","Connections"))
            {
                var connectionsDeserializeJson = userSettingsStore.GetString("Docker", "Connections");
                dockerConnectionSettings = JsonSerializer.Deserialize<List<DockerConnectionSetting>>(connectionsDeserializeJson);
            }
            else
            {
                dockerConnectionSettings = new List<DockerConnectionSetting>();
            }
            dockerConnectionSettings.Add(new DockerConnectionSetting() { Name = textBoxName.Text, Endpoint = textBoxEndpoint.Text });
            var connectionsSerializeJson = JsonSerializer.Serialize<List<DockerConnectionSetting>>(dockerConnectionSettings);
            userSettingsStore.SetString("Docker", "Connections", connectionsSerializeJson);
        }
    }
}
