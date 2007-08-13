<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturedAccountFeedItemsRss.aspx.cs" Inherits="FeaturedAccountFeedItemsRss" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(Name); %></title>
    <description>all featured blogs</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Featured Blogs</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(base.GetAccountFeedItem((int) Eval("DataRowId")).Title) %></title>
       <pubDate><%# base.AdjustToRFC822(base.GetAccountFeedItem((int) Eval("DataRowId")).Created) %></pubDate>
       <description>
        <![CDATA[
         <div>
          <a href="<%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).Link) %>">x-posted</a> 
          by 
          <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# GetAccountFeedItem((int)Eval("DataRowId")).AccountId %>">
           <%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).AccountName) %>
          </a>
          in
          <a href="<% Response.Write(WebsiteUrl); %>/AccountFeedView.aspx?id=<%# GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedId %>">
           <%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedName) %>
          </a>
         </div>
         <div>
          <%# base.GetSummary(GetAccountFeedItem((int)Eval("DataRowId")).Description, GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedLinkUrl) %>
         </div>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountFeedItemView.aspx?id=<%# Eval("DataRowId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountFeedItem/<%# Eval("DataRowId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
