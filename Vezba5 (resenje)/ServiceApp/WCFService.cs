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
                string path = "";
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

                    DBParam p = new DBParam(id, region, cit, year, month, eUsage);
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
                string path = "";
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

                    DBParam p = new DBParam(id, reg, city, year, month, eUsage);
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
                string path = "";
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

                    DBParam p = new DBParam(id, reg, city, year, month, eUsage);
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
    }
}
