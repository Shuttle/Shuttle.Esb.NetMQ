using System;
using System.IO;
using System.Net;
using NetMQ;
using NetMQ.Sockets;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;
using Shuttle.Core.Streams;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQRequestClient : INetMQRequestClient, IDisposable
    {
        private readonly Type _transportFrameType = typeof(TransportFrame);
        private readonly Type _responseType = typeof(Response);

        private readonly TimeSpan _timeout;
        private readonly RequestSocket _requestSocket;
        private readonly ISerializer _serializer;
        
        public NetMQRequestClient(ISerializer serializer, IPEndPoint ipEndPoint, TimeSpan timeout)
        {
            Guard.AgainstNull(serializer, nameof(serializer));
            Guard.AgainstNull(ipEndPoint, nameof(ipEndPoint));

            _serializer = serializer;
            _timeout = timeout;

            _requestSocket = new RequestSocket();
            _requestSocket.Connect($"tcp://{ipEndPoint.Address}:{ipEndPoint.Port}");
        }

        public TResponse GetResponse<TResponse>(object request, string queueName) 
        {
            Guard.AgainstNull(request, nameof(request));
            Guard.AgainstNullOrEmptyString(queueName, nameof(queueName));

            TransportFrame frame;
            var responseTypeName = typeof(TResponse).FullName;

            using (var stream = _serializer.Serialize(request))
            {
                frame = new TransportFrame
                {
                    QueueName = queueName,
                    MessageType = request.GetType().FullName,
                    Message = stream.ToBytes()
                };
            }

            using (var stream = _serializer.Serialize(frame))
            {
                if (!_requestSocket.TrySendFrame(_timeout, stream.ToBytes()) ||
                    !_requestSocket.TryReceiveFrameBytes(_timeout, out var bytes))
                {
                    throw NetMQException.For(responseTypeName, frame.MessageType, Resources.CommunicationException);
                }

                using (var ms = new MemoryStream(bytes))
                {
                    frame = (TransportFrame)_serializer.Deserialize(_transportFrameType, ms);
                }

                using (var ms = new MemoryStream(frame.Message))
                {
                    if (frame.MessageType.Equals(responseTypeName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return (TResponse) _serializer.Deserialize(typeof(TResponse), ms);
                    }

                    throw new NetMQException(string.Format(Resources.ServerException,
                        ((Response) _serializer.Deserialize(_responseType, ms)).Exception));
                }
            }
        }

        public void Dispose()
        {
            _requestSocket?.Dispose();
        }
    }
}