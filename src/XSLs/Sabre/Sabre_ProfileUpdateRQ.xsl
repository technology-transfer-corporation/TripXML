<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_ProfileReadRQ.xsl 													-->
  <!-- ================================================================== -->
  <!-- Date: 02 Aug 2016 - Rastko - added mapping of owning office id				-->
  <!-- Date: 11 Jul 2016 - Rastko - new file												-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <ProfileUpdate>
      <Sabre_OTA_ProfileReadRQ xmlns="http://www.sabre.com/eps/schemas" Version="1.0">
        <Profile>
          <TPA_Identity ClientCode="TN" ClientContextCode="TMP" >
            <xsl:attribute name="DomainID">
              <xsl:value-of select="OTA_ProfileUpdateRQ/POS/Source/@PseudoCityCode"/>
            </xsl:attribute>
            <xsl:attribute name="UniqueID">
              <xsl:value-of select="OTA_ProfileUpdateRQ/UniqueID/@ID"/>
            </xsl:attribute>
          </TPA_Identity>
        </Profile>
      </Sabre_OTA_ProfileReadRQ>
      
      <xsl:if test="OTA_ProfileUpdateRQ/Profile/Accesses/Access[@ActionType='Update']">
        <Sabre_OTA_ProfileUpdateRQ Version="1.0" xmlns="http://www.sabre.com/eps/schemas">
          <ProfileInfo>
            <Profile CreateDateTime="UseRetrieveProfileToReplaceCreateDate" UpdateDateTime="UseRetrieveProfileToReplaceUpdateDate">
              <TPA_Identity ProfileTypeCode="TVL" ClientContextCode="TMP" ClientCode="TN" ProfileName="TestProfile">
                <xsl:attribute name="DomainID">
                  <xsl:choose>
                    <xsl:when test="OTA_ProfileUpdateRQ/UniqueID/@ID_Context!=''">
                      <xsl:value-of select="OTA_ProfileUpdateRQ/UniqueID/@ID_Context"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="OTA_ProfileUpdateRQ/POS/Source/@PseudoCityCode"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="UniqueID">
                  <xsl:value-of select="OTA_ProfileUpdateRQ/UniqueID/@ID"/>
                </xsl:attribute>
                <!--<Login >
                  <xsl:attribute name="LoginID">
                    <xsl:value-of select="OTA_ProfileUpdateRQ/Profile/Accesses/Access/@ID"/>
                  </xsl:attribute>
                </Login>-->
              </TPA_Identity>
              <Traveler>
                UseCreateProfileToReplaceTravelerDate
              </Traveler>
            </Profile>
          </ProfileInfo>
        </Sabre_OTA_ProfileUpdateRQ>
      </xsl:if>
      <xsl:if test="OTA_ProfileUpdateRQ/Profile/Accesses/Access[@ActionType='Delete']">
        <Sabre_OTA_ProfileDeleteRQ Version="1.0" xmlns="http://www.sabre.com/eps/schemas">
          <Delete>
            <Profile PurgeDays="0">
              <TPA_Identity ProfileTypeCode="TVL" ClientCode="TN" ClientContextCode="TMP" ProfileName="TestProfile"  >
                <xsl:attribute name="UniqueID">
                  <xsl:value-of select="OTA_ProfileUpdateRQ/UniqueID/@ID"/>
                </xsl:attribute>
                <xsl:attribute name="DomainID">
                  <xsl:choose>
                    <xsl:when test="OTA_ProfileUpdateRQ/UniqueID/@ID_Context!=''">
                      <xsl:value-of select="OTA_ProfileUpdateRQ/UniqueID/@ID_Context"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="OTA_ProfileUpdateRQ/POS/Source/@PseudoCityCode"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </TPA_Identity>
            </Profile>
          </Delete>
        </Sabre_OTA_ProfileDeleteRQ>
      </xsl:if>
    </ProfileUpdate>
  </xsl:template>
</xsl:stylesheet>
