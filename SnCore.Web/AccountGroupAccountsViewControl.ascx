'<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountGroupAccountsViewControl.ascx.cs"
 Inherits="AccountGroupAccountsViewControl" %>
<%@Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_h2">
 Group Members
</div>
<div class="sncore_h2sub">
 <asp:HyperLink runat="server" ID="linkAll" Text="&#187; All Members" />
</div>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" ID="friendsList"
   ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td"
   RepeatColumns="4" RepeatRows="1" AllowCustomPaging="true">
   <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next" PrevPageText="Prev"
		  HorizontalAlign="Center" />
   <ItemTemplate>
    <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
     <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>" />
     <div class="sncore_link_description">
      <asp:Image id="imgAdministrator" runat="server" ImageUrl="images/account/star.gif" Visible='<%# Eval("IsAdministrator") %>'
       Align="AbsMiddle" AlternateText="group administrator" />      
      <%# base.Render(Eval("AccountName")) %>
     </div>
    </a>
   </ItemTemplate>
  </SnCoreWebControls:PagedList>
 </ContentTemplate>
</asp:UpdatePanel>
