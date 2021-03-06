<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="PlaceChangeRequestEdit.aspx.cs"
 Inherits="PlaceChangeRequestEdit" Title="Place Change Request" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <asp:Label ID="labelName" runat="server" Text="New Place" />
 </div>
 <div class="sncore_cancel">
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" NavigateUrl="PlacesView.aspx"
   runat="server" />
 </div>
 <table cellpadding="4" class="sncore_account_table" border="1">
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
   </td>
  </tr>
 </table>
 <asp:UpdatePanel id="panelLookup" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <div style="color: red; font-weight: bold; font-size: smaller; margin-top: 2px; margin-bottom: 5px; text-align: center;">
    <asp:Label EnableViewState="false" ID="labelLookup" runat="server" />
   </div>
   <asp:DataList RepeatColumns="2" CssClass="sncore_account_table" ID="gridLookupPlaces" runat="server">
    <ItemStyle VerticalAlign="Top" CssClass="sncore_table_tr_td" Width="50%" />
    <ItemTemplate>
     <a href='PlaceView.aspx?id=<%# Eval("Id") %>' target="_blank">
      <b><%# base.Render(Eval("Name")) %></b>
     </a>
     <div class="sncore_li_description">
      <%# base.Render(Eval("Type")) %>
     </div>
     <div class="sncore_li_description">
      <%# base.Render(Eval("Neighborhood")) %>
     </div>
     <div class="sncore_li_description">
      <%# base.Render(Eval("Street")) %>
      <%# base.Render(Eval("Zip")) %>
      <%# base.Render(Eval("City")) %>
     </div>
     <div class="sncore_li_description">     
      <%# base.Render(Eval("State")) %>
      <%# base.Render(Eval("Country")) %>
     </div>
     <div class="sncore_li_description">
      <%# base.Render(Eval("Phone")) %>
     </div>
    </ItemTemplate>
   </asp:DataList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <table cellpadding="4" class="sncore_account_table" border="1">
  <tr>
   <td class="sncore_form_label">
    description:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputDescription" runat="server" TextMode="MultiLine" Rows="5" CssClass="sncore_form_textbox" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    country and state:</td>
   <td class="sncore_form_value">
    <asp:UpdatePanel runat="server" ID="panelCountryState" RenderMode="Inline" UpdateMode="Conditional">
     <ContentTemplate>
      <asp:DropDownList AutoPostBack="true" CssClass="sncore_form_dropdown_small" ID="inputCountry" 
       DataTextField="Name" DataValueField="Name" runat="server" />       
      <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState" DataTextField="Name"
       DataValueField="Name" runat="server" />
     </ContentTemplate>
    </asp:UpdatePanel>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    city:</td>
   <td class="sncore_form_value">
    <asp:UpdatePanel runat="server" ID="panelCity" RenderMode="Inline" UpdateMode="Conditional">
     <ContentTemplate>
      <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoCompleteCity" TargetControlID="inputCity"
       ServiceMethod="GetCitiesCompletionList" ServicePath="ScriptServices.asmx" MinimumPrefixLength="0" 
       CompletionInterval="500" EnableCaching="true" CompletionSetCount="25" UseContextKey="true" />
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputCity" AutoPostBack="true" runat="server" />
     </ContentTemplate>
    </asp:UpdatePanel>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    neighborhood:</td>
   <td class="sncore_form_value">
    <asp:UpdatePanel runat="server" ID="panelNeighborhood" RenderMode="Inline" UpdateMode="Conditional">
     <ContentTemplate>
      <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoCompleteNeighborhood" TargetControlID="inputNeighborhood"
       ServiceMethod="GetNeighborhoodsCompletionList" ServicePath="ScriptServices.asmx" MinimumPrefixLength="0" 
       CompletionInterval="500" EnableCaching="true" CompletionSetCount="25" UseContextKey="true" />
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputNeighborhood" runat="server" />
     </ContentTemplate>
    </asp:UpdatePanel>
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
 </table>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="save" runat="server" Text="Submit" CausesValidation="true"
     CssClass="sncore_form_button" OnClick="save_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
