<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- 
  ================================================================== 
  v04_Sabre_TravelBuildRQ.xsl 															     
  ================================================================== 
  Date: 31 May 2021 - Kobelev Code Refactoring														       
  Date: 21 APR 2010 - Shashin														       
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:variable name="PCC">
    <xsl:value-of select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
  </xsl:variable>
  <xsl:variable name="paxtype">
    <xsl:choose>
      <xsl:when test="$PCC = 'B66B' or $PCC = 'B67B' or $PCC = 'B68B'">Y</xsl:when>
      <xsl:otherwise>N</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:template match="/">
    <TravelBuild>
      <xsl:apply-templates select="OTA_TravelItineraryRQ"/>
    </TravelBuild>
  </xsl:template>

  <xsl:template match="OTA_TravelItineraryRQ">
    <AddInfo>
      <xsl:choose>
        <xsl:when test="$paxtype='Y'">
          <xsl:apply-templates select="TPA_Extensions/PNRData" mode="ixplore"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="TPA_Extensions/PNRData" mode="other"/>
        </xsl:otherwise>
      </xsl:choose>
    </AddInfo>
    <xsl:if test="OTA_AirBookRQ">
      <AirBook>
        <xsl:apply-templates select="OTA_AirBookRQ"/>
      </AirBook>
    </xsl:if>
    <xsl:if test="OTA_HotelResRQ">
      <HotelBook>
        <xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation"/>
      </HotelBook>
    </xsl:if>
    <xsl:if test="OTA_VehResRQ">
      <CarBook>
        <xsl:apply-templates select="OTA_VehResRQ/VehResRQCore"/>
      </CarBook>
    </xsl:if>
    <xsl:if test="OTA_AirBookRQ/MiscellaneousSegments/Segment">
      <MiscellaneousSegments>
        <xsl:apply-templates select="OTA_AirBookRQ/MiscellaneousSegments/Segment" mode="Seg"/>
      </MiscellaneousSegments>
    </xsl:if>
    <!--xsl:if test="TPA_Extensions/PriceData">
			<Pricing>
				<xsl:apply-templates select="TPA_Extensions/PriceData"/>
			</Pricing>
		</xsl:if-->
    <!--********************PassengerDetails************************-->
    <PassengerDetails>
      <PassengerDetailsRQ xmlns="http://services.sabre.com/sp/pd/v3_4" Version="3.4.0">
        <POS>
          <Source>
            <xsl:choose>
              <xsl:when test="POS/Source/@AgentSin !=''">
                <xsl:attribute name="AgentSine">
                  <xsl:value-of select="POS/Source/@AgentSine"/>
                </xsl:attribute>
              </xsl:when>
            </xsl:choose>
            <xsl:attribute name="AirlineVendorID">
              <xsl:value-of select="Airline/@Code"/>
            </xsl:attribute>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC"/>
            </xsl:attribute>
          </Source>
        </POS>
        <TravelerInfo>
          <xsl:for-each select="Traveler">
            <PersonName>
              <xsl:attribute name="TravelerRefNumber">
                <xsl:value-of select="TravelerRefNumber/@RPH"/>
                <GivenName>
                  <xsl:value-of select="PersonName/GivenName"/>
                  <xsl:if test="PersonName/NamePrefix != ''">
                    <xsl:text/>
                    <xsl:value-of select="PersonName/NamePrefix"/>
                  </xsl:if>
                </GivenName>
                <Surname>
                  <xsl:value-of select="PersonName/Surname"/>
                </Surname>
                <xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
                  <Infant Ind="true"/>
                </xsl:if>
              </xsl:attribute>
            </PersonName>
          </xsl:for-each>

          <Telephone>
            <xsl:attribute name="PhoneLocationType">
              <xsl:choose>
                <xsl:when test="@PhoneLocationType='Home'">H</xsl:when>
                <xsl:when test="@PhoneLocationType='Work'">W</xsl:when>
                <xsl:when test="@PhoneLocationType='Mobile'">M</xsl:when>
                <xsl:when test="@PhoneLocationType='Fax'">F</xsl:when>
                <xsl:when test="@PhoneLocationType='Business'">B</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="@PhoneLocationType"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:if test="@AreaCityCode!=''">
              <xsl:attribute name="AreaCityCode">
                <xsl:value-of select="@AreaCityCode"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="PhoneNumber">
              <xsl:value-of select="@PhoneNumber"/>
            </xsl:attribute>
          </Telephone>


          <Telephone>
            <xsl:attribute name="PhoneUseType">
              <xsl:choose>
                <xsl:when test="@PhoneLocationType='Home'">H</xsl:when>
                <xsl:when test="@PhoneLocationType='Work'">W</xsl:when>
                <xsl:when test="@PhoneLocationType='Mobile'">M</xsl:when>
                <xsl:when test="@PhoneLocationType='Fax'">F</xsl:when>
                <xsl:when test="@PhoneLocationType='Business'">B</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="@PhoneLocationType"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:if test="@AreaCityCode!=''">
              <xsl:attribute name="AreaCityCode">
                <xsl:value-of select="@AreaCityCode"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="PhoneNumber">
              <xsl:value-of select="@PhoneNumber"/>
            </xsl:attribute>
          </Telephone>

          <Email>
            <xsl:attribute name="EmailAddress">
              <xsl:value-of select="Email"/>
            </xsl:attribute>
          </Email>
          <Address>
          </Address>

          <xsl:for-each select="Traveler">
            <xsl:variable name="RefNumber">
              <xsl:choose>
                <xsl:when test="TravelerRefNumber/@RPH != ''">
                  <xsl:value-of select="TravelerRefNumber/@RPH" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="position()"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:apply-templates select="CustLoyalty" mode="ixplore">
              <xsl:with-param name="RefNumber">
                <xsl:value-of select="$RefNumber"/>
              </xsl:with-param>
            </xsl:apply-templates>
          </xsl:for-each>

          <xsl:for-each select="Traveler">
            <xsl:variable name="refnum">
              <xsl:choose>
                <xsl:when test="TravelerRefNumber/@RPH != ''">
                  <xsl:value-of select="TravelerRefNumber/@RPH" />
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="position()"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:variable>
            <xsl:if test="@PassengerTypeCode != '' and $paxtype='Y'">
              <PassengerType>
                <xsl:attribute name="Code">
                  <xsl:value-of select="@PassengerTypeCode"/>
                </xsl:attribute>
                <xsl:attribute name="NameNumber">
                  <xsl:value-of select="$refnum"/>
                  <xsl:choose>
                    <xsl:when test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
                      <xsl:text>.1</xsl:text>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:text>.1</xsl:text>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </PassengerType>
            </xsl:if>
          </xsl:for-each>
          <Ticketing>
            <xsl:attribute name="TicketType">7TAW</xsl:attribute>
            <xsl:attribute name="TicketTimeLimit">
              <xsl:value-of select="Ticketing/@TicketTimeLimit"/>
            </xsl:attribute>
            <xsl:if test="Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!=''">
              <xsl:choose>
                <xsl:when test="Queue/@QueueNumber!=''">
                  <xsl:attribute name="PseudoCityCode">
                    <xsl:value-of select="Queue/@PseudoCityCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="QueueNumber">
                    <xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="PseudoCityCode">
                    <xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/>
                  </xsl:attribute>
                  <xsl:attribute name="QueueNumber">
                    <xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:if>
          </Ticketing>
        </TravelerInfo>
        <Remarks>
          <PaymentDetails>
            <PaymentDetail Type="CC">

              <PaymentCard>
                <xsl:attribute name="CardCode">
                  <xsl:choose>
                    <xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MC'">CA</xsl:when>
                    <xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='DS'">DI</xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="CardNumber">
                  <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
                </xsl:attribute>
                <xsl:attribute name="ExpireDate">
                  <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate"/>
                </xsl:attribute>
              </PaymentCard>
            </PaymentDetail>
          </PaymentDetails>
        </Remarks>
        <SeatRequest>
        </SeatRequest>
        <PriceQuoteInfo>
        </PriceQuoteInfo>
        <EndTransactionRQ>
          <UpdatedBy>
            <TPA_Extensions>
              <Access>
                <AccessPerson>
                  <GivenName>
                    <xsl:choose>
                      <xsl:when test="POS/Source/@AgentSine != ''">
                        <xsl:value-of select="POS/Source/@AgentSine"/>
                      </xsl:when>
                      <xsl:otherwise>TRIPXML</xsl:otherwise>
                    </xsl:choose>
                  </GivenName>
                </AccessPerson>
              </Access>
            </TPA_Extensions>
          </UpdatedBy>
          <EndTransaction Ind="true">
            <SendEmail Ind="true"/>
          </EndTransaction>
        </EndTransactionRQ>
        <Queue>
          <QPlace>
            <xsl:attribute name="QueueNumber">
              <xsl:value-of select="OTA_AirBookRQ/Queue/@QueueNumber"/>
            </xsl:attribute>
            <xsl:attribute name="SystemCode">
              <xsl:value-of select="OTA_AirBookRQ/Queue/@QueueCategory"/>
            </xsl:attribute>

            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="OTA_AirBookRQ/Queue/@PseudoCityCode"/>
            </xsl:attribute>
          </QPlace>
        </Queue>
      </PassengerDetailsRQ>
    </PassengerDetails>
    <!--
		<Enhanced_AirBook>
			<Enhanced_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" TimeStamp="2001-12-17T09:30:47-05:00" Version="OTA_2007A.TsabreXML1.2.1">
				<POS>
					<Source>
						<xsl:choose>
							<xsl:when test="POS/Source/@AgentSin !=''">
								<xsl:attribute name="AgentSine">
									<xsl:value-of select="POS/Source/@AgentSine"/>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<xsl:attribute name="AirlineVendorID">
							<xsl:value-of select="Airline/@Code"/>
						</xsl:attribute>
						<xsl:attribute name="PseudoCityCode">
							<xsl:value-of select="$PCC"/>
						</xsl:attribute>
					</Source>
				</POS>
				<AirBookRQ>
					<AirItinerary>
						<HaltOnError Ind="true"/>
						<RedisplayReservation WaitInterval="5000" NumAttempts="2" HaltOnUCStatus="true"/>
						<OriginDestinationOptions>
							<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption"/>
						</OriginDestinationOptions>
					</AirItinerary>
				</AirBookRQ>
				<AirPriceRQ>
					<HaltOnError Ind="true"/>
					<PriceRequestInformation>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="PassengerFare/BaseFare/@CurrencyCode"/>
						</xsl:attribute>
						
					</PriceRequestInformation>
				</AirPriceRQ>
				<IgnoreAfter>
					<HaltOnError Ind="true"/>
					<IgnoreTransaction Ind="true"/>
				</IgnoreAfter>
			</Enhanced_AirBookRQ>
		</Enhanced_AirBook>
		-->
    <!-- ******************************************** -->
    <xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/DirectBill/Address or OTA_AirBookRQ/Fulfillment/DeliveryAddress">
      <Remarks>
        <AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
          <POS>
            <xsl:value-of select="$PCC"/>
          </POS>
          <xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark">
            <BasicRemark>
              <xsl:attribute name="Text">
                <xsl:value-of select="."/>
              </xsl:attribute>
            </BasicRemark>
          </xsl:for-each>
          <xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address" mode="billing"/>
          <xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/DirectBill/Address" mode="billing"/>
          <xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
          <xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail">
            <FOPRemark>
              <CCInfo>
                <CreditCardVendor>
                  <xsl:attribute name="Code">
                    <xsl:choose>
                      <xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MC'">CA</xsl:when>
                      <xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='DS'">DI</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                </CreditCardVendor>
                <CreditCardNumber>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
                  </xsl:attribute>
                </CreditCardNumber>
                <CreditCardExpiration>
                  <xsl:attribute name="ExpireDate">
                    <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate"/>
                  </xsl:attribute>
                </CreditCardExpiration>
              </CCInfo>
            </FOPRemark>
          </xsl:if>
        </AddRemarkRQ>
      </Remarks>
    </xsl:if>
    <!--xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard or TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='CHD' or @PassengerTypeCode='INF'] or OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest or OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation"-->
    <xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest or OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation or TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='INF']">
      <SpecialServices>
        <SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.1.1">
          <POS>
            <Source>
              <xsl:attribute name="PseudoCityCode">
                <xsl:value-of select="$PCC"/>
              </xsl:attribute>
            </Source>
          </POS>
          <!--xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
						<Service SSRCode="OTHS">
							<Text>
								<xsl:text>CH </xsl:text>
								<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/CardHolderName"/>
							</Text>
						</Service>
						<Service SSRCode="OTHS">
							<Text>
								<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/>
								<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
								<xsl:text>EXP </xsl:text>
								<xsl:value-of select="substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,1,2)"/>
								<xsl:text> </xsl:text>
								<xsl:value-of select="substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,3,2)"/>
							</Text>
						</Service>
					</xsl:if-->
          <!--xsl:if test="TPA_Extensions/PNRData/Traveler[substring(@PassengerTypeCode,1,1)='C' or @PassengerTypeCode='INF']"-->
          <xsl:if test="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='INF']">
            <!--xsl:for-each select="TPA_Extensions/PNRData/Traveler[substring(@PassengerTypeCode,1,1)='C']">
							<Service SSRCode="CHLD">
								<TPA_Extensions>
									<Name>
										<xsl:attribute name="Number">
											<xsl:value-of select="TravelerRefNumber/@RPH"/>
											<xsl:text>.1</xsl:text>
										</xsl:attribute>
									</Name>
								</TPA_Extensions>
								<Text>
									<xsl:value-of select="substring(@BirthDate,9)"/>
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring(@BirthDate,6,2)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:value-of select="substring(@BirthDate,3,2)" />
								</Text>
							</Service>
						</xsl:for-each-->
            <xsl:for-each select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='INF']">
              <Service SSRCode="INFT">
                <TPA_Extensions>
                  <Name>
                    <xsl:attribute name="Number">1.1</xsl:attribute>
                  </Name>
                </TPA_Extensions>
                <Text>
                  <xsl:value-of select="PersonName/Surname"/>
                  <xsl:text>/</xsl:text>
                  <xsl:value-of select="PersonName/GivenName"/>
                  <xsl:if test="PersonName/NamePrefix != ''">
                    <xsl:value-of select="string(' ')"/>
                    <xsl:value-of select="PersonName/NamePrefix"/>
                  </xsl:if>
                  <xsl:text>/</xsl:text>
                  <xsl:value-of select="substring(@BirthDate,9)"/>
                  <xsl:call-template name="month">
                    <xsl:with-param name="month">
                      <xsl:value-of select="substring(@BirthDate,6,2)"/>
                    </xsl:with-param>
                  </xsl:call-template>
                  <xsl:value-of select="substring(@BirthDate,3,2)"/>
                </Text>
              </Service>
            </xsl:for-each>
          </xsl:if>
          <xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest">
            <xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest">
              <Service>
                <xsl:attribute name="SSRCode">
                  <xsl:value-of select="@SSRCode"/>
                </xsl:attribute>
                <TPA_Extensions>
                  <Name>
                    <xsl:attribute name="Number">
                      <xsl:value-of select="@TravelerRefNumberRPHList"/>
                      <xsl:text>.1</xsl:text>
                    </xsl:attribute>
                  </Name>
                </TPA_Extensions>
                <Text>
                  <xsl:value-of select="Text"/>
                </Text>
              </Service>
            </xsl:for-each>
          </xsl:if>
          <xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation">
            <xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation">
              <Service>
                <xsl:attribute name="SSRCode">OSI</xsl:attribute>
                <Airline>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="Airline/@Code"/>
                  </xsl:attribute>
                </Airline>
                <!--TPA_Extensions>
									<Name>
										<xsl:attribute name="Number">
											<xsl:value-of select="@TravelerRefNumberRPHList"/>
											<xsl:text>.1</xsl:text>
										</xsl:attribute>
									</Name>
								</TPA_Extensions-->
                <Text>
                  <xsl:value-of select="Text"/>
                </Text>
              </Service>
            </xsl:for-each>
          </xsl:if>
        </SpecialServiceRQ>
      </SpecialServices>
    </xsl:if>
    <xsl:if test="OTA_AirBookRQ/Queue/@QueueNumber!=''">
      <Queue>
        <QPlaceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
          <POS>
            <Source>
              <xsl:attribute name="PseudoCityCode">
                <xsl:value-of select="$PCC"/>
              </xsl:attribute>
            </Source>
          </POS>
          <QInfo>
            <QueueIdentifier>
              <xsl:attribute name="PseudoCityCode">
                <xsl:value-of select="OTA_AirBookRQ/Queue/@PseudoCityCode"/>
              </xsl:attribute>
              <xsl:attribute name="Number">
                <xsl:value-of select="OTA_AirBookRQ/Queue/@QueueNumber"/>
              </xsl:attribute>
            </QueueIdentifier>
            <InstructionCode>
              <xsl:attribute name="Code">
                <xsl:value-of select="OTA_AirBookRQ/Queue/@QueueCategory"/>
              </xsl:attribute>
            </InstructionCode>
          </QInfo>
          <UniqueID/>
        </QPlaceRQ>
      </Queue>
    </xsl:if>
    <ET>
      <EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.01">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC"/>
            </xsl:attribute>
          </Source>
        </POS>
        <UpdatedBy>
          <TPA_Extensions>
            <Access>
              <AccessPerson>
                <GivenName>
                  <xsl:choose>
                    <xsl:when test="POS/Source/@AgentSine != ''">
                      <xsl:value-of select="POS/Source/@AgentSine"/>
                    </xsl:when>
                    <xsl:otherwise>TRIPXML</xsl:otherwise>
                  </xsl:choose>
                </GivenName>
              </AccessPerson>
            </Access>
          </TPA_Extensions>
        </UpdatedBy>
        <EndTransaction Ind="true"/>
      </EndTransactionRQ>
    </ET>
    <Read>
      <GetReservationRQ Version="1.19.0" xmlns="http://webservices.sabre.com/pnrbuilder/v1_19">
        <RequestType>Stateful</RequestType>
        <ReturnOptions PriceQuoteServiceVersion="4.0.0">
          <SubjectAreas>
            <SubjectArea>PRICE_QUOTE</SubjectArea>
          </SubjectAreas>
        </ReturnOptions>
      </GetReservationRQ>
    </Read>
    <Ignore>
      <IgnoreTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
        <POS>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC"/>
          </xsl:attribute>
        </POS>
        <IgnoreTransaction Ind="true"/>
      </IgnoreTransactionRQ>
    </Ignore>
  </xsl:template>
  <!--************************************************************-->
  <!--  		Customer Information                                -->
  <!--************************************************************-->
  <xsl:template match="PNRData" mode="ixplore">
    <TravelItineraryAddInfoRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC"/>
          </xsl:attribute>
        </Source>
      </POS>
      <CustomerInfo>
        <xsl:apply-templates select="Traveler"/>
        <xsl:apply-templates select="Telephone" mode="ixplore"/>
        <!--xsl:if test="$RefNumber = '1'"-->
        <Email>
          <xsl:attribute name="EmailAddress">
            <xsl:value-of select="Email"/>
          </xsl:attribute>
          <!--xsl:attribute name="NameNumber">
							<xsl:value-of select="$RefNumber" />
							<xsl:text>.1</xsl:text>
						</xsl:attribute-->
        </Email>
        <!--/xsl:if-->
        <xsl:for-each select="Traveler">
          <xsl:variable name="RefNumber">
            <xsl:choose>
              <xsl:when test="TravelerRefNumber/@RPH != ''">
                <xsl:value-of select="TravelerRefNumber/@RPH"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="position()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:apply-templates select="CustLoyalty" mode="ixplore">
            <xsl:with-param name="RefNumber">
              <xsl:value-of select="$RefNumber"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </xsl:for-each>
        <xsl:if test="AccountingLine!=''">
          <CustomerIdentifier>
            <xsl:attribute name="Identifier">
              <xsl:value-of select="AccountingLine"/>
            </xsl:attribute>
          </CustomerIdentifier>
        </xsl:if>
        <xsl:for-each select="Traveler">
          <xsl:variable name="refnum">
            <xsl:choose>
              <xsl:when test="TravelerRefNumber/@RPH != ''">
                <xsl:value-of select="TravelerRefNumber/@RPH"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="position()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:if test="@PassengerTypeCode != '' and $paxtype='Y'">
            <PassengerType>
              <xsl:attribute name="Code">
                <xsl:value-of select="@PassengerTypeCode"/>
              </xsl:attribute>
              <xsl:attribute name="TravelerRefNumber">
                <xsl:value-of select="$refnum"/>
                <xsl:choose>
                  <xsl:when test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
                    <xsl:text>.1</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>.1</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </PassengerType>
          </xsl:if>
        </xsl:for-each>
      </CustomerInfo>
      <AgencyInfo>
        <Ticketing>
          <xsl:attribute name="TicketType">7TAW</xsl:attribute>
          <xsl:attribute name="TicketTimeLimit">
            <xsl:value-of select="Ticketing/@TicketTimeLimit"/>
          </xsl:attribute>
          <xsl:if test="Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!=''">
            <xsl:choose>
              <xsl:when test="Queue/@QueueNumber!=''">
                <xsl:attribute name="PseudoCityCode">
                  <xsl:value-of select="Queue/@PseudoCityCode"/>
                </xsl:attribute>
                <xsl:attribute name="QueueNumber">
                  <xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="PseudoCityCode">
                  <xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/>
                </xsl:attribute>
                <xsl:attribute name="QueueNumber">
                  <xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
        </Ticketing>
      </AgencyInfo>
    </TravelItineraryAddInfoRQ>
  </xsl:template>

  <xsl:template match="PNRData" mode="other">
    <TravelItineraryAddInfoRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.2.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC"/>
          </xsl:attribute>
        </Source>
      </POS>
      <CustomerInfo>
        <xsl:apply-templates select="Traveler"/>
        <xsl:apply-templates select="Telephone" mode="other"/>
        <!--xsl:if test="$RefNumber = '1'"-->
        <Email>
          <xsl:attribute name="EmailAddress">
            <xsl:value-of select="Email"/>
          </xsl:attribute>
          <!--xsl:attribute name="NameNumber">
							<xsl:value-of select="$RefNumber" />
							<xsl:text>.1</xsl:text>
						</xsl:attribute-->
        </Email>
        <!--/xsl:if-->
        <xsl:for-each select="Traveler">
          <xsl:variable name="RefNumber">
            <xsl:choose>
              <xsl:when test="TravelerRefNumber/@RPH != ''">
                <xsl:value-of select="TravelerRefNumber/@RPH"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="position()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:apply-templates select="CustLoyalty" mode="other">
            <xsl:with-param name="RefNumber">
              <xsl:value-of select="$RefNumber"/>
            </xsl:with-param>
          </xsl:apply-templates>
        </xsl:for-each>
        <xsl:if test="AccountingLine!=''">
          <CustomerIdentifier>
            <xsl:attribute name="Identifier">
              <xsl:value-of select="AccountingLine"/>
            </xsl:attribute>
          </CustomerIdentifier>
        </xsl:if>
        <xsl:for-each select="Traveler">
          <xsl:variable name="refnum">
            <xsl:choose>
              <xsl:when test="TravelerRefNumber/@RPH != ''">
                <xsl:value-of select="TravelerRefNumber/@RPH"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="position()"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:if test="@PassengerTypeCode != '' and $paxtype='Y'">
            <PassengerType>
              <xsl:attribute name="Code">
                <xsl:value-of select="@PassengerTypeCode"/>
              </xsl:attribute>
              <xsl:attribute name="NameNumber">
                <xsl:value-of select="$refnum"/>
                <xsl:choose>
                  <xsl:when test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
                    <xsl:text>.1</xsl:text>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>.1</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
            </PassengerType>
          </xsl:if>
        </xsl:for-each>
      </CustomerInfo>
      <AgencyInfo>
        <Ticketing>
          <xsl:attribute name="TicketingDate">
            <xsl:choose>
              <xsl:when test="Ticketing/@TicketTimeLimit!=''">
                <xsl:value-of select="substring(Ticketing/@TicketTimeLimit,1,10)"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="substring(../../OTA_AirBookRQ/Ticketing/@TicketTimeLimit,1,10)"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:if test="Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!=''">
            <xsl:choose>
              <xsl:when test="Queue/@QueueNumber!=''">
                <xsl:attribute name="PseudoCityCode">
                  <xsl:value-of select="Queue/@PseudoCityCode"/>
                </xsl:attribute>
                <xsl:attribute name="QueueNumber">
                  <xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
                <xsl:attribute name="PseudoCityCode">
                  <xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/>
                </xsl:attribute>
                <xsl:attribute name="QueueNumber">
                  <xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/>
                </xsl:attribute>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:if>
        </Ticketing>
      </AgencyInfo>
    </TravelItineraryAddInfoRQ>
  </xsl:template>

  <xsl:template match="Traveler">
    <xsl:variable name="RefNumber">
      <xsl:choose>
        <xsl:when test="TravelerRefNumber/@RPH != ''">
          <xsl:value-of select="TravelerRefNumber/@RPH"/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="position()"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <PersonName>

      <xsl:attribute name="RPH">
        <xsl:value-of select="$RefNumber"/>
      </xsl:attribute>
      <GivenName>
        <xsl:value-of select="PersonName/GivenName"/>
        <xsl:if test="PersonName/NamePrefix != ''">
          <xsl:text> </xsl:text>
          <xsl:value-of select="PersonName/NamePrefix"/>
        </xsl:if>
      </GivenName>
      <Surname>
        <xsl:value-of select="PersonName/Surname"/>
      </Surname>
      <xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
        <Infant Ind="true"/>
      </xsl:if>
    </PersonName>
  </xsl:template>
  <!--************************************************************-->
  <!--  			 Air Itinerary                                       -->
  <!--************************************************************-->
  <xsl:template match="OTA_AirBookRQ">
    <OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC"/>
          </xsl:attribute>
        </Source>
      </POS>
      <AirItinerary>
        <xsl:attribute name="DirectionInd">
          <xsl:choose>
            <xsl:when test="AirItinerary/@DirectionInd = 'Circle'">Return</xsl:when>
            <xsl:when test="not(AirItinerary/@DirectionInd)">
              <xsl:choose>
                <xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode = AirItinerary/OriginDestinationOptions/OriginDestinationOption[position()=last()]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode">Return</xsl:when>
                <xsl:otherwise>Oneway</xsl:otherwise>
              </xsl:choose>
            </xsl:when>
            <xsl:otherwise>Oneway</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <OriginDestinationOptions>
          <xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption"/>
        </OriginDestinationOptions>
      </AirItinerary>
    </OTA_AirBookRQ>
  </xsl:template>
  <!--************************************************************-->
  <!--  			           Hotel                                       -->
  <!--************************************************************-->
  <xsl:template match="HotelReservation">
    <HotelReservation>
      <RoomStays>
        <RoomStay>
          <RoomTypes>
            <RoomType>
              <xsl:attribute name="RoomTypeCode">
                <xsl:value-of select="RoomStays/RoomStay/RoomRates/RoomRate/@RatePlanCode"/>
              </xsl:attribute>
              <xsl:attribute name="NumberOfUnits">
                <xsl:value-of select="RoomStays/RoomStay/RoomRates/RoomRate/@NumberOfUnits"/>
              </xsl:attribute>
              <xsl:attribute name="ReqdGuaranteeType">GuaranteeRequired</xsl:attribute>
            </RoomType>
          </RoomTypes>
          <GuestCounts>
            <GuestCount>
              <xsl:attribute name="Count">
                <xsl:value-of select="RoomStays/RoomStay/GuestCounts/GuestCount /@Count"/>
              </xsl:attribute>
            </GuestCount>
          </GuestCounts>
          <TimeSpan>
            <xsl:attribute name="Start">
              <xsl:value-of select="substring(string(RoomStays/RoomStay/TimeSpan/@Start),1,10)"/>
            </xsl:attribute>
            <xsl:attribute name="End">
              <xsl:value-of select="substring(string(RoomStays/RoomStay/TimeSpan/@End),1,10)"/>
            </xsl:attribute>
          </TimeSpan>
          <BasicPropertyInfo>
            <xsl:attribute name="HotelCode">
              <xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@HotelCode"/>
            </xsl:attribute>
            <!--Note: sabre doesn't use ChainCode - only HotelCode -->
            <!--xsl:attribute name="ChainCode"><xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@ChainCode"/></xsl:attribute-->
          </BasicPropertyInfo>
          <xsl:if test="RoomStays/RoomStay/Memberships">
            <Memberships>
              <xsl:apply-templates select="RoomStays/RoomStay/Memberships/Membership"/>
            </Memberships>
          </xsl:if>
        </RoomStay>
      </RoomStays>
    </HotelReservation>
  </xsl:template>
  <!--************************************************************-->
  <!--  			               Car                                      -->
  <!--************************************************************-->
  <xsl:template match="OTA_VehResRQ">
    <VehResRQCore>
      <VehRentalCore>
        <xsl:attribute name="PickUpDateTime">
          <xsl:value-of select="VehResRQCore/VehRentalCore/@PickUpDateTime"/>
        </xsl:attribute>
        <xsl:attribute name="ReturnDateTime">
          <xsl:value-of select="VehResRQCore/VehRentalCore/@ReturnDateTime"/>
        </xsl:attribute>
        <PickUpLocation>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode"/>
          </xsl:attribute>
        </PickUpLocation>
        <ReturnLocation>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="VehResRQCore/VehRentalCore/ReturnLocation /@LocationCode"/>
          </xsl:attribute>
        </ReturnLocation>
      </VehRentalCore>
      <xsl:if test="Customer">
        <Customer>
          <Primary>
            <xsl:apply-templates select="VehResRQCore/Customer/Primary/CustLoyalty"/>
          </Primary>
        </Customer>
      </xsl:if>
      <xsl:if test="VehResRQCore/VendorPref">
        <VendorPref>
          <xsl:attribute name="Code">
            <xsl:value-of select="VehResRQCore/VendorPref/@Code"/>
          </xsl:attribute>
        </VendorPref>
      </xsl:if>
      <VehPref>
        <xsl:choose>
          <xsl:when test="VehResRQCore/VehPref/@AirConditionInd!=''">
            <xsl:attribute name="AirConditionInd">
              <xsl:value-of select="VehResRQCore/VehPref/@AirConditionInd"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="AirConditionInd">1</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="VehResRQCore/VehPref/@TransmissionType!=''">
            <xsl:attribute name="TransmissionType">
              <xsl:value-of select="VehResRQCore/VehPref/@TransmissionType"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="TransmissionType">Automatic</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <VehType>
          <xsl:attribute name="VehicleCategory">
            <xsl:value-of select="VehResRQCore/VehPref/VehType/@VehicleCategory"/>
          </xsl:attribute>
        </VehType>
      </VehPref>
      <RateQualifier>
        <xsl:if test="VehResRQCore/RateQualifier/@RateCategory!=''">
          <xsl:attribute name="RateCategory">
            <xsl:value-of select="VehResRQCore/RateQualifier/@RateCategory"/>
          </xsl:attribute>
        </xsl:if>
      </RateQualifier>
      <TPA_Extensions>
        <NumberOfCars>
          <xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/@NumCars"/>
        </NumberOfCars>
      </TPA_Extensions>
    </VehResRQCore>
  </xsl:template>

  <xsl:template match="OriginDestinationOption">
    <OriginDestinationOption>
      <xsl:apply-templates select="FlightSegment"/>
    </OriginDestinationOption>
  </xsl:template>

  <xsl:template match="FlightSegment">
    <FlightSegment>
      <xsl:attribute name="ActionCode">NN</xsl:attribute>
      <xsl:attribute name="DepartureDateTime">
        <xsl:value-of select="@DepartureDateTime"/>
      </xsl:attribute>
      <xsl:attribute name="ArrivalDateTime">
        <xsl:value-of select="@ArrivalDateTime"/>
      </xsl:attribute>
      <xsl:attribute name="FlightNumber">
        <xsl:value-of select="@FlightNumber"/>
      </xsl:attribute>
      <xsl:attribute name="NumberInParty">
        <xsl:value-of select="@NumberInParty"/>
      </xsl:attribute>
      <xsl:attribute name="ResBookDesigCode">
        <xsl:value-of select="@ResBookDesigCode"/>
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="DepartureAirport/@LocationCode"/>
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="ArrivalAirport/@LocationCode"/>
        </xsl:attribute>
      </ArrivalAirport>
      <OperatingAirline>
        <xsl:choose>
          <xsl:when test="OperatingAirline">
            <xsl:attribute name="Code">
              <xsl:value-of select="OperatingAirline/@Code"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="Code">
              <xsl:value-of select="MarketingAirline/@Code"/>
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
      </OperatingAirline>
      <xsl:if test="Equipment">
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
    </FlightSegment>
  </xsl:template>

  <xsl:template match="PTC_FareBreakdown">
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="PassengerTypeQuantity/@Code"/>
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:apply-templates select="FareBasisCodes/FareBasisCode"/>
      <PassengerFare>
        <xsl:attribute name="NegotiatedFare">
          <xsl:choose>
            <xsl:when test="PassengerFare/@NegotiatedFare='1'">true</xsl:when>
            <xsl:otherwise>false</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <BaseFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="PassengerFare/BaseFare/@Amount!=''">
                <xsl:value-of select="PassengerFare/BaseFare/@Amount"/>
              </xsl:when>
              <xsl:otherwise>0.00</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PassengerFare/BaseFare/@CurrencyCode"/>
          </xsl:attribute>
        </BaseFare>
        <EquivFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="PassengerFare/EquivFare /@Amount!=''">
                <xsl:value-of select="PassengerFare/EquivFare/@Amount"/>
              </xsl:when>
              <xsl:otherwise>0.00</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PassengerFare/EquivFare/@CurrencyCode"/>
          </xsl:attribute>
        </EquivFare>
        <Taxes>
          <xsl:apply-templates select="PassengerFare/Taxes/Tax"/>
        </Taxes>
        <Fees>
          <xsl:apply-templates select="PassengerFare/Fees/Fee "/>
        </Fees>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="PassengerFare/TotalFare/@Amount!=''">
                <xsl:value-of select="PassengerFare/TotalFare/@Amount"/>
              </xsl:when>
              <xsl:otherwise>0.00</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PassengerFare/TotalFare/@CurrencyCode"/>
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
    </PTC_FareBreakdown>
  </xsl:template>
  <xsl:template match="FareBasisCode">
    <FareBasisCode>
      <xsl:value-of select="."/>
    </FareBasisCode>
  </xsl:template>
  <xsl:template match="Tax">
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="@TaxCode"/>
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="@Amount"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="@CurrencyCode"/>
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <xsl:template match="Fee ">
    <Fee>
      <xsl:attribute name="Amount">
        <xsl:value-of select="@Amount"/>
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="@CurrencyCode"/>
      </xsl:attribute>
    </Fee>
  </xsl:template>
  <xsl:template match="Membership">
    <Membership>
      <xsl:attribute name="ProgramCode">
        <xsl:value-of select="@ProgramCode"/>
      </xsl:attribute>
      <xsl:attribute name="BonusCode">
        <xsl:value-of select="@BonusCode"/>
      </xsl:attribute>
    </Membership>
  </xsl:template>

  <xsl:template match="CustLoyalty" mode="ixplore">
    <xsl:param name="RefNumber"/>
    <CustLoyalty>

      <xsl:attribute name="RPH">
        <xsl:value-of select="$RefNumber"/>
      </xsl:attribute>
      <xsl:attribute name="ProgramID">
        <xsl:value-of select="@ProgramID"/>
      </xsl:attribute>
      <xsl:attribute name="MembershipID">
        <xsl:value-of select="@MembershipID"/>
      </xsl:attribute>
      <xsl:attribute name="TravelerRefNumber">
        <xsl:value-of select="$RefNumber"/>
        <xsl:text>.1</xsl:text>
      </xsl:attribute>
    </CustLoyalty>
  </xsl:template>

  <xsl:template match="CustLoyalty" mode="other">
    <xsl:param name="RefNumber"/>
    <CustLoyalty>

      <xsl:attribute name="RPH">
        <xsl:value-of select="$RefNumber"/>
      </xsl:attribute>
      <xsl:attribute name="ProgramID">
        <xsl:value-of select="@ProgramID"/>
      </xsl:attribute>
      <xsl:attribute name="MembershipID">
        <xsl:value-of select="@MembershipID"/>
      </xsl:attribute>
      <xsl:attribute name="NameNumber">
        <xsl:value-of select="$RefNumber"/>
        <xsl:text>.1</xsl:text>
      </xsl:attribute>
    </CustLoyalty>
  </xsl:template>

  <xsl:template match="Telephone" mode="other">
    <Telephone>
      <xsl:attribute name="PhoneLocationType">
        <xsl:choose>
          <xsl:when test="@PhoneLocationType='Home'">H</xsl:when>
          <xsl:when test="@PhoneLocationType='Work'">W</xsl:when>
          <xsl:when test="@PhoneLocationType='Mobile'">M</xsl:when>
          <xsl:when test="@PhoneLocationType='Fax'">F</xsl:when>
          <xsl:when test="@PhoneLocationType='Business'">B</xsl:when>
        </xsl:choose>
      </xsl:attribute>
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

  <xsl:template match="Telephone" mode="ixplore">
    <Telephone>
      <xsl:attribute name="PhoneUseType">
        <xsl:choose>
          <xsl:when test="@PhoneLocationType='Home'">H</xsl:when>
          <xsl:when test="@PhoneLocationType='Work'">W</xsl:when>
          <xsl:when test="@PhoneLocationType='Mobile'">M</xsl:when>
          <xsl:when test="@PhoneLocationType='Fax'">F</xsl:when>
          <xsl:when test="@PhoneLocationType='Business'">B</xsl:when>
        </xsl:choose>
      </xsl:attribute>
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

  <xsl:template match="Address" mode="billing">
    <ClientAddressRemark>
      <xsl:attribute name="Text">
        <xsl:value-of select="StreetNmbr"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CityName"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="PostalCode"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="StateProv/@StateCode"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CountryName/@Code"/>
      </xsl:attribute>
    </ClientAddressRemark>
  </xsl:template>

  <xsl:template match="DeliveryAddress">
    <DeliveryAddressRemark>
      <xsl:attribute name="Text">
        <xsl:value-of select="StreetNmbr"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CityName"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="PostalCode"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="StateProv/@StateCode"/>
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CountryName/@Code"/>
      </xsl:attribute>
    </DeliveryAddressRemark>
  </xsl:template>

  <xsl:template match="SeatRequest">
    <SeatPref>
      <xsl:attribute name="SeatPreference">
        <xsl:value-of select="@SeatPreference"/>
      </xsl:attribute>
    </SeatPref>
  </xsl:template>

  <xsl:template match="PriceData">
    <OTA_AirPriceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.13.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC"/>
          </xsl:attribute>
        </Source>
      </POS>
      <TravelerInfoSummary>
        <xsl:if test="../../POS/Source/@ISOCurrency!=''">
          <PriceRequestInformation>
            <xsl:attribute name="CurrencyCode">
              <xsl:value-of select="../../POS/Source/@ISOCurrency"/>
            </xsl:attribute>
          </PriceRequestInformation>
        </xsl:if>
        <TPA_Extensions>
          <BargainFinder>
            <xsl:attribute name="Ind">
              <xsl:choose>
                <xsl:when test="PriceRequestInformation/@FareQualifier = '12'">true</xsl:when>
                <xsl:otherwise>false</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
          </BargainFinder>
          <xsl:choose>
            <xsl:when test="PassengerTypeQuantity/@Code!=''">
              <xsl:for-each select="PassengerTypeQuantity">
                <PassengerType>
                  <xsl:attribute name="Quantity">
                    <xsl:value-of select="@Quantity"/>
                  </xsl:attribute>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="@Code"/>
                  </xsl:attribute>
                </PassengerType>
              </xsl:for-each>
            </xsl:when>
            <xsl:otherwise>
              <xsl:for-each select="../PNRData/Traveler">
                <PassengerType>
                  <xsl:attribute name="Quantity">1</xsl:attribute>
                  <xsl:attribute name="Code">
                    <xsl:choose>
                      <xsl:when test="@PassengerTypeCode='CHD'">C09</xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="@PassengerTypeCode"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:attribute name="AlternatePassengerType">true</xsl:attribute>
                </PassengerType>
              </xsl:for-each>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:choose>
            <xsl:when test="@PriceType ='Private'">
              <PrivateFare Ind="true"/>
            </xsl:when>
            <xsl:otherwise>
              <PublicFare Ind="true"/>
            </xsl:otherwise>
          </xsl:choose>
          <PriceRetention Default="true"/>
        </TPA_Extensions>
      </TravelerInfoSummary>
    </OTA_AirPriceRQ>
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
  <!-- **************************************OTH Segemnt*******************************************-->
  <xsl:template match="Segment" mode="Seg">
    <MiscSegmentSellRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC"/>
          </xsl:attribute>
        </Source>
      </POS>
      <Segment>
        <xsl:attribute name="Type">
          <xsl:choose>
            <xsl:when test="@Type='OTH'">OTH</xsl:when>
            <xsl:otherwise>OTH</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="NumberInParty">
          <xsl:value-of select="@NumberInParty"/>
        </xsl:attribute>
        <xsl:attribute name="Vendor">
          <xsl:value-of select="@Vendor"/>
        </xsl:attribute>
        <xsl:attribute name="Date">
          <xsl:value-of select="@Date"/>
        </xsl:attribute>
        <MiscLocation>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="@LocationCode"/>
          </xsl:attribute>
          <xsl:attribute name="CodeContext">IATA</xsl:attribute>
        </MiscLocation>
        <Text>
          <xsl:value-of select="Text"/>
        </Text>
      </Segment>
    </MiscSegmentSellRQ>
  </xsl:template>
</xsl:stylesheet>