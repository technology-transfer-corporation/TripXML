<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_LowFareRQ.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 09 Sep 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">		
		<xsl:apply-templates select="OTA_AirLowFareSearchRQ"/>	
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OTA_AirLowFareSearchRQ">
		<PoweredFare_LowFareSearch>
			<MessageAction>
				<Function>
					<ID>183</ID>
				</Function>
			</MessageAction>
			<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code!='INF'][@Quantity!='']">
				<xsl:call-template name="create_type">
					<xsl:with-param name="count"><xsl:value-of select="@Quantity"/></xsl:with-param>
					<xsl:with-param name="count1"><xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity[@Code!='INF']/@Quantity) + 1"/></xsl:with-param>
				</xsl:call-template>
			</xsl:for-each>
			<PricingDetails>
				<Ticketing>
					<!--xsl:choose>
						<xsl:when test="POS/Source/@ISOCurrency!='' and not(TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode)">
							<Indicators><Code>CUC</Code></Indicators>
						</xsl:when>
						<xsl:when test="POS/Source/@ISOCurrency!='' and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
							<Indicators><Code>CUC</Code></Indicators>
						</xsl:when>
					</xsl:choose-->
				<!-- ************************************************************************************************* -->
				<!-- Restrictions						   									             -->
				<!-- ************************************************************************************************* -->
					<xsl:if test="TravelPreferences/FareRestrictPref/AdvResTicketing/@AdvResInd = 'false'">
						<Indicators>
							<Code>NAP</Code>
						</Indicators>
					</xsl:if>
					<xsl:if test="TravelPreferences/FareRestrictPref/StayRestrictions/@StayRestrictionInd = 'false'">
						<Indicators>
							<Code>NR</Code>
						</Indicators>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="TravelPreferences/FareRestrictPref/VoluntaryChanges/@VolChangeInd = 'false'">
							<Indicators>
								<Code>NPE</Code>
							</Indicators>
						</xsl:when>
						<xsl:when test="TravelPreferences/FareRestrictPref/VoluntaryChanges/Penalty/@PenaltyType = 'Ref'">
							<Indicators>
								<Code>RF</Code>
							</Indicators>
						</xsl:when>
					</xsl:choose>
					<!-- ************************************************************************************************* -->
					<!-- Private (Nego) or Published (Normal) 									    -->
					<!-- ************************************************************************************************* -->
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Both'">
							<Indicators>
								<Code>RU</Code>
								<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
									<Code>RW</Code>
								</xsl:if>
								<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@NegotiatedFaresOnly='true'">
									<Code>PTC</Code>
								</xsl:if>
								<Code>RP</Code>
								<xsl:choose>
									<xsl:when test="POS/Source/@ISOCurrency!='' and not(TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode)">
										<Code>CUC</Code>
									</xsl:when>
									<xsl:when test="POS/Source/@ISOCurrency!='' and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
										<Code>CUC</Code>
									</xsl:when>
								</xsl:choose>
							</Indicators>
						</xsl:when>
						<xsl:when test="TravelerInfoSummary/PriceRequestInformation/@PricingSource='Private'">
							<Indicators>
								<Code>RU</Code>
								<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
									<Code>RW</Code>
								</xsl:if>
								<xsl:if test="TravelerInfoSummary/PriceRequestInformation/@NegotiatedFaresOnly='true'">
									<Code>PTC</Code>
								</xsl:if>
								<xsl:choose>
									<xsl:when test="POS/Source/@ISOCurrency!='' and not(TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode)">
										<Code>CUC</Code>
									</xsl:when>
									<xsl:when test="POS/Source/@ISOCurrency!='' and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
										<Code>CUC</Code>
									</xsl:when>
								</xsl:choose>
							</Indicators>
						</xsl:when>
						<xsl:otherwise>
							<Indicators>
								<Code>
									<xsl:choose>
										<xsl:when test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">RW</xsl:when>
										<xsl:otherwise>RP</xsl:otherwise>
									</xsl:choose>
								</Code>
								<xsl:choose>
									<xsl:when test="POS/Source/@ISOCurrency!='' and not(TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode)">
										<Code>CUC</Code>
									</xsl:when>
									<xsl:when test="POS/Source/@ISOCurrency!='' and TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode/@Code!=''">
										<Code>CUC</Code>
									</xsl:when>
								</xsl:choose>
							</Indicators>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:if test="POS/Source/@ISOCountry != '' and not(TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode)">
						<SalesLocation>
							<SalesCity>
								<xsl:value-of select="POS/Source/@AirportCode"/>
							</SalesCity>
							<SalesCountry>
								<xsl:value-of select="POS/Source/@ISOCountry"/>
							</SalesCountry>
						</SalesLocation>	
					</xsl:if>			
				</Ticketing>
				<xsl:if test="POS/Source/@ISOCurrency!=''">
					<CurrencyOverrideForFare>
						<Currency2>
							<Code><xsl:value-of select="POS/Source/@ISOCurrency"/></Code>
						</Currency2>
					</CurrencyOverrideForFare>
				</xsl:if>
				<xsl:if test="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
					<corporateSpecifications>
						<corporateId>
							<priceTypeQualifier>RW</priceTypeQualifier>
							<xsl:for-each select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode">
								<identityNumber><xsl:value-of select="@Code"/></identityNumber>
							</xsl:for-each>
						</corporateId>
					</corporateSpecifications>
				</xsl:if>
			</PricingDetails>
			<xsl:if test="TravelPreferences/CabinPref/@Cabin != ''">
				<ItineraryCabinOption>
					<CabinCode>
						<xsl:choose>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='Business'">C</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='First'">F</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='Economy'">Y</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='Premium'">W</xsl:when>
							<xsl:when test="TravelPreferences/CabinPref/@Cabin='Main'">M</xsl:when>
						</xsl:choose>
					</CabinCode>
				</ItineraryCabinOption>		
			</xsl:if>
			<ItineraryValues>
				<Unit>
					<Value><xsl:value-of select="sum(TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code!='INF']/@Quantity)"/></Value>
					<ValueQualifier>PX</ValueQualifier>
				</Unit>
			</ItineraryValues>
			<xsl:apply-templates select="OriginDestinationInformation"/>
		</PoweredFare_LowFareSearch>
	</xsl:template>	
	<!---********************************************************************-->
	<!--   Pax type 									  	           -->
	<!---********************************************************************-->
	<xsl:template name="create_type">
		<xsl:param name="count"/>
		<xsl:param name="count1"/>
		<xsl:if test="$count !=0">
			<xsl:variable name="infrefcount"><xsl:value-of select="../PassengerTypeQuantity[@Code='INF']/@Quantity"/></xsl:variable>
	    		<xsl:variable name="precount"><xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity)"/></xsl:variable>
			<xsl:variable name="infpos">
				<xsl:apply-templates select="../PassengerTypeQuantity" mode="infnum"/>
			</xsl:variable>
			<xsl:variable name="chdpos">
				<xsl:apply-templates select="../PassengerTypeQuantity" mode="chdnum"/>
			</xsl:variable>
			<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
			<xsl:variable name="chdcount">
				<xsl:choose>
					<xsl:when test="$pos > $chdpos">
						<xsl:value-of select="../PassengerTypeQuantity[@Code='CHD']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="infcount">
				<xsl:choose>
					<xsl:when test="$pos > $infpos">
						<xsl:value-of select="../PassengerTypeQuantity[@Code='INF']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
	    		<xsl:variable name="total"><xsl:value-of select="$count + $precount - $infcount - $chdcount"/></xsl:variable>
			<Passenger>
				<Traveller>
					<PassengerIndicators>
						<ReferenceNumber><xsl:value-of select="$count1"/></ReferenceNumber>
						<xsl:if test="../PassengerTypeQuantity[@Code='INF'] and @Code!='CHD' and $total &lt;= $infrefcount">
								<InfantIndicator>1</InfantIndicator>
						</xsl:if>
					</PassengerIndicators>
				</Traveller>
				<xsl:if test="../PassengerTypeQuantity[@Code='INF'] and @Code!='CHD' and $total &lt;= $infrefcount">
					<Fare>
						<PassengerType>
							<Code>INF</Code>
						</PassengerType>
					</Fare>
				</xsl:if>
				<Fare>
					<PassengerType>
						<Code>
							<xsl:choose>
								<xsl:when test="@Code='CHD'">CNN</xsl:when>
								<xsl:when test="@Code='SRC'">YCD</xsl:when>
								<xsl:when test="@Code='SCR'">YCD</xsl:when>
								<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
							</xsl:choose>
						</Code>
					</PassengerType>
				</Fare>
			</Passenger>
			<xsl:call-template name="create_type">
				<xsl:with-param name="count"><xsl:value-of select="$count - 1"/></xsl:with-param>
				<xsl:with-param name="count1"><xsl:value-of select="$count1 + 1"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match = "PassengerTypeQuantity" mode="chdnum">	
		<xsl:if test="@Code='CHD'">	
			<xsl:value-of select="position()"/>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match = "PassengerTypeQuantity" mode="infnum">	
		<xsl:if test="@Code='INF'">	
			<xsl:value-of select="position()"/>
		</xsl:if>
	</xsl:template>
	<!---********************************************************************-->
	<!--   Itinerary information begins here                                 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationInformation">
		<RequestedSegment>
			<OriginAndDestination>
				<DeparturePoint>
					<xsl:value-of select="OriginLocation/@LocationCode"/>
				</DeparturePoint>
				<ArrivalPoint>
					<xsl:value-of select="DestinationLocation/@LocationCode"/>
				</ArrivalPoint>
			</OriginAndDestination>
			<xsl:apply-templates select="../TravelPreferences/FlightTypePref"/>
			<!---********************************************************************-->
			<!--   If PreferredCarriers are passed then move them into the proper    -->
			<!--   fields and add indicator; do the same with excluded carriers      -->
			<!---********************************************************************-->
			<xsl:choose>
				<xsl:when test="../TravelPreferences/VendorPref/@Code!=''">
					<AirlineOption>
						<AirlineList>
							<CodeList>
								<Code1>
									<xsl:value-of select="../TravelPreferences/VendorPref[1]/@Code"/>
								</Code1>
								<xsl:if test="../TravelPreferences/VendorPref[position() = 2]/@Code != ''">
									<Code2>
										<xsl:value-of select="../TravelPreferences/VendorPref[position() = 2]/@Code"/>
									</Code2>
								</xsl:if>
								<xsl:if test="../TravelPreferences/VendorPref[position() = 3]/@Code != ''">
									<Code3>
										<xsl:value-of select="../TravelPreferences/VendorPref[position() = 3]/@Code"/>
									</Code3>
								</xsl:if>
							</CodeList>
							<OptionQualifier>
								<MandatoryOrExclude>
									<xsl:choose>
										<xsl:when test="../TravelPreferences/VendorPref[1]/@PreferLevel = 'Unacceptable'">X</xsl:when>
										<xsl:otherwise>M</xsl:otherwise>
									</xsl:choose>
								</MandatoryOrExclude>
							</OptionQualifier>
						</AirlineList>
					</AirlineOption>
				</xsl:when>
			</xsl:choose>
			<!---********************************************************************-->
			<!--   If 2 IncldConnections are passed then move them into the proper   -->
			<!--   fields don't add any Excluded Connections                         -->
			<!--   Maximum 2 fields total, ie. 2 include, 1 include & 1 exclude or   -->
			<!--   2 exclude                                                         -->
			<!---********************************************************************-->
			<xsl:if test="ConnectionLocations/ConnectionLocation">
				<ConnectionOption>
					<xsl:apply-templates select="ConnectionLocations/ConnectionLocation"/>
				</ConnectionOption>
			</xsl:if>
			<!---********************************************************************-->
			<DateOption>
				<DepartureDateAndTime>
					<DateAndTimeDetails>
						<xsl:choose>
							<xsl:when test="Preferences/@Sort='DEPARTURE'">
								<Qualifier>AD</Qualifier>
							</xsl:when>
							<xsl:when test="Preferences/@Sort='ARRIVAL'">
								<Qualifier>AA</Qualifier>
							</xsl:when>
							<xsl:when test="DepartureDateTime/@WindowBefore or DepartureDateTime/@WindowAfter">
								<Qualifier>AD</Qualifier>
							</xsl:when>
						</xsl:choose>
						<Date>
							<xsl:value-of select="substring(DepartureDateTime,9,2)"/>
							<xsl:value-of select="substring(DepartureDateTime,6,2)"/>
							<xsl:value-of select="substring(DepartureDateTime,3,2)"/>					
						</Date>
						<Time>
							<xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')"/>
						</Time>
					</DateAndTimeDetails>
				</DepartureDateAndTime>
				<xsl:if test="DepartureDateTime/@WindowBefore or DepartureDateTime/@WindowAfter">
					<TimeWindowSize>
						<Unit>
							<Value>
								<xsl:choose>
									<xsl:when test="DepartureDateTime/@WindowBefore != ''">
										<xsl:value-of select="DepartureDateTime/@WindowBefore"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="DepartureDateTime/@WindowAfter"/>
									</xsl:otherwise>
								</xsl:choose>
							</Value>
							<ValueQualifier>TH</ValueQualifier>
						</Unit>
					</TimeWindowSize>
				</xsl:if>
			</DateOption>
		</RequestedSegment>
	</xsl:template>
	<xsl:template match="FlightTypePref">
		<CategoryOption>
			<Category>
				<xsl:choose>
					<xsl:when test="@FlightType='Nonstop'">
						<Code>N</Code>
					</xsl:when>
					<xsl:when test="@FlightType='Direct'">
						<Code>D</Code>
					</xsl:when>
					<xsl:when test="@MaxConnections !=' '">
						<Code>C</Code>
					</xsl:when>
				</xsl:choose>
			</Category>
		</CategoryOption>
	</xsl:template>
	<!---********************************************************************-->
	<xsl:template match="ConnectionLocation">
		<!--xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="Preference"><xsl:value-of select="@PreferLevel"/></xsl:variable-->
		<ConnectionList>
			<Unit>
				<Point>
					<xsl:value-of select="@LocationCode"/>
				</Point>
				<Qualifier>
					<xsl:choose>
						<xsl:when test="@PreferLevel = 'Unacceptable'">X</xsl:when>
						<xsl:otherwise>M</xsl:otherwise>
					</xsl:choose>
				</Qualifier>
			</Unit>
		</ConnectionList>
	</xsl:template>
<!---********************************************************************-->
</xsl:stylesheet>
