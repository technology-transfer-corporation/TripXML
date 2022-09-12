<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
	================================================================== 
	Sabre_AirAvailRQ.xsl 												
	================================================================== 
	Date: 06 Jun 2022 - Kobelev - Version update											
	Date: 08 Jul 2009 - Rastko											
	================================================================== 
	-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirAvailRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_AirAvailRQ">
		<OTA_AirAvailRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.4.0">
			<!--
			<xsl:attribute name="Target">
				<xsl:choose>
					<xsl:when test="POS/TPA_Extensions/Provider/System='Test'">CERT</xsl:when>
					<xsl:otherwise>Production</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
			-->
			<OriginDestinationInformation>
				<DepartureDateTime>
					<xsl:attribute name="DateTime">
						<xsl:value-of select="OriginDestinationInformation/DepartureDateTime" />
					</xsl:attribute>
				</DepartureDateTime>
				<OriginLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="OriginDestinationInformation/OriginLocation/@LocationCode" />
					</xsl:attribute>
				</OriginLocation>
				<DestinationLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="OriginDestinationInformation/DestinationLocation/@LocationCode" />
					</xsl:attribute>
				</DestinationLocation>
				<xsl:if test="OriginDestinationInformation/ConnectionLocations/ConnectionLocation">
					<ConnectionLocations>
						<xsl:apply-templates select="OriginDestinationInformation/ConnectionLocations/ConnectionLocation" />
					</ConnectionLocations>
				</xsl:if>
			</OriginDestinationInformation>
			<xsl:if test="TravelPreferences/VendorPref or TravelPreferences/FlightTypePref or TravelPreferences/CabinPref">
				<xsl:element name="TravelPreferences">
					<xsl:for-each select="TravelPreferences/VendorPref">
						<xsl:element name="VendorPref">
							<xsl:attribute name="Code">
								<xsl:value-of select="@Code"/>
							</xsl:attribute>
						</xsl:element>
					</xsl:for-each>
					<xsl:for-each select="TravelPreferences/FlightTypePref">
						<FlightTypePref>
							<xsl:attribute name="FlightType">
								<xsl:value-of select="@FlightType" />
							</xsl:attribute>
						</FlightTypePref>
					</xsl:for-each>
					<xsl:if test="TravelPreferences/CabinPref">
						<CabinPref>
							<xsl:attribute name="Cabin">
								<xsl:choose>
									<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Economy'">Y</xsl:when>
									<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'Business'">B</xsl:when>
									<xsl:when test="TravelPreferences/CabinPref/@Cabin = 'First'">F</xsl:when>
								</xsl:choose>
							</xsl:attribute>
						</CabinPref>
					</xsl:if>
				</xsl:element>
			</xsl:if>
			<!--TravelerInfoSummary>
				<SeatsRequested>
					<xsl:choose>
						<xsl:when test="TravelerInfoSummary/SeatsRequested &gt; 0">
							<xsl:value-of select="TravelerInfoSummary/SeatsRequested" />
						</xsl:when>
						<xsl:otherwise>1</xsl:otherwise>
					</xsl:choose>
				</SeatsRequested>
			</TravelerInfoSummary-->
		</OTA_AirAvailRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="ConnectionLocation">
		<ConnectionLocation>
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="@LocationCode" />
			</xsl:attribute>
		</ConnectionLocation>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
