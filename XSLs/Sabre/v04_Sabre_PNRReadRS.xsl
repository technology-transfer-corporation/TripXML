<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:stl="http://services.sabre.com/STL/v01"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <!-- 
  ================================================================== 
  v04_Sabre_PNRReadRS.xsl 														
  ==================================================================
  Date: 31 May 2021 - Kobelev - Upgraded to GetReservationRS version 1.9.0.  	
  ================================================================== 
  -->
  <xsl:output omit-xml-declaration="yes"/>

  <xsl:variable name="PNR">
    <xsl:value-of select="GetReservationRS/Reservation/BookingDetails/RecordLocator"/>
  </xsl:variable>

  <xsl:template match="/">
    <OTA_TravelItineraryRS>
      <xsl:attribute name="Version">v04</xsl:attribute>
      <xsl:if test="GetReservationRS/EchoToken!=''">
        <xsl:attribute name="EchoToken">
          <xsl:value-of select="GetReservationRS/EchoToken"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:apply-templates select="GetReservationRS"/>
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
        <xsl:if test="PNRReply/EchoToken!=''">
          <xsl:attribute name="EchoToken">
            <xsl:value-of select="PNRReply/EchoToken"/>
          </xsl:attribute>
        </xsl:if>
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
      <xsl:if test="GetReservationRS/ConversationID!='' and not(ErrorRS/TPA_Extensions/ErrorInfo) and not(EndTransactionRS/Errors) and not(PNRReply)">
        <ConversationID>
          <xsl:value-of select="GetReservationRS/ConversationID"/>
        </ConversationID>
      </xsl:if>
    </OTA_TravelItineraryRS>
  </xsl:template>
  <!--************************************************************************************************************-->
  <xsl:template match="GetReservationRS">
    <xsl:if test="Ticketed">
      <xsl:attribute name="TransactionStatusCode">Ticketed</xsl:attribute>
    </xsl:if>
    <xsl:choose>
      <xsl:when test="Errors/Error != ''">
        <Errors>
          <Error>
            <xsl:attribute name="Type">Sabre</xsl:attribute>
            <xsl:attribute name="Code">
              <xsl:choose>
                <xsl:when test="Errors/Error/@ErrorCode!= ''">
                  <xsl:value-of select="Errors/Error/@ErrorCode"/>
                </xsl:when>
                <xsl:otherwise>E</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="Errors/Error"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:when test="ApplicationResults/@status='NotProcessed'">
        <Errors>
          <Error>
            <xsl:attribute name="Type">
              <xsl:value-of select="ApplicationResults/Error/@type"/>
            </xsl:attribute>
            <xsl:value-of select="ApplicationResults/Error/SystemSpecificResults/ShortText"/>
          </Error>
        </Errors>
      </xsl:when>
      <xsl:otherwise>
        <Success/>
        <xsl:if test="Warning!=''">
          <Warnings>
            <xsl:apply-templates select="Warning"/>
          </Warnings>
        </xsl:if>
        <TravelItinerary>
          <ItineraryRef>
            <xsl:attribute name="Type">PNR</xsl:attribute>
            <xsl:attribute name="ID">
              <xsl:value-of select="Reservation/BookingDetails/RecordLocator"/>
            </xsl:attribute>
            <xsl:attribute name="ID_Context">
              <xsl:value-of select="Reservation/POS/Source/@PseudoCityCode"/>
            </xsl:attribute>
            <xsl:choose>
              <xsl:when test="PriceQuote/PriceQuoteInfo/Details[1]/AgentInfo/WorkLocation">
                <CompanyName>
                  <xsl:variable name="pricedPCC">
                    <xsl:value-of select="PriceQuote/PriceQuoteInfo/Details[1]/AgentInfo/WorkLocation"/>
                  </xsl:variable>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="$pricedPCC"/>
                  </xsl:attribute>
                  <xsl:attribute name="CodeContext">
                    <xsl:value-of select="TravelItinerary/ItineraryRef/OfficeStationCode"/>
                  </xsl:attribute>
                  <xsl:value-of select="concat($pricedPCC, '/', Reservation/POS/Source/@PseudoCityCode)"/>
                </CompanyName>
              </xsl:when>
              <xsl:when test="Reservation/POS/Source/@HomePseudoCityCode">
                <CompanyName>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="Reservation/POS/Source/@HomePseudoCityCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="CodeContext">
                    <xsl:value-of select="Reservation/POS/OfficeStationCode"/>
                  </xsl:attribute>
                  <xsl:value-of select="concat(Reservation/POS/Source/@HomePseudoCityCode, '/', Reservation/POS/Source/@PseudoCityCode)"/>
                </CompanyName>
              </xsl:when>
            </xsl:choose>
          </ItineraryRef>
          <xsl:apply-templates select="Reservation"/>
        </TravelItinerary>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="Warning">
    <Warning>
      <xsl:value-of select="normalize-space(translate(.,'Â',''))"/>
    </Warning>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			TravelItinerary detail Information                                             -->
  <!--************************************************************************************-->
  <xsl:template match="Reservation">
    <!--******************************************************-->
    <!--				Names                                      -->
    <!--******************************************************-->
    <CustomerInfos>
      <xsl:variable name="pd">
        <xsl:value-of select="../SabreCommandLLSRS/Response"/>
      </xsl:variable>
      <xsl:apply-templates select="PassengerReservation/Passengers/Passenger">
        <xsl:with-param name="pd">
          <xsl:value-of select="$pd"/>
        </xsl:with-param>
      </xsl:apply-templates>
    </CustomerInfos>
    <!--******************************************************-->
    <!--			Air Itinerary                                      -->
    <!--******************************************************-->
    <ItineraryInfo>
      <xsl:if test="PassengerReservation/Segments/Segment">
        <ReservationItems>
          <xsl:apply-templates select="PassengerReservation/Segments/Segment/Air | PassengerReservation/Segments/Segment/Arunk" mode="Air">
            <xsl:sort data-type="number" order="ascending" select="@sequence"/>
          </xsl:apply-templates>
          <xsl:apply-templates select="PassengerReservation/Segments/Segment/Vehicle" mode="Car"/>
          <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Hotel" mode="Hotel"/>
          <xsl:if test="PassengerReservation/Segments/Segment/General[Line/@Type='OTH']">
            <xsl:apply-templates select="PassengerReservation/Segments/Segment/General[Line/@Type='OTH']" mode="Other"/>
          </xsl:if>
          <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Misc" mode="Misc"/>
          <xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/MiscSegment" mode="Misc"/>
          <xsl:if test="../PriceQuote/PriceQuoteInfo[Summary/NameAssociation/PriceQuote[@latestPQFlag='true']]">
            <xsl:variable name="PQnum" select="../PriceQuote/PriceQuoteInfo/Summary/NameAssociation/PriceQuote[@latestPQFlag='true']/@number"/>
            <xsl:if test="../PriceQuote/PriceQuoteInfo/Details[@number=$PQnum]/FareInfo/BaseFare">
              <ItemPricing>
                <xsl:apply-templates select="../PriceQuote/PriceQuoteInfo[Summary/NameAssociation/PriceQuote[@latestPQFlag='true']]" mode="Fare"/>
              </ItemPricing>
            </xsl:if>
          </xsl:if>
        </ReservationItems>
      </xsl:if>
      <xsl:apply-templates select="PassengerReservation/TicketingInfo" mode="Ticketing"/>
      <xsl:if test="OpenReservationElements/OpenReservationElement[ServiceRequest/@serviceType='SSR'] or RemarkInfo != '' or PassengerReservation/Segments/Segment/Air/Seats/PreReservedSeats/PreReservedSeat">
        <SpecialRequestDetails>
          <xsl:if test="PassengerReservation/Segments/Segment/Air/Seats/PreReservedSeats/PreReservedSeat">
            <SeatRequests>
              <xsl:apply-templates select="PassengerReservation/Segments/Segment/Air/Seats/PreReservedSeats/PreReservedSeat" mode="Seat"/>
            </SeatRequests>
          </xsl:if>
          <xsl:if test="OpenReservationElements/OpenReservationElement[ServiceRequest/@serviceType='SSR']">
            <SpecialServiceRequests>
              <xsl:apply-templates select="OpenReservationElements/OpenReservationElement[ServiceRequest/@serviceType='SSR']" mode="SSR"/>
            </SpecialServiceRequests>
          </xsl:if>
          <xsl:if test="../PriceQuote/PriceQuoteInfo/Details/MiscellaneousInfo/TourNumber!='' or ../PriceQuote/PriceQuoteInfo/Details/MessageInfo/Remarks[@type='ENS']">
            <SpecialRemarks>
              <xsl:if test="../PriceQuote/PriceQuoteInfo/Details/MiscellaneousInfo/TourNumber!=''">
                <xsl:apply-templates select="../PriceQuote/PriceQuoteInfo/Details[MiscellaneousInfo/TourNumber!='']" mode="TourCode"/>
              </xsl:if>
              <xsl:if test="../PriceQuote/PriceQuoteInfo/Details/MessageInfo/Remarks[@type='ENS']">
                <xsl:apply-templates select="../PriceQuote/PriceQuoteInfo/Details/MessageInfo[Remarks/@type='ENS']" mode="Endorsement"/>
              </xsl:if>
            </SpecialRemarks>
          </xsl:if>
          <xsl:if test="OpenReservationElements/OpenReservationElement[ServiceRequest/@serviceType='OSI']">
            <OtherServiceInformations>
              <xsl:apply-templates select="OpenReservationElements/OpenReservationElement[ServiceRequest/@serviceType='OSI']" mode="OSI"/>
            </OtherServiceInformations>
          </xsl:if>
          <xsl:if test="Remarks!= ''">
            <Remarks>
              <xsl:for-each select="Remarks/Remark">
                <xsl:if test="@type!='FOP'">
                  <Remark>
                    <xsl:attribute name="RPH">
                      <xsl:value-of select="format-number(@index,'#0')"/>
                    </xsl:attribute>
                    <xsl:attribute name="Category">
                      <xsl:choose>
                        <xsl:when test="@type='HS'">Historical</xsl:when>
                        <xsl:when test="@type='INVOICE'">Invoice</xsl:when>
                        <xsl:when test="@type='CODED'">Coded</xsl:when>
                        <xsl:when test="@type='CLIADR'">Client Address</xsl:when>
                        <xsl:otherwise>General</xsl:otherwise>
                      </xsl:choose>
                    </xsl:attribute>
                    <xsl:if test="@SegmentNumber!=''">
                      <xsl:attribute name="FlightRefNumberRPHList">
                        <xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
                      </xsl:attribute>
                    </xsl:if>
                    <xsl:choose>
                      <xsl:when test="RemarkLines/RemarkLine/Text!=''">
                        <xsl:value-of select="RemarkLines/RemarkLine/Text"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="' '"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Remark>
                </xsl:if>
              </xsl:for-each>
            </Remarks>
          </xsl:if>
        </SpecialRequestDetails>
      </xsl:if>
      <xsl:variable name="dqb">
        <xsl:call-template name="ParseDQBLines">
          <xsl:with-param name="LostText" select="../SabreCommandLLSRS/Response[contains(.,'SALES AUDIT REPORT')]"/>
        </xsl:call-template>
      </xsl:variable>
      <xsl:if test="PassengerReservation/TicketingInfo[ETicketNumber] or $dqb!=''">
        <TPA_Extensions>
          <IssuedTickets>
            <xsl:apply-templates select="PassengerReservation/TicketingInfo[ETicketNumber]/TicketDetails" mode="IssuedTicket"/>
            <xsl:if test="$dqb!=''">
              <xsl:copy-of select="$dqb"/>
            </xsl:if>
          </IssuedTickets>
        </TPA_Extensions>
      </xsl:if>
    </ItineraryInfo>
    <!--******************************************************-->
    <!--			Form of Payment                               -->
    <!--******************************************************-->
    <xsl:choose>
      <xsl:when test="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Check">
        <TravelCost>
          <xsl:for-each select="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Check">
            <FormOfPayment>
              <xsl:attribute name="RPH">
                <xsl:variable name="ccid">
                  <xsl:value-of select="../../@id"/>
                </xsl:variable>
                <xsl:value-of select="format-number(../../../../Remarks/Remark[@id=$ccid]/@index,'#0')"/>
              </xsl:attribute>
              <DirectBill>
                <xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
              </DirectBill>
            </FormOfPayment>
          </xsl:for-each>
        </TravelCost>
      </xsl:when>
      <xsl:when test="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Cash">
        <TravelCost>
          <xsl:for-each select="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Cash">
            <FormOfPayment>
              <xsl:attribute name="RPH">
                <xsl:variable name="ccid">
                  <xsl:value-of select="../../@id"/>
                </xsl:variable>
                <xsl:value-of select="format-number(../../../../Remarks/Remark[@id=$ccid]/@index,'#0')"/>
              </xsl:attribute>
              <DirectBill>
                <xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
              </DirectBill>
            </FormOfPayment>
          </xsl:for-each>
        </TravelCost>
      </xsl:when>
      <xsl:when test="CustomerInfo/PaymentInfo/Payment/Form[position()=2] = 'CHECK' or CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CK' or CustomerInfo/PaymentInfo/Payment/Form[position()=1] = 'CHEQUE'">
        <TravelCost>
          <FormOfPayment>
            <xsl:attribute name="RPH">
              <xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=2]/@RPH,'#0')"/>
            </xsl:attribute>
            <DirectBill>
              <xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
            </DirectBill>
          </FormOfPayment>
        </TravelCost>
      </xsl:when>
      <xsl:when test="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text='CS'">
        <TravelCost>
          <xsl:for-each select="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other[Text='CS']">
            <FormOfPayment>
              <xsl:attribute name="RPH">
                <xsl:variable name="ccid">
                  <xsl:value-of select="../../@id"/>
                </xsl:variable>
                <xsl:value-of select="format-number(../../../../Remarks/Remark[@id=$ccid]/@index,'#0')"/>
              </xsl:attribute>
              <DirectBill>
                <xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
              </DirectBill>
            </FormOfPayment>
          </xsl:for-each>
        </TravelCost>
      </xsl:when>
      <xsl:when test="CustomerInfo/PaymentInfo/Payment/Form[position()=2] = 'CASH'">
        <TravelCost>
          <FormOfPayment>
            <xsl:attribute name="RPH">
              <xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form[position()=2]/@RPH,'#0')"/>
            </xsl:attribute>
            <DirectBill>
              <xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
            </DirectBill>
          </FormOfPayment>
        </TravelCost>
      </xsl:when>
      <xsl:when test="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/PaymentCard">
        <xsl:for-each select="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/PaymentCard">
          <xsl:variable name="card">
            <xsl:value-of select="CustomerInfo/PaymentInfo/Payment/Form/TPA_Extensions/Text"/>
          </xsl:variable>
          <TravelCost>
            <FormOfPayment>
              <xsl:attribute name="RPH">
                <xsl:variable name="ccid">
                  <xsl:value-of select="../../@id"/>
                </xsl:variable>
                <xsl:value-of select="format-number(../../../../Remarks/Remark[@id=$ccid]/@index,'#0')"/>
              </xsl:attribute>
              <PaymentCard>
                <xsl:attribute name="CardCode">
                  <xsl:choose>
                    <xsl:when test="CardCode='CA'">MC</xsl:when>
                    <xsl:when test="CardCode='MA'">MC</xsl:when>
                    <xsl:when test="CardCode='DC'">DN</xsl:when>
                    <xsl:when test="CardCode='DI'">DS</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="CardCode"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="CardNumber">
                  <xsl:value-of select="CardNumber"/>
                </xsl:attribute>
                <xsl:attribute name="ExpireDate">
                  <xsl:value-of select="substring(ExpiryMonth,3)"/>
                  <xsl:value-of select="concat('/',substring(ExpiryYear,3))"/>
                </xsl:attribute>
              </PaymentCard>
            </FormOfPayment>
          </TravelCost>
        </xsl:for-each>
      </xsl:when>
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
      <xsl:when test="contains(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text,'XXXXX')">
        <xsl:variable name="card">
          <xsl:choose>
            <xsl:when test="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text,3,1)>=0 and substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text,3,1)&lt;=9">
              <xsl:value-of select="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text,2)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <TravelCost>
          <FormOfPayment>
            <xsl:attribute name="RPH">
              <xsl:value-of select="format-number(../../@displayIndex,'#0')"/>
            </xsl:attribute>
            <PaymentCard>
              <xsl:attribute name="CardCode">
                <xsl:choose>
                  <xsl:when test="substring($card,1,2)='CA'">MC</xsl:when>
                  <xsl:when test="substring($card,1,2)='MA'">MC</xsl:when>
                  <xsl:when test="substring($card,1,2)='DC'">DN</xsl:when>
                  <xsl:when test="substring($card,1,2)='DI'">DS</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="substring($card,1,2)"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="CardNumber">
                <xsl:value-of select="substring-before(substring($card,3),'Â')"/>
                <xsl:value-of select="substring-before(substring($card,3),'?')"/>
                <xsl:value-of select="substring-before(substring($card,3),'¥')"/>
              </xsl:attribute>
              <xsl:attribute name="ExpireDate">
                <xsl:variable name="expdate">
                  <xsl:value-of select="substring-after(substring($card,3),'Â')"/>
                  <xsl:value-of select="substring-after(substring($card,3),'?')"/>
                  <xsl:value-of select="substring-after(substring($card,3),'¥')"/>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="contains($expdate,'-')">
                    <xsl:value-of select="translate(substring-before($expdate,'-'),'/','')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="translate($expdate,'/','')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </PaymentCard>
          </FormOfPayment>
        </TravelCost>
      </xsl:when>
      <!--xsl:when test="contains(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text,'XXXXX')">
				<xsl:variable name="card">
					<xsl:value-of select="OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text"/>
				</xsl:variable>
				<TravelCost>
					<FormOfPayment>
						<xsl:attribute name="RPH"><xsl:value-of select="format-number(CustomerInfo/PaymentInfo/Payment/Form/@RPH,'#0')"/></xsl:attribute>
						<PaymentCard>
							<xsl:attribute name="CardCode"><xsl:choose><xsl:when test="substring($card,2,2)='CA'">MC</xsl:when><xsl:when test="substring($card,1,2)='MA'">MC</xsl:when><xsl:when test="substring($card,1,2)='DC'">DN</xsl:when><xsl:when test="substring($card,2,2)='DI'">DS</xsl:when><xsl:otherwise><xsl:value-of select="substring($card,2,2)"/></xsl:otherwise></xsl:choose></xsl:attribute>
							<xsl:attribute name="CardNumber"><xsl:value-of select="substring-before(substring($card,4),'Â')"/></xsl:attribute>
							<xsl:attribute name="ExpireDate">XXXX</xsl:attribute>
						</PaymentCard>
					</FormOfPayment>
				</TravelCost>
			</xsl:when-->
      <xsl:when test="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=1],1,1) = '*' or substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=2],1,1) = '*'">
        <xsl:variable name="card">
          <xsl:choose>
            <xsl:when test="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=2],1,1) = '*'">
              <xsl:value-of select="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=2],2)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=1],2)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:variable>
        <xsl:if test="contains($card,'')">
          <TravelCost>
            <FormOfPayment>
              <xsl:attribute name="RPH">
                <xsl:choose>
                  <xsl:when test="substring(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=2],1,1) = '*'">
                    <xsl:value-of select="format-number(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=2]/@RPH,'#0')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="format-number(OpenReservationElements/OpenReservationElement[@type='FP']/FormOfPayment/Other/Text[position()=1]/@RPH,'#0')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <PaymentCard>
                <xsl:attribute name="CardCode">
                  <xsl:choose>
                    <xsl:when test="substring($card,1,2)='CA'">MC</xsl:when>
                    <xsl:when test="substring($card,1,2)='DI'">DS</xsl:when>
                    <xsl:when test="substring($card,1,2)='MA'">MC</xsl:when>
                    <xsl:when test="substring($card,1,2)='DC'">DN</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="substring($card,1,2)"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="CardNumber">
                  <xsl:value-of select="substring-before(substring($card,3),'')"/>
                </xsl:attribute>
                <xsl:attribute name="ExpireDate">
                  <xsl:value-of select="translate(substring-after($card,''),'/','')"/>
                </xsl:attribute>
              </PaymentCard>
            </FormOfPayment>
          </TravelCost>
        </xsl:if>
      </xsl:when>
    </xsl:choose>
    <UpdatedBy>
      <xsl:attribute name="CreateDateTime">
        <xsl:value-of select="BookingDetails/CreationTimestamp"/>
      </xsl:attribute>
    </UpdatedBy>
    <xsl:if test="DKNumbers/DKNumber!='' or AccountingLines/AccountingLine or PassengerReservation/ItineraryPricing/FuturePriceInfo">
      <TPA_Extensions>
        <xsl:if test="DKNumbers/DKNumber!=''">
          <AccountingLine>
            <xsl:value-of select="translate(DKNumbers/DKNumber,' ','')"/>
          </AccountingLine>
        </xsl:if>
        <xsl:if test="AccountingLines/AccountingLine">
          <xsl:for-each select="AccountingLines/AccountingLine">
            <AccountingInfo RPH="{@index}">
              <xsl:if test="@LinkCode!=''">
                <xsl:attribute name="LinkCode">
                  <xsl:value-of select="@LinkCode"/>
                </xsl:attribute>
              </xsl:if>
              <xsl:if test="@Type!=''">
                <xsl:attribute name="Type">
                  <xsl:value-of select="@Type"/>
                </xsl:attribute>
              </xsl:if>
              <xsl:if test="AirlineDesignator!=''">
                <AccountVendor Code="{AirlineDesignator}"/>
              </xsl:if>
              <xsl:if test="Airline/@Code!=''">
                <AccountVendor Code="{Airline/@Code}"/>
              </xsl:if>
              <BaseFare>
                <xsl:choose>
                  <xsl:when test="BaseFare='0.00'">
                    <xsl:attribute name="Amount">0</xsl:attribute>
                    <xsl:attribute name="DecimalPlaces">
                      <xsl:value-of select="'0'"/>
                    </xsl:attribute>
                  </xsl:when>
                  <xsl:when test="contains(BaseFare,'.')">
                    <xsl:attribute name="Amount">
                      <xsl:value-of select="translate(BaseFare,'.','')"/>
                    </xsl:attribute>
                    <xsl:attribute name="DecimalPlaces">
                      <xsl:value-of select="'2'"/>
                    </xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="Amount">
                      <xsl:value-of select="BaseFare"/>
                    </xsl:attribute>
                    <xsl:attribute name="DecimalPlaces">
                      <xsl:value-of select="'0'"/>
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
              </BaseFare>
              <xsl:if test="DocumentNumber!=''">
                <DocumentInfo Number="{DocumentNumber}"/>
              </xsl:if>
              <xsl:if test="ChargeCategory!=''">
                <ChargeCategory>
                  <xsl:value-of select="ChargeCategory"/>
                </ChargeCategory>
              </xsl:if>
              <xsl:if test="FareApplication!=''">
                <FareApplication>
                  <xsl:value-of select="FareApplication"/>
                </FareApplication>
              </xsl:if>
              <xsl:if test="CommissionAmount!='' or CreditCardCode!='XX'">
                <PaymentInfo>
                  <xsl:if test="CommissionAmount!=''">
                    <xsl:variable name="comAmt">
                      <xsl:value-of select="translate(CommissionAmount,'QWERTYUIOPASDFGHJKLZXCVBNM ','')"/>
                    </xsl:variable>
                    <Commission>
                      <xsl:choose>
                        <xsl:when test="$comAmt='0.00' or $comAmt='.00'">
                          <xsl:attribute name="Amount">0</xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="'0'"/>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:when test="contains($comAmt,'.')">
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="translate(CommissionAmount,'.','')"/>
                          </xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="'2'"/>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="CommissionAmount"/>
                          </xsl:attribute>
                          <xsl:attribute name="DecimalPlaces">
                            <xsl:value-of select="'0'"/>
                          </xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                    </Commission>
                  </xsl:if>
                  <xsl:if test="CreditCardCode!='XX'">
                    <FormOfPayment>
                      <xsl:choose>
                        <xsl:when test="CreditCardCode!=''">
                          <PaymentCard>
                            <xsl:attribute name="CardCode">
                              <xsl:choose>
                                <xsl:when test="CreditCardCode='CA'">MC</xsl:when>
                                <xsl:when test="CreditCardCode='DI'">DS</xsl:when>
                                <xsl:when test="CreditCardCode='MA'">MC</xsl:when>
                                <xsl:when test="CreditCardCode='DC'">DN</xsl:when>
                                <xsl:otherwise>
                                  <xsl:value-of select="CreditCardCode"/>
                                </xsl:otherwise>
                              </xsl:choose>
                            </xsl:attribute>
                            <xsl:attribute name="CardNumber">
                              <xsl:value-of select="CreditCardNumber"/>
                            </xsl:attribute>
                          </PaymentCard>
                        </xsl:when>
                        <xsl:otherwise>
                          <DirectBill DirectBill_ID="CHECK"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </FormOfPayment>
                  </xsl:if>
                </PaymentInfo>
              </xsl:if>
              <xsl:if test="PassengerName">
                <xsl:variable name="paxlast">
                  <xsl:value-of select="substring-before(PassengerName,' ')"/>
                </xsl:variable>
                <xsl:variable name="paxfirst">
                  <xsl:value-of select="substring-after(PassengerName,' ')"/>
                </xsl:variable>
                <TravelerRefNumber RPH="{../../PassengerReservation/Passengers/Passenger[LastName=$paxlast and FirstName=$paxfirst]/@nameAssocId}"/>
              </xsl:if>
              <Taxes>
                <xsl:for-each select="TaxAmount">
                  <Tax>
                    <xsl:choose>
                      <xsl:when test=".='0.00'">
                        <xsl:attribute name="Amount">0</xsl:attribute>
                        <xsl:attribute name="DecimalPlaces">
                          <xsl:value-of select="'0'"/>
                        </xsl:attribute>
                      </xsl:when>
                      <xsl:when test="contains(.,'.')">
                        <xsl:attribute name="Amount">
                          <xsl:value-of select="translate(.,'.','')"/>
                        </xsl:attribute>
                        <xsl:attribute name="DecimalPlaces">
                          <xsl:value-of select="'2'"/>
                        </xsl:attribute>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:attribute name="Amount">
                          <xsl:choose>
                            <xsl:when test=".!=''">
                              <xsl:value-of select="."/>
                            </xsl:when>
                            <xsl:otherwise>0</xsl:otherwise>
                          </xsl:choose>
                        </xsl:attribute>
                        <xsl:attribute name="DecimalPlaces">
                          <xsl:value-of select="'0'"/>
                        </xsl:attribute>
                      </xsl:otherwise>
                    </xsl:choose>
                  </Tax>
                </xsl:for-each>
              </Taxes>
              <TicketingInfo>
                <xsl:choose>
                  <xsl:when test="TicketingInfo/Exchange">
                    <xsl:attribute name="ExchangeInd">true</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="ExchangeInd">false</xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:if test="TicketingInfo/OriginalInvoice">
                  <xsl:attribute name="ExchangeInd">
                    <xsl:value-of select="TicketingInfo/OriginalInvoice/@Number"/>
                  </xsl:attribute>
                </xsl:if>
                <xsl:if test="TicketingInfo/TarriffBasis">
                  <xsl:attribute name="TarriffBasis">
                    <xsl:value-of select="TicketingInfo/TarriffBasis"/>
                  </xsl:attribute>
                </xsl:if>
              </TicketingInfo>
            </AccountingInfo>
          </xsl:for-each>
        </xsl:if>
        <xsl:for-each select="PassengerReservation/ItineraryPricing/FuturePriceInfo">
          <FuturePriceInfo RPH="{format-number(position(),'#0')}">
            <xsl:value-of select="Text"/>
          </FuturePriceInfo>
        </xsl:for-each>
      </TPA_Extensions>
    </xsl:if>
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
  <xsl:template match="TicketDetails" mode="IssuedTicket">
    <xsl:variable name="pos">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <xsl:variable name="tktno">
      <xsl:value-of select="substring(TicketNumber,4,13)"/>
    </xsl:variable>
    <xsl:variable name="paxref">
      <xsl:choose>
        <xsl:when test="../../../OpenReservationElements/OpenReservationElement[ServiceRequest/@code='TKNE' and contains(ServiceRequest/FullText,$tktno)]">
          <xsl:value-of select="../../../OpenReservationElements/OpenReservationElement[ServiceRequest/@code='TKNE' and contains(ServiceRequest/FullText,$tktno)]/NameAssociation/NameRefNumber"/>
        </xsl:when>
        <xsl:otherwise>01.01</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <IssuedTicket>
      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:value-of select="$paxref"/>
      </xsl:attribute>
      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:for-each select="../../../OpenReservationElements/OpenReservationElement[ServiceRequest/@code='TKNE' and contains(ServiceRequest/FullText,$tktno)]">
          <xsl:value-of select="SegmentAssociation/@SegmentAssociationId"/>
          <xsl:if test="position() &lt; last()">
            <xsl:value-of select="string(' ')"/>
          </xsl:if>
        </xsl:for-each>
      </xsl:attribute>
      <xsl:value-of select="../ETicketNumber[position()=$pos]"/>
    </IssuedTicket>
  </xsl:template>

  <!-- ************************************************************** -->
  <!-- Issued Tickets fron DQB lines                                          -->
  <!-- ************************************************************** -->

  <xsl:template name="ParseDQBLines">
    <xsl:param name="LostText"/>
    <xsl:choose>
      <xsl:when test="substring-after($LostText,$PNR)!='' and $PNR != ''">
        <xsl:variable name="ls">
          <xsl:value-of select="translate(substring-before(substring-after($LostText,$PNR),'    F '), '&#x9;&#xa;&#xd;', '')"/>
        </xsl:variable>
        <xsl:variable name="tn">
          <xsl:value-of select="translate(substring($ls,string-length($ls)-15),' ','')"/>
        </xsl:variable>
        <xsl:variable name="tn1">
          <xsl:value-of select="translate(translate(substring($ls,string-length($ls)-15),' ',''),'-','/')"/>
        </xsl:variable>
        <xsl:if test="count(ItineraryInfo/Ticketing[contains(@eTicketNumber,$tn) and @RPH!='']) = 0 and count(ItineraryInfo/Ticketing[contains(@eTicketNumber,$tn1) and @RPH!='']) = 0">
          <IssuedTicket>
            TE <xsl:value-of select="concat($tn,' *DQB*')"/>
          </IssuedTicket>
        </xsl:if>
        <xsl:call-template name="ParseDQBLines">
          <xsl:with-param name="LostText" select="substring-after($LostText,$PNR)"/>
        </xsl:call-template>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <!--************************************************************************************-->
  <!--						 Passenger Information         		                        -->
  <!--************************************************************************************-->
  <xsl:template match="Passenger">
    <xsl:param name="pd"/>
    <xsl:variable name="paxref">
      <xsl:value-of select="translate(@nameId,'0','')"/>
    </xsl:variable>
    <CustomerInfo>
      <xsl:attribute name="RPH">
        <xsl:value-of select="@nameAssocId"/>
      </xsl:attribute>
      <Customer>
        <PersonName>
          <xsl:choose>
            <xsl:when test="@nameType='I'">
              <xsl:attribute name="NameType">INF</xsl:attribute>
            </xsl:when>
            <xsl:when test="contains($pd,$paxref)">
              <xsl:attribute name="NameType">
                <xsl:variable name="paxtype">
                  <xsl:value-of select="translate(substring(substring-after($pd,$paxref),8,4),' ','')"/>
                </xsl:variable>
                <xsl:choose>
                  <xsl:when test="substring($paxtype,1,1)!=' '">
                    <xsl:choose>
                      <xsl:when test="substring($paxtype,1,2)='CH'">
                        <xsl:value-of select="'CHD'"/>
                      </xsl:when>
                      <xsl:when test="substring($paxtype,1,2)='C0'">
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
                  <xsl:when test="$paxtype=' IN'">INF</xsl:when>
                  <xsl:otherwise>ADT</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="NameType">ADT</xsl:attribute>
            </xsl:otherwise>
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
            <xsl:value-of select="FirstName"/>
          </GivenName>
          <Surname>
            <xsl:value-of select="LastName"/>
          </Surname>
        </PersonName>
        <xsl:apply-templates select="../../../EmailAddresses/EmailAddress/Address" mode="email"/>
        <xsl:variable name="custref">
          <xsl:value-of select="@nameId"/>
        </xsl:variable>
        <xsl:for-each select="../CustLoyalty[@NameNumber=$custref]">
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
        <xsl:apply-templates select="../../../Addresses/Address/AddressLines" mode="agency"/>
        <xsl:apply-templates select="../../../../Remarks/Remark/Line[@Type='Client Address']"/>
        <xsl:apply-templates select="../../../../Remarks/Remark/Line[@Type='Delivery']"/>
      </Customer>
      <ProfileRef ID="{@nameId}"/>
    </CustomerInfo>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			      Air Itinerary Section				                              	    -->
  <!--************************************************************************************-->
  <xsl:template match="Air | Arunk" mode="Air">
    <Item>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="format-number(@sequence,'#0')"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="@ResBookDesigCode">
          <xsl:if test="FlightSegment/@ActionCode = 'GK'">
            <xsl:attribute name="IsPassive">Y</xsl:attribute>
          </xsl:if>
          <Air>
            <xsl:attribute name="DepartureDateTime">
              <xsl:value-of select="DepartureDateTime"/>
            </xsl:attribute>
            <xsl:attribute name="ArrivalDateTime">
              <xsl:value-of select="ArrivalDateTime"/>
            </xsl:attribute>
            <xsl:if test="@StopQuantity != ''">
              <xsl:attribute name="StopQuantity">
                <xsl:value-of select="format-number(@StopQuantity,'#0')"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="RPH">
              <xsl:value-of select="format-number(@sequence,'#0')"/>
            </xsl:attribute>
            <xsl:attribute name="FlightNumber">
              <xsl:value-of select="MarketingFlightNumber"/>
            </xsl:attribute>
            <xsl:attribute name="ResBookDesigCode">
              <xsl:value-of select="MarketingClassOfService"/>
            </xsl:attribute>
            <xsl:attribute name="NumberInParty">
              <xsl:value-of select="NumberInParty"/>
            </xsl:attribute>
            <xsl:attribute name="Status">
              <xsl:value-of select="ActionCode"/>
            </xsl:attribute>
            <xsl:if test="Eticket">
              <xsl:attribute name="E_TicketEligibility">
                <xsl:choose>
                  <xsl:when test="Eticket= 'true'">Eligible</xsl:when>
                  <xsl:otherwise>NotEligible</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </xsl:if>
            <DepartureAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="DepartureAirport"/>
              </xsl:attribute>
              <xsl:if test="DepartureTerminalName!=''">
                <xsl:attribute name="Terminal">
                  <xsl:value-of select="DepartureTerminalName"/>
                </xsl:attribute>
              </xsl:if>
            </DepartureAirport>
            <ArrivalAirport>
              <xsl:attribute name="LocationCode">
                <xsl:value-of select="ArrivalAirport"/>
              </xsl:attribute>
              <xsl:if test="ArrivalTerminalName!=''">
                <xsl:attribute name="Terminal">
                  <xsl:value-of select="ArrivalTerminalName"/>
                </xsl:attribute>
              </xsl:if>
            </ArrivalAirport>
            <OperatingAirline>
              <xsl:attribute name="Code">
                <xsl:choose>
                  <xsl:when test="OperatingAirlineCode!=''">
                    <xsl:value-of select="OperatingAirlineCode"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="MarketingAirlineCode"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </OperatingAirline>
            <xsl:if test="EquipmentType!= ''">
              <Equipment>
                <xsl:attribute name="AirEquipType">
                  <xsl:value-of select="EquipmentType"/>
                </xsl:attribute>
              </Equipment>
            </xsl:if>
            <MarketingAirline>
              <xsl:attribute name="Code">
                <xsl:value-of select="MarketingAirlineCode"/>
              </xsl:attribute>
            </MarketingAirline>
            <xsl:if test="MarriageGrp/Group!='' and MarriageGrp/Sequence!='0'">
              <MarriageGrp>
                <xsl:value-of select="MarriageGrp/Group"/>
              </MarriageGrp>
            </xsl:if>
            <TPA_Extensions>
              <xsl:attribute name="ConfirmationNumber">
                <xsl:choose>
                  <xsl:when test="AirlineRefId!=''">
                    <xsl:choose>
                      <xsl:when test="contains(AirlineRefId,'*')">
                        <xsl:value-of select="substring-after(AirlineRefId,'*')"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="AirlineRefId"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="../../../../BookingDetails/RecordLocator"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:variable name="seq">
                <xsl:value-of select="@sequence"/>
              </xsl:variable>
              <xsl:if test="../../../../../PriceQuote/PriceQuoteInfo/Details[1]/SegmentInfo[@number=$seq]/BrandedFare/@code!=''">
                <FareFamily>
                  <xsl:value-of select="concat(../../../../../PriceQuote/PriceQuoteInfo/Details[1]/SegmentInfo[@number=$seq]/BrandedFare/@code,'-',../../../../../PriceQuote/PriceQuoteInfo/Details[1]/SegmentInfo[@number=$seq]/BrandedFare/@description)"/>
                </FareFamily>
              </xsl:if>
            </TPA_Extensions>
          </Air>
        </xsl:when>
        <xsl:otherwise>
          <TPA_Extensions>
            <Arnk/>
          </TPA_Extensions>
        </xsl:otherwise>
      </xsl:choose>
    </Item>
  </xsl:template>

  <xsl:template match="Misc | MiscSegment" mode="Misc">
    <Item>
      <xsl:attribute name="Status">
        <xsl:value-of select="@Status"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
      </xsl:attribute>
      <General>
        <xsl:attribute name="Start">
          <xsl:variable name="curdate">
            <xsl:value-of select="substring(../../../../ItineraryRef/Source/@CreateDateTime,1,10)"/>
          </xsl:variable>
          <xsl:variable name="curyear">
            <xsl:value-of select="substring($curdate,1,4)"/>
          </xsl:variable>
          <xsl:variable name="curmonth">
            <xsl:value-of select="substring($curdate,6,2)"/>
          </xsl:variable>
          <xsl:variable name="curday">
            <xsl:value-of select="substring($curdate,9)"/>
          </xsl:variable>
          <xsl:variable name="miscmonth">
            <xsl:value-of select="substring(@DepartureDateTime,1,2)"/>
          </xsl:variable>
          <xsl:variable name="miscday">
            <xsl:value-of select="substring(@DepartureDateTime,4,2)"/>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="$miscmonth > $curmonth">
              <xsl:value-of select="$curyear"/>
              <xsl:value-of select="concat('-',@DepartureDateTime)"/>
            </xsl:when>
            <xsl:when test="$curmonth > $miscmonth">
              <xsl:value-of select="$curyear + 1"/>
              <xsl:value-of select="concat('-',@DepartureDateTime)"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:choose>
                <xsl:when test="$miscday >= $curday">
                  <xsl:value-of select="$curyear"/>
                  <xsl:value-of select="concat('-',@DepartureDateTime)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$curyear + 1"/>
                  <xsl:value-of select="concat('-',@DepartureDateTime)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <Description>
          <!--xsl:text>Miscellaneous</xsl:text>
					<xsl:value-of select="concat(' - Board point: ',OriginLocation/@LocationCode)"/-->
          <xsl:value-of select="Text"/>
        </Description>
        <TPA_Extensions Status="{@Status}" NumberInParty="{format-number(@NumberInParty,'#0')}">
          <Vendor Code="{Vendor/@Code}"/>
          <OriginCityCode>
            <xsl:value-of select="OriginLocation/@LocationCode"/>
          </OriginCityCode>
        </TPA_Extensions>
      </General>
    </Item>
  </xsl:template>

  <!--*****************************************************************-->
  <!--			Car Segs						   		     -->
  <!--*****************************************************************-->
  <xsl:template match="Vehicle" mode="Car">
    <Item>
      <xsl:attribute name="Status">
        <xsl:value-of select="LineStatus"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="format-number(@sequence,'#0')"/>
      </xsl:attribute>
      <Vehicle>
        <ConfID>
          <xsl:attribute name="Type">C</xsl:attribute>
          <xsl:attribute name="ID">
            <xsl:value-of select="ConfId/@Id"/>
            <xsl:value-of select="ConfId"/>
          </xsl:attribute>
        </ConfID>
        <Vendor>
          <xsl:attribute name="Code">
            <xsl:value-of select="VendorCode"/>
          </xsl:attribute>
        </Vendor>
        <VehRentalCore>
          <xsl:attribute name="PickUpDateTime">
            <xsl:value-of select="PickUpDateTime" />
          </xsl:attribute>
          <xsl:attribute name="ReturnDateTime">
            <xsl:value-of select="ReturnDateTime" />
          </xsl:attribute>
          <PickUpLocation>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="PickUpLocation/LocationCode"/>
            </xsl:attribute>
          </PickUpLocation>
          <ReturnLocation>
            <xsl:attribute name="LocationCode">
              <xsl:choose>
                <xsl:when test="not(ReturnLocation)">
                  <xsl:value-of select="PickUpLocation/LocationCode"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="ReturnLocation/LocationCode"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </ReturnLocation>
        </VehRentalCore>
        <Veh>
          <xsl:attribute name="AirConditionInd">
            <xsl:choose>
              <xsl:when test="substring(PricedEquipment/EquipType,4,1) = 'R'">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="TransmissionType">
            <xsl:choose>
              <xsl:when test="substring(PricedEquipment/EquipType,3,1) = 'A'">Automatic</xsl:when>
              <xsl:otherwise>Manual</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <VehType>
            <xsl:attribute name="VehicleCategory">
              <xsl:choose>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'C'">2/4 Door Car</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'B'">2-Door Car</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'D'">4-Door Car</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'W'">Wagon</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'V'">Van</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'L'">Limousine</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'S'">Sport</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'T'">Convertible</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'F'">4-Wheel Drive</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'P'">Pickup</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,2,1) = 'J'">All-Terrain</xsl:when>
                <xsl:otherwise>Unavailable</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:value-of select="PricedEquipment/EquipType"/>
          </VehType>
          <VehClass>
            <xsl:attribute name="Size">
              <xsl:choose>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'M'">Mini</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'E'">Economy</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'C'">Compact</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'I'">Intermediate</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'S'">Standard</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'F'">Full-Size</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'P'">Premium</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'L'">Luxury</xsl:when>
                <xsl:when test="substring(PricedEquipment/EquipType,1,1) = 'X'">Special</xsl:when>
                <xsl:otherwise>Unavailable</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </VehClass>
        </Veh>
        <xsl:if test="../Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount!=''">
          <RentalRate>
            <RateDistance>
              <xsl:attribute name="Unlimited">
                <xsl:choose>
                  <xsl:when test="../Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@mileageAllowance='UNL'">true</xsl:when>
                  <xsl:otherwise>false</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="DistUnitName">
                <xsl:choose>
                  <xsl:when test="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Mileage/@UnitOfMeasure='K'">Km</xsl:when>
                  <xsl:otherwise>Mile</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </RateDistance>
            <VehicleCharges>
              <VehicleCharge>
                <xsl:attribute name="Amount">
                  <xsl:variable name="vehAmt">
                    <xsl:choose>
                      <xsl:when test="../Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount!=''">
                        <xsl:value-of select="translate(string(../Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount),'@.','')"/>
                      </xsl:when>
                      <xsl:when test="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Amount !=''">
                        <xsl:value-of select="translate(string(VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Amount),'.','')"/>
                      </xsl:when>
                    </xsl:choose>
                  </xsl:variable>
                  <xsl:value-of select="translate($vehAmt,' QWERTYUIOPASDFGHJKLZXCVBNM','')"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="substring(RentalRate/VehicleCharges/VehicleChargeAmount,1,3)"/>
                  <xsl:value-of select="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Mileage/@CurrencyCode"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
                <xsl:attribute name="TaxInclusive">
                  <xsl:choose>
                    <xsl:when test="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/@TaxInclusive='true'">true</xsl:when>
                    <xsl:otherwise>false</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="GuaranteedInd">
                  <xsl:choose>
                    <xsl:when test="VehVendorAvail/VehResCore/RentalRate/@GuaranteedQuoted='G'">true</xsl:when>
                    <xsl:otherwise>false</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </VehicleCharge>
            </VehicleCharges>
          </RentalRate>
          <xsl:if test="Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount!=''">
            <TotalCharge>
              <xsl:attribute name="RateTotalAmount">
                <xsl:variable name="vehAmt">
                  <xsl:choose>
                    <xsl:when test="Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount!=''">
                      <xsl:value-of select="translate(string(Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount),'@.','')"/>
                    </xsl:when>
                    <xsl:when test="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Amount !=''">
                      <xsl:value-of select="translate(string(VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Amount),'.','')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
                <xsl:value-of select="translate($vehAmt,' QWERTYUIOPASDFGHJKLZXCVBNM','')"/>
              </xsl:attribute>
              <xsl:attribute name="EstimatedTotalAmount">
                <xsl:variable name="vehAmt">
                  <xsl:choose>
                    <xsl:when test="Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount!=''">
                      <xsl:value-of select="translate(string(Product/ProductDetails/Vehicle/VehicleVendorAvail/VehicleResCore/Charge/ChargeDetails/ApproximateTotalCharge/@amount),'@.','')"/>
                    </xsl:when>
                    <xsl:when test="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Amount !=''">
                      <xsl:value-of select="translate(string(VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Amount),'.','')"/>
                    </xsl:when>
                  </xsl:choose>
                </xsl:variable>
                <xsl:value-of select="translate($vehAmt,' QWERTYUIOPASDFGHJKLZXCVBNM','')"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="substring(RentalRate/VehicleCharges/VehicleChargeAmount,1,3)"/>
                <xsl:value-of select="VehVendorAvail/VehResCore/VehicleCharges/VehicleCharge/Mileage/@CurrencyCode"/>
              </xsl:attribute>
            </TotalCharge>
          </xsl:if>
        </xsl:if>
      </Vehicle>
    </Item>
  </xsl:template>
  <!--*****************************************************************-->
  <!--			Hotel Segs								    -->
  <!--*****************************************************************-->
  <xsl:template match="Hotel" mode="Hotel">
    <Item>
      <xsl:attribute name="Status">
        <xsl:value-of select="@Status"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="format-number(@SegmentNumber,'#0')"/>
      </xsl:attribute>
      <Hotel>
        <Reservation>
          <RoomTypes>
            <RoomType>
              <xsl:attribute name="RoomTypeCode">
                <xsl:value-of select="Reservation/RoomTypes/RoomType/@RoomTypeCode"/>
                <xsl:value-of select="RoomRates/Rate/@RoomTypeCode"/>
              </xsl:attribute>
              <xsl:if test="Reservation/RoomTypes/RoomType/@NumberOfUnits!='' or @NumberOfUnits!=''">
                <xsl:attribute name="NumberOfUnits">
                  <xsl:value-of select="Reservation/RoomTypes/RoomType/@NumberOfUnits"/>
                  <xsl:value-of select="@NumberOfUnits"/>
                </xsl:attribute>
              </xsl:if>
            </RoomType>
          </RoomTypes>
          <xsl:if test="translate(string(Reservation/RoomRates/RoomRate/Rates/Rate/Base/@AmountBeforeTax),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')!='' or translate(string(RoomRates/Rate/@Amount),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')!=''">
            <RoomRates>
              <RoomRate>
                <Rates>
                  <Rate>
                    <Base>
                      <xsl:attribute name="AmountBeforeTax">
                        <xsl:value-of select="translate(string(Reservation/RoomRates/RoomRate/Rates/Rate/Base/@AmountBeforeTax),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')"/>
                        <xsl:value-of select="translate(string(RoomRates/Rate/@Amount),'.ABCDEFGHIJKLMNOPQRSTUVWXYZ','')"/>
                      </xsl:attribute>
                      <xsl:attribute name="CurrencyCode">
                        <xsl:value-of select="Reservation/RoomRates/RoomRate/Rates/Rate/Base/@CurrencyCode"/>
                        <xsl:value-of select="RoomRates/Rate/@CurrencyCode"/>
                      </xsl:attribute>
                    </Base>
                  </Rate>
                </Rates>
              </RoomRate>
            </RoomRates>
          </xsl:if>
          <GuestCounts>
            <GuestCount>
              <xsl:attribute name="AgeQualifyingCode">
                <xsl:choose>
                  <xsl:when test="Reservation/GuestCounts/GuestCount/@AgeQualifyingCode">
                    <xsl:value-of select="Reservation/GuestCounts/GuestCount/@AgeQualifyingCode"/>
                  </xsl:when>
                  <xsl:otherwise>10</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="Count">
                <xsl:value-of select="Reservation/GuestCounts/GuestCount/@Count"/>
                <xsl:value-of select="@NumberInParty"/>
              </xsl:attribute>
            </GuestCount>
          </GuestCounts>
          <xsl:variable name="ny">
            <xsl:value-of select="substring(../../../../../ApplicationResults/Success/@timeStamp,1,4)"/>
          </xsl:variable>
          <TimeSpan>
            <xsl:attribute name="Start">
              <xsl:value-of select="Reservation/TimeSpan/@Start"/>
              <xsl:value-of select="concat($ny,'-',TimeSpan/@Start)"/>
            </xsl:attribute>
            <xsl:if test="TimeSpan/@Duration!='' or Reservation/TimeSpan/@Duration!=''">
              <xsl:attribute name="Duration">
                <xsl:value-of select="Reservation/TimeSpan/@Duration"/>
                <xsl:value-of select="TimeSpan/@Duration"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="TimeSpan/@End!=''">
              <xsl:attribute name="End">
                <xsl:value-of select="Reservation/TimeSpan/@End"/>
                <xsl:value-of select="concat($ny,'-',TimeSpan/@End)"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:if test="Reservation/TimeSpan/@End!=''">
              <xsl:attribute name="End">
                <xsl:value-of select="Reservation/TimeSpan/@End"/>
                <xsl:value-of select="concat($ny,'-',TimeSpan/@End)"/>
              </xsl:attribute>
            </xsl:if>
          </TimeSpan>
          <BasicPropertyInfo>
            <xsl:attribute name="ChainCode">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@ChainCode"/>
              <xsl:value-of select="BasicPropertyInfo/@ChainCode"/>
            </xsl:attribute>
            <xsl:attribute name="HotelCityCode">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCityCode"/>
              <xsl:value-of select="BasicPropertyInfo/@HotelCityCode"/>
            </xsl:attribute>
            <xsl:if test="Reservation/BasicPropertyInfo/@HotelCode!= '' or BasicPropertyInfo/@HotelCode!=''">
              <xsl:attribute name="HotelCode">
                <xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCode"/>
                <xsl:value-of select="BasicPropertyInfo/@HotelCode"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="HotelName">
              <xsl:value-of select="Reservation/BasicPropertyInfo/@HotelName"/>
              <xsl:value-of select="BasicPropertyInfo/@HotelName"/>
            </xsl:attribute>
            <xsl:if test="Reservation/BasicPropertyInfo/Address">
              <xsl:attribute name="AreaID">
                <xsl:value-of select="Reservation/BasicPropertyInfo/Address/AddressLine[1]"/>
                <xsl:value-of select="Reservation/BasicPropertyInfo/Address/AddressLine[2]"/>
                <xsl:value-of select="BasicPropertyInfo/Address/AddressLine[1]"/>
                <xsl:value-of select="concat(' ',BasicPropertyInfo/Address/AddressLine[2])"/>
              </xsl:attribute>
            </xsl:if>
          </BasicPropertyInfo>
        </Reservation>
        <TPA_Extensions>
          <xsl:attribute name="ConfirmationNumber">
            <xsl:value-of select="TPA_Extensions/ConfirmationNumber"/>
            <xsl:value-of select="BasicPropertyInfo/ConfirmationNumber"/>
            <xsl:value-of select="POS/Source/RequestorID"/>
          </xsl:attribute>
        </TPA_Extensions>
      </Hotel>
    </Item>
  </xsl:template>
  <!--************************************************************************************-->
  <!--					Calculate Total FareTotals	 	      			           -->
  <!--***********************************************************************************-->
  <xsl:template match="PriceQuoteInfo" mode="Fare">
    <xsl:variable name="dect">
      <xsl:choose>
        <xsl:when test="Details/FareInfo/TotalFare/@decimalPlace!=''">
          <xsl:value-of select="Details/FareInfo/TotalFare/@decimalPlace"/>
        </xsl:when>
        <xsl:otherwise>0</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="Details">
      <xsl:for-each select="Details[not(RefundInfo)]">
        <xsl:variable name="num">
          <xsl:value-of select="@number"/>
        </xsl:variable>
        <xsl:if test="../Summary/NameAssociation/PriceQuote[@latestPQFlag='true']/@number = $num">
          <xsl:copy-of select="."/>
        </xsl:if>
      </xsl:for-each>
    </xsl:variable>
    <xsl:if test="$Details!=''">
      <AirFareInfo>
        <xsl:variable name="posFO">
          <xsl:value-of select="Summary/NameAssociation/PriceQuote[@latestPQFlag='true'][1]/@number"/>
        </xsl:variable>
        <xsl:attribute name="PricingSource">
          <xsl:choose>
            <xsl:when test="Details[@number=$posFO]/FareInfo/FareIndicators/@privateFareType!=''">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:when test="../../../DisplayPriceQuoteRS/PriceQuote/ResponseHeader/Text[contains(text(),'PRIVATE FARE APPLIED')]">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:when test="Details[@number=$posFO]/@passengerType='PFA' or Details[1]/@passengerType='JCB' or Details[1]/@passengerType='JNN' or Details[1]/@passengerType='JNF'">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Published'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <ItinTotalFare>
          <BaseFare>
            <xsl:choose>
              <xsl:when test="Details[@number=$posFO][1]/FareInfo/EquivalentFare!=''">
                <xsl:attribute name="Amount">
                  <xsl:apply-templates select="msxsl:node-set($Details)/Details[1]/FareInfo/EquivalentFare">
                    <xsl:with-param name="totalbf">
                      <xsl:value-of select="0"/>
                    </xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(msxsl:node-set($Details)/Details)+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="Details" select="$Details"/>
                  </xsl:apply-templates>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="msxsl:node-set($Details)/Details[1]/FareInfo/EquivalentFare/@currencyCode"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="Amount">
                  <xsl:apply-templates select="msxsl:node-set($Details)/Details[1]/FareInfo/BaseFare">
                    <xsl:with-param name="totalbf">0</xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(msxsl:node-set($Details)/Details)+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="Details" select="$Details"/>
                  </xsl:apply-templates>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="msxsl:node-set($Details)/Details[1]/FareInfo/BaseFare/@currencyCode"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </BaseFare>
          <xsl:if test="Details[@number=$posFO][1]/FareInfo/EquivalentFare!=''">
            <EquivFare>
              <xsl:attribute name="Amount">
                <xsl:apply-templates select="msxsl:node-set($Details)/Details[1]/FareInfo/BaseFare">
                  <xsl:with-param name="totalbf">0</xsl:with-param>
                  <xsl:with-param name="pos">1</xsl:with-param>
                  <xsl:with-param name="bfcount">
                    <xsl:value-of select="count(msxsl:node-set($Details)/Details)+1"/>
                  </xsl:with-param>
                  <xsl:with-param name="Details" select="$Details"/>
                </xsl:apply-templates>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="msxsl:node-set($Details)/Details[1]/FareInfo/BaseFare/@currencyCode"/>
              </xsl:attribute>
              <xsl:attribute name="DecimalPlaces">
                <xsl:value-of select="$dect"/>
              </xsl:attribute>
            </EquivFare>
          </xsl:if>
          <Taxes>
            <xsl:attribute name="Amount">
              <xsl:apply-templates select="msxsl:node-set($Details)/Details[1]/FareInfo/TotalTax">
                <xsl:with-param name="totalbf">0</xsl:with-param>
                <xsl:with-param name="pos">1</xsl:with-param>
                <xsl:with-param name="bfcount">
                  <xsl:value-of select="count(msxsl:node-set($Details)/Details)+1"/>
                </xsl:with-param>
                <xsl:with-param name="Details" select="$Details"/>
              </xsl:apply-templates>
            </xsl:attribute>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="msxsl:node-set($Details)/Details[1]/FareInfo/TotalTax/@currencyCode"/>
            </xsl:attribute>
            <xsl:attribute name="DecimalPlaces">
              <xsl:value-of select="$dect"/>
            </xsl:attribute>
          </Taxes>
          <TotalFare>
            <xsl:choose>
              <xsl:when test="msxsl:node-set($Details)/Details[1]/FareInfo/TotalFare">
                <xsl:attribute name="Amount">
                  <xsl:apply-templates select="msxsl:node-set($Details)/Details[1]/FareInfo/TotalFare">
                    <xsl:with-param name="totalbf">0</xsl:with-param>
                    <xsl:with-param name="pos">1</xsl:with-param>
                    <xsl:with-param name="bfcount">
                      <xsl:value-of select="count(msxsl:node-set($Details)/Details)+1"/>
                    </xsl:with-param>
                    <xsl:with-param name="Details" select="$Details"/>
                  </xsl:apply-templates>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="msxsl:node-set($Details)/Details[1]/FareInfo/TotalFare/@currencyCode"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="Amount">0</xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="msxsl:node-set($Details)/Details[1]/FareInfo/TotalTax/@currencyCode"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </TotalFare>
        </ItinTotalFare>
        <PTC_FareBreakdowns>
          <xsl:apply-templates select="msxsl:node-set($Details)/Details"/>
        </PTC_FareBreakdowns>
      </AirFareInfo>
    </xsl:if>
  </xsl:template>

  <xsl:template match="BaseFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:param name="Details"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(.,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="count(../../NameAssociationInfo)"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and msxsl:node-set($Details)/Details[$pos + 1]/FareInfo/BaseFare">
        <xsl:apply-templates select="msxsl:node-set($Details)/Details[$pos + 1]/FareInfo/BaseFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="Details" select="$Details"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="EquivalentFare">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:param name="Details"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(.,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="count(../../NameAssociationInfo)"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and msxsl:node-set($Details)/Details[position() = ($pos + 1)]/FareInfo/EquivalentFare">
        <xsl:apply-templates select="msxsl:node-set($Details)/Details[position() = ($pos + 1)]/FareInfo/EquivalentFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="Details" select="$Details"/>
        </xsl:apply-templates>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$totalbf + $bf"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  <xsl:template match="TotalTax">
    <xsl:param name="totalbf"/>
    <xsl:param name="pos"/>
    <xsl:param name="bfcount"/>
    <xsl:param name="Details"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(.,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="count(../../NameAssociationInfo)"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and msxsl:node-set($Details)/Details[$pos + 1]/FareInfo/TotalTax">
        <xsl:apply-templates select="msxsl:node-set($Details)/Details[$pos + 1]/FareInfo/TotalTax">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="Details" select="$Details"/>
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
    <xsl:param name="Details"/>
    <xsl:variable name="bf1">
      <xsl:value-of select="translate(.,'.','')"/>
    </xsl:variable>
    <xsl:variable name="nip">
      <xsl:value-of select="count(../../NameAssociationInfo)"/>
    </xsl:variable>
    <xsl:variable name="bf">
      <xsl:value-of select="$bf1 * $nip"/>
    </xsl:variable>
    <xsl:choose>
      <xsl:when test="$pos &lt; $bfcount and msxsl:node-set($Details)/Details[$pos + 1]/FareInfo/TotalFare">
        <xsl:apply-templates select="msxsl:node-set($Details)/Details[$pos + 1]/FareInfo/TotalFare">
          <xsl:with-param name="totalbf">
            <xsl:value-of select="$totalbf + $bf"/>
          </xsl:with-param>
          <xsl:with-param name="pos">
            <xsl:value-of select="$pos + 1"/>
          </xsl:with-param>
          <xsl:with-param name="bfcount">
            <xsl:value-of select="$bfcount"/>
          </xsl:with-param>
          <xsl:with-param name="Details" select="$Details"/>
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
  <xsl:template match="Details">
    <xsl:variable name="pos">
      <xsl:value-of select="position()"/>
    </xsl:variable>
    <xsl:if test="not(../Summary/NameAssociation/PriceQuote[position()=$pos]/Indicators/@isExpired) or ../Summary/NameAssociation/PriceQuote[position()=$pos]/Indicators/@isExpired='false'">
      <xsl:variable name="dect1">
        <xsl:choose>
          <xsl:when test="FareInfo/TotalFare/@decimalPlace">
            <xsl:value-of select="FareInfo/TotalFare/@decimalPlace"/>
          </xsl:when>
          <xsl:when test="FareInfo/BaseFare/@decimalPlace">
            <xsl:value-of select="FareInfo/BaseFare/@decimalPlace"/>
          </xsl:when>
          <xsl:otherwise>0</xsl:otherwise>
        </xsl:choose>
      </xsl:variable>
      <PTC_FareBreakdown>
        <xsl:attribute name="RPH">
          <xsl:value-of select="@number"/>
        </xsl:attribute>
        
        <xsl:attribute name="PricingSource">
          <xsl:choose>
            <xsl:when test="FareInfo/FareIndicators/@privateFareType!=''">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:when test="../../../../../../DisplayPriceQuoteRS/PriceQuote/ResponseHeader[contains(Text,'PRIVATE FARE APPLIED')]">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:when test="@passengerType='JCB' or @passengerType='JNN' or @passengerType='JNF'">
              <xsl:value-of select="'Private'"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="'Published'"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        
        <xsl:attribute name="TravelerRefNumberRPHList">
          <xsl:for-each select="NameAssociationInfo">
            <xsl:if test="position() > 1">
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="@nameId"/>
          </xsl:for-each>
        </xsl:attribute>
        
        <xsl:attribute name="FlightRefNumberRPHList">
          <xsl:for-each select="SegmentInfo">
            <xsl:if test="position() > 1">
              <xsl:text> </xsl:text>
            </xsl:if>
            <xsl:value-of select="@number"/>
          </xsl:for-each>
        </xsl:attribute>
        
        <PassengerTypeQuantity>
          <xsl:choose>
            <xsl:when test="substring(@passengerType,1,2)='C0'">
              <xsl:attribute name="Code">CHD</xsl:attribute>
            </xsl:when>
            <xsl:when test="@passengerType='JCB'">
              <xsl:attribute name="Code">ADT</xsl:attribute>
            </xsl:when>
            <xsl:when test="@passengerType='JNN'">
              <xsl:attribute name="Code">CHD</xsl:attribute>
            </xsl:when>
            <xsl:when test="@passengerType='JNF'">
              <xsl:attribute name="Code">INF</xsl:attribute>
            </xsl:when>
            <xsl:otherwise>
              <xsl:attribute name="Code">
                <xsl:value-of select="@passengerType"/>
              </xsl:attribute>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:if test="@passengerType='JCB' or @passengerType='JNN' or @passengerType='JNF'">
            <xsl:attribute name="CodeContext">
              <xsl:value-of select="@passengerType"/>
            </xsl:attribute>
          </xsl:if>
          <xsl:attribute name="Quantity">
            <xsl:value-of select="count(NameAssociationInfo)"/>
          </xsl:attribute>
        </PassengerTypeQuantity>
        
        <FareBasisCodes>
          <xsl:for-each select="SegmentInfo">
            <FareBasisCode>
              <xsl:choose>
                <xsl:when test="FareBasis!=''">
                  <xsl:value-of select="FareBasis"/>
                </xsl:when>
                <xsl:otherwise>VOID</xsl:otherwise>
              </xsl:choose>
            </FareBasisCode>
          </xsl:for-each>
        </FareBasisCodes>
        
        <xsl:if test="SegmentInfo/BrandedFare/@code!=''">
          <BrandedFares>
            <xsl:for-each select="SegmentInfo[BrandedFare/@code!='']">
              <FareFamily>
                <xsl:value-of select="concat(BrandedFare/@code,'-',BrandedFare/@description)"/>
              </FareFamily>
            </xsl:for-each>            
          </BrandedFares>
        </xsl:if>
        
        <xsl:variable name="nip">
          <xsl:value-of select="count(NameAssociationInfo)"/>
        </xsl:variable>
        <PassengerFare>
          <BaseFare>
            <xsl:choose>
              <xsl:when test="FareInfo/EquivalentFare!=''">
                <xsl:attribute name="Amount">
                  <xsl:value-of select="translate(string(FareInfo/EquivalentFare),'.','') * $nip"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect1"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="FareInfo/EquivalentFare/@currencyCode"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="Amount">
                  <xsl:value-of select="translate(string(FareInfo/BaseFare),'.','') * $nip"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect1"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="FareInfo/BaseFare/@currencyCode"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </BaseFare>
          <Taxes>
            <xsl:apply-templates select="FareInfo/TaxInfo/Tax"/>
          </Taxes>
          <TotalFare>
            <xsl:choose>
              <xsl:when test="FareInfo/TotalFare">
                <xsl:attribute name="Amount">
                  <xsl:value-of select="translate(string(FareInfo/TotalFare),'.','') * $nip"/>
                </xsl:attribute>
                <xsl:attribute name="DecimalPlaces">
                  <xsl:value-of select="$dect1"/>
                </xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="FareInfo/TotalFare/@currencyCode"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="Amount">0</xsl:attribute>
                <xsl:attribute name="DecimalPlaces">2</xsl:attribute>
                <xsl:attribute name="CurrencyCode">
                  <xsl:value-of select="FareInfo/BaseFare/@currencyCode"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </TotalFare>
        </PassengerFare>
        <TPA_Extensions>
          <xsl:if test="FareInfo/FareCalculation!=''">
            <FareCalculation>
              <xsl:value-of select="FareInfo/FareCalculation"/>
            </FareCalculation>
          </xsl:if>
          <xsl:if test="MiscellaneousInfo/ValidatingCarrier != ''">
            <ValidatingAirlineCode>
              <xsl:value-of select="MiscellaneousInfo/ValidatingCarrier"/>
            </ValidatingAirlineCode>
            <Vendor>
              <xsl:attribute name="Code">
                <xsl:value-of select="MiscellaneousInfo/ValidatingCarrier"/>
              </xsl:attribute>
            </Vendor>
          </xsl:if>
          
          <xsl:choose>
            <xsl:when test="contains(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'P')">
              <BagAllowance>
                <xsl:attribute name="Quantity">
                  <xsl:value-of select="translate(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'PW','')"/>
                </xsl:attribute>
                <xsl:attribute name="Type">
                  <xsl:value-of select="'Piece'"/>
                </xsl:attribute>
              </BagAllowance>
            </xsl:when>
            <xsl:when test="contains(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'W')">
              <BagAllowance>
                <xsl:attribute name="Weight">
                  <xsl:value-of select="translate(PTC_FareBreakdown/FlightSegment [1]/BaggageAllowance/@Number,'PW','')"/>
                </xsl:attribute>
                <xsl:attribute name="Type">
                  <xsl:value-of select="'Weight'"/>
                </xsl:attribute>
              </BagAllowance>
            </xsl:when>
          </xsl:choose>
        
          <xsl:if test="TransactionInfo/InputEntry">
            <SupplementalInfo>
              <xsl:value-of select="TransactionInfo/InputEntry"/>
            </SupplementalInfo>
          </xsl:if>
        </TPA_Extensions>
      </PTC_FareBreakdown>
    </xsl:if>
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
  <xsl:template match="Tax">
    <Tax>
      <xsl:attribute name="Amount">
        <xsl:value-of select="translate(Amount,'.','')"/>
      </xsl:attribute>
      <xsl:attribute name="DecimalPlaces">
        <xsl:value-of select="string-length(substring-after(Amount,'.'))"/>
      </xsl:attribute>
      <xsl:attribute name="Code">
        <xsl:value-of select="@code"/>
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
  <xsl:template match="Address" mode="email">
    <xsl:if test=".  != ''">
      <Email>
        <xsl:if test="@Comment!=''">
          <xsl:attribute name="EmailType">
            <xsl:value-of select="@Comment"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:value-of select="translate(.,'Â','')"/>
      </Email>
    </xsl:if>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Address/Delivery Addres information						    -->
  <!--************************************************************************************-->
  <xsl:template match="Line">
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

  <xsl:template match="AddressLines" mode="agency">
    <Address>
      <xsl:attribute name="UseType">
        <xsl:choose>
          <xsl:when test="AddressLine[1]/@type = 'O'">Agency</xsl:when>
          <xsl:otherwise>Mailing</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <AddressLine>
        <xsl:value-of select="AddressLine[1]"/>
      </AddressLine>
      <xsl:if test="AddressLine[position()=2]!=''">
        <StreetNmbr>
          <xsl:value-of select="AddressLine[position()=2]"/>
        </StreetNmbr>
      </xsl:if>
      <xsl:if test="AddressLine[position()=3]!=''">
        <CityName>
          <xsl:value-of select="AddressLine[position()=3]"/>
        </CityName>
      </xsl:if>
      <xsl:if test="AddressLine[position()=4]!=''">
        <PostalCode>
          <xsl:value-of select="AddressLine[position()=4]"/>
        </PostalCode>
      </xsl:if>
    </Address>
  </xsl:template>
  <!-- ***********************************************************-->
  <!--  				Ticketing info      			     -->
  <!-- ********************************************************** -->
  <xsl:template match="TicketingInfo" mode="Ticketing">
    <Ticketing>
      <xsl:attribute name="TicketType">
        <xsl:choose>
          <xsl:when test="../Segments/Segment/Air/Eticket='true'">eTicket</xsl:when>
          <xsl:otherwise>Paper</xsl:otherwise>
        </xsl:choose>
      </xsl:attribute>
      <xsl:variable name="ttl1">
        <xsl:value-of select="substring-after(../ItineraryPricing/TPA_Extensions/TicketRecordInfo/Text[contains(.,'LAST DAY TO PURCHASE')],'LAST DAY TO PURCHASE ')"/>
        <xsl:value-of select="substring-after(../ItineraryPricing/PriceQuote/PricedItinerary/AirItineraryPricingInfo/PTC_FareBreakdown/ResTicketingRestrictions[contains(.,'LAST DAY TO PURCHASE')],'LAST DAY TO PURCHASE ')"/>
      </xsl:variable>
      <xsl:choose>
        <xsl:when test="(FutureTicketing[Time!=''] and (Code='TAW' or Code='TL') and FutureTicketing/Date!='') or TicketingTimeLimit/Time!=''">
          <xsl:attribute name="TicketTimeLimit">
            <xsl:variable name="ttld">
              <xsl:value-of select="substring(FutureTicketing/Date,1,2)"/>
              <xsl:value-of select="substring(substring-after(TicketingTimeLimit/Time,'/'),1,2)"/>
            </xsl:variable>
            <xsl:variable name="ttlm">
              <xsl:call-template name="month">
                <xsl:with-param name="month">
                  <xsl:value-of select="substring(Date,3,3)"/>
                  <xsl:value-of select="substring(substring-after(TicketingTimeLimit/Time,'/'),3,3)"/>
                </xsl:with-param>
              </xsl:call-template>
            </xsl:variable>
            <xsl:variable name="nd">
              <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,9,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,6,2)"/>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:choose>
                <xsl:when test="../../BookingDetails/FlightsRange/@Start!=''">
                  <xsl:value-of select="substring(../../BookingDetails/FlightsRange/@Start,1,4)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,1,4)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:variable name="tktime">
              <xsl:variable name="tkt">
                <xsl:value-of select="FutureTicketing/Time"/>
                <xsl:value-of select="substring(substring-before(TicketingTimeLimit/Time,'/'),3)"/>
              </xsl:variable>
              <xsl:choose>
                <xsl:when test="$tkt!=''">
                  <xsl:choose>
                    <xsl:when test="substring($tkt,5,1)='P' and substring($tkt,1,2)='12'">
                      <xsl:value-of select="'T00'"/>
                    </xsl:when>
                    <xsl:when test="substring($tkt,5,1)='P'">
                      <xsl:value-of select="concat('T',substring($tkt,1,2) + 12)"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="concat('T',substring($tkt,1,2))"/>
                    </xsl:otherwise>
                  </xsl:choose>
                  <xsl:value-of select="concat(':',substring($tkt,3,2),':00')"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="'T00:00:00'"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:choose>
              <xsl:when test="$ttlm &lt; $nm">
                <xsl:value-of select="$ny + 1"/>
                <xsl:value-of select="concat('-',$ttlm,'-',$ttld,$tktime)"/>
              </xsl:when>
              <xsl:when test="$ttlm = $nm and $ttld &lt; ($nd - 2)">
                <xsl:value-of select="$ny + 1"/>
                <xsl:value-of select="concat('-',$ttlm,'-',$ttld,$tktime)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="concat($ny,'-',$ttlm,'-',$ttld,$tktime)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </xsl:when>
        <xsl:when test="AlreadyTicketed">
          <TicketAdvisory>
            <xsl:value-of select="string('OK')"/>
            <xsl:variable name="ttl">
              <xsl:value-of select="substring-before(substring-after(AlreadyTicketed/Code,'-'),'-')"/>
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
              <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,9,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,6,2)"/>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:choose>
                <xsl:when test="../../BookingDetails/FlightsRange/@Start!=''">
                  <xsl:value-of select="substring(../../BookingDetails/FlightsRange/@Start,1,4)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,1,4)"/>
                </xsl:otherwise>
              </xsl:choose>
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
                <xsl:value-of select="concat('-',$ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
              </xsl:otherwise>
            </xsl:choose>
          </TicketAdvisory>
        </xsl:when>
        <xsl:when test="TicketingTimeLimit">
          <TicketAdvisory>
            <xsl:value-of select="string('OK')"/>
            <xsl:variable name="ttl">
              <xsl:value-of select="substring-before(substring-after(AlreadyTicketed/Code,'-'),'-')"/>
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
              <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,9,2)"/>
            </xsl:variable>
            <xsl:variable name="nm">
              <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,6,2)"/>
            </xsl:variable>
            <xsl:variable name="ny">
              <xsl:choose>
                <xsl:when test="../../BookingDetails/FlightsRange/@Start!=''">
                  <xsl:value-of select="substring(../../BookingDetails/FlightsRange/@Start,1,4)"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="substring(../../BookingDetails/CreationTimestamp,1,4)"/>
                </xsl:otherwise>
              </xsl:choose>
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
                <xsl:value-of select="concat('-',$ny,'-',$ttlm,'-',$ttld,'T00:00:00')"/>
              </xsl:otherwise>
            </xsl:choose>
          </TicketAdvisory>
        </xsl:when>
      </xsl:choose>
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
          <xsl:when test="PaymentCard/@CardType='MA'">MC</xsl:when>
          <xsl:when test="PaymentCard/@CardType='DC'">DN</xsl:when>
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
    <FormOfPayment>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <PaymentCard>
        <xsl:attribute name="CardCode">
          <xsl:choose>
            <xsl:when test="substring(TPA_Extensions/Text,2,2)='CA'">MC</xsl:when>
            <xsl:when test="substring(TPA_Extensions/Text,2,2)='MA'">MC</xsl:when>
            <xsl:when test="substring(TPA_Extensions/Text,2,2)='DC'">DN</xsl:when>
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
    </FormOfPayment>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Other Service Information (OSI) Processing				    -->
  <!--************************************************************************************-->
  <xsl:template match="OpenReservationElement" mode="OSI">
    <OtherServiceInformation>
      <Airline>
        <xsl:choose>
          <xsl:when test="ServiceRequest/@airlineCode">
            <xsl:value-of select="ServiceRequest/@airlineCode"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="substring(FullText,1,2)"/>
          </xsl:otherwise>
        </xsl:choose>
      </Airline>
      <Text>
        <xsl:value-of select="ServiceRequest/FullText"/>
      </Text>
    </OtherServiceInformation>
  </xsl:template>
  <!--************************************************************************************-->
  <!--			Special Service Request (SSR) Processing				    -->
  <!--************************************************************************************-->
  <xsl:template match="OpenReservationElement" mode="SSR">
    <SpecialServiceRequest>
      <xsl:attribute name="SSRCode">
        <xsl:value-of select="ServiceRequest/@code"/>
      </xsl:attribute>
      <xsl:if test="NameAssociation/NameRefNumber!= ''">
        <xsl:attribute name="TravelerRefNumberRPHList">
          <xsl:value-of select="NameAssociation/NameRefNumber"/>
        </xsl:attribute>
      </xsl:if>
      <Airline>
        <xsl:attribute name="Code">
          <xsl:choose>
            <xsl:when test="SegmentAssociation/AirSegment/CarrierCode">
              <xsl:value-of select="SegmentAssociation/AirSegment/CarrierCode"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="substring(ServiceRequest/FullText,6,2)"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
      </Airline>
      <Text>
        <xsl:value-of select="ServiceRequest/FullText"/>
      </Text>
    </SpecialServiceRequest>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Seat Elements 	                               		    -->
  <!-- ************************************************************** -->
  <xsl:template match="PreReservedSeat" mode="Seat">
    <SeatRequest>
      <xsl:choose>
        <xsl:when test="SeatNumber">
          <xsl:attribute name="SeatNumber">
            <xsl:value-of select="SeatNumber"/>
          </xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="SeatPreference">
            <xsl:value-of select="Preference"/>
          </xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <xsl:attribute name="SmokingAllowed">false</xsl:attribute>
      <xsl:attribute name="Status">
        <xsl:value-of select="SeatStatusCode"/>
      </xsl:attribute>
      <xsl:attribute name="TravelerRefNumberRPHList">
        <xsl:variable name="paxpos">
          <xsl:value-of select="NameNumber"/>
        </xsl:variable>
        <xsl:value-of select="../../../../../../Passengers/Passenger[@nameId=$paxpos]/@nameAssocId"/>
      </xsl:attribute>
      <xsl:attribute name="FlightRefNumberRPHList">
        <xsl:value-of select="format-number(../../../../@sequence,'0')"/>
      </xsl:attribute>
      <xsl:if test="../../seatPaxInfo/seatPaxIndicator/statusDetails/indicator='C0'">
        <xsl:attribute name="Chargeable">true</xsl:attribute>
      </xsl:if>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="BoardPoint"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="OffPoint"/>
        </xsl:attribute>
      </ArrivalAirport>
    </SeatRequest>
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
  <!-- ************************************************************** -->
  <!-- TourCode Remarks	   	                                          -->
  <!-- ************************************************************** -->
  <xsl:template match="Details" mode="TourCode">
    <SpecialRemark>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="MiscellaneousInfo/TourNumber!=''">
          <xsl:attribute name="RemarkType">TourCode</xsl:attribute>
          <TravelerRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="NameAssociationInfo">
                <xsl:if test="position()>1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="@nameId"/>
              </xsl:for-each>
            </xsl:attribute>
          </TravelerRefNumber>
          <FlightRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="SegmentInfo">
                <xsl:if test="position()>1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="@number"/>
              </xsl:for-each>
            </xsl:attribute>
          </FlightRefNumber>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">TourCode</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <Text>
        <xsl:value-of select="MiscellaneousInfo/TourNumber"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- ************************************************************** -->
  <!-- Endorsement Remarks	   	                              -->
  <!-- ************************************************************** -->
  <xsl:template match="MessageInfo" mode="Endorsement">
    <SpecialRemark>
      <xsl:attribute name="RPH">
        <xsl:value-of select="position()"/>
      </xsl:attribute>
      <xsl:choose>
        <xsl:when test="Remarks[@type='ENS']">
          <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
          <TravelerRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="../NameAssociationInfo">
                <xsl:if test="position()>1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="@nameId"/>
              </xsl:for-each>
            </xsl:attribute>
          </TravelerRefNumber>
          <FlightRefNumber>
            <xsl:attribute name="RPH">
              <xsl:for-each select="../SegmentInfo">
                <xsl:if test="position()>1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="@number"/>
              </xsl:for-each>
            </xsl:attribute>
          </FlightRefNumber>
        </xsl:when>
        <xsl:otherwise>
          <xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <Text>
        <xsl:value-of select="Remarks[@type='ENS']"/>
      </Text>
    </SpecialRemark>
  </xsl:template>
  <!-- Miscellaneous other               -->
  <!-- ********************************************************************************* -->
  <xsl:template match="General" mode="Other">
    <Item>
      <xsl:attribute name="Status">
        <xsl:value-of select="Line/@Status"/>
      </xsl:attribute>
      <xsl:attribute name="ItinSeqNumber">
        <xsl:value-of select="../@sequence"/>
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
            <xsl:value-of select="NumberInParty"/>
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


  <!-- ************************************************************** -->
  <!-- Replace a string 	   	                                          -->
  <!-- ************************************************************** -->
  <xsl:template name="string-replace-all">
    <xsl:param name="text" />
    <xsl:param name="replace" />
    <xsl:param name="by" />
    <xsl:choose>
      <xsl:when test="contains($text, $replace)">
        <xsl:value-of select="substring-before($text,$replace)" />
        <xsl:value-of select="$by" />
        <xsl:call-template name="string-replace-all">
          <xsl:with-param name="text"
          select="substring-after($text,$replace)" />
          <xsl:with-param name="replace" select="$replace" />
          <xsl:with-param name="by" select="$by" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$text" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="rphFormat">
    <xsl:param name="rn" />
    <xsl:variable name="whole" select="format-number(number(substring-before($rn, '.')),'00')"/>
    <xsl:variable name="tenth" select="format-number(number(substring-after($rn, '.')),'00')"/>

    <xsl:value-of select="concat($whole,'.',$tenth)" />
  </xsl:template>

  <xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

  <!-- Strips leading whitespace characters from 'string' -->
  <xsl:template name="string-ltrim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />

    <xsl:if test="string-length($string) &gt; 0">
      <xsl:choose>
        <xsl:when test="contains($trim, substring($string, 1, 1))">
          <xsl:call-template name="string-ltrim">
            <xsl:with-param name="string" select="substring($string, 2)" />
            <xsl:with-param name="trim"   select="$trim" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- Strips trailing whitespace characters from 'string' -->
  <xsl:template name="string-rtrim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />

    <xsl:variable name="length" select="string-length($string)" />

    <xsl:if test="$length &gt; 0">
      <xsl:choose>
        <xsl:when test="contains($trim, substring($string, $length, 1))">
          <xsl:call-template name="string-rtrim">
            <xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
            <xsl:with-param name="trim"   select="$trim" />
          </xsl:call-template>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$string" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
  </xsl:template>

  <!-- Strips leading and trailing whitespace characters from 'string' -->
  <xsl:template name="string-trim">
    <xsl:param name="string" />
    <xsl:param name="trim" select="$whitespace" />
    <xsl:call-template name="string-rtrim">
      <xsl:with-param name="string">
        <xsl:call-template name="string-ltrim">
          <xsl:with-param name="string" select="$string" />
          <xsl:with-param name="trim"   select="$trim" />
        </xsl:call-template>
      </xsl:with-param>
      <xsl:with-param name="trim"   select="$trim" />
    </xsl:call-template>
  </xsl:template>

  <!--############################################################-->
  <!--## Template to tokenize strings                           ##-->
  <!--############################################################-->
  <xsl:template name="tokenizeString">
    <!--passed template parameter -->
    <xsl:param name="list"/>
    <xsl:param name="delimiter"/>
    <xsl:choose>
      <xsl:when test="contains($list, $delimiter)">
        <elem>
          <!-- get everything in front of the first delimiter -->
          <xsl:value-of select="substring-before($list,$delimiter)"/>
        </elem>
        <xsl:call-template name="tokenizeString">
          <!-- store anything left in another variable -->
          <xsl:with-param name="list" select="substring-after($list,$delimiter)"/>
          <xsl:with-param name="delimiter" select="$delimiter"/>
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:choose>
          <xsl:when test="$list = ''">
            <xsl:text/>
          </xsl:when>
          <xsl:otherwise>
            <elem>
              <xsl:value-of select="$list"/>
            </elem>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
