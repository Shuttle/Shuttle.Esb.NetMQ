using System;
using System.Collections.Generic;
using Ninject;
using Shuttle.Core.Container;
using Shuttle.Core.Contract;
using Shuttle.Core.Ninject;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Reflection;
using Shuttle.Core.Serialization;
using Shuttle.Esb.NetMQ.Server.Processing;

namespace Shuttle.Esb.NetMQ.Server
{
    public class ContainerFactory
    {
        public static NinjectComponentContainer Create(IKernel kernel, INetMQServerConfiguration configuration)
        {
            Guard.AgainstNull(kernel, nameof(kernel));
            Guard.AgainstNull(configuration, nameof(configuration));

            var container = new NinjectComponentContainer(kernel);

            container.RegisterInstance(configuration);
            container.Register(typeof(ISerializer), configuration.GetSerializerType());
            container.Register<IPipelineFactory, DefaultPipelineFactory>();
            container.Register<INetMQRequestClientProvider, NetMQRequestClientProvider>();
            container.Register<INetMQRequestServer, NetMQRequestServer>();
            container.Register<IUriResolver, DefaultUriResolver>();
            container.Register<IQueueManager, QueueManager>();
            container.Register<INetMQServer, NetMQServer>();

            var reflectionService = new ReflectionService();

            foreach (var type in reflectionService.GetTypesAssignableTo<IPipeline>())
            {
                if (type.IsInterface || type.IsAbstract || container.IsRegistered(type))
                {
                    continue;
                }

                container.Register(type, type, Lifestyle.Transient);
            }

            foreach (var type in reflectionService.GetTypesAssignableTo<IPipelineObserver>())
            {
                if (type.IsInterface || type.IsAbstract)
                {
                    continue;
                }

                var interfaceType = type.InterfaceMatching($"I{type.Name}");

                if (interfaceType != null)
                {
                    if (container.IsRegistered(type))
                    {
                        continue;
                    }

                    container.Register(interfaceType, type, Lifestyle.Singleton);
                }
            }

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
            var queueFactories = container.ResolveAll<IQueueFactory>();

            foreach (var queueFactory in queueFactories)
            {
                queueManager.RegisterQueueFactory(queueFactory);
            }

            return container;
        }
    }
}