<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PlaceAttributesManage.aspx.cs"
 Inherits="PlaceAttributesManage" Title="Place | Attributes" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Place Attributes
    </div>
    <table cellspacing="0" cellpadding="4" class="sncore_account_table">
     <tr>
      <td class="sncore_table_tr_td" style="text-align: center; vertical-align: top; width: 100px;">
       <a runat="server" id="placeLink" href="PlaceView.aspx">
        <img border="0" src="images/PlaceThumbnail.gif" runat="server" id="placeImage" />
       </a>
      </td>
      <td valign="top" width="*">
       <asp:Label CssClass="sncore_place_name" ID="placeName" runat="server" />
      </td>
     </tr>
    </table>
    <asp:HyperLink ID="linkNew" Text="&#187; Create New" CssClass="sncore_createnew" NavigateUrl="PlaceAttributeEdit.aspx"
     runat="server" />
    <atlas:UpdatePanel ID="panelGrid" runat="server" Mode="Always">
     <ContentTemplate>
      <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
       runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table"
       AllowPaging="true" AllowCustomPaging="true" PageSize="5">
       <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
       <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
       <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
        PrevPageText="Prev" HorizontalAlign="Center" />
       <Columns>
        <asp:BoundColumn DataField="Id" Visible="false" />
        <asp:TemplateColumn HeaderText="Attribute">
         <itemtemplate>
          <a href='<%# string.IsNullOrEmpty((string) Eval("Url")) ? Render(Eval("Attribute.DefaultUrl")) : Render(Eval("Url")) %>'>
           <img src='SystemAttribute.aspx?id=<%# Eval("AttributeId") %>' border="0" 
            alt='<%# string.IsNullOrEmpty((string) Eval("Value")) ? Render(Eval("Attribute.DefaultValue")) : Render(Eval("Value")) %>' />
          </a>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn ItemStyle-Width="150">
         <itemtemplate>
          <a href="PlaceAttributeEdit.aspx?id=<%# Eval("Id") %>&aid=<%# Eval("PlaceId") %>">Edit</a>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:ButtonColumn ItemStyle-Width="150" ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
       </Columns>
      </SnCoreWebControls:PagedGrid>
     </ContentTemplate>
    </atlas:UpdatePanel>
   </td>
  </tr>
 </table>
</asp:Content>
