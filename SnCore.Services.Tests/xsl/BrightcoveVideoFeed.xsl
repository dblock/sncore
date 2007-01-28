<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
 <xsl:template match="rss/channel/item">
  <item>
   <title><xsl:value-of select="title"/></title>
   <link><xsl:value-of select="link"/></link>
   <pubDate><xsl:value-of select="pubDate"/></pubDate>
   <description>
<![CDATA[<p>]]>
 <xsl:value-of select="substring-after(description, '&gt;')"/>
<![CDATA[</p>]]>
<![CDATA[
 <embed 
  src='http://services.brightcove.com/services/viewer/federated_f8/271529994' bgcolor='#FFFFFF' 
  flashVars='videoId=]]><xsl:value-of 
  select="substring-before(substring-after(link, 'bctid'), '?')"/><![CDATA[&playerId=271529994&viewerSecureGatewayURL=https://services.brightcove.com/services/amfgateway&servicesURL=http://services.brightcove.com/services&cdnURL=http://admin.brightcove.com&domain=embed&autoStart=false&'
  base='http://admin.brightcove.com' name='flashObj' width='486' height='412' seamlesstabbing='false' 
  type='application/x-shockwave-flash' swLiveConnect='true' 
  pluginspage='http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash'>
 </embed>
]]>
   </description>
  </item>
 </xsl:template>
 <xsl:template match="@*|node()">
  <xsl:copy>
   <xsl:apply-templates select="@*|node()"/>
  </xsl:copy>
 </xsl:template>
</xsl:stylesheet>
