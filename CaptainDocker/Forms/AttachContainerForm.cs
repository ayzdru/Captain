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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CaptainDocker.Forms
{
    public partial class AttachContainerForm : BaseForm
    {
        public Guid DockerConnectionId { get; set; }
        public string ContainerId { get; set; }
        private readonly DockerConnection _dockerConnection;
        private readonly DockerClient _dockerClient;
        MultiplexedStream _stream;
        public AttachContainerForm(Guid dockerConnectionId, string containerId, string name, string id)
        {
            DockerConnectionId = dockerConnectionId;
            ContainerId = containerId;
            InitializeComponent();
            labelHeader.Text = name;
            labelDescription.Text = id;
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
        
        private async void ContainerForm_Load(object sender, EventArgs e)
        {
            RichTextBox.CheckForIllegalCrossThreadCalls = false;
            _stream =await _dockerClient.Containers.AttachContainerAsync("4118f9aa3dbe", true, new ContainerAttachParameters() { Stderr = true, Stdin = true, Stdout = true, Stream = true });
            var read = Task.Run(async () => {
                try
                {
                    while (true)
                    {
                        var buffer = new byte[81920];
                        bool finishedReading = false;
                        while (finishedReading == false)
                        {
                            var result = await _stream.ReadOutputAsync(buffer, 0, buffer.Length, default).ConfigureAwait(false);
                            var output = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            richTextBoxConsole.AppendText(output);
                            Array.Clear(buffer, 0, buffer.Length);
                            if (result.EOF)
                            {
                                finishedReading = true;
                            }
                        }
                    }
                }
                catch
                {

                    
                }
                
            });
            await read.ConfigureAwait(false);
        }

       private async void Send()            
        {
            try
            {
                var wsBuffer = Encoding.UTF8.GetBytes(textBoxCommand.Text + "\n");
                await _stream.WriteAsync(wsBuffer, 0, wsBuffer.Length, default).ConfigureAwait(false);
                textBoxCommand.Text = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, _dockerConnection.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void buttonSend_Click(object sender, EventArgs e)
        {
            Send();
        }

        private async void textBoxCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Send();
            }
        }

        private void AttachContainerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(_stream!=null)
            {                
                _stream.Dispose();
            }
        }
    }
}
