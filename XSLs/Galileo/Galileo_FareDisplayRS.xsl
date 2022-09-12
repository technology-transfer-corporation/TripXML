<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_AirFareDisplayRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 16 Aug 2010 - Rastko - corrected parsing of error messages			       -->
<!-- Date: 22 Aug 2006 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="FareQuoteMultiDisplay_8_2" />
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="FareQuoteMultiDisplay_8_2">
		<OTA_AirFareDisplayRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="FareInfo/RespHeader/ErrMsg='Y'">
					<Errors>
						<Error Type="Galileo">
							<xsl:value-of select="FareInfo/InfoMsg/Text"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="FareInfo/ErrText">
					<Errors>
						<Error Type="Galileo">
							<xsl:value-of select="FareInfo/ErrText/Text"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<xsl:apply-templates select="FareInfo" />
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirFareDisplayRS>
	</xsl:template>

	<xsl:template match="FareInfo">
		<FareDisplayInfos>
			<xsl:variable name="start"><xsl:value-of select="../Request/OTA_AirFareDisplayRQ/OriginDestinationInformation[1]/DepartureDateTime"/></xsl:variable>
			<xsl:variable name="end"><xsl:value-of select="../Request/OTA_AirFareDisplayRQ/OriginDestinationInformation[position()=last()]/DepartureDateTime"/></xsl:variable>
			<xsl:apply-templates select="Tariff[Type1Qual/AirV != '' or Type2Qual/AirV != '' or Type3Qual/AirV != '' or Type4Qual/AirV != '']" mode="start">
				<xsl:with-param name="start"><xsl:value-of select="$start"/></xsl:with-param>
				<xsl:with-param name="end"><xsl:value-of select="$end"/></xsl:with-param>
			</xsl:apply-templates>
		</FareDisplayInfos>	
	</xsl:template>
	
	<xsl:template match="Tariff" mode="start">
		<xsl:param name="start"/>
		<xsl:param name="end"/>
		<xsl:choose>
			<xsl:when test="Type1 = 'Y'">
				<xsl:apply-templates select="." mode="type1">
					<xsl:with-param name="start"><xsl:value-of select="$start"/></xsl:with-param>
					<xsl:with-param name="end"><xsl:value-of select="$end"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="Type2 = 'Y'">
				<xsl:apply-templates select="." mode="type2">
					<xsl:with-param name="start"><xsl:value-of select="$start"/></xsl:with-param>
					<xsl:with-param name="end"><xsl:value-of select="$end"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:when test="Type3 = 'Y'">
				<xsl:apply-templates select="." mode="type3">
					<xsl:with-param name="start"><xsl:value-of select="$start"/></xsl:with-param>
					<xsl:with-param name="end"><xsl:value-of select="$end"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="type4">
					<xsl:with-param name="start"><xsl:value-of select="$start"/></xsl:with-param>
					<xsl:with-param name="end"><xsl:value-of select="$end"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="Tariff" mode="type1">
		<xsl:param name="start"/>
		<xsl:param name="end"/>
		<FareDisplayInfo>
			<xsl:attribute name="FareRPH"><xsl:value-of select="position()"/></xsl:attribute>
			<xsl:attribute name="FareApplicationType">
				<xsl:choose>
					<xsl:when test="Type1Qual/RTInd = 'R'">Return</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="Type1Qual/Class"/>
			</xsl:attribute>
			<TravelDates>
				<xsl:attribute name="DepartureDate"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:attribute name="ArrivalDate"><xsl:value-of select="$end"/></xsl:attribute>
			</TravelDates>
			<FareReference><xsl:value-of select="Type1Qual/FIC"/></FareReference>
			<RuleInfo>
				<xsl:if test="Type1Qual/Validity != ''">
					<LengthOfStayRules>
						<MinimumStay>
							<xsl:attribute name="MinStay"><xsl:value-of select="substring-before(Type1Qual/Validity,'/')"/></xsl:attribute>
						</MinimumStay>
						<MaximumStay>
							<xsl:attribute name="MaxStay"><xsl:value-of select="substring-after(Type1Qual/Validity,'/')"/></xsl:attribute>
						</MaximumStay>
					</LengthOfStayRules>
				</xsl:if>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="Type1Qual/AirV"/></xsl:attribute>
			</FilingAirline>
			<DepartureLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="substring(../Tariff[UniqueKey='0']/CitiesHeading,1,3)"/></xsl:attribute>
			</DepartureLocation>
			<ArrivalLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="substring(../Tariff[UniqueKey='0']/CitiesHeading,4,3)"/></xsl:attribute>
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
					<xsl:when test="Type1Qual/PFInd = '-'">
						<xsl:attribute name="NegotiatedFare">true</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="NegotiatedFare">false</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:attribute name="PassengerTypeCode">
					<xsl:choose>
						<xsl:when test="HasFreeForm = 'Y'">
							<xsl:value-of select="substring-after(FreeForm,'PTC:')"/>
						</xsl:when>
						<xsl:when test="PIC = ''">
							<xsl:text>ADT</xsl:text>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="PIC"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<BaseFare>
					<xsl:attribute name="Amount"><xsl:value-of select="translate(Type1Qual/Fare,'.','')"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(Type1Qual/Fare,'.'))"/></xsl:attribute>
				</BaseFare>
			</PricingInfo>
		</FareDisplayInfo>
	</xsl:template>
	
	<xsl:template match="Tariff" mode="type2">
		<xsl:param name="start"/>
		<xsl:param name="end"/>
		<FareDisplayInfo>
			<xsl:attribute name="FareRPH"><xsl:value-of select="position()"/></xsl:attribute>
			<xsl:attribute name="FareApplicationType">
				<xsl:choose>
					<xsl:when test="Type2Qual/RTInd = 'R'">Return</xsl:when>
					<xsl:otherwise>OneWay</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="ResBookDesigCode">
				<xsl:value-of select="substring(Type2Qual/FIC,1,1)"/>
			</xsl:attribute>
			<TravelDates>
				<xsl:attribute name="DepartureDate"><xsl:value-of select="$start"/></xsl:attribute>
				<xsl:attribute name="ArrivalDate"><xsl:value-of select="$end"/></xsl:attribute>
			</TravelDates>
			<FareReference><xsl:value-of select="Type2Qual/FIC"/></FareReference>
			<RuleInfo>
				<xsl:if test="Type2Qual/MinStay != '' or Type2Qual/MaxStay != ''">
					<LengthOfStayRules>
						<MinimumStay>
							<xsl:attribute name="MinStay"><xsl:value-of select="Type2Qual/MinStay"/></xsl:attribute>
						</MinimumStay>
						<MaximumStay>
							<xsl:attribute name="MaxStay"><xsl:value-of select="Type2Qual/MaxStay"/></xsl:attribute>
						</MaximumStay>
					</LengthOfStayRules>
				</xsl:if>
			</RuleInfo>
			<FilingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="Type2Qual/AirV"/></xsl:attribute>
			</FilingAirline>
			<DepartureLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="substring(../Tariff[UniqueKey='0']/CitiesHeading,1,3)"/></xsl:attribute>
			</DepartureLocation>
			<ArrivalLocation>
				<xsl:attribute name="LocationCode"><xsl:value-of select="substring(../Tariff[UniqueKey='0']/CitiesHeading,4,3)"/></xsl:attribute>
			</ArrivalLocation>
			<xsl:if test="Type2Qual/Seasons != ''">
				<Restrictions>
					<Restriction>
						<DateRestriction>
							<xsl:attribute name="StartDate"><xsl:value-of select="substring-before(Type2Qual/Seasons,'-')"/></xsl:attribute>
							<xsl:attribute name="EndDate"><xsl:value-of select="substring-after(Type2Qual/Seasons,'-')"/></xsl:attribute>
						</DateRestriction>
					</Restriction>
				</Restrictions>
			</xsl:if>
			<PricingInfo>
				<xsl:choose>
					<xsl:when test="HasPF = 'Y'">
						<xsl:attribute name="NegotiatedFare">true</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="NegotiatedFare">false</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:attribute name="PassengerTypeCode">
					<xsl:choose>
						<xsl:when test="HasFreeForm = 'Y'">
							<xsl:value-of select="substring-after(FreeForm,'PTC:')"/>
						</xsl:when>
						<xsl:when test="PIC = ''">
							<xsl:text>ADT</xsl:text>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="PIC"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<BaseFare>
					<xsl:attribute name="Amount"><xsl:value-of select="translate(Type2Qual/Fare,'.','')"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(Type2Qual/Fare,'.'))"/></xsl:attribute>
				</BaseFare>
			</PricingInfo>
		</FareDisplayInfo>
	</xsl:template>

</xsl:stylesheet>
