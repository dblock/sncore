<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedsView.aspx.cs"
 Inherits="AccountFeedsView" Title="Blog Directory" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="AToZ" Src="AToZControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Cities" Src="AccountCitiesControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <SnCore:Title ID="titleBlogs" Text="Blog Directory" runat="server">  
       <Template>
        <div class="sncore_title_paragraph">
         Do you have time to read two hundred blogs? These are <a href="AccountFeedsView.aspx">syndicated blogs</a>.
         You can read the combined blog posts <a href="AccountFeedItemsView.aspx">here</a>. It's a convenient way to 
         keep up with all this information. Blogs are updated several times a day. We also 
         extract and publish <a href="AccountFeedItemImgsView.aspx">pictures</a>,
         <a href="AccountFeedItemMediasView.aspx">podcasts and videos</a> from all posts.
        </div>
        <div class="sncore_title_paragraph">
         Do you have a blog? You can <a href="AccountFeedWizard.aspx">syndicate yours here</a>. It's free!
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountFeedItemsView.aspx">&#187; Blog Posts</a>
       <a href="AccountFeedItemImgsView.aspx">&#187; Pictures</a>
       <a href="AccountFeedItemMediasView.aspx">&#187; Podcasts &amp; Videos</a>
       <asp:LinkButton ID="linkAll" OnClick="linkAll_Click" runat="server" Text="&#187; All Blogs" />
       <asp:LinkButton ID="linkLocal" OnClick="linkLocal_Click" runat="server" Text="&#187; All Local Blogs" />
       <a href="AccountFeedWizard.aspx">&#187; Syndicate Yours</a>
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
       <asp:Hyperlink id="linkPermalink" NavigateUrl="AccountFeedsView.aspx" runat="server" Text="&#187; Permalink" />
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountFeedsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelSearch" UpdateMode="Conditional">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server" EnableViewState="true">
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputName" runat="server" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       country and state:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelCountryState" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown_small"
          ID="inputCountry" DataTextField="Name" AutoPostBack="true" DataValueField="Name"
          runat="server" />
         <asp:DropDownList CssClass="sncore_form_dropdown_small" ID="inputState"
          AutoPostBack="true" DataTextField="Name" DataValueField="Name" runat="server" />
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       city:
      </td>
      <td class="sncore_form_value">
       <asp:UpdatePanel runat="server" ID="panelCity" UpdateMode="Conditional">
        <ContentTemplate>
         <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputCity" DataTextField="Name"
          DataValueField="Name" runat="server" AutoPostBack="true" />
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox CssClass="sncore_form_checkbox" ID="checkboxPicturesOnly" runat="server"
        Text="show blogs with blogger pictures only" Checked="false" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
        OnClick="search_Click" EnableViewState="false" />
      </td>
     </tr>
    </table>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCore:AToZ id="atoz" runat="server" OnSelectedChanged="atoz_SelectedChanged" />
   <SnCore:Cities id="cities" runat="server" OnSelectedChanged="cities_SelectedChanged" />
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="2" RepeatRows="6" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <table width="100%">
      <tr>
       <td width="150px" align="center">
        <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">
         <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>&width=75&height=75" />
        </a>
        <div class="sncore_link">
         <a href="AccountFeedView.aspx?id=<%# Eval("Id") %>">
          <%# base.Render(Eval("AccountName")) %>
         </a>
        </div>
       </td>
       <td width="*" align="left">
        <div>
         <a href='AccountFeedView.aspx?id=<%# Eval("Id") %>'>
          <%# base.Render(Eval("Name")) %>
         </a>
         <span style="font-size: xx-small">
          <%# Renderer.GetLink(Renderer.Render(Eval("LinkUrl")), "&#187; x-posted") %>
         </span>
        </div>      
        <div class="sncore_description">
         <%# Renderer.GetSummary((string) Eval("Description")) %>
        </div>
       </td>
      </tr>
     </table>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
