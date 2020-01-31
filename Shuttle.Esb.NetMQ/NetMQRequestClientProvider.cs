using System;
using System.Collections.Generic;
using System.Net;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQRequestClientProvider : INetMQRequestClientProvider
    {
        public static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1);

        private readonly ISerializer _serializer;
        private static readonly object Lock = new object();

        private readonly Dictionary<string, INetMQRequestClient> _clients = new Dictionary<string, INetMQRequestClient>();

        public NetMQRequestClientProvider(ISerializer serializer)
        {
            Guard.AgainstNull(serializer, nameof(serializer));

            _serializer = serializer;
        }

        public INetMQRequestClient Get(IPEndPoint ipEndPoint, TimeSpan timeout)
        {
            Guard.AgainstNull(ipEndPoint, nameof(ipEndPoint));

            lock (Lock)
            {
                var key = $"{ipEndPoint.Address}:{ipEndPoint.Port}";

                if (!_clients.ContainsKey(key))
                {
                    _clients.Add(key, new NetMQRequestClient(_serializer, ipEndPoint, timeout));
                }

                return _clients[key];
            }
        }
    }
}