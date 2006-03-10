<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="AccountPicturesManage.aspx.cs" Inherits="AccountPicturesManage" Title="Account | Pictures" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="WilcoWebControls" Namespace="Wilco.Web.UI.WebControls" Assembly="Wilco.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <SnCore:AccountReminder ID="accountReminder" runat="server" />
    <div class="sncore_h2">
     My Pictures
    </div>
    <table class="sncore_account_table">
     <tr>
      <td style="vertical-align: top;" class="sncore_form_label">
       add photos:
      </td>
      <td>
       <WilcoWebControls:MultiFileUpload ID="files" runat="server" InputCssClass="sncore_form_upload"
        OnFilesPosted="files_FilesPosted" />
       <asp:HyperLink ID="addFile" runat="server" CssClass="sncore_form_label" NavigateUrl="#">+</asp:HyperLink>
       <br />
       <br />
       <SnCoreWebControls:Button ID="save" CssClass="sncore_form_button" runat="server" Text="Upload" />
      </td>
     </tr>
    </table>
    <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand" runat="server"
     ID="gridManage" AutoGenerateColumns="false" ShowHeader="false" CssClass="sncore_account_table">
     <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
      PrevPageText="Prev" HorizontalAlign="Center" />
     <Columns>
      <asp:BoundColumn DataField="Id" Visible="false" />
      <asp:BoundColumn DataField="Name" Visible="false" />
      <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
       <itemtemplate>
     <a href="AccountPictureEdit.aspx?id=<%# Eval("Id").ToString() %>">
      <img src="AccountPictureThumbnail.aspx?id=<%# Eval("Id").ToString() %>&CacheDuration=0" style="border: 0px;" />
     </a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-CssClass="sncore_table_tr_td" HeaderStyle-CssClass="sncore_table_tr_th">
       <itemtemplate>
     <b>
      <%# base.Render(Eval("Name")) %>
     </b>
     <br />
     <%# base.Render(Eval("Description")) %>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:TemplateColumn ItemStyle-HorizontalAlign="center" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th">
       <itemtemplate>
     <a href="AccountPictureEdit.aspx?id=<%# Eval("Id") %>">Edit</a>
    </itemtemplate>
      </asp:TemplateColumn>
      <asp:ButtonColumn ItemStyle-HorizontalAlign="Center" ButtonType="LinkButton" ItemStyle-CssClass="sncore_table_tr_td"
       HeaderStyle-CssClass="sncore_table_tr_th" CommandName="Delete" Text="Delete">
      </asp:ButtonColumn>
     </Columns>
    </SnCoreWebControls:PagedGrid>
   </td>
  </tr>
 </table>
</asp:Content>
