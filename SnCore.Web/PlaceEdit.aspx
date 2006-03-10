<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlaceEdit.aspx.cs"
 Inherits="PlaceEdit" Title="Place" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkPlaces" NavigateUrl="PlacesManage.aspx"
   Text="Places" runat="server" />
  <asp:Label CssClass="sncore_navigate_item" ID="linkPlaceId" Text="Place" runat="server" />
 </div>
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Place
    </div>
    <asp:HyperLink ID="linkBack" Text="Cancel" CssClass="sncore_cancel" NavigateUrl="PlacesManage.aspx"
     runat="server" />
    <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
     ShowSummary="true" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       type:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList CssClass="sncore_form_textbox" ID="selectType" DataTextField="Name"
        DataValueField="Name" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
       <asp:RequiredFieldValidator ID="inputNameRequired" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="name is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       description:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputDescription" TextMode="MultiLine"
        Rows="7" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       country and state:</td>
      <td class="sncore_form_value">
       <asp:DropDownList AutoPostBack="true" OnSelectedIndexChanged="inputCountry_SelectedIndexChanged"
        CssClass="sncore_form_dropdown_small" ID="inputCountry" DataTextField="Name" DataValueField="Name"
        runat="server" />
       <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
        DataValueField="Name" runat="server" />
       <asp:RequiredFieldValidator ID="countryRequired" runat="server" ControlToValidate="inputCountry"
        CssClass="sncore_form_validator" ErrorMessage="country is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       city:</td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" runat="server" />
       <asp:RequiredFieldValidator ID="cityRequired" runat="server" ControlToValidate="inputCity"
        CssClass="sncore_form_validator" ErrorMessage="city is required" Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       address:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputStreet" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       zip:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputZip" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       cross-street:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputCrossStreet" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       phone:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputPhone" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       fax:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputFax" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       e-mail:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmail" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       website:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputWebsite" runat="server" />
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
    <asp:Panel ID="panelPlaceAltName" runat="server">
     <div class="sncore_h2">
      Alternate Names
     </div>
     <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridPlaceNamesManage_ItemCommand"
      runat="server" ID="gridPlaceNamesManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
      <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
      <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="Center" />
      <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:TemplateColumn>
        <itemtemplate>
         <img src="images/Item.gif" />
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
        <itemtemplate>
         <%# base.Render(Eval("Name")) %>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
     <div class="sncore_h2">
      Add New</div>
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        alternate name:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputAltName" runat="server" />
       </td>
      </tr>
      <tr>
       <td>
       </td>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="Button1" runat="server" Text="Add" CausesValidation="true"
         CssClass="sncore_form_button" OnClick="altname_save_Click" />
       </td>
      </tr>
     </table>
    </asp:Panel>
   </td>
  </tr>
 </table>
</asp:Content>
