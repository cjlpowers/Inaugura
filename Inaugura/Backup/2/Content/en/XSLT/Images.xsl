<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="admin" />

  <xsl:template match="ImageOld">
    <table class="Grid" cellpadding="0" cellspacing="0">
      <tr>
        <xsl:if test="@name">
          <th colspan="2">
            <xsl:value-of select="substring(@name,0,20)"/>
            <xsl:if test="string-length(@name) > 20">
              <xsl:text>...</xsl:text>
            </xsl:if>
          </th>
        </xsl:if>
        <xsl:if test="$admin='True'">
          <tr>
            <td class="admin">
              <xsl:call-template name="ImageAdmin"/>
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td colspan="2" style="text-align:center">
            <!--<img src="ImageHandler.ashx?id={@fileId}&amp;mode=160" onclick="javascript:SetFrame('ShowImage.aspx?listingID={/Listing/@id}&amp;ImageID={@id}',490,430)" title="{Description/text()}"/>-->
            <a href="javascript:SetImage('{@fileId}')">
              <img class="Photo" src="ImageHandler.ashx?id={@fileId}&amp;mode=160" title="{Description/text()}"/>
            </a>
          </td>
        </tr>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="ImageOld2">
    <table class="Grid" cellpadding="0" cellspacing="0">
      <tr>
        <xsl:if test="@name">
          <th colspan="2">
            <xsl:value-of select="substring(@name,0,20)"/>
            <xsl:if test="string-length(@name) > 20">
              <xsl:text>...</xsl:text>
            </xsl:if>
          </th>
        </xsl:if>
        <xsl:if test="$admin='True'">
          <tr>
            <td class="admin">
              <xsl:call-template name="ImageAdmin"/>
            </td>
          </tr>
        </xsl:if>
        <tr>
          <td colspan="2" style="text-align:center">
            <a href="javascript:SetImage('{@fileId}')">
              <img class="Photo"  src="ImageHandler.ashx?id={@fileId}&amp;mode=160" title="{Description/text()}"/>
            </a>
          </td>
        </tr>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="Image">
    <span class="PhotoSm">
      <a href="javascript:SetImage('{@fileId}')">
        <img class="PhotoSm"  src="ImageHandler.ashx?id={@fileId}&amp;mode=80" title="{Description/text()}"/>
      </a>
    </span>
  </xsl:template>

  <xsl:template name="ImageAdmin">
    <span class="ToolEdit" title="Edit this Image"  onclick="SetFrame('Secure/PopupControls/EditImage.aspx?listingID={/Listing/@id}&amp;imageID={@id}',450,180)"/>
    <xsl:if test="position() !=1">
      <span class="ToolUp" onclick="ListingOpperation('{/Listing/@id}','{@id}','imageup')" title="Move this image up in the list"/>
    </xsl:if>
    <xsl:if test="position() !=last()">
      <span class="ToolDown" onclick="ListingOpperation('{/Listing/@id}','{@id}','imagedown')" title="Move this image down in the list"/>
    </xsl:if>
    <xsl:variable name="defaultImageID" select="/Listing/Images/@defaultImage"></xsl:variable>
    <xsl:if test="@id != $defaultImageID">
      <span class="ToolFlag" onclick="ListingOpperation('{/Listing/@id}','{@id}','imagedefault')" title="Make this my default image"/>
    </xsl:if>
    <span class="ToolDelete" onclick="if(confirm('Are you sure you want to delete this image?'))ListingOpperation('{/Listing/@id}','{@id}','imagedelete');" title="Delete this image"/>
  </xsl:template>
</xsl:stylesheet>