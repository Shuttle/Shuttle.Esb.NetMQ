using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Shuttle.Core.Configuration;
using Shuttle.Esb.NetMQ.Server;

namespace Shuttle.Esb.NetMQ.Tests
{
    [TestFixture]
    public class NetMQServerSectionFixture
    {
        protected NetMQServerSection GetNetMQServerSection(string file)
        {
            return ConfigurationSectionProvider.OpenFile<NetMQServerSection>("shuttle", "netmq",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $@".\.files\{file}"));
        }

        [Test]
        [TestCase("NetMQ.config")]
        [TestCase("NetMQ-Grouped.config")]
        public void Should_be_able_to_load_a_valid_configuration(string file)
        {
            var section = GetNetMQServerSection(file);

            Assert.That(section, Is.Not.Null);
            Assert.That(section.Queues, Is.Not.Null);
            Assert.That(section.Queues.Count, Is.EqualTo(3));

            Assert.That(section.Port, Is.EqualTo(6565));

            var queueElements = section.Queues.Cast<QueueElement>().ToList();

            Assert.That(queueElements[0].Name, Is.EqualTo("queue-one"));
            Assert.That(queueElements[0].Uri.EndsWith("queue-one"), Is.True);

            Assert.That(queueElements[1].Name, Is.EqualTo("queue-two"));
            Assert.That(queueElements[1].Uri.EndsWith("queue-two"), Is.True);

            Assert.That(queueElements[2].Name, Is.EqualTo("queue-three"));
            Assert.That(queueElements[2].Uri.EndsWith("queue-three"), Is.True);
        }
    }
}