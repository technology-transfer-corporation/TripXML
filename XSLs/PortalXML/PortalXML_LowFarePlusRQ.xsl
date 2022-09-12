<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- PortalXML_LowFarePlusRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 11 Aug 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">		
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ"/>	
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="OTA_AirLowFareSearchPlusRQ">
		<xsl:text>&amp;PortalXML=Y&amp;AirSegCount=</xsl:text>
		<xsl:value-of select="count(OriginDestinationInformation)"/>
		<xsl:text>&amp;aCount=</xsl:text>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='ADT']/@Quantity"/>
			</xsl:when>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='JCB']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='JCB']/@Quantity"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
		<xsl:text>&amp;sCount=</xsl:text>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='SRC']/@Quantity"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
		<xsl:text>&amp;cCount=</xsl:text>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='CHD']/@Quantity"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
		<xsl:text>&amp;yCount=</xsl:text>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='YTH']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='YTH']/@Quantity"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
		<xsl:text>&amp;iSCount=</xsl:text>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INS']/@Quantity"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
		<xsl:text>&amp;iLCount=</xsl:text>
		<xsl:choose>
			<xsl:when test="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity!=''">
				<xsl:value-of select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity[@Code='INF']/@Quantity"/>
			</xsl:when>
			<xsl:otherwise>0</xsl:otherwise>
		</xsl:choose>
		<xsl:text>&amp;Airline=</xsl:text>
		<xsl:text>&amp;Class=</xsl:text>
		<xsl:value-of select="TravelPreferences/CabinPref/@Cabin"/>
		<xsl:text>&amp;DirectFlight=</xsl:text>
		<xsl:text>&amp;Refundable=</xsl:text>
		<xsl:for-each select="OriginDestinationInformation">
			<xsl:text>&amp;AirSeg_</xsl:text>
			<xsl:value-of select="position()"/>
			<xsl:text>=</xsl:text>
			<xsl:value-of select="OriginLocation/@LocationCode"/>
			<xsl:text>|</xsl:text>
			<xsl:value-of select="DestinationLocation/@LocationCode"/>
			<xsl:text>|</xsl:text>
			<xsl:value-of select="substring(DepartureDateTime,9,2)"/>
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(DepartureDateTime,6,2)" />
				</xsl:with-param>
			</xsl:call-template>
			<xsl:value-of select="substring(DepartureDateTime,1,4)"/>
			<xsl:text>|</xsl:text>
			<xsl:value-of select="translate(substring(DepartureDateTime,12),':','')"/>
		</xsl:for-each>
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
