<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ************************************************************** -->
	<!-- CarDescriptionRQ                     	                              -->
	<!--		 Input   : OTA_VehLocDetailRQ                           -->
	<!--         Output: Galileo_CarDescriptionRQ                      -->
	<!-- ************************************************************** -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehLocDetailRQ " />
	</xsl:template>
	<xsl:template match="OTA_VehLocDetailRQ">
		<CarDescription_5_0>
			<CarDescMods>
				<StartDt />
				<City>
					<xsl:value-of select="Location/@LocationCode" />
				</City>
				<CarSupplier>
					<xsl:value-of select="Vendor/@Code" />
				</CarSupplier>
				<KeywordAry>
					<Keyword>CARS</Keyword>
				</KeywordAry>
			</CarDescMods>
		</CarDescription_5_0>
	</xsl:template>
</xsl:stylesheet>