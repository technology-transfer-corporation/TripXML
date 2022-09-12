<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_EncodeDecodeRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 18 Jan 2006 - Rastko									   					       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<EncodeDecodeRQ>
			<xsl:apply-templates select="OTA_EncodeDecodeRQ/Encode/CityOrAirportName" />
			<xsl:apply-templates select="OTA_EncodeDecodeRQ/Decode/CityOrAirportCode" />
		</EncodeDecodeRQ>
	</xsl:template>
	
	<xsl:template match="CityOrAirportName">
		<PoweredInfo_EncodeDecodeCity>
			<locationInformation>
				<locationType>L</locationType>
				<locationDescription>
					<name><xsl:value-of select="translate(@Name,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/></name>
				</locationDescription>
			</locationInformation>
			<requestOption>
				<selectionDetails>
					<option>ALG</option>
					<optionInformation>EXT</optionInformation>
				</selectionDetails>
			</requestOption>
			<xsl:if test="@CountryCode != ''">
				<countryStateRestriction>
					<countryIdentification>
						<countryCode><xsl:value-of select="@CountryCode"/></countryCode>
						<stateCode><xsl:value-of select="@StateCode "/></stateCode>
					</countryIdentification>
				</countryStateRestriction>
			</xsl:if>
		</PoweredInfo_EncodeDecodeCity>
	</xsl:template>
	
	<xsl:template match="CityOrAirportCode">
		<PoweredInfo_EncodeDecodeCity>
			<locationInformation>
				<locationType>L</locationType>
				<locationDescription>
					<code><xsl:value-of select="@Code"/></code>
				</locationDescription>
			</locationInformation>
		</PoweredInfo_EncodeDecodeCity>
	</xsl:template>

</xsl:stylesheet>
