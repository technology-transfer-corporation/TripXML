<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_SalesReportRQ.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 24 Jun 2020 - Samokhvalov - new file											-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="SalesReportRQ" />
  </xsl:template>
  <xsl:template match="SalesReportRQ">
    <SalesReports_DisplayQueryReport>
      <agencyDetails>
        <sourceType>
          <sourceQualifier1>REP</sourceQualifier1>
        </sourceType>
        <originatorDetails>
          <inHouseIdentification1>
            <xsl:value-of select="PCC"/>
          </inHouseIdentification1>
        </originatorDetails>
      </agencyDetails>
      <xsl:choose>
        <xsl:when test="ReportDate!=''">
          <salesPeriodDetails>
            <beginDateTime>
              <year>
                <xsl:value-of select="substring(ReportDate,1,4)"/>
              </year>
              <month>
                <xsl:value-of select="substring(ReportDate,6,2)"/>
              </month>
              <day>
                <xsl:value-of select="substring(ReportDate,9,2)"/>
              </day>
            </beginDateTime>
            <endDateTime>
              <year>
                <xsl:value-of select="substring(ReportDate,1,4)"/>
              </year>
              <month>
                <xsl:value-of select="substring(ReportDate,6,2)"/>
              </month>
              <day>
                <xsl:value-of select="substring(ReportDate,9,2)"/>
              </day>
            </endDateTime>
          </salesPeriodDetails>
        </xsl:when>
        <xsl:when test="ReportDateRange/@Start!='' and ReportDateRange/@End!=''">
          <salesPeriodDetails>
            <beginDateTime>
              <year>
                <xsl:value-of select="substring(ReportDateRange/@Start,1,4)"/>
              </year>
              <month>
                <xsl:value-of select="substring(ReportDateRange/@Start,6,2)"/>
              </month>
              <day>
                <xsl:value-of select="substring(ReportDateRange/@Start,9,2)"/>
              </day>
            </beginDateTime>
            <endDateTime>
              <year>
                <xsl:value-of select="substring(ReportDateRange/@End,1,4)"/>
              </year>
              <month>
                <xsl:value-of select="substring(ReportDateRange/@End,6,2)"/>
              </month>
              <day>
                <xsl:value-of select="substring(ReportDateRange/@End,9,2)"/>
              </day>
            </endDateTime>
          </salesPeriodDetails>
        </xsl:when>
      </xsl:choose>
      <xsl:choose>
        <xsl:when test="ReportType!=''">
          <transactionData>
            <transactionDetails>
              <type>
                <xsl:value-of select="ReportType"/>
              </type>
            </transactionDetails>
          </transactionData>
        </xsl:when>
      </xsl:choose>
      <requestOption>
        <selectionDetails>
          <option>SOF</option>
        </selectionDetails>
      </requestOption>
    </SalesReports_DisplayQueryReport>
  </xsl:template>
</xsl:stylesheet>
