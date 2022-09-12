<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- v03_TravelFusion_TB_Errors.xsl                     								                   -->
<!-- ================================================================== -->
<!-- Date: 06 Nov 2012 - Rastko - added mapping for ProcessTerms errors		                   -->
<!-- Date: 20 Oct 2012 - Rastko - added mapping for Errors tag				                   -->
<!-- Date: 11 Apr 2012 - Rastko - new file											                   -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CommandList/CommandExecutionFailure"/>
		<xsl:apply-templates select="CommandList/@etext"/>
		<xsl:apply-templates select="CommandList/DataValidationFailure/StartBooking"/>
		<xsl:apply-templates select="CommandList/DataValidationFailure/ProcessTerms"/>
		<xsl:apply-templates select="Errors/Error"/>
	</xsl:template>
	
	<xsl:template match="CommandExecutionFailure">
		<Error>
			<xsl:attribute name="Type">TravelFusion</xsl:attribute>
			<xsl:value-of select="ProcessDetails/@etext"/>
			<xsl:value-of select="StartBooking/@etext"/>
			<xsl:value-of select="ProcessTerms/@etext"/>
		</Error>
	</xsl:template>
	
	<xsl:template match="StartBooking">
		<Error>
			<xsl:attribute name="Type">TravelFusion</xsl:attribute>
			<xsl:choose>
				<xsl:when test="ExpectedPrice/Amount!=''">
					<xsl:value-of select="'Booking expected price of '"/>
					<xsl:value-of select="ExpectedPrice/Amount"/>
					<xsl:value-of select="' does not match the journey final price of '"/>
					<xsl:value-of select="JourneyPrice/Amount"/>
				</xsl:when>
				<xsl:otherwise><xsl:value-of select="@etext"/></xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>
	
	<xsl:template match="ProcessTerms">
		<Error>
			<xsl:attribute name="Type">TravelFusion</xsl:attribute>
			<xsl:value-of select="'Some passenger input data is invalid'"/>
		</Error>
	</xsl:template>
	
	<xsl:template match="@etext | Error">
		<Error>
			<xsl:attribute name="Type">TravelFusion</xsl:attribute>
			<xsl:value-of select="."/>
		</Error>
	</xsl:template>
	
</xsl:stylesheet>
