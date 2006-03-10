using System;
using System.Web;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Provides an interface for progress providers.
	/// </summary>
	public interface IProgressProvider
	{
		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="bar"></param>
		void Initialize(HttpContext context, ProgressBar bar);

		/// <summary>
		/// Creates a new job ID.
		/// </summary>
		/// <returns></returns>
		string CreateJobID();

		/// <summary>
		/// Removes a job ID.
		/// </summary>
		/// <param name="jobID"></param>
		void RemoveJobID(string jobID);

		/// <summary>
		/// Gets the progress of the progress bar.
		/// </summary>
		/// <param name="jobID">The ID of the job.</param>
		/// <returns>The progress the job has made.</returns>
		double GetProgress(string jobID);

		/// <summary>
		/// Sets the progress of the progress bar.
		/// </summary>
		/// <param name="jobID">The ID of the job.</param>
		/// <param name="value">The progress the job has made.</param>
		void SetProgress(string jobID, double value);
	}
}