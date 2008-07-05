<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemImgsView.aspx.cs"
 Inherits="AccountFeedItemImgsView" Title="Pictures" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <asp:UpdatePanel ID="panelLinks" UpdateMode="Conditional" RenderMode="Inline" runat="server">
  <ContentTemplate>
   <table cellpadding="0" cellspacing="0" width="630">
    <tr>
     <td>
      <div class="sncore_h2">
       Pictures
      </div>
     </td>
     <td width="200">
      <asp:Label ID="labelCount" runat="server" CssClass="sncore_h2sub" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <asp:UpdatePanel runat="server" ID="panelGrid" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="4" RepeatRows="2" RepeatDirection="Horizontal"
    CssClass="sncore_table" ShowHeader="false" OnDataBinding="gridManage_DataBinding">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" Width="25%" />
    <ItemTemplate>
     <a href="AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %>">
      <img border="0" src="AccountFeedItemImgThumbnail.aspx?id=<%# Eval("Id") %>" alt='<%# Renderer.Render(Eval("Description")) %>' />
     </a>
     <div>
      x-posted in 
      <a href="AccountFeedView.aspx?id=<%# Eval("AccountFeedId") %>">
       <%# Renderer.Render(Eval("AccountFeedName"))%>
      </a>
     </div>
     <div>
      <a href="AccountFeedItemView.aspx?id=<%# Eval("AccountFeedItemId") %>">
       <%# Renderer.Render(Eval("AccountFeedItemTitle"))%>
      </a>    
     </div>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>   
</asp:Content>
