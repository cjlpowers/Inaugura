<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="PropertyType">
		<xsl:choose>
			<xsl:when test="not(.)">
				<xsl:text>Not Specified</xsl:text>
			</xsl:when>
			<xsl:when test=".=0">
				<xsl:text>Not Specified</xsl:text>
			</xsl:when>
			<xsl:when test=".=1">
				<xsl:text>Bungalow</xsl:text>
			</xsl:when>
			<xsl:when test=".=2">
				<xsl:text>1.5 Storey</xsl:text>
			</xsl:when>
			<xsl:when test=".=3">
				<xsl:text>2 Storey Split</xsl:text>
			</xsl:when>
			<xsl:when test=".=4">
				<xsl:text>2 Storey</xsl:text>
			</xsl:when>
			<xsl:when test=".=5">
				<xsl:text>4 Level Split</xsl:text>
			</xsl:when>
			<xsl:when test=".=6">
				<xsl:text>2.5 Storey</xsl:text>
			</xsl:when>
			<xsl:when test=".=7">
				<xsl:text>Bi-Level</xsl:text>
			</xsl:when>
			<xsl:when test=".=8">
				<xsl:text>3 Level Split</xsl:text>
			</xsl:when>
			<xsl:when test=".=9">
				<xsl:text>5 Level Split</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<xsl:text>Other</xsl:text>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
