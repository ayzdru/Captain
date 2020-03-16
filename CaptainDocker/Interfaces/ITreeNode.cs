using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Interfaces
{
    public interface ITreeNode
    {
        string Name { get; set; }
        ObservableCollection<ITreeNode> ChildNodes { get; set; }
    }
}
