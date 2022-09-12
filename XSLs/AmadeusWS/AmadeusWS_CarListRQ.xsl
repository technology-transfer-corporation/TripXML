<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CarListRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 25 Dec 2012 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehLocSearchRQ"/>
	</xsl:template>
	<xsl:template match="OTA_VehLocSearchRQ">
		<Car_LocationList>
			<xsl:if test="Vendor/@Code!=''">
				<companySelection>
					<companyInformation>
						<companyCode><xsl:value-of select="Vendor/@Code"/></companyCode>
					</companyInformation>
				</companySelection>
			</xsl:if>
			<cityAirportSelection>
				<cityOrAirportTag>26</cityOrAirportTag>
				<locationInfo>
					<airportOrCityCode>
						<xsl:value-of select="VehLocSearchCriterion/CodeRef/@LocationCode"/>
					</airportOrCityCode>
				</locationInfo>
			</cityAirportSelection>
		</Car_LocationList>
	</xsl:template>
</xsl:stylesheet>
