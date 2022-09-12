<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CarInfoRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 29 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Car_PolicyReply" />
	</xsl:template>
	<xsl:template match="Car_PolicyReply">
		<OTA_VehLocDetailRS Version="1.001">
			<Success />
			<xsl:apply-templates select="locationAndCompanyGroup" />
			<Vehicles>
				<xsl:apply-templates select="scrollingGroup/vehiclePolicyDetails" />
			</Vehicles>
		</OTA_VehLocDetailRS>
	</xsl:template>
	<xsl:template match="locationAndCompanyGroup">
		<xsl:apply-templates select="companyInfo" />
		<xsl:apply-templates select="locationInformation" />
	</xsl:template>
	<xsl:template match="locationInformation">
		<LocationDetail>
			<xsl:attribute name="AtAirport">
				<xsl:choose>
					<xsl:when test="cityOrAirportTag='215'">
						<xsl:text>1</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>0</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="locationInfo/locationCode" />
			</xsl:attribute>
			<xsl:attribute name="Name">
				<xsl:value-of select="locationInfo/locationName" />
			</xsl:attribute>
		</LocationDetail>
	</xsl:template>
	<xsl:template match="companyInfo">
		<Vendor>
			<xsl:attribute name="Code">
				<xsl:value-of select="companyInformation/companyCode" />
			</xsl:attribute>
			<xsl:value-of select="companyInformation/companyName" />
		</Vendor>
	</xsl:template>
	<xsl:template match="vehiclePolicyDetails">
		<xsl:if test="boolean(vehicleIndicator/ruleDetails/ruleQuantityUnit)">
			<Vehicle>
				<xsl:apply-templates select="vehicleIndicator/ruleDetails" mode="Quantity" />
				<xsl:apply-templates select="vehicleInfo/vehicleIdentificationInfo" />
				<xsl:apply-templates select="vehicleIndicator/ruleDetails" mode="Model" />
			</Vehicle>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ruleDetails" mode="Quantity">
		<xsl:if test="ruleQuantityUnit='911'">
			<xsl:attribute name="PassengerQuantity">
				<xsl:value-of select="ruleQuantity" />
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="ruleQuantityUnit='915'">
			<xsl:attribute name="BaggageQuantity">
				<xsl:value-of select="ruleQuantity" />
			</xsl:attribute>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ruleDetails" mode="Model">
		<xsl:if test="boolean(ruleDescription)">
			<VehMakeModel>
				<xsl:attribute name="Name">
					<xsl:value-of select="ruleDescription" />
				</xsl:attribute>
			</VehMakeModel>
		</xsl:if>
	</xsl:template>
	<xsl:template match="vehicleIdentificationInfo">
		<VehType>
			<xsl:attribute name="VehicleCategory">
				<xsl:value-of select="vehicleType" />
			</xsl:attribute>
		</VehType>
	</xsl:template>
</xsl:stylesheet>
