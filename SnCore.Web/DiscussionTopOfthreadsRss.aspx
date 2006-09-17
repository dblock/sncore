<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DiscussionTopOfThreadsRss.aspx.cs" Inherits="DiscussionTopOfThreadsRss" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(RssTitle); %></title>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>New Discussion Threads</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("Subject")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description><![CDATA[<%# base.RenderEx(Eval("Body")) %>]]></description>
       <category><%# base.Render(Eval("DiscussionName")) %></category>
       <link><% Response.Write(WebsiteUrl); %>/DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&amp;id=<%# Eval("DiscussionThreadId")%></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Discussion/<%# Eval("DiscussionId") %>/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
