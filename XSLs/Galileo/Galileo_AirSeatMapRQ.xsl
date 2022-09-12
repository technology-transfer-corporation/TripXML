<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirSeatMapRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirSeatMapRQ">
		<SeatMap_5_0>
			<SeatMapMods>
				<xsl:apply-templates select="SeatMapRequests/SeatMapRequest" />
				<xsl:if test="AirTravelers">
					<NameAry>
						<xsl:apply-templates select="AirTravelers/AirTraveler" />
					</NameAry>
				</xsl:if>
			</SeatMapMods>
		</SeatMap_5_0>
	</xsl:template>
	<!-- *************************************************************************-->
	<!--       				   Process Flight Information                          -->
	<!-- *************************************************************************-->
	<xsl:template match="SeatMapRequest">
		<xsl:choose>
			<xsl:when test="FlightSegmentInfo/OperatingAirline/@FlightNumber!=''">
				<AirV>
					<xsl:value-of select="FlightSegmentInfo/OperatingAirline/@Code" />
					<xsl:text><![CDATA[ ]]></xsl:text>
				</AirV>
				<FltNum>
					<xsl:value-of select="FlightSegmentInfo/OperatingAirline/@FlightNumber" />
				</FltNum>
			</xsl:when>
			<xsl:otherwise>
				<AirV>
					<xsl:value-of select="FlightSegmentInfo/MarketingAirline/@Code" />
					<xsl:text><![CDATA[ ]]></xsl:text>
				</AirV>
				<FltNum>
					<xsl:value-of select="FlightSegmentInfo/@FlightNumber" />
				</FltNum>
			</xsl:otherwise>
		</xsl:choose>
		<BIC>
			<xsl:value-of select="SeatDetails/ResBookDesignations/ResBookDesignation[1]/@ResBookDesigCode" />
			<xsl:text><![CDATA[ ]]></xsl:text>
		</BIC>
		<StartDt>
			<xsl:value-of select="substring(FlightSegmentInfo/@DepartureDateTime,1,4)" />
			<xsl:value-of select="substring(FlightSegmentInfo/@DepartureDateTime,6,2)" />
			<xsl:value-of select="substring(FlightSegmentInfo/@DepartureDateTime,9,2)" />
		</StartDt>
		<StartCity>
			<xsl:value-of select="FlightSegmentInfo/DepartureAirport/@LocationCode" />
			<xsl:text><![CDATA[  ]]></xsl:text>
		</StartCity>
		<EndCity>
			<xsl:value-of select="FlightSegmentInfo/ArrivalAirport/@LocationCode" />
			<xsl:text><![CDATA[  ]]></xsl:text>
		</EndCity>
		<!--<NumSeats></NumSeats>
	 <Spare></Spare></xsl:template> -->
	</xsl:template>
	<!-- *************************************************************************-->
	<!--       			   Process Traveler Information                           -->
	<!-- *************************************************************************-->
	<xsl:template match="AirTraveler">
		<Name>
			<LName>
				<xsl:value-of select="PersonName/Surname" />
			</LName>
			<FNameAry>
				<FNameInfo>
					<FName>
						<xsl:value-of select="PersonName/GivenName" />
					</FName>
					<xsl:if test="CustLoyalty">
						<FreqFlierID>
							<xsl:value-of select="CustLoyalty[1]/@ProgramID" />
						</FreqFlierID>
						<FreqFlierNum>
							<xsl:value-of select="CustLoyalty[1]/@MembershipID" />
						</FreqFlierNum>
					</xsl:if>
				</FNameInfo>
			</FNameAry>
		</Name>
	</xsl:template>
</xsl:stylesheet>