<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
 CodeFile="AccountFlagEdit.aspx.cs" Inherits="AccountFlagEdit" Title="Account | Flag Abuse" %>

<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <asp:Panel ID="panelMessage" runat="server">
  <SnCore:Title ID="titleReport" Text="Report" runat="server" ExpandedSize="200">  
   <Template>
    <div class="sncore_title_paragraph">
     If you are receiving spam in your mailbox or notice other kinds of behavior that you believe is violating the
     use policies of this website, we encourage you to report it so we can take action against the sender.
     By submitting this form, you are giving us permission to review the reported message and enter your account, 
     if necessary, for further investigation.
    </div>
    <div class="sncore_title_paragraph">
     Please note that we will only act upon reports of spam, such as commercial solicitations (including for 
     adult-oriented businesses), URLs, junk mail, etc.
     We cannot act upon any reports not related to spam (such as lewd emails, personal complaints about other 
     members, or correspondence that you may find personally objectionable but is not illegal or threatening).
    </div>
   </Template>
  </SnCore:Title>
  <asp:HyperLink ID="linkBack" Text="&#187; Cancel" CssClass="sncore_cancel" NavigateUrl="AccountFlagsManage.aspx"
   runat="server" />
  <asp:ValidationSummary runat="server" ID="manageValidationSummary" CssClass="sncore_form_validator"
   ShowSummary="true" />
  <table class="sncore_account_table">
   <tr>
    <td valign="top" class="sncore_table_tr_td" style="text-align: center;">
     <div>
      <asp:Image ID="imageKeen" ImageUrl="AccountPictureThumbnail.aspx" runat="server" />
     </div>
     <div>
      <asp:HyperLink ID="linkKeen" runat="server" />
     </div>
    </td>
    <td>
     <table>
      <tr>
       <td class="sncore_form_label">
        report:
       </td>
       <td class="sncore_form_value">
        <asp:DropDownList CssClass="sncore_form_dropdown" ID="inputType" DataTextField="Name"
         DataValueField="Name" runat="server" />
       </td>
      </tr>
      <tr>
       <td class="sncore_form_label">
        comment:
       </td>
       <td class="sncore_form_value">
        <asp:TextBox CssClass="sncore_form_textbox" ID="inputDescription" runat="server"
         TextMode="MultiLine" Rows="10" />
       </td>
      </tr>
      <tr>
       <td>
       </td>
       <td class="sncore_form_value">
        <SnCoreWebControls:Button ID="manageAdd" runat="server" Text="Report" CausesValidation="true" CssClass="sncore_form_button"
         OnClick="save_Click" />
       </td>
      </tr>
     </table>
    </td>
   </tr>
  </table>
 </asp:Panel>
</asp:Content>
