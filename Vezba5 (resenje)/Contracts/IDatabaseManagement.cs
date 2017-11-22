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
        bool Edit(string userName, DBParam bdp);

        [OperationContract]
        bool CreateDatabase(string userName);

        [OperationContract]
        bool DeleteDatabase(string userName);
    }
}
