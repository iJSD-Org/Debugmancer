using System;

namespace Debugmancer.NamedPipeClient.IO.Exceptions
{
    public class NamedPipeOpenException : Exception
    {
        public int ErrorCode { get; }
        internal NamedPipeOpenException(int err) : base("An exception has occured while trying to open the pipe. Error Code: " + err)
        {
            ErrorCode = err;
        }        
    }
}
