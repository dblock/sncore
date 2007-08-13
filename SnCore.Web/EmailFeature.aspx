<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailFeature.aspx.cs"
 Inherits="EmailFeature" Title="Featured" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Featured <% Response.Write(Renderer.Render(GetDataObjectType())); %>
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     Dear <b><% Response.Write(Renderer.Render(this.Account.Name)); %></b>,
    </p>
    <p>
     Your 
     <% Response.Write(Renderer.Render(GetDataObjectType())); %>
     <a href='<% Response.Write(Renderer.Render(this.Feature.DataObjectName)); %>View.aspx?id=<% Response.Write(Renderer.Render(this.Feature.DataRowId)); %>'>
      <% Response.Write(Renderer.Render(GetDataObjectName())); %>
     </a>
     has been featured on     
     <a href='<% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost"))); %>'>
      the front page of <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
     </a>
     - enjoy the celebrity!
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
