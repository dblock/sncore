<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlaceWebsiteEdit.aspx.cs"
 Inherits="PlaceWebsiteEdit" Title="Place | Link a Site" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Link a Site
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="PlaceWebsitesManage.aspx"
  runat="server" />
 <asp:UpdatePanel ID="panelAdd" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <table class="sncore_table">
    <tr>
     <td class="sncore_form_label">
      url:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" Text="http://" ID="inputUrl" runat="server" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
     </td>
     <td class="sncore_form_value">
      <asp:LinkButton ID="linkFetch" CssClass="sncore_link" runat="server" Text="&#187; fetch title" OnClick="linkFetch_Click" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      title:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      description:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputDescription" runat="server"
       TextMode="MultiLine" Rows="5" />
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true"
       CssClass="sncore_form_button" OnClick="save_Click" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
