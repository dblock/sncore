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
using SnCore.Services;
using SnCore.WebServices;
using SnCore.SiteMap;
using SnCore.Data.Hibernate;
using System.Text.RegularExpressions;
using SnCore.Tools.Web.Html;

public partial class AccountStoryEdit : AuthenticatedPage
{
    public void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Me Me", Request, "AccountManage.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Stories", Request, "AccountStoriesManage.aspx"));

            DomainClass cs = SessionManager.GetDomainClass("AccountStory");
            inputName.MaxLength = cs["Name"].MaxLengthInChars;

            if (RequestId > 0)
            {
                TransitAccountStory ts = SessionManager.StoryService.GetAccountStoryById(
                    SessionManager.Ticket, RequestId);

                inputName.Text = ts.Name;
                inputSummary.Content = ts.Summary;
                inputPublish.Checked = ts.Publish;

                linkAddPictures.NavigateUrl = string.Format("AccountStoryPicturesManage.aspx?id={0}", ts.Id);
                linkView.NavigateUrl = string.Format("AccountStoryView.aspx?id={0}", ts.Id);

                labelTitle.Text = "Edit Story";
                sitemapdata.Add(new SiteMapDataAttributeNode(ts.Name, Request.Url));

                labelLastSaved.Text = string.Format("Last saved: {0}", Adjust(ts.Modified));
            }
            else
            {
                linkAddPictures.Visible = false;
                sitemapdata.Add(new SiteMapDataAttributeNode("New Story", Request.Url));
            }

            StackSiteMap(sitemapdata);

            GetImagesData(sender, e);
        }

        if (!SessionManager.HasVerified())
        {
            ReportWarning("You don't have any verified e-mail addresses and/or profile photos.\n" +
                "You must add/confirm a valid e-mail address and upload a profile photo before posting stories.");

            linkSave.Enabled = false;
        }

        SetDefaultButton(linkSave);
    }

    public void GetImagesData(object sender, EventArgs e)
    {
        gridManage.DataSource = SessionManager.StoryService.GetAccountStoryPictures(SessionManager.Ticket, RequestId, null);
        gridManage.DataBind();
    }

    public void gridManage_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int id = int.Parse(e.CommandArgument.ToString());

        switch (e.CommandName)
        {
            case "Delete":
                SessionManager.Delete<TransitAccountStoryPicture>(id, SessionManager.StoryService.DeleteAccountStoryPicture);
                ReportInfo("Image deleted.");
                GetImagesData(source, e);
                break;
            case "Up":
                SessionManager.StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, -1);
                GetImagesData(source, e);
                break;
            case "Down":
                SessionManager.StoryService.MoveAccountStoryPicture(SessionManager.Ticket, id, 1);
                GetImagesData(source, e);
                break;
            case "Insert":
                inputSummary.Content = inputSummary.Content +
                        string.Format("<P><IMG SRC=\"AccountStoryPicture.aspx?id={0}\" WIDTH=\"250\" /></P>", id);
                break;
        }
    }

    private int saveOnly()
    {
        TransitAccountStory s = new TransitAccountStory();
        s.Name = inputName.Text;
        s.Summary = inputSummary.Content;
        s.Publish = inputPublish.Checked;
        s.Id = RequestId;
        s.Id = SessionManager.CreateOrUpdate<TransitAccountStory>(
            s, SessionManager.StoryService.CreateOrUpdateAccountStory);
        labelLastSaved.Text = string.Format("Last saved: {0}", Adjust(DateTime.UtcNow));
        return s.Id;
    }

    public void saveAndContinue(object sender, EventArgs e)
    {
        int id = saveOnly();
        Redirect(string.Format("AccountStoryEdit.aspx?id={0}", id));
    }

    public void save(object sender, EventArgs e)
    {
        int id = saveOnly();
        Redirect(string.Format("AccountStoryView.aspx?id={0}", id));
    }

    public void clean(object sender, EventArgs e)
    {
        inputSummary.Content = Renderer.CleanHtml(inputSummary.Content);
        inputSummary.Content = Renderer.RemoveHtml(inputSummary.Content);
        inputSummary.Content = Regex.Replace(inputSummary.Content, @"([\s*][\r\n]+[\s]*)(?<text>[^\r\n]*)", "<p>${text}</p>");
    }

    public void linkSummarize_Click(object sender, EventArgs e)
    {
        List<HtmlImage> list = HtmlImageExtractor.Extract(inputSummary.Content);
        string imageuri = string.Empty;
        if (list.Count > 0)
        {
            imageuri = list[0].Src;
            imageuri = imageuri.Replace("AccountStoryPicture.aspx", "AccountStoryPictureThumbnail.aspx");
        }
        else
        {
            TransitAccountStory ts = SessionManager.StoryService.GetAccountStoryById(
                SessionManager.Ticket, RequestId);

            imageuri = string.Format("AccountStoryPictureThumbnail.aspx?id={0}", ts.AccountStoryPictureId);
        }
        labelSummary.Text = string.Format(
            "<table cellpadding='4' cellspacing='4'>\n" +
             "<tr>\n" +
              "<td valign='middle'>\n" +
               "<a href='{2}'><img border='0' src='{0}'></a>\n" +
              "</td>\n" +
              "<td valign='middle'>\n" +
               "<p><a href='{2}'>{3}</a></p>\n" +
               "<p>{1}</p>\n" +
              "</td>\n" +
             "</tr>\n" +
            "</table>",
            imageuri,
            Renderer.GetSummary(inputSummary.Content),
            string.Format("AccountStoryView.aspx?id={0}", RequestId),
            Renderer.Render(inputName.Text));
    }
}
