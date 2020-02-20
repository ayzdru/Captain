using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
    public class DockerTreeViewItem
    {
        public DockerTreeViewItem()
        {
            this.Items = new ObservableCollection<DockerTreeViewItem>();
        }
        public string Title { get; set; }
        public ObservableCollection<DockerTreeViewItem> Items { get; set; }
    }
}
