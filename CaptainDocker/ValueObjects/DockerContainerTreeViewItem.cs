using CaptainDocker.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
   public class DockerContainerTreeViewItem : ITreeNode
    {
        public string Name { get; set; }
        public ObservableCollection<ITreeNode> ChildNodes { get; set; }
    }
}
