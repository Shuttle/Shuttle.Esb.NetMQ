using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;
using Shuttle.Esb.NetMQ.Frames;

namespace Shuttle.Esb.NetMQ.Server
{
    public class NetMQRequestServer : INetMQRequestServer, IDisposable
    {
        private readonly INetMQConfiguration _configuration;
        private readonly IQueueManager _queueManager;
        private readonly ISerializer _serializer;
        private readonly ResponseSocket _responseSocket;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly Task _task;
        private readonly Type _transportFrameType = typeof(TransportFrame);

        public NetMQRequestServer(INetMQConfiguration configuration, IQueueManager queueManager,
            ISerializer serializer)
        {
            Guard.AgainstNull(configuration, nameof(configuration));
            Guard.AgainstNull(queueManager, nameof(queueManager));
            Guard.AgainstNull(serializer, nameof(serializer));

            _configuration = configuration;
            _queueManager = queueManager;
            _serializer = serializer;

            _responseSocket = new ResponseSocket();
            _responseSocket.Bind($"tcp://localhost:{configuration.Port}");

            _task = Task.Run(() => Listen());
        }

        private void Listen()
        {
            var timeout = TimeSpan.FromSeconds(1);
            var cancellationToken = _cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_responseSocket.TryReceiveFrameBytes(timeout, out var bytes))
                {
                    TransportFrame frame = null;

                    try
                    {
                        object message;

                        using (var ms = new MemoryStream(bytes))
                        {
                            frame = (TransportFrame)_serializer.Deserialize(_transportFrameType, ms);
                        }

                        using (var ms = new MemoryStream(frame.Message))
                        {
                            message = _serializer.Deserialize(Type.GetType(frame.MessageType), ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (frame)
                    }
                }
            }
        }
        
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();

            _responseSocket?.Dispose();
            _cancellationTokenSource?.Dispose();
        }
    }
}