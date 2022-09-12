<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_HotelInfoRQ.xsl 													-->
  <!-- ================================================================== -->
  <!-- Date: 17 Jul 2014 - Rastko - added support for old and new hotel info message		-->
  <!-- Date: 23 Dec 2013 - Rastko - corrected HotelCode mapping					-->
  <!-- Date: 07 Dec 2012 - Rastko - allow for multiple hotels to be searched			-->
  <!-- Date: 08 Aug 2012 - Rastko - new implementation								-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="username" select="OTA_HotelDescriptiveInfoRQ/POS/TPA_Extensions/Provider/Userid"/>
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_HotelDescriptiveInfoRQ" />
  </xsl:template>
  <!-- ********************************************************************************************************** -->
  <xsl:template match="OTA_HotelDescriptiveInfoRQ">
    <xsl:choose>
      <xsl:when test="$username='Control-Risks'">
        <Hotel_Features>
          <hotelPropertySelection>
            <xsl:apply-templates select="HotelDescriptiveInfos/HotelDescriptiveInfo" />
          </hotelPropertySelection>
        </Hotel_Features>
      </xsl:when>
      <xsl:otherwise>
        <OTA_HotelDescriptiveInfoRQ>
          <xsl:attribute name="EchoToken">
            <xsl:choose>
              <xsl:when test="count(HotelDescriptiveInfos/HotelDescriptiveInfo) &gt; 1">
                <xsl:value-of select="'PartialWithParsing'"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="'WithParsing'"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="PrimaryLangID">
            <xsl:value-of select="'en'"/>
          </xsl:attribute>
          <xsl:attribute name="Version">
            <xsl:value-of select="'6.001'"/>
          </xsl:attribute>
          <HotelDescriptiveInfos>
            <xsl:for-each select="HotelDescriptiveInfos/HotelDescriptiveInfo">
              <HotelDescriptiveInfo>
                <xsl:attribute name="HotelCode">
                  <xsl:value-of select="concat(@ChainCode,@HotelCityCode,@HotelCode)"/>
                </xsl:attribute>
                <HotelInfo SendData="true"/>
                <FacilityInfo SendGuestRooms="true" SendMeetingRooms="true" SendRestaurants="true"/>
                <Policies SendPolicies="true"/>
                <AreaInfo SendAttractions="true" SendRecreations="true" SendRefPoints="true"/>
                <AffiliationInfo SendAwards="true" SendLoyalPrograms="true"/>
                <ContactInfo SendData="true"/>
                <MultimediaObjects SendData="true"/>
                <ContentInfos>
                  <ContentInfo Name="SecureMultimediaURLs"/>
                </ContentInfos>
              </HotelDescriptiveInfo>
            </xsl:for-each>
          </HotelDescriptiveInfos>
        </OTA_HotelDescriptiveInfoRQ>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- ********************************************************************************************************** -->
  <xsl:template match="HotelDescriptiveInfo">
    <xsl:variable name="ArrDate">
      <xsl:value-of select="substring-before(TPA_Extensions/ArrivalDate,'T')" />
    </xsl:variable>
    <xsl:variable name="DepDate">
      <xsl:value-of select="substring-before(TPA_Extensions/DepartureDate,'T')" />
    </xsl:variable>
    <propertyInformation>
      <chainCode>
        <xsl:value-of select="@ChainCode" />
      </chainCode>
      <cityCode>
        <xsl:value-of select="@HotelCityCode" />
      </cityCode>
      <propertyCode>
        <xsl:value-of select="@HotelCode" />
      </propertyCode>
    </propertyInformation>
    <displayCatSelection>
      <!--Amadeus allows only a max of 3 featuresCategory fields -->
      <!--xsl:if test="ContactInfo/@SendData = 'true' or AreaInfo/@SendRefPoints = 'true'"-->
      <featuresCategory>1</featuresCategory>
      <!--/xsl:if>
			<xsl:if test="HotelInfo/@SendData = 'true'">
				<featuresCategory>2</featuresCategory>
				<featuresCategory>4</featuresCategory>
			</xsl:if>
			<xsl:if test="Policies/@SendPolicies='true'">
				<featuresCategory>3</featuresCategory>
				<featuresCategory>5</featuresCategory>
				<featuresCategory>6</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo != ' ' ">
				<featuresCategory>11</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo/@SendMeetingRooms = 'true'">
				<featuresCategory>14</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo/@SendGuestRooms = 'true'">
				<featuresCategory>12</featuresCategory>
			</xsl:if>
			<xsl:if test="FacilityInfo/@SendRestaurants = 'true'">
				<featuresCategory> 13</featuresCategory>
			</xsl:if-->
    </displayCatSelection>
  </xsl:template>
</xsl:stylesheet>