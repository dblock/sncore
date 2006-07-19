using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Wilco.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using SnCore.WebServices;
using SnCore.Services;

public partial class AccountContentEdit : AuthenticatedPage
{
    public int AccountContentGroupId
    {
        get
        {
            return GetId("gid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(linkSave);
            if (!IsPostBack)
            {
                if (RequestId > 0)
                {
                    TransitAccountContent tf = ContentService.GetAccountContentById(
                        SessionManager.Ticket, RequestId);

                    inputTag.Text = tf.Tag;
                    inputText.Text = tf.Text;
                    inputPosition.Text = tf.Position.ToString();

                    linkPreview.NavigateUrl = string.Format("AccountContentView.aspx?id={0}", RequestId);
                }
                else
                {
                    linkPreview.Visible = false;
                }

                linkBack.NavigateUrl = string.Format("AccountContentGroupEdit.aspx?id={0}", AccountContentGroupId);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save(object sender, EventArgs e)
    {
        try
        {
            TransitAccountContent s = new TransitAccountContent();
            s.Id = RequestId;
            s.Tag = inputTag.Text;
            s.AccountContentGroupId = AccountContentGroupId;

            if (string.IsNullOrEmpty(s.Tag))
            {
                throw new ArgumentException("Missing tag.");
            }

            s.Text = inputText.Text;

            int position = 0;
            if (!int.TryParse(inputPosition.Text, out position))
            {
                throw new ArgumentException("Position must be a number.");
            }

            s.Position = position;

            ContentService.CreateOrUpdateAccountContent(SessionManager.Ticket, s);
            Redirect(string.Format("AccountContentGroupEdit.aspx?id={0}", AccountContentGroupId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
