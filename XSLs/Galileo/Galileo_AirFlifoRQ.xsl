<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!--************************************************************************************************************-->
	<!--  					Flight Information Request				                                          -->
	<!--************************************************************************************************************-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<FlightInfo_6_0>
			<FltInfoMods>
				<ItemAry>
					<Item>
						<xsl:apply-templates select="OTA_AirFlifoRQ" />
					</Item>
				</ItemAry>
			</FltInfoMods>
		</FlightInfo_6_0>
	</xsl:template>
	<xsl:template match="OTA_AirFlifoRQ">
		<DataBlkInd>
			<xsl:text><![CDATA[F ]]></xsl:text>
		</DataBlkInd>
		<FltQual>
			<AirV>
				<xsl:value-of select="Airline/@Code" />
			</AirV>
			<StartCity>
				<xsl:value-of select="DepartureAirport/@LocationCode" />
			</StartCity>
			<EndCity>
				<xsl:value-of select="ArrivalAirport/@LocationCode" />
			</EndCity>
			<FltNum>
				<xsl:value-of select="FlightNumber" />
			</FltNum>
			<StartDt>
				<xsl:value-of select="substring(DepartureDate,1,4)" />
				<xsl:value-of select="substring(DepartureDate,6,2)" />
				<xsl:value-of select="substring(DepartureDate,9,2)" />
			</StartDt>
		</FltQual>
	</xsl:template>
</xsl:stylesheet>
