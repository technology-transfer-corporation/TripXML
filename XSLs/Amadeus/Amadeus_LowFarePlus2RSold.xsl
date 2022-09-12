<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ======================================================================= -->
	<!-- Author  : Rastko Ilic										                                           -->
	<!-- Date    : 16 Sep 2004									                                                         -->
	<!-- ======================================================================= -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="MessagesOnly_Reply">
				<OTA_AirLowFareSearchPlusRS Version="1.001">
					<Errors>
						<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages" mode="Error" />
					</Errors>
				</OTA_AirLowFareSearchPlusRS>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="PoweredLowestFare_SearchReply" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="CAPI_Messages" mode="Error">
		<Error Type="Amadeus">
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<xsl:template match="PoweredLowestFare_SearchReply">
		<OTA_AirLowFareSearchPlusRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="errorMessage">
					<Errors>
						<Error Type="Amadeus">
							<xsl:attribute name="Code">
								<xsl:value-of select="errorMessage/applicationError/applicationErrorDetail/error" />
							</xsl:attribute>
							<xsl:value-of select="errorMessage/errorMessageDetail/description"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="not(recommendation)">
					<Errors>
						<Error Type="Traveltalk">
							<xsl:attribute name="Code">0001</xsl:attribute>
							<xsl:text>No selection matches search criteria</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="recommendation"/>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchPlusRS>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="recommendation">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="itemNumber/itemNumberId/number" />
			</xsl:attribute>
			<AirItinerary>
				<OriginDestinationOptions>
					<xsl:apply-templates select="segmentFlightRef"/>
				</OriginDestinationOptions>
			</AirItinerary>
			<AirItineraryPricingInfo>
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
					<xsl:choose>
						<xsl:when test="paxFareProduct/fareDetails/productInformation[1]/fareProductDetail/fareType='RP'">
							<xsl:attribute name="NegotiatedFare">0</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="NegotiatedFare">1</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<!--Come back here - need to map NegotiatedFareCode -->
					<xsl:if test="paxFareProduct/fareDetails/productInformation[1]/fareProductDetail/fareType='RN'">
						<xsl:attribute name="NegotiatedFareCode" />
					</xsl:if>
					<BaseFare>
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
				<PTC_FareBreakdowns>
					<xsl:apply-templates select="paxFareProduct" />
				</PTC_FareBreakdowns>
				<!--FareInfos>
					<xsl:apply-templates select="Product/ProductFlightSegment[position()='1']" mode="FareRule"/>
				</FareInfos-->
			</AirItineraryPricingInfo>
		</PricedItinerary>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- 											 							   -->
	<!--****************************************************************************************-->
	<xsl:template match="paxFareProduct">
		<PTC_FareBreakdown>
			<xsl:variable name="TaxAmountPTC">
				<xsl:value-of select="translate(string(paxFareDetail/totalTaxAmount),'.','')" />
			</xsl:variable>
			<xsl:variable name="TotalAmountPTC">
				<xsl:value-of select="translate(string(paxFareDetail/totalFareAmount),'.','')" />
			</xsl:variable>
			<xsl:variable name="NumberOfPax">
				<xsl:value-of select="count(paxReference/traveller)" />
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="fareDetails/productInformation/fareProductDetail/fareType='RN'">
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="paxReference/ptc" />
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="count(paxReference/traveller)" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:apply-templates select="fareDetails/productInformation/fareProductDetail/fareBasis" mode="FareBasis" />
			</FareBasisCodes>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="($TotalAmountPTC - $TaxAmountPTC) * $NumberOfPax" />
					</xsl:attribute>
				</BaseFare>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC * $NumberOfPax" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
		</PTC_FareBreakdown>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="fareBasis" mode="FareBasis">
		<FareBasisCode>
			<xsl:value-of select="." />
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
	<xsl:template match="segmentFlightRef">
		<OriginDestinationOption>
			<xsl:apply-templates select="RPH"/>
		</OriginDestinationOption>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="RPH">
		<xsl:apply-templates select="flightDetails">
			<xsl:with-param name="pos"><xsl:value-of select="position()"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="flightDetails">
		<xsl:param name="pos" />
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
			<xsl:variable name="zeros">000</xsl:variable>
			<xsl:attribute name="DepartureDateTime">20<xsl:value-of select="$depyear" />-<xsl:value-of select="$depmonth" />-<xsl:value-of select="$depday" />T<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfDeparture),1,2)" />:<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfDeparture),3,2)" />:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">20<xsl:value-of select="$arryear" />-<xsl:value-of select="$arrmonth" />-<xsl:value-of select="$arrday" />T<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfArrival),1,2)" />:<xsl:value-of select="substring(string(flightInformation/productDateTime/timeOfArrival),3,2)" />:00</xsl:attribute>
			<!--xsl:attribute name="StopQuantity"><xsl:value-of select="AdditionalDetails/FlightDetails/NumberOfStops"/></xsl:attribute-->
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="flightInformation/flightNumber" />
			</xsl:attribute>
			<xsl:variable name="classes">
				<xsl:for-each select="../../../paxFareProduct[1]/fareDetails">
					<xsl:for-each select="productInformation">
						<xsl:value-of select="avlProductDetails/rbd" />
					</xsl:for-each>
				</xsl:for-each>
			</xsl:variable>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="substring($classes,position(),1)" />
			</xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="count(../../../paxFareProduct/paxReference[ptc != 'INF']/traveller)" /></xsl:attribute>
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
			<xsl:if test="flightInformation/companyId/marketingCarrier != flightInformation/companyId/operatingCarrier">
				<OperatingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="flightInformation/companyId/operatingCarrier" />
					</xsl:attribute>
				</OperatingAirline>
			</xsl:if>
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
		</FlightSegment>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="ProductFlightSegment" mode="FareRule">
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<xsl:apply-templates select="ProductFareDetails">
			<xsl:with-param name="pos1">
				<xsl:value-of select="$pos" />
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	<xsl:template match="ProductFareDetails">
		<xsl:param name="pos1" />
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
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
			<!--Couldn't find negotiated fare code in response - research more -->
			<!--xsl:attribute name="NegotiatedFareCode"></xsl:attribute-->
			<xsl:variable name="depdayfarerule">
				<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureDate),1,2)" />
			</xsl:variable>
			<xsl:variable name="depmonthfarerule">
				<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureDate),3,2)" />
			</xsl:variable>
			<xsl:variable name="depyearfarerule">
				<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureDate),5,2)" />
			</xsl:variable>
			<DepartureDate>20<xsl:value-of select="$depyearfarerule" />-<xsl:value-of select="$depmonthfarerule" />-<xsl:value-of select="$depdayfarerule" />T<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureTime),1,2)" />:<xsl:value-of select="substring(string(../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Schedule/DepartureTime),3,2)" />:00</DepartureDate>
			<FareReference>
				<xsl:value-of select="../../../Product/ProductFlightSegment[position()=$pos]/ProductFareDetails/Fare/FareInformation/FareBasis" />
			</FareReference>
			<!--FareReference><xsl:value-of select="Fare/FareInformation/FareBasis"/></FareReference-->
			<RuleInfo>
				<xsl:apply-templates select="../../Warning/AppendedMessage[Qualification/Type = 'PEN']" mode="Penalty" />
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/Airline/Code" />
				</xsl:attribute>
			</FilingAirline>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/DepartureAirport/Code" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../../../RecommendedSegment[position()=$pos1]/FlightSegment[position()=$pos]/Description/ArrivalAirport/Code" />
				</xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- Tax 																    -->
	<!--****************************************************************************************-->
	<xsl:template match="Details">
		<xsl:variable name="TaxDeci" select="substring-after(string(../../TotalPrice/Money[1]/Amount),'.')" />
		<xsl:variable name="TaxDeci2" select="string-length($TaxDeci)" />
		<Tax>
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="Country" />
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(string(Amount),'.','')" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../CurrencyDetails/Currency/Code" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$TaxDeci2" />
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- Fare Rules - Penalty													    -->
	<!--****************************************************************************************-->
	<xsl:template match="AppendedMessage" mode="Penalty">
		<xsl:if test="Qualification/Code='73'">
			<ChargesRules>
				<VoluntaryChanges>
					<Penalty>
						<xsl:variable name="Deci" select="substring-after(string(../Penalty/Money/Amount),'.')" />
						<xsl:variable name="NoDeci" select="string-length($Deci)" />
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(string(../Penalty/Money/Amount),'.','')" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="../Penalty/Money/CurrencyCode" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$NoDeci" />
						</xsl:attribute>
					</Penalty>
				</VoluntaryChanges>
			</ChargesRules>
		</xsl:if>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="FlightSegment" mode="TicketingInfo">
		<xsl:if test="Description/Indicator/CodeShareAndElectronicTicket='ET'">
			<TicketingInfo>
				<xsl:attribute name="TicketType">eTicket</xsl:attribute>
			</TicketingInfo>
		</xsl:if>
		<xsl:if test="Description/Indicator/CodeShareAndElectronicTicket='EN'">
			<TicketingInfo>
				<xsl:attribute name="TicketType">eTicket</xsl:attribute>
			</TicketingInfo>
		</xsl:if>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="Code" mode="depterm">
		<Terminal>
			<xsl:value-of select="." />
		</Terminal>
	</xsl:template>
	<xsl:template match="Code" mode="arrterm">
		<Terminal>
			<xsl:value-of select="." />
		</Terminal>
	</xsl:template>
	<!--****************************************************************************************-->
</xsl:stylesheet>
