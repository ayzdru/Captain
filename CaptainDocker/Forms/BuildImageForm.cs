using CaptainDocker.Data;
using CaptainDocker.Entities;
using CaptainDocker.Extensions;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.Models;
using EnvDTE;
using ICSharpCode.SharpZipLib.Tar;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE80;

namespace CaptainDocker.Forms
{
    public partial class BuildImageForm : BaseForm
    {
        private System.Threading.CancellationTokenSource _cts;
        public System.Threading.CancellationTokenSource Cts
        {
            get
            {
                if (_cts == null)
                {
                    _cts = new System.Threading.CancellationTokenSource();
                }
                return _cts;
            }
            set
            {
                _cts = value;
            }
        }
        public Guid DockerConnectionId { get; set; }
        private EnvDTE.Project[] GetProjects(EnvDTE.Project project)
        {
            if (null == project)
            {
                return new EnvDTE.Project[0];
            }

            var projects = new List<EnvDTE.Project>();

            if (project.Kind != ProjectKinds.vsProjectKindSolutionFolder)
            {
                projects.Add(project);
            }

            if (project.ProjectItems != null)
            {
                for (var i = 1; i <= project.ProjectItems.Count; i++)
                {
                    var subProject = project.ProjectItems.Item(i).SubProject;
                    projects.AddRange(GetProjects(subProject));
                }
            }

            return projects.ToArray();
        }
        public BuildImageForm(Guid dockerConnectionId)
        {
            DockerConnectionId = dockerConnectionId;
            ThreadHelper.ThrowIfNotOnUIThread();
            InitializeComponent();
            

        }
        private void ButtonDirectoryBrowse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogDirectory.ShowDialog() == DialogResult.OK)
            {
                textBoxDirectory.Text = folderBrowserDialogDirectory.SelectedPath;
            }
        }
        private void ButtonDockerfileBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialogDockerfile.ShowDialog() == DialogResult.OK)
            {
                textBoxDockerfile.Text = openFileDialogDockerfile.FileName;
            }
        }
        private Stream CreateTarballForDockerfileDirectory(string directory)
        {
            var tarball = new MemoryStream();
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories).ToList();
            using (var archive = new TarOutputStream(tarball) { IsStreamOwner = false })
            {
                files.Add(textBoxDockerfile.Text);
                foreach (var file in files)
                {

                    //Replacing slashes as KyleGobel suggested and removing leading /                 
                    string tarName = file.Contains(directory) ? file.Substring(directory.Length).Replace('\\', '/').TrimStart('/') : Path.GetFileName(file);
                    
                    //Let's create the entry header
                    var entry = TarEntry.CreateTarEntry(tarName);
                    var fileStream = File.OpenRead(file);
                    entry.Size = fileStream.Length;
                    archive.PutNextEntry(entry);

                    //Now write the bytes of data
                    byte[] localBuffer = new byte[32 * 1024];
                    while (true)
                    {
                        int numRead = fileStream.Read(localBuffer, 0, localBuffer.Length);
                        if (numRead <= 0)
                            break;

                        archive.Write(localBuffer, 0, numRead);
                    }

                    //Nothing more to do with this entry
                    archive.CloseEntry();
                }

                archive.Close();
                tarball.Position = 0;
            }

            return tarball;
        }
        private async void ButtonBuild_Click(object sender, EventArgs e)
        {
            buttonBuild.Enabled = false;
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerEngineItem = comboBoxDockerEngine.SelectedItem as SelectListItem;
                var dockerConnection = dbContext.DockerConnections.GetById(dockerEngineItem.Value).SingleOrDefault();
                if (dockerConnection != null)
                {
                    DockerClient dockerClient = new DockerClientConfiguration(new Uri(dockerConnection.EngineApiUrl)).CreateClient();
                    var imageBuildParameters = new ImageBuildParameters
                    {
                        Dockerfile = Path.GetFileName(textBoxDockerfile.Text),
                        Tags = new List<string> { textBoxImageName.Text }
                    };
                    var tarball = CreateTarballForDockerfileDirectory(textBoxDirectory.Text);
                    var responseStream = await dockerClient.Images.BuildImageFromDockerfileAsync(tarball, imageBuildParameters, Cts.Token);
                    var streamReader = new StreamReader(responseStream);
                    var text = await streamReader.ReadToEndAsync();

                    if (comboBoxProjects.SelectedItem != null)
                    {
                        var projectItem = comboBoxProjects.SelectedItem as SelectListItem<string>;
                        var project = dbContext.Projects.Where(q => q.Name == projectItem.Text).SingleOrDefault();
                        if (project != null)
                        {
                            project.ImageName = textBoxImageName.Text;
                            project.Directory = textBoxDirectory.Text;
                            project.Dockerfile = textBoxDockerfile.Text;
                            dbContext.Projects.Update(project);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            dbContext.Projects.Add(new Entities.Project(projectItem.Text, textBoxImageName.Text,
                                textBoxDirectory.Text, textBoxDockerfile.Text));
                            dbContext.SaveChanges();
                        }
                    }
                   
                }
            }
            buttonBuild.Enabled = true;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            Cts.Dispose();
            Cts = null;
            buttonBuild.Enabled = true;
        }

        private void BuildImageForm_Load(object sender, EventArgs e)
        {
            using (var dbContext = new ApplicationDbContext())
            {
                var dockerConnections = dbContext.DockerConnections.GetComboBoxItems().ToList();
                comboBoxDockerEngine.DataSource = dockerConnections;
                comboBoxDockerEngine.SelectById(DockerConnectionId);
            }

            var projects = CaptainDockerPackage._dte.Solution.Projects;
            foreach (EnvDTE.Project project in projects)
            {
                var subProjects = GetProjects(project);
                foreach (var subProject in subProjects)
                {
                    if (!string.IsNullOrEmpty(subProject.FullName))
                    {
                        comboBoxProjects.Items.Add(new SelectListItem<string>() { Text = subProject.Name, Value = Path.GetDirectoryName(subProject.FullName) });
                    }
                }
            }

        }

        private void comboBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProjects.SelectedItem != null)
            {
                var projectItem = comboBoxProjects.SelectedItem as SelectListItem<string>;
                using (var dbContext = new ApplicationDbContext())
                {
                    var project =
                        dbContext.Projects.Where(q => q.Name == projectItem.Text).SingleOrDefault();
                    if (project != null)
                    {
                        textBoxImageName.Text = project.ImageName;
                        textBoxDirectory.Text = project.Directory;
                        textBoxDockerfile.Text = project.Dockerfile;
                        folderBrowserDialogDirectory.SelectedPath = project.Directory;
                        openFileDialogDockerfile.InitialDirectory = Path.GetDirectoryName(project.Dockerfile);
                    }
                    else
                    {
                        textBoxImageName.Text = null;
                        textBoxDirectory.Text = projectItem.Value;
                        textBoxDockerfile.Text = Path.Combine(projectItem.Value, "Dockerfile");
                        folderBrowserDialogDirectory.SelectedPath = projectItem.Value;
                        openFileDialogDockerfile.InitialDirectory = projectItem.Value;
                    }
                }
            }
        }
    }
}
