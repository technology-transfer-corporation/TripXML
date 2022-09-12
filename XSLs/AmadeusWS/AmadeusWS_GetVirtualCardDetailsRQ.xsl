<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
   AmadeusWS_GetVirtualCardDetailsRQ.xsl															
  ==================================================================
   Date: 22 Jan 2019 - Kobelev - Adjustment for TripXML schema
   Date: 03 Jan 2019 - Alex Kobelev - new file      														
  ================================================================== 
  -->  
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PAY_GetVirtualCardDetailsRQ"/>
	</xsl:template>
	<xsl:template match="PAY_GetVirtualCardDetailsRQ">
    <AMA_PAY_GetVirtualCardDetailsRQ Version="2.0" xmlns="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2">
      <References>
          <xsl:for-each select="References/Reference">
            <Reference>
              <xsl:attribute name="Type">
                <xsl:value-of select="@Type"/>  
              </xsl:attribute>
              <xsl:value-of select="."/>
            </Reference>
          </xsl:for-each>
      </References>
      <DisplayFilter>Full</DisplayFilter>
		</AMA_PAY_GetVirtualCardDetailsRQ>
	</xsl:template>
</xsl:stylesheet>
