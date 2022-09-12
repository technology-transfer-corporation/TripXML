<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- PortalXML_LowFareRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 11 Aug 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:if test="SearchForPackagesResponse/SearchForPackagesResult/WasSuccesfull='false'">
			<OTA_AirLowFareSearchRS Version="1.001">
				<Errors>
					<xsl:apply-templates select="SearchForPackagesResponse/SearchForPackagesResult/ResultData/Errors" mode="Error" />
				</Errors>
			</OTA_AirLowFareSearchRS>
		</xsl:if>
		<xsl:apply-templates select="ArrayOfPackage" />
	</xsl:template>
	<xsl:template match="Errors" mode="Error">
		<Error Type="Portal">
			<xsl:value-of select="anyType/errvalue" />
		</Error>
	</xsl:template>
	<xsl:template match="ArrayOfPackage">
		<OTA_AirLowFareSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:attribute name="TransactionIdentifier"><xsl:value-of select="SessionID"/></xsl:attribute>
			<Success></Success>
			<xsl:variable name="adt">
				<xsl:choose>
					<xsl:when test="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity!=''">
						<xsl:value-of select="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="chd">
				<xsl:choose>
					<xsl:when test="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity!=''">
						<xsl:value-of select="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="yth">
				<xsl:choose>
					<xsl:when test="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='YTH']/@Quantity!=''">
						<xsl:value-of select="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='YTH']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="src">
				<xsl:choose>
					<xsl:when test="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity!=''">
						<xsl:value-of select="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ins">
				<xsl:choose>
					<xsl:when test="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity!=''">
						<xsl:value-of select="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="inf">
				<xsl:choose>
					<xsl:when test="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity!=''">
						<xsl:value-of select="OTA_AirLowFareSearchRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity"/>
					</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="nip"><xsl:value-of select="$adt + $chd + $yth + $src + $ins"/></xsl:variable>
			<PricedItineraries>
				<xsl:apply-templates select="Package">
					<xsl:with-param name="nip"><xsl:value-of select="$nip"/></xsl:with-param>
					<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
					<xsl:with-param name="chd"><xsl:value-of select="$chd"/></xsl:with-param>
					<xsl:with-param name="yth"><xsl:value-of select="$yth"/></xsl:with-param>
					<xsl:with-param name="src"><xsl:value-of select="$src"/></xsl:with-param>
					<xsl:with-param name="ins"><xsl:value-of select="$ins"/></xsl:with-param>
					<xsl:with-param name="inf"><xsl:value-of select="$inf"/></xsl:with-param>
				</xsl:apply-templates>
			</PricedItineraries>
		</OTA_AirLowFareSearchRS>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="Package">
		<xsl:param name="nip"/>
		<xsl:param name="adt"/>
		<xsl:param name="chd"/>
		<xsl:param name="yth"/>
		<xsl:param name="src"/>
		<xsl:param name="ins"/>
		<xsl:param name="inf"/>
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">
				<xsl:value-of select="position() - 1" />
			</xsl:attribute>
			<!--xsl:attribute name="Provider">
				<xsl:value-of select="Air/Provider"/>
			</xsl:attribute-->
			<AirItinerary>
				<OriginDestinationOptions>
					<xsl:for-each select="Air/Flights/Flight">
						<OriginDestinationOption>
							<xsl:for-each select="FlightSegments/FlightSegment">
								<FlightSegment>
									<xsl:attribute name="DepartureDateTime">
										<xsl:value-of select="substring(DepartureDateTime,1,19)"/>
									</xsl:attribute>
									<xsl:attribute name="ArrivalDateTime">
										<xsl:value-of select="substring(ArrivalDateTime,1,19)"/>
									</xsl:attribute>
									<xsl:attribute name="StopQuantity">
											<xsl:value-of select="Stops"/>
									</xsl:attribute>
									<xsl:attribute name="RPH">
										<xsl:value-of select="Rph" />
									</xsl:attribute>
									<xsl:attribute name="FlightNumber">
										<xsl:value-of select="FlightSegmentNumber" />
									</xsl:attribute>
									<xsl:attribute name="ResBookDesigCode">
										<xsl:value-of select="SelectedBookingClass/ID" />
									</xsl:attribute>
									<xsl:attribute name="NumberInParty">
										<xsl:value-of select="$nip" />
									</xsl:attribute>
									<xsl:attribute name="E_TicketEligibility">
										<xsl:choose>
											<xsl:when test="ETicketAvailable='true'">Eligible</xsl:when>
											<xsl:otherwise>NotEligible</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<DepartureAirport>
										<xsl:attribute name="LocationCode">
											<xsl:value-of select="DepartureAirport/ID" />
										</xsl:attribute>
									</DepartureAirport>
									<ArrivalAirport>
										<xsl:attribute name="LocationCode">
											<xsl:value-of select="ArrivalAirport/ID" />
										</xsl:attribute>
									</ArrivalAirport>
									<xsl:if test="OperatingAirline/Code!=''">
										<OperatingAirline>
											<xsl:attribute name="Code">
												<xsl:choose>
													<xsl:when test="OperatingAirline/HiddenCode!=''">
														<xsl:value-of select="OperatingAirline/HiddenCode" />
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="OperatingAirline/Code" />
													</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</OperatingAirline>
									</xsl:if>
									<Equipment>
										<xsl:attribute name="AirEquipType">
											<xsl:value-of select="Airplane/PseudoCode" />
										</xsl:attribute>
									</Equipment>
									<MarketingAirline>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="MarketingAirline/HiddenCode!=''">
													<xsl:value-of select="MarketingAirline/HiddenCode" />
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="MarketingAirline/Code" />
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</MarketingAirline>
								</FlightSegment>
							</xsl:for-each>
						</OriginDestinationOption>
					</xsl:for-each>
				</OriginDestinationOptions>
			</AirItinerary>
			<AirItineraryPricingInfo>
				<xsl:attribute name="PricingSource"><xsl:value-of select="Air/PricingSource"/></xsl:attribute>
				<ItinTotalFare>
					<xsl:apply-templates select="Air/AirPrices"/>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:if test="$adt &gt; 0">
						<xsl:apply-templates select="Air/AdultPrices">
							<xsl:with-param name="nip"><xsl:value-of select="$adt"/></xsl:with-param>
							<xsl:with-param name="type">ADT</xsl:with-param>
						</xsl:apply-templates>
					</xsl:if>
					<xsl:if test="$chd &gt; 0">
						<xsl:apply-templates select="Air/ChildPrices">
							<xsl:with-param name="nip"><xsl:value-of select="$chd"/></xsl:with-param>
							<xsl:with-param name="type">CHD</xsl:with-param>
						</xsl:apply-templates>
					</xsl:if>
					<xsl:if test="$yth &gt; 0">
						<xsl:apply-templates select="Air/YouthPrices">
							<xsl:with-param name="nip"><xsl:value-of select="$yth"/></xsl:with-param>
							<xsl:with-param name="type">YTH</xsl:with-param>
						</xsl:apply-templates>
					</xsl:if>
					<xsl:if test="$src &gt; 0">
						<xsl:apply-templates select="Air/SeniorPrices">
							<xsl:with-param name="nip"><xsl:value-of select="$src"/></xsl:with-param>
							<xsl:with-param name="type">SRC</xsl:with-param>
						</xsl:apply-templates>
					</xsl:if>
					<xsl:if test="$ins &gt; 0">
						<xsl:apply-templates select="Air/SeatedInfantPrices">
							<xsl:with-param name="nip"><xsl:value-of select="$ins"/></xsl:with-param>
							<xsl:with-param name="type">INS</xsl:with-param>
						</xsl:apply-templates>
					</xsl:if>
					<xsl:if test="$inf &gt; 0">
						<xsl:apply-templates select="Air/InfantPrices">
							<xsl:with-param name="nip"><xsl:value-of select="$inf"/></xsl:with-param>
							<xsl:with-param name="type">INF</xsl:with-param>
						</xsl:apply-templates>
					</xsl:if>
				</PTC_FareBreakdowns>
			</AirItineraryPricingInfo>
		</PricedItinerary>
	</xsl:template>
	<!--****************************************************************************************-->
	<!-- 											 							   -->
	<!--****************************************************************************************-->
	<xsl:template match="AirPrices">
		<xsl:variable name="total">
			<xsl:choose>
				<xsl:when test="contains(Price[ID='23']/Amount,'.')">
					<xsl:choose>
						<xsl:when test="string-length(substring-after(Price[ID='23']/Amount,'.')) = 1">
							<xsl:value-of select="translate(Price[ID='23']/Amount,'.','')" />
							<xsl:text>0</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring-before(Price[ID='23']/Amount,'.')" />
							<xsl:value-of select="substring(substring-after(Price[ID='23']/Amount,'.'),1,2)" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Price[ID='23']/Amount" />
					<xsl:text>00</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="taxes">
			<xsl:choose>
				<xsl:when test="contains(Price[ID='8']/Amount,'.')">
					<xsl:choose>
						<xsl:when test="string-length(substring-after(Price[ID='8']/Amount,'.')) = 1">
							<xsl:value-of select="translate(Price[ID='8']/Amount,'.','')" />
							<xsl:text>0</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring-before(Price[ID='8']/Amount,'.')" />
							<xsl:value-of select="substring(substring-after(Price[ID='8']/Amount,'.'),1,2)" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Price[ID='8']/Amount" />
					<xsl:text>00</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<BaseFare>
			<xsl:attribute name="Amount">
				<xsl:value-of select="$total - $taxes"/>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../FromCurrencyId" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
		</BaseFare>
		<Taxes>
  			<Tax TaxCode="TotalTax">
  				<xsl:attribute name="Amount">
					<xsl:value-of select="$taxes"/>
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="../../FromCurrencyId" />
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
			</Tax>
 		 </Taxes>
		<TotalFare>
			<xsl:attribute name="Amount">
				<xsl:value-of select="$total"/>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="../../FromCurrencyId" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
		</TotalFare>
	</xsl:template>
	<!-- ****************************************************************** -->
	<xsl:template match="AdultPrices | ChildPrices | YouthPrices | SeniorPrices | SeatedInfantPrices | InfantPrices">
		<xsl:param name="nip"/>
		<xsl:param name="type"/>
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code"><xsl:value-of select="$type"/></xsl:attribute>
				<xsl:attribute name="Quantity"><xsl:value-of select="$nip"/></xsl:attribute>
			</PassengerTypeQuantity>
			<!--FareBasisCodes>
				<xsl:for-each select="../Flights/Flight/FlightSegments/FlightSegment">
					<FareBasisCode>
						<xsl:choose>
							<xsl:when test="$type='ADT'"><xsl:value-of select="SelectedBookingClass/AdultRateCode/ID"/></xsl:when>
							<xsl:when test="$type='CHD'"><xsl:value-of select="SelectedBookingClass/ChildRateCode/ID"/></xsl:when>
							<xsl:when test="$type='YTH'"><xsl:value-of select="SelectedBookingClass/YouthRateCode/ID"/></xsl:when>
							<xsl:when test="$type='SRC'"><xsl:value-of select="SelectedBookingClass/SeniorRateCode/ID"/></xsl:when>
							<xsl:when test="$type='INS'"><xsl:value-of select="SelectedBookingClass/SeatedInfantRateCode/ID"/></xsl:when>
							<xsl:when test="$type='INF'"><xsl:value-of select="SelectedBookingClass/InfantRateCode/ID"/></xsl:when>
						</xsl:choose>
					</FareBasisCode>
				</xsl:for-each>
			</FareBasisCodes-->
			<xsl:variable name="total">
				<xsl:variable name="totalall"><xsl:value-of select="Price[ID='23']/Amount * $nip * $nip"/></xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($totalall,'.')">
						<xsl:choose>
							<xsl:when test="string-length(substring-after($totalall,'.')) = 1">
								<xsl:value-of select="translate($totalall,'.','')" />
								<xsl:text>0</xsl:text>
							</xsl:when>
							<xsl:when test="string-length(substring-after($totalall,'.')) &gt; 2">
								<xsl:value-of select="substring-before($totalall,'.')" />
								<xsl:variable name="dec"><xsl:value-of select="substring(substring-after($totalall,'.'),1,3)" /></xsl:variable>
								<xsl:value-of select="substring(($dec + 5),1,2)" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="translate($totalall,'.','')" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$totalall" />
						<xsl:text>00</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="taxes">
				<xsl:variable name="taxesall"><xsl:value-of select="Price[ID='8']/Amount"/></xsl:variable>
				<xsl:choose>
					<xsl:when test="contains($taxesall,'.')">
						<xsl:choose>
							<xsl:when test="string-length(substring-after($taxesall,'.')) = 1">
								<xsl:value-of select="translate($taxesall,'.','')" />
								<xsl:text>0</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring-before($taxesall,'.')" />
								<xsl:value-of select="substring(substring-after($taxesall,'.'),1,2)" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$taxesall" />
						<xsl:text>00</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$total - $taxes"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="../../FromCurrencyId" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</BaseFare>
				<Taxes>
		  			<Tax TaxCode="TotalTax">
		  				<xsl:attribute name="Amount">
							<xsl:value-of select="$taxes"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="../../FromCurrencyId" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
					</Tax>
		 		 </Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$total"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="../../FromCurrencyId" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</TotalFare>
			</PassengerFare>
		</PTC_FareBreakdown>
	</xsl:template>
</xsl:stylesheet>
