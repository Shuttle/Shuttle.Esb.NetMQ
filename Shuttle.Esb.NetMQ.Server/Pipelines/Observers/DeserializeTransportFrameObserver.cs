using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ.Server.Pipelines.Observers
{
    public interface IDeserializeTransportFrameObserver : IPipelineObserver<OnDeserializeTransportFrame>
    {
    }

    public class DeserializeTransportFrameObserver : IDeserializeTransportFrameObserver
    {
        private readonly ILog _log;
        private readonly ISerializer _serializer;

        public DeserializeTransportFrameObserver(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            _serializer = serializer;

            _log = Log.For(this);
        }

        public void Execute(OnDeserializeTransportFrame pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var stream = state.Get<Stream>(StateKeys.Stream);

            Guard.AgainstNull(stream, nameof(stream));

            var transportFrame = (TransportFrame) _serializer.Deserialize(typeof(TransportFrame), stream);

            state.Replace(StateKeys.TransportFrame, transportFrame);
        }
    }
}