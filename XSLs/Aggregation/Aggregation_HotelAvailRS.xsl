<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Aggregation_HotelAvailRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 24 Mar 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="SuperRS" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="SuperRS">
		<OTA_HotelAvailRS>
			<xsl:attribute name="Version"><xsl:value-of select="OTA_HotelAvailRS/@Version"/></xsl:attribute>
			<xsl:choose>
				<xsl:when test="OTA_HotelAvailRS/Success">
					<xsl:if test="OTA_HotelAvailRS/Errors/Error != ''">
						<Warnings>
							<xsl:for-each select="OTA_HotelAvailRS">
								<xsl:if test="Errors/Error">
									<Warning>
										<xsl:value-of select="Errors/Error[position()=last()]" />
									</Warning>
								</xsl:if>
							</xsl:for-each>
						</Warnings>
					</xsl:if>
					<Success/>
					<RoomStays>
						<xsl:apply-templates select="OTA_HotelAvailRS/RoomStays/RoomStay"/>
					</RoomStays>
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<xsl:for-each select="OTA_HotelAvailRS/Errors/Error">
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
		</OTA_HotelAvailRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RoomStay">
		<xsl:copy-of select="." />
	</xsl:template>
	<!--*************************************************************-->
	
</xsl:stylesheet>
