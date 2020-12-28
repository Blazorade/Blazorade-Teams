using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Components.Interop
{
    public class LocaleInfo
    {
        public string LongDate { get; set; }

        public string LongTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>windows</item>
        /// <item>macos</item>
        /// </list>
        /// </remarks>
        public string Platform { get; set; }

        public string RegionalFormat { get; set; }

        public string ShortDate { get; set; }

        public string ShortTime { get; set; }

    }
}
