<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountFeedItemView.aspx.cs"
 Inherits="AccountFeedItemView" Title="FeedItem" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <div class="sncore_links">
  &#187;
  <asp:HyperLink ID="feeditemXPosted" Text="x-posted" runat="server" />
  on
  <asp:Label ID="feeditemCreated" runat="server" />
  <asp:HyperLink ID="linkDiscussion" runat="server" Text="&#187; Comments" />
 </div>
 <div>
  <asp:Label ID="feeditemDescription" runat="server" />
 </div>
</asp:Content>
