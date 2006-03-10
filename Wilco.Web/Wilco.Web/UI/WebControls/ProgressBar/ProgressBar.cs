using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
	/// <summary>
	/// Represents a control which can display the progress of a process on the client-side.
	/// </summary>
	public class ProgressBar : WebControl, ICallbackEventHandler, INamingContainer
	{
		private string processStarter;
		private bool autoStart;
		private bool startOnSubmit;
		private IProgressProvider provider;
		private WebControl processStarterControl;
		private Division bar;
		private HiddenField job;
        private string callbackResult;

		/// <summary>
		/// Gets whether the progress bar automatically starts when the page is loaded.
		/// </summary>
		[
		DefaultValue(false),
		Description("Gets whether the progress bar automatically starts when the page is loaded.")
		]
		public bool AutoStart
		{
			get
			{
				return this.autoStart;
			}
			set
			{
				this.autoStart = value;
			}
		}

		/// <summary>
		/// Gets the bar style.
		/// </summary>
		public Style BarStyle
		{
			get
			{
				this.EnsureChildControls();
				return this.bar.ControlStyle;
			}
		}

		/// <summary>
		/// Gets the empty control collection, which prevents a user from adding child controls.
		/// </summary>
		[Browsable(false)]
		public override ControlCollection Controls
		{
			get
			{
				return new EmptyControlCollection(this);
			}
		}

		/// <summary>
		/// Gets or sets the job ID.
		/// </summary>
		[Description("Gets or sets the job ID. If no job ID is specified, a unique job ID will be used.")]
		public string JobID
		{
			get
			{
				this.EnsureChildControls();

				if (String.IsNullOrEmpty(this.job.Value))
					this.job.Value = this.ProgressProvider.CreateJobID();

				return this.job.Value;
			}
			set
			{
				this.EnsureChildControls();
				this.job.Value = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum progress value. If this value is set, the progres equals 100%.
		/// </summary>
		[
		DefaultValue(100),
		Description("Gets or sets the maximum progress value. If this value is set, the progres equals 100%.")
		]
		public int Maximum
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<int>(this.ViewState, "Maximum", 100);
			}
			set
			{
				this.ViewState["Maximum"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the minimum progress value. If this value is set, the progres equals 0%.
		/// </summary>
		[
		DefaultValue(0),
		Description("Gets or sets the minimum progress value. If this value is set, the progres equals 0%.")
		]
		public int Minimum
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<int>(this.ViewState, "Minimum", 0);
			}
			set
			{
				this.ViewState["Minimum"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the control which starts the process.
		/// </summary>
		[
		DefaultValue(""),
		Description("Gets or sets the control which starts the process.")
		]
		public string ProcessStarter
		{
			get
			{
				return this.processStarter;
			}
			set
			{
				this.processStarter = value;
			}
		}

		/// <summary>
		/// Gets the progress provider.
		/// </summary>
		protected IProgressProvider ProgressProvider
		{
			get
			{
				if (this.provider == null)
				{
					this.provider = new ProgressProvider();
					this.provider.Initialize(this.Context, this);
				}

				return this.provider;
			}
		}

		/// <summary>
		/// Gets or sets the refresh interval.
		/// </summary>
		[
		DefaultValue(1000),
		Description("Gets or sets the refresh interval.")
		]
		public int RefreshInterval
		{
			get
			{
				return ViewStateUtility.GetViewStateValue<int>(this.ViewState, "RefreshInterval", 1000);
			}
			set
			{
				this.ViewState["RefreshInterval"] = value;
			}
		}

		/// <summary>
		/// Gets whether the progress bar should starts when the page is submitted.
		/// </summary>
		[
		DefaultValue(false),
		Description("Gets whether the progress bar should starts when the page is submitted.")
		]
		public bool StartOnSubmit
		{
			get
			{
				return this.startOnSubmit;
			}
			set
			{
				this.startOnSubmit = value;
			}
		}

		/// <summary>
		/// Gets or sets the position within the range of the progress bar. The default is 0.
		/// </summary>
		[
		DefaultValue(0),
		Description("Gets or sets the position within the range of the progress bar. The default is 0.")
		]
		public int Value
		{
			get
			{
				this.EnsureChildControls();

				IProgressProvider provider = this.ProgressProvider;
				return (int)provider.GetProgress(this.JobID);
			}
			set
			{
				this.EnsureChildControls();

				IProgressProvider provider = this.ProgressProvider;
				provider.SetProgress(this.JobID, value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.ProgressBar"/> class.
		/// </summary>
		public ProgressBar()
			: base(HtmlTextWriterTag.Div)
		{
			//
		}

		/// <summary>
		/// Sets the starter control. If the control is clicked, the progress checker will start.
		/// </summary>
		/// <param name="starterControl"></param>
		public void SetStarterControl(WebControl starterControl)
		{
			this.processStarterControl = starterControl;
		}

		/// <summary>
		/// Notifies server controls that use composition-based implementation to create any 
		/// child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.job = new HiddenField();
			base.Controls.Add(this.job);
			this.job.ID = "job";

			this.bar = new Division();
			base.Controls.Add(this.bar);
			this.bar.ID = "bar";
			this.bar.Controls.Add(new LiteralControl("&nbsp;"));
		}

		/// <summary>
		/// Ensures that the process starter is set.
		/// </summary>
		private void EnsureProcessStarter()
		{
			if (this.processStarterControl == null)
			{
				if (!String.IsNullOrEmpty(this.processStarter))
				{
					this.processStarterControl = this.NamingContainer.FindControl(this.processStarter) as WebControl;
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.Init"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			this.Page.RegisterRequiresControlState(this);
		}

		/// <summary>
		/// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			this.EnsureProcessStarter();
			if (this.processStarterControl == null && !this.startOnSubmit)
				throw new InvalidOperationException("No progress starter specified.");

			this.RegisterClientScript();

			this.UpdateProgress();
		}

		/// <summary>
		/// Registers the client script.
		/// </summary>
		private void RegisterClientScript()
		{
			ClientScriptManager csm = this.Page.ClientScript;

			string callbackFunction = csm.GetCallbackEventReference(this, String.Format("document.getElementById('{0}').value", this.job.ClientID), "__progressBarCallback", "'" + this.bar.ClientID + "'", true);

			csm.RegisterArrayDeclaration("__progressBarInstances", String.Format("new __progressBarInstance('{0}', \"{1}\", {2})", this.bar.ClientID, callbackFunction, this.RefreshInterval));
			
			// Register the initialize progress bar script.
			string initScriptKey = this.GetType().Name;
			if (!csm.IsStartupScriptRegistered(initScriptKey))
			{
				string initScript = "__progressBarInit(__progressBarInstances);";
				csm.RegisterStartupScript(this.GetType(), initScriptKey, initScript, true);
			}

			csm.RegisterClientScriptResource(this.GetType(), "Wilco.Web.Resources.ProgressBar.js");

			if (this.AutoStart)
			{
				string autoStartScriptKey = "__progressBarAutoStart" + this.ClientID;
				if (!csm.IsStartupScriptRegistered(this.GetType(), autoStartScriptKey))
				{
					string script = String.Format("__progressBarEnqueue('{0}');", this.bar.ClientID);
					csm.RegisterStartupScript(this.GetType(), autoStartScriptKey, script, true);
				}
			}

			if (this.startOnSubmit)
			{
				string submitScriptKey = "__progressBarOnSubmit" + this.ClientID;
				if (!csm.IsOnSubmitStatementRegistered(this.GetType(), submitScriptKey))
				{
					string script = String.Format("__progressBarEnqueue('{0}');", this.bar.ClientID);
					csm.RegisterOnSubmitStatement(this.GetType(), submitScriptKey, script);
				}
			}
			else
			{
				WebControl control = (WebControl)this.processStarterControl;
				control.Attributes["onclick"] += callbackFunction + ";";
			}
		}

		/// <summary>
		/// Ensures that the width of the progress bar is up to date.
		/// </summary>
		protected void UpdateProgress()
		{
			this.EnsureChildControls();

			double progress = this.ProgressProvider.GetProgress(this.JobID);
			if (progress < 0 || progress > 100)
				progress = 0;

			this.bar.Width = new Unit(progress, UnitType.Percentage);
		}

		#region ICallbackEventHandler Members
		/// <summary>
		/// Executes the logic for the current callback.
		/// </summary>
		/// <param name="eventArgument"></param>
		public void RaiseCallbackEvent(string eventArgument)
		{
			double progress = this.ProgressProvider.GetProgress(eventArgument);
			if (progress >= 100)
				provider.RemoveJobID(eventArgument);

			this.callbackResult = progress.ToString();
		}

        /// <summary>
        /// Gets the callback result.
        /// </summary>
        /// <returns></returns>
        public string GetCallbackResult()
        {
            return this.callbackResult;
        }
		#endregion
	}
}