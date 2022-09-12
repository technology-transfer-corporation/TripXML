<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CCValidRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_CCValidRQ/CreditCard" />
	</xsl:template>
	<xsl:template match="CreditCard">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:text>DECC</xsl:text>
					<xsl:choose>
						<xsl:when test="Code = 'MC'">CA</xsl:when>
						<xsl:otherwise><xsl:value-of select="Code" /></xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="Number" />
					<xsl:text>/</xsl:text>
					<xsl:value-of select="Expiration/Month" />
					<xsl:value-of select="Expiration/Year" />
					<xsl:text>/</xsl:text>
					<xsl:value-of select="Authorization/CurrencyCode" />
					<xsl:value-of select="Authorization/Amount" />
					<xsl:text>/</xsl:text>
					<xsl:value-of select="Authorization/CarrierCode" />
					</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
</xsl:stylesheet>
