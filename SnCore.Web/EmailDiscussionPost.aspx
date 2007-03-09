<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailDiscussionPost.aspx.cs"
 Inherits="EmailDiscussionPost" Title="New discussion post" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  New Discussion Post
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.Recepient.Name)); %></b>,
    </p>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(this.DiscussionPost.AccountId); %>">
      <% Response.Write(Renderer.Render(this.DiscussionPost.AccountName)); %>
     </a>
     has posted a new message for you in
     <a href="<% Response.Write(GetNavigateUri()); %>">
      <% Response.Write(Renderer.Render(GetNavigateName())); %>
     </a>
    </p>
    <table class="sncore_account_table">
     <tr>
      <td style="text-align: left; vertical-align: top;" class="sncore_table_tr_td">
       <div class="sncore_message_subject">
        <% Response.Write(Renderer.Render(this.DiscussionPost.Subject)); %>
       </div>
       <div class="sncore_description">
        from 
        <a href="AccountView.aspx?id=<% Response.Write(this.DiscussionPost.AccountId); %>">
         <% Response.Write(Renderer.Render(this.DiscussionPost.AccountName)); %>
        </a>
        on
        <% Response.Write(Renderer.Render(this.DiscussionPost.Created.ToString("d"))); %>        
       </div>
       <div style="margin: 10px 0px 10px 0px;">
        <% Response.Write(RenderEx(this.DiscussionPost.Body)); %>
       </div>
      </td>
     </tr>
    </table>  
    <p>
     <a href="DiscussionThreadView.aspx?id=<% Response.Write(this.DiscussionPost.DiscussionThreadId); %>">&#187; Read Thread</a>
     <a href="DiscussionPost.aspx?did=<% Response.Write(this.DiscussionPost.DiscussionId); %>&pid=<% Response.Write(this.DiscussionPost.Id); %>#edit">&#187; Reply</a>
     <a href="DiscussionPost.aspx?did=<% Response.Write(this.DiscussionPost.DiscussionId); %>&pid=<% Response.Write(this.DiscussionPost.Id); %>&Quote=true#edit">&#187; Quote</a>
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
