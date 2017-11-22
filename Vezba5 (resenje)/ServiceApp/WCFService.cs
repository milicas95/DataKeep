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
    
    public class WCFService : IDatabaseManagement
    {
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
    //                Audit.ReadFailed(path, "Exception was thrown");     //ispis u Log fajlu da korisnik nema pravo pisanja
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
            //    Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik
            string path = "DataBase.txt";

            if (cert != null)
            {
                //   Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat
                //      Audit.ReadSuccess(database);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka

                try
                {

                    using (StreamWriter sw = new StreamWriter(path, true))
                    {
                        sw.WriteLine(dbp.Id + "/" + dbp.Region + "/" + dbp.City + "/" + dbp.Year + "/" + dbp.Month + "/" + dbp.ElEnergySpent);
                    }

                    dbp.IDs.Add(dbp.Id);
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
                    
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        sw.WriteLine(dbp.Id + "/" + dbp.Region + "/" + dbp.City + "/" + dbp.Year + "/" + dbp.Month + "/" + dbp.ElEnergySpent);
                    }

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

        public bool CreateDatabase(string userName)
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
                        File.CreateText(path);
                        Console.WriteLine("Successfuly created new database!");
                        return true;
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }

                }
                else
                {
                    Console.WriteLine("Database already exists");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Certificate is invalid");
                return false;
            }
        }

        public bool DeleteDatabase(string userName)
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
                        Console.WriteLine("Successfuly deleted database!");
                        return true;
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                        return false;
                    }

                }
                else
                {
                    Console.WriteLine("Database already exists");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Certificate is invalid");
                return false;
            }
        }
    }
}
