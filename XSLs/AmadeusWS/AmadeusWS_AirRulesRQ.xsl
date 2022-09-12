<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirRulesRQ.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 20 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<AirRules>
			<xsl:apply-templates select="OTA_AirRulesRQ" />
		</AirRules>
	</xsl:template>
	
	<xsl:template match="OTA_AirRulesRQ">
		<xsl:if test="RuleReqInfo/SubSection">
			<fareRule>
				<tarifFareRule>
					<xsl:apply-templates select="RuleReqInfo/SubSection" mode="sub" />
				</tarifFareRule>
			</fareRule>
		</xsl:if>
		<RuleReqInfo>
			<xsl:apply-templates select="RuleReqInfo" />
		</RuleReqInfo>
		<Fare_DisplayFaresForCityPair>
			<msgType>
			     <messageFunctionDetails>
			     	<messageFunction>711</messageFunction>
			     </messageFunctionDetails> 
			</msgType>
			<!--xsl:if test="POS/Source/@ISOCurrency != ''">
				<conversionRate>
					<conversionRateDetails>
						<currency><xsl:value-of select="POS/Source/@ISOCurrency"/></currency>
					</conversionRateDetails>
				</conversionRate>
			</xsl:if-->
			<pricingTickInfo>
				<priceTicketDetails>
					<xsl:choose>
						<xsl:when test="RuleReqInfo/SubSection[@SubTitle='CorporateCode']">
							<indicators>RW</indicators>
						</xsl:when>
						<xsl:when test="RuleReqInfo/@NegotiatedFare='true'">
							<indicators>RU</indicators>
						</xsl:when>
						<xsl:otherwise>
							<indicators>RP</indicators>
						</xsl:otherwise>
					</xsl:choose>
				</priceTicketDetails>
			</pricingTickInfo>
		  	<transportInformation>
		  		<transportService>
		  			<companyIdentification>
		  				<marketingCompany><xsl:value-of select="RuleReqInfo/FilingAirline/@Code" /></marketingCompany>
		  			</companyIdentification> 
		  		</transportService>
			</transportInformation> 
			<tripDescription>
				<origDest>
				     <origin><xsl:value-of select="RuleReqInfo/DepartureAirport/@LocationCode" /></origin>
				     <destination><xsl:value-of select="RuleReqInfo/ArrivalAirport/@LocationCode" /></destination> 
				</origDest>
				<dateFlightMovement>
					<dateAndTimeDetails>
						<qualifier>O</qualifier>
						<date>
							<xsl:value-of select="substring(RuleReqInfo/DepartureDate,9,2)" />
							<xsl:value-of select="substring(RuleReqInfo/DepartureDate,6,2)" />
							<xsl:value-of select="substring(RuleReqInfo/DepartureDate,3,2)" />
						</date>
					</dateAndTimeDetails>
					<!--dateAndTimeDetails>
						<qualifier>I</qualifier>
						<date></date>
					</dateAndTimeDetails-->
				</dateFlightMovement>
			</tripDescription>
			<xsl:if test="RuleReqInfo/@NegotiatedFareCode!='' or RuleReqInfo/@NegotiatedFare='true'">
				<!--FareType1>
					<xsl:choose>
						<xsl:when test="RuleReqInfo/@NegotiatedFareCode!=''">
							<xsl:choose>
								<xsl:when test="RuleReqInfo/@NegotiatedFareCode='CHD'">IN</xsl:when>
								<xsl:when test="RuleReqInfo/@NegotiatedFareCode='INF'">IN</xsl:when>
								<xsl:otherwise><xsl:value-of select="RuleReqInfo/@NegotiatedFareCode"/></xsl:otherwise>
							</xsl:choose>
						</xsl:when>
					</xsl:choose>
				</FareType1-->
				<xsl:apply-templates select="RuleReqInfo/SubSection[@SubTitle!='']" mode="subtitle" />
			</xsl:if>
		</Fare_DisplayFaresForCityPair>
	</xsl:template>
	<xsl:template match="SubSection" mode="sub">
		<xsl:if test="not(@SubTitle)">
			<ruleSectionId><xsl:value-of select="@SubCode" /></ruleSectionId>
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
</xsl:stylesheet>