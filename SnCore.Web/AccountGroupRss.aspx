<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountGroupRss.aspx.cs"
 Inherits="AccountGroupRss" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Import Namespace="SnCore.Services" %>
<rss version="2.0">
  <channel>
    <title><% Response.Write(RssTitle); %></title>
    <description><% Response.Write(Renderer.Render(AccountGroup.Description)); %></description>
    <link><% Response.Write(string.Format("{0}/AccountGroupView.aspx?id={1}", SessionManager.WebsiteUrl, AccountGroup.Id)); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Group</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeaterDiscussion" runat="server">
     <ItemTemplate>
      <item>
       <title>New Discussion Post: <%# base.Render(Eval("Subject")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description>
        <![CDATA[<%# base.RenderEx(Eval("Body")) %>]]>
       </description>
       <category>Group Discussion Posts</category>
       <link><% Response.Write(WebsiteUrl); %>/DiscussionThreadView.aspx?did=<%# Eval("DiscussionId") %>&amp;id=<%# Eval("DiscussionThreadId")%></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Discussion/<%# Eval("DiscussionId") %>/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>    
    <asp:Repeater id="rssRepeaterMembers" runat="server">
     <ItemTemplate>
      <item>
       <title>New Member: <%# base.Render(Eval("AccountName")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountGroupName"))%></author>
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
             <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("AccountId") %>">
              <%# base.Render(Eval("AccountName")) %>
             </a>
            </div>
            <div class="sncore_description">
             date joined: <%# base.Render(SessionManager.Adjust((DateTime) Eval("Created")).ToString("d")) %>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category>Group Members</category>
       <link><% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("AccountId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Account/<%# Eval("AccountId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater id="rssRepeaterPlaces" runat="server">
     <ItemTemplate>
      <item>
       <title>New Place: <%# base.Render(Eval("PlaceName")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountGroupName")) %></author>
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
              <%# base.Render(Eval("PlaceName")) %>
             </a>
            </div>
            <div class="sncore_description">
             <%# base.Render(Eval("PlaceCity")) %>
            </div>
            <div class="sncore_description">
             date added: <%# base.Render(SessionManager.Adjust((DateTime) Eval("Created")).ToString("d")) %>
            </div>
           </td>
          </tr>
         </table>
        ]]>
       </description>
       <category>Group Places</category>
       <link><% Response.Write(WebsiteUrl); %>/PlaceView.aspx?id=<%# Eval("PlaceId") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Place/<%# Eval("PlaceId") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
    <asp:Repeater id="rssRepeaterBlogItems" runat="server">
     <ItemTemplate>
      <item>
       <title>New Blog Post: <%# base.Render(Eval("Title")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("Created")) %></pubDate>
       <author><%# base.Render(Eval("AccountName")) %></author>
       <description>
        <![CDATA[<%# base.RenderEx(Eval("Body")) %>]]>
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountBlogPostView.aspx?id=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Blog/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
