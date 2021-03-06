﻿using Shuttle.Core.Contract;
using Shuttle.Core.Logging;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ.Server.Pipelines.Observers
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