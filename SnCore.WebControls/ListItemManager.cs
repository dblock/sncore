using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SnCore.WebControls
{
    public abstract class ListItemManager
    {
        public static ListItem SelectAdd(ListControl list, object value)
        {
            return SelectAdd(list, value.ToString(), value.ToString());
        }

        public static ListItem SelectAdd(ListControl list, object text, object value)
        {
            return SelectAdd(list, text.ToString(), value.ToString());
        }

        public static ListItem SelectAdd(ListControl list, string textvalue)
        {
            return SelectAdd(list, textvalue, textvalue);
        }

        public static ListItem SelectAdd(ListControl list, string text, string value)
        {
            ListItem li = list.Items.FindByValue(value);
            if (li == null)
            {
                li = new ListItem(text, value);
                list.Items.Add(li);
            }
            list.ClearSelection();
            li.Selected = true;
            return li;
        }

        public static bool TrySelect(ListControl list, object textvalue)
        {
            return TrySelect(list, textvalue, textvalue.ToString());
        }

        public static bool TrySelect(ListControl list, string textvalue)
        {
            return TrySelect(list, textvalue, textvalue);
        }

        public static bool TrySelect(ListControl list, object text, object value)
        {
            return TrySelect(list, text.ToString(), value.ToString());
        }

        public static bool TrySelect(ListControl list, string text, string value)
        {
            ListItem li = null;
            return TrySelect(list, text, value, ref li);
        }

        public static bool TrySelect(ListControl list, string text, string value, ref ListItem li)
        {
            li = null;
            if (string.IsNullOrEmpty(value)) return false;
            li = list.Items.FindByValue(value);
            if (li != null)
            {
                list.ClearSelection();
                li.Selected = true;
            }
            return (li != null);
        }
    }
}
