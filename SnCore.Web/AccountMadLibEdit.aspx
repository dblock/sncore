<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountMadLibEdit.aspx.cs" Inherits="AccountMadLibEdit" Title="Mad Lib" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Mad Lib
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="MadLibsManage.aspx"
     runat="server" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       name:       
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputName" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       template:       
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputTemplate" runat="server" TextMode="MultiLine" Rows="5" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <div class="sncore_description">
        use [type of word] to define a blank
       </div>
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="save" runat="server" Text="Save" CausesValidation="true" CssClass="sncore_form_button"
        OnClick="save_Click" />
      </td>
     </tr>
    </table>
   </td>
  </tr>
 </table>
</asp:Content>
