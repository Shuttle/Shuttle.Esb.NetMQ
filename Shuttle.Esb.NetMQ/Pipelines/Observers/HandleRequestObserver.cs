using System;
using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Streams;
using Shuttle.Esb.NetMQ.Frames;
using Shuttle.Esb.NetMQ.Server;

namespace Shuttle.Esb.NetMQ
{
    public interface IHandleRequestObserver : IPipelineObserver<OnHandleRequest>
    {
    }

    public class HandleRequestObserver : IHandleRequestObserver
    {
        private readonly INetMQConfiguration _configuration;
        private readonly IQueueManager _queueManager;

        public HandleRequestObserver(IQueueManager queueManager, INetMQConfiguration configuration)
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
            var transportFrame = state.Get<TransportFrame>();
            var message = state.Get<object>(StateKeys.Message);
            Response response = null;

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
                    throw new ApplicationException(string.Format(Resources.UnknownRequestType,
                        message.GetType().FullName));
                }
            }

            state.Replace(StateKeys.Response, response);
        }
    }
}