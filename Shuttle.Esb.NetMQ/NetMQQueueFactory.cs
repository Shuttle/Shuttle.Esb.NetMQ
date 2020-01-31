using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQQueueFactory : IQueueFactory
    {
        private readonly INetMQRequestClientProvider _requestClientProvider;
        private readonly INetMQConfiguration _configuration;

        public NetMQQueueFactory(INetMQRequestClientProvider requestClientProvider, INetMQConfiguration configuration)
        {
            Guard.AgainstNull(requestClientProvider, nameof(requestClientProvider));
            Guard.AgainstNull(configuration, nameof(configuration));

            _requestClientProvider = requestClientProvider;
            _configuration = configuration;
        }

        public string Scheme => NetMQUriParser.Scheme;

        public IQueue Create(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            var parser = new NetMQUriParser(uri);

            return new NetMQQueue(parser, _requestClientProvider.Get(parser.GetIPEndPoint(), _configuration.RequestTimeout));
        }

        public bool CanCreate(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            return Scheme.Equals(uri.Scheme, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}