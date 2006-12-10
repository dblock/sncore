<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountAttributesManage.aspx.cs"
 Inherits="AccountAttributesManage" Title="Account | Attributes" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Account Attributes
 </div>
 <table cellspacing="0" cellpadding="4" class="sncore_account_table">
  <tr>
   <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 100px;">
    <a runat="server" id="accountLink" href="AccountView.aspx">
     <img border="0" src="images/AccountThumbnail.gif" runat="server" id="accountImage" />
    </a>
   </td>
   <td valign="top" width="*">
    <asp:Label CssClass="sncore_account_name" ID="accountName" runat="server" />
   </td>
  </tr>
 </table>
 <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="AccountAttributeEdit.aspx"
  runat="server" />
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
     <asp:TemplateColumn HeaderText="Attribute">
      <itemtemplate>
       <a href='<%# string.IsNullOrEmpty((string) Eval("Url")) ? Render(Eval("Attribute.DefaultUrl")) : Render(Eval("Url")) %>'>
        <img src='SystemAttribute.aspx?id=<%# Eval("AttributeId") %>' border="0" 
         alt='<%# string.IsNullOrEmpty((string) Eval("Value")) ? Render(Eval("Attribute.DefaultValue")) : Render(Eval("Value")) %>' />
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-Width="150">
      <itemtemplate>
       <a href="AccountAttributeEdit.aspx?id=<%# Eval("Id") %>&aid=<%# Eval("AccountId") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ItemStyle-Width="150" ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
