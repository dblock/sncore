<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountSurveysViewControl.ascx.cs"
 Inherits="AccountSurveysViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Surveys</div>
<SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountSurveys"
 CssClass="sncore_inner_table" AutoGenerateColumns="false" ShowHeader="false"
 Width="95%">
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <Columns>
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td">
   <itemtemplate>
    <b>
     <a href='AccountSurveyView.aspx?aid=<%# AccountId %>&id=<%# Eval("Id") %>'>
      <%# base.Render(Eval("Name")) %>
     </a>
    </b>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>