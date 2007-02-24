<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemRefererAccountEdit.aspx.cs" Inherits="SystemRefererAccountEdit" Title="System | Referer Account" %>

<%@ Register TagPrefix="SnCore" tagname="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Referer Account
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="SystemRefererAccountsManage.aspx"
  runat="server" />
 <asp:UpdatePanel runat="server" id="panelEdit" UpdateMode="Always">
  <ContentTemplate>
   <table class="sncore_inner_table">
    <tr>
     <td class="sncore_form_label">
      referer host:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputRefererHost" runat="server" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      account id:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputAccount" runat="server" />
      <div class="sncore_link">
       <asp:LinkButton ID="linkLookup" CausesValidation="false" runat="server" 
        OnClick="linkLookup_Click" Text="&#187; lookup" />
      </div>
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
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridLookup" AutoGenerateColumns="false" CssClass="sncore_account_table" 
    AllowPaging="false" OnItemCommand="gridLookup_ItemCommand" >
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn HeaderText="Account">
      <ItemTemplate>
       <a target="_blank" href='AccountView.aspx?id=<%# Eval("Id") %>'>
        <%# Eval("Name") %>
       </a>
      </ItemTemplate>   
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Select" Text="Select" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
