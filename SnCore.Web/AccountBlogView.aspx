<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountBlogView.aspx.cs" Inherits="AccountBlogView" Title="Blog" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td align="center" width="150">
    <a runat="server" id="linkAccount" href="AccountView.aspx">
     <img border="0" src="images/AccountThumbnail.gif" runat="server" id="imageAccount" />
     <div>
      <asp:Label ID="labelAccountName" runat="server" />
     </div>
    </a>
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     <asp:Label ID="labelBlog" runat="server" Text="Blog" />
    </div>
    <div class="sncore_h2sub">
     <asp:Label ID="labelBlogDescription" runat="server" />
    </div>
    <asp:Panel ID="panelAdmin" runat="server" HorizontalAlign="Right">
     <div>
      <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="Feature" />
     </div>
     <div>
      <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures" Text="Delete Features" />
     </div>
    </asp:Panel>
    <asp:Panel ID="panelOwner" runat="server" HorizontalAlign="Right">
     <div>
      <asp:HyperLink ID="linkEdit" Text="&#187; Edit Blog" runat="server" />
     </div>
    </asp:Panel>
   </td>   
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountBlogRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountBlogRss.aspx" />
   </td>
  </tr>
 </table>
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    socially bookmark this blog:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid runat="server" ID="gridManage" PageSize="5"
  AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
  ShowHeader="false" BorderWidth="0">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Left">
    <ItemTemplate>
     <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
       <td align="left" valign="top" width="*" class="sncore_message_table">
        <div class="sncore_message_header">
         <div class="sncore_message_subject">
          <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
           <%# base.Render(GetValue(Eval("Title"), "Untitled")) %>
          </a>
         </div>
         <div class="sncore_description">
          by <a href='AccountView.aspx?id=<%# Eval("AccountId") %>'>
           <%# base.Render(Eval("AccountName")) %>
          </a>
          on 
          <%# base.Adjust(Eval("Created")) %>
          <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>&#comments'>
           &#187; <%# GetComments((int) Eval("CommentCount"))%></a>
         </div>
        </div>
        <div class="sncore_message_body">
         <%# base.RenderEx(Eval("Body")) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
