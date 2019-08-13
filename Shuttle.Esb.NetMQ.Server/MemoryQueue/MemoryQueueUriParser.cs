using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class MemoryQueueUriParser
    {
        internal const string Scheme = "memory";

        public MemoryQueueUriParser(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            if (!uri.Scheme.Equals(Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidSchemeException(Scheme, uri.ToString());
            }

            QueueName = uri.Host;
        }

        public string QueueName { get; }
    }
}