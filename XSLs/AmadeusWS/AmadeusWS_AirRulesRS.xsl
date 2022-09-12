<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_AirRulesRS.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 11 Aug 2009 - Rastko - corrected CheckRulesReply mapping				-->
	<!-- Date: 20 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="Fare_CheckRulesReply" />
		<xsl:apply-templates select="Fare_DisplayFaresForCityPairReply"/>
	</xsl:template>
	<xsl:template match="Fare_CheckRulesReply">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="errorInfo">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Amadeus</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:value-of select="errorInfo/rejectErrorCode/errorDetails/errorCode" />
							</xsl:attribute>
							<xsl:value-of select="errorInfo/errorFreeText/freeText" />
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
								<xsl:value-of select="RuleReqInfo/FareReference" />
							</FareReference>
							<RuleInfo />
							<FilingAirline Code="{RuleReqInfo/FilingAirline/@Code}" />
							<DepartureAirport LocationCode="{RuleReqInfo/DepartureAirport/@LocationCode}" />
							<ArrivalAirport LocationCode="{RuleReqInfo/ArrivalAirport/@LocationCode}" />
						</FareRuleInfo>
						<FareRules>
							<xsl:apply-templates select="tariffInfo" mode="section" />
						</FareRules>
					</FareRuleResponseInfo>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirRulesRS>
	</xsl:template>
	<xsl:template match="tariffInfo" mode="section">
		<SubSection>
			<xsl:attribute name="SubTitle">
				<xsl:value-of select="substring-after(fareRuleText[1]/freeText,'.')"/>
			</xsl:attribute>
			<xsl:attribute name="SubCode">
				<xsl:value-of select="substring-before(fareRuleText[1]/freeText,'.')"/>
			</xsl:attribute>
			<xsl:attribute name="SubSectionNumber">
				<xsl:value-of select="fareRuleInfo/ruleSectionLocalId" />
			</xsl:attribute>
			<xsl:apply-templates select="fareRuleText[position()>1]" mode="text" />
		</SubSection>
	</xsl:template>
	<xsl:template match="fareRuleText" mode="text">
		<xsl:if test="freeText != ''">
			<Paragraph>
				<Text>
					<xsl:value-of select="freeText" />
				</Text>
			</Paragraph>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Fare_DisplayFaresForCityPairReply">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<Errors>
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:choose>
						<xsl:when test="errorInfo">
							<xsl:attribute name="Code">
								<xsl:value-of select="errorInfo/rejectErrorCode/errorDetails/errorCode" />
							</xsl:attribute>
							<xsl:for-each select="errorInfo/errorFreeText/freeText">
								<xsl:if test="position()>1"><xsl:text> </xsl:text></xsl:if>
								<xsl:value-of select="." />
							</xsl:for-each>
						</xsl:when>
						<xsl:when test="flightDetails/itemGrp">
							<xsl:attribute name="Code">0</xsl:attribute>
							<xsl:text>FARE REFERENCE NOT FOUND</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Code">
								<xsl:value-of select="infoText[position()=last()]/freeTextQualification/informationType"/>
							</xsl:attribute>
							<xsl:value-of select="infoText[position()=last()]/freeText"/>
						</xsl:otherwise>
					</xsl:choose>
				</Error>
			</Errors>
		</OTA_AirRulesRS>
	</xsl:template>
</xsl:stylesheet>
