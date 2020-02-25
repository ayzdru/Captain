using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptainDocker.Settings
{
    public class DockerConnectionSetting
    {
        public string Endpoint { get; set; }
        public DockerConnectionBasicAuthCredentialSetting BasicAuthCredential { get; set; }
        public DockerConnectionCertificateCredentialSetting CertificateCredential { get; set; }
    }
}
