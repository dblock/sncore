<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="DiscussionThreadMove.aspx.cs"
 Inherits="DiscussionThreadMove" Title="Discussion Thread" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_navigate">
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkDiscussions" NavigateUrl="DiscussionsView.aspx"
   Text="Discussions" runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkDiscussion" Text="Discussion"
   runat="server" />
  <asp:HyperLink CssClass="sncore_navigate_item" ID="linkThread" Text="Thread" runat="server" />
 </div>
 <div class="sncore_h2">
  Move Thread
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    destination:
   </td>
   <td class="sncore_form_value">
    <asp:DropDownList ID="listDiscussions" CssClass="sncore_form_dropdown" DataTextField="Name"
     DataValueField="Id" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="move" runat="server" Text="Move"
     CausesValidation="true" CssClass="sncore_form_button" OnClick="move_Click" />
   </td>
  </tr>
 </table>
</asp:Content>
