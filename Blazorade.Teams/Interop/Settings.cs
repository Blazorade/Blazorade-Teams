using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazorade.Teams.Interop
{
    /// <summary>
    /// Settings for configuring tab applications with.
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Sets the URL to use for the content of this instance.
        /// </summary>
        public string ContentUrl { get; set; }

        /// <summary>
        /// The developer-defined unique ID for the entity to which this content points.
        /// </summary>
        public string EntityId { get; set; }

        /// <summary>
        /// Sets the URL for the removal configuration experience.
        /// </summary>
        public string RemoveUrl { get; set; }

        /// <summary>
        /// A suggested display name for the new content. In the settings for an existing instance being updated, this call has no effect.
        /// </summary>
        public string SuggestedDisplayName { get; set; }

        /// <summary>
        /// Sets the URL to use for the external link to view the underlying resource in a browser.
        /// </summary>
        public string WebsiteUrl { get; set; }

    }
}
