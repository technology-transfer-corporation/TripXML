<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- 
================================================================== 
Galileo_QueueRQ.xsl 															
================================================================== 
Date: 17 Feb 2020 - Kobelev - Finilized RemoveQueue request format.
Date: 07 Feb 2020 - Kobelev - Added Queue Category to request.
Date: 11 Mar 2019 - Kobelev
Date: 22 Nov 2012 - Rastko																
================================================================== 
-->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_QueueRQ" />
  </xsl:template>

  <xsl:template match="OTA_QueueRQ">
    <xsl:apply-templates select="ListQueue"/>
    <xsl:apply-templates select="CountQueue"/>
    <xsl:apply-templates select="RemoveQueue"/>
    <xsl:apply-templates select="BounceQueue"/>
    <xsl:apply-templates select="Move"/>
    <xsl:apply-templates select="PlaceQueue"/>
    <!--xsl:apply-templates select="Clean"/-->
  </xsl:template>

  <xsl:template match="PlaceQueue">
    <PNRBFManagement_53>
      <PNRBFRetrieveMods>
        <PNRAddr>
          <FileAddr />
          <CodeCheck />
          <RecLoc>
            <xsl:value-of select="UniqueID/@ID"/>
          </RecLoc>
        </PNRAddr>
      </PNRBFRetrieveMods>
      <EndTransactionMods>
        <EndTransactRequest>
          <ETInd>Q</ETInd>
          <RcvdFrom>TRIPXML</RcvdFrom>
        </EndTransactRequest>
        <GlobalAccessInfo>
          <GlobAccessCRSAry>
            <GlobAccessCRS>
              <CRS>
                <xsl:choose>
                  <xsl:when test="../POS/TPA_Extensions/Provider/Name = 'Apollo'">1V</xsl:when>
                  <xsl:otherwise>1G</xsl:otherwise>
                </xsl:choose>
              </CRS>
              <PCC>
                <xsl:value-of select="@PseudoCityCode"/>
              </PCC>
              <QNum>
                <xsl:value-of select="@Number"/>
              </QNum>
              <QCat>
                <xsl:value-of select="@Category"/>
              </QCat>
              <DtRange />
            </GlobAccessCRS>
          </GlobAccessCRSAry>
        </GlobalAccessInfo>
      </EndTransactionMods>
    </PNRBFManagement_53>
  </xsl:template>

  <!--xsl:template match="Clean">
	</xsl:template-->

  <xsl:template match="BounceQueue">
    <xsl:choose>
      <xsl:when test="@Action='R'">
        <Cryptic_GetScreen_Query>
          <Command>QN</Command>
        </Cryptic_GetScreen_Query>
      </xsl:when>
      <xsl:when test="@Action='I'">
        <Cryptic_GetScreen_Query>
          <Command>IG</Command>
        </Cryptic_GetScreen_Query>
      </xsl:when>
      <xsl:when test="@Action='E'">
        <Cryptic_GetScreen_Query>
          <Command>ER</Command>
        </Cryptic_GetScreen_Query>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="RemoveQueue">
    <QueueProcessing_16>
      <QueueMods>
        <QueueRemoveSignOutMods>
          <Action>QR</Action>
          <Recvd/>
        </QueueRemoveSignOutMods>
      </QueueMods>
    </QueueProcessing_16>
  </xsl:template>

  <xsl:template match="Move">
    <PoweredQueue_MoveItem>
      <placementOption>
        <selectionDetails>
          <option>QBD</option>
        </selectionDetails>
      </placementOption>
      <targetDetails>
        <targetOffice>
          <sourceType>
            <sourceQualifier1>3</sourceQualifier1>
          </sourceType>
        </targetOffice>
        <QueueNumber>
          <queueDetails>
            <number>
              <xsl:value-of select="FromQueue/@Number"/>
            </number>
            <category>
              <xsl:value-of select="FromQueue/@Category"/>
            </category>
          </queueDetails>
        </QueueNumber>
      </targetDetails>
      <targetDetails>
        <targetOffice>
          <sourceType>
            <sourceQualifier1>3</sourceQualifier1>
          </sourceType>
        </targetOffice>
        <QueueNumber>
          <queueDetails>
            <number>
              <xsl:value-of select="ToQueue/@Number"/>
            </number>
            <category>
              <xsl:value-of select="ToQueue/@Category"/>
            </category>
          </queueDetails>
        </QueueNumber>
      </targetDetails>
      <numberOfPNRs>
        <quantityDetails>
          <numberOfUnit>
            <xsl:value-of select="@ItemsQuantity"/>
          </numberOfUnit>
        </quantityDetails>
      </numberOfPNRs>
    </PoweredQueue_MoveItem>
  </xsl:template>

  <xsl:template match="ListQueue">
    <QueueProcessing_16>
      <QueueMods>
        <QueueSignInCountListMods>
          <Action>QLD</Action>
          <GetMoreInd></GetMoreInd>
          <PCCAry>
            <PCCInfo>
              <PCC>
                <xsl:value-of select="@PseudoCityCode"/>
              </PCC>
              <QNum>
                <xsl:value-of select="@Number"/>
              </QNum>
              <QCat>
                <xsl:value-of select="@Category"/>
              </QCat>
              <DtRange></DtRange>
            </PCCInfo>
          </PCCAry>
        </QueueSignInCountListMods>
      </QueueMods>
    </QueueProcessing_16>
  </xsl:template>

  <xsl:template match="CountQueue">
    <QueueProcessing_16>
      <QueueMods>
        <QueueSignInCountListMods>
          <Action>QCT</Action>
          <PCCAry>
            <PCCInfo>
              <PCC>
                <xsl:choose>
                  <xsl:when test="@PseudoCityCode != ''">
                    <xsl:value-of select="@PseudoCityCode"/>
                  </xsl:when>
                  <xsl:otherwise><![CDATA[    ]]></xsl:otherwise>
                </xsl:choose>
              </PCC>
              <QNum>
                <xsl:choose>
                  <xsl:when test="@Number != ''">
                    <xsl:value-of select="@Number"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text><![CDATA[  ]]></xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </QNum>
              <QCat>
                <xsl:choose>
                  <xsl:when test="@Category != ''">
                    <xsl:value-of select="@Category"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text><![CDATA[  ]]></xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </QCat>
            </PCCInfo>
          </PCCAry>
        </QueueSignInCountListMods>
      </QueueMods>
    </QueueProcessing_16>
  </xsl:template>
</xsl:stylesheet>
