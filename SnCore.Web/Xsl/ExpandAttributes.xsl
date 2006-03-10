<!-- AttrToElement.xsl: Turn all attributes into subelements -->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- Import the identity transformation. -->
  <!-- Whenever you match any node or any attribute -->
  <xsl:template match="node()|@*">
    <!-- Copy the current node -->
    <xsl:copy>
      <!-- Including any attributes it has and any child nodes -->
      <xsl:apply-templates select="@*|node()"/>
    </xsl:copy>
  </xsl:template>
  <xsl:strip-space elements="*"/>
  <xsl:output indent="yes"/>
  <!-- Match any Attribute and turn it into an element -->
  <xsl:template match="@*">
    <xsl:element name="{name(.)}">
	<xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>
