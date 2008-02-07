<%@ Control Language="C#" AutoEventWireup="true" CodeFile="NoticeControl.ascx.cs"
 Inherits="NoticeControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelNotice" runat="server" Visible="false">
 <asp:Label ID="labelMessage" runat="server" />
 <div class="sncore_description" runat="server" id="divDetail" style="display: none;">
  <asp:Label ID="labelDetail" runat="server" />
 </div>
 <div class="sncore_description" style="margin-top: 10px;">
  <asp:HyperLink ID="linkContinue" runat="server" NavigateUrl="~/Default.aspx"
   Text="Click here to continue ..." />
 </div>
</asp:Panel>
