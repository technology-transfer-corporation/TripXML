<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="2.0">
<!-- ================================================================== -->
<!-- BL_LowFareRS.xsl 																                   -->
<!-- ================================================================== -->
<!-- Date: 02 Oct 2012 - Rastko - added support for LowOfferMatrix and LowOfferSearch	       -->
<!-- Date: 24 Sep 2012 - Rastko	- added support for markups in BL files			       -->
<!-- Date: 18 Jul 2011 - Rastko	- added support for LowFareMatrix					       -->
<!-- Date: 10 Sep 2008 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="bl" select="OTA_AirLowFareSearchRS/ProviderBL/Fees | OTA_AirLowFareSearchPlusRS/ProviderBL/Fees | OTA_AirLowFareSearchScheduleRS/ProviderBL/Fees | OTA_AirLowFareSearchMatrixRS/ProviderBL/Fees | OTA_AirLowOfferMatrixRS/ProviderBL/Fees | OTA_AirLowOfferSearchRS/ProviderBL/Fees"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRS | OTA_AirLowFareSearchPlusRS | OTA_AirLowFareSearchScheduleRS | OTA_AirLowFareSearchMatrixRS | OTA_AirLowOfferMatrixRS | OTA_AirLowOfferSearchRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirLowFareSearchRS">
		<OTA_AirLowFareSearchRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowFareSearchPlusRS">
		<OTA_AirLowFareSearchPlusRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchPlusRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowFareSearchScheduleRS">
		<OTA_AirLowFareSearchScheduleRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchScheduleRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowFareSearchMatrixRS">
		<OTA_AirLowFareSearchMatrixRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowFareSearchMatrixRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowOfferMatrixRS">
		<OTA_AirLowOfferMatrixRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowOfferMatrixRS>
	</xsl:template>
	
	<xsl:template match="OTA_AirLowOfferSearchRS">
		<OTA_AirLowOfferSearchRS>
			<xsl:if test="@EchoToken != ''">
				<xsl:attribute name="EchoToken"><xsl:value-of select="@EchoToken"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Version"><xsl:value-of select="@Version"/></xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="@TransactionIdentifier"/></xsl:attribute>	
			<xsl:choose>
				<xsl:when test="Success">
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="Errors"/>
				</xsl:otherwise>
			</xsl:choose>	
		</OTA_AirLowOfferSearchRS>
	</xsl:template>

	<!--*************************************************************-->
	<xsl:template match="PricedItinerary">
		<xsl:variable name="ods" select="AirItinerary/OriginDestinationOptions"/>
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber"><xsl:value-of select="@SequenceNumber" /></xsl:attribute>
			<xsl:copy-of select="AirItinerary"/>
			<xsl:variable name="originCntry">
				<xsl:value-of select="substring($ods/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport,string-length($ods/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport) - 1)"/>
			</xsl:variable>
			<xsl:variable name="arrivalCntry">
				<xsl:value-of select="substring($ods/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport,string-length($ods/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport) - 1)"/>
			</xsl:variable>
			<xsl:apply-templates select="AirItineraryPricingInfo">
				<xsl:with-param name="airline"><xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[1]/MarketingAirline/@Code"/></xsl:with-param>
				<xsl:with-param name="class"><xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[1]/@ResBookDesigCode"/></xsl:with-param>
				<xsl:with-param name="cabin"><xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[1]/TPA_Extensions/CabinType/@Cabin"/></xsl:with-param>
				<xsl:with-param name="depDate"><xsl:value-of select="translate(substring($ods/OriginDestinationOption[1]/FlightSegment[1]/@DepartureDateTime,1,10),'-','')"/></xsl:with-param>
				<xsl:with-param name="origin"><xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode"/></xsl:with-param>
				<xsl:with-param name="arrival"><xsl:value-of select="$ods/OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/></xsl:with-param>
				<xsl:with-param name="originCntry"><xsl:value-of select="$originCntry"/></xsl:with-param>
				<xsl:with-param name="arrivalCntry"><xsl:value-of select="$arrivalCntry"/></xsl:with-param>
			</xsl:apply-templates>
			<xsl:copy-of select="TicketingInfo"/>
		</PricedItinerary>
	</xsl:template>

	<xsl:template match="AirItineraryPricingInfo">
		<xsl:param name="airline"/>
		<xsl:param name="class"/>
		<xsl:param name="cabin"/>
		<xsl:param name="depDate"/>
		<xsl:param name="origin"/>
		<xsl:param name="arrival"/>
		<xsl:param name="originCntry"/>
		<xsl:param name="arrivalCntry"/>
		<AirItineraryPricingInfo>
			<xsl:attribute name="PricingSource">	<xsl:value-of select="@PricingSource" /></xsl:attribute>
			<xsl:variable name="nip">
				<xsl:variable name="all"><xsl:value-of select="sum(PTC_FareBreakdowns/PTC_FareBreakdown/PassengerTypeQuantity/@Quantity)"/></xsl:variable>
				<xsl:variable name="inf">
					<xsl:choose>
						<xsl:when test="PTC_FareBreakdowns/PTC_FareBreakdown/PassengerTypeQuantity/@Code = 'INF'">
							<xsl:choose>
								<xsl:when test="PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='INF']/PassengerFare/TotalFare/@Amount = '0'">
									<xsl:value-of select="PTC_FareBreakdowns/PTC_FareBreakdown/PassengerTypeQuantity[@Code='INF']/@Quantity"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:value-of select="$all - $inf"/>
			</xsl:variable>
			<xsl:variable name="feep" select="$bl/Airline[@Code='**' or @Code=$airline][@ClassOfService='*' or @ClassOfService=$class][not(@Cabin) or @Cabin='*' or @Cabin=$cabin][@StartDate &lt;= $depDate and @EndDate >= $depDate]/Fee[@Fare='Private']"/>
			<xsl:variable name="fees" select="$bl/Airline[@Code='**' or @Code=$airline][@ClassOfService='*' or @ClassOfService=$class][not(@Cabin) or @Cabin='*' or @Cabin=$cabin][@StartDate &lt;= $depDate and @EndDate >= $depDate]/Fee[@Fare='Published']"/>
			<xsl:variable name="pricesource"><xsl:value-of select="@PricingSource"/></xsl:variable>
			<ItinTotalFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="@PricingSource = 'Private'">
								<xsl:choose>
									<xsl:when test="$feep != '' and (not($feep/Exclude) or not($feep/Exclude[@Code=$airline]))">
										<xsl:variable name="valp" select="$feep/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
										<xsl:variable name="markup1">
											<xsl:choose>
												<xsl:when test="$valp !='' ">
													<xsl:choose>
														<xsl:when test="$valp/@Type = 'P'">
															<xsl:variable name="base"><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:variable>
															<xsl:value-of select="format-number(($base * $valp) div 100,'0')"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate($valp,'.','')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:variable name="markup">
											<xsl:value-of select="$markup1 * $nip"/>
										</xsl:variable>
										<xsl:variable name="bf">
											<xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" />
										</xsl:variable>
										<xsl:value-of select="$markup + $bf"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<!--xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /-->
								<xsl:choose>
									<xsl:when test="$fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
										<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
										<xsl:variable name="markup1">
											<xsl:choose>
												<xsl:when test="$vals !='' ">
													<xsl:choose>
														<xsl:when test="$vals/@Type = 'P'">
															<xsl:variable name="base"><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:variable>
															<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate($vals,'.','')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:variable name="markup">
											<xsl:value-of select="$markup1 * $nip"/>
										</xsl:variable>
										<xsl:variable name="bf">
											<xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" />
										</xsl:variable>
										<xsl:value-of select="$markup + $bf"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:copy-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
					<xsl:copy-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">
							<xsl:value-of select="ItinTotalFare/Taxes/Tax/@TaxCode" />
						</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(ItinTotalFare/Taxes/Tax/@Amount,'.','')" />
						</xsl:attribute>
						<xsl:copy-of select="ItinTotalFare/Taxes/Tax/@CurrencyCode"/>
						<xsl:copy-of select="ItinTotalFare/Taxes/Tax/@DecimalPlaces"/>
					</Tax>
				</Taxes>
				<xsl:choose>
					<xsl:when test="ItinTotalFare/Fees and $fees=''">
						<xsl:copy-of select="ItinTotalFare/Fees"/>
					</xsl:when>
					<!--xsl:when test="not(ItinTotalFare/Fees) and @PricingSource = 'Published' and $fees != ''">
						<xsl:variable name="valsCty" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]"/>
						<xsl:variable name="valsCntry" select="$fees/Route[@DepCountry=$originCntry][@ArrCountry=$arrivalCntry]"/>
						<xsl:variable name="vals" select="if ($valsCntry!='') then $valsCntry else $valsCty"/>
						<xsl:if test="$vals != ''">
							<Fees>
								<xsl:for-each select="$vals/Value">
									<Fee>
										<xsl:attribute name="FeeCode"><xsl:value-of select="@Type"/></xsl:attribute>
										<xsl:attribute name="Amount">
											<xsl:variable name="service1">
												<xsl:choose>
													<xsl:when test="@Type = 'P'">
														<xsl:variable name="base">
															<xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" />
														</xsl:variable>														
														<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="translate($vals,'.','')"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:value-of select="$service1 * $nip"/>
										</xsl:attribute>
										<xsl:copy-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
										<xsl:copy-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/>
									</Fee>
								</xsl:for-each>
							</Fees>
						</xsl:if>
					</xsl:when-->
					<xsl:when test="not(ItinTotalFare/Fees) and @PricingSource = 'Published' and $fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
						<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
						<Fees>
							<Fee>
								<xsl:attribute name="FeeCode">
									<xsl:value-of select="$fees/@Type"/>
								</xsl:attribute>
								<xsl:attribute name="Amount">
									<xsl:variable name="service1">
										<xsl:choose>
											<xsl:when test="$vals/@Type = 'P'">
												<xsl:variable name="base"><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:variable>																							<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="translate($vals,'.','')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:variable name="service2">
										<xsl:value-of select="$service1 * $nip"/>
									</xsl:variable>
									<xsl:value-of select="$service2"/>
								</xsl:attribute>
								<xsl:copy-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
								<xsl:copy-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/>
							</Fee>
						</Fees>
					</xsl:when>
					<xsl:when test="ItinTotalFare/Fees and @PricingSource = 'Published' and $fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
						<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
						<Fees>
							<Fee>
								<xsl:attribute name="FeeCode">
									<xsl:value-of select="$fees/@Type"/>
								</xsl:attribute>
								<xsl:attribute name="Amount">
									<xsl:variable name="service1">
										<xsl:choose>
											<xsl:when test="$vals/@Type = 'P'">
												<xsl:variable name="base"><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:variable>																							<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="translate($vals,'.','')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:variable name="service2">
										<xsl:value-of select="$service1 * $nip"/>
									</xsl:variable>
									<xsl:variable name="service3">
										<xsl:value-of select="ItinTotalFare/Fees/Fee/@Amount"/>
									</xsl:variable>
									<xsl:value-of select="$service2 + $service3"/>
								</xsl:attribute>
								<xsl:copy-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
								<xsl:copy-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/>
							</Fee>
						</Fees>
					</xsl:when>
				</xsl:choose>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="@PricingSource = 'Private'">
								<xsl:choose>
									<xsl:when test="$feep != '' and (not($feep/Exclude) or not($feep/Exclude[@Code=$airline]))">
										<xsl:variable name="valp" select="$feep/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
										<xsl:variable name="markup1">
											<xsl:choose>
												<xsl:when test="$valp !='' ">
													<xsl:choose>
														<xsl:when test="$valp/@Type = 'P'">
															<xsl:variable name="base"><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:variable>																								<xsl:value-of select="format-number(($base * $valp) div 100,'0')"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate($valp,'.','')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:variable name="markup">
											<xsl:value-of select="$markup1 * $nip"/>
										</xsl:variable>
										<xsl:variable name="tf">
											<xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" />
										</xsl:variable>
										<xsl:value-of select="$markup + $tf"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" /></xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="@PricingSource = 'Published'">
								<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
								<xsl:choose>
									<xsl:when test="$fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
										<xsl:variable name="markup1">
											<xsl:choose>
												<xsl:when test="$vals !='' ">
													<xsl:choose>
														<xsl:when test="$vals/@Type = 'P'">
															<xsl:variable name="base"><xsl:value-of select="translate(ItinTotalFare/BaseFare/@Amount,'.','')" /></xsl:variable>																								<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>	
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate($vals,'.','')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>										
										</xsl:variable>
										<xsl:variable name="markup">
											<xsl:value-of select="$markup1 * $nip"/>
										</xsl:variable>
										<xsl:variable name="tf">
											<xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" />
										</xsl:variable>
										<xsl:value-of select="$markup + $tf"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" /></xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" /></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" /></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="ItinTotalFare/TotalFare/@DecimalPlaces" /></xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="PTC_FareBreakdowns/PTC_FareBreakdown" mode="PaxType">
					<xsl:with-param name="airline"><xsl:value-of select="$airline"/></xsl:with-param>
					<xsl:with-param name="class"><xsl:value-of select="$class"/></xsl:with-param>
					<xsl:with-param name="cabin"><xsl:value-of select="$cabin"/></xsl:with-param>
					<xsl:with-param name="depDate"><xsl:value-of select="$depDate"/></xsl:with-param>
					<xsl:with-param name="origin"><xsl:value-of select="$origin"/></xsl:with-param>
					<xsl:with-param name="arrival"><xsl:value-of select="$arrival"/></xsl:with-param>
					<xsl:with-param name="pricesource"><xsl:value-of select="$pricesource"/></xsl:with-param>
				</xsl:apply-templates>
			</PTC_FareBreakdowns>
			<xsl:copy-of select="FareInfos"/>
		</AirItineraryPricingInfo>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Fare Breakdown per pax type					    -->
	<!-- ************************************************************** -->
	<xsl:template match="PTC_FareBreakdown" mode="PaxType">
		<xsl:param name="airline"/>
		<xsl:param name="class"/>
		<xsl:param name="cabin"/>
		<xsl:param name="depDate"/>
		<xsl:param name="origin"/>
		<xsl:param name="arrival"/>
		<xsl:param name="pricesource"/>
		<xsl:variable name="feep" select="$bl/Airline[@Code='**' or @Code=$airline][@ClassOfService='*' or @ClassOfService=$class][not(@Cabin) or @Cabin='*' or @Cabin=$cabin][@StartDate &lt;= $depDate and @EndDate >= $depDate]/Fee[@Fare='Private']"/>
		<xsl:variable name="fees" select="$bl/Airline[@Code='**' or @Code=$airline][@ClassOfService='*' or @ClassOfService=$class][not(@Cabin) or @Cabin='*' or @Cabin=$cabin][@StartDate &lt;= $depDate and @EndDate >= $depDate]/Fee[@Fare='Published']"/>
		<xsl:variable name="nip">
			<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
		</xsl:variable>
		<xsl:variable name="PaxTypeQuantity">
			<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
		</xsl:variable>
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="PassengerTypeQuantity/@Code" />
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<xsl:if test="FareBasisCode">
				<FareBasisCodes>
					<xsl:choose>
						<xsl:when test="contains(FareBasisCode,'*')">
							<xsl:for-each select="FareBasisCode">
								<xsl:call-template name="parsefb">
									<xsl:with-param name="fbc">
										<xsl:value-of select="." />
									</xsl:with-param>
								</xsl:call-template>
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="FareBasisCodes/FareBasisCode" mode="PaxFareBasis" />
						</xsl:otherwise>
					</xsl:choose>
				</FareBasisCodes>
			</xsl:if>
			<PassengerFare>
				<xsl:if test="PassengerFare/BaseFare">
					<BaseFare>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="$pricesource = 'Private'">
									<xsl:choose>
										<xsl:when test="$feep != '' and (not($feep/Exclude) or not($feep/Exclude[@Code=$airline]))">
											<xsl:variable name="valp" select="$feep/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
											<xsl:variable name="markup1">
												<xsl:choose>
													<xsl:when test="$valp !='' ">
														<xsl:choose>
															<xsl:when test="$valp/@Type = 'P'">
																<xsl:variable name="base"><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:variable>
																<xsl:value-of select="format-number(($base * $valp) div 100,'0')"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="translate($valp,'.','')"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:when>
													<xsl:otherwise>0</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable name="markup">
												<xsl:value-of select="$markup1 * $nip"/>
											</xsl:variable>
											<xsl:variable name="bf">
												<xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" />
											</xsl:variable>
											<xsl:value-of select="$markup + $bf"/>
										</xsl:when>
										<xsl:otherwise><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<!--xsl:otherwise><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:otherwise-->
								<xsl:when test="$pricesource = 'Published'">
									<xsl:choose>
										<xsl:when test="$fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
											<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
											<xsl:variable name="markup1">
												<xsl:choose>
													<xsl:when test="$vals !='' ">
														<xsl:choose>
															<xsl:when test="$vals/@Type = 'P'">
																<xsl:variable name="base"><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:variable>
																<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="translate($vals,'.','')"/>
															</xsl:otherwise>
														</xsl:choose>
													</xsl:when>
													<xsl:otherwise>0</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable name="markup">
												<xsl:value-of select="$markup1 * $nip"/>
											</xsl:variable>
											<xsl:variable name="bf">
												<xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" />
											</xsl:variable>
											<xsl:value-of select="$markup + $bf"/>
										</xsl:when>
										<xsl:otherwise><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:otherwise>
									</xsl:choose>
								</xsl:when>
							</xsl:choose>
						</xsl:attribute>
					</BaseFare>
				</xsl:if>
				<xsl:if test="PassengerFare/Taxes">
					<Taxes>
						<Tax>
							<xsl:attribute name="TaxCode">
								<xsl:value-of select="PassengerFare/Taxes/Tax/@TaxCode" />
							</xsl:attribute>
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(PassengerFare/Taxes/Tax/@Amount,'.','')" />
							</xsl:attribute>
						</Tax>
					</Taxes>
				</xsl:if>
				<xsl:if test="$pricesource = 'Published'">
					<xsl:if test="$fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
						<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
						<xsl:if test="$vals != ''">
							<Fees>
								<Fee>
									<xsl:attribute name="FeeCode"><xsl:value-of select="$fees/@Type"/></xsl:attribute>
									<xsl:attribute name="Amount">
										<xsl:variable name="service1">
											<xsl:choose>
												<xsl:when test="$vals/@Type = 'P'">
													<xsl:variable name="base"><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:variable>																							<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="translate($vals,'.','')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:value-of select="$service1 * $nip"/>
									</xsl:attribute>
									<xsl:copy-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
									<xsl:copy-of select="ItinTotalFare/BaseFare/@DecimalPlaces"/>
								</Fee>
							</Fees>
						</xsl:if>
					</xsl:if>
				</xsl:if>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="$pricesource = 'Private'">
								<xsl:choose>
									<xsl:when test="$feep != '' and (not($feep/Exclude) or not($feep/Exclude[@Code=$airline]))">
										<xsl:variable name="valp" select="$feep/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
										<xsl:variable name="markup1">
											<xsl:choose>
												<xsl:when test="$valp !='' ">
													<xsl:choose>
														<xsl:when test="$valp/@Type = 'P'">
															<xsl:variable name="base"><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:variable>
															<xsl:value-of select="format-number(($base * $valp) div 100,'0')"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate($valp,'.','')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:variable name="markup">
											<xsl:value-of select="$markup1 * $nip"/>
										</xsl:variable>
										<xsl:variable name="tf">
											<xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','')" />
										</xsl:variable>
										<xsl:value-of select="$markup + $tf"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','')" /></xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:when test="$pricesource = 'Published'">
								<xsl:variable name="vals" select="$fees/Route[@Depart='***' or @Depart=$origin][@Arrival='***' or @Arrival=$arrival]/Value"/>
								<xsl:choose>
									<xsl:when test="$fees != '' and (not($fees/Exclude) or not($fees/Exclude[@Code=$airline]))">
										<xsl:variable name="markup1">
											<xsl:choose>
												<xsl:when test="$vals !='' ">
													<xsl:choose>
														<xsl:when test="$vals/@Type = 'P'">
															<xsl:variable name="base"><xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','')" /></xsl:variable>																								<xsl:value-of select="format-number(($base * $vals) div 100,'0')"/>	
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="translate($vals,'.','')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:when>
												<xsl:otherwise>0</xsl:otherwise>
											</xsl:choose>										
										</xsl:variable>
										<xsl:variable name="markup">
											<xsl:value-of select="$markup1 * $nip"/>
										</xsl:variable>
										<xsl:variable name="tf">
											<xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','')" />
										</xsl:variable>
										<xsl:value-of select="$markup + $tf"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','')" /></xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<!--xsl:otherwise><xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','')" /></xsl:otherwise-->
						</xsl:choose>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<xsl:if test="TPA_Extensions/PricedCode != ''">
				<TPA_Extensions>
  					<PricedCode><xsl:value-of select="TPA_Extensions/PricedCode"/></PricedCode> 
  					<xsl:copy-of select="TPA_Extensions/Text"/>
  				</TPA_Extensions>
			</xsl:if>
		</PTC_FareBreakdown>
	</xsl:template>
	<xsl:template name="parsefb">
		<xsl:param name="fbc" />
		<xsl:choose>
			<xsl:when test="contains($fbc,'*')">
				<xsl:variable name="item">
					<xsl:value-of select="substring-before($fbc,'*')" />
				</xsl:variable>
				<FareBasisCode>
					<xsl:value-of select="$item" />
				</FareBasisCode>
				<xsl:call-template name="parsefb">
					<xsl:with-param name="fbc">
						<xsl:value-of select="substring-after($fbc,'*')" />
					</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<FareBasisCode>
					<xsl:value-of select="$fbc" />
				</FareBasisCode>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="FareBasisCode" mode="PaxFareBasis">
		<FareBasisCode>
			<xsl:value-of select="." />
		</FareBasisCode>
	</xsl:template>
	
</xsl:stylesheet>
