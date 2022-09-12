<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
	Worldspan_AuthorizationRS.xsl
	==================================================================
  Date: 01 Feb 2018 - Samokhvalov - New file
	================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_AuthorizationRS"/>
  </xsl:template>


  <!--
  <OTA_AuthorizationRS  TransactionIdentifier="P105514301517578086797" Version="1">
    <Success />
    <Authorization>
      <Warnings>
        <Warning Code="450" Type="3">0000084CCQ DCCA - INVLD FORMAT         - CA</Warning>
      </Warnings>
    </Authorization>
  </OTA_AuthorizationRS>
  -->

  <xsl:template match="OTA_AuthorizationRS">
    <OTA_AuthorizationRS Version="1">
      <xsl:choose>
        <xsl:when test="Authorization/Warnings">
          <Errors>
            <Error>
              <xsl:value-of select="Authorization/Warnings/Warning"/>
            </Error>
          </Errors>
        </xsl:when>
        <xsl:otherwise>
          <Success/>
          <xsl:copy-of select="Authorization"/>
        </xsl:otherwise>
      </xsl:choose>
      <ConversationID>None</ConversationID>
    </OTA_AuthorizationRS>
  </xsl:template>


</xsl:stylesheet>
