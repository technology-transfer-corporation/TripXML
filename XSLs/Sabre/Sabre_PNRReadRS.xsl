<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:stl="http://services.sabre.com/STL/v01">
  <!-- ================================================================== -->
  <!-- Sabre_PNRReadRS.xsl 															-->
  <!-- ================================================================== -->
  <!-- Date: 13 Oct 2015 - Kobelev - corrected PTC identifier for passengers -->
  <!-- Date: 17 May 2012 - Kasun - corrected Arrival date format				-->
  <!-- Date: 25 Apr 2012 - Kasun - corrected error mapping				-->
  <!-- Date: 22 Mar 2012 - Kasun - mapped cash and check form of payments				-->
  <!-- Date: 11 Mar 2012 - Shashin - corrected year part in ticketingtimelimit				-->
  <!-- Date: 14 Feb 2012 - Rastko - corrected misc segment mapping				-->
  <!-- Date: 26 Sep 2010 - Rastko - mapped CustLoyalty	  							-->
  <!-- Date: 08 Apr 2010 - Rastko - fixed total fares calculation 						-->
  <!-- Date: 29 Apr 2010 - Rastko - fixed parsing of fares when hostorical fares exist		-->
  <!-- Date: 20 May 2010 - Rastko - fixed ticket time limit parsing					-->
  <!-- Date: 25 May 2010 - Rastko - fixed ticket time limit parsing when code is 7TAW/		-->
  <!-- Date: 02 Jul 2010 - Rastko - fixed missing remarks in display					-->
  <!-- Date: 16 Jul 2010 - Rastko - fixed ticket time limit parsing when code is 7TAW/		-->
  <!-- Date: 06 Sep 2010 - Rastko - corrected calculation of base and total fares			-->
  <!-- ================================================================== -->
  <xsl:output omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <OTA_TravelItineraryRS>
      <xsl:attribute name="Version">1.001</xsl:attribute>
      <xsl:if test="TravelItineraryReadRS/EchoToken!=''">
        <xsl:attribute name="EchoToken">
          <xsl:value-of select="TravelItineraryReadRS/EchoToken"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="TravelItineraryReadRS"/>
      <xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:attribute name="Code">E</xsl:attribute>
            <xsl:text>INVALID INPUT FILE</xsl:text>
          </Error>
        </Errors>
      </xsl:if>
      <xsl:if test="PNRReply">
        <Errors>
          <xsl:for-each select="PNRReply/Error">
            <Error>
              <xsl:attribute name="Type">
                <xsl:value-of select="@Type"/>
              </xsl:attribute>
              <xsl:value-of select="."/>
            </Error>
          </xsl:for-each>
        </Errors>
      </xsl:if>
      <xsl:if test="EndTransactionRS/Errors">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:value-of select="EndTransactionRS/Errors/Error"/>
          </Error>
        </Errors>
      </xsl:if>
      <xsl:if test="TravelItineraryReadRS/ConversationID!=''">
        <ConversationID>
          <xsl:value-of select="TravelItineraryReadRS/ConversationID"/>
        </ConversationID>
      </xsl:if>
    </OTA_TravelItineraryRS>
  </xsl:template>
  <!--************************************************************************************************************-->
  <xsl:template match="TravelItineraryReadRS">
    <xsl:choose>
      <xsl:when test="Errors/Error != ''">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:value-of select="Errors/Error"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:when test="stl:ApplicationResults/@status='NotProcessed'">
        <Errors>
          <Error>
            <xsl:attribute name="Type">
              <xsl:value-of select="stl:ApplicationResults/stl:Error/@type"/>
            </xsl:attribute>
            <xsl:value-of select="stl:ApplicationResults/stl:Error/stl:SystemSpecificResults"/>
          </Error>
        </Errors>
      </xsl:when>
      <!--xsl:when test="not(TravelItinerary/ItineraryRef) and not(Errors/Error)">			
						<Errors>
								<Error>
									<xsl:attribute name="Type">Sabre</xsl:attribute>
									<xsl:attribute name="Code">E</xsl:attribute>
									<xsl:text>INVALID INPUT FILE</xsl:text>
								</Error>
						</Errors>
				</xsl:when-->
      <xsl:otherwise>
        <Success/>
        <TravelItinerary>
          <ItineraryRef>
            <xsl:attribute name="Type">PNR</xsl:attribute>
            <xsl:attribute name="ID">
              <xsl:value-of select="TravelItinerary/ItineraryRef/@ID"/>
            </xsl:attribute>
            <xsl:attribute name="ID_Context">
              <xsl:value-of select="TravelItinerary/ItineraryRef //Source/@PseudoCityCode"/>
            </xsl:attribute>
          </ItineraryRef>
          <xsl:apply-templates select="TravelItinerary"/>
        </TravelItinerary>
        <!--Errors>
					<xsl:apply-templates select="Errors/Error"/>
				</Errors-->
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			TravelItinerary detail Information                                             -->
  <!--************************************************************************************-->
  <xsl:template match="TravelItinerary">
    <!--******************************************************-->
    <!--				Names                                      -->
    <!--******************************************************-->
    <CustomerInfos>
      <xsl:variable name="pd">
        <xsl:value-of select="../SabreCommandLLSRS/Response"/>
      </xsl:variable>
      <xsl:apply-templates select="CustomerInfo//PersonName">
        <xsl:with-param name="pd">
          <xsl:value-of select="$pd"/>
        </xsl:with-param>
      </xsl:apply-templates>
    </CustomerInfos>
    <!--******************************************************-->
    <!--			Air Itinerary                                      -->
    <!--******************************************************-->
    <ItineraryInfo>
      <ReservationItems>
        <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/FlightSegment " mode="Air"/>
        <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Vehicle" mode="Car"/>
        <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Hotel" mode="Hotel"/>
        <xsl:if test="ItineraryInfo/ReservationItems/Item/TPA_Extensions/Line[@Type='OTH']">
          <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/TPA_Extensions" mode="Other"/>
        </xsl:if>
        <!--xsl:if test="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo">
					<ItemPricing>
						<xsl:apply-templates select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo" mode="Air" />
					</ItemPricing>
				</xsl:if-->
        <xsl:if test="ItineraryInfo/ItineraryPricing!=''">
          <ItemPricing>
            <xsl:apply-templates select="ItineraryInfo/ItineraryPricing" mode="Fare"/>
          </ItemPricing>
        </xsl:if>
      </ReservationItems>
      <xsl:apply-templates select="ItineraryInfo/Ticketing[@TicketTimeLimit!='']" mode="Ticketing"/>
      <xsl:if test="SpecialServiceInfo/Service/@SSR_Code!= ''">
        <SpecialRequestDetails>
          <xsl:if test="SpecialServiceInfo/Service/@SSR_Code= 'OSI'">
            <OtherServiceInformations>
              <xsl:apply-templates select="SpecialServiceInfo/Service[@SSR_Code= 'OSI']" mode="OSI"/>
            </OtherServiceInformations>
          </xsl:if>
          <xsl:if test="SpecialServiceInfo/Service/@SSR_Code!= 'OSI'">
            <SpecialServiceRequests>
              <xsl:apply-templates select="SpecialServiceInfo/Service[@SSR_Code= 'SSR']" mode="SSR"/>
            </SpecialServiceRequests>
          </xsl:if>
        </SpecialRequestDetails>
      </xsl:if>
      <xsl:if test="ItineraryInfo/Ticketing[@eTicketNumber!='']">
        <TPA_Extensions>
          <IssuedTickets>
            <xsl:apply-templates select="ItineraryInfo/Ticketing[@eTicketNumber!='']" mode="IssuedTicket"/>
          </IssuedTickets>
        </TPA_Extensions>
      </xsl:if>
    </ItineraryInfo>
    <!--******************************************************-->
    <!--			Form of Payment                               -->
    <!--******************************************************-->
    <xsl:choose>
      <xsl:when test="CustomerInfo/PaymentInfo/Payment/Form/PaymentCard/@CardType != '' or CustomerInfos/CustomerInfo[1]/Customer[1]/PaymentForm/PaymentCard">
        <TravelCost>
          <xsl:if test="CustomerInfo/PaymentInfo/Payment/Form">
            <FormOfPayment>
              <xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form"/>
            </FormOfPayment>
          </xsl:if>
          <xsl:if test="CustomerInfo/PaymentInfo/Payment/Form/PaymentCard">
            <FormOfPayment>
              <xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form"/>
            </FormOfPayment>
          </xsl:if>
        </TravelCost>
      </xsl:when>
      <xsl:when test="contains(CustomerInfo/PaymentInfo/Payment/Form/TPA_Extensions/Text,'XXXXX')">
        <xsl:variable name="card">
          <xsl:value-of select="CustomerInfo/PaymentInfo/Payment/Form/TPA_Extensions/Text"/>
        </xsl:variable>
        <TravelCost>
          <FormOfPayment>
            <xsl:attribute name="RPH">
              <xsl:value-of select="CustomerInfo/PaymentInfo/Payment/Form/@RPH"/>
            </xsl:attribute>
            <PaymentCard>
              <xsl:attribute name="CardCode">
                <xsl:choose>
                  <xsl:when test="substring($card,2,2)='CA'">MC</xsl:when>
                  <xsl:when test="substring($card,2,2)='DI'">DS</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring($card,2,2)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="CardNumber">
                <xsl:value-of select="substring-before(substring($card,4),'Â')"/>
              </xsl:attribute>
              <xsl:attribute name="ExpireDate">XXXX</xsl:attribute>
            </PaymentCard>
          </FormOfPayment>
        </TravelCost>
      </xsl:when>
      <xsl:otherwise>
        <TravelCost>
          <xsl:if test="CustomerInfo/PaymentInfo/Payment/Form">
            <FormOfPayment>
              <xsl:attribute name="RPH">
                <xsl:value-of select="CustomerInfo/PaymentInfo/Payment/Form/@RPH"/>
              </xsl:attribute>
              <DirectBill>
                <xsl:attribute name="DirectBill_ID">
                  <xsl:apply-templates select="CustomerInfo/PaymentInfo/Payment/Form"/>
                </xsl:attribute>
              </DirectBill>
            </FormOfPayment>
          </xsl:if>
        </TravelCost>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="RemarkInfo != ''">
      <Remarks>
        <xsl:for-each select="RemarkInfo/Remark">
          <Remark>
            <xsl:attribute name="RPH">
              <xsl:value-of select="@RPH"/>
            </xsl:attribute>
            <xsl:attribute name="Type">
              <xsl:value-of select="@Type"/>
            </xsl:attribute>
            <xsl:value-of select="Text"/>
          </Remark>
        </xsl:for-each>
      </Remarks>
    </xsl:if>
    <UpdatedBy>
      <!--<xsl:attribute name="CreateDateTime"><xsl:value-of select="ItineraryRef/Source/@CreateDateTime"/></xsl:attribute>-->
      <xsl:attribute name="CreateDateTime">
        <xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,1,4)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,6,2)"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,9,2)"/>
        <xsl:text>T</xsl:text>
        <xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,12,2)"/>
        <xsl:text>:</xsl:text>
        <xsl:value-of select="substring(ItineraryRef/Source/@CreateDateTime,15,2)"/>
        <xsl:text>:00</xsl:text>
      </xsl:attribute>
    </UpdatedBy>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			PNR Retrieve Errors                                           	                 -->
  <!--************************************************************************************-->
  <xsl:template match="Err">
    <Error>
      <xsl:attribute name="Type">General</xsl:attribute>
      <xsl:value-of select="Text"/>
    </Error>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Issued Tickets Elements 	                               		    -->
  <!-- ************************************************************** -->
  <xsl:template match="Ticketing" mode="IssuedTicket">
    <IssuedTicket>
      <xsl:value-of select="@eTicketNumber"/>
    </IssuedTicket>
  </xsl:template>
  <!--************************************************************************************-->
  <!--						 Passenger Information         		                        -->
  <!--************************************************************************************-->
  <xsl:template match="PersonName">
    <xsl:param name="pd"/>
    <xsl:variable name="paxref">
      <xsl:value-of select="translate(@NameNumber,'0','')"/>
    </xsl:variable>
    <CustomerInfo>
      <xsl:attribute name="RPH">
        <xsl:value-of select="@RPH"/>
      </xsl:attribute>
      <Customer>
        <PersonName>
          <xsl:choose>
            <xsl:when test="@PassengerType!=''">
              <xsl:attribute name="NameType">
                <xsl:value-of select="@PassengerType"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:when test="@Infant='true'">
              <xsl:attribute name="NameType">INF</xsl:attribute>
            </xsl:when>
            <!-- this is for parsing *PQS response -->
            <xsl:when test="contains($pd,$paxref)">
              <xsl:attribute name="NameType">
                <xsl:variable name="vDigits" select="'0123456789'"/>
                <xsl:variable name="alpha" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
                <xsl:variable name="paxtype">
                  <xsl:choose>
                    <xsl:when test="translate(substring(substring-after($pd,$paxref),9,3),(translate(substring(substring-after($pd,$paxref),9,3), $alpha,'')), '') = ''">ADT</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring(substring-after($pd,$paxref),9,3)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="substring($paxtype,1,1)!=' '">
                    <xsl:choose>
                      <xsl:when test="substring($paxtype,1,2)='CH'">
                        <xsl:value-of select="'CHD'"/>
                      </xsl:when>
                      <xsl:when test="substring($paxtype,1,1)='C' and string-length(translate($paxtype, $vDigits,'')) &gt; 0">
                        <xsl:value-of select="'CHD'"/>
                      </xsl:when>
                      <xsl:when test="$paxtype='CNN'">
                        <xsl:value-of select="'CHD'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="$paxtype"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>ADT</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>ADT</xsl:otherwise>
          </xsl:choose>
          <xsl:if test="Remarks != ''">
            <xsl:attribute name="NameType">
              <xsl:value-of select="substring(string(Remarks),23,3)"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:if test="NamePrefix!=''">
            <NamePrefix>
              <xsl:value-of select="NamePrefix"/>
            </NamePrefix>
          </xsl:if>
          <GivenName>
            <xsl:value-of select="GivenName"/>
          </GivenName>
          <Surname>
            <xsl:value-of select="Surname"/>
          </Surname>
        </PersonName>
        <xsl:apply-templates select="Email"/>
        <xsl:variable name="custref">
          <xsl:value-of select="$paxref"/>
        </xsl:variable>
        <xsl:for-each select="CustLoyalty[@NameNumber=$custref]">
          <CustLoyalty>
            <xsl:attribute name="ProgramID">
              <xsl:value-of select="@ProgramID"/>
            </xsl:attribute>
            <xsl:attribute name="MembershipID">
              <xsl:value-of select="@MembershipID"/>
            </xsl:attribute>
            <xsl:attribute name="RPH">
              <xsl:value-of select="@RPH"/>
            </xsl:attribute>
          </CustLoyalty>
        </xsl:for-each>
        <xsl:apply-templates select="../../../../Remarks/Remark/Line[@Type='Client Address']"/>
        <xsl:apply-templates select="../../../../Remarks/Remark/Line[@Type='Delivery']"/>
      </Customer>
    </CustomerInfo>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			      Air Itinerary Section				                              	    -->
  <!--************************************************************************************-->
  <xsl:template match="Item/FlightSegment " mode="Air">
    <Item>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:if test="@ActionCode = 'GK'">
        <xsl:attribute name="IsPassive">Y</xsl:attribute>
      </xsl:if>
      <Air>
        <xsl:attribute name="DepartureDateTime">
          <xsl:value-of select="@DepartureDateTime"/>
        </xsl:attribute>
        <xsl:attribute name="ArrivalDateTime">
          <xsl:choose>
            <xsl:when test="starts-with(@ArrivalDateTime,'20')">
              <xsl:value-of select="@ArrivalDateTime"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(@DepartureDateTime,1,4)"/>
              <xsl:text>-</xsl:text>
              <xsl:value-of select="@ArrivalDateTime"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:if test="@StopQuantity != ''">
          <xsl:attribute name="StopQuantity">
            <xsl:value-of select="@StopQuantity"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:attribute name="RPH">
          <xsl:value-of select="@SegmentNumber"/>
        </xsl:attribute>
        <xsl:attribute name="FlightNumber">
          <xsl:value-of select="@FlightNumber"/>
        </xsl:attribute>
        <xsl:attribute name="ResBookDesigCode">
          <xsl:value-of select="@ResBookDesigCode"/>
        </xsl:attribute>
        <xsl:attribute name="Status">
          <xsl:value-of select="@Status"/>
        </xsl:attribute>
        <xsl:attribute name="NumberInParty">
          <xsl:value-of select="translate(@NumberInParty,'0','')"/>
        </xsl:attribute>
        <xsl:if test="@eTicket">
          <xsl:attribute name="E_TicketEligibility">
            <xsl:choose>
              <xsl:when test="@eTicket= 'true'">Eligible</xsl:when>
              <xsl:otherwise>NotEligible</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:if>
        <DepartureAirport>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="OriginLocation/@LocationCode"/>
          </xsl:attribute>
        </DepartureAirport>
        <ArrivalAirport>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="DestinationLocation/@LocationCode"/>
          </xsl:attribute>
        </ArrivalAirport>
        <OperatingAirline>
          <xsl:attribute name="Code">
            <xsl:choose>
              <xsl:when test="OperatingAirline/@Code!=''">
                <xsl:value-of select="OperatingAirline/@Code"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="MarketingAirline/@Code"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </OperatingAirline>
        <xsl:if test="Equipment/@AirEquipType != ''">
          <Equipment>
            <xsl:attribute name="AirEquipType">
              <xsl:value-of select="Equipment/@AirEquipType"/>
            </xsl:attribute>
          </Equipment>
        </xsl:if>
        <MarketingAirline>
          <xsl:attribute name="Code">
            <xsl:value-of select="MarketingAirline/@Code"/>
          </xsl:attribute>
        </MarketingAirline>
        <TPA_Extensions>
          <xsl:attribute name="ConfirmationNumber">
            <xsl:value-of select="../../../../ItineraryRef/@ID"/>
          </xsl:attribute>
        </TPA_Extensions>
      </Air>
    </Item>
  </xsl:template>
  <!--*****************************************************************-->
  <!--			Car Segs						   		     -->
  <!--*****************************************************************-->
  <xsl:template match="Vehicle" mode="Car">
    <Item>
      <Vehicle>
        <ConfID>
          <xsl:attribute name="Type">C</xsl:attribute>
          <xsl:attribute name="ID">
            <xsl:value-of select="ConfId/@Id"/>
          </xsl:attribute>
        </ConfID>
        <Vendor>
          <xsl:attribute name="Code">
            <xsl:value-of select="Vendor/@Code"/>
          </xsl:attribute>
        </Vendor>
        <VehRentalCore>
          <xsl:attribute name="PickUpDateTime">
            <xsl:value-of select="VehRentalCore/@PickUpDateTime"/>
          </xsl:attribute>
          <xsl:attribute name="ReturnDateTime">
            <xsl:value-of select="VehRentalCore/@ReturnDateTime"/>
          </xsl:attribute>
          <PickUpLocation>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode"/>
            </xsl:attribute>
          </PickUpLocation>
          <ReturnLocation>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode"/>
            </xsl:attribute>
          </ReturnLocation>
        </VehRentalCore>
        <Vehicle>
          <xsl:attribute name="AirConditionInd">
            <xsl:value-of select="Vehicle/@AirConditionInd"/>
          </xsl:attribute>
          <xsl:attribute name="TransmissionType">
            <xsl:value-of select="Vehicle/@TransmissionType"/>
          </xsl:attribute>
          <VehType>
            <xsl:attribute name="VehicleCategory">
              <xsl:value-of select="Vehicle/VehType/@VehicleCategory"/>
            </xsl:attribute>
          </VehType>
        </Vehicle>
        <RentalRate>
          <RateDistance>
            <xsl:attribute name="Unlimited">
              <xsl:choose>
                <xsl:when test="RentalRate/RateDistance/@Unlimited='true'">1</xsl:when>
                <xsl:otherwise>0</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="DistUnitName">
              <xsl:value-of select="RentalRate/RateDistance/@DistUnitName"/>
            </xsl:attribute>
            <xsl:attribute name="VehiclePeriodUnitName">
              <xsl:value-of select="RentalRate/RateDistance/@VehiclePeriodUnitName"/>
            </xsl:attribute>
          </RateDistance>
          <VehicleCharges>
            <VehicleCharge>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(RentalRate/VehicleCharges/VehicleCharge/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="RentalRate/VehicleCharges/VehicleCharge/@CurrencyCode"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="RentalRate/VehicleCharges/VehicleCharge/@DecimalPlaces"/>
              </xsl:attribute>
              <xsl:attribute name="TaxInclusive">
                <xsl:choose>
                  <xsl:when test="RentalRate/VehicleCharges/VehicleCharge/@TaxInclusive='true'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="GuaranteedInd">
                <xsl:choose>
                  <xsl:when test="RentalRate/VehicleCharges/VehicleCharge/@GuaranteedInd='true'">1</xsl:when>
                  <xsl:otherwise>0</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="Purpose">L</xsl:attribute>
              <!--plugged this value in for now - come back here -->
            </VehicleCharge>
          </VehicleCharges>
        </RentalRate>
        <TotalCharge>
          <xsl:attribute name="RateTotalAmount">
            <xsl:value-of select="translate(string(TotalCharge/@RateTotalAmount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="EstimatedTotalAmount">
            <xsl:value-of select="translate(string(TotalCharge/@EstimatedTotalAmount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="TotalCharge/@CurrencyCode"/>
          </xsl:attribute>
        </TotalCharge>
      </Vehicle>
    </Item>
  </xsl:template>
  <!--*****************************************************************-->
  <!--			Hotel Segs								    -->
  <!--*****************************************************************-->
  <xsl:template match="Hotel" mode="Hotel">
    <Item>
      <Hotel>
        <Reservation>
          <RoomTypes>
            <RoomType>
              <xsl:attribute name="RoomTypeCode">
                <xsl:value-of select="Reservation/RoomTypes/RoomType/@RoomTypeCode"/>
              </xsl:attribute>
              <xsl:attribute name="NumberOfUnits">
                <xsl:value-of select="Reservation/RoomTypes/RoomType/@NumberOfUnits"/>
              </xsl:attribute>
            </RoomType>
          </RoomTypes>
          <RoomRates>
            <RoomRate>
              <Rates>
                <Rate>
                  <Base>
                    <xsl:attribute name="AmountBeforeTax">
                      <xsl:value-of select="translate(string(Reservation/RoomRates/RoomRate/Rates/Rate/Base/@AmountBeforeTax),'.','')"/>
                    </xsl:attribute>
                    <xsl:attribute name="CurrencyCode">
                      <xsl:value-of select="Reservation/RoomRates/RoomRate/Rates/Rate/Base/@CurrencyCode"/>
                    </xsl:attribute>
                  </Base>
                </Rate>
              </Rates>
            </RoomRate>
          </RoomRates>
          <GuestCounts>
            <GuestCount>
              <xsl:attribute name="AgeQualifyingCode">
                <xsl:value-of select="Reservation/GuestCounts/GuestCount/@AgeQualifyingCode"/>
              </xsl:attribute>
              <xsl:attribute name="Count">
                <xsl:value-of select="Reservation/GuestCounts/GuestCount/@Count"/>
              </xsl:attribute>
            </GuestCount>
          </GuestCounts>
          <TimeSpan>
            <xsl:attribute name="Start">
              <xsl:value-of select="Reservation/TimeSpan/@Start"/>
            </xsl:attribute>
            <xsl:attribute name="Duration">
              <xsl:value-of select="Reservation/TimeSpan/@Duration"/>
            </xsl:attribute>
            <xsl:attribute name="End">
              <xsl:value-of select="Reservation/TimeSpan/@End"/>
            </xsl:attribute>
          </TimeSpan>
          <BasicPropertyInfo>
            <xsl:attribute name="ChainCode">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@ChainCode"/>
            </xsl:attribute>
            <xsl:attribute name="HotelCityCode">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCityCode"/>
            </xsl:attribute>
            <xsl:attribute name="HotelCode">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCode"/>
            </xsl:attribute>
            <xsl:attribute name="HotelName">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@HotelName"/>
            </xsl:attribute>
          </BasicPropertyInfo>
        </Reservation>
        <TPA_Extensions>
          <xsl:attribute name="ConfirmationNumber">
            <xsl:value-of select="TPA_Extensions/ConfirmationNumber"/>
          </xsl:attribute>
        </TPA_Extensions>
      </Hotel>
    </Item>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			           -->
  <!--***********************************************************************************-->
  <xsl:template match="PriceQuote" mode="Air">
    <AirFareInfo>
      <ItinTotalFare>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(string(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
          </xsl:attribute>
        </BaseFare>
        <EquivFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(string(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
          </xsl:attribute>
        </EquivFare>
        <Taxes>
          <xsl:variable name="TaxAmountNoDec">
            <xsl:value-of select="translate(sum(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax/@Amount),'.','')"/>
          </xsl:variable>
          <xsl:attribute name="Amount">
            <xsl:value-of select="$TaxAmountNoDec"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax/@Amount,'.'))"/>
          </xsl:attribute>
          <xsl:apply-templates select="PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax" mode="TotalFare"/>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(string(ItinTotalFare/TotalFare/@Amount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="string-length(substring-after(PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@Amount,'.'))"/>
          </xsl:attribute>
        </TotalFare>
      </ItinTotalFare>
      <PTC_FareBreakdowns>
        <xsl:apply-templates select="PTC_FareInfo"/>
      </PTC_FareBreakdowns>
    </AirFareInfo>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			           -->
  <!--***********************************************************************************-->
  <xsl:template match="ItineraryPricing" mode="Fare">
    <xsl:variable name="dect">
      <xsl:choose>
        <xsl:when test="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
          <xsl:value-of select="string-length(substring-after(PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount,'.'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="string-length(substring-after(PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@Amount,'.'))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <AirFareInfo>
      <ItinTotalFare>
        <BaseFare>
          <xsl:choose>
            <xsl:when test="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@Amount!=''">
              <xsl:attribute name="Amount">
                <xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
                  <xsl:with-param name="totalbf">0</xsl:with-param>
                  <xsl:with-param name="pos">1</xsl:with-param>
                  <xsl:with-param name="bfcount">
                    <xsl:value-of select="count(PriceQuote)+1"/>
                  </xsl:with-param>
                </xsl:apply-templates>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare/@CurrencyCode"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Amount">
                <xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
                  <xsl:with-param name="totalbf">0</xsl:with-param>
                  <xsl:with-param name="pos">1</xsl:with-param>
                  <xsl:with-param name="bfcount">
                    <xsl:value-of select="count(PriceQuote)+1"/>
                  </xsl:with-param>
                </xsl:apply-templates>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </BaseFare>
        <Taxes>
          <xsl:attribute name="Amount">
            <xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax">
              <xsl:with-param name="totalbf">0</xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
              <xsl:with-param name="bfcount">
                <xsl:value-of select="count(PriceQuote)+1"/>
              </xsl:with-param>
            </xsl:apply-templates>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dect"/>
          </xsl:attribute>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:apply-templates select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
              <xsl:with-param name="totalbf">0</xsl:with-param>
              <xsl:with-param name="pos">1</xsl:with-param>
              <xsl:with-param name="bfcount">
                <xsl:value-of select="count(PriceQuote)+1"/>
              </xsl:with-param>
            </xsl:apply-templates>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PriceQuote[1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dect"/>
          </xsl:attribute>
        </TotalFare>
      </ItinTotalFare>
      <PTC_FareBreakdowns>
        <xsl:apply-templates select="PriceQuote/PricedItinerary/AirItineraryPricingInfo"/>
      </PTC_FareBreakdowns>
    </AirFareInfo>
  </xsl:template>
  <xsl:template match="BaseFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[$pos + 1]">
        <xsl:apply-templates select="../../../../..//PriceQuote[$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/BaseFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="EquivFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[$pos + 1]">
        <xsl:apply-templates select="../../../../..//PriceQuote[$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/EquivFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Tax">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and ../../../../../../PriceQuote[$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax/@Amount!=''">
        <xsl:apply-templates select="../../../../../../PriceQuote[$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/Taxes/Tax[@Amount!='']">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="TotalFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(@Amount,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="../../PassengerTypeQuantity/@Quantity"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and ../../../../../PriceQuote[$pos + 1]">
        <xsl:apply-templates select="../../../../..//PriceQuote[$pos + 1]/PricedItinerary/AirItineraryPricingInfo/ItinTotalFare/TotalFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template name="calcbf">
    <xsl:param name="bf"/>
    <xsl:param name="totalbf"/>
    <xsl:if test="$bf != ''">
      <xsl:variable name="temp">
        <xsl:value-of select="substring-before($bf,'/')"/>
      </xsl:variable>
      <xsl:call-template name="calcbf">
        <xsl:with-param name="bf">
          <xsl:value-of select="substring-after($bf,'/')"/>
        </xsl:with-param>
        <xsl:with-param name="totalbf">
          <xsl:value-of select="$totalbf + $temp"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
    <xsl:value-of select="$totalbf"/>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Individual Tax element 	 	      			                -->
  <!--***********************************************************************************-->
  <xsl:template match="Tax" mode="TotalFare">
    <Tax>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(string(@Amount),'.','')"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="../../../ItinTotalFare/TotalFare/@CurrencyCode"/>
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="string-length(substring-after(@Amount,'.'))"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                 -->
  <!--************************************************************************************-->
  <xsl:template match="AirItineraryPricingInfo">
    <xsl:variable name="dect1">
      <xsl:choose>
        <xsl:when test="ItinTotalFare/EquivFare/@Amount!=''">
          <xsl:value-of select="string-length(substring-after(ItinTotalFare/EquivFare/@Amount,'.'))"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="string-length(substring-after(ItinTotalFare/BaseFare/@Amount,'.'))"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:choose>
          <xsl:when test="PassengerTypeQuantity/@Code='C09'">
            <xsl:attribute name="Code">CHD</xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="Code">
              <xsl:value-of select="PassengerTypeQuantity/@Code"/>
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="PassengerTypeQuantity/@Quantity"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:if test="PTC_FareBreakdown/FareBasis/@Code">
        <FareBasisCodes>
          <xsl:variable name="fbc">
            <xsl:value-of select="PTC_FareBreakdown/FareBasis/@Code"/>
          </xsl:variable>
          <xsl:call-template name="farebasis">
            <xsl:with-param name="fbc">
              <xsl:value-of select="$fbc"/>
            </xsl:with-param>
          </xsl:call-template>
        </FareBasisCodes>
      </xsl:if>
      <PassengerFare>
        <BaseFare>
          <xsl:choose>
            <xsl:when test="ItinTotalFare/EquivFare/@Amount!=''">
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect1"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Amount">
                <xsl:value-of select="translate(string(ItinTotalFare/BaseFare/@Amount),'.','')"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect1"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="ItinTotalFare/BaseFare/@CurrencyCode"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
        </BaseFare>
        <xsl:if test="ItinTotalFare/BaseFare/EquivFare">
          <EquivFare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="translate(string(ItinTotalFare/EquivFare/@Amount),'.','')"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect1"/>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="ItinTotalFare/EquivFare/@CurrencyCode"/>
            </xsl:attribute>
          </EquivFare>
        </xsl:if>
        <Taxes>
          <xsl:apply-templates select="ItinTotalFare/Taxes/Tax" mode="PTC"/>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:value-of select="translate(string(ItinTotalFare/TotalFare/@Amount),'.','')"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="$dect1"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="ItinTotalFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
      <TPA_Extensions>
        <xsl:if test="../../PricedItinerary/@ValidatingCarrier != ''">
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="../../PricedItinerary/@ValidatingCarrier"/>
            </xsl:attribute>
          </Vendor>
        </xsl:if>
        <BaggageAllowance>
          <xsl:attribute name="Number">
            <xsl:value-of select="PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number"/>
          </xsl:attribute>
        </BaggageAllowance>
      </TPA_Extensions>
    </PTC_FareBreakdown>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Calculate Fare Totals per Passenger Type	 	                 -->
  <!--************************************************************************************-->
  <xsl:template match="AirFareInfo">
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Code"/>
        </xsl:attribute>
        <xsl:attribute name="Quantity">
          <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:if test="PTC_FareBreakdown/FareBasisCode">
        <FareBasisCodes>
          <xsl:variable name="fbc">
            <xsl:value-of select="PTC_FareBreakdown/FareBasisCode"/>
          </xsl:variable>
          <xsl:call-template name="farebasis">
            <xsl:with-param name="fbc">
              <xsl:value-of select="$fbc"/>
            </xsl:with-param>
          </xsl:call-template>
        </FareBasisCodes>
      </xsl:if>
      <PassengerFare>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:variable name="bf">
              <xsl:choose>
                <xsl:when test="PTC_FareBreakdown/PassengerFare/EquivFare/@Amount!=''">
                  <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/BaseFare/@Amount),'.','')"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="nip">
              <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
            </xsl:variable>
            <xsl:value-of select="$bf * $nip"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </BaseFare>
        <!--xsl:if test="PTC_FareBreakdown/PassengerFare/EquivFare">
					<EquivFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/EquivFare/@Amount),'.','')" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/BaseFare/@DecimalPlaces" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PTC_FareBreakdown/PassengerFare/EquivFare/@CurrencyCode" />
						</xsl:attribute>
					</EquivFare>
				</xsl:if-->
        <Taxes>
          <xsl:apply-templates select="PTC_FareBreakdown/PassengerFare//Taxes/Tax" mode="PTC"/>
        </Taxes>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:variable name="bf">
              <xsl:value-of select="translate(string(PTC_FareBreakdown/PassengerFare/TotalFare/@Amount),'.','')"/>
            </xsl:variable>
            <xsl:variable name="nip">
              <xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Quantity"/>
            </xsl:variable>
            <xsl:value-of select="$bf * $nip"/>
          </xsl:attribute>
          <xsl:attribute name="DecimalPlaces">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@DecimalPlaces"/>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
    </PTC_FareBreakdown>
  </xsl:template>
  <xsl:template name="farebasis">
    <xsl:param name="fbc"/>
    <xsl:if test="$fbc != ''">
      <FareBasisCode>
        <xsl:choose>
          <xsl:when test="contains($fbc,'/')">
            <xsl:value-of select="substring-before($fbc,'/')"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$fbc"/>
          </xsl:otherwise>
        </xsl:choose>
      </FareBasisCode>
      <xsl:call-template name="farebasis">
        <xsl:with-param name="fbc">
          <xsl:value-of select="substring-after($fbc,'/')"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <xsl:template match="FareBasisCode">
    <FareBasisCode>
      <xsl:value-of select="."/>
    </FareBasisCode>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Individual Tax element 	 	      			                -->
  <!--***********************************************************************************-->
  <xsl:template match="Tax" mode="PTC">
    <Tax>
      <xsl:attribute name="Amount">
        <xsl:variable name="bf">
          <xsl:value-of select="translate(string(@Amount),'.','')"/>
        </xsl:variable>
        <xsl:variable name="nip">
          <xsl:value-of select="../../..//PassengerTypeQuantity/@Quantity"/>
        </xsl:variable>
        <xsl:variable name="tottax">
          <xsl:value-of select="$bf * $nip"/>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$tottax='NaN'">0</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$tottax"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="string-length(substring-after(@Amount,'.'))"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="../../TotalFare/@CurrencyCode"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <!--************************************************************************************-->
  <!-- 						Telephone									    -->
  <!--************************************************************************************-->
  <xsl:template match="Telephone">
    <Telephone>
      <xsl:if test="@PhoneUseType != ''">
        <xsl:attribute name="PhoneUseType">
          <xsl:value-of select="@PhoneUseType"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="@AreaCityCode!=''">
        <xsl:attribute name="AreaCityCode">
          <xsl:value-of select="@AreaCityCode"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="PhoneNumber">
        <xsl:value-of select="@PhoneNumber"/>
      </xsl:attribute>
    </Telephone>
  </xsl:template>
  <!--************************************************************************************-->
  <!--		EmailAddress  Processing									    -->
  <!--************************************************************************************-->
  <xsl:template match="Email">
    <xsl:if test=".  != ''">
      <Email>
        <xsl:value-of select="."/>
      </Email>
    </xsl:if>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Address/Delivery Addres information						    -->
  <!--************************************************************************************-->
  <xsl:template match="Address">
    <Address>
      <xsl:attribute name="UseType">
        <xsl:choose>
          <xsl:when test="@Type = 'Client Address'">Billing</xsl:when>
          <xsl:otherwise>Mailing</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="contains(Text,',')">
          <StreetNmbr>
            <xsl:value-of select="substring-before(Text,',')"/>
          </StreetNmbr>
          <xsl:variable name="txt">
            <xsl:value-of select="substring-after(Text,',')"/>
          </xsl:variable>
          <CityName>
            <xsl:value-of select="substring-before($txt,',')"/>
          </CityName>
          <xsl:variable name="txt1">
            <xsl:value-of select="substring-after($txt,',')"/>
          </xsl:variable>
          <PostalCode>
            <xsl:value-of select="substring-before($txt1,',')"/>
          </PostalCode>
          <xsl:variable name="txt2">
            <xsl:value-of select="substring-after($txt1,',')"/>
          </xsl:variable>
          <StateProv>
            <xsl:value-of select="substring-before($txt2,',')"/>
          </StateProv>
          <xsl:variable name="txt3">
            <xsl:value-of select="substring-after($txt2,',')"/>
          </xsl:variable>
          <CountryName>
            <xsl:attribute name="Code">
              <xsl:value-of select="$txt3"/>
            </xsl:attribute>
          </CountryName>
        </xsl:when>
        <xsl:otherwise>
          <AddressLine>
            <xsl:value-of select="Text"/>
          </AddressLine>
        </xsl:otherwise>
      </xsl:choose>
    </Address>
  </xsl:template>
  <!-- ***********************************************************-->
  <!--  				Ticketing info      			     -->
  <!-- ********************************************************** -->
  <xsl:template match="Ticketing" mode="Ticketing">
    <Ticketing>
      <xsl:variable name="ttl1">
        <xsl:value-of select="substring-after(../ItineraryPricing/TPA_Extensions/TicketRecordInfo/Text[contains(.,'LAST DAY TO PURCHASE')],'LAST DAY TO PURCHASE ')"/>
      </xsl:variable>
      <xsl:choose>
        <xsl:when test="substring(@TicketTimeLimit,1,3)='TAW' and string-length(@TicketTimeLimit)>4">
          <xsl:attribute name="TicketTimeLimit">
            <xsl:variable name="ttl">
              <xsl:value-of select="substring(@TicketTimeLimit,8,5)"/>
            </xsl:variable>
            <xsl:variable name="ttld">
              <xsl:value-of select="substring($ttl,1,2)"/>
            </xsl:variable>
            <xsl:variable name="ttlm">
              <xsl:call-template name="month">
                <xsl:with-param name="month">
                  <xsl:value-of select="substring($ttl,3,3)"/>
                </xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="nd">
              <xsl:value-of select="substring(../../../TimeStamp,9,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:value-of select="substring(../../../TimeStamp,6,2)"/>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:value-of select="substring(../../../TimeStamp,1,4)"/>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$ttlm &lt; $nm">
                <xsl:value-of select="$ny + 1"/>
                <xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T00:00:00')"/>
              </xsl:when>
              <xsl:when test="$ttlm = $nm and $ttld &lt; ($nd - 2)">
                <xsl:value-of select="$ny + 1"/>
                <xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T00:00:00')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="@TicketTimeLimit='TAW/' and $ttl1!=''">
          <xsl:attribute name="TicketTimeLimit">
            <xsl:variable name="nd">
              <xsl:value-of select="substring($ttl1,1,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:call-template name="month">
                <xsl:with-param name="month">
                  <xsl:value-of select="substring($ttl1,3,3)"/>
                </xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:value-of select="substring(../../UpdatedBy/@CreateDateTime,1,4)"/>
            </xsl:variable>
            <xsl:value-of select="concat($ny,'-',$nm,'-',$nd,'T23:59:00')"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="@TicketTimeLimit='TAW/' and $ttl1=''">
          <xsl:attribute name="TicketTimeLimit">
            <xsl:variable name="nd">
              <xsl:value-of select="substring(../ReservationItems/Item[1]/FlightSegment/@DepartureDateTime,9,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:value-of select="substring(../ReservationItems/Item[1]/Air/@DepartureDateTime,6,2)"/>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:value-of select="substring(../ReservationItems/Item[1]/FlightSegment/@DepartureDateTime,1,4)"/>
            </xsl:variable>
            <xsl:value-of select="concat($ny,'-',$nm,'-',$nd,'T23:59:00')"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="substring(@TicketTimeLimit,1,2)='TL'">
          <xsl:attribute name="TicketTimeLimit">
            <xsl:variable name="ttl">
              <xsl:value-of select="substring-before(substring-after(@TicketTimeLimit,'/'),'-')"/>
            </xsl:variable>
            <xsl:variable name="ttld">
              <xsl:value-of select="substring($ttl,1,2)"/>
            </xsl:variable>
            <xsl:variable name="ttlm">
              <xsl:call-template name="month">
                <xsl:with-param name="month">
                  <xsl:value-of select="substring($ttl,3,3)"/>
                </xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="nd">
              <xsl:value-of select="substring(../../../TimeStamp,9,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:value-of select="substring(../../../TimeStamp,6,2)"/>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:value-of select="substring(../../../TimeStamp,1,4)"/>
            </xsl:variable>
            <xsl:variable name="tm1">
              <xsl:value-of select="substring-before(substring(@TicketTimeLimit,3),'/')"/>
            </xsl:variable>
            <xsl:variable name="tm">
              <xsl:choose>
                <xsl:when test="contains($tm1,'P')">
                  <xsl:variable name="tm2">
                    <xsl:value-of select="substring-before($tm1,'P')"/>
                  </xsl:variable>
                  <xsl:choose>
                    <xsl:when test="substring($tm2,1,2)='12' and string-length($tm2)=4">
                      <xsl:value-of select="$tm2"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="$tm2 + 1200"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:when>
                <xsl:when test="contains($tm1,'A')">
                  <xsl:variable name="tm2">
                    <xsl:value-of select="substring-before($tm1,'A')"/>
                  </xsl:variable>
                  <xsl:value-of select="format-number($tm2,'0000')"/>
                </xsl:when>
                <xsl:when test="contains($tm1,'N')">
                  <xsl:variable name="tm2">
                    <xsl:value-of select="substring-before($tm1,'N')"/>
                  </xsl:variable>
                  <xsl:value-of select="format-number($tm2,'0000')"/>
                </xsl:when>
              </xsl:choose>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$ttlm &lt; $nm">
                <xsl:value-of select="$ny + 1"/>
                <xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T',substring($tm,1,2),':',substring($tm,3),':00')"/>
              </xsl:when>
              <xsl:when test="$ttlm = $nm and $ttld &lt; ($nd - 2)">
                <xsl:value-of select="$ny + 1"/>
                <xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T',substring($tm,1,2),':',substring($tm,3),':00')"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T',substring($tm,1,2),':',substring($tm,3),':00')"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:when>
      </xsl:choose>
      <xsl:attribute name="TicketType">
        <xsl:choose>
          <xsl:when test="../ReservationItems/Item[1]/FlightSegment/@eTicket='true'">eTicket</xsl:when>
          <xsl:otherwise>Paper</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:if test="substring(@TicketTimeLimit,1,2)='T-' and substring(@TicketTimeLimit,8,1)='-'">
        <TicketAdvisory>
          <xsl:text>OK-</xsl:text>
          <xsl:variable name="ttl">
            <xsl:value-of select="substring(@TicketTimeLimit,3,5)"/>
          </xsl:variable>
          <xsl:variable name="ttld">
            <xsl:value-of select="substring($ttl,1,2)"/>
          </xsl:variable>
          <xsl:variable name="ttlm">
            <xsl:call-template name="month">
              <xsl:with-param name="month">
                <xsl:value-of select="substring($ttl,3,3)"/>
              </xsl:with-param>
            </xsl:call-template>
          </xsl:variable>
          <xsl:variable name="nd">
            <xsl:value-of select="substring(../../../TimeStamp,9,2)"/>
          </xsl:variable>
          <xsl:variable name="nm">
            <xsl:value-of select="substring(../../../TimeStamp,6,2)"/>
          </xsl:variable>
          <xsl:variable name="ny">
            <xsl:value-of select="substring(../../../TimeStamp,1,4)"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$ttlm &lt; $nm">
              <xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
            </xsl:when>
            <xsl:when test="$ttlm = $nm and $ttld &lt; $nd">
              <xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$ny - 1"/>
              <xsl:value-of select="concat('-',$ttlm,'-',$ttld,'T00:00:00')"/>
            </xsl:otherwise>
          </xsl:choose>
        </TicketAdvisory>
      </xsl:if>
    </Ticketing>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Form of Payment       						                       -->
  <!--************************************************************************************-->
  <xsl:template match="FormOfPayment">
    <xsl:attribute name="RPH">
      <xsl:value-of select="position()"/>
    </xsl:attribute>
    <PaymentCard>
      <xsl:attribute name="CardCode">
        <xsl:choose>
          <xsl:when test="PaymentCard/@CardType='CA'">MC</xsl:when>
          <xsl:when test="PaymentCard/@CardType='DI'">DS</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="PaymentCard/@CardType"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="CardNumber">
        <xsl:variable name="cardnum">
          <xsl:value-of select="PaymentCard/@CardNumber"/>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="string-length($cardnum) = 14">
            <xsl:text>xxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,11)"/>
          </xsl:when>
          <xsl:when test="string-length($cardnum) = 15">
            <xsl:text>xxxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,12)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>xxxxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,13)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="ExpireDate">
        <xsl:value-of select="PaymentCard/@ExpireDate"/>
      </xsl:attribute>
    </PaymentCard>
    <!--Note:  TPA_Extensions/@FOPType needs to be put on the schema so that it validates -->
    <TPA_Extensions>
      <xsl:attribute name="FOPType">CC</xsl:attribute>
    </TPA_Extensions>
  </xsl:template>
  <xsl:template match="PaymentForm">
    <xsl:attribute name="RPH">
      <xsl:value-of select="position()"/>
    </xsl:attribute>
    <PaymentCard>
      <xsl:attribute name="CardCode">
        <xsl:choose>
          <xsl:when test="substring(TPA_Extensions/Text,2,2)='CA'">MC</xsl:when>
          <xsl:when test="substring(TPA_Extensions/Text,2,2)='DI'">DS</xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="substring(TPA_Extensions/Text,2,2)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="CardNumber">
        <xsl:variable name="cardnum">
          <xsl:choose>
            <xsl:when test="substring(TPA_Extensions/Text,2,2) = 'AX'">
              <xsl:value-of select="substring(TPA_Extensions/Text,4,15)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(TPA_Extensions/Text,4,16)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="string-length($cardnum) = 14">
            <xsl:text>xxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,11)"/>
          </xsl:when>
          <xsl:when test="string-length($cardnum) = 15">
            <xsl:text>xxxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,12)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>xxxxxxxxxxxx</xsl:text>
            <xsl:value-of select="substring($cardnum,13)"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:attribute name="ExpireDate">
        <xsl:variable name="temp">
          <xsl:value-of select="substring-before(TPA_Extensions/Text,'/')"/>
        </xsl:variable>
        <xsl:variable name="templ">
          <xsl:value-of select="string-length($temp)"/>
        </xsl:variable>
        <xsl:value-of select="substring(TPA_Extensions/Text,$templ - 1,2)"/>
        <xsl:value-of select="substring-after(TPA_Extensions/Text,'/')"/>
      </xsl:attribute>
    </PaymentCard>
    <!--Note:  TPA_Extensions/@FOPType needs to be put on the schema so that it validates -->
    <TPA_Extensions>
      <xsl:attribute name="FOPType">CC</xsl:attribute>
    </TPA_Extensions>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Other Service Information (OSI) Processing				    -->
  <!--************************************************************************************-->
  <xsl:template match="Service" mode="OSI">
    <OtherServiceInformation>
      <Airline>
        <xsl:choose>
          <xsl:when test="Airline/@Code">
            <xsl:value-of select="Airline/@Code"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="substring(Text,1,2)"/>
          </xsl:otherwise>
        </xsl:choose>
      </Airline>
      <Text>
        <xsl:value-of select="Text"/>
      </Text>
    </OtherServiceInformation>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Special Service Request (SSR) Processing				    -->
  <!--************************************************************************************-->
  <xsl:template match="Service" mode="SSR">
    <SpecialServiceRequest>
      <xsl:attribute name="SSRCode">
        <xsl:value-of select="@SSR_Type"/>
      </xsl:attribute>
      <xsl:if test="PersonName/@NameNumber != ''">
        <xsl:attribute name="TravelerRefNumberRPHList">
          <xsl:value-of select="PersonName/@NameNumber"/>
        </xsl:attribute>
      </xsl:if>
      <Airline>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="Airline/@Code">
              <xsl:value-of select="Airline/@Code"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(Text,1,2)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </Airline>
      <Text>
        <xsl:value-of select="Text"/>
      </Text>
    </SpecialServiceRequest>
  </xsl:template>
  <!--************************************************************************************-->
  <xsl:template name="month">
    <xsl:param name="month"/>
    <xsl:choose>
      <xsl:when test="$month = 'JAN'">01</xsl:when>
      <xsl:when test="$month = 'FEB'">02</xsl:when>
      <xsl:when test="$month = 'MAR'">03</xsl:when>
      <xsl:when test="$month = 'APR'">04</xsl:when>
      <xsl:when test="$month = 'MAY'">05</xsl:when>
      <xsl:when test="$month = 'JUN'">06</xsl:when>
      <xsl:when test="$month = 'JUL'">07</xsl:when>
      <xsl:when test="$month = 'AUG'">08</xsl:when>
      <xsl:when test="$month = 'SEP'">09</xsl:when>
      <xsl:when test="$month = 'OCT'">10</xsl:when>
      <xsl:when test="$month = 'NOV'">11</xsl:when>
      <xsl:when test="$month = 'DEC'">12</xsl:when>
    </xsl:choose>
  </xsl:template>
  <!-- ********************************************************************************	-->
  <!-- Miscellaneous other,  Air Taxi, Land, Sea, Rail, Car and hotel               -->
  <!-- ********************************************************************************* -->
  <xsl:template match="TPA_Extensions" mode="Other">
    <!--General Segment in OTA -->
    <!-- Other Segments -->
    <Item>
      <xsl:attribute name="Status">
        <xsl:value-of select="Line/@Status"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:choose>
          <xsl:when test="substring(Line/@Number,1,3)='000'">
            <xsl:value-of select="substring(Line/@Number,4)"/>
          </xsl:when>
          <xsl:when test="substring(Line/@Number,1,2)='00'">
            <xsl:value-of select="substring(Line/@Number,3)"/>
          </xsl:when>
          <xsl:when test="substring(Line/@Number,1,1)='0'">
            <xsl:value-of select="substring(Line/@Number,2)"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="Line/@Number"/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <General>
        <xsl:attribute name="Start">
          <xsl:value-of select="substring(DateTime,1,10)"/>
        </xsl:attribute>
        <Description>
          <xsl:value-of select="Text"/>
        </Description>
        <TPA_Extensions>
          <xsl:attribute name="Status">
            <xsl:value-of select="Line/@Status"/>
          </xsl:attribute>
          <xsl:attribute name="NumberInParty">
            <xsl:value-of select="substring(NumberInParty,2)"/>
          </xsl:attribute>
          <Vendor>
            <xsl:attribute name="Code">
              <xsl:value-of select="Vendor/@Code"/>
            </xsl:attribute>
          </Vendor>
          <OriginCityCode>
            <xsl:value-of select="Location/@LocationCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </General>
    </Item>
  </xsl:template>
</xsl:stylesheet>
