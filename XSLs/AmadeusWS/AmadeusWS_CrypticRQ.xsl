<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CrypticRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CrypticRQ" />
	</xsl:template>
	<xsl:template match="CrypticRQ">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:value-of select="Entry" />
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
</xsl:stylesheet>
