<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_IssueTicketRQ.xsl												 	-->
  <!-- ================================================================== -->
  <!-- Date: 14 Dec 2015 - Rastko - corrected specific entry for Discovery	-->
  <!-- Date: 24 Nov 2015 - Rastko - added support for IssueJointInvoice parameter	-->
  <!-- Date: 24 Nov 2015 - Rastko - added specific entry for Discovery	-->
  <!-- Date: 01 Oct 2014 - Rastko - commented out passenger association in DocIssuance_IssueTicket	-->
  <!-- Date: 9 Sep 2014 - Rastko - coded DocIssuance_IssueTicket for OneTwoTrip 				-->
  <!-- Date:19 Oct 2013 - Rastko - added attribute SpecialInstruction				-->
  <!-- Date:12 Mar 2013 - Rastko - have a default value for satellite print for DTT			-->
  <!-- Date:19 Feb 2013 - Rastko - added support for RemoteLocation parameter			-->
  <!-- Date:19 Feb 2013 - Rastko - changed option STFLL to STFL for DTT			-->
  <!-- Date:07 Feb 2013 - Rastko - added test of EXCH option for all cases			-->
  <!-- Date:05 Dec 2012 - Rastko - added test of INV option for all cases				-->
  <!-- Date:14 Sep 2012 - Rastko - corrected J to invoice option if no infant in PNR for DTT		-->
  <!-- Date:14 Sep 2012 - Rastko - add J to invoice option if no infant in PNR for DTT		-->
  <!-- Date:26 Jan 2012 - Rastko - added option to issue INF only 					-->
  <!-- Date:15 Jan 2012 - Rastko - added option to issue PAX only in DocIssuance_IssueTicket	-->
  <!-- Date:12 Jan 2012 - Rastko - added mapping of DocIssuance_IssueTicket			-->
  <!-- Date:31 Aug 2011 - Rastko - added TktPayment option						-->
  <!-- Date:09 Aug 2011 - Rastko - mapped new OmitInfant attribute					-->
  <!-- Date:08 Aug 2011 - Rastko - added PAX entry specific to Banamex	 and Avianca		-->
  <!-- Date:18 Mar 2011 - Rastko - added ITR entry specific to Banamex				-->
  <!-- Date:27 Feb 2011 - Rastko - added FareNumber to the mapping				-->
  <!-- Date:26 Jul 2010 - Rastko - added /VC to every entry to be used in business logic		-->
  <!-- Date:20 Dec 2009 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="pcc" select="TT_IssueTicketRQ/POS/Source/@PseudoCityCode"/>
  <xsl:variable name="userid" select="TT_IssueTicketRQ/POS/Source/RequestorID/@ID"/>
  <xsl:template match="/">
    <TT_IssueTicketRQ>
      <xsl:apply-templates select="TT_IssueTicketRQ"/>
    </TT_IssueTicketRQ>
  </xsl:template>
  <xsl:template match="TT_IssueTicketRQ">
    <PNRRead>
      <PNR_RetrieveByRecLoc>
        <sbrRecLoc>
          <reservation>
            <controlNumber>
              <xsl:value-of select="UniqueID/@ID"/>
            </controlNumber>
          </reservation>
        </sbrRecLoc>
      </PNR_RetrieveByRecLoc>
    </PNRRead>
    <xsl:if test="Fulfillment/PaymentDetails/PaymentDetail/MiscChargeOrder/@TicketNumber!=''">
      <TktPayment>
        <Command_Cryptic>
          <messageAction>
            <messageFunctionDetails>
              <messageFunction>M</messageFunction>
            </messageFunctionDetails>
          </messageAction>
          <longTextString>
            <textStringDetails>
              <xsl:text>FPMSCC</xsl:text>
              <xsl:value-of select="Fulfillment/PaymentDetails/PaymentDetail/MiscChargeOrder/@TicketNumber"/>
              <xsl:text>;RFTRIPXML;ER</xsl:text>
            </textStringDetails>
          </longTextString>
        </Command_Cryptic>
      </TktPayment>
    </xsl:if>
    <xsl:choose>
      <xsl:when test="$userid='Discovery'">
        <TicketCryptic>
          <Command_Cryptic>
            <messageAction>
              <messageFunctionDetails>
                <messageFunction>M</messageFunction>
              </messageFunctionDetails>
            </messageAction>
            <longTextString>
              <textStringDetails>
                <xsl:value-of select="concat('TTP',Ticketing/TicketingPrinter)"/>
                <xsl:text>/IBP-EMLA</xsl:text>
              </textStringDetails>
            </longTextString>
          </Command_Cryptic>
        </TicketCryptic>
      </xsl:when>
      <xsl:when test="(Ticketing/@TicketType = 'eTicket' or Ticketing/@IssueMCO = 'true' or Ticketing/@IssueInvoice = 'true' or Ticketing/Notification/@ByEmail = 'true' or Ticketing/Notification/@ByFax = 'true' or Ticketing/@RemoteLocation != '' or Ticketing/@Exchange='true') and $pcc!='BOGAV08AZ' and $pcc!='MIA1S21AV' and ($userid!='OneTwoTrip' or $pcc='NYC1S21GQ') and $userid!='DTT'">
        <TicketCryptic>
          <xsl:choose>
            <xsl:when test="Ticketing/Notification/@ByEmail = 'true'">
              <Command_Cryptic>
                <messageAction>
                  <messageFunctionDetails>
                    <messageFunction>M</messageFunction>
                  </messageFunctionDetails>
                </messageAction>
                <longTextString>
                  <textStringDetails>
                    <xsl:text>TTP</xsl:text>
                    <xsl:if test="Ticketing/@Exchange='true'">/EXCH</xsl:if>
                    <xsl:if test="Ticketing/@OmitInfant = 'true'">/PAX</xsl:if>
                    <xsl:if test="Ticketing/@InfantOnly = 'true'">/INF</xsl:if>
                    <xsl:if test="Ticketing/@IssueInvoice = 'true'">/INV</xsl:if>
                    <xsl:choose>
                      <xsl:when test="Ticketing/@RemoteLocation != ''">
                        <xsl:value-of select="concat('/ST',Ticketing/@RemoteLocation)"/>
                      </xsl:when>
                      <xsl:when test="$pcc='NYC1S21GQ'">/STFL</xsl:when>
                    </xsl:choose>
                    <xsl:text>/ET/ITR-EMLA/VC</xsl:text>
                  </textStringDetails>
                </longTextString>
              </Command_Cryptic>
            </xsl:when>
            <xsl:when test="Ticketing/Notification/@ByFax = 'true'">
              <Command_Cryptic>
                <messageAction>
                  <messageFunctionDetails>
                    <messageFunction>M</messageFunction>
                  </messageFunctionDetails>
                </messageAction>
                <longTextString>
                  <textStringDetails>
                    <xsl:text>TTP</xsl:text>
                    <xsl:if test="Ticketing/@Exchange='true'">/EXCH</xsl:if>
                    <xsl:if test="Ticketing/@OmitInfant = 'true'">/PAX</xsl:if>
                    <xsl:if test="Ticketing/@InfantOnly = 'true'">/INF</xsl:if>
                    <xsl:if test="Ticketing/@IssueInvoice = 'true'">/INV</xsl:if>
                    <xsl:choose>
                      <xsl:when test="Ticketing/@RemoteLocation != ''">
                        <xsl:value-of select="concat('/ST',Ticketing/@RemoteLocation)"/>
                      </xsl:when>
                      <xsl:when test="$pcc='NYC1S21GQ'">/STFL</xsl:when>
                    </xsl:choose>
                    <xsl:text>/ET/ITR-FAXA/VC</xsl:text>
                  </textStringDetails>
                </longTextString>
              </Command_Cryptic>
            </xsl:when>
            <xsl:otherwise>
              <Command_Cryptic>
                <messageAction>
                  <messageFunctionDetails>
                    <messageFunction>M</messageFunction>
                  </messageFunctionDetails>
                </messageAction>
                <longTextString>
                  <textStringDetails>
                    <xsl:text>TTP</xsl:text>
                    <xsl:if test="Ticketing/@Exchange='true'">/EXCH</xsl:if>
                    <xsl:if test="Ticketing/@OmitInfant = 'true' and $pcc!='NYC1S21GQ'">/PAX</xsl:if>
                    <xsl:if test="Ticketing/@InfantOnly = 'true' and $pcc!='NYC1S21GQ'">/INF</xsl:if>
                    <xsl:if test="Ticketing/@IssueInvoice = 'true'">/INV</xsl:if>
                    <xsl:if test="$pcc='NYC1S21GQ' and (not(Ticketing/@InfantOnly) or Ticketing/@InfantOnly='false')">
                      <xsl:value-of select="'J'"/>
                    </xsl:if>
                    <xsl:if test="Ticketing/@TicketType = 'eTicket'">/ET</xsl:if>
                    <xsl:if test="Ticketing/@IssueMCO = 'true'">/TTM</xsl:if>
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
                  </textStringDetails>
                </longTextString>
              </Command_Cryptic>
            </xsl:otherwise>
          </xsl:choose>
        </TicketCryptic>
        <Ticketing>
          <xsl:if test="Ticketing/FareNumber!=''">
            <FareNumber>
              <xsl:value-of select="Ticketing/FareNumber"/>
            </FareNumber>
          </xsl:if>
        </Ticketing>
      </xsl:when>
      <xsl:otherwise>
        <Ticket>
          <DocIssuance_IssueTicket>
            <xsl:if test="Ticketing/@Exchange='true'">
              <optionGroup>
                <switches>
                  <statusDetails>
                    <indicator>EXC</indicator>
                  </statusDetails>
                </switches>
              </optionGroup>
            </xsl:if>
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
            <xsl:if test="$pcc='MEXMX219Q'">
              <optionGroup>
                <switches>
                  <statusDetails>
                    <indicator>ITR</indicator>
                  </statusDetails>
                </switches>
              </optionGroup>
            </xsl:if>
            <xsl:choose>
              <xsl:when test="Ticketing/@OmitInfant = 'true' and $pcc!='NYC1S21GQ'">
                <infantOrAdultAssociation>
                  <paxDetails>
                    <type>A</type>
                  </paxDetails>
                </infantOrAdultAssociation>
              </xsl:when>
              <xsl:when test="Ticketing/@InfantOnly = 'true' and $pcc!='NYC1S21GQ'">
                <infantOrAdultAssociation>
                  <paxDetails>
                    <type>IN</type>
                  </paxDetails>
                </infantOrAdultAssociation>
              </xsl:when>
              <!--xsl:otherwise>
								<paxSelection>
									<passengerReference>
										<type>ADT</type>
										<value>T</value>
									</passengerReference>
								</paxSelection>
							</xsl:otherwise-->
            </xsl:choose>
            <xsl:if test="Ticketing/@RemoteLocation != '' or $pcc='NYC1S21GQ' or $pcc='NYC1S211F'">
              <otherCompoundOptions>
                <attributeDetails>
                  <attributeType>ST</attributeType>
                  <attributeDescription>
                    <xsl:choose>
                      <xsl:when test="$pcc='NYC1S21GQ'">FL</xsl:when>
                      <xsl:when test="$pcc='NYC1S211F'">FL</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="Ticketing/@SpecialInstruction"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </attributeDescription>
                </attributeDetails>
              </otherCompoundOptions>
            </xsl:if>
          </DocIssuance_IssueTicket>
        </Ticket>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
