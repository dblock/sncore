<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="CodeOfConduct.aspx.cs"
 Inherits="CodeOfConduct" Title="Code of Conduct" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td">
    <div class="sncore_h2">
     Code of Conduct
    </div>
    <div class="sncore_h2sub">
     <a href="PrivacyPolicy.aspx">&#187; Privacy Policy</a>
     <a href="TermsOfUse.aspx">&#187; Terms of Use</a>
    </div>
    <div class="sncore_h2sub">
      <p>
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       ("Service") offers a place to like-minded individuals to mingle via the
       internet and also through various activities that happen in the real (as opposed
       to virtual) world.
      </p>
      <p>
       By using the Service and web site (whether or not you choose to become a member)
       and attending any events sponsored by the Service, you agree to accept this Code
       of Conduct as part of the Service Agreement. In other words, this is a contract
       between you and us, just like the Terms of Use. You may review the terms of each
       contract at any time on this site.
      </p>
      <p>
       You also agree that we may modify terms of this agreement from time to time at our
       sole discretion, which we will do by updating the web page.
      </p>
      <p>
       If you do not agree with these terms as posted, you are prohibited from using the
       Service.
      </p>
      <p>
       <b>1. Conduct and Content</b>
      </p>
      <p>
       While using or accessing the Service, directly or indirectly, you agree to adopt
       a constructive tone and practice good etiquette and courtesy.</p>
      <p>
       More importantly, <b>you agree NOT to:</b>
      </p>
      <ul>
      <li>threaten, disrupt, inflame, intimidate, libel, stalk, defame, or defraud any individual,
       entity, or group on any basis including but not limited to age, gender, sexual orientation,
       ethnicity, race, religion or disability or encourage any one else to do so;
      </li>
       <li>post, publish, or transmit any text, graphics, or material that is illegal or
       violates local and national laws or that contains, encourages, advocates, or expresses:
       obscenity, child pornography, the making or buying of illegal weapons or illegal
       drugs, hatred, bigotry, racism, vulgarity or gratuitous violence or use of weapons;</li>
       <li>harm or exploit minors in any way including collecting personally identifiable
       information, including but not limited to name, email address, home address, phone
       number, or school;</li>
       <li>invade privacy by attempting to collect, store, or publish private or personally
       identifiable information, including, but not limited to, password, account information,
       credit card number, address, or other contact information;</li>
       <li>modify or falsify the source of any content or information, impersonate another
       individual, Biznik representative, or moderator, or falsely state or misrepresent
       affiliation with a person or entity; </li>
       <li>disrupt, harm or inappropriately access any computer, network, server or telecommunication
       system or violate related security or operational procedures, policies or regulations;</li>
       <li>flood, overload, or mail bomb any mail service or transmit or transfer any harmful
       or disruptive viruses, worms, computer code, files, or programs;</li>
       <li>infringe any patent, trademark, trade secret, copyright, or other proprietary
       rights or violate contractual or fiduciary relationships (such as insider, proprietary,
       or confidential information);</li>
       <li>encourage or instruct unauthorized copying or circumvention of copy protection
       or pirating of intellectual property or content including but not limited to software,
       music, text, photographs, illustrations, and movies;</li>
       <li>access restricted, password-only, or hidden pages or images (those not linked
       to or from another accessible page);</li>
       <li>distribute or disseminate inappropriate, unauthorized or unsolicited advertising
       or promotional offers including, but not limited to, contests, sweepstakes, barter,
       junk mail, spam, chain letters, and pyramid schemes;</li>
       <li>use Service messages or member profile content to obtain personally identifiable
       information, or to solicit or sell to any member inappropriately;</li>
       <li>support or help any organization designated by the United States government as
       a foreign terrorist organization pursuant to section 219 of the Immigration and
       Nationality Act;</li>
       <li>create "trolling" posts (trolling is deliberately posting false or provocative
       information in order to elicit responses from people who would not respond if they
       knew the motivation behind the post); or</li>
       <li>attempt (i) to access or search the Service with any engine, software, tool,
       agent, device, or mechanism other than the software or search agents provided by
        the Service or other generally available third party web browsers (such as Microsoft
       Internet Explorer or Mozilla Firefox); or (ii) to "frame," "mirror" or otherwise
       copy any portion of the Site without our express written authorization.</li>
       <li>include or create links or otherwise refer to external sites that violate these
       restrictions </li>
      </ul>
      <p>
       <b>2. Spam Is Prohibited</b>
      </p>
      <p>
       You will not use the Service to transmit, either directly or indirectly, or facilitate
       the transmission of any unsolicited bulk or commercial email ("Spam"). We may use
       filtering technology or other measures to stop incoming Spam. This filtering technology
       may block some email sent to you through the Service. This may happen even if the
       email does not violate the Service Agreement.
      </p>
      <p>
       You will not post content: in inappropriate categories, with excessive frequency;
       in multiple categories or multiple cities; or with identical or substantially identical
       text or photography. You will not use unrelated words in content to skew search
       results.
      </p>
      <p>
       <b>3. Child Pornography</b>
      </p>
      <p>
       In no event will you post, or seek, images of children, whether nude, clothed or
       partially clothed, in association with sexuality, in any area of the Service. We
       will attempt to remove any such postings and prohibits any postings or text containing
       or referring to child pornography, such as those containing the word "Lolita" or
       referring to nude photographs of "girls," "teens" or "children."
      </p>
      <p>
       <b>4. Violation</b></p>
      <p>
       While we retain the right to cancel or suspend your Service at any time without
       cause or notice, we consider the following to be a reasonable basis for termination:
       suspected violations of the Code of Conduct, this Service Agreement, or other guidelines;
       extended periods of inactivity; fraudulent or illegal activities; age misrepresentation;
       or nonpayment of any fees owed by you in connection with the Service.
      </p>
     </div>
   </td>
  </tr>
 </table>
</asp:Content>
