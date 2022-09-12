<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_CheckRulesPlus_Reply" />
	</xsl:template>
	<xsl:template match="Fare_CheckRulesPlus_Reply">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">
								<xsl:value-of select="Errors/Error/@Type" />
							</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:value-of select="Errors/Error/@Code" />
							</xsl:attribute>
							<xsl:value-of select="Errors/Error" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<FareRuleResponseInfo>
						<FareRuleInfo>
							<DepartureDate>
								<xsl:value-of select="RuleReqInfo/DepartureDate" />
							</DepartureDate>
							<FareReference>
								<xsl:value-of select="CAPI_PsgrFareTypeLinePlus/FareClass" />
							</FareReference>
							<RuleInfo />
							<FilingAirline Code="{RuleReqInfo/FilingAirline/@Code}" />
							<DepartureAirport LocationCode="{RuleReqInfo/DepartureAirport/@LocationCode}" />
							<ArrivalAirport LocationCode="{RuleReqInfo/ArrivalAirport/@LocationCode}" />
						</FareRuleInfo>
						<FareRules>
							<xsl:apply-templates select="CAPI_FareNotesPlus[CategoryCode != '']" mode="section" />
						</FareRules>
					</FareRuleResponseInfo>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirRulesRS>
	</xsl:template>
	<xsl:template match="CAPI_FareNotesPlus" mode="section">
		<SubSection>
			<xsl:attribute name="SubTitle">
				<xsl:value-of select="CategoryTitle" />
			</xsl:attribute>
			<xsl:attribute name="SubCode">
				<xsl:value-of select="CategoryCode" />
			</xsl:attribute>
			<xsl:attribute name="SubSectionNumber">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<xsl:apply-templates select="following-sibling::CAPI_FareNotesPlus[1]" mode="text" />
		</SubSection>
	</xsl:template>
	<xsl:template match="CAPI_FareNotesPlus" mode="text">
		<xsl:if test="CategoryContent != ''">
			<Paragraph>
				<Text>
					<xsl:value-of select="CategoryContent" />
				</Text>
			</Paragraph>
			<xsl:apply-templates select="following-sibling::CAPI_FareNotesPlus[1]" mode="text" />
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
