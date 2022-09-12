<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_AirRulesRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 29 Aug 2006 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="FareQuoteRulesDisplay_8_0" />
		<xsl:apply-templates select="FareQuoteTariffDisplay_8_0" />
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="FareQuoteRulesDisplay_8_0">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="FareInfo/RespHeader/ErrMsg='Y' or ErrText/Err!='' or FareInfo/ErrorCode != ''">
					<Errors>
						<xsl:apply-templates select="//ErrText" />
						<xsl:apply-templates select="FareInfo/InfoMsg" mode="warning" />
					</Errors>
				</xsl:when>
				<xsl:when test="TransactionErrorCode">
					<Warnings>
						<xsl:apply-templates select="TransactionErrorCode" />
					</Warnings>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<FareRuleResponseInfo>
						<FareRuleInfo>
							<!--FareReference>
								<xsl:apply-templates select="FareInfo/RulesData[RulesDataType != 'T']/RulesText" mode="FareRef" />						
							</FareReference>
							<RuleInfo></RuleInfo>
							<FilingAirline></FilingAirline>
							<DepartureAirport>
								<xsl:attribute name="LocationCode"></xsl:attribute>
							</DepartureAirport>
							<ArrivalAirport>
								<xsl:attribute name="LocationCode"></xsl:attribute>
							</ArrivalAirport--></FareRuleInfo>
						<FareRules>
							<xsl:apply-templates select="FareInfo/RulesData[RulesDataType != 'T']/RulesText" mode="Text" />
						</FareRules>
					</FareRuleResponseInfo>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirRulesRS>
	</xsl:template>
	
	<xsl:template match="FareQuoteTariffDisplay_8_0">
		<OTA_AirRulesRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<Errors>
				<Error  Type="Galileo">
					<xsl:value-of select="FareInfo/OutputMsg/Text"/>
				</Error>
			</Errors>
		</OTA_AirRulesRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RulesText" mode="FareRef">
		<xsl:variable name="RulesTextPositionFare">
			<xsl:value-of select="../UniqueKey" />
		</xsl:variable>
		<xsl:if test="contains(., 'DAY/TIME')">
			<xsl:apply-templates select="../../RulesData[UniqueKey = $RulesTextPositionFare]" mode="FareRef" />
			<!--TEST><xsl:value-of select="."/></TEST-->
		</xsl:if>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="RulesData" mode="FareRef">
		<xsl:variable name="AfterDash">
			<xsl:value-of select="substring-after(RulesText,'-')" />
		</xsl:variable>
		<xsl:variable name="AfterSpace">
			<xsl:value-of select="substring-before($AfterDash,' ')" />
		</xsl:variable>
		<xsl:if test="RulesDataType = 'T'">
			<xsl:value-of select="$AfterSpace" />
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="RulesText" mode="Text">
		<xsl:variable name="RulesTextPosition">
			<xsl:value-of select="../UniqueKey" />
		</xsl:variable>
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<SubSection>
			<xsl:attribute name="SubTitle">
				<xsl:value-of select="substring-after(../../RulesData[UniqueKey = $RulesTextPosition][RulesDataType = 'F']/RulesText,' ')"/>
			</xsl:attribute>
			<xsl:attribute name="SubSectionNumber"><xsl:value-of select="$pos"/></xsl:attribute>
			<Paragraph>
				<Text>
					<xsl:apply-templates select="../../RulesData[UniqueKey = $RulesTextPosition]" mode="text" />
				</Text>
			</Paragraph>
		</SubSection>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="RulesData" mode="text">
		<xsl:variable name="pos">
			<xsl:value-of select="position()" />
		</xsl:variable>
		<xsl:if test="$pos  != '1'">
			<xsl:apply-templates select="RulesText" mode="NextLine" />
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="RulesText" mode="NextLine">
		<xsl:value-of select="translate(.,'&#164;',' ')" />
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="InfoMsg" mode="warning">
		<Error Type="Fare">
			<xsl:attribute name="Code">
				<xsl:value-of select="AppNum" />
				<xsl:value-of select="MsgType" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="TransactionErrorCode">
		<Warning Type="General">
			<xsl:attribute name="Code">
				<xsl:value-of select="Domain" />
				<xsl:value-of select="Code" />
			</xsl:attribute>GALILEO - SYSTEM ERROR
		</Warning>
	</xsl:template>
	<!-- ************************************************************** -->
</xsl:stylesheet>
