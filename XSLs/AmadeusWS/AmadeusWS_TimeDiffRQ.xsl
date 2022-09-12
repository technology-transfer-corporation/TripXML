<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_TimeDiffRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_TimeDiffRQ" />
	</xsl:template>
	<xsl:template match="OTA_TimeDiffRQ">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:text>DDMIA</xsl:text><xsl:value-of select="substring(translate(LocalInfo/Time,':',''),1,4)" />/<xsl:value-of select="LocalInfo/CityCode" />
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
</xsl:stylesheet>
