<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AccountFeedsViewControl.ascx.cs"
 Inherits="AccountFeedsViewControl" %>
<%@ Register TagPrefix="SnCore" TagName="Notice" Src="NoticeControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<%@ Register TagPrefix="SnCore" TagName="FeedPreview" Src="AccountFeedPreviewControl.ascx" %>
<SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="accountFeeds"
 CssClass="sncore_inner_table" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false"
 Width="95%">
 <Columns>
  <asp:TemplateColumn>
   <itemtemplate>
    <div class="sncore_h2left">
     <a href='AccountFeedView.aspx?id=<%# Eval("Id") %>'>
      <%# base.Render(Eval("Name")) %>
     </a>
    </div>
    <SnCore:FeedPreview runat="server" FeedId='<%# Eval("Id") %>' />
   </itemtemplate>
  </asp:TemplateColumn>
 </Columns>
</SnCoreWebControls:PagedGrid>