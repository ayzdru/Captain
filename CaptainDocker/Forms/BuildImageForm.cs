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

namespace CaptainDocker.Forms
{
    public partial class BuildImageForm : Form
    {
        public static IEnumerable<EnvDTE.Project> GetProjects(IVsSolution solution)
        {
            foreach (IVsHierarchy hier in GetProjectsInSolution(solution))
            {
                EnvDTE.Project project = GetDTEProject(hier);
                if (project != null)
                    yield return project;
            }
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution)
        {
            return GetProjectsInSolution(solution, __VSENUMPROJFLAGS.EPF_LOADEDINSOLUTION);
        }

        public static IEnumerable<IVsHierarchy> GetProjectsInSolution(IVsSolution solution, __VSENUMPROJFLAGS flags)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (solution == null)
                yield break;

            IEnumHierarchies enumHierarchies;
            Guid guid = Guid.Empty;
            solution.GetProjectEnum((uint)flags, ref guid, out enumHierarchies);
            if (enumHierarchies == null)
                yield break;

            IVsHierarchy[] hierarchy = new IVsHierarchy[1];
            uint fetched;
            while (enumHierarchies.Next(1, hierarchy, out fetched) == VSConstants.S_OK && fetched == 1)
            {
                if (hierarchy.Length > 0 && hierarchy[0] != null)
                    yield return hierarchy[0];
            }
        }

        public static EnvDTE.Project GetDTEProject(IVsHierarchy hierarchy)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (hierarchy == null)
                throw new ArgumentNullException("hierarchy");

            object obj;
            hierarchy.GetProperty(VSConstants.VSITEMID_ROOT, (int)__VSHPROPID.VSHPROPID_ExtObject, out obj);
            return obj as EnvDTE.Project;
        }

        public BuildImageForm()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            InitializeComponent();
            IVsSolution solution = (IVsSolution)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(IVsSolution));
            foreach (Project project in GetProjects(solution))
            {

            }
        }
        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
           if( folderBrowserDialogDirectory.ShowDialog() == DialogResult.OK)
            {
                textBoxDirectory.Text = folderBrowserDialogDirectory.SelectedPath;
            }
        }
        private Stream CreateTarballForDockerfileDirectory(string directory)
        {
            var tarball = new MemoryStream();
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
            var archive = new TarOutputStream(tarball)
            {
                //Prevent the TarOutputStream from closing the underlying memory stream when done
                IsStreamOwner = false
            };

            foreach (var file in files)
            {

                //Replacing slashes as KyleGobel suggested and removing leading /
                string tarName = file.Substring(directory.Length).Replace('\\', '/').TrimStart('/');

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

            //Reset the stream and return it, so it can be used by the caller
            tarball.Position = 0;
            return tarball;
        }
        private async void ButtonFinish_ClickAsync(object sender, EventArgs e)
        {
            DockerClient dockerClient = new DockerClientConfiguration(new Uri("http://kubernetemaster:2375/")).CreateClient();
            var imageBuildParameters = new ImageBuildParameters
            {
                Dockerfile = "Dockerfile",
                Tags = new List<string> { textBoxName.Text }
            };
            //C# 8.0 using syntax
            var tarball = CreateTarballForDockerfileDirectory(textBoxDirectory.Text);
            var responseStream =await dockerClient.Images.BuildImageFromDockerfileAsync(tarball, imageBuildParameters);
            var p = new Progress<JSONMessage>(status =>
            {
                Console.WriteLine(status.Status);
            });
        }
    }
}
