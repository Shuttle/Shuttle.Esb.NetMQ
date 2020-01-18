using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.NetMQ
{
    public class ServerPipeline : Pipeline
    {
        public ServerPipeline(IGetFrameObserver getFrameObserver,
            IDeserializeTransportFrameObserver deserializeTransportFrameObserver,
            IDeserializeMessageObserver deserializeMessageObserver, IHandleRequestObserver handleRequestObserver,
            ISerializeMessageObserver serializeMessageObserver,
            ISerializeTransportFrameObserver serializeTransportFrameObserver,
            IReceiveExceptionObserver exceptionObserver)
        {
            Guard.AgainstNull(getFrameObserver, nameof(getFrameObserver));
            Guard.AgainstNull(deserializeTransportFrameObserver, nameof(deserializeTransportFrameObserver));
            Guard.AgainstNull(deserializeMessageObserver, nameof(deserializeMessageObserver));
            Guard.AgainstNull(handleRequestObserver, nameof(handleRequestObserver));
            Guard.AgainstNull(serializeTransportFrameObserver, nameof(serializeTransportFrameObserver));
            Guard.AgainstNull(serializeMessageObserver, nameof(serializeMessageObserver));
            Guard.AgainstNull(exceptionObserver, nameof(exceptionObserver));

            RegisterStage("Receive")
                .WithEvent<OnGetFrame>()
                .WithEvent<OnDeserializeTransportMessage>()
                .WithEvent<OnDeserializeMessage>()
                .WithEvent<OnHandleRequest>()
                .WithEvent<OnSerializeMessage>()
                .WithEvent<OnSerializeTransportFrame>();

            RegisterObserver(getFrameObserver);
            RegisterObserver(deserializeTransportFrameObserver);
            RegisterObserver(deserializeMessageObserver);
            RegisterObserver(handleRequestObserver);
            RegisterObserver(serializeMessageObserver);
            RegisterObserver(serializeTransportFrameObserver);

            RegisterObserver(exceptionObserver);
        }
    }
}