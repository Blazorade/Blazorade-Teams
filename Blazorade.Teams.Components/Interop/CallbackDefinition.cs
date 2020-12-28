using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    internal class CallbackDefinition
    {

        public static CallbackDefinition Create(Delegate method)
        {
            return new CallbackDefinition
            {
                Target = DotNetObjectReference.Create(method.Target),
                MethodName = method.Method.Name
            };
        }

        public DotNetObjectReference<object> Target { get; set; }

        public string MethodName { get; set; }

    }
}
