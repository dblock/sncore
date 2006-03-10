using System;
using System.Collections.ObjectModel;
using System.Web;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a method which handles the 
	/// <see cref="Wilco.Web.UI.WebControls.MultiFileUpload.FilesPosted"/> event.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public delegate void FilesPostedEventHandler(object sender, FilesPostedEventArgs e);

	/// <summary>
	/// Represents event data.
	/// </summary>
	[Serializable]
	public sealed class FilesPostedEventArgs : EventArgs
	{
		private ReadOnlyCollection<HttpPostedFile> postedFiles;

		/// <summary>
		/// Gets the array of posted files.
		/// </summary>
		public ReadOnlyCollection<HttpPostedFile> PostedFiles
		{
			get
			{
				return this.postedFiles;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanelMovedEventArgs"/> class.
		/// </summary>
		/// <param name="postedFiles">An array of posted files.</param>
		public FilesPostedEventArgs(HttpPostedFile[] postedFiles)
		{
			this.postedFiles = new ReadOnlyCollection<HttpPostedFile>(postedFiles);
		}
	}
}