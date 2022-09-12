<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
	 Sabre_CrypticRQ.xsl 																
	 ================================================================== 
	 Date: 08 Jun 2006 - Rastko														
	 Date: 15 Jun 2010 - Rastko - moved message to Sabre version 1.6.1			
	 ================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes" cdata-section-elements="text" />
	<xsl:template match="/">
		<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.0" ReturnHostCommand="true" >
			<xsl:apply-templates select="CrypticRQ" />
		</SabreCommandLLSRQ>
	</xsl:template>
	<xsl:template match="CrypticRQ">
		<xsl:element name="Request">
			<xsl:attribute name="Output">SCREEN</xsl:attribute>
			<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
			<xsl:attribute name="CDATA">true</xsl:attribute>
			<xsl:element name="HostCommand">
        <xsl:value-of select="Entry" />
			</xsl:element>
		</xsl:element>
	</xsl:template>
</xsl:stylesheet>
