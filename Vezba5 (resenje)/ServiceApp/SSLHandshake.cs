using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using Manager;
using System.Security.Principal;

namespace ServiceApp
{
    public class SSLHandshake : ISSLHandshake
    {
        static byte[] session_key;

        public X509Certificate2 RequestSession()
        {
            X509Certificate2 serverCertificate;
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            serverCertificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN, "Servers");

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
    }
}
