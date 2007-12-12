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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using SnCore.Services;
using System.Collections.Generic;
using SnCore.WebServices;
using SnCore.WebControls;

public partial class DiscussionsViewControl : Control
{
    private int mObjectId = 0;
    private string mType;
    private string mCssClass;
    private string mPostNewText;
    private int mOuterWidth;

    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetData(sender, e);
        }
    }

    private void GetData(object sender, EventArgs e)
    {
        if (! IsPostBack)
        {
            if (string.IsNullOrEmpty(Type) || ObjectId == 0)
                return;

            listDiscussions.DataSource = SessionManager.GetCollection<TransitDiscussion, string, int>(
                Type, ObjectId, null, SessionManager.DiscussionService.GetDiscussionsByObjectId);
            listDiscussions.DataBind();
        }
    }

    public int ObjectId
    {
        get
        {
            return mObjectId;
        }
        set
        {
            mObjectId = value;
        }
    }

    public string Type
    {
        get
        {
            return mType;
        }
        set
        {
            mType = value;
        }
    }

    public string CssClass
    {
        get
        {
            return mCssClass;
        }
        set
        {
            mCssClass = value;
        }
    }

    public string PostNewText
    {
        get
        {
            return mPostNewText;
        }
        set
        {
            mPostNewText = value;
        }
    }

    public int OuterWidth
    {
        get
        {
            return mOuterWidth;
        }
        set
        {
            mOuterWidth = value;
        }
    }

}
