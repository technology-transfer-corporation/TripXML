<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
  Sabre_IssueTicketRQ.xsl															
  ================================================================== 
  Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0			
  Date: 09 May 2012- Kasun- include End Transact logic after issuing an ETicket		
  Date: 10 Dec 2010 - Rastko														
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <TT_IssueTicketRQ>
      <xsl:apply-templates select="TT_IssueTicketRQ"/>
    </TT_IssueTicketRQ>
  </xsl:template>
  <xsl:template match="TT_IssueTicketRQ">
    <PNRRead>
      <TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
        <MessagingDetails>
          <SubjectAreas>
            <SubjectArea>FULL</SubjectArea>
          </SubjectAreas>
        </MessagingDetails>
        <UniqueID>
          <xsl:attribute name="ID">
            <xsl:value-of select="UniqueID/@ID" />
          </xsl:attribute>
        </UniqueID>
      </TravelItineraryReadRQ>
    </PNRRead>
    <TicketCryptic>
      <AirTicketRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.9.1">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="POS/Source/@PseudoCityCode"/>
            </xsl:attribute>
          </Source>
        </POS>
        <NumResponses Count="1"/>
        <TicketingInfo TicketType="ETR"/>
        <OptionalQualifiers>
          <MiscQualifiers>
            <VendorPref Code="xx"/>
          </MiscQualifiers>
        </OptionalQualifiers>
      </AirTicketRQ>
    </TicketCryptic>
    <ET>
      <EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.01">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="POS/Source/@PseudoCityCode"/>
            </xsl:attribute>
          </Source>
        </POS>
        <UpdatedBy>
          <TPA_Extensions>
            <Access>
              <AccessPerson>
                <GivenName>
                  <xsl:choose>
                    <xsl:when test="POS/Source/@AgentSine != ''">
                      <xsl:value-of select="POS/Source/@AgentSine"/>
                    </xsl:when>
                    <xsl:otherwise>TRIPXML</xsl:otherwise>
                  </xsl:choose>
                </GivenName>.
              </AccessPerson>
            </Access>
          </TPA_Extensions>
        </UpdatedBy>
        <EndTransaction Ind="true"/>
      </EndTransactionRQ>
    </ET>
  </xsl:template>
</xsl:stylesheet>
