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
        public Guid DockerConnectionId { get; set; }
        public string Name { get; set; }
        public string EngineApiUrl { get; set; }
        public ObservableCollection<ITreeNode> ChildNodes { get; set; }
    }

}
