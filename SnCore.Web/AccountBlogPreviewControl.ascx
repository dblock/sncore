<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountBlogPreviewControl.ascx.cs"
 Inherits="AccountBlogPreviewControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="1"
 RepeatRows="1" ShowHeader="false" AllowCustomPaging="false">
 <pagerstyle cssclass="sncore_table_pager" position="Bottom" nextpagetext="Next"
  prevpagetext="Prev" horizontalalign="Center" />
 <ItemTemplate>
  <div>
   <a href='AccountBlogPostView.aspx?id=<%# Eval("Id") %>'>
    <%# base.GetTitle(Eval("Title")) %>
   </a>
  </div>
  <div style="font-size: smaller;">
   <%# base.GetDescription(Eval("Body")) %>
  </div>
 </ItemTemplate>
</SnCoreWebControls:PagedList>
