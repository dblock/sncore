<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedFeaturedViewControl.ascx.cs"
 Inherits="AccountFeedFeaturedViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<asp:Panel runat="server" ID="panelFeatured">
 <table cellpadding="2" cellspacing="0" class="sncore_featured_table">
  <tr>
   <td width="*" valign="top" class="sncore_featured_tr_td">
    <a runat="server" id="linkFeature2">
     <img border="0" id="imgFeature" runat="server" />
    </a>
    <div class="sncore_description">
     <a runat="server" id="linkFeature3">
      <asp:Label ID="labelFeatureName" runat="server" />
     </a>
    </div>
   </td>
  </tr>
 </table>
</asp:Panel>
