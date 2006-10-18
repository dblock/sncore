<%@ Page Language="C#" MasterPageFile="~/SnCore.master" AutoEventWireup="true" CodeFile="TermsOfUse.aspx.cs"
 Inherits="TermsOfUse" Title="Terms of Use" %>

<%@ Import Namespace="SnCore.Tools.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <table class="sncore_table">
  <tr>
   <td class="sncore_table_tr_td">
    <div class="sncore_h2">
     Terms of Use
    </div>
    <div class="sncore_h2sub">
     <a href="PrivacyPolicy.aspx">&#187; Privacy Policy</a>
     <a href="CodeOfConduct.aspx">&#187; Code of Conduct</a>
    </div>
    <div class="sncore_h2sub">
     <p>
      The
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      service includes the website at
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/"))); %>
      ("Service"), and is organized primarily to help members network with other like-minded
      individuals and businesses.
     </p>
     <p>
      <b>1. Acceptance Triggered By Use</b>
     </p>
     <p>
      BY USING THE SERVICE, YOU AGREE TO BE BOUND BY THE TERMS AND CONDITIONS OF THIS
      TERMS OF USE CONTRACT ("Service Agreement"), which is between you and Vestris Inc.,
      a British Virgin Islands company. Vestris Inc. is a registered British Virgin Islands company at 
      Trident Chambers, P.O. Box 146, Wickhams Cay, Road Town, Tortola, British Virgin Islands.
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      is owned by Vestris Inc., and is alternately referred to as "<% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>",
      "we" or "our" in this Service Agreement. This contract is triggered by use of this
      site and by viewing content, and exists regardless of whether or not you are or
      choose to become a member of this Service. Your access to and use of the Service
      is expressly conditioned on your compliance with these terms and conditions.
     </p>
     <p>
      <b>Please note that this Service Agreement limits our liability and we do not provide
       warranties for the Service. This Service Agreement also limits your remedies. These
       warranty disclaimers and limitations are covered in Sections 15 and 16 and we urge
       you to read them carefully.</b>
     </p>
     <p>
      We may modify these terms from time to time at our sole discretion, as set forth
      in Section 9. You agree to check these terms regularly and accept all modified terms.
      You may review the current version of this Service Agreement at <a href='AccountTermsOfUse.aspx'>
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/"))); %>
      </a>.</p>
     <p>
      <b>2. Registration as a Member</b>
     </p>
     <p>
      By registering as a member, you confirm your acceptance of this agreement and warrant
      that you are at least 18 years old. Your membership is solely for your individual
      use and you will not authorize others to use your account, profile, or messages.
      You are solely responsible for all use of your account including, but not limited
      to, content published, messages sent or posted, and interactions with other members,
      whether or not you have authorized such activities. You agree to maintain the accuracy
      of your registration information and to keep account information confidential. You
      must tell us right away <b>in writing</b> about anyone besides you using your account,
      or any security breach that relates to the Service. You understand and agree that
      we have no obligation to prescreen, manage, or edit content made available through
      the service. You agree to bear all risk associated with content you or others provide
      using your account. Membership in the Service is void where prohibited.
     </p>
     <p>
      <b>3. Monitoring and Disclosure</b>
     </p>
     <p>
      We may monitor your communications and may disclose content and information about
      you, including contents of communications, if we deem it reasonably necessary to:
      (1) conform to legal requirements or respond to legal process; (2) ensure your compliance
      with this Service Agreement including maintaining this Service and the Code of Conduct;
      or (3) protect the rights, property, personal safety or interests of
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      , its employees, its customers, or the public.
     </p>
     <p>
      <b>4. Code of Conduct</b>
     </p>
     <p>
      You will not use the Service in a way that is against the law or harms
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>, 
      our members, partners, affiliates, service providers, partners or suppliers. We
      may tell you about harmful uses but have no duty to do so. You will comply with
      the Code of Conduct and other notices we provide. You may review the Code of Conduct
      at any time at <a href="CodeOfConduct.aspx">
       <% Response.Write(Renderer.Render(new Uri(Request.Url, "CodeOfConduct.aspx"))); %>
      </a>. Violations of the Code of Conduct may result in the termination of access
      or deletion of content without notice. We reserve the right to cancel, suspend,
      or block access to the Service in whole or part at any time.
     </p>
     <p>
      <b>5. Content</b>
     </p>
     <p>
      We do not claim ownership of the content you post or otherwise provide to the Service.
      We offer you the opportunity to <a href="AccountLicenseEdit.aspx">choose a creative license</a> 
      for all your content. However, if you don't, you hereby grant, and agree to grant as an effect of 
      posting or otherwise providing content, the following license: to the public, a license for personal
      non-commercial use; and to
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>,
      a perpetual license to use, copy, distribute, display, perform, and modify any
      and all content that you post on the Service. This license cannot be withdrawn except
      that any content deleted from the
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      site will terminate the
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      license. Your use of the service is in consideration for this license, and we will
      not otherwise pay you for your content. You agree that you have not granted and
      will not grant any rights in content inconsistent with this license.
     </p>
     <p>
      <b>6. Content Deletion</b>
     </p>
     <p>
      We have no obligation to maintain or store your content. We have the right to delete
      any content for any reason, including, but not limited to, service cancellation,
      violation of the Service Agreement, violation of the Code of Conduct. Once deleted,
      any content you have stored on the service cannot be retrieved.
     </p>
     <p>
      <b>7. Limitations</b>
     </p>
     <p>
      You acknowledge that we may establish limits concerning use of the Service, including
      the length, number or size of photos, email messages, postings, or other content
      that are retained, stored, displayed, or transmitted by the Service. We may also
      limit the frequency with which you may access the Service if it appears excessive
      or automated. We reserve the right to modify or discontinue the Service (or any
      part or feature thereof) at any time, with or without notice.
     </p>
     <p>
      <b>8. Access to the Service</b>
     </p>
     <p>
      We offer parts of the Service in RSS format and through a rich web services API so that 
      users can embed individual feeds into a personal website or weblog or view postings through 
      third party software news aggregators, build "mash-ups", etc. We permit you to display, 
      excerpt from, and link to the source of information; provided each title is correctly linked back 
      to the Service; and your use does not overburden our systems. We may terminate any such service or feed at any time. 
      We prohibit access to the Service for data collection, aggregation, mining or extraction. We authorize an
      exception for general purpose internet search engines and non-commercial public
      archives that use such tools to gather information for the sole purpose of displaying
      hyperlinks to the Service, provided they each do so from a stable IP address or
      range of IP addresses using an easily identifiable agent. This exception excludes
      websites or search engines that specialize in listings, postings, profiles, discussion
      groups or subsets such as jobs, housing, for sale, or services, or are in the business
      of providing ad listing services.
     </p>
     <p>
      <b>9. Changes to this Service Agreement</b>
     </p>
     <p>
      If we change this Service Agreement, then we will notify you of the change online
      through a post in the service blog. If you do not agree with the changed Service Agreement,
      then you must stop using the Service within two weeks of notification. You agree
      that you are solely responsible for reviewing the terms of this Service Agreement
      from time to time. If you choose not to accept the terms as revised and posted,
      you have a single two-week grace period from the date that you first visited the
      Service (with the revised terms posted), during which your use will be governed
      under the then-prior version of this Service Agreement and after which you agree
      not to use the Service.
     </p>
     <p>
      <b>10. Termination</b>
     </p>
     <p>
      You may cancel the Service at any time for any reason. We may cancel, suspend, or
      block your access to the Service at any time, with immediate effect. This may be
      without cause and without notice. <b>Once the Service is cancelled, you will no longer
       be able to access or retrieve any data you had stored on the Service.</b>
     </p>
     <p>
      <b>11. Proprietary Rights</b>
     </p>
     <p>
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      owns and retains, to the fullest extent of the law, all intellectual property and
      rights in the Service including, but not limited to, copyrighted material, trademarks,
      patented processes, trade secrets, and other proprietary information. You will not
      distribute, transmit, transfer, offer, copy, publish, display, modify, or sell,
      assign, or sublicense any portion of the Service, its code, or any proprietary information
      on the Service, or any intellectual property or other proprietary right in any of
      the foregoing. You will not reverse engineer, reverse assemble, or otherwise attempt
      to discover any source code or proprietary information, or obtain unauthorized access
      to the Service. The foregoing does not apply to user content.
     </p>
     <p>
      <b>12. Infringement</b>
     </p>
     <p>
      If you believe that your work has been used in a way that constitutes copyright
      infringement, or your intellectual property rights have been otherwise violated,
      please notify
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      at
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Admin.Email", "admin at localhost dot com").Replace("@", " at ").Replace(".", " dot "))); %>.
      Provide all of the following in writing: identify the copyrighted work that you
      claim has been infringed (or if multiple copyrighted works, then a representative
      list of such works); identify the content on the Service that you claim is infringing
      with enough detail so that we may locate it; provide a statement by you that you
      have a good faith belief that the disputed use is not authorized by the copyright
      owner, its agent, or the law; provide a statement by you declaring that the notification
      is accurate, and, under penalty of perjury, that you are the exclusive owner of
      the copyright interest involved or that you are authorized to act on behalf of the
      exclusive owner; provide information reasonably sufficient to permit
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      to contact you, such as an address, telephone number, and email address; and your
      physical or electronic signature.
     </p>
     <p>
      Upon receipt of notice as described above,
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      will take whatever action, in its sole discretion, it deems appropriate, including
      removal of the challenged use from the Service or termination of the posting account.
     </p>
     <p>
      <b>13. Advertising and Fees</b>
     </p>
     <p>
      You understand and agree that the Service may include advertisements and commercial
      listings and that these are necessary for us to provide the Service. You will not
      obscure these from general view via HTML/CSS or any other means. By using the Service
      and by becoming a Member, you acknowledge that we reserve the right to charge for
      the Service or a portion of the Service and have the right to terminate a membership
      for failure to pay for the Service.
     </p>
     <p>
      <b>14. Indemnity</b>
     </p>
     <p>
      Members agree to indemnify and hold
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      and its affiliates, officers, agents or other partners, and employees, harmless
      from any claim or demand, including reasonable attorneys' fees, made by any third
      party due to or arising out of (i) content you submit, post, transmit, or make available
      through the Service, (ii) your use of the Service, (iii) your connection to the
      Service, (iv) your violation of the Service Agreement or Code of Conduct, or (v)
      your violation of any rights of another including but not limited to another's copyright
      or other intellectual property right.
     </p>
     <p>
      <b>15. NO WARRANTY</b>
     </p>
     <p>
      <b>We provide the Service "as-is," with all faults and as available. We give no express
       warranties, guarantees, or conditions. To the extent permitted by law, we disclaim
       any implied warranties including those of merchantability, fitness for a particular
       purpose, workmanlike effort, and non-infringement and including those arising by
       usage of trade, course of dealing, or course of performance. Without limiting the
       generality of the foregoing, we do not warrant that the service or the content will
       be accurate, error-free, virus-free, or uninterrupted or that it will meet your
       requirements. You may have additional consumer rights under your local laws that
       this Service Agreement cannot change.</b>
     </p>
     <p>
      <b>16. LIABILITY LIMITATION; YOUR EXCLUSIVE REMEDY</b>
     </p>
     <p>
      <b>
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       's aggregate liability to you for any and all claims related to the Service or its
       content is limited to the amount of your service fee for one month. To the extent
       permitted by applicable law,
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       will not be liable for any consequential, lost profits, special, or incidental damages
       resulting from your access to or use of--or inability to access or use (including
       due to modification, suspension, blocking, discontinuance, cancellation, or termination
       of the Service or any part thereof)--the Service or content, whether based on breach
       of contract, breach of warranty, tort (including negligence), or any other legal
       theory. Without limiting the foregoing, you specifically acknowledge that
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
       is not liable for the defamatory, offensive, infringing, breaching, fraudulent,
       or illegal conduct of other users or third parties and that the risk of injury from
       the foregoing rests entirely with you. These limitations apply to any matter related
       to: the Service or its content; third party Internet sites, programs or conduct;
       viruses or other disabling features; incompatibility between the Service and other
       services, software, or hardware; and any delay or failure in initiating, conducting,
       or completing any transmission or transaction in connection with the Service in
       an accurate or timely manner. These limitations also apply even if this remedy does
       not fully compensate you for any losses, or fails its essential purpose; or if we
       knew or should have known about the possibility of the damages. </b>
     </p>
     <p>
      You acknowledge that
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      cannot provide the Service inexpensively to you without limiting its liability as
      set forth above, so, in order to use the Service, you agree to limit
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      's potential liability to you in this way. These limitations of liability are fundamental
      elements of the basis of the bargain between
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      and you.
     </p>
     <p>
      Some states, provinces and countries do not allow the exclusion or limitation of
      incidental or consequential damages, so the above limitations or exclusions may
      not apply to you.
     </p>
     <p>
      <b>17. Special Admonitions for International Use</b>
     </p>
     <p>
      As a consequence of the global nature of the Internet, you agree to comply with
      all local rules and laws regarding user conduct on the Internet and acceptable content.
      Specifically, you agree to comply with all applicable laws regarding obscene and
      indecent content and communications and those regarding the transmission of technical
      data exported from the United States or the country in which you reside.
     </p>
     <p>
      <b>18. General</b>
     </p>
     <p>
      You understand and agree that this Service Agreement and any notices given pursuant
      to this Service Agreement are enforceable in electronic format. This Service Agreement
      constitutes the complete agreement and supersedes all prior agreements between you
      and
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>.
      Vestris Inc. is a registered British Virgin Islands company. If arbitration should occur, the court 
      can only be governed by laws of London, Great Britain, without regard to its conflict of laws provisions. 
      All disputes related to or arising from this Agreement will be subject to the exclusive jurisdiction and venue of the
      courts located in London, Great Britain; to which jurisdiction and venue you and
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      each irrevocably consent. If any provision of this Service Agreement is held to
      be invalid or unenforceable, such provision shall be deemed superseded by a valid,
      enforceable provision that most closely matches the intent of the original provision
      and the remaining provisions will remain in full force and effect.
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      's failure to act or your failure to act with respect to a breach does not waive
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      's rights or your right to act subsequently. You may not assign or transfer rights
      under this Service Agreement, or delegate any duties.
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      retains the right to assign its rights under this Service Agreement delegate its
      duties in connection with a merger, reorganization, or sale of substantially all
      of its assets. This Service Agreement will bind successors and permitted assigns.
     </p>
     <p>
      <b>19. Notices</b>
     </p>
     <p>
      Except for infringement notice as set forth in Section 12, any notice from you to
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      must be addressed to 
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Admin.Email", "admin at localhost dot com").Replace("@", " at ").Replace(".", " dot "))); %>.
      This Service Agreement is in electronic form.
      There may be other information regarding the Service that the law requires us to
      send you. You consent to
      <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.Name", "SnCore"))); %>
      's sending you this information in electronic form. You have the right to withdraw
      this consent by notice to us, but if you do, you must stop using this Service. <b>
       We may provide required information to you by email at the your registered address
       or by access to
       <% Response.Write(Renderer.Render(SessionManager.GetCachedConfiguration("SnCore.WebSite.Url", "http://localhost/"))); %>
       or another website designated in an email notice or generally designated in advance
       for this purpose. Notices provided to you via email will be deemed given and received
       on the transmission date. </b>
     </p>
    </div>
   </td>
  </tr>
 </table>
</asp:Content>
