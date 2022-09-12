<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- TravelFusion_LowFarePlusRQ.xsl													-->
	<!-- ================================================================== -->
	<!-- Date: 19 Nov 2012 - Rastko - added radius parameter							-->
	<!-- Date: 5 Mar 2012 - Shashin														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ"/>
	</xsl:template>
	<xsl:template match="OTA_AirLowFareSearchPlusRQ">
		<OTA_AirLowFareSearchPlusRQ>
			<Start>
				<CommandList>
					<StartRouting>
						<XmlLoginId>***XMLLogin***</XmlLoginId>
						<LoginId>***LoginID***</LoginId>
						<Mode>plane</Mode>
						<Origin>
							<Descriptor>
								<xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode"/>
							</Descriptor>
							<Type>airportcode</Type>
							<Radius>1000</Radius>
						</Origin>
						<Destination>
							<Descriptor>
								<xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode"/>
							</Descriptor>
							<Type>airportcode</Type>
							<Radius>1000</Radius>
						</Destination>
						<OutwardDates>
							<DateOfSearch>
								<xsl:value-of select="substring(string(OriginDestinationInformation[1]/DepartureDateTime),9,2)"/>
								<xsl:text>/</xsl:text>
								<xsl:value-of select="substring(string(OriginDestinationInformation[1]/DepartureDateTime),6,2)"/>
								<xsl:text>/</xsl:text>
								<xsl:value-of select="substring(string(OriginDestinationInformation[1]/DepartureDateTime),1,4)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring(string(OriginDestinationInformation[1]/DepartureDateTime),12,2)"/>
								<xsl:text>:</xsl:text>
								<xsl:value-of select="substring(string(OriginDestinationInformation[1]/DepartureDateTime),15,2)"/>
							</DateOfSearch>
						</OutwardDates>
						<xsl:if test="OriginDestinationInformation[position()=2] != ''">
							<ReturnDates>
								<DateOfSearch>
									<xsl:value-of select="substring(string(OriginDestinationInformation[position()=2]/DepartureDateTime),9,2)"/>
									<xsl:text>/</xsl:text>
									<xsl:value-of select="substring(string(OriginDestinationInformation[position()=2]/DepartureDateTime),6,2)"/>
									<xsl:text>/</xsl:text>
									<xsl:value-of select="substring(string(OriginDestinationInformation[position()=2]/DepartureDateTime),1,4)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(string(OriginDestinationInformation[position()=2]/DepartureDateTime),12,2)"/>
									<xsl:text>:</xsl:text>
									<xsl:value-of select="substring(string(OriginDestinationInformation[position()=2]/DepartureDateTime),15,2)"/>
								</DateOfSearch>
							</ReturnDates>
						</xsl:if>
						<MaxChanges>1</MaxChanges>
						<MaxHops>2</MaxHops>
						<!--SupplierList>***SupplierList***</SupplierList-->
						<Timeout>40</Timeout>
						<TravelClass>
							<xsl:choose>
								<xsl:when test="TravelPreferences/CabinPref/@Cabin='Economy'">Economy With Restrictions</xsl:when>
								<xsl:otherwise>Economy With Restrictions</xsl:otherwise>
							</xsl:choose>
						</TravelClass>
						<TravellerList>
							<xsl:for-each select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity">
								<xsl:call-template name="create_pax">
									<xsl:with-param name="count"><xsl:value-of select="@Quantity" /></xsl:with-param>
									<xsl:with-param name="code"><xsl:value-of select="@Code" /></xsl:with-param>
								</xsl:call-template>
							</xsl:for-each>
						</TravellerList>
						<IncrementalResults>true</IncrementalResults>
					</StartRouting>
				</CommandList>
			</Start>
			<Check>
				<CommandList>
					<CheckRouting>
						<XmlLoginId>***XMLLogin***</XmlLoginId>
						<LoginId>***LoginID***</LoginId>
						<RoutingId>***RoutingID***</RoutingId>
					</CheckRouting>
				</CommandList>
			</Check>
		</OTA_AirLowFareSearchPlusRQ>
	</xsl:template>
	<xsl:template name="create_pax">
		<xsl:param name="count" />
		<xsl:param name="code" />
		<xsl:if test="$count !=0">
			<Traveller>
				<Age>
					<xsl:choose>
						<xsl:when test="$code='INF'">0</xsl:when>
						<xsl:when test="$code='CHD'">7</xsl:when>
						<xsl:otherwise>30</xsl:otherwise>
					</xsl:choose>
				</Age>
			</Traveller>			
			<xsl:call-template name="create_pax">
				<xsl:with-param name="count"><xsl:value-of select="$count -1" /></xsl:with-param>
				<xsl:with-param name="code"><xsl:value-of select="$code" /></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
