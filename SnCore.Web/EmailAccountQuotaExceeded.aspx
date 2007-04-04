<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="EmailAccountQuotaExceeded.aspx.cs"
 Inherits="EmailAccountQuotaExceeded" Title="Quota exceeded" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Quota Exceeded
 </div>
 <table class="sncore_table">
  <tr>
   <td>
    <p>
     <a href="AccountView.aspx?id=<% Response.Write(this.Account.Id); %>">
      <% Response.Write(Renderer.Render(this.Account.Name)); %>
     </a>
     has exceeded a hard quota limit. You might want to check this out.
    </p>
   </td>
  </tr>
 </table>
</asp:Content>
