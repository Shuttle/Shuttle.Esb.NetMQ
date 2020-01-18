using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class QueueConfiguration
    {
        public QueueConfiguration(string name, string uri)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));
            Guard.AgainstNullOrEmptyString(uri, nameof(uri));

            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public string Uri { get; }
    }
}