<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountAuditEntriesView.aspx.cs" Inherits="AccountAuditEntriesView" Title="User Activity" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  User Activity
 </div>
 <asp:UpdatePanel runat="server" ID="panelAuditEntries" UpdateMode="Always" RenderMode="Inline">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridActivity" 
    AllowCustomPaging="true" RepeatColumns="1" RepeatRows="7" RepeatDirection="Horizontal"
    CssClass="sncore_inner_table" ShowHeader="false">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemTemplate>
     <span class="sncore_message_tr_td_span">
      <div class="sncore_message">
       <div class="sncore_person" style="margin-top: 0px;">
        <a href="AccountView.aspx?id=<%# Eval("AccountId") %>">
         <img border="0" src="AccountPictureThumbnail.aspx?id=<%# Eval("AccountPictureId") %>&width=75&height=75" />
        </a>
       </div>
       <div class='sncore_content' style='width: 460px;'>
        <div class='sncore_message_body_halftable' style="padding: 10px 10px 10px 10px;">
         <%# RenderEx(Eval("Description")) %>
        </div>
        <div class="sncore_footer">
         <span class='<%# (DateTime.UtcNow.Subtract((DateTime) Eval("Updated")).TotalDays < 3) ? "sncore_datetime_highlight" : string.Empty %>'>
          &#187; <%# SessionManager.ToAdjustedString((DateTime) Eval("Updated")) %>
         </span>
         &#187; <a href='<%# Eval("Url") %>'>click</a>
        </div>
       </div>
      </div>
     </span>
    </ItemTemplate>
   </SnCoreWebControls:PagedList>
  </ContentTemplate>
 </asp:UpdatePanel>
</asp:Content>
