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

public partial class DiscussionsFullViewControl : Control
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        }
    }

    public int ObjectId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "ObjectId", 0);
        }
        set
        {
            ViewState["ObjectId"] = value;
        }
    }

    public string Type
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "Type");
        }
        set
        {
            ViewState["Type"] = value;
        }
    }
}
