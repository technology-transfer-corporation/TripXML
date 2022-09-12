<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Sabre_AirFareDisplayRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 19 Oct 2006 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:variable name="depcity"><xsl:value-of select="FareRS/HeaderInfo/OriginLocation/@LocationCode"/></xsl:variable>
	<xsl:variable name="arrcity"><xsl:value-of select="FareRS/HeaderInfo/DestinationLocation/@LocationCode"/></xsl:variable>
	<xsl:variable name="start"><xsl:value-of select="FareRS/OTA_AirFareDisplayRQ/OriginDestinationInformation[1]/DepartureDateTime"/></xsl:variable>
	<xsl:variable name="end"><xsl:value-of select="FareRS/OTA_AirFareDisplayRQ/OriginDestinationInformation[position()=last()]/DepartureDateTime"/></xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="FareRS" />
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="FareRS">
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
					<Success />
					<xsl:choose>
						<xsl:when test="LineNumber">
							<FareDisplayInfos>
								<xsl:apply-templates select="LineNumber" />
							</FareDisplayInfos>	
						</xsl:when>
						<xsl:otherwise>
							<Warnings>
								<Warning Type="Sabre">
									<xsl:value-of select="HeaderInfo/Text"/>
								</Warning>
							</Warnings>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirFareDisplayRS>
	</xsl:template>

	<xsl:template match="LineNumber">
		<FareDisplayInfo>
			<xsl:attribute name="FareRPH"><xsl:value-of select="position()"/></xsl:attribute>
			<xsl:attribute name="FareApplicationType">
				<xsl:choose>
					<xsl:when test="BaseFare/@DirectionInd = 'R'">Return</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="BookingClass/@Code"/>
			</xsl:attribute>
			<TravelDates>
				<xsl:attribute name="DepartureDate"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:attribute name="ArrivalDate"><xsl:value-of select="$end"/></xsl:attribute>
			</TravelDates>
			<FareReference><xsl:value-of select="FareBasisCode/@Code"/></FareReference>
			<RuleInfo>
				<xsl:if test="MinStay/@Code != '' or MaxStay/@Code != ''">
					<LengthOfStayRules>
						<MinimumStay>
							<xsl:attribute name="MinStay"><xsl:value-of select="MinStay/@Code"/></xsl:attribute>
						</MinimumStay>
						<MaximumStay>
							<xsl:attribute name="MaxStay"><xsl:value-of select="MaxStay/@Code"/></xsl:attribute>
						</MaximumStay>
					</LengthOfStayRules>
				</xsl:if>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="../HeaderInfo/VendorPref/@Code"/></xsl:attribute>
			</FilingAirline>
			<DepartureLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$depcity"/></xsl:attribute>
			</DepartureLocation>
			<ArrivalLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="$arrcity"/></xsl:attribute>
			</ArrivalLocation>
			<xsl:if test="Type1Qual/Seasons != ''">
				<Restrictions>
					<Restriction>
						<DateRestriction>
							<xsl:attribute name="StartDate"><xsl:value-of select="substring-before(Type1Qual/Seasons,'-')"/></xsl:attribute>
							<xsl:attribute name="EndDate"><xsl:value-of select="substring-after(Type1Qual/Seasons,'-')"/></xsl:attribute>
						</DateRestriction>
					</Restriction>
				</Restrictions>
			</xsl:if>
			<PricingInfo>
				<xsl:choose>
					<xsl:when test="FareBasisCode/@Type = 'P'">
						<xsl:attribute name="NegotiatedFare">false</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="NegotiatedFare">true</xsl:attribute>
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
					<xsl:attribute name="Amount"><xsl:value-of select="translate(BaseFare/@Amount,'.','')"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="BaseFare/@CurrencyCode"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="BaseFare/@DecimalPlaces"/></xsl:attribute>
				</BaseFare>
			</PricingInfo>
		</FareDisplayInfo>
	</xsl:template>
	
</xsl:stylesheet>
