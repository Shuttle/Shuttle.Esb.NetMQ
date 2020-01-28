using System;
using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ
{
    public class NetMQException : Exception
    {
        public NetMQException()
        {
        }

        public NetMQException(string message) : base(message)
        {
        }

        public static NetMQException For<TResponse, TRequest>(string exception)
        {
            return For(typeof(TResponse).FullName, typeof(TRequest).FullName, exception);
        }

        public static NetMQException For(string responseTypeName, string requestTypeName, string exception)
        {
            Guard.AgainstNullOrEmptyString(responseTypeName, nameof(responseTypeName));
            Guard.AgainstNullOrEmptyString(requestTypeName, nameof(requestTypeName));

            return new NetMQException(string.Format(Resources.GetResponseException, responseTypeName, requestTypeName,
                !string.IsNullOrWhiteSpace(exception) ? exception : Resources.UnknownException));
        }

        public static TResponse GuardAgainstException<TResponse, TRequest>(TResponse response)
            where TResponse : Response
        {
            Guard.AgainstNull(response, nameof(response));

            if (!response.IsOk())
            {
                throw For<TResponse, TRequest>(response.Exception);
            }

            return response;
        }
    }
}