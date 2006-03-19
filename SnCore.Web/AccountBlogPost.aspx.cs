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
using SnCore.Services;
using SnCore.WebServices;

public partial class AccountBlogPost : AuthenticatedPage
{
    public int BlogId
    {
        get
        {
            return GetId("bid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(manageAdd);
            if (!IsPostBack)
            {
                linkBack.NavigateUrl = string.Format("AccountBlogEdit.aspx?id={0}", BlogId);

                if (RequestId != 0)
                {
                    TransitAccountBlogPost post = BlogService.GetAccountBlogPostById(SessionManager.Ticket, RequestId);
                    inputBody.Text = post.Body;
                    inputTitle.Text = post.Title;                    
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
            TransitAccountBlogPost tp = new TransitAccountBlogPost();
            tp.Title = inputTitle.Text;
            tp.Body = inputBody.Text;
            tp.AccountBlogId = BlogId;
            tp.Id = RequestId;
            BlogService.CreateOrUpdateAccountBlogPost(SessionManager.Ticket, tp);
            Redirect(string.Format("AccountBlogView.aspx?id={0}", BlogId));
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
