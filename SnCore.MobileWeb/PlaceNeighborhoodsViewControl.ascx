<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceNeighborhoodsViewControl.ascx.cs"
 Inherits="PlaceNeighborhoodsViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<div class="sncore_place_neighborhoods">
 <SnCoreWebControls:PagedList runat="server" ID="values" BorderWidth="0" AllowCustomPaging="false"
  RepeatColumns="2" RepeatDirection="Vertical" ShowHeader="false" ItemStyle-Width="150"
  OnItemCommand="values_ItemCommand">
  <ItemTemplate>
   <asp:LinkButton ID="linkNeighborhood" runat="server" Text='<%# Eval("Name") %>' CommandName="Change"
    CommandArgument='<%# Eval("Name") %>' />
   (<%# Eval("Count") %>)
  </ItemTemplate>
 </SnCoreWebControls:PagedList>
</div>
