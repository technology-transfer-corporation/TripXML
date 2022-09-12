<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_LowFarePlus_Sch1RS.xsl 											-->
	<!-- ================================================================== -->
	<!-- Date: 18 Jul 2006 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PoweredLowestFare_SearchReply" />
		<xsl:apply-templates select="MessagesOnly_Reply" />
	</xsl:template>

	<xsl:template match="MessagesOnly_Reply">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="PoweredLowestFare_SearchReply">
		<PoweredLowestFare_SearchReply>
			<xsl:choose>
				<xsl:when test="errorMessage">
					<xsl:copy-of select="errorMessage"/>
				</xsl:when>
				<xsl:otherwise>
					<PricedItineraries>
						<xsl:apply-templates select="flightIndex[1]/groupOfFlights" mode="start"/>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</PoweredLowestFare_SearchReply>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="groupOfFlights" mode="start">
		<xsl:variable name="flindex"><xsl:value-of select="propFlightGrDetail/flightProposal/ref"/></xsl:variable>
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<AirItinerary>
				<OriginDestinationOptions>
					<OriginDestinationOption>
						<xsl:apply-templates select="flightDetails">
							<xsl:with-param name="pos">1</xsl:with-param>
							<xsl:with-param name="odpos">1</xsl:with-param>
							<xsl:with-param name="Deci"></xsl:with-param>
							<xsl:with-param name="NoDeci"></xsl:with-param>
							<xsl:with-param name="TotalAmount"></xsl:with-param>
							<xsl:with-param name="classes"></xsl:with-param>
						</xsl:apply-templates>
					</OriginDestinationOption>
					<OriginDestinationOption>
						<xsl:apply-templates select="../../recommendation/segmentFlightRef[referencingDetail[1]/refNumber = $flindex]"/>
					</OriginDestinationOption>
				</OriginDestinationOptions>
			</AirItinerary>
			<AirItineraryPricingInfo>
				<xsl:apply-templates select="../../recommendation[segmentFlightRef/referencingDetail[1]/refNumber = $flindex][1]" mode="fare"/>
			</AirItineraryPricingInfo>
		</PricedItinerary>
	</xsl:template>

	<xsl:template match="flightDetails">
		<xsl:param name="pos" />
		<xsl:param name="odpos" />
		<xsl:param name="Cur" />
		<xsl:param name="NoDeci" />
		<xsl:param name="TotalAmount" />
		<xsl:param name="classes" />
		<xsl:variable name="posit"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="depday">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),1,2)" />
		</xsl:variable>
		<xsl:variable name="depmonth">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),3,2)" />
		</xsl:variable>
		<xsl:variable name="depyear">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfDeparture),5,2)" />
		</xsl:variable>
		<xsl:variable name="arrday">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),1,2)" />
		</xsl:variable>
		<xsl:variable name="arrmonth">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),3,2)" />
		</xsl:variable>
		<xsl:variable name="arryear">
			<xsl:value-of select="substring(string(flightInformation/productDateTime/dateOfArrival),5,2)" />
		</xsl:variable>
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime">20<xsl:value-of select="$depyear" />-<xsl:value-of select="$depmonth" />-<xsl:value-of select="$depday" />T<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfDeparture),1,2)" />:<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfDeparture),3,2)" />:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">20<xsl:value-of select="$arryear" />-<xsl:value-of select="$arrmonth" />-<xsl:value-of select="$arrday" />T<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfArrival),1,2)" />:<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfArrival),3,2)" />:00</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="flightInformation/flightNumber" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<!--xsl:value-of select="$classes/fareDetails[position()=$odpos]/productInformation[position()=$posit]/avlProductDetails/rbd" /-->
			</xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="count(../../../paxFareProduct/paxReference[ptc != 'INF']/traveller)" /></xsl:attribute>
			<xsl:attribute name="E_TicketEligibility">
				<xsl:choose>
					<xsl:when test="flightInformation/addProductDetail/electronicTicketing='Y'">Eligible</xsl:when>
					<xsl:otherwise>NotEligible</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="flightInformation/location[1]/locationId" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="flightInformation/location[2]/locationId" />
				</xsl:attribute>
			</ArrivalAirport>
			<OperatingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="flightInformation/companyId/operatingCarrier" />
				</xsl:attribute>
			</OperatingAirline>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="flightInformation/productDetail/equipmentType" />
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="flightInformation/companyId/marketingCarrier" />
				</xsl:attribute>
			</MarketingAirline>
			<TPA_Extensions>
				<JourneyTotalDuration>
					<xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,1,2)"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,3,4)"/>
				</JourneyTotalDuration> 
				<xsl:if test="$odpos > 1">
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$TotalAmount" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="$Cur" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$NoDeci" />
						</xsl:attribute>
					</TotalFare>
					<!--ResBookDesigCodes>
						<xsl:for-each select="$classes/paxFareProduct/fareDetails">
							<xsl:for-each select="productInformation">
								<ResBookDesigCode>
									<xsl:value-of select="avlProductDetails/rbd" />
								</ResBookDesigCode>
							</xsl:for-each>
						</xsl:for-each>
					</ResBookDesigCodes-->
				</xsl:if>
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>

	<xsl:template match="recommendation" mode="flight">
		<xsl:param name="flindex"/>
		<xsl:apply-templates select="segmentFlightRef[referencingDetail[1]/refNumber = $flindex]"/>
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
		<xsl:variable name="Deci" select="substring-after(string(recPriceInfo/monetaryDetail[1]/amount),'.')" />
		<xsl:variable name="NoDeci" select="string-length($Deci)" />
		<xsl:variable name="TotalAmount" select="translate(string(recPriceInfo/monetaryDetail[1]/amount),'.,','')" />
		<xsl:variable name="TaxTotal">
			<xsl:choose>
				<xsl:when test="recPriceInfo/monetaryDetail[2]/amount">
					<xsl:value-of select="translate(string(recPriceInfo/monetaryDetail[2]/amount),'.','')" />
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<ItinTotalFare>
			<xsl:if test="paxFareProduct/fareDetails/productInformation[1]/fareProductDetail/fareType='RN'">
				<xsl:attribute name="NegotiatedFareCode" />
			</xsl:if>
			<!--BaseFare>
				<xsl:attribute name="Amount">
					<xsl:value-of select="$TotalAmount - $TaxTotal" />
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="../conversionRate/conversionRateDetail/currency" />
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="$NoDeci" />
				</xsl:attribute>
			</BaseFare>
			<Taxes>
				<Tax>
					<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TaxTotal" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="../conversionRate/conversionRateDetail/currency" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$NoDeci" />
					</xsl:attribute>
				</Tax>
			</Taxes-->
			<TotalFare>
				<xsl:attribute name="Amount">
					<xsl:value-of select="$TotalAmount" />
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="../conversionRate/conversionRateDetail/currency" />
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:value-of select="$NoDeci" />
				</xsl:attribute>
			</TotalFare>
		</ItinTotalFare>
	</xsl:template>
	
	<xsl:template match="segmentFlightRef">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="ref"><xsl:value-of select="referencingDetail[2]/refNumber"/></xsl:variable>
		<xsl:variable name="Deci" select="substring-after(string(../recPriceInfo/monetaryDetail[1]/amount),'.')" />
		<xsl:variable name="NoDeci" select="string-length($Deci)" />
		<xsl:variable name="Cur" select="../../conversionRate/conversionRateDetail/currency"/>
		<xsl:variable name="TotalAmount" select="translate(string(../recPriceInfo/monetaryDetail[1]/amount),'.,','')" />
		<xsl:variable name="classes" select="../paxFareProduct[1]"/>
		<!--xsl:variable name="classes">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails">
				<xsl:for-each select="productInformation">
					<xsl:value-of select="avlProductDetails/rbd" />
				</xsl:for-each>
			</xsl:for-each>
		</xsl:variable-->
		<xsl:apply-templates select="../../flightIndex[2]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]" mode="segs">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="odpos">2</xsl:with-param>
			<xsl:with-param name="Cur"><xsl:value-of select="$Cur"/></xsl:with-param>
			<xsl:with-param name="NoDeci"><xsl:value-of select="$NoDeci"/></xsl:with-param>
			<xsl:with-param name="TotalAmount"><xsl:value-of select="$TotalAmount"/></xsl:with-param>
			<xsl:with-param name="classes"><xsl:copy-of select="$classes"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="groupOfFlights" mode="segs">
		<xsl:param name="pos" />
		<xsl:param name="odpos" />
		<xsl:param name="Cur" />
		<xsl:param name="NoDeci" />
		<xsl:param name="TotalAmount" />
		<xsl:param name="classes" />
		<xsl:apply-templates select="flightDetails">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="odpos">2</xsl:with-param>
			<xsl:with-param name="Cur"><xsl:value-of select="$Cur"/></xsl:with-param>
			<xsl:with-param name="NoDeci"><xsl:value-of select="$NoDeci"/></xsl:with-param>
			<xsl:with-param name="TotalAmount"><xsl:value-of select="$TotalAmount"/></xsl:with-param>
			<xsl:with-param name="classes"><xsl:copy-of select="$classes"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>

</xsl:stylesheet>
