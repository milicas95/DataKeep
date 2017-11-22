using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using DBparam;

namespace Contracts
{
    [ServiceContract]
    public interface IDatabaseManagement
    {
        [OperationContract]
        int AverageUsageInCity(string city, string userName);

        [OperationContract]
        int AverageUsageInRegion(string region, string userName);

        [OperationContract]
        string HighestSpenderInRegion(string region, string userName);

        [OperationContract]
        bool Add(string userName, DBParam bdp);

        [OperationContract]
<<<<<<< HEAD
        bool Edit(string userName, DBParam bdp);
=======
<<<<<<< HEAD
        bool Edit(string database,string userName);
=======
        bool Edit(string userName);
>>>>>>> a0fc13df0e33cfbcf78c2a6d0ada03d1c16601e3

        [OperationContract]
        bool CreateDatabase(string userName);

        [OperationContract]
        bool DeleteDatabase(string userName);
>>>>>>> 6848f57ed3758e1d5fe2ad9c4eac414f0142f89a
    }
}
