using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class QueueConfiguration
    {
        public QueueConfiguration(string name, Uri uri)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));
            Guard.AgainstNull(uri, nameof(uri));

            Name = name;
            Uri = uri;
        }

        public string Name { get; }
        public Uri Uri { get; }
    }
}