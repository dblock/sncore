<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="AccountLicenseEdit.aspx.cs"
 Inherits="AccountLicenseEdit" Title="Account | Content | License" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Content Licensing
    </div>
    <table cellspacing="0" cellpadding="4" class="sncore_account_table">
     <tr>
      <td>
       <p>
        This site does not own your content. It is owned by <b>you</b>. This includes, but
        is not limited to all syndicated feeds, blog and discussion posts, stories and comments.
        You've made a work you're proud of. Now it's time to get creative with how you make
        it available.
       </p>
       <p>
        Creative Commons licenses help you share your work while keeping your copyright.
        Other people can copy and distribute your work provided they give you credit - and
        only on the conditions you specify here.
       </p>
      </td>
     </tr>
    </table>
    <atlas:UpdatePanel runat="server" ID="panelLicenseUpdate" Mode="Always">
     <ContentTemplate>
      <div class="sncore_h2">
       Current License
      </div>
      <table cellspacing="0" cellpadding="0" class="sncore_account_table" style="border: none;">
       <tr>
        <td>
         <div class="sncore_h2sub">
          <asp:LinkButton ID="linkDeleteLicense" OnClientClick="return confirm('Are you sure you want to delete the current license?');" 
           runat="server" OnClick="linkDeleteLicense_Click" Text="&#187; Delete License" />
          <asp:HyperLink ID="linkChooseLicense" runat="server" Text="&#187; Choose a License" />
         </div>
        </td>
        <td>
         <a runat="server" id="licenseLink" target="_blank">
          <img border="0" runat="server" id="licenseImage" />
         </a>
        </td>
       </tr>
      </table>
      <div class="sncore_cc_license" runat="server" id="licenseDiv">
       <asp:Label ID="licenseContent" runat="server" />
      </div>
     </ContentTemplate>
    </atlas:UpdatePanel>
   </td>
  </tr>
 </table>
</asp:Content>
