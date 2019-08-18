using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Shuttle.Core.Streams;
using Shuttle.Esb.NetMQ.Server;

namespace Shuttle.Esb.NetMQ.Tests
{
    [TestFixture]
    public class MemoryQueueFixture
    {
        [Test]
        public void Should_be_able_to_perform_all_operations()
        {
            MemoryQueue queue = null;
            ReceivedMessage receivedMessage = null;

            var transportMessage = new TransportMessage {CorrelationId = Guid.NewGuid().ToString()};
            var stream = new MemoryStream(Encoding.ASCII.GetBytes("contents"));

            Assert.That(() => queue = new MemoryQueue(new Uri("memory://test-queue")), Throws.Nothing);
            Assert.That(queue, Is.Not.Null);
            Assert.That(queue.IsEmpty, Is.True);

            Assert.That(() => queue.Enqueue(transportMessage, stream), Throws.Nothing);
            Assert.That(queue.IsEmpty, Is.False);

            Assert.That(() => receivedMessage = queue.GetMessage(), Throws.Nothing);
            Assert.That(receivedMessage, Is.Not.Null);
            Assert.That(Encoding.ASCII.GetString(receivedMessage.Stream.ToBytes()), Is.EqualTo("contents"));
            Assert.That(queue.IsEmpty, Is.True);

            Assert.That(() => queue.Release(receivedMessage.AcknowledgementToken), Throws.Nothing);
            Assert.That(queue.IsEmpty, Is.False);

            Assert.That(() => receivedMessage = queue.GetMessage(), Throws.Nothing);
            Assert.That(receivedMessage, Is.Not.Null);
            Assert.That(Encoding.ASCII.GetString(receivedMessage.Stream.ToBytes()), Is.EqualTo("contents"));
            Assert.That(queue.IsEmpty, Is.True);

            Assert.That(() => queue.Acknowledge(receivedMessage.AcknowledgementToken), Throws.Nothing);
            Assert.That(queue.IsEmpty, Is.True);
        }
    }
}