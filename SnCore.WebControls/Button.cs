using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace SnCore.WebControls
{
    public class Button : System.Web.UI.WebControls.Button
    {
        public Button()
        {
                        
        }

        protected override void OnLoad(EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("if (typeof(Page_ClientValidate) == 'function') { ");
            sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
            sb.Append("this.value = 'Working';");
            sb.Append("this.disabled = true;");
            sb.Append(Page.ClientScript.GetPostBackEventReference(this, string.Empty));
            sb.Append(";");
            this.Attributes.Add("onclick", sb.ToString()); 

            base.OnLoad(e);
        }
    }
}
