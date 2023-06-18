using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TizenDotNet4.components
{
    /// <summary>
    /// Represents a document.
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Gets or sets the key of the document.
        /// </summary>
        /// <value>Description of the key.</value>
        public string _key { get; set; }

        /// <summary>
        /// Gets or sets the ID of the document.
        /// </summary>
        /// <value>Description of the ID.</value>
        public string _id { get; set; }

        /// <summary>
        /// Gets or sets the revision of the document.
        /// </summary>
        /// <value>Description of the revision.</value>
        public string _rev { get; set; }

        /// <summary>
        /// Gets or sets the process step.
        /// </summary>
        public string ProcessStep { get; set; }

        /// <summary>
        /// Gets or sets the measured value.
        /// </summary>
        public string MeasuredValue { get; set; }
    }
}
