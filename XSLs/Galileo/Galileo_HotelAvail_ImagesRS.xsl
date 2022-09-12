<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================================== -->
<!-- HotelAvailRS Image Viewer                                                                  -->
<!-- Copyright Elleipsis, Inc. 2004                                                     -->
<!-- ================================================================================== -->
<!-- Author        : Rastko Ilic                                                   -->
<!-- Date          : 16 Jan 2005                                                        -->
<!-- Defect number : N/A                                                                -->
<!-- Changes       : Upgraded multi avail to Galileo HotelAvailability_11_0_2 message  -->
<!-- Changes       : added Facilities description  -->
<!-- ================================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<ImageSetRequest>
			<xsl:apply-templates select="HotelCompleteAvailability_7_0"/>
			<xsl:apply-templates select="HotelAvailability_11_0_2"/>
		</ImageSetRequest>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelCompleteAvailability_7_0">
		<Success/>
		<xsl:apply-templates select="HotelCompleteAvailability"/>
	</xsl:template>
	<!-- ********************************************************************************************************** -->
	<xsl:template match="HotelAvailability_11_0_2">
		<Properties>
			<xsl:apply-templates select="HotelAvailability/HotelInsideShopProperty"/>
		</Properties>
	</xsl:template>
	
	<xsl:template match="HotelInsideShopProperty">
		<Property SignaturePhoto="true" SignatureSize="T">
			<xsl:attribute name="Id">
				<xsl:if test="string-length(PropNum) = 4">0</xsl:if>
				<xsl:value-of select="PropNum"/>
			</xsl:attribute>
			<Description GeneralInformation="true">
				<Keyword>DESC</Keyword>
				<Keyword>FACI</Keyword>
			</Description>
		</Property>
	</xsl:template>
	
</xsl:stylesheet>
