<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="HeatingPrimary|HeatingSecondary">
    <xsl:choose>
      <xsl:when test="not(.)">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=0">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=1">
        <xsl:text>Forced Air</xsl:text>
      </xsl:when>
      <xsl:when test=".=2">
        <xsl:text>Radiant</xsl:text>
      </xsl:when>
      <xsl:when test=".=3">
        <xsl:text>Hot Water</xsl:text>
      </xsl:when>
      <xsl:when test=".=4">
        <xsl:text>Base Board Electric</xsl:text>
      </xsl:when>
      <xsl:when test=".=5">
        <xsl:text>Wood Stove</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>Other</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
