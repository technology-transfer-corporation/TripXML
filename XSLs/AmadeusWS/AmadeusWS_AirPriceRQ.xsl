<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirPriceRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 10 Nov 2012 - Rastko - added flight type details							 -->
	<!-- Date: 03 Sep 2012 - Rastko - added cabin options to IBP						 -->
	<!-- Date: 27 Jun 2012 - Shashin - Fixed sending both option for IBP				 -->
	<!-- Date: 29 Mar 2012 - Shashin -POS AirportCode change						 -->
	<!-- Date: 24 May 2011 -  Rastko - added mapping of validating carrier				-->
	<!-- Date: 18 May 2011 -  Rastko - added additonal ticket indicators				-->
	<!-- Date: 10 Mar 2011 -  Rastko - added corporate code mapping					-->
	<!-- Date: 02 Mar 2011 -  Rastko - added Fare_InformativeBestPricingWithoutPNR mapping	-->
	<!-- Date: 13 Aug 2010 -  Rastko - added currency mapping						-->
	<!-- Date: 14 Jun 2009 -  Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirPriceRQ"/>
	</xsl:template>
	<xsl:template match="OTA_AirPriceRQ">
		<AirPrice>
			<xsl:choose>
				<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@FareQualifier='12'">
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
						<corporateFares>
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
						</corporateFares>
						<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
							<passengersGroup>
								<segmentRepetitionControl>
									<segmentControlDetails>
										<quantity>1</quantity>
										<numberOfUnits>
											<xsl:value-of select="@Quantity"/>
										</numberOfUnits>
									</segmentControlDetails>
								</segmentRepetitionControl>
								<xsl:variable name="paxid">
									<xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/>
								</xsl:variable>
								<travellersID>
									<xsl:call-template name="create_names">
										<xsl:with-param name="count">
											<xsl:value-of select="@Quantity"/>
										</xsl:with-param>
										<xsl:with-param name="count1">
											<xsl:value-of select="$paxid + 1"/>
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
						<pricingOptionsGroup>
							<pricingDetails>
								<priceTicketDetails>
									<indicators>L</indicators>
									<indicators>E</indicators>
									<indicators>VOA</indicators>
								</priceTicketDetails>
								<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode">
									<companyDetails>
										<marketingCompany>
											<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode"/>
										</marketingCompany>
									</companyDetails>
								</xsl:if>
								<xsl:if test="POS/Source/@AirportCode != ''">
									<locationDetails>
										<city>
											<xsl:value-of select="POS/Source/@AirportCode "/>
										</city>
									</locationDetails>
									<otherLocationDetails>
										<city>
											<xsl:value-of select="POS/Source/@AirportCode "/>
										</city>
									</otherLocationDetails>
								</xsl:if>
							</pricingDetails>
						</pricingOptionsGroup>
						<xsl:apply-templates select="AirItinerary/OriginDestinationOptions"/>
					</Fare_InformativePricingWithoutPNR>
				</xsl:when>
				<xsl:otherwise>
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
										<quantity>1</quantity>
										<numberOfUnits>
											<xsl:value-of select="@Quantity"/>
										</numberOfUnits>
									</segmentControlDetails>
								</segmentRepetitionControl>
								<xsl:variable name="paxid">
									<xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/>
								</xsl:variable>
								<travellersID>
									<xsl:call-template name="create_names">
										<xsl:with-param name="count">
											<xsl:value-of select="@Quantity"/>
										</xsl:with-param>
										<xsl:with-param name="count1">
											<xsl:value-of select="$paxid + 1"/>
										</xsl:with-param>
									</xsl:call-template>
								</travellersID>
								<ptcGroup>
									<discountPtc>
										<valueQualifier>
											<xsl:value-of select="@Code"/>
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
						<cabinPreferenceOption>
							<preferenceLevel>
								<productDetailsQualifier>FC</productDetailsQualifier>
							</preferenceLevel>
							<cabinPreference>
								<genericDetails>
									<xsl:choose>
										<xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode='F'">
											<cabinClass><xsl:value-of select="'1'"/></cabinClass>
											<compartmentDesignator><xsl:value-of select="'F'"/></compartmentDesignator>
										</xsl:when>
										<xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode='C'">
											<cabinClass><xsl:value-of select="'2'"/></cabinClass>
											<compartmentDesignator><xsl:value-of select="'C'"/></compartmentDesignator>
										</xsl:when>
										<xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode='W'">
											<cabinClass><xsl:value-of select="'4'"/></cabinClass>
											<compartmentDesignator><xsl:value-of select="'W'"/></compartmentDesignator>
										</xsl:when>
										<xsl:otherwise>
											<cabinClass><xsl:value-of select="'3'"/></cabinClass>
											<compartmentDesignator><xsl:value-of select="'Y'"/></compartmentDesignator>
										</xsl:otherwise>
									</xsl:choose>
								</genericDetails>
							</cabinPreference>
						</cabinPreferenceOption>
						<pricingOptionsGroup>
							<pricingDetails>
								<priceTicketDetails>
									<!--<indicators>L</indicators>-->
									<indicators>A</indicators>
									<indicators>E</indicators>
									<!--indicators>NS</indicators-->
									<indicators>VOA</indicators>
								</priceTicketDetails>
								<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode">
									<companyDetails>
										<marketingCompany>
											<xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode"/>
										</marketingCompany>
									</companyDetails>
								</xsl:if>
								<xsl:if test="POS/Source/@AirportCode != ''">
									<locationDetails>
										<city>
											<xsl:value-of select="POS/Source/@AirportCode "/>
										</city>
									</locationDetails>
									<otherLocationDetails>
										<city>
											<xsl:value-of select="POS/Source/@AirportCode "/>
										</city>
									</otherLocationDetails>
								</xsl:if>
							</pricingDetails>
						</pricingOptionsGroup>
						<xsl:apply-templates select="AirItinerary/OriginDestinationOptions"/>
						<!--tripsGroup>
							<originDestination>
								<origin><xsl:value-of select="OD[1]/flightInfo[1]/basicFlightInfo/departureLocation/cityAirport" /></origin>
								<destination><xsl:value-of select="OD[1]/flightInfo[position()=last()]/basicFlightInfo/arrivalLocation/cityAirport" /></destination>
							</originDestination>
							<xsl:apply-templates select="AirItinerary/OriginDestinationOptions" />
						</tripsGroup-->
					</Fare_InformativeBestPricingWithoutPNR>
				</xsl:otherwise>
			</xsl:choose>
			<AirTravelerAvail>
				<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity"/>
			</AirTravelerAvail>
			<FlightSegments>
				<xsl:copy-of select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"/>
			</FlightSegments>
		</AirPrice>
	</xsl:template>
	<xsl:template match="PassengerTypeQuantity">
		<PassengerTypeQuantity>
			<xsl:attribute name="Code"><xsl:value-of select="@Code"/></xsl:attribute>
			<xsl:attribute name="Quantity"><xsl:value-of select="@Quantity"/></xsl:attribute>
		</PassengerTypeQuantity>
	</xsl:template>
	<xsl:template match="OriginDestinationOptions">
		<tripsGroup>
			<originDestination>
				<origin>
					<xsl:value-of select="OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode"/>
				</origin>
				<destination>
					<xsl:value-of select="OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
				</destination>
			</originDestination>
			<xsl:apply-templates select="OriginDestinationOption"/>
		</tripsGroup>
	</xsl:template>
	<xsl:template match="OriginDestinationOption">
		<xsl:variable name="ODpos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:apply-templates select="FlightSegment">
			<xsl:with-param name="ODpos"><xsl:value-of select="$ODpos"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="FlightSegment">
		<xsl:param name="ODpos"/>
		<segmentGroup>
			<segmentInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="substring(@DepartureDateTime,9,2)"/>
						<xsl:value-of select="substring(@DepartureDateTime,6,2)"/>
						<xsl:value-of select="substring(@DepartureDateTime,3,2)"/>
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId>
						<xsl:value-of select="DepartureAirport/@LocationCode"/>
					</trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId>
						<xsl:value-of select="ArrivalAirport/@LocationCode"/>
					</trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany>
						<xsl:value-of select="MarketingAirline/@Code"/>
					</marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber>
						<xsl:value-of select="@FlightNumber"/>
					</flightNumber>
					<bookingClass>
						<xsl:value-of select="@ResBookDesigCode"/>
					</bookingClass>
				</flightIdentification>
				<flightTypeDetails> 
		       	   <flightIndicator>
		       	   		<xsl:value-of select="$ODpos"/>
		       	   </flightIndicator> 
		        	</flightTypeDetails> 
		        	<itemNumber>
		        		<xsl:value-of select="position()"/>
		        	</itemNumber> 
			</segmentInformation>
		</segmentGroup>
	</xsl:template>
	<xsl:template name="create_names">
		<xsl:param name="count"/>
		<xsl:param name="count1"/>
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
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
