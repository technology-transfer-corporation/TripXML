<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Sabre_ProfileCreateRQ.xsl                													-->
  <!-- ================================================================== -->
  <!-- Date: 10 May 2018 - Miroslav - new file											     	-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <Sabre_OTA_ProfileCreateRQ xmlns="http://www.sabre.com/eps/schemas" Version="6.40">

      <xsl:variable name="domainId">
        <xsl:choose>
          <xsl:when test="OTA_ProfileCreateRQ/UniqueID/@ID_Context!=''">
            <xsl:value-of select="OTA_ProfileCreateRQ/UniqueID/@ID_Context"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="OTA_ProfileCreateRQ/POS/Source/@PseudoCityCode"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <Profile  CreateDateTime="DateTimeNowToReplace" UpdateDateTime="DateTimeNowToReplace">
        <TPA_Identity UniqueID="*" ClientCode="TN" ClientContextCode="TMP" ProfileName="TestProfile" ProfileTypeCode="TVL">
          <xsl:attribute name="DomainID">
            <xsl:value-of select="$domainId"/>
          </xsl:attribute>
          <xsl:attribute name="ProfileStatusCode">
            <xsl:choose>
              <xsl:when test="OTA_ProfileCreateRQ/Profile/Customer/EmployeeInfo[@EmployeeStatus='1']">AC</xsl:when>
              <xsl:otherwise>IN</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </TPA_Identity>
        <Traveler>
          <Customer>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/@BirthDate != ''">
              <xsl:attribute name="BirthDate">
                <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/@BirthDate"/>
              </xsl:attribute>
            </xsl:if>

            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/@Gender != ''">
              <xsl:attribute name="GenderCode">
                <xsl:choose>
                  <xsl:when test="OTA_ProfileCreateRQ/Profile/Customer[@Gender='Male']">M</xsl:when>
                  <xsl:otherwise>F</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/CitizenCountryName/@Code != ''">
              <xsl:attribute name="NationalityCode">
                <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/CitizenCountryName/@Code"/>
              </xsl:attribute>
            </xsl:if>
            <PersonName>
              <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/PersonName/NameTitle!=''">
                <NamePrefix>
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/PersonName/NameTitle"/>
                </NamePrefix>
              </xsl:if>
              <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/PersonName/GivenName!=''">
                <GivenName>
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/PersonName/GivenName"/>
                </GivenName>
              </xsl:if>
              <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/PersonName/MiddleName!=''">
                <MiddleName>
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/PersonName/MiddleName"/>
                </MiddleName>
              </xsl:if>
              <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/PersonName/Surname!=''">
                <SurName>
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/PersonName/Surname"/>
                </SurName>
              </xsl:if>
            </PersonName>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='6']">
              <Telephone LocationTypeCode="HOM">
                <FullPhoneNumber >
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='6']/@AreaCityCode"/>
                  <xsl:text>-</xsl:text>
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='6']/@PhoneNumber"/>
                </FullPhoneNumber >
              </Telephone>
            </xsl:if>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='7']">
              <Telephone LocationTypeCode="BUS">
                <FullPhoneNumber >
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='7']/@AreaCityCode"/>
                  <xsl:text>-</xsl:text>
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='7']/@PhoneNumber"/>
                </FullPhoneNumber >
              </Telephone>
            </xsl:if>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Email!=''">
              <Email>
                <xsl:attribute name="EmailAddress">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Email"/>
                </xsl:attribute>
              </Email>
            </xsl:if>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Address[@Type='1']">
              <Address LocationTypeCode="HOM">
                <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Address[@Type='1']">
                  <AddressLine>
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Address[@Type='1']/AddressLine"/>
                  </AddressLine>
                </xsl:if>
                <!--<CountryCode>
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/CitizenCountryName/@Code"/>
                  </CountryCode>-->
              </Address>
            </xsl:if>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Address[@Type='2']">
              <Address LocationTypeCode="BUS">
                <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Address[@Type='2']">
                  <AddressLine>
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Address[@Type='2']/AddressLine"/>
                  </AddressLine>
                </xsl:if>
                <!--<CountryCode>
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/CitizenCountryName/@Code"/>
                  </CountryCode >-->
              </Address>
            </xsl:if>
            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/ContactPerson/PersonName/GivenName!='' or OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='8'] or OTA_ProfileCreateRQ/Profile/Customer/ContactPerson/Email!=''">
              <EmergencyContactPerson>
                <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/ContactPerson/PersonName/GivenName!=''">
                  <GivenName>
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/ContactPerson/PersonName/GivenName"/>
                  </GivenName>
                </xsl:if>
                <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='8']">
                  <Telephone>
                    <FullPhoneNumber >
                      <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Telephone[@PhoneLocationType='8']/@PhoneNumber"/>
                    </FullPhoneNumber >
                  </Telephone>
                </xsl:if>
                <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/ContactPerson/Email!=''">
                  <Email>
                    <xsl:attribute name="EmailAddress">
                      <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/ContactPerson/Email"/>
                    </xsl:attribute>
                  </Email>
                </xsl:if>
              </EmergencyContactPerson>
            </xsl:if>

            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Document[@DocID != '']">
              <Document OrderSequenceNo="1" DocTypeCode="PSPT">
                <xsl:attribute name="BirthCountryCode">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/CitizenCountryName/@Code"/>
                </xsl:attribute>
                <xsl:attribute name="DocIssueCountryCode">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Document/@DocIssueCountry"/>
                </xsl:attribute>
                <xsl:attribute name="DocID">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Document/@DocID"/>
                </xsl:attribute>
                <xsl:attribute name="BirthDate">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/@BirthDate"/>
                </xsl:attribute>
                <xsl:attribute name="ExpireDate">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Document/@ExpireDate"/>
                </xsl:attribute>
                <xsl:attribute name="EffectiveDate">
                  <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Document/@EffectiveDate"/>
                </xsl:attribute>
              </Document>
            </xsl:if>
            <xsl:for-each select="OTA_ProfileCreateRQ/Profile/Customer/CustLoyalty">
              <CustLoyalty>
                <xsl:attribute name="OrderSequenceNo">
                  <xsl:value-of select="position()"/>
                </xsl:attribute>
                <xsl:attribute name="VendorTypeCode">
                  <xsl:choose>
                    <xsl:when test="@TravelSector='1'">AL</xsl:when>
                    <xsl:when test="@TravelSector='2'">CR</xsl:when>
                    <xsl:otherwise>HL</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="VendorCode">
                  <xsl:value-of select="@VendorCode"/>
                </xsl:attribute>
                <xsl:attribute name="MembershipID">
                  <xsl:value-of select="@MembershipID"/>
                </xsl:attribute>
              </CustLoyalty>
            </xsl:for-each>


            <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/EmployeeInfo[@EmployeeId!='']">
              <EmploymentInfo>
                <EmployeeInfo>
                  <xsl:attribute name="EmployeeId">
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/EmployeeInfo/@EmployeeId"/>
                  </xsl:attribute>
                </EmployeeInfo>
              </EmploymentInfo>
            </xsl:if>
          </Customer>
          <xsl:if test="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/MealPref[@MealType != '']">
            <!--or OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/AirportOriginPref[@LocationCode != '']-->
            <PrefCollections>
              <AirlinePref TripTypeCode="AZ">
                <!--<xsl:if test="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/AirportOriginPref[@LocationCode != '']">
                    <AirportOriginPref>
                      <xsl:attribute name="LocationCode">
                        <xsl:value-of select="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/AirportOriginPref/@LocationCode"/>
                      </xsl:attribute>
                    </AirportOriginPref>
                  </xsl:if>-->
                <xsl:if test="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/MealPref[@MealType != '']">
                  <AirlineMealPref>
                    <MealInfo>
                      <xsl:attribute name="MealTypeCode">
                        <xsl:value-of select="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/MealPref/@MealType"/>
                      </xsl:attribute>
                    </MealInfo>
                  </AirlineMealPref>
                </xsl:if>
              </AirlinePref>
            </PrefCollections>
          </xsl:if>

          <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Document[@DocID != ''] or OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/MealPref/@MealType!=''">
            <TPA_Extensions>
              <xsl:if test="OTA_ProfileCreateRQ/Profile/Customer/Document[@DocID != '']">
                <SSR SSRCode="PSPT">
                  <xsl:attribute name="Text">
                    P/<xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Document/@DocIssueCountry"/>/<xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/Document/@DocID"/>/<xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/CitizenCountryName/@Code"/>/<xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/@BirthDate,9,2)"/>
                    <xsl:call-template name="month">
                      <xsl:with-param name="month">
                        <xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/@BirthDate,6,2)"/>
                      </xsl:with-param>
                    </xsl:call-template>
                    <xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/@BirthDate,3,2)"/>/<xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/@Gender,1,1)"/>/<xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/Document/@ExpireDate,9,2)"/><xsl:call-template name="month">
                      <xsl:with-param name="month">
                        <xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/Document/@ExpireDate,6,2)"/>
                      </xsl:with-param>
                    </xsl:call-template><xsl:value-of select="substring(OTA_ProfileCreateRQ/Profile/Customer/Document/@ExpireDate,3,2)"/>/<xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/PersonName/Surname"/>/<xsl:value-of select="OTA_ProfileCreateRQ/Profile/Customer/PersonName/GivenName"/>
                  </xsl:attribute>
                </SSR>
              </xsl:if>
              <xsl:if test="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/MealPref/@MealType!=''">
                <SSR>
                  <xsl:attribute name="SSRCode">
                    <xsl:value-of select="OTA_ProfileCreateRQ/Profile/PrefCollections/PrefCollection/AirlinePref/MealPref/@MealType"/>
                  </xsl:attribute>
                </SSR>
              </xsl:if>
            </TPA_Extensions>
          </xsl:if>

        </Traveler>
      </Profile>
    </Sabre_OTA_ProfileCreateRQ>
  </xsl:template>
  <xsl:template name="month">
    <xsl:param name="month"/>
    <xsl:choose>
      <xsl:when test="$month = '01'">JAN</xsl:when>
      <xsl:when test="$month = '02'">FEB</xsl:when>
      <xsl:when test="$month = '03'">MAR</xsl:when>
      <xsl:when test="$month = '04'">APR</xsl:when>
      <xsl:when test="$month = '05'">MAY</xsl:when>
      <xsl:when test="$month = '06'">JUN</xsl:when>
      <xsl:when test="$month = '07'">JUL</xsl:when>
      <xsl:when test="$month = '08'">AUG</xsl:when>
      <xsl:when test="$month = '09'">SEP</xsl:when>
      <xsl:when test="$month = '10'">OCT</xsl:when>
      <xsl:when test="$month = '11'">NOV</xsl:when>
      <xsl:when test="$month = '12'">DEC</xsl:when>
    </xsl:choose>
  </xsl:template>


</xsl:stylesheet>
