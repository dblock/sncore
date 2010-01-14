<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountGroupAccountInvitationEdit.aspx.cs" Inherits="AccountGroupAccountInvitationEdit"
 Title="Group | Join" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Invite a Friend
 </div>
 <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" runat="server" />
 <asp:Panel ID="panelInvite" runat="server">
  <table class="sncore_account_table">
   <tr>
    <td valign="top" class="sncore_table_tr_td" style="text-align: center;">
     <asp:Image ID="imageAccountGroup" ImageUrl="AccountGroupPictureThumbnail.aspx" runat="server" />
     <div class="sncore_link_description">
      <asp:HyperLink ID="linkAccountGroup" runat="server" />
     </div>
    </td>
    <td>
     <table>
      <tr>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputMessage" runat="server" TextMode="MultiLine"
         Rows="6" />
       </td>
      </tr>
     </table>
    </td>
   </tr>
  </table>     
  <div class="sncore_h2">
   Your Friends
  </div>  
  <table>
   <tr>
    <td class="sncore_form_label">
     search:
    </td>
    <td class="sncore_form_value">
     <asp:TextBox CssClass="sncore_form_textbox" ID="searchFriends" runat="server" />     
    </td>
   </tr>
   <tr>
    <td class="sncore_form_label">
    </td>
    <td class="sncore_form_value">
     <SnCoreWebControls:Button ID="search" runat="server" Text="Search" CssClass="sncore_form_button"
      OnClick="searchFriends_Click" />
    </td>
   </tr>
  </table>  
  <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
   <ContentTemplate>
    <SnCoreWebControls:PagedList CssClass="sncore_account_table" runat="server" ID="friendsList"
     ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-CssClass="sncore_table_tr_td"
     RepeatColumns="4" RepeatRows="2" AllowCustomPaging="true" OnItemCommand="friendsList_ItemCommand">
     <PagerStyle CssClass="sncore_table_pager" Position="Bottom" NextPageText="Next" PrevPageText="Prev"
		    HorizontalAlign="Center" />
     <ItemTemplate>
      <div>
       <asp:ImageButton id="friendImage" ImageUrl='<%# string.Format("AccountPictureThumbnail.aspx?id={0}", Eval("FriendPictureId")) %>' 
        runat="server"/> 
      </div>
      <div class="sncore_link_description">
       <asp:LinkButton id="linkSelectName" runat="server" Text='<%# base.Render(Eval("FriendName")) %>' CommandName="Select"
        CommandArgument='<%# Eval("FriendId") %>' />
      </div>
      <div class="sncore_link_description">
       <asp:LinkButton id="linkSelect" runat="server" Text='&#187; Select' CommandName="Select"
        CommandArgument='<%# Eval("FriendId") %>' />
      </div>
     </ItemTemplate>
    </SnCoreWebControls:PagedList>
   </ContentTemplate>
  </asp:UpdatePanel> 
 </asp:Panel>
</asp:Content>
