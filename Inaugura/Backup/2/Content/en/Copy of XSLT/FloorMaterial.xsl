<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="FloorMaterial">
    <xsl:choose>
      <xsl:when test="not(.)">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=0">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=1">
        <xsl:text>Carpet</xsl:text>
      </xsl:when>
      <xsl:when test=".=2">
        <xsl:text>Hardwood</xsl:text>
      </xsl:when>
      <xsl:when test=".=3">
        <xsl:text>Ceramic</xsl:text>
      </xsl:when>
      <xsl:when test=".=4">
        <xsl:text>Porcelan</xsl:text>
      </xsl:when>
      <xsl:when test=".=5">
        <xsl:text>Vinyl</xsl:text>
      </xsl:when>
      <xsl:when test=".=6">
        <xsl:text>Cork</xsl:text>
      </xsl:when>
      <xsl:when test=".=7">
        <xsl:text>Laminate</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>Other</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
