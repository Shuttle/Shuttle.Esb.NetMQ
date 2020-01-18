using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Reflection;

namespace Shuttle.Esb.NetMQ
{
    public interface IReceiveExceptionObserver : IPipelineObserver<OnPipelineException>
    {
    }

    public class ReceiveExceptionObserver : IReceiveExceptionObserver
    {
        private readonly ILog _log;

        public ReceiveExceptionObserver()
        {
            _log = Log.For(this);
        }

        public void Execute(OnPipelineException pipelineEvent)
        {
            _log.Error(pipelineEvent.Pipeline.Exception.AllMessages());

            pipelineEvent.Pipeline.MarkExceptionHandled();
        }
    }
}