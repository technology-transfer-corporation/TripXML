<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- TravelFusion_StartRouting.xsl													-->
	<!-- ================================================================== -->
	<!-- Date: 5 Mar 2012 - Shashin														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirLowFareSearchRQ"/>
	</xsl:template>
	<xsl:template match="OTA_AirLowFareSearchRQ">
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
						<ResolutionTypeList>
							<ResolutionType>citycode</ResolutionType>
							<ResolutionType>airportcode</ResolutionType>
						</ResolutionTypeList>
						<Radius>0</Radius>
					</Origin>
				
					<Destination>
						<Descriptor>
							<xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode"/>
						</Descriptor>
						<Type>airportcode</Type>
						<Radius>0</Radius>
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
				<MaxChanges>1</MaxChanges>
				<MaxHops>2</MaxHops>
				<SupplierList>
					<Supplier>easyjet</Supplier>
				</SupplierList>
				<Timeout>40</Timeout>
				<TravelClass>Economy With Restrictions</TravelClass>
				<TravellerList>
					<Traveller>
						<Age>30</Age>
					</Traveller>
				</TravellerList>
				<IncrementalResults>true</IncrementalResults>
			</StartRouting>
		</CommandList>
	</xsl:template>
</xsl:stylesheet>
