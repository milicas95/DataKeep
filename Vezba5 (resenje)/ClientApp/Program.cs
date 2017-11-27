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
        static byte[] session_key = null;

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

                using (SSLHandshakeClient proxyshake = new SSLHandshakeClient(binding, address))
                {
                    byte[] encrypted_session_key = null;
                    X509Certificate2 serverCert = null;

                    serverCert = proxyshake.RequestSession();
                    if (serverCert == null)
                    {
                        Console.WriteLine("Error with servers public key!");
                    }

                    session_key = RSA_ASymm_Algorithm.GenerateSessionKey();

                    encrypted_session_key = RSA_ASymm_Algorithm.RSAEncrypt(session_key, serverCert.PublicKey);
                    if (encrypted_session_key == null)
                    {
                        Console.WriteLine("Error with encryption of session key!");
                    }

                    bool result = proxyshake.SendSessionKey(encrypted_session_key);
                    if (result == false)
                    {
                        Console.WriteLine("Error with decryption of session key!");
                    }
                }

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

                        string s;
                        switch (action)
                        {
                            case 0:
                                continue;
                            case 1:
                                s = proxy.CreateDatabase(clientCertCN);
                                if (s != "")
                                    Console.WriteLine(s);
                                continue;
                            case 2:
                                s = proxy.DeleteDatabase(clientCertCN);
                                if (s != "")
                                    Console.WriteLine(s);
                                continue;
                            case 3:
                                DBParam dbp = new DBParam();
                                string path1 = "C://Users//Administrator.DOMAINADMINS0//Desktop//DataKeep//Vezba5 (resenje)//ServiceApp//bin//Debug//DataBase.txt";
                                List<int> IDs = new List<int>();
                                string[] lines = File.ReadAllLines(path1);
                                for (int i = 0; i < lines.Count(); i++)
                                {
                                    string[] separeted = lines[i].Split('/');
                                    IDs.Add(Convert.ToInt32(separeted[0]));
                                }
                                Console.WriteLine("Enter ID: ");
                                int id;
                                string sid = Console.ReadLine();
                                while (!Int32.TryParse(sid, out id))
                                {
                                    //Audit.AddFailed("User " + userName + " put wrong parameter for id.");     //ispis u Log fajl za unos losih parametara
                                    Console.WriteLine("Error, wrong input for ID");
                                }
                                int find = 0;
                                foreach (int i in IDs)
                                {
                                    if (i == id)
                                    {
                                        find = 1;
                                    }
                                }
                                switch(find)
                                {
                                    case 0:
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
                                            //Audit.AddFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
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
                                            //Audit.AddFailed("User " + userName + " put wrong parameter for usage.");        //ispis u Log fajl za unos losih parametara
                                            Console.WriteLine("Error, wrong input for usage");
                                        }
                                        dbp.ElEnergySpent = eUsage;
                                        if (proxy.Add(clientCertCN, dbp))
                                        {
                                            Console.WriteLine("Added to batabase!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error! Information can't be added!");
                                        }
                                        break;
                                    case 1:
                                        //Audit.AddFailed("ID already exists.");     //ispis u Log fajl za unos losih parametara
                                        Console.WriteLine("ID exists!");
                                        break;
                                    default:
                                        break;
                                }
                                continue;
                            case 4:
                                string path2 = "C://Users//Administrator.DOMAINADMINS0//Desktop//DataKeep//Vezba5 (resenje)//ServiceApp//bin//Debug//DataBase.txt";
                                DBParam dbp1 = new DBParam();
                                List<int> IDs1 = new List<int>();

                                string[] lines1 = File.ReadAllLines(path2);

                                for (int i = 0; i < lines1.Count(); i++)
                                {
                                    string[] separeted = lines1[i].Split('/');
                                    IDs1.Add(Convert.ToInt32(separeted[0]));
                                }
                                Console.WriteLine("Current database: ");
                                using (StreamReader sr = new StreamReader(path2))
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
                                    //Audit.EditFailed("User " + userName + " put wrong parameter for ID.");     //ispis u Log fajl za unos losih parametara
                                    Console.WriteLine("Error, wrong input for ID");
                                }
                                int find1 = 0;
                                foreach (int i in IDs1)
                                {
                                    if (i == id1)
                                    {
                                        find1 = 1;
                                    }
                                }
                                switch (find1)
                                {
                                    case 0:
                                        //Audit.AddFailed("ID already exists.");     //ispis u Log fajl za unos losih parametara
                                        Console.WriteLine("ID doesn't exists!");
                                        break;
                                    case 1:
                                        int counter = 0;
                                        string[] lines2 = File.ReadAllLines(path2);
                                        for (int i = 0; i < lines2.Count(); i++)
                                        {
                                            string[] separeted = lines2[i].Split('/');
                                            if (separeted[0] != sid1)          //pronadjena ta linija IDja
                                            {
                                                counter++;      //broji na kojoj liniji je taj entitet
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        Console.WriteLine("Enter new informations: ");
                                        dbp1.Id = id1;
                                        Console.WriteLine("Region");
                                        dbp1.Region = Console.ReadLine();
                                        Console.WriteLine("City");
                                        dbp1.City = Console.ReadLine();
                                        Console.WriteLine("Year");
                                        dbp1.Year = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Month");
                                        dbp1.Month = Console.ReadLine();
                                        Console.WriteLine("Usage");
                                        dbp1.ElEnergySpent = Convert.ToInt32(Console.ReadLine());
                                        dbp1.CNT = counter;
                                        if (proxy.Edit(clientCertCN, dbp1))
                                        {
                                            Console.WriteLine("Database edited!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Error! Information can't be edited!");
                                        }
                                        break;
                                    default:
                                        break;
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
