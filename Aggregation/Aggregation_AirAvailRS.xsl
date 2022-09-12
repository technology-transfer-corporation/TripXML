<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Aggregation_AirAvailRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 24 Mar 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="SuperRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="SuperRS">
		<OTA_AirAvailRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<Success></Success>
			<OriginDestinationOptions>
				<xsl:apply-templates select="OTA_AirAvailRS/OriginDestinationOptions/OriginDestinationOption">
					<xsl:sort data-type="text" order="ascending" select="FlightSegment[1]/@DepartureDateTime"/>
				</xsl:apply-templates>
			</OriginDestinationOptions>
			<xsl:if test="OTA_AirAvailRS/Errors/Error != ''">
				<Errors>
					<xsl:for-each select="OTA_AirAvailRS/Errors/Error">
						<Error>
							<xsl:attribute name="Type"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="@Code != ''">
										<xsl:value-of select="@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:value-of select="." />
						</Error>
					</xsl:for-each>
				</Errors>
			</xsl:if>
		</OTA_AirAvailRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="OriginDestinationOption">
		<OriginDestinationOption>
			<xsl:attribute name="Provider"><xsl:value-of select="../../@TransactionIdentifier"/></xsl:attribute>
			<xsl:copy-of select="TPA_Extensions" />
			<xsl:copy-of select="FlightSegment" />
		</OriginDestinationOption>
	</xsl:template>
	<!--*************************************************************-->
	
</xsl:stylesheet>
