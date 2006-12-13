<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountSurveyView.aspx.cs"
 Inherits="AccountSurveyView" Title="Survey" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td runat="server" id="accountcolumn" 
    class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 150px;">
    <a runat="server" id="accountLink" href="AccountView.aspx">
     <img border="0" src="images/AccountThumbnail.gif" runat="server" id="accountImage" />
     <div>
      <asp:Label ID="accountName" runat="server" />
     </div>
    </a>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label ID="surveyName" runat="server" />
    </div>
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountSurveyAnswers"
  CssClass="sncore_table" AutoGenerateColumns="false" ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <b>
      <a href='AccountSurveyQuestionView.aspx?id=<%# Eval("SurveyQuestionId") %>'>
       <%# base.Render(Eval("SurveyQuestion")) %>
      </a>
     </b>
     <div>
      <%# base.RenderEx(Eval("Answer")) %>
     </div>
   </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
