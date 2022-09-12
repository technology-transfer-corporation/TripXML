<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_AirRulesRS.xsl 														       -->
<!-- ================================================================== -->
<!-- Date: 24 Jun 2005 - New message - Rastko										       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="FRW3" />
		<xsl:apply-templates select="XXW" />
	</xsl:template>
	
	<xsl:template match="FRW3">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<Success></Success>
			<FareRuleResponseInfo>
				<FareRuleInfo>
					<DepartureDate><xsl:value-of select="RuleReqInfo/DepartureDate"/></DepartureDate>
					<FareReference><xsl:value-of select="RuleReqInfo/FareReference"/></FareReference>
					<RuleInfo />
					<FilingAirline>
						<xsl:attribute name="Code"><xsl:value-of select="RuleReqInfo/FilingAirline/@Code"/></xsl:attribute>
					</FilingAirline> 
					<DepartureAirport>
						<xsl:attribute name="LocationCode"><xsl:value-of select="RuleReqInfo/DepartureAirport/@LocationCode"/></xsl:attribute>
					</DepartureAirport>
					<ArrivalAirport>
						<xsl:attribute name="LocationCode"><xsl:value-of select="RuleReqInfo/ArrivalAirport/@LocationCode"/></xsl:attribute>
					</ArrivalAirport>
				</FareRuleInfo>
				<FareRules>
					<SubSection>
						<xsl:apply-templates select="RUL_INF" mode="section"/>
					</SubSection>
				</FareRules>
			</FareRuleResponseInfo>
		</OTA_AirRulesRS>
	</xsl:template>
	
	<xsl:template match="RUL_INF" mode="section">
		<Paragraph>
			<Text>
				<xsl:value-of select="." />
			</Text>
		</Paragraph>
	</xsl:template>
	
	<xsl:template match="XXW">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<Errors>
				<Error>
					<xsl:attribute name="Type">Worldspan</xsl:attribute>
					<xsl:attribute name="Code">
						<xsl:value-of select="ERROR/CODE"/>
					</xsl:attribute>
					<xsl:value-of select="ERROR/TEXT"/>
				</Error>
			</Errors>
		</OTA_AirRulesRS>
	</xsl:template>

</xsl:stylesheet>
