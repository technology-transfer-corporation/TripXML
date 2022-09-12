<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- ================================================================== -->
	<!-- Markups_LowFareRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 04 Jul 2013 - Rastko - added missing Notes mapping					 -->
	<!-- Date: 15 Mar 2013 - Rastko - do not show markup for siid 21 and 43			 -->
	<!-- Date: 06 Feb 2013 - Rastko - added Markup by siid								 -->
	<!-- Date: 24 Oct 2012 - Rastko - added Markup by cabin type						 -->
	<!-- Date: 22 Feb 2012 - Rastko - added Markup by departure and arrival country		 -->
	<!-- Date: 16 Feb 2012 - Rastko - corrected typo in text Total Markup				 -->
	<!-- Date: 17 Jan 2012 - Rastko - added exchange rate for GulfTravel used currencies		 -->
	<!-- Date: 30 Dec 2010 - Rastko - added exchange rate for RUB and ZAR			 -->
	<!-- Date: 09 Dec 2010 - Rastko - apply discount only when published fares and not CC surcharge -->
	<!-- Date: 01 Dec 2010 - Rastko - apply discount only when published fares			-->
	<!-- Date: 12 Nov 2010 - Rastko - apply discount only when 1 airline in itinerary			-->
	<!-- Date: 28 Oct 2010 - Rastko - corrected determination of arrival country			-->
	<!-- Date: 05 Oct 2010 - Rastko - added specific business logic for 9W				-->
	<!-- Date: 15 Sep 2010 - Rastko - corrected % markup calculation on total fare			-->
	<!-- Date: 20 Aug 2010 - Rastko - only show one total line of markup				-->
	<!-- Date: 14 Aug 2010 - Rastko - added hard coded markups						-->
	<!-- Date: 22 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="promos" select="document('Markups.xml')"/>
	<xsl:variable name="siid">
		<xsl:choose>
			<xsl:when test="OTA_AirLowFareSearchRS">
				<xsl:value-of select="OTA_AirLowFareSearchRS/SearchPromotionsResponse/OTA_AirLowFareSearchRQ/POS/TPA_Extensions/Provider/Name[. = 'Portal']/@PseudoCityCode"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="OTA_AirLowFareSearchPlusRS/SearchPromotionsResponse/OTA_AirLowFareSearchPlusRQ/POS/TPA_Extensions/Provider/Name[. = 'Portal']/@PseudoCityCode"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="username">
		<xsl:choose>
			<xsl:when test="OTA_AirLowFareSearchRS">
				<xsl:value-of select="OTA_AirLowFareSearchRS/SearchPromotionsResponse/OTA_AirLowFareSearchRQ/POS/TPA_Extensions/Provider/Userid"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="OTA_AirLowFareSearchPlusRS/SearchPromotionsResponse/OTA_AirLowFareSearchPlusRQ/POS/TPA_Extensions/Provider/Userid"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="currency">
		<xsl:choose>
			<xsl:when test="OTA_AirLowFareSearchRS">
				<xsl:value-of select="OTA_AirLowFareSearchRS/SearchPromotionsResponse/OTA_AirLowFareSearchRQ/POS/Source/@ISOCurrency"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="OTA_AirLowFareSearchPlusRS/SearchPromotionsResponse/OTA_AirLowFareSearchPlusRQ/POS/Source/@ISOCurrency"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="OTA_AirLowFareSearchRS">
				<OTA_AirLowFareSearchRS>
					<xsl:if test="OTA_AirLowFareSearchRS/@EchoToken != ''">
						<xsl:attribute name="EchoToken"><xsl:value-of select="OTA_AirLowFareSearchRS/@EchoToken" /></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<xsl:choose>
						<xsl:when test="OTA_AirLowFareSearchRS/Errors/Error != '' and OTA_AirLowFareSearchRS/Success">
							<Warnings>
								<xsl:for-each select="OTA_AirLowFareSearchRS/Errors/Error">
									<Warning>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:value-of select="." />
									</Warning>
								</xsl:for-each>
							</Warnings>
						</xsl:when>
						<xsl:when test="OTA_AirLowFareSearchRS/Errors/Error != ''">
							<Errors>
								<xsl:for-each select="OTA_AirLowFareSearchRS/Errors/Error">
									<Error>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="@Code != ''">
													<xsl:value-of select="@Code" />
												</xsl:when>
												<xsl:otherwise>E</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:value-of select="." />
									</Error>
								</xsl:for-each>
							</Errors>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="OTA_AirLowFareSearchRS/Success">
						<Success></Success>
						<PricedItineraries>
							<xsl:variable name="priceditin">
								<xsl:apply-templates select="OTA_AirLowFareSearchRS/PricedItineraries/PricedItinerary" mode="promo"/>
							</xsl:variable>
							<xsl:variable name="priceditin1">
								<xsl:apply-templates select="msxsl:node-set($priceditin)/PricedItinerary" mode="total"/>
							</xsl:variable>
							<xsl:apply-templates select="msxsl:node-set($priceditin1)/PricedItinerary" mode="final">
								<xsl:sort data-type="number" order="ascending" select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount"/>
							</xsl:apply-templates>
						</PricedItineraries>
					</xsl:if>
				</OTA_AirLowFareSearchRS>
			</xsl:when>
			<xsl:when test="OTA_AirLowFareSearchPlusRS">
				<OTA_AirLowFareSearchPlusRS>
					<xsl:if test="OTA_AirLowFareSearchPlusRS/@EchoToken != ''">
						<xsl:attribute name="EchoToken"><xsl:value-of select="OTA_AirLowFareSearchPlusRS/@EchoToken" /></xsl:attribute>
					</xsl:if>
					<xsl:attribute name="Version">1.001</xsl:attribute>
					<xsl:choose>
						<xsl:when test="OTA_AirLowFareSearchPlusRS/Errors/Error != '' and OTA_AirLowFareSearchPlusRS/Success">
							<Warnings>
								<xsl:for-each select="OTA_AirLowFareSearchRS/Errors/Error | OTA_AirLowFareSearchPlusRS/Errors/Error">
									<Warning>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:value-of select="." />
									</Warning>
								</xsl:for-each>
							</Warnings>
						</xsl:when>
						<xsl:when test="OTA_AirLowFareSearchPlusRS/Errors/Error != ''">
							<Errors>
								<xsl:for-each select="OTA_AirLowFareSearchPlusRS/Errors/Error">
									<Error>
										<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="@Code != ''">
													<xsl:value-of select="@Code" />
												</xsl:when>
												<xsl:otherwise>E</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:value-of select="." />
									</Error>
								</xsl:for-each>
							</Errors>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="OTA_AirLowFareSearchPlusRS/Success">
						<Success></Success>
						<PricedItineraries>
							<xsl:variable name="priceditin">
								<xsl:apply-templates select="OTA_AirLowFareSearchPlusRS/PricedItineraries/PricedItinerary" mode="promo"/>
							</xsl:variable>
							<xsl:variable name="priceditin1">
								<xsl:apply-templates select="msxsl:node-set($priceditin)/PricedItinerary" mode="total"/>
							</xsl:variable>
							<xsl:apply-templates select="msxsl:node-set($priceditin1)/PricedItinerary" mode="final">
								<xsl:sort data-type="number" order="ascending" select="AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount"/>
							</xsl:apply-templates>
						</PricedItineraries>
					</xsl:if>
				</OTA_AirLowFareSearchPlusRS>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="PricedItinerary" mode="promo">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber"><xsl:value-of select="@SequenceNumber"/></xsl:attribute>
			<xsl:attribute name="Provider"><xsl:value-of select="@Provider"/></xsl:attribute>
			<xsl:copy-of select="AirItinerary" />
			<xsl:variable name="itin" select="AirItinerary/OriginDestinationOptions"/>
			<xsl:apply-templates select="AirItineraryPricingInfo" mode="calc">
				<xsl:with-param name="itin" select="$itin"/>
				<xsl:with-param name="pcc"><xsl:value-of select="substring-after(@Provider,'-')"/></xsl:with-param>
			</xsl:apply-templates>
			<xsl:copy-of select="TicketingInfo" />
			<xsl:copy-of select="Notes" />
		</PricedItinerary>
	</xsl:template>
	
	<xsl:template match="PricedItinerary" mode="total">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber"><xsl:value-of select="@SequenceNumber"/></xsl:attribute>
			<xsl:attribute name="Provider"><xsl:value-of select="@Provider"/></xsl:attribute>
			<xsl:copy-of select="AirItinerary" />
			<xsl:apply-templates select="AirItineraryPricingInfo" mode="total"/>
			<xsl:copy-of select="TicketingInfo" />
			<xsl:copy-of select="Notes" />
		</PricedItinerary>
	</xsl:template>

	<xsl:template match="PricedItinerary" mode="final">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber"><xsl:value-of select="@SequenceNumber"/></xsl:attribute>
			<xsl:attribute name="Provider"><xsl:value-of select="@Provider"/></xsl:attribute>
			<xsl:copy-of select="AirItinerary" />
			<xsl:copy-of select="AirItineraryPricingInfo" />
			<xsl:copy-of select="TicketingInfo" />
			<xsl:copy-of select="Notes" />
		</PricedItinerary>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="AirItineraryPricingInfo" mode="calc">
		<xsl:param name="itin"/>
		<xsl:param name="pcc"/>
		<AirItineraryPricingInfo>
			<xsl:attribute name="PricingSource"><xsl:value-of select="@PricingSource"/></xsl:attribute>
			<xsl:if test="@ValidatingAirlineCode != ''">
				<xsl:attribute name="ValidatingAirlineCode"><xsl:value-of select="@ValidatingAirlineCode"/></xsl:attribute>
			</xsl:if>
			<xsl:variable name="PricingSource"><xsl:value-of select="@PricingSource"/></xsl:variable>
			<xsl:variable name="CurrencyCode"><xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/></xsl:variable>
			<xsl:variable name="DecimalPlaces"><xsl:value-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:variable>
			<xsl:copy-of select="ItinTotalFare"/>
			<PTC_FareBreakdowns>
				<xsl:for-each select="PTC_FareBreakdowns/PTC_FareBreakdown">
					<PTC_FareBreakdown>
						<xsl:copy-of select="PassengerTypeQuantity"/>
						<xsl:copy-of select="FareBasisCodes"/>
						<PassengerFare>
							<xsl:variable name="fees">
								<xsl:element name="Fees">
									<xsl:call-template name="markups">
										<xsl:with-param name="amount"><xsl:value-of select="PassengerFare/TotalFare/@Amount"/></xsl:with-param>
										<xsl:with-param name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:with-param>
										<xsl:with-param name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:with-param>
										<xsl:with-param name="PricingSource"><xsl:value-of select="$PricingSource"/></xsl:with-param>
										<xsl:with-param name="itin" select="$itin"/>
										<xsl:with-param name="pcc"><xsl:value-of select="$pcc"/></xsl:with-param>
										<xsl:with-param name="pax"><xsl:value-of select="PassengerTypeQuantity/@Quantity"/></xsl:with-param>
									</xsl:call-template>
								</xsl:element>
							</xsl:variable>	
							<xsl:variable name="totf">
 								<xsl:choose>
									<xsl:when test="msxsl:node-set($fees)/Fees/Fee">
										<xsl:value-of select="sum(msxsl:node-set($fees)/Fees/Fee/@Amount)"/>
									</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
 							</xsl:variable>
  							<BaseFare>
  								<xsl:variable name="bf"><xsl:value-of select="PassengerFare/BaseFare/@Amount"/></xsl:variable>
  								<xsl:attribute name="Amount">
									<xsl:value-of select="$bf + $totf"/>
  								</xsl:attribute>
  							</BaseFare>
  							<xsl:copy-of select="PassengerFare/Taxes"/>
  							<xsl:if test="msxsl:node-set($fees)/Fees/Fee and $siid!='21' and $siid!='43'">
  								<!--xsl:copy-of select="$fees"/-->
  								<xsl:element name="Fees">
									<xsl:element name="Fee">
										<xsl:attribute name="FeeCode"><xsl:value-of select="'Total Markup'"/></xsl:attribute>
										<xsl:attribute name="FeeType">Owner Markup</xsl:attribute>
										<xsl:attribute name="Amount">
											<xsl:value-of select="$totf"/>
										</xsl:attribute>
										<xsl:attribute name="CurrencyCode">
											<xsl:value-of select="msxsl:node-set($fees)/Fees/Fee[1]/@CurrencyCode"/>
										</xsl:attribute>
										<xsl:attribute name="DecimalPlaces">
											<xsl:value-of select="msxsl:node-set($fees)/Fees/Fee[1]/@DecimalPlaces"/>
										</xsl:attribute>
									</xsl:element>
								</xsl:element>
  							</xsl:if>
  							<TotalFare>
  								<xsl:variable name="tf"><xsl:value-of select="PassengerFare/TotalFare/@Amount"/></xsl:variable>
  								<xsl:attribute name="Amount">
									<xsl:value-of select="$tf + $totf"/>
  								</xsl:attribute>
  							</TotalFare>
						</PassengerFare>
						<xsl:copy-of select="TPA_Extensions"/>
					</PTC_FareBreakdown>
				</xsl:for-each>
			</PTC_FareBreakdowns>
			<xsl:copy-of select="FareInfos"/>
		</AirItineraryPricingInfo>
	</xsl:template>
	
	<xsl:template match="AirItineraryPricingInfo" mode="total">
		<AirItineraryPricingInfo>
			<xsl:attribute name="PricingSource"><xsl:value-of select="@PricingSource"/></xsl:attribute>
			<xsl:if test="@ValidatingAirlineCode != ''">
				<xsl:attribute name="ValidatingAirlineCode"><xsl:value-of select="@ValidatingAirlineCode"/></xsl:attribute>
			</xsl:if>
			<ItinTotalFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="sum(PTC_FareBreakdowns/PTC_FareBreakdown/PassengerFare/BaseFare/@Amount)"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:attribute>
				</BaseFare>
				<xsl:copy-of select="ItinTotalFare/Taxes"/>
				<xsl:if test="PTC_FareBreakdowns/PTC_FareBreakdown/PassengerFare/Fees/Fee and $siid!='21' and $siid!='43'">
					<Fees>
						<xsl:for-each select="PTC_FareBreakdowns/PTC_FareBreakdown[1]/PassengerFare/Fees/Fee">
							<Fee>
								<xsl:attribute name="FeeCode"><xsl:value-of select="@FeeCode"/></xsl:attribute>
								<xsl:attribute name="FeeType"><xsl:value-of select="@FeeType"/></xsl:attribute>
								<xsl:variable name="fc"><xsl:value-of select="@FeeCode"/></xsl:variable>
								<xsl:attribute name="Amount">
									<xsl:value-of select="sum(../../../../PTC_FareBreakdown/PassengerFare/Fees/Fee[@FeeCode=$fc]/@Amount)"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="@CurrencyCode"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="@DecimalPlaces"/></xsl:attribute>
							</Fee>
						</xsl:for-each>
					</Fees>
				</xsl:if>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="sum(PTC_FareBreakdowns/PTC_FareBreakdown/PassengerFare/TotalFare/@Amount)"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="ItinTotalFare/TotalFare/@DecimalPlaces"/></xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<xsl:copy-of select="PTC_FareBreakdowns"/>
			<xsl:copy-of select="FareInfos"/>
		</AirItineraryPricingInfo>
	</xsl:template>
	
	<xsl:template name="markups">
		<xsl:param name="amount"/>
		<xsl:param name="CurrencyCode"/>
		<xsl:param name="DecimalPlaces"/>
		<xsl:param name="PricingSource"/>
		<xsl:param name="itin"/>
		<xsl:param name="pcc"/>
		<xsl:param name="pax"/>
		<xsl:variable name="depcountry">
			<xsl:choose>
				<xsl:when test="contains(substring-after($itin/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,','),',')">
					<xsl:value-of select="substring-after(substring-after($itin/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,', '),', ')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-after($itin/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,', ')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="arrcountry1">
			<xsl:choose>
				<xsl:when test="contains(substring-after($itin/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,','),',')">
					<xsl:value-of select="substring-after(substring-after($itin/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,', '),', ')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring-after($itin/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,', ')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="arrcountry">
			<xsl:choose>
				<xsl:when test="$depcountry!=$arrcountry1">
					<xsl:value-of select="$arrcountry1"/>
				</xsl:when>
				<xsl:when test="count($itin/OriginDestinationOption)>1">
					<xsl:variable name="country1">
						<xsl:for-each select="$itin/OriginDestinationOption[position()=2]/FlightSegment">
							<xsl:variable name="country">
								<xsl:choose>
									<xsl:when test="contains(substring-after(ArrivalAirport,','),',')">
										<xsl:value-of select="substring-after(substring-after(ArrivalAirport,', '),',')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="substring-after(ArrivalAirport,', ')"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:if test="$country!=$arrcountry1">
								<xsl:value-of select="concat($country,'/')"/>
							</xsl:if>
						</xsl:for-each>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$country1!=''"><xsl:value-of select="substring-before($country1,'/')"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="$arrcountry1"/></xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$arrcountry1"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$depcountry = $arrcountry and $PricingSource = 'Published'">
				<xsl:apply-templates select="$promos/SearchPromotionsResponse/Promotion[(FareType = '' or FareType='DomesticPublished') and (ApplicationTypeID='' or ApplicationTypeID='B2C')]">
					<xsl:with-param name="amount"><xsl:value-of select="$amount"/></xsl:with-param>
					<xsl:with-param name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:with-param>
					<xsl:with-param name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:with-param>
					<xsl:with-param name="itin" select="$itin"/>
					<xsl:with-param name="pcc"><xsl:value-of select="$pcc"/></xsl:with-param>
					<xsl:with-param name="pax"><xsl:value-of select="$pax"/></xsl:with-param>
					<xsl:with-param name="PricingSource"><xsl:value-of select="$PricingSource"/></xsl:with-param>
					<xsl:with-param name="depcountry"><xsl:value-of select="$depcountry"/></xsl:with-param>
					<xsl:with-param name="arrcountry"><xsl:value-of select="$arrcountry"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$depcountry = $arrcountry and $PricingSource = 'Private'">
				<xsl:apply-templates select="$promos/SearchPromotionsResponse/Promotion[(FareType = '' or FareType='DomesticPrivate') and (ApplicationTypeID='' or ApplicationTypeID='B2C')]">
					<xsl:with-param name="amount"><xsl:value-of select="$amount"/></xsl:with-param>
					<xsl:with-param name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:with-param>
					<xsl:with-param name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:with-param>
					<xsl:with-param name="itin" select="$itin"/>
					<xsl:with-param name="pcc"><xsl:value-of select="$pcc"/></xsl:with-param>
					<xsl:with-param name="pax"><xsl:value-of select="$pax"/></xsl:with-param>
					<xsl:with-param name="PricingSource"><xsl:value-of select="$PricingSource"/></xsl:with-param>
					<xsl:with-param name="depcountry"><xsl:value-of select="$depcountry"/></xsl:with-param>
					<xsl:with-param name="arrcountry"><xsl:value-of select="$arrcountry"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$depcountry != $arrcountry and $PricingSource = 'Published'">
				<xsl:apply-templates select="$promos/SearchPromotionsResponse/Promotion[(FareType = '' or FareType='InternationalPublished') and (ApplicationTypeID='' or ApplicationTypeID='B2C')]">
					<xsl:with-param name="amount"><xsl:value-of select="$amount"/></xsl:with-param>
					<xsl:with-param name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:with-param>
					<xsl:with-param name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:with-param>
					<xsl:with-param name="itin" select="$itin"/>
					<xsl:with-param name="pcc"><xsl:value-of select="$pcc"/></xsl:with-param>
					<xsl:with-param name="pax"><xsl:value-of select="$pax"/></xsl:with-param>
					<xsl:with-param name="PricingSource"><xsl:value-of select="$PricingSource"/></xsl:with-param>
					<xsl:with-param name="depcountry"><xsl:value-of select="$depcountry"/></xsl:with-param>
					<xsl:with-param name="arrcountry"><xsl:value-of select="$arrcountry"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="$depcountry != $arrcountry and $PricingSource = 'Private'">
				<xsl:apply-templates select="$promos/SearchPromotionsResponse/Promotion[(FareType = '' or FareType='InternationalPrivate') and (ApplicationTypeID='' or ApplicationTypeID='B2C')]">
					<xsl:with-param name="amount"><xsl:value-of select="$amount"/></xsl:with-param>
					<xsl:with-param name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:with-param>
					<xsl:with-param name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:with-param>
					<xsl:with-param name="itin" select="$itin"/>
					<xsl:with-param name="pcc"><xsl:value-of select="$pcc"/></xsl:with-param>
					<xsl:with-param name="pax"><xsl:value-of select="$pax"/></xsl:with-param>
					<xsl:with-param name="PricingSource"><xsl:value-of select="$PricingSource"/></xsl:with-param>
					<xsl:with-param name="depcountry"><xsl:value-of select="$depcountry"/></xsl:with-param>
					<xsl:with-param name="arrcountry"><xsl:value-of select="$arrcountry"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="Promotion">
		<xsl:param name="amount"/>
		<xsl:param name="CurrencyCode"/>
		<xsl:param name="DecimalPlaces"/>
		<xsl:param name="itin"/>
		<xsl:param name="pcc"/>
		<xsl:param name="pax"/>
		<xsl:param name="PricingSource"/>
		<xsl:param name="depcountry"/>
		<xsl:param name="arrcountry"/>
		<xsl:if test="YieldType != ''">
			<xsl:variable name="airlines">
				<xsl:for-each select="$itin/OriginDestinationOption/FlightSegment">
					<xsl:value-of select="MarketingAirline/@Code"/>
					<xsl:text>/</xsl:text>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="airlinescount">
				<xsl:call-template name="getairline">
					<xsl:with-param name="airlines"><xsl:value-of select="$airlines"/></xsl:with-param>
					<xsl:with-param name="oneair"/>
				</xsl:call-template>
			</xsl:variable>
			<xsl:variable name="shareairlines">
				<xsl:for-each select="$itin/OriginDestinationOption/FlightSegment">
					<xsl:value-of select="OperatingAirline/@Code"/>
					<xsl:text>/</xsl:text>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="classes">
				<xsl:for-each select="$itin/OriginDestinationOption/FlightSegment">
					<xsl:value-of select="concat(MarketingAirline/@Code,@ResBookDesigCode,'/')"/>
				</xsl:for-each>
			</xsl:variable>
			<xsl:variable name="pid"><xsl:value-of select="PromotionId"/></xsl:variable>
			<xsl:variable name="aircode">
				<xsl:if test="SupplierCode != ''">
					<xsl:choose>
						<xsl:when test="preceding-sibling::Promotion[1][PromotionId = $pid]">
							<xsl:variable name="sid"><xsl:value-of select="SupplierId"/></xsl:variable>
							<xsl:if test="preceding-sibling::Promotion[1][PromotionId = $pid]/SupplierId != $sid">
								<xsl:value-of select="SupplierCode"/>
							</xsl:if>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="SupplierCode"/></xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="codeshare">
				<xsl:if test="SupplierCode != '' and preceding-sibling::Promotion[1][PromotionId = $pid]">    
					<!--xsl:variable name="sid"><xsl:value-of select="supplierID"/></xsl:variable>
					<xsl:if test="preceding-sibling::Promotion[1][PromotionId = $pid]/supplierID = $sid">
						<xsl:value-of select="SupplierCode"/>
					</xsl:if-->
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="bkgClass" select="concat(SupplierCode,BookingClass)"/>
			<xsl:variable name="depCountry">
				<xsl:if test="contains($itin/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,',')">
					<xsl:if test="contains(substring-after($itin/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,','),', US') or contains(substring-after($itin/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,','),', CA')">
						<xsl:value-of select="'US'"/>
					</xsl:if>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="arrCountry">
				<xsl:if test="contains($itin/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,',')">
					<xsl:if test="contains(substring-after($itin/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,','),', US') or contains(substring-after($itin/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,','),', CA')">
						<xsl:value-of select="'US'"/>
					</xsl:if>
				</xsl:if>
			</xsl:variable>
			<xsl:variable name="cabin">
				<xsl:value-of select="$itin/OriginDestinationOption[1]/FlightSegment[1]/TPA_Extensions/CabinType/@Cabin"/>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="contains(Name,'Paper')"></xsl:when>
				<xsl:when test="Name = preceding-sibling::Promotion/Name and PromotionId= preceding-sibling::Promotion/PromotionId and SupplierCode= preceding-sibling::Promotion/SupplierCode"></xsl:when>
				<xsl:when test="$aircode != '' and contains($airlines,$aircode) and SupplierExclude='true'"></xsl:when>
				<xsl:when test="$aircode != '' and not(contains($airlines,$aircode))"></xsl:when>
				<xsl:when test="string-length($airlinescount)>2 and substring(YieldAmount,1,1)='-'"></xsl:when>
				<xsl:when test="$PricingSource='Private' and substring(YieldAmount,1,1)='-'"></xsl:when>
				<xsl:when test="$codeshare  != '' and contains($shareairlines,$codeshare ) and CodeShareExclude='true'"></xsl:when>
				<xsl:when test="$codeshare != '' and not(contains($shareairlines,$codeshare))"></xsl:when>
				<xsl:when test="officeID != '' and officeID != $pcc"></xsl:when>	
				<xsl:when test="BookingClass != '' and not(contains($classes,$bkgClass))"></xsl:when>
				<xsl:when test="$aircode='9W' and $depCountry='' and $arrCountry=''"></xsl:when>
				<xsl:when test="$username='nedorogo' and $PricingSource='Published' and YieldType='$'"></xsl:when>
				<xsl:when test="$username='jizo' and $PricingSource='Published' and YieldType='$'"></xsl:when>
				<xsl:when test="FromCountry!='' and FromCountry!=$depcountry"></xsl:when>
				<xsl:when test="ToCountry!='' and ToCountry!=$arrcountry"></xsl:when>
				<xsl:when test="Cabin!='' and Cabin!=$cabin"></xsl:when>
				<xsl:when test="SiteItemId!='' and SiteItemId!=$siid"></xsl:when>
				<xsl:otherwise>
					<xsl:element name="Fee">
						<xsl:attribute name="FeeCode"><xsl:value-of select="Name"/></xsl:attribute>
						<xsl:attribute name="FeeType"><xsl:value-of select="PromotionTypeName"/></xsl:attribute>
						<xsl:variable name="deci">
							<xsl:choose>
								<xsl:when test="$DecimalPlaces='0'">1</xsl:when>
								<xsl:when test="$DecimalPlaces='1'">10</xsl:when>
								<xsl:when test="$DecimalPlaces='2'">100</xsl:when>
								<xsl:when test="$DecimalPlaces='3'">1000</xsl:when>
							</xsl:choose>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="YieldType='%'">
								<xsl:attribute name="Amount">
									<xsl:variable name="ya"><xsl:value-of select="YieldAmount"/></xsl:variable>
									<xsl:variable name="ym"><xsl:value-of select="(($amount * $ya) div 100) + 0.5"/></xsl:variable>
									<xsl:value-of select="format-number($ym,'##')"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="Amount">
									<xsl:variable name="YA">
										<xsl:choose>
											<xsl:when test="$currency='RUB'"><xsl:value-of select="YieldAmount * 31.6"/></xsl:when>
											<xsl:when test="$currency='ZAR'"><xsl:value-of select="YieldAmount * 8"/></xsl:when>
											<xsl:when test="$currency='AED'"><xsl:value-of select="YieldAmount * 3.7"/></xsl:when>
											<xsl:when test="$currency='IRR'"><xsl:value-of select="YieldAmount * 11305"/></xsl:when>
											<xsl:when test="$currency='GBP'"><xsl:value-of select="YieldAmount * 0.65"/></xsl:when>
											<xsl:when test="$currency='EUR'"><xsl:value-of select="YieldAmount * 0.78"/></xsl:when>
											<xsl:when test="$currency='SEK'"><xsl:value-of select="YieldAmount * 6.9"/></xsl:when>
											<xsl:otherwise><xsl:value-of select="YieldAmount"/></xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="format-number($YA * $deci * $pax,'0')"/>
								</xsl:attribute>
								<xsl:attribute name="CurrencyCode"><xsl:value-of select="$CurrencyCode"/></xsl:attribute>
								<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$DecimalPlaces"/></xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:element>
				</xsl:otherwise>		
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="getairline">
		<xsl:param name="airlines"/>
		<xsl:param name="oneair"/>
		<xsl:choose>
			<xsl:when test="$airlines!=''">
				<xsl:variable name="newair">
				<xsl:if test="not(contains($oneair,substring($airlines,1,2)))">
					<xsl:value-of select="substring($airlines,1,2)"/>
				</xsl:if>
				</xsl:variable>
				<xsl:call-template name="getairline">
					<xsl:with-param name="airlines"><xsl:value-of select="substring-after($airlines,'/')"/></xsl:with-param>
					<xsl:with-param name="oneair"><xsl:value-of select="concat($oneair,$newair)"/></xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$oneair"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
