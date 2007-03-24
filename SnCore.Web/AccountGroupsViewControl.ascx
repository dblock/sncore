<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountGroupsViewControl.ascx.cs" Inherits="AccountGroupsViewControl" %>

<%@Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Groups</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" ID="groupsList"
   ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td"
   RepeatColumns="4" AllowCustomPaging="true" RepeatRows="1">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next" PrevPageText="Prev"
		  HorizontalAlign="Center" />
   <ItemTemplate>
    <a href="AccountGroupView.aspx?id=<%# Eval("AccountGroupId") %>">
     <img border="0" src="AccountGroupPictureThumbnail.aspx?id=<%# Eval("AccountGroupPictureId") %>" />
     <div class="sncore_link_description">
      <%# base.Render(Eval("AccountGroupName")) %>
     </div>
    </a>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>
