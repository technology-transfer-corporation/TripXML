<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CarListRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 25 Dec 2012 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Car_LocationListReply" />
	</xsl:template>
	<xsl:template match="Car_LocationListReply">
		<OTA_VehLocSearchRS Version="1.001">
			<Success />
			<VehMatchedLocs>
				<xsl:apply-templates select="carLocationListDetails" />
			</VehMatchedLocs>
		</OTA_VehLocSearchRS>
	</xsl:template>
	<xsl:template match="carLocationListDetails">
		<VehMatchedLoc>
			<LocationDetail>
				<xsl:attribute name="Code">
					<xsl:value-of select="locationAddress/locationInformation/locationCode"/>
				</xsl:attribute>
				<Address>
					<xsl:if test="locationAddress/addressInfo/number!=''">
						<StreetNmbr><xsl:value-of select="locationAddress/addressInfo/number"/></StreetNmbr>
					</xsl:if>
					<AddressLine><xsl:value-of select="locationAddress/addressInfo/address"/></AddressLine>
					<CityName><xsl:value-of select="locationAddress/cityName"/></CityName>
					<PostalCode><xsl:value-of select="locationAddress/postalCode"/></PostalCode>
				</Address>
				<AdditionalInfo>
					<TPA_Extensions>
						<Vendor>
							<xsl:attribute name="Code"><xsl:value-of select="companyInfo/companyInformation/companyCode"/></xsl:attribute>
							<xsl:value-of select="companyInfo/companyInformation/companyName"/>
						</Vendor>
					</TPA_Extensions>
				</AdditionalInfo>
			</LocationDetail>
			<VehLocSearchCriterion>
				<Position>
					<xsl:attribute name="Latitude"><xsl:value-of select="geoCoding/position/porPosition/porLatitude"/></xsl:attribute>
					<xsl:attribute name="Longitude"><xsl:value-of select="geoCoding/position/porPosition/porLongitude"/></xsl:attribute>
				</Position>
			</VehLocSearchCriterion>
		</VehMatchedLoc>
	</xsl:template>
</xsl:stylesheet>
