<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="Features|LocationFeatures|InteriorFeatures|ExteriorFeatures|Appliances">
  <ul>
  <xsl:for-each select="Item">
      <li>
        <xsl:value-of select="."></xsl:value-of>
      </li>
  </xsl:for-each>
  </ul>
</xsl:template>
</xsl:stylesheet> 

