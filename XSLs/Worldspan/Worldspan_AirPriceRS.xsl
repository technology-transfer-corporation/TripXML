<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_AirPriceRS.xsl 														-->
<!-- ================================================================== -->
<!-- Date: 28 Jan 2009 -  Rastko														-->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<xsl:apply-templates select="XXW"/>
	<xsl:apply-templates select="BPW9"/>
</xsl:template>

<xsl:template match="XXW">
	<OTA_AirPriceRS>
		<xsl:attribute name="Version">2.000</xsl:attribute>
		<Errors>
			<Error>
				<xsl:attribute name="Type">Worldspan</xsl:attribute>
				<xsl:attribute name="Code"><xsl:value-of select="ERROR/CODE"/></xsl:attribute>
			<xsl:value-of select="ERROR/TEXT"/>
			</Error>	
		</Errors>
	</OTA_AirPriceRS>
</xsl:template>

<xsl:template match="BPW9">
	<OTA_AirPriceRS>
		<xsl:attribute name="Version">2.000</xsl:attribute>
		<Success/>
		<xsl:if test="WARNING_INFO/WRN_ITEM/WRN_TEXT != ''">
			<Warnings>
				<xsl:for-each select="WARNING_INFO/WRN_ITEM">
					<Warning Type="Worldspan"><xsl:value-of select="WRN_TEXT"/></Warning>
				</xsl:for-each>
			</Warnings>
		</xsl:if>
		<PricedItineraries>
			<PricedItinerary>
				<xsl:attribute name="SequenceNumber">1</xsl:attribute>
				<xsl:apply-templates select="PRICING_INFO"/>
			</PricedItinerary>
		</PricedItineraries>
	</OTA_AirPriceRS>
</xsl:template>

<xsl:template match="PRICING_INFO">
	<AirItineraryPricingInfo>
		<xsl:choose>
			<xsl:when test="SRC_NUM = 'PUB'">
				<xsl:attribute name="PricingSource">Published</xsl:attribute>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="PricingSource">Private</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
		<ItinTotalFare>
			<xsl:variable name="base">
				<xsl:value-of select="BASEFARE_ALLPAX" />
			</xsl:variable>
			<xsl:variable name="total">
				<xsl:value-of select="TOTALFARE_ALLPAX" />
			</xsl:variable>
			<BaseFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$base"/></xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="CURRENCY != ''"><xsl:value-of select="CURRENCY"/></xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces"><xsl:value-of select="format-number(DEC_POS,'0')"/></xsl:attribute>
			</BaseFare>
			<Taxes>
				<Tax>
					<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
					<xsl:attribute name="Amount"><xsl:value-of select="$total - $base"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="CURRENCY != ''"><xsl:value-of select="CURRENCY"/></xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="format-number(DEC_POS,'0')"/></xsl:attribute>
				</Tax>
			</Taxes>
			<TotalFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$total"/></xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="CURRENCY != ''"><xsl:value-of select="CURRENCY"/></xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DecimalPlaces"><xsl:value-of select="format-number(DEC_POS,'0')"/></xsl:attribute>
			</TotalFare>
		</ItinTotalFare>
		<PTC_FareBreakdowns>
			<xsl:apply-templates select="PTC" mode="ptc"/>
		</PTC_FareBreakdowns>
		<FareInfos>
			<xsl:apply-templates select="../SEGMENT_INFO"/>
		</FareInfos>
	</AirItineraryPricingInfo>
</xsl:template>

<xsl:template match="PTC" mode="ptc">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="base"><xsl:value-of select="PTC_BASEFARE" /></xsl:variable>
	<xsl:variable name="total"><xsl:value-of select="PTC_TOTALFARE" /></xsl:variable>
	<xsl:variable name="nip"><xsl:value-of select="PTC_NUMPAX"/></xsl:variable> 
	<xsl:variable name="totbase"><xsl:value-of select="$base * $nip"/></xsl:variable>
	<xsl:variable name="tottotal"><xsl:value-of select="$total * $nip"/></xsl:variable>
	<xsl:variable name="tottax"><xsl:value-of select="$tottotal - $totbase"/></xsl:variable>
	<PTC_FareBreakdown>
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:choose>
					<xsl:when test="PTC_CODE = 'CNN'">CHD</xsl:when>
					<xsl:when test="PTC_CODE = 'GGV'">GOV</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="PTC_CODE" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Quantity"><xsl:value-of select="PTC_NUMPAX"/></xsl:attribute>
		</PassengerTypeQuantity>
		<FareBasisCodes>
			<xsl:for-each select="../TRIPS/PTCREF[position()=$pos]/FAREBASIS_PTC">
				<FareBasisCode><xsl:value-of select="." /></FareBasisCode>
			</xsl:for-each>
		</FareBasisCodes>
		<PassengerFare>
			<BaseFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$totbase"/></xsl:attribute>
			</BaseFare>
			<Taxes>
				<Tax>
					<xsl:attribute name="TaxCode">TotalTax</xsl:attribute>
					<xsl:attribute name="Amount"><xsl:value-of select="$tottax"/></xsl:attribute>
				</Tax>
			</Taxes>
			<TotalFare>
				<xsl:attribute name="Amount"><xsl:value-of select="$tottotal" /></xsl:attribute>
			</TotalFare>
		</PassengerFare>
	</PTC_FareBreakdown>
</xsl:template>

<xsl:template match="SEGMENT_INFO">
	<FareInfo>
		<DepartureDate>
			<xsl:text>2009</xsl:text>
			<xsl:text>-</xsl:text>
			<xsl:call-template name="month">
				<xsl:with-param name="month"><xsl:value-of select="DEP_DATE/DEP_MONTH"/></xsl:with-param>
			</xsl:call-template>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="DEP_DATE/DEP_DAY"/>
			<xsl:text>T</xsl:text>
			<xsl:value-of select="DEP_TIME/DEP_HOUR"/>
			<xsl:text>:</xsl:text>
			<xsl:value-of select="DEP_TIME/DEP_MIN"/>
			<xsl:text>:00</xsl:text>
		</DepartureDate> 
		<FareReference><xsl:value-of select="DEP_CLASS"/></FareReference> 
		<FilingAirline><xsl:value-of select="AIRLINE_CODE"/></FilingAirline> 
		<DepartureAirport>
			<xsl:attribute name="LocationCode"><xsl:value-of select="DEP_AIRPORT"/></xsl:attribute>
		</DepartureAirport>
		<ArrivalAirport>
			<xsl:attribute name="LocationCode"><xsl:value-of select="ARRIV_AIRPORT"/></xsl:attribute>
		</ArrivalAirport>
	</FareInfo>
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