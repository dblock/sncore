<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountEventsRss.aspx.cs" Inherits="AccountEventsRss" %>

<rss version="2.0">
  <channel>
    <title>Events</title>
    <description>all events</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Events</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("Name")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Modified")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountEventView.aspx?id=<%# Eval("Id") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountEventPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
            </a>        
           </td>
           <td>
            <div>
             at
             <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("PlaceId") %>">
              <%# base.Render(Eval("PlaceName")) %>             
             </a>        
            </div>
            <div>
             <%# base.Render(Eval("Schedule")) %>      
            </div>
            <div style="color: silver">
             <%# base.Render(Eval("PlaceCity")) %>, 
             <%# base.Render(Eval("PlaceState")) %>, 
             <%# base.Render(Eval("PlaceCountry")) %>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountEventView.aspx?id=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountEvent/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
