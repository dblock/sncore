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

public partial class AccountPicture2 : AccountPicturePage
{
    public override PicturePage.PicturePageType PageType
    {
        get
        {
            return PicturePageType.Bitmap;
        }
    }
}
