<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="Size">
    <xsl:choose>
      <xsl:when test ="@value = '0'">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="@value"/>
        <xsl:text> </xsl:text>
        <xsl:value-of select="@unit"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>

