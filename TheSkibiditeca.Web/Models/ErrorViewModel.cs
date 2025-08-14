// Copyright (c) dominuxLABS. All rights reserved.

namespace TheSkibiditeca.Web.Models
{
    /// <summary>
    /// Represents the view model for error pages.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request identifier for debugging purposes.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether the request ID should be displayed.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}
