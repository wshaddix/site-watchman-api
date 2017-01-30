using System;
using System.Runtime.Serialization;

namespace SiteWatchman.Application.Shared.Messages
{
    [Serializable]
    public class MessageContextMissingException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MessageContextMissingException()
        {
        }

        public MessageContextMissingException(string message) : base(message)
        {
        }

        public MessageContextMissingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MessageContextMissingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}