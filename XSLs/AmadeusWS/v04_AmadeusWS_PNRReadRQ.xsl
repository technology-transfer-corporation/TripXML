<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- v04_AmadeusWS_PNRReadRQ.xsl 												-->
<!-- ================================================================== -->
<!-- Date: 08 Oct 2010 - Rastko - new file												-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNR_RetrieveByRecLoc>
			<sbrRecLoc>
				<reservation>
					<controlNumber>
						<xsl:choose>
							<xsl:when test="OTA_UpdateRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_UpdateRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_TransferOwnershipRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_TransferOwnershipRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_StoredFareBuildRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_StoredFareBuildRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_StoredFareUpdateRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_StoredFareUpdateRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_TravelModifyRQ/UniqueId/@ID != ''">
									<xsl:value-of select="OTA_TravelModifyRQ/UniqueId/@ID" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID" />
								</xsl:otherwise>
						</xsl:choose>
					</controlNumber>
				</reservation>
			</sbrRecLoc>
			<!--settings>
				<options>
					<optionCode>51</optionCode>
				</options>
			</settings>
			<retrievalFacts>
				<retrieve>
					<type>2</type>
					<office></office>
				</retrieve>
				<reservationOrProfileIdentifier>
					<reservation>
						<controlNumber>
							<xsl:choose>
								<xsl:when test="OTA_UpdateRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_UpdateRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_TransferOwnershipRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_TransferOwnershipRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_StoredFareBuildRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_StoredFareBuildRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_StoredFareUpdateRQ/UniqueID/@ID != ''">
									<xsl:value-of select="OTA_StoredFareUpdateRQ/UniqueID/@ID" />
								</xsl:when>
								<xsl:when test="OTA_TravelModifyRQ/UniqueId/@ID != ''">
									<xsl:value-of select="OTA_TravelModifyRQ/UniqueId/@ID" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID" />
								</xsl:otherwise>
							</xsl:choose>
						</controlNumber>
					</reservation>
				</reservationOrProfileIdentifier>
			</retrievalFacts-->
		</PNR_RetrieveByRecLoc>
	</xsl:template>
</xsl:stylesheet>
