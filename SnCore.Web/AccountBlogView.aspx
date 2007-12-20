<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountBlogView.aspx.cs" Inherits="AccountBlogView" Title="Blog" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="BookmarksView" Src="BookmarksViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="LicenseView" Src="AccountLicenseViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="CounterView" Src="CounterViewControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="TellAFriend" Src="TellAFriendControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td align="center" width="150">
    <a runat="server" id="linkAccount" href="AccountView.aspx">
     <img border="0" src="AccountPictureThumbnail.aspx" runat="server" id="imageAccount" />
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
   </td>
   <td align="right" valign="middle">
    <p>
     <SnCore:RssLink ID="linkRelRss" runat="server" />
    </p>
    <!-- NOEMAIL-START -->
    <div>
     <SnCore:TellAFriend ID="linkTellAFriend" runat="server" />
    </div>
    <asp:Panel ID="panelAdmin" runat="server">
     <div>
      <asp:LinkButton OnClick="feature_Click" runat="server" ID="linkFeature" Text="Feature" />
     </div>
     <div>
      <asp:LinkButton OnClick="deletefeature_Click" runat="server" ID="linkDeleteFeatures" Text="Delete Features" />
     </div>
    </asp:Panel>
    <asp:Panel ID="panelOwner" runat="server" HorizontalAlign="Right">
     <div>
      <asp:HyperLink ID="linkPostNew" Text="&#187; Post New" runat="server" />
     </div>
     <div>
      <asp:HyperLink ID="linkEdit" Text="&#187; Manage Blog" runat="server" />
     </div>
    </asp:Panel>
    <!-- NOEMAIL-END -->
   </td>
  </tr>
 </table>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid runat="server" ID="gridManage" PageSize="5"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_table"
    ShowHeader="false" BorderWidth="0" OnItemCommand="gridManage_ItemCommand">
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
            <span style='<%# (bool) Eval("EnableComments") ? string.Empty : "display: none;" %>'>
             <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
              &#187; <%# GetComments((int) Eval("CommentCount"))%>
             </a>
            </span>
            <span style='<%# (bool) Eval("CanEdit") ? string.Empty : "display: none;" %>'>
             <a href='AccountBlogPost.aspx?bid=<%# Eval("AccountBlogId") %>&id=<%# Eval("Id") %>'>
              &#187; edit
             </a>
            </span>
            <span style='<%# (bool) Eval("CanEdit") && (bool) Eval("CanDelete") ? string.Empty : "display: none;" %>'>
             <a href='AccountBlogPostMove.aspx?id=<%# Eval("Id") %>'>
              &#187; move
             </a>
            </span>
            <span style='<%# (bool) Eval("CanDelete") ? string.Empty : "display: none;" %>'>
             <asp:LinkButton id="linkDelete" runat="server" Text="&#187; delete" CommandName="Delete" 
              CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Are you sure you want to delete this blog post?')" />
            </span>
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
  </ContentTemplate>
 </asp:UpdatePanel>
 <table class="sncore_table">
  <tr>
   <td>
    <SnCore:LicenseView runat="server" ID="licenseView" />       
   </td>
   <!-- NOEMAIL-START -->
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    <div class="sncore_description">
     views: <SnCore:CounterView ID="counterBlogViews" runat="server" />
    </div>
   </td>
   <td class="sncore_table_tr_td" style="font-size: smaller;" align="right">
    socially bookmark this blog:
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:BookmarksView ID="bookmarksView" ShowThumbnail="true" runat="server" RepeatColumns="-1" />
   </td>
   <!-- NOEMAIL-END -->
  </tr>
 </table>
</asp:Content>
