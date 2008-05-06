<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountEventsViewControl.ascx.cs"
 Inherits="AccountEventsViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Events
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="2"
   AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table"
   ShowHeader="false">
   <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
   <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
   <Columns>
    <asp:BoundColumn DataField="Id" Visible="false" />
    <asp:TemplateColumn ItemStyle-VerticalAlign="Middle">
     <itemtemplate>
      <a href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>">
       <img border="0" src="AccountEventPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
      </a>
     </itemtemplate>
    </asp:TemplateColumn>
    <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
     <itemtemplate>
      <div>
       <a class="sncore_event_name" href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>">
        <%# base.Render(Eval("Name")) %>
       </a>
       <span style="font-size: smaller;">
        <a href="AccountEventView.aspx?id=<%# Eval("AccountEventId") %>"> 
         &#187; event details
        </a>
       </span>
      </div>
      <div class="sncore_description">
       Starts: <%# base.Adjust(Eval("StartDateTime")).ToString("f") %>
      </div>
      <div class="sncore_description" style='<%# (bool) Eval("NoEndDateTime") ? "display: none;" : string.Empty %>'>
       Ends: <%# base.Adjust(Eval("EndDateTime")).ToString("f") %>
      </div>
      <div class="sncore_summary">
       <%# base.GetSummary((string)Eval("Description"))%>
      </div>
     </itemtemplate>
    </asp:TemplateColumn>
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </ContentTemplate>
</asp:UpdatePanel>
