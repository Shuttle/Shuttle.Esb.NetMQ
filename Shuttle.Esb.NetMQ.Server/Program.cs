using System;
using Shuttle.Core.ServiceHost;

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
        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }
    }
}
