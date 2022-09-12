<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Sabre_PNRCancelRQ.xsl 															-->
  <!-- ================================================================== -->
  <!-- Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0				-->
  <!-- Date: 01 Sep 2006 - Rastko														-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="PCC">
    <xsl:value-of select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
  </xsl:variable>
  <xsl:template match="/">
    <PNRCancel>
      <xsl:apply-templates select="OTA_CancelRQ" />
    </PNRCancel>
  </xsl:template>
  <!--************************************************************************************************************	-->
  <xsl:template match="OTA_CancelRQ">
    <Read>
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
    </Read>
    <Cancel>
      <OTA_CancelRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.01">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC" />
            </xsl:attribute>
          </Source>
        </POS>
        <TPA_Extensions>
          <SegmentCancel Type="Entire"/>
        </TPA_Extensions>
      </OTA_CancelRQ>
    </Cancel>
    <ET>
      <EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.2">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC" />
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
                    <xsl:otherwise>TRAVELTALK</xsl:otherwise>
                  </xsl:choose>
                </GivenName>
              </AccessPerson>
            </Access>
          </TPA_Extensions>
        </UpdatedBy>
        <EndTransaction Ind="true"/>
      </EndTransactionRQ>
    </ET>
  </xsl:template>
  <!--************************************************************************************************************	-->
</xsl:stylesheet>
