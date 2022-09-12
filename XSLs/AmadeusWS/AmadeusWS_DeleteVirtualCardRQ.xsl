<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- 
==================================================================
AmadeusWS_DeleteVirtualCardRQ.xsl									
================================================================== 
Date: 01 Feb 2019 - Kobelev - Refectoring for TripXML		
Date: 16 Jan 2019 - Rastko - new file											
================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PAY_DeleteVirtualCardRQ"/>
	</xsl:template>
	<xsl:template match="PAY_DeleteVirtualCardRQ">
    <AMA_PAY_DeleteVirtualCardRQ  Version="2.0" xmlns="http://xml.amadeus.com/2010/06/PAY_VirtualCard_v2" xmlns:pay1="http://xml.amadeus.com/2010/06/PAY_Types_v1">
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
      <Reason>
        <xsl:value-of select="Reason"/>
      </Reason>
		</AMA_PAY_DeleteVirtualCardRQ>
	</xsl:template>
</xsl:stylesheet>
