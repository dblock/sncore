using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Wilco.Web.UI.WebControls
{
    /// <summary>
    /// Represents a control which lets a user upload multiple files at once.
    /// </summary>
    [ParseChildren(true)]
    public class MultiFileUpload : WebControl
    {
        private HiddenField fileCount;
        private static readonly object EventFilesPosted = new object();

        /// <summary>
        /// Occurs when one or more files are posted.
        /// </summary>
        [Description("Occurs when one or more files are posted.")]
        public event FilesPostedEventHandler FilesPosted
        {
            add
            {
                this.Events.AddHandler(MultiFileUpload.EventFilesPosted, value);
            }
            remove
            {
                this.Events.RemoveHandler(MultiFileUpload.EventFilesPosted, value);
            }
        }

        /// <summary>
        /// Gets or sets the number of files the user can initially upload.
        /// </summary>
        [
        DefaultValue(1),
        Description("Gets or sets the number of files the user can initially upload."),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public int FileCount
        {
            get
            {
                this.EnsureChildControls();
                if (this.fileCount.Value.Length > 0)
                {
                    int result;
                    if (Int32.TryParse(this.fileCount.Value, out result))
                    {
                        return result;
                    }
                }

                return 1;
            }
            set
            {
                this.EnsureChildControls();
                this.fileCount.Value = value.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the css class for each file's container.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the css class for each file's container."),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public string FileCssClass
        {
            get
            {
                return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "FileCssClass", String.Empty);
            }
            set
            {
                this.ViewState["FileCssClass"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the css class for each file's container.
        /// </summary>
        [
        Category("Appearance"),
        Description("Gets or sets the css class for each file's input."),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public string InputCssClass
        {
            get
            {
                return ViewStateUtility.GetViewStateValue<string>(this.ViewState, "InputCssClass", String.Empty);
            }
            set
            {
                this.ViewState["InputCssClass"] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.MultiFileUpload"/> class.
        /// </summary>
        public MultiFileUpload()
            : base(HtmlTextWriterTag.Div)
        {
            //
        }

        /// <summary>
        /// Gets a reference to the script which can add a file upload control.
        /// </summary>
        /// <returns></returns>
        public string GetAddFileScriptReference()
        {
            this.EnsureChildControls();
            return String.Format("__multiFileUploadAddFile('{0}', '{1}', '{2}', '{3}');", this.ClientID, this.fileCount.ClientID, this.FileCssClass, this.InputCssClass);
        }

        /// <summary>
        /// Notifies server controls that use composition-based implementation to create any 
        /// child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.fileCount = new HiddenField();
            this.Controls.Add(this.fileCount);
            this.fileCount.ID = "fileCount";
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // Ensure that the child controls are loaded, so that their state will be loaded in time.
            this.EnsureChildControls();

            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            HtmlForm form = this.Page.Form;
            if ((form != null) && (form.Enctype.Length == 0))
            {
                form.Enctype = "multipart/form-data";
            }

            List<HttpPostedFile> postedFiles = null;
            if (this.Page.IsPostBack)
                postedFiles = new List<HttpPostedFile>();

            Division fileContainer;
            string customUniqueID;
            for (int i = 0; i < this.FileCount; i++)
            {
                customUniqueID = this.ClientID + i.ToString();
                if (this.Page.Request.Files[customUniqueID] == null)
                    customUniqueID = null;

                fileContainer = new Division();
                this.Controls.Add(fileContainer);
                fileContainer.CssClass = this.FileCssClass;

                FileUploadEx file = new FileUploadEx(customUniqueID, InputCssClass);
                fileContainer.Controls.Add(file);

                if (file.PostedFile != null && file.PostedFile.FileName.Length > 0)
                    postedFiles.Add(file.PostedFile);
            }

            if (this.Page.IsPostBack)
                this.OnFilesPosted(new FilesPostedEventArgs(postedFiles.ToArray()));

            this.RegisterClientScript();
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (this.Page != null)
            {
                this.Page.VerifyRenderingInServerForm(this);
            }

            base.Render(writer);
        }

        /// <summary>
        /// Registers the client script.
        /// </summary>
        private void RegisterClientScript()
        {
            ClientScriptManager csm = this.Page.ClientScript;
            csm.RegisterClientScriptResource(this.GetType(), "Wilco.Web.Resources.MultiFileUpload.js");
        }

        /// <summary>
        /// Raises the <see cref="Wilco.Web.UI.WebControls.MultiFileUpload.FilesPosted"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFilesPosted(FilesPostedEventArgs e)
        {
            FilesPostedEventHandler handler = this.Events[MultiFileUpload.EventFilesPosted] as FilesPostedEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}