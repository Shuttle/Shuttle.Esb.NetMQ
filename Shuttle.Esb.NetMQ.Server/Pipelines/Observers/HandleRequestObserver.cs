using System;
using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Streams;

namespace Shuttle.Esb.NetMQ.Server.Pipelines.Observers
{
    public interface IHandleRequestObserver : IPipelineObserver<OnHandleRequest>
    {
    }

    public class HandleRequestObserver : IHandleRequestObserver
    {
        private readonly INetMQServerConfiguration _configuration;
        private readonly IQueueManager _queueManager;

        public HandleRequestObserver(IQueueManager queueManager, INetMQServerConfiguration configuration)
        {
            Guard.AgainstNull(queueManager, nameof(queueManager));
            Guard.AgainstNull(configuration, nameof(configuration));

            _queueManager = queueManager;
            _configuration = configuration;
        }

        public void Execute(OnHandleRequest pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var transportFrame = state.Get<TransportFrame>(StateKeys.TransportFrame);
            var message = state.Get<object>(StateKeys.Message);
            Response response;

            var queue = _queueManager.GetQueue(_configuration.GetQueue(transportFrame.QueueName).Uri);

            switch (message)
            {
                case AcknowledgeRequest request:
                {
                    queue.Acknowledge(request.AcknowledgementToken);

                    response = new Response();

                    break;
                }
                case EnqueueRequest request:
                {
                    queue.Enqueue(request.TransportMessage, new MemoryStream(request.StreamBytes));

                    response = new Response();

                    break;
                }
                case GetMessageRequest _:
                {
                    var receivedMessage = queue.GetMessage();

                    response = new GetMessageResponse
                    {
                        AcknowledgementToken = receivedMessage?.AcknowledgementToken,
                        StreamBytes = receivedMessage?.Stream.ToBytes()
                    };

                    receivedMessage?.Stream.Dispose();

                    break;
                }
                case IsEmptyRequest _:
                {
                    response = new IsEmptyResponse {Result = queue.IsEmpty()};

                    break;
                }
                case ReleaseRequest request:
                {
                    queue.Release(request.AcknowledgementToken);

                    response = new Response();

                    break;
                }
                default:
                {
                    throw new ApplicationException(string.Format(NetMQ.Resources.UnknownRequestType,
                        message.GetType().FullName));
                }
            }

            var type = response.GetType();

            state.Replace(StateKeys.Response, response);
            state.Replace(StateKeys.TransportFrame, new TransportFrame
            {
                QueueName = transportFrame.QueueName,
                AssemblyQualifiedName = type.AssemblyQualifiedName,
                MessageType = type.FullName
            });
        }
    }
}