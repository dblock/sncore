<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="SystemAccountQuotasManage.aspx.cs"
 Inherits="SystemAccountQuotasManage" Title="System | Account Quotas" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Account Quotas
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
    AllowPaging="true" AllowCustomPaging="true" PageSize="5">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/quota.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Object">
      <itemtemplate>
       <%# Eval("DataObjectName") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Limit">
      <itemtemplate>
       <%# Eval("Limit") %>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
   <div class="sncore_h2">
    Set Quota
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      object:
     </td>
     <td class="sncore_form_value">
      <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputObject" runat="server"
        DataTextField="Name" DataValueField="Name" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
      limit:
     </td>
     <td class="sncore_form_value">
      <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputLimit" runat="server">
       <asp:ListItem Text="10" Value="10" />
       <asp:ListItem Text="100" Value="100" />
       <asp:ListItem Text="250" Value="250" />
       <asp:ListItem Text="500" Value="500" />
       <asp:ListItem Text="1000" Value="1000" />
       <asp:ListItem Text="10000" Value="10000" />
       <asp:ListItem Text="100000" Value="100000" />
      </asp:DropDownList>
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="setQuota" runat="server" Text="Add" CssClass="sncore_form_button" 
       OnClick="save_Click" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
