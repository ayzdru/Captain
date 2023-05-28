using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.Models;
using Docker.Registry.DotNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class ContainerForm : BaseForm
    {
        public Guid DockerConnectionId { get; set; }
        public string ContainerId { get; set; }
        private readonly DockerConnection _dockerConnection;
        private readonly DockerClient _dockerClient;
        public ContainerForm(Guid dockerConnectionId, string containerId)
        {
            DockerConnectionId = dockerConnectionId;
            ContainerId = containerId;
            InitializeComponent();
            using (var dbContext = new ApplicationDbContext())
            {
                _dockerConnection = dbContext.DockerConnections.GetById(DockerConnectionId).SingleOrDefault();
                if (_dockerConnection != null)
                {
                    try
                    {
                        _dockerClient = _dockerConnection.GetDockerClientConfiguration().CreateClient();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Docker Connection is not exist.", "Docker Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private async Task InspectContainer()
        {
            try
            {
                var container = await _dockerClient.Containers.InspectContainerAsync(ContainerId);
                labelHeader.Text = container.Name;
                labelDescription.Text = container.ID;
                var options = new JsonSerializerOptions { WriteIndented = true };
                options.Converters.Add(new JsonStringEnumConverter());
                richTextBoxInspect.Text = JsonSerializer.Serialize(container, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void ContainerForm_Load(object sender, EventArgs e)
        {
            RichTextBox.CheckForIllegalCrossThreadCalls = false;
            await InspectContainer();
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                await _dockerClient.Containers.StartContainerAsync(ContainerId, new ContainerStartParameters() { });
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                await _dockerClient.Containers.StopContainerAsync(ContainerId, new ContainerStopParameters() { });
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonRestart_Click(object sender, EventArgs e)
        {
            try
            {
                await _dockerClient.Containers.RestartContainerAsync(ContainerId, new  ContainerRestartParameters() { });
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonKill_Click(object sender, EventArgs e)
        {
            try
            {
                await _dockerClient.Containers.KillContainerAsync(ContainerId, new  ContainerKillParameters() { });
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonPause_Click(object sender, EventArgs e)
        {
            try
            {
                await _dockerClient.Containers.PauseContainerAsync(ContainerId);
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonUnpause_Click(object sender, EventArgs e)
        {
            try
            {
                await _dockerClient.Containers.UnpauseContainerAsync(ContainerId);
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAttach_Click(object sender, EventArgs e)
        {
            this.Hide();
            new AttachContainerForm(DockerConnectionId, ContainerId, labelHeader.Text, labelDescription.Text).ShowDialog();
            this.Show();
        }

        private async void buttonInspect_Click(object sender, EventArgs e)
        {
            try
            {               
                await InspectContainer();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonLogs_Click(object sender, EventArgs e)
        {
            try
            {
                richTextBoxInspect.Clear();
                var stream = await _dockerClient.Containers.GetContainerLogsAsync(ContainerId, false, new ContainerLogsParameters() { Timestamps = true, ShowStderr = true, ShowStdout = true, Follow = true });
                var buffer = new byte[81920];
                bool finishedReading = false;
                while (finishedReading == false)
                {
                    var result = await stream.ReadOutputAsync(buffer, 0, buffer.Length, default).ConfigureAwait(false);
                    var output = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    richTextBoxInspect.AppendText(output);
                    Array.Clear(buffer, 0, buffer.Length);
                    if (result.EOF)
                    {
                        finishedReading = true;
                    }
                }
                stream.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ContainerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_dockerClient!=null)
            {
                _dockerClient.Dispose();
            }            
        }
    }
}
