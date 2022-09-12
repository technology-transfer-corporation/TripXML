<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_ShowMileageRQ.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_ShowMileageRQ" />
	</xsl:template>
	<xsl:template match="OTA_ShowMileageRQ">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:text>FQM</xsl:text>
					<xsl:value-of select="FromCity" />
					<xsl:apply-templates select="ToCity" />
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
	<xsl:template match="ToCity">
		<xsl:value-of select="." />
	</xsl:template>
</xsl:stylesheet>
