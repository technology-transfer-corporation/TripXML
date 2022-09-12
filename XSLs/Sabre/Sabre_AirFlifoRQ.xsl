<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirFlifoRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_AirFlifoRQ">
		<OTA_AirFlifoRQ Version="2.0.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<OriginDestinationInformation>
				<FlightSegment DepartureDateTime="{DepartureDate}" FlightNumber="{FlightNumber}">
					<MarketingAirline Code="{Airline/@Code}" FlightNumber="{FlightNumber}"/>
				</FlightSegment>
			</OriginDestinationInformation>
		</OTA_AirFlifoRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
