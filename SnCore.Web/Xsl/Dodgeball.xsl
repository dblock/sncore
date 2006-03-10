<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
 <xsl:template match="/">
  <rss version="2.0">
   <channel>
    <xsl:for-each select="rss/channel/item">
     <item>
      <title><xsl:value-of select="substring-after(substring-before(title, '('), '@')"/></title>
      <link><xsl:value-of select="link"/></link>
      <description>
       <xsl:value-of select="description"/> on <xsl:value-of select="substring-before(substring-after(title, '('), ')')"/>
      </description>
     </item>
    </xsl:for-each>
   </channel>
  </rss>
 </xsl:template>
</xsl:stylesheet>
