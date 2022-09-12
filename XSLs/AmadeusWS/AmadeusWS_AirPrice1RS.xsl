<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirPrice1RS.xsl	 											-->
	<!-- ================================================================== -->
	<!-- Date: 15 Jan 2011 - Shashin - new file										-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="FIP" />
	</xsl:template>
	<xsl:template match="FIP">
		<Fare_InformativePricingWithoutPNRReply>
			<xsl:copy-of select="Errors"/>
			<xsl:copy-of select="Fare_InformativePricingWithoutPNRReply/messageDetails"/>
			<xsl:copy-of select="Fare_InformativePricingWithoutPNRReply/errorGroup"/>
			<mainGroup>
				<xsl:copy-of select="Fare_InformativePricingWithoutPNRReply/mainGroup/dummySegment"/>
				<xsl:copy-of select="Fare_InformativePricingWithoutPNRReply/mainGroup/convertionRate"/>
				<xsl:copy-of select="Fare_InformativePricingWithoutPNRReply/mainGroup/generalIndicatorsGroup"/>
				<xsl:for-each select="Fare_InformativePricingWithoutPNR/passengersGroup">
					<xsl:variable name="paxid"><xsl:value-of select="travellersID/travellerDetails/measurementValue"/></xsl:variable>
					<xsl:copy-of select="../../Fare_InformativePricingWithoutPNRReply/mainGroup/pricingGroupLevelGroup[passengersID/travellerDetails/measurementValue=$paxid][position()=last()]"/>
				</xsl:for-each>
			</mainGroup>
		</Fare_InformativePricingWithoutPNRReply>
	</xsl:template>
</xsl:stylesheet>
