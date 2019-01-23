<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="RoomType">
    <xsl:choose>
      <xsl:when test=".=0">
        <xsl:text>Not Specified</xsl:text>
      </xsl:when>
      <xsl:when test=".=1">
        <xsl:text>Bathroom</xsl:text>
      </xsl:when>
      <xsl:when test=".=2">
        <xsl:text>Bedroom</xsl:text>
      </xsl:when>
      <xsl:when test=".=3">
        <xsl:text>Kitchen</xsl:text>
      </xsl:when>
      <xsl:when test=".=4">
        <xsl:text>Dining Room</xsl:text>
      </xsl:when>
      <xsl:when test=".=5">
        <xsl:text>Living Room</xsl:text>
      </xsl:when>
		<xsl:when test=".=5">
			<xsl:text>Living Room</xsl:text>
		</xsl:when>
		<xsl:when test=".=5">
			<xsl:text>Living Room</xsl:text>
		</xsl:when>
		<xsl:when test=".=6">
			<xsl:text>Family Room</xsl:text>
		</xsl:when>
		<xsl:when test=".=7">
			<xsl:text>Den</xsl:text>
		</xsl:when>
		<xsl:when test=".=8">
			<xsl:text>Office</xsl:text>
		</xsl:when>
		<xsl:when test=".=9">
			<xsl:text>Rec Room</xsl:text>
		</xsl:when>
		<xsl:when test=".=10">
			<xsl:text>Laundry</xsl:text>
		</xsl:when>
		<xsl:otherwise>
        <xsl:text>Other</xsl:text>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
