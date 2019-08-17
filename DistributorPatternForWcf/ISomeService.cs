using System.ServiceModel;

namespace DistributorPatternForWcf
{
    [ServiceContract]

    public interface ISomeService
    {
        [OperationContract]
        void DoIt();
    }
}
