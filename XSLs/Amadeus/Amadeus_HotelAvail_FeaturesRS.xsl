<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================================== -->
<!-- HotelAvailRS Amadeus Features                                                                  -->
<!-- Copyright Elleipsis, Inc. 2004                                                     -->
<!-- ================================================================================== -->
<!-- Author        : Rastko Ilic                                                   -->
<!-- Date          : 24 Feb 2005                                                        -->
<!-- Defect number : new file                                                                -->
<!-- ================================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<PoweredHotels>
			<xsl:apply-templates select="PoweredHotel_AvailabilityMultiPropertiesReply"/>
			<xsl:apply-templates select="PoweredHotel_SingleAvailabilityReply"/>
		</PoweredHotels>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="PoweredHotel_AvailabilityMultiPropertiesReply">
		<xsl:apply-templates select="propertyAvailabilityList"/>
	</xsl:template>
	<xsl:template match="PoweredHotel_SingleAvailabilityReply">
		<PoweredHotel_Features>
			<xsl:apply-templates select="singleAvailabilityDetails/propertyAvailability/hotelProductInfo/propertyHeaderDetails"/>
		</PoweredHotel_Features>
	</xsl:template>

	<!-- ********************************************************************************************************** -->
	<xsl:template match="propertyAvailabilityList">
		<PoweredHotel_Features>
			<xsl:apply-templates select="hotelProductInfo/propertyHeaderDetails"/>
		</PoweredHotel_Features>
	</xsl:template>
	
	<xsl:template match="propertyHeaderDetails">
		<hotelPropertySelection>
			<propertyInformation>
				<chainCode><xsl:value-of select="chainCode"/></chainCode>
				<cityCode><xsl:value-of select="cityCode"/></cityCode>
				<propertyCode><xsl:value-of select="propertyCode"/></propertyCode>
			</propertyInformation>
			<displayCatSelection>
				<featuresCategory>1</featuresCategory>
				<featuresCategory>11</featuresCategory>
				<featuresCategory>4</featuresCategory>
			</displayCatSelection>
		</hotelPropertySelection>
	</xsl:template>
	
</xsl:stylesheet>
