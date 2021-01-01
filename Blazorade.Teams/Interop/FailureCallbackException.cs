using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    public class FailureCallbackException : Exception
    {
        public FailureCallbackException(object result) : this(result, null) { }

        public FailureCallbackException(object result, string message) : base(message)
        {
            this.Result = result;
        }

        public object Result { get; private set; }

    }
}
