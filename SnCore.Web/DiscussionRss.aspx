<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DiscussionRss.aspx.cs" Inherits="DiscussionRss" %>
<%@ Register TagPrefix="SnCore" TagName="DiscussionRss" Src="DiscussionRssControl.ascx" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(RssTitle); %></title>
    <description><% Response.Write(Description); %></description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Discussion</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <SnCore:DiscussionRss runat="server" ID="discussionRss" />
  </channel>
</rss>
