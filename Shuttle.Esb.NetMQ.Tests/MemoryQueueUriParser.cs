using System;
using NUnit.Framework;
using Shuttle.Core.Contract;
using Shuttle.Esb.NetMQ.Server;

namespace Shuttle.Esb.NetMQ.Tests
{
    [TestFixture]
    public class MemoryQueueUriParserFixture
    {
        [Test]
        public void Should_be_able_to_parse_valid_uri()
        {
            MemoryQueueUriParser parser = null;

            Assert.That(()=> parser = new MemoryQueueUriParser(new Uri("memory://queue-name")), Throws.Nothing);
            Assert.That(parser, Is.Not.Null);
            Assert.That(parser.QueueName, Is.EqualTo("queue-name"));
        }

        public void Should_not_be_able_to_parse_invalid_scheme_uri()
        {
            Assert.That(() => new NetMQUriParser(new Uri("unsupported://host/queue")), Throws.TypeOf<InvalidSchemeException>());
        }
    }
}