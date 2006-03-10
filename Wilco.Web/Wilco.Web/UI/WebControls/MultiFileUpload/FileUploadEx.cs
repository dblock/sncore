using System;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
    /// <summary>
    /// Represents a file upload control with the additiona ability to 
    /// control its client ID.
    /// </summary>
    internal class FileUploadEx : FileUpload
    {
        private string customUniqueID;

        /// <summary>
        /// Gets the unique ID.
        /// </summary>
        public override string UniqueID
        {
            get
            {
                if (String.IsNullOrEmpty(this.customUniqueID))
                    return base.UniqueID;

                return this.customUniqueID;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.FileUploadEx"/> class.
        /// </summary>
        public FileUploadEx()
        {
            //
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.FileUploadEx"/> class.
        /// </summary>
        /// <param name="uniqueID">The unique ID.</param>
        public FileUploadEx(string uniqueID, string cssClass)
        {
            this.customUniqueID = uniqueID;
            this.CssClass = cssClass;
        }
    }
}