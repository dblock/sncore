using System;
using System.Web;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a progress provider, which stores the progress temporarily in the cache.
	/// </summary>
	public class ProgressProvider : IProgressProvider
	{
		private HttpContext context;
		private ProgressBar progressBar;

		/// <summary>
		/// Gets the context.
		/// </summary>
		protected HttpContext Context
		{
			get
			{
				return this.context;
			}
		}

		/// <summary>
		/// Gets the progress bar.
		/// </summary>
		protected ProgressBar ProgressBar
		{
			get
			{
				return this.progressBar;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.ProgressProvider"/> class.
		/// </summary>
		public ProgressProvider()
		{
			//
		}

		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="bar"></param>
		public virtual void Initialize(HttpContext context, ProgressBar bar)
		{
			this.context = context;
			this.progressBar = bar;
		}

		/// <summary>
		/// Creates a new job ID.
		/// </summary>
		/// <returns></returns>
		public string CreateJobID()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Removes a job ID.
		/// </summary>
		/// <param name="jobID"></param>
		public void RemoveJobID(string jobID)
		{
			if (this.context.Cache[jobID] != null)
				this.context.Cache.Remove(jobID);
		}

		/// <summary>
		/// Gets the progress of the progress bar.
		/// </summary>
		/// <param name="jobID">The ID of the job.</param>
		/// <returns>The progress the job has made.</returns>
		public virtual double GetProgress(string jobID)
		{
			object savedState = this.context.Cache[jobID];
			if (savedState != null)
			{
				double range = this.progressBar.Maximum - this.progressBar.Minimum;
				double value = (double)savedState - this.progressBar.Minimum;
				double result = value / range * 100;
				if (result >= 100)
					this.RemoveJobID(jobID);

				return result;
			}

			return 0;
		}

		/// <summary>
		/// Sets the progress of the progress bar.
		/// </summary>
		/// <param name="jobID">The ID of the job.</param>
		/// <param name="value">The progress the job has made.</param>
		public virtual void SetProgress(string jobID, double value)
		{
			if (value < this.progressBar.Minimum || value > this.progressBar.Maximum)
				throw new ArgumentOutOfRangeException("value");

			this.context.Cache[jobID] = value;
		}
	}
}