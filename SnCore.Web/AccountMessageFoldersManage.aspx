<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountMessageFoldersManage.aspx.cs"
 Inherits="AccountMessageFoldersManage" Title="Account | Messages" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <SnCore:Notice ID="noticeFolder" runat="server" />
    <%--    
   <div class="sncore_h2">
     Folders
    </div>
--%>
    <%--
 <asp:HyperLink ID="createNew" NavigateUrl="AccountMessageFolderEdit.aspx" Text="&#187; Create New" CssClass="sncore_createnew" runat="server" /> 
--%>
    <%--    <SnCoreWebControls:PagedGrid CellPadding="4" showheader="false" runat="server" id="messagefoldersView"
     autogeneratecolumns="false" cssclass="sncore_account_table" onitemdatabound="messagefoldersView_ItemDataBound"
     onitemcommand="messagefoldersView_ItemCommand">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next" PrevPageText="Prev"
  		HorizontalAlign="Center" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:BoundColumn DataField="System" Visible="false" />
   <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" ItemStyle-HorizontalAlign="Left">
    <ItemTemplate>
     <table>
      <tr>
       <td width="<%# (int) Eval("Level") * 10 %>px">
        <img src="images/Spacer.gif" width="<%# (int) Eval("Level") * 10 %>px" />
       </td>
       <td width="*" align="left">
        <img src="images/Folder.gif" style="vertical-align: middle;" />
        <asp:LinkButton CommandArgument='<%# Eval("Id") %>' ID="linkFolder" Text='<%# Eval("Name") %>'
         runat="server" OnCommand="linkFolder_Click" />
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn Visible="false" Text="New" CommandName="New" ItemStyle-CssClass="sncore_table_tr_td"
    ItemStyle-HorizontalAlign="Center" />
   <asp:ButtonColumn Text="Edit" CommandName="Edit" ItemStyle-CssClass="sncore_table_tr_td"
    ItemStyle-HorizontalAlign="Center" />
   <asp:ButtonColumn CommandName="Delete" ItemStyle-CssClass="sncore_table_tr_td"
    ItemStyle-HorizontalAlign="Center" Text="Delete" />
  </Columns>
 </SnCorewebcontrols:pagedgrid>
    <br />
    <br />
--%>
    <asp:Panel ID="messagesPanel" runat="server">
     <asp:Label ID="labelFolderName" runat="server" CssClass="sncore_h2" Text="Messages"></asp:Label>
     <asp:Panel ID="emptyPanel" runat="server">
      <table class="sncore_account_table" width="100%">
       <tr>
        <td colspan="2" class="sncore_table_tr_td" style="text-align: right;">
         <asp:LinkButton runat="server" ID="linkEmpty" OnClick="linkEmpty_Click" Text="Empty Folder" />
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
           <td valign="top" width="150" align="center">
            <a href="AccountView.aspx?id=<%# Eval("SenderAccountId") %>">
             <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("SenderAccountPictureId") %>" />
             <br />
             <b>
              <%# base.Render(Eval("SenderAccountName")) %>
             </b>
            </a>
           </td>
           <td align="left" valign="top" width="*">
            <b>from:</b> 
            <a href="AccountView.aspx?id=<%# Eval("SenderAccountId") %>">
             <%# base.Render(Eval("SenderAccountName")) %>
            </a>
            <br />
            <b>to:</b> 
            <a href="AccountView.aspx?id=<%# Eval("RecepientAccountId") %>">
             <%# base.Render(Eval("RecepientAccountName")) %>
            </a>
            <br />
            <b>subject:</b>
            <%# base.Render(Eval("Subject"))%>
            <br />
            <b>posted:</b>
            <%# base.Adjust(Eval("Sent")).ToString() %>
            <br />
            <br />
            <%# base.RenderEx(Eval("Body"))%>
           </td>
          </tr>
         </table>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:TemplateColumn>
        <itemtemplate>
         <a href="AccountMessageEdit.aspx?id=<%# Eval("SenderAccountId") %>&pid=<%# Eval("Id") %>&ReturnUrl=<%# SnCore.Tools.Web.Renderer.UrlEncode(base.ReturnUrl) %>#edit">
          Reply</a>
        </itemtemplate>
       </asp:TemplateColumn>
       <asp:ButtonColumn CommandName="Delete" Text="Delete" />
      </Columns>
     </SnCoreWebControls:PagedGrid>
    </asp:Panel>
   </td>
  </tr>
 </table>
</asp:Content>
