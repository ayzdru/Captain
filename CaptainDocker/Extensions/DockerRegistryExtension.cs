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
    public static class DockerRegistryExtension
    {
        public static IQueryable<SelectListItem> GetComboBoxItems(this DbSet<DockerRegistry>  dockerRegistries)
        {
            return dockerRegistries.Select(d => new SelectListItem { Text = $"{d.Name} - {d.Address}", Value = d.Id });
        }
        public static IQueryable<DockerRegistry> GetById(this DbSet<DockerRegistry> dockerRegistries, Guid id)
        {
            return dockerRegistries.Where(q => q.Id == id);
        }
    }
}
