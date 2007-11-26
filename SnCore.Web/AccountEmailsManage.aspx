<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountEmailsManage.aspx.cs"
 Inherits="AccountEmailsManage" Title="Account | E-Mails" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleEmailAddresses" Text="My Email Addresses" runat="server">  
  <Template>
   <div class="sncore_title_paragraph">
    Before you post anything you must have a verified e-mail address. This helps prevent spam.
    Your address is kept private. We will <b>never</b> give your address away to an advertiser, partner or 
    member without your explicit consent. Every time you add an e-mail address you receive a confirmation e-mail 
    with a link to click. Accounts with no verified e-mail addresses are deleted monthly.
   </div>
   <div class="sncore_title_paragraph">
    You can have multiple e-mail addresses and login with any of them. You can also set a principal
    one and all notifications will be sent to it. Otherwise they are sent to the first
    verified address.
   </div>
  </Template>
 </SnCore:Title>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemDataBound="gridManage_ItemDataBound"
    OnItemCommand="gridManage_ItemCommand" runat="server" ID="gridManage" AutoGenerateColumns="false"
    CssClass="sncore_account_table" AllowPaging="true" AllowCustomPaging="true" PageSize="5">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="Id" Visible="false" />
     <asp:BoundColumn DataField="Address" Visible="false" />
     <asp:BoundColumn DataField="Verified" Visible="false" />
     <asp:BoundColumn DataField="Principal" Visible="false" />
     <asp:BoundColumn DataField="Failed" Visible="false" />
     <asp:BoundColumn ReadOnly="True" DataFormatString="<img src='images/AccountEmailVerified{0}.gif'>"
      DataField="Verified"></asp:BoundColumn>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Center">
      <itemtemplate>
       <img src="images/question.gif" alt="<%# base.Render(Eval("LastError")) %>" 
        style='<%# (bool) Eval("Failed") ? string.Empty : "display: none;" %>' />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Email Address">
      <itemtemplate>
       <a href="mailto:<%# base.Render(Eval("Address")) %>">
        <%# base.Render(Eval("Address")) %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:ButtonColumn ButtonType="LinkButton" CommandName="Delete" Text="Delete" />
     <asp:ButtonColumn Visible="true" ButtonType="LinkButton" CommandName="Resend" DataTextField="Id"
      DataTextFormatString="Resend" />
     <asp:ButtonColumn Visible="true" ButtonType="LinkButton" CommandName="SetPrincipal"
      DataTextField="Id" DataTextFormatString="Set Principal" />
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <div class="sncore_h2">
  Add New</div>
 <table class="sncore_account_table">
  <tr>
   <td class="sncore_form_label">
    e-mail address:
   </td>
   <td class="sncore_form_value">
    <asp:TextBox CssClass="sncore_form_textbox" ID="inputEmailAddress" runat="server" />
   </td>
  </tr>
  <tr>
   <td>
   </td>
   <td class="sncore_form_value">
    <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Add" CssClass="sncore_form_button" 
     OnClick="save_Click" />
   </td>
  </tr>
 </table>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
