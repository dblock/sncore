<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="SystemPicturesManage.aspx.cs" Inherits="SystemPicturesManage" Title="Pictures" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Pictures
 </div>
 <table class="sncore_account_table">
  <tr>
   <td style="vertical-align: top;" class="sncore_form_label">
    add photos:
   </td>
   <td class="sncore_table_tr_td">
    <asp:DropDownList ID="selectPictureType" CssClass="sncore_form_dropdown" DataTextField="Name"
     DataValueField="Name" runat="server" />
    <br />
    <br />
    <WilcoWebControls:MultiFileUpload ID="files" runat="server" InputCssClass="sncore_form_upload"
     OnFilesPosted="files_FilesPosted" />
    <asp:HyperLink ID="addFile" runat="server" CssClass="sncore_form_label" NavigateUrl="#">+</asp:HyperLink>
    <br />
    <br />
    <SnCoreWebControls:Button ID="save" CssClass="sncore_form_button" runat="server" Text="Upload" />
   </td>
  </tr>
 </table>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
  OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_account_table">
  <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
   PrevPageText="Prev" HorizontalAlign="Center" />
  <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
  <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:BoundColumn DataField="Id" Visible="false" />
   <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
    <itemtemplate>
     <a href='SystemPictureView.aspx?id=<%# Eval("Id") %>'><img 
      border="0" src='SystemPictureThumbnail.aspx?id=<%# Eval("Id") %>&CacheDuration=0' />
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="Left">
    <itemtemplate>
     <%# base.Render(Eval("Name")) %>
     <div class="sncore_description">
      <%# base.Render(Eval("Description")) %>
     </div>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <itemtemplate>
     <a href='SystemPictureEdit.aspx?id=<%# Eval("Id") %>'>
      Edit
     </a>
    </itemtemplate>
   </asp:TemplateColumn>
   <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
  </Columns>
 </SnCoreWebControls:PagedGrid>
</asp:Content>
