<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedsViewControl.ascx.cs"
 Inherits="AccountFeedsViewControl" %>
<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="FeedPreview" Src="AccountFeedPreviewControl.ascx" %>
<asp:UpdatePanel ID="panelGrid" runat="server" UpdateMode="Conditional" RenderMode="Inline">
 <ContentTemplate>
  <SnCoreWebControls:PagedGrid runat="server" ID="accountFeeds" BorderWidth="0" AutoGenerateColumns="false" 
   ShowHeader="false" >
   <Columns>
    <asp:TemplateColumn>
     <itemtemplate>
      <div class="sncore_h2">
       <a target="_blank" href='AccountFeedView.aspx?id=<%# Eval("Id") %>'>
        <%# base.Render(Eval("Name")) %>
       </a>
       <span class="sncore_link" style="font-size: xx-small;">
         <%# Renderer.GetLink(Renderer.Render(Eval("LinkUrl")), "&#187; x-posted") %>
        </span> 
      </div>
      <SnCore:FeedPreview runat="server" FeedId='<%# Eval("Id") %>' />
     </itemtemplate>
    </asp:TemplateColumn>
   </Columns>
  </SnCoreWebControls:PagedGrid>
 </ContentTemplate>
</asp:UpdatePanel>
