<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_PNRCancelRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 22 Jun 2005 - New message - Rastko										       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<XPC3>
		<xsl:apply-templates select="OTA_CancelRQ"/>
	</XPC3>
</xsl:template>

<xsl:template match="OTA_CancelRQ">
	<MSG_VERSION>3</MSG_VERSION>
	<PNR_RLOC><xsl:value-of select="UniqueID/@ID"/></PNR_RLOC>
	<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>	
</xsl:template>
</xsl:stylesheet>