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

public partial class TellAFriendControl : Control
{
    protected void Page_Load(object sender, EventArgs e)
    {
            if (!IsPostBack)
            {
                linkTellAFriend.NavigateUrl = string.Format("TellAFriend.aspx?Url={0}&Subject={1}",
                    Renderer.UrlEncode(Request.Url.PathAndQuery),
                    Renderer.UrlEncode(Page.Title));
            }
    }
}
