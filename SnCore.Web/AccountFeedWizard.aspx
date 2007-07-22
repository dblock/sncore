<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountFeedWizard.aspx.cs"
 Inherits="AccountFeedWizard" Title="Syndicate Wizard" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleSyndicatedContent" Text="Syndication Wizard" runat="server">  
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
 <asp:UpdatePanel runat="server" ID="panelMain">
  <ContentTemplate>
   <div class="sncore_cancel">
    <asp:LinkButton ID="linkBack" Text="&#187; Cancel" OnClick="linkBack_Click"
     runat="server" />
    <asp:HyperLink ID="linkSkip" Text="&#187; Skip Wizard" NavigateUrl="AccountFeedEdit.aspx"
     runat="server" />
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      your website or
      <br />
      feed address:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox ID="inputLinkUrl" runat="server" CssClass="sncore_form_textbox" />
     </td>
    </tr>
    <tr>
     <td class="sncore_form_label">
     </td>
     <td class="sncore_form_value">
      <div class="sncore_link_small">
       eg. http://myblog.blogserver.com/ or feed://myblog.blogserver.com/atom.xml
      </div>
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td>
      <sncorewebcontrols:button id="linkDiscover" cssclass="sncore_form_button" onclick="discover_Click"
       runat="server" text="Discover" />
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridFeeds" OnItemCommand="gridFeeds_ItemCommand"
    AutoGenerateColumns="false" CssClass="sncore_account_table">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Name" Visible="false" />
     <asp:BoundColumn DataField="FeedUrl" Visible="false" />
     <asp:BoundColumn DataField="LinkUrl" Visible="false" />
     <asp:BoundColumn DataField="Description" Visible="false" />
     <asp:TemplateColumn HeaderText="Feed" ItemStyle-HorizontalAlign="Left" 
      HeaderStyle-HorizontalAlign="Left">
      <itemtemplate>
       <div class="sncore_h2">
        <%# base.Render(Eval("Name")) %>
       </div>
       <div class="sncore_h2sub">
        <a href='<%# string.Format("AccountFeedTest.aspx?url={0}", Renderer.UrlEncode(Eval("FeedUrl"))) %>' target="_blank">
         &#187; Test
        </a>
        <asp:LinkButton id="linkChoose" runat="server" Text="&#187; Choose" CommandName="Choose" />
        <asp:LinkButton id="linkItemBack" runat="server" Text="&#187; Cancel" OnClick="linkBack_Click" 
         Visible='<%# ! string.IsNullOrEmpty(PreviousUrl) %>' />
       </div>
       <div style="font-size: smaller;">
        feed: <%# Renderer.GetLink(Renderer.Render(Eval("FeedUrl")), Renderer.Render(Eval("FeedUrl"))) %>
       </div>
       <div style="font-size: smaller;">
        link: <%# Renderer.GetLink(Renderer.Render(Eval("LinkUrl")), Renderer.Render(Eval("LinkUrl"))) %>
       </div>
       <div style="font-size: smaller;">
        description: <b><%# base.Render(Eval("Description")) %></b>
       </div>
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
