using System;
using System.Net;
using System.Text.RegularExpressions;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQUriParser
    {
        internal const string Scheme = "netmq";

        private readonly Regex _regexIpAddress =
            new Regex(
                @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$");

        public NetMQUriParser(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            if (!uri.Scheme.Equals(Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new InvalidSchemeException(Scheme, uri.ToString());
            }

            var builder = new UriBuilder(uri);
            var host = uri.Host;

            if (host.Equals(".") || 
                host.Equals("localhost",StringComparison.InvariantCultureIgnoreCase) || 
                host.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase))
            {
                builder.Host = IPAddress.Loopback.ToString();
            }

            Uri = builder.Uri;

            if (uri.LocalPath == "/" || !_regexIpAddress.IsMatch(Uri.Host) || uri.Port < 0)
            {
                throw new UriFormatException(string.Format(Esb.Resources.UriFormatException,
                    "netmq://{{server-ip}}:{{port}}/{{queue-reference-name}}", uri));
            }

            ServerIPAddress = IPAddress.Parse(Uri.Host);
            Port = Uri.Port;
            QueueName = Uri.Segments[1];
        }

        public string QueueName { get; }
        public int Port { get; }
        public IPAddress ServerIPAddress { get; }
        public Uri Uri { get; }

        public IPEndPoint GetIPEndPoint()
        {
            return new IPEndPoint(ServerIPAddress, Port);
        }
    }
}