<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountSurveysManage.aspx.cs" Inherits="AccountSurveysManage" Title="Account | Surveys" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <div class="sncore_h2">
     My Surveys
    </div>
    <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" AutoGenerateColumns="false"
     CssClass="sncore_account_table">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
     <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
        <img src="images/Item.gif" />
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Survey">
       <itemtemplate>
        <a href="AccountSurvey.aspx?id=<%# Eval("Id") %>">
         <%# base.Render(Eval("Name")) %>
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountSurveyView.aspx?id=<%# Eval("Id") %>">
         View
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn>
       <itemtemplate>
        <a href="AccountSurvey.aspx?id=<%# Eval("Id") %>">
         Edit
        </a>
       </itemtemplate>
      </asp:TemplateColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
