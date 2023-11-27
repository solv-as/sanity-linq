using System;

namespace Sanity.Linq.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SanityDocumentName : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the document type in sanity.
        /// </summary>
        /// <value>The name of the document type.</value>
        public string DocumentName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SanityDocumentName"/> class with the specified name.
        /// </summary>
        /// <param name="documentName">The name of the document type.</param>
        public SanityDocumentName(string documentName)
        {
            DocumentName = documentName;
        }

    }
}
