using CaptainDocker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CaptainDocker;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        public DbSet<DockerConnection> DockerConnections { get; set; }
        public DbSet<DockerRegistry> DockerRegistries { get; set; }

    }
}
