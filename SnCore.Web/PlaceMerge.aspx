<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="PlaceMerge.aspx.cs"
 Inherits="PlaceMerge" Title="Merge Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="PlacePropertyGroupEdit" Src="PlacePropertyGroupEditControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountRedirectEdit" Src="AccountRedirectEditControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <asp:Label ID="labelName" runat="server" Text="Merge Places" />
 </div>
 <div class="sncore_cancel">
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" NavigateUrl="PlacesView.aspx"
   runat="server" />
 </div>
 <asp:UpdatePanel id="panelSelect" runat="server" UpdateMode="Conditional">
  <ContentTemplate>
   <table cellpadding="4" class="sncore_account_table" border="1">
    <tr>
     <td class="sncore_form_label">
      merge into:
     </td>
     <td class="sncore_form_value">
      <asp:HyperLink ID="linkMergeInto" runat="server" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      merge from:
     </td>
     <td class="sncore_form_value">
      <asp:HyperLink ID="linkMergeFrom" runat="server" />
      <asp:Panel ID="panelSelectMergeFrom" runat="server" Visible="false">
       <asp:TextBox ID="selectMergeFrom" runat="server" CssClass="sncore_form_textbox" />
       <div>
        <asp:LinkButton ID="select_MergeFrom" CssClass="sncore_link" runat="server" 
         OnClick="select_MergeFrom_Click" Text="&#187; lookup" />
       </div>
      </asp:Panel>
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel id="panelLookup" runat="server" UpdateMode="Conditional" Visible="false">
  <ContentTemplate>
   <div style="color: red; font-weight: bold; font-size: smaller; margin-top: 2px; margin-bottom: 5px; text-align: center;">
    <asp:Label EnableViewState="false" ID="labelLookup" runat="server" />
   </div>
   <asp:DataList RepeatColumns="2" CssClass="sncore_account_table" ID="gridLookupPlaces" runat="server">
    <ItemStyle VerticalAlign="Top" CssClass="sncore_table_tr_td" Width="50%" />
    <ItemTemplate>
     <a href="PlaceMerge.aspx?id=<%# RequestId %>&mid=<%# Eval("Id") %>">
      <%# base.Render(Eval("Name")) %>
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
 <asp:Panel ID="panelMerge" runat="server">
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
     website:
    </td>
    <td class="sncore_form_value">
     <asp:RadioButtonList ID="selectWebsite" runat="server" />
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
 </asp:Panel>
</asp:Content>
