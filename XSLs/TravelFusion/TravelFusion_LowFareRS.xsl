<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- TravelFusion_LowFareRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 29 May 2012 - Shashin - added EquivFare									-->	
	<!-- Date: 21 Mar 2012 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CommandList" />
	</xsl:template>

	<xsl:template match="CommandList">
		<OTA_AirLowFareSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">TravelFusion</xsl:attribute>
			<xsl:choose>
				<xsl:when test="not(CheckRouting/RouterList/Router/GroupList)">
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
						<xsl:apply-templates select="CheckRouting/RouterList/Router/GroupList"/>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="GroupList">
		<xsl:for-each select="Group/OutwardList/Outward">
			<xsl:variable name="outward" select="."/>
			<xsl:for-each select="../../ReturnList/Return">
				<PricedItinerary>
					<xsl:attribute name="SequenceNumber">
						<xsl:value-of select="position()" />
					</xsl:attribute>
					<AirItinerary>
						<xsl:attribute name="DirectionInd">
							<xsl:choose>
								<xsl:when test="../../ReturnList">Circle</xsl:when>
								<xsl:otherwise>OneWay</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<OriginDestinationOptions>
							<xsl:apply-templates select="$outward/SegmentList"/>
							<xsl:apply-templates select="SegmentList"/>
						</OriginDestinationOptions>
					</AirItinerary>
					<AirItineraryPricingInfo>
						<xsl:attribute name="PricingSource">Published</xsl:attribute>
						<xsl:attribute name="ValidatingAirlineCode">
							<xsl:value-of select="SegmentList/Segment[1]/VendingOperator/Code"/>
						</xsl:attribute>
						<xsl:variable name="Deci" select="substring-after(string(Price/Amount),'.')" />
						<xsl:variable name="NoDeci" select="string-length($Deci)" />
						<xsl:variable name="TotalAmount" select="translate(string(Price/Amount),'.,','')" />
						<xsl:variable name="TotOutward" select="translate(string($outward/Price/Amount),'.','')"/>
						<xsl:variable name="TaxTotal">
							<xsl:variable name="tax">
								<xsl:value-of select="sum(Price/TaxItemList/TaxItem/Amount)"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="TaxTotalOutward">
							<xsl:variable name="tax">
								<xsl:value-of select="sum($outward/Price/TaxItemList/TaxItem/Amount)"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<ItinTotalFare>
							<BaseFare>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$TotalAmount + $TotOutward - $TaxTotal - $TaxTotalOutward" />
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="Price/Currency" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci" />
								</xsl:attribute>
							</BaseFare>
							<Taxes>
								<Tax>
									<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
									<xsl:attribute name="Amount">
										<xsl:value-of select="$TaxTotal + $TaxTotalOutward" />
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="Price/Currency" />
									</xsl:attribute>
									<xsl:attribute name="DecimalPlaces">
										<xsl:value-of select="$NoDeci" />
									</xsl:attribute>
								</Tax>
							</Taxes>
							<TotalFare>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$TotalAmount + $TotOutward" />
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="Price/Currency" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci" />
								</xsl:attribute>
							</TotalFare>
							<EquivFare>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$TotalAmount + $TotOutward" />
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="Price/Currency" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci" />
								</xsl:attribute>
							</EquivFare>
						</ItinTotalFare>
						<PTC_FareBreakdowns>
							<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='30'][1]">
								<xsl:with-param name="age">30</xsl:with-param>
								<xsl:with-param name="outward" select="$outward"/>
							</xsl:apply-templates> 
							<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='7'][1]">
								<xsl:with-param name="age">7</xsl:with-param>
								<xsl:with-param name="outward" select="$outward"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="Price/PassengerPriceList/PassengerPrice[Age='0'][1]">
								<xsl:with-param name="age">0</xsl:with-param>
								<xsl:with-param name="outward" select="$outward"/>
							</xsl:apply-templates>
						</PTC_FareBreakdowns>
					</AirItineraryPricingInfo>
					<Notes><xsl:value-of select="'RoutingId'"/></Notes>
					<Notes><xsl:value-of select="concat('OutwardId:',$outward/Id)"/></Notes>
					<Notes><xsl:value-of select="concat('ReturnId:',Id)"/></Notes>
					<TicketingInfo>
						<xsl:variable name="depday">
							<xsl:value-of select="substring($outward/SegmentList/Segment/DepartDate,1,2)" />
						</xsl:variable>
						<xsl:variable name="depmonth">
							<xsl:value-of select="substring($outward/SegmentList/Segment/DepartDate,4,2)" />
						</xsl:variable>
						<xsl:variable name="depyear">
							<xsl:value-of select="substring($outward/SegmentList/Segment/DepartDate,7,4)" />
						</xsl:variable>
						<xsl:attribute name="TicketTimeLimit">
							<xsl:value-of select="$depyear" />-<xsl:value-of select="$depmonth" />-<xsl:value-of select="$depday" />T<xsl:value-of select="substring-after($outward/SegmentList/Segment/DepartDate,'-')" />
							<xsl:text>:00</xsl:text>
						</xsl:attribute>
					</TicketingInfo>
				</PricedItinerary>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- 											 							   -->
	<!--****************************************************************************************-->
	<xsl:template match="fareDetails" mode="FareRule">
		<xsl:variable name="ref"><xsl:value-of select="segmentRef/segRef"/></xsl:variable>
		<xsl:apply-templates select="groupOfFares/productInformation" mode="FareRule">
			<xsl:with-param name="ref"><xsl:value-of select="$ref"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="PassengerPrice">
		<xsl:param name="age"/>
		<xsl:param name="outward"/>
		<PTC_FareBreakdown>
			<xsl:variable name="Deci" select="substring-after(string(Amount),'.')" />
			<xsl:variable name="NoDeci" select="string-length($Deci)" />
			<xsl:variable name="TaxAmountPTC">
				<xsl:variable name="tax">
					<xsl:value-of select="sum(../PassengerPrice[Age=$age]/TaxItemList/TaxItem/Amount)"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="TaxAmountPTCOutward">
				<xsl:variable name="tax">
					<xsl:value-of select="sum($outward/Price/PassengerPriceList/PassengerPrice[Age=$age]/TaxItemList/TaxItem/Amount)"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$NoDeci=0"><xsl:value-of select="$tax"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="translate(format-number($tax,'#.00'),'.,','')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="TotalAmountPTC">
				<xsl:value-of select="translate(format-number(sum(../PassengerPrice[Age=$age]/Amount),'#.00'),'.','')" />
			</xsl:variable>
			<xsl:variable name="TotalAmountPTCOutward">
				<xsl:value-of select="translate(format-number(sum($outward/Price/PassengerPriceList/PassengerPrice[Age=$age]/Amount),'#.00'),'.','')" />
			</xsl:variable>
			<xsl:variable name="NumberOfPax">
				<xsl:value-of select="count(../PassengerPrice[Age=$age])" />
			</xsl:variable>
			<xsl:attribute name="PricingSource">Published</xsl:attribute>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="$age='30'">ADT</xsl:when>
						<xsl:when test="$age = '7'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="INF" /></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="$NumberOfPax" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC + $TotalAmountPTCOutward - $TaxAmountPTC - $TaxAmountPTCOutward" />
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$TaxAmountPTC + $TaxAmountPTCOutward" />
						</xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$TotalAmountPTC + $TotalAmountPTCOutward" />
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<PricedCode>
					<xsl:choose>
						<xsl:when test="$age='30'">ADT</xsl:when>
						<xsl:when test="$age = '7'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="INF" /></xsl:otherwise>
					</xsl:choose>
				</PricedCode>
			</TPA_Extensions>
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
	<xsl:template match="SegmentList">
		<xsl:variable name="odpos"><xsl:value-of select="position()"/></xsl:variable>
		<OriginDestinationOption>
			<xsl:attribute name="SectorSequence"><xsl:value-of select="$odpos"/></xsl:attribute>
			<xsl:apply-templates select="Segment">
				<xsl:with-param name="pos">1</xsl:with-param>
				<xsl:with-param name="odpos"><xsl:value-of select="$odpos"/></xsl:with-param>
			</xsl:apply-templates>
		</OriginDestinationOption>
	</xsl:template>
	
	<xsl:template match="Segment">
		<xsl:param name="pos" />
		<xsl:param name="odpos" />
		<xsl:variable name="posit"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="depday">
			<xsl:value-of select="substring(DepartDate,1,2)" />
		</xsl:variable>
		<xsl:variable name="depmonth">
			<xsl:value-of select="substring(DepartDate,4,2)" />
		</xsl:variable>
		<xsl:variable name="depyear">
			<xsl:value-of select="substring(DepartDate,7,4)" />
		</xsl:variable>
		<xsl:variable name="arrday">
			<xsl:value-of select="substring(ArriveDate,1,2)" />
		</xsl:variable>
		<xsl:variable name="arrmonth">
			<xsl:value-of select="substring(ArriveDate,4,2)" />
		</xsl:variable>
		<xsl:variable name="arryear">
			<xsl:value-of select="substring(ArriveDate,7,4)" />
		</xsl:variable>
		<FlightSegment>
			<xsl:variable name="zeros">000</xsl:variable>
			<xsl:attribute name="DepartureDateTime"><xsl:value-of select="$depyear" />-<xsl:value-of select="$depmonth" />-<xsl:value-of select="$depday" />T<xsl:value-of select="substring-after(DepartDate,'-')" />:00</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime"><xsl:value-of select="$arryear" />-<xsl:value-of select="$arrmonth" />-<xsl:value-of select="$arrday" />T<xsl:value-of select="substring-after(ArriveDate,'-')" />:00</xsl:attribute>
			<xsl:if test="flightInformation/productDetail/techStopNumber!=''">
				<xsl:attribute name="StopQuantity"><xsl:value-of select="flightInformation/productDetail/techStopNumber"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="FlightId/Number" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:choose>
					<xsl:when test="string-length(TravelClass/SupplierClass)=1"><xsl:value-of select="TravelClass/SupplierClass"/></xsl:when>
					<xsl:otherwise>Y</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="NumberInParty"><xsl:value-of select="count(../../Price/PassengerPriceList/PassengerPrice[Age!='0'])" /></xsl:attribute>
			<xsl:attribute name="E_TicketEligibility">Eligible</xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="Origin/Code" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="Destination/Code" />
				</xsl:attribute>
			</ArrivalAirport>
			<OperatingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="Operator/Code" />
				</xsl:attribute>
			</OperatingAirline>
			<Equipment>
				<xsl:attribute name="AirEquipType">JET</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="VendingOperator/Code" />
				</xsl:attribute>
			</MarketingAirline>
			<TPA_Extensions>
				<xsl:variable name="cabin">
					<xsl:value-of select="TravelClass/TfClass" />
				</xsl:variable>
				<CabinType>
					<xsl:attribute name="Cabin">
						<xsl:choose>
							<xsl:when test="$cabin = 'F'">First</xsl:when>
							<xsl:when test="$cabin = 'C'">Business</xsl:when>
							<xsl:when test="$cabin = 'W'">Premium</xsl:when>
							<xsl:when test="$cabin = 'Economy With Restrictions'">Economy</xsl:when>
							<xsl:otherwise>Economy</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</CabinType>
				<xsl:variable name="jt">
					<xsl:value-of select="../../Duration" />
				</xsl:variable>
				<xsl:variable name="hours">
					<xsl:choose>
						<xsl:when test="substring-before(($jt div 60),'.')=''">
							<xsl:value-of select="$jt div 60" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring-before(($jt div 60),'.')" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="minutes">
					<xsl:value-of select="$jt - ($hours*60)" />
				</xsl:variable>
				<JourneyTotalDuration>
					<xsl:choose>
						<xsl:when test="$minutes = 'NaN'">
							<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
							<xsl:value-of select="$hours" />
							<xsl:text>:00</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring(string($zeros),1,2-string-length($hours))" />
							<xsl:value-of select="$hours" />:<xsl:value-of select="substring(string($zeros),1,2-string-length($minutes))" />
							<xsl:value-of 	select="$minutes" />
						</xsl:otherwise>
					</xsl:choose>
				</JourneyTotalDuration> 
			</TPA_Extensions>
		</FlightSegment>
	</xsl:template>
	<!--****************************************************************************************-->
	<xsl:template match="productInformation" mode="FareRule">
		<xsl:param name="ref"/>
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<FareInfo>
			<xsl:variable name="depdayfarerule">
				<xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/dateOfDeparture),1,2)"/>
			</xsl:variable>
			<xsl:variable name="depmonthfarerule">
				<xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/dateOfDeparture),3,2)"/>
			</xsl:variable>
			<xsl:variable name="depyearfarerule">
				<xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/dateOfDeparture),5,2)"/>
			</xsl:variable>
			<DepartureDate>
				<xsl:text>20</xsl:text>
				<xsl:value-of select="$depyearfarerule"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="$depmonthfarerule"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="$depdayfarerule"/>
				<xsl:text>T</xsl:text>
				<xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/timeOfDeparture),1,2)"/>
				<xsl:text>:</xsl:text>
				<xsl:value-of select="substring(string(../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/productDateTime/timeOfDeparture),3,2)"/>
				<xsl:text>:00</xsl:text>
			</DepartureDate>
			<FareReference>
				<xsl:value-of select="fareProductDetail/fareBasis" />
			</FareReference>
			<RuleInfo>
				<xsl:apply-templates select="../../../fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']" mode="Advance"/> 
				<xsl:apply-templates select="../../../fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'PEN']" mode="Penalty"/>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/companyId/marketingCarrier" />
				</xsl:attribute>
			</FilingAirline>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/location[1]/locationId" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="../../../../segmentFlightRef/OD[position()=$ref]/flightDetails[position()=$pos]/flightInformation/location[2]/locationId" />
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
	<xsl:template match="pricingMessage" mode="Penalty">
		<xsl:choose>
			<xsl:when test="freeTextQualification/informationType = '70' or freeTextQualification/informationType = '71' or freeTextQualification/informationType = '73'">
				<ChargesRules>
					<VoluntaryChanges>
						<Penalty>									 
							<xsl:attribute name="PenaltyType">
								<xsl:value-of select="description"/>
							</xsl:attribute>				
						</Penalty> 
					</VoluntaryChanges>
				</ChargesRules>
			</xsl:when>
			<xsl:when test="freeTextQualification/informationType = '72'">
				<ChargesRules>
					<VoluntaryChanges>
						<Penalty>									 
							<xsl:variable name="Deci" select="substring-after(string(../monetaryInformation/monetaryDetail/amount),'.')"/>
							<xsl:variable name="NoDeci" select="string-length($Deci)"/>		
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(string(../monetaryInformation/monetaryDetail/amount),'.','')"/>
							</xsl:attribute>									
							<xsl:attribute name="CurrencyCode">					
								<xsl:value-of select="../monetaryInformation/monetaryDetail/currency"/>
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
	<!-- Fare Rules - Advance Purchase										    -->
	<!--****************************************************************************************-->
	<xsl:template match = "pricingMessage" mode="Advance">
		<xsl:choose>
			<xsl:when test="freeTextQualification/informationType = '40'">
				<!--ResTicketingRules>
					<AdvResTicketing>
						<AdvTicketing>
							<xsl:for-each select="description">
								<xsl:value-of select="."/>
								<xsl:value-of select="string(' ')"/>
							</xsl:for-each>
						</AdvTicketing> 
					</AdvResTicketing>
				</ResTicketingRules-->
			</xsl:when>
		</xsl:choose>
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
