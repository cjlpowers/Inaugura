<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template name="ListingStatus">
    <div class="info">
      <xsl:choose>
        <xsl:when test="@status = 1">
          <span class="alert">
            This listing is <b>Inactive</b> and can only be viewed by you. When you would like people to see your listing simply change the status of this listing to <b>active</b>.
          </span>
          <xsl:call-template name="Tasks"></xsl:call-template>
          <xsl:call-template name="Expiration"></xsl:call-template>
        </xsl:when>
        <xsl:when test="@status = 2">
          <p>
            This listing is <b>active</b> and can be viewed by all users.
          </p>
          <xsl:call-template name="Tasks"></xsl:call-template>
          <xsl:call-template name="Expiration"></xsl:call-template>
        </xsl:when>
        <xsl:when test="@status = 4">
          <p>
            This listing is <b>sold</b>.
          </p>
        </xsl:when>
        <xsl:when test="@status = 8">
          This listing has been <b>suspended</b>.
        </xsl:when>
      </xsl:choose>
      <h3>Advertising Elsewhere?</h3>
      <p>
        <xsl:text>Link this listing in your forms of advertising using the following address. </xsl:text>
        <b>
          <xsl:text>www.rentleads.ca/</xsl:text>
          <xsl:value-of select="@code"></xsl:value-of>
        </b>  
      </p>
    </div>
  </xsl:template>
  <xsl:template name="Expiration">
    <xsl:if test="@expiration">
      <br></br>
      <p>Your Listing will expire on <b>
        <xsl:value-of select="@expiration"/>
      </b>
      </p>
    </xsl:if>
  </xsl:template>

  <xsl:template name="Tasks">
    <xsl:variable name="minRooms">
      <xsl:for-each select="/Listing/Levels/Level">
        <xsl:sort select="count(Rooms/Room)" order="ascending" data-type="number" />
        <xsl:if test="position()=1">
          <xsl:value-of select="count(Rooms/Room)"/>
        </xsl:if>
      </xsl:for-each>
    </xsl:variable>
    
    <xsl:if test ="count(Images/Image) = 0 or count(Levels/Level) = 0 or @status = 1 or $minRooms = 0">
      <h3>Todo</h3>
      <ui>
        <xsl:if test ="count(Images/Image) = 0">
          <li>
            Add pictures to your listing by clicking <b>Add Image</b> below.
          </li>
        </xsl:if>
        <xsl:if test ="count(Levels/Level) = 0">
          <li>
            Add Levels and Rooms to your listing by clicking <b>Add Level</b> near the bottom of this page.
          </li>
        </xsl:if>
        <xsl:for-each select="/Listing/Levels/Level">
          <xsl:if test ="count(Rooms/Room) = 0">
            <li>
              Add Rooms to level '<xsl:value-of select="@name"/>'.
            </li>
          </xsl:if>
        </xsl:for-each>
        <xsl:if test ="@status = 1">
          <li>
            Change your listings status to <b>Active</b> so renters can see it.
          </li>
        </xsl:if>
      </ui>
    </xsl:if>
  </xsl:template>

  <xsl:template name="ChangeStatus">
    <a href="javascript:SetFrame('Secure/PopupControls/EditStatus.aspx?listingId={@id}',600,200)">
      <span class="ToolEdit"  onclick="" title="Change Status"/>
      <span>Change Status</span>
    </a>
  </xsl:template>
</xsl:stylesheet>
