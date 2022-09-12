<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
    AmadeusWS_SalesReportRS_v2.xsl 												
  ==================================================================
  Date: 03 May 2021 - Kobelev - Added Conversation ID to the schema.	
  Date: 24 Jun 2020 - Samokhvalov - new file											
  ================================================================== 
  -->
  
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:key name="controlNumbers" match="documentData" use="reservation/controlNumber" />
  <xsl:template match="/">
    <SalesReportRS Version="1.0">
      <xsl:if test="Errors!=''">
        <Errors>
          <xsl:for-each select="Errors/Error">
            <Error>
              <xsl:value-of select="." />
            </Error>
          </xsl:for-each>
        </Errors>
      </xsl:if>
      <JournalEntries>
        <!--<xsl:apply-templates select="Sales/SalesReports_DisplayQueryReportReply/queryReportDataDetails"/>-->
        <xsl:for-each select="//documentData">
          <xsl:call-template name="documentData"/>
        </xsl:for-each>
      </JournalEntries>
      <xsl:if test="ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="ConversationID"/>
        </ConversationID>
      </xsl:if>
    </SalesReportRS>
  </xsl:template>
  <xsl:template match="queryReportDataDetails">
    <llll>
      <xsl:value-of select="."/>
    </llll>
  </xsl:template>
  <xsl:template name="documentData">
    <JournalEntry>
      <xsl:variable name="seqNum">
        <xsl:value-of select="sequenceIdentification/itemNumberDetails/number"/>
      </xsl:variable>
      <RecordLocator>
        <xsl:value-of select="reservationInformation/reservation/controlNumber"/>
      </RecordLocator>
      <FOP>
        <xsl:value-of select="fopDetails/fopDescription/formOfPayment/type"/>
      </FOP>
      <SequanceNo>
        <xsl:value-of select="$seqNum"/>
      </SequanceNo>
      <Airline>
        <xsl:attribute name="Code">
          <xsl:value-of select="substring(documentNumber/documentDetails/number,1,3)"/>
        </xsl:attribute>
        <xsl:value-of select="validatingCarrierDetails/companyIdentification/marketingCompany"/>
      </Airline>
      <DocumentNumber>
        <xsl:value-of select="substring(documentNumber/documentDetails/number,4)"/>
      </DocumentNumber>
      <DocumentType>
        <xsl:value-of select="transactionDataDetails/transactionDetails/code"/>
      </DocumentType>
      <TicketAmount>
        <xsl:choose>
          <xsl:when test="monetaryInformation/otherMonetaryDetails[typeQualifier='T']/amount != ''"><xsl:value-of select="monetaryInformation/otherMonetaryDetails[typeQualifier='T']/amount"/></xsl:when>  
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>        
      </TicketAmount>
      <BaseFareAmount>
        <xsl:choose>
          <xsl:when test="monetaryInformation/monetaryDetails[typeQualifier='712']/amount != ''"><xsl:value-of select="monetaryInformation/monetaryDetails[typeQualifier='712']/amount"/></xsl:when>  
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>        
      </BaseFareAmount>
      <CommissionAmount>
        <xsl:choose>
          <xsl:when test="monetaryInformation/otherMonetaryDetails[typeQualifier='F']/amount != ''"><xsl:value-of select="monetaryInformation/otherMonetaryDetails[typeQualifier='F']/amount"/></xsl:when>  
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>  
      </CommissionAmount>
      <PassangerName>
        <xsl:value-of select="passengerName/paxDetails/surname"/>
      </PassangerName>
      <TicketingAgent>
        <xsl:value-of select="bookingAgent/originIdentification/originatorId"/>
      </TicketingAgent>
    </JournalEntry>
  </xsl:template>
</xsl:stylesheet>
