<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlacePropertyValuesViewControl.ascx.cs"
 Inherits="PlacePropertyValuesViewControl" %>
 
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<SnCoreWebControls:PagedList runat="server" ID="values" BorderWidth="0" AllowCustomPaging="false" 
 RepeatRows="5" RepeatDirection="Vertical" ShowHeader="false">
 <ItemTemplate>
  <a href='PlacesByPropertyValueView.aspx?GroupName=<%# Renderer.UrlEncode(GroupName) %>&PropertyName=<%# Renderer.UrlEncode(PropertyName) %>&PropertyValue=<%# Renderer.UrlEncode(Eval("Value")) %>'>
   <%# Renderer.Render(Eval("Value")) %>
  </a>
  (<%# Eval("Count") %>)
 </ItemTemplate>
</SnCoreWebControls:PagedList>
