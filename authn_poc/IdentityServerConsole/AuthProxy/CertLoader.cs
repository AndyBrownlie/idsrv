using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace AuthProxy
{
    public class CertLoader
    {
        public static X509Certificate2 LoadCertificate()
        {
            var thumbprint = ConfigManager.AppSettings["IdSvr:Thumbprint"];
            var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var certificateThumbprint = Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
            var certCollection = certStore
                .Certificates
                .Find(X509FindType.FindByThumbprint,
                    certificateThumbprint,
                    false);

            X509Certificate2 cert;
            if (certCollection.Count > 0)
                cert = certCollection[0];
            else
                throw new CryptographicException("No valid certificate for signing tokens.");

            certStore.Close();
            return cert;
        }
    }
}