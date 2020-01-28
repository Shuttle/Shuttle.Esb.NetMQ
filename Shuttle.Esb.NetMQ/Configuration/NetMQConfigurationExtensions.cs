using System;
using System.Configuration;
using Shuttle.Core.Contract;
using Shuttle.Core.Serialization;

namespace Shuttle.Esb.NetMQ
{
    public static class NetMQConfigurationExtensions
    {
        public static Type GetSerializerType(this INetMQConfiguration configuration)
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
                throw new ConfigurationErrorsException(string.Format(Resources.UnknownSerializerType, configuration.SerializerType));
            }
        }
    }
}