using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Security.Principal;
using Manager;

namespace ClientApp
{
    public class SSLHandshakeClient : ChannelFactory<ISSLHandshake>, ISSLHandshake, IDisposable
    {
        ISSLHandshake factory;

        public SSLHandshakeClient(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            IdentityReferenceCollection clGroups = WindowsIdentity.GetCurrent().Groups;
            string groupName = "";
            foreach (IdentityReference group in clGroups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                groupName = Formatter.ParseName(name.ToString());    /// return name of the Windows group				
                if (groupName == "Admins" || groupName == "Writers" || groupName == "Readers")
                {
                    break;
                }

            }
            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;

            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN, groupName);
            factory = this.CreateChannel();
        }


        public X509Certificate2 RequestSession()
        {
            try
            {
                X509Certificate2 pk = factory.RequestSession();
                return pk;
            }
            catch(Exception e)
            {
                Console.WriteLine("[RequestSession] Error={0}", e.Message);
                return null;
            }
        }

        public bool SendSessionKey(byte[] session_key)
        {

            try
            {
                return factory.SendSessionKey(session_key);
            }
            catch (Exception e)
            {
                Console.WriteLine("[RequestSession] Error={0}", e.Message);
                return false;
            }

        }
    }
}
