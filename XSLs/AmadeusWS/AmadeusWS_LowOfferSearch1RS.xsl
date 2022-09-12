<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowOfferSearch1RS.xsl 											-->
	<!-- ================================================================== -->
	<!-- Date: 30 Sep 2012 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_SellByFareSearchReply" />
		<xsl:apply-templates select="MessagesOnly_Reply" />
	</xsl:template>

	<xsl:template match="MessagesOnly_Reply">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="Fare_SellByFareSearchReply">
		<Fare_SellByFareSearchReply>
			<xsl:choose>
				<xsl:when test="errorMessage">
					<xsl:copy-of select="errorMessage"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="replyStatus"/>
					<xsl:copy-of select="conversionRate"/>
					<xsl:apply-templates select="recommendation"/>
				</xsl:otherwise>
			</xsl:choose>
		</Fare_SellByFareSearchReply>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="recommendation">
		<xsl:apply-templates select="segmentFlightRef"/>
	</xsl:template>
	
	<xsl:template match="segmentFlightRef">
		<recommendation>
			<xsl:copy-of select="../itemNumber"/>
			<xsl:copy-of select="../recPriceInfo"/>
			<xsl:copy-of select="../paxFareProduct"/>
			<xsl:variable name="fref">
				<xsl:value-of select="referencingDetail[refQualifier='A']/refNumber"/>
			</xsl:variable>
			<xsl:copy-of select="../specificRecDetails[specificRecItem/refNumber=$fref]"/>
			<segmentFlightRef>
				<xsl:apply-templates select="referencingDetail[refQualifier='S']"/>
			</segmentFlightRef>	
		</recommendation>
	</xsl:template>

	<xsl:template match="referencingDetail">
		<xsl:variable name="rank"><xsl:value-of select="position()"/></xsl:variable>
		<xsl:variable name="ref"><xsl:value-of select="refNumber"/></xsl:variable>
		<OD>
			<xsl:copy-of select="../../../flightIndex[position() = $rank]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]/propFlightGrDetail"/>
			<xsl:copy-of select="../../../flightIndex[position() = $rank]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]/flightDetails"/>
		</OD>
	</xsl:template>
</xsl:stylesheet>
