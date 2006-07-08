<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountStoriesView.aspx.cs" Inherits="AccountStoriesView" Title="Stories" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     Stories
    </div>
    <div class="sncore_h2sub">
     <a href="AccountStoryEdit.aspx">&#187; Tell a Story</a>
    </div>
    <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountStoriesRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountStoriesRss.aspx" />
   </td>
  </tr>
 </table> 
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    search:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputSearch" runat="server" />
    <asp:RequiredFieldValidator ID="inputSearchRequired" runat="server" ControlToValidate="inputSearch"
     CssClass="sncore_form_validator" ErrorMessage="search string is required" Display="Dynamic" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="search" runat="server" Text="Search!" CausesValidation="true" CssClass="sncore_form_button"
     OnClick="search_Click" />
   </td>
  </tr>
 </table> 
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
  AllowCustomPaging="true" RepeatColumns="2" RepeatRows="4" RepeatDirection="Horizontal"
  CssClass="sncore_table" ShowHeader="false">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" horizontalalign="Center" />
  <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="Center" Width="25%" />
  <ItemTemplate>
   <table width="100%">
    <tr>
     <td width="150px">
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
        posted on <%# base.Adjust(Eval("Created")).ToString("d") %>     
      </div>
     </td>
    </tr>
   </table>
  </itemtemplate>
 </SnCoreWebControls:PagedList>
</asp:Content>
