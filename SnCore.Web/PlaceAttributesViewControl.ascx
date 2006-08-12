<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlaceAttributesViewControl.ascx.cs"
 Inherits="PlaceAttributesViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="attributes" RepeatRows="5" RepeatColumns="2" 
 RepeatDirection="Vertical" CssClass="sncore_inner_table" BorderWidth="0" ShowHeader="false" AllowCustomPaging="true">
 <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
  prevpagetext="Prev" horizontalalign="Center" />
 <ItemTemplate>
  <a href='<%# string.IsNullOrEmpty((string) Eval("Url")) ? Render(Eval("Attribute.DefaultUrl")) : Render(Eval("Url")) %>'>
   <img src='SystemAttribute.aspx?id=<%# Eval("AttributeId") %>' border="0" 
    alt='<%# string.IsNullOrEmpty((string) Eval("Value")) ? Render(Eval("Attribute.DefaultValue")) : Render(Eval("Value")) %>' />
  </a>
 </ItemTemplate>
</SnCoreWebControls:PagedList>