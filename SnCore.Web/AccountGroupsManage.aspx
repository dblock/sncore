<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true" CodeFile="AccountGroupsManage.aspx.cs"
 Inherits="AccountGroupsManage" Title="Account | Groups" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <SnCore:Title ID="titleGroups" Text="My Groups" runat="server" ExpandedSize="75">  
  <Template>
   <div class="sncore_title_paragraph">
    <a href="AccountGroupEdit.aspx">Create a Group</a> of friends, share places and focus discussions.
   </div>
  </Template>
 </SnCore:Title>
 <div class="sncore_createnew">
  <asp:HyperLink ID="HyperLink1" Text="&#187; Create New" NavigateUrl="AccountGroupEdit.aspx" runat="server" />
  <asp:HyperLink ID="linkAll" Text="&#187; All Groups" NavigateUrl="AccountGroupsView.aspx" runat="server" />
 </div>
 <asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Always">
  <ContentTemplate>
   <SnCoreWebControls:PagedGrid CellPadding="4" OnItemCommand="gridManage_ItemCommand"
    runat="server" ID="gridManage" AutoGenerateColumns="false" CssClass="sncore_account_table" 
    PageSize="10" AllowPaging="true" AllowCustomPaging="true">
    <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
    <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
    <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
     PrevPageText="Prev" HorizontalAlign="Center" />
    <Columns>
     <asp:BoundColumn DataField="AccountGroupId" Visible="false" />
     <asp:TemplateColumn>
      <itemtemplate>
       <img src="images/account/groups.gif" />
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderText="Group">
      <itemtemplate>
       <a href="AccountGroupView.aspx?id=<%# Eval("AccountGroupId") %>">
        <%# base.Render(Eval("AccountGroupName")) %>
       </a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountGroupView.aspx?id=<%# Eval("AccountGroupId") %>">View</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <a href="AccountGroupEdit.aspx?id=<%# Eval("AccountGroupId") %>">Edit</a>
      </itemtemplate>
     </asp:TemplateColumn>
     <asp:TemplateColumn>
      <itemtemplate>
       <asp:LinkButton id="linkDelete" CommandName="Delete" 
        OnClientClick="return confirm('This will delete the group, all membership information and discussion contents.\nThis cannot be undone! Are you sure you want to do this?');"
        CommandArgument='<%# Eval("AccountGroupId") %>' Text="Delete" runat="server" />
      </itemtemplate>
     </asp:TemplateColumn>
    </Columns>
   </SnCoreWebControls:PagedGrid>
  </ContentTemplate>
 </asp:UpdatePanel>
 <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
</asp:Content>
