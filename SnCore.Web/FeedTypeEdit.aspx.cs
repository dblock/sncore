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
using SnCore.Tools.Web;
using System.IO;
using SnCore.Services;
using SnCore.SiteMap;
using Wilco.Web.UI;

public partial class FeedTypeEdit : AuthenticatedPage
{
    public string Xsl
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(
                ViewState, "Xsl", string.Empty);
        }
        set
        {
            ViewState["Xsl"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            for (int i = 1; i < 20; i++)
            {
                inputSpanColumns.Items.Add(new ListItem(i.ToString(), i.ToString()));
                inputSpanRows.Items.Add(new ListItem(i.ToString(), i.ToString()));
                inputSpanColumnsPreview.Items.Add(new ListItem(i.ToString(), i.ToString()));
                inputSpanRowsPreview.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("System Preferences", Request, "SystemPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Feed Types", Request, "FeedTypesManage.aspx"));

            if (RequestId > 0)
            {
                TransitFeedType t = SessionManager.SyndicationService.GetFeedTypeById(
                    SessionManager.Ticket, RequestId);
                inputName.Text = t.Name;
                inputSpanColumns.Items.FindByValue(t.SpanColumns.ToString()).Selected = true;
                inputSpanRows.Items.FindByValue(t.SpanRows.ToString()).Selected = true;
                inputSpanColumnsPreview.Items.FindByValue(t.SpanColumnsPreview.ToString()).Selected = true;
                inputSpanRowsPreview.Items.FindByValue(t.SpanRowsPreview.ToString()).Selected = true;
                if (!string.IsNullOrEmpty(t.Xsl))
                {
                    Xsl = t.Xsl;
                    labelXsl.Text = string.Format("{0} Kb",
                        ((double)t.Xsl.Length / 1024).ToString("0.00"));
                }
                else
                {
                    linkXslClear.Enabled = false;
                }

                sitemapdata.Add(new SiteMapDataAttributeNode(t.Name, Request.Url));
            }
            else
            {
                sitemapdata.Add(new SiteMapDataAttributeNode("New Feed Type", Request.Url));
            }

            StackSiteMap(sitemapdata);
        }

        SetDefaultButton(manageAdd);
    }

    public void save_Click(object sender, EventArgs e)
    {
        TransitFeedType t = new TransitFeedType();
        t.Name = inputName.Text;
        t.Id = RequestId;
        t.SpanColumns = int.Parse(inputSpanColumns.SelectedValue);
        t.SpanRows = int.Parse(inputSpanRows.SelectedValue);
        t.SpanColumnsPreview = int.Parse(inputSpanColumnsPreview.SelectedValue);
        t.SpanRowsPreview = int.Parse(inputSpanRowsPreview.SelectedValue);
        if (inputXsl.HasFile)
        {
            Xsl = t.Xsl = new StreamReader(inputXsl.FileContent).ReadToEnd();
        }
        else
        {
            t.Xsl = Xsl;
        }
        SessionManager.CreateOrUpdate<TransitFeedType>(
            t, SessionManager.SyndicationService.CreateOrUpdateFeedType);
        Redirect("FeedTypesManage.aspx");
    }

    public void linkXslClear_Click(object sender, EventArgs e)
    {
        Xsl = string.Empty;
        labelXsl.Text = string.Empty;
        linkXslClear.Enabled = false;
    }
}
