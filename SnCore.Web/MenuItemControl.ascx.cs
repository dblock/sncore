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
using Wilco.Web.UI;
using Wilco.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;

public partial class MenuItemControl : System.Web.UI.UserControl
{
    private string mNavigateUrl = "Default.aspx";
    private string mCssClass = "sncore_menu_item";
    private string mCssClassSelected = "sncore_menu_item_selected";
    private string mCssClassActivated = "sncore_menu_item_activated";
    private ListItemCollection mHighlights = new ListItemCollection();
    private ListItemCollection mDownlights = new ListItemCollection();

    protected override void OnLoad(EventArgs e)
    {
        if (!IsPostBack)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("document.getElementById('{0}').attributes['class'].value = '{1}';",
                labelItem.ClientID, CssClassActivated));
            // sb.Append(Page.ClientScript.GetPostBackEventReference(this, string.Empty));
            sb.Append(";");
            labelItem.Attributes.Add("style", "padding-left: 10px; padding-right: 10px;"); // for e-mail readers that strip <style> tags
            menuItem.Attributes.Add("onclick", sb.ToString());
            base.OnLoad(e);
        }
    }

    public ListItemCollection Highlights
    {
        get
        {
            return mHighlights;
        }
        set
        {
            mHighlights = value;
        }
    }

    public ListItemCollection Downlights
    {
        get
        {
            return mDownlights;
        }
        set
        {
            mDownlights = value;
        }
    }

    public string NavigateUrl
    {
        get
        {
            return mNavigateUrl;
        }
        set
        {
            mNavigateUrl = value;
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

    public string CssClassSelected
    {
        get
        {
            return mCssClassSelected;
        }
        set
        {
            mCssClassSelected = value;
        }
    }

    public string CssClassActivated
    {
        get
        {
            return mCssClassActivated;
        }
        set
        {
            mCssClassActivated = value;
        }
    }

    public string Text
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "Text", "Menu Item");
        }
        set
        {
            ViewState["Text"] = value;
        }
    }

    public string CurrentCssClass
    {
        get
        {
            if (Request.Url.LocalPath.EndsWith(string.Format("/{0}", NavigateUrl)))
            {
                return CssClassSelected;
            }

            foreach (ListItem d in Downlights)
            {
                if (Regex.IsMatch(Request.Url.LocalPath, d.Value))
                {
                    return CssClass;
                }
            }

            foreach (ListItem h in Highlights)
            {
                if (Regex.IsMatch(Request.Url.LocalPath, h.Value))
                {
                    return CssClassSelected;
                }
            }

            return CssClass;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        labelItem.Text = Text;
        labelItem.CssClass = CurrentCssClass;
        menuItem.HRef = NavigateUrl;
        base.OnPreRender(e);
    }
}
