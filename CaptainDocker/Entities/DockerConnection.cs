using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Entities
{
    public class DockerConnection : BaseEntity
    {
        public DockerConnection(string name, string engineApiUrl)
        {
            Name = name;
            EngineApiUrl = engineApiUrl;
        }

        public string Name { get; set; }
        public string EngineApiUrl { get; set; }
    }
}
