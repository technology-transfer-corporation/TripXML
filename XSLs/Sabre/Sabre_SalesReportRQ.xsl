<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!--
  ================================================================== 
	Sales_SalesReportRQ.xsl 												
	================================================================== 
	Date: 03 Aug 2020 - Kobelev - new file												
	================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="SalesReportRQ" />
  </xsl:template>
  <xsl:template match="SalesReportRQ">
    <DailySalesReportRQ Version="2.0.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
      <SalesReport>
        <xsl:attribute name="PseudoCityCode">
          <xsl:value-of select="PCC" />
        </xsl:attribute>

        <xsl:if test="ReportDate">
          <xsl:attribute name="StartDate">
            <xsl:value-of select="ReportDate" />
          </xsl:attribute>
        </xsl:if>

        <xsl:if test="ReportDateRange">
          <xsl:attribute name="StartDate">
            <xsl:value-of select="ReportDateRange/@Start" />
          </xsl:attribute>
        </xsl:if>

        <xsl:if test="ReportType">
          <xsl:attribute name="DirectTicketing">
            <xsl:text>false</xsl:text>
          </xsl:attribute>
        </xsl:if>

      </SalesReport>
    </DailySalesReportRQ>
  </xsl:template>

</xsl:stylesheet>
