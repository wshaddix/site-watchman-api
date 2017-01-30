using System;
using System.Runtime.Serialization;

namespace SiteWatchman.Application.Applications.Exceptions
{
    [Serializable]
    public class ApplicationNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ApplicationNotFoundException()
        {
        }

        public ApplicationNotFoundException(string message) : base(message)
        {
        }

        public ApplicationNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ApplicationNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}