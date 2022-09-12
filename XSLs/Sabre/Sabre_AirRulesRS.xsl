<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirRulesRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_AirRulesRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_AirRulesRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_AirRulesRS">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="Errors/Error/@Code != ''">
										<xsl:value-of select="Errors/Error/@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:value-of select="Errors/Error" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="not(FareRuleResponseInfo/FareRuleInfo) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<FareRuleResponseInfo>
						<FareRuleInfo>
							<DepartureDate>
								<xsl:value-of select="FareRuleResponseInfo/FareRuleInfo/DepartureDate" />
							</DepartureDate>
							<FareReference>
								<xsl:value-of select="FareRuleResponseInfo/FareRuleInfo/FareReference/@RuleReference" />
							</FareReference>
							<FilingAirline Code="{FareRuleResponseInfo/FareRuleInfo/FilingAirline/@Code}" />
							<DepartureAirport LocationCode="{FareRuleResponseInfo/FareRuleInfo/DepartureAirport/@LocationCode}" />
							<ArrivalAirport LocationCode="{FareRuleResponseInfo/FareRuleInfo/ArrivalAirport/@LocationCode}" />
						</FareRuleInfo>
						<FareRules Language="{FareRuleResponseInfo/FareRules/@Language}" Title="{FareRuleResponseInfo/FareRules/@Title}">
							<xsl:apply-templates select="FareRuleResponseInfo/FareRules/SubSection" />
						</FareRules>
					</FareRuleResponseInfo>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirRulesRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="SubSection">
		<SubSection>
			<xsl:attribute name="SubSectionNumber">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<xsl:attribute name="SubTitle">
				<xsl:value-of select="@SubTitle" />
			</xsl:attribute>
			<Paragraph ParagraphNumber="{Paragraph/@ParagraphNumber}">
				<Text>
					<xsl:value-of select="Paragraph/Text"/>
				</Text>
			</Paragraph>
		</SubSection>
	</xsl:template>
</xsl:stylesheet>
