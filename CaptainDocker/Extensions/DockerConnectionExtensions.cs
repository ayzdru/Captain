using CaptainDocker.Entities;
using CaptainDocker.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Extensions
{
    public static class DockerConnectionExtensions
    {
        public static IQueryable<SelectListItem> GetComboBoxItems(this DbSet<DockerConnection> dockerConnections)
        {
            return dockerConnections.Select(d => new SelectListItem { Text = $"{d.Name} - {d.EngineApiUrl}", Value = d.Id });
        }
        public static IQueryable<DockerConnection> GetById(this DbSet<DockerConnection>  dockerConnections, Guid id)
        {
            return dockerConnections.Where(q => q.Id == id);
        }
    }
}
