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
                        int id = Int32.Parse(separeted[0]);
                        string region = separeted[1];
                        string cit = separeted[2];
                        int year = Int32.Parse(separeted[3]);
                        string month = separeted[4];
                        int eUsage = Int32.Parse(separeted[5]);

                        DBParam p = new DBParam(region, cit, year, month, eUsage);
                        if (p.City == city)
                        {
                            wantedCity.Add(p);
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
                        int id = Int32.Parse(separeted[0]);
                        string reg = separeted[1];
                        string city = separeted[2];
                        int year = Int32.Parse(separeted[3]);
                        string month = separeted[4];
                        int eUsage = Int32.Parse(separeted[5]);

                        DBParam p = new DBParam(reg, city, year, month, eUsage);
                        if (p.Region == region)
                        {
                            wantedReg.Add(p);
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
                        int id = Int32.Parse(separeted[0]);
                        string reg = separeted[1];
                        string city = separeted[2];
                        int year = Int32.Parse(separeted[3]);
                        string month = separeted[4];
                        int eUsage = Int32.Parse(separeted[5]);

                        DBParam p = new DBParam(reg, city, year, month, eUsage);
                        if (p.Region == region)
                        {
                            wantedReg.Add(p);
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

        public bool Add(string database, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Writers");
        //    Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik

            if (cert != null)
            {
             //   Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat

                if (File.Exists(database))
                {
              //      Audit.ReadSuccess(database);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka

                    try
                    {
               //         Audit.ReadSuccess(database);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka

                        Console.WriteLine("Enter region: ");
                        string region = Console.ReadLine();

                        Console.WriteLine("Enter city: ");
                        string city = Console.ReadLine();

                        Console.WriteLine("Enter year: ");
                        int year;
                        string syear = Console.ReadLine();

                        while (!Int32.TryParse(syear, out year))
                        {
                            Audit.AddFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
                            Console.WriteLine("Error, wrong input for year");
                        }

                        Console.WriteLine("Enter month: ");
                        string month = Console.ReadLine();

                        Console.WriteLine("Enter usage: ");
                        int eUsage;
                        string seUsage = Console.ReadLine();

                        while (!Int32.TryParse(seUsage, out eUsage))
                        {
                            Audit.AddFailed("User " + userName + " put wrong parameter for usage.");        //ispis u Log fajl za unos losih parametara
                            Console.WriteLine("Error, wrong input for usage");
                        }

                        DBParam p = new DBParam(region, city, year, month, eUsage);

                        using (StreamWriter sw = new StreamWriter(database))
                        {
                            sw.WriteLine(p.ID + "/" + p.Region + "/" + p.City + "/" + p.Year + "/" + p.Month + "/" + p.ElEnergySpent);
                        }

         //               Audit.AddSuccess();         //ispis u Log fajlu da je dodat podatak u datoteku

                        Console.WriteLine("Added to database!");
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
        //            Audit.ReadFailed(database, "Database doesn't exist.");          //ispis u Log fajlu da datoteka ne postoji
                    Console.WriteLine("Database with that name doesn't exist!");
                    return false;
                }
            }
            else
            {
     //           Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return false;
            }
        }

        public bool Edit(string database, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Writers");
     //       Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);       //ispis da je autentifikovan korisnik

            if (cert != null)
            {
    //            Audit.CertificateSuccess();     //ispis u Log fajl da je ok certifikat

                if (File.Exists(database))
                {
                    try
                    {
    //                    Audit.ReadSuccess(database);        //ispis u Log fajl da je uspesno procitana (otvorena) datoteka

                        Console.WriteLine("Current database: ");

                        using (StreamReader sr = new StreamReader(database))
                        {
                            String line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                Console.WriteLine(line);
                            }
                        }


                        Console.WriteLine("Enter ID of information you want to change: ");
                        int id;
                        string sid = Console.ReadLine();
                        while (!Int32.TryParse(sid, out id))
                        {
         //                   Audit.UpdateFailed("User " + userName + " put wrong parameter for id.");     //ispis u Log fajl za unos losih parametara
                            Console.WriteLine("Error, wrong format for id");
                        }

                        int[] idDB = new int[] { };

                        using (StreamReader sr = new StreamReader(database))
                        {
                            string path = "";
                            string[] lines = File.ReadAllLines(path);

                            for (int i = 0; i < lines.Count(); i++)
                            {
                                string[] separeted = lines[i].Split('/');

                                if (!Int32.TryParse(separeted[0], out idDB[i]))
                                {
          //                          Audit.UpdateFailed("Convertion of int to string failed.");     //ispis u Log fajl za gresku
                                    Console.WriteLine("Error while trying to convert int to string.");
                                }

                            }

                            for (int i = 0; i < idDB.Length; i++)
                            {
                                if (id == idDB[i])
                                {
                                    Console.WriteLine("Enter region: ");
                                    string region = Console.ReadLine();

                                    Console.WriteLine("Enter city: ");
                                    string city = Console.ReadLine();

                                    Console.WriteLine("Enter year: ");
                                    int year;
                                    string syear = Console.ReadLine();
                                    while (!Int32.TryParse(syear, out year))
                                    {
                                        Audit.UpdateFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
                                        Console.WriteLine("Error, wrong input for year");
                                    }

                                    Console.WriteLine("Enter month: ");
                                    string month = Console.ReadLine();

                                    Console.WriteLine("Enter usage: ");
                                    int eUsage;
                                    string seUsage = Console.ReadLine();
                                    while (!Int32.TryParse(seUsage, out eUsage))
                                    {
                                        Audit.UpdateFailed("User " + userName + " put wrong parameter for year.");     //ispis u Log fajl za unos losih parametara
                                        Console.WriteLine("Error, wrong input for usage");
                                    }

                                    DBParam p = new DBParam(region, city, year, month, eUsage);

                                    using (StreamWriter sw = new StreamWriter(database))
                                    {
                                        sw.WriteLine(p.ID + "/" + p.Region + "/" + p.City + "/" + p.Year + "/" + p.Month + "/" + p.ElEnergySpent);
                                    }

          //                          Audit.UpdateSuccess();         //ispis u Log fajlu da je promenjen podatak
                                    Console.WriteLine("Database edited!");
                                }
                            }
                        }

                        return true;
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
   //                 Audit.ReadFailed(database, "Database doesn't exist.");          //ispis u Log fajlu da datoteka ne postoji
                    Console.WriteLine("Database with that name doesn't exist!");
                    return false;
                }
            }
            else
            {
    //            Audit.CertificateFailed();
                Console.WriteLine("Certificate is invalid");
                return false;
            }
        }
    }
}
