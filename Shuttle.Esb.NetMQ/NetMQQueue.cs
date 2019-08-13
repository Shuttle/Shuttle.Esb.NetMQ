using System;
using System.IO;
using Shuttle.Core.Contract;
using Shuttle.Core.Streams;
using Shuttle.Esb.NetMQ.Frames;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQQueue : IQueue
    {
        private readonly NetMQUriParser _uriParser;
        private readonly INetMQRequestClient _requestClient;

        public NetMQQueue(NetMQUriParser uriUriParser, INetMQRequestClient requestClient)
        {
            Guard.AgainstNull(uriUriParser, nameof(uriUriParser));
            Guard.AgainstNull(requestClient, nameof(requestClient));

            _requestClient = requestClient;
            _uriParser = uriUriParser;

            Uri = _uriParser.Uri;
        }

        public bool IsEmpty()
        {
            return _requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), _uriParser.QueueName).Result;
        }

        public void Enqueue(TransportMessage message, Stream stream)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(stream, nameof(stream));

            NetMQException.GuardAgainstException<Response, EnqueueRequest>(_requestClient.GetResponse<Response>(new EnqueueRequest
            {
                TransportMessage = message,
                StreamBytes = stream.ToBytes()
            }, _uriParser.QueueName));
        }

        public ReceivedMessage GetMessage()
        {
            var response = NetMQException.GuardAgainstException<GetMessageResponse, GetMessageRequest>(
                _requestClient.GetResponse<GetMessageResponse>(new GetMessageRequest(), _uriParser.QueueName));

            return new ReceivedMessage(new MemoryStream(response.StreamBytes), response.AcknowledgementToken);
        }

        public void Acknowledge(object acknowledgementToken)
        {
            Guard.AgainstNull(acknowledgementToken, nameof(acknowledgementToken));

            NetMQException.GuardAgainstException<Response, AcknowledgeRequest>(_requestClient.GetResponse<Response>(new AcknowledgeRequest
            {
                AcknowledgementToken = acknowledgementToken
            }, _uriParser.QueueName));
        }

        public void Release(object acknowledgementToken)
        {
            Guard.AgainstNull(acknowledgementToken, nameof(acknowledgementToken));

            NetMQException.GuardAgainstException<Response, ReleaseRequest>(_requestClient.GetResponse<Response>(new ReleaseRequest
            {
                AcknowledgementToken = acknowledgementToken
            }, _uriParser.QueueName));
        }

        public Uri Uri { get; }
    }
}