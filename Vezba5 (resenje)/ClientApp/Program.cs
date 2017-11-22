using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Security.Principal;
using DBparam;
using System.IO;

namespace ClientApp
{
	public class Program
	{
		static void Main(string[] args)
		{
            Console.ReadLine();
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
                X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "dkservice", "Servers");
                EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                              new X509CertificateEndpointIdentity(srvCert));
                
                using (WCFClient proxy = new WCFClient(binding, address))
                {
                    int action;
                    do
                    {
                        Console.WriteLine("================================================");
                        Console.WriteLine("Welcome {0} chose your action:", clientCertCN);
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
                            case 1:
                                if (!proxy.CreateDatabase(clientCertCN))
                                    Console.WriteLine("You cannot create a new database");
                                else
                                    Console.WriteLine("You created a new database");
                                continue;
                            case 2:
                                if (!proxy.DeleteDatabase(clientCertCN))
                                    Console.WriteLine("You cannot delete a database");
                                else
                                    Console.WriteLine("You deleted the database");
                                continue;
                            case 3:
                                DBParam dbp = new DBParam();
                                Console.WriteLine("Enter ID: ");
                                int id;
                                string sid = Console.ReadLine();
                                while (!Int32.TryParse(sid, out id))
                                {
                                    //               Audit.AddFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
                                    Console.WriteLine("Error, wrong input for year");
                                }
                                //dodati proveru da li postoji id u bazi
                                dbp.Id = id;
                                Console.WriteLine("Enter region: ");
                                dbp.Region = Console.ReadLine();
                                Console.WriteLine("Enter city: ");
                                dbp.City = Console.ReadLine();
                                Console.WriteLine("Enter year: ");
                                int year;
                                string syear = Console.ReadLine();
                                while (!Int32.TryParse(syear, out year))
                                {
                                    //               Audit.AddFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
                                    Console.WriteLine("Error, wrong input for year");
                                }
                                dbp.Year = year;
                                Console.WriteLine("Enter month: ");
                                dbp.Month = Console.ReadLine();
                                Console.WriteLine("Enter usage: ");
                                int eUsage;
                                string seUsage = Console.ReadLine();
                                while (!Int32.TryParse(seUsage, out eUsage))
                                {
                                    //               Audit.AddFailed("User " + userName + " put wrong parameter for usage.");        //ispis u Log fajl za unos losih parametara
                                    Console.WriteLine("Error, wrong input for usage");
                                }
                                dbp.ElEnergySpent = eUsage;
                                if (proxy.Add(clientCertCN,dbp))
                                {
                                    Console.WriteLine("Added to database!");
                                }
                                else
                                {
                                    Console.WriteLine("Error! Information can't be added to database!");
                                }
                                continue;
                            case 4:
                                DBParam dbp1 = new DBParam();
                                string path = "C://Users//Administrator.DOMAINADMINS0//Desktop//DataKeep//Vezba5 (resenje)//ServiceApp//bin//Debug//DataBase.txt";
                                Console.WriteLine("Current database: ");
                                using (StreamReader sr = new StreamReader(path))
                                {
                                    String line;
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        Console.WriteLine(line);
                                    }
                                }
                                Console.WriteLine("Enter ID of information you want to change: ");
                                int id1;
                                string sid1 = Console.ReadLine();
                                while (!Int32.TryParse(sid1, out id1))
                                {
                                    //               Audit.AddFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
                                    Console.WriteLine("Error, wrong input for year");
                                }
                                //dodati proveru da li postoji id u bazi
                                dbp1.Id = id1;



                                if (proxy.Edit(clientCertCN,dbp1))
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
