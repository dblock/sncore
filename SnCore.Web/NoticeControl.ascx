<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NoticeControl.ascx.cs" Inherits="NoticeControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PersistentPanel id="panelNotice" runat="server" Visible="false">
 <table width="100%">
  <tr>
   <td>
    <asp:Image ID="imageMessage" Width="24" Height="24" runat="server" />
   </td>
   <td>
    <asp:Label ID="labelMessage" runat="server" />
   </td>
  </tr>
 </table>
</SnCoreWebControls:PersistentPanel>
