<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountProfilesViewControl.ascx.cs"
 Inherits="AccountProfilesViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel ID="panelAboutMe" runat="server">
 <div class="sncore_h2">
  About Me
 </div>
 <table class="sncore_inner_table" style="width: 95%;">
  <tr>
   <td>
    <asp:Label ID="labelAboutMe" runat="server" />
   </td>
  </tr>
 </table>
</asp:Panel>
