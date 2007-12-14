<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionRssControl.ascx.cs"
 Inherits="DiscussionRssControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Repeater id="rssRepeater" runat="server">
 <ItemTemplate>
  <item>
   <title><%# base.Render(Eval("Subject")) %></title>
   <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
   <author><%# base.Render(Eval("AccountName")) %></author>
   <category><%# base.Render(Eval("DiscussionName")) %></category>
   <description>
    <![CDATA[<%# base.RenderEx(Eval("Body")) %>]]>
   </description>
   <link><% Response.Write(WebsiteUrl); %>/DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&amp;id=<%# Eval("DiscussionThreadId")%></link>
   <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Discussion/<%# Eval("DiscussionId") %>/<%# Eval("Id") %></guid>
  </item>
 </ItemTemplate>
</asp:Repeater>
