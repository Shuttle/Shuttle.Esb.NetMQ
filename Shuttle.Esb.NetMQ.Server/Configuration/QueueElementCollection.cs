using System.Configuration;

namespace Shuttle.Esb.NetMQ.Server
{
    public class QueueElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new QueueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((QueueElement) element).Name;
        }
    }
}