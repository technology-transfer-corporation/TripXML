<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ********************************************************************************************************* -->
  <!--  							Car Availability Request                                                         -->
  <!-- Date: 18 Oct 2010 - Rastko - added support for corporate discount and passenger ID	-->
  <!-- Date: 09  Apr 2013 - Suraj - Changed to multiple vendors issue	-->
  <!-- ********************************************************************************************************* -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_VehAvailRateRQ" />
  </xsl:template>
  <xsl:template match="OTA_VehAvailRateRQ">
    <CarStandardAvail_6_0>
      <CarAvailMods>
        <StartDt>
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@PickUpDateTime,1,4)" />
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@PickUpDateTime,6,2)" />
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@PickUpDateTime,9,2)" />
        </StartDt>
        <StartTm>
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@PickUpDateTime,12,2)" />
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@PickUpDateTime,15,2)" />
        </StartTm>
        <Pt>
          <xsl:value-of select="VehAvailRQCore/VehRentalCore/PickUpLocation/@LocationCode" />
        </Pt>
        <xsl:choose>
          <xsl:when test="VehAvailRQCore/TPA_Extensions/VehOptions/@LocationCategory!='T'">
            <LocnCity>
              <xsl:value-of select="VehAvailRQCore/TPA_Extensions/VehOptions/@LocationCode" />
            </LocnCity>
            <LocnCat>
              <xsl:value-of select="VehAvailRQCore/TPA_Extensions/VehOptions/@LocationCategory" />
            </LocnCat>
          </xsl:when>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="count(VehAvailRQCore/VendorPrefs/VendorPref)='1'">
            <ExactVAry>
              <xsl:value-of select="VehAvailRQCore/VendorPrefs/VendorPref/@Code" />
            </ExactVAry>
          </xsl:when>
        </xsl:choose>
        <EndDt>
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@ReturnDateTime,1,4)" />
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@ReturnDateTime,6,2)" />
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@ReturnDateTime,9,2)" />
        </EndDt>
        <EndTm>
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@ReturnDateTime,12,2)" />
          <xsl:value-of select="substring(VehAvailRQCore/VehRentalCore/@ReturnDateTime,15,2)" />
        </EndTm>
        <xsl:if test="VehAvailRQCore/VehRentalCore/PickUpLocation/@LocationCode != VehAvailRQCore/VehRentalCore/ReturnLocation/@LocationCode">
          <EndLocn>
            <xsl:value-of select="VehAvailRQCore/VehRentalCore/ReturnLocation/@LocationCode" />
          </EndLocn>
        </xsl:if>
        <xsl:if test="VehAvailRQCore/VendorPrefs/VendorPref">
          <xsl:variable name="VendorCount" select="count(VehAvailRQCore/VendorPrefs/VendorPref)" />
          <!--xsl:if test= "$VendorCount!= '1'"-->
          <xsl:apply-templates select="VehAvailRQCore/VendorPrefs/VendorPref" mode="vendor" />
          <!--/xsl:if-->
        </xsl:if>
        <xsl:apply-templates select="VehAvailRQCore/VehPrefs/VehPref" />
        <xsl:apply-templates select="VehAvailRQCore/RateQualifier" />
        <xsl:if test="VehAvailRQCore/TPA_Extensions/VehOptions/@GuaranteedRateInd">
          <GuarRateInd>
            <xsl:choose>
              <xsl:when test="VehAvailRQCore/TPA_Extensions/VehOptions/@GuaranteedRateInd='0'">
                <xsl:text>N</xsl:text>
              </xsl:when>
              <xsl:otherwise>Y</xsl:otherwise>
            </xsl:choose>
          </GuarRateInd>
        </xsl:if>
        <xsl:if test="VehAvailRQCore/TPA_Extensions/VehOptions/@UnlimitedMilesInd">
          <UnlimitedMilesInd>
            <xsl:choose>
              <xsl:when test="VehAvailRQCore/TPA_Extensions/VehOptions/@UnlimitedMilesInd='0'">
                <xsl:text>N</xsl:text>
              </xsl:when>
              <xsl:otherwise>Y</xsl:otherwise>
            </xsl:choose>
          </UnlimitedMilesInd>
        </xsl:if>
        <xsl:if test="VehAvailRQCore/TPA_Extensions/VehOptions/@LowRateRange">
          <LowRateRange>
            <xsl:value-of select="VehAvailRQCore/TPA_Extensions/VehOptions/@LowRateRange" />
          </LowRateRange>
        </xsl:if>
        <xsl:if test="VehAvailRQCore/TPA_Extensions/VehOptions/@LowRateRange">
          <HighRateRange>
            <xsl:value-of select="VehAvailRQCore/TPA_Extensions/VehOptions/@HighRateRange" />
          </HighRateRange>
        </xsl:if>
        <Currency>
          <xsl:value-of select="POS/Source/@ISOCurrency" />
        </Currency>
      </CarAvailMods>
    </CarStandardAvail_6_0>
  </xsl:template>
  <xsl:template match="VendorPref[position()=1]" mode="vendor">
    <CarV1>
      <xsl:value-of select="@Code" />
    </CarV1>
  </xsl:template>
  <xsl:template match="VendorPref[position()=2]" mode="vendor">
    <CarV2>
      <xsl:value-of select="@Code" />
    </CarV2>
  </xsl:template>
  <xsl:template match="VendorPref[position()=3]" mode="vendor">
    <CarV3>
      <xsl:value-of select="@Code" />
    </CarV3>
  </xsl:template>
  <xsl:template match="VendorPref[position()=4]" mode="vendor">
    <CarV4>
      <xsl:value-of select="@Code" />
    </CarV4>
  </xsl:template>
  <xsl:template match="VehPref[position()=1]">
    <CarType1>
      <xsl:value-of select="VehType/@VehicleCategory" />
    </CarType1>
  </xsl:template>
  <xsl:template match="VehPref[position()=2]">
    <CarType2>
      <xsl:value-of select="VehType/@VehicleCategory" />
    </CarType2>
  </xsl:template>
  <xsl:template match="VehPref[position()=3]">
    <CarType3>
      <xsl:value-of select="VehType/@VehicleCategory" />
    </CarType3>
  </xsl:template>
  <xsl:template match="RateQualifier">
    <xsl:if test="@RatePeriod!=''">
      <RateType>
        <xsl:choose>
          <xsl:when test="@RatePeriod='Hourly'">
            <xsl:text>H</xsl:text>
          </xsl:when>
          <xsl:when test="@RatePeriod='Daily'">
            <xsl:text>D</xsl:text>
          </xsl:when>
          <xsl:when test="@RatePeriod='Weekend'">
            <xsl:text>E</xsl:text>
          </xsl:when>
          <xsl:when test="@RatePeriod='Weekly'">
            <xsl:text>W</xsl:text>
          </xsl:when>
          <xsl:when test="@RatePeriod='Monthly'">
            <xsl:text>M</xsl:text>
          </xsl:when>
        </xsl:choose>
      </RateType>
    </xsl:if>
    <xsl:if test="@RateCategory!=''">
      <RateCat>
        <xsl:value-of select="@RateCategory" />
      </RateCat>
    </xsl:if>
    <xsl:if test="@CorpDiscountNmbr!='' or @PromotionCode!=''">
      <VCodeAry>
        <VCode>
          <Vnd>
            <xsl:value-of select="../VendorPrefs/VendorPref/@Code"/>
          </Vnd>
          <xsl:if test="@CorpDiscountNmbr!=''">
            <CorpDiscNum>
              <xsl:value-of select="@CorpDiscountNmbr" />
            </CorpDiscNum>
          </xsl:if>
          <xsl:if test="@PromotionCode!=''">
            <CustID>
              <xsl:value-of select="@PromotionCode" />
            </CustID>
          </xsl:if>
        </VCode>
      </VCodeAry>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
