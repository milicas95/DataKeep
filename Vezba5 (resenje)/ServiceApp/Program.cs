﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Contracts;
using System.ServiceModel.Security;
using Manager;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace ServiceApp
{
	public class Program
	{
		static void Main(string[] args)
		{
            /// srvCertCN.SubjectName should be set to the service's username. .NET WindowsIdentity class provides information about Windows user running the given process
            Console.ReadLine();
			string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            IdentityReferenceCollection clGroups = WindowsIdentity.GetCurrent().Groups;
            bool found = false;
            string groupName = "";
            foreach (IdentityReference group in clGroups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                groupName = Formatter.ParseName(name.ToString());    /// return name of the Windows group				
                if (groupName == "Servers")
                {
                    found = true;
                    break;
                }

            }
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

			string address = "net.tcp://localhost:9999/Receiver";
			ServiceHost host = new ServiceHost(typeof(WCFService));
            host.AddServiceEndpoint(typeof(IDatabaseManagement), binding, address);
            host.AddServiceEndpoint(typeof(ISSLHandshake), binding, address);

            ///PeerTrust - for development purposes only to temporarily disable the mechanism that checks the chain of trust for a certificate. 
            ///To do this, set the CertificateValidationMode property to PeerTrust (PeerOrChainTrust) - specifies that the certificate can be self-issued (peer trust) 
            ///To support that, the certificates created for the client and server in the personal certificates folder need to be copied in the Trusted people -> Certificates folder.
            ///host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.PeerTrust;

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            
            if (found)
            {
                ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
                host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN, groupName);
                /// host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromFile("WCFService.pfx");
                
                try
                {
                    host.Open();
                    Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] {0}", e.Message);
                    Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                }
                finally
                {
                    host.Close();
                }
            }
            else
            {
                Console.WriteLine("Server is in no groupe");
                Console.ReadLine();
                host.Close();
            }
		}
	}
}
