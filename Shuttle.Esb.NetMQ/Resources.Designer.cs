﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shuttle.Esb.NetMQ {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Shuttle.Esb.NetMQ.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (communication exception).
        /// </summary>
        public static string CommunicationException {
            get {
                return ResourceManager.GetString("CommunicationException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not deserialize the transport frame.  Exception reported: {0}.
        /// </summary>
        public static string DeserializerTransportMessageException {
            get {
                return ResourceManager.GetString("DeserializerTransportMessageException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not obtain response type &apos;{0}&apos; for request type &apos;{1}&apos;: {2}.
        /// </summary>
        public static string GetResponseException {
            get {
                return ResourceManager.GetString("GetResponseException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server exception: {0}.
        /// </summary>
        public static string ServerException {
            get {
                return ResourceManager.GetString("ServerException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (unknown exception).
        /// </summary>
        public static string UnknownException {
            get {
                return ResourceManager.GetString("UnknownException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find a queue with name &apos;{0}&apos;..
        /// </summary>
        public static string UnknownQueueNameException {
            get {
                return ResourceManager.GetString("UnknownQueueNameException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown request type &apos;{0}&apos;..
        /// </summary>
        public static string UnknownRequestType {
            get {
                return ResourceManager.GetString("UnknownRequestType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown serializer type &apos;{0}&apos;..
        /// </summary>
        public static string UnknownSerializerType {
            get {
                return ResourceManager.GetString("UnknownSerializerType", resourceCulture);
            }
        }
    }
}
