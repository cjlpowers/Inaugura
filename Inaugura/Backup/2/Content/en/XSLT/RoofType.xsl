<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="RoofType">
    <xsl:choose>
      <xsl:when test="not(.)">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=0">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=1">
        <xsl:text>Asphalt Shingles</xsl:text>
      </xsl:when>
      <xsl:when test=".=2">
        <xsl:text>Metal</xsl:text>
      </xsl:when>
      <xsl:when test=".=3">
        <xsl:text>Wood Shakes</xsl:text>
      </xsl:when>
      <xsl:when test=".=4">
        <xsl:text>Slate</xsl:text>
      </xsl:when>
      <xsl:when test=".=5">
        <xsl:text>Ceramic</xsl:text>
      </xsl:when>      
      <xsl:otherwise>
        <xsl:text>Other</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
