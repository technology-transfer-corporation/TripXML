<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_LowFareRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 20 Mar 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="blist" select="document('FlightBlackList.xml')"/>
	<xsl:template match="/">
		<xsl:if test="MessagesOnly_Reply">
			<OTA_AirLowFareSearchRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
				<Errors>
					<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages" mode="Error"/>
				</Errors>
			</OTA_AirLowFareSearchRS>
		</xsl:if>
		<xsl:apply-templates select="PoweredFare_LowFareSearchReply"/>
	</xsl:template>
	<xsl:template match="CAPI_Messages" mode="Error">
		<Error Type="Amadeus">
			<xsl:attribute name="Code"><xsl:value-of select="ErrorCode"/></xsl:attribute>	
			<xsl:value-of select="Text"/>		
		</Error>
	</xsl:template>
	<xsl:template match="PoweredFare_LowFareSearchReply">
		<OTA_AirLowFareSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
			<xsl:variable name="recommendation">
				<xsl:apply-templates select="Recommendation"/>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="$recommendation!=''">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="Recommendation"/>
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error Type="TripXML">
							<xsl:attribute name="Code">0100</xsl:attribute>
							<xsl:text>ALL FLIGHTS ARE ON BLACK LIST</xsl:text>
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="Recommendation">
		<xsl:variable name="fm">
			<xsl:for-each select="RecommendedSegment/FlightSegment">
				<xsl:variable name="airline"><xsl:value-of select="Description/Airline/Code"/></xsl:variable>
				<xsl:variable name="flight"><xsl:value-of select="Description/Identification/FlightNumber"/></xsl:variable>
				<xsl:variable name="class"><xsl:value-of select="Description/Identification/ClassOfService"/></xsl:variable>
				<xsl:variable name="ddate">
					<xsl:text>20</xsl:text>
					<xsl:value-of select="substring(Description/Schedule/DepartureDate,5,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(Description/Schedule/DepartureDate,3,2)"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring(Description/Schedule/DepartureDate,1,2)"/>
				</xsl:variable>
				<xsl:for-each select="$blist/BlackList/Flight">
					<xsl:if test="@AC=$airline and @FN=$flight and @CS=$class and @DD=$ddate">1</xsl:if>
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$fm=1"></xsl:when>
			<xsl:otherwise>
				<PricedItinerary>
					<xsl:variable name="rec"><xsl:value-of select="position()"/></xsl:variable>
					<xsl:attribute name="SequenceNumber"><xsl:value-of select="$rec"/></xsl:attribute>
					<AirItinerary>
						<xsl:attribute name="DirectionInd">
							<xsl:choose>
								<xsl:when test="RecommendedSegment[1]/OriginAndDestination/DeparturePoint = RecommendedSegment[position()=last()]/OriginAndDestination/ArrivalPoint">Circle</xsl:when>
								<xsl:otherwise>OneWay</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<OriginDestinationOptions>
							<xsl:apply-templates select="RecommendedSegment"/>
						</OriginDestinationOptions>
					</AirItinerary>
					<AirItineraryPricingInfo>
						<xsl:choose>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RN']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RV']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RW']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RX']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RZ']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RA']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RB']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RC']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:when test="Product/PricingDetails[Ticketing/Code='RD']">
								<xsl:attribute name="PricingSource">Private</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="PricingSource">Published</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>									
						<xsl:variable name="Deci" select="substring-after(string(TotalPrice/Money[1]/Amount),'.')"/>
						<xsl:variable name="NoDeci" select="string-length($Deci)"/>
						<xsl:variable name="BaseNoDeci" select="translate(string(Product/Summary/MonetaryAmount),'.,','')"/>
						<xsl:variable name="TotalAmount" select="translate(string(TotalPrice/Money[1]/Amount),'.,','')"/>		
						<xsl:variable name="TaxTotal">
							<xsl:choose>
								<xsl:when test="TotalPrice/Money[2]/Amount">
									<xsl:value-of select="translate(string(TotalPrice/Money[2]/Amount),'.','')"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<ItinTotalFare>
							<BaseFare>
								<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount - $TaxTotal"/></xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="../CurrencyDetails/Currency/Code"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
							</BaseFare>		
							<Taxes>
								<Tax>
									<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
									<xsl:attribute name="Amount"><xsl:value-of select="$TaxTotal"/></xsl:attribute>
									<xsl:attribute name="CurrencyCode"><xsl:value-of select="../CurrencyDetails/Currency/Code"/></xsl:attribute>
									<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
								</Tax>
							</Taxes>
							<TotalFare>
								<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount"/></xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="../CurrencyDetails/Currency/Code"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
							</TotalFare>
						</ItinTotalFare>
						<PTC_FareBreakdowns>	
							<xsl:apply-templates select="../AirTravelerAvail/PassengerTypeQuantity[@Quantity != '0'][@Code!='INF']" mode="ptc">
								<xsl:with-param name="rec"><xsl:value-of select="$rec"/></xsl:with-param>
							</xsl:apply-templates>
							<xsl:apply-templates select="../AirTravelerAvail/PassengerTypeQuantity[@Quantity != '0'][@Code='INF']" mode="ptcinf">
								<xsl:with-param name="rec"><xsl:value-of select="$rec"/></xsl:with-param>
							</xsl:apply-templates>
						</PTC_FareBreakdowns>	
						<FareInfos>
							<xsl:apply-templates select="../AirTravelerAvail/PassengerTypeQuantity[@Code!='INF']" mode="FareRule">
								<xsl:with-param name="rec"><xsl:value-of select="$rec"/></xsl:with-param>
							</xsl:apply-templates>
							<xsl:apply-templates select="../AirTravelerAvail/PassengerTypeQuantity[@Code='INF']" mode="FareRuleInf">
								<xsl:with-param name="rec"><xsl:value-of select="$rec"/></xsl:with-param>
							</xsl:apply-templates>
						</FareInfos>	
					</AirItineraryPricingInfo>
					<xsl:if test="WarningMessage/Text != ''">
						<Notes>
							<xsl:value-of select="WarningMessage/Text"/>
						</Notes>
					</xsl:if>
					<xsl:if test="RecommendedSegment/FlightSegment/Description/Indicator/CodeShareAndElectronicTicket != '' or Product/Warning/AppendedMessage/Qualification/Type = 'LTD'">
						<TicketingInfo>
							<xsl:if test="Product/Warning/AppendedMessage/Qualification/Type = 'LTD'">
								<xsl:attribute name="TicketTimeLimit">
									<xsl:text>20</xsl:text>
									<xsl:value-of select="substring(Product/Warning/AppendedMessage[Qualification/Type = 'LTD']/Text[2],6,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring(Product/Warning/AppendedMessage[Qualification/Type = 'LTD']/Text[2],3,3)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(Product/Warning/AppendedMessage[Qualification/Type = 'LTD']/Text[2],1,2)"/>
									<xsl:text>T23:59:00</xsl:text>
								</xsl:attribute>
							</xsl:if>
						</TicketingInfo>
					</xsl:if>
				</PricedItinerary>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="PassengerTypeQuantity" mode="ptc">
		<xsl:param name="rec"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="nip"><xsl:value-of select="@Quantity"/></xsl:variable>
		<xsl:variable name="code"><xsl:value-of select="@Code"/></xsl:variable>
		<xsl:apply-templates select="../../Recommendation[position() = $rec]/Product[position() = $pos]" mode="ptc">
			<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
			<xsl:with-param name="code"><xsl:value-of select="$code"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="PassengerTypeQuantity" mode="ptcinf">
		<xsl:param name="rec"/>
		<xsl:variable name="nip"><xsl:value-of select="@Quantity"/></xsl:variable>
		<xsl:variable name="code"><xsl:value-of select="@Code"/></xsl:variable>
		<xsl:apply-templates select="../../Recommendation[position() = $rec]/Product[position()=last()]" mode="ptc">
			<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
			<xsl:with-param name="code"><xsl:value-of select="$code"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>

	
	<!--****************************************************************************************-->
	<!-- 											 							   -->
	<!--****************************************************************************************-->
	<xsl:template match="Product" mode="ptc">		
		<xsl:param name="nip"/>
		<xsl:param name="code"/>
		<PTC_FareBreakdown>	
			<xsl:variable name="TaxAmountPTC"><xsl:value-of select="translate(string(TaxInformation/Details[1]/Amount),'.','')"/></xsl:variable>
			<xsl:variable name="TotalAmountPTC"><xsl:value-of select="translate(string(Summary/MonetaryAmount),'.','')"/></xsl:variable>
			<xsl:variable name="NumberOfPax"><xsl:value-of select="$nip"/></xsl:variable>
			<xsl:variable name="TotalAmountDeci"><xsl:value-of select="substring-after(string(Summary/MonetaryAmount),'.')"/></xsl:variable>
			<xsl:variable name="TotalAmountNoDeci"><xsl:value-of select="string-length($TotalAmountDeci)"/></xsl:variable> 					
			<PassengerTypeQuantity>
				<xsl:attribute name="Code"><xsl:value-of select="$code"/></xsl:attribute>
				<xsl:attribute name="Quantity"><xsl:value-of select="$nip"/></xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>	
				<xsl:apply-templates select="ProductFlightSegment/ProductFareDetails" mode="FareBasis"/>
			</FareBasisCodes>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount"><xsl:value-of select="($TotalAmountPTC - $TaxAmountPTC) * $nip"/></xsl:attribute>
				</BaseFare>	
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount"><xsl:value-of select="$TaxAmountPTC * $nip"/></xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmountPTC * $nip"/></xsl:attribute>
				</TotalFare>							
			</PassengerFare>		
			<TPA_Extensions>
				<PricedCode>
					<xsl:choose>
						<xsl:when test="ProductFlightSegment/ProductFareDetails/Fare/FareType/Code = 'CH'">CHD</xsl:when>
						<xsl:when test="ProductFlightSegment/ProductFareDetails/Fare/FareType/Code = 'CNN'">CHD</xsl:when>
						<xsl:when test="ProductFlightSegment/ProductFareDetails/Fare/FareType/Code = 'YCD'">SRC</xsl:when>
						<xsl:otherwise><xsl:value-of select="ProductFlightSegment/ProductFareDetails/Fare/FareType/Code"/></xsl:otherwise>
					</xsl:choose>
				</PricedCode>
				<xsl:if test="Warning/AppendedMessage/Qualification/Type = 'APM'">
					<Text><xsl:value-of select="Warning/AppendedMessage[Qualification/Type='APM']/Text"/></Text>
				</xsl:if>
			</TPA_Extensions>	
		</PTC_FareBreakdown>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="ProductFareDetails" mode="FareBasis">
		<FareBasisCode>
				<xsl:value-of select="Fare/FareInformation/FareBasis"/>
				<xsl:if test="Fare/FareInformation/TicketDesignator !=''">/<xsl:value-of select="Fare/FareInformation/TicketDesignator"/></xsl:if>
		</FareBasisCode>	
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="Text" mode="diftypes">
		<xsl:choose>
			<xsl:when test="contains(string(.),'NOT FARED AT PASSENGER TYPE REQUESTED')">Y</xsl:when>
			<xsl:otherwise>N</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="RecommendedSegment">
		<xsl:variable name="pos1">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<OriginDestinationOption>
			<xsl:attribute name="SectorSequence"><xsl:value-of select="$pos1"/></xsl:attribute>
			<xsl:apply-templates select="FlightSegment">
				<xsl:with-param name="pos1"><xsl:value-of select="$pos1"/></xsl:with-param>
			</xsl:apply-templates>
		</OriginDestinationOption>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="FlightSegment">
		<xsl:param name="pos1"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="depday">
			<xsl:value-of select="substring(string(Description/Schedule/DepartureDate),1,2)"/>
		</xsl:variable>
		<xsl:variable name="depmonth">
			<xsl:value-of select="substring(string(Description/Schedule/DepartureDate),3,2)"/>
		</xsl:variable>
		<xsl:variable name="depyear">
			<xsl:value-of select="substring(string(Description/Schedule/DepartureDate),5,2)"/>
		</xsl:variable>
		<xsl:variable name="arrday">
			<xsl:value-of select="substring(string(Description/Schedule/ArrivalDate),1,2)"/>
		</xsl:variable>
		<xsl:variable name="arrmonth">
			<xsl:value-of select="substring(string(Description/Schedule/ArrivalDate),3,2)"/>
		</xsl:variable>
		<xsl:variable name="arryear">
			<xsl:value-of select="substring(string(Description/Schedule/ArrivalDate),5,2)"/>
		</xsl:variable>
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime">20<xsl:value-of select="$depyear"/>-<xsl:value-of select="$depmonth"/>-<xsl:value-of select="$depday"/>T<xsl:value-of select="substring(string(Description/Schedule/DepartureTime),1,2)"/>:<xsl:value-of select="substring(string(Description/Schedule/DepartureTime),3,2)"/>:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">20<xsl:value-of select="$arryear"/>-<xsl:value-of select="$arrmonth"/>-<xsl:value-of select="$arrday"/>T<xsl:value-of select="substring(string(Description/Schedule/ArrivalTime),1,2)"/>:<xsl:value-of select="substring(string(Description/Schedule/ArrivalTime),3,2)"/>:00</xsl:attribute>
			<xsl:attribute name="StopQuantity"><xsl:value-of select="AdditionalDetails/FlightDetails/NumberOfStops"/></xsl:attribute>
			<xsl:attribute name="RPH">1</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="Description/Identification/FlightNumber"/>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<!--xsl:value-of select="Description/Identification/ClassOfService"/-->
				<xsl:value-of select="../../Product/ProductFlightSegment[position()=$pos1]/ProductFareDetails[position()=$pos]/Reservation/Booking/Code"/>
			</xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="sum(../../Product[Passenger/PassengerIndicators/InfantIndicator != '1']/NumberOfPassengers/Unit/Value)"/></xsl:attribute>
			<xsl:attribute name="E_TicketEligibility">
				<xsl:choose>
					<xsl:when test="Description/Indicator/CodeShareAndElectronicTicket = 'ET'">Eligible</xsl:when>
					<xsl:otherwise>NotEligible</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="Description/DepartureAirport/Code"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="Description/ArrivalAirport/Code"/></xsl:attribute>
			</ArrivalAirport>
			<xsl:choose>
				<xsl:when test="Description/Airline/JointOrCodeShared">
					<OperatingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="Description/Airline/JointOrCodeShared"/></xsl:attribute>
					</OperatingAirline>
				</xsl:when>
				<xsl:otherwise>
					<OperatingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="Description/Airline/Code"/></xsl:attribute>
					</OperatingAirline>
				</xsl:otherwise>
			</xsl:choose>
			<Equipment>
				<xsl:attribute name="AirEquipType"><xsl:value-of select="AdditionalDetails/FlightDetails/AircraftType"/></xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="Description/Airline/Code"/></xsl:attribute>
			</MarketingAirline>		
			<TPA_Extensions>
				<JourneyTotalDuration>
					<xsl:value-of select="substring(../ElapsedFlyingTime/FlightDetails/EFT,1,2)"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="substring(../ElapsedFlyingTime/FlightDetails/EFT,3,4)"/>
				</JourneyTotalDuration> 
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
	
	<xsl:template match="PassengerTypeQuantity" mode="FareRule">
		<xsl:param name="rec"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<!--xsl:variable name="pos">
			<xsl:choose>
				<xsl:when test="position() = 1">1</xsl:when>
				<xsl:otherwise><xsl:value-of select="sum(preceding-sibling::PassengerTypeQuantity/@Quantity) + 1"/></xsl:otherwise>
			</xsl:choose>
		</xsl:variable-->
		<xsl:apply-templates select="../../Recommendation[position() = $rec]/Product[position() = $pos]" mode="FareRule"/>
	</xsl:template>
	
	<xsl:template match="PassengerTypeQuantity" mode="FareRuleInf">
		<xsl:param name="rec"/>
		<xsl:apply-templates select="../../Recommendation[position() = $rec]/Product[position() = last()]" mode="FareRule"/>
	</xsl:template>

	
	<xsl:template match="Product" mode="FareRule">
		<xsl:apply-templates select="ProductFlightSegment" mode="FareRule"/>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="ProductFlightSegment" mode="FareRule">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:apply-templates select="ProductFareDetails">
			<xsl:with-param name="pos1">
				<xsl:value-of select="$pos"/>				
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="ProductFareDetails">	
		<xsl:param name="pos1"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>		
		<FareInfo>
			<xsl:choose>
				<xsl:when test="../../../Product/PricingDetails[Ticketing/Code='RN']">
					<xsl:attribute name="NegotiatedFare">1</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="NegotiatedFare">0</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:variable name="depdayfarerule">
				<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureDate),1,2)"/>
			</xsl:variable>
			<xsl:variable name="depmonthfarerule">
				<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureDate),3,2)"/>
			</xsl:variable>
			<xsl:variable name="depyearfarerule">
				<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureDate),5,2)"/>
			</xsl:variable>			
			<DepartureDate>20<xsl:value-of select="$depyearfarerule"/>-<xsl:value-of select="$depmonthfarerule"/>-<xsl:value-of select="$depdayfarerule"/>T<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureTime),1,2)"/>:<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureTime),3,2)"/>:00</DepartureDate>
			<FareReference><xsl:value-of select="../../ProductFlightSegment[position()=$pos1]/ProductFareDetails[position()=$pos]/Fare/FareInformation/FareBasis"/></FareReference>
			<!--FareReference><xsl:value-of select="Fare/FareInformation/FareBasis"/></FareReference-->	
			<RuleInfo>
				<xsl:apply-templates select="../../Warning/AppendedMessage[Qualification/Type = 'LTD']" mode="Advance"/> 
				<xsl:apply-templates select="../../Warning/AppendedMessage[Qualification/Type = 'PEN']" mode="Penalty"/>  
			</RuleInfo>		
			<FilingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Airline/Code"/>
				</xsl:attribute>
			</FilingAirline>			
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/DepartureAirport/Code"/>
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/ArrivalAirport/Code"/>
				</xsl:attribute>
			</ArrivalAirport>
		</FareInfo>				
	</xsl:template>	
	<!--****************************************************************************************-->
	<!-- Tax 																    -->
	<!--****************************************************************************************-->
	<xsl:template match="Details">
		<xsl:variable name="TaxDeci" select="substring-after(string(../../TotalPrice/Money[1]/Amount),'.')"/>
		<xsl:variable name="TaxDeci2" select="string-length($TaxDeci)"/>
		<Tax>
			<xsl:attribute name="TaxCode"><xsl:value-of select="Country"/></xsl:attribute>
			<xsl:attribute name="Amount"><xsl:value-of select="translate(string(Amount),'.','')"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../CurrencyDetails/Currency/Code"/></xsl:attribute>
			<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$TaxDeci2"/></xsl:attribute>
		</Tax>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- Fare Rules - Penalty													    -->
	<!--****************************************************************************************-->
	<xsl:template match = "AppendedMessage" mode="Penalty">
		<xsl:choose>
			<xsl:when test="Qualification/Code='70' or Qualification/Code='71'">
				<ChargesRules>
					<VoluntaryChanges>
						<Penalty>									 
							<xsl:attribute name="PenaltyType">
								<xsl:value-of select="Text"/>
							</xsl:attribute>				
						</Penalty> 
					</VoluntaryChanges>
				</ChargesRules>
			</xsl:when>
			<xsl:when test="Qualification/Code='73'">
				<ChargesRules>
					<VoluntaryChanges>
						<Penalty>									 
							<xsl:variable name="Deci" select="substring-after(string(../Penalty/Money/Amount),'.')"/>
							<xsl:variable name="NoDeci" select="string-length($Deci)"/>		
								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(string(../Penalty/Money/Amount),'.','')"/>
								</xsl:attribute>									
								<xsl:attribute name="CurrencyCode">					
									<xsl:value-of select="../Penalty/Money/CurrencyCode"/>
								</xsl:attribute>					
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci"/>
								</xsl:attribute>				
						</Penalty> 
					</VoluntaryChanges>
				</ChargesRules>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- Fare Rules - Advance Purchase													    -->
	<!--****************************************************************************************-->
	<xsl:template match = "AppendedMessage" mode="Advance">
		<xsl:choose>
			<xsl:when test="Qualification/Code='40'">
				<ResTicketingRules>
					<AdvResTicketing>
						<AdvTicketing>
							<xsl:for-each select="Text">
								<xsl:value-of select="."/>
								<xsl:value-of select="string(' ')"/>
							</xsl:for-each>
						</AdvTicketing> 
					</AdvResTicketing>
				</ResTicketingRules>
			</xsl:when>
		</xsl:choose>
	</xsl:template>

<!--****************************************************************************************-->
	<xsl:template match="Code" mode="depterm">
		<Terminal>
			<xsl:value-of select="."/>
		</Terminal>
	</xsl:template>
	<xsl:template match="Code" mode="arrterm">
		<Terminal>
			<xsl:value-of select="."/>
		</Terminal>
	</xsl:template>
	<!--****************************************************************************************-->
	
	<xsl:template name="month">
		<xsl:param name="month" />
		<xsl:choose>
			<xsl:when test="$month = 'JAN'">01</xsl:when>
			<xsl:when test="$month = 'FEB'">02</xsl:when>
			<xsl:when test="$month = 'MAR'">03</xsl:when>
			<xsl:when test="$month = 'APR'">04</xsl:when>
			<xsl:when test="$month = 'MAY'">05</xsl:when>
			<xsl:when test="$month = 'JUN'">06</xsl:when>
			<xsl:when test="$month = 'JUL'">07</xsl:when>
			<xsl:when test="$month = 'AUG'">08</xsl:when>
			<xsl:when test="$month = 'SEP'">09</xsl:when>
			<xsl:when test="$month = 'OCT'">10</xsl:when>
			<xsl:when test="$month = 'NOV'">11</xsl:when>
			<xsl:when test="$month = 'DEC'">12</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
