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

public partial class FeedTypeEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                for (int i = 1; i < 20; i++)
                {
                    inputSpanColumns.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    inputSpanRows.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    inputSpanColumnsPreview.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    inputSpanRowsPreview.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                if (RequestId > 0)
                {
                    TransitFeedType t = SyndicationService.GetFeedTypeById(RequestId);
                    inputName.Text = t.Name;
                    inputSpanColumns.Items.FindByValue(t.SpanColumns.ToString()).Selected = true;
                    inputSpanRows.Items.FindByValue(t.SpanRows.ToString()).Selected = true;
                    inputSpanColumnsPreview.Items.FindByValue(t.SpanColumnsPreview.ToString()).Selected = true;
                    inputSpanRowsPreview.Items.FindByValue(t.SpanRowsPreview.ToString()).Selected = true;
                    if (!string.IsNullOrEmpty(t.Xsl))
                    {
                        labelXsl.Text = string.Format("{0} Kb",
                            ((double)t.Xsl.Length / 1024).ToString("0.00"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
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
                t.Xsl = new StreamReader(inputXsl.FileContent).ReadToEnd();
            }
            SyndicationService.CreateOrUpdateFeedType(SessionManager.Ticket, t);
            Redirect("FeedTypesManage.aspx");
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
