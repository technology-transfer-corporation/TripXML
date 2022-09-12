<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_PNRReadRS_FlightInfo.xsl 									       -->
	<!-- ================================================================== -->
	<!-- Date: 01 Mar 2012 - Rastko - new file											       -->
	<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PNR_RetrieveByRecLocReply"/>
		<xsl:apply-templates select="PNR_Reply"/>
	</xsl:template>
	<xsl:template match="PNR_RetrieveByRecLocReply | PNR_Reply">
		<FlightInfos>
			<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']" mode="Air"/>
		</FlightInfos>
	</xsl:template>

	<xsl:template match="itineraryInfo" mode="Air">
		<Air_FlightInfo>
			<generalFlightInfo>
				<flightDate>
					<departureDate><xsl:value-of select="travelProduct/product/depDate"/></departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></trueLocationId>
				</boardPointDetails>
				<offPointDetails>
					<trueLocationId><xsl:value-of select="travelProduct/offpointDetail/cityCode"/></trueLocationId>
				</offPointDetails>
				<companyDetails>
					<marketingCompany><xsl:value-of select="travelProduct/companyDetail/identification"/></marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber><xsl:value-of select="travelProduct/productDetails/identification"/></flightNumber>
				</flightIdentification>
			</generalFlightInfo>
		</Air_FlightInfo>
	</xsl:template>
</xsl:stylesheet>

