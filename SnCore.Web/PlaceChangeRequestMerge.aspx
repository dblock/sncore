<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="PlaceChangeRequestMerge.aspx.cs"
 Inherits="PlaceChangeRequestMerge" Title="Merge Place Change Requests" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="PlacePropertyGroupEdit" Src="PlacePropertyGroupEditControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountRedirectEdit" Src="AccountRedirectEditControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <asp:Label ID="labelName" runat="server" Text="Merge Changes" />
 </div>
 <div class="sncore_cancel">
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" NavigateUrl="PlaceChangeRequestsManage.aspx"
   runat="server" />
 </div>
 <table cellpadding="4" class="sncore_account_table" border="1">
  <tr>
   <td class="sncore_form_label">
    type:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectType" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectName" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    description:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectDescription" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    country:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectCountry" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    state:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectState" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    city:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectCity" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    neighborhood:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectNeighborhood" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    street:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectStreet" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    zip:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectZip" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    cross-street:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectCrossStreet" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    phone:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectPhone" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    fax:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectFax" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    e-mail:
   </td>
   <td class="sncore_form_value">
    <asp:RadioButtonList ID="selectEmail" runat="server" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="merge" runat="server" Text="Merge" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="merge_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
