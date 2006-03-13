<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FeaturedAccountsRss.aspx.cs" Inherits="FeaturedAccountsRss" %>

<rss version="2.0">
  <channel>
    <title>Featured People</title>
    <description>all featured people</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Featured People</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(base.GetAccount((int) Eval("DataRowId")).Name) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("DataRowId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountPictureThumbnail.aspx?id=<%# base.GetAccount((int) Eval("DataRowId")).PictureId %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("DataRowId") %>">
              <%# base.Render(base.GetAccount((int) Eval("DataRowId")).Name) %>
             </a>
            </div>
            <div style="color: silver">
             <%# base.GetSummary(base.GetDescription((int) Eval("DataRowId"))) %>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("DataRowId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Account/<%# Eval("DataRowId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
