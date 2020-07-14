using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Entities
{
    public class Project : BaseEntity
    {
        public Project(string name, string imageName, string directory, string dockerfile)
        {
            Name = name;
            ImageName = imageName;
            Directory = directory;
            Dockerfile = dockerfile;
        }

        public string Name { get; set; }
        public string ImageName { get; set; }
        public string Directory { get; set; }
        public string Dockerfile { get; set; }
    }
}
