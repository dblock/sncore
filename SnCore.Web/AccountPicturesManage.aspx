<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountPicturesManage.aspx.cs"
 Inherits="AccountPicturesManage" Title="Account | Pictures" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     My Pictures
    </div>
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
    <atlas:UpdatePanel ID="panelGrid" runat="server" Mode="Always">
     <ContentTemplate>
      <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" RepeatDirection="Horizontal"
       ID="gridManage" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top"
       OnItemCommand="gridManage_ItemCommand" ItemStyle-CssClass="sncore_table_tr_td" RepeatColumns="3"
       RepeatRows="4" AllowCustomPaging="true">
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
       </ItemTemplate>
      </SnCoreWebControls:PagedList>
     </ContentTemplate>
    </atlas:UpdatePanel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
