<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountRssWatchEdit.aspx.cs"
 Inherits="AccountRssWatchEdit" Title="Syndicated RssWatch" %>

<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountRedirectEdit" Src="AccountRedirectEditControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSyndicatedContent" Text="Subscribe" runat="server" ExpandedSize="100">  
  <Template>
   <div class="sncore_title_paragraph">
    Manage your content subscriptions via RSS.
    Really Simple Syndication (RSS) is a lightweight XML format designed for sharing headlines and other Web content.    
   </div>
  </Template>
 </SnCore:Title>
 <asp:LinkButton ID="linkBack" OnClick="linkBack_Click" Text="&#187; Cancel" CssClass="sncore_cancel"
  CausesValidation="false" runat="server" />
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    name:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputName" runat="server" CssClass="sncore_form_textbox" />
    <asp:RequiredFieldValidator ID="inputNameValidator" runat="server" ControlToValidate="inputName"
     CssClass="sncore_form_validator" ErrorMessage="subscription name is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td>
    <div class="sncore_link_small">
     name this subscription
    </div>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    url:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox ID="inputRssWatchUrl" runat="server" CssClass="sncore_form_textbox" />
    <asp:RequiredFieldValidator ID="inputRssWatchUrlValidator" runat="server" ControlToValidate="inputRssWatchUrl"
     CssClass="sncore_form_validator" ErrorMessage="rss url is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td>
    <div class="sncore_link_small">
     any valid relative RSS feed url on this site
     <br />
     see <a href="SiteMap.aspx">SiteMap</a> for a list
     or look for an RSS icon - copy-paste a shortcut
    </div>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    update:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList ID="inputUpdateFrequency" runat="server" CssClass="sncore_form_dropdown">
     <asp:ListItem Text="Weekly" Value="168" />
     <asp:ListItem Text="Twice a Week" Value="84" />
     <asp:ListItem Text="Twice a Day" Value="12" />
     <asp:ListItem Selected="true" Text="Daily" Value="24" />
     <asp:ListItem Text="Hourly" Value="1" />
    </asp:DropDownList>
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputEnabled" runat="server" Text="enable subscription"
     Checked="true" />
    <div class="sncore_link_small">
     notifications are sent to your <a href="AccountEmailsManage.aspx">primary e-mail</a>
    </div>
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
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
