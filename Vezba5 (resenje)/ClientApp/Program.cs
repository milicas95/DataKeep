using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Security.Principal;

namespace ClientApp
{
	public class Program
	{
		static void Main(string[] args)
		{
			/// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string clientCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            IdentityReferenceCollection clGroups = WindowsIdentity.GetCurrent().Groups;
            bool found = false;
            string groupName = "";
            foreach (IdentityReference group in clGroups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var name = sid.Translate(typeof(NTAccount));
                groupName = Formatter.ParseName(name.ToString());    /// return name of the Windows group				
                if(groupName=="Admins" || groupName=="Writers" || groupName == "Readers")
                {
                    found = true;
                    break;
                }
                
            }
            NetTcpBinding binding = new NetTcpBinding();
			binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            if (!found)
            {
                Console.WriteLine("This client is not in any group.");
                Console.ReadLine();
            }
            else
            {
                X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, clientCertCN, groupName);
                EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                          new X509CertificateEndpointIdentity(srvCert));

                using (WCFClient proxy = new WCFClient(binding, address))
                {
                    int action;
                    do
                    {
                        Console.WriteLine("================================================");
                        Console.WriteLine("Wellcome {0} chose your action:", clientCertCN);
                        Console.WriteLine("To create new data base press 1");
                        Console.WriteLine("To delete exsisting data base press 2");
                        Console.WriteLine("To make new parameter in data base press 3");
                        Console.WriteLine("To modify existing parameter press 4");
                        Console.WriteLine("To get average usage in city press 5");
                        Console.WriteLine("To get average usage in region press 6");
                        Console.WriteLine("To get highest spender in region press 7");
                        Console.WriteLine("To exit app press 0");
                        Console.WriteLine("================================================");
                        action = Convert.ToInt32(Console.ReadLine());

                        switch (action)
                        {
                            case 0:
                                continue;
                            case 3:
                                Console.WriteLine("Enter database name: ");
                                string db = Console.ReadLine();
                                if (proxy.Add(db, clientCertCN))
                                {
                                    Console.WriteLine("Added to database!");
                                }
                                else
                                {
                                    Console.WriteLine("Error! Information can't be added to database!");
                                }
                                continue;
                            case 4:
                                Console.WriteLine("Enter database name: ");
                                string dbs = Console.ReadLine();
                                if (proxy.Add(dbs, clientCertCN))
                                {
                                    Console.WriteLine("Database edited!");
                                }
                                else
                                {
                                    Console.WriteLine("Error! Information can't be edited!");
                                }
                                continue;
                            case 5:
                                Console.WriteLine("What is the city?");
                                string city = Console.ReadLine();
                                int averageCity = proxy.AverageUsageInCity(city, clientCertCN);
                                if (averageCity == -1)
                                    Console.WriteLine("You don't have promission to use this method");
                                else
                                    Console.WriteLine("Average usage in {0} is {1}", city, averageCity);
                                continue;
                            case 6:
                                Console.WriteLine("What is the region?");
                                string region = Console.ReadLine();
                                int averageRegion = proxy.AverageUsageInRegion(region, clientCertCN);
                                if (averageRegion == -1)
                                    Console.WriteLine("You don't have promission to use this method");
                                else
                                    Console.WriteLine("Average usage in {0} is {1}", region, averageRegion);
                                continue;
                            case 7:
                                Console.WriteLine("What is the region?");
                                string reg = Console.ReadLine();
                                string highestSpender = proxy.HighestSpenderInRegion(reg, clientCertCN);
                                if (highestSpender == "You can't use this option")
                                    Console.WriteLine("You don't have promission to use this method");
                                else
                                    Console.WriteLine("Highest spender in {0} is {1}", reg, highestSpender);
                                continue;
                            default:
                                continue;
                        }
                    } while (action != 0);

                }
            }
		}
	}
}
