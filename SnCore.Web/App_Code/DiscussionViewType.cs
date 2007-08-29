using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public enum DiscussionViewTypes
{
    ThreadedWithNewestOnTop = 0,
    ThreadedFullWithNewestOnTop,
    FlatWithNewestOnTop,
    FlatFullWithNewestOnTop,
}

public class DiscussionViewType
{
    private DiscussionViewTypes mView;

    public DiscussionViewTypes View
    {
        get
        {
            return mView;
        }
    }

    private string mDescription;

    public string Description
    {
        get
        {
            return mDescription;
        }
    }

    public DiscussionViewType(DiscussionViewTypes view, string description)
    {
        mView = view;
        mDescription = description;
    }

    public static DiscussionViewType[] DefaultTypes = {
        new DiscussionViewType(DiscussionViewTypes.ThreadedWithNewestOnTop, "threaded, newest on top"),
        new DiscussionViewType(DiscussionViewTypes.ThreadedFullWithNewestOnTop, "threaded, full posts, newest on top"),
        new DiscussionViewType(DiscussionViewTypes.FlatWithNewestOnTop, "flat, newest on top"),
        new DiscussionViewType(DiscussionViewTypes.FlatFullWithNewestOnTop, "flat, full posts, newest on top"),
    };
}

