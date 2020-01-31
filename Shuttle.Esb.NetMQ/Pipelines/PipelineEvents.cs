using Shuttle.Core.Pipelines;

namespace Shuttle.Esb.NetMQ
{
    public class OnDeserializeMessage : PipelineEvent
    {
    }

    public class OnDeserializeTransportFrame : PipelineEvent
    {
    }

    public class OnGetFrame : PipelineEvent
    {
    }
    
    public class OnHandleRequest : PipelineEvent
    {
    }

    public class OnSendFrame : PipelineEvent
    {
    }

    public class OnSerializeMessage : PipelineEvent
    {
    }

    public class OnSerializeTransportFrame : PipelineEvent
    {
    }
}