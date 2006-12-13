using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Provider;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using SnCore.Tools.Web;

namespace SnCore.SiteMap
{
    public class SiteMapDataAttributeNode
    {
        public string Title;
        public string Url;

        public SiteMapDataAttributeNode(string title)
        {
            Title = title;
        }

        public SiteMapDataAttributeNode(string title, HttpRequest request, string url)
            : this(title, new Uri(request.Url, url))
        {

        }

        public SiteMapDataAttributeNode(string title, Uri url)
        {
            Title = title;
            Url = url.PathAndQuery + url.Fragment;
        }

        public SiteMapDataAttributeNode(string title, string url)
        {
            Title = title;
            Url = url;
        }
    }

    public class SiteMapDataAttribute : Attribute
    {
        private List<SiteMapDataAttributeNode> mPath = new List<SiteMapDataAttributeNode>();

        public List<SiteMapDataAttributeNode> Path
        {
            get
            {
                return mPath;
            }
        }

        public SiteMapDataAttribute()
        {
        }

        public SiteMapDataAttribute(string title, string uri)
        {
            mPath.Add(new SiteMapDataAttributeNode(title, uri));
        }

        public SiteMapDataAttribute(string title)
        {
            mPath.Add(new SiteMapDataAttributeNode(title));
        }

        public SiteMapDataAttribute(SiteMapDataAttributeNode node)
        {
            mPath.Add(node);
        }

        public SiteMapDataAttribute(SiteMapDataAttributeNode[] path)
        {
            mPath.AddRange(path);
        }

        public void Add(SiteMapDataAttributeNode attribute)
        {
            mPath.Add(attribute);
        }

        public void AddRange(IEnumerable<SiteMapDataAttributeNode> attributes)
        {
            mPath.AddRange(attributes);
        }

        public static IEnumerable<SiteMapDataAttributeNode> GetLocationAttributeNodes(
            HttpRequest request,
            string baseuri,
            string country,
            string state,
            string city,
            string neighborhood,
            string type)
        {
            List<SiteMapDataAttributeNode> sitemapdata = new List<SiteMapDataAttributeNode>();

            if (!string.IsNullOrEmpty(country))
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(country, request, string.Format("{0}?country={1}",
                    baseuri, Renderer.UrlEncode(country))));
            }

            if (!string.IsNullOrEmpty(state))
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(state, request, string.Format("{0}?country={1}&state={2}",
                    baseuri, Renderer.UrlEncode(country), Renderer.UrlEncode(state))));
            }

            if (!string.IsNullOrEmpty(city))
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(city, request, string.Format("{0}?country={1}&state={2}&city={3}",
                    baseuri, Renderer.UrlEncode(country), Renderer.UrlEncode(state), Renderer.UrlEncode(city))));
            }

            if (!string.IsNullOrEmpty(neighborhood))
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(neighborhood, request, string.Format("{0}?country={1}&state={2}&city={3}&neighborhood={4}",
                    baseuri, Renderer.UrlEncode(country), Renderer.UrlEncode(state), Renderer.UrlEncode(city), Renderer.UrlEncode(neighborhood))));
            }

            if (!string.IsNullOrEmpty(type))
            {
                sitemapdata.Add(new SiteMapDataAttributeNode(type, request, string.Format("{0}?country={1}&state={2}&city={3}&neighborhood={4}&type={5}",
                    baseuri, Renderer.UrlEncode(country), Renderer.UrlEncode(state), Renderer.UrlEncode(city), Renderer.UrlEncode(neighborhood), Renderer.UrlEncode(type))));
            }

            return sitemapdata;
        }
    }

    public class SiteMapDataProvider : StaticSiteMapProvider
    {
        private SiteMapNode mRootNode = null;

        public SiteMapDataProvider()
        {
        }

        public override void Initialize(string name, NameValueCollection attributes)
        {
            base.Initialize(name, attributes);
            mRootNode = new SiteMapNode(this, "Home", "Default.aspx", "Home");
            AddNode(mRootNode);
        }

        public override SiteMapNode BuildSiteMap()
        {
            return mRootNode;
        }

        public override SiteMapNode RootNode
        {
            get
            {
                return mRootNode;
            }
        }

        protected override SiteMapNode GetRootNodeCore()
        {
            return RootNode;
        }

        public override SiteMapNode FindSiteMapNode(string rawUrl)
        {
            return base.FindSiteMapNode(rawUrl);
        }

        public SiteMapNode Stack(string title, string uri)
        {
            return Stack(title, uri, mRootNode);
        }

        public SiteMapNode Stack(SiteMapDataAttribute attribute)
        {
            return Stack(attribute, string.Empty);
        }

        public SiteMapNode Stack(SiteMapDataAttribute attribute, string uri)
        {
            SiteMapNode node = RootNode;
            foreach (SiteMapDataAttributeNode s in attribute.Path)
            {
                node = Stack(s.Title, string.IsNullOrEmpty(s.Url) ? uri : s.Url, node);
            }
            return node;
        }

        public SiteMapNode Stack(string title, string uri, SiteMapNode parentnode)
        {
            lock (this)
            {
                SiteMapNode node = base.FindSiteMapNodeFromKey(uri);

                if (node == null)
                {
                    node = new SiteMapNode(this, uri, uri, title);
                    node.ParentNode = ((parentnode == null) ? mRootNode : parentnode);
                    AddNode(node);
                }
                else if (node.Title != title)
                {
                    node.Title = title;
                }
                
                return node;
            }
        }
    }
}
