<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_SearchNameRQ.xsl												-->
<!-- ================================================================== -->
<!-- Date: 11 Aug 2014 - Rastko - new file												-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="TT_SearchNameRQ"/>
	</xsl:template>
	<xsl:template match="TT_SearchNameRQ">
		<PNR_Retrieve>
			<retrievalFacts>
				<retrieve>
					<type>3</type>
					<option1>A</option1>
				</retrieve>
				<personalFacts>
					<travellerInformation>
						<traveller>
							<surname>
								<xsl:value-of select="Surname"/>
							</surname>
						</traveller>
					</travellerInformation>
				</personalFacts>
			</retrievalFacts>
		</PNR_Retrieve>
	</xsl:template>
</xsl:stylesheet>
