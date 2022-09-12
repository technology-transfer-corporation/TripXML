<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fn="http://www.w3.org/2004/07/xpath-functions" xmlns:xdt="http://www.w3.org/2004/07/xpath-datatypes">
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="/">		
	<CruiseByPass_ItineraryDescription>
		<xsl:apply-templates select="OTA_CruiseItineraryDescRQ"/>
	</CruiseByPass_ItineraryDescription>	
</xsl:template>

<xsl:template match="OTA_CruiseItineraryDescRQ">
	<agentEnvironment>
		<agentTerminalId>
			<xsl:value-of select="('12345678')"/>
		</agentTerminalId>
	</agentEnvironment>
	<sailingGroup>
		<sailingDescription>
			<providerDetails>
				<shipCode>
					<xsl:value-of select="SelectedSailing/@ShipCode"/>
				</shipCode>
				<cruiselineCode>
					<xsl:value-of select="SelectedSailing/@VendorCode"/>
				</cruiselineCode>
			</providerDetails>
			<sailingDateTime>
				<sailingDepartureDate>
					<xsl:value-of select="substring(SelectedSailing/@Start,9,2)"/>
					<xsl:value-of select="substring(SelectedSailing/@Start,6,2)"/>
					<xsl:value-of select="substring(SelectedSailing/@Start,1,4)"/>
				</sailingDepartureDate>
				<sailingDuration>
					<xsl:value-of select="SelectedSailing/@Duration"/>
				</sailingDuration>
			</sailingDateTime>
			<xsl:apply-templates select="SelectedSailing/@VoyageID"/>
		</sailingDescription>
		<xsl:apply-templates select="PackageOption"/>
	</sailingGroup>		
</xsl:template>


<xsl:template match="@VoyageID">
	<sailingId>
		<cruiseVoyageNbr>
			<xsl:value-of select="."/>
		</cruiseVoyageNbr>
	</sailingId>
</xsl:template>

<xsl:template match="PackageOption">
	<packageDescription>
		<packageType>
			<xsl:value-of select="('I')"/>
		</packageType>
		<packageDetails>
			<packageCode>
				<xsl:value-of select="@CruisePackageCode"/>
			</packageCode>
		</packageDetails>
		<packageDateTime>
			<packageStartDate>
				<xsl:value-of select="@Start"/>
			</packageStartDate>
		</packageDateTime>
	</packageDescription>
</xsl:template>

</xsl:stylesheet>
