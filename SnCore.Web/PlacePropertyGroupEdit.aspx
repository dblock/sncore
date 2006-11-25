<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="PlacePropertyGroupEdit.aspx.cs" Inherits="PlacePropertyGroupEdit" Title="Place | Profile" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="PlacePropertyGroupEdit" Src="PlacePropertyGroupEditControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <table width="100%">
     <tr>
      <td>
       <div class="sncore_h2">
        <asp:Label ID="labelName" runat="server" />
       </div>
      </td>
      <td>
       <div class="sncore_description">
        <asp:Label ID="labelDescription" runat="server" />
       </div>
      </td>
     </tr>
    </table>
    <div class="sncore_cancel">
     <asp:HyperLink runat="server" ID="linkBack" Text="&#187; Cancel" />
    </div>
    <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
     <ContentTemplate>
      <SnCore:PlacePropertyGroupEdit id="ppg" runat="server" />
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">
         <SnCoreWebControls:Button ID="save" runat="server" Text="Save" CausesValidation="true"
          CssClass="sncore_form_button" OnClick="save_Click" />
        </td>
       </tr>
      </table>
     </ContentTemplate>
    </asp:UpdatePanel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
