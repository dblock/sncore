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

public class TitleContainer : Control, INamingContainer
{
    private string mText;

    public TitleContainer(string value)
    {
        mText = value;
    }

    public string Text
    {
        get
        {
            return mText;
        }
    }
}

[ToolboxData("<{0}:TitleControl runat=server></{0}:TitleControl>"), ParseChildren(true)]
public partial class TitleControl : Control, INamingContainer
{
    ITemplate mTemplate;
    TitleContainer mContainer;

    [TemplateContainer(typeof(TitleContainer))]
    public virtual ITemplate Template
    {
        get
        {
            return mTemplate;
        }
        set
        {
            mTemplate = value;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        mContainer = new TitleContainer(string.Empty);
        Template.InstantiateIn(mContainer);
        divHelp.Controls.Add(mContainer);
        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        DataBind();
        base.OnLoad(e);
    }

    public virtual TitleContainer Container
    {
        get
        {
            return mContainer;
        }
    }

    public string Text
    {
        get
        {
            return labelText.Text;
        }
        set
        {
            labelText.Text = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            divHelp.Attributes["style"] = "display: none;";
            imageHelp.OnClientClick = string.Format("document.getElementById('{0}').style.cssText = '';",
                divHelp.ClientID);
        }
        else
        {
            divHelp.Attributes["style"] = string.Empty;
        }
    }
}
