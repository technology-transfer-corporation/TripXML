<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
   AmadeusWS_ReIssueTicketRQ.xsl										
  ================================================================== 
   Date: 4 May 2020 - Kobelev - Initial load	
  ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="pcc" select="TT_ReIssueTicketRQ/POS/Source/@PseudoCityCode"/>
  <xsl:variable name="userid" select="TT_ReIssueTicketRQ/POS/TPA_Extensions/Provider/Userid"/>

  <xsl:template match="/">
    <TT_ReIssueTicketRQ>
      <xsl:apply-templates select="TT_ReIssueTicketRQ" />
    </TT_ReIssueTicketRQ>
  </xsl:template>

  <xsl:template match="TT_ReIssueTicketRQ">
    <PNRRead>
      <PNR_RetrieveByRecLoc>
        <sbrRecLoc>
          <reservation>
            <controlNumber>
              <xsl:value-of select="UniqueID/@ID" />
            </controlNumber>
          </reservation>
        </sbrRecLoc>
      </PNR_RetrieveByRecLoc>
    </PNRRead>

    <!-- Insert Ticket)AutomaticUpdate -->
    <Ticket_AutomaticUpdate>
      <xsl:apply-templates select="Reissue/Ticket" mode="AutoUpdate" />
    </Ticket_AutomaticUpdate>
    
    <PNR_AddMultiElements>
      <pnrActions>
        <optionCode>11</optionCode>
      </pnrActions>
      <dataElementsMaster>
        <marker1></marker1>
        <dataElementsIndiv>
          <elementManagementData>
            <segmentName>RF</segmentName>
          </elementManagementData>
          <freetextData>
            <freetextDetail>
              <subjectQualifier>3</subjectQualifier>
              <type>P22</type>
            </freetextDetail>
            <longFreetext>TRIPXML</longFreetext>
          </freetextData>
        </dataElementsIndiv>
      </dataElementsMaster>
    </PNR_AddMultiElements>

    <!-- DocIsuance -->
    <xsl:apply-templates select="Reissue/Ticket" mode="IssueTicket" />

  </xsl:template>

  <xsl:template match="Ticket" mode="IssueTicket">
    <Ticket>
      <DocIssuance_IssueTicket>
        <!--
        <xsl:choose>
          <xsl:when test="@InfantOnly = 'true' or @OmitInfant = 'true'"></xsl:when>
          <xsl:otherwise>
        -->
            <paxSelection>
              <passengerReference>
                <type>PT</type>
                <value>
                  <xsl:value-of select="PassangerInfo/@RPH"/>
                </value>
              </passengerReference>
            </paxSelection>
          <!--
          </xsl:otherwise>
        </xsl:choose>
          -->
        <xsl:if test="@TicketType = 'eTicket' or @Exchange='true' or Reissue/@IssueInvoice = 'true'">
          <xsl:if test="@TicketType = 'eTicket'">
            <optionGroup>
              <switches>
                <statusDetails>
                  <indicator>ET</indicator>
                </statusDetails>
              </switches>
            </optionGroup>
          </xsl:if>
          <xsl:if test="@Exchange='true'">
            <optionGroup>
              <switches>
                <statusDetails>
                  <indicator>EXC</indicator>
                </statusDetails>
              </switches>
            </optionGroup>
          </xsl:if>
          <xsl:if test="@Retrive='true'">
            <optionGroup>
              <switches>
                <statusDetails>
                  <indicator>RT</indicator>
                </statusDetails>
              </switches>
            </optionGroup>
          </xsl:if>
        </xsl:if>
        <xsl:choose>
          <xsl:when test="@OmitInfant = 'true'">
            <infantOrAdultAssociation>
              <paxDetails>
                <type>A</type>
              </paxDetails>
            </infantOrAdultAssociation>
          </xsl:when>
          <xsl:when test="@InfantOnly = 'true'">
            <infantOrAdultAssociation>
              <paxDetails>
                <type>IN</type>
              </paxDetails>
            </infantOrAdultAssociation>
          </xsl:when>
        </xsl:choose>
      </DocIssuance_IssueTicket>
    </Ticket>
  </xsl:template>

  <xsl:template match="Ticket" mode="AutoUpdate">
    <exchangeInformationGroup>
      <transactionIdentifier>
        <itemNumberDetails>
          <number>
            <xsl:value-of select="@RPH"/>
          </number>
        </itemNumberDetails>
      </transactionIdentifier>
      <documentInfoGroup>
        <documentInfo>
          <documentDetails>
            <number>
              <xsl:value-of select="@Number"/>
            </number>
            <xsl:choose>
              <xsl:when test="@TicketType='EMD'">
                <type>Y</type>
              </xsl:when>
              <xsl:otherwise>
                <type>T</type>
              </xsl:otherwise>
            </xsl:choose>
          </documentDetails>
        </documentInfo>
      </documentInfoGroup>
      <passengerSegmentSelection>
        <referenceDetails>
          <type>
            <xsl:choose>
              <xsl:when test="PassangerInfo/@Code='INF'">PI</xsl:when>
              <xsl:otherwise>PA</xsl:otherwise>
            </xsl:choose>
          </type>
          <value>
            <xsl:value-of select="PassangerInfo/@RPH"/>
          </value>
        </referenceDetails>
      </passengerSegmentSelection>
    </exchangeInformationGroup>


  </xsl:template>
</xsl:stylesheet>
