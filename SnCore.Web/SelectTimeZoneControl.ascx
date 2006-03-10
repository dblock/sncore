<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SelectTimeZoneControl.ascx.cs" Inherits="SelectTimezoneControl" %>
<asp:DropDownList ID="selectTimezoneControl" runat="server" CssClass="sncore_form_dropdown">
			<asp:ListItem Value="-12" Text="(GMT -12:00 hours)" />
			<asp:ListItem Value="-11" Text="(GMT -11:00 hours)" />
			<asp:ListItem Value="-10" Text="(GMT -10:00 hours) Hawaii" />
			<asp:ListItem Value="-9" Text="(GMT -9:00 hours) Alaska" />
			<asp:ListItem Value="-8" Text="(GMT -8:00 hours) Pacific Time (US & Canada)" />
			<asp:ListItem Value="-7" Text="(GMT -7:00 hours) Arizona" />
			<asp:ListItem Value="-7" Text="(GMT -7:00 hours) Mountain Standard Time (US & Canada)" />
			<asp:ListItem Value="-6" Text="(GMT -6:00 hours) Central Time (US & Canada)" />
			<asp:ListItem Value="-5" Text="(GMT -5:00 hours) Eastern Time (US & Canada)" />
			<asp:ListItem Value="-4" Text="(GMT -4:00 hours) Atlantic Time (Canada)" />
			<asp:ListItem Value="-3" Text="(GMT -3:00 hours)" />
			<asp:ListItem Value="-2" Text="(GMT -2:00 hours) Mid-Atlantic" />
			<asp:ListItem Value="-1" Text="(GMT -1:00 hours)" />
			<asp:ListItem Selected="true" Value="0" Text="(GMT) Western Europe Time, London" />
			<asp:ListItem Value="1" Text="(GMT +1:00 hours) CET(Central Europe Time)" />
			<asp:ListItem Value="2" Text="(GMT +2:00 hours) EET(Eastern Europe Time)" />
			<asp:ListItem Value="3" Text="(GMT +3:00 hours) Moscow" />
			<asp:ListItem Value="4" Text="(GMT +4:00 hours)" />
			<asp:ListItem Value="5" Text="(GMT +5:00 hours)" />
			<asp:ListItem Value="6" Text="(GMT +6:00 hours)" />
			<asp:ListItem Value="7" Text="(GMT +7:00 hours) Bangkok, Jakarta" />
			<asp:ListItem Value="8" Text="(GMT +8:00 hours) Singapore" />
			<asp:ListItem Value="9" Text="(GMT +9:00 hours) Tokyo" />
			<asp:ListItem Value="10" Text="(GMT +10:00 hours)" />
			<asp:ListItem Value="11" Text="(GMT +11:00 hours)" />
			<asp:ListItem Value="12" Text="(GMT +12:00 hours)" />
</asp:DropDownList>
