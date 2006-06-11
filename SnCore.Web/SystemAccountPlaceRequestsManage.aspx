<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="SystemAccountPlaceRequestsManage.aspx.cs" Inherits="SystemAccountPlaceRequestsManage"
 Title="System | AccountPlaceRequests" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="100%">
    <div class="sncore_h2">
     Pending Account Place Requests
    </div>
    <SnCore:Notice ID="noticeRequests" runat="server" />
    <asp:Panel ID="panelRequests" runat="server">
     <table class="sncore_account_table">
      <tr>
       <td valign="top" class="sncore_form_label">
        message:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputMessage" TextMode="MultiLine"
         Rows="5" runat="server" />
       </td>
      </tr>
     </table>
     <SnCoreWebControls:PagedGrid CellPadding="4" AllowPaging="true" PageSize="15"
      runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
      OnItemCommand="gridManage_ItemCommand">
      <PagerStyle CssClass="sncore_account_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:TemplateColumn>
        <itemtemplate>
        <img src="images/Item.gif" />
       </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="What">
        <itemtemplate>
        <a href='AccountView.aspx?id=<%# Eval("PlaceId") %>'>
         <%# base.Render(Eval("PlaceName")) %>
        </a>
       </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="Message" ItemStyle-HorizontalAlign="Left">
        <itemtemplate>
        <div style="font-size: smaller">
         <%# base.Render(Eval("Message")) %>
        </div>
       </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="Who">
        <itemtemplate>
        <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
         <%# base.Render(Eval("AccountName")) %>
        </a>
       </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn HeaderText="Sent">
        <itemtemplate>
        <%# base.Adjust(Eval("Submitted")) %>
       </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Accept" Text="Accept" />
       <asp:ButtonColumn ButtonType="LinkButton" CommandName="Reject" Text="Reject" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
 </asp:Panel>
</asp:Content>
