using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using SnCore.Services;
using SnCore.Tools.Web;
using SnCore.SiteMap;
using SnCore.Tools;
using System.ComponentModel; 

[SiteMapDataAttribute("Facebook Graph")]
public partial class FacebookGraph : Page
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FacebookPageManager facebook = new FacebookPageManager(SessionManager);
            if (string.IsNullOrEmpty(facebook.FacebookAPIKey))
            {
                throw new Exception("This site is not configured for Facebook login: missing Facebook API key.");
            }

            if (! facebook.HasFacebookCookies(HttpContext.Current.Request.Cookies))
            {
                Redirect(facebook.GetLoginUrl(Request.Url.ToString()));
            }

            SortedList<string, string> facebookCookies = facebook.GetFacebookCookies(HttpContext.Current.Request.Cookies);
            List<String> cookieNames = new List<String>(facebookCookies.Keys);
            List<String> cookieValues = new List<String>(facebookCookies.Values);

            Facebook.Schema.user user = SessionManager.FacebookGraphService.GetUser(
                cookieNames.ToArray(), cookieValues.ToArray());

            facebookUser.SelectedObject = user;
            facebookUserStatus.SelectedObject = user.status;
            facebookUserHometownLocation.SelectedObject = user.hometown_location;
            facebookUserCurrentLocation.SelectedObject = user.current_location;
        }
    }
}
