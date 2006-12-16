<%@ Page Language="C#" MasterPageFile="~/SnCoreAccount.master" AutoEventWireup="true"
    Title="Account | Browser Information" CodeFile="AccountBrowser.aspx.cs" Inherits="AccountBrowser" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<%@ Register TagPrefix="SnCoreWebControls" Namespace="SnCore.WebControls" Assembly="SnCore.WebControls" %>
<asp:Content ID="Content" ContentPlaceHolderID="AccountContentPlaceHolder" runat="Server">
 <div class="sncore_h2">
  Browser Info
 </div>
 <ul>
  <li><% Response.Write(Request.ServerVariables["HTTP_USER_AGENT"].ToString()); %>
  <li><% Response.Write(string.Format("Type: {0}", Request.Browser.Type)); %>
  <li><% Response.Write(string.Format("Name: {0}", Request.Browser.Browser)); %>
  <li><% Response.Write(string.Format("Version: {0}", Request.Browser.Version)); %>
  (<% Response.Write(string.Format("Major Version: {0}", Request.Browser.MajorVersion)); %>,
   <% Response.Write(string.Format("Minor Version: {0}", Request.Browser.MinorVersion)); %>)
  <li><% Response.Write(string.Format("Platform: {0}", Request.Browser.Platform)); %>
  <li><% Response.Write(string.Format("Is Beta: {0}", Request.Browser.Beta)); %>
  <li><% Response.Write(string.Format("Is Crawler: {0}", Request.Browser.Crawler)); %>
  <li><% Response.Write(string.Format("Is AOL: {0}", Request.Browser.AOL)); %>
  <li><% Response.Write(string.Format("Is Win16: {0}", Request.Browser.Win16)); %>
  <li><% Response.Write(string.Format("Is Win32: {0}", Request.Browser.Win32)); %>
  <li><% Response.Write(string.Format("Supports Frames: {0}", Request.Browser.Frames)); %>
  <li><% Response.Write(string.Format("Supports Tables: {0}", Request.Browser.Tables)); %>
  <li><% Response.Write(string.Format("Supports Cookies: {0}", Request.Browser.Cookies)); %>
  <li><% Response.Write(string.Format("Supports VB Script: {0}", Request.Browser.VBScript)); %>
  <li><% Response.Write(string.Format("Ecma Script Version: {0}", Request.Browser.EcmaScriptVersion)); %>
  <li><% Response.Write(string.Format("Supports Java Applets: {0}", Request.Browser.JavaApplets)); %>
  <li><% Response.Write(string.Format("CDF: {0}", Request.Browser.CDF)); %>  
 </ul>
 <div class="sncore_h2">
  Cookies
 </div>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridCookies" AutoGenerateColumns="false" 
  CssClass="sncore_account_table" AllowPaging="false" AllowCustomPaging="false">
  <ItemStyle CssClass="sncore_table_tr_td" />
  <HeaderStyle CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:TemplateColumn>
    <ItemTemplate>
     <div>
      <b><%# Renderer.Render(Eval("Name")) %></b>
     </div>
     <div class="sncore_description">
      <%# Renderer.Render(GetSplitValue((string) Eval("Value"), 48)) %>
     </div>
    </ItemTemplate>
   </asp:TemplateColumn>
   <asp:TemplateColumn>
    <ItemTemplate>
     <div class="sncore_description">
      Expires: <%# Renderer.Render(Eval("Expires")) %>
     </div>
     <div class="sncore_description">
      Path: <%# Renderer.Render(Eval("Path")) %>
     </div>
     <div class="sncore_description">
      Domain: <%# Renderer.Render(Eval("Domain")) %>
     </div>
    </ItemTemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid> 
 <div class="sncore_h2">
  Headers
 </div>
 <SnCoreWebControls:PagedGrid CellPadding="4" runat="server" ID="gridHeaders" AutoGenerateColumns="false" 
  CssClass="sncore_account_table" AllowPaging="false" AllowCustomPaging="false">
  <ItemStyle CssClass="sncore_table_tr_td" />
  <HeaderStyle CssClass="sncore_table_tr_th" />
  <Columns>
   <asp:TemplateColumn>
    <ItemTemplate>
     <div>
      <b><%# Renderer.Render(Container.DataItem) %></b>
     </div>
     <div class="sncore_description">
      <%# Renderer.Render(GetSplitValue((string) Request.Headers[(string) Container.DataItem], 48)) %>
     </div>
    </ItemTemplate>
   </asp:TemplateColumn>
  </Columns>
 </SnCoreWebControls:PagedGrid> 
</asp:Content>
