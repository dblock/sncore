<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="PrivacyPolicy.aspx.cs"
 Inherits="PrivacyPolicy" Title="Privacy Policy" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content" ContentPlaceHolderID="ContentPlaceHolder" runat="Server">
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td">
    <div class="sncore_h2">
     Privacy Policy
    </div>
    <div class="sncore_h2sub">
     <a href="CodeOfConduct.aspx">&#187; Code of Conduct</a>
     <a href="TermsOfUse.aspx">&#187; Terms of Use</a>
    </div>
    <div class="sncore_h2sub">
      We take your privacy seriously. Please read the following to learn more about our
      privacy policy.
      <p>
       <b>1. What This Privacy Policy Covers</b><br />
      </p>
      <p>
       This policy covers how 
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       ("Service") treats personal information that 
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       collects and receives, including information related to your past use of our products
       and services. Personal information is information about you that is personally identifiable
       like your name, address, email address, or phone number, and that is not otherwise
       publicly available.
      </p>
      <p>
       This policy does not apply to the practices of companies that the service does not
       own or control, or to people that the Service does not employ or manage. In addition,
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       associated companies may have their own privacy statements.
      </p>
      <b>2. Information Collection and Use</b>
      <p>
       This Service collects personal information when you register with us, when you use
       the service, and when you visit Service pages.</p>
      <p>
       When you register we ask for information such as your name, email address, birth
       date, zip code and personal interests. Once you register with the Service and sign in, 
       you are not anonymous to us.</p>
      <p>
       The Service automatically receives and records information on our server logs from
       your browser, including your IP address, Service cookie information and the page
       you request.
      </p>
      <p>
       The Service uses information for the following general purposes: to customize the
       advertising and content you see, fulfill your requests for products and services,
       improve our services, contact you, conduct research, and provide anonymous reporting
       for internal and external clients.
      </p>
      <b>3. Children</b><br />
      <p>
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       prohibits usage by any individual under age of 18, enforced to our best effort.
      </p>
      <p>
       <b>4. Information Sharing and Disclosure</b></p>
      <p>
       We won't sell or share your information to anyone outside of the Service, period.
       We won't publish your email address, your physical address or your birthday.</p>
      <p>
       There may be exceptions to the above.</p>
      <p>
       We may be required respond to subpoenas, court orders, or legal process, or to establish
       or exercise our legal rights or defend against legal claims.&nbsp;</p>
      <p>
       We may believe it is necessary to share information in order to investigate, prevent,
       or take action regarding illegal activities, suspected fraud, situations involving
       potential threats to the physical safety of any person, violations of the terms
       of use, or as otherwise required by law.
      </p>
      <p>
       We transfer information about you if the Service is acquired by or merged with another
       company. In this event, we will notify you before information about you is transferred
       and becomes subject to a different privacy policy.
      </p>
      <p>
       <b>5. Confidentiality and Security </b>
      </p>
      <p>
       We limit access to personal information about you to employees who we believe reasonably
       need to come into contact with that information to provide products or services
       to you or in order to do their jobs.
      </p>
      <p>
       We have physical, electronic, and procedural safeguards that comply with international
       regulations to protect personal information about you.
      </p>
      <b>6. Changes to this Privacy Policy</b><br />
      <p>
       The Service may update this policy. We will notify you about significant changes
       in the way we treat personal information by sending a notice to the primary email
       address specified in your member account or by placing a prominent notice on our
       site.
      </p>
      <b>7. Questions and Suggestions </b>
      <br />
      <p>
       Please contact us at
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Admin.Email", "admin at localhost dot com").Replace("@", " at ").Replace(".", " dot "))); %>.     
      </p>
     </div>
   </td>
  </tr>
 </table>
</asp:Content>
