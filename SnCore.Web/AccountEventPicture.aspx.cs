using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class AccountEventPicture2 : AccountEventPicturePage
{
    public override PicturePageType PageType
    {
        get
        {
            return PicturePageType.Bitmap;
        }
    }
}
