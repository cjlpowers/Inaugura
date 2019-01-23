<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template name="FlashPlayer">
    <xsl:if test="Details/Detail[@key='IsSetup']='True'">
      <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0" name="movie" width="250px" height="97px" align="center" id="movie">
        <param name="movie" value="MP3Player.swf" />
        <param name="menu" value="false" />
        <param name="quality" value="high" />
        <param name="scale" value="noborder" />
        <param name="wmode" value="transparent" />
        <param name="bgcolor" value="#000000" />
        <PARAM NAME="FlashVars">
          <xsl:attribute name="value">
            <xsl:if test ="Details/Detail[@key='InformationPrompt']">
              <xsl:text>description=</xsl:text>
              <xsl:text>Content%2fAudio%2f</xsl:text>
              <xsl:value-of select="Details/Detail[@key='InformationPrompt']"></xsl:value-of>
              <xsl:text>.mp3&amp;</xsl:text>
            </xsl:if>
            <xsl:if test ="Details/Detail[@key='OpenHousePrompt']">
              <xsl:text>openhouse=</xsl:text>
              <xsl:text>Content%2fAudio%2f</xsl:text>
              <xsl:value-of select="Details/Detail[@key='OpenHousePrompt']"></xsl:value-of>
              <xsl:text>.mp3&amp;</xsl:text>
            </xsl:if>
          </xsl:attribute>
        </PARAM>
        <EMBED wmode="transparent" src="MP3Player.swf" width="250px" height="97px" FlashVars="description=Content%2fAudio%2f3490cb06-a5f5-48fd-92a1-37480e165860.mp3&amp;openhouse=Content%2fAudio%2fd7b05df4-785a-4004-849e-2fe5c3a7b227.mp3"/>
      </object>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>

