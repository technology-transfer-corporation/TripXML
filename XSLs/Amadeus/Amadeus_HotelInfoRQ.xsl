<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_HotelInfoRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 05 Mar 2006 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelDescriptiveInfoRQ" />
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="OTA_HotelDescriptiveInfoRQ">
		<PoweredHotel_Features>
			<hotelPropertySelection>
				<xsl:apply-templates select="HotelDescriptiveInfos/HotelDescriptiveInfo" />
			</hotelPropertySelection>
		</PoweredHotel_Features>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelDescriptiveInfo">
		<xsl:variable name="ArrDate">
			<xsl:value-of select="substring-before(TPA_Extensions/ArrivalDate,'T')" />
		</xsl:variable>
		<xsl:variable name="DepDate">
			<xsl:value-of select="substring-before(TPA_Extensions/DepartureDate,'T')" />
		</xsl:variable>
		<propertyInformation>
			<chainCode>
				<xsl:value-of select="@ChainCode" />
			</chainCode>
			<cityCode>
				<xsl:value-of select="@HotelCityCode" />
			</cityCode>
			<propertyCode>
				<xsl:value-of select="@HotelCode" />
			</propertyCode>
		</propertyInformation>
		<displayCatSelection>
			<!--Amadeus allows only a max of 3 featuresCategory fields -->
			<xsl:if test="ContactInfo/@SendData = 'true' or AreaInfo/@SendRefPoints = 'true'">
				<featuresCategory>1</featuresCategory>
			</xsl:if>
			<xsl:if test="HotelInfo/@SendData = 'true'">
				<featuresCategory>2</featuresCategory>
				<featuresCategory>4</featuresCategory>
			</xsl:if>
			<xsl:if test="Policies/@SendPolicies='true'">
				<featuresCategory>3</featuresCategory>
				<featuresCategory>5</featuresCategory>
				<featuresCategory>6</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo != ' ' ">
				<featuresCategory>11</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo/@SendMeetingRooms = 'true'">
				<featuresCategory>14</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo/@SendGuestRooms = 'true'">
				<featuresCategory>12</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo/@SendRestaurants = 'true'">
				<featuresCategory> 13</featuresCategory>
			</xsl:if>
		</displayCatSelection>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
</xsl:stylesheet>