<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CurConvRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_CurConvRQ" />
	</xsl:template>
	<xsl:template match="OTA_CurConvRQ">
		<xsl:apply-templates select="CurrencyRequest" />
	</xsl:template>
	<xsl:template match="CurrencyRequest">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:apply-templates select="Amount" />
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
	
	<xsl:template match="Amount">
		<xsl:text>FQC</xsl:text><xsl:value-of select="." /><xsl:value-of select="../FromCurrencyCode" />/<xsl:value-of select="../ToCurrencyCode" />
	</xsl:template>
</xsl:stylesheet>
