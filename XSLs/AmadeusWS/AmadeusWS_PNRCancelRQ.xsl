<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_PNRCancelRQ.xsl 													-->
<!-- ================================================================== -->
<!-- Date: 06 Jun 2012 - Rastko - temp fix for 12trip									-->
<!-- Date: 21 Jun 2009 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="username" select="OTA_CancelRQ/POS/TPA_Extensions/Provider/Userid"/>
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
			<PNR_RetrieveByRecLoc>
				<sbrRecLoc>
					<reservation>
						<controlNumber>
							<xsl:value-of select="UniqueID/@ID" />
						</controlNumber>
					</reservation>
				</sbrRecLoc>
			</PNR_RetrieveByRecLoc>
		</Retrieve>
		<CancelHotel>
			<PNR_Cancel>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<cancelElements>
					<!-- Cancel Itinerary -->
					<entryType>I</entryType>
				</cancelElements>
			</PNR_Cancel>
		</CancelHotel>
		<ET>
			<PNR_AddMultiElements>
				<pnrActions>
					<optionCode>11</optionCode>
				</pnrActions>
				<dataElementsMaster>
					<marker1/>
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
									<xsl:when test="POS/Source/@AgentSine!=''">
										<xsl:value-of select="POS/Source/@AgentSine"/>
									</xsl:when>
									<xsl:otherwise>TRIPXML</xsl:otherwise>
								</xsl:choose>
							</longFreetext>
						</freetextData>
					</dataElementsIndiv>
				</dataElementsMaster>
			</PNR_AddMultiElements>
		</ET>
		<Cancel>
			<!--xsl:choose>
				<xsl:when test="$username='OneTwoTrip'">
					<Command_Cryptic>
						<messageAction>
							<messageFunctionDetails>
								<messageFunction>M</messageFunction>
							</messageFunctionDetails>
						</messageAction>
						<longTextString>
							<textStringDetails>XI</textStringDetails>
						</longTextString>
					</Command_Cryptic>
				</xsl:when>
				<xsl:otherwise-->
					<PNR_Cancel>
						<pnrActions>
							<optionCode>11</optionCode>
						</pnrActions>
						<cancelElements>
							<entryType>I</entryType>
						</cancelElements>
					</PNR_Cancel>
				<!--/xsl:otherwise>
			</xsl:choose-->
		</Cancel>
	</xsl:template>
</xsl:stylesheet>
