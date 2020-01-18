using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.NetMQ
{
    public class OnGetFrame : PipelineEvent
    {
    }

    public class OnDeserializeMessage : PipelineEvent
    {
    }

    public class OnDeserializeTransportFrame : PipelineEvent
    {
    }

    public class OnSerializeMessage : PipelineEvent
    {
    }

    public class OnSerializeTransportFrame : PipelineEvent
    {
    }

    public class OnHandleRequest : PipelineEvent
    {
    }
}