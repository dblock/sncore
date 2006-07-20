<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountContentGroupView.aspx.cs" Inherits="AccountContentGroupView" Title="Content Group" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table cellpadding="0" cellspacing="0" width="784">
  <tr>
   <td>
    <div class="sncore_h2">
     <asp:Label ID="labelName" runat="server" />
    </div>
   </td>
   <td>
    <asp:Label ID="labelDescription" CssClass="sncore_description" runat="server" />
   </td>
   <td align="right" valign="middle">
    <asp:HyperLink runat="server" ID="linkRss" ImageUrl="images/rss.gif" NavigateUrl="AccountContentGroupViewRss.aspx" />
    <link runat="server" id="linkRelRss" rel="alternate" type="application/rss+xml" title="Rss"
     href="AccountContentGroupViewRss.aspx" />
   </td>
  </tr>
 </table>
 <atlas:UpdatePanel runat="server" ID="panelGrid" Mode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage"
    AllowCustomPaging="true" RepeatColumns="1" RepeatRows="4" RepeatDirection="Horizontal"
    CssClass="sncore_table" BorderWidth="0" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_table_tr_td" />
    <ItemTemplate>
     <div class="sncore_message_table">
      <div class="sncore_message_body">
       <%# ((bool) Eval("AccountContentGroupTrusted")) ? Eval("Text") : base.RenderEx(Eval("Text"))  %>
      </div>
     </div>
    </itemtemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </atlas:UpdatePanel>   
</asp:Content>
