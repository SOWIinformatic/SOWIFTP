// SOWI Informatik, www.sowi.ch
// Franz Schönbächler, Juni 2024

namespace SOWIFTP.Web.Models
{
    /// <summary>
    /// Represents the model for displaying errors.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Gets or sets the request ID associated with the error.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show the request ID.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
