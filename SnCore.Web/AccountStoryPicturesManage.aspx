<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountStoryPicturesManage.aspx.cs"
 Inherits="AccountStoryPicturesManage" Title="Story Pictures" %>

<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkStories" Text="Stories" NavigateUrl="AccountStoriesView.aspx" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccount" Text="Account" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkAccountStory" Text="Story" runat="server" />
 </div>
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
 <asp:UpdatePanel runat="server" id="panelStory" UpdateMode="Always">
  <ContentTemplate>
   <asp:DataList ItemStyle-HorizontalAlign="center" ItemStyle-Width="200px" RepeatColumns="4"
    runat="server" ID="gridManage" ShowHeader="false" CssClass="sncore_table" RepeatDirection="Horizontal"
    OnItemCommand="gridManage_ItemCommand">
    <ItemTemplate>
     <a href='AccountStoryPictureView.aspx?id=<%# Eval("Id") %>'>
      <img border="0" alt='<%# Eval("Name") %>' src='AccountStoryPictureThumbnail.aspx?id=<%# Eval("Id") %>' />
     </a>
     <div>
      <asp:LinkButton CssClass="sncore_link" ID="linkDelete" runat="server" CommandArgument='<%# Eval("Id") %>'
       CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this picture?');"
       Text="Delete" />
     </div>
     <div>
      <asp:LinkButton CssClass="sncore_link" ID="linkUp" runat="server" CommandArgument='<%# Eval("Id") %>'
       CommandName="Up" Text="<" />
      <asp:LinkButton CssClass="sncore_link" ID="linkDown" runat="server" CommandArgument='<%# Eval("Id") %>'
       CommandName="Down" Text=">" />
     </div>
    </ItemTemplate>
   </asp:DataList>
  </ContentTemplate>
 </asp:UpdatePanel>
 <sncore:accountreminder id="accountReminder" runat="server" style="width: 582px;" />
</asp:Content>
