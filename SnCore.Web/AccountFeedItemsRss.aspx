<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountFeedItemsRss.aspx.cs" Inherits="AccountFeedItemsRss" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<rss version="2.0">
  <channel>
    <title>Feed Items</title>
    <description>all feed items</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Feed Items</title> 
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
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("AccountId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
            </a>
            <div>
             <%# base.Render(Eval("AccountName")) %>
            </div>            
           </td>
           <td>
            <%# Renderer.CleanHtml(Eval("Description")) %>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><%# Eval("Link") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountFeedItem/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
