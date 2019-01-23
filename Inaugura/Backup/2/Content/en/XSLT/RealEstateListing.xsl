<?xml version="1.0"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:import href="FlashPlayer.xsl"/>
	<xsl:import href="PropertyType.xsl"/>
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
					<xsl:value-of select="Title"/>
          <xsl:call-template name="Distance"></xsl:call-template>
					<span>
						<xsl:call-template name="HeaderIcons"/>
					</span>
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
					<td width="160" rowspan="5"  align="center">
						<a href="Listing.aspx?id={@id}">
							<img>
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
					Price
				</td>
				<td>
					<xsl:choose>
						<xsl:when test="not(@price)">
							<xsl:text>Not Specified</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>$</xsl:text>
							<xsl:value-of select="format-number(@price,'###,###,###')"/>
						</xsl:otherwise>
					</xsl:choose>
				</td>
				<td class="dark">
					Property Type
				</td>
				<td>
					<xsl:apply-templates select="PropertyType"/>
				</td>
			</tr>
			<tr>
				<td class="dark">
					Rooms
				</td>
				<td>
					<xsl:value-of select="@numberOfBedrooms"/>
				</td>
				<td class="dark">
					Bathrooms
				</td>
				<td>
					<xsl:value-of select="@numberOfBathrooms"/>
				</td>
			</tr>
			<tr>
				<td class="dark">
					Lot Size
				</td>
				<td>
					<xsl:apply-templates select="Size"/>
				</td>
				<td colspan="2">
					<a href="Listing.aspx?id={@id}">
						<img src="Content/en/images/buttons/moreinfo.gif"></img>
					</a>
				</td>
			</tr>
		</table>
	</xsl:template>
	<!-- The template used to display search results-->
	<xsl:template name="Details">		
		<table>			
			<tr>
				<td valign="top" width="100%">
					<table class="Grid"  cellpadding="0" cellspacing="0">
						<tr>
							<th colspan="4">
								<xsl:value-of select="Title"></xsl:value-of>
                <xsl:call-template name="Distance"></xsl:call-template>
                <span>
                  <xsl:call-template name="HeaderIcons"/>
                </span>
              </th>
            </tr>
						<xsl:if test="$admin='True'">
							<tr>
								<td class="admin" colspan="4">
									<xsl:call-template name="ListingAdmin"/>
								</td>
							</tr>
							<tr>
								<td class="info" colspan="4">
                  <xsl:call-template name="ListingStatus"/>
                </td>
              </tr>
            </xsl:if>
            <tr>
              <td colspan="4" align="center" id="Player">
                <xsl:call-template name="FlashPlayer"></xsl:call-template>
              </td>
            </tr>
            <xsl:if test="count(Images/Image) > 0">
              <tr>
                <td colspan="4" align="center" id="ListingImage">
                  <xsl:variable name="defaultImageID" select="Images/@defaultImage"></xsl:variable>
                  <img>
                    <xsl:attribute name="src">
                      <xsl:text>ImageHandler.ashx?id=</xsl:text>
                      <xsl:value-of select="Images/Image[@id=$defaultImageID]/@fileId"></xsl:value-of>
                      <xsl:text>&amp;mode=480</xsl:text>
                    </xsl:attribute>
                    <xsl:attribute name="title">
                      <xsl:value-of select="Images/Image[@id=$defaultImageID]/Description"></xsl:value-of>
                    </xsl:attribute>
                  </img>
                </td>
              </tr>
						</xsl:if>
						<tr>
							<td class="dark" rowspan="3">Address</td>
							<td rowspan="3">
								<xsl:call-template name="LongAddress"></xsl:call-template>
								<xsl:if test="@mapUrl">
									<a href="{@mapUrl}">Map</a>
								</xsl:if>
							</td>
							<td class="dark">Price</td>
							<td>
								<xsl:choose>
									<xsl:when test="not(@price)">
										<xsl:text>Not Specified</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>$</xsl:text>
										<xsl:value-of select="format-number(@price,'###,###,###')"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
						<tr>
							<td class="dark">Appraisal</td>
							<td>
								<xsl:choose>
									<xsl:when test="not(@appraisal)">
										<xsl:text>Not Specified</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>$</xsl:text>
										<xsl:value-of select="format-number(@appraisal,'###,###,###')"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
						<tr>
							<td class="dark">Taxes</td>
							<td>
								<xsl:choose>
									<xsl:when test="not(PropertyTax/@value)">
										<xsl:text>Not Specified</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>$</xsl:text>
										<xsl:value-of select="format-number(PropertyTax/@value,'###,###,###')"/>
									</xsl:otherwise>
								</xsl:choose>
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
								<xsl:apply-templates select="PropertyType"></xsl:apply-templates>
							</td>
							<td class="dark">
								Home Size
							</td>
							<td>
								<xsl:apply-templates select="Size"></xsl:apply-templates>
							</td>
						</tr>
						<tr>
							<td class="dark">Age</td>
							<td>
								<xsl:value-of select="@yearBuilt"></xsl:value-of>
							</td>
							<td class="dark">Levels</td>
							<td>
								<xsl:value-of select="count(Levels/Level)"></xsl:value-of>
							</td>
						</tr>
						<tr>
							<td class="dark">Exterior</td>
							<td>
								<xsl:apply-templates select="ExteriorPrimary"></xsl:apply-templates>
								<xsl:if test="not(ExteriorSecondary/text()=0)">
									<xsl:text> &amp; </xsl:text>
									<xsl:apply-templates select="ExteriorSecondary"></xsl:apply-templates>
								</xsl:if>
							</td>
							<td class="dark">Bedrooms</td>
							<td>
								<xsl:value-of select="@numberOfBedrooms"></xsl:value-of>
							</td>
						</tr>
						<tr>
							<td class="dark">Roof</td>
							<td>
								<xsl:apply-templates select="RoofType"></xsl:apply-templates>
							</td>
							<td class="dark">Bathrooms</td>
							<td>
								<xsl:value-of select="@numberOfBathrooms"></xsl:value-of>
							</td>
						</tr>
						<tr>
							<td class="dark">Driveway</td>
							<td>
								<xsl:apply-templates select="DrivewayType"></xsl:apply-templates>
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
						<tr>
							<td class="dark">Foundation</td>
							<td>
								<xsl:apply-templates select="FoundationType"></xsl:apply-templates>
							</td>
							<td class="dark">Heating</td>
							<td>
								<xsl:apply-templates select="HeatingPrimary"></xsl:apply-templates>
								<xsl:if test="not(HeatingSecondary/text()=0)">
									<xsl:text> &amp; </xsl:text>
									<xsl:apply-templates select="HeatingSecondary"></xsl:apply-templates>
								</xsl:if>
							</td>
						</tr>
						<tr>
							<td class="dark">Water</td>
							<td>
								<xsl:apply-templates select="WaterType"></xsl:apply-templates>
							</td>
							<td class="dark">Electrical</td>
							<td>
								<xsl:apply-templates select="ElectricalService"></xsl:apply-templates>
							</td>
						</tr>
						<tr>
							<td class="dark">Exterior Features</td>
							<td>
								<xsl:apply-templates select="ExteriorFeatures"></xsl:apply-templates>
							</td>
							<td class="dark">Interior Features</td>
							<td>
								<xsl:apply-templates select="InteriorFeatures"></xsl:apply-templates>
							</td>
						</tr>
					</table>
					<xsl:apply-templates select="Lot"/>
					<xsl:apply-templates select="Gararge"/>
					<xsl:if test="$admin='True'">
						<table class="Grid" cellpadding="0" cellspacing="0">
							<td class="admin">
								<a href="javascript:SetFrame('Secure/PopupControls/EditLevel.aspx?listingId={@id}&amp;levelId=new',450,320)">
									<span class="ToolEdit"  onclick="" title="Add New Level"/>
									<span>Add Level</span>
								</a>
							</td>
						</table>
					</xsl:if>
					<xsl:apply-templates select="Levels/Level">
						<xsl:with-param name="admin" />
					</xsl:apply-templates>
				</td>
				<td valign="top">
					<xsl:if test="$admin='True'">
						<table class="Grid" cellpadding="0" cellspacing="0">
							<td class="admin">
								<a href="javascript:SetFrame('Secure/PopupControls/AddImage.aspx?listingId={@id}',500,240)">
									<span class="ToolEdit"  onclick="" title="Add a new Image"/>
									<span>Add Image</span>
								</a>
							</td>
						</table>
					</xsl:if>
					<xsl:apply-templates select="Images/Image"></xsl:apply-templates>
				</td>
			</tr>
		</table>
	</xsl:template>

	<xsl:template name="ListingAdmin">		
		<span class="ToolEdit" onclick="SetFrame('Secure/PopupControls/EditListing.aspx?listingId={@id}',600,320)" title="Edit the listing details"/>
	</xsl:template>

	<xsl:template name="DetailsAdmin">
			<span class="ToolEdit" onclick="SetFrame('Secure/PopupControls/EditListingDetails.aspx?listingId={@id}',600,450)" title="Edit the listing details"/>		
	</xsl:template>


	<!-- The template to use for the mini results -->
	<xsl:template name="Mini">
		<td>
			<table class="Grid"  cellpadding="0" cellspacing="0" style="width:171px; height:95px; margin:2px; font-size:80%;" onclick="GetListingContent('{@id}')">
				<tr>
					<th colspan="2">
						<xsl:value-of select="Title"/>
            <xsl:call-template name="Distance"></xsl:call-template>
            <span>
              <xsl:call-template name="HeaderIcons"/>
            </span>
          </th>
        </tr>
				<tr>
					<xsl:if test="count(Images/Image) > 0">
						<td width="80px"  rowspan="3"  align="center" class="dark">
							<img>
								<xsl:variable name="displayImage">
									<xsl:value-of select="Images/@defaultImage"/>
								</xsl:variable>
								<xsl:attribute name="src">
									<xsl:text>ImageHandler.ashx?id=</xsl:text>
									<xsl:value-of select="Images/Image[@id=$displayImage]/@fileId"></xsl:value-of>
									<xsl:text>&amp;mode=80</xsl:text>
								</xsl:attribute>
							</img>
						</td>
					</xsl:if>
					<td>
						<xsl:value-of select="Address/@city"/>
						<xsl:text>, </xsl:text>
						<xsl:value-of select="Address/@stateProv"/>
					</td>
				</tr>
				<tr>
					<td>
						<xsl:choose>
							<xsl:when test="not(@price)">
								<xsl:text>Not Specified</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>$</xsl:text>
								<xsl:value-of select="format-number(@price,'###,###,###')"/>
							</xsl:otherwise>
						</xsl:choose>
					</td>
				</tr>
				<tr>
					<td>
						<xsl:value-of select="format-number(Size/@valueStdUnit * 10.764,'###,###,###')"/>
						<xsl:text> sq ft</xsl:text>
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
					<table class="Grid"  cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<a href="Listing.aspx?id={@id}">
									<img id="featureImage" style="border: solid 1px #BBBBBB;filter:progid:DXImageTransform.Microsoft.Fade(duration=2)">
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
							</td>
						</tr>
					</table>
				</td>
				<td style="vertical-align:top;width:100%">
					<table  style="width:100%;">
						<tr>
							<th>
								<xsl:value-of select="Title"></xsl:value-of>
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
											Price
										</td>
										<td>
											<xsl:choose>
												<xsl:when test="not(@price)">
													<xsl:text>Not Specified</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>$</xsl:text>
													<xsl:value-of select="format-number(@price,'###,###,###')"/>
												</xsl:otherwise>
											</xsl:choose>
										</td>
										<td class="dark">
											Property Type
										</td>
										<td>
											<xsl:apply-templates select="PropertyType"/>
										</td>
									</tr>
									<tr>
										<td class="dark">
											Home Size
										</td>
										<td>
											<xsl:apply-templates select="Size"></xsl:apply-templates>
										</td>
										<td class="dark">Lot Size</td>
										<td>
											<xsl:apply-templates select="Lot/Size"/>
										</td>
									</tr>
									<tr>
										<td class="dark">Age</td>
										<td>
											<xsl:value-of select="@yearBuilt"></xsl:value-of>
										</td>
										<td class="dark">Levels</td>
										<td>
											<xsl:value-of select="count(Levels/Level)"></xsl:value-of>
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
										<td class="dark">Gararge</td>
										<td>
											<xsl:choose>
												<xsl:when test="Gararge">
													<xsl:choose>
														<xsl:when test="Gararge/@attached='True'">
															<xsl:text>Attached</xsl:text>
														</xsl:when>
														<xsl:otherwise>
															<xsl:text>Detached</xsl:text>
														</xsl:otherwise>
													</xsl:choose>
													<xsl:text> </xsl:text>
													<xsl:value-of select="Gararge/@parkingSpaces"></xsl:value-of>
													<xsl:text> Car</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													Not Specified
												</xsl:otherwise>
											</xsl:choose>

										</td>
									</tr>
									<tr>
										<td class="dark">Exterior Features</td>
										<td>
											<xsl:apply-templates select="ExteriorFeatures"></xsl:apply-templates>
										</td>
										<td class="dark">Interior Features</td>
										<td>
											<xsl:apply-templates select="InteriorFeatures"></xsl:apply-templates>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		<script language="javascript">
			InitImageRotator('<xsl:for-each select="Images/Image">
				<xsl:text>/</xsl:text>
				<xsl:value-of select="@fileId"></xsl:value-of>
			</xsl:for-each>');
		</script>
	</xsl:template>

	<!-- Misc Templates-->
	<xsl:template name="HeaderIcons">		
		<xsl:if test="@pool">
			<span class="ToolPool" Title="This location features a pool"/>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
