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
using SnCore.Data.Hibernate;

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
        SetDefaultButton(linkSave);
        if (!IsPostBack)
        {
            DomainClass cs = SessionManager.GetDomainClass("AccountContent");
            inputTag.MaxLength = cs["Tag"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitAccountContent tf = SessionManager.ContentService.GetAccountContentById(
                    SessionManager.Ticket, RequestId);

                inputTag.Text = tf.Tag;
                inputText.Text = tf.Text;
                inputTimestamp.SelectedDate = Adjust(tf.Timestamp);

                linkPreview.NavigateUrl = string.Format("AccountContentGroupView.aspx?id={0}", tf.AccountContentGroupId);
            }
            else
            {
                inputTimestamp.SelectedDate = Adjust(DateTime.UtcNow);
                linkPreview.Visible = false;
            }

            linkBack.NavigateUrl = string.Format("AccountContentGroupEdit.aspx?id={0}", AccountContentGroupId);
        }
    }

    public void save(object sender, EventArgs e)
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

        if (!inputTimestamp.HasDate)
        {
            throw new ArgumentException("Missing timestamp.");
        }

        s.Timestamp = base.ToUTC(inputTimestamp.SelectedDate);

        SessionManager.CreateOrUpdate<TransitAccountContent>(
            s, SessionManager.ContentService.CreateOrUpdateAccountContent);
        Redirect(string.Format("AccountContentGroupEdit.aspx?id={0}", AccountContentGroupId));
    }
}
