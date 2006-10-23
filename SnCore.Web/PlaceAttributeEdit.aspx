<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="PlaceAttributeEdit.aspx.cs" Inherits="PlaceAttributeEdit" Title="Place | Attribute" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Place Attribute
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="PlaceAttributesManage.aspx"
     runat="server" />
    <asp:UpdatePanel ID="panelAttribute" runat="server" UpdateMode="Always">
     <ContentTemplate>
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">
         attribute:
        </td>
        <td class="sncore_form_value">
         <asp:DropDownList ID="listAttributes" CssClass="sncore_form_dropdown" runat="server" DataTextField="Name" 
          DataValueField="Id" AutoPostBack="true" OnSelectedIndexChanged="listAttributes_SelectedItemChanged" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         value:
        </td>
        <td class="sncore_form_value">
         <asp:TextBox CssClass="sncore_form_textbox" ID="inputValue" TextMode="MultiLine" Rows="3" runat="server" />
         <asp:Label CssClass="sncore_description" ID="inputDefaultValue" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
         url:
        </td>
        <td class="sncore_form_value">
         <asp:TextBox CssClass="sncore_form_textbox" ID="inputUrl" runat="server" />
         <asp:Label CssClass="sncore_description" ID="inputDefaultUrl" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">
        </td>
        <td class="sncore_form_value">
         <asp:Image ID="previewImage" Visible="false" runat="server" />
        </td>
       </tr>
       <tr>
        <td>
        </td>
        <td class="sncore_form_value">
         <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Save" CausesValidation="true" CssClass="sncore_form_button"
          OnClick="save_Click" />
        </td>
       </tr>
      </table>
     </ContentTemplate>
    </asp:UpdatePanel>
   </td>
  </tr>
 </table>
</asp:Content>
