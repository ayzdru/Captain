using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Entities
{
    public class DockerRegistry : BaseEntity
    {
        public DockerRegistry(string name, string address, string username, string password)
        {
            Name = name;
            Address = address;
            Username = username;
            Password = password;
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
