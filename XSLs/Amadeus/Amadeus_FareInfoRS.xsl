<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_FareInfoRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 22 Apr 2008 - Rastko														-->
	<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<xsl:apply-templates select="MessagesOnly_Reply" />
	</xsl:template>
	
	<xsl:template match="MessagesOnly_Reply">
		<OTA_AirFareInfoRS Version="1.000">
			<Errors>
				<xsl:apply-templates select="CAPI_Messages" />
			</Errors>
		</OTA_AirFareInfoRS>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:choose>
				<xsl:when test="Text = 'Local error'">
					<xsl:attribute name="Type">Traveltalk</xsl:attribute>
					<xsl:attribute name="Code">TT001</xsl:attribute>
					<xsl:text>Invalid provider message structure or message content</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:value-of select="Text" />
				</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>
		
</xsl:stylesheet>
