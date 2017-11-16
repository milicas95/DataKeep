﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security;

namespace Manager
{
	public class CertManager
	{
		/// <summary>
		/// Get a certificate with the specified subject name from the predefined certificate storage
		/// Only valid certificates should be considered
		/// </summary>
		/// <param name="storeName"></param>
		/// <param name="storeLocation"></param>
		/// <param name="subjectName"></param>
		/// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
		public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName,string group)
		{
            X509Certificate2 certificate = null;
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            foreach (X509Certificate2 cert in store.Certificates)
            {
                if (cert.SubjectName.Equals(string.Format("CN={0} OU={1} O={2}", subjectName, group, "DataKeepCA")))
                {
                    certificate = cert;
                    break;
                }
            }
            return certificate;
        }


		/// <summary>
		/// Get a certificate from the specified .pfx file		
		/// </summary>
		/// <param name="fileName"> .pfx file name </param>
		/// <returns> The requested certificate. If no valid certificate is found, returns null. </returns>
		public static X509Certificate2 GetCertificateFromFile(string fileName)
		{
			X509Certificate2 certificate = null;

			///In order to create .pfx file, access to a protected .pvk file will be required.
			///For security reasons, password must not be kept as string. .NET class SecureString provides a confidentiality of a plaintext
			Console.Write("Insert password for the private key: ");
			string pwd = Console.ReadLine();

			///Convert string to SecureString
			SecureString secPwd = new SecureString();
			foreach (char c in pwd)
			{
				secPwd.AppendChar(c);
			}
			pwd = String.Empty;

			/// try-catch necessary if either the speficied file doesn't exist or password is incorrect
			try
			{
				certificate = new X509Certificate2(fileName, secPwd);
			}
			catch (Exception e)
			{
				Console.WriteLine("Erroro while trying to GetCertificateFromFile {0}. ERROR = {1}", fileName, e.Message);
			}

			return certificate;
		}
	}
}
