using System;
using System.Net;
using Ninject;
using NUnit.Framework;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Reflection;
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

                requestServer = container.Resolve<INetMQRequestServer>();
                requestClient = container.Resolve<INetMQRequestClientProvider>()
                    .Get(ipEndpoint, TimeSpan.FromSeconds(30));

                var isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.True);

                var response = requestClient.GetResponse<Response>(
                    new EnqueueRequest
                    {
                        StreamBytes = new byte[] {1, 2, 3, 4},
                        TransportMessage = new TransportMessage {MessageType = "test"}
                    }, queueName);

                Assert.That(response.IsOk, Is.True);

                isEmptyResponse = requestClient.GetResponse<IsEmptyResponse>(new IsEmptyRequest(), queueName);

                Assert.That(isEmptyResponse.Result, Is.False);

                var getMessageResponse = requestClient.GetResponse<GetMessageResponse>(new GetMessageRequest(), queueName);
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