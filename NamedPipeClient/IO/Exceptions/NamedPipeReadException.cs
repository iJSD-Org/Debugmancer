using System;

namespace Debugmancer.NamedPipeClient.IO.Exceptions
{
    public class NamedPipeReadException : Exception
    {
        public int ErrorCode { get; }
        internal NamedPipeReadException(int err) : base("An exception occured while reading from the pipe. Error Code: " + err)
        {
            ErrorCode = err;
        }
    }
}
