using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ
{
    public class QueueConfiguration
    {
        public QueueConfiguration(string name, string uri)
        {
            Guard.AgainstNullOrEmptyString(name, nameof(name));
            Guard.AgainstNullOrEmptyString(uri, nameof(uri));

            Name = name;
            Uri = new Uri(uri).ToString();
        }

        public string Name { get; }
        public string Uri { get; }
    }
}