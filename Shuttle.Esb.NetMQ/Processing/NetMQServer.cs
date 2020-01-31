using System;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Threading;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQServer : INetMQServer, IDisposable
    {
        private readonly IPipelineFactory _pipelineFactory;
        private ProcessorThreadPool _threadPool;

        public NetMQServer(IPipelineFactory pipelineFactory)
        {
            Guard.AgainstNull(pipelineFactory, nameof(pipelineFactory));

            _pipelineFactory = pipelineFactory;

            _threadPool =
                new ProcessorThreadPool("ServerProcessor", 1, new ServerProcessorFactory(_pipelineFactory));
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            _threadPool.Start();
        }

        public void Stop()
        {
            _threadPool?.Dispose();
            _threadPool = null;
        }
    }
}