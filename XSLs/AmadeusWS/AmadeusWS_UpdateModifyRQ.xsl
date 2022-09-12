<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- AmadeusWS_PNRUpdateRQ.xsl												  -->
  <!-- ================================================================== -->
  <!-- Date: 22 Jan 2015 - Rastko - corrected issue with deleting FOP from PNR		    -->
  <!-- Date: 28 Jul 2014 - Rastko - added passenger association support to FOP		    -->
  <!-- Date: 01 Jul 2014 - Rastko - changed mapping of CC FOP	 to use PNR_addmultielements    -->
  <!-- Date: 29 May 2014 - Rastko - changed mapping of CC FOP				    -->
  <!-- Date: 21 May 2014 - Rastko - added mapping of passenger association for FOP	    -->
  <!-- Date: 23 Apr 2014 - Rastko - corrected FOP amount calculations			    -->
  <!-- Date: 18 Apr 2014 - Rastko - added support for OTA_UpdateSessionedRQ		    -->
  <!-- Date: 16 Apr 2014 - Rastko - added support for INF commission			    -->
  <!-- Date: 26 Jan 2014 - Rastko - added change segment (delete and add segment)	    -->
  <!-- Date: 19 Sep 2013 - Rastko - corrected approval number in FOP CC		    -->
  <!-- Date: 13 Mar 2013 - Rastko - insert FOP if does not exist in PNR			    -->
  <!-- Date: 13 Mar 2013 - Rastko - insert commissioni if does not exist in PNR		    -->
  <!-- Date: 18 Feb 2013 - Rastko - added modify FOP with CHECK option		    -->
  <!-- Date: 16 Oct 2012 - Rastko - corrected FOP mapping in PAX or INF case		    -->
  <!-- Date: 03 Aug 2012 - Shashin - Change traveller names 					    -->
  <!-- Date: 12 Jul 2012 - Rastko - corrected FOP order estimation in cryptic entry	    -->
  <!-- Date: 27 Jun 2012 - Rastko - corrected decimal point calculation FOP		    -->
  <!-- Date: 05 Jun 2012 - Rastko - added modify ticketing						    -->
  <!-- Date: 27 Mar 2012 - Shashin - added modify SSR							    -->
  <!-- Date: 08 Mar 2012 - Rastko - adjusted FOP MSCC process				    -->
  <!-- Date: 04 Mar 2012 - Rastko - corrected mapping of amount in FOP			    -->
  <!-- Date: 04 Mar 2012 - Rastko - always delete FOP when only one in modify request	    -->
  <!-- Date: 22 Feb 2012 - Rastko - added change for multiple FOPs			    -->
  <!-- Date: 01 Feb 2012 - Rastko - if only one commission in input, change first one in PNR    -->
  <!-- Date: 01 Feb 2012 - Rastko - agency commission modification                 		       -->
  <!-- Date: 18 Jan 2012 - Shashin - EMD modification                                      		       -->
  <!-- Date: 12 May 2010 - Rastko													  -->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="username" select="UpdateModify/OTA_UpdateRQ/POS/TPA_Extensions/Provider/Userid | UpdateModify/OTA_UpdateSessionedRQ/POS/TPA_Extensions/Provider/Userid"/>
  <xsl:variable name="system" select="UpdateModify/OTA_UpdateRQ/POS/TPA_Extensions/Provider/System | UpdateModify/OTA_UpdateSessionedRQ/POS/TPA_Extensions/Provider/System"/>
  <xsl:template match="/">
    <ChangeElement>
      <xsl:apply-templates select="UpdateModify/OTA_UpdateRQ | UpdateModify/OTA_UpdateSessionedRQ"/>
    </ChangeElement>
  </xsl:template>
  <xsl:template match="OTA_UpdateRQ | OTA_UpdateSessionedRQ">
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='CustomerInfos']"/>
    <xsl:apply-templates select="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/ReservationItems']" mode="xlseg"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='CustomerInfo']"/>
    <!--xsl:apply-templates select="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SeatRequests']"/>
		<xsl:apply-templates select="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SpecialServiceRequests']"/-->
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='Remarks']"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='SpecialRemarks']"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='AgencyCommission']"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='FormOfPayment']"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='ReservationItems' and Element/ReservationItems/Item/Air[@Status='RR']]"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='SpecialServiceRequests']"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='Ticketing']"/>
    <xsl:apply-templates select="Position[Element/@Operation='modify' and Element/@Child='Telephone']"/>
    <!--PNR_AddMultiElements>
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
								<xsl:when test="POS/Source/@AgentSine!=''">
									<xsl:value-of select="POS/Source/@AgentSine"/>
								</xsl:when>
								<xsl:otherwise>TRIPXML</xsl:otherwise>
							</xsl:choose>
						</longFreetext>
					</freetextData>
				</dataElementsIndiv>
			</dataElementsMaster>
		</PNR_AddMultiElements-->
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Position[Element/@Operation='modify' and Element/@Child='CustomerInfos']">
    <xsl:apply-templates select="Element[1][CustomerInfos]" mode="name"/>
  </xsl:template>
  <xsl:template match="Element" mode="name">
    <xsl:variable name="repName" select="following-sibling::Element[1]"/>
    <xsl:for-each select="CustomerInfos/CustomerInfo">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="firstname">
        <xsl:value-of select="Customer/PersonName/GivenName"/>
      </xsl:variable>
      <xsl:variable name="lastname">
        <xsl:value-of select="Customer/PersonName/Surname"/>
      </xsl:variable>
      <xsl:variable name="paxtype">
        <xsl:value-of select="Customer/PersonName/@NameType"/>
      </xsl:variable>
      <xsl:variable name="month">
        <xsl:value-of select="substring($repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/@BirthDate,6,2)"/>
      </xsl:variable>
      <xsl:variable name="date">
        <xsl:value-of select="substring($repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/@BirthDate,9,2)"/>
      </xsl:variable>
      <xsl:variable name="year">
        <xsl:value-of select="substring($repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/@BirthDate,3,2)"/>
      </xsl:variable>
      <xsl:variable name="month1">
        <xsl:choose>
          <xsl:when test="$month='01'">JAN</xsl:when>
          <xsl:when test="$month='02'">FEB</xsl:when>
          <xsl:when test="$month='03'">MAR</xsl:when>
          <xsl:when test="$month='04'">APR</xsl:when>
          <xsl:when test="$month='05'">MAY</xsl:when>
          <xsl:when test="$month='06'">JUN</xsl:when>
          <xsl:when test="$month='07'">JUL</xsl:when>
          <xsl:when test="$month='08'">AUG</xsl:when>
          <xsl:when test="$month='09'">SEP</xsl:when>
          <xsl:when test="$month='10'">OCT</xsl:when>
          <xsl:when test="$month='11'">NOV</xsl:when>
          <xsl:when test="$month='12'">DEC</xsl:when>
        </xsl:choose>
      </xsl:variable>
      <!--<PNR_NameUpdate>
				<recordLocator>
					<reservation>
						<controlNumber>
							<xsl:value-of select="../../../../UniqueID/@ID"/>
						</controlNumber>
					</reservation>
				</recordLocator>
				<xsl:apply-templates select="../../PNR_RetrieveByRecLocReply/travellerInfo[passengerData/travellerInformation/passenger/firstName = $firstname and passengerData/travellerInformation/passenger/type = $paxtype and passengerData/travellerInformation/traveller/surname = $lastname]">
					<xsl:with-param name="repName" select="$repName"/>
					<xsl:with-param name="pos">
						<xsl:value-of select="$pos"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</PNR_NameUpdate>-->
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <textStringDetails>
            <xsl:choose>
              <xsl:when test="$paxtype='INF'">
                <xsl:value-of select="concat(@RPH,'/(ADT)(INF',$repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/Surname,'/',$repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/GivenName,'/',$date,$month1,$year,')')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat('NU',@RPH,'/1',$repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/Surname,'/',$repName/CustomerInfos[1]/CustomerInfo/Customer/PersonName/GivenName)"/>
              </xsl:otherwise>
            </xsl:choose>
          </textStringDetails>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <textStringDetails>TTF/ALL </textStringDetails>
      </longTextString>
    </Command_Cryptic>
  </xsl:template>
  <xsl:template match="travellerInfo">
    <xsl:param name="repName"/>
    <xsl:param name="pos"/>
    <passenger>
      <passengerDetails>
        <paxDetails>
          <surname>
            <xsl:value-of select="$repName/CustomerInfos/CustomerInfo[position()=$pos]/Customer/PersonName/Surname"/>
          </surname>
          <quantity>1</quantity>
          <status>C</status>
        </paxDetails>
        <otherPaxDetails>
          <givenName>
            <xsl:value-of select="$repName/CustomerInfos/CustomerInfo[position()=$pos]/Customer/PersonName/GivenName"/>
          </givenName>
          <uniqueCustomerIdentifier>
            <xsl:value-of select="elementManagementPassenger/reference/number"/>
          </uniqueCustomerIdentifier>
        </otherPaxDetails>
      </passengerDetails>
    </passenger>
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/ReservationItems']" mode="xlseg">
    <PNR_Cancel>
      <!--reservationInfo>
				<reservation>
					<controlNumber>
						<xsl:value-of select="../UniqueID/@ID"/>
					</controlNumber>
				</reservation>
			</reservationInfo-->
      <pnrActions>
        <optionCode>0</optionCode>
      </pnrActions>
      <cancelElements>
        <entryType>E</entryType>
        <xsl:apply-templates select="Element[1]/ReservationItems/Item" mode="delseg"/>
      </cancelElements>
    </PNR_Cancel>
    <PNR_AddMultiElements>
      <pnrActions>
        <optionCode>0</optionCode>
      </pnrActions>
      <xsl:apply-templates select="Element[2]/ReservationItems/Item" mode="modseg"/>
    </PNR_AddMultiElements>
  </xsl:template>
  <xsl:template match="Item" mode="delseg">
    <xsl:param name="segmentref">
      <xsl:value-of select="@ItinSeqNumber"/>
    </xsl:param>
    <element>
      <identifier>ST</identifier>
      <xsl:variable name="st">
        <xsl:value-of select="../../../../../PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]/reference/number"/>
      </xsl:variable>
      <number>
        <xsl:choose>
          <xsl:when test="$st=''">0</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$st"/>
          </xsl:otherwise>
        </xsl:choose>
      </number>
    </element>
  </xsl:template>
  <xsl:template match="Item" mode="modseg">
    <xsl:param name="segmentref">
      <xsl:value-of select="@ItinSeqNumber"/>
    </xsl:param>
    <originDestinationDetails>
      <originDestination>
        <origin>
          <xsl:value-of select="Air/DepartureAirport/@LocationCode"/>
        </origin>
        <destination>
          <xsl:value-of select="Air/ArrivalAirport/@LocationCode"/>
        </destination>
      </originDestination>
      <itineraryInfo>
        <elementManagementItinerary>
          <reference>
            <qualifier>SR</qualifier>
            <number>
              <xsl:value-of select="Air/@RPH"/>
            </number>
          </reference>
          <segmentName>AIR</segmentName>
        </elementManagementItinerary>
        <airAuxItinerary>
          <travelProduct>
            <product>
              <depDate>
                <xsl:value-of select="substring(string(Air/@DepartureDateTime),9,2)"/>
                <xsl:value-of select="substring(string(Air/@DepartureDateTime),6,2)"/>
                <xsl:value-of select="substring(string(Air/@DepartureDateTime),3,2)"/>
              </depDate>
            </product>
            <boardpointDetail>
              <cityCode>
                <xsl:value-of select="Air/DepartureAirport/@LocationCode"/>
              </cityCode>
            </boardpointDetail>
            <offpointDetail>
              <cityCode>
                <xsl:value-of select="Air/ArrivalAirport/@LocationCode"/>
              </cityCode>
            </offpointDetail>
            <company>
              <identification>
                <xsl:value-of select="Air/MarketingAirline/@Code"/>
              </identification>
            </company>
            <productDetails>
              <identification>
                <xsl:value-of select="Air/@FlightNumber"/>
              </identification>
              <classOfService>
                <xsl:value-of select="Air/@ResBookDesigCode"/>
              </classOfService>
            </productDetails>
          </travelProduct>
          <messageAction>
            <business>
              <function>1</function>
            </business>
          </messageAction>
          <relatedProduct>
            <quantity>
              <xsl:value-of select="Air/@NumberInParty"/>
            </quantity>
            <status>NN</status>
          </relatedProduct>
          <selectionDetailsAir>
            <selection>
              <option>P10</option>
            </selection>
          </selectionDetailsAir>
        </airAuxItinerary>
      </itineraryInfo>
    </originDestinationDetails>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary" mode="modify">
    <number>
      <xsl:value-of select="reference/number"/>
    </number>
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Position[Element/@Operation='modify' and Element/@Child='CustomerInfo'] | Position[Element/@Operation='modify' and Element/@Child='FormOfPayment'] | Position[Element/@Operation='modify' and Element/@Child='Remarks'] | Position[Element/@Operation='modify' and Element/@Child='SpecialRemarks']| Position[Element/@Operation='modify' and Element/@Child='SpecialServiceRequests'] | Position[Element/@Operation='modify' and Element/@Child='AgencyCommission'] | Position[Element/@Operation='modify' and Element/@Child='Ticketing'] | Position[Element/@Operation='modify' and Element/@Child='Telephone']">
    <xsl:apply-templates select="Element[1][CustomerInfo/Customer/Telephone]" mode="phone"/>
    <xsl:apply-templates select="Element[1][CustomerInfo/Customer/Email]" mode="email"/>
    <xsl:apply-templates select="Element[1][CustomerInfo/Customer/Address[@UseType='Billing']/AddressLine]" mode="billing"/>
    <xsl:apply-templates select="Element[1][CustomerInfo/Customer/Address[@UseType='Mailing']]" mode="mailing"/>
    <xsl:apply-templates select="Element[1][CustomerInfo/Customer/CustLoyalty]" mode="custloyalty"/>
    <xsl:apply-templates select="Element[1][Remarks/Remark]" mode="remark"/>
    <xsl:apply-templates select="Element[1][SpecialRemarks/SpecialRemark]" mode="remark"/>
    <xsl:apply-templates select="Element[1][SpecialServiceRequests/SpecialServiceRequest]" mode="remark"/>
    <xsl:apply-templates select="Element[1][AgencyCommission]" mode="commission"/>
    <xsl:apply-templates select="Element[1][FormOfPayment]" mode="fop"/>
    <xsl:apply-templates select="Element[1][Ticketing]" mode="ticketing"/>
  </xsl:template>
  <xsl:template match="Element" mode="phone">
    <xsl:variable name="repPhone" select="following-sibling::Element[1]"/>
    <xsl:for-each select="CustomerInfo/Customer/Telephone">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="phoneref">
        <xsl:value-of select="@PhoneNumber"/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$phoneref]" mode="phone">
            <xsl:with-param name="repPhone" select="$repPhone"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="phone">
    <xsl:param name="repPhone"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repPhone/CustomerInfo/Customer/Telephone[position()=$pos]/@PhoneNumber)"/>
    </textStringDetails>
  </xsl:template>
  <!--********************************************************************************************************************* -->
  <xsl:template match="Element" mode="email">
    <xsl:variable name="repPhone" select="following-sibling::Element[1]"/>
    <xsl:for-each select="CustomerInfo/Customer/Email">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="emailref">
        <xsl:value-of select="."/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$emailref]" mode="email">
            <xsl:with-param name="repPhone" select="$repPhone"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="email">
    <xsl:param name="repPhone"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repPhone/CustomerInfo/Customer/Email[position()=$pos])"/>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="Element" mode="billing">
    <xsl:variable name="repAddress" select="following-sibling::Element[1]"/>
    <xsl:for-each select="CustomerInfo/Customer/Address[@UseType='Billing']">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="billingaddressref">
        <xsl:value-of select="."/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$billingaddressref]" mode="billingaddress">
            <xsl:with-param name="repAddress" select="$repAddress"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="Element" mode="ticketing">
    <xsl:variable name="repTkt" select="following-sibling::Element[1]"/>
    <xsl:variable name="TktTime">
      <xsl:value-of select="substring-after($repTkt/Ticketing/@TicketTimeLimit,'T')"/>
    </xsl:variable>
    <xsl:variable name="TktTime2">
      <xsl:value-of select="substring(string($TktTime),1,5)"/>
    </xsl:variable>
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <textStringDetails>
          <xsl:value-of select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='TK']/elementManagementData/lineNumber"/>
          <xsl:value-of select="'/'"/>
          <xsl:choose>
            <xsl:when test="$repTkt/Ticketing/TicketAdvisory = 'OK'">
              <xsl:text>OK</xsl:text>
              <xsl:choose>
                <xsl:when test="$repTkt/Ticketing/@TicketTimeLimit">
                  <xsl:value-of select="'/'"/>
                  <xsl:value-of select="substring(string($repTkt/Ticketing/@TicketTimeLimit),9,2)"/>
                  <xsl:call-template name="month">
                    <xsl:with-param name="month">
                      <xsl:value-of select="substring(string($repTkt/Ticketing/@TicketTimeLimit),6,2)"/>
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:when>
              </xsl:choose>
            </xsl:when>
            <xsl:when test="$repTkt/Ticketing/@TicketTimeLimit">
              <xsl:choose>
                <xsl:when test="$repTkt/Ticketing/TicketAdvisory = 'XL'">XL</xsl:when>
                <xsl:when test="$repTkt/Ticketing/TicketAdvisory = 'TL'">TL</xsl:when>
                <xsl:when test="$repTkt/Ticketing/TicketAdvisory = 'OK'">OK</xsl:when>
                <xsl:otherwise>TL</xsl:otherwise>
              </xsl:choose>
              <xsl:value-of select="'/'"/>
              <xsl:value-of select="substring(string($repTkt/Ticketing/@TicketTimeLimit),9,2)"/>
              <xsl:call-template name="month">
                <xsl:with-param name="month">
                  <xsl:value-of select="substring(string($repTkt/Ticketing/@TicketTimeLimit),6,2)"/>
                </xsl:with-param>
              </xsl:call-template>
              <xsl:value-of select="'/'"/>
              <xsl:choose>
                <xsl:when test="$TktTime2 = '0000'">
                  <xsl:text>2400</xsl:text>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(string($TktTime2),':','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:when>
          </xsl:choose>
        </textStringDetails>
      </longTextString>
    </Command_Cryptic>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="billingaddress">
    <xsl:param name="repAddress"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repAddress/CustomerInfo/Customer/Address[position()=$pos and @UseType='Billing']/AddressLine)"/>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="Element" mode="mailing">
    <xsl:variable name="repAddress" select="following-sibling::Element[1]"/>
    <xsl:for-each select="CustomerInfo/Customer/Address[@UseType='Mailing']">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="mailingaddressstreetref">
        <xsl:value-of select="StreetNmbr"/>
      </xsl:variable>
      <xsl:variable name="mailingaddresscityref">
        <xsl:value-of select="CityName"/>
      </xsl:variable>
      <xsl:variable name="mailingaddresszipref">
        <xsl:value-of select="PostalCode"/>
      </xsl:variable>
      <xsl:variable name="mailingaddressstateref">
        <xsl:value-of select="StateProv/@StateCode"/>
      </xsl:variable>
      <xsl:variable name="mailingaddresscountryref">
        <xsl:value-of select="CountryName/@Code"/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/structuredAddress[(address/option='A1' and  address/optionText=$mailingaddressstreetref) and (address/option='ZP' and address/optionText=$mailingaddresszipref) and (address/option='CI' and 	address/optionText=$mailingaddresscityref) and (address/option='ST' and address/optionText=$mailingaddressstateref) and (address/option='CO' and address/optionText=$mailingaddresscountryref)]">
            <xsl:with-param name="repAddress" select="$repAddress"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/structuredAddress">
    <xsl:param name="repAddress"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:value-of select="concat(../elementManagementData/lineNumber, '/A1-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/StreetNmbr, '/CI-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/CityName, '/ZP-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/PostalCode, '/ST-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/StateProv/@StateCode, '/CO-', $repAddress/CustomerInfo/Customer/Address[@UseType='Mailing']/CountryName/@Code)"/>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="Element" mode="custloyalty">
    <xsl:variable name="repCL" select="following-sibling::Element[1]"/>
    <xsl:for-each select="CustomerInfo/Customer/CustLoyalty">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="loyaltyref">
        <xsl:value-of select="@MembershipID"/>
      </xsl:variable>
      <xsl:variable name="programref">
        <xsl:value-of select="@ProgramID"/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo[frequentTraveler/company=$programref and frequentTraveler/membershipNumber=$loyaltyref]">
            <xsl:with-param name="repCL" select="$repCL"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/frequentTravellerInfo">
    <xsl:param name="repCL"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repCL/CustomerInfo/Customer/CustLoyalty/@MembershipID)"/>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="Element" mode="fop">
    <xsl:variable name="payForm" select="FormOfPayment/MiscChargeOrder/@OriginalPaymentForm"/>
    <xsl:variable name="repFOP" select="following-sibling::Element[1]"/>
    <xsl:variable name="repFOP1" select="."/>
    <xsl:variable name="fopref1">
      <xsl:if test="FormOfPayment/PaymentCard">
        <xsl:value-of select="concat('CC',FormOfPayment/PaymentCard/@CardCode,translate(FormOfPayment/PaymentCard/@CardNumber,'x','X'),'/',FormOfPayment/PaymentCard/@ExpireDate)"/>
        <xsl:if test="FormOfPayment[PaymentCard]/PaymentAmount/@Amount!= ''">
          <xsl:text>/</xsl:text>
          <xsl:value-of select="FormOfPayment[PaymentCard]/PaymentAmount/@CurrencyCode"/>
          <xsl:choose>
            <xsl:when test="FormOfPayment[PaymentCard]/PaymentAmount/@DecimalPlaces='2'">
              <xsl:variable name="am">
                <xsl:value-of select="substring(FormOfPayment[PaymentCard]/PaymentAmount/@Amount,1,string-length(FormOfPayment[PaymentCard]/PaymentAmount/@Amount)-2)"/>
              </xsl:variable>
              <xsl:choose>
                <xsl:when test="$am!=''">
                  <xsl:value-of select="$am"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
              <xsl:value-of select="'.'"/>
              <xsl:value-of select="substring(FormOfPayment[PaymentCard]/PaymentAmount/@Amount,string-length(FormOfPayment[PaymentCard]/PaymentAmount/@Amount)-1)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="FormOfPayment[PaymentCard]/PaymentAmount/@Amount"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
        <xsl:if test="FormOfPayment/TPA_Extensions/@ConfirmationNumber!= ''">
          <xsl:text>/</xsl:text>
          <xsl:choose>
            <xsl:when test="substring(FormOfPayment/TPA_Extensions/@ConfirmationNumber,1,1) != 'N'">
              <xsl:value-of select="concat('N',FormOfPayment/TPA_Extensions/@ConfirmationNumber)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="FormOfPayment/TPA_Extensions/@ConfirmationNumber"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:if>
        <xsl:if test="FormOfPayment[PaymentCard]/PaymentAmount/@TravelerRefNumberRPHList != ''">
          <xsl:value-of select="'/P'"/>
          <xsl:call-template name="RPHTraveler">
            <xsl:with-param name="RPH">
              <xsl:value-of select="FormOfPayment[PaymentCard]/PaymentAmount/@TravelerRefNumberRPHList"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:if>
      </xsl:if>
    </xsl:variable>
    <xsl:variable name="cashref1">
      <xsl:if test="FormOfPayment/DirectBill/@DirectBill_ID='Cash'">
        <xsl:value-of select="'CASH'"/>
        <xsl:if test="FormOfPayment[DirectBill]/PaymentAmount/@Amount!=''">
          <xsl:value-of select="'/'"/>
          <xsl:if test="FormOfPayment[DirectBill]/PaymentAmount/@CurrencyCode!=''">
            <xsl:value-of select="FormOfPayment[DirectBill]/PaymentAmount/@CurrencyCode"/>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="FormOfPayment[DirectBill]/PaymentAmount/@DecimalPlaces='2'">
              <xsl:variable name="am">
                <xsl:value-of select="substring(FormOfPayment[DirectBill]/PaymentAmount/@Amount,1,string-length(FormOfPayment[DirectBill]/PaymentAmount/@Amount)-2)"/>
              </xsl:variable>
              <xsl:choose>
                <xsl:when test="$am!=''">
                  <xsl:value-of select="$am"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
              <xsl:value-of select="'.'"/>
              <xsl:value-of select="substring(FormOfPayment[DirectBill]/PaymentAmount/@Amount,string-length(FormOfPayment[DirectBill]/PaymentAmount/@Amount)-1)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="FormOfPayment[DirectBill]/PaymentAmount/@Amount"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="FormOfPayment[DirectBill]/PaymentAmount/@TravelerRefNumberRPHList != ''">
            <xsl:value-of select="'/P'"/>
            <xsl:call-template name="RPHTraveler">
              <xsl:with-param name="RPH">
                <xsl:value-of select="FormOfPayment[DirectBill]/PaymentAmount/@TravelerRefNumberRPHList"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
        </xsl:if>
      </xsl:if>
    </xsl:variable>
    <xsl:variable name="checkref1">
      <xsl:if test="FormOfPayment/DirectBill/@DirectBill_ID='Check'">
        <xsl:value-of select="'CHECK'"/>
        <xsl:if test="FormOfPayment[DirectBill]/PaymentAmount/@Amount!=''">
          <xsl:value-of select="'/'"/>
          <xsl:if test="FormOfPayment[DirectBill]/PaymentAmount/@CurrencyCode!=''">
            <xsl:value-of select="FormOfPayment[DirectBill]/PaymentAmount/@CurrencyCode"/>
          </xsl:if>
          <xsl:choose>
            <xsl:when test="FormOfPayment[DirectBill]/PaymentAmount/@DecimalPlaces='2'">
              <xsl:variable name="am">
                <xsl:value-of select="substring(FormOfPayment[DirectBill]/PaymentAmount/@Amount,1,string-length(FormOfPayment[DirectBill]/PaymentAmount/@Amount)-2)"/>
              </xsl:variable>
              <xsl:choose>
                <xsl:when test="$am!=''">
                  <xsl:value-of select="$am"/>
                </xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
              <xsl:value-of select="'.'"/>
              <xsl:value-of select="substring(FormOfPayment[DirectBill]/PaymentAmount/@Amount,string-length(FormOfPayment[DirectBill]/PaymentAmount/@Amount)-1)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="FormOfPayment[DirectBill]/PaymentAmount/@Amount"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="FormOfPayment[DirectBill]/PaymentAmount/@TravelerRefNumberRPHList != ''">
            <xsl:value-of select="'/P'"/>
            <xsl:call-template name="RPHTraveler">
              <xsl:with-param name="RPH">
                <xsl:value-of select="FormOfPayment[DirectBill]/PaymentAmount/@TravelerRefNumberRPHList"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
        </xsl:if>
      </xsl:if>
    </xsl:variable>
    <xsl:variable name="paxref">
      <xsl:choose>
        <xsl:when test="FormOfPayment/@InfantOnly='true'">INF</xsl:when>
        <xsl:when test="FormOfPayment/@OmitInfant='true'">PAX</xsl:when>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="fopref">
      <xsl:value-of select="$paxref"/>
      <xsl:choose>
        <xsl:when test="FormOfPayment[1]/PaymentCard">
          <xsl:value-of select="$fopref1"/>
          <xsl:if test="$fopref1!='' and ($cashref1!='' or $checkref1!='')">
            <xsl:value-of select="'+'"/>
          </xsl:if>
          <xsl:value-of select="$cashref1"/>
          <xsl:value-of select="$checkref1"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$cashref1"/>
          <xsl:value-of select="$checkref1"/>
          <xsl:if test="$fopref1!='' and ($cashref1!='' or $checkref1!='')">
            <xsl:value-of select="'+'"/>
          </xsl:if>
          <xsl:value-of select="$fopref1"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="paxassoc">
      <xsl:value-of select="FormOfPayment/@TravelerRefNumberRPHList"/>
    </xsl:variable>
    <xsl:if test="(./../following-sibling::Position[1]/@XPath='OTA_TravelItineraryRS/TravelItinerary/TravelCost/FormOfPayment' or count(../../Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/TravelCost/FormOfPayment'])=1 or $username='OneTwoTrip') and not(FormOfPayment/MiscChargeOrder/@OriginalPaymentForm) and not($repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber) and (//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP'] or //PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP'])">
      <xsl:choose>
        <xsl:when test="$username='Downtown' and $system='Test'">
          <PNR_Cancel>
            <pnrActions>
              <optionCode>0</optionCode>
            </pnrActions>
            <cancelElements>
              <entryType>E</entryType>
              <xsl:for-each select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']">
                <element>
                  <identifier>OT</identifier>
                  <number>
                    <xsl:value-of select="elementManagementData/reference/number"/>
                  </number>
                </element>
              </xsl:for-each>
              <xsl:for-each select="//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']">
                <element>
                  <identifier>OT</identifier>
                  <number>
                    <xsl:value-of select="elementManagementData/reference/number"/>
                  </number>
                </element>
              </xsl:for-each>
            </cancelElements>
          </PNR_Cancel>
          <PNR_AddMultiElements>
            <pnrActions>
              <optionCode>0</optionCode>
            </pnrActions>
            <dataElementsMaster>
              <marker1/>
              <dataElementsIndiv>
                <elementManagementData>
                  <segmentName>FP</segmentName>
                </elementManagementData>
                <xsl:variable name="confChange">
                  <xsl:choose>
                    <xsl:when test="$repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber!=''">
                      <xsl:choose>
                        <xsl:when test="FormOfPayment/PaymentCard/@CardCode = $repFOP/FormOfPayment/PaymentCard/@CardCode and FormOfPayment/PaymentCard/@CardNumber = $repFOP/FormOfPayment/PaymentCard/@CardNumber and FormOfPayment/PaymentCard/@ExpireDate = $repFOP/FormOfPayment/PaymentCard/@ExpireDate">
                          <xsl:choose>
                            <xsl:when test="not(FormOfPayment/PaymentAmount) and not($repFOP/FormOfPayment/PaymentAmount)">Y</xsl:when>
                            <xsl:when test="FormOfPayment/PaymentAmount/@Amount != '' and FormOfPayment/PaymentAmount/@Amount = $repFOP/FormOfPayment/PaymentAmount/@Amount">Y</xsl:when>
                            <xsl:otherwise>N</xsl:otherwise>
                          </xsl:choose>
                        </xsl:when>
                        <xsl:otherwise>N</xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>N</xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="$repFOP/FormOfPayment and $paxref!=''">
                    <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,	$paxref)]" 	mode="fopDTT">
                      <xsl:with-param name="repFOP" select="$repFOP"/>
                      <xsl:with-param name="fopref" select="$fopref"/>
                      <xsl:with-param name="confChange" select="$confChange"/>
                    </xsl:apply-templates>
                    <xsl:apply-templates select="//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,$paxref)]" 	mode="fopDTT">
                      <xsl:with-param name="repFOP" select="$repFOP"/>
                      <xsl:with-param name="fopref" select="$fopref"/>
                      <xsl:with-param name="confChange" select="$confChange"/>
                    </xsl:apply-templates>
                  </xsl:when>
                  <xsl:when test="$repFOP/FormOfPayment">
                    <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fopDTT">
                      <xsl:with-param name="repFOP" select="$repFOP"/>
                      <xsl:with-param name="fopref" select="$fopref"/>
                      <xsl:with-param name="confChange" select="$confChange"/>
                    </xsl:apply-templates>
                    <xsl:apply-templates select="//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fopDTT">
                      <xsl:with-param name="repFOP" select="$repFOP"/>
                      <xsl:with-param name="fopref" select="$fopref"/>
                      <xsl:with-param name="confChange" select="$confChange"/>
                    </xsl:apply-templates>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:choose>
                      <xsl:when test="$paxref!='' and //PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,			$paxref)]">
                        <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,			$paxref)]" 	mode="fopDTT">
                          <xsl:with-param name="repFOP" select="$repFOP1"/>
                          <xsl:with-param name="fopref" select="$fopref"/>
                          <xsl:with-param name="confChange" select="$confChange"/>
                        </xsl:apply-templates>
                      </xsl:when>
                      <xsl:when test="$paxref!='' and //PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,			$paxref)]">
                        <xsl:apply-templates select="//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,			$paxref)]" 	mode="fopDTT">
                          <xsl:with-param name="repFOP" select="$repFOP1"/>
                          <xsl:with-param name="fopref" select="$fopref"/>
                          <xsl:with-param name="confChange" select="$confChange"/>
                        </xsl:apply-templates>
                      </xsl:when>
                      <xsl:when test="not(//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext) and not(//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext)">
                        <xsl:apply-templates select="FormOfPayment[1]" mode="fopDTT">
                          <xsl:with-param name="repFOP" select="$repFOP1"/>
                          <xsl:with-param name="fopref" select="$fopref"/>
                          <xsl:with-param name="confChange" select="$confChange"/>
                        </xsl:apply-templates>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fopDTT">
                          <xsl:with-param name="repFOP" select="$repFOP1"/>
                          <xsl:with-param name="fopref" select="$fopref"/>
                          <xsl:with-param name="confChange" select="$confChange"/>
                        </xsl:apply-templates>
                        <xsl:apply-templates select="//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fopDTT">
                          <xsl:with-param name="repFOP" select="$repFOP1"/>
                          <xsl:with-param name="fopref" select="$fopref"/>
                          <xsl:with-param name="confChange" select="$confChange"/>
                        </xsl:apply-templates>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:otherwise>
                </xsl:choose>
              </dataElementsIndiv>
            </dataElementsMaster>
          </PNR_AddMultiElements>
        </xsl:when>
        <xsl:otherwise>
          <xsl:if test="not(./../preceding-sibling::Position[1]/@XPath='OTA_TravelItineraryRS/TravelItinerary/TravelCost/FormOfPayment')">
            <Command_Cryptic>
              <messageAction>
                <messageFunctionDetails>
                  <messageFunction>M</messageFunction>
                </messageFunctionDetails>
              </messageAction>
              <longTextString>
                <textStringDetails>
                  <xsl:value-of select="'XE'"/>
                  <xsl:for-each select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']">
                    <xsl:value-of select="elementManagementData/lineNumber"/>
                    <xsl:if test="position() &lt; last()">
                      <xsl:value-of select="','"/>
                    </xsl:if>
                  </xsl:for-each>
                  <xsl:for-each select="//PNR_Reply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']">
                    <xsl:value-of select="elementManagementData/lineNumber"/>
                    <xsl:if test="position() &lt; last()">
                      <xsl:value-of select="','"/>
                    </xsl:if>
                  </xsl:for-each>
                </textStringDetails>
              </longTextString>
            </Command_Cryptic>
          </xsl:if>
          <Command_Cryptic>
            <messageAction>
              <messageFunctionDetails>
                <messageFunction>M</messageFunction>
              </messageFunctionDetails>
            </messageAction>
            <xsl:variable name="confChange">
              <xsl:choose>
                <xsl:when test="$repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber!=''">
                  <xsl:choose>
                    <xsl:when test="FormOfPayment/PaymentCard/@CardCode = $repFOP/FormOfPayment/PaymentCard/@CardCode and FormOfPayment/PaymentCard/@CardNumber = 			$repFOP/FormOfPayment/PaymentCard/@CardNumber and FormOfPayment/PaymentCard/@ExpireDate = $repFOP/FormOfPayment/PaymentCard/@ExpireDate">
                      <xsl:choose>
                        <xsl:when test="not(FormOfPayment/PaymentAmount) and not($repFOP/FormOfPayment/PaymentAmount)">Y</xsl:when>
                        <xsl:when test="FormOfPayment/PaymentAmount/@Amount != '' and FormOfPayment/PaymentAmount/@Amount = $repFOP/FormOfPayment/PaymentAmount/@Amount">Y</xsl:when>
                        <xsl:otherwise>N</xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>N</xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:otherwise>N</xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <longTextString>
              <xsl:choose>
                <xsl:when test="$payForm='MSCC'">
                  <textStringDetails>
                    <xsl:value-of select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/elementManagementData[segmentName='FP']/lineNumber"/>
                    <xsl:text>/+/</xsl:text>
                    <xsl:value-of select="$repFOP/MiscChargeOrder/@OriginalPaymentForm"/>
                  </textStringDetails>
                </xsl:when>
                <xsl:when test="$repFOP/FormOfPayment and $paxref!=''">
                  <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,$paxref)]" 			mode="fop">
                    <xsl:with-param name="repFOP" select="$repFOP"/>
                    <xsl:with-param name="fopref" select="$fopref"/>
                    <xsl:with-param name="confChange" select="$confChange"/>
                    <xsl:with-param name="paxassoc" select="$paxassoc"/>
                  </xsl:apply-templates>
                </xsl:when>
                <xsl:when test="$repFOP/FormOfPayment">
                  <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fop">
                    <xsl:with-param name="repFOP" select="$repFOP"/>
                    <xsl:with-param name="fopref" select="$fopref"/>
                    <xsl:with-param name="confChange" select="$confChange"/>
                    <xsl:with-param name="paxassoc" select="$paxassoc"/>
                  </xsl:apply-templates>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:choose>
                    <xsl:when test="not(following-sibling::Element[1])">
                      <xsl:apply-templates select="FormOfPayment[1]" mode="fop">
                        <xsl:with-param name="repFOP" select="$repFOP1"/>
                        <xsl:with-param name="fopref" select="$fopref"/>
                        <xsl:with-param name="confChange" select="$confChange"/>
                        <xsl:with-param name="paxassoc" select="$paxassoc"/>
                      </xsl:apply-templates>
                    </xsl:when>
                    <xsl:when test="$paxref!='' and  //PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,	$paxref)]">
                      <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext[starts-with(longFreetext,	$paxref)]" 	mode="fop">
                        <xsl:with-param name="repFOP" select="$repFOP1"/>
                        <xsl:with-param name="fopref" select="$fopref"/>
                        <xsl:with-param name="confChange" select="$confChange"/>
                        <xsl:with-param name="paxassoc" select="$paxassoc"/>
                      </xsl:apply-templates>
                    </xsl:when>
                    <xsl:when test="not(//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext)">
                      <xsl:apply-templates select="FormOfPayment[1]" mode="fop">
                        <xsl:with-param name="repFOP" select="$repFOP1"/>
                        <xsl:with-param name="fopref" select="$fopref"/>
                        <xsl:with-param name="confChange" select="$confChange"/>
                        <xsl:with-param name="paxassoc" select="$paxassoc"/>
                      </xsl:apply-templates>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FP']/otherDataFreetext" mode="fop">
                        <xsl:with-param name="repFOP" select="$repFOP1"/>
                        <xsl:with-param name="fopref" select="$fopref"/>
                        <xsl:with-param name="confChange" select="$confChange"/>
                        <xsl:with-param name="paxassoc" select="$paxassoc"/>
                      </xsl:apply-templates>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:otherwise>
              </xsl:choose>
            </longTextString>
          </Command_Cryptic>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>
  <xsl:template match="otherDataFreetext | FormOfPayment" mode="fopDTT">
    <xsl:param name="repFOP"/>
    <xsl:param name="fopref"/>
    <xsl:param name="confChange"/>
    <xsl:variable name="fop">
      <xsl:value-of select="translate(longFreetext,' ','')"/>
    </xsl:variable>
    <xsl:variable name="fopref2">
      <xsl:choose>
        <xsl:when test="substring($fopref,3,2)='MC'">
          <xsl:value-of select="concat('CCCA',substring($fopref, 5))"/>
        </xsl:when>
        <xsl:when test="substring($fopref,3,2)='DN'">
          <xsl:value-of select="concat('CCDC',substring($fopref, 5))"/>
        </xsl:when>
        <xsl:when test="substring($fopref,6,2)='MC'">
          <xsl:value-of select="concat(substring($fopref, 1,3),'CCCA',substring($fopref, 8))"/>
        </xsl:when>
        <xsl:when test="substring($fopref,6,2)='DN'">
          <xsl:value-of select="concat(substring($fopref, 1,3),'CCDC',substring($fopref, 8))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$fopref"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$fop=$fopref2">
        <xsl:variable name="cc">
          <xsl:choose>
            <xsl:when test="$repFOP/FormOfPayment/PaymentCard/@CardCode='MC'">CA</xsl:when>
            <xsl:when test="$repFOP/FormOfPayment/PaymentCard/@CardCode='DN'">DC</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardCode"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:variable name="cn">
          <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardNumber"/>
        </xsl:variable>
        <xsl:variable name="ed">
          <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@ExpireDate"/>
        </xsl:variable>
        <xsl:variable name="amt">
          <xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@Amount"/>
        </xsl:variable>
        <xsl:variable name="conf">
          <xsl:if test="substring($repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber,1,1)!='N'">N</xsl:if>
          <xsl:value-of select="$repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber"/>
        </xsl:variable>

        <textStringDetails>

          <xsl:choose>
            <xsl:when test="$confChange = 'Y'">
              <xsl:value-of select="concat(../elementManagementData/lineNumber, '//', $conf)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:variable name="newcard">
                <xsl:value-of select="concat(../elementManagementData/lineNumber, '/CC', $cc,$cn,'/',$ed)"/>
                <xsl:if test="$amt != ''">
                  <xsl:text>/</xsl:text>
                  <xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@CurrencyCode"/>
                  <xsl:variable name="amt1">
                    <xsl:variable name="dec">
                      <xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@DecimalPlaces"/>
                    </xsl:variable>
                    <xsl:value-of select="substring($repFOP/FormOfPayment/PaymentAmount/@Amount,1,string-length($repFOP/FormOfPayment/PaymentAmount/@Amount) - $dec)"/>
                    <xsl:value-of select="substring($repFOP/FormOfPayment/PaymentAmount/@Amount,string-length($repFOP/FormOfPayment/PaymentAmount/@Amount) - ($dec - 1),2)"/>
                  </xsl:variable>
                  <xsl:value-of select="substring($amt1,1,string-length($amt) - 2)"/>
                  <xsl:text>.</xsl:text>
                  <xsl:value-of select="substring($amt1,string-length($amt) - 1)"/>
                </xsl:if>
                <xsl:if test="$conf != ''">
                  <xsl:text>/</xsl:text>
                  <xsl:value-of select="$conf"/>
                </xsl:if>
              </xsl:variable>
              <xsl:value-of select="$newcard"/>
            </xsl:otherwise>
          </xsl:choose>
        </textStringDetails>
      </xsl:when>
      <xsl:otherwise>
        <formOfPayment>
          <xsl:choose>
            <xsl:when test="$repFOP/FormOfPayment[1]/PaymentCard">
              <fop>
                <identification>CC</identification>
                <creditCardCode>
                  <xsl:choose>
                    <xsl:when test="$repFOP/FormOfPayment[1]/PaymentCard/@CardCode='MC'">CA</xsl:when>
                    <xsl:when test="$repFOP/FormOfPayment[1]/PaymentCard/@CardCode='DN'">DC</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$repFOP/FormOfPayment[1]/PaymentCard/@CardCode"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </creditCardCode>
                <accountNumber>
                  <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardNumber"/>
                </accountNumber>
                <expiryDate>
                  <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@ExpireDate"/>
                </expiryDate>
              </fop>
            </xsl:when>
          </xsl:choose>
          <xsl:choose>
            <xsl:when test="$repFOP/FormOfPayment[position()=2]/DirectBill/@DirectBill_ID='Cash'">
              <fop>
                <identification>CA</identification>
                <amount>
                  <xsl:variable name="amt">
                    <xsl:value-of select="$repFOP/FormOfPayment[position()=2]/PaymentAmount/@Amount"/>
                  </xsl:variable>
                  <xsl:variable name="amt1">
                    <xsl:variable name="dec">
                      <xsl:value-of select="$repFOP/FormOfPayment[position()=2]/PaymentAmount/@DecimalPlaces"/>
                    </xsl:variable>
                    <xsl:value-of select="substring($repFOP/FormOfPayment[position()=2]/PaymentAmount/@Amount,1,string-length($repFOP/FormOfPayment[position()=2]/PaymentAmount/@Amount) - $dec)"/>
                    <xsl:value-of select="substring($repFOP/FormOfPayment[position()=2]/PaymentAmount/@Amount,string-length($repFOP/FormOfPayment[position()=2]/PaymentAmount/@Amount) - ($dec - 1),2)"/>
                  </xsl:variable>
                  <xsl:value-of select="substring($amt1,1,string-length($amt) - 2)"/>
                  <xsl:text>.</xsl:text>
                  <xsl:value-of select="substring($amt1,string-length($amt) - 1)"/>
                </amount>
                <currencyCode>
                  <xsl:value-of select="$repFOP/FormOfPayment[position()=2]/PaymentAmount/@CurrencyCode"/>
                </currencyCode>
              </fop>
            </xsl:when>
          </xsl:choose>
        </formOfPayment>
        <xsl:if test="$repFOP/FormOfPayment[1]/@OmitInfant='true' or $repFOP/FormOfPayment[1]/@InfantOnly='true'">
          <fopExtension>
            <fopSequenceNumber>1</fopSequenceNumber>
            <passengerType>
              <xsl:choose>
                <xsl:when test="$repFOP/FormOfPayment[1]/@OmitInfant='true'">PAX</xsl:when>
                <xsl:otherwise>INF</xsl:otherwise>
              </xsl:choose>
            </passengerType>
          </fopExtension>
        </xsl:if>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="otherDataFreetext | FormOfPayment" mode="fop">
    <xsl:param name="repFOP"/>
    <xsl:param name="fopref"/>
    <xsl:param name="confChange"/>
    <xsl:param name="paxassoc"/>
    <xsl:variable name="fop">
      <xsl:value-of select="translate(longFreetext,' ','')"/>
    </xsl:variable>
    <xsl:variable name="fopref2">
      <xsl:choose>
        <xsl:when test="substring($fopref,3,2)='MC'">
          <xsl:value-of select="concat('CCCA',substring($fopref, 5))"/>
        </xsl:when>
        <xsl:when test="substring($fopref,3,2)='DN'">
          <xsl:value-of select="concat('CCDC',substring($fopref, 5))"/>
        </xsl:when>
        <xsl:when test="substring($fopref,6,2)='MC'">
          <xsl:value-of select="concat(substring($fopref, 1,3),'CCCA',substring($fopref, 8))"/>
        </xsl:when>
        <xsl:when test="substring($fopref,6,2)='DN'">
          <xsl:value-of select="concat(substring($fopref, 1,3),'CCDC',substring($fopref, 8))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$fopref"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$fop=$fopref2">
        <textStringDetails>
          <xsl:variable name="cc">
            <xsl:choose>
              <xsl:when test="$repFOP/FormOfPayment/PaymentCard/@CardCode='MC'">CA</xsl:when>
              <xsl:when test="$repFOP/FormOfPayment/PaymentCard/@CardCode='DN'">DC</xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardCode"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:variable name="cn">
            <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@CardNumber"/>
          </xsl:variable>
          <xsl:variable name="ed">
            <xsl:value-of select="$repFOP/FormOfPayment/PaymentCard/@ExpireDate"/>
          </xsl:variable>
          <xsl:variable name="amt">
            <xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@Amount"/>
          </xsl:variable>
          <xsl:variable name="conf">
            <xsl:if test="substring($repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber,1,1)!='N'">N</xsl:if>
            <xsl:value-of select="$repFOP/FormOfPayment/TPA_Extensions/@ConfirmationNumber"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$confChange = 'Y'">
              <xsl:value-of select="concat(../elementManagementData/lineNumber, '//', $conf)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:variable name="newcard">
                <xsl:value-of select="concat(../elementManagementData/lineNumber, '/CC', $cc,$cn,'/',$ed)"/>
                <xsl:if test="$amt != ''">
                  <xsl:text>/</xsl:text>
                  <xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@CurrencyCode"/>
                  <xsl:variable name="amt1">
                    <xsl:variable name="dec">
                      <xsl:value-of select="$repFOP/FormOfPayment/PaymentAmount/@DecimalPlaces"/>
                    </xsl:variable>
                    <xsl:value-of select="substring($repFOP/FormOfPayment/PaymentAmount/@Amount,1,string-length($repFOP/FormOfPayment/PaymentAmount/@Amount) - $dec)"/>
                    <xsl:value-of select="substring($repFOP/FormOfPayment/PaymentAmount/@Amount,string-length($repFOP/FormOfPayment/PaymentAmount/@Amount) - ($dec - 1),2)"/>
                  </xsl:variable>
                  <xsl:value-of select="substring($amt1,1,string-length($amt) - 2)"/>
                  <xsl:text>.</xsl:text>
                  <xsl:value-of select="substring($amt1,string-length($amt) - 1)"/>
                </xsl:if>
                <xsl:if test="$conf != ''">
                  <xsl:text>/</xsl:text>
                  <xsl:value-of select="$conf"/>
                </xsl:if>
              </xsl:variable>
              <xsl:value-of select="$newcard"/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="$paxassoc!=''">
            <xsl:value-of select="'/P'"/>
            <xsl:call-template name="RPHTraveler">
              <xsl:with-param name="RPH">
                <xsl:value-of select="$paxassoc"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
        </textStringDetails>
      </xsl:when>
      <xsl:otherwise>
        <textStringDetails>
          <xsl:value-of select="concat('FP',$fopref2)"/>
          <!--xsl:choose>
						<xsl:when test="substring($fopref2,1,3)='PAX' or substring($fopref2,1,3)='INF'">
							<xsl:value-of select="concat('FP',$fopref2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat(../elementManagementData/lineNumber,'/',$fopref2)"/>
						</xsl:otherwise>
					</xsl:choose-->
          <xsl:if test="$paxassoc!=''">
            <xsl:value-of select="'/P'"/>
            <xsl:call-template name="RPHTraveler">
              <xsl:with-param name="RPH">
                <xsl:value-of select="$paxassoc"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:if>
        </textStringDetails>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Element" mode="commission">
    <xsl:variable name="repCommission" select="following-sibling::Element[1]"/>
    <xsl:variable name="repCommission1" select="."/>
    <xsl:for-each select="AgencyCommission">
      <xsl:if test="@Amount!='' or @Percent!=''">
        <xsl:variable name="comref">
          <xsl:choose>
            <xsl:when test="@Percent!=''">
              <xsl:value-of select="concat('*M*',@Percent)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="concat('*M*',@Amount,'A')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$repCommission/AgencyCommission">
            <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$comref]" mode="commission">
              <xsl:with-param name="repCommission" select="$repCommission"/>
            </xsl:apply-templates>
          </xsl:when>
          <xsl:when test="not(//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']/otherDataFreetext)">
            <xsl:apply-templates select="." mode="commissionInsert"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']/otherDataFreetext" mode="commission">
              <xsl:with-param name="repCommission" select="$repCommission1"/>
            </xsl:apply-templates>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
      <xsl:if test="@InfantAmount!='' or @InfantPercent!=''">
        <xsl:variable name="comref">
          <xsl:choose>
            <xsl:when test="@InfantPercent!=''">
              <xsl:value-of select="concat('*M*',@InfantPercent)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="concat('*M*',@InfantAmount,'A')"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$repCommission/AgencyCommission">
            <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$comref]" mode="commissionINF">
              <xsl:with-param name="repCommission" select="$repCommission"/>
            </xsl:apply-templates>
          </xsl:when>
          <xsl:when test="not(//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']/otherDataFreetext)">
            <xsl:apply-templates select="." mode="commissionInsertINF"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']/otherDataFreetext" mode="commissionINF">
              <xsl:with-param name="repCommission" select="$repCommission1"/>
            </xsl:apply-templates>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="commission">
    <xsl:param name="repCommission"/>
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <textStringDetails>
          <xsl:value-of select="concat('XE',../elementManagementData/lineNumber)"/>
        </textStringDetails>
      </longTextString>
    </Command_Cryptic>
    <xsl:apply-templates select="$repCommission/AgencyCommission" mode="commissionInsert"/>
    <!--xsl:variable name="pax">
			<xsl:if test="@InfantAmount!='' or @InfantPercent!=''">PAX</xsl:if>
		</xsl:variable>
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:choose>
						<xsl:when test="$repCommission/AgencyCommission/@Percent!=''">
							<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $pax, $repCommission/AgencyCommission/@Percent)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $pax, $repCommission/AgencyCommission/@Amount,'A')"/>
						</xsl:otherwise>
					</xsl:choose>
				</textStringDetails>
			</longTextString>
		</Command_Cryptic-->
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="commissionINF">
    <xsl:param name="repCommission"/>
    <xsl:if test="not($repCommission/AgencyCommission/@Amount) and not($repCommission/AgencyCommission/@Percent)">
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <textStringDetails>
            <xsl:value-of select="concat('XE',../elementManagementData/lineNumber)"/>
          </textStringDetails>
        </longTextString>
      </Command_Cryptic>
    </xsl:if>
    <xsl:apply-templates select="$repCommission/AgencyCommission" mode="commissionInsertINF"/>
    <!--Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:choose>
						<xsl:when test="$repCommission/AgencyCommission/@Percent!=''">
							<xsl:value-of select="concat(../elementManagementData/lineNumber, '/INF', $repCommission/AgencyCommission/@Percent)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat(../elementManagementData/lineNumber, '/INF', $repCommission/AgencyCommission/@Amount,'A')"/>
						</xsl:otherwise>
					</xsl:choose>
				</textStringDetails>
			</longTextString>
		</Command_Cryptic-->
  </xsl:template>
  <xsl:template match="AgencyCommission" mode="commissionInsert">
    <xsl:variable name="pax">
      <xsl:if test="@InfantAmount!='' or @InfantPercent!=''">PAX</xsl:if>
    </xsl:variable>
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <textStringDetails>
          <xsl:choose>
            <xsl:when test="@Percent!=''">
              <xsl:value-of select="concat('FM', $pax, @Percent)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="concat('FM', $pax, @Amount,'A')"/>
            </xsl:otherwise>
          </xsl:choose>
        </textStringDetails>
      </longTextString>
    </Command_Cryptic>
  </xsl:template>
  <xsl:template match="AgencyCommission" mode="commissionInsertINF">
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <textStringDetails>
          <xsl:choose>
            <xsl:when test="@InfantPercent!=''">
              <xsl:value-of select="concat('FMINF', @InfantPercent)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="concat('FMINF', @InfantAmount,'A')"/>
            </xsl:otherwise>
          </xsl:choose>
        </textStringDetails>
      </longTextString>
    </Command_Cryptic>
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SeatRequests']">
    <ChangeElement>
      <!-- this block is sent for any element update-->
      <xsl:apply-templates select="Element/SeatRequests/SeatRequest[../../@Operation='delete']"/>
    </ChangeElement>
  </xsl:template>
  <xsl:template match="SeatRequest">
    <xsl:param name="seatref">
      <xsl:value-of select="@SeatPreference"/>
    </xsl:param>
    <xsl:param name="segmentref">
      <xsl:value-of select="@FlightRefNumberRPHList"/>
    </xsl:param>
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <xsl:apply-templates select="../../../../../PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/freeText=$seatref and referenceForDataElement/reference/qualifier='ST' and referenceForDataElement/reference/number=$segmentref]" mode="seat"/>
      </longTextString>
    </Command_Cryptic>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv" mode="seat">
    <textStringDetails>
      <xsl:value-of select="concat(elementManagementData/reference/number, '/', ../../../OTA_UpdateRQ/Position/Element/SeatRequests/SeatRequest/@SeatPreference[../../../@Operation='modify'])"/>
    </textStringDetails>
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Position[@XPath='OTA_TravelItineraryRS/TravelItinerary/ItineraryInfo/SpecialRequestDetails/SpecialServiceRequests']">
    <ChangeElement>
      <!-- this block is sent for any element update-->
      <xsl:apply-templates select="Element/SpecialServiceRequests/SpecialServiceRequest[../../@Operation='delete']"/>
    </ChangeElement>
  </xsl:template>
  <xsl:template match="SpecialServiceRequest">
    <xsl:param name="ssrref">
      <xsl:value-of select="@SSRCode"/>
    </xsl:param>
    <Command_Cryptic>
      <messageAction>
        <messageFunctionDetails>
          <messageFunction>M</messageFunction>
        </messageFunctionDetails>
      </messageAction>
      <longTextString>
        <xsl:apply-templates select="../../../../../PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv[serviceRequest/ssr/type=$ssrref]" mode="ssr"/>
      </longTextString>
    </Command_Cryptic>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv" mode="ssr">
    <textStringDetails>
      <xsl:value-of select="concat(elementManagementData/reference/number, '/', ../../../OTA_UpdateRQ/Position/Element/SpecialServiceRequests/SpecialServiceRequest/@SSRCode[../../../@Operation='modify'])"/>
    </textStringDetails>
  </xsl:template>
  <!-- *********************************************************************************************************  -->
  <xsl:template match="Element" mode="remark">
    <xsl:variable name="repRemark" select="following-sibling::Element[1]"/>
    <xsl:for-each select="Remarks/Remark">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="rmkref">
        <xsl:value-of select="."/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$rmkref]" mode="remark">
            <xsl:with-param name="repRemark" select="$repRemark"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
    <xsl:for-each select="SpecialRemarks/SpecialRemark">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="rmkref">
        <xsl:value-of select="Text"/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <!--xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks[remarks/freetext=$rmkref]" mode="remark">
						<xsl:with-param name="repRemark" select="$repRemark"/>
						<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
					</xsl:apply-templates-->
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext[longFreetext=$rmkref]" mode="remark">
            <xsl:with-param name="repRemark" select="$repRemark"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
    </xsl:for-each>
    <xsl:for-each select="SpecialServiceRequests/SpecialServiceRequest">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:variable name="rmkref">
        <xsl:value-of select="Text"/>
      </xsl:variable>
      <Command_Cryptic>
        <messageAction>
          <messageFunctionDetails>
            <messageFunction>M</messageFunction>
          </messageFunctionDetails>
        </messageAction>
        <longTextString>
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/serviceRequest/ssr[freeText=$rmkref]" mode="remark">
            <xsl:with-param name="repRemark" select="$repRemark"/>
            <xsl:with-param name="pos">
              <xsl:value-of select="$pos"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </longTextString>
      </Command_Cryptic>
      <PNR_AddMultiElements>
        <pnrActions>
          <optionCode>0</optionCode>
        </pnrActions>
        <dataElementsMaster>
          <marker1/>
          <dataElementsIndiv>
            <elementManagementData>
              <segmentName>SSR</segmentName>
            </elementManagementData>
            <serviceRequest>
              <ssr>
                <type>
                  <xsl:value-of select="$repRemark/SpecialServiceRequests/SpecialServiceRequest/@SSRCode"/>
                </type>
                <status>HK</status>
                <quantity>
                  <xsl:choose>
                    <xsl:when test="$repRemark/SpecialServiceRequests/SpecialServiceRequest/@TravelerRefNumberRPHList!=''">
                      <xsl:value-of select="string-length($repRemark/SpecialServiceRequests/SpecialServiceRequest/@TravelerRefNumberRPHList)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="count(../../../../../TPA_Extensions/PNRData/Traveler)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </quantity>
                <companyId>
                  <xsl:text>YY</xsl:text>
                </companyId>
                <xsl:if test="$repRemark/SpecialServiceRequests/SpecialServiceRequest/Text!=''">
                  <freetext>
                    <xsl:value-of select="$repRemark/SpecialServiceRequests/SpecialServiceRequest/Text"/>
                  </freetext>
                </xsl:if>
              </ssr>
            </serviceRequest>
            <xsl:if test="$repRemark/SpecialServiceRequests/SpecialServiceRequest/@TravelerRefNumberRPHList != '' ">
              <referenceForDataElement>
                <xsl:if test="$repRemark/SpecialServiceRequests/SpecialServiceRequest/@TravelerRefNumberRPHList != ''">
                  <xsl:call-template name="RPH">
                    <xsl:with-param name="RPH">
                      <xsl:value-of select="$repRemark/SpecialServiceRequests/SpecialServiceRequest/@TravelerRefNumberRPHList"/>
                    </xsl:with-param>
                    <xsl:with-param name="Type">
                      <xsl:text>P</xsl:text>
                    </xsl:with-param>
                  </xsl:call-template>
                </xsl:if>
              </referenceForDataElement>
            </xsl:if>
          </dataElementsIndiv>
        </dataElementsMaster>
      </PNR_AddMultiElements>
    </xsl:for-each>
  </xsl:template>
  <xsl:template name="RPH">
    <xsl:param name="RPH"/>
    <xsl:param name="Type"/>
    <xsl:if test="string-length($RPH) != 0">
      <xsl:variable name="tRPH">
        <xsl:value-of select="substring($RPH,1,1)"/>
      </xsl:variable>
      <reference>
        <qualifier>
          <xsl:value-of select="$Type"/>
          <xsl:text>T</xsl:text>
        </qualifier>
        <number>
          <xsl:variable name="pnrRPH">
            <xsl:choose>
              <xsl:when test="$Type='P'">
                <xsl:value-of select="//PNR_RetrieveByRecLocReply/travellerInfo[elementManagementPassenger/lineNumber=$tRPH]/elementManagementPassenger/reference/number"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="//PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo[elementManagementItinerary/lineNumber=$tRPH]/elementManagementItinerary/reference/number"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:value-of select="$pnrRPH"/>
        </number>
      </reference>
      <xsl:call-template name="RPH">
        <xsl:with-param name="RPH">
          <xsl:value-of select="substring($RPH,2)"/>
        </xsl:with-param>
        <xsl:with-param name="Type">
          <xsl:value-of select="$Type"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template name="RPHTraveler">
    <xsl:param name="RPH"/>
    <xsl:if test="string-length($RPH) != 0">
      <xsl:variable name="tRPH">
        <xsl:value-of select="substring($RPH,1,1)"/>
      </xsl:variable>
      <xsl:value-of select="$tRPH"/>
      <xsl:if test="string-length(substring($RPH,2))>0">
        <xsl:value-of select="','"/>
      </xsl:if>
      <xsl:call-template name="RPHTraveler">
        <xsl:with-param name="RPH">
          <xsl:value-of select="substring($RPH,2)"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/miscellaneousRemarks" mode="remark">
    <xsl:param name="repRemark"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:choose>
        <xsl:when test="$repRemark/Remarks">
          <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repRemark/Remarks/Remark[position()=$pos])"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repRemark/SpecialRemarks/SpecialRemark[position()=$pos])"/>
        </xsl:otherwise>
      </xsl:choose>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/otherDataFreetext" mode="remark">
    <xsl:param name="repRemark"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:choose>
        <xsl:when test="$repRemark/Remarks">
          <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repRemark/Remarks/Remark[position()=$pos])"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="concat(../elementManagementData/lineNumber, '/', $repRemark/SpecialRemarks/SpecialRemark[position()=$pos]/Text)"/>
        </xsl:otherwise>
      </xsl:choose>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/serviceRequest/ssr" mode="remark">
    <xsl:param name="repRemark"/>
    <xsl:param name="pos"/>
    <textStringDetails>
      <xsl:value-of select="concat('XE',../../elementManagementData/lineNumber)"/>
    </textStringDetails>
  </xsl:template>
  <xsl:template match="PNR_RetrieveByRecLocReply/dataElementsMaster/dataElementsIndiv/serviceRequest/ssr" mode="remark2">
    <xsl:param name="repRemark"/>
    <xsl:param name="pos"/>
    <xsl:value-of select="../../elementManagementData/lineNumber"/>
  </xsl:template>
  <!--********************************************************************************************************************* -->
  <xsl:template match="Position[Element/@Operation='modify' and Element/@Child='ReservationItems' and Element/ReservationItems/Item/Air[@Status='RR']]">
    <xsl:for-each select="Element/ReservationItems/Item">
      <xsl:variable name="pos">
        <xsl:value-of select="position()"/>
      </xsl:variable>
      <xsl:if test="Air[@Status='RR']">
        <xsl:variable name="indref">
          <xsl:value-of select="Air/@Status"/>
        </xsl:variable>
        <xsl:variable name="resref">
          <xsl:value-of select="@RPH"/>
        </xsl:variable>
        <Command_Cryptic>
          <messageAction>
            <messageFunctionDetails>
              <messageFunction>M</messageFunction>
            </messageFunctionDetails>
          </messageAction>
          <longTextString>
            <textStringDetails>
              <xsl:value-of select="concat($resref, '/', $indref)"/>
            </textStringDetails>
          </longTextString>
        </Command_Cryptic>
      </xsl:if>
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="Item" mode="xlseg">
    <xsl:param name="segmentref">
      <xsl:value-of select="@ItinSeqNumber"/>
    </xsl:param>
    <element>
      <identifier>ST</identifier>
      <xsl:choose>
        <xsl:when test="//PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]">
          <xsl:apply-templates select="//PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary[lineNumber=$segmentref]" mode="xlseg"/>
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

  <xsl:template match="PNR_RetrieveByRecLocReply/originDestinationDetails/itineraryInfo/elementManagementItinerary" mode="xlseg">
    <number>
      <xsl:value-of select="reference/number"/>
    </number>
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
