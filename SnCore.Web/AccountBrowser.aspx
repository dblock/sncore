<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true"
    Title="Account | Preferences" %>

<%@ Register TagPrefix="SnCore" TagName="AccountMenu" Src="AccountMenuControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_inner_table">
  <tr>
   <td valign="top">
    <SnCore:AccountMenu runat="server" ID="menu" />
   </td>
   <td valign="top">
    <div class="sncore_h2">
     Browser Info
    </div>
    <ul>
     <li><% Response.Write(Request.ServerVariables["HTTP_USER_AGENT"].ToString()); %>
     <li><% Response.Write(string.Format("Type: {0}", Request.Browser.Type)); %>
     <li><% Response.Write(string.Format("Name: {0}", Request.Browser.Browser)); %>
     <li><% Response.Write(string.Format("Version: {0}", Request.Browser.Version)); %>
     <li><% Response.Write(string.Format("Major Version: {0}", Request.Browser.MajorVersion)); %>
     <li><% Response.Write(string.Format("Minor Version: {0}", Request.Browser.MinorVersion)); %>
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
   </td>
  </tr>
 </table>
</asp:Content>
