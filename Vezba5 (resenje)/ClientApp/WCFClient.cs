using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using Manager;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using DBparam;

namespace ClientApp
{
	public class WCFClient : ChannelFactory<IDatabaseManagement>, IDatabaseManagement, IDisposable
	{
		IDatabaseManagement factory;

		public WCFClient(NetTcpBinding binding, EndpointAddress address)
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

        public bool Add(string userName, DBParam bdp)
        {
            try
            {
                return factory.Add(userName, bdp);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Add] Error={0}", e.Message);
                return false;
            }

        }

        public int AverageUsageInCity(string city, string userName)
        {
            try
            {
                return factory.AverageUsageInCity(city, userName);
            }
            catch(Exception e)
            {
                Console.WriteLine("[AverageUsageInCIty] Error={0}", e.Message);
                return -1;
            }
        }

        public int AverageUsageInRegion(string region, string userName)
        {
            try
            {
                return factory.AverageUsageInRegion(region, userName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[AverageUsageInRegion] Error={0}", e.Message);
                return -1;
            }
        }

        public bool Edit(string userName, DBParam bdp)
        {
            try
            {
                return factory.Edit(userName, bdp);
            }
            catch (Exception e)
            {
                Console.WriteLine("[Edit] Error={0}", e.Message);
                return false;
            }
        }

        public string HighestSpenderInRegion(string region, string userName)
        {
            try
            {
                return factory.HighestSpenderInRegion(region, userName);
            }
            catch (Exception e)
            {
                Console.WriteLine("[AverageUsageInCIty] Error={0}", e.Message);
                return e.Message;
            }
        }
    }
}
