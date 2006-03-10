<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedPreviewControl.ascx.cs"
 Inherits="AccountFeedPreviewControl" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" RepeatColumns="2"
 RepeatRows="3" ShowHeader="false" AllowCustomPaging="true">
 <pagerstyle cssclass="sncore_table_pager" position="Bottom" nextpagetext="Next"
  prevpagetext="Prev" horizontalalign="Center" />
 <ItemTemplate>
  <div>
   <a target="_blank" href='<%# base.Render(Eval("Link")) %>'>
    <%# base.GetTitle(Eval("Title")) %>
   </a>
  </div>
  <div style="font-size: smaller;">
   <%# base.GetDescription(Eval("Link"), Eval("Description")) %>
  </div>
 </ItemTemplate>
</SnCoreWebControls:PagedList>
