using System;
using System.Configuration;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ.Server
{
    public static class NetMQServerConfigurationExtensions
    {
        public static Type GetSerializerType(this INetMQServerConfiguration configuration)
        {
            Guard.AgainstNull(configuration, nameof(configuration));

            if (string.IsNullOrWhiteSpace(configuration.SerializerType))
            {
                return typeof(DefaultSerializer);
            }

            try
            {
                return Type.GetType(configuration.SerializerType);
            }
            catch
            {
                throw new ConfigurationErrorsException(string.Format(NetMQ.Resources.UnknownSerializerType, configuration.SerializerType));
            }
        }
    }
}