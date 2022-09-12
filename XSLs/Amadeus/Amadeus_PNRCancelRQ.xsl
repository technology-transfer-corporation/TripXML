<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_PNRCancelRQ.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 13 Nov 2009 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNRCancel>
			<xsl:apply-templates select="OTA_CancelRQ" />
		</PNRCancel>
	</xsl:template>
	<xsl:template match="OTA_CancelRQ">
		<UniqueID>
			<xsl:value-of select="UniqueID/@ID" />
		</UniqueID>
		<Retrieve>
			<PoweredPNR_Retrieve>
				<settings>
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
								<xsl:value-of select="UniqueID/@ID" />
							</controlNumber>
						</reservation>
					</reservationOrProfileIdentifier>
				</retrievalFacts>
			</PoweredPNR_Retrieve>
		</Retrieve>
		<CancelHotel>
			<PoweredPNR_Cancel>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<cancelElements>
					<!-- Cancel Itinerary -->
					<entryType>I</entryType>
				</cancelElements>
			</PoweredPNR_Cancel>
		</CancelHotel>
		<ET>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>11</optionCode>
				</pnrActions>
				<dataElementsMaster>
					<marker1>1</marker1>
					<dataElementsIndiv>
						<elementManagementData>
							<segmentName>RF</segmentName>
						</elementManagementData>
						<freetextData>
							<freetextDetail>
								<subjectQualifier>3</subjectQualifier>
								<type>P22</type>
							</freetextDetail>
							<longFreetext>
								<xsl:choose>
									<xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine/@AgentSine"/></xsl:when>
									<xsl:otherwise>TRIPXML</xsl:otherwise>
								</xsl:choose>
							</longFreetext>
						</freetextData>
					</dataElementsIndiv>
				</dataElementsMaster>
			</PoweredPNR_AddMultiElements>
		</ET>
		<Cancel>
			<PoweredPNR_Cancel>
				<pnrActions>
					<optionCode>11</optionCode>
				</pnrActions>
				<cancelElements>
					<!-- Cancel Itinerary -->
					<entryType>I</entryType>
				</cancelElements>
			</PoweredPNR_Cancel>
		</Cancel>
	</xsl:template>
</xsl:stylesheet>
