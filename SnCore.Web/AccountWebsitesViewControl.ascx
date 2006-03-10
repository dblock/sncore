<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountWebsitesViewControl.ascx.cs"
 Inherits="AccountWebsitesViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Websites</div>
<SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountWebsites"
 CssClass="sncore_inner_table" Width="95%" AutoGenerateColumns="false" ShowHeader="false">
 <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
  PrevPageText="Prev" HorizontalAlign="Center" />
 <Columns>
  <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td">
   <itemtemplate>
    <a href="<%# base.Render(Eval("Url")) %>" target="_blank">
     <%# base.Render(Eval("Name")) %>
    </a>
    <div style="font-size: smaller;">
     <%# base.Render(Eval("Description")) %>
    </div>
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>
