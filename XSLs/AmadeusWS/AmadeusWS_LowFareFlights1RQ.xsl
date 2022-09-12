<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFareFlights1RQ.xsl 											-->
	<!-- ================================================================== -->
  <!-- Date: 20 Oct 2013 - Dharshan changed the create names template to refernce the adult to infant -->
	<!-- Date: 09 Jan 2013 - Rastko added NS option to IBP 							 -->
	<!-- Date: 12 Dec 2012 - Rastko added A option to IBP and improved flightTypeDetails process	 -->
	<!-- Date: 07 Jul 2012 - Shashin -  Removed L option from IBP    					 -->
	<!-- Date: 09 Apr 2012 - Shashin - added flightTypeDetails recommended by Amadeus -->
	<!-- Date: 29 Mar 2012 - Shashin pricingOptionsGroup change						 -->	
	<!-- Date: 26 Mar 2012 -  Shashin - user 1 pricingOptionsGroup 					-->	
	<!-- Date: 26 Mar 2012 -  Shashin - added VOA option								-->
	<!-- Date: 26 Mar 2012 -  Shashin - added NS option								-->
	<!-- Date: 18 Aug 2011 -  Rastko - select cabin only if no specific class requested		-->
	<!-- Date: 13 Aug 2011 -  Rastko - mapped cabin									-->
	<!-- Date: 10 Mar 2011 -  Rastko - added corporate code mapping					-->
	<!-- Date: 02 jan 2010 -  Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchFlightsRQ"/>
	</xsl:template>
	<xsl:template match="OTA_AirLowFareSearchFlightsRQ">
		<Fare_InformativeBestPricingWithoutPNR>
			<messageDetails>
				<messageFunctionDetails>
					<businessFunction>1</businessFunction>
					<messageFunction>741</messageFunction>
					<responsibleAgency>1A</responsibleAgency>
					<additionalMessageFunction>170</additionalMessageFunction>
				</messageFunctionDetails>
			</messageDetails>
			<xsl:if test="POS/Source/@ISOCurrency!=''">
				<currencyOverride>
					<conversionRateDetails>
						<currency>
							<xsl:value-of select="POS/Source/@ISOCurrency"/>
						</currency>
					</conversionRateDetails>
					<otherConvRateDetails>
						<conversionType>707</conversionType>
						<currency>
							<xsl:value-of select="POS/Source/@ISOCurrency"/>
						</currency>
					</otherConvRateDetails>
				</currencyOverride>
			</xsl:if>
			<corporateFareInfo>
				<xsl:choose>
					<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">
						<corporateFareIdentifiers>
							<fareQualifier>U</fareQualifier>
							<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
								<corporateID>
									<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
								</corporateID>
							</xsl:if>
						</corporateFareIdentifiers>
						<corporateFareIdentifiers>
							<fareQualifier>P</fareQualifier>
						</corporateFareIdentifiers>
					</xsl:when>
					<xsl:otherwise>
						<corporateFareIdentifiers>
							<fareQualifier>
								<xsl:choose>
									<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">UP</xsl:when>
									<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">U</xsl:when>
									<xsl:otherwise>P</xsl:otherwise>
								</xsl:choose>
							</fareQualifier>
							<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
								<corporateID>
									<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code"/>
								</corporateID>
							</xsl:if>
						</corporateFareIdentifiers>
					</xsl:otherwise>
				</xsl:choose>
			</corporateFareInfo>
			<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
				<passengersGroup>
					<segmentRepetitionControl>
						<segmentControlDetails>
							<quantity> 	<xsl:value-of select="position()"/></quantity>
							<numberOfUnits>
								<xsl:value-of select="@Quantity"/>
							</numberOfUnits>
						</segmentControlDetails>
					</segmentRepetitionControl>
					<xsl:variable name="paxid">
					<xsl:choose>
							<xsl:when test="./@Code!='INF'">
								<xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<travellersID>
						<xsl:call-template name="create_names">
							<xsl:with-param name="count">
								<xsl:value-of select="@Quantity"/>
							</xsl:with-param>
							<xsl:with-param name="count1">
								<xsl:value-of select="$paxid + 1"/>
							</xsl:with-param>
							<xsl:with-param name="inf">
								<xsl:value-of select="./@Code"/>
							</xsl:with-param>
						</xsl:call-template>
					</travellersID>
					<ptcGroup>
						<discountPtc>
							<valueQualifier>
								<xsl:choose>
									<xsl:when test="@Code='CHD'">CNN</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</valueQualifier>
							<xsl:if test="@Code='INF'">
								<fareDetails> 
						          	<qualifier>766</qualifier> 
						        	</fareDetails> 
							</xsl:if>
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
			<pricingOptionsGroup>
				<pricingDetails>
					<priceTicketDetails>
						<!--<indicators>L</indicators>-->
						<indicators>NS</indicators>
						<indicators>A</indicators>
						<indicators>E</indicators>
						<indicators>VOA</indicators>
					</priceTicketDetails>
				</pricingDetails>
			</pricingOptionsGroup>
			<tripsGroup>
				<originDestination>
					<origin>
						<xsl:value-of select="OD[1]/flightInfo[1]/basicFlightInfo/departureLocation/cityAirport"/>
					</origin>
					<destination>
						<xsl:value-of select="OD[1]/flightInfo[position()=last()]/basicFlightInfo/arrivalLocation/cityAirport"/>
					</destination>
				</originDestination>
				<xsl:apply-templates select="OD"/>
			</tripsGroup>
		</Fare_InformativeBestPricingWithoutPNR>
	</xsl:template>
	
	<xsl:template match="OD">
		<xsl:variable name="odpos"><xsl:value-of select="position()"/></xsl:variable>		
		<xsl:apply-templates select="flightInfo">
			<xsl:with-param name="pos">1</xsl:with-param>
			<xsl:with-param name="odpos"><xsl:value-of select="$odpos"/></xsl:with-param>
		</xsl:apply-templates>		
	</xsl:template>
	
	<xsl:template match="flightInfo">
		<xsl:param name="pos" />
		<xsl:param name="odpos" />
		<segmentGroup>
			<segmentInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="basicFlightInfo/flightDetails/departureDate"/>
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId>
						<xsl:value-of select="basicFlightInfo/departureLocation/cityAirport"/>
					</trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId>
						<xsl:value-of select="basicFlightInfo/arrivalLocation/cityAirport"/>
					</trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany>
						<xsl:value-of select="basicFlightInfo/marketingCompany/identifier"/>
					</marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber>
						<xsl:value-of select="basicFlightInfo/flightIdentification/number"/>
					</flightNumber>
					<bookingClass>Y</bookingClass>
				</flightIdentification>
				<flightTypeDetails> 
                     		<flightIndicator><xsl:value-of select="$odpos"/></flightIndicator> 
		              </flightTypeDetails> 
             			 <itemNumber><xsl:value-of select="position()"/></itemNumber>
			</segmentInformation>
		</segmentGroup>		
	</xsl:template>
	
	<xsl:template name="create_names">
		<xsl:param name="count"/>
		<xsl:param name="count1"/>
		<xsl:param name="inf"/>
		
		<xsl:if test="$count !=0">
			<travellerDetails>
				<measurementValue>
					<xsl:value-of select="$count1"/>
				</measurementValue>
			</travellerDetails>
			<xsl:call-template name="create_names">
				<xsl:with-param name="count">
					<xsl:value-of select="$count - 1"/>
				</xsl:with-param>
				<xsl:with-param name="count1">
					<xsl:value-of select="$count1 + 1"/>
				</xsl:with-param>
				<xsl:with-param name="inf">
					<xsl:value-of select="$inf"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
