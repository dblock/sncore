<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountPlacesManage.aspx.cs" Inherits="AccountPlacesManage" Title="Places" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titlePlaces" Text="My Places" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    <a href="PlaceEdit.aspx">Suggest a new place</a> for everybody to see. Upload pictures and post reviews.
   </div>
  </Template>
 </SnCore:Title>
 <asp:HyperLink ID="linkSuggest" Text="&#187; Suggest a New Place" CssClass="sncore_createnew" NavigateUrl="PlaceEdit.aspx"
  runat="server" />
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedList CellPadding="4" runat="server" ID="gridManage" PageSize="10"
    AllowCustomPaging="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="sncore_account_table"
    ShowHeader="false" RepeatColumns="2" RepeatRows="3" RepeatDirection="Horizontal" OnItemCommand="gridManage_ItemCommand">
    <PagerStyle cssclass="sncore_table_pager" position="TopAndBottom" nextpagetext="Next"
     prevpagetext="Prev" horizontalalign="Center" />
    <ItemStyle CssClass="sncore_description" HorizontalAlign="Center" />
    <ItemTemplate>
     <table width="100%">
      <tr>
       <td>
        <div>
         <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
          <img border="0" src="PlacePictureThumbnail.aspx?id=<%# Eval("PictureId") %>" />
         </a>
        </div>
        <div>
         <a href="PlaceView.aspx?id=<%# Eval("Id") %>">
          <%# base.Render(Eval("Name")) %>
         </a>
        </div>
       </td>
       <td align="left">
        <div>
         <a href='PlaceEdit.aspx?id=<%# Eval("Id") %>'>&#187; Edit</a>
        </div>
        <div>
         <a href='PlacePicturesManage.aspx?id=<%# Eval("Id") %>'>&#187; Pictures</a>
        </div>
        <div>
         <a href='SystemAccountPlaceRequestsManage.aspx?id=<%# Eval("Id") %>'>&#187; Requests</a>
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
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
