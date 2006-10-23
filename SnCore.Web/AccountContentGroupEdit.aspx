<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountContentGroupEdit.aspx.cs"
 Inherits="AccountContentGroupEdit" Title="ContentGroup" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="AccountReminder" Src="AccountReminderControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td valign="top" width="150">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top" width="*">
    <div class="sncore_h2">
     Content Group
    </div>
    <div>
     <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountContentGroupsManage.aspx"
      runat="server" />
    </div>
    <table class="sncore_account_table">
     <tr>
      <td class="sncore_form_label">
       name:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox ID="inputName" runat="server" CssClass="sncore_form_textbox" />
       <asp:RequiredFieldValidator ID="inputNameValidator" runat="server" ControlToValidate="inputName"
        CssClass="sncore_form_validator" ErrorMessage="blog name is required" Display="Dynamic" />
       <div class="sncore_link_small">
        name of your content group
       </div>
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
       description:
      </td>
      <td class="sncore_form_value">
       <asp:TextBox Mode="MultiLine" Rows="3" ID="inputDescription" runat="server" CssClass="sncore_form_textbox" />
      </td>
     </tr>
     <tr>
      <td class="sncore_form_label">
      </td>
      <td class="sncore_form_value">
       <asp:CheckBox ID="inputTrusted" runat="server" Text="trusted content" CssClass="sncore_form_checkbox" />
      </td>
     </tr>
     <tr>
      <td>
      </td>
      <td>
       <SnCoreWebControls:Button ID="linkSave" CssClass="sncore_form_button" OnClick="save"
        runat="server" Text="Save" />
      </td>
     </tr>
    </table>
    <asp:Panel ID="panelContent" runat="server">
     <div class="sncore_h2">
      Contents
     </div>
     <div class="sncore_cancel">
      <asp:HyperLink ID="linkView" Text="&#187; Preview" NavigateUrl="AccountContentGroupView.aspx" 
       runat="server" />
      <asp:HyperLink ID="linkNew" Text="&#187; Create New" NavigateUrl="AccountContentEdit.aspx"
       runat="server" />
     </div>
     <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridManageContent"
      OnItemCommand="gridManageContent_ItemCommand" AutoGenerateColumns="false" CssClass="sncore_account_table"
      AllowPaging="true" AllowCustomPaging="true" PageSize="5">
      <ItemStyle HorizontalAlign="Center" CssClass="sncore_table_tr_td" />
      <HeaderStyle HorizontalAlign="Center" CssClass="sncore_table_tr_th" />
      <PagerStyle CssClass="sncore_table_pager" Position="TopAndBottom" NextPageText="Next"
       PrevPageText="Prev" HorizontalAlign="Center" />
      <Columns>
       <asp:BoundColumn DataField="Id" Visible="false" />
       <asp:TemplateColumn HeaderText="Content" ItemStyle-HorizontalAlign="Left">
        <itemtemplate>        
         <div class="sncore_link">
          <a href='AccountContentEdit.aspx?id=<%# Eval("Id") %>&gid=<%# base.RequestId %>'>
           &#187; <%# base.Render(Eval("Tag")) %>
           &#187; edit
          </a>
          &#187; <%# base.Adjust(Eval("Timestamp")).ToString("d") %>
          <asp:LinkButton id="linkDelete" Text="&#187; delete" runat="server" CommandName="Delete" 
           CommandArgument='<%# Eval("Id") %>' OnClientClick="return confirm('Are you sure you want to delete this content?')" />
         </div>
         <div class="sncore_message_body">
          <%# ((bool) Eval("AccountContentGroupTrusted")) ? Eval("Text") : base.RenderEx(Eval("Text"))  %>
         </div>
        </itemtemplate>
       </asp:TemplateColumn>
      </Columns>
     </SnCoreWebControls:PagedGrid>
    </asp:Panel>
    <SnCore:AccountReminder ID="accountReminder" runat="server" Style="width: 582px;" />
   </td>
  </tr>
 </table>
</asp:Content>
