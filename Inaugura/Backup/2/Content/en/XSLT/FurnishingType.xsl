<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="FurnishingType">
		<xsl:choose>
			<xsl:when test="not(.)">
			</xsl:when>
			<xsl:when test=".=0">
			</xsl:when>
			<xsl:when test=".=1">
				<xsl:text></xsl:text>
				<!-- Dont output anything for unfurnished places-->
			</xsl:when>
			<xsl:when test=".=2">
				<xsl:text> (Semi-furnished)</xsl:text>
			</xsl:when>
			<xsl:when test=".=3">
				<xsl:text> (Furnished)</xsl:text>
			</xsl:when>
			<xsl:otherwise>				
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
