using Shuttle.Core.Contract;

namespace Shuttle.Esb.NetMQ.Frames
{
    public static class ResponseExtensions
    {
        public static bool IsOk(this Response response)
        {
            Guard.AgainstNull(response, nameof(response));

            return string.IsNullOrEmpty(response.Exception);
        }
    }
}