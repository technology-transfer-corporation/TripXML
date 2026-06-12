<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_ProfileReadRS.xsl 													-->
	<!-- ================================================================== -->
	<!-- Date: 02 Aug 2016 - Rastko - added support for Title and email parsing			-->
	<!-- Date: 29 Jul 2016 - Rastko - added support for errors							-->
	<!-- Date: 28 Jul 2016 - Rastko - added company record locator					-->
	<!-- Date: 11 Jul 2016 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_ProfileReadRS Version="1.0">
      <xsl:choose>
        <xsl:when test="boolean(Sabre_OTA_ProfileReadRS/ResponseMessage/Errors)">
          <xsl:apply-templates select="Sabre_OTA_ProfileReadRS/ResponseMessage/Errors" />
        </xsl:when>
        <xsl:when test="boolean(Sabre_OTA_ProfileReadRS)">
          <xsl:apply-templates select="Sabre_OTA_ProfileReadRS" />
        </xsl:when>
      </xsl:choose>
      
		</OTA_ProfileReadRS>
	</xsl:template>
	<!-- -->
	<xsl:template match="Errors">
    <xsl:attribute name="Version">3.14</xsl:attribute>
    <xsl:attribute name="Status">Error</xsl:attribute>
		<Errors>
			<xsl:for-each select="ErrorMessage">
				<Error>
					<xsl:attribute name="Type">Sabre</xsl:attribute>
          <xsl:value-of select="." />
				</Error>
			</xsl:for-each>
		</Errors>
	</xsl:template>
	<!-- -->
	<xsl:template match="Sabre_OTA_ProfileReadRS">
    <Success />
    <Profiles>
      <ProfileInfo>
        <UniqueID Type="21">
          <xsl:attribute name="ID">
            <xsl:value-of select="Profile/TPA_Identity/@UniqueID"/>
          </xsl:attribute>
        </UniqueID>
        <Profile ProfileType="Customer">
          <Customer>
            <xsl:if test="Profile/Traveler/Customer[@BirthDate != '']">
              <xsl:attribute name="BirthDate">
                <xsl:value-of select="Profile/Traveler/Customer/@BirthDate"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="Profile/Traveler/Customer[@GenderCode!='']">
            <xsl:attribute name="Gender">
              <xsl:choose>
                <xsl:when test="Profile/Traveler/Customer[@GenderCode='M']">Male</xsl:when>
                <xsl:otherwise>Female</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            </xsl:if>
            <PersonName>
              <GivenName>
                <xsl:value-of select="Profile/Traveler/Customer/PersonName/GivenName" />
              </GivenName>
              <Surname>
                <xsl:value-of select="Profile/Traveler/Customer/PersonName/SurName" />
              </Surname>
              <MiddleName>
                <xsl:value-of select="Profile/Traveler/Customer/PersonName/MiddleName" />
              </MiddleName>
              <NameTitle>
                <xsl:value-of select="Profile/Traveler/Customer/PersonName/NamePrefix" />
              </NameTitle>
            </PersonName>
            
            <xsl:if test="Profile/Traveler/Customer/Email/@EmailAddress!=''">
              <Email>
                <xsl:value-of select="Profile/Traveler/Customer/Email/@EmailAddress"/>
              </Email>
            </xsl:if>

            <xsl:if test="Profile/Traveler/Customer/EmergencyContactPerson/Telephone/FullPhoneNumber!=''">
              <Telephone RPH="1" PhoneLocationType="8" >
                <xsl:attribute name="PhoneNumber">
                  <xsl:value-of select="Profile/Traveler/Customer/EmergencyContactPerson/Telephone/FullPhoneNumber"/>
                </xsl:attribute>
              </Telephone>
            </xsl:if>
            
            
            <xsl:if test="Profile/Traveler/Customer/Telephone[@LocationTypeCode='HOM']">
              <xsl:choose>
                <xsl:when test="contains(Profile/Traveler/Customer/Telephone[@LocationTypeCode='HOM']/FullPhoneNumber, '-')">
                  <Telephone PhoneLocationType="6">
                    <xsl:attribute name="AreaCityCode">
                      <xsl:value-of select="substring-before(Profile/Traveler/Customer/Telephone[@LocationTypeCode='HOM']/FullPhoneNumber, '-')"/>
                    </xsl:attribute>
                    <xsl:attribute name="PhoneNumber">
                      <xsl:value-of select="substring-after(Profile/Traveler/Customer/Telephone[@LocationTypeCode='HOM']/FullPhoneNumber,'-')"/>
                    </xsl:attribute>
                  </Telephone>
                </xsl:when>
                <xsl:otherwise>
                  <Telephone PhoneLocationType="6">
                    <xsl:attribute name="PhoneNumber">
                      <xsl:value-of select="Profile/Traveler/Customer/Telephone[@LocationTypeCode='HOM']/FullPhoneNumber"/>
                    </xsl:attribute>
                  </Telephone>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
            
            <xsl:if test="Profile/Traveler/Customer/Telephone[@LocationTypeCode='BUS']">
              <xsl:choose>
                <xsl:when test="contains(Profile/Traveler/Customer/Telephone[@LocationTypeCode='BUS']/FullPhoneNumber, '-')">
                  <Telephone PhoneLocationType="7">
                    <xsl:attribute name="AreaCityCode">
                      <xsl:value-of select="substring-before(Profile/Traveler/Customer/Telephone[@LocationTypeCode='BUS']/FullPhoneNumber, '-')"/>
                    </xsl:attribute>
                    <xsl:attribute name="PhoneNumber">
                      <xsl:value-of select="substring-after(Profile/Traveler/Customer/Telephone[@LocationTypeCode='BUS']/FullPhoneNumber,'-')"/>
                    </xsl:attribute>
                  </Telephone>
                </xsl:when>
                <xsl:otherwise>
                  <Telephone PhoneLocationType="7">
                    <xsl:attribute name="PhoneNumber">
                      <xsl:value-of select="Profile/Traveler/Customer/Telephone[@LocationTypeCode='BUS']/FullPhoneNumber"/>
                    </xsl:attribute>
                  </Telephone>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
            
            <xsl:if test="Profile/Traveler/Customer/Address[@LocationTypeCode='HOM']">
              <Address Type="1">
                  <AddressLine>
                    <xsl:value-of select="Profile/Traveler/Customer/Address[@LocationTypeCode='HOM']/AddressLine"/>
                  </AddressLine>
              </Address>
            </xsl:if>
            
            <xsl:if test="Profile/Traveler/Customer/Address[@LocationTypeCode='BUS']">
              <Address Type="2">
                <AddressLine>
                  <xsl:value-of select="Profile/Traveler/Customer/Address[@LocationTypeCode='BUS']/AddressLine"/>
                </AddressLine>
              </Address>
            </xsl:if>
            
            <xsl:if test="Profile/Traveler/Customer/@NationalityCode != ''">
              <CitizenCountryName>
                <xsl:attribute name="Code">
                  <xsl:value-of select="Profile/Traveler/Customer/@NationalityCode"/>
                </xsl:attribute>
              </CitizenCountryName>
            </xsl:if>
            
            <xsl:if test="Profile/Traveler/Customer/EmergencyContactPerson!=''">
            <ContactPerson EmergencyFlag="true">
              <xsl:if test="Profile/Traveler/Customer/EmergencyContactPerson/GivenName!=''">
                <PersonName>
                  <GivenName>
                    <xsl:value-of select="Profile/Traveler/Customer/EmergencyContactPerson/GivenName"/>
                  </GivenName>
                </PersonName>
              </xsl:if>
              <xsl:if test="Profile/Traveler/Customer/EmergencyContactPerson/Telephone/FullPhoneNumber!=''">
                <Telephone RPH="1"></Telephone>
              </xsl:if>
              <xsl:if test="Profile/Traveler/Customer/EmergencyContactPerson/Email/@EmailAddress!=''">
                <Email>
                  <xsl:value-of select="Profile/Traveler/Customer/EmergencyContactPerson/Email/@EmailAddress"/>
                </Email>
              </xsl:if>
            </ContactPerson>
            </xsl:if>
            
            <xsl:if test="Profile/Traveler/Customer/Document[@DocID != '']">
              <Document DocType="2">
                <xsl:attribute name="DocIssueCountry">
                  <xsl:value-of select="Profile/Traveler/Customer/Document/@DocIssueCountryCode"/>
                </xsl:attribute>
                <xsl:attribute name="DocID">
                  <xsl:value-of select="Profile/Traveler/Customer/Document/@DocID"/>
                </xsl:attribute>
                <xsl:attribute name="ExpireDate">
                  <xsl:value-of select="Profile/Traveler/Customer/Document/@ExpireDate"/>
                </xsl:attribute>
                <xsl:attribute name="EffectiveDate">
                  <xsl:value-of select="Profile/Traveler/Customer/Document/@EffectiveDate"/>
                </xsl:attribute>
              </Document>
            </xsl:if>
            <EmployeeInfo>
              <xsl:attribute name="EmployeeId">
                <xsl:value-of select="Profile/Traveler/Customer/EmploymentInfo/EmployeeInfo/@EmployeeId"/>
              </xsl:attribute>
              <xsl:attribute name="EmployeeStatus">
                <xsl:choose>
                  <xsl:when test="Profile/TPA_Identity[@ProfileStatusCode='AC']">1</xsl:when>
                  <xsl:otherwise>3</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="EmployeeTitle">
                <xsl:value-of select="Profile/Traveler/Customer/PersonName/NamePrefix" />
              </xsl:attribute>
            </EmployeeInfo>
            <xsl:for-each select="Profile/Traveler/Customer/CustLoyalty">
              <CustLoyalty>
                <xsl:attribute name="OrderSequenceNo">
                  <xsl:value-of select="position()"/>
                </xsl:attribute>
                <xsl:attribute name="TravelSector">
                  <xsl:choose>
                    <xsl:when test="@VendorTypeCode='AL'">1</xsl:when>
                    <xsl:when test="@VendorTypeCode='CR'">2</xsl:when>
                    <xsl:otherwise>3</xsl:otherwise>
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
            
          </Customer>
        </Profile>
      </ProfileInfo>
    </Profiles>
	</xsl:template>
	<!-- -->

</xsl:stylesheet>
