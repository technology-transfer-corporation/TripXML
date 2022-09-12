<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_AirFareInfoRQ.xsl 				       					                   -->
	<!-- ================================================================== -->
	<!-- Date: 17 Jun 2008 - Rastko														            -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<FareInfoRQ>
			<xsl:apply-templates select="OTA_AirFareInfoRQ/TravelerInfoSummary/PassengerTypeQuantity"/>
		</FareInfoRQ>
	</xsl:template>
	
	<xsl:template match="PassengerTypeQuantity">
		<Cryptic_GetScreen_Query>
			<xsl:variable name="pax"><xsl:value-of select="@Code"/></xsl:variable>
			<xsl:apply-templates select="//OTA_AirFareInfoRQ" mode="pax">
				<xsl:with-param name="pax"><xsl:value-of select="$pax"/></xsl:with-param>
			</xsl:apply-templates>
		</Cryptic_GetScreen_Query>
	</xsl:template>
	
	<xsl:template match="OTA_AirFareInfoRQ" mode="pax">
		<xsl:param name="pax"/>
		<xsl:variable name="DepartureCity">
			<xsl:value-of select="OriginDestinationInformation[1]/FlightSegment[1]/DepartureAirport/@LocationCode"/>
		</xsl:variable>		
		
		<xsl:variable name="FareType">
			<xsl:choose>
				<xsl:when test="TravelPreferences/FareTypePref/@FareType != ''">
					<xsl:value-of select="TravelPreferences/FareTypePref/@FareType"/>
				</xsl:when>
				<xsl:otherwise>Lowest</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>		
		
		<xsl:variable name="FareCurrency">
			<xsl:choose>
				<xsl:when test="TravelPreferences/FareRestrictPref/@FareDisplayCurrency != '' and string-length(TravelPreferences/FareRestrictPref/@FareDisplayCurrency) &gt; 0">
					<xsl:value-of select="TravelPreferences/FareRestrictPref/@FareDisplayCurrency"/>
				</xsl:when>
				<xsl:otherwise>USD</xsl:otherwise>
			</xsl:choose>		
		</xsl:variable>
		
		<xsl:variable name="PaxType">
			<xsl:choose>
				<xsl:when test="$pax= 'C10'">RC10</xsl:when>
				<xsl:when test="$pax= 'CHD'">RC10</xsl:when>
				<xsl:when test="$pax= 'INF'">RINF</xsl:when>
				<xsl:otherwise>RADT</xsl:otherwise>
			</xsl:choose>		
		</xsl:variable>

		<Command>
			<xsl:text>FQP</xsl:text>
			<xsl:value-of select="$DepartureCity"/>
			<xsl:apply-templates select="OriginDestinationInformation"/>
			<xsl:text>/</xsl:text>
			<xsl:choose>
				<xsl:when test="$FareType='Lowest'">L</xsl:when>
				<xsl:otherwise>L</xsl:otherwise>
			</xsl:choose>
			<xsl:text>/</xsl:text>
			<xsl:value-of select="$PaxType"/>
			<xsl:text>,FC-</xsl:text>
			<xsl:value-of select="$FareCurrency"/>
			<xsl:if test="TravelPreferences/ValidatingAirline/@Code!=''">
				<xsl:text>,VC-</xsl:text>
				<xsl:value-of select="TravelPreferences/ValidatingAirline/@Code"/>
			</xsl:if>
		</Command>
	</xsl:template>
	
	<xsl:template match="OriginDestinationInformation">
		<!-- calculate node position -->
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		
		<!-- add connection indicator -->
		<xsl:if test="$pos &gt; 1">
			<xsl:text>-</xsl:text>
			
				<xsl:if test="FlightSegment[1]/DepartureAirport/@LocationCode != preceding-sibling::OriginDestinationInformation[1]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode">
					<xsl:text>--</xsl:text>
					<xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode"/>
				</xsl:if>		
				
		</xsl:if>
		
		<xsl:apply-templates select="FlightSegment"/>
	</xsl:template>
		
	<xsl:template match="FlightSegment">
	
		<!-- Add delimiter + date keyword -->
		<xsl:text>/D</xsl:text>
		
		<!-- parse date -->
		<xsl:variable name="DepDate">
			<xsl:value-of select="@DepartureDateTime"/>
		</xsl:variable>		
		<xsl:variable name="DepDateMonth">
			<xsl:choose>
				<xsl:when test="substring(string($DepDate),6,2)='01'">JAN</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='02'">FEB</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='03'">MAR</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='04'">APR</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='05'">MAY</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='06'">JUN</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='07'">JUL</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='08'">AUG</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='09'">SEP</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='10'">OCT</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='11'">NOV</xsl:when>
				<xsl:when test="substring(string($DepDate),6,2)='12'">DEC</xsl:when>
			</xsl:choose>
		</xsl:variable>
		
		<xsl:variable name="DepDateDay">
			<xsl:value-of select="substring(string($DepDate),9,2)"/>
		</xsl:variable>
		
		<!-- Add departure date -->
		<xsl:value-of select="$DepDateDay"/>
		<xsl:value-of select="$DepDateMonth"/>
		
		<!-- parse vendor code -->
		<xsl:variable name="VendorCode">
			<xsl:choose>
				<xsl:when test="MarketingAirline/@Code != ''">
					<xsl:value-of select="MarketingAirline/@Code"/>
				</xsl:when>
				<xsl:otherwise>na</xsl:otherwise>
			</xsl:choose>		
		</xsl:variable>
		
		<!-- Add delimiter + vendor keyword -->
		<xsl:choose>
			<xsl:when test="$VendorCode != 'na'">
				<xsl:text>/A</xsl:text>
				<xsl:value-of select="$VendorCode"/>
			</xsl:when>
			<xsl:otherwise>/</xsl:otherwise>
		</xsl:choose>	
		
		<xsl:if test="@ResBookDesigCode != ''">
			<xsl:if test="$VendorCode != 'na'">/</xsl:if>
			<xsl:text>C</xsl:text>
			<xsl:value-of select="@ResBookDesigCode"/>
		</xsl:if>
				
		<!-- parse departure city -->
		<xsl:value-of select="ArrivalAirport/@LocationCode"/>
			
	</xsl:template>
</xsl:stylesheet>
