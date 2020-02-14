using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;
using Shuttle.Core.Streams;

namespace Shuttle.Esb.NetMQ.Server.Pipelines.Observers
{
    public interface ISerializeMessageObserver : IPipelineObserver<OnSerializeMessage>
    {
    }

    public class SerializeMessageObserver : ISerializeMessageObserver
    {
        private readonly ISerializer _serializer;

        public SerializeMessageObserver(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            _serializer = serializer;
        }

        public void Execute(OnSerializeMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var message = state.Get<object>(StateKeys.Response);
            var transportFrame = state.Get<TransportFrame>(StateKeys.TransportFrame);

            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(transportFrame, nameof(transportFrame));

            using (var stream = _serializer.Serialize(message))
            {
                transportFrame.Message = stream.ToBytes();
            }
        }
    }
}