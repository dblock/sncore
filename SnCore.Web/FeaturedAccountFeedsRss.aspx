<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturedAccountFeedsRss.aspx.cs" Inherits="FeaturedAccountFeedsRss" %>

<rss version="2.0">
  <channel>
    <title>Featured Feeds</title>
    <description>all featured feeds</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Featured Feeds</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(base.GetAccountFeed((int) Eval("DataRowId")).Name) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountFeedView.aspx?id=<%# Eval("DataRowId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountPictureThumbnail.aspx?id=<%# base.GetAccountFeed((int) Eval("DataRowId")).AccountPictureId %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/AccountFeedView.aspx?id=<%# Eval("DataRowId") %>">
              <%# base.Render(base.GetAccountFeed((int) Eval("DataRowId")).Name) %>
             </a>
            </div>
            <div style="color: silver">
             <%# base.Render(base.GetAccountFeed((int)Eval("DataRowId")).Description)%>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountFeedView.aspx?id=<%# Eval("DataRowId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountFeed/<%# Eval("DataRowId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
