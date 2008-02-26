<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountPicturesManage.aspx.cs"
 Inherits="AccountPicturesManage" Title="Account | Pictures" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titlePictures" Text="My Pictures" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    A picture is worth a thousand words. Plus when you upload a picture everyone can see you, while if you don't, your member
    profile will require some work for others to find you.
   </div>
   <div class="sncore_title_paragraph">
    Uploaded pictures are automatically resized to 640x480. You could resize them on your computer to save transfer time.
   </div>
  </Template>
 </SnCore:Title>
 <table class="sncore_account_table">
  <tr>
   <td style="vertical-align: top;" class="sncore_form_label">
    add photos:
   </td>
   <td>
    <WilcoWebControls:MultiFileUpload ID="files" runat="server" InputCssClass="sncore_form_upload"
     OnFilesPosted="files_FilesPosted" />
    <asp:HyperLink ID="addFile" runat="server" CssClass="sncore_form_label" NavigateUrl="#">+</asp:HyperLink>
    <br />
    <br />
    <SnCoreWebControls:Button ID="save" CssClass="sncore_form_button" runat="server"
     Text="Upload" />
   </td>
  </tr>
 </table>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
    ID="gridManage" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    OnItemCommand="gridManage_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
    RepeatRows="3" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <a href="AccountPictureEdit.aspx?id=<%# Eval("Id") %>">
      <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("Id") %>&CacheDuration=0" alt="<%# base.Render(Eval("Name")) %>" />
     </a>
     <div style="font-size: smaller;">
     <a href="AccountPictureEdit.aspx?id=<%# Eval("Id") %>">
      &#187; Edit
     </div>
     <div style="font-size: smaller;">
      <asp:LinkButton Text="&#187; Delete" ID="deletePicture" runat="server" OnClientClick="return confirm('Are you sure you want to do this?')"
       CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
     </div>
     <div style="font-size: smaller;">
      <asp:LinkButton Text='<%# GetShowHideButtonText((bool) Eval("Hidden")) %>' ID="showhidePictre" runat="server"
       CommandName="ShowHide" CommandArgument='<%# Eval("Id") %>' />
     </div>
     <div style="font-size: smaller;">
      <asp:LinkButton Text="&#171;" ID="linkLeft" runat="server" CommandName="Left" CommandArgument='<%# Eval("Id") %>' />
      <asp:LinkButton Text="&#187;" ID="linkRight" runat="server" CommandName="Right" CommandArgument='<%# Eval("Id") %>' />
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
