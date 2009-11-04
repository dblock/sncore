<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DiscussionQuickPostControl.ascx.cs" Inherits="DiscussionQuickPostControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Panel ID="panelQuickPost" runat="server">
 <div class="sncore_quickpost">
  <asp:TextBox TextMode="MultiLine" Rows="3" ID="inputPost" runat="server" CssClass="sncore_form_textbox" />
  <br /><br />
  <SnCoreWebControls:Button ID="linkPost" CssClass="sncore_form_button" OnClick="post_Click" runat="server" Text="Post" />
 </div>
</asp:Panel>
