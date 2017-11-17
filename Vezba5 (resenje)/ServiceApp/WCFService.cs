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

namespace ServiceApp
{
    public class WCFService : IDatabaseManagement
    {

        public int AverageUsageInCity(string city, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Readers");
            if (cert != null)
            {
                string path = "DataBase.txt";
                string[] lines = File.ReadAllLines(path);
                List<DBParam> wantedCity = new List<DBParam>();
                
                for(int i = 0; i < lines.Count(); i++)
                {
                    string[] separeted = lines[i].Split('/');
                    int id = Int32.Parse(separeted[0]);
                    string region = separeted[1];
                    string cit = separeted[2];
                    int year = Int32.Parse(separeted[3]);
                    string month = separeted[4];
                    int eUsage = Int32.Parse(separeted[5]);

                    DBParam p = new DBParam(region, cit, year, month, eUsage);
                    if (p.City == cit)
                    {
                        wantedCity.Add(p);
                    }
                }

                int totalUsage = 0;
                for(int i = 0; i < wantedCity.Count; i++)
                {
                    totalUsage += wantedCity[i].ElEnergySpent;
                }
                totalUsage = totalUsage / wantedCity.Count;
                return totalUsage;
            }
            return -1;
        }

        public int AverageUsageInRegion(string region, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Readers");
            if (cert != null)
            {
                string path = "DataBase.txt";
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

                int totalUsage = 0;
                for (int i = 0; i < wantedReg.Count; i++)
                {
                    totalUsage += wantedReg[i].ElEnergySpent;
                }
                totalUsage = totalUsage / wantedReg.Count;
                return totalUsage;
            }
            return -1;
        }

        public string HighestSpenderInRegion(string region, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Readers");
            if (cert != null)
            {
                string path = "DataBase.txt";
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

                Dictionary<string,int> citiesInRegion = new Dictionary<string, int>();
                foreach(DBParam p in wantedReg)
                {
                    if (citiesInRegion.ContainsKey(p.City))
                    {
                        citiesInRegion[p.City] += p.ElEnergySpent;
                    }
                    else
                        citiesInRegion.Add(p.City,p.ElEnergySpent);
                }
                List<KeyValuePair<string,int>> lista=citiesInRegion.ToList();
                string hs = lista[0].Key;
                int hsVal = lista[0].Value;
                for(int i = 1; i < citiesInRegion.Count; i++)
                {
                    if (lista[i].Value > hsVal)
                    {
                        hsVal = lista[i].Value;
                        hs = lista[i].Key;
                    }
                }
                return hs;
            }
            else
                return "You can't use this option";
        }

        public bool Add(string database,string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Writers");

            if (cert != null)
            {
                if (File.Exists(database))
                {
                    try
                    {
                        Console.WriteLine("Enter region: ");
                        string region = Console.ReadLine();
                        Console.WriteLine("Enter city: ");
                        string city = Console.ReadLine();
                        Console.WriteLine("Enter year: ");
                        int year = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter month: ");
                        string month = Console.ReadLine();
                        Console.WriteLine("Enter usage: ");
                        int eUsage = Convert.ToInt32(Console.ReadLine());

                        DBParam p = new DBParam(region, city, year, month, eUsage);

                        using (StreamWriter sw = new StreamWriter(database))
                        {
                            sw.WriteLine(p.ID + "/" + p.Region + "/" + p.City + "/" + p.Year + "/" + p.Month + "/" + p.ElEnergySpent);
                        }

                        Console.WriteLine("Added to database!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("Database with that name doesn't exist!");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool Edit(string database, string userName)
        {
            X509Certificate2 cert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, userName, "Writers");

            if (cert != null)
            {
                if (File.Exists(database))
                {
                    try
                    {
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
                        int id = Convert.ToInt32(Console.ReadLine());

                        int[] idDB = new int[] { };

                        using (StreamReader sr = new StreamReader(database))
                        {
                            string path = "";
                            string[] lines = File.ReadAllLines(path);

                            for (int i = 0; i < lines.Count(); i++)
                            {
                                string[] separeted = lines[i].Split('/');
                                idDB[i] = Convert.ToInt32(separeted[0]);
                            }

                            for(int i=0; i<idDB.Length; i++)
                            {
                                if(id==idDB[i])
                                {
                                    Console.WriteLine("Enter region: ");
                                    string region = Console.ReadLine();
                                    Console.WriteLine("Enter city: ");
                                    string city = Console.ReadLine();
                                    Console.WriteLine("Enter year: ");
                                    int year = Convert.ToInt32(Console.ReadLine());
                                    Console.WriteLine("Enter month: ");
                                    string month = Console.ReadLine();
                                    Console.WriteLine("Enter usage: ");
                                    int eUsage = Convert.ToInt32(Console.ReadLine());

                                    DBParam p = new DBParam(region, city, year, month, eUsage);

                                    using (StreamWriter sw = new StreamWriter(database))
                                    {
                                        sw.WriteLine(p.ID + "/" + p.Region + "/" + p.City + "/" + p.Year + "/" + p.Month + "/" + p.ElEnergySpent);
                                    }

                                    Console.WriteLine("Database edited!");
                                }
                            }
                        }

                        return true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }

                    return true;
                }
                else
                {
                    Console.WriteLine("Database with that name doesn't exist!");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
