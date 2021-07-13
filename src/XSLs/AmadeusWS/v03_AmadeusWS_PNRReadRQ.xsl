<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- v03_AmadeusWS_PNRReadRQ.xsl 													-->
<!-- ================================================================== -->
<!-- Date: 30 Dec 2015 - Rastko - added specific fix for JetBrave					-->
<!-- Date: 03 Nov 2014 - Rastko - corrected mapping of OTA_ReadRQ/UniqueID			-->
<!-- Date: 10 Aug 2014 - Rastko - added support for retrieve by passenger name			-->
<!-- Date: 08 Jul 2009 - Rastko																-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="username" select="OTA_ReadRQ/POS/TPA_Extensions/Provider/Userid"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_UpdateRQ/UniqueID | OTA_TransferOwnershipRQ/UniqueID | OTA_StoredFareBuildRQ/UniqueID | OTA_StoredFareUpdateRQ/UniqueID | OTA_TravelModifyRQ/UniqueId | OTA_ReadRQ/UniqueID[@ID!='']" />
    <xsl:apply-templates select="OTA_ReadRQ/ReadRequests[GlobalReservationReadRequest/TravelerName/Surname!='']"/>
	</xsl:template>
	
	<xsl:template match="UniqueID | UniqueId">
		<!--<xsl:choose>-->
				<PNR_Retrieve>
					<retrievalFacts>
						<retrieve>
							<type>2</type>
						</retrieve>
						<reservationOrProfileIdentifier>
							<reservation>
								<controlNumber>
									<xsl:value-of select="@ID" />
								</controlNumber>
							</reservation>
						</reservationOrProfileIdentifier>
					</retrievalFacts>
				</PNR_Retrieve>
			<!--<xsl:otherwise>
				<PNR_RetrieveByRecLoc>
					<sbrRecLoc>
						<reservation>
							<controlNumber>
								<xsl:value-of select="@ID" />
							</controlNumber>
						</reservation>
					</sbrRecLoc>
				</PNR_RetrieveByRecLoc>
			</xsl:otherwise>
		</xsl:choose>-->
	</xsl:template>
	
	<xsl:template match="ReadRequests">
		<PNR_Retrieve>
			<retrievalFacts>
				<retrieve>
					<type>3</type>
					<option1>A</option1>
				</retrieve>
				<personalFacts>
					<travellerInformation>
						<traveller>
							<surname>
								<xsl:value-of select="GlobalReservationReadRequest/TravelerName/Surname" />
							</surname>
						</traveller>
					</travellerInformation>
				</personalFacts>
			</retrievalFacts>
		</PNR_Retrieve>
	</xsl:template>
</xsl:stylesheet>
