<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturedPlacesRss.aspx.cs" Inherits="FeaturedPlacesRss" %>

<rss version="2.0">
  <channel>
    <title>Featured Places</title>
    <description>all featured places</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Featured Places</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(base.GetPlace((int) Eval("DataRowId")).Name) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("DataRowId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/PlacePictureThumbnail.aspx?id=<%# base.GetPlace((int) Eval("DataRowId")).PictureId %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("DataRowId") %>">
              <%# base.Render(base.GetPlace((int) Eval("DataRowId")).Name) %>
             </a>
            </div>
            <div style="color: silver">
             <%# base.Render(GetSummary(base.GetPlace((int) Eval("DataRowId")).Description)) %>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("DataRowId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Place/<%# Eval("DataRowId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
