<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- v02_AmadeusWS_HotelInfo1RQ.xsl 												-->
  <!-- ================================================================== -->
  <!-- Date: 16 Jul 2014 - Rastko - send hotel pricing when only 1 hotel in request		-->
  <!-- Date: 19 Jun 2014 - Rastko - get rate info for all rates							-->
  <!-- Date: 16 Jun 2014 - Rastko - new implementation								-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <HtlMed>
      <xsl:apply-templates select="OTA_HotelAvailRS" mode="content"/>
      <xsl:apply-templates select="OTA_HotelAvailRS" mode="pricing"/>
    </HtlMed>
  </xsl:template>
  <!-- ********************************************************************************************************** -->
  <xsl:template match="OTA_HotelAvailRS" mode="pricing">
    <xsl:if test="count(HotelStays/HotelStay)=1">
      <xsl:for-each select="RoomStays/RoomStay">
        <OTA_HotelAvailRQ EchoToken="Pricing" Version="4.000" SummaryOnly="false" AvailRatesOnly="true" 	RateRangeOnly="false">
          <xsl:attribute name="RequestedCurrency">
            <xsl:value-of select="RoomRates/RoomRate/Total/@CurrencyCode"/>
          </xsl:attribute>
          <AvailRequestSegments>
            <AvailRequestSegment InfoSource="MultiSource">
              <HotelSearchCriteria>
                <Criterion>
                  <HotelRef ChainCode="{../../HotelStays/HotelStay/BasicPropertyInfo/@ChainCode}" HotelCode="{../../HotelStays/HotelStay/BasicPropertyInfo/@HotelCode}" 	HotelCityCode="{../../HotelStays/HotelStay/BasicPropertyInfo/@HotelCityCode}" HotelCodeContext="IA" />
                  <StayDateRange Start="{TimeSpan/@Start}" End="{TimeSpan/@End}" />
                  <RatePlanCandidates>
                    <RatePlanCandidate RatePlanCode="{RoomRates/RoomRate/@RatePlanCode}" />
                  </RatePlanCandidates>
                  <RoomStayCandidates>
                    <RoomStayCandidate RoomTypeCode="{RoomRates/RoomRate/@RoomTypeCode}" Quantity="{RoomRates/RoomRate/@NumberOfUnits}" BookingCode="{RoomRates/RoomRate/@BookingCode}">
                      <GuestCounts>
                        <GuestCount AgeQualifyingCode="{GuestCounts/GuestCount/@AgeQualifyingCode}" Count="{GuestCounts/GuestCount/@Count}" />
                      </GuestCounts>
                    </RoomStayCandidate>
                  </RoomStayCandidates>
                </Criterion>
              </HotelSearchCriteria>
            </AvailRequestSegment>
          </AvailRequestSegments>
        </OTA_HotelAvailRQ>
      </xsl:for-each>
    </xsl:if>
  </xsl:template>

  <xsl:template match="OTA_HotelAvailRS" mode="content">
    <OTA_HotelDescriptiveInfoRQ EchoToken="WithParsing" PrimaryLangID="en" Version="6.001">
      <HotelDescriptiveInfos>
        <xsl:apply-templates select="HotelStays/HotelStay"/>
      </HotelDescriptiveInfos>
    </OTA_HotelDescriptiveInfoRQ>
  </xsl:template>

  <xsl:template match="HotelStay">
    <HotelDescriptiveInfo>
      <xsl:attribute name="HotelCode">
        <xsl:value-of select="BasicPropertyInfo/@HotelCode"/>
      </xsl:attribute>
      <HotelInfo SendData="true"/>
      <FacilityInfo SendGuestRooms="true" SendMeetingRooms="true" SendRestaurants="true"/>
      <Policies SendPolicies="true"/>
      <AreaInfo SendAttractions="true" SendRecreations="true" SendRefPoints="true"/>
      <AffiliationInfo SendAwards="true" SendLoyalPrograms="false"/>
      <ContactInfo SendData="true"/>
      <MultimediaObjects SendData="true"/>
      <ContentInfos>
        <ContentInfo Name="SecureMultimediaURLs"/>
      </ContentInfos>
    </HotelDescriptiveInfo>
  </xsl:template>

</xsl:stylesheet>