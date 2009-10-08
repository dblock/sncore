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
using System.Drawing;

public partial class AToZControl : Control
{
    public event CommandEventHandler SelectedChanged;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public Char SelectedValue    
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<char>(ViewState, "Letter", Char.MinValue);
        }
        set
        {
            ViewState["Letter"] = value;
        }
    }

    protected override void CreateChildControls()
    {
        base.CreateChildControls();

        Char selected = SelectedValue;

        for (char c = 'A'; c <= 'Z'; c++)
        {
            AddButton(c);
        }

        //for (char c = '0'; c <= '9'; c++)
        //{
        //    AddButton(c);
        //}
    }

    void AddButton(char c)
    {
        LinkButton link = new LinkButton();
        link.ID = string.Format("A2Z_{0}", c);
        link.CommandArgument = c.ToString();
        link.CommandName = string.Format("A2Z_{0}", c);
        link.Text = string.Format("{0}&nbsp;", c);
        link.Command += new CommandEventHandler(link_Command);
        divatoz.Controls.Add(link);
    }
    public bool SelectIfChar(string s)
    {
        return SelectIfChar(s, true);
    }

    public void ClearSelection()
    {
        EnableDisableSelectedValue(true);
        SelectedValue = default(char);
    }

    /// <summary>
    /// Clear the selection and select if the target is a single character.
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public bool SelectIfChar(string s, bool alwaysSelect)
    {
        if (s.Length == 1)
        {
            ClearSelection();
            SelectedValue = s[0];
            return true;
        }
        else if (alwaysSelect)
        {
            ClearSelection();
        }

        return false;
    }

    protected override void OnPreRender(EventArgs e)
    {
        EnableDisableSelectedValue(false);
        base.OnPreRender(e);
    }

    void EnableDisableSelectedValue(bool enabled)
    {
        LinkButton button = (LinkButton)FindControl(string.Format("A2Z_{0}", SelectedValue));
        if (button != null)
        {
            button.Font.Bold = true;
            button.Font.Size = (enabled ? FontUnit.Empty : FontUnit.Larger);
        }
    }

    void link_Command(object sender, CommandEventArgs e)
    {
        EnableDisableSelectedValue(true);
        SelectedValue = Char.Parse(e.CommandArgument.ToString());
        panelAToZ.Update();

        if (SelectedChanged != null)
        {
            SelectedChanged(sender, e);
        }
    }
}
