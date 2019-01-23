<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="Features.xsl"/>
  <xsl:import href="FloorMaterial.xsl"/>
  <xsl:import href="RoomType.xsl"/>
  <xsl:param name="admin" />
  <xsl:template match="Room">
    <div class="h5">
      <xsl:value-of select="@name"></xsl:value-of>
      <xsl:if test="RoomType">
        <xsl:text> (</xsl:text>
        <xsl:apply-templates select="RoomType"/>
        <xsl:text>)</xsl:text>
      </xsl:if>
    </div>
    <xsl:if test="$admin='True'">
      <table class="Grid" cellpadding="0" cellspacing="0">
        <tr>
          <td class="admin">
            <xsl:call-template name="RoomAdmin"/>
          </td>
        </tr>
      </table>
    </xsl:if>
    <xsl:if test="Description">
      <div>
        <xsl:value-of select="Description"></xsl:value-of>
      </div>
    </xsl:if>
    <table class="Grid" cellspacing="0" cellpadding="0">
      <tr>
        <td class="dark">Dimensions</td>
        <td>
          <xsl:choose>
            <xsl:when test="Dimensions">
              <xsl:value-of select="Dimensions/Width/@value"/>
              <xsl:text> x </xsl:text>
              <xsl:value-of select="Dimensions/Depth/@value"/>
              <xsl:text> </xsl:text>
              <xsl:value-of select="Dimensions/Depth/@unit"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>Not Specified</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td class="dark">Flooring</td>
        <td>
          <xsl:if test="not(FloorMaterial)">
            Not Specified
          </xsl:if>
          <xsl:apply-templates select="FloorMaterial"></xsl:apply-templates>
        </td>
      </tr>
      <xsl:if test="Features">
        <tr>
          <td class="dark">Features</td>
          <td colspan="3">
            <xsl:apply-templates select="Features"></xsl:apply-templates>
          </td>
        </tr>
      </xsl:if>
    </table>
  </xsl:template>

  <xsl:template name="RoomAdmin">
    <span class="ToolEdit" title="Edit this room"  onclick="SetFrame('Secure/PopupControls/EditRoom.aspx?listingId={/Listing/@id}&amp;roomId={@id}',500,300)"/>
    <xsl:if test="position() !=1">
      <span class="ToolUp" onclick="ListingOpperation('{/Listing/@id}','{@id}','roomup')" title="Move this room up in the list"/>
    </xsl:if>
    <xsl:if test="position() !=last()">
      <span class="ToolDown" onclick="ListingOpperation('{/Listing/@id}','{@id}','roomdown')" title="Move this room down in the list"/>
    </xsl:if>
    <span class="ToolDelete" onclick="if(confirm('Are you sure you want to delete this room?'))ListingOpperation('{/Listing/@id}','{@id}','roomdelete');" title="Delete this room"/>
  </xsl:template>
</xsl:stylesheet>

