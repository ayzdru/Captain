using CaptainDocker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
    public class DockerContainerTitleTreeViewItem : ITreeNode
    {
        public string Name { get; set; }
        public List<ITreeNode> ChildNodes { get; set; }
    }
}
