<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
 Inherits="_Default" Title="Welcome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <ul>
  <li><a href="AccountsView.aspx">Foodies</a></li>
  <li><a href="PlacesView.aspx">Eat Out</a></li>
  <li><a href="DiscussionsView.aspx">Discussions</a></li>
  <li><a href="AccountFeedItemsView.aspx">Blog Roll</a></li>
  <li><a href="AccountFeedsView.aspx">Blog Directory</a></li>
  <li><a href="Search.aspx">Search</a></li>
  <li><asp:LinkButton ID="linkLoginLogout" runat="server" OnClick="linkLoginLogout_Click" /></li>
  <li><asp:HyperLink ID="linkStandard" runat="server" Text="Full Version" /></li>
 </ul>
</asp:Content>
