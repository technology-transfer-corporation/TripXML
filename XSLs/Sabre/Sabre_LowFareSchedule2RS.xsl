<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- ================================================================== -->
	<!-- Sabre_LowFareSchedule2RS.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 28 Oct 2011 - Shashin - corrected JourneyTotalDuration format			-->
	<!-- Date: 04 Dec 2010 - Rastko - added calculation of JourneyTotalDuration			-->
	<!-- Date: 10 Sep 2010 - Rastko - mapped pricing source and validating airline			-->
	<!-- Date: 16 Jul 2010 - Rastko	- fixed display of error message					-->
	<!-- Date: 13 Jul 2010 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchScheduleRS"/>
	</xsl:template>
	<xsl:template match="OTA_AirLowFareSearchScheduleRS">
		<OTA_AirLowFareSearchScheduleRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Sabre</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code"><xsl:choose><xsl:when test="Errors/Error/@Code != ''"><xsl:value-of select="Errors/Error/@Code"/></xsl:when><xsl:otherwise>E</xsl:otherwise></xsl:choose></xsl:attribute>
							<xsl:value-of select="Errors/Error"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary[1]" mode="start">
							<xsl:with-param name="flts"/>
							<xsl:with-param name="seq">1</xsl:with-param>
						</xsl:apply-templates>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchScheduleRS>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="PricedItinerary" mode="start">
		<xsl:param name="flts"/>
		<xsl:param name="seq"/>
		<xsl:variable name="flindex">
			<xsl:for-each select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment">
				<xsl:value-of select="MarketingAirline/@Code"/>
				<xsl:value-of select="@FlightNumber"/>
			</xsl:for-each>
			<xsl:value-of select="'-'"/>
		</xsl:variable>
		<xsl:variable name="classes">
			<xsl:for-each select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment">
				<xsl:value-of select="@ResBookDesigCode"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="cabins">
			<xsl:for-each select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment">
				<xsl:value-of select="concat(TPA_Extensions/CabinType/@Cabin,'-')"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="seq1">
			<xsl:choose>
				<xsl:when test="$flts='' or not(contains($flts,$flindex))">
					<xsl:value-of select="($seq + 1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$seq"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="$flts='' or not(contains($flts,$flindex))">
			<PricedItinerary>
				<xsl:attribute name="SequenceNumber"><xsl:value-of select="$seq"/></xsl:attribute>
				<AirItinerary>
					<OriginDestinationOptions>
						<OriginDestinationOption>
							<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment" mode="od">
								<xsl:with-param name="type">1</xsl:with-param>
								<xsl:with-param name="classes"/>
								<xsl:with-param name="cabins"/>
								<xsl:with-param name="rph">1</xsl:with-param>
							</xsl:apply-templates>
						</OriginDestinationOption>
						<OriginDestinationOption>
							<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[position()=2]/FlightSegment" mode="od">
								<xsl:with-param name="type">2</xsl:with-param>
								<xsl:with-param name="classes">
									<xsl:value-of select="$classes"/>
								</xsl:with-param>
								<xsl:with-param name="cabins">
									<xsl:value-of select="$cabins"/>
								</xsl:with-param>
								<xsl:with-param name="rph">1</xsl:with-param>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::PricedItinerary[1]" mode="next">
								<xsl:with-param name="flindex">
									<xsl:value-of select="$flindex"/>
								</xsl:with-param>
								<xsl:with-param name="rph">1</xsl:with-param>
							</xsl:apply-templates>
						</OriginDestinationOption>
					</OriginDestinationOptions>
				</AirItinerary>
				<TicketingInfo>
					<xsl:attribute name="TicketTimeLimit"><xsl:apply-templates select="TicketingInfo/@TicketTimeLimit"/></xsl:attribute>
				</TicketingInfo>
			</PricedItinerary>
		</xsl:if>
		<xsl:apply-templates select="following-sibling::PricedItinerary[1]" mode="start">
			<xsl:with-param name="flts">
				<xsl:value-of select="concat($flts,$flindex)"/>
			</xsl:with-param>
			<xsl:with-param name="seq">
				<xsl:value-of select="$seq1"/>
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="PricedItinerary" mode="next">
		<xsl:param name="flindex"/>
		<xsl:param name="rph"/>
		<xsl:variable name="flindex1">
			<xsl:for-each select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment">
				<xsl:value-of select="MarketingAirline/@Code"/>
				<xsl:value-of select="@FlightNumber"/>
			</xsl:for-each>
			<xsl:value-of select="'-'"/>
		</xsl:variable>
		<xsl:variable name="classes">
			<xsl:for-each select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment">
				<xsl:value-of select="@ResBookDesigCode"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="cabins">
			<xsl:for-each select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment">
				<xsl:value-of select="concat(TPA_Extensions/CabinType/@Cabin,'-')"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="rph1">
			<xsl:choose>
				<xsl:when test="contains($flindex,$flindex1)">
					<xsl:value-of select="($rph + 1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$rph"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:if test="contains($flindex,$flindex1)">
			<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption[position()=2]/FlightSegment" mode="od">
				<xsl:with-param name="type">2</xsl:with-param>
				<xsl:with-param name="classes">
					<xsl:value-of select="$classes"/>
				</xsl:with-param>
				<xsl:with-param name="cabins">
					<xsl:value-of select="$cabins"/>
				</xsl:with-param>
				<xsl:with-param name="rph">
					<xsl:value-of select="$rph1"/>
				</xsl:with-param>
			</xsl:apply-templates>
		</xsl:if>
		<xsl:apply-templates select="following-sibling::PricedItinerary[1]" mode="next">
			<xsl:with-param name="flindex">
				<xsl:value-of select="$flindex"/>
			</xsl:with-param>
			<xsl:with-param name="rph">
				<xsl:value-of select="$rph1"/>
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="FlightSegment" mode="od">
		<xsl:param name="type"/>
		<xsl:param name="classes"/>
		<xsl:param name="cabins"/>
		<xsl:param name="rph"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime"><xsl:value-of select="@DepartureDateTime"/></xsl:attribute>
			<xsl:attribute name="ArrivalDateTime"><xsl:value-of select="@ArrivalDateTime"/></xsl:attribute>
			<xsl:attribute name="StopQuantity"><xsl:value-of select="@StopQuantity"/></xsl:attribute>
			<xsl:attribute name="RPH"><xsl:choose><xsl:when test="$type='1'">1</xsl:when><xsl:otherwise><xsl:value-of select="$rph"/></xsl:otherwise></xsl:choose></xsl:attribute>
			<xsl:attribute name="FlightNumber"><xsl:value-of select="@FlightNumber"/></xsl:attribute>
			<xsl:attribute name="ResBookDesigCode"><xsl:if test="$type='2'"><xsl:value-of select="@ResBookDesigCode"/></xsl:if></xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="@NumberInParty"/></xsl:attribute>
			<xsl:attribute name="E_TicketEligibility"><xsl:choose><xsl:when test="TPA_Extensions/eTicket/@Ind = 'true'">Eligible</xsl:when><xsl:otherwise>NotEligible</xsl:otherwise></xsl:choose></xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="DepartureAirport/@LocationCode"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="ArrivalAirport/@LocationCode"/></xsl:attribute>
			</ArrivalAirport>
			<OperatingAirline>
				<xsl:attribute name="Code"><xsl:choose><xsl:when test="OperatingAirline/@Code !=''"><xsl:value-of select="OperatingAirline/@Code"/></xsl:when><xsl:otherwise><xsl:value-of select="MarketingAirline/@Code"/></xsl:otherwise></xsl:choose></xsl:attribute>
			</OperatingAirline>
			<Equipment>
				<xsl:attribute name="AirEquipType"><xsl:value-of select="Equipment/@AirEquipType"/></xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="MarketingAirline/@Code"/></xsl:attribute>
			</MarketingAirline>
			<TPA_Extensions>
				<xsl:if test="$type='2'">
					<xsl:attribute name="PricingSource"><xsl:value-of select="../../../../AirItineraryPricingInfo/@PricingSource"/></xsl:attribute>
					<xsl:attribute name="ValidatingAirlineCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/@ValidatingAirlineCode"/></xsl:attribute>
					<CabinType>
						<xsl:attribute name="Cabin"><xsl:value-of select="TPA_Extensions/CabinType/@Cabin"/></xsl:attribute>
					</CabinType>
				</xsl:if>
				<JourneyTotalDuration>
					<xsl:value-of select="normalize-space(TPA_Extensions/JourneyTotalDuration)"/>
				</JourneyTotalDuration>
				<JourneyDuration>
					<xsl:value-of select="TPA_Extensions/JourneyDuration"/>
				</JourneyDuration>
				<xsl:choose>
					<xsl:when test="$type='1'">
						<FromTotalBaseFare>
							<xsl:attribute name="Amount"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:attribute>
						</FromTotalBaseFare>
						<FromTotalTax>
							<xsl:attribute name="Amount"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@TaxCode='TotalTax']/@Amount"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@TaxCode='TotalTax']/@CurrencyCode"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@TaxCode='TotalTax']/@DecimalPlaces"/></xsl:attribute>
						</FromTotalTax>
						<FromTotalFare>
							<xsl:attribute name="Amount"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/TotalFare/@DecimalPlaces"/></xsl:attribute>
						</FromTotalFare>
					</xsl:when>
					<xsl:otherwise>
						<TotalBaseFare>
							<xsl:attribute name="Amount"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:attribute>
						</TotalBaseFare>
						<TotalTax>
							<xsl:attribute name="Amount"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@TaxCode='TotalTax']/@Amount"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@TaxCode='TotalTax']/@CurrencyCode"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@TaxCode='TotalTax']/@DecimalPlaces"/></xsl:attribute>
						</TotalTax>
						<TotalFare>
							<xsl:attribute name="Amount"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="../../../../AirItineraryPricingInfo/ItinTotalFare/TotalFare/@DecimalPlaces"/></xsl:attribute>
						</TotalFare>
						<xsl:call-template name="prevclasses">
							<xsl:with-param name="prevclasses">
								<xsl:value-of select="$classes"/>
							</xsl:with-param>
							<xsl:with-param name="prevcabins">
								<xsl:value-of select="$cabins"/>
							</xsl:with-param>
							<xsl:with-param name="rph">1</xsl:with-param>
						</xsl:call-template>
						<xsl:for-each select="../../../../AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown">
							<FareBasisCodes>
								<xsl:attribute name="PassengerType"><xsl:value-of select="PassengerTypeQuantity/@Code"/></xsl:attribute>
								<xsl:for-each select="FareBasisCodes/FareBasisCode">
									<FareBasisCode>
										<xsl:value-of select="."/>
									</FareBasisCode>
								</xsl:for-each>
							</FareBasisCodes>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
	<xsl:template match="flightDetails">
		<xsl:param name="pos"/>
		<xsl:param name="odpos"/>
		<xsl:param name="Cur"/>
		<xsl:param name="NoDeci"/>
		<xsl:param name="TotalAmount"/>
		<xsl:param name="TaxTotal"/>
		<xsl:param name="classes"/>
		<xsl:param name="prevclasses"/>
		<xsl:param name="cabins"/>
		<xsl:param name="prevcabins"/>
		<xsl:param name="flindex"/>
		<xsl:param name="pricesource"/>
		<xsl:param name="fbc"/>
		<xsl:param name="vc"/>
		<xsl:variable name="posit">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="depday">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),1,2)"/>
		</xsl:variable>
		<xsl:variable name="depmonth">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),3,2)"/>
		</xsl:variable>
		<xsl:variable name="depyear">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),5,2)"/>
		</xsl:variable>
		<xsl:variable name="arrday">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),1,2)"/>
		</xsl:variable>
		<xsl:variable name="arrmonth">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),3,2)"/>
		</xsl:variable>
		<xsl:variable name="arryear">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),5,2)"/>
		</xsl:variable>
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime">20<xsl:value-of select="$depyear"/>-<xsl:value-of select="$depmonth"/>-<xsl:value-of select="$depday"/>T<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfDeparture),1,2)"/>:<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfDeparture),3,2)"/>:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">20<xsl:value-of select="$arryear"/>-<xsl:value-of select="$arrmonth"/>-<xsl:value-of select="$arrday"/>T<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfArrival),1,2)"/>:<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfArrival),3,2)"/>:00</xsl:attribute>
			<xsl:if test="flightInformation/productDetail/techStopNumber!=''">
				<xsl:attribute name="StopQuantity"><xsl:value-of select="flightInformation/productDetail/techStopNumber"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="RPH"><xsl:value-of select="$pos"/></xsl:attribute>
			<xsl:attribute name="FlightNumber"><xsl:value-of select="flightInformation/flightNumber"/></xsl:attribute>
			<xsl:attribute name="ResBookDesigCode"><xsl:choose><xsl:when test="$TotalAmount!=''"><xsl:value-of select="substring($classes,$posit,1)"/></xsl:when><xsl:otherwise><xsl:value-of select="$classes"/></xsl:otherwise></xsl:choose><!--xsl:value-of select="$classes/fareDetails[position()=$odpos]/productInformation[position()=$posit]/avlProductDetails/rbd" /--></xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="count(../../../recommendation[1]/paxFareProduct/paxReference[ptc != 'INF' and ptc != 'IN']/traveller)"/></xsl:attribute>
			<xsl:attribute name="E_TicketEligibility"><xsl:choose><xsl:when test="flightInformation/addProductDetail/electronicTicketing='Y'">Eligible</xsl:when><xsl:otherwise>NotEligible</xsl:otherwise></xsl:choose></xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="flightInformation/location[1]/locationId"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="flightInformation/location[2]/locationId"/></xsl:attribute>
			</ArrivalAirport>
			<OperatingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="flightInformation/companyId/operatingCarrier"/></xsl:attribute>
			</OperatingAirline>
			<Equipment>
				<xsl:attribute name="AirEquipType"><xsl:value-of select="flightInformation/productDetail/equipmentType"/></xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="flightInformation/companyId/marketingCarrier"/></xsl:attribute>
			</MarketingAirline>
			<TPA_Extensions>
				<xsl:if test="$TotalAmount!=''">
					<xsl:attribute name="PricingSource"><xsl:value-of select="$pricesource"/></xsl:attribute>
					<xsl:attribute name="ValidatingAirlineCode"><xsl:value-of select="$vc"/></xsl:attribute>
				</xsl:if>
				<cabins>
					<xsl:copy-of select="$cabins"/>
				</cabins>
				<classes>
					<xsl:copy-of select="$classes"/>
				</classes>
				<xsl:if test="$TotalAmount!=''">
					<CabinType>
						<xsl:attribute name="Cabin"><xsl:choose><xsl:when test="substring($cabins,$posit,1) = 'F'">First</xsl:when><xsl:when test="substring($cabins,$posit,1) = 'C'">Business</xsl:when><xsl:when test="substring($cabins,$posit,1) = 'W'">Premium</xsl:when><xsl:otherwise>Economy</xsl:otherwise></xsl:choose></xsl:attribute>
					</CabinType>
				</xsl:if>
				<JourneyTotalDuration>
					<xsl:value-of select="normalize-space(substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,1,2))"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,3,4)"/>
				</JourneyTotalDuration>
				<xsl:choose>
					<xsl:when test="$TotalAmount!=''">
						<xsl:if test="$odpos > 1">
							<TotalBaseFare>
								<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount - $TaxTotal"/></xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="$Cur"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
							</TotalBaseFare>
							<TotalTax>
								<xsl:attribute name="Amount"><xsl:value-of select="$TaxTotal"/></xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="$Cur"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
							</TotalTax>
							<TotalFare>
								<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount"/></xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="$Cur"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
							</TotalFare>
							<xsl:call-template name="prevclasses">
								<xsl:with-param name="prevclasses">
									<xsl:value-of select="$prevclasses"/>
								</xsl:with-param>
								<xsl:with-param name="prevcabins">
									<xsl:value-of select="$prevcabins"/>
								</xsl:with-param>
								<xsl:with-param name="rph">1</xsl:with-param>
							</xsl:call-template>
							<xsl:copy-of select="$fbc"/>
						</xsl:if>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="../../../recommendation[segmentFlightRef/referencingDetail[1]/refNumber = $flindex][1]" mode="fromfare"/>
					</xsl:otherwise>
				</xsl:choose>
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
	<xsl:template match="recommendation" mode="flight">
		<xsl:param name="flindex"/>
		<xsl:apply-templates select="segmentFlightRef[referencingDetail[1]/refNumber = $flindex]"/>
	</xsl:template>
	<xsl:template match="recommendation" mode="classes">
		<xsl:value-of select="paxFareProduct/fareDetails[1]/groupOfFares/productInformation[1]/cabinProduct/rbd"/>
	</xsl:template>
	<xsl:template match="recommendation" mode="cabins">
		<xsl:value-of select="paxFareProduct/fareDetails[1]/groupOfFares/productInformation[1]/cabinProduct/cabin"/>
	</xsl:template>
	<xsl:template match="recommendation" mode="ticketlimit">
		<xsl:attribute name="TicketTimeLimit"><xsl:text>20</xsl:text><xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],6,2)"/><xsl:text>-</xsl:text><xsl:call-template name="month"><xsl:with-param name="month"><xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],3,3)"/></xsl:with-param></xsl:call-template><xsl:text>-</xsl:text><xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],1,2)"/><xsl:text>T23:59:00</xsl:text></xsl:attribute>
	</xsl:template>
	<xsl:template match="recommendation" mode="fare">
		<xsl:choose>
			<xsl:when test="paxFareProduct/fareDetails/productInformation[1]/fareProductDetail/fareType='RP'">
				<xsl:attribute name="PricingSource">Published</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="PricingSource">Private</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:variable name="Deci" select="substring-after(string(recPriceInfo/monetaryDetail[1]/amount),'.')"/>
		<xsl:variable name="NoDeci" select="string-length($Deci)"/>
		<xsl:variable name="TotalAmount" select="translate(string(recPriceInfo/monetaryDetail[1]/amount),'.,','')"/>
		<xsl:variable name="TaxTotal">
			<xsl:choose>
				<xsl:when test="recPriceInfo/monetaryDetail[2]/amount">
					<xsl:value-of select="translate(string(recPriceInfo/monetaryDetail[2]/amount),'.','')"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<ItinTotalFare>
			<xsl:choose>
				<xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RN'">
					<xsl:attribute name="NegotiatedFareCode"/>
				</xsl:when>
				<xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RA'">
					<xsl:attribute name="NegotiatedFareCode">CAT35</xsl:attribute>
				</xsl:when>
				<xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId!=''">
					<xsl:attribute name="NegotiatedFareCode"><xsl:value-of select="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId"/></xsl:attribute>
				</xsl:when>
			</xsl:choose>
			<TotalFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount"/></xsl:attribute>
				<xsl:attribute name="CurrencyCode"><xsl:value-of select="../conversionRate/conversionRateDetail/currency"/></xsl:attribute>
				<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
			</TotalFare>
		</ItinTotalFare>
	</xsl:template>
	<xsl:template match="recommendation" mode="fromfare">
		<xsl:variable name="Deci" select="substring-after(string(recPriceInfo/monetaryDetail[1]/amount),'.')"/>
		<xsl:variable name="NoDeci" select="string-length($Deci)"/>
		<xsl:variable name="TotalAmount" select="translate(string(recPriceInfo/monetaryDetail[1]/amount),'.,','')"/>
		<xsl:variable name="TaxTotal">
			<xsl:choose>
				<xsl:when test="recPriceInfo/monetaryDetail[2]/amount">
					<xsl:value-of select="translate(string(recPriceInfo/monetaryDetail[2]/amount),'.','')"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<FromTotalBaseFare>
			<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount - $TaxTotal"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="../conversionRate/conversionRateDetail/currency"/></xsl:attribute>
			<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
		</FromTotalBaseFare>
		<FromTotalTax>
			<xsl:attribute name="Amount"><xsl:value-of select="$TaxTotal"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="../conversionRate/conversionRateDetail/currency"/></xsl:attribute>
			<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
		</FromTotalTax>
		<FromTotalFare>
			<xsl:choose>
				<xsl:when test="paxFareProduct/fareDetails/productInformation[1]/fareProductDetail/fareType='RP'">
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:attribute name="Amount"><xsl:value-of select="$TotalAmount"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="../conversionRate/conversionRateDetail/currency"/></xsl:attribute>
			<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
		</FromTotalFare>
	</xsl:template>
	<xsl:template match="segmentFlightRef">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="ref">
			<xsl:value-of select="referencingDetail[2]/refNumber"/>
		</xsl:variable>
		<xsl:variable name="Deci" select="substring-after(string(../recPriceInfo/monetaryDetail[1]/amount),'.')"/>
		<xsl:variable name="NoDeci" select="string-length($Deci)"/>
		<xsl:variable name="Cur" select="../../conversionRate/conversionRateDetail/currency"/>
		<xsl:variable name="TotalAmount" select="translate(string(../recPriceInfo/monetaryDetail[1]/amount),'.,','')"/>
		<xsl:variable name="TaxTotal">
			<xsl:choose>
				<xsl:when test="../recPriceInfo/monetaryDetail[2]/amount">
					<xsl:value-of select="translate(string(../recPriceInfo/monetaryDetail[2]/amount),'.','')"/>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="classes">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[2]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/rbd"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="prevclasses">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[1]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/rbd"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="cabins">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[2]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/cabin"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="prevcabins">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[1]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/cabin"/>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="pricesource">
			<xsl:choose>
				<xsl:when test="../paxFareProduct/fareDetails/groupOfFares[1]/productInformation/fareProductDetail/fareType='RP'">Published</xsl:when>
				<xsl:otherwise>Private</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="fbc">
			<xsl:for-each select="../paxFareProduct">
				<FareBasisCodes>
					<xsl:attribute name="PassengerType"><xsl:choose><xsl:when test="paxReference/ptc='CNN'">CHD</xsl:when><xsl:otherwise><xsl:value-of select="paxReference/ptc"/></xsl:otherwise></xsl:choose></xsl:attribute>
					<xsl:for-each select="fareDetails/groupOfFares/productInformation">
						<FareBasisCode>
							<xsl:value-of select="fareProductDetail/fareBasis"/>
						</FareBasisCode>
					</xsl:for-each>
				</FareBasisCodes>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="vc">
			<xsl:value-of select="../paxFareProduct/paxFareDetail/codeShareDetails[transportStageQualifier='V']/company"/>
		</xsl:variable>
		<xsl:apply-templates select="../../flightIndex[2]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]" mode="segs">
			<xsl:with-param name="pos">
				<xsl:value-of select="$pos"/>
			</xsl:with-param>
			<xsl:with-param name="odpos">2</xsl:with-param>
			<xsl:with-param name="Cur">
				<xsl:value-of select="$Cur"/>
			</xsl:with-param>
			<xsl:with-param name="NoDeci">
				<xsl:value-of select="$NoDeci"/>
			</xsl:with-param>
			<xsl:with-param name="TotalAmount">
				<xsl:value-of select="$TotalAmount"/>
			</xsl:with-param>
			<xsl:with-param name="TaxTotal">
				<xsl:value-of select="$TaxTotal"/>
			</xsl:with-param>
			<xsl:with-param name="classes">
				<xsl:value-of select="$classes"/>
			</xsl:with-param>
			<xsl:with-param name="prevclasses">
				<xsl:value-of select="$prevclasses"/>
			</xsl:with-param>
			<xsl:with-param name="cabins">
				<xsl:value-of select="$cabins"/>
			</xsl:with-param>
			<xsl:with-param name="prevcabins">
				<xsl:value-of select="$prevcabins"/>
			</xsl:with-param>
			<xsl:with-param name="pricesource">
				<xsl:value-of select="$pricesource"/>
			</xsl:with-param>
			<xsl:with-param name="fbc" select="$fbc"/>
			<xsl:with-param name="vc" select="$vc"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="groupOfFlights" mode="segs">
		<xsl:param name="pos"/>
		<xsl:param name="odpos"/>
		<xsl:param name="Cur"/>
		<xsl:param name="NoDeci"/>
		<xsl:param name="TotalAmount"/>
		<xsl:param name="TaxTotal"/>
		<xsl:param name="classes"/>
		<xsl:param name="prevclasses"/>
		<xsl:param name="cabins"/>
		<xsl:param name="prevcabins"/>
		<xsl:param name="pricesource"/>
		<xsl:param name="fbc"/>
		<xsl:param name="vc"/>
		<xsl:apply-templates select="flightDetails">
			<xsl:with-param name="pos">
				<xsl:value-of select="$pos"/>
			</xsl:with-param>
			<xsl:with-param name="odpos">2</xsl:with-param>
			<xsl:with-param name="Cur">
				<xsl:value-of select="$Cur"/>
			</xsl:with-param>
			<xsl:with-param name="NoDeci">
				<xsl:value-of select="$NoDeci"/>
			</xsl:with-param>
			<xsl:with-param name="TotalAmount">
				<xsl:value-of select="$TotalAmount"/>
			</xsl:with-param>
			<xsl:with-param name="TaxTotal">
				<xsl:value-of select="$TaxTotal"/>
			</xsl:with-param>
			<xsl:with-param name="classes">
				<xsl:value-of select="$classes"/>
			</xsl:with-param>
			<xsl:with-param name="prevclasses">
				<xsl:value-of select="$prevclasses"/>
			</xsl:with-param>
			<xsl:with-param name="cabins">
				<xsl:value-of select="$cabins"/>
			</xsl:with-param>
			<xsl:with-param name="prevcabins">
				<xsl:value-of select="$prevcabins"/>
			</xsl:with-param>
			<xsl:with-param name="flindex"/>
			<xsl:with-param name="pricesource">
				<xsl:value-of select="$pricesource"/>
			</xsl:with-param>
			<xsl:with-param name="fbc" select="$fbc"/>
			<xsl:with-param name="vc" select="$vc"/>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template name="prevclasses">
		<xsl:param name="prevclasses"/>
		<xsl:param name="prevcabins"/>
		<xsl:param name="rph"/>
		<xsl:if test="string-length($prevclasses) > 0">
			<OriginClass>
				<xsl:attribute name="Index"><xsl:value-of select="$rph"/></xsl:attribute>
				<xsl:attribute name="Cabin"><xsl:value-of select="substring-before($prevcabins,'-')"/></xsl:attribute>
				<xsl:value-of select="substring($prevclasses,1,1)"/>
			</OriginClass>
			<xsl:call-template name="prevclasses">
				<xsl:with-param name="prevclasses">
					<xsl:value-of select="substring($prevclasses,2)"/>
				</xsl:with-param>
				<xsl:with-param name="prevcabins">
					<xsl:value-of select="substring-after($prevcabins,'-')"/>
				</xsl:with-param>
				<xsl:with-param name="rph">
					<xsl:value-of select="$rph + 1"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template name="month">
		<xsl:param name="month"/>
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
