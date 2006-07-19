<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountContentEdit.aspx.cs"
 Inherits="AccountContentEdit" Title="Content" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Content
    </div>
    <div>
     <asp:HyperLink ID="linkPreview" Text="&#187; Preview" CssClass="sncore_cancel" NavigateUrl="AccountContentGroup.aspx"
      runat="server" />
    </div>
    <div>
     <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountContentGroupEdit.aspx"
      runat="server" />
    </div>
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       tag:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputTag" runat="server" CssClass="sncore_form_textbox" />
       <div class="sncore_link_small">
        content tag
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       text:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox TextMode="MultiLine" Rows="5" ID="inputText" runat="server" CssClass="sncore_form_textbox" />
       <div class="sncore_link_small">
        trusted content groups allow un-encoded html and support bb-tags
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       position:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox Text="0" ID="inputPosition" runat="server" CssClass="sncore_form_textbox" />
       <div class="sncore_link_small">
        desired position within other content
       </div>
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <SnCoreWebControls:Button ID="linkSave" CssClass="sncore_form_button" OnClick="save"
        runat="server" Text="Save" />
      </td>
     </tr>
    </table>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
