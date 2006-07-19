<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountContentGroupViewRss.aspx.cs" Inherits="AccountContentGroupRss" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(Name); %></title>
    <description><% Response.Write(base.Render(ContentGroup.Description)); %></description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title><% Response.Write(base.Render(ContentGroup.Name)); %></title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width>
     <height>100</height> 
    </image>
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("Tag")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Modified")) %></pubDate>
       <author><% Response.Write(base.Render(ContentGroup.AccountName)); %></author>
       <description>
        <![CDATA[<%# ((bool) Eval("AccountContentGroupTrusted")) ? Eval("Text") : base.RenderEx(Eval("Text")) %>]]>
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountContentGroupView.aspx?id=<%# Eval("AccountContentGroupId") %>&amp;cid=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/ContentGroup/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
