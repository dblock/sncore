<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountCityControl.ascx.cs"
 Inherits="AccountCityControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:HyperLink ID="welcome" runat="server" Visible="false" /> 
<asp:HyperLink ID="change" runat="server" Text="&#187; change" Visible="false" NavigateUrl="AccountPreferencesManage.aspx" />