<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/0">
			<xsl:apply-templates select="OTA_CurConvRQ" />
		</SabreCommandLLSRQ>
	</xsl:template>
	<xsl:template match="OTA_CurConvRQ">
		<xsl:text>DC'</xsl:text>
		<xsl:value-of select="CurrencyRequest/FromCurrencyCode" />
		<xsl:value-of select="CurrencyRequest/Amount" />
		<xsl:text>/</xsl:text>
		<xsl:value-of select="CurrencyRequest/ToCurrencyCode" />
		<xsl:text>@screen</xsl:text>
	</xsl:template>
</xsl:stylesheet>
