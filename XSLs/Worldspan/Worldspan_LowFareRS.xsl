<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_LowFareRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 16 May 2008 - Rastko														       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<xsl:apply-templates select="PSW5"/>	
	<xsl:apply-templates select="XXW"/>	
</xsl:template>

<xsl:template match="PSW5">
	<OTA_AirLowFareSearchRS>
		<xsl:attribute name="Version">1.001</xsl:attribute>
		<xsl:attribute name="TransactionIdentifier">Worldspan</xsl:attribute>
			<xsl:choose>
				<xsl:when test="ERR">
					<xsl:apply-templates select="ERR"/>
				</xsl:when>
				<xsl:when test="ADV_MSG != '' and ALT_COU = '0'">
					<Errors>
						<Error Type="Worldspan">
			   				<xsl:value-of select="ADV_MSG"/>
						</Error>       	
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<PricedItineraries>
						<xsl:apply-templates select="ALT_INF[BAS_FAR != '']"/>	
					</PricedItineraries>
				</xsl:otherwise>
			</xsl:choose>
	</OTA_AirLowFareSearchRS>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="ALT_INF">
	<PricedItinerary>
		<xsl:attribute name="SequenceNumber"><xsl:value-of select="position()"/></xsl:attribute>
		<AirItinerary>
			<xsl:attribute name="DirectionInd">
				<xsl:choose>
					<xsl:when test="OriginDestination[1]/FLI_INF[1]/DEP_ARP = OriginDestination[position()=last()]/FLI_INF[position()=last()]/ARR_ARP">Circle</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<OriginDestinationOptions>
				<xsl:for-each select="OriginDestination">
					<OriginDestinationOption>
						<xsl:variable name="tjd">
							<xsl:value-of select="format-number(FLI_INF[position()=last()]/ADD_FLI_SVC/ACU_FLI_TIM,'00:00')"/>
						</xsl:variable>
						<xsl:apply-templates select="FLI_INF" mode="air">
							<xsl:with-param name="tjd"><xsl:value-of select="$tjd"/></xsl:with-param>
						</xsl:apply-templates>
					</OriginDestinationOption>
				</xsl:for-each>
			</OriginDestinationOptions>
		</AirItinerary>
		<AirItineraryPricingInfo>
			<xsl:attribute name="PricingSource">
				<xsl:choose>
					<xsl:when test="FAR_SEL = 'PUB'">Published</xsl:when>
					<xsl:otherwise>Private</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<ItinTotalFare>
				<xsl:variable name="base"><xsl:value-of select="translate(string(BAS_FAR),'.','')" /></xsl:variable>
				<xsl:variable name="total"><xsl:value-of select="translate(string(TTL_FAR),'.','')" /></xsl:variable>
				<xsl:variable name="NoDeci"><xsl:value-of select="string-length(substring-after(BAS_FAR,'.'))"/></xsl:variable>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="substring($base,1,1) = '0'">
								<xsl:value-of select="substring-after($base, substring-before(translate($base, '123456789', 'XXXXXXXXX'), 'X'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$base"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="ISO_CUR_COD"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
				</BaseFare>		
				<Taxes>
					<Tax>
						<xsl:variable name="tax"><xsl:value-of select="translate(string(TAX),'.','')" /></xsl:variable>
						<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="substring($tax,1,1) = '0'">
									<xsl:value-of select="substring-after($tax, substring-before(translate($tax, '123456789', 'XXXXXXXXX'), 'X'))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$tax"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="ISO_CUR_COD"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
					</Tax>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="substring($total,1,1) = '0'">
								<xsl:value-of select="substring-after($total, substring-before(translate($total, '123456789', 'XXXXXXXXX'), 'X'))"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$total"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="ISO_CUR_COD"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="$NoDeci"/></xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>	
				<xsl:apply-templates select="PTC_FAR_INF" mode="fareb"/>	
			</PTC_FareBreakdowns>	
			<FareInfos>
				<xsl:apply-templates select="PTC_FAR_INF" mode="fareinfo"/>
			</FareInfos>	
		</AirItineraryPricingInfo>
		<xsl:if test="LAS_TIC_DAT != ''">
			<xsl:variable name="tktlmt1"><xsl:value-of select="substring-after(substring-before(LAS_TIC_DAT,' IS LAST DATE'),'/ ')"/></xsl:variable>
			<xsl:variable name="tktlmt">
				<xsl:choose>
					<xsl:when test="$tktlmt1 = '*********'">
						<xsl:value-of select="substring-after(substring-before(LAS_TIC_DAT,' DEPARTURE DATE'),'** ')"/>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="$tktlmt1"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<TicketingInfo>
				<xsl:attribute name="TicketTimeLimit">
					<xsl:value-of select="substring($tktlmt,6,4)"/>
					<xsl:text>-</xsl:text>
					<xsl:call-template name="month">
						<xsl:with-param name="month">
							<xsl:value-of select="substring($tktlmt,3,3)" />
						</xsl:with-param>
					</xsl:call-template>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="substring($tktlmt,1,2)"/>
					<xsl:text>T00:00:00</xsl:text>
				</xsl:attribute>
			</TicketingInfo>
		</xsl:if>
	</PricedItinerary>
</xsl:template>

<xsl:template match="TAX_INF">
	<Tax>
		<xsl:variable name="tax"><xsl:value-of select="translate(string(TAX_AMT),'.','')" /></xsl:variable>
		<xsl:attribute name="TaxCode">
			<xsl:value-of select="TAX_TYP"/>
		</xsl:attribute>
		<xsl:attribute name="Amount">
			<xsl:choose>
				<xsl:when test="substring($tax,1,1) = '0'">
					<xsl:value-of select="substring-after($tax, substring-before(translate($tax, '123456789', 'XXXXXXXXX'), 'X'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$tax"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</Tax>
</xsl:template>
<!-- **************************************************************** -->

<xsl:template match="PTC_FAR_INF" mode="fareb">		
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<PTC_FareBreakdown>	
		<xsl:variable name="TotalAmount">
			<xsl:choose>
				<xsl:when test="substring(TTL_FAR,1,1) = '0'">
					<xsl:value-of select="translate(substring-after(TTL_FAR, substring-before(translate(TTL_FAR, '123456789', 'XXXXXXXXX'), 'X')),'.','')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(TTL_FAR,'.','')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="NumberOfPax"><xsl:value-of select="NUM_PAX"/></xsl:variable>
		<xsl:variable name="BaseAmount">
			<xsl:choose>
				<xsl:when test="substring(BAS_FAR,1,1) = '0'">
					<xsl:value-of select="translate(substring-after(BAS_FAR, substring-before(translate(BAS_FAR, '123456789', 'XXXXXXXXX'), 'X')),'.','')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="translate(BAS_FAR,'.','')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>		
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:choose>
					<xsl:when test="PTC = 'CNN'">CHD</xsl:when>
					<xsl:otherwise><xsl:value-of select="PTC"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Quantity">
				<xsl:value-of select="NUM_PAX"/>
			</xsl:attribute>
		</PassengerTypeQuantity>
		<xsl:variable name="paxtype"><xsl:value-of select="PTC"/></xsl:variable>
		<FareBasisCodes>	
			<xsl:variable name="tottypes"><xsl:value-of select="count(../PTC_FAR_INF)"/></xsl:variable>
			<xsl:apply-templates select="../OriginDestination" mode="paxtype">
				<xsl:with-param name="postypes"><xsl:value-of select="$pos"/></xsl:with-param>
				<xsl:with-param name="tottypes"><xsl:value-of select="$tottypes"/></xsl:with-param>
			</xsl:apply-templates>
		</FareBasisCodes>
		<PassengerFare>
			<xsl:variable name="NoDeci"><xsl:value-of select="string-length(substring-after(BAS_FAR,'.'))"/></xsl:variable>
			<xsl:variable name="bf"><xsl:value-of select="$BaseAmount * $NumberOfPax"/></xsl:variable>
			<xsl:variable name="tf"><xsl:value-of select="$TotalAmount * $NumberOfPax"/></xsl:variable>
			<BaseFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$bf"/></xsl:attribute>
			</BaseFare>	
			<Taxes>
				<Tax>
					<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
					<xsl:attribute name="Amount"><xsl:value-of select="$tf - $bf"/></xsl:attribute>
				</Tax>
			</Taxes>
			<TotalFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$tf"/></xsl:attribute>
			</TotalFare>							
		</PassengerFare>			
		<TPA_Extensions>
			<PricedCode>
				<xsl:choose>
					<xsl:when test="PTC = 'CNN'">CHD</xsl:when>
					<xsl:otherwise><xsl:value-of select="PTC"/></xsl:otherwise>
				</xsl:choose>
			</PricedCode> 
		</TPA_Extensions>
	</PTC_FareBreakdown>
</xsl:template>

<xsl:template match="OriginDestination" mode="paxtype">
	<xsl:param name="postypes"/>
	<xsl:param name="tottypes"/>
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="posit">
		<xsl:choose>
			<xsl:when test="$pos = 1"><xsl:value-of select="$postypes"/></xsl:when>
			<xsl:otherwise><xsl:value-of select="$pos + $tottypes + $postypes - 2"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:for-each select="FLI_INF">
		<FareBasisCode><xsl:value-of select="../../TRI_INF[position()=$posit]/PTC_DTL_INF/FAR_BAS_COD"/></FareBasisCode>
	</xsl:for-each>
</xsl:template>

<xsl:template match="PTC_FAR_INF" mode="fareinfo">		
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="paxtype"><xsl:value-of select="PTC"/></xsl:variable>
	<xsl:variable name="tottypes"><xsl:value-of select="count(../PTC_FAR_INF)"/></xsl:variable>
	<xsl:apply-templates select="../OriginDestination" mode="fareinfo">
		<xsl:with-param name="postypes"><xsl:value-of select="$pos"/></xsl:with-param>
		<xsl:with-param name="tottypes"><xsl:value-of select="$tottypes"/></xsl:with-param>
	</xsl:apply-templates>

</xsl:template>

<xsl:template match="OriginDestination" mode="fareinfo">
	<xsl:param name="postypes"/>
	<xsl:param name="tottypes"/>
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="posit">
		<xsl:choose>
			<xsl:when test="$pos = 1"><xsl:value-of select="$postypes"/></xsl:when>
			<xsl:otherwise><xsl:value-of select="$pos + $tottypes + $postypes - 2"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:for-each select="FLI_INF">
		<FareInfo>
			<DepartureDate><xsl:value-of select="FLI_DAT"/></DepartureDate>
			<xsl:apply-templates select="../../TRI_INF[position()=$posit]/PTC_DTL_INF" mode="fareinfo"/>
			<DepartureAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="DEP_ARP"/>
				</xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="ARR_ARP"/>
				</xsl:attribute>
			</ArrivalAirport>
		</FareInfo>
	</xsl:for-each>
</xsl:template>
	
<xsl:template match="PTC_DTL_INF" mode="fareinfo">
	<FareReference><xsl:value-of select="FAR_BAS_COD"/></FareReference>
	<RuleInfo>
		<xsl:if test="FAR_RST != ''">
			<ChargesRules>
				<VoluntaryChanges>
					<Penalty>
						<xsl:attribute name="PenaltyType">
							<xsl:choose>
								<xsl:when test="FAR_RST = 'A'">SecuRate Air Direct</xsl:when>
								<xsl:when test="FAR_RST = 'B'">No penalty</xsl:when>
								<xsl:when test="FAR_RST = 'C'">Non-refundable</xsl:when>
								<xsl:when test="FAR_RST = 'D'">No changes allowed</xsl:when>
								<xsl:when test="FAR_RST = 'E'">Non-refundable and no changes allowed</xsl:when>
							</xsl:choose>
						</xsl:attribute>
					</Penalty>
				</VoluntaryChanges>
			</ChargesRules>
		</xsl:if>
	</RuleInfo>		
	<FilingAirline>
		<xsl:attribute name="Code"><xsl:value-of select="ARL_COD"/></xsl:attribute>
	</FilingAirline>			
</xsl:template>

<xsl:template match="FLI_INF" mode="air"> 
	<xsl:param name="tjd"/>
	<FlightSegment>
		<xsl:attribute name="DepartureDateTime">
			<xsl:value-of select="FLI_DAT"/>
			<xsl:text>T</xsl:text>
			<xsl:value-of select="substring(DEP_TIM,1,2)"/>
			<xsl:text>:</xsl:text>
			<xsl:value-of select="substring(DEP_TIM,3,2)"/>
			<xsl:text>:00</xsl:text>
		</xsl:attribute>
		<xsl:attribute name="ArrivalDateTime">
			<xsl:value-of select="ARR_DAT"/>
			<xsl:text>T</xsl:text>
			<xsl:value-of select="substring(ARR_TIM,1,2)"/>
			<xsl:text>:</xsl:text>
			<xsl:value-of select="substring(ARR_TIM,3,2)"/>
			<xsl:text>:00</xsl:text>
		</xsl:attribute>
		<xsl:attribute name="StopQuantity"><xsl:value-of select="NUM_STO"/></xsl:attribute>
		<xsl:attribute name="RPH">1</xsl:attribute>
		<xsl:attribute name="FlightNumber"><xsl:value-of select="FLI_NUM"/></xsl:attribute>
		<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="FAR_CLA"/></xsl:attribute>
		<xsl:attribute name="NumberInParty"><xsl:value-of select="sum(../../PTC_FAR_INF[PTC != 'INF']/NUM_PAX)"/></xsl:attribute>
		<xsl:if test="E_TIC_ELI != ''">
			<xsl:attribute name="E_TicketEligibility">
				<xsl:choose>
					<xsl:when test="E_TIC_ELI = 'E'">Eligible</xsl:when>
					<xsl:otherwise>NotEligible</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:if>
		<DepartureAirport>
			<xsl:attribute name="LocationCode"><xsl:value-of select="DEP_ARP"/></xsl:attribute>
		</DepartureAirport>
		<ArrivalAirport>
			<xsl:attribute name="LocationCode"><xsl:value-of select="ARR_ARP"/></xsl:attribute>
		</ArrivalAirport>
		<OperatingAirline>
			<xsl:attribute name="Code">
				<xsl:choose>
					<xsl:when test="COD_SHA_INF != ''"><xsl:value-of select="COD_SHA_INF"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="ARL_COD"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</OperatingAirline>
		<Equipment>
			<xsl:attribute name="AirEquipType"><xsl:value-of select="EQP_TYP"/></xsl:attribute>
		</Equipment>
		<MarketingAirline>
			<xsl:attribute name="Code"><xsl:value-of select="ARL_COD"/></xsl:attribute>
		</MarketingAirline>			
		<TPA_Extensions>
			<JourneyTotalDuration><xsl:value-of select="$tjd"/></JourneyTotalDuration>
			<JourneyDuration><xsl:value-of select="format-number(ADD_FLI_SVC/ELA_FLI_TIM,'00:00')"/></JourneyDuration>
		</TPA_Extensions>	
	</FlightSegment>
</xsl:template>
<!-- **************************************************************** -->

<xsl:template match="FLIGHT_DATA">
	<xsl:choose>
		<xsl:when test="DAY_CHANGE_IND = '#'">
			<JourneyTime><xsl:value-of select="(2360 - DEP_TIME) + ARR_TIME" /></JourneyTime>
		</xsl:when>
		<xsl:otherwise>
			<JourneyTime><xsl:value-of select="ARR_TIME - DEP_TIME" /></JourneyTime>
		</xsl:otherwise>
	</xsl:choose>
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable> 
	<Segment>
		<xsl:choose>
			<xsl:when test="DEP_AIRPORT = preceding-sibling::ARR_AIRPORT or $pos = 1">
				<xsl:attribute name="ChangeOfAirport">N</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="ChangeOfAirport">Y</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
		<Departure>
			<AirportCode><xsl:value-of select="DEP_AIRPORT" /></AirportCode>
			<Time><xsl:value-of select="DEP_TIME" /></Time>
		</Departure>
		<Arrival>
			<AirportCode><xsl:value-of select="ARR_AIRPORT" /></AirportCode>
			<xsl:choose>
				<xsl:when test="DAY_CHANGE_IND = '#'">
					<Date>ConvertDate(<xsl:value-of select="FLIGHT_DATE" />+<xsl:value-of select="DEP_ARR_DATE_DIF" />,ddMMM,yyyymmdd)</Date> 
				</xsl:when>
				<xsl:otherwise>
					<Date>ConvertDate(<xsl:value-of select="FLIGHT_DATE" />-<xsl:value-of select="DEP_ARR_DATE_DIF" />,ddMMM,yyyymmdd)</Date> 
				</xsl:otherwise>
			</xsl:choose>
			<xsl:choose>
				<xsl:when test="DAY_CHANGE_IND = '#'">
					<ChangeOfDay>+<xsl:value-of select="DEP_ARR_DATE_DIF" /></ChangeOfDay>
				</xsl:when>
				<xsl:otherwise>
					<ChangeOfDay><xsl:value-of select="DAY_CHANGE_IND" /><xsl:value-of select="DEP_ARR_DATE_DIF" /></ChangeOfDay>
				</xsl:otherwise>
			</xsl:choose>
			<Time><xsl:value-of select="ARR_TIME" /></Time>			
		</Arrival>
		<Carrier>
			<AirlineCode><xsl:value-of select="CARRIER" /></AirlineCode>
			<xsl:variable name="zeros">000</xsl:variable>
			<FlightNumber><xsl:value-of select="substring($zeros,1,4-string-length(FLIGHT_NUM))"/><xsl:value-of select="FLIGHT_NUM" /></FlightNumber>
		</Carrier>
		<xsl:if test="CODE_SHARE_INFO!=''">
			<OperatingCarrier>
				<AirlineName><xsl:value-of select="CODE_SHARE_INFO" /></AirlineName>
			</OperatingCarrier>
		</xsl:if>
		<Equipment>
			<Code><xsl:value-of select="AIRCRAFT_TYPE" /></Code>
		</Equipment>
		<NumberOfStops><xsl:value-of select="NUMBER_OF_STOPS" /></NumberOfStops>
		<xsl:if test="MEAL_TYPE!=''">
			<Meals><xsl:value-of select="MEAL_TYPE"/></Meals>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="DAY_CHANGE_IND = '#'">
				<FlightDuration><xsl:value-of select="(2360 - DEP_TIME) + ARR_TIME" /></FlightDuration>
			</xsl:when>
			<xsl:otherwise>
				<FlightDuration><xsl:value-of select="ARR_TIME - DEP_TIME" /></FlightDuration>
			</xsl:otherwise>
		</xsl:choose>
		<Classes>
			<ClassOfService Status="A"><xsl:value-of select="FARE_CLASS" /></ClassOfService>
		</Classes>
	</Segment>
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
			<xsl:when test="$month = 'Jan'">01</xsl:when>
			<xsl:when test="$month = 'Feb'">02</xsl:when>
			<xsl:when test="$month = 'Mar'">03</xsl:when>
			<xsl:when test="$month = 'Apr'">04</xsl:when>
			<xsl:when test="$month = 'May'">05</xsl:when>
			<xsl:when test="$month = 'Jun'">06</xsl:when>
			<xsl:when test="$month = 'Jul'">07</xsl:when>
			<xsl:when test="$month = 'Aug'">08</xsl:when>
			<xsl:when test="$month = 'Sep'">09</xsl:when>
			<xsl:when test="$month = 'Oct'">10</xsl:when>
			<xsl:when test="$month = 'Nov'">11</xsl:when>
			<xsl:when test="$month = 'Dec'">12</xsl:when>
		</xsl:choose>
</xsl:template>

<!--********************************************************-->
<!--			Process Error					-->
<!--********************************************************-->
<xsl:template match="XXW">
	<OTA_AirLowFareSearchRS>
		<xsl:attribute name="Version">1.001</xsl:attribute>
		<xsl:attribute name="TransactionIdentifier">Worldspan</xsl:attribute>
		<Errors>
			<Error Type="Worldspan">
				<xsl:attribute name="Code"><xsl:value-of select="ERROR/CODE"/></xsl:attribute>
   				<xsl:value-of select="ERROR/TEXT"/>
			</Error>       	
		</Errors>
	</OTA_AirLowFareSearchRS>
</xsl:template>  

<xsl:template match="ERR">
	<Errors>
		<Error Type="Worldspan">
			<xsl:attribute name="Code"><xsl:value-of select="NUM"/></xsl:attribute>
  			<xsl:value-of select="MSG_TXT"/>
		</Error>       	
	</Errors>
</xsl:template>  

<!--********************************************************-->
</xsl:stylesheet>