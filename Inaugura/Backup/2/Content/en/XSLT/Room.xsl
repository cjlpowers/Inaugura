<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="Features.xsl"/>
  <xsl:import href="FloorMaterial.xsl"/>
  <xsl:import href="RoomType.xsl"/>
  <xsl:param name="admin" />
  <xsl:template match="Room">
    <div class="room">
      <xsl:if test="$admin='True'">
        <a class="adminTask"  href="javascript:if(confirm('Are you sure you want to remove this room?'))$postbackEvent'action,removeroom;listing,{//Listing/@id};level,{../../@id};room,{@id}')">Remove</a>
        <a class="adminTask"  href="$postbackHyperlink'edit,~/Controls/Edit/EditRoom.ascx;listing,{//Listing/@id};level,{../../@id};room,{@id}')">Edit</a>        
      </xsl:if>
      <h2>
        <xsl:value-of select ="@name"></xsl:value-of>
        <xsl:if test="RoomType">
          <xsl:text> (</xsl:text>
          <xsl:apply-templates select="RoomType"/>
          <xsl:text>)</xsl:text>
        </xsl:if>
      </h2>
      <xsl:if test="Dimensions">
        <span class="detail">
          <font class="label">Size: </font>
          <xsl:apply-templates select="Dimensions"></xsl:apply-templates>
        </span>
      </xsl:if>
      <xsl:if test="FloorMaterial">
        <span class="detail">
          <font class="label">Flooring: </font>
          <font class="highlight">
            <xsl:apply-templates select="FloorMaterial"></xsl:apply-templates>
          </font>
        </span>
      </xsl:if>
      <xsl:if test="Description">
        <div>
          <xsl:value-of select="Description"></xsl:value-of>
        </div>
      </xsl:if>
      <xsl:if test="Features">
        <h4>Features</h4>
        <xsl:apply-templates select="Features"></xsl:apply-templates>
      </xsl:if>
    </div>
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

