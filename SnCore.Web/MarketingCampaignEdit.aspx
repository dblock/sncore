<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="MarketingCampaignEdit.aspx.cs" Inherits="MarketingCampaignEdit" Title="Marketing Campaign" %>

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
     Marketing Campaign
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="MarketingCampaignsManage.aspx"
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
       description:       
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       sender's name:       
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputSenderName" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       sender's email:       
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputSenderEmail" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       content url:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputUrl" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox ID="inputActive" runat="server" CssClass="sncore_form_checkbox" Text="active campaign" />
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
