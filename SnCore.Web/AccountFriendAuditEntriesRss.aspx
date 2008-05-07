<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountFriendAuditEntriesRss.aspx.cs" Inherits="AccountFriendAuditEntriesRss" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(RssTitle); %></title>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Friends Activity</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("AccountName"))%></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Updated")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("AccountId") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
            </a>
           </td>
           <td>
            <div>
             <%# RenderEx(Eval("Description")) %>
            </div>
           </td>
          </tr>
         </table>       
        ]]>       
       </description>
       <link><![CDATA[<% Response.Write(WebsiteUrl); %>/<%# Eval("Url") %>]]></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/AccountAuditEntry/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
