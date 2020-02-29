using CaptainDocker.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
    public class DockerTreeViewItem : ITreeNode
    {
        public string Name { get; set; }
        public string EngineApiUrl { get; set; }
        public List<ITreeNode> ChildNodes { get; set; }
    }

}
