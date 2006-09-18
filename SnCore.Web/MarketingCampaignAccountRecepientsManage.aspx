<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
 CodeFile="MarketingCampaignAccountRecepientsManage.aspx.cs" Inherits="MarketingCampaignAccountRecepientsManage" Title="Marketing Campaign Account Recepients" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     <asp:Label ID="campaignName" runat="server" Text="Recepients" />
    </div>
    <atlas:UpdatePanel ID="panelGrid" Mode="Always" runat="server">
     <ContentTemplate>
      <div class="sncore_h2sub">
       <a href="MarketingCampaignsManage.aspx">&#187; Cancel</a>
       <asp:LinkButton ID="deleteAllRecepients" OnClientClick="return confirm('Are you sure you want to do this?')"
        OnClick="deleteAllRecepients_Click" runat="server" Text="&#187; Delete All" />
      </div>
      <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManage" PageSize="15" AllowPaging="true"
       OnItemCommand="gridManage_ItemCommand" AutoGenerateColumns="false" AllowCustomPaging="true" 
       CssClass="sncore_account_table">
       <ItemStyle CssClass="sncore_table_tr_td" HorizontalAlign="center" />
       <HeaderStyle CssClass="sncore_table_tr_th" HorizontalAlign="center" />
       <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
        PrevPageText="Prev" HorizontalAlign="Center" />
       <Columns>
        <asp:BoundColumn DataField="Id" Visible="false" />
        <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
         <itemtemplate>
          <img src='<%# ((bool) Eval("Sent")) ? "images/Item.gif" : "images/Question.gif" %>' />
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Account Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
         <itemtemplate>
          <a href='AccountView.aspx?id=<%# Eval("Account.Id") %>'>
           <%# base.Render(Eval("Account.Name")) %>
          </a>
          <div class="sncore_description" style="color: Red;">
           <%# base.Render(Eval("LastError")) %>
          </div>
         </itemtemplate>
        </asp:TemplateColumn>
        <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
       </Columns>
      </SnCoreWebControls:PagedGrid>
      <div class="sncore_h2">
       Import
      </div>
      <div class="sncore_h3">
       All Users w/ Verified or Unverified E-Mails
      </div>
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">       
        </td>
        <td class="sncore_form_value">
         <asp:CheckBox ID="importAllVerifiedEmails" Checked="True" CssClass="sncore_form_checkbox" Text="Verified E-Mails" runat="server" />
        </td>
       </tr>
       <tr>
        <td class="sncore_form_label">       
        </td>
        <td class="sncore_form_value">
         <asp:CheckBox ID="importAllUnverifiedEmails" CssClass="sncore_form_checkbox" Text="Unverified E-Mails" runat="server" />
        </td>
       </tr>
       <tr>
        <td>
        </td>
        <td class="sncore_form_value">
         <SnCoreWebControls:Button ID="importAllUsers" runat="server" Text="Import" CausesValidation="true" CssClass="sncore_form_button"
          OnClick="importAllUsers_Click" />
        </td>
       </tr>
      </table>
      <div class="sncore_h3">
       Single User
      </div>
      <table class="sncore_account_table">
       <tr>
        <td class="sncore_form_label">
         user id(s):
        </td>
        <td class="sncore_form_value">
         <asp:TextBox ID="importSingleUserIds" CssClass="sncore_form_textbox" runat="server" />
         <div class="sncore_description">
          separate by spaces
         </div>
        </td>
       </tr>
       <tr>
        <td>
        </td>
        <td class="sncore_form_value">
         <SnCoreWebControls:Button ID="Button1" runat="server" Text="Import" CausesValidation="true" CssClass="sncore_form_button"
          OnClick="importSingleUser_Click" />
        </td>
       </tr>
      </table>
     </ContentTemplate>
    </atlas:UpdatePanel>
   </td>
  </tr>
 </table>
</asp:Content>
