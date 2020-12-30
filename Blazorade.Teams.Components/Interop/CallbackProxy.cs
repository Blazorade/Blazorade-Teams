using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    internal class CallbackProxy<TSuccess, TFailure>
    {
        public CallbackProxy(IJSObjectReference module)
        {
            this.Module = module ?? throw new ArgumentNullException(nameof(module));
        }

        private IJSObjectReference Module;
        private TaskCompletionSource<TSuccess> Promise;

        [JSInvokable]
        public Task SuccessCallbackAsync(TSuccess result = default)
        {
            this.Promise.TrySetResult(result);

            return Task.CompletedTask;
        }

        public Task FailureCallbackAsync(TFailure result = default)
        {
            this.Promise.TrySetException(new FailureCallbackException(result));

            return Task.CompletedTask;
        }

        public Task<TSuccess> GetResultAsync(string identifier, Dictionary<string, object> args = null)
        {
            this.Promise = new TaskCompletionSource<TSuccess>();
            var input = new CallbackMethodArgs
            {
                SuccessCallback = CallbackDefinition.Create<TSuccess>(this.SuccessCallbackAsync),
                FailureCallback = CallbackDefinition.Create<TFailure>(this.FailureCallbackAsync),
                Args = args ?? new Dictionary<string, object>()
            };

            this.Module.InvokeVoidAsync(identifier, input);
            
            return this.Promise.Task;
        }

    }

    internal class CallbackProxy<TResult> : CallbackProxy<TResult, object>
    {
        public CallbackProxy(IJSObjectReference module) : base(module) { }
    }

    internal class CallbackProxy : CallbackProxy<object, object>
    {
        public CallbackProxy(IJSObjectReference module) : base(module) { }
    }
}
