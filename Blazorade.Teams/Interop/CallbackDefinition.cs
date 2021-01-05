using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    internal class CallbackDefinition
    {

        public static CallbackDefinition Create(Delegate method)
        {
            if(null != method)
            {
                ValidateCallbackMethod(method.Method);

                return new CallbackDefinition
                {
                    Target = DotNetObjectReference.Create(method.Target),
                    MethodName = method.Method.Name
                };
            }
            return null;
        }

        public static CallbackDefinition Create(Func<Task> method)
        {
            return Create(method as Delegate);
        }

        public static CallbackDefinition Create<T>(Func<T, Task> method)
        {
            return Create(method as Delegate);
        }

        private static void ValidateCallbackMethod(MethodInfo method)
        {
            var attribute = method.GetCustomAttribute<JSInvokableAttribute>();
            if(null == attribute)
            {
                throw new ArgumentException($"A callback method must be a defined method decorated with the '{typeof(JSInvokableAttribute).FullName}' attribute.", nameof(method));
            }
        }

        public DotNetObjectReference<object> Target { get; set; }

        public string MethodName { get; set; }

    }
}
