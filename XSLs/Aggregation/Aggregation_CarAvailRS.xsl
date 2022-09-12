<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Aggregation_CarAvailRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 21 Aug 2010 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="SuperRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="SuperRS">
		<OTA_VehAvailRateRS>
			<xsl:attribute name="Version"><xsl:value-of select="OTA_VehAvailRateRS/@Version"/></xsl:attribute>
			<xsl:choose>
				<xsl:when test="OTA_VehAvailRateRS/Success">
					<xsl:if test="OTA_VehAvailRateRS/Errors/Error != ''">
						<Warnings>
							<xsl:for-each select="OTA_VehAvailRateRS">
								<xsl:if test="Errors/Error">
									<Warning>
										<xsl:value-of select="Errors/Error[position()=last()]" />
									</Warning>
								</xsl:if>
							</xsl:for-each>
						</Warnings>
					</xsl:if>
					<Success/>
					<VehAvailRSCore>
						<VehRentalCore>
							<xsl:attribute name="PickUpDateTime">
								<xsl:value-of select="OTA_VehAvailRateRS/VehAvailRSCore/VehRentalCore/@PickUpDateTime"/>
							</xsl:attribute>
							<xsl:attribute name="ReturnDateTime">
								<xsl:value-of select="OTA_VehAvailRateRS/VehAvailRSCore/VehRentalCore/@ReturnDateTime"/>
							</xsl:attribute>
							<PickUpLocation>
								<xsl:attribute name="LocationCode">
									<xsl:value-of select="substring(OTA_VehAvailRateRS/VehAvailRSCore/VehRentalCore/PickUpLocation/@LocationCode,1,3)"/>
								</xsl:attribute>
							</PickUpLocation>
							<ReturnLocation>
								<xsl:attribute name="LocationCode">
									<xsl:value-of select="substring(OTA_VehAvailRateRS/VehAvailRSCore/VehRentalCore/ReturnLocation/@LocationCode,1,3)"/>
								</xsl:attribute>
							</ReturnLocation>
						</VehRentalCore>
						<VehVendorAvails>
							<xsl:apply-templates select="OTA_VehAvailRateRS/VehAvailRSCore/VehVendorAvails/VehVendorAvail"/>
						</VehVendorAvails>
					</VehAvailRSCore>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<xsl:for-each select="OTA_VehAvailRateRS/Errors/Error">
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
				</xsl:otherwise>
			</xsl:choose>
		</OTA_VehAvailRateRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="VehVendorAvail">
		<xsl:copy-of select="." />
	</xsl:template>
	<!--*************************************************************-->
	
</xsl:stylesheet>
