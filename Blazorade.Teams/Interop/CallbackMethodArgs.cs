using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    internal class CallbackMethodArgs
    {

        public CallbackDefinition SuccessCallback { get; set; }

        public CallbackDefinition FailureCallback { get; set; }

        public Dictionary<string, object> Args { get; set; }

    }
}
