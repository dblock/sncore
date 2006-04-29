<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedEdit.aspx.cs"
 Inherits="AccountFeedEdit" Title="Syndicated Feed" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Syndicate
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountFeedsManage.aspx"
     runat="server" />
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputName" runat="server" CssClass="sncore_form_textbox" />
       <asp:RequiredFieldValidator ID="inputNameValidator" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="feed name is required" Display="Dynamic" />
       <div class="sncore_link_small">
        name of your website or feed
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       description:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox TextMode="MultiLine" Rows="3" ID="inputDescription" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       type:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList ID="inputFeedType" runat="server" CssClass="sncore_form_dropdown"
        DataTextField="Name" DataValueField="Name" />
        <div class="sncore_link_small">
         choose <em>Generic RSS</em> if you're unsure
        </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       feed url:
       <div class="sncore_link_small">
        <a href="/docs/html/faq.html#faq_syndication_rss" target="_new">what's this?</a>
       </div> 
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputFeedUrl" runat="server" Text="http://" CssClass="sncore_form_textbox" />
       <asp:RequiredFieldValidator ID="inputFeedUrlValidator" runat="server" ControlToValidate="inputFeedUrl"
        CssClass="sncore_form_validator" ErrorMessage="feed url is required" Display="Dynamic" />
       <div class="sncore_link_small">
        the RSS feed url, supported types are RDF, RSS and ATOM
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       website url:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputLinkUrl" runat="server" CssClass="sncore_form_textbox" />
       <div class="sncore_link_small">
        a free-formed url to your syndicated content
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       username:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputUsername" runat="server" CssClass="sncore_form_textbox" />
       <div class="sncore_link_small">
        optional value, when required by RSS service
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       password:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputPassword" TextMode="Password" runat="server" CssClass="sncore_form_textbox" />
       <div class="sncore_link_small">
        optional value, when required by RSS service
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       update frequency:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList ID="inputUpdateFrequency" runat="server" CssClass="sncore_form_dropdown">
        <asp:ListItem Selected="true" Text="Twice a Day" Value="12" />
        <asp:ListItem Text="Daily" Value="24" />
        <asp:ListItem Text="Hourly" Value="1" />
       </asp:DropDownList>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputPublish" runat="server" Text="publish"
        Checked="true" />
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
   </td>
  </tr>
 </table>
</asp:Content>
