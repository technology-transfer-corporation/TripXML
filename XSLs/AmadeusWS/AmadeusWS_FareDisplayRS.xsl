<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_FareDisplayRS.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 20 Jun 2009 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:variable name="depcity"><xsl:value-of select="Fare_DisplayFaresForCityPairReply/flightDetails/odiGrp/originDestination/origin"/></xsl:variable>
	<xsl:variable name="arrcity"><xsl:value-of select="Fare_DisplayFaresForCityPairReply/flightDetails/odiGrp/originDestination/destination"/></xsl:variable>
	<xsl:variable name="start">
		<xsl:value-of select="Fare_DisplayFaresForCityPairReply/flightDetails/odiGrp/flightDateAndTime/dateAndTimeDetails[1]/date"/>
	</xsl:variable>
	<xsl:variable name="end">
		<xsl:value-of select="Fare_DisplayFaresForCityPairReply/flightDetails/odiGrp/flightDateAndTime/dateAndTimeDetails[2]/date"/>
	</xsl:variable>
	<xsl:variable name="airline">
		<xsl:value-of select="Fare_DisplayFaresForCityPairReply/flightDetails/transportService/companyIdentification/marketingCompany"/>
	</xsl:variable>
	<xsl:variable name="currency">
		<xsl:value-of select="Fare_DisplayFaresForCityPairReply/flightDetails/amountConversion/conversionRateDetails/currency"/>
	</xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_DisplayFaresForCityPairReply" />
		<xsl:apply-templates select="Fare_ListFareRequestTypesPlus_Reply"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="Fare_DisplayFaresForCityPairReply">
		<OTA_AirFareDisplayRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="errorInfo">
					<Errors>
						<Error Type="Amadeus">
							<xsl:attribute name="Code">
								<xsl:value-of select="errorInfo/rejectErrorCode/errorDetails/errorCode" />
							</xsl:attribute>
							<xsl:for-each select="errorInfo/errorFreeText/freeText">
								<xsl:if test="position()>1"><xsl:text> </xsl:text></xsl:if>
								<xsl:value-of select="." />
							</xsl:for-each>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="flightDetails/itemGrp">
							<Success />
							<FareDisplayInfos>
								<xsl:apply-templates select="flightDetails/itemGrp" />
							</FareDisplayInfos>	
						</xsl:when>
						<xsl:otherwise>
							<Errors>
								<Error Type="Amadeus">
									<xsl:value-of select="CAPI_Messages[LineType='W']/Text"/>
								</Error>
							</Errors>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirFareDisplayRS>
	</xsl:template>
	
	<xsl:template match="Fare_ListFareRequestTypesPlus_Reply">
		<OTA_AirFareDisplayRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<Errors>
				<Error Type="Amadeus">
					<xsl:value-of select="CAPI_Messages[LineType='W']/Text"/>
				</Error>
			</Errors>
		</OTA_AirFareDisplayRS>
	</xsl:template>


	<xsl:template match="itemGrp">
		<FareDisplayInfo>
			<xsl:attribute name="FareRPH"><xsl:value-of select="itemNb/itemNumberDetails/number"/></xsl:attribute>
			<xsl:attribute name="FareApplicationType">
				<xsl:choose>
					<xsl:when test="monetaryGrp/monetaryValues/monetaryDetails/typeQualifier = '701' ">Return</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:choose>
					<xsl:when test="productAvailabilityStatus/bookingClassDetails/designator !='' ">
						<xsl:value-of select="productAvailabilityStatus/bookingClassDetails/designator"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(fareQualifItem/additionalFareDetails/rateClass,1,1)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<TravelDates>
				<xsl:attribute name="DepartureDate">
					<xsl:choose>
						<xsl:when test="count(monetaryGrp/qualifGrp/qualifDateFlightMovement/dateAndTimeDetails) = 2">
							<xsl:variable name="dep"><xsl:value-of select="monetaryGrp/qualifGrp/qualifDateFlightMovement/dateAndTimeDetails[1]/date"/></xsl:variable>
							<xsl:text>20</xsl:text>
							<xsl:variable name="depM"><xsl:value-of select="substring($dep,3,2)"/></xsl:variable>
							<xsl:variable name="startM"><xsl:value-of select="substring($start,3,2)"/></xsl:variable>
							<xsl:variable name="startY"><xsl:value-of select="substring($start,5,2)"/></xsl:variable>
							<xsl:choose>
								<xsl:when test="$depM > $startM"><xsl:value-of select="$startY - 1"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="$startY"/></xsl:otherwise>
							</xsl:choose>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($dep,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($dep,1,2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>20</xsl:text>
							<xsl:value-of select="substring($start,5)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($start,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($start,1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="ArrivalDate">
					<xsl:variable name="arr">
						<xsl:choose>
							<xsl:when test="count(monetaryGrp/qualifGrp/qualifDateFlightMovement/dateAndTimeDetails) = 2">
								<xsl:value-of select="monetaryGrp/qualifGrp/qualifDateFlightMovement/dateAndTimeDetails[2]/date"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="monetaryGrp/qualifGrp/qualifDateFlightMovement/dateAndTimeDetails[1]/date"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$arr!=''">
							<xsl:text>20</xsl:text>
							<xsl:variable name="arrM"><xsl:value-of select="substring($arr,3,2)"/></xsl:variable>
							<xsl:variable name="endM"><xsl:value-of select="substring($end,3,2)"/></xsl:variable>
							<xsl:variable name="endY"><xsl:value-of select="substring($end,5,2)"/></xsl:variable>
							<xsl:choose>
								<xsl:when test="arrM > endM"><xsl:value-of select="$endY + 1"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="$endY"/></xsl:otherwise>
							</xsl:choose>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($arr,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($arr,1,2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>20</xsl:text>
							<xsl:value-of select="substring($end,5)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($end,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($end,1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</TravelDates>
			<FareReference><xsl:value-of select="fareQualifItem/additionalFareDetails/rateClass"/></FareReference>
			<RuleInfo>
				<xsl:choose>
					<xsl:when test="AdvancePurchase='-'">
						<ResTicketingRules>
							<AdvResTicketing>
								<xsl:attribute name="AdvTicketingInd">None</xsl:attribute>
							</AdvResTicketing>
						</ResTicketingRules>
					</xsl:when>
					<xsl:when test="AdvancePurchase='@'">
						<ResTicketingRules>
							<AdvResTicketing>
								<xsl:attribute name="AdvTicketingInd">Check rules</xsl:attribute>
							</AdvResTicketing>
						</ResTicketingRules>
					</xsl:when>
					<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='702'">
						<ResTicketingRules>
							<AdvResTicketing>
								<AdvTicketing>
									<xsl:attribute name="FromDepartPeriod">
										<xsl:value-of select="monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='702']/qualifSelection/selectionDetails/optionInformation"/>
									</xsl:attribute>
									<xsl:attribute name="FromDepartUnit">Days</xsl:attribute>
								</AdvTicketing>
							</AdvResTicketing>
						</ResTicketingRules>
					</xsl:when>
				</xsl:choose>
				<LengthOfStayRules>
					<MinimumStay>
						<xsl:choose>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='752' or monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='753'">
								<xsl:attribute name="MinStay">None</xsl:attribute>
							</xsl:when>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='MNM'">
								<xsl:attribute name="MinStay">
									<xsl:value-of select="monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='MNM']/qualifSelection/selectionDetails/optionInformation"/>
								</xsl:attribute>
								<xsl:attribute name="StayUnit">Months</xsl:attribute>
							</xsl:when>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='745' and monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='745']/qualifSelection/selectionDetails/optionInformation = 'SU'">
								<xsl:attribute name="MinStay">1</xsl:attribute>
								<xsl:attribute name="StayUnit">SUN</xsl:attribute>
							</xsl:when>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='745'">
								<xsl:attribute name="MinStay">
									<xsl:value-of select="monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='745']/qualifSelection/selectionDetails/optionInformation"/>
								</xsl:attribute>
								<xsl:attribute name="StayUnit">Days</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</MinimumStay>
					<MaximumStay>
						<xsl:choose>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='752' or monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='751'">
								<xsl:attribute name="MaxStay">None</xsl:attribute>
							</xsl:when>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='794'">
								<xsl:attribute name="MaxStay">
									<xsl:value-of select="monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='794']/qualifSelection/selectionDetails/optionInformation"/>
								</xsl:attribute>
								<xsl:attribute name="StayUnit">Months</xsl:attribute>
							</xsl:when>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='794' and monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='794']/qualifSelection/selectionDetails/optionInformation = 'SU'">
							<xsl:attribute name="MaxStay">1</xsl:attribute>
								<xsl:attribute name="StayUnit">SUN</xsl:attribute>
							</xsl:when>
							<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='746'">
								<xsl:attribute name="MaxStay">
									<xsl:value-of select="monetaryGrp/qualifGrp[qualificationFare/discountDetails/fareQualifier='746']/qualifSelection/selectionDetails/optionInformation"/>
								</xsl:attribute>
								<xsl:attribute name="StayUnit">Days</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</MaximumStay>
				</LengthOfStayRules>
				<xsl:if test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier ='741' or monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier ='736' or monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier ='798' or monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier ='797'">
					<ChargesRules>
						<VoluntaryRefunds>
							<Penalty>
								<xsl:attribute name="PenaltyType">
									<xsl:choose>
										<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='741'">Non-refundable</xsl:when>
										<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='736'">Refundable</xsl:when>
										<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='798'">Cancellation/change penalty</xsl:when>
										<xsl:when test="monetaryGrp/qualifGrp/qualificationFare/discountDetails/fareQualifier='797'">Penalty</xsl:when>
									</xsl:choose>
								</xsl:attribute>
								<xsl:choose>
									<xsl:when test="substring(Penalty,1,1)='P'">
										<xsl:attribute name="Percent">
											<xsl:value-of select="substring-after(Penalty,'P')"/>
										</xsl:attribute>
									</xsl:when>
									<xsl:when test="Penalty != 'NRF' and Penalty != '+' and Penalty != '@' and Penalty != '-'">
										<xsl:attribute name="Amount">
											<xsl:value-of select="Penalty"/>
											<xsl:text>00</xsl:text>
										</xsl:attribute>
										<xsl:attribute name="CurrencyCode"><xsl:value-of select="../CAPI_ROEPlus/Currency"/></xsl:attribute>
										<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
									</xsl:when>
								</xsl:choose>
							</Penalty>
						</VoluntaryRefunds>
					</ChargesRules>
				</xsl:if>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="transportServiceItem/companyIdentification/marketingCompany !='' ">
							<xsl:value-of select="transportServiceItem/companyIdentification/marketingCompany"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="$airline"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</FilingAirline>
			<DepartureLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$depcity"/></xsl:attribute>
			</DepartureLocation>
			<ArrivalLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$arrcity"/></xsl:attribute>
			</ArrivalLocation>
			<xsl:if test="AppliDateCode1 != '' or AppliDateCode2 != '' or AppliDateCode3 != ''">
				<Restrictions>
					<Restriction>
						<xsl:if test="AppliDateCode1 = 'A' or AppliDateCode2 = 'A' or AppliDateCode3 = 'A'">
							<DateRestriction>
								<xsl:attribute name="Application">Ticket after</xsl:attribute>
								<xsl:attribute name="StartDate">
									<xsl:variable name="adate">
										<xsl:choose>
											<xsl:when test="AppliDateCode1 = 'A'"><xsl:value-of select="ApplicationDate1"/></xsl:when>
											<xsl:when test="AppliDateCode2 = 'A'"><xsl:value-of select="ApplicationDate2"/></xsl:when>
											<xsl:when test="AppliDateCode3 = 'A'"><xsl:value-of select="ApplicationDate3"/></xsl:when>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="substring($adate,6)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring($adate,3,3)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring($adate,1,2)"/>
								</xsl:attribute>
							</DateRestriction>
						</xsl:if>
						<xsl:if test="AppliDateCode1 = 'B' or AppliDateCode2 = 'B' or AppliDateCode3 = 'B'">
							<DateRestriction>
								<xsl:attribute name="Application">Ticket before</xsl:attribute>
								<xsl:attribute name="StartDate">
									<xsl:variable name="adate">
										<xsl:choose>
											<xsl:when test="AppliDateCode1 = 'B'"><xsl:value-of select="ApplicationDate1"/></xsl:when>
											<xsl:when test="AppliDateCode2 = 'B'"><xsl:value-of select="ApplicationDate2"/></xsl:when>
											<xsl:when test="AppliDateCode3 = 'B'"><xsl:value-of select="ApplicationDate3"/></xsl:when>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="substring($adate,6)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring($adate,3,3)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring($adate,1,2)"/>
								</xsl:attribute>
							</DateRestriction>
						</xsl:if>
						<xsl:if test="AppliDateCode1 = 'S' or AppliDateCode2 = 'S' or AppliDateCode3 = 'S'">
							<DateRestriction>
								<xsl:attribute name="Application">Season end</xsl:attribute>
								<xsl:attribute name="EndDate">
									<xsl:variable name="adate">
										<xsl:choose>
											<xsl:when test="AppliDateCode1 = 'S'"><xsl:value-of select="ApplicationDate1"/></xsl:when>
											<xsl:when test="AppliDateCode2 = 'S'"><xsl:value-of select="ApplicationDate2"/></xsl:when>
											<xsl:when test="AppliDateCode3 = 'S'"><xsl:value-of select="ApplicationDate3"/></xsl:when>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="substring($adate,6)"/>
									<xsl:text>-</xsl:text>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring($adate,3,3)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring($adate,1,2)"/>
								</xsl:attribute>
							</DateRestriction>
						</xsl:if>
					</Restriction>
				</Restrictions>
			</xsl:if>
			<PricingInfo>
				<xsl:choose>
					<xsl:when test="TypeOfFare != ''">
						<xsl:attribute name="NegotiatedFare">true</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="NegotiatedFare">false</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<!--xsl:attribute name="PassengerTypeCode">
					<xsl:choose>
						<xsl:when test="HasFreeForm = 'Y'">
							<xsl:value-of select="substring-after(FreeForm,'PTC:')"/>
						</xsl:when>
						<xsl:when test="PIC = ''">
							<xsl:text>ADT</xsl:text>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="PIC"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute-->
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="monetaryGrp/monetaryValues/monetaryDetails/amount"/>
						<xsl:text>00</xsl:text>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="$currency"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</BaseFare>
			</PricingInfo>
		</FareDisplayInfo>
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
