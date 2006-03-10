<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SearchSmallControl.ascx.cs"
 Inherits="SearchSmallControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:TextBox ValidationGroup="searchValidation" OnTextChanged="search_Click" CssClass="sncore_search_textbox"
 ID="inputSearch" runat="server" />
<asp:RequiredFieldValidator ID="inputSearchRequired" ValidationGroup="searchValidation"
 runat="server" ControlToValidate="inputSearch" CssClass="sncore_form_validator"
 ErrorMessage="search string is required" Display="None" />
<asp:ImageButton ValidationGroup="searchValidation" runat="server" ID="search" OnClick="search_Click"
 ImageAlign="AbsMiddle" ImageUrl="images/Search.gif" CssClass="sncore_search_link" />