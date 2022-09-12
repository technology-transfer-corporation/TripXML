<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_StoredFareBuildRS.xsl 				       					      -->
	<!-- ================================================================== -->
	<!-- Date: 19 Nov 2010 - Rastko													            -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_StoredFareBuildRS>
			<xsl:choose>
				<xsl:when test="MessagesOnly_Reply != ''">
					<xsl:apply-templates select="MessagesOnly_Reply"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="Ticket_DisplayTSTReply"/>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_StoredFareBuildRS>
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply ">
		<Errors>
			<Error>
				<xsl:attribute name="Type"><xsl:value-of select="CAPI_Messages/LineType"/></xsl:attribute>
				<xsl:attribute name="Code"><xsl:value-of select="CAPI_Messages/ErrorCode"/></xsl:attribute>
				<xsl:value-of select="CAPI_Messages/Text"/>
			</Error>
		</Errors>
	</xsl:template>
	<xsl:template match="Ticket_DisplayTSTReply">
		<xsl:attribute name="Version">1.0</xsl:attribute>
		<xsl:choose>
			<xsl:when test="applicationError">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Amadeus</xsl:attribute>
						<xsl:attribute name="Code"><xsl:value-of select="applicationError/applicationErrorInfo/applicationErrorDetail/applicationErrorCode"/></xsl:attribute>
						<xsl:value-of select="applicationError/errorText/freeText"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="PNR_RetrieveByRecLocReply/pnrHeader/reservationInfo/reservation/controlNumber"/>
					</xsl:attribute>
				</UniqueID>
				<xsl:for-each select="fareList">
					<StoredFareInformation>
						<xsl:attribute name="RPH"><xsl:value-of select="fareReference/uniqueReference"/></xsl:attribute>
						<xsl:apply-templates select="paxSegReference/refDetails" mode="Traveler"/>
						<xsl:apply-templates select="segmentReference/refDetails" mode="Segment"/>
					</StoredFareInformation>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="refDetails" mode="Traveler">
		<TravelerReference>
			<xsl:attribute name="RPH">
				<xsl:variable name="ref"><xsl:value-of select="refNumber"/></xsl:variable>
				<xsl:value-of select="../../../PNR_RetrieveByRecLocReply/travellerInfo[elementManagementPassenger/reference/number=$ref]/elementManagementPassenger/lineNumber"/>
			</xsl:attribute>
		</TravelerReference>
	</xsl:template>
	<xsl:template match="refDetails" mode="Segment">

		<FlightSegmentReference>
			<xsl:attribute name="RPH"><xsl:value-of select="refNumber"/></xsl:attribute>
		</FlightSegmentReference>
	</xsl:template>
</xsl:stylesheet>
