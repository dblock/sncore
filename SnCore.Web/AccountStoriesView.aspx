<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountStoriesView.aspx.cs" Inherits="AccountStoriesView" Title="Stories" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="784">
    <tr>
     <td>
      <div class="sncore_h2">
       Stories
      </div>
      <div class="sncore_h2sub">
       <a href="AccountStoryEdit.aspx">&#187; Tell a Story</a>
       <asp:LinkButton ID="linkSearch" OnClick="linkSearch_Click" runat="server" Text="&#187; Search" />
       <asp:LinkButton ID="linkAll" OnClick="linkAll_Click" runat="server" Text="&#187; All Stories" />
       <SnCore:AccountContentGroupLink ID="linkAddGroup" runat="server" ConfigurationName="SnCore.AddContentGroup.Id" />
      </div>
     </td>
     <td>
      <asp:Label ID="labelCount" runat="server" />
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountStoriesRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelSearch" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PersistentPanel Visible="False" ID="panelSearchInternal" runat="server">
    <table class="sncore_table">
     <tr>
      <td class="sncore_form_label">
       search:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td class="sncore_form_value">
       <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CausesValidation="true" CssClass="sncore_form_button"
        OnClick="search_Click" />
      </td>
     </tr>
    </table> 
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="2" RepeatRows="3" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <table width="100%">
      <tr>
       <td width="150px" align="center">
        <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
         <img border="0" src="AccountStoryPictureThumbnail.aspx?id=<%# Eval("AccountStoryPictureId") %>" />
        </a>
        <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
         <div class="sncore_link">
          <%# base.Render(Eval("AccountName")) %>
         </div>
        </a>
       </td>
       <td width="*" align="left">
        <div>
         <a class="sncore_story_name" href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
          <%# base.Render(Eval("Name")) %>
         </a>
        </div>
        <div class="sncore_link">
         <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">&#187; read</a>
         <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">&#187; <%# GetComments((int) Eval("CommentCount")) %></a>
        </div>
        <div class="sncore_description">
          posted <%# SessionManager.ToAdjustedString((DateTime) Eval("Created")) %>
        </div>
       </td>
      </tr>
     </table>
    </itemtemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
