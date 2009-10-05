<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountRedirectEditControl.ascx.cs"
 Inherits="AccountRedirectEditControl" %>


<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="Title" Src="TitleControl.ascx" %>
<%@ Import Namespace="SnCore.Tools.Web" %>

<SnCore:Title ID="title" Text="Permanent Redirect" runat="server" ExpandedSize="100">  
 <Template>
  Establish a permanent presence with a unique friendly <b>keyword</b>. Users can navigate to /keyword on this website
  to find you.
 </Template>
</SnCore:Title>
<asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="panelPermanentRedirect">
 <ContentTemplate>
  <div class="sncore_h2sub">
   <asp:HyperLink ID="linkSource" Target="_blank" runat="server" Text="&#187; Test Redirect" />
  </div>
  <table class="sncore_account_table">
   <tr>
    <td class="sncore_form_label">
     keyword:
    </td>
    <td class="sncore_form_value">
     <ajaxToolkit:TextBoxWatermarkExtender ID="inputSourceEx" runat="server" TargetControlID="inputSource"
      WatermarkText="Enter a Unique Keyword" WatermarkCssClass="sncore_watermark" />
     <asp:TextBox ID="inputSource" runat="server" CssClass="sncore_form_textbox" />
    </td>
   </tr>
   <tr>
    <td>
    </td>
    <td>
     <SnCoreWebControls:Button ID="btnSave" CssClass="sncore_form_button" OnClick="save"
      runat="server" Text="Save" />
    </td>
   </tr>
  </table>
 </ContentTemplate>
</asp:UpdatePanel>
