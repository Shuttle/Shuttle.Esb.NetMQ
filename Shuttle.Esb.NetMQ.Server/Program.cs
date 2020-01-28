using System;
using System.IO;
using log4net;
using Ninject;
using Shuttle.Core.Container;
using Shuttle.Core.Log4Net;
using Shuttle.Core.Logging;
using Shuttle.Core.Ninject;
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
        private IServiceBus _bus;
        private IKernel _kernel;

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

            var container = new NinjectComponentContainer(_kernel);
            var configuration = NetMQSection.GetConfiguration();

            container.RegisterInstance(configuration);
            container.Register(typeof(ISerializer), configuration.GetSerializerType());
            container.Register<INetMQRequestClientProvider, NetMQRequestClientProvider>();
            container.Register<INetMQRequestServer, NetMQRequestServer>();

            ServiceBus.Register(container);

            _bus = ServiceBus.Create(container).Start();

            _log.Information("[started]");
        }

        public void Stop()
        {
            _bus?.Dispose();
            _kernel?.Dispose();
        }
    }
}
