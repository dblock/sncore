<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="TagWordsManage.aspx.cs"
 Inherits="TagWordsManage" Title="Tags | Words" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Tag Words
    </div>
    <atlas:UpdatePanel runat="server" Mode="Always" ID="panelTagWords">
     <ContentTemplate>
      <SnCore:Notice id="noticeManage" runat="server" EnableViewState="false" />
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">
         tag words:
        </td>
        <td class="sncore_form_value">
         <asp:DropDownList CssClass="sncore_form_dropdown" ID="listboxSelectType" runat="server"
          AutoPostBack="True" OnSelectedIndexChanged="listboxSelectType_SelectedIndexChanged" />
        </td>
       </tr>
       <tr>
        <td colspan="2" align="center">
         <asp:Button ID="buttonPromote" runat="server" OnClick="buttonPromote_Click" CssClass="sncore_form_button"
          Text="Pro/Demote" />
         <asp:Button ID="buttonExclude" runat="server" OnClick="buttonExclude_Click" CssClass="sncore_form_button"
          Text="Inc/Exclude" />
        </td>
       </tr>
      </table>
      <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="25"
       AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table" AllowCustomPaging="true">
       <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
       <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
       <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
        PrevPageText="Prev" HorizontalAlign="Center" />
       <Columns>
        <asp:BoundColumn DataField="Id" Visible="false" />
        <asp:TemplateColumn>
         <itemtemplate>
        <img alt="" src="images/Item.gif" />
       </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn>
         <itemtemplate>
        <asp:Checkbox Checked="true" runat="server" />
       </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Word">
         <itemtemplate>
        <%# base.Render(Eval("Word")) %>
       </itemtemplate>
        </asp:TemplateColumn>
       </Columns>
      </SnCoreWebControls:PagedGrid>
      </td> </tr> </table>
     </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>
