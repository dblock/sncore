<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountFeedItemMediasRss.aspx.cs"
 Inherits="AccountFeedItemMediasRss" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(Name); %></title>
    <description>all syndicated media</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Media</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("AccountFeedItemTitle")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Modified")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <%# Renderer.CleanHtml(Eval("EmbeddedHtml")) %>
           </td>
           <td>
            <div>
             x-posted in 
             <a href="AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>">
              <%# base.Render(Eval("AccountFeedName")) %>
             </a>
            </div>
            <div>
             <a href="AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %>">
              <%# base.Render(Eval("AccountFeedItemTitle")) %>
             </a>    
            </div>
           </td>
          </tr>
         </table>
        ]]>
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountFeedItemMedia/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
