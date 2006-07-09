using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace SnCore.WebControls
{
    public class PersistentPanel : System.Web.UI.WebControls.Panel
    {
        public new bool Visible
        {
            set
            {
                PersistentVisible = value;
            }
        }

        public bool PersistentVisible
        {
            get
            {
                return string.IsNullOrEmpty(Attributes["style"]);
            }
            set
            {
                Attributes["style"] = value ? string.Empty : "display: none;";
            }
        }
    }
}
