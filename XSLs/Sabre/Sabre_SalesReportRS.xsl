<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!--
  ================================================================== 
	Sabre_SalesReportRS.xsl 												
	================================================================== 
	Date: 04 Aug 2020 - Babin - new file												
	================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <!--<xsl:key name="controlNumbers" match="documentData" use="reservation/controlNumber" />-->
  <xsl:template match="/">
    <SalesReportRS Version="2.0.0">
      <JournalEntries>
        <xsl:call-template name="documentData"/>
      </JournalEntries>
    </SalesReportRS>
  </xsl:template>
  <xsl:template match="queryReportDataDetails">
    <llll>
      <xsl:value-of select="."/>
    </llll>
  </xsl:template>
  <xsl:template name="documentData">
    <xsl:for-each select="DailySalesReportRS/SalesReport/IssuanceData">
      <JournalEntry>
        <RecordLocator>
          <xsl:value-of select="@ItineraryRef"/>
        </RecordLocator>
        <FOP>
          <xsl:value-of select="Payment/Form[1]"/>
        </FOP>
        <SequanceNo>
          <xsl:value-of select="generate-id(./TicketingInfo)"/>
        </SequanceNo>
        <Airline>
          <xsl:attribute name="Code">
            <xsl:value-of select="substring(TicketingInfo/Ticketing/@eTicketNumber, 1, 3)"/>
          </xsl:attribute>
        </Airline>
        <DocumentNumber>
          <xsl:value-of select="substring(TicketingInfo/Ticketing/@eTicketNumber, 4)"/>
        </DocumentNumber>
        <DocumentType>
          <xsl:value-of select="@DocumentType"/>
        </DocumentType>
        <TicketAmount>
          <xsl:choose>
            <xsl:when test="Payment/Form/@Amount != ''">
              <xsl:value-of select="Payment/Form/@Amount"/>
            </xsl:when>
            <xsl:otherwise>0</xsl:otherwise>
          </xsl:choose>
        </TicketAmount>
        <BaseFareAmount>0</BaseFareAmount>
        <CommissionAmount>
          <xsl:choose>
            <xsl:when test="@Commission != ''">
              <xsl:value-of select="@Commission"/>
            </xsl:when>
            <xsl:otherwise>0</xsl:otherwise>
          </xsl:choose>
        </CommissionAmount>
        <PassangerName>
          <xsl:value-of select="PersonName[1]"/>
        </PassangerName>
        <TicketingAgent>
          <xsl:value-of select="@AgentSine"/>
        </TicketingAgent>
        <!--<IssueTime>
          <xsl:value-of select="@IssueTime"/>
        </IssueTime>-->
      </JournalEntry>
    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>
