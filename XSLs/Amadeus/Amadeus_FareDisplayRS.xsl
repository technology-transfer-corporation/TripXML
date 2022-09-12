<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_FareDisplayRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 12 Feb 2009 - Rastko															       -->
<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:variable name="depcity"><xsl:value-of select="substring(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/CityPair,1,3)"/></xsl:variable>
	<xsl:variable name="arrcity"><xsl:value-of select="substring(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/CityPair,4)"/></xsl:variable>
	<xsl:variable name="start">
		<xsl:choose>
			<xsl:when test="contains(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/ValidityPeriod,'**')">
				<xsl:value-of select="substring-before(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/ValidityPeriod,'**')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring-before(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/ValidityPeriod,'*')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="end">
		<xsl:choose>
			<xsl:when test="contains(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/ValidityPeriod,'**')">
				<xsl:value-of select="substring-after(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/ValidityPeriod,'**')"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="substring-after(Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/ValidityPeriod,'*')"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="airline"><xsl:value-of select="Fare_DisplayFaresForCityPairPlus_Reply/CAPI_ValidityLinePlus/AirlineCode"/></xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_DisplayFaresForCityPairPlus_Reply" />
		<xsl:apply-templates select="Fare_ListFareRequestTypesPlus_Reply"/>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="Fare_DisplayFaresForCityPairPlus_Reply">
		<OTA_AirFareDisplayRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors">
					<Errors>
						<Error Type="Sabre">
							<xsl:value-of select="Errors/Error/ErrorInfo/Message"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="CAPI_FareDisplayListPlus">
							<Success />
							<FareDisplayInfos>
								<xsl:apply-templates select="CAPI_FareDisplayListPlus" />
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


	<xsl:template match="CAPI_FareDisplayListPlus">
		<FareDisplayInfo>
			<xsl:attribute name="FareRPH"><xsl:value-of select="FareLineNo"/></xsl:attribute>
			<xsl:attribute name="FareApplicationType">
				<xsl:choose>
					<xsl:when test="RoundTrip != ''">Return</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:choose>
					<xsl:when test="BookingClass!=''">
						<xsl:value-of select="BookingClass"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(FareBasisCode,1,1)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<TravelDates>
				<xsl:attribute name="DepartureDate">
					<xsl:choose>
						<xsl:when test="ApplicationDate1!=''">
							<xsl:value-of select="substring(ApplicationDate1,6)"/>
							<xsl:text>-</xsl:text>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(ApplicationDate1,3,3)" />
								</xsl:with-param>
							</xsl:call-template>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(ApplicationDate1,1,2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>20</xsl:text>
							<xsl:value-of select="substring($start,6)"/>
							<xsl:text>-</xsl:text>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($start,3,3)" />
								</xsl:with-param>
							</xsl:call-template>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($start,1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="ArrivalDate">
					<xsl:choose>
						<xsl:when test="ApplicationDate2!=''">
							<xsl:value-of select="substring(ApplicationDate2,6)"/>
							<xsl:text>-</xsl:text>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(ApplicationDate2,3,3)" />
								</xsl:with-param>
							</xsl:call-template>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(ApplicationDate2,1,2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>20</xsl:text>
							<xsl:value-of select="substring($end,6)"/>
							<xsl:text>-</xsl:text>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring($end,3,3)" />
								</xsl:with-param>
							</xsl:call-template>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($end,1,2)"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</TravelDates>
			<FareReference><xsl:value-of select="FareBasisCode"/></FareReference>
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
					<xsl:when test="AdvancePurchase!=''">
						<ResTicketingRules>
							<AdvResTicketing>
								<xsl:if test="MoreAdvancePurchase='@'">
									<xsl:attribute name="AdvTicketingInd">Check rules</xsl:attribute>
								</xsl:if>
								<AdvTicketing>
									<xsl:attribute name="FromDepartPeriod"><xsl:value-of select="AdvancePurchase"/></xsl:attribute>
									<xsl:attribute name="FromDepartUnit">Days</xsl:attribute>
								</AdvTicketing>
							</AdvResTicketing>
						</ResTicketingRules>
					</xsl:when>
				</xsl:choose>
				<xsl:if test="MiniStay != '' or MaxiStay != ''">
					<LengthOfStayRules>
						<MinimumStay>
							<xsl:choose>
								<xsl:when test="MiniStay='-'">
									<xsl:attribute name="MinStay">None</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(MiniStay,'M')">
									<xsl:attribute name="MinStay">
										<xsl:value-of select="substring-before(MiniStay,'M')"/>
									</xsl:attribute>
									<xsl:attribute name="StayUnit">Months</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(MiniStay,'SU')">
									<xsl:attribute name="MinStay">1</xsl:attribute>
									<xsl:attribute name="StayUnit">SUN</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(MiniStay,'@')">
									<xsl:attribute name="MinStay">
										<xsl:value-of select="substring-before(MiniStay,'@')"/>
									</xsl:attribute>
									<xsl:attribute name="StayUnit">Days</xsl:attribute>
								</xsl:when>
								<xsl:when test="MiniStay!=''">
									<xsl:attribute name="MinStay">
										<xsl:value-of select="MiniStay"/>
									</xsl:attribute>
									<xsl:attribute name="StayUnit">Days</xsl:attribute>
								</xsl:when>
							</xsl:choose>
						</MinimumStay>
						<MaximumStay>
							<xsl:choose>
								<xsl:when test="MaxiStay='-'">
									<xsl:attribute name="MaxStay">None</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(MaxiStay,'M')">
									<xsl:attribute name="MaxStay">
										<xsl:value-of select="substring-before(MaxiStay,'M')"/>
									</xsl:attribute>
									<xsl:attribute name="StayUnit">Months</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(MaxiStay,'SU')">
									<xsl:attribute name="MaxStay">1</xsl:attribute>
									<xsl:attribute name="StayUnit">SUN</xsl:attribute>
								</xsl:when>
								<xsl:when test="contains(MaxiStay,'@')">
									<xsl:attribute name="MaxStay">
										<xsl:value-of select="substring-before(MaxiStay,'@')"/>
									</xsl:attribute>
									<xsl:attribute name="StayUnit">Days</xsl:attribute>
								</xsl:when>
								<xsl:when test="MaxiStay!=''">
									<xsl:attribute name="MaxStay">
										<xsl:value-of select="MaxiStay"/>
									</xsl:attribute>
									<xsl:attribute name="StayUnit">Days</xsl:attribute>
								</xsl:when>
							</xsl:choose>
						</MaximumStay>
					</LengthOfStayRules>
				</xsl:if>
				<xsl:if test="Penalty!=''">
					<ChargesRules>
						<VoluntaryRefunds>
							<Penalty>
								<xsl:attribute name="PenaltyType">
									<xsl:choose>
										<xsl:when test="Penalty='NRF'">Non-refundable</xsl:when>
										<xsl:when test="Penalty='-'">Refundable</xsl:when>
										<xsl:when test="Penalty='@' or Penalty='+'">Check rules</xsl:when>
										<xsl:otherwise>Penalty</xsl:otherwise>
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
						<xsl:when test="Airline!=''"><xsl:value-of select="Airline"/></xsl:when>
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
						<xsl:choose>
							<xsl:when test="RoundTrip!=''">
								<xsl:choose>
									<xsl:when test="contains(RoundTrip,'.')">
										<xsl:value-of select="translate(RoundTrip,'.','')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="RoundTrip"/>
										<xsl:text>00</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="contains(OneWayFare,'.')">
										<xsl:value-of select="translate(OneWayFare,'.','')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="OneWayFare"/>
										<xsl:text>00</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="../CAPI_ROEPlus/Currency"/></xsl:attribute>
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
