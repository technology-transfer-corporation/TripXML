<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirPriceRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 06 Sep 2012 -  Shashin - fixed the issue in sending negotiated code		-->
	<!-- Date: 06 Sep 2010 -  Rastko - added validating airline mapping				-->
	<!-- Date: 11 Apr 2010 -  Rastko - New files											-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirPriceRQ" />
	</xsl:template>
	<xsl:template match="OTA_AirPriceRQ">
		<AirPrice>
			<AddMultiElements>
				<xsl:apply-templates select="AirItinerary" />
			</AddMultiElements>
			<SellFlights>
				<Air_SellFromRecommendation>
					<messageActionDetails>
						<messageFunctionDetails>
							<messageFunction>183</messageFunction>
							<additionalMessageFunction>M1</additionalMessageFunction>
						</messageFunctionDetails>
					</messageActionDetails>
					<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="MP"/>
				</Air_SellFromRecommendation>
			</SellFlights>
			<FarePlus>
				<xsl:choose>
					<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@FareQualifier='12'">
						<Fare_PricePNRWithBookingClass> 
							<overrideInformation>
								<attributeDetails>
									<attributeType>RLO</attributeType>
								</attributeDetails>
								<xsl:choose>
									<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
										<attributeDetails>
											<attributeType>RU</attributeType>
										</attributeDetails>
										<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode!='JCB'">
											<xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
												<attributeDetails>
													<attributeType>RW</attributeType>
													<attributeDescription><xsl:value-of select="@Code"/></attributeDescription>
												</attributeDetails>
											</xsl:for-each>
										</xsl:if>
									</xsl:when>
									<xsl:otherwise>
										<attributeDetails>
											<attributeType>RP</attributeType>
										</attributeDetails>
									</xsl:otherwise>
								</xsl:choose>
							</overrideInformation>
							<xsl:if test="TravelPreferences/TicketDistribPref/@TicketTime != ''">
								<ticketingDate>
									<date>
										<xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,9,2)"/>
										<xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,6,2)"/>
										<xsl:value-of select="substring(TravelPreferences/TicketDistribPref/@TicketTime,3,2)"/>
									</date>
								</ticketingDate>
							</xsl:if>
							<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode != ''">
								<validatingCarrier>
									<carrierInformation>
										<carrierCode><xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode"/></carrierCode>
									</carrierInformation>
								</validatingCarrier>
							</xsl:if>
							<xsl:if test="POS/Source/@ISOCurrency != ''">
								<currencyOverride>
				   					<firstRateDetail>
				  						<currencyCode><xsl:value-of select="POS/Source/@ISOCurrency"/></currencyCode>
				      				</firstRateDetail>      
								</currencyOverride>
							</xsl:if>
						</Fare_PricePNRWithBookingClass> 
					</xsl:when>
					<xsl:otherwise>
						<Fare_PricePNRWithLowerFares>
							<overrideInformation>
								<attributeDetails>
									<attributeType>RLA</attributeType>
								</attributeDetails> 
								<attributeDetails>
									<attributeType>CAB</attributeType>
								</attributeDetails>
								<xsl:choose>
									<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
										<attributeDetails>
											<attributeType>RU</attributeType>
										</attributeDetails>
										<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode!='JCB'">
											<xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
												<attributeDetails>
													<attributeType>RW</attributeType>
													<attributeDescription><xsl:value-of select="@Code"/></attributeDescription>
												</attributeDetails>
											</xsl:for-each>
										</xsl:if>
									</xsl:when>
									<xsl:otherwise>
										<attributeDetails>
											<attributeType>RP</attributeType>
										</attributeDetails>
									</xsl:otherwise>
								</xsl:choose>
							</overrideInformation>
							<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode != ''">
								<validatingCarrier>
									<carrierInformation>
										<carrierCode><xsl:value-of select="TravelerInfoSummary/PriceRequestInformation/@ValidatingAirlineCode"/></carrierCode>
									</carrierInformation>
								</validatingCarrier>
							</xsl:if>
							<xsl:if test="POS/Source/@ISOCurrency != ''">
								<currencyOverride>
		     						<firstRateDetail>
		     							<currencyCode><xsl:value-of select="POS/Source/@ISOCurrency"/></currencyCode>
		         						</firstRateDetail>      
								</currencyOverride>
							</xsl:if>
						</Fare_PricePNRWithLowerFares>
					</xsl:otherwise>
				</xsl:choose>
			</FarePlus>
			<Ignore>
				<Command_Cryptic>
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<textStringDetails>IG</textStringDetails>
					</longTextString>
				</Command_Cryptic>
			</Ignore>
			<AirTravelerAvail>
				<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity" />
			</AirTravelerAvail>
			<FlightSegments>
				<xsl:copy-of select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"/>
			</FlightSegments>
		</AirPrice>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group - Master Pricer						 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationOption" mode="MP">
		<itineraryDetails>
			<originDestinationDetails>
				<origin><xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode" /></origin>
				<destination><xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode" /></destination>
			</originDestinationDetails>
			<message>
				<messageFunctionDetails>
					<messageFunction>183</messageFunction>
				</messageFunctionDetails>
			</message>
			<xsl:apply-templates select="FlightSegment" mode="MP"/>
		</itineraryDetails>
	</xsl:template>
	
	<xsl:template match="FlightSegment" mode="MP">
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(@DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<xsl:variable name="ArrTime">
			<xsl:value-of select="substring-after(@ArrivalDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="ArrTime2">
			<xsl:value-of select="substring(string($ArrTime),1,5)" />
		</xsl:variable>
		<segmentInformation>
			<travelProductInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="substring(string(@DepartureDateTime),9,2)" />
						<xsl:value-of select="substring(string(@DepartureDateTime),6,2)" />
						<xsl:value-of select="substring(string(@DepartureDateTime),3,2)" />
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId>
						<xsl:value-of select="DepartureAirport/@LocationCode" />
					</trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId>
						<xsl:value-of select="ArrivalAirport/@LocationCode" />
					</trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany>
						<xsl:value-of select="MarketingAirline/@Code" />
					</marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber>
						<xsl:value-of select="@FlightNumber" />
					</flightNumber>
					<bookingClass>
						<xsl:value-of select="@ResBookDesigCode" />
					</bookingClass>
				</flightIdentification>
				<xsl:if test="@FlightContext!=''">
					<flightTypeDetails>
			               <flightIndicator><xsl:value-of select="@FlightContext"/></flightIndicator>
			            </flightTypeDetails>
				</xsl:if>
			</travelProductInformation>
			<relatedproductInformation>
				<quantity><xsl:value-of select="../../../../TravelerInfoSummary/SeatsRequested" /></quantity>
				<statusCode>NN</statusCode>
			</relatedproductInformation>
		</segmentInformation>
	</xsl:template>
	
	<xsl:template match="AirItinerary">
		<PNR_AddMultiElements>
			<pnrActions>
				<optionCode>0</optionCode>
			</pnrActions>
			<xsl:variable name="inf">
				<xsl:choose>
					<xsl:when test="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INF' or @Code = 'ITF']/@Quantity">
						<xsl:value-of select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INF' or @Code = 'ITF']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="chd">
				<xsl:choose>
					<xsl:when test="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'CHD' or @Code = 'INS' or @Code = 'INN' or @Code = 'ITS']/@Quantity">
						<xsl:value-of select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'CHD' or @Code = 'INS' or @Code = 'INN' or @Code = 'ITS']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="adt">
				<xsl:variable name="all">
					<xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity/@Quantity)"/>
				</xsl:variable>
				<xsl:value-of select="$all - $inf - $chd"/>
			</xsl:variable>
			<xsl:variable name="adttypes">
				<xsl:for-each select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code != 'INF' and @Code != 'INS' and @Code != 'CHD' and @Code != 'ITF' and @Code != 'INN' and @Code != 'ITS']">
					<xsl:call-template name="gettypes">
						<xsl:with-param name="count">
							<xsl:choose>
								<xsl:when test="@Quantity != ''"><xsl:value-of select="@Quantity" /></xsl:when>
								<xsl:otherwise>1</xsl:otherwise>
							</xsl:choose>
						</xsl:with-param>
						<xsl:with-param name="count1">1</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="chdtypes">
				<xsl:for-each select="../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INS' or @Code = 'CHD' or @Code = 'INN'  or @Code = 'ITS']">
					<xsl:call-template name="gettypes">
						<xsl:with-param name="count">
							<xsl:choose>
								<xsl:when test="@Quantity != ''"><xsl:value-of select="@Quantity" /></xsl:when>
								<xsl:otherwise>1</xsl:otherwise>
							</xsl:choose>
						</xsl:with-param>
						<xsl:with-param name="count1">1</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</xsl:variable>
			<xsl:call-template name="create_nameadt">
				<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
				<xsl:with-param name="inf"><xsl:value-of select="$inf"/></xsl:with-param>
				<xsl:with-param name="adttypes"><xsl:value-of select="$adttypes"/></xsl:with-param>
				<xsl:with-param name="count">
					<xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code != 'INF' and @Code != 'INS' and @Code != 'CHD' and @Code != 'ITF' and @Code != 'INN' and @Code != 'ITS']/@Quantity)" />
				</xsl:with-param>
				<xsl:with-param name="count1">1</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="create_namechd">
				<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
				<xsl:with-param name="chdtypes"><xsl:value-of select="$chdtypes"/></xsl:with-param>
				<xsl:with-param name="count">
					<xsl:value-of select="sum(../TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code = 'INS' or @Code = 'CHD' or @Code = 'INN'  or @Code = 'ITS']/@Quantity)" />
				</xsl:with-param>
				<xsl:with-param name="count1">1</xsl:with-param>
			</xsl:call-template>
		</PNR_AddMultiElements>
	</xsl:template>
	
	<xsl:template match="PassengerTypeQuantity">
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:value-of select="@Code" />
			</xsl:attribute>
			<xsl:attribute name="Quantity">
				<xsl:value-of select="@Quantity" />
			</xsl:attribute>
		</PassengerTypeQuantity>
	</xsl:template>
	
	<xsl:template match="OriginDestinationOptions">
		<tripsGroup>
			<originDestination>
				<origin><xsl:value-of select="OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode" /></origin>
				<destination><xsl:value-of select="OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode" /></destination>
			</originDestination>
			<xsl:apply-templates select="OriginDestinationOption" />
		</tripsGroup>
	</xsl:template>
	
	<xsl:template match="OriginDestinationOption">
		<xsl:apply-templates select="FlightSegment" />	
	</xsl:template>
	
	<xsl:template match="FlightSegment">
		<segmentGroup>
			<segmentInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="substring(@DepartureDateTime,9,2)" />
						<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
						<xsl:value-of select="substring(@DepartureDateTime,3,2)" />
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId><xsl:value-of select="DepartureAirport/@LocationCode" /></trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId><xsl:value-of select="ArrivalAirport/@LocationCode" /></trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany><xsl:value-of select="MarketingAirline/@Code" /></marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber><xsl:value-of select="@FlightNumber" /></flightNumber>
					<bookingClass><xsl:value-of select="@ResBookDesigCode" /></bookingClass>
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
	
	<xsl:template name="create_nameadt">
		<xsl:param name="adt"/>
		<xsl:param name="inf"/>
		<xsl:param name="adttypes"/>
		<xsl:param name="count" />
		<xsl:param name="count1" />
		<xsl:if test="$count !=0">
			<travellerInfo>
				<elementManagementPassenger>
					<reference>
						<qualifier>PR</qualifier>
						<number><xsl:value-of select="$count1"/></number>
					</reference>
					<segmentName>NM</segmentName>
				</elementManagementPassenger>
				<passengerData>
					<travellerInformation>
						<traveller>
							<surname>SMITH</surname>
							<quantity>
								<xsl:choose>
									<xsl:when test="$count1 > $inf">1</xsl:when>
									<xsl:otherwise>2</xsl:otherwise>
								</xsl:choose>
							</quantity>
						</traveller>
						<passenger>
							<firstName>
								<xsl:choose>
									<xsl:when test="$count1=2">SUZANA</xsl:when>
									<xsl:when test="$count1=3">THOMAS</xsl:when>
									<xsl:when test="$count1=4">ALEX</xsl:when>
									<xsl:otherwise>JOHN</xsl:otherwise>
								</xsl:choose>
							</firstName>
							<type>
								<xsl:choose>
									<xsl:when test="substring($adttypes,($count1 * 3) - 2,3) = 'SRC'">YCD</xsl:when>
									<xsl:when test="substring($adttypes,($count1 * 3) - 2,3) = 'SCR'">YCD</xsl:when>
									<xsl:otherwise><xsl:value-of select="substring($adttypes,($count1 * 3) - 2,3)"/></xsl:otherwise>
								</xsl:choose>
							</type>
							<xsl:if test="$count1 &lt; $inf or $count1 = $inf">
								<infantIndicator>3</infantIndicator>
							</xsl:if>
						</passenger>
					</travellerInformation>
				</passengerData>
				<xsl:if test="$count1 &lt; $inf or $count1 = $inf">
					<passengerData>
						<travellerInformation>
							<traveller>
								<surname>SMITH</surname>
							</traveller>
							<passenger>
								<firstName>PETER</firstName>
								<type>INF</type>
							</passenger>
						</travellerInformation>
					</passengerData>
				</xsl:if>
			</travellerInfo>
			<xsl:call-template name="create_nameadt">
				<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
				<xsl:with-param name="inf"><xsl:value-of select="$inf"/></xsl:with-param>
				<xsl:with-param name="adttypes"><xsl:value-of select="$adttypes"/></xsl:with-param>
				<xsl:with-param name="count">
					<xsl:value-of select="$count - 1" />
				</xsl:with-param>
				<xsl:with-param name="count1">
					<xsl:value-of select="$count1 + 1" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="create_namechd">
		<xsl:param name="adt"/>
		<xsl:param name="chdtypes"/>
		<xsl:param name="count" />
		<xsl:param name="count1" />
		<xsl:variable name="pos"><xsl:value-of select="$count1 + $adt"/></xsl:variable>
		<xsl:if test="$count !=0">
			<travellerInfo>
				<elementManagementPassenger>
					<reference>
						<qualifier>PR</qualifier>
						<number><xsl:value-of select="$pos"/></number>
					</reference>
					<segmentName>NM</segmentName>
				</elementManagementPassenger>
				<passengerData>
					<travellerInformation>
						<traveller>
							<surname>SMITH</surname>
							<quantity>1</quantity>
						</traveller>
						<passenger>
							<firstName>
								<xsl:choose>
									<xsl:when test="$count1=2">ROBERT</xsl:when>
									<xsl:when test="$count1=3">ISABELLA</xsl:when>
									<xsl:when test="$count1=4">TOM</xsl:when>
									<xsl:otherwise>MARIE</xsl:otherwise>
								</xsl:choose>
							</firstName>
							<type>
								<xsl:value-of select="substring($chdtypes,($count1 * 3) - 2,3)"/>
							</type>
						</passenger>
					</travellerInformation>
				</passengerData>
			</travellerInfo>
			<xsl:call-template name="create_namechd">
				<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
				<xsl:with-param name="chdtypes"><xsl:value-of select="$chdtypes"/></xsl:with-param>
				<xsl:with-param name="count">
					<xsl:value-of select="$count - 1" />
				</xsl:with-param>
				<xsl:with-param name="count1">
					<xsl:value-of select="$count1 + 1" />
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="gettypes">
		<xsl:param name="count" />
		<xsl:param name="count1" />
		<xsl:if test="$count !=0">
			<xsl:choose>
				<xsl:when test="@Code='CH'">CHD</xsl:when>
				<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
			</xsl:choose>
			<xsl:call-template name="gettypes">
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
