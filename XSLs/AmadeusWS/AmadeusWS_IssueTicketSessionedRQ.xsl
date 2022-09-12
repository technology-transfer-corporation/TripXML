<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_IssueTicketSessionedRQ.xsl										-->
  <!-- ================================================================== -->
  <!-- Date: 22 Mar 2016 - Rastko - added PNR retrieve oprtion after ticket is issued	-->
  <!-- Date: 21 Mar 2016 - Kobelev - use DocIssuance_IssueTicket for all users		-->
  <!-- Date: 04 Feb 2016 - Rastko - use DocIssuance_IssueTicket for Downtown user		-->
  <!-- Date: 30 Nov 2015 - Rastko - added support for INVJ							-->
  <!-- Date: 16 May 2014 - Rastko - force retrieve after cryptic TTP					-->
  <!-- Date: 12 May 2014 - Rastko - in cryptic mode always send ET as first parameter		-->
  <!-- Date: 07 May 2014 - Rastko - corrected order of paxSelection element in request		-->
  <!-- Date: 05 May 2014 - Rastko - added support for DocIssuance_IssueTicket request		-->
  <!-- Date: 14 Jan 2014 - Rastko - added same options as IssueTicket message			-->
  <!-- Date:27 Jun 2012 - Shashin - added INV option to ticketing					-->
  <!-- Date:09 Aug 2011 - Rastko - mapped new OmitInfant attribute					-->
  <!-- Date:27 Apr 2011 - Rastko - added PNR redisplay								-->
  <!-- Date:11 Apr 2011 - Rastko - added PNRead part								-->
  <!-- Date:20 Mar 2011 - Rastko - simplified to fit t-robot requirement				-->
  <!-- Date:27 Feb 2011 - Rastko - added FareNumber to the mapping				-->
  <!-- Date:26 Jul 2010 - Rastko - added /VC to every entry to be used in business logic		-->
  <!-- Date:20 Dec 2009 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="pcc" select="TT_IssueTicketRQ/POS/Source/@PseudoCityCode"/>
  <xsl:variable name="userid" select="TT_IssueTicketRQ/POS/TPA_Extensions/Provider/Userid"/>
  <xsl:template match="/">
    <TT_IssueTicketRQ>
      <xsl:apply-templates select="TT_IssueTicketRQ" />
    </TT_IssueTicketRQ>
  </xsl:template>

  <xsl:template match="TT_IssueTicketRQ">
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
    <xsl:choose>
      <!-- <xsl:when test="$userid='Thomalex' or $userid='Autoticket' or $userid='Downtown'"> -->
      <xsl:when test="$userid != 'Cryptic'">
        <Ticket>
          <DocIssuance_IssueTicket>
            <xsl:if test="Ticketing/FareNumber">
              <selection>
                <xsl:for-each select="Ticketing/FareNumber">
                  <referenceDetails>
                    <type>TS</type>
                    <value>
                      <xsl:value-of select="."/>
                    </value>
                  </referenceDetails>
                </xsl:for-each>
              </selection>
            </xsl:if>
            <xsl:choose>
              <xsl:when test="Ticketing/@InfantOnly = 'true' or Ticketing/@OmitInfant = 'true'"></xsl:when>
              <xsl:otherwise>
                <paxSelection>
                  <passengerReference>
                    <type>ADT</type>
                    <value>T</value>
                  </passengerReference>
                </paxSelection>
              </xsl:otherwise>
            </xsl:choose>
            <xsl:if test="Ticketing/@TicketType = 'eTicket' or Ticketing/@Exchange='true' or Ticketing/Notification/@ByEmail = 'true' or Ticketing/@IssueInvoice = 'true'">
              <xsl:if test="Ticketing/@TicketType = 'eTicket'">
                <optionGroup>
                  <switches>
                    <statusDetails>
                      <indicator>ET</indicator>
                    </statusDetails>
                  </switches>
                </optionGroup>
              </xsl:if>
              <xsl:choose>
                <xsl:when test="Ticketing/@IssueJointInvoice = 'true'">
                  <optionGroup>
                    <switches>
                      <statusDetails>
                        <indicator>IVJ</indicator>
                      </statusDetails>
                    </switches>
                  </optionGroup>
                </xsl:when>
                <xsl:when test="Ticketing/@IssueInvoice = 'true'">
                  <optionGroup>
                    <switches>
                      <statusDetails>
                        <indicator>INV</indicator>
                      </statusDetails>
                    </switches>
                  </optionGroup>
                </xsl:when>
              </xsl:choose>
              <xsl:if test="Ticketing/@Exchange='true'">
                <optionGroup>
                  <switches>
                    <statusDetails>
                      <indicator>EXC</indicator>
                    </statusDetails>
                  </switches>
                </optionGroup>
              </xsl:if>
              <xsl:if test="Ticketing/Notification/@ByEmail = 'true'">
                <optionGroup>
                  <switches>
                    <statusDetails>
                      <indicator>ITR</indicator>
                    </statusDetails>
                  </switches>
                </optionGroup>
              </xsl:if>
              <optionGroup>
                <switches>
                  <statusDetails>
                    <indicator>RT</indicator>
                  </statusDetails>
                </switches>
              </optionGroup>
            </xsl:if>
            <xsl:choose>
              <xsl:when test="Ticketing/@OmitInfant = 'true'">
                <infantOrAdultAssociation>
                  <paxDetails>
                    <type>A</type>
                  </paxDetails>
                </infantOrAdultAssociation>
              </xsl:when>
              <xsl:when test="Ticketing/@InfantOnly = 'true'">
                <infantOrAdultAssociation>
                  <paxDetails>
                    <type>IN</type>
                  </paxDetails>
                </infantOrAdultAssociation>
              </xsl:when>
            </xsl:choose>
            <xsl:if test="Ticketing/Notification/@ByEmail = 'true' or Ticketing/@RemoteLocation != ''">
              <xsl:if test="Ticketing/Notification/@ByEmail = 'true'">
                <otherCompoundOptions>
                  <attributeDetails>
                    <attributeType>EMPRA</attributeType>
                  </attributeDetails>
                </otherCompoundOptions>
              </xsl:if>
              <xsl:if test="Ticketing/@RemoteLocation != ''">
                <otherCompoundOptions>
                  <attributeDetails>
                    <attributeType>ST</attributeType>
                    <attributeDescription>
                      <xsl:value-of select="Ticketing/@RemoteLocation"/>
                    </attributeDescription>
                  </attributeDetails>
                </otherCompoundOptions>
              </xsl:if>
            </xsl:if>
          </DocIssuance_IssueTicket>
        </Ticket>
      </xsl:when>
      <xsl:otherwise>
        <TicketCryptic>
          <Command_Cryptic>
            <messageAction>
              <messageFunctionDetails>
                <messageFunction>M</messageFunction>
              </messageFunctionDetails>
            </messageAction>
            <longTextString>
              <textStringDetails>
                <xsl:text>TTP</xsl:text>
                <xsl:if test="Ticketing/@TicketType = 'eTicket'">/ET</xsl:if>
                <xsl:if test="Ticketing/@Exchange='true'">/EXCH</xsl:if>
                <xsl:if test="Ticketing/@OmitInfant = 'true' and $pcc!='NYC1S21GQ'">/PAX</xsl:if>
                <xsl:if test="Ticketing/@InfantOnly = 'true' and $pcc!='NYC1S21GQ'">/INF</xsl:if>
                <xsl:if test="Ticketing/@IssueInvoice = 'true'">/INV</xsl:if>
                <xsl:if test="$pcc='NYC1S21GQ' and (not(Ticketing/@InfantOnly) or Ticketing/@InfantOnly='false')">
                  <xsl:value-of select="'J'"/>
                </xsl:if>
                <xsl:for-each select="Ticketing/FareNumber">
                  <xsl:if test="position()=1">/T</xsl:if>
                  <xsl:value-of select="."/>
                  <xsl:if test="position()!=last()">,</xsl:if>
                </xsl:for-each>
                <xsl:if test="Ticketing/@IssueMCO = 'true'">/TTM</xsl:if>
                <xsl:if test="Ticketing/Notification/@ByEmail = 'true'">/ITR-EMLA</xsl:if>
                <xsl:if test="$pcc='MEXMX219Q'">/ITR</xsl:if>
                <xsl:if test="Ticketing/@SpecialInstruction!=''">
                  <xsl:value-of select="concat('/',Ticketing/@SpecialInstruction)"/>
                </xsl:if>
                <xsl:choose>
                  <xsl:when test="Ticketing/@RemoteLocation != ''">
                    <xsl:value-of select="concat('/ST',Ticketing/@RemoteLocation)"/>
                  </xsl:when>
                  <xsl:when test="$pcc='NYC1S21GQ'">/STFL</xsl:when>
                </xsl:choose>
                <xsl:text>/VC</xsl:text>
                <xsl:text>/RT</xsl:text>
              </textStringDetails>
            </longTextString>
          </Command_Cryptic>
        </TicketCryptic>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
