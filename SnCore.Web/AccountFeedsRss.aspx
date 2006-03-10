<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountFeedsRss.aspx.cs" Inherits="AccountFeedsRss" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<rss version="2.0">
  <channel>
    <title>Feeds</title>
    <description>all feeds</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Feeds</title> 
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
            <%# base.Render(Eval("Description")) %>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><%# Eval("FeedUrl") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountFeed/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
