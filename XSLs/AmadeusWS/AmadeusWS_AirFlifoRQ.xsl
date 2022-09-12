<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirFlifoRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 14 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirFlifoRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirFlifoRQ">
		<Air_FlightInfo>
			<generalFlightInfo>
				<flightDate>
					<departureDate>
						<xsl:value-of select="substring(DepartureDate,9,2)" />
						<xsl:value-of select="substring(DepartureDate,6,2)" />
						<xsl:value-of select="substring(DepartureDate,3,2)" />
					</departureDate>
				</flightDate>
				<xsl:if test="DepartureAirport/@LocationCode!=''">
					<boardPointDetails>
						<trueLocationId>
							<xsl:value-of select="DepartureAirport/@LocationCode" />
						</trueLocationId>
					</boardPointDetails>
				</xsl:if>
				<xsl:if test="ArrivalAirport/@LocationCode!=''">
					<offPointDetails>
						<trueLocationId>
							<xsl:value-of select="ArrivalAirport/@LocationCode" />
						</trueLocationId>
					</offPointDetails>
				</xsl:if>
				<companyDetails>
					<marketingCompany>
						<xsl:value-of select="Airline/@Code" />
					</marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber>
						<xsl:value-of select="FlightNumber" />
					</flightNumber>
				</flightIdentification>
			</generalFlightInfo>
		</Air_FlightInfo>
	</xsl:template>
</xsl:stylesheet>