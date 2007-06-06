<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NoticeControl.ascx.cs" Inherits="NoticeControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<!-- NOEMAIL-START -->
<SnCoreWebControls:PersistentPanel id="panelNotice" runat="server" Visible="false">
 <script language="javascript">
  function CollapseExpandDetail(id)
  {
   var panel = document.getElementById(id);
   panel.style.cssText = (panel.style.cssText == "") ? "display: none;" : "";
  }
 </script>
 <table width="100%">
  <tr>
   <td>
    <a href="#" runat="server" id="linkCollapseExpand">
     <asp:Image ID="imageMessage" BorderWidth="0" Width="24" Height="24" runat="server" />
    </a>
   </td>
   <td>
    <asp:Label ID="labelMessage" runat="server" />
    <div class="sncore_description" runat="server" id="divDetail" style="display: none;">
     <asp:Label ID="labelDetail" runat="server" />
    </div>
   </td>
  </tr>
 </table>
</SnCoreWebControls:PersistentPanel>
<!-- NOEMAIL-END -->
