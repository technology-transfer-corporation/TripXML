<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_PNRCancelRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 22 Jun 2005 - New message - Rastko										       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<xsl:apply-templates select="XPW3" />
	<xsl:apply-templates select="XXW" />
</xsl:template>

<xsl:template match="XPW3">
	<OTA_CancelRS>
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:attribute name="Status">Cancelled</xsl:attribute>
		<Success />
		<UniqueID>
			<xsl:attribute name="ID">
				<xsl:value-of select="PNR_RLOC" />
			</xsl:attribute>
		</UniqueID>
	</OTA_CancelRS>
</xsl:template>

<xsl:template match="XXW">
	<OTA_CancelRS>
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:attribute name="Status">Unsuccessful</xsl:attribute>
		<Errors>
			<Error>
				<xsl:attribute name="Type">Worldspan</xsl:attribute>
				<xsl:attribute name="Code">
					<xsl:value-of select="ERROR/CODE"/>
				</xsl:attribute>
				<xsl:value-of select="ERROR/TEXT"/>
			</Error>
		</Errors>
	</OTA_CancelRS>    		
 </xsl:template>  
  
</xsl:stylesheet> 


