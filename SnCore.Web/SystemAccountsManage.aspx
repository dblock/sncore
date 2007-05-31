<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemAccountsManage.aspx.cs" Inherits="SystemAccountsManage" Title="Accounts" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Find Account(s)
 </div>
 <asp:UpdatePanel runat="server" id="panelFind" UpdateMode="Always">
  <ContentTemplate>
   <table class="sncore_inner_table">
    <tr>
     <td class="sncore_form_label">
      e-mail:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmail" runat="server" />
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="linkLookup" runat="server" Text="Find" CausesValidation="true" CssClass="sncore_form_button"
       OnClick="linkLookup_Click" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridLookup" AutoGenerateColumns="false" 
    CssClass="sncore_account_table"  AllowPaging="false" OnItemCommand="gridLookup_ItemCommand">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn HeaderText="Account">
      <ItemTemplate>
       <a target="_blank" href='AccountView.aspx?id=<%# Eval("Id") %>'>
        <%# Eval("Name") %>
       </a>
       <div class="sncore_link">
        <a href='AccountChangePassword.aspx?id=<%# Eval("Id") %>'>&#187; Reset Password</a>
        <a href='AccountDelete.aspx?id=<%# Eval("Id") %>'>&#187; Delete</a>
        <asp:LinkButton CommandName="Impersonate" CommandArgument='<%# Eval("Id") %>' id="linkImpersonate" 
         runat="server" Text="&#187; Impersonate" />
       </div>
      </ItemTemplate>   
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Birthday">
      <ItemTemplate>
       <div class="sncore_link_small">
        <%# ((DateTime) Eval("Birthday")).ToString("d") %>
       </div>
      </ItemTemplate>   
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Last Activity">
      <ItemTemplate>
       <div class="sncore_link_small">
        <%# Adjust(Eval("LastLogin")) %>
       </div>
      </ItemTemplate>   
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
