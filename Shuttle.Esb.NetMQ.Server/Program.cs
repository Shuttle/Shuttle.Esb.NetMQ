using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Ninject;
using Shuttle.Core.Container;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.Ninject;
using Shuttle.Core.Reflection;
using Shuttle.Core.Serialization;
using Shuttle.Core.ServiceHost;
using ILog = Shuttle.Core.Logging.ILog;

namespace Shuttle.Esb.NetMQ.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost.Run<Host>();
        }
    }

    internal class Host : IServiceHost
    {
        private readonly ILog _log;
        private IKernel _kernel;
        private INetMQRequestServer _server;

        public Host()
        {
            var logConfigurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.xml");

            if (File.Exists(logConfigurationFilePath))
            {
                Log.Assign(
                    new Log4NetLog(LogManager.GetLogger(typeof(Program)), new FileInfo(logConfigurationFilePath)));
            }

            _log = Log.For(this);
        }

        public void Start()
        {
            _log.Information("[starting]");

            _kernel = new StandardKernel();

            var configuration = NetMQSection.GetConfiguration();
            var container = ContainerFactory.Create(_kernel, configuration);

            var queueFactoryType = typeof(IQueueFactory);
            var queueFactoryImplementationTypes = new HashSet<Type>();

            void AddQueueFactoryImplementationType(Type type)
            {
                queueFactoryImplementationTypes.Add(type);
            }

            if (configuration.ScanForQueueFactories)
            {
                foreach (var type in new ReflectionService().GetTypesAssignableTo<IQueueFactory>())
                {
                    AddQueueFactoryImplementationType(type);
                }
            }

            foreach (var type in configuration.QueueFactoryTypes)
            {
                AddQueueFactoryImplementationType(type);
            }

            container.RegisterCollection(queueFactoryType, queueFactoryImplementationTypes, Lifestyle.Singleton);

            var queueManager = container.Resolve<IQueueManager>();

            foreach (var queueFactory in container.ResolveAll<IQueueFactory>())
            {
                queueManager.RegisterQueueFactory(queueFactory);
            }

            _server = container.Resolve<INetMQRequestServer>();

            _log.Information("[started]");
        }

        public void Stop()
        {
            _server?.AttemptDispose();
            _kernel?.Dispose();
        }
    }
}
