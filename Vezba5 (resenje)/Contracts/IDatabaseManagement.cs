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

        [OperationContract]
        bool Add(string userName);

        [OperationContract]
<<<<<<< HEAD
        bool Edit(string database,string userName);
=======
        bool Edit(string userName);

        [OperationContract]
        bool CreateDatabase(string userName);

        [OperationContract]
        bool DeleteDatabase(string userName);
>>>>>>> 6848f57ed3758e1d5fe2ad9c4eac414f0142f89a
    }
}
