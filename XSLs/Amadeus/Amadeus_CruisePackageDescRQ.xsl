<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruisePackageDescRQ.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 25 Dec 2005 - Rastko											        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruisePackageDescRQ">
	<xsl:element name="CruiseByPass_PrePostPackageDescription">
		<xsl:element name="agentEnvironment">
			<xsl:element name="agentTerminalId">
				<xsl:value-of select="('12345678')"/>
			</xsl:element>
		</xsl:element>
		<xsl:element name="sailingGroup">
			<xsl:element name="sailingDescription">
				<xsl:element name="providerDetails">
					<xsl:element name="shipCode">
						<xsl:value-of select="SailingInfo/SelectedSailing/@ShipCode"/>
					</xsl:element>
					<xsl:element name="cruiselineCode">
						<xsl:value-of select="SailingInfo/SelectedSailing/@VendorCode"/>
					</xsl:element>
				</xsl:element>
				<xsl:element name="sailingDateTime">
					<xsl:element name="sailingDepartureDate">
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,9,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,6,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,1,4)"/>
					</xsl:element>
					<xsl:element name="sailingDuration">
						<xsl:value-of select="SailingInfo/SelectedSailing/@Duration"/>
					</xsl:element>
				</xsl:element>
				<xsl:apply-templates select="SailingInfo/SelectedSailing/@VoyageID"/>
			</xsl:element>
			<packageDescription>
				<packageType>
					<xsl:choose>
						<xsl:when test="SailingInfo/PackageOption/@PackageType = 'Pre'">B</xsl:when>
						<xsl:when test="SailingInfo/PackageOption/@PackageType = 'Bus'">Z</xsl:when>
						<xsl:when test="SailingInfo/PackageOption/@PackageType = 'Shore'">S</xsl:when>
						<xsl:otherwise>A</xsl:otherwise>
					</xsl:choose>
				</packageType>
				<packageDetails>
					<packageCode>
						<xsl:value-of select="SailingInfo/PackageOption/@PackageCode"/>
					</packageCode>
				</packageDetails>
			</packageDescription>
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="@VoyageID">
	<xsl:element name="sailingId">
		<xsl:element name="cruiseVoyageNbr">
			<xsl:value-of select="."/>
		</xsl:element>
	</xsl:element>
</xsl:template>

</xsl:stylesheet>
