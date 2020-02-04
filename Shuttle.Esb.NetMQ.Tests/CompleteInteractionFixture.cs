using System;
using System.IO;
using System.Linq;
using System.Net;
using Ninject;
using NUnit.Framework;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Reflection;
using Shuttle.Core.Serialization;
using Shuttle.Core.Streams;
using Shuttle.Esb.NetMQ.Server;

namespace Shuttle.Esb.NetMQ.Tests
{
    [TestFixture]
    public class CompleteInteractionFixture
    {
        [Test]
        public void Should_be_able_to_perform_request_response()
        {
            const int port = 3030;
            const string queueName = "memory-queue";

            var configuration = new NetMQConfiguration
            {
                Port = port
            };

            configuration.AddQueueFactoryType(typeof(MemoryQueueFactory));
            configuration.AddQueue(new QueueConfiguration(queueName, "memory://memory-queue"));

            var container = ContainerFactory.Create(new StandardKernel(), configuration);

            ((DefaultPipelineFactory) container.Resolve<IPipelineFactory>()).Assign(container);

            var server = container.Resolve<INetMQServer>();

            INetMQRequestServer requestServer = null;
            INetMQRequestClient requestClient = null;

            var ipEndpoint = new IPEndPoint(IPAddress.Loopback, port);

            try
            {
                server.Start();

                var serializer = container.Resolve<ISerializer>();

                requestServer = container.Resolve<INetMQRequestServer>();
                requestClient = container.Resolve<INetMQRequestClientProvider>()
                    .Get(ipEndpoint, TimeSpan.FromSeconds(30));

                var isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.True);

                var transportMessage = new TransportMessage
                {
                    MessageType = "test",
                    Message = new byte[] { 1, 2, 3, 4 }
                };

                var response = requestClient.GetResponse<Response>(
                    new EnqueueRequest
                    {
                        StreamBytes = serializer.Serialize(transportMessage).ToBytes(),
                        TransportMessage = transportMessage
                    }, queueName);

                Assert.That(response.IsOk, Is.True);

                isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.False);

                var getMessageResponse = requestClient.GetResponse<GetMessageResponse>(new GetMessageRequest(), queueName);

                Assert.That(response.IsOk, Is.True);

                transportMessage = (TransportMessage)serializer.Deserialize(typeof(TransportMessage), new MemoryStream(getMessageResponse.StreamBytes));

                Assert.That(transportMessage.MessageType, Is.EqualTo("test"));

                var bytes = transportMessage.Message.ToArray();

                Assert.That(bytes[0], Is.EqualTo(1));
                Assert.That(bytes[1], Is.EqualTo(2));
                Assert.That(bytes[2], Is.EqualTo(3));
                Assert.That(bytes[3], Is.EqualTo(4));

                isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.True);

                response = requestClient.GetResponse<Response>(
                    new ReleaseRequest {AcknowledgementToken = getMessageResponse.AcknowledgementToken}, queueName);

                Assert.That(response.IsOk, Is.True);

                isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.False);

                getMessageResponse = requestClient.GetResponse<GetMessageResponse>(new GetMessageRequest(), queueName);

                Assert.That(getMessageResponse.IsOk, Is.True);

                isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.True);

                response = requestClient.GetResponse<Response>(
                    new AcknowledgeRequest{ AcknowledgementToken = getMessageResponse.AcknowledgementToken }, queueName);

                Assert.That(response.IsOk, Is.True);

                response = requestClient.GetResponse<Response>(
                    new ReleaseRequest { AcknowledgementToken = getMessageResponse.AcknowledgementToken }, queueName);

                Assert.That(response.IsOk, Is.True);

                isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.True);

                getMessageResponse = requestClient.GetResponse<GetMessageResponse>(new GetMessageRequest(), queueName);

                Assert.That(getMessageResponse.IsOk, Is.True);
                Assert.That(getMessageResponse.AcknowledgementToken, Is.Null);
                Assert.That(getMessageResponse.StreamBytes, Is.Null);
            }
            finally
            {
                requestClient?.AttemptDispose();
                requestServer?.AttemptDispose();
                server?.AttemptDispose();
            }
        }
    }
}