<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFarePlus1RS.xsl 												-->
	<!-- ================================================================== -->
	<!-- Date: 18 Dec 2012 - Rastko - added support for option 	MKT (2 one ways in response)	-->
	<!-- Date: 29 Sep 2012 - Rastko - added support for option 	LFPLight				-->
	<!-- Date: 14 Feb 2011 - Rastko - added new mapping for operating airline	 		-->
	<!-- Date: 21 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="LFPLight"><xsl:value-of select="Fare_MasterPricerTravelBoardSearchReply/LFPLight"/></xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_MasterPricerTravelBoardSearchReply" />
		<xsl:apply-templates select="Errors" />
	</xsl:template>

	<xsl:template match="Errors">
		<xsl:copy-of select="."/>
	</xsl:template>

	<xsl:template match="Fare_MasterPricerTravelBoardSearchReply">
		<Fare_MasterPricerTravelBoardSearchReply>
			<xsl:choose>
				<xsl:when test="errorMessage">
					<xsl:copy-of select="errorMessage"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:copy-of select="replyStatus"/>
					<xsl:copy-of select="conversionRate"/>
					<xsl:copy-of select="companyIdText"/>
					<xsl:apply-templates select="recommendation"/>
				</xsl:otherwise>
			</xsl:choose>
		</Fare_MasterPricerTravelBoardSearchReply>
	</xsl:template>
	<!--****************************************************************************************-->
	<!--											    -->
	<!--****************************************************************************************-->
	<xsl:template match="recommendation">
		<xsl:apply-templates select="segmentFlightRef[referencingDetail/refQualifier='S']"/>
	</xsl:template>
	
	<xsl:template match="segmentFlightRef">
		<recommendation>
			<xsl:copy-of select="../itemNumber"/>
			<xsl:copy-of select="../recPriceInfo"/>
			<xsl:if test="$LFPLight=''">
				<xsl:copy-of select="../paxFareProduct"/>
			</xsl:if>
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
		<xsl:variable name="flightrank"><xsl:value-of select="../../paxFareProduct/fareDetails[position()=$rank]/segmentRef/segRef"/></xsl:variable>
		<xsl:variable name="ref"><xsl:value-of select="refNumber"/></xsl:variable>
		<OD>
			<segRef><xsl:value-of select="$flightrank"/></segRef>
			<xsl:copy-of select="../../../flightIndex[position() = $flightrank]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]/propFlightGrDetail"/>
			<xsl:copy-of select="../../../flightIndex[position() = $flightrank]/groupOfFlights[propFlightGrDetail/flightProposal[1]/ref = $ref]/flightDetails"/>
		</OD>
	</xsl:template>
</xsl:stylesheet>
