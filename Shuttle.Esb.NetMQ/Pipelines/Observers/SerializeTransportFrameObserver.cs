using System;
using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;
using Shuttle.Esb.NetMQ.Frames;

namespace Shuttle.Esb.NetMQ
{
    public interface ISerializeTransportFrameObserver : IPipelineObserver<OnSerializeTransportFrame>
    {
    }

    public class SerializeTransportFrameObserver : ISerializeTransportFrameObserver
    {
        private readonly ILog _log;
        private readonly ISerializer _serializer;

        public SerializeTransportFrameObserver(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            _serializer = serializer;

            _log = Log.For(this);
        }

        public void Execute(OnSerializeTransportFrame pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var transportFrame = state.Get<TransportFrame>(StateKeys.TransportFrame);

            Guard.AgainstNull(transportFrame, nameof(transportFrame));

            state.SetTransportMessageStream(_serializer.Serialize(transportFrame));
        }
    }
}