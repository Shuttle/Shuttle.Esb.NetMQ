using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetMQ;
using NetMQ.Sockets;
using Shuttle.Core.Contract;
using Shuttle.Core.Streams;

namespace Shuttle.Esb.NetMQ.Server.Processing
{
    public class NetMQRequestServer : INetMQRequestServer, IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ResponseSocket _responseSocket;
        private readonly Task _task;
        private Stream _stream;

        public NetMQRequestServer(INetMQServerConfiguration configuration)
        {
            _responseSocket = new ResponseSocket();
            _responseSocket.Bind($"tcp://localhost:{configuration.Port}");

            // ReSharper disable once ConvertClosureToMethodGroup
            _task = Task.Run(() => Listen());
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _task?.Wait();
            _task?.Dispose();
            _responseSocket?.Dispose();
            _cancellationTokenSource?.Dispose();
        }

        public Stream GetFrameStream()
        {
            return _stream;
        }

        public void SendFrameStream(Stream stream)
        {
            Guard.AgainstNull(stream, nameof(stream));

            _responseSocket.SendFrame(stream.ToBytes());

            _stream.Dispose();
            _stream = null;
        }

        private void Listen()
        {
            var timeout = TimeSpan.FromSeconds(1);
            var cancellationToken = _cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                if (_stream != null)
                {
                    continue;
                }

                if (_responseSocket.TryReceiveFrameBytes(timeout, out var bytes))
                {
                    _stream = new MemoryStream(bytes);
                }
            }
        }
    }
}