<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_StoredFareBuildRQ.xsl 		  			       					      -->
	<!-- ================================================================== -->
	<!-- Date: 19 Nov 2010 - Rastko 													            -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:value-of select="'TTC'" />
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
	
</xsl:stylesheet>
