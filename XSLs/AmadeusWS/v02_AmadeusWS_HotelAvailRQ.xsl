<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- v02_AmadeusWS_HotelAvailRQ.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 09 Jul 2014 - Rastko - send default room quantity as 1					-->
  <!-- Date: 03 Mar 2014 - Rastko - new file												-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_HotelAvailRQ" />
  </xsl:template>
  <xsl:template match="OTA_HotelAvailRQ">
    <xsl:choose>
      <xsl:when test="AvailRequestSegments/AvailRequestSegment/@AvailReqType = 'PricingDetails'">
        <PricingDetails>
          <Hotel_SingleAvailability>
            <scrollingInformation>
              <displayRequest>050</displayRequest>
              <maxNumberItems>32</maxNumberItems>
              <nextItemReference></nextItemReference>
            </scrollingInformation>
            <xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="single" />
          </Hotel_SingleAvailability>
          <Hotel_StructuredPricing>
            <xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="pricing" />
          </Hotel_StructuredPricing>
        </PricingDetails>
      </xsl:when>
      <xsl:otherwise>
        <OTA_HotelAvailRQ AvailRatesOnly="true" EchoToken="MultiSingle" ExactMatchOnly="false" PrimaryLangID="EN" RateDetailsInd="false" RateRangeOnly="true" SummaryOnly="true" Version="4.000">
          <xsl:attribute name="RateDetailsInd">
            <xsl:choose>
              <xsl:when test="AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion/HotelRef/@HotelCode!=''">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="MaxResponses">
            <xsl:choose>
              <xsl:when test="@MaxResponses!=''">
                <xsl:value-of select="@MaxResponses"/>
              </xsl:when>
              <xsl:otherwise>40</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:if test="POS/Source/@ISOCurrency!='' or @RequestedCurrency!=''">
            <xsl:attribute name="RequestedCurrency">
              <xsl:choose>
                <xsl:when test="POS/Source/@ISOCurrency!=''">
                  <xsl:value-of select="POS/Source/@ISOCurrency"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="@RequestedCurrency"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </xsl:if>
          <AvailRequestSegments>
            <xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="multi" />
          </AvailRequestSegments>
        </OTA_HotelAvailRQ>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="AvailRequestSegment" mode="pricing">
    <hotelPropertyInfo>
      <hotelReference>
        <chainCode>
          <xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode" />
        </chainCode>
        <cityCode>
          <xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
        </cityCode>
        <hotelCode>
          <xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCode" />
        </hotelCode>
      </hotelReference>
    </hotelPropertyInfo>
    <bookingPeriod>
      <businessSemantic>CHK</businessSemantic>
      <beginDateTime>
        <year>
          <xsl:value-of select="substring(string(StayDateRange/@Start),1,4)" />
        </year>
        <month>
          <xsl:value-of select="substring(string(StayDateRange/@Start),6,2)" />
        </month>
        <day>
          <xsl:value-of select="substring(string(StayDateRange/@Start),9,2)" />
        </day>
      </beginDateTime>
      <endDateTime>
        <year>
          <xsl:value-of select="substring(string(StayDateRange/@End),1,4)" />
        </year>
        <month>
          <xsl:value-of select="substring(string(StayDateRange/@End),6,2)" />
        </month>
        <day>
          <xsl:value-of select="substring(string(StayDateRange/@End),9,2)" />
        </day>
      </endDateTime>
    </bookingPeriod>
    <roomInformation>
      <bookingCode>
        <xsl:value-of select="RatePlanCandidates/RatePlanCandidate/@RatePlanID"/>
      </bookingCode>
    </roomInformation>
  </xsl:template>

  <xsl:template match="AvailRequestSegment" mode="multi">
    <AvailRequestSegment InfoSource="Distribution">
      <HotelSearchCriteria AvailableOnlyIndicator="true">
        <Criterion ExactMatch="true">
          <xsl:if test="count(HotelSearchCriteria/Criterion)=1">
            <xsl:copy-of select="Position"/>
            <xsl:copy-of select="Address"/>
            <xsl:copy-of select="Telephone"/>
            <xsl:copy-of select="RefPoint"/>
            <HotelRef HotelCodeContext="1A">
              <xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@ChainCode!=''">
                <xsl:attribute name="ChainCode">
                  <xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode" />
                </xsl:attribute>
              </xsl:if>
              <xsl:attribute name="HotelCityCode">
                <xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
              </xsl:attribute>
              <xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelCode!=''">
                <xsl:attribute name="HotelCode">
                  <xsl:value-of select="concat(HotelSearchCriteria/Criterion/HotelRef/@ChainCode,HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode,HotelSearchCriteria/Criterion/HotelRef/@HotelCode)" />
                </xsl:attribute>
              </xsl:if>
              <xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelName!=''">
                <xsl:attribute name="HotelName">
                  <xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelName" />
                </xsl:attribute>
              </xsl:if>
            </HotelRef>
            <xsl:copy-of select="Radius"/>
            <xsl:copy-of select="HotelAmenity"/>
          </xsl:if>
          <StayDateRange>
            <xsl:attribute name="Start">
              <xsl:value-of select="StayDateRange/@Start"/>
            </xsl:attribute>
            <xsl:attribute name="End">
              <xsl:value-of select="StayDateRange/@End"/>
            </xsl:attribute>
          </StayDateRange>
          <xsl:apply-templates select="RatePlanCandidates" mode="multi" />
          <RoomStayCandidates>
            <RoomStayCandidate  RoomID="1">
              <xsl:attribute name="Quantity">
                <xsl:choose>
                  <xsl:when test="RoomStayCandidates/RoomStayCandidate/@Quantity!=''">
                    <xsl:value-of select="RoomStayCandidates/RoomStayCandidate/@Quantity"/>
                  </xsl:when>
                  <xsl:otherwise>1</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <GuestCounts IsPerRoom="true">
                <GuestCount AgeQualifyingCode="10" Count="{RoomStayCandidates/RoomStayCandidate/GuestCounts/GuestCount/@Count}"/>
              </GuestCounts>
            </RoomStayCandidate>
          </RoomStayCandidates>
        </Criterion>
        <xsl:if test="count(HotelSearchCriteria/Criterion)>1">
          <Criterion>
            <xsl:for-each select="HotelSearchCriteria/Criterion">
              <HotelRef HotelCodeContext="1A">
                <xsl:if test="HotelRef/@ChainCode!=''">
                  <xsl:attribute name="ChainCode">
                    <xsl:value-of select="HotelRef/@ChainCode" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:attribute name="HotelCityCode">
                  <xsl:value-of select="HotelRef/@HotelCityCode" />
                </xsl:attribute>
                <xsl:if test="HotelRef/@HotelCode!=''">
                  <xsl:attribute name="HotelCode">
                    <xsl:value-of select="concat(HotelRef/@ChainCode,HotelRef/@HotelCityCode,HotelRef/@HotelCode)" />
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="HotelRef/@HotelName!=''">
                  <xsl:attribute name="HotelName">
                    <xsl:value-of select="HotelRef/@HotelName" />
                  </xsl:attribute>
                </xsl:if>
              </HotelRef>
            </xsl:for-each>
          </Criterion>
        </xsl:if>
      </HotelSearchCriteria>
    </AvailRequestSegment>
  </xsl:template>

  <xsl:template match="RatePlanCandidates" mode="multi">
    <xsl:if test="RatePlanCandidate/@RatePlanCode != ''">
      <RatePlanCandidates>
        <xsl:apply-templates select="RatePlanCandidate" mode="multi" />
      </RatePlanCandidates>
    </xsl:if>
  </xsl:template>

  <xsl:template match="RatePlanCandidate" mode="multi">
    <xsl:if test="@RatePlanCode != ''">
      <RatePlanCandidates>
        <RatePlanCandidate>
          <xsl:attribute name="RatePlanCode">
            <xsl:value-of select="@RatePlanCode" />
          </xsl:attribute>
        </RatePlanCandidate>
      </RatePlanCandidates>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>
