<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_AirPriceRS.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 17 Sep 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirPriceRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_AirPriceRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_AirPriceRS>
		</xsl:if>
		<xsl:if test="OTA_AirBookRS">
			<OTA_AirPriceRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code"><xsl:value-of select="OTA_AirBookRS/Errors/Error/@ErrorCode"/></xsl:attribute>
						<xsl:value-of select="OTA_AirBookRS/Errors/Error/ErrorInfo/Message"/>
					</Error>
				</Errors>
			</OTA_AirPriceRS>
		</xsl:if>
		<xsl:if test="OTA_TravelItineraryRS">
			<OTA_AirPriceRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<xsl:for-each select="OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/ReservationItems/Item/Air">
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code"><xsl:value-of select="@ActionCode"/></xsl:attribute>
								<xsl:text>Flight </xsl:text>
								<xsl:value-of select="@FlightNumber"/>
								<xsl:text> </xsl:text>
								<xsl:value-of select="@ActionCode"/>
						</Error>
					</xsl:for-each>
				</Errors>
			</OTA_AirPriceRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirPriceRS">
		<OTA_AirPriceRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="Errors/Error/@Code != ''">
										<xsl:value-of select="Errors/Error/@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:value-of select="Errors/Error" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="not(PricedItineraries/PricedItinerary) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="PricedItineraries/PricedItinerary" />
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirPriceRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="PricedItinerary">
		<PricedItinerary>
			<xsl:attribute name="SequenceNumber">1</xsl:attribute>
			<xsl:apply-templates select="AirItineraryPricingInfo" />
			<xsl:if test="AirItineraryPricingInfo/TPA_Extensions/LastTicketingDate!=''">
				<TicketingInfo>
					<xsl:attribute name="TicketTimeLimit">
						<xsl:value-of select="AirItineraryPricingInfo/TPA_Extensions/LastTicketingDate"/>
            <xsl:text>T00:00:00</xsl:text>
					</xsl:attribute>
				</TicketingInfo>
			</xsl:if>
		</PricedItinerary>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Total Fare and PTC_FareBreakdown 				    -->
	<!-- ************************************************************** -->
	<xsl:template match="AirItineraryPricingInfo">
		<AirItineraryPricingInfo>
			<!--xsl:attribute name="PricingSource">
				<xsl:value-of select="@PricingSource" />
			</xsl:attribute-->
			<ItinTotalFare>
				<xsl:variable name="dec"><xsl:value-of select="string-length(substring-after(ItinTotalFare/TotalFare/@Amount,'.'))"/></xsl:variable>
				<xsl:variable name="amtbase1">
					<xsl:apply-templates select="PTC_FareBreakdown[1]" mode="basefare">
						<xsl:with-param name="total">0</xsl:with-param>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:variable name="amtbase"><xsl:value-of select="substring-before($amtbase1,'/')" /></xsl:variable>
				<xsl:variable name="amttot">
					<xsl:value-of select="translate(ItinTotalFare/TotalFare/@Amount,'.','')" />
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$amtbase" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dec"/></xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount"><xsl:value-of select="$amttot - $amtbase"/></xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dec"/></xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$amttot" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dec"/></xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="PTC_FareBreakdown" mode="PaxType" />
			</PTC_FareBreakdowns>
			<FareInfos>
				<xsl:variable name="fareref" select="TPA_Extensions/AlternateBooking" />
				<xsl:apply-templates select="../../../OTA_AirPriceRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"
					mode="fareinfos">
					<xsl:with-param name="fareref" select="$fareref" />
				</xsl:apply-templates>
			</FareInfos>
		</AirItineraryPricingInfo>
	</xsl:template>
	<!-- ******************************************************************** -->
	<xsl:template match="Tax">
		<Tax>
			<xsl:attribute name="TaxCode">
				<xsl:value-of select="@TaxCode" />
			</xsl:attribute>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(@Amount,'.','')" />
			</xsl:attribute>
		</Tax>
	</xsl:template>
	<!-- ************************************************************** -->
	<!-- Fare Breakdown per pax type					    -->
	<!-- ************************************************************** -->
	<xsl:template match="PTC_FareBreakdown" mode="PaxType">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="PaxTypeQuantity">
			<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
		</xsl:variable>
		<xsl:variable name="dec"><xsl:value-of select="string-length(substring-after(PassengerFare/TotalFare/@Amount,'.'))"/></xsl:variable>
		<PTC_FareBreakdown>
			<!--xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="@PricingSource">
						<xsl:value-of select="@PricingSource" />
					</xsl:when>
					<xsl:otherwise>Published</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute-->
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="PassengerTypeQuantity/@Code = 'C09'">CHD</xsl:when>
						<xsl:otherwise><xsl:value-of select="PassengerTypeQuantity/@Code" /></xsl:otherwise>
					</xsl:choose>
					<!--xsl:value-of select="../../../../OTA_AirPriceRQ/TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[position()=$pos]/@Code"/-->
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="PassengerTypeQuantity/@Quantity" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:for-each select="FareBasis">
					<FareBasisCode><xsl:value-of select="@Code"/></FareBasisCode>
				</xsl:for-each>
			</FareBasisCodes>
			<PassengerFare>
				<xsl:variable name="tbase">
					<xsl:choose>
						<xsl:when test="PassengerFare/EquivFare/@Amount!=''">
							<xsl:value-of select="translate(PassengerFare/EquivFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="ttot">
					<xsl:value-of select="translate(PassengerFare/TotalFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$tbase" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PassengerFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount"><xsl:value-of select="$ttot - $tbase"/></xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PassengerFare/TotalFare/@CurrencyCode" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$dec"/>
						</xsl:attribute>
					</Tax>
					<xsl:for-each select="PassengerFare/Taxes/Tax">
						<Tax>
							<xsl:attribute name="TaxCode"><xsl:value-of select="@TaxCode"/></xsl:attribute>
							<xsl:attribute name="Amount"><xsl:value-of select="translate(@Amount,'.','') * $PaxTypeQuantity"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="../../TotalFare/@CurrencyCode" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="$dec"/>
							</xsl:attribute>
						</Tax>
					</xsl:for-each>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="$ttot" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PassengerFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$dec"/>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
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
	<!--Fare Rule Info section							    -->
	<!-- ************************************************************** -->
	<xsl:template match="FlightSegment" mode="fareinfos">
		<xsl:param name="fareref" />
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<FareInfo>
			<DepartureDate>
				<xsl:value-of select="@DepartureDateTime" />
			</DepartureDate>
			<FareReference>
				<xsl:choose>
					<xsl:when test="$fareref/Segment">
						<xsl:value-of select="substring($fareref/Segment[position()=$pos]/@Class,2,1)" />
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="@ResBookDesigCode"/></xsl:otherwise>
				</xsl:choose>
			</FareReference>
			<RuleInfo />
			<FilingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="MarketingAirline/@Code" />
				</xsl:attribute>
			</FilingAirline>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="DepartureAirport/@LocationCode" />
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="ArrivalAirport/@LocationCode" />
				</xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:template>
<!-- ******************************************************************** -->
	<xsl:template match="PTC_FareBreakdown" mode="basefare">
		<xsl:param name="total" />
		<xsl:variable name="thistotal">
			<xsl:choose>
				<xsl:when test="PassengerFare/EquivFare/@Amount != ''">
					<xsl:value-of select="translate(PassengerFare/EquivFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(PassengerFare/BaseFare/@Amount,'.','') * PassengerTypeQuantity/@Quantity" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::PTC_FareBreakdown[1]" mode="basefare">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>
</xsl:stylesheet>
