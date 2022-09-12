<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_CurConvRQ" />
	</xsl:template>
	<xsl:template match="OTA_CurConvRQ">
		<CurrencyConversion_6_0>
			<CurrencyConversionMods>
				<CurrencyAry>
					<xsl:apply-templates select="CurrencyRequest" />
				</CurrencyAry>
			</CurrencyConversionMods>
		</CurrencyConversion_6_0>
	</xsl:template>
	<xsl:template match="CurrencyRequest">
		<Currency>
			<Currency1>
				<xsl:value-of select="FromCurrencyCode" />
			</Currency1>
			<Currency2>
				<xsl:value-of select="ToCurrencyCode" />
			</Currency2>
		</Currency>
	</xsl:template>
</xsl:stylesheet>