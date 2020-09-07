using System;

namespace Debugmancer.NamedPipeClient.IO.Exceptions
{
    public class NamedPipeConnectionException : Exception
    {
        internal NamedPipeConnectionException(string message) : base(message) { }
    }
}
