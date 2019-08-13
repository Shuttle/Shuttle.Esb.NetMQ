using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class MemoryQueueFactory : IQueueFactory
    {
        public IQueue Create(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            return new MemoryQueue(uri);
        }

        public bool CanCreate(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
        }

        public string Scheme => MemoryQueueUriParser.Scheme;
    }
}