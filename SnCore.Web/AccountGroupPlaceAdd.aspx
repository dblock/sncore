<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountGroupPlaceAdd.aspx.cs" Inherits="AccountGroupPlaceAdd" Title="GroupPlaces" %>
<%@ Import Namespace="SnCore.Services" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="RedirectView" Src="AccountRedirectViewControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table cellspacing="0" cellpadding="4" class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td_images">
    <asp:Image ID="placeImage" runat="server" />
   </td>
   <td valign="top" width="*">
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td class="sncore_table_tr_td">
       <asp:Label CssClass="sncore_place_name" ID="placeName" runat="server" />
       <div class="sncore_description">
        <asp:HyperLink ID="placeType" runat="server" />
       </div>
      </td>
      <td class="sncore_table_tr_td" valign="top" align="right">
       <asp:Label ID="placeId" CssClass="sncore_place_id" runat="server" />
      </td>
     </tr>
    </table>
    <div class="sncore_cancel">
     <asp:HyperLink ID="linkBack" runat="server" NavigateUrl="PlaceView.aspx" 
      Text="&#187; Cancel" />
     <a href="AccountGroupsView.aspx">&#187; Groups</a>
    </div>
    <table class="sncore_inner_table" width="95%">
     <tr>
      <td class="sncore_form_label">
       select:
      </td>
      <td class="sncore_form_value">
       <asp:DropDownList ID="listGroups" CssClass="sncore_form_textbox" runat="server"
        DataTextField="AccountGroupName" DataValueField="AccountGroupId" AutoPostBack="true"
        OnSelectedIndexChanged="listGroups_OnSelectedIndexChanged" />
      </td>
     </tr>
    </table>    
   </td>
  </tr>
 </table>
</asp:Content>
