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

public partial class BookmarksViewControl : Control
{
    private bool mShowThumbnail = false;
    private int mRepeatColumns = 1;

    public int RepeatColumns
    {
        get
        {
            return mRepeatColumns;
        }
        set
        {
            mRepeatColumns = value;
        }
    }

    public bool ShowThumbnail
    {
        get
        {
            return mShowThumbnail;
        }
        set
        {
            mShowThumbnail = value;
        }
    }

    public string GetUrl(string urlformat)
    {
        string result = urlformat;
        result = result.Replace("{url}", Renderer.UrlEncode(Request.Url.ToString()));
        result = result.Replace("{title}", Renderer.UrlEncode(Page.Title));
        return result;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BookmarkQueryOptions options = new BookmarkQueryOptions();
            options.WithFullBitmaps = ! ShowThumbnail;
            options.WithLinkedBitmaps = ShowThumbnail;
            object[] args = { options };
            bookmarksView.DataSource = SessionManager.GetCachedCollection<TransitBookmark>(
                SessionManager.SystemService, "GetBookmarksWithOptions", args);
            bookmarksView.DataBind();
            bookmarksView.RepeatColumns = (RepeatColumns <= 0) ? bookmarksView.Items.Count : RepeatColumns; 
        }
    }
}
