<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountFriendsRss.aspx.cs"
 Inherits="AccountFriendsRss" %>
<%@ Import Namespace="SnCore.Services" %>

<rss version="2.0">
  <channel>
    <title><% Response.Write(Account.Name); %>'s Friends</title>
    <description><% Response.Write(Account.Name); %>'s friends</description>
    <link><% Response.Write(Link); %></link>
    <generator>sncore/sncore.vestris.com</generator>
    <image>
     <url><% Response.Write(WebsiteUrl); %>/images/site/rsslogo.jpg</url> 
     <title>Friends</title> 
     <link><% Response.Write(Link); %></link> 
     <width>100</width> 
     <height>100</height> 
    </image>    
    <asp:Repeater id="rssRepeater" runat="server">
     <ItemTemplate>
      <item>
       <title><%# base.Render(Eval("Name")) %></title>
       <pubDate><%# base.AdjustToRFC822(Eval("LastLogin")) %></pubDate>
       <author><%# base.Render(Eval("Name")) %></author>
       <description>
        <![CDATA[
         <table cellpadding="4">
          <tr>
           <td>
            <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("Id") %>">
             <img border="0" src="<% Response.Write(WebsiteUrl); %>/AccountPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
            </a>        
           </td>
           <td>
            <div>
             <a href="<% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("Id") %>">
              <%# base.Render(Eval("Name")) %>
             </a>
            </div>
            <div style="color: silver">
             Last activity: <%# base.Adjust(Eval("LastLogin")).ToString() %>
             <br />
             <%# base.Render(Eval("City")) %>
             <%# base.Render(Eval("State")) %>
             <%# base.Render(Eval("Country")) %>
            </div>
            <br />
            <div>
             <a href='<% Response.Write(WebsiteUrl); %>/AccountPicturesView.aspx?id=<%# Eval("Id") %>'>
              <%# GetNewPictures((int) Eval("NewPictures")) %>
             </a>
            </div>
            <div>
             <a href='<% Response.Write(WebsiteUrl); %>/AccountStoryView.aspx?id=<%# GetAccountStoryId((TransitAccountStory) Eval("LatestStory")) %>'>
              <%# GetAccountStory((TransitAccountStory)Eval("LatestStory"))%>
             </a>
            </div>
            <div>
             <a href='<% Response.Write(WebsiteUrl); %>/AccountSurveyView.aspx?aid=<%# Eval("Id") %>&id=<%# GetSurveyId((TransitSurvey) Eval("LatestSurvey")) %>'>
              <%# GetSurvey((TransitSurvey)Eval("LatestSurvey"))%>
             </a>
            </div>
            <div>
             <a href='<% Response.Write(WebsiteUrl); %>/AccountDiscussionThreadsView.aspx?id=<%# Eval("Id") %>'>
              <%# GetNewDiscussionPosts((int) Eval("NewDiscussionPosts")) %>
             </a>
            </div>           
            <div>
             <a href='AccountView.aspx?id=<%# Eval("Id") %>'>
              <%# GetNewSyndicatedContent((int) Eval("NewSyndicatedContent")) %>
             </a>
            </div>
           </td>
          </tr>
         </table>
        ]]>       
       </description>
       <category />
       <link><% Response.Write(WebsiteUrl); %>/AccountView.aspx?id=<%# Eval("Id") %></link>
       <guid isPermaLink="false"><% Response.Write(WebsiteUrl); %>/Account/<%# Eval("Id") %></guid>
      </item>
     </ItemTemplate>
    </asp:Repeater>
  </channel>
</rss>
