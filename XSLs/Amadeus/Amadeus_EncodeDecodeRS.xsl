<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_EncodeDecodeRS.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 18 Jan 2006 - Rastko									   					       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="EncodeDecodeRS" />
	</xsl:template>
	
	<xsl:template match="EncodeDecodeRS">
		<OTA_EncodeDecodeRS Version="1.000">
			<xsl:choose>
				<xsl:when test="(PoweredInfo_EncodeDecodeCityReply/errorDescription or MessagesOnly_Reply) and not(PoweredInfo_EncodeDecodeCityReply/mainLocation)">
					<Errors>
						<xsl:apply-templates select="PoweredInfo_EncodeDecodeCityReply/errorDescription" mode="error"/>
						<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages" mode="error"/>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<xsl:if test="(PoweredInfo_EncodeDecodeCityReply/errorDescription or MessagesOnly_Reply) and PoweredInfo_EncodeDecodeCityReply/mainLocation">
						<Warnings>
							<xsl:apply-templates select="PoweredInfo_EncodeDecodeCityReply/errorDescription" mode="warning"/>
							<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages" mode="warning"/>
						</Warnings>
					</xsl:if>
					<xsl:if test="PoweredInfo_EncodeDecodeCityReply/mainLocation">
						<Success/>
						<xsl:apply-templates select="PoweredInfo_EncodeDecodeCityReply[mainLocation]"/>
					</xsl:if>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_EncodeDecodeRS>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages" mode="error">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:value-of select="../@City"/>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages" mode="warning">
		<Warning>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:value-of select="../@City"/>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="Text" />
		</Warning>
	</xsl:template>

	<xsl:template match="PoweredInfo_EncodeDecodeCityReply">
		<xsl:apply-templates select="mainLocation"/>
	</xsl:template>
	
	<xsl:template match="mainLocation">
		<City>
			<xsl:attribute name="Code"><xsl:value-of select="locationInformation/locationDescription/code"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="locationInformation/locationDescription/name"/></xsl:attribute>
			<xsl:if test="countryStateInformation/countryIdentification/stateCode != ''">
				<xsl:attribute name="StateCode"><xsl:value-of select="countryStateInformation/countryIdentification/stateCode"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="CountryCode"><xsl:value-of select="countryStateInformation/countryIdentification/countryCode"/></xsl:attribute>
			<xsl:apply-templates select="relatedLocation"/>
		</City>
	</xsl:template>
	
	<xsl:template match="relatedLocation">
		<AssociatedLocation>
			<xsl:attribute name="Code"><xsl:value-of select="sublocationInformation/locationDescription/code"/></xsl:attribute>
			<xsl:attribute name="Name"><xsl:value-of select="sublocationInformation/locationDescription/name"/></xsl:attribute>
			<xsl:if test="countryStateInformation/countryIdentification/stateCode != ''">
				<xsl:attribute name="StateCode"><xsl:value-of select="countryStateInformation/countryIdentification/stateCode"/></xsl:attribute>
			</xsl:if>
			<xsl:attribute name="CountryCode"><xsl:value-of select="countryStateInformation/countryIdentification/countryCode"/></xsl:attribute>
			<xsl:attribute name="LocationType">
				<xsl:choose>
					<xsl:when test="sublocationInformation/locationType = 'A'">Airport</xsl:when>
					<xsl:when test="sublocationInformation/locationType = 'B'">Bus</xsl:when>
					<xsl:when test="sublocationInformation/locationType = 'R'">Rail</xsl:when>
					<xsl:when test="sublocationInformation/locationType = 'C'">City</xsl:when>
					<xsl:when test="sublocationInformation/locationType = 'G'">Ground</xsl:when>
					<xsl:when test="sublocationInformation/locationType = 'H'">Heliport</xsl:when>
					<xsl:when test="sublocationInformation/locationType = 'O'">Offpoint</xsl:when>
				</xsl:choose>
			</xsl:attribute>
		</AssociatedLocation>
	</xsl:template>
	
	<xsl:template match="errorDescription" mode="error">
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:attribute name="Code"><xsl:value-of select="appliErrorData/errorDetails/errorCode"/></xsl:attribute>
			<xsl:value-of select="../@City"/>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="freeTextInfo/freeText"/>
		</Error>
	</xsl:template>
	
	<xsl:template match="errorDescription" mode="warning">
		<Warning>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:attribute name="Code"><xsl:value-of select="appliErrorData/errorDetails/errorCode"/></xsl:attribute>
			<xsl:value-of select="../@City"/>
			<xsl:text> - </xsl:text>
			<xsl:value-of select="freeTextInfo/freeText"/>
		</Warning>
	</xsl:template>

</xsl:stylesheet>
