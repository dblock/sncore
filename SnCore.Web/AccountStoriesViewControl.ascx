<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountStoriesViewControl.ascx.cs"
 Inherits="AccountStoriesViewControl" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountStories" ShowHeader="false"
 AutoGenerateColumns="false" CssClass="sncore_inner_table" BorderWidth="0" Width="95%" PageSize="1">
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <Columns>
  <asp:BoundColumn DataField="Id" Visible="false" />
  <asp:TemplateColumn>
   <itemtemplate>
    <div class="sncore_h2left">
     <a href='AccountStoryView.aspx?id=<%# Eval("Id") %>'>
      <%# base.Render(Eval("Name")) %>
     </a>
    </div>
    <div style="font-size: smaller;">
     <%# GetDescription(Eval("Summary")) %>
    </div>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>
