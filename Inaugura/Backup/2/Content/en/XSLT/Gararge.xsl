<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
 <xsl:import href="Features.xsl"/>
  <xsl:import href="Size.xsl"/>
  <xsl:template match="Gararge">
    <table class="Grid"  cellpadding="0" cellspacing="0">
      <tr>
        <th colspan="4">
          Gararge
        </th>
      </tr>
      <tr>
        <td colspan="4">
          <xsl:value-of select="Description"></xsl:value-of>
        </td>
      </tr>
      <tr>
        <td class="dark">Type</td>
        <td>
          <xsl:choose>
          <xsl:when test="@attached='True'">
            <xsl:text>Attached</xsl:text>
          </xsl:when>
            <xsl:otherwise>
              <xsl:text>Detached</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:apply-templates select="Size"/>
        </td>
        <td class="dark">Parking Spaces</td>
        <td>
          <xsl:value-of select="@parkingSpaces"></xsl:value-of>
        </td>
      </tr>
      <tr>
        <td class="dark">Size</td>
        <td>
          <xsl:apply-templates select="Dimensions/@areaStdUnit"/>
        </td>
        <td class="dark" rowspan="3">Features</td>
        <td rowspan="3">
          <xsl:apply-templates select="Features"/>
        </td>
      </tr>
      <tr>
        <td class="dark">Width</td>
        <td>
          <xsl:value-of select="Dimensions/Width/@value"></xsl:value-of>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Dimensions/Width/@unit"></xsl:value-of>
        </td>
      </tr>
      <tr>
        <td class="dark">Depth</td>
        <td>
          <xsl:value-of select="Dimensions/Depth/@value"></xsl:value-of>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Dimensions/Depth/@unit"></xsl:value-of>
        </td>
      </tr>
    </table>
</xsl:template>
</xsl:stylesheet> 

