<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_PNREndRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 13 Oct 2010 - Rastko - new file												       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_PNREndRQ" />
	</xsl:template>
	<!-- ************************************************************************************************************-->
	<xsl:template match="OTA_PNREndRQ">
		<PNR_AddMultiElements>
			<pnrActions>
				<optionCode>
					<xsl:choose>
						<xsl:when test="EndRequest/@Type='SaveOnly'">10</xsl:when>
						<xsl:when test="EndRequest/@Type='SaveAndRedisplay'">11</xsl:when>
						<xsl:when test="EndRequest/@Type='IgnoreOnly'">20</xsl:when>
						<xsl:when test="EndRequest/@Type='IgnoreAndRedisplay'">21</xsl:when>
						<xsl:when test="EndRequest/@Type='SaveAndShowWarnings'">30</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</optionCode>
			</pnrActions>
			<xsl:if test="EndRequest/@Type='SaveOnly' or EndRequest/@Type='SaveAndRedisplay' or EndRequest/@Type='SaveAndShowWarnings'">
				<dataElementsMaster>
					<marker1></marker1>
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
									<xsl:when test="POS/Source/@AgentSine != ''">
										<xsl:value-of select="POS/Source/@AgentSine"/>
									</xsl:when>
									<xsl:otherwise>TRIPXML</xsl:otherwise>
								</xsl:choose>
							</longFreetext>
						</freetextData>
					</dataElementsIndiv>
				</dataElementsMaster>
			</xsl:if>
		</PNR_AddMultiElements>
	</xsl:template>
</xsl:stylesheet>
