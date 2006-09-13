<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountSurveysViewControl.ascx.cs"
 Inherits="AccountSurveysViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Surveys
</div>
<SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="accountSurveys" 
 ShowHeader="false" AllowCustomPaging="true">
 <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <ItemTemplate>
  <div class="sncore_h2left">
   <a href='AccountSurveyView.aspx?aid=<%# AccountId %>&id=<%# Eval("Id") %>'>
    <%# base.Render(Eval("Name")) %>
   </a>
  </div>
 </ItemTemplate>
</SnCoreWebControls:PagedList>