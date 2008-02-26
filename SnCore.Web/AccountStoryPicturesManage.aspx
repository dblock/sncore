<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountStoryPicturesManage.aspx.cs"
 Inherits="AccountStoryPicturesManage" Title="Story Pictures" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Add Photos
 </div>
 <div class="sncore_h2sub">
  <asp:HyperLink ID="linkBack" runat="server" Text="&#187; Edit Story" />
 </div>
 <table class="sncore_table">
  <tr>
   <td style="vertical-align: top;" class="sncore_form_label">
    add photos:
   </td>
   <td>
    <wilcowebcontrols:multifileupload id="files" runat="server" inputcssclass="sncore_form_upload"
     onfilesposted="files_FilesPosted" />
    <asp:HyperLink ID="addFile" runat="server" CssClass="sncore_form_label" NavigateUrl="#">+</asp:HyperLink>
    <br />
    <br />
    <sncorewebcontrols:button id="picturesAdd" cssclass="sncore_form_button" runat="server"
     text="Upload" />
   </td>
  </tr>
 </table>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CssClass="sncore_table" runat="server" RepeatDirection="Horizontal"
    ID="gridManage" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
    OnItemCommand="gridManage_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="4"
    RepeatRows="3" AllowCustomPaging="true">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <a href='AccountStoryPictureEdit.aspx?id=<%# Eval("Id") %>&pid=<%# Eval("AccountStoryId") %>'>
      <img border="0" src="AccountStoryPictureThumbnail.aspx?id=<%# Eval("Id") %>&CacheDuration=0" alt="<%# base.Render(Eval("Name")) %>" />
     </a>
     <div style="font-size: smaller;">
     <a href='AccountStoryPictureEdit.aspx?id=<%# Eval("Id") %>&pid=<%# Eval("AccountStoryId") %>'>
      &#187; Edit
     </div>
     <div style="font-size: smaller;">
      <asp:LinkButton Text="&#187; Delete" ID="deletePicture" runat="server" OnClientClick="return confirm('Are you sure you want to do this?')"
       CommandName="Delete" CommandArgument='<%# Eval("Id") %>' />
     </div>
     <div style="font-size: smaller;">
      <asp:LinkButton Text="&#171;" ID="linkLeft" runat="server" CommandName="Left" CommandArgument='<%# Eval("Id") %>' />
      <asp:LinkButton Text="&#187;" ID="linkRight" runat="server" CommandName="Right" CommandArgument='<%# Eval("Id") %>' />
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <sncore:accountreminder id="accountReminder" runat="server" style="width: 582px;" />
</asp:Content>
