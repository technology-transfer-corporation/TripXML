<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_AirRulesRQ.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 10 Jan 2006 - Rastko														       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<FRC3>
		<MSG_VER>3</MSG_VER> 
		<xsl:apply-templates select="OTA_AirRulesRQ/RuleReqInfo" />
	</FRC3>
</xsl:template>

<xsl:template match="RuleReqInfo">
	<DEP_ARP><xsl:value-of select="DepartureAirport/@LocationCode" /></DEP_ARP> 
	<ARR_ARP><xsl:value-of select="ArrivalAirport/@LocationCode" /></ARR_ARP> 
	<DEP_ARL><xsl:value-of select="FilingAirline/@Code" /></DEP_ARL> 
	<DEP_DAY><xsl:value-of select="substring(DepartureDate,9,2)" /></DEP_DAY> 
	<DEP_MON>
		<xsl:call-template name="month">
			<xsl:with-param name="month"><xsl:value-of select="substring(DepartureDate,6,2)" /></xsl:with-param>
		</xsl:call-template>
	</DEP_MON> 
	<FAR_BAS_COD><xsl:value-of select="FareReference" /></FAR_BAS_COD> 
	<xsl:if test="@NegotiatedFareCode = 'PFA'">
		<FAR_TYP>SR</FAR_TYP>
	</xsl:if>
	<FAR_BAS_CAT>ALL</FAR_BAS_CAT>
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
