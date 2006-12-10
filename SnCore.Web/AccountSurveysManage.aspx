<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountSurveysManage.aspx.cs" Inherits="AccountSurveysManage" Title="Account | Surveys" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
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
     <img src="images/account/surveys.gif" />
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
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
