using CaptainDocker.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileContextCore;
using FileContextCore.Extensions;
using FileContextCore.FileManager;
using FileContextCore.Serializer;

namespace CaptainDocker.Data
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                    .Entity<DockerConnection>()
                    .Property(p => p.AuthenticationType)
                    .HasConversion<short?>().IsRequired(false);

            modelBuilder.Entity<DockerConnection>().Property(p => p.BasicAuthCredentialUsername).IsRequired(false);
            modelBuilder.Entity<DockerConnection>().Property(p => p.BasicAuthCredentialPassword).IsRequired(false);
            modelBuilder.Entity<DockerConnection>().Property(p => p.CertificateCredentialFilePath).IsRequired(false);
            modelBuilder.Entity<DockerConnection>().Property(p => p.CertificateCredentialPassword).IsRequired(false);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Constants.Application.DatabaseConnection);
            optionsBuilder.UseFileContext(databasename: Constants.Application.DatabaseConnection);
        }

        public DbSet<DockerConnection> DockerConnections { get; set; }
        public DbSet<DockerRegistry> DockerRegistries { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
