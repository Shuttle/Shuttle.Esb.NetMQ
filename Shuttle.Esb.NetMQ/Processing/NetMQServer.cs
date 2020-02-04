using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQServer : INetMQServer, IDisposable
    {
        private ProcessorThreadPool _threadPool;

        public NetMQServer(IPipelineFactory pipelineFactory)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));

            _threadPool =
                new ProcessorThreadPool("ServerProcessor", 1, new ServerProcessorFactory(pipelineFactory));
        }

        public void Dispose()
        {
            Stop();
        }

        public INetMQServer Start()
        {
            _threadPool.Start();

            return this;
        }

        public void Stop()
        {
            _threadPool?.Dispose();
            _threadPool = null;
        }
    }
}