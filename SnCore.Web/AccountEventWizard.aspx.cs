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
using Wilco.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SnCore.Tools.Drawing;
using SnCore.Tools.Web;
using SnCore.Tools.Web.Html;
using SnCore.WebServices;
using SnCore.Services;
using System.Collections.Specialized;
using Rss;
using Atom.Core;
using System.Net;
using Wilco.Web.UI;
using SnCore.SiteMap;

public partial class AccountEventWizard : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountPreferencesManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Events", Request, "AccountEventsManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Wizard", Request.Url));
            StackSiteMap(sitemapdata);

            inputLinkUrl.Text = Request.QueryString["url"];

            if (!string.IsNullOrEmpty(inputLinkUrl.Text))
            {
                discover_Click(sender, e);
            }
        }

        SetDefaultButton(linkDiscover);
    }

    protected HttpWebRequest GetEventHttpRequest(string url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        System.Net.ServicePointManager.Expect100Continue = false;
        request.UserAgent = SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0");
        request.Timeout = 60 * 1000;
        request.KeepAlive = false;
        request.MaximumAutomaticRedirections = 5;
        return request;
    }

    protected Stream GetEventStream(string url)
    {
        return GetEventHttpRequest(url).GetResponse().GetResponseStream();
    }

    public void discover_Click(object sender, EventArgs e)
    {
        if (!Uri.IsWellFormedUriString(inputLinkUrl.Text, UriKind.Absolute))
            inputLinkUrl.Text = "http://" + inputLinkUrl.Text;

        List<TransitAccountEvent> events = discoverRel(inputLinkUrl.Text);

        if (events.Count == 0)
        {
            ReportInfo("Sorry, I couldn't find any iCalendar meta data on this page. <a href='AccountFeedEdit.aspx'>Click here</a> to create an event.");
            return;
        }

        gridEvents.DataSource = events;
        gridEvents.DataBind();
    }

    protected List<TransitAccountEvent> discoverRel(string url)
    {
        List<TransitAccountEvent> result = new List<TransitAccountEvent>();
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        request.UserAgent = SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0");
        WebResponse response = request.GetResponse();
        string content;
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            content = sr.ReadToEnd();
            sr.Close();
        }

        List<HtmlLinkControl> links = HtmlLinkExtractor.Extract(content, new Uri(url));

        foreach (HtmlLinkControl link in links)
        {
            try
            {
                switch (link.Type.ToLower())
                {
                    case "text/calendar":
                        TransitAccountEventICALEmitter emitter = TransitAccountEventICALEmitter.Parse(
                            link.Href, SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0"));
                        emitter.AccountEvent.Website = link.Href;
                        AddUnique(result, emitter.AccountEvent);
                        break;
                }
            }
            catch
            {
            }
        }

        List<Uri> webcallinks = HtmlUriExtractor.Extract(content, new Uri(url));

        foreach (Uri link in webcallinks)
        {
            try
            {
                if (link.Scheme.ToLower() == "webcal" || link.GetLeftPart(UriPartial.Path).EndsWith(".ics"))
                {
                    TransitAccountEventICALEmitter emitter = TransitAccountEventICALEmitter.Parse(
                        link.ToString(), SessionManager.GetCachedConfiguration("SnCore.Web.UserAgent", "SnCore/1.0"));
                    emitter.AccountEvent.Website = link.ToString();
                    AddUnique(result, emitter.AccountEvent);
                }
            }
            catch
            {

            }
        }

        return result;
    }

    private static bool AddUnique(List<TransitAccountEvent> list, TransitAccountEvent evt)
    {
        foreach (TransitAccountEvent existing in list)
        {
            if (existing.Website == evt.Website)
                return false;
            if (existing.Name == evt.Name)
                return false;
        }

        list.Add(evt);
        return true;
    }
}
