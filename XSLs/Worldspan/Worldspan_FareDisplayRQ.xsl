<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_FareDisplayRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 26 Nov 2005 - Rastko														       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>
<xsl:template match="/">
	<FAC5>
		<MSG_VER>5</MSG_VER>
		<xsl:apply-templates select="OTA_AirFareDisplayRQ"/>
	</FAC5>
</xsl:template>
	
<xsl:template match="OTA_AirFareDisplayRQ">
	<DEP_APT><xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode"/></DEP_APT>
	<ARR_APT><xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode"/></ARR_APT>
	<xsl:if test="TravelPreferences/VendorPref/@Code != ''">
		<DEP_AIR_LST>
			<xsl:for-each select="TravelPreferences/VendorPref">
				<DEP_AIR><xsl:value-of select="@Code"/></DEP_AIR>
			</xsl:for-each>
		</DEP_AIR_LST>
	</xsl:if>
	<xsl:if test="TravelPreferences/CabinPref/@Cabin != ''">
		<DEP_CLS_LST>
			<xsl:choose>
				<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'First'">F</xsl:when>
				<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Business'">C</xsl:when>
				<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Economy'">Y</xsl:when>
			</xsl:choose>
		</DEP_CLS_LST>
	</xsl:if>
	<DEP_DAY><xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,9,2)"/></DEP_DAY>
	<DEP_MON>
		<xsl:call-template name="month">
			<xsl:with-param name="month"><xsl:value-of select="substring(OriginDestinationInformation[1]/DepartureDateTime,6,2)" /></xsl:with-param>
		</xsl:call-template>
	</DEP_MON>
	<RT_OW_IND>
		<xsl:choose>
			<xsl:when test="count(OriginDestinationInformation) = 1">O</xsl:when>
			<xsl:otherwise>R</xsl:otherwise>
		</xsl:choose>
	</RT_OW_IND>
	<xsl:if test="count(OriginDestinationInformation) > 1">
		<RET_DAY><xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,9,2)"/></RET_DAY>
		<RET_MON>
			<xsl:call-template name="month">
				<xsl:with-param name="month"><xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,6,2)" /></xsl:with-param>
			</xsl:call-template>
		</RET_MON>
	</xsl:if>
	<FAR_TYP_LST>
		<FAR_TYP>
			<xsl:choose>
				<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Private'">SR</xsl:when>
				<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Both'">SRA</xsl:when>
				<xsl:when test="TravelPreferences/FareAccessTypePref/@FareAccess = 'Private'">SR</xsl:when>
				<xsl:when test="TravelPreferences/FareAccessTypePref/@FareAccess = 'Both'">SRA</xsl:when>
				<xsl:otherwise>NLX</xsl:otherwise>
			</xsl:choose>
		</FAR_TYP>
	</FAR_TYP_LST>	
	<PFS_IND>Y</PFS_IND>
	<xsl:if test="TravelerInfoSummary/PassengerTypeQuantity and TravelerInfoSummary/PassengerTypeQuantity/@Code != 'JCB'">
		<PSG_TYP_COD_LST>
			<xsl:apply-templates select="TravelerInfoSummary/PassengerTypeQuantity"/>
		</PSG_TYP_COD_LST>
	</xsl:if>
</xsl:template>

<xsl:template match="PassengerTypeQuantity">
	<PSG_TYP_COD>
		<xsl:choose>
			<xsl:when test="@Code = 'CHD'">CNN</xsl:when>
			<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
		</xsl:choose>
	</PSG_TYP_COD>
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
