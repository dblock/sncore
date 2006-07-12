<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPlacesManage.aspx.cs" Inherits="AccountPlacesManage" Title="Places" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     My Places
    </div>
    <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="PlaceEdit.aspx"
     runat="server" />
    <atlas:UpdatePanel ID="panelGrid" runat="server" Mode="Always">
     <ContentTemplate>
      <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
       AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table"
       ShowHeader="false" RepeatColumns="2" RepeatRows="4" RepeatDirection="Horizontal" OnItemCommand="gridManage_ItemCommand">
       <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
        prevpagetext="Prev" horizontalalign="Center" />
       <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" />
       <ItemTemplate>
        <table width="100%">
         <tr>
          <td>
           <div>
            <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
             <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PlacePictureId") %>" />
            </a>
           </div>
           <div>
            <a href="PlaceView.aspx?id=<%# Eval("PlaceId") %>">
             <%# base.Render(Eval("PlaceName")) %>
            </a>
           </div>
          </td>
          <td align="left">
           <div>
            <a href='PlaceEdit.aspx?id=<%# Eval("PlaceId") %>'>&#187; Edit</a>
           </div>
           <div>
            <a href='PlacePicturesManage.aspx?id=<%# Eval("PlaceId") %>'>&#187; Pictures</a>
           </div>
           <div>
            <a href='SystemAccountPlaceRequestsManage.aspx?id=<%# Eval("PlaceId") %>'>&#187; Requests</a>
           </div>
           <div>
            <asp:LinkButton ID="linkDelete" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" 
             Text="&#187; Delete" OnClientClick="return confirm('Are you sure you want to do this?')" />
           </div>
          </td>
         </tr>
        </table>
       </ItemTemplate>
      </SnCoreWebControls:PagedList>
     </ContentTemplate>
    </atlas:UpdatePanel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
