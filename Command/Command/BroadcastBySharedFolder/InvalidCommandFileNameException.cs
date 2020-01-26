using System;
using System.Runtime.Serialization;

namespace CommandCore.BroadcastBySharedFolder
{
    [Serializable]
    internal class CommandFileNameFormatException : Exception
    {
        public CommandFileNameFormatException()
        {
        }

        public CommandFileNameFormatException(string cmmdFileName)
            : base($"The command file name '{cmmdFileName}' is invalidate.")
        {
        }

        public CommandFileNameFormatException(string cmmdFileName, Exception innerException) 
            : base($"The command file name '{cmmdFileName}' is invalidate.", innerException)
        {
        }

        protected CommandFileNameFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}