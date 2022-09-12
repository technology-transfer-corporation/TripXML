<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_LowFareFlights1RS.xsl 											-->
	<!-- ================================================================== -->
	<!-- Date: 27 Sep 2012 -  Rastko - added case when multiple pax types have same fare		-->
	<!-- Date: 13 Aug 2011 -  Rastko - mapped errorGroup								-->
	<!-- Date: 29 Apr 2011 - Rastko - corrected error processing						-->
	<!-- Date: 15 Jan 2011 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="FIP" />
	</xsl:template>
	<xsl:template match="FIP">
		<Fare_InformativeBestPricingWithoutPNRReply>
			<xsl:copy-of select="Errors"/>
			<xsl:copy-of select="Fare_InformativeBestPricingWithoutPNRReply/messageDetails"/>
			<xsl:copy-of select="Fare_InformativeBestPricingWithoutPNRReply/errorGroup"/>
			<mainGroup>
				<xsl:copy-of select="Fare_InformativeBestPricingWithoutPNRReply/mainGroup/dummySegment"/>
				<xsl:copy-of select="Fare_InformativeBestPricingWithoutPNRReply/mainGroup/convertionRate"/>
				<xsl:copy-of select="Fare_InformativeBestPricingWithoutPNRReply/mainGroup/generalIndicatorsGroup"/>
				<xsl:for-each select="Fare_InformativeBestPricingWithoutPNR/passengersGroup">
					<xsl:variable name="paxid"><xsl:value-of select="travellersID/travellerDetails[1]/measurementValue"/></xsl:variable>
						<xsl:choose>
							<xsl:when test="../../Fare_InformativeBestPricingWithoutPNRReply/mainGroup/pricingGroupLevelGroup[passengersID/travellerDetails[1]/measurementValue=$paxid][position()=last()]">
								<xsl:copy-of select="../../Fare_InformativeBestPricingWithoutPNRReply/mainGroup/pricingGroupLevelGroup[passengersID/travellerDetails[1]/measurementValue=$paxid][position()=last()]"/>
							</xsl:when>
							<xsl:otherwise>
								<pricingGroupLevelGroup/>
							</xsl:otherwise>
						</xsl:choose>
				</xsl:for-each>
			</mainGroup>
		</Fare_InformativeBestPricingWithoutPNRReply>
	</xsl:template>
</xsl:stylesheet>
