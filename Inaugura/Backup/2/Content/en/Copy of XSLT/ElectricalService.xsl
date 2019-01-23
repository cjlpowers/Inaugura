<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="ElectricalService">
    <xsl:choose>
      <xsl:when test="not(.)">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=0">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=1">
        <xsl:text>60 AMP</xsl:text>
      </xsl:when>
      <xsl:when test=".=2">
        <xsl:text>100 AMP</xsl:text>
      </xsl:when>
      <xsl:when test=".=3">
        <xsl:text>200 AMP</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>Other</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
