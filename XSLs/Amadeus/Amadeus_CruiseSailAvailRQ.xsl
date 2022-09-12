<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruiseSailAvailRQ.xsl 	     											       -->
<!-- ================================================================== -->
<!-- Date: 14 Feb 2006 - Rastko											        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>
<xsl:template match="OTA_CruiseSailAvailRQ">
	<xsl:element name="CruiseByPass_SailingAvailability">
		<xsl:element name="processingInfo">
			<xsl:element name="processingDetails">
				<xsl:element name="businessType">
					<xsl:value-of select="('5')"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
		<xsl:element name="agentEnvironment">
			<xsl:element name="agentTerminalId">
				<xsl:value-of select="('12345678')"/>
			</xsl:element>
		</xsl:element>
		<xsl:element name="numberOfUnitsDescription">
			<xsl:element name="nbrOfUnitsDetails">
				<xsl:element name="unitValue">
					<xsl:value-of select="sum(GuestCounts/GuestCount/@Quantity)"/>
				</xsl:element>
				<xsl:element name="unitQualifier">
					<xsl:value-of select="('NI')"/>
				</xsl:element>
			</xsl:element>
		</xsl:element>
		<xsl:element name="sailingGroup">
			<xsl:element name="sailingDescription">
				<xsl:element name="providerDetails">
					<xsl:apply-templates select="CruiseLinePrefs/CruiseLinePref/@ShipCode"/>
					<xsl:apply-templates select="CruiseLinePrefs/CruiseLinePref"/>
				</xsl:element>
				<xsl:element name="sailingDateTime">
					<xsl:element name="sailingDepartureDate">
						<xsl:value-of select="substring(SailingDateRange/@Start,9,2)"/>
						<xsl:value-of select="substring(SailingDateRange/@Start,6,2)"/>
						<xsl:value-of select="substring(SailingDateRange/@Start,1,4)"/>
					</xsl:element>
					<xsl:element name="sailingDuration">
						<xsl:choose>
							<xsl:when test="SailingDateRange/@MinDuration != ''">
								<xsl:value-of select="SailingDateRange/@MinDuration"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="SailingDateRange/@Duration"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:element>
					<xsl:if test="SailingDateRange/@MaxDuration">
						<xsl:element name="maxSailingDuration">
							<xsl:value-of select="SailingDateRange/@MaxDuration"/>
						</xsl:element>
					</xsl:if>
				</xsl:element>
				<xsl:apply-templates select="RegionPref"/>
				<xsl:if test="CruiseLinePrefs/CruiseLinePref/SearchQualifiers">
					<xsl:element name="sailingDetails">
						<xsl:apply-templates select="CruiseLinePrefs/CruiseLinePref/SearchQualifiers[@GroupCode]"/>
					</xsl:element>
				</xsl:if>
			</xsl:element>
			<xsl:apply-templates select="CruiseLinePrefs/CruiseLinePref/InclusivePackageOption"/>
		</xsl:element>		
	</xsl:element>
</xsl:template>

<xsl:template match="@ShipCode">
	<xsl:element name="shipCode">
		<xsl:value-of select="."/>
	</xsl:element>
</xsl:template>

<xsl:template match="CruiseLinePref">
	<xsl:element name="cruiselineCode">
		<xsl:value-of select="@VendorCode"/>
	</xsl:element>
</xsl:template>

<xsl:template match="RegionPref">
	<xsl:element name="regionDetails">
		<xsl:element name="cruiseRegionCode">
			<xsl:value-of select="@RegionCode"/>
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="SearchQualifiers">
	<xsl:element name="sailingIndicators">
		<xsl:value-of select="('G')"/>
	</xsl:element>
</xsl:template>

<xsl:template match="InclusivePackageOption">
	<xsl:element name="packageDescription">
		<xsl:element name="packageType">
			<xsl:value-of select="('I')"/>
		</xsl:element>
		<xsl:element name="packageDetails">
			<xsl:element name="packageCode">
				<xsl:value-of select="@CruisePackageCode"/>
			</xsl:element>
		</xsl:element>
	</xsl:element>
</xsl:template>

</xsl:stylesheet>
