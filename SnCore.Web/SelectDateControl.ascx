<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectDateControl.ascx.cs"
 Inherits="SelectDateControl" %>
<asp:DropDownList ID="selectdateYear" runat="server" CssClass="sncore_form_dropdown_auto" />
<asp:DropDownList ID="selectdateMonth" runat="server" CssClass="sncore_form_dropdown_auto" />
<asp:DropDownList ID="selectdateDay" runat="server" CssClass="sncore_form_dropdown_auto" />
<asp:RequiredFieldValidator ID="selectdateYearRequired" runat="server" ControlToValidate="selectdateYear"
 CssClass="sncore_form_validator" ErrorMessage="year is required" Display="None" />
<asp:RequiredFieldValidator ID="selectdateMonthRequired" runat="server" ControlToValidate="selectdateMonth"
 CssClass="sncore_form_validator" ErrorMessage="month is required" Display="None" />
<asp:RequiredFieldValidator ID="selectdateDayRequired" runat="server" ControlToValidate="selectdateDay"
 CssClass="sncore_form_validator" ErrorMessage="day is required" Display="None" />
