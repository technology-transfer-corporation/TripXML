<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirAvailRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 24 Mar 2011 - Rastko - added missing Errors tag						-->
	<!-- Date: 21 Mar 2011 - Rastko - added missing Errors tag						-->
	<!-- Date: 16 Jan 2011 - Rastko - corrected error processing						-->
	<!-- Date: 09 Nov 2010 - Rastko - added premium class								-->
	<!-- Date: 21 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		<xsl:apply-templates select="Air_MultiAvailabilityReply" />
		<xsl:apply-templates select="Errors" />
	</xsl:template>
	
	<xsl:template match="Errors">
		<OTA_AirAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
			<Errors>
				<Error Type="Amadeus">
					<xsl:value-of select="Error"/>
				</Error>
			</Errors>
		</OTA_AirAvailRS>
	</xsl:template>

	<xsl:template match="cityPairErrorOrWarningText" mode="Error">
		<Error Type="Amadeus">
			<xsl:value-of select="concat(../../locationDetails/origin,' ')"/>
			<xsl:value-of select="concat(../../locationDetails/destination,' ')"/>
			<xsl:value-of select="freeText" />
		</Error>
	</xsl:template>
		
	<xsl:template match="Air_MultiAvailabilityReply">
		<OTA_AirAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
			<xsl:choose>
				<xsl:when test="singleCityPairInfo/cityPairErrorOrWarning/cityPairErrorOrWarningText[freeTextQualification/codedIndicator='3']">
					<Errors>
						<xsl:apply-templates select="singleCityPairInfo/cityPairErrorOrWarning/cityPairErrorOrWarningText[freeTextQualification/codedIndicator='3']" mode="Error" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
						<xsl:choose>
							<xsl:when test="//cityPairErrorOrWarning/cityPairErrorOrWarningInfo">						
								<xsl:for-each select="//cityPairErrorOrWarning/cityPairErrorOrWarningInfo">
									<xsl:choose>
										<xsl:when test="error/code='A7V'">
											<Warnings>
												<Warning>
													<xsl:attribute name="Type">A</xsl:attribute>
													<xsl:value-of select="../cityPairErrorOrWarningText/freeText"/>
													<xsl:apply-templates select="//cityPairFreeFlowText"/>
												</Warning>	
											</Warnings>
										</xsl:when>
										<xsl:otherwise>
											<Warnings>
												<Warning>
													<xsl:attribute name="Type">A</xsl:attribute>
													<xsl:attribute name="Code"><xsl:value-of select="error/code"/></xsl:attribute><xsl:value-of select="../cityPairErrorOrWarningText/freeText"/>					
													<xsl:apply-templates select="//cityPairFreeFlowText"/>
												</Warning>	
											</Warnings>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:for-each>
							</xsl:when>
							<xsl:when test="//cityPairErrorOrWarning/cityPairErrorOrWarningText">
								<Warnings>
									<xsl:apply-templates select="//cityPairErrorOrWarning/cityPairErrorOrWarningText"/>
								</Warnings>
							</xsl:when>
							<xsl:when test="//errorOrWarningSection">
								<Warnings>
									<xsl:apply-templates select="//errorOrWarningSection/textInformation"/>
								</Warnings>
							</xsl:when>
						</xsl:choose>
						<xsl:for-each select="singleCityPairInfo[flightInfo]">
							<OriginDestinationOptions>
								<xsl:for-each select="flightInfo[basicFlightInfo/productTypeDetail/productIndicators='S' or basicFlightInfo/productTypeDetail/productIndicators='D']">
									<OriginDestinationOption>		
										<TPA_Extensions>
											<TotalJourneyDuration>
												<xsl:choose>
													<xsl:when test="basicFlightInfo/productTypeDetail/productIndicators = 'D'">
														<xsl:value-of select="substring(string(additionalFlightInfo/flightDetails/legDuration),1,2)"/>:<xsl:value-of select="substring(string(additionalFlightInfo/flightDetails/legDuration),3,2)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="substring(string(following-sibling::flightInfo/additionalFlightInfo/flightDetails/legDuration),1,2)"/>:<xsl:value-of select="substring(string(following-sibling::flightInfo/additionalFlightInfo/flightDetails/legDuration),3,2)"/>
													</xsl:otherwise>
												</xsl:choose>
											</TotalJourneyDuration> 
										</TPA_Extensions>		
										<xsl:apply-templates select="." mode="leg">
											<xsl:with-param name="pot">0</xsl:with-param>
											<xsl:with-param name="rph"><xsl:value-of select="position()"/></xsl:with-param>
										</xsl:apply-templates>
										<xsl:if test="basicFlightInfo/productTypeDetail/productIndicators = 'S'">
											<xsl:apply-templates select="following-sibling::flightInfo[1]" mode="conx">
												<xsl:with-param name="pot">1</xsl:with-param>
												<xsl:with-param name="rph"><xsl:value-of select="position()"/></xsl:with-param>
											</xsl:apply-templates>
										</xsl:if>
									</OriginDestinationOption>
							</xsl:for-each>				
						</OriginDestinationOptions>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>				
	</OTA_AirAvailRS>
</xsl:template>
<!-- *********************************************************************************************************   -->
	<xsl:template match="cityPairErrorOrWarningText | textInformation">
		<Warning>
			<xsl:attribute name="Type">A</xsl:attribute>
			<xsl:attribute name="Code"><xsl:value-of select="freeTextQualification/codedIndicator"/></xsl:attribute>		
			<xsl:value-of select="freeText"/>			
			<xsl:apply-templates select="//cityPairFreeFlowText"/>
		</Warning>
	</xsl:template>
	<xsl:template match="cityPairFreeFlowText">
			<xsl:value-of select="freeText"/>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!-- Segment Section						    							           -->
	<!--**********************************************************************************************-->
	<xsl:template match="flightInfo" mode="conx">
		<xsl:param name="pot"/>
		<xsl:param name="rph"/>
		<xsl:variable name="ind"><xsl:value-of select="basicFlightInfo/productTypeDetail/productIndicators[1]"/></xsl:variable>
		<xsl:if test="$ind != 'S'">
			<xsl:apply-templates select="." mode="leg">
				<xsl:with-param name="pot"><xsl:value-of select="$pot"/></xsl:with-param>
				<xsl:with-param name="rph"><xsl:value-of select="$rph"/></xsl:with-param>
			</xsl:apply-templates>
			<xsl:apply-templates select="following-sibling::flightInfo[1]" mode="conx">
				<xsl:with-param name="pot"><xsl:value-of select="$pot"/></xsl:with-param>
				<xsl:with-param name="rph"><xsl:value-of select="$rph"/></xsl:with-param>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!--**********************************************************************************************-->
	<!-- Segments								    								    -->
	<!--**********************************************************************************************-->
	<xsl:template match="flightInfo" mode="leg">
		<xsl:param name="pot"/>
		<xsl:param name="rph"/>
		<xsl:variable name="DepDate">
			<xsl:value-of select="basicFlightInfo/flightDetails/departureDate"/>
		</xsl:variable>
		<xsl:variable name="DepDateDay">
			<xsl:value-of select="substring(string($DepDate),1,2)"/>
		</xsl:variable>
		<xsl:variable name="DepDateMonth">
			<xsl:value-of select="substring(string($DepDate),3,2)"/>
		</xsl:variable>
		<xsl:variable name="DepDateYear">
			<xsl:value-of select="substring(string($DepDate),5,2)"/>
		</xsl:variable>		
		<xsl:variable name="ArrDate">
			<xsl:value-of select="basicFlightInfo/flightDetails/arrivalDate"/>
		</xsl:variable>
		<xsl:variable name="ArrDateDay">
			<xsl:value-of select="substring(string($ArrDate),1,2)"/>
		</xsl:variable>
		<xsl:variable name="ArrDateMonth">
			<xsl:value-of select="substring(string($ArrDate),3,2)"/>
		</xsl:variable>
		<xsl:variable name="ArrDateYear">
			<xsl:value-of select="substring(string($ArrDate),5,2)"/>
		</xsl:variable>
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime">20<xsl:value-of select="$DepDateYear"/>-<xsl:value-of select="$DepDateMonth"/>-<xsl:value-of select="$DepDateDay"/>T<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureTime),1,2)"/>:<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureTime),3,2)"/>:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">20<xsl:value-of select="$ArrDateYear"/>-<xsl:value-of select="$ArrDateMonth"/>-<xsl:value-of select="$ArrDateDay"/>T<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalTime),1,2)"/>:<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalTime),3,2)"/>:00</xsl:attribute>
			<xsl:attribute name="StopQuantity"><xsl:value-of select="additionalFlightInfo/flightDetails/numberOfStops"/><xsl:value-of select="NumOfStops"/></xsl:attribute>	
			<xsl:attribute name="RPH"><xsl:value-of select="$rph"/></xsl:attribute>
			<xsl:attribute name="FlightNumber"><xsl:value-of select="basicFlightInfo/flightIdentification/number"/></xsl:attribute>
			<xsl:if test="additionalFlightInfo/flightDetails/onTimePercentage != 'N' and additionalFlightInfo/flightDetails/onTimePercentage != 'U'">
				<xsl:attribute name="OnTimeRate"><xsl:value-of select="additionalFlightInfo/flightDetails/onTimePercentage"/>0</xsl:attribute>
			</xsl:if>
			<xsl:if test="basicFlightInfo/productTypeDetail/productIndicators='ET'"><xsl:attribute name="Ticket">eTicket</xsl:attribute></xsl:if>
			<xsl:if test="basicFlightInfo/productTypeDetail/productIndicators='EN'"><xsl:attribute name="Ticket">Paper</xsl:attribute></xsl:if>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="basicFlightInfo/departureLocation/cityAirport"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="basicFlightInfo/arrivalLocation/cityAirport"/></xsl:attribute>
			</ArrivalAirport>
			<xsl:choose>		
				<xsl:when test="basicFlightInfo/operatingCompany/identifier">
					<OperatingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="basicFlightInfo/operatingCompany/identifier"/></xsl:attribute>
					</OperatingAirline>
				</xsl:when>
				<xsl:otherwise>
				<OperatingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="basicFlightInfo/marketingCompany/identifier"/></xsl:attribute>
				</OperatingAirline>
				</xsl:otherwise>
			</xsl:choose>
			<Equipment>
				<xsl:attribute name="AirEquipType"><xsl:value-of select="additionalFlightInfo/flightDetails/typeOfAircraft"/></xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="basicFlightInfo/marketingCompany/identifier"/></xsl:attribute>
			</MarketingAirline>	
			<!--xsl:if test="additionalFlightInfo/productFacilities='M'"-->
				<xsl:for-each select="cabinClassInfo">
					<MarketingCabin>
						<xsl:attribute name="CabinType">
							<xsl:choose>
								<xsl:when test="cabinInfo/cabinDesignation/cabinClassOfService = '1'">First</xsl:when>
								<xsl:when test="cabinInfo/cabinDesignation/cabinClassOfService = '2'">Business</xsl:when>
								<xsl:when test="cabinInfo/cabinDesignation/cabinClassOfService = '3'">Economy</xsl:when>
								<xsl:when test="cabinInfo/cabinDesignation/cabinClassOfService = '4'">Premium</xsl:when>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="RPH"><xsl:value-of select="position()"/></xsl:attribute>
						<xsl:if test="../additionalFlightInfo/productFacilities[type='M']">
							<Meal MealCode="M"/> 
						</xsl:if>
					</MarketingCabin>
				</xsl:for-each>
			<!--/xsl:if-->		
			<xsl:choose>
				<xsl:when test="cabinClassInfo/infoByCabinOnClasses">			
					<xsl:for-each select="cabinClassInfo/infoByCabinOnClasses">				
						<xsl:variable name="posi">
							<xsl:value-of select="position()"/>
						</xsl:variable>
						<BookingClassAvail>
							<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="productClassDetail/serviceClass"/></xsl:attribute>
							<xsl:attribute name="ResBookDesigStatusCode"><xsl:value-of select="productClassDetail/availabilityStatus"/></xsl:attribute>
							<xsl:attribute name="RPH"><xsl:value-of select="$posi"/></xsl:attribute>
						</BookingClassAvail>				
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="infoOnClasses">				
						<xsl:variable name="posi">
							<xsl:value-of select="position()"/>
						</xsl:variable>
						<BookingClassAvail>
							<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="productClassDetail/serviceClass"/></xsl:attribute>
							<xsl:attribute name="ResBookDesigStatusCode"><xsl:value-of select="productClassDetail/availabilityStatus"/></xsl:attribute>
							<xsl:attribute name="RPH"><xsl:value-of select="$posi"/></xsl:attribute>
						</BookingClassAvail>				
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
			<TPA_Extensions>
				<AirportChange>
					<xsl:choose>
						<xsl:when test="basicFlightInfo/departureLocation/cityAirport = preceding-sibling::flightInfo[1]/basicFlightInfo/arrivalLocation/cityAirport or $pot = 0">N</xsl:when>
						<xsl:otherwise>Y</xsl:otherwise>
					</xsl:choose>
				</AirportChange>
				<xsl:if test="basicFlightInfo/flightDetails/departureDate != basicFlightInfo/flightDetails/arrivalDate">
					<xsl:variable name="start">20<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureDate),5,2)" />-<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureDate),3,2)" />-<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/departureDate),1,2)" /></xsl:variable>
					<xsl:variable name="end">20<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalDate),5,2)" />-<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalDate),3,2)" />-<xsl:value-of select="substring(string(basicFlightInfo/flightDetails/arrivalDate),1,2)" /></xsl:variable>
					<xsl:variable name="dc" select="ttVB:FctDateDuration(string($start),string($end))"/>
					<DayChange><xsl:value-of select="$dc"/></DayChange>
				</xsl:if> 
				<xsl:if test="additionalFlightInfo/departureStation/terminal != ''">
					<DepartureTerminal><xsl:value-of select="additionalFlightInfo/departureStation/terminal"/></DepartureTerminal>
				</xsl:if>
				<xsl:if test="additionalFlightInfo/arrivalStation/terminal != ''">
					<ArrivalTerminal><xsl:value-of select="additionalFlightInfo/arrivalStation/terminal"/></ArrivalTerminal>
				</xsl:if> 
				<xsl:if test="additionalFlightInfo/flightDetails/daysOfOperation != ''">
					<FlightFrequency><xsl:value-of select="additionalFlightInfo/flightDetails/daysOfOperation"/></FlightFrequency>
				</xsl:if>
				<xsl:if test="trafficRestrictionList/trafficRestrictionList/trafficRestriction/code">
					<TrafficRestrictions><xsl:value-of select="trafficRestrictionList/trafficRestrictionList/trafficRestriction/code"/></TrafficRestrictions>
				</xsl:if>
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
<!--**********************************************************************************************-->	
<msxsl:script language="VisualBasic" implements-prefix="ttVB">
<![CDATA[
Function FctDateDuration(byval p_startDate as string, byval p_endDate as string) as string
   	
    If (IsDate(p_startDate) And IsDate(p_endDate)) Then
        FctDateDuration = CStr(DateDiff("d", p_startDate, p_endDate)) 
    Else
        FctDateDuration = p_startDate & p_endDate
    End If

End Function
]]>
</msxsl:script>
	
</xsl:stylesheet>
