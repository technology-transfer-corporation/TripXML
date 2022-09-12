<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFareFlights2RQ.xsl 											-->
	<!-- ================================================================== -->
	<!-- Date: 18 Aug 2011 -  Rastko - select cabin only if no specific class requested		-->
	<!-- Date: 13 Aug 2011 -  Rastko - mapped cabin									-->
	<!-- Date: 12 Mar 2011 -  Rastko - added corporate code mapping					-->
	<!-- Date: 11 March 2011 -  Sajith - new file											-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchFlightsRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirLowFareSearchFlightsRQ">
		<Fare_InformativePricingWithoutPNR>
				<messageDetails>
					<messageFunctionDetails>
						<businessFunction>1</businessFunction>
						<messageFunction>741</messageFunction>
						<responsibleAgency>1A</responsibleAgency>
					</messageFunctionDetails>
				</messageDetails>
				<xsl:if test="POS/Source/@ISOCurrency!=''">
					<currencyOverride>
						<conversionRateDetails>
							<currency><xsl:value-of select="POS/Source/@ISOCurrency"/></currency>
						</conversionRateDetails>
						<otherConvRateDetails>
							<conversionType>707</conversionType>
							<currency><xsl:value-of select="POS/Source/@ISOCurrency"/></currency>
						</otherConvRateDetails>
					</currencyOverride>
				</xsl:if>
				<corporateFares>
					<corporateFareIdentifiers>
						<fareQualifier>
							<xsl:choose>
								<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">U</xsl:when>
								<xsl:otherwise>P</xsl:otherwise>
							</xsl:choose>
						</fareQualifier>
						<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
							<corporateID><xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/></corporateID>
						</xsl:if>
					</corporateFareIdentifiers>
				</corporateFares>
				<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
					<passengersGroup>
						<segmentRepetitionControl>
							<segmentControlDetails>
								<quantity>1</quantity>
								<numberOfUnits><xsl:value-of select="@Quantity"/></numberOfUnits>
							</segmentControlDetails>
						</segmentRepetitionControl>
						<xsl:variable name="paxid">
							<xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/>
						</xsl:variable>
						<travellersID>
							<xsl:call-template name="create_names">
								<xsl:with-param name="count">
									<xsl:value-of select="@Quantity" />
								</xsl:with-param>
								<xsl:with-param name="count1"><xsl:value-of select="$paxid + 1"/></xsl:with-param>
							</xsl:call-template>
						</travellersID>
						<ptcGroup>
							<discountPtc>
								<valueQualifier>
									<xsl:choose>
										<xsl:when test="@Code='CHD'">CNN</xsl:when>
										<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
									</xsl:choose>
								</valueQualifier>
							</discountPtc>
						</ptcGroup>				
					</passengersGroup>
				</xsl:for-each>
				<xsl:if test="not(SpecificFlightInfo/BookingClassPref/@ResBookDesigCode)">
					<xsl:if test="TravelPreferences/CabinPref/@Cabin!=''">
						<cabinPreferenceOption>    
							<preferenceLevel>      
								<productDetailsQualifier>FC</productDetailsQualifier>    
							</preferenceLevel>    
							<cabinPreference>      
								<genericDetails>        
									<cabinClass>
										<xsl:choose>
											<xsl:when test="TravelPreferences/CabinPref/@Cabin='Premium'">4</xsl:when>
											<xsl:when test="TravelPreferences/CabinPref/@Cabin='Economy'">5</xsl:when>
											<xsl:when test="TravelPreferences/CabinPref/@Cabin='Business'">2</xsl:when>
											<xsl:when test="TravelPreferences/CabinPref/@Cabin='First'">1</xsl:when>
										</xsl:choose>
									</cabinClass>        
								</genericDetails>    
							</cabinPreference>  
						</cabinPreferenceOption>  
					</xsl:if>
				</xsl:if>
				<tripsGroup>
					<originDestination>
						<origin><xsl:value-of select="OD[1]/flightInfo[1]/basicFlightInfo/departureLocation/cityAirport" /></origin>
						<destination><xsl:value-of select="OD[1]/flightInfo[position()=last()]/basicFlightInfo/arrivalLocation/cityAirport" /></destination>
					</originDestination>
					<xsl:apply-templates select="OD/flightInfo" />
				</tripsGroup>
				<xsl:apply-templates select="AirItinerary/OriginDestinationOptions" />
			</Fare_InformativePricingWithoutPNR>
	</xsl:template>		
	<xsl:template match="flightInfo">
		<segmentGroup>
			<segmentInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="basicFlightInfo/flightDetails/departureDate" />
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId><xsl:value-of select="basicFlightInfo/departureLocation/cityAirport" /></trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId><xsl:value-of select="basicFlightInfo/arrivalLocation/cityAirport" /></trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany><xsl:value-of select="basicFlightInfo/marketingCompany/identifier" /></marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber><xsl:value-of select="basicFlightInfo/flightIdentification/number" /></flightNumber>
					<bookingClass><xsl:value-of select="infoOnClasses/productClassDetail/serviceClass" /></bookingClass>				
				</flightIdentification>
			</segmentInformation>
		</segmentGroup>
	</xsl:template>
	<xsl:template name="create_names">
		<xsl:param name="count" />
		<xsl:param name="count1" />
		<xsl:if test="$count !=0">
			<travellerDetails>
				<measurementValue><xsl:value-of select="$count1"/></measurementValue>
			</travellerDetails>
			<xsl:call-template name="create_names">
				<xsl:with-param name="count">
					<xsl:value-of select="$count - 1" />
				</xsl:with-param>
				<xsl:with-param name="count1">
					<xsl:value-of select="$count1 + 1" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
