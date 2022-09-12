<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ==================================================================
  v03_Galileo_IssueTicketRS.xsl 													
  ================================================================== 
  Date: 03 Nov 2020 - Kobelev - Printer Check and Assignment.
  Date: 28 Jul 2012 - Rastko - added error text when none returned by Galileo			
  Date: 15 Sep 2009 - Rastko														
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />

  <xsl:template match="/">
    <TT_IssueTicketRS>
      <xsl:attribute name="Version">3.000</xsl:attribute>
      <xsl:apply-templates select="DocProdFareManipulation_29" />
      <xsl:apply-templates select="PNRBFManagement_53" />
      <xsl:apply-templates select="Screen"/>
      <xsl:apply-templates select="TicketPrinterLinkage_1_0" />
      <xsl:if test="*/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="*/ConversationID"/>
        </ConversationID>
      </xsl:if>
    </TT_IssueTicketRS>
  </xsl:template>

  <xsl:template match="DocProdFareManipulation_29">
    <xsl:choose>
      <xsl:when test="TicketNumberData/ETicketNum/FirstTkNum!=''">
        <xsl:apply-templates select="TicketNumberData" mode="Success" />
      </xsl:when>
      <xsl:when test="ManualFareUpdate/ErrText/Text != ''">
        <xsl:apply-templates select="ManualFareUpdate" mode="Unsuccessful" />
      </xsl:when>
      <xsl:when test="HostApplicationError">
        <xsl:apply-templates select="HostApplicationError"/>
      </xsl:when>
      <xsl:when test="TransactionErrorCode">
        <xsl:apply-templates select="TransactionErrorCode"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="Ticketing" mode="Unsuccessful" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="TicketNumberData" mode="Success">
    <Success />
    <xsl:if test="TextMsg/Txt != ''">
      <Warnings>
        <xsl:for-each select="TextMsg">
          <Warning>
            <xsl:value-of select="Txt"/>
          </Warning>
        </xsl:for-each>
      </Warnings>
    </xsl:if>
    <UniqueID>
      <xsl:attribute name="ID">
        <xsl:value-of select="../PNRBFManagement_53/PNRBFRetrieve/GenPNRInfo/RecLoc"/>
      </xsl:attribute>
    </UniqueID>
    <TicketingControl>
      <Type>OK</Type>
    </TicketingControl>
      <!-- ETicketNum[CurHistoryInd='C'] |  -->
        <xsl:for-each select="ETicketNum[AirV != '']">
          <Ticket>
            <xsl:attribute name="Type">
              <xsl:choose>
                <xsl:when test="TkType='E'">Electronic</xsl:when>
                <xsl:when test="TkType='P'">Paper</xsl:when>
                <xsl:when test="TkType='M'">MCO</xsl:when>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="Number">
              <xsl:value-of select="concat(AirV,FirstTkNum)"/>
            </xsl:attribute>
            <xsl:if test="../ETicketNum[CurHistoryInd='C'][Name=Name]/ItinInvNum != ''">
                <xsl:attribute name="InvoiceNumber">
                  <xsl:value-of select="../ETicketNum[CurHistoryInd='C'][Name=Name]/ItinInvNum"/>
                </xsl:attribute>
              </xsl:if>
            <xsl:if test="Name!=''">
              <Name>
                <xsl:value-of select="Name"/>
              </Name>
            </xsl:if>
            <xsl:if test="Fare!=''">
              <Fare>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="normalize-space(Fare)"/>
                </xsl:attribute>
                <xsl:attribute name="Currency">
                  <xsl:value-of select="Currency"/>
                </xsl:attribute>
              </Fare>
            </xsl:if>
          </Ticket>
        </xsl:for-each>
      



  </xsl:template>

  <xsl:template match="PNRBFManagement_53">
    <xsl:if test="PNRBFManagement_53/TransactionErrorCode | PNRBFRetrieve/ErrText | OTA_AirPriceRS/PNRBFManagement_53/EndTransaction/ErrorCode | OTA_AirPriceRS/PNRBFManagement_53/TransactionErrorCode">
      <Errors>
        <xsl:apply-templates select="PNRBFRetrieve" mode="Unsuccessful"/>
        <xsl:apply-templates select="OTA_AirPriceRS" mode="Unsuccessful"/>
      </Errors>
    </xsl:if>

    <xsl:apply-templates select="PNRBFSecondaryBldChg"/>
  </xsl:template>

  <xsl:template match="Ticketing | PNRBFRetrieve | ManualFareUpdate | LinkageDisplay" mode="Unsuccessful">
    <xsl:choose>
      <xsl:when test="ErrText/Text">
        <Error>
          <xsl:attribute name="Type">Galileo</xsl:attribute>
          <xsl:value-of select="concat(ErrText/Err,' - ',ErrText/Text)" />
        </Error>
      </xsl:when>
      <xsl:when test="TextMsg/Txt">
        <xsl:for-each select="TextMsg[Txt != '']/Txt">
          <Error>
            <xsl:attribute name="Type">Galileo</xsl:attribute>
            <xsl:value-of select="."/>
          </Error>
        </xsl:for-each>
      </xsl:when>
      <xsl:when test="PNRBFManagement_53/EndTransaction/ErrorCode">
        <Error>
          <xsl:attribute name="Type">Galileo</xsl:attribute>
          <xsl:value-of select="concat(PNRBFManagement_53/EndTransaction/EndTransactMessage/TypeInd,PNRBFManagement_53/EndTransaction/EndTransactMessage/Num,' - ', PNRBFManagement_53/EndTransaction/EndTransactMessage/Text)" />
        </Error>
      </xsl:when>
    </xsl:choose>
    <xsl:if test="PNRBFManagement_53/TransactionErrorCode">
      <Error>
        <xsl:attribute name="Type">Galileo</xsl:attribute>
        <xsl:value-of select="concat(PNRBFManagement_53/TransactionErrorCode/Code, ' - ', PNRBFManagement_53/TransactionErrorCode/Domain)" />
      </Error>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PNRBFSecondaryBldChg | HostApplicationError">
    <Errors>
      <Error>
        <xsl:attribute name="Type">Galileo</xsl:attribute>
        <xsl:value-of select="Text" />
      </Error>
    </Errors>
  </xsl:template>

  <xsl:template match="TransactionErrorCode">
    <Errors>
      <xsl:choose>
        <xsl:when test="../Ticketing/ErrText">
          <Error>
            <xsl:attribute name="Type">Galileo</xsl:attribute>
            <xsl:value-of select="'CANNOT ISSUE TICKET'" />
          </Error>
          <Error>
            <xsl:attribute name="Type">Galileo</xsl:attribute>
            <xsl:value-of select="concat(../Ticketing/ErrText/Err, ' - ', ../Ticketing/ErrText/Text)" />
          </Error>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:attribute name="Type">Galileo</xsl:attribute>
            <xsl:value-of select="'CANNOT ISSUE TICKET - UNKNOWN GALILEO ERROR'" />
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </Errors>
  </xsl:template>

  <xsl:template match="Screen">
    <xsl:variable name="lines">
      <xsl:for-each select="Line">
        <xsl:value-of select="."/>
      </xsl:for-each>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="contains($lines,'FILED FARE 1 SENT TO PRINTER') or contains($lines,'ELECTRONIC TKT GENERATED')">
        <Success />
        <Warnings>
          <xsl:for-each select="Line">
            <Warning>
              <xsl:value-of select="."/>
            </Warning>
          </xsl:for-each>
        </Warnings>
        <UniqueID>
          <xsl:variable name="recloc">
            <xsl:value-of select="substring-after($lines,'RECORD LOCATOR: *')"/>
          </xsl:variable>
          <xsl:attribute name="ID">
            <xsl:value-of select="substring($recloc,1,6)"/>
          </xsl:attribute>
        </UniqueID>
        <TicketingControl>
          <Type>OK</Type>
        </TicketingControl>
      </xsl:when>
      <xsl:otherwise>
        <Errors>
          <xsl:for-each select="Line">
            <Error>
              <xsl:value-of select="."/>
            </Error>
          </xsl:for-each>
        </Errors>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="TicketPrinterLinkage_1_0">
    <xsl:choose>
      <xsl:when test="LinkageDisplay/ErrText/Text != ''">
        <xsl:apply-templates select="LinkageDisplay" mode="Unsuccessful" />
      </xsl:when>
      <xsl:when test="LinkageDisplay/PrinterParameters[Type = 'T'][1]='U'">
        <Success />
        <Warnings>
          <xsl:for-each select="Line">
            <Warning>
              <xsl:value-of select="."/>
            </Warning>
          </xsl:for-each>
        </Warnings>
        <xsl:if test="UniqueID">
          <UniqueID>
            <xsl:attribute name="ID">
              <xsl:value-of select="UniqueID"/>
            </xsl:attribute>
          </UniqueID>
        </xsl:if>
        <TicketingControl>
          <Type>OK</Type>
        </TicketingControl>
      </xsl:when>
      <xsl:otherwise>
        <TicketingControl>
          <Type>OK</Type>
        </TicketingControl>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>