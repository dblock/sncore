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
using Wilco.Web.UI;
using SnCore.Services;
using System.Collections.Generic;
using System.Text;
using SnCore.WebServices;
using SnCore.Tools.Web;

public partial class MadLibInstancesViewControl : Control
{
    public int ObjectId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "ObjectId", 0);
        }
        set
        {
            ViewState["ObjectId"] = value;
        }
    }

    public int MadLibId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "MadLibId", 0);
        }
        set
        {
            ViewState["MadLibId"] = value;
        }
    }

    public string Table
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "Table", string.Empty);
        }
        set
        {
            ViewState["Table"] = value;
        }
    }

    public string ReturnUrl
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<string>(ViewState, "ReturnUrl", string.Empty);
        }
        set
        {
            ViewState["ReturnUrl"] = value;
        }
    }

    public void Page_Load(object sender, EventArgs e)
    {
        try
        {
            madlibs.OnGetDataSource += new EventHandler(madlibs_OnGetDataSource);
            if (!IsPostBack)
            {
                linkNew.NavigateUrl = string.Format("AccountMadLibInstanceEdit.aspx?ObjectName=Place&oid={0}&mid={1}&ReturnUrl={2}",
                    ObjectId, MadLibId, Renderer.UrlEncode(ReturnUrl));
                GetData(sender, e);
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    void madlibs_OnGetDataSource(object sender, EventArgs e)
    {
        ServiceQueryOptions options = new ServiceQueryOptions();
        options.PageNumber = madlibs.CurrentPageIndex;
        options.PageSize = madlibs.PageSize;
        object[] args = { Table, ObjectId, options };
        madlibs.DataSource = SessionManager.GetCachedCollection<TransitMadLibInstance>(
            MadLibService, "GetMadLibInstances", args);
    }

    public void GetData(object sender, EventArgs e)
    {
        madlibs.CurrentPageIndex = 0;
        object[] args = { Table, ObjectId };
        madlibs.VirtualItemCount = SessionManager.GetCachedCollectionCount(
            MadLibService, "GetMadLibInstancesCount", args);
        madlibs_OnGetDataSource(sender, e);
        madlibs.DataBind();
    }

    public string GetCssClass(DateTime ts)
    {
        return (ts.AddDays(5) < DateTime.UtcNow) ? "sncore_message" : "sncore_new_message";
    }

    public string GetEditUrl(int id)
    {
        return string.Format("AccountMadLibInstanceEdit.aspx?ObjectName={0}&oid={1}&mid={2}&id={3}&ReturnUrl={4}",
            Table, ObjectId, MadLibId, id, Renderer.UrlEncode(ReturnUrl));
    }

    public void madlibs_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            switch (e.CommandName)
            {
                case "Delete":
                    {
                        int id = int.Parse(e.CommandArgument.ToString());
                        MadLibService.DeleteMadLibInstance(SessionManager.Ticket, id);
                        GetData(source, e);
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }
}
