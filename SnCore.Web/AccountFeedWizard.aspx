<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedWizard.aspx.cs"
 Inherits="AccountFeedWizard" Title="Syndicate Wizard" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <sncore:accountmenu runat="server" id="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Syndicate Wizard
    </div>
    <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountFeedsManage.aspx"
     runat="server" />
    <asp:HyperLink ID="linkSkip" Text="&#187; Skip Wizard" CssClass="sncore_cancel" NavigateUrl="AccountFeedEdit.aspx"
     runat="server" />
    <atlas:UpdatePanel runat="server" ID="panelMain">
     <ContentTemplate>
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
        <asp:ButtonColumn ItemStyle-Width="100px" ButtonType="LinkButton" CommandName="Test" Text="&#187; Test" />
        <asp:ButtonColumn ItemStyle-Width="100px" ButtonType="LinkButton" CommandName="Choose" Text="&#187; Choose" />
       </Columns>
      </SnCoreWebControls:PagedGrid>
     </ContentTemplate>
    </atlas:UpdatePanel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table> 
</asp:Content>
