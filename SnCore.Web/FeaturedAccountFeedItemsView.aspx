<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="FeaturedAccountFeedItemsView.aspx.cs" Inherits="FeaturedAccountFeedItemsView" Title="Featured Blog Posts" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Featured Blog Posts
    </div>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="FeaturedAccountFeedItemsRss.aspx" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="10"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0" bordercolor="white">
      <tr>
       <td align="left" valign="top" width="*" class="sncore_message_left">
        <div class="sncore_message_header">
         <div class="sncore_message_subject">
          <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
           <%# base.Render(GetValue(GetAccountFeedItem((int)Eval("DataRowId")).Title, "Untitled"))%>
          </a>
         </div>
         <div class="sncore_description">
          <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>'>
           &#187; <%# GetComments(GetAccountFeedItem((int)Eval("DataRowId")).CommentCount)%>
          </a>
          <a target="_blank" href='<%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).Link) %>'>
           &#187; x-posted
          </a>
          in
          <a href="AccountView.aspx?id=<%# GetAccountFeedItem((int)Eval("DataRowId")).AccountId %>">
           <%# base.Render(GetAccountFeedItem((int)Eval("DataRowId")).AccountName)%>
          </a>'s
          <a href='AccountFeedView.aspx?id=<%# GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedId %>'>
           <%# base.Render(GetValue(GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedName, "Untitled"))%>
          </a>
          <span class='<%# (DateTime.UtcNow.Subtract(GetAccountFeedItem((int)Eval("DataRowId")).Created).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
           &#187; <%# SessionManager.ToAdjustedString(GetAccountFeedItem((int)Eval("DataRowId")).Created)%>            
          </span>
         </div>
        </div>
        <div class="sncore_message_body">
         <%# base.GetSummary(GetAccountFeedItem((int)Eval("DataRowId")).Description, GetAccountFeedItem((int)Eval("DataRowId")).AccountFeedLinkUrl) %>
        </div>
       </td>
      </tr>
     </table>     
    </itemtemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
