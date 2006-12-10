<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="About.aspx.cs" Inherits="About" Title="About" %>

<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <%@ register tagprefix="SnCore" tagname="Notice" src="NoticeControl.ascx" %>
 <div class="sncore_h2">
  About
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    Copyright:
   </td>
   <td class="sncore_form_value">
    <% Response.Write(Copyright); %>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    Version:
   </td>
   <td class="sncore_form_value">
    <% Response.Write(Version); %>
   </td>
  </tr>
 </table>
 <div class="sncore_h2">
  Credits
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    Development:
   </td>
   <td class="sncore_form_value">
    dB. (<a target="_blank" href="http://www.dblock.org/">dblock.org</a>)
   </td>
  </tr>
  <tr>
   <td valign="top" class="sncore_form_label">
    Technology:
   </td>
   <td class="sncore_form_value">
    Built on the <a target="_blank" href="http://sncore.vestris.com">SnCore SDK</a>
   </td>
  </tr>
 </table>
 <div class="sncore_h2">
  Uptime
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    Front-End:
   </td>
   <td class="sncore_form_value">
    <% Response.Write(Uptime.ToString()); %>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    Web Services:
   </td>
   <td class="sncore_form_value">
    <% Response.Write(WebServicesUptime.ToString()); %>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    Back-End:
   </td>
   <td class="sncore_form_value">
    <% Response.Write(BackEndServicesUptime.ToString()); %>
   </td>
  </tr>
 </table>
 <div class="sncore_h2">
  Stats
 </div>
 <div class="sncore_h2sub">
  <a href="SystemStatsHits.aspx">&#187; Detailed Stats</a>
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    Total hits:
   </td>
   <td class="sncore_form_value">
    <% Response.Write(Summary.TotalHits); %>
   </td>
  </tr>
 </table> 
</asp:Content>
