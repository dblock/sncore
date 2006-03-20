<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchDefaultControl.ascx.cs"
 Inherits="SearchDefaultControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<table cellpadding="0" cellspacing="0" class="sncore_half_inner_table">
 <tr>
  <td>
   <link rel="alternate" type="application/rss+xml" title="Rss" href="AccountsRss.aspx" />
   <div class="sncore_h2">
    <a href='Search.aspx'>
     Search
     <img src="images/site/right.gif" border="0" />
    </a>
   </div>
  </td>
 </tr>
 <tr>
  <td>
   <asp:Panel CssClass="sncore_createnew" ID="panellLinks" runat="server">
    <span class="sncore_link"><a href="Search.aspx">&#187; search people, places and
     more ...</a> </span>
   </asp:Panel>
  </td>
 </tr>
 <tr>
  <td style="padding-top: 10px; padding-left: 20px;">
   <asp:Panel ID="panelSearch" runat="server">
    <asp:TextBox OnTextChanged="search_Click" CssClass="sncore_default_search_textbox"
     ID="inputSearch" runat="server" />
    <asp:ImageButton runat="server" ID="search" OnClick="search_Click" ImageAlign="AbsMiddle"
     ImageUrl="images/Search.gif" CssClass="sncore_search_link" />
   </asp:Panel>
   <div class="sncore_link" style="display: none;" id="panelSearching" runat="server">
    searching ...
   </div>
  </td>
 </tr>
</table>
