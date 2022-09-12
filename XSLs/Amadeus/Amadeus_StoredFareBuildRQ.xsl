<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_StoredFareBuildRQ.xsl 		    			       					      -->
	<!-- ================================================================== -->
	<!-- Date: 03 Jun 2008 - Rastko 													            -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<PoweredTicket_CreateManualTST>
			<xsl:apply-templates select="OTA_StoredFareBuildRQ"/>
			<xsl:apply-templates select="PoweredPNR_PNRReply"/>
		</PoweredTicket_CreateManualTST>
	</xsl:template>
	
	<xsl:template match="OTA_StoredFareBuildRQ">
		<paxReference>
			<xsl:apply-templates select="Traveler "/>					
		</paxReference>		
		<segReference>
			<xsl:apply-templates select="FlightSegment "/>			
		</segReference>		
		<pnrLocatorData>
			<reservationInformation>
				<controlNumber>
					<xsl:value-of select="UniqueID/@ID"/>
				 </controlNumber>
			</reservationInformation>
		</pnrLocatorData>
		<xsl:if test="Options /@Type!=''">
			<tstOption>
				<attributeDetails>
					<attributeType>
						<xsl:choose>
							<xsl:when test="Options /@Type='Sata'">SAT</xsl:when>
							<xsl:otherwise>OPN</xsl:otherwise>
						</xsl:choose>
					 </attributeType>
				</attributeDetails>
			</tstOption>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Traveler">
		<refDetails>
			<refQualifier>
				<xsl:choose>
					<xsl:when test="@Infant='true'">PI</xsl:when>
					<xsl:otherwise>PA</xsl:otherwise>
				</xsl:choose>
			</refQualifier>
			<refNumber>
				<xsl:variable name="rph">
					<xsl:value-of select="@RPH"/>
				</xsl:variable>
				<xsl:value-of select="../PoweredPNR_PNRReply/travellerInfo[elementManagementPassenger/lineNumber = $rph]/elementManagementPassenger/reference/number"/>
			 </refNumber>
		</refDetails>
	</xsl:template>
	
	<xsl:template match="FlightSegment ">
		<refDetails>
			<refQualifier>S</refQualifier>
			<refNumber>
				<xsl:value-of select="./@RPH"/>
			 </refNumber>
		</refDetails>
	</xsl:template>

	<xsl:template match="PoweredPNR_PNRReply">
		<paxReference>
			<xsl:variable name="pax">
				<xsl:value-of select="OTA_StoredFareUpdateRQ/Fare/@RPH"/>
			</xsl:variable>
			<xsl:apply-templates select="travellerInfo[elementManagementPassenger/lineNumber = $pax]"/>					
		</paxReference>		
		<segReference>
			<xsl:apply-templates select="originDestinationDetails/itineraryInfo"/>			
		</segReference>		
		<pnrLocatorData>
			<reservationInformation>
				<controlNumber>
					<xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber"/>
				 </controlNumber>
			</reservationInformation>
		</pnrLocatorData>
	</xsl:template>

	<xsl:template match="travellerInfo">
		<refDetails>
			<refQualifier>
				<xsl:choose>
					<xsl:when test="../OTA_StoredFareUpdateRQ/PassengerType/@Code='INF'">PI</xsl:when>
					<xsl:otherwise>PA</xsl:otherwise>
				</xsl:choose>
			</refQualifier>
			<refNumber>
				<xsl:value-of select="elementManagementPassenger/reference/number"/>
			 </refNumber>
		</refDetails>
	</xsl:template>
	
	<xsl:template match="itineraryInfo">
		<refDetails>
			<refQualifier>S</refQualifier>
			<refNumber>
				<xsl:value-of select="elementManagementItinerary/reference/number"/>
			 </refNumber>
		</refDetails>
	</xsl:template>

</xsl:stylesheet>
