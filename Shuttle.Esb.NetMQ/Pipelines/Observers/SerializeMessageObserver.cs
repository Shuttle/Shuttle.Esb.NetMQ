using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;
using Shuttle.Core.Streams;
using Shuttle.Esb.NetMQ.Frames;

namespace Shuttle.Esb.NetMQ
{
    public interface ISerializeMessageObserver : IPipelineObserver<OnDeserializeMessage>
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

        public void Execute(OnDeserializeMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var message = state.GetMessage();
            var transportFrame = state.Get<TransportFrame>();

            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(transportFrame, nameof(transportFrame));

            using (var stream = _serializer.Serialize(message))
            {
                transportFrame.Message = stream.ToBytes();
                state.SetMessageBytes(transportFrame.Message);
            }
        }
    }
}