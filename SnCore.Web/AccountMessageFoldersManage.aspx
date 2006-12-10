<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountMessageFoldersManage.aspx.cs"
 Inherits="AccountMessageFoldersManage" Title="Account | Messages" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Folders
 </div>
 <asp:HyperLink ID="createNew" NavigateUrl="AccountMessageFolderEdit.aspx" Text="&#187; Create New"
  CssClass="sncore_createnew" runat="server" />
 <SnCoreWebControls:PagedGrid CellPadding="4" ShowHeader="false" runat="server" ID="messagefoldersView"
  AutoGenerateColumns="false" CssClass="sncore_account_table" OnItemDataBound="messagefoldersView_ItemDataBound"
  OnItemCommand="messagefoldersView_ItemCommand">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="System" Visible="false" />
   <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <table>
      <tr>
       <td width="<%# (int) Eval("Level") * 10 %>px">
        <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 10 %>px" />
       </td>
       <td width="*" align="left">
        <img src="<%# base.GetFolderPicture((string) Eval("Name"), (bool) Eval("System")) %>" style="vertical-align: middle;" />
        <asp:LinkButton CommandArgument='<%# Eval("Id") %>' ID="linkFolder" Text='<%# Eval("Name") %>'
         runat="server" OnCommand="linkFolder_Click" />
       </td>
       <td>
        <img src="images/account/star.gif" style='<%# ((int) Eval("Id") == base.FolderId) ? "" : "display:none" %>' />
       </td>
      </tr>
     </table>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn Text="New" CommandName="New" ItemStyle-CssClass="sncore_table_tr_td"
    ItemStyle-HorizontalAlign="Center" />
   <asp:ButtonColumn Text="Rename" CommandName="Edit" ItemStyle-CssClass="sncore_table_tr_td"
    ItemStyle-HorizontalAlign="Center" />
   <asp:ButtonColumn CommandName="Delete" ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Center"
    Text="Delete" />
  </Columns>
 </SnCoreWebControls:PagedGrid>
 <SnCore:Notice ID="noticeFolder" Style="width: 582px;" runat="server" />
 <br /><br />
 <asp:Panel ID="messagesPanel" runat="server">
  <asp:Label ID="labelFolderName" runat="server" CssClass="sncore_h2" Text="Messages"></asp:Label>
  <asp:Panel ID="emptyPanel" runat="server">
   <table class="sncore_account_table" width="100%">
    <tr>
     <td colspan="2" class="sncore_table_tr_td" style="text-align: right;">
      <asp:LinkButton runat="server" ID="linkEmpty" OnClick="linkEmpty_Click" Text="&#187; Empty Folder" />
     </td>
    </tr>
   </table>
  </asp:Panel>
  <SnCoreWebControls:PagedGrid CellPadding="4" ShowHeader="false" runat="server" ID="messagesView"
   AutoGenerateColumns="false" CssClass="sncore_account_table" OnItemCommand="messagesView_ItemCommand">
   <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
    PrevPageText="Prev" HorizontalAlign="Center" />
   <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
   <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
   <Columns>
    <asp:BoundColumn DataField="Id" Visible="false" />
    <asp:TemplateColumn>
     <itemtemplate>
      <table width="100%">
       <tr>
        <td>
         <a href='AccountMessageView.aspx?id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>'>
          <img border="0" alt="Unread message" 
           src="<%# (bool) Eval("Unread") ? "images/account/star.gif" : "images/account/inbox.gif" %>" />
         </a>
        </td>
        <td align="left" valign="top" width="*">
         <div class="sncore_message_subject">            
          <a href='AccountMessageView.aspx?id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>'>
           <%# base.Render(Eval("Subject"))%>
          </a>
         </div>
         <div class="sncore_description">
          <span style="<%# ((int) Eval("SenderAccountId") ==  base.SessionManager.Account.Id) ? "display: none;" : "" %>">
           from
           <a href="AccountView.aspx?id=<%# Eval("SenderAccountId") %>">
            <%# base.Render(Eval("SenderAccountName")) %>
           </a>
          </span>
          <span style="<%# ((int) Eval("RecepientAccountId") ==  base.SessionManager.Account.Id) ? "display: none;" : "" %>">
           sent to
           <a href="AccountView.aspx?id=<%# Eval("RecepientAccountId") %>">
            <%# base.Render(Eval("RecepientAccountName")) %>
           </a>
          </span>
          on
          <%# base.Adjust(Eval("Sent")).ToString() %>
         </div>
         <div class="sncore_description">
          <a href="AccountMessageView.aspx?id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>#edit">
           &#187; read</a>
          <a href="AccountMessageEdit.aspx?id=<%# Eval("SenderAccountId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>#edit">
           &#187; reply</a>
          <a href="AccountMessageMove.aspx?id=<%# Eval("Id") %>&ReturnUrl=<%# Renderer.UrlEncode(base.ReturnUrl) %>">
           &#187; move</a>
          <asp:LinkButton CommandName="Delete" id="linkDelete" runat="server" Text="&#187; delete" OnClientClick="return confirm('Are you sure you want to do this?')" />
         </div>
        </td>
        <td width="150" align="center" valign="top">
         <a href="AccountMessageView.aspx?id=<%# Eval("Id") %>">
          <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("SenderAccountPictureId") %>" style="height:50px;" />
          <div class="sncore_link_description">
           <%# base.Render(Eval("SenderAccountName")) %>
          </div>
         </a>
        </td>
       </tr>
      </table>
     </itemtemplate>
    </asp:TemplateColumn>
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </asp:Panel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
