<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
   AmadeusWS_ListVirtualCardsRQ.xsl															
  ================================================================== 
   Date: 03 Jan 2019 - Alex Kobelev      														
  ================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="PAY_ListVirtualCardsRQ"/>
	</xsl:template>
	<xsl:template match="PAY_ListVirtualCardsRQ">
		<AMA_PAY_ListVirtualCardsRQ Version="2.0">
     <xsl:if test="SubType">
		    <xsl:copy-of select="SubType"/>
		  </xsl:if>
		  <xsl:if test="CurrencyCode">
		    <xsl:copy-of select="CurrencyCode"/>
		  </xsl:if>
      <xsl:if test="Period">
        <xsl:copy-of select="Period"/>
      </xsl:if>
		</AMA_PAY_ListVirtualCardsRQ>
	</xsl:template>
</xsl:stylesheet>
