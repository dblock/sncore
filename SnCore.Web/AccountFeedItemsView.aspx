<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemsView.aspx.cs"
 Inherits="AccountFeedItemsView" Title="Feeds" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <atlas:UpdatePanel ID="panelLinks" Mode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       Feeds
      </div>
      <div class="sncore_h2sub">
       <a href="AccountFeedsView.aspx">&#187; Feeds</a>
       <a href="AccountFeedItemImgsView.aspx">&#187; Pictures</a>
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountFeedItemsRss.aspx" />
      <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
       href="AccountFeedItemsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </atlas:UpdatePanel>
 <atlas:UpdatePanel runat="server" ID="panelSearch" Mode="Conditional" RenderMode="Inline">
  <ContentTemplate> 
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server">
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       search:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true"
        CssClass="sncore_form_button" OnClick="search_Click" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </atlas:UpdatePanel>
 <atlas:UpdatePanel runat="server" ID="panelGrid" Mode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="5"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
    ShowHeader="false" OnDataBinding="gridManage_DataBinding" BorderWidth="0">
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
            <a href='AccountFeedItemView.aspx?id=<%# Eval("Id") %>&#comments'>
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
  </ContentTemplate>
 </atlas:UpdatePanel>   
</asp:Content>
