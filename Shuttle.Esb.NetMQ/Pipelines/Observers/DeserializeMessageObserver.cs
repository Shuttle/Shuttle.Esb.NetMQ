using System;
using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ
{
    public interface IDeserializeMessageObserver : IPipelineObserver<OnDeserializeMessage>
    {
    }

    public class DeserializeMessageObserver : IDeserializeMessageObserver
    {
        private readonly ISerializer _serializer;

        public DeserializeMessageObserver(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            _serializer = serializer;
        }

        public void Execute(OnDeserializeMessage pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var transportFrame = state.Get<TransportFrame>(StateKeys.TransportFrame);

            Guard.AgainstNull(transportFrame, nameof(transportFrame));

            var data = transportFrame.Message;

            using (var stream = new MemoryStream(data, 0, data.Length, false, true))
            {
                state.Replace(StateKeys.Message,
                    _serializer.Deserialize(Type.GetType(transportFrame.AssemblyQualifiedName, true, true), stream));
            }
        }
    }
}