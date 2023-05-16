using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Entities
{
    public class DockerConnection : BaseEntity
    {
        public enum AuthenticationTypes
        {
            LocalDocker = 0,
            Certificate =1,
            BasicAuth = 2
        }
        public DockerConnection(string name, string engineApiUrl, AuthenticationTypes? authenticationType, string basicAuthCredentialUsername, string basicAuthCredentialPassword, string certificateCredentialFilePath, string certificateCredentialPassword)
        {
            Name = name;
            EngineApiUrl = engineApiUrl;
            AuthenticationType = authenticationType;
            BasicAuthCredentialUsername = basicAuthCredentialUsername;                
            BasicAuthCredentialPassword = basicAuthCredentialPassword;                
            CertificateCredentialFilePath = certificateCredentialFilePath;
            CertificateCredentialPassword = certificateCredentialPassword;
        }

        public string Name { get; set; }
        public string EngineApiUrl { get; set; }
        public AuthenticationTypes? AuthenticationType { get; set; }
        public string BasicAuthCredentialUsername { get; set; }
        public string BasicAuthCredentialPassword { get; set; }
        public string CertificateCredentialFilePath{ get; set; }
        public string CertificateCredentialPassword { get; set; }
    }
}
