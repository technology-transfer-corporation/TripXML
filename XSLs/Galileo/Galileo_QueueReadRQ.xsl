<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- 
  ================================================================== 
  Galileo_QueueReadRQ.xsl 						 								       
  ================================================================== 
  Date: 07 Feb 2020 - Kobelev - Added Queue Category to request.
  Date: 11 Mar 2019 - Kobelev - Web Services Version Update
  Date: 17 Sep 2010 - Rastko - formatted pcc to be on 4 digits					       
  Date: 15 Sep 2010 - Rastko - added NextKeep code							       
  Date: 23 Nov - Rastko																       
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_QueueReadRQ" />
  </xsl:template>

  <xsl:template match="OTA_QueueReadRQ">
    <xsl:apply-templates select="ItemOnQueue"/>
    <xsl:apply-templates select="AccessQueue"/>
    <xsl:apply-templates select="ExitQueue"/>
  </xsl:template>

  <xsl:template match="ItemOnQueue">
    <xsl:choose>
      <xsl:when test="@Action = 'NextRemove'">
        <QueueProcessing_16>
          <QueueMods>
            <QueueRemoveSignOutMods>
              <Action>QR</Action>
              <Recvd>TRIPXML</Recvd>
            </QueueRemoveSignOutMods>
          </QueueMods>
        </QueueProcessing_16>
      </xsl:when>
      <xsl:when test="@Action = 'NextKeep'">
        <Cryptic>I</Cryptic>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="ExitQueue">
    <QueueProcessing_16>
      <QueueMods>
        <QueueRemoveSignOutMods>
          <Action>QXI</Action>
          <Recvd>TRIPXML</Recvd>
        </QueueRemoveSignOutMods>
      </QueueMods>
    </QueueProcessing_16>
  </xsl:template>

  <xsl:template match="AccessQueue">
    <QueueProcessing_16>
      <QueueMods>
        <QueueSignInCountListMods>
          <Action>Q</Action>
          <GetMoreInd></GetMoreInd>
          <QLDNameReq></QLDNameReq>
          <PCCAry>
            <PCCInfo>
              <PCC>
                <xsl:if test="string-length(@PseudoCityCode)=3">
                  <xsl:value-of select="'0'"/>
                </xsl:if>
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

</xsl:stylesheet>
