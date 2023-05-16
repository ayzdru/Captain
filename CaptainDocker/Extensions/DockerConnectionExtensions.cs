using CaptainDocker.Entities;
using CaptainDocker.ValueObjects;
using Docker.DotNet;
using Docker.DotNet.BasicAuth;
using Docker.DotNet.Models;
using Docker.DotNet.X509;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
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
        public static DockerClientConfiguration GetDockerClientConfiguration(this DockerConnection dockerConnection)
        {
            Credentials credentials = null;
            Uri endpoint = null;
            if(dockerConnection.AuthenticationType.HasValue == true)
            {
                switch(dockerConnection.AuthenticationType.Value)
                {
                    case DockerConnection.AuthenticationTypes.BasicAuth:
                        credentials = new BasicAuthCredentials(dockerConnection.BasicAuthCredentialUsername, dockerConnection.BasicAuthCredentialPassword);
                        endpoint = new Uri(dockerConnection.EngineApiUrl);
                        break;
                    case DockerConnection.AuthenticationTypes.Certificate:
                        credentials = new CertificateCredentials(new X509Certificate2(dockerConnection.CertificateCredentialFilePath, dockerConnection.CertificateCredentialPassword));
                        ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                        endpoint = new Uri(dockerConnection.EngineApiUrl);
                        break;
                    case DockerConnection.AuthenticationTypes.LocalDocker:
                        return new DockerClientConfiguration();
                }               
            }
            else
            {
                endpoint = new Uri(dockerConnection.EngineApiUrl);
            }

            return new DockerClientConfiguration(endpoint, credentials);
        }
    }
}
