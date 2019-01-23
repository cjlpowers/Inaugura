<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="Size|Width|Depth">
    <xsl:choose>
      <xsl:when test ="@value = '0'">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="@value"/>
        <xsl:text> </xsl:text>
        <xsl:value-of select="@symbol"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Dimensions">
    <font class="highlight">
      <xsl:choose>
        <xsl:when test="Width/@symbol = Depth/@symbol">
          <xsl:value-of select="Width/@value"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="Width"></xsl:apply-templates>
        </xsl:otherwise>
      </xsl:choose>
    </font>
    x
    <font class="highlight">
      <xsl:apply-templates select="Depth"></xsl:apply-templates>
    </font>
    <xsl:text> (</xsl:text>
    <font class="highlight">
      <xsl:value-of select="@area"/>
    </font>
    <xsl:text>)</xsl:text>
  </xsl:template>
</xsl:stylesheet>

