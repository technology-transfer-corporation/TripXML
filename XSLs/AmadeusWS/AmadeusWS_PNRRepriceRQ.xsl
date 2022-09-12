<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- 
 ================================================================== 
 AmadeusWS_PNRRepriceRQ.xsl 												
 ================================================================== 
 Date: 21 Mar 2010 - Rastko														
 ================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNR_RetrieveByRecLoc>
			<sbrRecLoc>
				<reservation>
					<controlNumber>
						<xsl:value-of select="OTA_PNRRepriceRQ/UniqueID/@ID" />
					</controlNumber>
				</reservation>
			</sbrRecLoc>
		</PNR_RetrieveByRecLoc>

    <xsl:if test="OTA_PNRRepriceRQ/StoredFare/@FareQualifier = 'EXC' or OTA_PNRRepriceRQ/StoredFare/@FareQualifier = 'EX' or OTA_PNRRepriceRQ/StoredFare/@FareQualifier = 'EXL'">
      <xsl:apply-templates select="OTA_PNRRepriceRQ/StoredFare[@FareQualifier = 'EXC' or @FareQualifier = 'EX' or @FareQualifier = 'EXL']" mode="EX"/>
    </xsl:if>
	</xsl:template>

  <xsl:template match="StoredFare" mode="EX">
    <xsl:variable name="pax">
      <xsl:value-of select="PassengerType/@Quantity"/>
    </xsl:variable>
    <Ticket_RepricePNRWithBookingClass>
      <exchangeInformationGroup>
        <transactionIdentifier>
          <itemNumberDetails>
            <number>1</number>
          </itemNumberDetails>
        </transactionIdentifier>
        <documentInfoGroup>
          <paperticketDetailsLastCoupon>
            <documentDetails>
              <number>1348601654601</number>
              <type>ET</type>
            </documentDetails>
          </paperticketDetailsLastCoupon>
        </documentInfoGroup>
      </exchangeInformationGroup>
      <pricingOption>
        <pricingOptionKey>
          <!-- RP = Published Fare; RN = Negociated Fare; RU = Unifare -->
          <pricingOptionKey>RP</pricingOptionKey>
        </pricingOptionKey>
      </pricingOption>
      <pricingOption>
        <pricingOptionKey>
          <!-- VC = Validating Carrier -->
          <pricingOptionKey>VC</pricingOptionKey>
        </pricingOptionKey>
        <carrierInformation>
          <companyIdentification>
            <!-- AV = Airline Code -->
            <otherCompany>AV</otherCompany>
          </companyIdentification>
        </carrierInformation>
      </pricingOption>
      <pricingOption>
        <pricingOptionKey>
          <!-- SEL = Passenger/Segment/Line/TST selection -->
          <pricingOptionKey>SEL</pricingOptionKey>
        </pricingOptionKey>
        <paxSegTstReference>
          <referenceDetails>
            <!-- P = Passenger -->
            <type>P</type>
            <value>2</value>
          </referenceDetails>
          <referenceDetails>
            <!-- E = Exchange Element. could be S = Segment or T = TST -->
            <type>E</type>
            <value>1</value>
          </referenceDetails>
          <referenceDetails>
            <type>S</type>
            <value>1</value>
          </referenceDetails>
        </paxSegTstReference>
      </pricingOption>
    </Ticket_RepricePNRWithBookingClass>
  </xsl:template>
</xsl:stylesheet>
