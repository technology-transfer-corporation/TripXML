<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFareScheduleRS.xsl 											-->
	<!-- ================================================================== -->
  <!-- Date: 07 May 2013 - Lakshitha	- added <xsl:choose> tags to OperatingAirline to fix WTP / Inventor issue			-->
	<!-- Date: 17 Aug 2012 - Rastko - added specific decoding for Kulula airline			-->
	<!-- Date: 27 Oct 2011 - Rastko - added support for exception error handling			-->
	<!-- Date: 04 Apr 2011 - Rastko - added extra test for mapping WEB fares			-->
	<!-- Date: 13 Mar 2011 - Rastko - removed test elements							-->
	<!-- Date: 19 Jan 2011 - Rastko	- added W web fares mapping			 		-->
	<!-- Date: 19 Jul 2010 - Rastko	- added mapping for NegotiatedFareCode attribute 		-->
	<!-- Date: 23 Jun 2010 - Rastko	- added cabin info 								-->
  
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_MasterPricerTravelBoardSearchReply" />
		<xsl:apply-templates select="Errors/Error" />
		<xsl:apply-templates select="Error" />
	</xsl:template>

	<xsl:template match="Error">
		<OTA_AirLowFareSearchScheduleRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
			<Errors>
				<Error Type="Amadeus">
					<xsl:value-of select="."/>
				</Error>
			</Errors>
		</OTA_AirLowFareSearchScheduleRS>
	</xsl:template>

	<xsl:template match="Fare_MasterPricerTravelBoardSearchReply">
		<OTA_AirLowFareSearchScheduleRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier">Amadeus</xsl:attribute>
			<xsl:choose>
				<xsl:when test="errorMessage">
					<Errors>
						<Error Type="Amadeus">
							<xsl:attribute name="Code">
								<xsl:value-of select="errorMessage/applicationError/applicationErrorDetail/error" />
							</xsl:attribute>
							<xsl:value-of select="errorMessage/errorMessageText/description"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="flightIndex[1]/groupOfFlights" mode="start"/>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirLowFareSearchScheduleRS>
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
						<xsl:variable name="classes">
							<xsl:apply-templates select="../../recommendation[segmentFlightRef/referencingDetail[1]/refNumber = $flindex]" mode="classes"/>
						</xsl:variable>
						<xsl:variable name="cabins">
							<xsl:apply-templates select="../../recommendation[segmentFlightRef/referencingDetail[1]/refNumber = $flindex]" mode="cabins"/>
						</xsl:variable>
						<xsl:apply-templates select="flightDetails">
							<xsl:with-param name="pos">1</xsl:with-param>
							<xsl:with-param name="odpos">1</xsl:with-param>
							<xsl:with-param name="Deci"></xsl:with-param>
							<xsl:with-param name="NoDeci"></xsl:with-param>
							<xsl:with-param name="TotalAmount"></xsl:with-param>
							<xsl:with-param name="TaxTotal"></xsl:with-param>
							<xsl:with-param name="classes"></xsl:with-param>
							<xsl:with-param name="prevclasses"></xsl:with-param>
							<xsl:with-param name="cabins"></xsl:with-param>
							<xsl:with-param name="prevcabins"></xsl:with-param>
							<xsl:with-param name="flindex"><xsl:value-of select="$flindex"/></xsl:with-param>
							<xsl:with-param name="pricesource"></xsl:with-param>
							<xsl:with-param name="fbc"></xsl:with-param>
							<xsl:with-param name="vc"></xsl:with-param>
							<xsl:with-param name="negocode"></xsl:with-param>
						</xsl:apply-templates>
					</OriginDestinationOption>
					<OriginDestinationOption>
						<xsl:apply-templates select="../../recommendation/segmentFlightRef[referencingDetail[1]/refNumber = $flindex]"/>
					</OriginDestinationOption>
				</OriginDestinationOptions>
			</AirItinerary>
			<!--AirItineraryPricingInfo>
				<xsl:apply-templates select="../../recommendation[segmentFlightRef/referencingDetail[1]/refNumber = $flindex][1]" mode="fare"/>
			</AirItineraryPricingInfo-->
			<TicketingInfo>
				<xsl:apply-templates select="../../recommendation[segmentFlightRef/referencingDetail[1]/refNumber = $flindex]" mode="ticketlimit"/>
			</TicketingInfo>
		</PricedItinerary>
	</xsl:template>

	<xsl:template match="flightDetails">
		<xsl:param name="pos" />
		<xsl:param name="odpos" />
		<xsl:param name="Cur" />
		<xsl:param name="NoDeci" />
		<xsl:param name="TotalAmount" />
		<xsl:param name="TaxTotal" />
		<xsl:param name="classes" />
		<xsl:param name="prevclasses" />
		<xsl:param name="cabins" />
		<xsl:param name="prevcabins" />
		<xsl:param name="flindex" />
		<xsl:param name="pricesource"/>
		<xsl:param name="fbc"/>
		<xsl:param name="vc"/>
		<xsl:param name="negocode"/>
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
			<xsl:if test="flightInformation/productDetail/techStopNumber!=''">
				<xsl:attribute name="StopQuantity"><xsl:value-of select="flightInformation/productDetail/techStopNumber"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$pos" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="flightInformation/flightNumber" />
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:choose>
					<xsl:when test="$TotalAmount!=''">
						<xsl:value-of select="substring($classes,$posit,1)"/>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="$classes"/></xsl:otherwise>
				</xsl:choose>
				<!--xsl:value-of select="$classes/fareDetails[position()=$odpos]/productInformation[position()=$posit]/avlProductDetails/rbd" /-->
			</xsl:attribute>
			<xsl:attribute name="NumberInParty">
				<xsl:value-of select="count(../../../recommendation[1]/paxFareProduct/paxReference[ptc != 'INF' and ptc != 'IN']/traveller)" />
			</xsl:attribute>
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
      <xsl:choose>
        <xsl:when test="flightInformation/companyId/operatingCarrier != ''">
          <OperatingAirline>
            <xsl:attribute name="Code">
              <xsl:value-of select="flightInformation/companyId/operatingCarrier" />
            </xsl:attribute>
          </OperatingAirline>
        </xsl:when>
        <xsl:when test="commercialAgreement/codeshareDetails[codeShareType='S']/flightNumber!=''">
          <OperatingAirline>
            <xsl:variable name="oa">
              <xsl:value-of select="commercialAgreement/codeshareDetails[codeShareType='S']/flightNumber"/>
            </xsl:variable>
            <xsl:value-of select="../../../../companyIdText[textRefNumber=$oa]/companyText" />
          </OperatingAirline>
        </xsl:when>
      </xsl:choose>
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="flightInformation/productDetail/equipmentType" />
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="flightInformation/companyId/marketingCarrier" />
				</xsl:attribute>
				<xsl:if test="flightInformation/companyId/marketingCarrier = 'MN'"><xsl:value-of select="'Kulula'"/></xsl:if>
			</MarketingAirline>
			<TPA_Extensions>
				<xsl:if test="$TotalAmount!=''">
					<xsl:attribute name="PricingSource"><xsl:value-of select="$pricesource"/></xsl:attribute>
					<xsl:attribute name="ValidatingAirlineCode"><xsl:value-of select="$vc"/></xsl:attribute>
					<xsl:if test="$negocode!=''">
						<xsl:attribute name="NegotiatedFareCode"><xsl:value-of select="$negocode"/></xsl:attribute>
					</xsl:if>
				</xsl:if>
				<xsl:if test="$TotalAmount!=''">
					<CabinType>
						<xsl:attribute name="Cabin">
							<xsl:choose>
								<xsl:when test="substring($cabins,$posit,1) = 'F'">First</xsl:when>
								<xsl:when test="substring($cabins,$posit,1) = 'C'">Business</xsl:when>
								<xsl:when test="substring($cabins,$posit,1) = 'W'">Premium</xsl:when>
								<xsl:otherwise>Economy</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</CabinType>
				</xsl:if>
				<JourneyTotalDuration>
					<xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,1,2)"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="substring(../propFlightGrDetail/flightProposal[unitQualifier='EFT']/ref,3,4)"/>
				</JourneyTotalDuration> 
				<xsl:choose>
					<xsl:when test="$TotalAmount!=''">
						<xsl:if test="$odpos > 1">
							<TotalBaseFare>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$TotalAmount - $TaxTotal" />
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="$Cur" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci" />
								</xsl:attribute>
							</TotalBaseFare>
							<TotalTax>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$TaxTotal" />
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode">
									<xsl:value-of select="$Cur" />
								</xsl:attribute>
								<xsl:attribute name="DecimalPlaces">
									<xsl:value-of select="$NoDeci" />
								</xsl:attribute>
							</TotalTax>
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
							<xsl:call-template name="prevclasses">
								<xsl:with-param name="prevclasses"><xsl:value-of select="$prevclasses"/></xsl:with-param>
								<xsl:with-param name="prevcabins"><xsl:value-of select="$prevcabins"/></xsl:with-param>
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
	<xsl:choose>  
		<xsl:when test="paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier= 'LTD']">
			<xsl:attribute name="TicketTimeLimit">
				<xsl:text>20</xsl:text>
				<xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],6,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],3,3)" />
					</xsl:with-param>
				</xsl:call-template>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(paxFareProduct/fare/pricingMessage[freeTextQualification/textSubjectQualifier = 'LTD']/description[2],1,2)"/>
				<xsl:text>T23:59:00</xsl:text>
			</xsl:attribute>	
		</xsl:when>
		<xsl:otherwise>
			<xsl:attribute name="TicketTimeLimit">
				<xsl:text>20</xsl:text>
				<xsl:value-of   select="substring(string(../flightIndex[1]/groupOfFlights[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),5,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of  select="substring(string(../flightIndex[1]/groupOfFlights[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),3,2)" />
				<xsl:text>-</xsl:text>
				<xsl:value-of   select="substring(string(../flightIndex[1]/groupOfFlights[1]/flightDetails[1]/flightInformation/productDateTime/dateOfDeparture),1,2)"/>
				<xsl:text>T00:00:00</xsl:text>
			</xsl:attribute>
		</xsl:otherwise> 
		</xsl:choose>
		
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
			<xsl:choose>
				<xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RN'">
					<xsl:attribute name="NegotiatedFareCode"/>
				</xsl:when>
				<xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/fareProductDetail/fareType='RA'">
					<xsl:attribute name="NegotiatedFareCode">CAT35</xsl:attribute>
				</xsl:when>
				<xsl:when test="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId!=''">
					<xsl:attribute name="NegotiatedFareCode">
						<xsl:value-of select="paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId"/>
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
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
	
	<xsl:template match="recommendation" mode="fromfare">
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
		<FromTotalBaseFare>
			<xsl:attribute name="Amount">
				<xsl:value-of select="$TotalAmount - $TaxTotal" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../conversionRate/conversionRateDetail/currency" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$NoDeci" />
			</xsl:attribute>
		</FromTotalBaseFare>
		<FromTotalTax>
			<xsl:attribute name="Amount">
				<xsl:value-of select="$TaxTotal" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../conversionRate/conversionRateDetail/currency" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$NoDeci" />
			</xsl:attribute>
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
			<xsl:attribute name="Amount">
				<xsl:value-of select="$TotalAmount" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../conversionRate/conversionRateDetail/currency" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:value-of select="$NoDeci" />
			</xsl:attribute>
		</FromTotalFare>
	</xsl:template>

	
	<xsl:template match="segmentFlightRef">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="ref"><xsl:value-of select="referencingDetail[2]/refNumber"/></xsl:variable>
		<xsl:variable name="Deci" select="substring-after(string(../recPriceInfo/monetaryDetail[1]/amount),'.')" />
		<xsl:variable name="NoDeci" select="string-length($Deci)" />
		<xsl:variable name="Cur" select="../../conversionRate/conversionRateDetail/currency"/>
		<xsl:variable name="TotalAmount" select="translate(string(../recPriceInfo/monetaryDetail[1]/amount),'.,','')" />
		<xsl:variable name="TaxTotal">
			<xsl:choose>
				<xsl:when test="../recPriceInfo/monetaryDetail[2]/amount">
					<xsl:value-of select="translate(string(../recPriceInfo/monetaryDetail[2]/amount),'.','')" />
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="classes">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[2]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/rbd" />
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="prevclasses">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[1]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/rbd" />
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="cabins">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[2]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/cabin" />
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="prevcabins">
			<xsl:for-each select="../paxFareProduct[1]/fareDetails[1]/groupOfFares">
				<xsl:value-of select="productInformation/cabinProduct/cabin" />
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
					<xsl:attribute name="PassengerType">
						<xsl:choose>
							<!--xsl:when test="paxReference/ptc='CNN'">CHD</xsl:when>
							<xsl:otherwise><xsl:value-of select="paxReference/ptc"/></xsl:otherwise-->
							<xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/fareType='W'"><xsl:value-of select="'WEB'"/></xsl:when>
							<xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType = 'CH'">CHD</xsl:when>
							<xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType = 'CNN'">CHD</xsl:when>
							<xsl:when test="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType = 'YCD'">SRC</xsl:when>
							<xsl:when test="fareDetails[1]/groupOfFares/ticketInfos/additionalFareDetails/ticketDesignator='WEB'"><xsl:value-of select="'WEB'"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="fareDetails[1]/groupOfFares/productInformation/fareProductDetail/passengerType"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:for-each select="fareDetails/groupOfFares/productInformation">
						<FareBasisCode>
							<xsl:value-of select="fareProductDetail/fareBasis"/>
							<xsl:if test="../ticketInfos/additionalFareDetails/ticketDesignator!=''">
								<xsl:value-of select="concat('/',../ticketInfos/additionalFareDetails/ticketDesignator)"/>
							</xsl:if>
						</FareBasisCode>
					</xsl:for-each>
				</FareBasisCodes>
			</xsl:for-each>
		</xsl:variable>
		<xsl:variable name="vc">
			<xsl:value-of select="../paxFareProduct/paxFareDetail/codeShareDetails[transportStageQualifier='V']/company"/>
		</xsl:variable>
		<xsl:variable name="negocode">
			<xsl:if test="../paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId!=''">
				<xsl:value-of select="../paxFareProduct/fareDetails/groupOfFares/productInformation[1]/corporateId"/>
			</xsl:if>
		</xsl:variable>
		<xsl:apply-templates select="../../flightIndex[2]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]" mode="segs">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="odpos">2</xsl:with-param>
			<xsl:with-param name="Cur"><xsl:value-of select="$Cur"/></xsl:with-param>
			<xsl:with-param name="NoDeci"><xsl:value-of select="$NoDeci"/></xsl:with-param>
			<xsl:with-param name="TotalAmount"><xsl:value-of select="$TotalAmount"/></xsl:with-param>
			<xsl:with-param name="TaxTotal"><xsl:value-of select="$TaxTotal"/></xsl:with-param>
			<xsl:with-param name="classes"><xsl:value-of select="$classes"/></xsl:with-param>
			<xsl:with-param name="prevclasses"><xsl:value-of select="$prevclasses"/></xsl:with-param>
			<xsl:with-param name="cabins"><xsl:value-of select="$cabins"/></xsl:with-param>
			<xsl:with-param name="prevcabins"><xsl:value-of select="$prevcabins"/></xsl:with-param>
			<xsl:with-param name="pricesource"><xsl:value-of select="$pricesource"/></xsl:with-param>
			<xsl:with-param name="fbc" select="$fbc"/>
			<xsl:with-param name="vc" select="$vc"/>
			<xsl:with-param name="negocode" select="$negocode"/>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="groupOfFlights" mode="segs">
		<xsl:param name="pos" />
		<xsl:param name="odpos" />
		<xsl:param name="Cur" />
		<xsl:param name="NoDeci" />
		<xsl:param name="TotalAmount" />
		<xsl:param name="TaxTotal" />
		<xsl:param name="classes" />
		<xsl:param name="prevclasses" />
		<xsl:param name="cabins" />
		<xsl:param name="prevcabins" />
		<xsl:param name="pricesource" />
		<xsl:param name="fbc"/>
		<xsl:param name="vc"/>
		<xsl:param name="negocode"/>
		<xsl:apply-templates select="flightDetails">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
			<xsl:with-param name="odpos">2</xsl:with-param>
			<xsl:with-param name="Cur"><xsl:value-of select="$Cur"/></xsl:with-param>
			<xsl:with-param name="NoDeci"><xsl:value-of select="$NoDeci"/></xsl:with-param>
			<xsl:with-param name="TotalAmount"><xsl:value-of select="$TotalAmount"/></xsl:with-param>
			<xsl:with-param name="TaxTotal"><xsl:value-of select="$TaxTotal"/></xsl:with-param>
			<xsl:with-param name="classes"><xsl:value-of select="$classes"/></xsl:with-param>
			<xsl:with-param name="prevclasses"><xsl:value-of select="$prevclasses"/></xsl:with-param>
			<xsl:with-param name="cabins"><xsl:value-of select="$cabins"/></xsl:with-param>
			<xsl:with-param name="prevcabins"><xsl:value-of select="$prevcabins"/></xsl:with-param>
			<xsl:with-param name="flindex"></xsl:with-param>
			<xsl:with-param name="pricesource"><xsl:value-of select="$pricesource"/></xsl:with-param>
			<xsl:with-param name="fbc" select="$fbc"/>
			<xsl:with-param name="vc" select="$vc"/>
			<xsl:with-param name="negocode" select="$negocode"/>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template name="prevclasses">
		<xsl:param name="prevclasses"/>
		<xsl:param name="prevcabins"/>
		<xsl:param name="rph"/>
		<xsl:if test="string-length($prevclasses) > 0">
			<OriginClass>
				<xsl:attribute name="Index"><xsl:value-of select="$rph"/></xsl:attribute>
				<xsl:attribute name="Cabin">
					<xsl:choose>
						<xsl:when test="substring($prevcabins,1,1) = 'F'">First</xsl:when>
						<xsl:when test="substring($prevcabins,1,1) = 'C'">Business</xsl:when>
						<xsl:when test="substring($prevcabins,1,1) = 'W'">Premium</xsl:when>
						<xsl:otherwise>Economy</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="substring($prevclasses,1,1)"/>
			</OriginClass>
			<xsl:call-template name="prevclasses">
				<xsl:with-param name="prevclasses"><xsl:value-of select="substring($prevclasses,2)"/></xsl:with-param>
				<xsl:with-param name="prevcabins"><xsl:value-of select="substring($prevcabins,2)"/></xsl:with-param>
				<xsl:with-param name="rph"><xsl:value-of select="$rph + 1"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
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
