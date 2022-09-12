<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_PNRSplitRQ.xsl 													-->
<!-- ================================================================== -->
<!-- Date: 07 Jun 2014 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNRSplit>
			<xsl:apply-templates select="OTA_PNRSplitRQ" />
		</PNRSplit>
	</xsl:template>
	<xsl:template match="OTA_PNRSplitRQ">
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
		<SplitRequest>
			<PNR_Split>
				<splitDetails>
					<passenger>
						<type>PT</type>
						<tattoo><xsl:value-of select="Travelers/TravelerRef[1]"/></tattoo>
					</passenger>
					<xsl:for-each select="Travelers/TravelerRef[position()>1]">
						<otherElement>
							<type>PT</type>
							<tattoo><xsl:value-of select="."/></tattoo>
						</otherElement>
					</xsl:for-each>
					<xsl:for-each select="Segments/SegmentRef">
						<otherElement>
							<type>ST</type>
							<tattoo><xsl:value-of select="."/></tattoo>
						</otherElement>
					</xsl:for-each>
					<xsl:for-each select="Elements/ElementRef">
						<otherElement>
							<type>OT</type>
							<tattoo><xsl:value-of select="."/></tattoo>
						</otherElement>
					</xsl:for-each>
				</splitDetails>
			</PNR_Split>
		</SplitRequest>
		<Split>
			<PNR_AddMultiElements>
				<pnrActions>
					<optionCode>14</optionCode>
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
		</Split>
		<ET>
			<PNR_AddMultiElements>
				<pnrActions>
					<optionCode>10</optionCode>
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
		<GetNewPNR>
			<PNR_RetrieveByRecLoc>
				<sbrRecLoc>
					<reservation>
						<controlNumber>XXXXXX</controlNumber>
					</reservation>
				</sbrRecLoc>
			</PNR_RetrieveByRecLoc>
		</GetNewPNR>
	</xsl:template>
</xsl:stylesheet>
