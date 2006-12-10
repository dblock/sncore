<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemSurveysManage.aspx.cs" Inherits="SystemSurveysManage" Title="System | Surveys" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Surveys
 </div>
 <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="SystemSurveyEdit.aspx"
  runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand" runat="server"
  ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="Name" Visible="false" />
   <asp:TemplateColumn>
    <itemtemplate>
     <img src="images/Item.gif" />
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Survey" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <a href="SystemSurveyEdit.aspx?id=<%# Eval("Id") %>">
      <%# base.Render(Eval("Name")) %>
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
     <a href="SystemSurveyEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
