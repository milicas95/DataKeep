using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

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
    }
}
