using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using SnCore.Tools.Web;
using System.IO;
using System.Text;

public class ContentPage
{
    public static string GetContent(Uri uri, Uri baseuri)
    {
        return GetContent(uri, baseuri, string.Empty);
    }

    public static string GetContent(Uri uri, Uri baseuri, string note)
    {
        string rawcontent;
        WebRequest request = HttpWebRequest.Create(uri);
        WebResponse response = request.GetResponse();
        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        {
            rawcontent = sr.ReadToEnd();
            sr.Close();
        }

        StringBuilder content = new StringBuilder(Renderer.CleanHtml(rawcontent.ToString(), baseuri, null));

        // hack: lots of header stuff comes first
        // hack: atlas script has a CDATA section and is not cleaned up properly
        int headerstart = content.ToString().IndexOf("<table class=\"sncore_master_table\" align=\"center\">");
        if (headerstart >= 0) content.Remove(0, headerstart);
        int atlasscript = content.ToString().IndexOf("<stripped type=\"text/xml-script\">");
        if (atlasscript >= 0) content.Remove(atlasscript, content.Length - atlasscript);

        content.Insert(0, string.Format("<p style=\"margin: 10px;\"><a href=\"{0}\">&#187;&#187; online version</a></p>", uri.ToString()));

        // insert additional note
        if (!string.IsNullOrEmpty(note))
        {
            content.Insert(0, string.Format("<p style=\"margin: 10px;\">{0}</p>", Renderer.Render(note)));
        }

        // hack: insert stylesheet
        content.Insert(0, string.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/Style.css\" />\n", baseuri));

        return content.ToString();
    }
}
