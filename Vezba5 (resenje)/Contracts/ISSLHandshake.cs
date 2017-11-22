using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;

namespace Contracts
{
    [ServiceContract]
    public interface ISSLHandshake
    {
        //Client sends a request for a new session, and as a result Server returns to client his public key
        [OperationContract]
        X509Certificate2 RequestSession();

        [OperationContract]
        bool SendSessionKey(byte[] session_key);

    }
}
