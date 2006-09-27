<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountTimeZoneControl.ascx.cs"
 Inherits="AccountTimeZoneControl" %>

<table id="tableMain" class="sncore_table" style="border-color: orange;" runat="server">
 <tr>
  <td class="sncore_table_tr_td">
   <img align="left" src="images/site/warning.gif" width="12" height="12" />
  </td>
  <td  class="sncore_table_tr_td" style="font-size: smaller;">
   <div>
    Your timezone setting may be incorrect. The times shown are <asp:Label ID="labelTimeZone" runat="server" />.
    <a href="AccountPreferencesManage.aspx">&#187; Change</a>
   </div>
  </td>
 </tr>
 </table>
