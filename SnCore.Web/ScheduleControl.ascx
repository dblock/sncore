<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScheduleControl.ascx.cs"
 Inherits="ScheduleControl" %>
<%@ Register TagPrefix="SnCore" TagName="SelectDate" Src="SelectDateControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="SelectTime" Src="SelectTimeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountTimeZone" Src="AccountTimeZoneControl.ascx" %>

 <SnCore:AccountTimeZone runat="server" ID="timezone" CssClass="sncore_account_table" />
 <asp:UpdatePanel runat="server" ID="panelSelectSchedule" UpdateMode="Conditional" RenderMode="Inline">
  <ContentTemplate>
   <table class="sncore_account_table">
    <tr>
     <td colspan="2">
      <div class="sncore_link" style="font-weight: bold;" id="panelWorking" runat="server">
      </div>
      <asp:Panel ID="panelButtons" CssClass="sncore_link" runat="server">
       <asp:LinkButton CausesValidation="false" ID="editCurrent" runat="server" Text="&#187; edit schedule"
        OnClick="editCurrent_Click" Enabled="false" />
       <asp:LinkButton CausesValidation="false" ID="addOneTime" runat="server" Text="&#187; schedule a one-time event"
        OnClick="addOneTime_Click" Enabled="false" />
       <asp:LinkButton CausesValidation="false" ID="addRecurrent" runat="server" Text="&#187; schedule a recurrent event"
        OnClick="addRecurrent_Click" Enabled="true" />
      </asp:Panel>
     </td>
    </tr>
   </table>
   <SnCoreWebControls:PersistentPanel ID="panelSchedule" runat="server">
    <SnCoreWebControls:PersistentPanel ID="panelStandard" runat="server">
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        start time:
       </td>
       <td class="sncore_form_value">
        <SnCore:SelectDate PastYears="0" FutureYears="10" RequiresSelection="true" ID="stdStartDate" runat="server" />
        <SnCore:SelectTime ID="stdStartTime" runat="server" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_label">
        <asp:Label ID="labelEndTime" runat="server" Text="end time:" />
       </td>
       <td class="sncore_form_value">
        <SnCore:SelectDate PastYears="0" FutureYears="10" RequiresSelection="true" ID="stdEndDate" runat="server" />
        <SnCore:SelectTime ID="stdEndTime" runat="server" />
       </td>
      </tr>
      <tr>
       <td>
       </td>
       <td class="sncore_form_value">
        <asp:CheckBox ID="stdAllDay" AutoPostBack="true" OnCheckedChanged="stdAllDay_CheckedChanged"
         runat="server" Text="all day event" />
        <asp:CheckBox ID="stdNoEndTime" AutoPostBack="true" OnCheckedChanged="stdNoEndTime_CheckedChanged"
         runat="server" Text="no end time" />
       </td>
      </tr>
     </table>
    </SnCoreWebControls:PersistentPanel>   
    <SnCoreWebControls:PersistentPanel ID="panelRecurrent" runat="server" Visible="false">
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label" style="width: auto;">
        start time:
       </td>
       <td class="sncore_form_value">
        <SnCore:SelectTime ID="recStartTime" runat="server" />
       </td>
       <td class="sncore_form_label" style="width: auto;">
        <asp:Label ID="labelRecEndTime" runat="server" Text="end time:" />
       </td>
       <td class="sncore_form_value">
        <SnCore:SelectTime ID="recEndTime" runat="server" />
       </td>
       <td class="sncore_form_value">
        <asp:CheckBox ID="recNoEndTime" AutoPostBack="true" OnCheckedChanged="recNoEndTime_CheckedChanged"
         runat="server" Text="no end time" />
       </td>
      </tr>
     </table>
     <table class="sncore_account_table">
      <tr>
       <td valign="top">
        <asp:RadioButtonList CssClass="sncore_form_radiobutton" AutoPostBack="true" ID="recPattern"
         runat="server">
         <asp:ListItem Text="Daily" Value="Daily" Selected="true" />
         <asp:ListItem Text="Weekly" Value="Weekly" />
         <asp:ListItem Text="Monthly" Value="Monthly" />
         <asp:ListItem Text="Yearly" Value="Yearly" />
        </asp:RadioButtonList>
       </td>
       <td colspan="5">
        <SnCoreWebControls:PersistentPanel ID="recDaily" runat="server" Visible="true">
         <div class="sncore_inner_div">
          <asp:RadioButton ID="recDaily_EveryNDays" runat="server" Text="Every" GroupName="recDailyType" />
          <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recDailyEveryNDays"
           Text="1" />
          day(s)
         </div>
         <div class="sncore_inner_div">
          <asp:RadioButton ID="recDaily_EveryWeekday" runat="server" Text="Every weekday" GroupName="recDailyType" />
         </div>
        </SnCoreWebControls:PersistentPanel>
        <SnCoreWebControls:PersistentPanel ID="recWeekly" runat="server" Visible="false">
         <div class="sncore_inner_div">
          Recur every
          <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recWeeklyEveryNWeeks"
           Text="1" />
          week(s) on:
         </div>
         <asp:CheckBoxList RepeatColumns="4" RepeatDirection="Horizontal" runat="server" ID="recWeeklyDaysOfWeek">
          <asp:ListItem Text="Sunday" Value="0" />
          <asp:ListItem Text="Monday" Value="1" />
          <asp:ListItem Text="Tuesday" Value="2" />
          <asp:ListItem Text="Wednesday" Value="3" />
          <asp:ListItem Text="Thursday" Value="4" />
          <asp:ListItem Text="Friday" Value="5" />
          <asp:ListItem Text="Saturday" Value="6" />
         </asp:CheckBoxList>
        </SnCoreWebControls:PersistentPanel>
        <SnCoreWebControls:PersistentPanel ID="recMonthly" runat="server" Visible="false">
         <div class="sncore_inner_div">
          <asp:RadioButton ID="recMonthly_DayNOfEveryNMonths" runat="server" Text="Day" GroupName="recMonthlyType"
           Checked="true" />
          <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recMonthlyDay" />
          of every
          <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recMonthlyMonth"
           Text="1" />
          month(s)
         </div>
         <div class="sncore_inner_div">
          <asp:RadioButton ID="recMonthly_NthWeekDayOfEveryNMonth" runat="server" Text="The"
           GroupName="recMonthlyType" />
          <asp:DropDownList CssClass="sncore_form_dropdown" Width="75px" ID="recMonthlyDayIndex"
           runat="server">
           <asp:ListItem Text="first" Selected="true" Value="1" />
           <asp:ListItem Text="second" Value="2" />
           <asp:ListItem Text="third" Value="3" />
           <asp:ListItem Text="fourth" Value="4" />
           <asp:ListItem Text="last" Value="-1" />
          </asp:DropDownList>
          <asp:DropDownList CssClass="sncore_form_dropdown" Width="75px" ID="recMonthlyDayName"
           runat="server">
           <asp:ListItem Text="Sunday" Value="0" />
           <asp:ListItem Text="Monday" Value="1" />
           <asp:ListItem Text="Tuesday" Value="2" />
           <asp:ListItem Text="Wednesday" Value="3" />
           <asp:ListItem Text="Thursday" Value="4" />
           <asp:ListItem Text="Friday" Value="5" />
           <asp:ListItem Text="Saturday" Value="6" />
           <asp:ListItem Text="day" Value="7" />
           <asp:ListItem Text="weekday" Value="8" />
           <asp:ListItem Text="weekend day" Value="9" />
          </asp:DropDownList>
          of every
          <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recMonthlyExMonth"
           Text="1" />
          month(s)
         </div>
        </SnCoreWebControls:PersistentPanel>
        <SnCoreWebControls:PersistentPanel ID="recYearly" runat="server" Visible="false">
         <div class="sncore_inner_div">
          <asp:RadioButton ID="recYearly_DayNOfMonth" runat="server" Text="Every" GroupName="recYearlyType"
           Checked="true" />
          <asp:DropDownList CssClass="sncore_form_dropdown" Width="75px" ID="recYearlyMonth"
           runat="server">
           <asp:ListItem Text="January" Value="1" />
           <asp:ListItem Text="February" Value="2" />
           <asp:ListItem Text="March" Value="3" />
           <asp:ListItem Text="April" Value="4" />
           <asp:ListItem Text="May" Value="5" />
           <asp:ListItem Text="June" Value="6" />
           <asp:ListItem Text="July" Value="7" />
           <asp:ListItem Text="August" Value="8" />
           <asp:ListItem Text="September" Value="9" />
           <asp:ListItem Text="October" Value="10" />
           <asp:ListItem Text="November" Value="11" />
           <asp:ListItem Text="December" Value="12" />
          </asp:DropDownList>
          <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recYearlyDay" />
         </div>
         <div class="sncore_inner_div">
          <asp:RadioButton ID="recYearly_NthWeekDayOfMonth" runat="server" Text="The" GroupName="recYearlyType" />
          <asp:DropDownList CssClass="sncore_form_dropdown" Width="75px" ID="recYearlyExDayIndex"
           runat="server">
           <asp:ListItem Text="first" Selected="true" Value="0" />
           <asp:ListItem Text="second" Value="1" />
           <asp:ListItem Text="third" Value="2" />
           <asp:ListItem Text="fourth" Value="3" />
           <asp:ListItem Text="last" Value="4" />
          </asp:DropDownList>
          <asp:DropDownList CssClass="sncore_form_dropdown" Width="75px" ID="recYearlyExDayName"
           runat="server">
           <asp:ListItem Text="Sunday" Value="0" />
           <asp:ListItem Text="Monday" Value="1" />
           <asp:ListItem Text="Tuesday" Value="2" />
           <asp:ListItem Text="Wednesday" Value="3" />
           <asp:ListItem Text="Thursday" Value="4" />
           <asp:ListItem Text="Friday" Value="5" />
           <asp:ListItem Text="Saturday" Value="6" />
           <asp:ListItem Text="day" Value="7" />
           <asp:ListItem Text="weekday" Value="8" />
           <asp:ListItem Text="weekend day" Value="9" />
          </asp:DropDownList>
          of
          <asp:DropDownList CssClass="sncore_form_dropdown" Width="75px" ID="recYearlyExMonth"
           runat="server">
           <asp:ListItem Text="January" Value="1" />
           <asp:ListItem Text="February" Value="2" />
           <asp:ListItem Text="March" Value="3" />
           <asp:ListItem Text="April" Value="4" />
           <asp:ListItem Text="May" Value="5" />
           <asp:ListItem Text="June" Value="6" />
           <asp:ListItem Text="July" Value="7" />
           <asp:ListItem Text="August" Value="8" />
           <asp:ListItem Text="September" Value="9" />
           <asp:ListItem Text="October" Value="10" />
           <asp:ListItem Text="November" Value="11" />
           <asp:ListItem Text="December" Value="12" />
          </asp:DropDownList>
         </div>
        </SnCoreWebControls:PersistentPanel>
       </td>
      </tr>
     </table>
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        start:
       </td>
       <td class="sncore_form_value">
        <SnCore:SelectDate PastYears="0" FutureYears="10" RequiresSelection="true" ID="recStartDate" runat="server" />
       </td>
      </tr>
     </table>
     <table class="sncore_account_table">
      <tr>
       <td class="sncore_form_label">
        end:
       </td>
       <td valign="top" class="sncore_form_value">
        <div class="sncore_inner_div">
         <asp:RadioButton ID="recNoEndDate" Checked="true" runat="server" Text="No end date"
          GroupName="recRange" />
        </div>
        <div class="sncore_inner_div">
         <asp:RadioButton ID="recEndAfter" runat="server" Text="End after" GroupName="recRange" />
         <asp:TextBox CssClass="sncore_form_textbox" Width="50px" runat="server" ID="recEndAfterNOccurences"
          Text="10" />
         occurrences
        </div>
        <div class="sncore_inner_div">
         <asp:RadioButton ID="recEndBy" runat="server" Text="End by" GroupName="recRange" />
         <SnCore:SelectDate PastYears="0" FutureYears="10" RequiresSelection="true" ID="recEndByDate" runat="server" />
        </div>
       </td>
      </tr>
     </table>
    </SnCoreWebControls:PersistentPanel>
   </SnCoreWebControls:PersistentPanel>
  </ContentTemplate>
 </asp:UpdatePanel>     
<asp:UpdatePanel runat="server" ID="panelConfirmedUpdate" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PersistentPanel ID="panelConfirmed" runat="server" Visible="false">
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_schedule_summary">
      <asp:Label ID="labelConfirmed" runat="server" />
     </td>
    </tr>
   </table>
  </SnCoreWebControls:PersistentPanel>
 </ContentTemplate>
</asp:UpdatePanel>
