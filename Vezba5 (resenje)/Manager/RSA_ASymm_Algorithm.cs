using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Manager
{
    public class RSA_ASymm_Algorithm
    {
        public static byte[] RSAEncrypt(byte[] dataToEncrypt, PublicKey publicKey)
        {
            try
            {
                byte[] encryptedData = null;

                using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)publicKey.Key)
                {
                    encryptedData = csp.Encrypt(dataToEncrypt, false);
                }
                return encryptedData;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with RSA encryption! Message: {0}",e.Message);
                return null;
            }

            /*
            try
            {
                byte[] encryptedData = null;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    // Import the RSA Key information. This only needs to include the public key information.
                    RSA.ImportParameters(RSAKeyInfo);


                    //Encrypt the passed byte array and specify OAEP padding.  
                    encryptedData =  RSA.Encrypt(dataToEncrypt, DoOAEPPadding);
                }

                return encryptedData;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);

                return null;
            }*/
        }

        public static byte[] RSADecrypt(byte[] dataToDecrypt, X509Certificate2 cert)
        {
            try
            {

                byte[] decryptedData = null;
                using (RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey)
                {
                    decryptedData = csp.Decrypt(dataToDecrypt, false);
                }
                return decryptedData;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error with RSA decryption! Message: {0}", e.Message);
                return null;
            }

            /*
            try
            {
                byte[] decryptedData = null;

                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    // Import the RSA Key information. This only needs to include the public key information.
                    RSA.ImportParameters(RSAKeyInfo);

                    //Encrypt the passed byte array and specify OAEP padding.  
                    decryptedData = RSA.Decrypt(dataToDecrypt, DoOAEPPadding);
                }

                return decryptedData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return null;
            }*/
        }

        public static byte[] GenerateSessionKey()
        {
            byte[] session_key = new byte[64];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(session_key);

            /*
            using (RijndaelManaged rijnAlg = new RijndaelManaged())
            {
                rijnAlg.KeySize = 64;
                rijnAlg.GenerateKey();
                session_key = rijnAlg.Key;
            }*/

            return session_key;
        }



    }
}
