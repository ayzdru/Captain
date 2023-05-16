using CaptainDocker.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.ValueObjects
{
   public class DockerContainerTreeViewItem : ITreeNode, INotifyPropertyChanged
    {
        public string _containerId { get; set; }
        public string ContainerId
        {
            get
            {
                return this._containerId;
            }

            set
            {
                if (value != this._containerId)
                {
                    this._containerId = value;
                    NotifyPropertyChanged();
                }
            }
        }       

        private Guid _dockerConnectionId;
        public Guid DockerConnectionId
        {
            get
            {
                return this._dockerConnectionId;
            }

            set
            {
                if (value != this._dockerConnectionId)
                {
                    this._dockerConnectionId = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private string _name;

        public string Name
        {
            get
            {
                return this._name;
            }

            set
            {
                if (value != this._name)
                {
                    this._name = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private ObservableCollection<ITreeNode> _childNodes;
        public ObservableCollection<ITreeNode> ChildNodes
        {
            get
            {
                return this._childNodes;
            }

            set
            {
                if (value != this._childNodes)
                {
                    this._childNodes = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
