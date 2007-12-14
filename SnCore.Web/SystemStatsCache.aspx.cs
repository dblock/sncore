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
using SnCore.SiteMap;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

public partial class SystemStatsCache : AuthenticatedPage
{
    private class CacheEntry
    {
        private string mName;

        public string Name
        {
            get
            {
                return mName;
            }
            set
            {
                mName = value;
            }
        }

        private string mType;

        public string Type
        {
            get
            {
                return mType;
            }
            set
            {
                mType = value;
            }
        }

        private string mValue;

        public string Value
        {
            get
            {
                return mValue;
            }
            set
            {
                mValue = value;
            }
        }

        private long mSize;

        public long Size
        {
            get
            {
                return mSize;
            }
            set
            {
                mSize = value;
            }
        }

        private int mCount;

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

        public CacheEntry()
        {

        }

        public CacheEntry(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public static int CompareByName(CacheEntry left, CacheEntry right)
        {
            return left.Name.CompareTo(right.Name);
        }

        public static int CompareByCount(CacheEntry left, CacheEntry right)
        {
            return right.Count.CompareTo(left.Count);
        }

        public static int CompareBySize(CacheEntry left, CacheEntry right)
        {
            return right.Size.CompareTo(left.Size);
        }
    }

    private static long SizeOfObject(object o)
    {
        MemoryStream memoryStream = new MemoryStream();
        StreamWriter sw = new StreamWriter(memoryStream);
        sw.Write(o);
        sw.Flush();
        return memoryStream.Length;
    }

    public void Page_Load(object sender, EventArgs e)
    {
        if (!SessionManager.IsAdministrator)
        {
            ReportWarning("This page is only available to the system administrator.");
            return;
        }

        gridCache.OnGetDataSource += new EventHandler(gridCache_OnGetDataSource);
        gridRolledUpCache.OnGetDataSource += new EventHandler(gridRolledUpCache_OnGetDataSource);

        if (!IsPostBack)
        {
            SiteMapDataAttribute sitemapdata = new SiteMapDataAttribute();
            sitemapdata.Add(new SiteMapDataAttributeNode("Statistics", Request, "SystemStatsHits.aspx"));
            sitemapdata.Add(new SiteMapDataAttributeNode("Cache", Request.Url));
            StackSiteMap(sitemapdata);

            GetData(sender, e);
        }
    }

    void gridRolledUpCache_OnGetDataSource(object sender, EventArgs e)
    {
        Dictionary<string, CacheEntry> si = new Dictionary<string, CacheEntry>();

        IDictionaryEnumerator enumerator = SessionManager.Cache.GetEnumerator();
        while (enumerator.MoveNext())
        {
            // table by type
            string typename = enumerator.Value.GetType().Name;
            CacheEntry hashentry = null;
            if (!si.TryGetValue(typename, out hashentry))
            {
                hashentry = new CacheEntry();
                hashentry.Type = typename;
                string key = enumerator.Key.ToString();
                if (key.Length > 48) key = key.Substring(0, 48) + " ...";
                hashentry.Name = key;
                hashentry.Count = 1;
                hashentry.Size = SizeOfObject(enumerator.Value);
                si.Add(typename, hashentry);
            }
            else
            {
                hashentry.Count++;
                hashentry.Size += SizeOfObject(enumerator.Value);
            }
        }

        CacheEntry[] arr = new CacheEntry[si.Values.Count];
        si.Values.CopyTo(arr, 0);
        Array.Sort(arr, CacheEntry.CompareBySize);
        gridRolledUpCache.DataSource = arr;
    }

    void gridCache_OnGetDataSource(object sender, EventArgs e)
    {
        List<CacheEntry> si = new List<CacheEntry>();

        IDictionaryEnumerator enumerator = SessionManager.Cache.GetEnumerator();
        while (enumerator.MoveNext())
        {
            // entry
            string key = enumerator.Key.ToString();
            if (key.Length > 24) key = key.Substring(0, 24) + " ...";
            CacheEntry entry = new CacheEntry();
            entry.Name = key;
            entry.Type = enumerator.Value.GetType().Name;
            entry.Size = SizeOfObject(enumerator.Value);
            si.Add(entry);
        }

        si.Sort(CacheEntry.CompareByName);
        gridCache.DataSource = si;
    }

    private void GetData(object sender, EventArgs e)
    {
        labelCacheDescription.Text = string.Format("Cache size: {0} element(s)", SessionManager.Cache.Count.ToString());

        gridCache.CurrentPageIndex = 0;
        gridCache_OnGetDataSource(sender, e);
        gridCache.DataBind();

        gridRolledUpCache.CurrentPageIndex = 0;
        gridRolledUpCache_OnGetDataSource(sender, e);
        gridRolledUpCache.DataBind();
    }

    public void linkFlush_Click(object sender, EventArgs e)
    {
        // clear session cache
        IDictionaryEnumerator enumerator = SessionManager.Cache.GetEnumerator();
        while (enumerator.MoveNext())
        {
            SessionManager.Cache.Remove(enumerator.Key.ToString());
        }

        // clear navigation crumbs
        if (SiteMap.Provider is SiteMapDataProvider)
        {
            SiteMapDataProvider provider = (SiteMapDataProvider)SiteMap.Provider;
            provider.RemoveAll();
        }

        GetData(sender, e);
    }
}
