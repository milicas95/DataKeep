using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contracts;
using Manager;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.IO;
using DBparam;
using System.ServiceModel;

namespace ServiceApp
{
    
    public class WCFService : IDatabaseManagement,ISSLHandshake
    {

        # region SSL Handshake
        static byte[] session_key;

        public X509Certificate2 RequestSession()
        {
            X509Certificate2 serverCertificate;
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            serverCertificate = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN, "Servers");
            //serverCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN, "Servers");

            return serverCertificate;
        }

        public bool SendSessionKey(byte[] encrypted_session_key)
        {
            //byte[] sessionkey = null;

            X509Certificate2 serverCertificate;
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            serverCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN, "Servers");

            session_key = RSA_ASymm_Algorithm.RSADecrypt(encrypted_session_key, serverCertificate);

            return true;

        }

        public byte[] GetSessionKey()
        {
            return session_key;
        }

        #endregion

        // autorizacija ide samo kod pravljenja i brisanja datoteka !!!!!!

        public int AverageUsageInCity(string city, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Readers");
            //         Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik
            DBParam dbp = new DBParam();

            string path = "DataBase.txt";
            int totalUsage = 0;

            if (cert != null)
            {
  //              Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat

                try
                {
                    string[] lines = File.ReadAllLines(path);
                    List<DBParam> wantedCity = new List<DBParam>();

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        string[] separeted = lines[i].Split('/');
                        dbp.Id = Int32.Parse(separeted[0]);
                        dbp.Region = separeted[1];
                        dbp.City = separeted[2];
                        dbp.Year = Int32.Parse(separeted[3]);
                        dbp.Month = separeted[4];
                        dbp.ElEnergySpent = Int32.Parse(separeted[5]);

                        if (dbp.City == city)
                        {
                            wantedCity.Add(dbp);
                        }
                    }

                    for (int i = 0; i < wantedCity.Count; i++)
                    {
                        totalUsage += wantedCity[i].ElEnergySpent;
                    }
                    totalUsage = totalUsage / wantedCity.Count;
                }
                catch (Exception e)
                {
                    Audit.ReadFailed(path, "Exception was thrown");     //ispis u Log fajlu da korisnik nema pravo pisanja
                    Console.WriteLine("Exception was thrown:");
                    Console.WriteLine(e.Message);
                }

                return totalUsage;
            }
            else
            {
         //       Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return -1;
            }
        }

        public int AverageUsageInRegion(string region, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Readers");
            //         Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik
            DBParam dbp = new DBParam();

            string path = "DataBase.txt";
            int totalUsage = 0;

            if (cert != null)
            {
//                Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat

                try
                {
   //                 Audit.ReadSuccess(path);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka

                    string[] lines = File.ReadAllLines(path);
                    List<DBParam> wantedReg = new List<DBParam>();

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        string[] separeted = lines[i].Split('/');
                        dbp.Id = Int32.Parse(separeted[0]);
                        dbp.Region = separeted[1];
                        dbp.City = separeted[2];
                        dbp.Year = Int32.Parse(separeted[3]);
                        dbp.Month = separeted[4];
                        dbp.ElEnergySpent = Int32.Parse(separeted[5]);

                        if (dbp.Region == region)
                        {
                            wantedReg.Add(dbp);
                        }
                    }

                    for (int i = 0; i < wantedReg.Count; i++)
                    {
                        totalUsage += wantedReg[i].ElEnergySpent;
                    }

                    totalUsage = totalUsage / wantedReg.Count;
                }
                catch (Exception e)
                {
  //                  Audit.ReadFailed(path,"Exception was thrown");     //ispis u Log fajlu da korisnik nema pravo pisanja
                    Console.WriteLine("Exception was thrown:");
                    Console.WriteLine(e.Message);
                }

                return totalUsage;
            }
            else
            {
  //              Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return -1;
            }
        }

        public string HighestSpenderInRegion(string region, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Readers");
            //           Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik
            DBParam dbp = new DBParam();

            string path = "DataBase.txt";
            string hs="";

            if (cert != null)
            {
  //              Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat

                try
                {
//                    Audit.ReadSuccess(path);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka
                    
                    string[] lines = File.ReadAllLines(path);

                    List<DBParam> wantedReg = new List<DBParam>();

                    for (int i = 0; i < lines.Count(); i++)
                    {
                        string[] separeted = lines[i].Split('/');
                        dbp.Id = Int32.Parse(separeted[0]);
                        dbp.Region = separeted[1];
                        dbp.City = separeted[2];
                        dbp.Year = Int32.Parse(separeted[3]);
                        dbp.Month = separeted[4];
                        dbp.ElEnergySpent = Int32.Parse(separeted[5]);

                        if (dbp.Region == region)
                        {
                            wantedReg.Add(dbp);
                        }
                    }

                    Dictionary<string, int> citiesInRegion = new Dictionary<string, int>();
                    foreach (DBParam p in wantedReg)
                    {
                        if (citiesInRegion.ContainsKey(p.City))
                        {
                            citiesInRegion[p.City] += p.ElEnergySpent;
                        }
                        else
                            citiesInRegion.Add(p.City, p.ElEnergySpent);
                    }
                    List<KeyValuePair<string, int>> lista = citiesInRegion.ToList();
                    hs = lista[0].Key;
                    int hsVal = lista[0].Value;
                    for (int i = 1; i < citiesInRegion.Count; i++)
                    {
                        if (lista[i].Value > hsVal)
                        {
                            hsVal = lista[i].Value;
                            hs = lista[i].Key;
                        }
                    }
                }
                catch (Exception e)
                {
  //                  Audit.ReadFailed(path, "Exception was thrown");     //ispis u Log fajlu da korisnik nema pravo pisanja
                    Console.WriteLine("Exception was thrown:");
                    Console.WriteLine(e.Message);
                }

                return hs;
            }
            else
            {
              //  Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return "You can't use this option";
            }
        }

        public bool Add(string userName, DBParam dbp)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Writers");
            //Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik
            string path = "DataBase.txt";

            if (cert != null)
            {
                Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat
                //      Audit.ReadSuccess(database);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka

                try
                {
                    StreamWriter sw;
                    using (sw = new StreamWriter(path, true))
                    {
                        sw.WriteLine(dbp.Id + "/" + dbp.Region + "/" + dbp.City + "/" + dbp.Year + "/" + dbp.Month + "/" + dbp.ElEnergySpent);
                    }
                    sw.Close();
                    //         Audit.AddSuccess();         //ispis u Log fajlu da je dodat podatak u datoteku
                }
                catch (Exception e)
                {
                    //               Audit.AddFailed("Exception was thrown");     //ispis u Log fajlu da korisnik nema pravo pisanja
                    Console.WriteLine("Exception was thrown:");
                    Console.WriteLine(e.Message);
                }

                return true;
            }
            else
            {
                //           Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return false;
            }
        }

        public bool Edit(string userName, DBParam dbp)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Writers");
            //       Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik
            string path = "DataBase.txt";

            if (cert != null)
            {
                //            Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat

                try
                {
                    //                    Audit.ReadSuccess(database);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka
                    string[] text = File.ReadAllLines(path);
                    text[dbp.CNT] = dbp.Id + "/" + dbp.Region + "/" + dbp.City + "/" + dbp.Month + "/" + dbp.ElEnergySpent;
                    File.WriteAllLines(path, text);
                    //       Audit.UpdateSuccess();
                }
                 catch (Exception e)
                 {
                 //              Audit.UpdateFailed("Exception was thrown");     //ispis u Log fajlu da korisnik nema pravo pisanja
                    Console.WriteLine("Exception was thrown: ");
                    Console.WriteLine(e.Message);
                 }

                 return true;
             }
             else
             {
                //            Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return false;
            }
        }

        public string CreateDatabase(string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Admins");

            string path = "DataBase.txt";

            if (cert != null)
            {
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    try
                    {
                        var myFile=File.CreateText(path);
                        myFile.Close();
                        //Console.WriteLine("Successfuly created new database!");
                        return "Successfuly created new database!";
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return e.Message;
                    }

                }
                else
                {
                    //Console.WriteLine("Database already exists");
                    return "Database already exists";
                }
            }
            else
            {
                //Console.WriteLine("Certificate is invalid");
                return "Certificate is invalid";
            }
        }

        public string DeleteDatabase(string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Admins");

            string path = "DataBase.txt";

            if (cert != null)
            {
                if (File.Exists(path))
                {
                    // Delete a file 
                    try
                    {
                        File.Delete(path);
                        //Console.WriteLine("Successfuly deleted database!");
                        return "Successfuly deleted database!";
                    }
                    catch (IOException e)
                    {
                        //Console.WriteLine(e.Message);
                        return e.Message;
                    }

                }
                else
                {
                    //Console.WriteLine("Database already exists");
                    return "Database doesn't exists";
                }
            }
            else
            {
                //Console.WriteLine("Certificate is invalid");
                return "Certificate is invalid";
            }
        }

    }
}
