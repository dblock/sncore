<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountStoryEdit.aspx.cs"
 Inherits="AccountStoryEdit" Title="Account Story" %>

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
     Tell a Story
    </div>
    <asp:HyperLink Target="_blank" CssClass="sncore_createnew" Text="&#187; Preview" id="linkPreview" runat="server" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       title:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputName" runat="server" CssClass="sncore_form_textbox" />
       <asp:RequiredFieldValidator ID="inputNameValidator" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="what's cooking title is required"
        Display="Dynamic" />
      </td>
     </tr>
     <tr>
      <td valign="top" class="sncore_form_label">
       story:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputSummary" TextMode="MultiLine" Rows="20" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <SnCoreWebControls:Button ID="linkSave" CssClass="sncore_form_button" OnClick="save"
        runat="server" Text="Save" />
      </td>
     </tr>
    </table>
    <asp:Panel ID="panelImages" runat="server">
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
        <SnCoreWebControls:Button ID="picturesAdd" CssClass="sncore_form_button" runat="server"
         Text="Upload" />
       </td>
      </tr>
     </table>
     <asp:DataList ItemStyle-HorizontalAlign="center" ItemStyle-Width="200px" RepeatColumns="4"
      runat="server" ID="gridManage" ShowHeader="false" CssClass="sncore_account_table"
      OnItemCommand="gridManage_ItemCommand">
      <ItemTemplate>
       <a href='AccountStoryPictureView.aspx?id=<%# Eval("Id") %>'>
        <img border="0" alt='<%# Eval("Name") %>' src='AccountStoryPictureThumbnail.aspx?id=<%# Eval("Id") %>' />
       </a>
       <br />
       <asp:LinkButton CssClass="sncore_link" ID="linkDelete" runat="server" CommandArgument='<%# Eval("Id") %>'
        CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this picture?');"
        Text="Delete" />
       <br />
       <asp:LinkButton CssClass="sncore_link" ID="LinkButton1" runat="server" CommandArgument='<%# Eval("Id") %>'
        CommandName="Up" Text="<" />
       <asp:LinkButton CssClass="sncore_link" ID="LinkButton2" runat="server" CommandArgument='<%# Eval("Id") %>'
        CommandName="Down" Text=">" />
      </ItemTemplate>
     </asp:DataList>
    </asp:Panel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
