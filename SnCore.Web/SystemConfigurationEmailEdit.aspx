<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemConfigurationEmailEdit.aspx.cs" Inherits="SystemConfigurationEmailEdit"
 Title="System | Outgoing E-Mail Configuration" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Outgoing E-Mail Settings
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemConfigurationsManage.aspx"
  runat="server" />
 <asp:UpdatePanel ID="panelManageUpdate" runat="server" UpdateMode="Always">
  <contenttemplate>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      delivery:
     </td>
     <td class="sncore_form_value">
      <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputDelivery" runat="server"
       DataValueField="Method" DataTextField="Description" AutoPostBack="true" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelServerPort" runat="server" EnableViewState="true">
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       server:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputServer" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       port:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputPort" runat="server" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelUsernamePassword" runat="server" EnableViewState="true">
    <table class="sncore_account_table">   
     <tr>
      <td class="sncore_form_label">
       username:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputUsername" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       password:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputPassword" TextMode="Password"
        runat="server" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelPickupDirectoryLocation" runat="server" EnableViewState="true">
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       pick-up directory:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputPickupDirectoryLocation" runat="server" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="save" runat="server" Text="Save" CausesValidation="true"
       CssClass="sncore_form_button" OnClick="save_Click" />
     </td>
    </tr>
   </table>
  </contenttemplate>
 </asp:UpdatePanel>
 <div class="sncore_h2">
  Test E-Mail
 </div>
 <div class="sncore_h2sub">
  This test uses the saved configuration settings. If you want to change and test e-mail delivery settings, 
  click the Save button above first.
 </div>
 <div class="sncore_h2sub">
  <SnCoreWebControls:Button ID="test" runat="server" Text="Test" CausesValidation="true"
   CssClass="sncore_form_button" OnClick="test_Click" />
 </div>
</asp:Content>
