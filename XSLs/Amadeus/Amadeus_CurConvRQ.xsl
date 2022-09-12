<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_CurConvRQ" />
	</xsl:template>
	<xsl:template match="OTA_CurConvRQ">
		<xsl:apply-templates select="CurrencyRequest" />
	</xsl:template>
	<xsl:template match="CurrencyRequest">
		<Cryptic_GetScreen_Query>
			<xsl:apply-templates select="Amount" />
		</Cryptic_GetScreen_Query>
	</xsl:template>
	<xsl:template match="Amount">
		<Command>FQC<xsl:value-of select="." /><xsl:value-of select="../FromCurrencyCode" />/<xsl:value-of select="../ToCurrencyCode" /></Command>
	</xsl:template>
</xsl:stylesheet>
