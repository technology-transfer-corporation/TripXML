<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_PNRReadRS_Rules.xsl 										       -->
	<!-- ================================================================== -->
	<!-- Date: 14 Nov 2012 - Rastko - new file											       -->
	<!-- ================================================================== -->
	<xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PNR_RetrieveByRecLocReply"/>
		<xsl:apply-templates select="PNR_Reply"/>
	</xsl:template>
	<xsl:template match="PNR_RetrieveByRecLocReply | PNR_Reply">
		<OTA_AirRulesRQ>
			<xsl:attribute name="Version"><xsl:value-of select="'1.000'"/></xsl:attribute>
			<AirItinerary>
				<OriginDestinationOptions>		
					<OriginDestinationOption>
							<xsl:apply-templates select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR']" mode="Air"/>
					</OriginDestinationOption>
				</OriginDestinationOptions>
			</AirItinerary>
			<TravelerInfoSummary>
				<SeatsRequested><xsl:value-of select="originDestinationDetails/itineraryInfo[elementManagementItinerary/segmentName='AIR'][1]/relatedProduct/quantity"/></SeatsRequested>
				<AirTravelerAvail>
					<xsl:apply-templates select="Ticket_DisplayTSTReply/fareList"/>
				</AirTravelerAvail>
				<PriceRequestInformation>
					<xsl:attribute name="PricingSource">
						<xsl:choose><xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'B'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'F'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/tstInformation/tstIndicator = 'G'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/fcmi = 'F'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/fcmi = 'I'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/fcmi = 'M'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/fcmi = 'N'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/fcmi = 'R'">Private</xsl:when><xsl:when test="fareList[1]/pricingInformation/fcmi = '7'">Private</xsl:when><xsl:otherwise>Published</xsl:otherwise></xsl:choose>
					</xsl:attribute>
				</PriceRequestInformation>
			</TravelerInfoSummary>
		</OTA_AirRulesRQ>
	</xsl:template>
	
	<xsl:template match="fareList">
		<PassengerTypeQuantity>
			<xsl:attribute name="Code">
				<xsl:variable name="paxref"><xsl:value-of select="paxSegReference/refDetails[1]/refNumber"/></xsl:variable><xsl:variable name="paxtype"><xsl:choose><xsl:when test="statusInformation/firstStatusDetails[1]/tstFlag='INF'">INF</xsl:when><xsl:when test="count(../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData)>1"><xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData[1]/travellerInformation/passenger/type"/></xsl:when><xsl:otherwise><xsl:value-of select="../../travellerInfo[elementManagementPassenger/reference/number=$paxref]/passengerData/travellerInformation/passenger/type"/></xsl:otherwise></xsl:choose></xsl:variable><xsl:choose><xsl:when test="$paxtype = ''">ADT</xsl:when><xsl:when test="$paxtype = 'CH'">CHD</xsl:when><xsl:when test="$paxtype = 'INN'">CHD</xsl:when><xsl:when test="$paxtype = 'CNN'">CHD</xsl:when><xsl:when test="$paxtype = 'YCD'">SRC</xsl:when><xsl:otherwise><xsl:value-of select="$paxtype"/></xsl:otherwise></xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Quantity"><xsl:value-of select="count(paxSegReference/refDetails)"/></xsl:attribute>
		</PassengerTypeQuantity> 
	</xsl:template>

	<xsl:template match="itineraryInfo" mode="Air">
		<FlightSegment>
			<xsl:attribute name="DepartureDateTime">
				<xsl:text>20</xsl:text><xsl:value-of select="substring(travelProduct/product/depDate,5,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(travelProduct/product/depDate,3,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(travelProduct/product/depDate,1,2)"/><xsl:text>T</xsl:text><xsl:choose><xsl:when test="substring(travelProduct/product/depTime,1,2)='24'">00</xsl:when><xsl:otherwise><xsl:value-of select="substring(travelProduct/product/depTime,1,2)"/></xsl:otherwise></xsl:choose><xsl:value-of select="concat(':',substring(travelProduct/product/depTime,3,2),':00')"/>
			</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">
				<xsl:text>20</xsl:text><xsl:value-of select="substring(travelProduct/product/arrDate,5,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(travelProduct/product/arrDate,3,2)"/><xsl:text>-</xsl:text><xsl:value-of select="substring(travelProduct/product/arrDate,1,2)"/><xsl:text>T</xsl:text><xsl:choose><xsl:when test="substring(travelProduct/product/arrTime,1,2)='24'">00</xsl:when><xsl:otherwise><xsl:value-of select="substring(travelProduct/product/arrTime,1,2)"/></xsl:otherwise></xsl:choose><xsl:value-of select="concat(':',substring(travelProduct/product/arrTime,3,2),':00')"/>
			</xsl:attribute>
			<xsl:attribute name="FlightNumber"><xsl:value-of select="travelProduct/productDetails/identification"/></xsl:attribute>
			<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="travelProduct/productDetails/classOfService"/></xsl:attribute>
			<DepartureAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="travelProduct/boardpointDetail/cityCode"/></xsl:attribute>
			</DepartureAirport>
			<ArrivalAirport>
				<xsl:attribute name="LocationCode"><xsl:value-of select="travelProduct/offpointDetail/cityCode"/></xsl:attribute>
			</ArrivalAirport> 
			<MarketingAirline>
				<xsl:attribute name="Code"><xsl:value-of select="travelProduct/companyDetail/identification"/></xsl:attribute>
			</MarketingAirline>
		</FlightSegment>
	</xsl:template>
</xsl:stylesheet>
