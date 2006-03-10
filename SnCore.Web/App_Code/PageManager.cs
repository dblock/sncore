using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class PageManager
{
    public PageManager()
    {

    }

    public static void SetDefaultButton(WebControl button, ControlCollection cc)
    {
        foreach (System.Web.UI.Control wc in cc)
        {
            SetDefaultButton(button, wc.Controls);

            if (wc is System.Web.UI.WebControls.WebControl)
            {
                if (wc is System.Web.UI.WebControls.TextBox)
                {
                    TextBox tb = (TextBox)wc;

                    if (tb.TextMode == TextBoxMode.MultiLine)
                        continue;

                    tb.Attributes.Add(
                        "onkeydown",
                        "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" +
                        button.UniqueID + "').click();return false;}} else {return true}; ");
                }
            }
        }
    }
}
