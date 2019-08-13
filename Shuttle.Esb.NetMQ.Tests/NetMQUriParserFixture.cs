using System;
using System.Net;
using NUnit.Framework;

namespace Shuttle.Esb.NetMQ.Tests
{
    [TestFixture]
    public class NetMQUriParserFixture
    {
        [Test]
        [TestCase("netmq://127.0.0.1:7070/test-queue", "127.0.0.1")]
        [TestCase("netmq://localhost:7070/test-queue", "127.0.0.1")]
        [TestCase("netmq://10.10.0.1:7070/test-queue", "10.10.0.1")]
        public void Should_be_able_to_parse_valid_uri(string uri, string ipAddress)
        {
            NetMQUriParser parser = null;

            Assert.That(() => parser = new NetMQUriParser(new Uri(uri)), Throws.Nothing);
            Assert.That(parser, Is.Not.Null);
            Assert.That(parser.ServerIPAddress, Is.EqualTo(IPAddress.Parse(ipAddress)));
            Assert.That(parser.Port, Is.EqualTo(7070));
            Assert.That(parser.QueueName, Is.EqualTo("test-queue"));
        }

        [Test]
        [TestCase("netmq://127.0.0.1/test-queue")]
        [TestCase("netmq://127.0.0.1:7070")]
        [TestCase("netmq://127.0.0.1:7070/")]
        public void Should_not_be_able_to_parse_invalid_uri(string uri)
        {
            Assert.That(()=> new NetMQUriParser(new Uri(uri)), Throws.TypeOf<UriFormatException>());
        }

        public void Should_not_be_able_to_parse_invalid_scheme_uri()
        {
            Assert.That(()=> new NetMQUriParser(new Uri("unsupported://host/queue")), Throws.TypeOf<InvalidSchemeException>());
        }
    }
}