<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountGroupsView.aspx.cs"
 Inherits="AccountGroupsView" Title="Groups" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RssLink" Src="RssLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" class="sncore_table_noborder">
    <tr>
     <td>
      <SnCore:Title ID="titleAccountGroups" Text="Groups" runat="server" ExpandedSize="100">  
       <Template>
        <div class="sncore_title_paragraph">
         These are public groups. Click <a href="AccountGroupEdit.aspx">here</a> to create a new group of your own.
        </div>
       </Template>
      </SnCore:Title>
      <div class="sncore_h2sub">
       <a href="AccountGroupEdit.aspx">&#187; Create a Group</a>
      </div>
     </td>
     <td align="right" valign="middle">
      <SnCore:RssLink ID="linkRelRss" runat="server" NavigateUrl="AccountGroupsRss.aspx" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" CssClass="sncore_table"
    ShowHeader="false" RepeatColumns="4" RepeatRows="3" RepeatDirection="Horizontal" 
    OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" />
    <ItemTemplate>
     <div class="sncore_link">
      <a href="AccountGroupView.aspx?id=<%# Eval("Id") %>">
       <img border="0" src="AccountGroupPictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
      </a>
     </div>
     <div class="sncore_link">
      <a href="AccountGroupView.aspx?id=<%# Eval("Id") %>">
       <%# base.Render(Eval("Name")) %>
      </a>
     </div>
     <div class="sncore_link">
      <a href="AccountGroupView.aspx?id=<%# Eval("Id") %>">
       &#187; view
      </a>
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
