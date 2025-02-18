<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />

	<xsl:template match="/">
		<!--<xsl:apply-templates select="OTA_ShowMileageRQ"/>-->
		<xsl:apply-templates select="OTA_ShowMileageRQ" />
		<xsl:apply-templates select="OTA_ShowMilesRQ" />
	</xsl:template>

	<xsl:template match="OTA_ShowMileageRQ">
		<MileageRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" ReturnHostCommand="false" Version="2.0.0">
			<OriginDestinationInformation>
				<!--
				<POS>
					<Source>
						<xsl:attribute name="PseudoCityCode">
							<xsl:value-of select="POS/Source/@PseudoCityCode" />
						</xsl:attribute>
					</Source>
				</POS>
				-->
				<OriginLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="FromCity" />
					</xsl:attribute>
					<xsl:if test="Airline">
						<Airline>
							<xsl:attribute name="Code">
								<xsl:value-of select="Airline"/>
							</xsl:attribute>
						</Airline>
					</xsl:if>
				</OriginLocation>
				<xsl:apply-templates select="ToCity" />
			</OriginDestinationInformation>
		</MileageRQ>
	</xsl:template>
	<xsl:template match="OTA_ShowMilesRQ">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.6.1" ReturnHostCommand="true" >
		<xsl:element name="Request">
			<xsl:attribute name="Output">SCREEN</xsl:attribute>
			<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
			<xsl:attribute name="CDATA">true</xsl:attribute>
			<xsl:element name="HostCommand">
				<xsl:choose>
					<xsl:when test="TravelDate = ''">
						<xsl:value-of select="concat('V*', CarrierCode, FlightNummber)" />					
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('V*', CarrierCode, FlightNummber,'/',TravelDate)" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:element>
		</xsl:element>
		</SabreCommandLLSRQ>
	</xsl:template>

	<xsl:template match="ToCity">
		<DestinationLocation>
			<xsl:if test="Connection">
				<xsl:attribute name="ConnectionInd">
					<xsl:value-of select="Connection"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="LocationCode">
				<xsl:value-of select="." />
			</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()"/>
			</xsl:attribute>
			<xsl:if test="Airline">
				<Airline>
					<xsl:attribute name="Code">
						<xsl:value-of select="Airline"/>
					</xsl:attribute>
				</Airline>
			</xsl:if>
		</DestinationLocation>
	</xsl:template>

</xsl:stylesheet>
