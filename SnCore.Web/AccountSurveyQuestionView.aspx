<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountSurveyQuestionView.aspx.cs"
 Inherits="AccountSurveyQuestionView" Title="Survey" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  <asp:Label ID="surveyName" runat="server" />
 </div>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountSurveyAnswers"
  CssClass="sncore_table" AllowCustomPaging="true" AllowPaging="true" PageSize="10"
  AutoGenerateColumns="false" ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td"  />
  <Columns>
   <asp:TemplateColumn ItemStyle-Width="200px">
    <itemtemplate>
     <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
      <div>
       <%# base.Render(Eval("AccountName")) %>
      </div>
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
    <%# base.RenderEx(Eval("Answer")) %>
   </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <ItemTemplate>
     <a href='AccountSurveyView.aspx?aid=<%# Eval("AccountId") %>&id=<% Response.Write(base.SurveyId); %>'>Survey</a>
    </ItemTemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
