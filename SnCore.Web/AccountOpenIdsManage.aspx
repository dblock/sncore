<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountOpenIdsManage.aspx.cs"
 Inherits="AccountOpenIdsManage" Title="Account | OpenIds" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleOpenIds" Text="My OpenIds" runat="server" ExpandedSize="120">  
  <Template>
   <div class="sncore_title_paragraph">
    OpenID is an open, decentralized, free framework for user-centric digital identity. With OpenID you
    don't have to remember multiple passwords for different sites any more. Find out more about OpenID 
    <a target="_blank" href="http://openid.net/">here</a> and <a target="_blank" href="http://videntity.org/">get one</a>. 
   </div>
  </Template>
 </SnCore:Title>
 <div class="sncore_cancel">
  <a target="_blank" href="http://openid.net/">&#187; What's This?</a> <a target="_blank"
   href="http://videntity.org/">&#187; Get One</a>
 </div>
 <asp:UpdatePanel id="panelManageUpdate" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table">
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="IdentityUrl" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/Item.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Identity Url" ItemStyle-HorizontalAlign="Left">
      <itemtemplate>
        <a target="_blank" href="<%# base.Render(Eval("IdentityUrl")) %>">
         <%# base.Render(Eval("IdentityUrl")) %>
        </a>
       </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
   <div class="sncore_h2">
    Add New</div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
      your OpenId:
     </td>
     <td class="sncore_form_value">
      <asp:TextBox CssClass="sncore_form_openid_textbox" ID="inputOpenIdIdentityUrl" runat="server" />
     </td>
    </tr>
    <tr>
     <td>
     </td>
     <td class="sncore_form_value">
      <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Add" CausesValidation="true"
       CssClass="sncore_form_button" OnClick="save_Click" />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
