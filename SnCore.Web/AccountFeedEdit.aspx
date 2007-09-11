<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountFeedEdit.aspx.cs"
 Inherits="AccountFeedEdit" Title="Syndicated Feed" %>

<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AccountRedirectEdit" Src="AccountRedirectEditControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSyndicatedContent" Text="Syndicate" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Really Simple Syndication (RSS) is a lightweight XML format designed for sharing headlines and other Web content. 
    In particular, it allows you to syndicate content from your blog. This means that when you write
    a new post on your blog it will appear on this site as well, automatically, within a short period of time. 
   </div>
   <div class="sncore_title_paragraph">
    Because we have so many users, syndicating will bring you more readers. You still own all your content
    and can even <a href="AccountLicenseEdit.aspx">add a creative license</a> for it.
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
    <asp:DropDownList ID="selectType" runat="server" CssClass="sncore_form_dropdown"
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
    <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputPublish" runat="server" Text="publish content"
     Checked="true" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputPublishImgs" runat="server" Text="publish pictures"
     Checked="true" />
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
   </td>
   <td class="sncore_form_value">
    <asp:CheckBox CssClass="sncore_form_checkbox" ID="inputPublishMedia" runat="server" Text="publish media (podcasts, videos, etc.)"
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
 <SnCore:AccountRedirectEdit id="feedredirect" runat="server" />
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
