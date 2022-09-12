<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_AirRulesRQ.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 13 Feb 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<AirRules>
			<xsl:apply-templates select="OTA_AirRulesRQ" />
		</AirRules>
	</xsl:template>
	
	<xsl:template match="OTA_AirRulesRQ">
		<Categories>
			<xsl:apply-templates select="RuleReqInfo/SubSection" mode="sub" />
		</Categories>
		<RuleReqInfo>
			<xsl:apply-templates select="RuleReqInfo" />
		</RuleReqInfo>
		<FarePlus_DisplayFaresForCityPair_Query>
			<OriginCity>
				<xsl:value-of select="RuleReqInfo/DepartureAirport/@LocationCode" />
			</OriginCity>
			<DestinationCity>
				<xsl:value-of select="RuleReqInfo/ArrivalAirport/@LocationCode" />
			</DestinationCity>
			<Airline1>
				<xsl:value-of select="RuleReqInfo/FilingAirline/@Code" />
			</Airline1>
			<CommonFares>T</CommonFares>
			<DateOutbound>
				<xsl:value-of select="substring(RuleReqInfo/DepartureDate,9,2)" />
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(RuleReqInfo/DepartureDate,6,2)" />
					</xsl:with-param>
				</xsl:call-template>
				<xsl:value-of select="substring(RuleReqInfo/DepartureDate,1,4)" />
			</DateOutbound>
			<xsl:if test="RuleReqInfo/@NegotiatedFareCode!='' or RuleReqInfo/@NegotiatedFare='true' or POS/Source/@ISOCurrency != ''">
				<FareType1>
					<xsl:choose>
						<xsl:when test="RuleReqInfo/@NegotiatedFareCode!=''">
							<xsl:choose>
								<xsl:when test="RuleReqInfo/@NegotiatedFareCode='CHD'">IN</xsl:when>
								<xsl:when test="RuleReqInfo/@NegotiatedFareCode='INF'">IN</xsl:when>
								<xsl:otherwise><xsl:value-of select="RuleReqInfo/@NegotiatedFareCode"/></xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<!--xsl:otherwise>JCB</xsl:otherwise-->
					</xsl:choose>
				</FareType1>
				<xsl:if test="POS/Source/@ISOCurrency != ''">
					<CurrencySelection><xsl:value-of select="POS/Source/@ISOCurrency"/></CurrencySelection>
				</xsl:if>
				<xsl:apply-templates select="RuleReqInfo/SubSection[@SubTitle!='']" mode="subtitle" />
				 <CAPI_TicketingIndicatorsPlus> 
				 	<xsl:choose>
							<xsl:when test="RuleReqInfo/SubSection[@SubTitle='CorporateCode']">
								<PricingTicketingIndicator>RW</PricingTicketingIndicator>
							</xsl:when>
							<xsl:when test="RuleReqInfo/@NegotiatedFare='true'">
								<PricingTicketingIndicator>RU</PricingTicketingIndicator>
							</xsl:when>
						</xsl:choose>
				</CAPI_TicketingIndicatorsPlus> 
			</xsl:if>
		</FarePlus_DisplayFaresForCityPair_Query>
	</xsl:template>
	<xsl:template match="SubSection" mode="sub">
		<xsl:if test="not(@SubTitle)">
			<xsl:value-of select="@SubCode" />
		</xsl:if>
	</xsl:template>
	<xsl:template match="SubSection" mode="subtitle">
		<xsl:if test="@SubTitle ='CorporateCode'">
			<CorporateContractNumber>
				<xsl:value-of select="@SubCode" />
			</CorporateContractNumber>
		</xsl:if>
	</xsl:template>
	<xsl:template match="RuleReqInfo">
		<DepartureDate>
			<xsl:value-of select="DepartureDate" />
		</DepartureDate>
		<FareReference>
			<xsl:value-of select="FareReference" />
		</FareReference>
		<RuleInfo>
			<xsl:value-of select="RuleInfo" />
		</RuleInfo>
		<FilingAirline>
			<xsl:attribute name="Code">
				<xsl:value-of select="FilingAirline/@Code" />
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
	</xsl:template>
	<xsl:template name="month">
		<xsl:param name="month" />
		<xsl:choose>
			<xsl:when test="$month = '01'">JAN</xsl:when>
			<xsl:when test="$month = '02'">FEB</xsl:when>
			<xsl:when test="$month = '03'">MAR</xsl:when>
			<xsl:when test="$month = '04'">APR</xsl:when>
			<xsl:when test="$month = '05'">MAY</xsl:when>
			<xsl:when test="$month = '06'">JUN</xsl:when>
			<xsl:when test="$month = '07'">JUL</xsl:when>
			<xsl:when test="$month = '08'">AUG</xsl:when>
			<xsl:when test="$month = '09'">SEP</xsl:when>
			<xsl:when test="$month = '10'">OCT</xsl:when>
			<xsl:when test="$month = '11'">NOV</xsl:when>
			<xsl:when test="$month = '12'">DEC</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>