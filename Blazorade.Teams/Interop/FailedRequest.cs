using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    public class FailedRequest
    {
        public FailedRequest()
        {
            this.Reason = FailedReason.Other;
        }

        public string Message { get; set; }


        public FailedReason? Reason { get; set; }

    }
}
