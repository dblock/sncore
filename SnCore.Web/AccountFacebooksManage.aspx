<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountFacebooksManage.aspx.cs"
 Inherits="AccountFacebooksManage" Title="Account | Facebook Identities" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleFacebooks" Text="My Facebook Identity" runat="server" ExpandedSize="120">  
  <Template>
   <div class="sncore_title_paragraph">
    Facebook is a social utility that connects people with friends and others who work, study and live around them.
    Find out more about Facebook <a target="_blank" href="http://www.facebook.com/">here</a>.
   </div>
  </Template>
 </SnCore:Title>
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
     <asp:BoundColumn DataField="FacebookAccountId" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/Item.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn HeaderText="Facebook Id" ItemStyle-HorizontalAlign="Left">
      <itemtemplate>
       <%# base.Render(Eval("FacebookAccountId"))%>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
   <div class="sncore_h2">
    Associate a Facebook Account
   </div>
   <div class="sncore_h2sub">
    Click "Connect" to associate an existing Facebook account.
   </div>
   <table class="sncore_account_table">
    <tr>
     <td class="sncore_form_label">
     </td>
     <td class="sncore_form_value">
        <asp:Panel ID="panelFacebookLogin" runat="server">
         <a href="<% Response.Write(FacebookLoginUri); %>">
          <img border="0" src="images/login/facebook.jpg" alt="Login with Facebook" />
         </a>
        </asp:Panel>
        <asp:Label ID="facebookLoginDisabled" runat="server" CssClass="sncore_notice_warning" Visible="false"
            Text="Facebook.APIKey has not been set, Facebook login disabled." />
     </td>
    </tr>
   </table>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
