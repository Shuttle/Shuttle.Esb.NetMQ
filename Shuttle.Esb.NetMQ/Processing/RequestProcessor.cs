using System.Threading;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.NetMQ
{
    public class RequestProcessor : IProcessor
    {
        public RequestProcessor()
        {
        }

        public void Execute(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}