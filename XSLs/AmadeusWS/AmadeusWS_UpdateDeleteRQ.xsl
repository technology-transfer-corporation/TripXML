<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_UpdateDeleteRQ.xsl											  -->
  <!-- ================================================================== -->
  <!-- Date: 129 Jan 2015 - Rastko - fixed issue with seat passenger and segment association		    -->
  <!-- Date: 12 Jun 2014 - Rastko - added delete remarks by RPH number		    -->
  <!-- Date: 18 Apr 2014 - Rastko - added support for FormOfPayment input		    -->
  <!-- Date: 18 Apr 2014 - Rastko - added support for OTA_UpdateSessionedRQ		    -->
  <!-- Date: 16 Apr 2014 - Rastko - added support for deleting multiple commissions	  -->
  <!-- Date: 03 Feb 2014 - Rastko - added mapping data for special services		  -->
  <!-- Date: 29 Oct 2013 - Rastko - added delete special remark - tour code		  -->
  <!-- Date: 10 Sep 2012 - Rastko - added delete commission					  -->
  <!-- Date: 12 Jun 2011 - Rastko - mapped delete of form of payment			  -->
  <!-- Date: 12 May 2010 - Rastko													  -->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:template match="UpdateDelete">
    <UpdateDelete>
      <Cancel>
        <PNR_Cancel>
          <xsl:apply-templates select="OTA_UpdateRQ | OTA_UpdateSessionedRQ"/>
        </PNR_Cancel>
      </Cancel>
      <RF>
        <PNR_AddMultiElements>
          <pnrActions>
            <optionCode>0</optionCode>
          </pnrActions>
          <dataElementsMaster>
            <marker1/>
            <dataElementsIndiv>
              <elementManagementData>
                <segmentName>RF</segmentName>
              </elementManagementData>
              <freetextData>
                <freetextDetail>
                  <subjectQualifier>3</subjectQualifier>
                  <type>P22</type>
                </freetextDetail>
                <longFreetext>
                  <xsl:choose>
                    <xsl:when test="OTA_UpdateRQ/POS/Source/@AgentSine!=''">
                      <xsl:value-of select="OTA_UpdateRQ/POS/Source/@AgentSine"/>
                    </xsl:when>
                    <xsl:otherwise>TRIPXML</xsl:otherwise>
                  </xsl:choose>
                </longFreetext>
              </freetextData>
            </dataElementsIndiv>
          </dataElementsMaster>
        </PNR_AddMultiElements>
      </RF>
    </UpdateDelete>
  </xsl:template>

  <xsl:template match="OTA_UpdateRQ | OTA_UpdateSessionedRQ">
    <pnrActions>
      <optionCode>0</optionCode>
    </pnrActions>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='CustomerInfos']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='ReservationItems']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='CustomerInfo']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='SeatRequests']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='SpecialServiceRequests']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='Remarks']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='SpecialRemarks']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='TravelCost']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='FormOfPayment']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='Ticketing']"/>
    <xsl:apply-templates select="Position/Element[@Operation='delete' and @Child='AgencyCommission']"/>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='CustomerInfos']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="CustomerInfos/CustomerInfo"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="CustomerInfo">
    <xsl:param name="customerref">
      <xsl:value-of select="@RPH"/>
    </xsl:param>
    <element>
      <identifier>PT</identifier>
      <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/travellerInfo/elementManagementPassenger[lineNumber=$customerref]"/>
    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/travellerInfo/elementManagementPassenger">
    <number>
      <xsl:value-of select="reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='ReservationItems']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="ReservationItems/Item"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="Item">
    <xsl:param name="segmentref">
      <xsl:value-of select="@ItinSeqNumber"/>
    </xsl:param>
    <element>
      <identifier>ST</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Segment number: </xsl:text>
            <xsl:value-of select="@ItinSeqNumber"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary">
    <number>
      <xsl:value-of select="reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='CustomerInfo']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="CustomerInfo/Customer/Telephone"/>
      <xsl:apply-templates select="CustomerInfo/Customer/Email"/>
      <xsl:apply-templates select="CustomerInfo/Customer/Address[@UseType='Billing']"/>
      <xsl:apply-templates select="CustomerInfo/Customer/Address[@UseType='Mailing']"/>
      <xsl:apply-templates select="CustomerInfo/Customer/CustLoyalty"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="Telephone">
    <xsl:param name="phoneref">
      <xsl:value-of select="@PhoneNumber"/>
    </xsl:param>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$phoneref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$phoneref]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Telephone element: </xsl:text>
            <xsl:value-of select="@PhoneNumber"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="Email">
    <xsl:param name="emailref">
      <xsl:value-of select="."/>
    </xsl:param>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$emailref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$emailref]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Email element: </xsl:text>
            <xsl:value-of select="."/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="Address[@UseType='Billing']">
    <xsl:param name="billingaddressref">
      <xsl:value-of select="AddressLine"/>
    </xsl:param>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$billingaddressref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$billingaddressref]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Address: </xsl:text>
            <xsl:value-of select="AddressLine"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext">
    <number>
      <xsl:value-of select="../elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <xsl:template match="Address[@UseType='Mailing']">
    <xsl:param name="mailingaddressstreetref">
      <xsl:value-of select="StreetNmbr"/>
    </xsl:param>
    <xsl:param name="mailingaddresscityref">
      <xsl:value-of select="CityName"/>
    </xsl:param>
    <xsl:param name="mailingaddresszipref">
      <xsl:value-of select="PostalCode"/>
    </xsl:param>
    <xsl:param name="mailingaddressstateref">
      <xsl:value-of select="StateProv/@StateCode"/>
    </xsl:param>
    <xsl:param name="mailingaddresscountryref">
      <xsl:value-of select="CountryName/@Code"/>
    </xsl:param>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/structuredAddress[(address/option='A1' and address/optionText=$mailingaddressstreetref) and (address/option='ZP' and address/optionText=$mailingaddresszipref) and (address/option='CI' and address/optionText=$mailingaddresscityref) and (address/option='ST' and address/optionText=$mailingaddressstateref) and (address/option='CO' and address/optionText=$mailingaddresscountryref)]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/structuredAddress[(address/option='A1' and address/optionText=$mailingaddressstreetref) and (address/option='ZP' and address/optionText=$mailingaddresszipref) and (address/option='CI' and address/optionText=$mailingaddresscityref) and (address/option='ST' and address/optionText=$mailingaddressstateref) and (address/option='CO' and address/optionText=$mailingaddresscountryref)]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Address element: </xsl:text>
            <xsl:value-of select="StreetNmbr"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="CityName"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="PostalCode"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="StateProv/@StateCode"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="CountryName/@Code"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>

    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/structuredAddress">
    <number>
      <xsl:value-of select="../elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <xsl:template match="CustLoyalty">
    <xsl:param name="loyaltyref">
      <xsl:value-of select="@MembershipID"/>
    </xsl:param>
    <xsl:param name="programref">
      <xsl:value-of select="@ProgramID"/>
    </xsl:param>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo[frequentTraveler/company=$programref and frequentTraveler/membershipNumber=$loyaltyref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo[frequentTraveler/company=$programref and frequentTraveler/membershipNumber=$loyaltyref]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>CustLoyalty element: </xsl:text>
            <xsl:value-of select="@MembershipID"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="@ProgramID"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo">
    <number>
      <xsl:value-of select="../elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='SeatRequests']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="SeatRequests/SeatRequest"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="SeatRequest">
    <xsl:param name="seatref">
      <xsl:value-of select="@SeatPreference"/>
    </xsl:param>
    <xsl:param name="seatnum">
      <xsl:value-of select="@SeatNumber"/>
    </xsl:param>
    <xsl:param name="segmentref">
      <xsl:value-of select="@FlightRefNumberRPHList"/>
    </xsl:param>
    <xsl:param name="paxref">
      <xsl:value-of select="@TravelerRefNumberRPHList"/>
    </xsl:param>
    <xsl:variable name="ST">
      <xsl:value-of select="//PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo[elementManagementItinerary/lineNumber=$segmentref]/elementManagementItinerary/reference/number"/>
    </xsl:variable>
    <xsl:variable name="PT">
      <xsl:value-of select="//PNR_RetrieveByRecLocReply/travellerInfo[elementManagementPassenger/lineNumber=$paxref]/elementManagementPassenger/reference/number"/>
    </xsl:variable>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/freeText=$seatref and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$ST and referenceForDataElement/reference/qualifier='PT' and referenceForDataElement/reference/number=$PT]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/freeText=$seatref and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$ST and referenceForDataElement/reference/qualifier='PT' and referenceForDataElement/reference/number=$PT]"/>
        </xsl:when>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb/data=$seatnum and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$ST and referenceForDataElement/reference/qualifier='PT' and referenceForDataElement/reference/number=$PT]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssrb/data=$seatnum and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$ST and referenceForDataElement/reference/qualifier='PT' and referenceForDataElement/reference/number=$PT]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Seat element: </xsl:text>
            <xsl:value-of select="@SeatPreference"/>
            <xsl:text> </xsl:text>
            <xsl:value-of select="@FlightRefNumberRPHList"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv">
    <number>
      <xsl:value-of select="elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='SpecialServiceRequests']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="SpecialServiceRequests/SpecialServiceRequest"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="SpecialServiceRequest">
    <xsl:variable name="ssrref">
      <xsl:value-of select="@SSRCode"/>
    </xsl:variable>
    <xsl:variable name="ssrtxt">
      <xsl:value-of select="Text"/>
    </xsl:variable>
    <xsl:variable name="ssrair">
      <xsl:value-of select="Airline/@Code"/>
    </xsl:variable>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type=$ssrref][serviceRequest/ssr/freeText=$ssrtxt][serviceRequest/ssr/companyId=$ssrair]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type=$ssrref][serviceRequest/ssr/freeText=$ssrtxt][serviceRequest/ssr/companyId=$ssrair]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>SSR element: </xsl:text>
            <xsl:value-of select="@SSRCode"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv">
    <number>
      <xsl:value-of select="elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='Remarks']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="Remarks/Remark"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="Element[@Child='SpecialRemarks']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="SpecialRemarks/SpecialRemark"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="Remark">
    <xsl:param name="remarkref">
      <xsl:value-of select="."/>
    </xsl:param>
    <xsl:variable name="rph">
      <xsl:value-of select="@RPH"/>
    </xsl:variable>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='RM']/elementManagementData[lineNumber=$rph]">
          <number>
            <xsl:value-of select="$rph"/>
          </number>
        </xsl:when>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$remarkref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$remarkref]"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Remark element: </xsl:text>
            <xsl:value-of select="."/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <xsl:template match="SpecialRemark">
    <xsl:param name="remarkref">
      <xsl:value-of select="Text"/>
    </xsl:param>
    <xsl:variable name="rph">
      <xsl:value-of select="@RPH"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="@RemarkType='TourCode'">
        <element>
          <identifier>OT</identifier>
          <xsl:choose>
            <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FT']/elementManagementData[lineNumber=$rph]">
              <number>
                <xsl:value-of select="$rph"/>
              </number>
            </xsl:when>
            <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FT']/otherDataFreetext[longFreetext=$remarkref]">
              <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FT']/otherDataFreetext[longFreetext=$remarkref]"/>
            </xsl:when>
            <xsl:otherwise>
              <Error>
                <xsl:text>Remark element: </xsl:text>
                <xsl:value-of select="."/>
                <xsl:text> does not exist in the PNR</xsl:text>
              </Error>
            </xsl:otherwise>
          </xsl:choose>
        </element>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks">
    <number>
      <xsl:value-of select="../elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='TravelCost']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="TravelCost/FormOfPayment"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="Element[@Child='FormOfPayment']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="FormOfPayment"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="FormOfPayment">
    <xsl:param name="fopref">
      <xsl:value-of select="@RPH"/>
    </xsl:param>
    <xsl:variable name="fop">
      <xsl:value-of select="concat('CC',PaymentCard/@CardCode,PaymentCard/@CardNumber,'/',PaymentCard/@ExpireDate)"/>
    </xsl:variable>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/elementManagementData[lineNumber=$fopref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/elementManagementData[lineNumber=$fopref]"/>
        </xsl:when>
        <xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$fop]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[otherDataFreetext/longFreetext=$fop]/elementManagementData"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>FormOfPayment element: </xsl:text>
            <xsl:value-of select="Text"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

  <!-- *********************************************************************************************************  -->
  <!--
	<xsl:template match="Element[@Child='Remarks']">
		<cancelElements>
			<entryType>E</entryType>
			<xsl:apply-templates select="Remarks/Remark"/>
		</cancelElements>
	</xsl:template>

	<xsl:template match="Remark">
	<xsl:param name="remarkref"><xsl:value-of select="."/></xsl:param>
		<element>
			<identifier>OT</identifier>
			<xsl:choose>
				<xsl:when test="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$remarkref]">
					<xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$remarkref]"/>
				</xsl:when>
				<xsl:otherwise>
					<Error>
						<xsl:text>Remark element: </xsl:text>
						<xsl:value-of select="."/>
						<xsl:text> does not exist in the PNR</xsl:text>
					</Error>
				</xsl:otherwise>
			</xsl:choose>
		</element>
	</xsl:template>

	
	<xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks">
		<number>
			<xsl:value-of select="../elementManagementData/reference/number"/>
		</number>
	</xsl:template>
		-->

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext">
    <number>
      <xsl:value-of select="../elementManagementData/reference/number"/>
    </number>
  </xsl:template>

  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/elementManagementData">
    <number>
      <xsl:value-of select="reference/number"/>
    </number>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='Ticketing']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="Ticketing"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="Ticketing">
    <xsl:param name="ticketref">
      <xsl:value-of select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/ticketElement/ticket/indicator "/>
    </xsl:param>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="contains($ticketref,'OK' ) or contains($ticketref,'TL' )">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/ticketElement"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Ticketing element: </xsl:text>
            <xsl:value-of select="Text"/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>
  <xsl:template match="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/ticketElement">
    <xsl:if test="not(../referenceForDataElement)">
      <number>
        <xsl:value-of select="../elementManagementData/reference/number"/>
      </number>
    </xsl:if>
  </xsl:template>

  <!-- *********************************************************************************************************  -->

  <xsl:template match="Element[@Child='AgencyCommission']">
    <cancelElements>
      <entryType>E</entryType>
      <xsl:apply-templates select="AgencyCommission"/>
    </cancelElements>
  </xsl:template>

  <xsl:template match="AgencyCommission">
    <xsl:variable name="commref">
      <xsl:value-of select="@Percent"/>
    </xsl:variable>
    <xsl:variable name="commrefINF">
      <xsl:value-of select="@InfantPercent"/>
    </xsl:variable>
    <element>
      <identifier>OT</identifier>
      <xsl:choose>
        <xsl:when test="$commref!='' and //PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'][contains(otherDataFreetext/longFreetext,$commref)]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'][contains(otherDataFreetext/longFreetext,$commref)]/elementManagementData"/>
        </xsl:when>
        <xsl:when test="$commrefINF!='' and //PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'][contains(otherDataFreetext/longFreetext,$commrefINF)]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM'][contains(otherDataFreetext/longFreetext,$commrefINF)]/elementManagementData"/>
        </xsl:when>
        <xsl:otherwise>
          <Error>
            <xsl:text>Commission element: </xsl:text>
            <xsl:value-of select="."/>
            <xsl:text> does not exist in the PNR</xsl:text>
          </Error>
        </xsl:otherwise>
      </xsl:choose>
    </element>
  </xsl:template>

</xsl:stylesheet>
