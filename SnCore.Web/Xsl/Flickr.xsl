<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:media="http://search.yahoo.com/mrss"  version="1.0">
  <xsl:template match="/">
    <rss version="2.0">
      <channel>
        <xsl:for-each select="rss/channel/item">
          <item>
            <title>
              <xsl:value-of select="title"/>
            </title>
            <link>
              <xsl:value-of select="link"/>
            </link>
            <description>
              <xsl:value-of select="media:text" />
            </description>
          </item>
        </xsl:for-each>
      </channel>
    </rss>
  </xsl:template>
</xsl:stylesheet>
