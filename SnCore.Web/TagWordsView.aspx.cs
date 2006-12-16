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
using SnCore.Tools.Web;
using SnCore.Services;
using SnCore.WebServices;
using System.Collections.Generic;
using System.Web.Caching;
using SnCore.SiteMap;

[SiteMapDataAttribute("Tags")]
public partial class TagWordsView : AccountPersonPage
{
    private int mCount = 200;
    private int mMinFrequency = -1;
    private int mMaxFrequency = -1;
    private static int minFontSize = 12;
    private static int maxFontSize = 28;
    private Random mRandomSort = new Random(0);

    public int Count
    {
        get
        {
            return mCount;
        }
        set
        {
            mCount = value;
        }
    }

    public int MaxFrequency
    {
        get
        {
            if (mMaxFrequency == -1)
            {
                object result = Cache[string.Format("maxfrequency:{0}", ClientID)];
                mMaxFrequency = (result == null) ? 0 : (int)result;
            }
            return mMaxFrequency;
        }
        set
        {
            Cache.Insert(string.Format("maxfrequency:{0}", ClientID),
                value, null, Cache.NoAbsoluteExpiration, SessionManager.DefaultCacheTimeSpan);
        }
    }

    public int MinFrequency
    {
        get
        {
            if (mMinFrequency == -1)
            {
                object result = Cache[string.Format("minfrequency:{0}", ClientID)];
                mMinFrequency = (result == null) ? 0 : (int)result;
            }
            return mMinFrequency;
        }
        set
        {
            Cache.Insert(string.Format("minfrequency:{0}", ClientID),
                value, null, Cache.NoAbsoluteExpiration, SessionManager.DefaultCacheTimeSpan);
        }
    }

    public int CompareByFrequency(TransitTagWord left, TransitTagWord right)
    {
        if (left == right || left.Id == right.Id)
            return 0;

        if (Math.Min(left.Frequency, right.Frequency) < mMinFrequency || mMinFrequency == -1)
            mMinFrequency = Math.Min(left.Frequency, right.Frequency);

        if (Math.Max(left.Frequency, right.Frequency) > mMaxFrequency || mMaxFrequency == -1)
            mMaxFrequency = Math.Max(left.Frequency, right.Frequency);

        return mRandomSort.Next(-Count, Count);
    }

    public void Page_Load()
    {
        try
        {
            if (!IsPostBack)
            {
                ServiceQueryOptions options = new ServiceQueryOptions();
                options.PageNumber = 0;
                options.PageSize = Count;
                object[] args = { TransitTagWordQueryOptions.Promoted, options };
                List<TransitTagWord> words = SessionManager.GetCachedCollection<TransitTagWord>(
                    SessionManager.TagWordService, "GetTagWords", args);
                words.Sort(CompareByFrequency);
                MaxFrequency = mMaxFrequency;
                MinFrequency = mMinFrequency;
                tagwords.DataSource = words;
                tagwords.DataBind();
            }
        }
        catch (Exception ex)
        {
            ReportException(ex);
        }
    }

    public int GetFontSize(int frequency)
    {
        int frequencyDelta = MaxFrequency - MinFrequency;
        if (frequencyDelta <= 0)
            return minFontSize;

        int fontDelta = maxFontSize - minFontSize;
        return (int)(minFontSize + (frequency * fontDelta) / frequencyDelta);
    }
}
