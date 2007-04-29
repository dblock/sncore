<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountFeedRss.aspx.cs" Inherits="AccountFeedRss" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(base.Render(AccountFeed.Name)); %></title>
    <description><% Response.Write(base.Render(AccountFeed.Description)); %></description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Feed</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("Title")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description>
        <![CDATA[
         <div>
          <a href="<%# base.Render(Eval("Link")) %>">x-posted</a> 
          by 
          <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("AccountId") %>">
           <%# base.Render(Eval("AccountName")) %>
          </a>
          in
          <a href="<% Response.Write(WebsiteUrl); %>/AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>">
           <%# base.Render(Eval("AccountFeedName")) %>
          </a>
         </div>
         <div>
          <%# base.GetSummary((string) Eval("Description"), (string) Eval("AccountFeedLinkUrl")) %>
         </div>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountFeedItemView.aspx?id=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountFeedItem/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
