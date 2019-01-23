<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="FlashPlayer.xsl"/>
  <xsl:import href="RentalPropertyType.xsl"/>
  <xsl:import href="FurnishingType.xsl"/>
  <xsl:import href="ExteriorMaterial.xsl"/>
  <xsl:import href="DrivewayType.xsl"/>
  <xsl:import href="FoundationType.xsl"/>
  <xsl:import href="HeatingType.xsl"/>
  <xsl:import href="WaterType.xsl"/>
  <xsl:import href="ElectricalService.xsl"/>
  <xsl:import href="RoofType.xsl"/>
  <xsl:import href="Address.xsl"/>
  <xsl:import href="Images.xsl"/>
  <xsl:import href="Gararge.xsl"/>
  <xsl:import href="Lot.xsl"/>
  <xsl:import href="Level.xsl"/>
  <xsl:import href="Size.xsl"/>
  <xsl:import href="ListingStatus.xsl"/>
  <xsl:output method="html"/>
  <xsl:key name="FloorKey" match="Listing/Levels/Level/Rooms/Room" use="FloorMaterial"/>
  <xsl:param name="mode" select="''"/>
  <xsl:param name="admin" select="''"/>
  <xsl:template match="/Listing">
    <xsl:choose>
      <xsl:when test="$mode='SearchResult'">
        <xsl:call-template name="SearchResult"/>
      </xsl:when>
      <xsl:when test="$mode='Mini'">
        <xsl:call-template name="Mini"/>
      </xsl:when>
      <xsl:when test="$mode='Pin'">
        <xsl:call-template name="Pin"/>
      </xsl:when>
      <xsl:when test="$mode='Feature'">
        <xsl:call-template name="Feature"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:call-template name="Details"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!-- The template used to display the real estate listing details-->
  <xsl:template name="SearchResult">
    <table class="Grid"  cellpadding="0" cellspacing="0">
      <tr>
        <th colspan="5">
          <span style="float:left">
            <xsl:value-of select="Title"></xsl:value-of>
            <xsl:call-template name="Distance"></xsl:call-template>
          </span>
          <xsl:call-template name="HeaderIcons"/>
        </th>
      </tr>
      <tr>
        <td colspan="4">
          <xsl:value-of select="substring(Description,0,200)"/>
          <xsl:if test="string-length(Description) > 200">
            <xsl:text>...</xsl:text>
          </xsl:if>
        </td>
        <xsl:if test="count(Images/Image) > 0">
          <td width="160" rowspan="6" style="text-align:center">
            <a href="Listing.aspx?id={@id}">
              <img class="Photo">
                <xsl:variable name="displayImage">
                  <xsl:value-of select="Images/@defaultImage"/>
                </xsl:variable>
                <xsl:attribute name="src">
                  <xsl:text>ImageHandler.ashx?id=</xsl:text>
                  <xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
                  <xsl:text>&amp;mode=160</xsl:text>
                </xsl:attribute>
              </img>
            </a>
          </td>
        </xsl:if>
      </tr>
      <tr>
        <td class="dark">
          Address
        </td>
        <td colspan="3">
          <xsl:call-template name="ShortAddress"></xsl:call-template>
        </td>
      </tr>
      <tr>
        <td class="dark">
          Monthly Rent
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="not(@monthlyRent)">
              <xsl:text>Not Specified</xsl:text>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>$</xsl:text>
              <xsl:value-of select="format-number(@monthlyRent,'###,###,###')"/>
            </xsl:otherwise>
          </xsl:choose>
        </td>
        <td class="dark">
          Property Type
        </td>
        <td>
          <xsl:if test="not(RentalPropertyType)">
            <xsl:text>Not Specified</xsl:text>
          </xsl:if>
          <xsl:apply-templates select="RentalPropertyType"/>
          <xsl:apply-templates select="FurnishingType"/>
        </td>
      </tr>
      <tr>
        <td class="dark">
          Bedrooms
        </td>
        <td>
          <xsl:value-of select="@numberOfBedrooms"/>
        </td>
        <td class="dark">
          Parking Spaces
        </td>
        <td>
          <xsl:choose>
            <xsl:when test="@parkingSpaces > 0">
              <xsl:value-of select="@parkingSpaces"/>
              <xsl:if test="@parkingIncluded">
                <xsl:text> included</xsl:text>
              </xsl:if>
            </xsl:when>
            <xsl:otherwise>
              <xsl:text>No Parking</xsl:text>
            </xsl:otherwise>
          </xsl:choose>
        </td>
      </tr>
      <tr>
        <td class="dark">
          Appliances
        </td>
        <td>
          <xsl:value-of select="count(Appliances/Item)"/>
        </td>
        <td class="dark">
          Available
        </td>
        <td>
          <xsl:value-of select="@availabilityStart"/>
          <xsl:if test="@availabilityEnd">
            <xsl:text> until </xsl:text>
            <xsl:value-of select="@availabilityEnd"/>
          </xsl:if>
        </td>
      </tr>
      <tr>
        <td class="dark" colspan="4">
          <a href="Listing.aspx?id={@id}" class="linkView" title="View Listing">View</a>
        </td>
      </tr>
    </table>
  </xsl:template>
  <!-- The template used to display search results-->
  <xsl:template name="Details">
    <xsl:if test="$admin='True'">      
      <xsl:call-template name="ListingStatus"/>      
    </xsl:if>
    <table width="100%">
      <tr>
        <td valign="top" width="100%">
          <table class="Grid"  cellpadding="0" cellspacing="0">
            <tr>
              <th colspan="4">
                <span style="float:left">
                  <xsl:value-of select="Title"></xsl:value-of>
                  <xsl:call-template name="Distance"></xsl:call-template>
                </span>
                <xsl:call-template name="HeaderIcons"/>
              </th>
            </tr>
            <xsl:if test="$admin='True'">
              <tr>
                <td class="admin" colspan="4">
                  <span class="ToolEdit" title="Edit the listing information" onclick="SetFrame('Secure/PopupControls/EditRental.aspx?listingId={@id}','550','400')"/>
                </td>
              </tr>
            </xsl:if>
            <tr>
              <td colspan="4" class="dark">
                <a href="http://www.rentleads.ca/Listing.aspx?id={@id}" class="linkListing" title="Permanent Link">Link</a>
                <a onclick="ShowListingMapWithPin('{Address/@latitude}','{Address/@longitude}','{Title/text()}','')" class="linkMap" title="See on Map">Map</a>
                <a onclick="SetFrame('Popups/ContactListingOwner.aspx?listingId={@id}&amp;userId={@userId}',450,320)" class="linkContact" Title="Contact Owner">Contact</a>
                <xsl:if test="$admin='True'">
                  <a onclick="SetFrame('Secure/PopupControls/EditStatus.aspx?listingId={@id}',600,200)" class="linkStatus" Title="Change Status">Change Status</a>
                </xsl:if>
              </td>
            </tr>
            <!--
            <tr>
              <td colspan="4" align="center" id="Player">
                <xsl:call-template name="FlashPlayer"></xsl:call-template>
              </td>
            </tr>-->
            <tr>
              <td colspan="4">
                <xsl:variable name="defaultImageID" select="Images/@defaultImage"></xsl:variable>
                <div>
                  <div id="listingMap" style="position:relative; width:100%; height:400px; display:none;"></div>
                  <div style="text-align:center">
                    <img id="listingImage"  class="Photo">
                      <xsl:choose>
                        <xsl:when test ="count(Images/Image) > 0">
                          <xsl:variable name="displayImage">
                            <xsl:value-of select="Images/@defaultImage"/>
                          </xsl:variable>
                          <xsl:attribute name="src">
                            <xsl:text>ImageHandler.ashx?id=</xsl:text>
                            <xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
                            <xsl:text>&amp;mode=480</xsl:text>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="src">
                            <xsl:text>ImageHandler.ashx?id=noimage&amp;mode=480</xsl:text>
                          </xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                    </img>
                  </div>
                </div>
              </td>
            </tr>
            <tr>
              <td class="dark">
                Address
              </td>
              <td colspan="3">
                <span>
                  <xsl:call-template name="LongAddress"></xsl:call-template>
                </span>
              </td>
            </tr>
            <tr>
              <td class="dark">Rent</td>
              <td>
                <xsl:choose>
                  <xsl:when test="not(@monthlyRent)">
                    <xsl:text>Not Specified</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>$</xsl:text>
                    <xsl:value-of select="format-number(@monthlyRent,'###,###,###')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td class="dark">
                Available
              </td>
              <td>
                <xsl:value-of select="@availabilityStart"/>
                <xsl:if test="@availabilityEnd">
                  <xsl:text> until </xsl:text>
                  <xsl:value-of select="@availabilityEnd"/>
                </xsl:if>
              </td>

            </tr>

            <tr>
              <td colspan="4">
                <xsl:for-each select="Description/p">
                  <p>
                    <xsl:value-of select="text()"/>
                  </p>
                </xsl:for-each>
              </td>
            </tr>
          </table>
          <table class="Grid"  cellpadding="0" cellspacing="0">
            <tr>
              <th colspan="4">
                Details
              </th>
            </tr>
            <xsl:if test="$admin='True'">
              <tr>
                <td class="admin" colspan="4">
                  <xsl:call-template name="DetailsAdmin"/>
                </td>
              </tr>
            </xsl:if>
            <tr>
              <td class="dark">Type</td>
              <td>
                <xsl:if test="not(RentalPropertyType)">
                  <xsl:text>Not Specified</xsl:text>
                </xsl:if>
                <xsl:apply-templates select="RentalPropertyType"/>
                <xsl:apply-templates select="FurnishingType"/>
              </td>
              <td class="dark">
                Size
              </td>
              <td>
                <xsl:apply-templates select="Size"></xsl:apply-templates>
              </td>
            </tr>
            <tr>
              <td class="dark">Bedrooms</td>
              <td>
                <xsl:value-of select="@numberOfBedrooms"></xsl:value-of>
              </td>
              <td class="dark">Bathrooms</td>
              <td>
                <xsl:value-of select="@numberOfBathrooms"></xsl:value-of>
              </td>
            </tr>
            <tr>
              <td class="dark">Parking Spaces</td>
              <td>
                <xsl:choose>
                  <xsl:when test="@parkingSpaces > 0">
                    <xsl:value-of select="@parkingSpaces"/>
                    <xsl:if test="@parkingIncluded">
                      <xsl:text> included</xsl:text>
                    </xsl:if>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>No Parking</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </td>
              <td class="dark">Flooring</td>
              <td>
                <xsl:for-each select="//Listing/Levels/Level/Rooms/Room[generate-id(.)=generate-id(key('FloorKey', FloorMaterial)[1])]">
                  <xsl:if test="not(FloorMaterial/text()=0)">
                    <xsl:apply-templates select="FloorMaterial"></xsl:apply-templates>
                    <xsl:choose>
                      <xsl:when test="position() != last()">
                        <xsl:text>, </xsl:text>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:if>
                </xsl:for-each>
              </td>
            </tr>
            <xsl:if test="Features or Appliances">
            <tr>
              <xsl:if test="Features">
                <td class="dark">
                  <xsl:attribute name="colspan">
                    <xsl:choose>
                      <xsl:when test="Appliances">
                        <xsl:text>2</xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>4</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:text>Features</xsl:text>
                </td>
              </xsl:if>
              <xsl:if test="Appliances">
                <td class="dark">
                  <xsl:attribute name="colspan">
                    <xsl:choose>
                      <xsl:when test="Features">
                        <xsl:text>2</xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>4</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:text>Appliances</xsl:text>
                </td>
              </xsl:if>
            </tr>          
            <tr>
              <xsl:if test="Features">
                <td>
                  <xsl:attribute name="colspan">
                    <xsl:choose>
                      <xsl:when test="Appliances">
                        <xsl:text>2</xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>4</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:apply-templates select="Features"></xsl:apply-templates>
                  <xsl:call-template name="HeaderIcons"/>
                </td>
              </xsl:if>
              <xsl:if test="Appliances">
                <td>
                  <xsl:attribute name="colspan">
                    <xsl:choose>
                      <xsl:when test="Features">
                        <xsl:text>2</xsl:text>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:text>4</xsl:text>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:apply-templates select="Appliances"></xsl:apply-templates>
                </td>
              </xsl:if>
            </tr>
            </xsl:if>
          </table>
          <xsl:apply-templates select="Lot"/>
          <xsl:apply-templates select="Gararge"/>
          <xsl:if test="$admin='True'">
            <table class="Grid" cellpadding="0" cellspacing="0">
              <td class="admin">
                <a onclick="SetFrame('Secure/PopupControls/EditLevel.aspx?listingId={@id}&amp;levelId=new',450,320)">
                  <span class="ToolAdd" title="Add New Level"/>
                  <span>Add Level</span>
                </a>
              </td>
            </table>
          </xsl:if>
          <xsl:apply-templates select="Levels/Level">
            <xsl:with-param name="admin" />
          </xsl:apply-templates>
          <a href="javascript:SetFrame('Popups/ReportAbuse.aspx?listingId={@id}',500,120)">
            Report Abuse
          </a>
        </td>
        <xsl:if test="count(Images/Image) >0 or $admin='True'">
          <td valign="top" width="400px">
            <xsl:if test="$admin='True'">
              <table class="Grid" cellpadding="0" cellspacing="0">
                <td class="admin">
                  <a onclick="SetFrame('Secure/PopupControls/AddImage.aspx?listingId={@id}',500,120)">
                    <span class="ToolAdd" title="Add a new Image"/>
                    <span>Add Image</span>
                  </a>
                </td>
              </table>
            </xsl:if>
            <xsl:apply-templates select="Images/Image"></xsl:apply-templates>
          </td>
        </xsl:if>
      </tr>
    </table>
    <!--<span>[Time]</span>-->
  </xsl:template>

  <xsl:template name="DetailsAdmin">
    <span class="ToolEdit" onclick="SetFrame('Secure/PopupControls/EditRentalDetails.aspx?listingId={@id}',600,350)" title="Edit the listing details"/>
  </xsl:template>

  <!-- The template to use for the mini results -->
  <xsl:template name="Mini">
    <td>
      <table class="Mini" onclick="GetListingContent('{@id}')">
        <tr>
          <th colspan="2">
            <span style="float:left">
              <xsl:value-of select="@code"/>
              <span class="dist">
                <xsl:call-template name="Distance"></xsl:call-template>
              </span>
            </span>
            <span style="float: right">
              <xsl:call-template name="HeaderIcons"/>
            </span>
          </th>
        </tr>
        <tr>
          <td class="miniImage" rowspan="3">
            <img>
              <xsl:choose>
                <xsl:when test ="count(Images/Image) > 0">
                  <xsl:variable name="displayImage">
                    <xsl:value-of select="Images/@defaultImage"/>
                  </xsl:variable>
                  <xsl:attribute name="src">
                    <xsl:text>ImageHandler.ashx?id=</xsl:text>
                    <xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
                    <xsl:text>&amp;mode=80</xsl:text>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="src">
                    <xsl:text>ImageHandler.ashx?id=noimage&amp;mode=80</xsl:text>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </img>
          </td>
          <td>
            <xsl:choose>
              <xsl:when test="not(@monthlyRent)">
                <xsl:text>Not Specified</xsl:text>
              </xsl:when>
              <xsl:otherwise>
                <span class="cost">
                  <xsl:text>$</xsl:text>
                  <xsl:value-of select="format-number(@monthlyRent,'###,###,###')"/>
                </span> / month
              </xsl:otherwise>
            </xsl:choose>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="Address/@street"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="Address/@city"/>
          </td>
        </tr>
        <tr>
          <td>
            <span class="label">Avaliable</span>
            <xsl:value-of select="@availabilityStart"/>
            <!--<xsl:if test="@availabilityEnd">
              <xsl:text> until </xsl:text>
              <xsl:value-of select="@availabilityEnd"/>
            </xsl:if>-->
          </td>
        </tr>
      </table>
    </td>
  </xsl:template>

  <!-- The template to use for the mini results -->
  <xsl:template name="MiniOld">
    <td>
      <table class="Grid"  cellpadding="0" cellspacing="0" style="width:233px; height:100px; margin-top:2px; font-size:80%;cursor:pointer;" onclick="GetListingContent('{@id}')">
        <tr>
          <th colspan="2">
            <span style="float:left">
              <xsl:value-of select="@code"/>
              <xsl:call-template name="Distance"></xsl:call-template>
            </span>
            <xsl:call-template name="HeaderIcons"/>
          </th>
        </tr>
        <tr>
          <td width="80px"  rowspan="3"  align="center" class="dark">
            <img>
              <xsl:choose>
                <xsl:when test ="count(Images/Image) > 0">
                  <xsl:variable name="displayImage">
                    <xsl:value-of select="Images/@defaultImage"/>
                  </xsl:variable>
                  <xsl:attribute name="src">
                    <xsl:text>ImageHandler.ashx?id=</xsl:text>
                    <xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
                    <xsl:text>&amp;mode=80</xsl:text>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="src">
                    <xsl:text>ImageHandler.ashx?id=noimage&amp;mode=80</xsl:text>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </img>
          </td>
          <td>
            <xsl:value-of select="Address/@city"/>
            <xsl:text>, </xsl:text>
            <xsl:value-of select="Address/@stateProv"/>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:choose>
              <xsl:when test="not(@monthlyRent)">
                <xsl:text>Not Specified</xsl:text>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>$</xsl:text>
                <xsl:value-of select="format-number(@monthlyRent,'###,###,###')"/>
              </xsl:otherwise>
            </xsl:choose>
          </td>
        </tr>
        <tr>
          <td>
            <xsl:value-of select="@availabilityStart"/>
            <xsl:if test="@availabilityEnd">
              <xsl:text> until </xsl:text>
              <xsl:value-of select="@availabilityEnd"/>
            </xsl:if>
          </td>
        </tr>
      </table>
    </td>
  </xsl:template>
  <!-- The template to use for the mini results -->
  <xsl:template name="Feature">
    <table>
      <tr>
        <td>
          <!--<table class="Grid"  cellpadding="0" cellspacing="0">
              <tr>
                <td style="vertical-align:top;">-->
          <xsl:if test="count(Images/Image) > 0">
            <a href="Listing.aspx?id={@id}">
              <img id="featureImage" class="Photo">
                <xsl:variable name="displayImage">
                  <xsl:value-of select="Images/@defaultImage"/>
                </xsl:variable>
                <xsl:attribute name="src">
                  <xsl:text>ImageHandler.ashx?id=</xsl:text>
                  <xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
                  <xsl:text>&amp;mode=320</xsl:text>
                </xsl:attribute>
              </img>
            </a>
            <script language="javascript">
              InitImageRotator('<xsl:for-each select="Images/Image">
                <xsl:text>/</xsl:text>
                <xsl:value-of select="@fileId"></xsl:value-of>
              </xsl:for-each>');
            </script>
          </xsl:if>
          <!--</td>
              </tr>
            </table>-->
        </td>
        <td style="vertical-align:top;width:100%">
          <table  style="width:100%;">
            <tr>
              <th>
                <span style="float:left; padding-right:5px;">
                  <xsl:value-of select="Title"></xsl:value-of>
                  <xsl:call-template name="Distance"></xsl:call-template>
                </span>
                <xsl:call-template name="HeaderIcons"/>
              </th>
            </tr>
            <tr>
              <td>
                <xsl:value-of select="substring(Description,0,300)"/>
                <xsl:if test="string-length(Description) > 300">
                  <xsl:text>...</xsl:text>
                </xsl:if>
              </td>
            </tr>
            <tr>
              <td>
                <table class="Grid" cellpadding="0" cellspacing="0">
                  <tr>
                    <td class="dark">
                      Address
                    </td>
                    <td colspan="3">
                      <xsl:call-template name="ShortAddress"></xsl:call-template>
                    </td>
                  </tr>
                  <tr>
                    <td class="dark">
                      Monthly Rent
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="not(@monthlyRent)">
                          <xsl:text>Not Specified</xsl:text>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>$</xsl:text>
                          <xsl:value-of select="format-number(@monthlyRent,'###,###,###')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                    <td class="dark">
                      Property Type
                    </td>
                    <td>
                      <xsl:if test="not(RentalPropertyType)">
                        <xsl:text>Not Specified</xsl:text>
                      </xsl:if>
                      <xsl:apply-templates select="RentalPropertyType"/>
                      <xsl:apply-templates select="FurnishingType"/>
                    </td>
                  </tr>
                  <tr>
                    <td class="dark">
                      Size
                    </td>
                    <td>
                      <xsl:apply-templates select="Size"></xsl:apply-templates>
                    </td>
                    <td class="dark">Lot Size</td>
                    <td>
                      <xsl:apply-templates select="Lot/Size"/>
                    </td>
                  </tr>
                  <!--
                  <tr>
                    <td class="dark">Age</td>
                    <td>
                      <xsl:value-of select="@yearBuilt"></xsl:value-of>
                    </td>
                    <td class="dark">Levels</td>
                    <td>
                      <xsl:value-of select="count(Levels/Level)"></xsl:value-of>
                    </td>
                  </tr>-->
                  <tr>
                    <td class="dark">Bedrooms</td>
                    <td>
                      <xsl:value-of select="@numberOfBedrooms"></xsl:value-of>
                    </td>
                    <td class="dark">Bathrooms</td>
                    <td>
                      <xsl:value-of select="@numberOfBathrooms"></xsl:value-of>
                    </td>
                  </tr>
                  <tr>
                    <td class="dark">Flooring</td>
                    <td>
                      <xsl:for-each select="//Listing/Levels/Level/Rooms/Room[generate-id(.)=generate-id(key('FloorKey', FloorMaterial)[1])]">
                        <xsl:if test="not(FloorMaterial/text()=0)">
                          <xsl:apply-templates select="FloorMaterial"></xsl:apply-templates>
                          <xsl:choose>
                            <xsl:when test="position() != last()">
                              <xsl:text>, </xsl:text>
                            </xsl:when>
                          </xsl:choose>
                        </xsl:if>
                      </xsl:for-each>
                    </td>
                    <td class="dark">
                      Parking Spaces
                    </td>
                    <td>
                      <xsl:choose>
                        <xsl:when test="@parkingSpaces > 0">
                          <xsl:value-of select="@parkingSpaces"/>
                          <xsl:if test="@parkingIncluded">
                            <xsl:text> included</xsl:text>
                          </xsl:if>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:text>No Parking</xsl:text>
                        </xsl:otherwise>
                      </xsl:choose>
                    </td>
                  </tr>
                  <xsl:if test="Features or Appliances">
                    <tr>
                      <xsl:if test="Features">
                        <td class="dark">
                          <xsl:attribute name="colspan">
                            <xsl:choose>
                              <xsl:when test="Appliances">
                                <xsl:text>2</xsl:text>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:text>4</xsl:text>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:attribute>
                          <xsl:text>Features</xsl:text>
                        </td>
                      </xsl:if>
                      <xsl:if test="Appliances">
                        <td class="dark">
                          <xsl:attribute name="colspan">
                            <xsl:choose>
                              <xsl:when test="Features">
                                <xsl:text>2</xsl:text>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:text>4</xsl:text>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:attribute>
                          <xsl:text>Appliances</xsl:text>
                        </td>
                      </xsl:if>
                    </tr>
                    <tr>
                      <xsl:if test="Features">
                        <td>
                          <xsl:attribute name="colspan">
                            <xsl:choose>
                              <xsl:when test="Appliances">
                                <xsl:text>2</xsl:text>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:text>4</xsl:text>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:attribute>
                          <xsl:apply-templates select="Features"></xsl:apply-templates>
                        </td>
                      </xsl:if>
                      <xsl:if test="Appliances">
                        <td>
                          <xsl:attribute name="colspan">
                            <xsl:choose>
                              <xsl:when test="Features">
                                <xsl:text>2</xsl:text>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:text>4</xsl:text>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:attribute>
                          <xsl:apply-templates select="Appliances"></xsl:apply-templates>
                        </td>
                      </xsl:if>
                    </tr>
                  </xsl:if>
                  <tr>
                    <td class="dark" colspan="4">
                      <a href="Listing.aspx?id={@id}" class="linkView" title="View Listing">View</a>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>

  <!-- The template to use for the pin results -->
  <xsl:template name="Pin">
    <div onclick="GetListingContent('{@id}')">
      <span>
        <xsl:call-template name="HeaderIcons"/>
      </span>
      <xsl:call-template name="Distance"></xsl:call-template>
    </div>
    <xsl:if test="count(Images/Image) > 0">
      <img id="featureImage" style="border: solid 1px #BBBBBB;cursor:pointer" onclick="GetListingContent('{@id}')" >
        <xsl:variable name="displayImage">
          <xsl:value-of select="Images/@defaultImage"/>
        </xsl:variable>
        <xsl:attribute name="src">
          <xsl:text>ImageHandler.ashx?id=</xsl:text>
          <xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
          <xsl:text>&amp;mode=160</xsl:text>
        </xsl:attribute>
      </img>
    </xsl:if>
    <p>
      <xsl:value-of select="substring(Description,0,300)"/>
      <xsl:if test="string-length(Description) > 300">
        <xsl:text>...</xsl:text>
      </xsl:if>
    </p>
    <a onclick="Pan({Address/@longitude},{Address/@latitude})">Centre</a>
    <a onclick="GetListingContent('{@id}')" class="linkView" title="View Listing">View</a>
  </xsl:template>

  <!-- Misc Templates-->
  <xsl:template name="HeaderIcons">
    <xsl:if test="@parkingIncluded">
      <span class="ToolParking" Title="Parking is included with rent"/>
    </xsl:if>
    <xsl:if test="@pets">
      <span class="ToolDog" Title="Pets are allowed"/>
    </xsl:if>
    <xsl:if test="@laundryService">
      <span class="ToolLaundry" Title="Laundry Facilities"/>
    </xsl:if>
    <xsl:if test="@internetService">
      <span class="ToolInternet" Title="Internet Service"/>
    </xsl:if>
    <xsl:if test="@televisionService">
      <span class="ToolTelevision" Title="Television Service"/>
    </xsl:if>
    <xsl:choose>
      <xsl:when test ="@includesElectricity ">
        <span class="ToolUtility" Title="Utilities Included"/>
      </xsl:when>
      <xsl:when test ="@includesHeating ">
        <span class="ToolUtility" Title="Utilities Included"/>
      </xsl:when>
      <xsl:when test ="@includesWater ">
        <span class="ToolUtility" Title="Utilities Included"/>
      </xsl:when>
    </xsl:choose>
    <xsl:if test="@pool">
      <span class="ToolPool" Title="This location features a pool"/>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>





