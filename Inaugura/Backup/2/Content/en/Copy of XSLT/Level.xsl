<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="Features.xsl"/>
  <xsl:import href="Size.xsl"/>
  <xsl:import href="Room.xsl"/>
  <xsl:param name="admin" />
  <xsl:template match="Level">
    <table class="Grid"  cellpadding="0" cellspacing="0">
      <xsl:if test="Features/Item">
        <tr>
          <th colspan="4">
            <xsl:value-of select ="@name"></xsl:value-of>
          </th>
        </tr>
        <xsl:if test="$admin='True'">
          <tr>
            <td class="admin" colspan="4">
              <xsl:call-template name="LevelAdmin"/>
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td colspan="4">
            <xsl:value-of select="Description"></xsl:value-of>
          </td>
        </tr>
        <tr>
          <td class="dark">Size</td>
          <td>
            <xsl:apply-templates select="Size"/>
          </td>
          <td class="dark" rowspan="2">Features</td>
          <td rowspan="2">
            <xsl:apply-templates select="Features"/>
          </td>
        </tr>
        <tr>
          <td class="dark">Above Grade</td>
          <td>
            <xsl:value-of select="@aboveGrade"/>
          </td>
        </tr>
        <tr>
          <td colspan="4"  class="clean">
            <xsl:apply-templates select="Rooms/Room"></xsl:apply-templates>
          </td>
        </tr>
      </xsl:if>
      <xsl:if test="not(Features/Item)">
        <tr>
          <th colspan="2">
            <xsl:value-of select ="@name"></xsl:value-of>
          </th>
        </tr>
        <xsl:if test="$admin='True'">
          <tr>
            <td class="admin" colspan="2">
              <xsl:call-template name="LevelAdmin"/>
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td colspan="2">
            <xsl:value-of select="Description"></xsl:value-of>
          </td>
        </tr>
        <tr>
          <td class="dark">Size</td>
          <td>
            <xsl:apply-templates select="Size"/>
          </td>
        </tr>
        <tr>
          <td class="dark">Above Grade</td>
          <td>
            <xsl:value-of select="@aboveGrade"/>
          </td>
        </tr>
        <tr>
          <td colspan="2"  class="clean">
            <xsl:apply-templates select="Rooms/Room"></xsl:apply-templates>
          </td>
        </tr>
      </xsl:if>
    </table>
  </xsl:template>

  <xsl:template name="LevelAdmin">
    <span class="ToolEdit" title="Edit this level" onclick="SetFrame('Secure/PopupControls/EditLevel.aspx?listingId={/Listing/@id}&amp;levelId={@id}',450,320)"/>
    <span class="ToolAdd" title="Add a new room to this level" onclick="SetFrame('Secure/PopupControls/EditRoom.aspx?listingId={/Listing/@id}&amp;levelId={@id}&amp;roomId=new',500,300)"/>
    <xsl:if test="position() !=1">
      <span class="ToolUp" onclick="ListingOpperation('{/Listing/@id}','{@id}','levelup')" title="Move this level up in the list"/>
    </xsl:if>
    <xsl:if test="position() !=last()">
      <span class="ToolDown" onclick="ListingOpperation('{/Listing/@id}','{@id}','leveldown')" title="Move this level down in the list"/>
    </xsl:if>
    <span class="ToolDelete" onclick="if(confirm('Are you sure you want to delete this level?'))ListingOpperation('{/Listing/@id}','{@id}','leveldelete');" title="Delete this level"/>
  </xsl:template>
</xsl:stylesheet>

