using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls
{
    /// <summary>
    /// Represents a select all check box.
    /// </summary>
    public class SelectAllCheckBox : HtmlInputCheckBox
    {
        private const string StartupScript = @"for (var i = 0; i < __rowSelectorField_participants.length; i++) __rowSelectorField_register(__rowSelectorField_participants[i]);";
        private const string SelectAllScript = @"__rowSelectorField_selectAll(this);";
        private const string CheckedChangedScript = @"__rowSelectorField_checkedChanged('{0}');";

        private bool autoPostBack;
        private List<CheckBox> participants;

        /// <summary>
        /// Gets or sets whether the field should postback when the selection changes.
        /// </summary>
        [
        DefaultValue(false),
        Description("Gets or sets whether the field should postback when the selection changes.")
        ]
        public bool AutoPostBack
        {
            get
            {
                return this.autoPostBack;
            }
            set
            {
                this.autoPostBack = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.SelectAllCheckBox"/> class.
        /// </summary>
        public SelectAllCheckBox()
        {
            this.participants = new List<CheckBox>();
        }

        /// <summary>
        /// Registers a participant.
        /// </summary>
        /// <param name="participant"></param>
        public void RegisterParticipant(CheckBox participant)
        {
            this.participants.Add(participant);
            participant.PreRender += new EventHandler(this.participant_PreRender);
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.RegisterClientScript();
        }

        /// <summary>
        /// Renders the attributes.
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            string onClick = this.Attributes["onclick"];
            if (onClick == null)
            {
                onClick = SelectAllCheckBox.SelectAllScript;
            }
            else
            {
                onClick += ";" + SelectAllCheckBox.SelectAllScript;
            }

            if (this.AutoPostBack)
            {
                onClick += this.Page.ClientScript.GetPostBackEventReference(this, null);
            }
            this.Attributes["onclick"] = onClick;

            base.RenderAttributes(writer);
        }

        private void RegisterClientScript()
        {
            if (this.Page == null || this.participants.Count == 0)
                return;

            ClientScriptManager csm = this.Page.ClientScript;

            StringBuilder participantArray = new StringBuilder();
            foreach (CheckBox participant in this.participants)
            {
                //script.AppendFormat(SelectAllCheckBox.RegisterParticipantScript, this.ClientID, participant.ClientID);
                if (participantArray.Length == 0){
                    participantArray.AppendFormat("{{ o: '{0}', c: '{1}' }}", this.ClientID, participant.ClientID);
                }
                else
                {
                    participantArray.AppendFormat(",{{ o: '{0}', c: '{1}' }}", this.ClientID, participant.ClientID);
                }
            }

            csm.RegisterArrayDeclaration("__rowSelectorField_participants", participantArray.ToString());

            string startupScriptKey = "RowSelectorField";
            if (!csm.IsStartupScriptRegistered(this.GetType(), startupScriptKey))
            {
                csm.RegisterStartupScript(this.GetType(), startupScriptKey, SelectAllCheckBox.StartupScript, true);
                csm.RegisterClientScriptResource(this.GetType(), "Wilco.Web.Resources.RowSelectorField.js");
            }
        }

        private void participant_PreRender(object sender, EventArgs e)
        {
            CheckBox participant = ((CheckBox)sender);
            string onClick = participant.Attributes["onclick"];
            if (onClick == null)
            {
                onClick = String.Format(SelectAllCheckBox.CheckedChangedScript, this.ClientID);
            }
            else
            {
                onClick += ";" + String.Format(SelectAllCheckBox.CheckedChangedScript, this.ClientID);
            }
            participant.Attributes["onclick"] = onClick;
        }
    }
}