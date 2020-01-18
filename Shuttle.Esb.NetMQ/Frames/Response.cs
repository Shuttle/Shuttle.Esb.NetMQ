namespace Shuttle.Esb.NetMQ.Frames
{
    public class Response
    {
        public static readonly string AssemblyQualifiedName = typeof(Response).AssemblyQualifiedName;
        public static readonly string FullName = typeof(Response).FullName;

        public string Exception { get; set; }
    }
}