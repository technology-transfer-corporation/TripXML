<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_AirRulesRQ.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 13 Dec 2007 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirRulesRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_AirRulesRQ">
		<OTA_AirRulesRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" Version="2003A.TsabreXML1.3.1">
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
			<RuleReqInfo>
				<FareReference>
					<xsl:attribute name="Code">
						<xsl:value-of select="RuleReqInfo/FareReference" />
					</xsl:attribute>
				</FareReference>
				<FilingAirline>
					<xsl:attribute name="Code">
						<xsl:value-of select="RuleReqInfo/FilingAirline/@Code" />
					</xsl:attribute>
				</FilingAirline>
				<DepartureAirport>
					<xsl:attribute name="CodeContext">IATA</xsl:attribute>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="RuleReqInfo/DepartureAirport/@LocationCode" />
					</xsl:attribute>
				</DepartureAirport>
				<ArrivalAirport>
					<xsl:attribute name="CodeContext">IATA</xsl:attribute>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="RuleReqInfo/ArrivalAirport/@LocationCode" />
					</xsl:attribute>
				</ArrivalAirport>
				<DepartureDate>
					<xsl:attribute name="DateTime">
						<xsl:value-of select="RuleReqInfo/DepartureDate" />
					</xsl:attribute>
				</DepartureDate>
			</RuleReqInfo>
		</OTA_AirRulesRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
