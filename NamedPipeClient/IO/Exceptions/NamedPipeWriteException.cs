using System;

namespace Debugmancer.NamedPipeClient.IO.Exceptions
{
    public class NamedPipeWriteException : Exception
    {
        public int ErrorCode { get; }
        internal NamedPipeWriteException(int err) : base("An exception occured while reading from the pipe. Error Code: " + err)
        {
            ErrorCode = err;
        }
    }
}
