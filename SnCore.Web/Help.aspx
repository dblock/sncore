<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="Help.aspx.cs"
 Inherits="Help" Title="Help" %>
<%@ Register TagPrefix="SnCore" TagName="AccountContentGroupLink" Src="AccountContentGroupLinkControl.ascx" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <%@ register tagprefix="SnCore" tagname="Notice" src="NoticeControl.ascx" %>
 <div class="sncore_h2">
  Help
 </div>
 <table class="sncore_table">
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <a href="SiteMap.aspx">Site Map</a>
    <br />
    Quick links around the site.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <a href="docs/html/faq.html">Frequently Asked Questions</a>
    <br />
    Please read the F.A.Q. first before asking a question.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkSiteDiscussion" runat="server" Text="Site Discussion" />
    <br />
    Post any site-related questions to the Site Discussion.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkGraphX" runat="server" NavigateUrl="docs/html/logos.html" Text="Graphics and Logos" />
    <br />
    Link to us! Use a picture or logo.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkReportBug" runat="server" Text="Report a Bug" />
    <br />
    Report a bug. If something is clearly wrong, please report the bug in the tracking
    system.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkSuggestFeature" runat="server" Text="Suggest a Feature" />
    <br />
    Suggest a new feature.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkDev" runat="server" NavigateUrl="docs/html/index.html" Text="Developer Documentation" />
    <br />
    This software is built with open standards in mind. You can browse the <a href="Services.htm">
     Web Services API</a> and check out source code. Contributors are welcome.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:LinkButton ID="linkAdministrator" runat="server" Text="E-Mail the Administrator" />
    <br />
    E-mail the Administrator for any other question.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkDeleteAccount" runat="server" NavigateUrl="AccountDelete.aspx"
     Text="Delete My Account" />
    <br />
    Unsubscribe from this site. 
    Permanently delete your account.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <asp:HyperLink ID="linkAbout" runat="server" NavigateUrl="About.aspx" Text="About" />
    <br />
    Information about the system.
   </td>
  </tr>
  <tr>
   <td class="sncore_form_label">
    <img src="images/Item.gif" />
   </td>
   <td class="sncore_table_tr_td">
    <SnCore:AccountContentGroupLink ID="linkPress" ShowLinkPrefix="false" runat="server" 
     ConfigurationName="SnCore.PressContentGroup.Id" />
    <br />
    Press and cuts.
   </td>
  </tr>
 </table>
</asp:Content>
