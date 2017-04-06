using System;
using System.Runtime.InteropServices;

namespace Rationally.Visio
{
    public class RationallyException : Exception
    {
        public RationallyException(COMException comException)
        {
            throw new NotImplementedException();
        }

        public RationallyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}