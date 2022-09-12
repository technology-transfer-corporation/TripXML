<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Sabre_AirFareDisplayRQ.xsl 														-->
<!-- ================================================================== -->
<!-- Date: 27 Oct 2005 -  Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirFareDisplayRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_AirFareDisplayRQ">
		<FareRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07"  Version="2003A.TsabreXML1.5.1">
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
			<TPA_Extensions>
				<MessagingDetails>
					<MDRSubset Code="FQ06"/>
				</MessagingDetails>
			</TPA_Extensions>
			<OriginLocation>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="OriginDestinationInformation[1]/OriginLocation/@LocationCode"/>
				</xsl:attribute>
				<xsl:attribute name="CodeContext">IATA</xsl:attribute>
			</OriginLocation>
			<DestinationLocation>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="OriginDestinationInformation[1]/DestinationLocation/@LocationCode"/>
				</xsl:attribute>
				<xsl:attribute name="CodeContext">IATA</xsl:attribute>
			</DestinationLocation>
			<TravelDateOptions>
				<xsl:attribute name="Date">
					<xsl:value-of select="OriginDestinationInformation[1]/DepartureDateTime"/>
				</xsl:attribute>
				<!--xsl:if test="count(OriginDestinationInformation) > 1">
					<xsl:attribute name="ReturnDate">
						<xsl:value-of select="OriginDestinationInformation[position()=last()]/DepartureDateTime"/>
					</xsl:attribute>
				</xsl:if-->
			</TravelDateOptions>
			<xsl:choose>
				<xsl:when test="TravelPreferences/VendorPref[1]/@Code != ''">
					<xsl:for-each select="TravelPreferences/VendorPref">
						<VendorPref>
							<xsl:attribute name="Code">
								<xsl:value-of select="@Code"/>
							</xsl:attribute>
						</VendorPref>
					</xsl:for-each>
				</xsl:when>
				<xsl:otherwise>
					<VendorPref>
							<xsl:attribute name="Code">YY</xsl:attribute>
						</VendorPref>
				</xsl:otherwise>
			</xsl:choose>
			<FareTypeQualifiers>
				<xsl:if test="TravelPreferences/FareTypePref/@FareType != ''">
					<FareType>
						<xsl:attribute name="Type">
							<xsl:value-of select="TravelPreferences/FareTypePref/@FareType"/>
						</xsl:attribute>
					</FareType>
				</xsl:if>
				<!--xsl:choose>
					<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Private'">
						<PrivateFares Ind="true" /> 
					</xsl:when>
					<xsl:when test="TravelPreferences/FareTypePref/@FareType = 'Both'">
						<PrivateFares Ind="true" /> 
						<PublicFares Ind="true" /> 
					</xsl:when>
					<xsl:otherwise>
						<PublicFares Ind="true" />
					</xsl:otherwise>
				</xsl:choose-->
			</FareTypeQualifiers>
			<MiscQualifiers>
				<!--xsl:choose>
					<xsl:when test="count(OriginDestinationInformation) = 1">
						<JourneyType>
							<xsl:attribute name="Type">OW</xsl:attribute>
						</JourneyType>
					</xsl:when>
					<xsl:when test="count(OriginDestinationInformation) = 2">
						<JourneyType>
							<xsl:attribute name="Type">RT</xsl:attribute>
						</JourneyType>
					</xsl:when>
					<xsl:otherwise>
						<JourneyType>
							<xsl:attribute name="Type">HR</xsl:attribute>
						</JourneyType>
					</xsl:otherwise>
				</xsl:choose-->
				<xsl:if test="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode/@Code != ''">
					<FareBasisCode>
						<xsl:value-of select="TravelPreferences/FareAccessTypePref/NegotiatedFareCodes/NegotiatedFareCode/@Code"/>
					</FareBasisCode>
				</xsl:if>
				<xsl:if test="POS/Source/@ISOCurrency != ''">
					<Currency>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="POS/Source/@ISOCurrency"/>
						</xsl:attribute>
					</Currency>
				</xsl:if>
				<!--Validation Ind="true" /-->
				<!--xsl:for-each select="TravelerInfoSummary/PassengerTypeQuantity">
					<PassengerType>
						<xsl:attribute name="Code">
							<xsl:choose>
								<xsl:when test="@Code = 'CHD'">C09</xsl:when>
								<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</PassengerType>
				</xsl:for-each-->
			</MiscQualifiers>
		</FareRQ>
	</xsl:template>

</xsl:stylesheet>