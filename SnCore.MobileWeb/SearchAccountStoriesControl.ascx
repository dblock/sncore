<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchAccountStoriesControl.ascx.cs"
 Inherits="SearchAccountStoriesControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelStoriesResults" runat="server">
 <div class="sncore_h2">
  Stories
 </div>
 <asp:Label ID="labelResults" runat="server" CssClass="sncore_h2sub" />
 <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridResults"
  AllowCustomPaging="true" RepeatColumns="2" RepeatRows="4" RepeatDirection="Horizontal"
  CssClass="sncore_table" ShowHeader="false">
  <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
   prevpagetext="Prev" />
  <ItemStyle CssClass="sncore_table_tr_td" Width="25%" />
  <ItemTemplate>
   <table width="100%">
    <tr>
     <td width="150px">
      <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
       <img border="0" src="AccountStoryPictureThumbnail.aspx?id=<%# Eval("AccountStoryPictureId") %>" />
      </a>
      <a href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
       <div class="sncore_link">
        <%# Renderer.Render(Eval("AccountName")) %>
       </div>
      </a>
     </td>
     <td width="*" align="left">
      <div>
       <a class="sncore_story_name" href="AccountStoryView.aspx?id=<%# Eval("Id") %>">
        <%# Renderer.Render(Eval("Name")) %>
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
</asp:Panel>
