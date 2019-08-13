using System;
using System.Collections.Generic;
using System.IO;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Server
{
    public class MemoryQueue : IQueue
    {
        private class QueueMessage
        {
            public QueueMessage(Stream stream)
            {
                AcknowledgementToken = Guid.NewGuid();
                Stream = stream;
            }

            public Guid AcknowledgementToken { get; }
            public Stream Stream { get; }
        }

        private readonly object _lock = new object();
        private readonly Queue<QueueMessage> _queue = new Queue<QueueMessage>();
        private readonly Dictionary<Guid, QueueMessage> _journal = new Dictionary<Guid, QueueMessage>();

        public MemoryQueue(Uri uri)
        {
            Guard.AgainstNull(uri, nameof(uri));

            Uri = uri;
        }

        public bool IsEmpty()
        {
            lock (_lock)
            {
                return _queue.Count == 0;
            }
        }

        public void Enqueue(TransportMessage message, Stream stream)
        {
            Guard.AgainstNull(message, nameof(message));
            Guard.AgainstNull(stream, nameof(stream));

            lock (_lock)
            {
                _queue.Enqueue(new QueueMessage(stream));
            }
        }

        public ReceivedMessage GetMessage()
        {
            lock (_lock)
            {
                if (_queue.Count == 0)
                {
                    return null;
                }

                var message = _queue.Dequeue();

                _journal.Add(message.AcknowledgementToken, message);

                return new ReceivedMessage(message.Stream, message.AcknowledgementToken);
            }
        }

        public void Acknowledge(object acknowledgementToken)
        {
            lock (_lock)
            {
                _journal.Remove((Guid) acknowledgementToken);
            }
        }

        public void Release(object acknowledgementToken)
        {
            lock (_lock)
            {
                var key = (Guid)acknowledgementToken;

                if (!_journal.ContainsKey(key))
                {
                    return;
                }

                _queue.Enqueue(_journal[key]);

                _journal.Remove(key);
            }
        }

        public Uri Uri { get; }
    }
}