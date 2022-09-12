<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirPriceIPRS2.xsl 													-->
	<!-- ================================================================== -->
	<!--Date: 17 Nov 2011 -  Shashin created to fix the issue in Avianca LowfareFlights		-->
	<!-- Date: 17 Nov 2011 -  Shashin													-->
	<!-- ================================================================== -->
	<xsl:variable name="segcount" select="count(//pricingGroupLevelGroup/segmentInformation)"/>
	<xsl:variable name="tktcount" select="count(//pricingGroupLevelGroup)"/>
	<xsl:variable name="count1" select="($segcount div $tktcount)"/>
	<xsl:variable name="loop">
		<xsl:choose>
			<xsl:when test="//pricingGroupLevelGroup/warningInformation/warningText/errorFreeText = 'LOWEST SOLD OUT//TRY WAIT LIST'">
				<xsl:value-of select="(count(//pricingGroupLevelGroup) div 2) + 1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="count(//pricingGroupLevelGroup) + 1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_InformativePricingWithoutPNRReply"/>
		<xsl:apply-templates select="Fare_InformativeBestPricingWithoutPNRReply"/>
	</xsl:template>
	<xsl:template match="Fare_InformativePricingWithoutPNRReply | Fare_InformativeBestPricingWithoutPNRReply">
		<OTA_AirPriceRS>
			<xsl:attribute name="Version">2003.2</xsl:attribute>
			<xsl:choose>
				<xsl:when test="applicationError">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Amadeus</xsl:attribute>
							<xsl:attribute name="Code"><xsl:value-of select="applicationError/applicationErrorInfo/applicationErrorDetail/applicationErrorCode"/></xsl:attribute>
							<xsl:value-of select="applicationError/errorText/errorFreeText"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="errorGroup">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Amadeus</xsl:attribute>
							<xsl:attribute name="Code"><xsl:value-of select="errorGroup/errorCode/errorDetails/errorCode"/></xsl:attribute>
							<xsl:value-of select="errorGroup/errorMessage/freeText"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<PricedItineraries>
						<PricedItinerary>
							<xsl:attribute name="SequenceNumber">1</xsl:attribute>
							<AirItineraryPricingInfo>
								<xsl:choose>
									<xsl:when test="mainGroup/pricingGroupLevelGroup[2] != ''">
										<xsl:choose>
											<xsl:when test="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/textData[contains(freeText,'PRIVATE RATES USED')]">
												<xsl:attribute name="PricingSource">Private</xsl:attribute>
											</xsl:when>
											<xsl:when test="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/pricingIndicators/priceTariffType='P'">
												<xsl:attribute name="PricingSource">Private</xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="PricingSource">Published</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
										<ItinTotalFare>
											<xsl:variable name="bf">
												<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup" mode="totalbase">
													<xsl:with-param name="sum">0</xsl:with-param>
													<xsl:with-param name="loop">
														<xsl:value-of select="$loop"/>
													</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
												</xsl:apply-templates>
											</xsl:variable>
											<xsl:variable name="tf">
												<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup" mode="totalprice">
													<xsl:with-param name="sum">0</xsl:with-param>
													<xsl:with-param name="loop">
														<xsl:value-of select="$loop"/>
													</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
												</xsl:apply-templates>
											</xsl:variable>
											<xsl:variable name="curt">
												<xsl:choose>
													<xsl:when test="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
														<xsl:value-of select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/currency"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable name="dect">
												<xsl:choose>
													<xsl:when test="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
														<xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.'))"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.'))"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<BaseFare>
												<xsl:attribute name="Amount"><xsl:value-of select="$bf"/></xsl:attribute>
												<xsl:attribute name="CurrencyCode"><xsl:value-of select="$curt"/></xsl:attribute>
												<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dect"/></xsl:attribute>
											</BaseFare>
											<Taxes>
												<Tax>
													<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
													<xsl:attribute name="Amount"><xsl:value-of select="$tf - $bf"/></xsl:attribute>
													<xsl:attribute name="CurrencyCode"><xsl:value-of select="$curt"/></xsl:attribute>
													<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dect"/></xsl:attribute>
												</Tax>
											</Taxes>
											<xsl:variable name="qfees">
												<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/textData[freeTextQualification/informationType='15']" mode="qfees">
													<xsl:with-param name="sum">0</xsl:with-param>
													<xsl:with-param name="loop">
														<xsl:value-of select="$loop"/>
													</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
												</xsl:apply-templates>
											</xsl:variable>
											<xsl:if test="$qfees>0">
												<Fees>
													<Fee>
														<xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
														<xsl:attribute name="Amount"><xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/></xsl:attribute>
														<xsl:attribute name="CurrencyCode"><xsl:value-of select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/></xsl:attribute>
														<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.'))"/></xsl:attribute>
													</Fee>
												</Fees>
											</xsl:if>
											<TotalFare>
												<xsl:attribute name="Amount"><xsl:value-of select="$tf"/></xsl:attribute>
												<xsl:attribute name="CurrencyCode"><xsl:value-of select="$curt"/></xsl:attribute>
												<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dect"/></xsl:attribute>
											</TotalFare>
										</ItinTotalFare>
										<PTC_FareBreakdowns>
											<xsl:for-each select="AirTravelerAvail/PassengerTypeQuantity">
												<xsl:variable name="pos">
													<xsl:value-of select="position()"/>
												</xsl:variable>
												<xsl:variable name="paxtype">
													<xsl:choose>
														<xsl:when test="@Code = 'SRC'">YCD</xsl:when>
														<xsl:when test="@Code = 'ITF'">INF</xsl:when>
														<xsl:when test="@Code = 'ITS'">INS</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="@Code"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>
												<xsl:apply-templates select="../../mainGroup/pricingGroupLevelGroup[position()=$pos]" mode="paxtypes">
													<xsl:with-param name="paxtype">
														<xsl:value-of select="$paxtype"/>
													</xsl:with-param>
												</xsl:apply-templates>
											</xsl:for-each>
										</PTC_FareBreakdowns>
										<FareInfos>
											<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/segmentLevelGroup" mode="fareinfo"/>
										</FareInfos>
									</xsl:when>
									<xsl:otherwise>
										<xsl:choose>
											<xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'PRIVATE RATES USED')]">
												<xsl:attribute name="PricingSource">Private</xsl:attribute>
											</xsl:when>
											<xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/priceTariffType='P'">
												<xsl:attribute name="PricingSource">Private</xsl:attribute>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="PricingSource">Published</xsl:attribute>
											</xsl:otherwise>
										</xsl:choose>
										<ItinTotalFare>
											<xsl:variable name="bf">
												<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup" mode="totalbase">
													<xsl:with-param name="sum">0</xsl:with-param>
													<xsl:with-param name="loop">
														<xsl:value-of select="$loop"/>
													</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
												</xsl:apply-templates>
											</xsl:variable>
											<xsl:variable name="tf">
												<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup" mode="totalprice">
													<xsl:with-param name="sum">0</xsl:with-param>
													<xsl:with-param name="loop">
														<xsl:value-of select="$loop"/>
													</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
												</xsl:apply-templates>
											</xsl:variable>
											<xsl:variable name="curt">
												<xsl:choose>
													<xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
														<xsl:value-of select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/currency"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable name="dect">
												<xsl:choose>
													<xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
														<xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.'))"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.'))"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<BaseFare>
												<xsl:attribute name="Amount"><xsl:value-of select="$bf"/></xsl:attribute>
												<xsl:attribute name="CurrencyCode"><xsl:value-of select="$curt"/></xsl:attribute>
												<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dect"/></xsl:attribute>
											</BaseFare>
											<Taxes>
												<Tax>
													<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
													<xsl:attribute name="Amount"><xsl:value-of select="$tf - $bf"/></xsl:attribute>
													<xsl:attribute name="CurrencyCode"><xsl:value-of select="$curt"/></xsl:attribute>
													<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dect"/></xsl:attribute>
												</Tax>
											</Taxes>
											<xsl:variable name="qfees">
												<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[freeTextQualification/informationType='15']" mode="qfees">
													<xsl:with-param name="sum">0</xsl:with-param>
													<xsl:with-param name="loop">
														<xsl:value-of select="$loop"/>
													</xsl:with-param>
													<xsl:with-param name="pos">1</xsl:with-param>
												</xsl:apply-templates>
											</xsl:variable>
											<xsl:if test="$qfees>0">
												<Fees>
													<Fee>
														<xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
														<xsl:attribute name="Amount"><xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/></xsl:attribute>
														<xsl:attribute name="CurrencyCode"><xsl:value-of select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/></xsl:attribute>
														<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.'))"/></xsl:attribute>
													</Fee>
												</Fees>
											</xsl:if>
											<TotalFare>
												<xsl:attribute name="Amount"><xsl:value-of select="$tf"/></xsl:attribute>
												<xsl:attribute name="CurrencyCode"><xsl:value-of select="$curt"/></xsl:attribute>
												<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dect"/></xsl:attribute>
											</TotalFare>
										</ItinTotalFare>
										<PTC_FareBreakdowns>
											<xsl:for-each select="AirTravelerAvail/PassengerTypeQuantity">
												<xsl:variable name="pos">
													<xsl:value-of select="position()"/>
												</xsl:variable>
												<xsl:variable name="paxtype">
													<xsl:choose>
														<xsl:when test="@Code = 'SRC'">YCD</xsl:when>
														<xsl:when test="@Code = 'ITF'">INF</xsl:when>
														<xsl:when test="@Code = 'ITS'">INS</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="@Code"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>
												<xsl:apply-templates select="../../mainGroup/pricingGroupLevelGroup[position()=$pos]" mode="paxtypes">
													<xsl:with-param name="paxtype">
														<xsl:value-of select="$paxtype"/>
													</xsl:with-param>
												</xsl:apply-templates>
											</xsl:for-each>
										</PTC_FareBreakdowns>
										<FareInfos>
											<xsl:choose>
												<xsl:when test="mainGroup/pricingGroupLevelGroup[2] != ''">
													<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[2]/fareInfoGroup/segmentLevelGroup" mode="fareinfo"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:apply-templates select="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/segmentLevelGroup" mode="fareinfo"/>
												</xsl:otherwise>
											</xsl:choose>
										</FareInfos>
									</xsl:otherwise>
								</xsl:choose>
							</AirItineraryPricingInfo>
							<xsl:for-each select="mainGroup/pricingGroupLevelGroup">
								<Notes>
									<xsl:for-each select="fareInfoGroup/textData[freeTextQualification[informationType='15' and textSubjectQualifier='4']]/freeText">
										<xsl:value-of select="."/>
									</xsl:for-each>
									<xsl:if test="fareInfoGroup/surchargesGroup/taxesAmount/taxDetails[countryCode='XF']">
										<xsl:value-of select="' XF '"/>
										<xsl:value-of select="fareInfoGroup/surchargesGroup/pfcAmount/monetaryDetails/location"/>
										<xsl:value-of select="fareInfoGroup/surchargesGroup/pfcAmount/monetaryDetails/amount"/>
										<xsl:for-each select="fareInfoGroup/surchargesGroup/pfcAmount/otherMonetaryDetails">
											<xsl:value-of select="concat(location,amount)"/>
										</xsl:for-each>
									</xsl:if>
								</Notes>
							</xsl:for-each>
							<TicketingInfo>
								<xsl:attribute name="TicketTimeLimit"><xsl:text>20</xsl:text><xsl:choose><xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate!=''"><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,5,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,3,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,1,2)"/></xsl:when><xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]"><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]/freeText,6,2)"/><xsl:text>-</xsl:text><xsl:call-template name="month"><xsl:with-param name="month"><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]/freeText,3,3)"/></xsl:with-param></xsl:call-template><xsl:text>-</xsl:text><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'FARE NOT VALID UNTIL TICKETED')]/freeText,1,2)"/></xsl:when><xsl:when test="mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/textData[contains(freeText,'DATE OF ORIGIN')]"><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,5)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,3,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(mainGroup/pricingGroupLevelGroup[1]/fareInfoGroup/pricingIndicators/productDateTimeDetails/departureDate,1,2)"/></xsl:when></xsl:choose><xsl:text>T23:59:00</xsl:text></xsl:attribute>
							</TicketingInfo>
						</PricedItinerary>
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirPriceRS>
	</xsl:template>
	<xsl:template match="fareInfoGroup" mode="totalbase">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="../numberOfPax/segmentControlDetails/numberOfUnits"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:choose>
				<xsl:when test="fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
					<xsl:value-of select="translate(fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.','') * $nopt"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.','') * $nopt"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../pricingGroupLevelGroup[$pos + 1]">
				<xsl:apply-templates select="../../pricingGroupLevelGroup[$pos + 1]/fareInfoGroup" mode="totalbase">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="fareInfoGroup" mode="totalprice">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="../numberOfPax/segmentControlDetails/numberOfUnits"/>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="translate(fareAmount/otherMonetaryDetails[typeQualifier = '712']/amount,'.','') * $nopt"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../pricingGroupLevelGroup[$pos + 1]">
				<xsl:apply-templates select="../../pricingGroupLevelGroup[$pos + 1]/fareInfoGroup" mode="totalprice">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="warningInformation">
		<Warnings>
			<Warning>
				<xsl:attribute name="Type">Amadeus</xsl:attribute>
				<xsl:value-of select="warningText/errorFreeText"/>
			</Warning>
		</Warnings>
	</xsl:template>
	<xsl:template match="pricingGroupLevelGroup" mode="paxtypes">
		<xsl:param name="paxtype"/>
		<xsl:variable name="nip">
			<xsl:value-of select="numberOfPax/segmentControlDetails/numberOfUnits"/>
		</xsl:variable>
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code"><xsl:value-of select="$paxtype"/></xsl:attribute>
				<xsl:attribute name="Quantity"><xsl:value-of select="$nip"/></xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:apply-templates select="fareInfoGroup/segmentLevelGroup/fareBasis/additionalFareDetails/rateClass" mode="farebasis"/>
			</FareBasisCodes>
			<PassengerFare>
				<xsl:variable name="bfpax">
					<xsl:choose>
						<xsl:when test="fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
							<xsl:value-of select="translate(fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/amount,'.','') * $nip"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="translate(fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.','') * $nip"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="tfpax">
					<xsl:value-of select="translate(fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = '712']/amount,'.','') * $nip"/>
				</xsl:variable>
				<xsl:variable name="cur">
					<xsl:choose>
						<xsl:when test="fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']">
							<xsl:value-of select="fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = 'E']/currency"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="dec">
					<xsl:value-of select="string-length(substring-after(fareInfoGroup/fareAmount/otherMonetaryDetails[typeQualifier = '712']/amount,'.'))"/>
				</xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount"><xsl:value-of select="$bfpax"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="$cur"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dec"/></xsl:attribute>
				</BaseFare>
				<Taxes>
					<Tax>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount"><xsl:value-of select="$tfpax - $bfpax"/></xsl:attribute>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="$cur"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dec"/></xsl:attribute>
					</Tax>
					<xsl:apply-templates select="fareInfoGroup/surchargesGroup/taxesAmount/taxDetails">
						<xsl:with-param name="nip">
							<xsl:value-of select="$nip"/>
						</xsl:with-param>
					</xsl:apply-templates>
				</Taxes>
				<xsl:variable name="qfees">
					<xsl:variable name="tot1">
						<xsl:choose>
							<xsl:when test="contains(fareInfoGroup/textData[freeTextQualification/informationType='15']/freeText,' Q') and contains(substring-after(fareInfoGroup/textData[freeTextQualification/informationType='15']/freeText,' Q'),'.')">
								<xsl:call-template name="qfeecalc">
									<xsl:with-param name="fca">
										<xsl:value-of select="fareInfoGroup/textData[freeTextQualification/informationType='15']/freeText"/>
									</xsl:with-param>
									<xsl:with-param name="qfeesum">0</xsl:with-param>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:value-of select="$tot1 * $nip"/>
				</xsl:variable>
				<xsl:if test="$qfees>0">
					<Fees>
						<Fee>
							<xsl:attribute name="FeeCode">Q Fees</xsl:attribute>
							<xsl:attribute name="Amount"><xsl:value-of select="translate(format-number($qfees,'0.00'),'.','')"/></xsl:attribute>
							<xsl:attribute name="CurrencyCode"><xsl:value-of select="fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/currency"/></xsl:attribute>
							<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(fareInfoGroup/fareAmount/monetaryDetails[typeQualifier = 'B']/amount,'.'))"/></xsl:attribute>
						</Fee>
					</Fees>
				</xsl:if>
				<TotalFare>
					<xsl:attribute name="Amount"><xsl:value-of select="$tfpax"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="$cur"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$dec"/></xsl:attribute>
				</TotalFare>
			</PassengerFare>
		</PTC_FareBreakdown>
	</xsl:template>
	<xsl:template match="taxDetails">
		<xsl:param name="nip"/>
		<Tax>
			<xsl:attribute name="TaxCode"><xsl:value-of select="countryCode"/></xsl:attribute>
			<xsl:attribute name="Amount"><xsl:value-of select="translate(rate,'.','') * $nip"/></xsl:attribute>
		</Tax>
	</xsl:template>
	<xsl:template match="rateClass" mode="farebasis">
		<FareBasisCode>
			<xsl:value-of select="."/>
		</FareBasisCode>
	</xsl:template>
	<xsl:template match="segmentLevelGroup" mode="fareinfo">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<FareInfo>
			<xsl:variable name="segment" select="../../../../FlightSegments/FlightSegment[position()=$pos]"/>
			<DepartureDate>
				<xsl:value-of select="$segment/@DepartureDateTime"/>
			</DepartureDate>
			<FareReference>
				<xsl:value-of select="fareBasis/additionalFareDetails/secondRateClass"/>
			</FareReference>
			<xsl:if test="../textData/freeText = 'NON-REFUNDABLE'">
				<RuleInfo>
					<ChargesRules>
						<VoluntaryChanges>
							<Penalty PenaltyType="Ticket Is Non Refundable"/>
						</VoluntaryChanges>
					</ChargesRules>
				</RuleInfo>
			</xsl:if>
			<FilingAirline>
				<xsl:value-of select="$segment/MarketingAirline/@Code"/>
			</FilingAirline>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$segment/DepartureAirport/@LocationCode"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$segment/ArrivalAirport/@LocationCode"/></xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:template>
	<xsl:template match="textData" mode="qfees">
		<xsl:param name="sum"/>
		<xsl:param name="loop"/>
		<xsl:param name="pos"/>
		<xsl:variable name="nopt">
			<xsl:value-of select="count(../../numberOfPax/segmentControlDetails/numberOfUnits)"/>
		</xsl:variable>
		<xsl:variable name="tot1">
			<xsl:choose>
				<xsl:when test="contains(freeText,' Q') and contains(substring-after(freeText,' Q'),'.')">
					<xsl:call-template name="qfeecalc">
						<xsl:with-param name="fca">
							<xsl:value-of select="freeText"/>
						</xsl:with-param>
						<xsl:with-param name="qfeesum">0</xsl:with-param>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tot">
			<xsl:value-of select="$tot1 * $nopt"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="($pos &lt; $loop) and ../../../pricingGroupLevelGroup[$pos + 1]">
				<xsl:apply-templates select="../../../pricingGroupLevelGroup[$pos + 1]/fareInfoGroup/textData[freeTextQualification/informationType='15']" mode="qfees">
					<xsl:with-param name="sum">
						<xsl:value-of select="$tot + $sum"/>
					</xsl:with-param>
					<xsl:with-param name="loop">
						<xsl:value-of select="$loop"/>
					</xsl:with-param>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos + 1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$tot + $sum"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="qfeecalc">
		<xsl:param name="fca"/>
		<xsl:param name="qfeesum"/>
		<xsl:variable name="q1">
			<xsl:value-of select="substring-after($fca,' Q')"/>
		</xsl:variable>
		<xsl:if test="$q1!=''">
			<xsl:variable name="q2">
				<xsl:value-of select="substring-before($q1,'.')"/>
			</xsl:variable>
			<xsl:if test="$q2!=''">
				<xsl:variable name="q3">
					<xsl:value-of select="substring(substring-after($q1,'.'),1,2)"/>
				</xsl:variable>
				<xsl:if test="$q3!=''">
					<xsl:variable name="q4">
						<xsl:value-of select="$q2"/>
						<xsl:value-of select="$q3"/>
					</xsl:variable>
					<xsl:variable name="q5">
						<xsl:value-of select="translate($q4,'0123456789','')"/>
					</xsl:variable>
					<xsl:variable name="totqfee">
						<xsl:choose>
							<xsl:when test="$q5=''">
								<xsl:value-of select="$q2"/>.<xsl:value-of select="$q3"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="newfee">
						<xsl:value-of select="$qfeesum + $totqfee"/>
					</xsl:variable>
					<xsl:call-template name="qfeecalc">
						<xsl:with-param name="fca">
							<xsl:value-of select="$q1"/>
						</xsl:with-param>
						<xsl:with-param name="qfeesum">
							<xsl:value-of select="$newfee"/>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:if>
			</xsl:if>
		</xsl:if>
		<xsl:if test="substring-after($fca,' Q') = ''">
			<xsl:value-of select="$qfeesum"/>
		</xsl:if>
	</xsl:template>
	<!--****************************************************************************************-->
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
