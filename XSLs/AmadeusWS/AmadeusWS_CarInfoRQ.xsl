<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CarInfoRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 29 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehLocDetailRQ " />
	</xsl:template>
	<xsl:template match="OTA_VehLocDetailRQ">
		<Car_Policy>
			<companySelection>
				<companyInformation>
					<companyCode>
						<xsl:value-of select="Vendor/@Code" />
					</companyCode>
				</companyInformation>
			</companySelection>
			<locationSelection>
				<cityOrAirportTag>215</cityOrAirportTag>
				<locationInfo>
					<locationCode>
						<xsl:value-of select="Location/@LocationCode" />
					</locationCode>
				</locationInfo>
			</locationSelection>
		</Car_Policy>
	</xsl:template>
</xsl:stylesheet>
