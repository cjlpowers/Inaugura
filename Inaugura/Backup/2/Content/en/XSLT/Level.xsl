<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:import href="Features.xsl"/>
  <xsl:import href="Size.xsl"/>
  <xsl:import href="Room.xsl"/>
  <xsl:param name="admin" />
  <xsl:template match="Level">
    <div class="level">
      <span class="h3">
        <xsl:value-of select ="@name"></xsl:value-of>
      </span>
      <xsl:if test="$admin='True'">
        <div class="admin">
          <a class="adminTask" href="javascript:if(confirm('Are you sure you want to remove this level?'))FireActionEvent('removelevel','listing,{//Listing/@id};level,{@id}')">Remove Level</a>
          <a class="adminTask"  href="javascript:FireEvent('edit,~/Controls/Edit/EditLevel.ascx;listing,{//Listing/@id};level,{@id}')">Edit Level</a>
          <a class="adminTask"  href="javascript:FireEvent('edit,~/Controls/Edit/EditRoom.ascx;listing,{//Listing/@id};level,{@id}')">New Room</a>
        </div>
      </xsl:if>
      <div class="details">
        <xsl:if test="Size">
          <span>
            <font class="label">Size: </font>
            <font class="highlight">
              <xsl:apply-templates select="Size"/>
            </font>
          </span>
        </xsl:if>
        <span>
          <xsl:choose>
            <xsl:when test="@aboveGrade ='True'">
              <span class="IconAboveGrade" Title="This level is above grade"/>
            </xsl:when>
            <xsl:otherwise>
              <span class="IconBelowGrade" Title="This level is below grade"/>
            </xsl:otherwise>
          </xsl:choose>
        </span>
        <xsl:if test="Description">
          <p>
            <xsl:value-of select="Description"></xsl:value-of>
          </p>
        </xsl:if>
        <xsl:if test ="count(Rooms/Room) > 0 or $admin='True'">          
          <div class="rooms">
            <xsl:if test="$admin='True'">
                          
            </xsl:if>
            <!--<xsl:apply-templates select="Rooms/Room"></xsl:apply-templates>-->
            <ul>
              <xsl:for-each select="Rooms/Room">
                <li>                  
                  <div><xsl:if test="$admin='True'">
                      <div class="admin">
                        <a class="adminTask" href="javascript:if(confirm('Are you sure you want to remove this room?'))FireActionEvent('removeroom','listing,{//Listing/@id};level,{../../@id};room,{@id}')">Remove</a>
                        <a class="adminTask" href="javascript:FireEvent('edit,~/Controls/Edit/EditRoom.ascx;listing,{//Listing/@id};level,{../../@id};room,{@id}')">Edit</a>
                      </div>
                    </xsl:if>
                    <span class="detail">
                      <xsl:value-of select ="@name"></xsl:value-of>
                      <xsl:if test="RoomType">
                        <xsl:text> (</xsl:text>
                        <xsl:apply-templates select="RoomType"/>
                        <xsl:text>)</xsl:text>
                      </xsl:if>
                    </span>
                    
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
                  </div>
                  <xsl:if test="Description">
                    <div>
                      <xsl:value-of select="Description"></xsl:value-of>
                    </div>
                  </xsl:if>
                  <xsl:if test="Features">
                    <h4>Features</h4>
                    <xsl:apply-templates select="Features"></xsl:apply-templates>
                  </xsl:if>                  
                </li>
              </xsl:for-each>
            </ul>
          </div>
        </xsl:if>
      </div>
    </div>

    <!--<div class="room">
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
    </div>-->
    <!--<table class="Grid"  cellpadding="0" cellspacing="0">
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
    </table>-->
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

