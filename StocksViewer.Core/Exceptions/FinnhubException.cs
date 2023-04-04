using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksViewer.Core.Exceptions
{
    public class FinnhubException : InvalidOperationException
    {
        public FinnhubException() : base() { }
        public FinnhubException(string? message) : base(message) { }
        public FinnhubException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
