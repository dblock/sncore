<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountStoriesRss.aspx.cs" Inherits="AccountStoriesRss" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(Name); %></title>
    <description>all stories</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Stories</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("Name")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description>
        <![CDATA[<%# base.RenderEx(Eval("Summary")) %>]]>
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountStoryView.aspx?id=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Story/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
