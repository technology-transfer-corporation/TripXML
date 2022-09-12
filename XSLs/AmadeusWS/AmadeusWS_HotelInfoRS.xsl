<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_HotelInfoRS.xsl 													-->
  <!-- ================================================================== -->
  <!-- Date: 17 Jul 2014 - Rastko - added support for old and new hotel info message		-->
  <!-- Date: 18 Dec 2013 - Rastko - added support for errors							-->
  <!-- Date: 08 Aug 2012 - Rastko - new implementation								-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <OTA_HotelDescriptiveInfoRS Version="1.0">
      <xsl:copy-of select="OTA_HotelDescriptiveInfoRS/Success"/>
      <xsl:copy-of select="OTA_HotelDescriptiveInfoRS/Errors"/>
      <xsl:copy-of select="OTA_HotelDescriptiveInfoRS/HotelDescriptiveContents"/>
      <xsl:apply-templates select="Hotel_FeaturesReply" />
    </OTA_HotelDescriptiveInfoRS>
  </xsl:template>

  <xsl:template match="Hotel_FeaturesReply">
    <Success />
    <HotelDescriptiveContents>
      <HotelDescriptiveContent LanguageCode="en-us">
        <xsl:apply-templates select="hotelProductInfo/propertyHeaderDetails" />
        <xsl:if test="hotelFeaturesTerms[featuresTerms/category = '2'] or hotelFeaturesTerms[featuresTerms/category = '4']">
          <HotelInfo>
            <xsl:apply-templates select="hotelFeaturesTerms[featuresTerms/category = '2']" mode="services"/>
            <xsl:apply-templates select="hotelFeaturesTerms[featuresTerms/category = '4']" mode="facilities"/>
          </HotelInfo>
        </xsl:if>
        <xsl:if test="hotelFeaturesTerms/featuresTerms[category = '3'] or hotelFeaturesTerms/featuresTerms[category = '5'] or hotelFeaturesTerms/featuresTerms[category = '6']">
          <Policies>
            <Policy>
              <xsl:apply-templates select="hotelFeaturesTerms/featuresTerms[category = '3']" mode="policy"/>
              <xsl:if test="hotelFeaturesTerms/featuresTerms[category = '5'] or hotelFeaturesTerms/featuresTerms[category = '6']">
                <PaymentPolicy>
                  <xsl:apply-templates select="hotelFeaturesTerms/featuresTerms[category = '5']" mode="deposit"/>
                  <xsl:apply-templates select="hotelFeaturesTerms/featuresTerms[category = '6']" mode="guarantee"/>
                </PaymentPolicy>
              </xsl:if>
            </Policy>
          </Policies>
        </xsl:if>
        <xsl:apply-templates select="hotelFeaturesTerms/featuresTerms[category = '1']" mode="location"/>
        <xsl:apply-templates select="hotelFeaturesTerms[featuresTerms/category = '1A']" mode="address"/>
      </HotelDescriptiveContent>
    </HotelDescriptiveContents>
  </xsl:template>
  <!--    **************************************************************   -->
  <xsl:template match="propertyHeaderDetails">
    <xsl:attribute name="ChainCode">
      <xsl:value-of select="chainCode" />
    </xsl:attribute>
    <xsl:attribute name="HotelCode">
      <xsl:value-of select="propertyCode" />
    </xsl:attribute>
    <xsl:attribute name="HotelCityCode">
      <xsl:value-of select="cityCode" />
    </xsl:attribute>
    <xsl:attribute name="HotelName">
      <xsl:value-of select="propertyName" />
    </xsl:attribute>
    <xsl:attribute name="HotelCodeContext">
      <xsl:choose>
        <xsl:when test="accessQualifier = 'CA'">Complete Access</xsl:when>
        <xsl:when test="accessQualifier = 'CP'">Complete Access Plus	</xsl:when>
        <xsl:when test="accessQualifier = 'DY'">Dynamic Access</xsl:when>
        <xsl:when test="accessQualifier = 'IA'">Independent Access</xsl:when>
        <xsl:when test="accessQualifier = 'SA'">Standard Access</xsl:when>
      </xsl:choose>
    </xsl:attribute>
    <xsl:attribute name="ChainName">
      <xsl:value-of select="chainName" />
    </xsl:attribute>
  </xsl:template>
  <!--    **************************************************************   -->
  <xsl:template match="featuresTerms" mode="CodeDetail">
    <xsl:if test="category = '1' or category = '2' or category = '3' or category = '4' or category = '5' or category = '6' or category = '7' or category = '8' or category = '9' or category = '10' or category = '11' or category = '12' or category = '13' or category = '14' or category = '15' or category = '16'">
      <Feature>
        <xsl:attribute name="CodeDetail">
          <xsl:if test="category = '1'">Location</xsl:if>
          <xsl:if test="category = '2'">Extra Charges</xsl:if>
          <xsl:if test="category = '3'">Policy</xsl:if>
          <xsl:if test="category = '4'">Facilities</xsl:if>
          <xsl:if test="category = '5'">Deposit</xsl:if>
          <xsl:if test="category = '6'">Guarantee</xsl:if>
          <xsl:if test="category = '7'">Stay</xsl:if>
          <xsl:if test="category = '8'">Other</xsl:if>
          <xsl:if test="category = '9'">Transportation</xsl:if>
          <xsl:if test="category = '10'">Safety</xsl:if>
          <xsl:if test="category = '11'">Hotel Category</xsl:if>
          <xsl:if test="category = '12'">Room Description</xsl:if>
          <xsl:if test="category = '13'">Dining</xsl:if>
          <xsl:if test="category = '14'">Meeting Facilities</xsl:if>
          <xsl:if test="category = '15'">Commission</xsl:if>
          <xsl:if test="category = '16'">Frequent Stay</xsl:if>
        </xsl:attribute>
        <Description>
          <xsl:choose>
            <xsl:when test="position()='1'">
              <xsl:apply-templates select="description/following-sibling::description" mode="NextLine" />
            </xsl:when>
            <xsl:otherwise>
              <xsl:apply-templates select="description" mode="NextLine" />
            </xsl:otherwise>
          </xsl:choose>
        </Description>
      </Feature>
    </xsl:if>
  </xsl:template>
  <!--    **************************************************************   -->
  <xsl:template match="description" mode="NextLine">
    <Text>
      <xsl:value-of select="translate(.,'&#164;',' ')" />
    </Text>
  </xsl:template>
  <!--    **************************************************************   -->
  <xsl:template match="hotelFeaturesTerms" mode="phone">
    <Phone>
      <xsl:attribute name="PhoneNumber">
        <xsl:value-of select="featuresTerms/description" />
      </xsl:attribute>
      <xsl:attribute name="PhoneUseType">PHN</xsl:attribute>
    </Phone>
  </xsl:template>

  <xsl:template match="hotelFeaturesTerms" mode="fax">
    <Phone>
      <xsl:attribute name="PhoneNumber">
        <xsl:value-of select="featuresTerms/description" />
      </xsl:attribute>
      <xsl:attribute name="PhoneUseType">FAX</xsl:attribute>
    </Phone>
  </xsl:template>

  <xsl:template match="featuresTerms" mode="location">
    <AreaInfo>
      <RefPoints>
        <xsl:for-each select="description">
          <RefPoint>
            <xsl:attribute name="Name">
              <xsl:value-of select="."/>
            </xsl:attribute>
          </RefPoint>
        </xsl:for-each>
      </RefPoints>
    </AreaInfo>
  </xsl:template>

  <xsl:template match="featuresTerms" mode="policy">
    <PolicyInfoCodes>
      <PolicyInfoCode>
        <Description>
          <xsl:for-each select="description">
            <Text>
              <xsl:value-of select="."/>
            </Text>
          </xsl:for-each>
        </Description>
      </PolicyInfoCode>
    </PolicyInfoCodes>
  </xsl:template>

  <xsl:template match="featuresTerms" mode="deposit">
    <RequiredPayment>
      <xsl:attribute name="PaymentCode">Deposit</xsl:attribute>
      <PaymentDescription>
        <xsl:for-each select="description">
          <Text>
            <xsl:value-of select="."/>
          </Text>
        </xsl:for-each>
      </PaymentDescription>
    </RequiredPayment>
  </xsl:template>

  <xsl:template match="featuresTerms" mode="guarantee">
    <RequiredPayment>
      <xsl:attribute name="PaymentCode">Guarantee</xsl:attribute>
      <PaymentDescription>
        <xsl:for-each select="description">
          <Text>
            <xsl:value-of select="."/>
          </Text>
        </xsl:for-each>
      </PaymentDescription>
    </RequiredPayment>
  </xsl:template>

  <xsl:template match="hotelFeaturesTerms" mode="facilities">
    <Descriptions>
      <Description>
        <xsl:attribute name="Name">Facilities</xsl:attribute>
        <xsl:for-each select="featuresTerms/description">
          <Text>
            <xsl:value-of select="."/>
          </Text>
        </xsl:for-each>
      </Description>
      <xsl:if test="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '4A']">
        <Description>
          <xsl:attribute name="Name">Features</xsl:attribute>
          <xsl:for-each select="featuresTerms/description">
            <Text>
              <xsl:value-of select="."/>
            </Text>
          </xsl:for-each>
        </Description>
      </xsl:if>
    </Descriptions>
  </xsl:template>

  <xsl:template match="hotelFeaturesTerms" mode="services">
    <Services>
      <Service>
        <xsl:attribute name="Code">Extra Charges</xsl:attribute>
        <Description>
          <xsl:for-each select="featuresTerms/description">
            <Text>
              <xsl:value-of select="."/>
            </Text>
          </xsl:for-each>
        </Description>
      </Service>
      <xsl:if test="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2A']">
        <Service>
          <xsl:attribute name="Code">Tax Charges</xsl:attribute>
          <Description>
            <Text>
              <xsl:value-of select="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2A']/featuresTerms/description"/>
            </Text>
          </Description>
        </Service>
      </xsl:if>
      <xsl:if test="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2B']">
        <Service>
          <xsl:attribute name="Code">Service Charges</xsl:attribute>
          <Description>
            <Text>
              <xsl:value-of select="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2B']/featuresTerms/description"/>
            </Text>
          </Description>
        </Service>
      </xsl:if>
      <xsl:if test="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2C']">
        <Service>
          <xsl:attribute name="Code">Meal Charges</xsl:attribute>
          <Description>
            <Text>
              <xsl:value-of select="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2C']/featuresTerms/description"/>
            </Text>
          </Description>
        </Service>
      </xsl:if>
      <xsl:if test="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2D']">
        <Service>
          <xsl:attribute name="Code">Additional Room Occupant Charges</xsl:attribute>
          <Description>
            <Text>
              <xsl:value-of select="preceding-sibling::hotelFeaturesTerms[featuresTerms/category = '2D']/featuresTerms/description"/>
            </Text>
          </Description>
        </Service>
      </xsl:if>
    </Services>
  </xsl:template>

  <xsl:template match="hotelFeaturesTerms" mode="address">
    <ContactInfos>
      <ContactInfo>
        <Addresses>
          <Address>
            <AddressLine>
              <xsl:value-of select="featuresTerms/description"/>
            </AddressLine>
            <xsl:if test="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1B']">
              <AddressLine>
                <xsl:value-of select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1B']/featuresTerms/description"/>
              </AddressLine>
            </xsl:if>
            <CityName>
              <xsl:value-of select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1C']/featuresTerms/description"/>
            </CityName>
            <xsl:if test="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1F']/featuresTerms/description!=''">
              <PostalCode>
                <xsl:value-of select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1F']/featuresTerms/description"/>
              </PostalCode>
            </xsl:if>
            <xsl:if test="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1D']">
              <StateProv>
                <xsl:attribute name="StateCode">
                  <xsl:value-of select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1D']/featuresTerms/description" />
                </xsl:attribute>
              </StateProv>
            </xsl:if>
            <CountryName>
              <xsl:value-of select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1E']/featuresTerms/description"/>
            </CountryName>
          </Address>
        </Addresses>
        <xsl:if test="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1G'] or following-sibling::hotelFeaturesTerms[featuresTerms/category = '1I']">
          <Phones>
            <xsl:apply-templates select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1G']" mode="phone"/>
            <xsl:apply-templates select="following-sibling::hotelFeaturesTerms[featuresTerms/category = '1I']" mode="fax"/>
          </Phones>
        </xsl:if>
      </ContactInfo>
    </ContactInfos>
  </xsl:template>

  <!--    **************************************************************   -->
  <xsl:template match="CAPI_Messages">
    <Errors>
      <Error Type="Amadeus">
        <xsl:attribute name="Code">
          <xsl:value-of select="ErrorCode" />
        </xsl:attribute>
        <xsl:value-of select="Text" />
      </Error>
    </Errors>
  </xsl:template>

</xsl:stylesheet>
