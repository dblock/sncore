<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountFeedItemsControl.ascx.cs"
 Inherits="SearchAccountFeedItemsControl" %>
<%@Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelFeedItemsResults" runat="server">
 <div class="sncore_h2">
  Feeds
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridResults" PageSize="5"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false" BorderWidth="0">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
       <td align="left" valign="top" width="*" class="sncore_message_left">
        <div class="sncore_message_header">
         <div class="sncore_message_subject">
          <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
           <%# base.Render(GetValue(Eval("Title"), "Untitled")) %>
          </a>
         </div>
         <div class="sncore_description">
          <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
           &#187; <%# GetComments((int) Eval("CommentCount"))%>
          </a>       
          <a target="_blank" href='<%# base.Render(Eval("Link")) %>'>
           &#187; x-posted
          </a>
          in
          <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
           <%# base.Render(Eval("AccountName")) %>
          </a>'s       
          <a href='AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>'>
           <%# base.Render(GetValue(Eval("AccountFeedName"), "Untitled")) %>
          </a>
          on
          <%# base.Adjust(Eval("Created")).ToString("d") %>
         </div>
        </div>
        <div class="sncore_message_body">
         <%# base.GetSummary((string) Eval("Description"), (string) Eval("AccountFeedLinkUrl")) %>
        </div>
       </td>
      </tr>
     </table>
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Panel>
