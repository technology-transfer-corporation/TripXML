<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirSeatMapRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirSeatMapRQ">
		<PoweredAir_RetrieveSeatMap>
			<travelProductIdent>
				<xsl:apply-templates select="SeatMapRequests/SeatMapRequest" />
				<xsl:if test="AirTravelers">
					<travelers>
						<xsl:apply-templates select="AirTravelers/AirTraveler" />
					</travelers>
				</xsl:if>
			</travelProductIdent>
		</PoweredAir_RetrieveSeatMap>
	</xsl:template>
	<xsl:template match="SeatMapRequest">
		<productDetails>
			<departureDate>
				<xsl:value-of select="substring(FlightSegmentInfo/@DepartureDateTime,9,2)" />
				<xsl:value-of select="substring(FlightSegmentInfo/@DepartureDateTime,6,2)" />
				<xsl:value-of select="substring(FlightSegmentInfo/@DepartureDateTime,3,2)" />
			</departureDate>
		</productDetails>
		<boardpointDetail>
			<departureCityCode>
				<xsl:value-of select="FlightSegmentInfo/DepartureAirport/@LocationCode" />
			</departureCityCode>
		</boardpointDetail>
		<offPointDetail>
			<arrivalCityCode>
				<xsl:value-of select="FlightSegmentInfo/ArrivalAirport/@LocationCode" />
			</arrivalCityCode>
		</offPointDetail>
		<companyIdentification>
			<xsl:choose>
				<xsl:when test="FlightSegmentInfo/OperatingAirline/@FlightNumber!=''">
					<operatingAirlineCode>
						<xsl:value-of select="FlightSegmentInfo/OperatingAirline/@Code" />
					</operatingAirlineCode>
				</xsl:when>
				<xsl:otherwise>
					<marketingAirlineCode>
						<xsl:value-of select="FlightSegmentInfo/MarketingAirline/@Code" />
					</marketingAirlineCode>
				</xsl:otherwise>
			</xsl:choose>
		</companyIdentification>
		<flightIdentification>
			<xsl:choose>
				<xsl:when test="FlightSegmentInfo/OperatingAirline/@FlightNumber!=''">
					<flightNumber>
						<xsl:value-of select="FlightSegmentInfo/OperatingAirline/@FlightNumber" />
					</flightNumber>
				</xsl:when>
				<xsl:otherwise>
					<flightNumber>
						<xsl:value-of select="FlightSegmentInfo/@FlightNumber" />
					</flightNumber>
				</xsl:otherwise>
			</xsl:choose>
			<classOfService>
				<xsl:value-of select="SeatDetails/ResBookDesignations/ResBookDesignation[1]/@ResBookDesigCode" />
			</classOfService>
		</flightIdentification>
	</xsl:template>
	<xsl:template match="AirTraveler">
		<travelerInformation>
			<paxSurnameDetails>
				<paxSurname>
					<xsl:value-of select="PersonName/Surname" />
				</paxSurname>
			</paxSurnameDetails>
			<individualPaxDetails>
				<individualPaxGivenName>
					<xsl:value-of select="PersonName/GivenName" />
				</individualPaxGivenName>
			</individualPaxDetails>
		</travelerInformation>
		<frequentTraveler>
			<frequentTravelerInfo>
				<frequentTravelerIdentCode>
					<xsl:value-of select="CustLoyalty[1]/@ProgramID" />
				</frequentTravelerIdentCode>
				<frequentTravelerRefNumber>
					<xsl:value-of select="CustLoyalty[1]/@MembershipID" />
				</frequentTravelerRefNumber>
			</frequentTravelerInfo>
		</frequentTraveler>
	</xsl:template>
</xsl:stylesheet>