<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

<xsl:template name="ShortAddress">
  <xsl:choose>
    <xsl:when test="not(Address)">
      <xsl:text>Not Specified</xsl:text>
    </xsl:when>
    <xsl:otherwise>
      <xsl:if test="Address/@street">
        <xsl:value-of select="Address/@street"/>
        <xsl:text>, </xsl:text>
      </xsl:if>
      <xsl:if test="Address/@city">
        <xsl:value-of select="Address/@city"/>
        <xsl:text>, </xsl:text>
      </xsl:if>
      <xsl:if test="Address/@stateProv">
        <xsl:value-of select="Address/@stateProv"/>
      </xsl:if>
    </xsl:otherwise>
  </xsl:choose>
</xsl:template>

  <xsl:template name="LongAddress">
	  <xsl:choose>
		  <xsl:when test="not(Address)">
			  <xsl:text>Not Specified</xsl:text>
		  </xsl:when>
		  <xsl:otherwise>
				  <xsl:value-of select="Address/@street"/>
          <xsl:if test="Address/@city">
            <xsl:text>, </xsl:text>
            <xsl:value-of select="Address/@city"/>
            <xsl:text>, </xsl:text>
          </xsl:if>
          <xsl:value-of select="Address/@stateProv"/>
		  </xsl:otherwise>
	  </xsl:choose>
  </xsl:template>

  <xsl:template name="Distance">
     <xsl:text>[distance]</xsl:text>
  </xsl:template>
</xsl:stylesheet> 

