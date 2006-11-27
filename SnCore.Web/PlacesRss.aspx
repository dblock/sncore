<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlacesRss.aspx.cs" Inherits="PlacesRss" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(RssTitle); %></title>
    <description>all places</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Places</title> 
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
            <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("Id") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("Id") %>">
              <%# base.Render(Eval("Name")) %>
             </a>
            </div>
            <div style="color: silver">
             <%# base.Render(Eval("Neighborhood")) %>, 
            </div>
            <div style="color: silver">
             <%# base.Render(Eval("City")) %>, 
             <%# base.Render(Eval("State")) %>, 
             <%# base.Render(Eval("Country")) %>
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
