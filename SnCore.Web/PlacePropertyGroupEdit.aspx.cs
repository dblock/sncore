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
using SnCore.WebServices;
using SnCore.Services;
using System.Text;

public partial class PlacePropertyGroupEdit : AuthenticatedPage
{
    public int PlaceId
    {
        get
        {
            return GetId("pid");
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetDefaultButton(save);

            if (!IsPostBack)
            {
                ppg.PlaceId = PlaceId;
                ppg.PlacePropertyGroupId = RequestId;
                ppg.DataBind();

                linkBack.NavigateUrl = string.Format("PlaceEdit.aspx?id={0}", PlaceId);

                TransitPlace tp = PlaceService.GetPlaceById(PlaceId);

                if (RequestId > 0)
                {
                    TransitPlacePropertyGroup tag = PlaceService.GetPlacePropertyGroupById(RequestId);
                    labelName.Text = string.Format("{0}: {1}", Render(tp.Name), Render(tag.Name));
                    labelDescription.Text = Render(tag.Description);
                }
                else
                {
                    labelName.Text = string.Format("{0}: All Property Groups", Render(tp.Name));
                }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public void save_Click(object sender, EventArgs e)
    {
        try
        {
            ppg.save_Click(sender, e);
            Redirect(linkBack.NavigateUrl);
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
