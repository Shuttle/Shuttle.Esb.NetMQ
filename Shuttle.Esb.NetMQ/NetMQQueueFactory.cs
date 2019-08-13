using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQQueueFactory : IQueueFactory
    {
        private readonly INetMQRequestClientProvider _requestClientProvider;

        public NetMQQueueFactory(INetMQRequestClientProvider requestClientProvider)
        {
            Guard.AgainstNull(requestClientProvider, nameof(requestClientProvider));

            _requestClientProvider = requestClientProvider;
        }

        public string Scheme => NetMQUriParser.Scheme;

        public IQueue Create(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            var parser = new NetMQUriParser(uri);

            return new NetMQQueue(parser, _requestClientProvider.Get(parser.GetIPEndPoint()));
        }

        public bool CanCreate(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}