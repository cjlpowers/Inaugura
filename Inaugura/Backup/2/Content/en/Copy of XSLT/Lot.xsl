<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="Features.xsl"/>
	<xsl:import href="Size.xsl"/>
	<xsl:param name="admin" />
	<xsl:template match="Lot">
    <xsl:if test="$admin='True' or Size/@value !='0'">
      <table class="Grid"  cellpadding="0" cellspacing="0">
        <tr>
          <th colspan="2">
            Lot
          </th>
        </tr>
        <xsl:if test="$admin='True'">
          <tr>
            <td class="admin" colspan="2">
              <xsl:call-template name="LotAdmin"/>
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td colspan="2">
            <xsl:value-of select="Description"></xsl:value-of>
          </td>
        </tr>
        <tr>
          <td class="dark">Lot Size</td>
          <td>
            <xsl:apply-templates select="Size"/>
          </td>
        </tr>
        <xsl:if test="Features">
          <tr>
            <td class="dark">Features</td>
            <td>
              <xsl:apply-templates select="Features"></xsl:apply-templates>
            </td>
          </tr>
        </xsl:if>
      </table>
    </xsl:if>
	</xsl:template>
	<xsl:template name="LotAdmin">
    <span class="ToolEdit" title="Edit Lot Details" onclick="SetFrame('Secure/PopupControls/EditListingLot.aspx?listingId={/Listing/@id}&amp;levelId={@id}',500,250)"/>
	</xsl:template>
</xsl:stylesheet>

