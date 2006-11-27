<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturedAccountEventsRss.aspx.cs" Inherits="FeaturedAccountEventsRss" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(Name); %></title>
    <description>all featured events</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Featured Events</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(base.GetAccountEvent((int) Eval("DataRowId")).Name) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountEventView.aspx?id=<%# Eval("DataRowId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountEventPictureThumbnail.aspx?id=<%# base.GetAccountEvent((int) Eval("DataRowId")).PictureId %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/AccountEventView.aspx?id=<%# Eval("DataRowId") %>">
              <%# base.Render(base.GetAccountEvent((int) Eval("DataRowId")).Name) %>
             </a>
            </div>            
            <div style="color: silver">
             at 
             <a href='<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# base.GetAccountEvent((int)Eval("DataRowId")).PlaceId %>'><%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).PlaceName) %></a>
            </div>
            <div style="color: silver">
             <%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).Schedule) %>
            </div>
            <div style="color: silver">
             <%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).PlaceNeighborhood) %>
             <%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).PlaceCity) %>
             <%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).PlaceState) %>
             <%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).PlaceCountry) %>
            </div>
            <div style="color: silver">
             <%# base.Render(base.GetAccountEvent((int)Eval("DataRowId")).Description) %>
            </div>            
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountEventView.aspx?id=<%# Eval("DataRowId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountEvent/<%# Eval("DataRowId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
