<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NoticeControl.ascx.cs" Inherits="NoticeControl" %>
<asp:Panel id="panelNotice" runat="server">
 <table class='<% Response.Write(base.CssClass); %>_<% Response.Write(base.Kind.ToString().ToLower()); %>' style='<% Response.Write(base.Style); %>'>
  <tr>
   <td class="sncore_notice_tr_td">
    <img src='images/site/<% Response.Write(base.Kind.ToString().ToLower()); %>.gif' width="24" height="24" />
   </td>
   <td class="sncore_notice_tr_td">   
    <% Response.Write(base.HtmlEncode ? base.Render(Message) : Message); %>
   </td>
  </tr>
 </table>
</asp:Panel>
