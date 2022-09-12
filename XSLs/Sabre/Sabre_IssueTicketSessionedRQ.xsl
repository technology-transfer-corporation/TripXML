<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Sabre_IssueTicketSessionedRQ.xsl												-->
  <!-- ================================================================== -->
  <!-- Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0				-->
  <!-- Date: 13 Feb 2015 - Rastko - changed if condition to determine when to send ET	-->
  <!-- Date: 05 Mar 2014 - Rastko - corrected mapping for TicketingPrinter and InvoicePrinter	-->
  <!-- Date: 03 Mar 2014 - Rastko - added mapping for TicketingPrinter and InvoicePrinter		-->
  <!-- Date: 17 Feb 2014 - Rastko - if issue invoice has redisplay do not end transact		-->
  <!-- Date: 02 Feb 2014 - Rastko - issue ticket with cryptic command				-->
  <!-- Date: 31 Jan 2014 - Rastko - added designate invoice printer					-->
  <!-- Date: 29 Jan 2014 - Rastko - made all entries but PNRRead optional			-->
  <!-- Date: 24 Jan 2014 - Rastko - redisplay PNR instead of retrieve with record locator		-->
  <!-- Date: 21 Jan 2014 - Rastko - New file												-->
  <!-- ================================================================== -->
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
      </TravelItineraryReadRQ>
    </PNRRead>
    <PNRRetrieve>
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
    </PNRRetrieve>
    <xsl:if test="Ticketing/@DesignatePrinter='true'">
      <DesignatePrinter>
        <DesignatePrinterRQ Version="1.2.1" xmlns="http://webservices.sabre.com/sabreXML/2003/07">
          <POS>
            <Source>
              <xsl:attribute name="PseudoCityCode">
                <xsl:value-of select="POS/Source/@PseudoCityCode"/>
              </xsl:attribute>
            </Source>
          </POS>
          <Printers>
            <xsl:if test="Ticketing/TicketingPrinter!=''">
              <Ticket>
                <xsl:attribute name="LineAddress">
                  <xsl:value-of select="Ticketing/TicketingPrinter"/>
                </xsl:attribute>
                <xsl:attribute name="CountryCode">
                  <xsl:value-of select="'AT'"/>
                </xsl:attribute>
              </Ticket>
            </xsl:if>
            <xsl:if test="Ticketing/InvoicePrinter!=''">
              <InvoiceItinerary LineAddress="{Ticketing/InvoicePrinter}"/>
            </xsl:if>
          </Printers>
        </DesignatePrinterRQ>
      </DesignatePrinter>
    </xsl:if>
    <xsl:if test="not(Ticketing/@IssueInvoiceOnly='true') and not(Ticketing/@DesignatePrinter='true')">
      <AirTicket>
        <AirTicketRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.9.1">
          <POS>
            <Source>
              <xsl:attribute name="PseudoCityCode">
                <xsl:value-of select="POS/Source/@PseudoCityCode"/>
              </xsl:attribute>
            </Source>
          </POS>
          <NumResponses Count="1"/>
          <xsl:choose>
            <xsl:when test="Ticketing/FutureTicket/Number!=''">
              <FutureTicket>
                <xsl:for-each select="Ticketing/FutureTicket/Number">
                  <Line Number="{.}"/>
                </xsl:for-each>
              </FutureTicket>
            </xsl:when>
            <xsl:otherwise>
              <TicketingInfo TicketType="ETR"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="Ticketing/FareNumber!=''">
            <OptionalQualifiers>
              <PricingQualifiers>
                <BasicPrice PQNumber="{Ticketing/FareNumber}"/>
              </PricingQualifiers>
              <!--MiscQualifiers>
								<VendorPref Code="xx"/>
							</MiscQualifiers-->
            </OptionalQualifiers>
          </xsl:if>
        </AirTicketRQ>
      </AirTicket>
    </xsl:if>
    
    <xsl:if test="Ticketing/IssueItinerary/AccountingLine!=''">
      <Invoice>
        <!--
        <InvoiceItineraryRQ Version="1.1.1" xmlns="http://webservices.sabre.com/sabreXML/2003/07">
					<POS>
						<Source>
							<xsl:attribute name="PseudoCityCode"><xsl:value-of select="POS/Source/@PseudoCityCode"/></xsl:attribute>
						</Source>
					</POS>
					<InvoiceItineraryInfo Print="InvoiceItinerary">
						<MiscOptions>
							<AccountingInfo>
								<xsl:for-each select="Ticketing/IssueItinerary/AccountingLine">
									<Line Number="{.}"/>
								</xsl:for-each>
							</AccountingInfo>
						</MiscOptions>
					</InvoiceItineraryInfo>
				</InvoiceItineraryRQ>
        -->
        <SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
          <xsl:element name="Request">
            <xsl:attribute name="Output">SCREEN</xsl:attribute>
            <xsl:attribute name="MDRSubset">AD01</xsl:attribute>
            <xsl:attribute name="CDATA">true</xsl:attribute>
            <xsl:element name="HostCommand">
              <xsl:variable name="apos">'</xsl:variable>
              <xsl:value-of select="concat('DIN',$apos,'DP',$apos,'A')"/>
              <xsl:for-each select="Ticketing/IssueItinerary/AccountingLine">
                <xsl:if test="position() > 1">
                  <xsl:value-of select="'/'"/>
                </xsl:if>
                <xsl:value-of select="."/>
              </xsl:for-each>
              <xsl:if test="Ticketing/IssueItinerary/@PNRRedisplay='true'">
                <xsl:value-of select="concat($apos,'R')"/>
              </xsl:if>
            </xsl:element>
          </xsl:element>
        </SabreCommandLLSRQ>
      </Invoice>
    </xsl:if>

    <xsl:if test="not(Ticketing/@EndTransact='false') and not(Ticketing/IssueItinerary/@PNRRedisplay='true')">
      <ET>
        <EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.2">
          <EndTransaction Ind="true"/>
          <Source>
            <xsl:attribute name="ReceivedFrom">
              <xsl:choose>
                <xsl:when test="POS/Source/@AgentSine != ''">
                  <xsl:value-of select="POS/Source/@AgentSine"/>
                </xsl:when>
                <xsl:otherwise>TRIPXML</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </Source>
        </EndTransactionRQ>
      </ET>
      <ReET>
        <EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.2">
          <EndTransaction Ind="true"/>
        </EndTransactionRQ>
      </ReET>

    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
