<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountGroupPlacesRss.aspx.cs"
 Inherits="AccountGroupPlacesRss" %>
<%@ Import Namespace="SnCore.Services" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(PageTitle); %></title>
    <description><% Response.Write(PageDescription); %></description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Group Members</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("PlaceName")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("PlaceId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/PlacePictureThumbnail.aspx?id=<%# Eval("PlacePictureId") %>" />
            </a>
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("PlaceId") %>">
              <%# base.Render(Eval("PlaceName"))%>
             </a>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Place/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
