<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!--******************************************************************
 ================================================================== 
  Sabre_UpdateInsertRQ.xsl 														       
  ================================================================== 
  Date: 12 Dec 2019 - Kobelev - Tour Code has to start with UN prefex instead of just U as it was before. BUG 1160
  Date: 09 Feb 2018 - Samokhvalov - FPPQ command format changes - Endorsment replaces vs add
  Date: 08 Jyl 2016 - Kobelev - Pre-Approval code added to Future Pricing Command. BUG 327
  Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0				
  Date: 12 Nov 2015 - Rastko - added mapping to AccountableDocument element    
  Date: 19 Feb 2104 - Rastko - added tour code and endorsment to future pricing command    
  Date: 18 Feb 2104 - Rastko - corrected accounting refund supplier code to be name	       
  Date: 30 Jan 2104 - Rastko - send future pricing in cryptic command			       
  Date: 29 Jan 2104 - Rastko - added code to support all special remark types		       
  Date: 24 Jan 2104 - Rastko - upgraded AddRemark version and added DesignatePrinter	       
  Date: 17 Jan 2104 - Rastko - added support for accounting lines and ticket instructions	       
  Date: 05 Dec 2013 - Rastko - added support for UpdateSessioned message		       
  Date: 24 Dec 2010 - Rastko - new file												       
  ================================================================== 
  *********************************************************************-->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="PCC">
    <xsl:value-of select="UpdateInsert/OTA_UpdateRQ/POS/Source/@PseudoCityCode"/>
    <xsl:value-of select="UpdateInsert/OTA_UpdateSessionedRQ/POS/Source/@PseudoCityCode"/>
  </xsl:variable>
  <xsl:variable name="paxtype">
    <xsl:choose>
      <xsl:when test="$PCC = '9N60' or $PCC = 'Z5PF'  or $PCC = 'EO8F'">Y</xsl:when>
      <xsl:otherwise>N</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <xsl:template match="/">
    <xsl:apply-templates select="UpdateInsert/OTA_UpdateRQ | UpdateInsert/OTA_UpdateSessionedRQ"/>
  </xsl:template>

  <xsl:template match="OTA_UpdateRQ | OTA_UpdateSessionedRQ">
    <UpdateInsert>
      <!--
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
      -->
      <xsl:if test="Position/Element[@Operation='insert' and @Child='OriginDestinationOptions']">
        <xsl:apply-templates select="Position/Element[@Operation='insert' and @Child='OriginDestinationOptions']"/>
      </xsl:if>
      <xsl:if test="Position/Element[@Operation='insert' and @Child='FormOfPayment']">
        <xsl:apply-templates select="Position/Element[@Operation='insert' and @Child='FormOfPayment']"/>
      </xsl:if>
      <!--
      <xsl:if test="OTA_HotelResRQ">
				<HotelBook>
					<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation" />
				</HotelBook>
			</xsl:if>
			<xsl:if test="OTA_VehResRQ">
				<CarBook>
					<xsl:apply-templates select="OTA_VehResRQ/VehResRQCore" />
				</CarBook>
			</xsl:if>
			<xsl:if test="TPA_Extensions/PriceData">
				<Pricing>
					<xsl:apply-templates select="TPA_Extensions/PriceData"/>
				</Pricing>
			</xsl:if>
      -->
      <xsl:if test="Position/Element[@Operation='insert']/Remarks or Position/Element[@Operation='insert']/SpecialRemarks or Position/Element[@Operation='insert']/Fulfillment/PaymentDetails/PaymentDetail or Position/Element[@Operation='insert']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or Position/Element[@Operation='insert']/Fulfillment/DeliveryAddress">
        <Remarks>
          <AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.1.0">
            <RemarkInfo>
              <xsl:if test="Position/Element[@Operation='insert']/Fulfillment/PaymentDetails/PaymentDetail">
                <xsl:choose>
                  <xsl:when test="DirectBill/@DirectBill_ID='Check'">
                    <FOPRemark  Type="CHECK"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <!--
                    <FOPRemark>
											<CCInfo>
												<CreditCardVendor>
													<xsl:attribute name="Code">
														<xsl:choose>
															<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MC'">
																<xsl:value-of select="'CA'"/>
															</xsl:when>
															<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='DS'">
																<xsl:value-of select="'DI'"/>
															</xsl:when>
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
                    -->
                    <FOP_Remark>
                      <CC_Info>
                        <PaymentCard>
                          <xsl:attribute name="Code">
                            <xsl:choose>
                              <xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MC'">
                                <xsl:value-of select="'CA'"/>
                              </xsl:when>
                              <xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='DS'">
                                <xsl:value-of select="'DI'"/>
                              </xsl:when>
                              <xsl:otherwise>
                                <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/>
                              </xsl:otherwise>
                            </xsl:choose>
                          </xsl:attribute>
                          <xsl:attribute name="ExpireDate">
                            <xsl:value-of select="concat('20',substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,1,2))"/>
                            <xsl:value-of select="concat('-',substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,3))"/>
                          </xsl:attribute>
                          <xsl:attribute name="Number">
                            <xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
                          </xsl:attribute>
                        </PaymentCard>
                      </CC_Info>
                    </FOP_Remark>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:if>
              <xsl:for-each select="Position/Element[@Operation='insert']/Remarks/Remark">
                <Remark Type="General">
                  <Text>
                    <xsl:value-of select="."/>
                  </Text>
                </Remark>
              </xsl:for-each>
              <xsl:for-each select="Position/Element[@Operation='insert']/SpecialRemarks/SpecialRemark[@RemarkType!='']">
                <Remark Type="{@RemarkType}">
                  <Text>
                    <xsl:value-of select="Text"/>
                  </Text>
                </Remark>
              </xsl:for-each>
              <xsl:apply-templates select="Position/Element[@Operation='insert']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address" mode="billing"/>
              <xsl:apply-templates select="Position/Element[@Operation='insert']/Fulfillment/DeliveryAddress"/>
            </RemarkInfo>
          </AddRemarkRQ>
        </Remarks>
      </xsl:if>
      <xsl:if test="Position/Element[@Operation='insert']/SpecialServiceRequests/SpecialServiceRequest or Position/Element[@Operation='insert']/OtherServiceInformations/OtherServiceInformation or Position/Element[@Operation='insert']/SeatRequests/SeatRequest">
        <SpecialServices>
          <SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.4.1">
            <POS>
              <Source>
                <xsl:attribute name="PseudoCityCode">
                  <xsl:value-of select="$PCC" />
                </xsl:attribute>
              </Source>
            </POS>
            <xsl:if test="Position/Element[@Operation='insert']/SeatRequests/SeatRequest">
              <xsl:for-each select="Position/Element[@Operation='insert']/SeatRequests/SeatRequest">
                <xsl:call-template name="RPHPSeat">
                  <xsl:with-param name="RPHP">
                    <xsl:value-of select="@TravelerRefNumberRPHList"/>
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:for-each>
            </xsl:if>
            <xsl:if test="Position/Element[@Operation='insert']/SpecialServiceRequests/SpecialServiceRequest">
              <xsl:for-each select="Position/Element[@Operation='insert']/SpecialServiceRequests/SpecialServiceRequest">
                <xsl:call-template name="RPHP">
                  <xsl:with-param name="RPHP">
                    <xsl:value-of select="@TravelerRefNumberRPHList"/>
                  </xsl:with-param>
                </xsl:call-template>
              </xsl:for-each>
            </xsl:if>
            <xsl:if test="Position/Element[@Operation='insert']/OtherServiceInformations/OtherServiceInformation">
              <xsl:for-each select="Position/Element[@Operation='insert']/OtherServiceInformations/OtherServiceInformation">
                <Service>
                  <xsl:attribute name="SSRCode">OSI</xsl:attribute>
                  <Airline>
                    <xsl:attribute name="Code">
                      <xsl:value-of select="Airline/@Code"/>
                    </xsl:attribute>
                  </Airline>
                  <!--
                  <TPA_Extensions>
										<Name>
											<xsl:attribute name="Number">
												<xsl:value-of select="@TravelerRefNumberRPHList"/>
												<xsl:text>.1</xsl:text>
											</xsl:attribute>
										</Name>
									</TPA_Extensions>
                  -->
                  <Text>
                    <xsl:value-of select="Text"/>
                  </Text>
                </Service>
              </xsl:for-each>
            </xsl:if>
          </SpecialServiceRQ>
        </SpecialServices>
      </xsl:if>
      <xsl:if test="Position/Element[@Operation='insert']/PNRData">
        <xsl:if test="Position/Element[@Operation='insert']/PNRData/FuturePriceInfo">
          <FuturePriceInfo>
            <xsl:for-each select="Position/Element[@Operation='insert']/PNRData/FuturePriceInfo">
              <SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
                <xsl:element name="Request">
                  <xsl:attribute name="Output">SCREEN</xsl:attribute>
                  <xsl:attribute name="MDRSubset">AD01</xsl:attribute>
                  <xsl:attribute name="CDATA">true</xsl:attribute>
                  <xsl:element name="HostCommand">
                    <xsl:value-of select="'FPPQ'"/>
                    <xsl:value-of select="@StoredFareRef"/>
                    <xsl:variable name="apos">'</xsl:variable>
                    <xsl:value-of select="concat($apos,'A',@Airline)"/>
                    <xsl:if test="Markup/@Amount!=''">
                      <xsl:variable name="comm">
                        <xsl:value-of select="string(Markup/@Amount)"/>
                      </xsl:variable>
                      <xsl:variable name="comm1">
                        <xsl:choose>
                          <xsl:when test="$comm='0'">
                            <xsl:value-of select="'0.00'"/>
                          </xsl:when>
                          <xsl:when test="Markup/@DecimalPlaces='2'">
                            <xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
                            <xsl:value-of select="'.'"/>
                            <xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="concat($comm,'.00')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:value-of select="concat($apos,'PU*',$comm1)"/>
                    </xsl:if>
                    <xsl:variable name="comm">
                      <xsl:choose>
                        <xsl:when test="Commission/@Amount!=''">
                          <xsl:value-of select="string(Commission/@Amount)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="string(Commission/@Percent)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="comm1">
                      <xsl:choose>
                        <xsl:when test="$comm='0'">
                          <xsl:value-of select="'0.00'"/>
                        </xsl:when>
                        <xsl:when test="Commission/@DecimalPlaces='2'">
                          <xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
                          <xsl:value-of select="'.'"/>
                          <xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat($comm,'.00')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:choose>
                      <xsl:when test="Commission/@Amount!=''">
                        <xsl:value-of select="concat($apos,'K',$comm1)"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="concat($apos,'KP',$comm1)"/>
                      </xsl:otherwise>
                    </xsl:choose>
                    <xsl:if test="TourCode!=''">
                      <xsl:value-of select="concat($apos,'UN*',TourCode)"/>
                    </xsl:if>
                    <xsl:if test="Endorsment!=''">
                      <xsl:value-of select="concat($apos,'EO/',Endorsment)"/>
                    </xsl:if>
                    <xsl:if test="ApprovalCode!=''">
                      <xsl:value-of select="concat($apos,'F*Z', ApprovalCode)"/>
                    </xsl:if>
                  </xsl:element>
                </xsl:element>
              </SabreCommandLLSRQ>
              <!--OTA_AirPriceRQ Version="2.4.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
								<PriceRequestInformation FutureTicket="true" Retain="true">
									<OptionalQualifiers>
										<MiscQualifiers>
											<Commission>
												<xsl:variable name="comm">
													<xsl:choose>
														<xsl:when test="Commission/@Amount!=''">
															<xsl:value-of select="string(Commission/@Amount)"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="string(Commission/@Percent)"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>
												<xsl:variable name="comm1">
													<xsl:choose>
														<xsl:when test="$comm='0'"><xsl:value-of select="'0.00'"/></xsl:when>
														<xsl:when test="Commission/@DecimalPlaces='2'">
															<xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
															<xsl:value-of select="'.'"/>
															<xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="concat($comm,'.00')"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:variable>
												<xsl:choose>
													<xsl:when test="Commission/@Amount!=''">
														<xsl:attribute name="Amount"><xsl:value-of select="$comm1"/></xsl:attribute>
													</xsl:when>
													<xsl:otherwise>
														<xsl:attribute name="Percent"><xsl:value-of select="$comm1"/></xsl:attribute>
													</xsl:otherwise>
												</xsl:choose>
											</Commission>
										</MiscQualifiers>
										<xsl:if test="Markup/@Amount">
											<PricingQualifiers>
												<PlusUp>
													<xsl:attribute name="Amount">
														<xsl:variable name="comm">
															<xsl:value-of select="string(Markup/@Amount)"/>
														</xsl:variable>
														<xsl:variable name="comm1">
															<xsl:choose>
																<xsl:when test="$comm='0'"><xsl:value-of select="'0.00'"/></xsl:when>
																<xsl:when test="Markup/@DecimalPlaces='2'">
																	<xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
																	<xsl:value-of select="'.'"/>
																	<xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:value-of select="concat($comm,'.00')"/>
																</xsl:otherwise>
															</xsl:choose>
														</xsl:variable>
														<xsl:value-of select="$comm1"/>
													</xsl:attribute>
												</PlusUp>
											</PricingQualifiers>
										</xsl:if>
									</OptionalQualifiers>
								</PriceRequestInformation>
							</OTA_AirPriceRQ-->
            </xsl:for-each>
          </FuturePriceInfo>
        </xsl:if>
        <xsl:if test="Position/Element[@Operation='insert']/PNRData/RefundDocument">
          <RefundDocument>
            <xsl:for-each select="Position/Element[@Operation='insert']/PNRData/RefundDocument">
              <AddAccountingLineRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="1.0.1">
                <POS>
                  <Source>
                    <xsl:attribute name="PseudoCityCode">
                      <xsl:value-of select="$PCC" />
                    </xsl:attribute>
                  </Source>
                </POS>
                <RefundDocument>
                  <Segment>
                    <xsl:attribute name="Number">
                      <xsl:value-of select="Segment/@Number"/>
                    </xsl:attribute>
                    <xsl:attribute name="Type">
                      <xsl:value-of select="Segment/@Type"/>
                    </xsl:attribute>
                  </Segment>
                  <SupplierInfo>
                    <xsl:attribute name="Name">
                      <xsl:value-of select="SupplierInfo/@Code"/>
                    </xsl:attribute>
                    <Document>
                      <xsl:attribute name="Count">
                        <xsl:value-of select="SupplierInfo/Document/@Count"/>
                      </xsl:attribute>
                      <xsl:attribute name="Number">
                        <xsl:value-of select="SupplierInfo/Document/@Number"/>
                      </xsl:attribute>
                    </Document>
                    <Coupon>
                      <xsl:attribute name="Number">
                        <xsl:value-of select="SupplierInfo/Coupon/@Number"/>
                      </xsl:attribute>
                    </Coupon>
                    <Invoice>
                      <xsl:attribute name="Number">
                        <xsl:value-of select="SupplierInfo/Invoice/@Number"/>
                      </xsl:attribute>
                      <xsl:attribute name="Type">
                        <xsl:value-of select="SupplierInfo/Invoice/@Type"/>
                      </xsl:attribute>
                    </Invoice>
                  </SupplierInfo>
                  <FOPInfo>
                    <xsl:variable name="ccType" select="FOPInfo/@Type"></xsl:variable>
                    <xsl:attribute name="Type">
                      <xsl:value-of select="$ccType"/>
                    </xsl:attribute>
                    <xsl:if test="$ccType='CC'">
                      <CCInfo>
                        <CreditCardVendor>
                          <xsl:attribute name="Code">
                            <xsl:value-of select="FOPInfo/PaymentCard/@Code"/>
                          </xsl:attribute>
                        </CreditCardVendor>
                        <CreditCardNumber>
                          <xsl:attribute name="Code">
                            <xsl:value-of select="FOPInfo/PaymentCard/@Number"/>
                          </xsl:attribute>
                        </CreditCardNumber>
                      </CCInfo>
                    </xsl:if>
                  </FOPInfo>
                  <Type>
                    <xsl:attribute name="Info">
                      <xsl:value-of select="Type/@Info"/>
                    </xsl:attribute>
                  </Type>
                  <ItinTotalFare>
                    <Commission>
                      <xsl:variable name="comm">
                        <xsl:choose>
                          <xsl:when test="ItinTotalFare/Commission/@Amount!=''">
                            <xsl:value-of select="string(ItinTotalFare/Commission/@Amount)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="string(ItinTotalFare/Commission/@Percent)"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:variable name="comm1">
                        <xsl:choose>
                          <xsl:when test="$comm='0'">
                            <xsl:value-of select="'0.00'"/>
                          </xsl:when>
                          <xsl:when test="ItinTotalFare/Commission/@DecimalPlaces='2'">
                            <xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
                            <xsl:value-of select="'.'"/>
                            <xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="concat($comm,'.00')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:choose>
                        <xsl:when test="ItinTotalFare/Commission/@Amount!=''">
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$comm1"/>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="Percent">
                            <xsl:value-of select="$comm1"/>
                          </xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                    </Commission>
                    <BaseFare>
                      <xsl:variable name="base">
                        <xsl:choose>
                          <xsl:when test="ItinTotalFare/BaseFare/@Amount!=''">
                            <xsl:value-of select="string(ItinTotalFare/BaseFare/@Amount)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="string(ItinTotalFare/BaseFare/@Percent)"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:variable name="base1">
                        <xsl:choose>
                          <xsl:when test="$base='0'">
                            <xsl:value-of select="'0.00'"/>
                          </xsl:when>
                          <xsl:when test="ItinTotalFare/BaseFare/@DecimalPlaces='2'">
                            <xsl:value-of select="substring($base,1,string-length($base) - 2)"/>
                            <xsl:value-of select="'.'"/>
                            <xsl:value-of select="substring($base,string-length($base) - 1)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="concat($base,'.00')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:choose>
                        <xsl:when test="ItinTotalFare/BaseFare/@Amount!=''">
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$base1"/>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="Percent">
                            <xsl:value-of select="$base1"/>
                          </xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                    </BaseFare>
                    <Taxes>
                      <Tax>
                        <xsl:variable name="tax">
                          <xsl:choose>
                            <xsl:when test="ItinTotalFare/Taxes/Tax/@Amount!=''">
                              <xsl:value-of select="string(ItinTotalFare/Taxes/Tax/@Amount)"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="string(ItinTotalFare/Taxes/Tax/@Percent)"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:variable>
                        <xsl:variable name="tax1">
                          <xsl:choose>
                            <xsl:when test="$tax='0'">
                              <xsl:value-of select="'0.00'"/>
                            </xsl:when>
                            <xsl:when test="ItinTotalFare/Taxes/Tax/@DecimalPlaces='2'">
                              <xsl:value-of select="substring($tax,1,string-length($tax) - 2)"/>
                              <xsl:value-of select="'.'"/>
                              <xsl:value-of select="substring($tax,string-length($tax) - 1)"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="concat($tax,'.00')"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:variable>
                        <xsl:choose>
                          <xsl:when test="ItinTotalFare/Taxes/Tax/@Amount!=''">
                            <xsl:attribute name="Amount">
                              <xsl:value-of select="$tax1"/>
                            </xsl:attribute>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:attribute name="Percent">
                              <xsl:value-of select="$tax1"/>
                            </xsl:attribute>
                          </xsl:otherwise>
                        </xsl:choose>
                      </Tax>
                    </Taxes>
                  </ItinTotalFare>
                </RefundDocument>
              </AddAccountingLineRQ>
            </xsl:for-each>
          </RefundDocument>
        </xsl:if>
        <xsl:if test="Position/Element[@Operation='insert']/PNRData/AccountableDocument">
          <AccountableDocument>
            <xsl:for-each select="Position/Element[@Operation='insert']/PNRData/AccountableDocument">
              <AddAccountingLineRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="1.0.1">
                <POS>
                  <Source>
                    <xsl:attribute name="PseudoCityCode">
                      <xsl:value-of select="$PCC" />
                    </xsl:attribute>
                  </Source>
                </POS>
                <AccountableDocument>
                  <Segment>
                    <xsl:attribute name="Number">
                      <xsl:value-of select="Segment/@Number"/>
                    </xsl:attribute>
                    <xsl:attribute name="Type">
                      <xsl:value-of select="Segment/@Type"/>
                    </xsl:attribute>
                  </Segment>
                  <SupplierInfo>
                    <xsl:attribute name="Name">
                      <xsl:value-of select="SupplierInfo/@Code"/>
                    </xsl:attribute>
                    <Document>
                      <xsl:attribute name="Count">
                        <xsl:value-of select="SupplierInfo/Document/@Count"/>
                      </xsl:attribute>
                      <xsl:attribute name="Number">
                        <xsl:value-of select="SupplierInfo/Document/@Number"/>
                      </xsl:attribute>
                    </Document>
                  </SupplierInfo>
                  <FOPInfo>
                    <xsl:variable name="ccType" select="FOPInfo/@Type"></xsl:variable>
                    <xsl:attribute name="Type">
                      <xsl:value-of select="$ccType"/>
                    </xsl:attribute>
                    <xsl:if test="$ccType='CC'">
                      <xsl:variable name="ccCode">
                        <xsl:choose>
                          <xsl:when test="not(FOPInfo/PaymentCard/@Number='')">
                            <xsl:value-of select="FOPInfo/PaymentCard/@Code"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="/UpdateInsert/TravelItineraryReadRS/TravelItinerary/CustomerInfo/PaymentInfo/Payment/Form/Text"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <CCInfo>
                        <CreditCardVendor>
                          <xsl:attribute name="Code">
                            <xsl:value-of select="FOPInfo/PaymentCard/@Code"/>
                          </xsl:attribute>
                        </CreditCardVendor>
                        <CreditCardNumber>
                          <xsl:attribute name="Code">
                            <xsl:value-of select="FOPInfo/PaymentCard/@Number"/>
                          </xsl:attribute>
                        </CreditCardNumber>
                      </CCInfo>
                    </xsl:if>
                  </FOPInfo>
                  <Type>
                    <xsl:attribute name="Info">
                      <xsl:value-of select="Type/@Info"/>
                    </xsl:attribute>
                  </Type>
                  <ItinTotalFare>
                    <Commission>
                      <xsl:variable name="comm">
                        <xsl:choose>
                          <xsl:when test="ItinTotalFare/Commission/@Amount!=''">
                            <xsl:value-of select="string(ItinTotalFare/Commission/@Amount)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="string(ItinTotalFare/Commission/@Percent)"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:variable name="comm1">
                        <xsl:choose>
                          <xsl:when test="$comm='0'">
                            <xsl:value-of select="'0.00'"/>
                          </xsl:when>
                          <xsl:when test="ItinTotalFare/Commission/@DecimalPlaces='2'">
                            <xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
                            <xsl:value-of select="'.'"/>
                            <xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="concat($comm,'.00')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:choose>
                        <xsl:when test="ItinTotalFare/Commission/@Amount!=''">
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$comm1"/>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="Percent">
                            <xsl:value-of select="$comm1"/>
                          </xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                    </Commission>
                    <BaseFare>
                      <xsl:variable name="base">
                        <xsl:choose>
                          <xsl:when test="ItinTotalFare/BaseFare/@Amount!=''">
                            <xsl:value-of select="string(ItinTotalFare/BaseFare/@Amount)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="string(ItinTotalFare/BaseFare/@Percent)"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:variable name="base1">
                        <xsl:choose>
                          <xsl:when test="$base='0'">
                            <xsl:value-of select="'0.00'"/>
                          </xsl:when>
                          <xsl:when test="ItinTotalFare/BaseFare/@DecimalPlaces='2'">
                            <xsl:value-of select="substring($base,1,string-length($base) - 2)"/>
                            <xsl:value-of select="'.'"/>
                            <xsl:value-of select="substring($base,string-length($base) - 1)"/>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:value-of select="concat($base,'.00')"/>
                          </xsl:otherwise>
                        </xsl:choose>
                      </xsl:variable>
                      <xsl:choose>
                        <xsl:when test="ItinTotalFare/BaseFare/@Amount!=''">
                          <xsl:attribute name="Amount">
                            <xsl:value-of select="$base1"/>
                          </xsl:attribute>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:attribute name="Percent">
                            <xsl:value-of select="$base1"/>
                          </xsl:attribute>
                        </xsl:otherwise>
                      </xsl:choose>
                    </BaseFare>
                    <Taxes>
                      <Tax>
                        <xsl:variable name="tax">
                          <xsl:choose>
                            <xsl:when test="ItinTotalFare/Taxes/Tax/@Amount!=''">
                              <xsl:value-of select="string(ItinTotalFare/Taxes/Tax/@Amount)"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="string(ItinTotalFare/Taxes/Tax/@Percent)"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:variable>
                        <xsl:variable name="tax1">
                          <xsl:choose>
                            <xsl:when test="$tax='0'">
                              <xsl:value-of select="'0.00'"/>
                            </xsl:when>
                            <xsl:when test="ItinTotalFare/Taxes/Tax/@DecimalPlaces='2'">
                              <xsl:value-of select="substring($tax,1,string-length($tax) - 2)"/>
                              <xsl:value-of select="'.'"/>
                              <xsl:value-of select="substring($tax,string-length($tax) - 1)"/>
                            </xsl:when>
                            <xsl:otherwise>
                              <xsl:value-of select="concat($tax,'.00')"/>
                            </xsl:otherwise>
                          </xsl:choose>
                        </xsl:variable>
                        <xsl:choose>
                          <xsl:when test="ItinTotalFare/Taxes/Tax/@Amount!=''">
                            <xsl:attribute name="Amount">
                              <xsl:value-of select="$tax1"/>
                            </xsl:attribute>
                          </xsl:when>
                          <xsl:otherwise>
                            <xsl:attribute name="Percent">
                              <xsl:value-of select="$tax1"/>
                            </xsl:attribute>
                          </xsl:otherwise>
                        </xsl:choose>
                      </Tax>
                    </Taxes>
                  </ItinTotalFare>
                </AccountableDocument>
              </AddAccountingLineRQ>
            </xsl:for-each>
          </AccountableDocument>
        </xsl:if>
        <xsl:if test="Position/Element[@Operation='insert']/PNRData/BulkDocument">
          <BulkDocument>
            <xsl:for-each select="Position/Element[@Operation='insert']/PNRData/BulkDocument">
              <SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
                <xsl:element name="Request">
                  <xsl:attribute name="Output">SCREEN</xsl:attribute>
                  <xsl:attribute name="MDRSubset">AD01</xsl:attribute>
                  <xsl:attribute name="CDATA">true</xsl:attribute>
                  <xsl:element name="HostCommand">
                    <xsl:value-of select="concat('ACB/',SupplierInfo/@Code,'/',SupplierInfo/Document/@Number,'/')" />
                    <xsl:variable name="base">
                      <xsl:choose>
                        <xsl:when test="ItinTotalFare/BaseFare/@Amount!=''">
                          <xsl:value-of select="string(ItinTotalFare/BaseFare/@Amount)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="string(ItinTotalFare/BaseFare/@Percent)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="base1">
                      <xsl:choose>
                        <xsl:when test="$base='0'">
                          <xsl:value-of select="'0.00'"/>
                        </xsl:when>
                        <xsl:when test="ItinTotalFare/BaseFare/@DecimalPlaces='2'">
                          <xsl:value-of select="substring($base,1,string-length($base) - 2)"/>
                          <xsl:value-of select="'.'"/>
                          <xsl:value-of select="substring($base,string-length($base) - 1)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat($base,'.00')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:if test="ItinTotalFare/BaseFare/@Percent">
                      <xsl:value-of select="'P'"/>
                    </xsl:if>
                    <xsl:value-of select="$base1"/>
                    <xsl:value-of select="'/'"/>
                    <xsl:variable name="tax">
                      <xsl:choose>
                        <xsl:when test="ItinTotalFare/Taxes/Tax/@Amount!=''">
                          <xsl:value-of select="string(ItinTotalFare/Taxes/Tax/@Amount)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="string(ItinTotalFare/Taxes/Tax/@Percent)"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:variable name="tax1">
                      <xsl:choose>
                        <xsl:when test="$tax='0'">
                          <xsl:value-of select="'0.00'"/>
                        </xsl:when>
                        <xsl:when test="ItinTotalFare/Taxes/Tax/@DecimalPlaces='2'">
                          <xsl:value-of select="substring($tax,1,string-length($tax) - 2)"/>
                          <xsl:value-of select="'.'"/>
                          <xsl:value-of select="substring($tax,string-length($tax) - 1)"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="concat($tax,'.00')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:if test="ItinTotalFare/Taxes/Tax/@Percent">
                      <xsl:value-of select="'P'"/>
                    </xsl:if>
                    <xsl:value-of select="$tax1"/>
                    <xsl:value-of select="'/'"/>
                    <xsl:choose>
                      <xsl:when test="Price/@PerPassenger='true'">
                        <xsl:value-of select="'PER'"/>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:value-of select="'ONE'"/>
                      </xsl:otherwise>
                    </xsl:choose>
                  </xsl:element>
                </xsl:element>
              </SabreCommandLLSRQ>
            </xsl:for-each>
          </BulkDocument>
        </xsl:if>
      </xsl:if>
      <xsl:if test="Position/Element[@Operation='insert']/Queue/@QueueNumber!=''">
        <Queue>
          <QPlaceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
            <POS>
              <Source>
                <xsl:attribute name="PseudoCityCode">
                  <xsl:value-of select="$PCC" />
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
                <xsl:value-of select="$PCC" />
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
        <TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
          <MessagingDetails>
            <SubjectAreas>
              <SubjectArea>FULL</SubjectArea>
            </SubjectAreas>
          </MessagingDetails>
        </TravelItineraryReadRQ>
      </Read>
      <Ignore>
        <IgnoreTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
          <POS>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC" />
            </xsl:attribute>
          </POS>
          <IgnoreTransaction Ind="true"/>
        </IgnoreTransactionRQ>
      </Ignore>
    </UpdateInsert>
  </xsl:template>
  <!-- -->
  <xsl:template name="RPHP">
    <xsl:param name="RPHP"/>
    <xsl:if test="string-length($RPHP) != 0">
      <xsl:variable name="paxref">
        <xsl:value-of select="substring($RPHP,1,1)"/>
      </xsl:variable>
      <xsl:call-template name="RPHS">
        <xsl:with-param name="RPHS">
          <xsl:value-of select="@FlightRefNumberRPHList"/>
        </xsl:with-param>
        <xsl:with-param name="paxref">
          <xsl:value-of select="$paxref"/>
        </xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="RPHP">
        <xsl:with-param name="RPHP">
          <xsl:value-of select="substring($RPHP,2)"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <!-- -->
  <xsl:template name="RPHS">
    <xsl:param name="RPHS"/>
    <xsl:param name="paxref"/>
    <xsl:if test="string-length($RPHS) != 0">
      <xsl:variable name="tRPH">
        <xsl:value-of select="substring($RPHS,1,1)"/>
      </xsl:variable>
      <Service>
        <xsl:attribute name="SSRCode">
          <xsl:choose>
            <xsl:when test="@SSRCode='DOCS' and starts-with(Text,'-R-')">DOCO</xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="@SSRCode"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <Airline>
          <xsl:attribute name="HostedCarrier">
            <xsl:choose>
              <xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='AA'">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </Airline>
        <TPA_Extensions>
          <Name>
            <xsl:attribute name="Number">
              <xsl:choose>
                <xsl:when test="../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$paxref]/@PassengerTypeCode='INF'">
                  <xsl:value-of select="'1'"/>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="$paxref"/>
                </xsl:otherwise>
              </xsl:choose>
              <xsl:text>.1</xsl:text>
            </xsl:attribute>
          </Name>
          <Segment>
            <xsl:attribute name="Number">
              <xsl:value-of select="$tRPH"/>
            </xsl:attribute>
          </Segment>
        </TPA_Extensions>
        <xsl:if test="Text!=''">
          <Text>
            <xsl:choose>
              <xsl:when test="@SSRCode='DOCS' and starts-with(Text,'----')">
                <xsl:value-of select="concat('DB/',substring(Text,5,7),'/')"/>
                <xsl:value-of select="concat(substring-before(substring(Text,13),'-'),'/')"/>
                <xsl:if test="../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$paxref]/@PassengerTypeCode='INF'">
                  <xsl:value-of select="'I'"/>
                </xsl:if>
                <xsl:value-of select="concat(substring-before(substring-after(substring(Text,13),'--'),'-'),'/')"/>
                <xsl:value-of select="translate(substring-after(substring-after(substring(Text,13),'--'),'-'),'-','/')"/>
              </xsl:when>
              <xsl:when test="@SSRCode='DOCS'">
                <xsl:choose>
                  <xsl:when test="../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$paxref]/@PassengerTypeCode='INF'">
                    <xsl:variable name="inft">
                      <xsl:choose>
                        <xsl:when test="contains(Text,'-M-')">
                          <xsl:value-of select="substring-before(Text,'-M-')"/>
                          <xsl:value-of select="'-MI-'"/>
                          <xsl:value-of select="substring-after(Text,'-M-')"/>
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="substring-before(Text,'-F-')"/>
                          <xsl:value-of select="'-FI-'"/>
                          <xsl:value-of select="substring-after(Text,'-F-')"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:variable>
                    <xsl:value-of select="translate($inft,'-','/')"/>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="translate(Text,'-','/')"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="Text"/>
              </xsl:otherwise>
            </xsl:choose>
          </Text>
        </xsl:if>
      </Service>
      <xsl:call-template name="RPHS">
        <xsl:with-param name="RPHS">
          <xsl:value-of select="substring($RPHS,2)"/>
        </xsl:with-param>
        <xsl:with-param name="paxref">
          <xsl:value-of select="$paxref"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <!-- -->
  <xsl:template name="RPHPSeat">
    <xsl:param name="RPHP"/>
    <xsl:if test="string-length($RPHP) != 0">
      <xsl:variable name="paxref">
        <xsl:value-of select="substring($RPHP,1,1)"/>
      </xsl:variable>
      <xsl:call-template name="RPHSSeat">
        <xsl:with-param name="RPHS">
          <xsl:value-of select="@FlightRefNumberRPHList"/>
        </xsl:with-param>
        <xsl:with-param name="paxref">
          <xsl:value-of select="$paxref"/>
        </xsl:with-param>
      </xsl:call-template>
      <xsl:call-template name="RPHPSeat">
        <xsl:with-param name="RPHP">
          <xsl:value-of select="substring($RPHP,2)"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <!-- -->
  <xsl:template name="RPHSSeat">
    <xsl:param name="RPHS"/>
    <xsl:param name="paxref"/>
    <xsl:if test="string-length($RPHS) != 0">
      <xsl:variable name="tRPH">
        <xsl:value-of select="substring($RPHS,1,1)"/>
      </xsl:variable>
      <Service>
        <xsl:attribute name="SSRCode">
          <xsl:choose>
            <xsl:when test="@SeatPreference='W'">NSSW</xsl:when>
            <xsl:when test="@SeatPreference='A'">NSSA</xsl:when>
            <xsl:when test="@SeatPreference='B'">NSSB</xsl:when>
            <xsl:otherwise>NSST</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <Airline>
          <xsl:attribute name="HostedCarrier">
            <xsl:choose>
              <xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='AA'">true</xsl:when>
              <xsl:otherwise>false</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
        </Airline>
        <TPA_Extensions>
          <Name>
            <xsl:attribute name="Number">
              <xsl:value-of select="$paxref"/>
              <xsl:text>.1</xsl:text>
            </xsl:attribute>
          </Name>
        </TPA_Extensions>
        <xsl:if test="Text!=''">
          <Text>
            <xsl:value-of select="Text"/>
          </Text>
        </xsl:if>
      </Service>
      <xsl:call-template name="RPHSSeat">
        <xsl:with-param name="RPHS">
          <xsl:value-of select="substring($RPHS,2)"/>
        </xsl:with-param>
        <xsl:with-param name="paxref">
          <xsl:value-of select="$paxref"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  <!--************************************************************-->
  <!--  		Customer Information                                -->
  <!--************************************************************-->
  <xsl:template match="PNRData" mode="ixplore">
    <TravelItineraryAddInfoRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC" />
          </xsl:attribute>
        </Source>
      </POS>
      <CustomerInfo>
        <xsl:apply-templates select="Traveler"/>
        <xsl:apply-templates select="Telephone" mode="ixplore"/>
        <!--xsl:if test="$RefNumber = '1'"-->
        <Email>
          <xsl:attribute name="EmailAddress">
            <xsl:value-of select="Email" />
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
                <xsl:choose>
                  <xsl:when test="@PassengerTypeCode='CHD'">C09</xsl:when>
                  <xsl:otherwise>
                    <xsl:value-of select="@PassengerTypeCode"/>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="TravelerRefNumber">
                <xsl:value-of select="$refnum" />
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

  <xsl:template match="PNRData" mode="other">
    <TravelItineraryAddInfoRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC" />
          </xsl:attribute>
        </Source>
      </POS>
      <CustomerInfo>
        <xsl:apply-templates select="Traveler"/>
        <xsl:apply-templates select="Telephone" mode="other"/>
        <!--xsl:if test="$RefNumber = '1'"-->
        <Email>
          <xsl:attribute name="EmailAddress">
            <xsl:value-of select="Email" />
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
                <xsl:value-of select="TravelerRefNumber/@RPH" />
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
                <xsl:value-of select="$refnum" />
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
          <xsl:value-of select="TravelerRefNumber/@RPH" />
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
        <xsl:value-of select="PersonName/GivenName" />
        <xsl:if test="PersonName/NamePrefix != ''">
          <xsl:text> </xsl:text>
          <xsl:value-of select="PersonName/NamePrefix"/>
        </xsl:if>
      </GivenName>
      <Surname>
        <xsl:value-of select="PersonName/Surname" />
      </Surname>
      <xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
        <Infant Ind="true"/>
      </xsl:if>
    </PersonName>
  </xsl:template>
  <!--************************************************************-->
  <!--  			 Air Itinerary                                       -->
  <!--************************************************************-->
  <xsl:template match="Element[@Operation='insert' and @Child='OriginDestinationOptions']">
    <AirBook>
      <OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC" />
            </xsl:attribute>
          </Source>
        </POS>
        <AirItinerary>
          <xsl:attribute name="DirectionInd">
            <xsl:choose>
              <xsl:when test="OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode = 	OriginDestinationOptions/OriginDestinationOption[position()=last()]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode">Return</xsl:when>
              <xsl:otherwise>Oneway</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <OriginDestinationOptions>
            <xsl:apply-templates select="OriginDestinationOptions/OriginDestinationOption" />
          </OriginDestinationOptions>
        </AirItinerary>
      </OTA_AirBookRQ>
    </AirBook>
  </xsl:template>
  <!--************************************************************-->
  <!--  			 Form of Payment                                       -->
  <!--************************************************************-->
  <xsl:template match="Element[@Operation='insert' and @Child='FormOfPayment']">
    <AirBook>
      <OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
        <POS>
          <Source>
            <xsl:attribute name="PseudoCityCode">
              <xsl:value-of select="$PCC" />
            </xsl:attribute>
          </Source>
        </POS>
        <PriceRequestInformation Retain="true">
          <OptionalQualifiers>
            <FOP_Qualifiers>
              <BasicFOP>
                <xsl:if test="FormOfPayment/PaymentAmount">
                  <Fare>
                    <xsl:attribute name="Amount">
                      <xsl:value-of select="FormOfPayment/PaymentAmount/@Amount"/>
                    </xsl:attribute>
                  </Fare>
                </xsl:if>
                <xsl:choose>
                  <xsl:when test="FormOfPayment/PaymentCard">
                    <CC_Info>
                      <PaymentCard>
                        <xsl:attribute name="Code">
                          <xsl:value-of select="FormOfPayment/PaymentCard/@CardCode"/>
                        </xsl:attribute>
                        <xsl:attribute name="ExpireDate">
                          <xsl:value-of select="FormOfPayment/PaymentCard/@ExpireDate"/>
                        </xsl:attribute>
                        <xsl:attribute name="Number">
                          <xsl:value-of select="FormOfPayment/PaymentCard/@CardNumber"/>
                        </xsl:attribute>
                      </PaymentCard>
                    </CC_Info>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="Type">
                      <xsl:value-of select="CA"/>
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
              </BasicFOP>
            </FOP_Qualifiers>
          </OptionalQualifiers>
        </PriceRequestInformation>
      </OTA_AirBookRQ>
    </AirBook>
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
                <xsl:value-of select="RoomStays/RoomStay/RoomRates/RoomRate/@RatePlanCode" />
              </xsl:attribute>
              <xsl:attribute name="NumberOfUnits">
                <xsl:value-of select="RoomStays/RoomStay/RoomRates/RoomRate/@NumberOfUnits" />
              </xsl:attribute>
              <xsl:attribute name="ReqdGuaranteeType">GuaranteeRequired</xsl:attribute>
            </RoomType>
          </RoomTypes>
          <GuestCounts>
            <GuestCount>
              <xsl:attribute name="Count">
                <xsl:value-of select="RoomStays/RoomStay/GuestCounts/GuestCount /@Count" />
              </xsl:attribute>
            </GuestCount>
          </GuestCounts>
          <TimeSpan>
            <xsl:attribute name="Start">
              <xsl:value-of select="substring(string(RoomStays/RoomStay/TimeSpan/@Start),1,10)" />
            </xsl:attribute>
            <xsl:attribute name="End">
              <xsl:value-of select="substring(string(RoomStays/RoomStay/TimeSpan/@End),1,10)" />
            </xsl:attribute>
          </TimeSpan>
          <BasicPropertyInfo>
            <xsl:attribute name="HotelCode">
              <xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@HotelCode" />
            </xsl:attribute>
            <!--Note: sabre doesn't use ChainCode - only HotelCode -->
            <!--xsl:attribute name="ChainCode"><xsl:value-of select="RoomStays/RoomStay/BasicPropertyInfo/@ChainCode"/></xsl:attribute-->
          </BasicPropertyInfo>
          <xsl:if test="RoomStays/RoomStay/Memberships">
            <Memberships>
              <xsl:apply-templates select="RoomStays/RoomStay/Memberships/Membership" />
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
          <xsl:value-of select="VehResRQCore/VehRentalCore/@PickUpDateTime" />
        </xsl:attribute>
        <xsl:attribute name="ReturnDateTime">
          <xsl:value-of select="VehResRQCore/VehRentalCore/@ReturnDateTime" />
        </xsl:attribute>
        <PickUpLocation>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode" />
          </xsl:attribute>
        </PickUpLocation>
        <ReturnLocation>
          <xsl:attribute name="LocationCode">
            <xsl:value-of select="VehResRQCore/VehRentalCore/ReturnLocation /@LocationCode" />
          </xsl:attribute>
        </ReturnLocation>
      </VehRentalCore>
      <xsl:if test="Customer">
        <Customer>
          <Primary>
            <xsl:apply-templates select="VehResRQCore/Customer/Primary/CustLoyalty" />
          </Primary>
        </Customer>
      </xsl:if>
      <xsl:if test="VehResRQCore/VendorPref">
        <VendorPref>
          <xsl:attribute name="Code">
            <xsl:value-of select="VehResRQCore/VendorPref/@Code" />
          </xsl:attribute>
        </VendorPref>
      </xsl:if>
      <VehPref>
        <xsl:choose>
          <xsl:when test="VehResRQCore/VehPref/@AirConditionInd!=''">
            <xsl:attribute name="AirConditionInd">
              <xsl:value-of select="VehResRQCore/VehPref/@AirConditionInd" />
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="AirConditionInd">1</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:choose>
          <xsl:when test="VehResRQCore/VehPref/@TransmissionType!=''">
            <xsl:attribute name="TransmissionType">
              <xsl:value-of select="VehResRQCore/VehPref/@TransmissionType" />
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="TransmissionType">Automatic</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
        <VehType>
          <xsl:attribute name="VehicleCategory">
            <xsl:value-of select="VehResRQCore/VehPref/VehType/@VehicleCategory" />
          </xsl:attribute>
        </VehType>
      </VehPref>
      <RateQualifier>
        <xsl:if test="VehResRQCore/RateQualifier/@RateCategory!=''">
          <xsl:attribute name="RateCategory">
            <xsl:value-of select="VehResRQCore/RateQualifier/@RateCategory" />
          </xsl:attribute>
        </xsl:if>
      </RateQualifier>
      <TPA_Extensions>
        <NumberOfCars>
          <xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/@NumCars" />
        </NumberOfCars>
      </TPA_Extensions>
    </VehResRQCore>
  </xsl:template>

  <xsl:template match="OriginDestinationOption">
    <OriginDestinationOption>
      <xsl:apply-templates select="FlightSegment" />
    </OriginDestinationOption>
  </xsl:template>

  <xsl:template match="FlightSegment">
    <FlightSegment>
      <xsl:attribute name="ActionCode">NN</xsl:attribute>
      <xsl:attribute name="DepartureDateTime">
        <xsl:value-of select="@DepartureDateTime" />
      </xsl:attribute>
      <xsl:attribute name="ArrivalDateTime">
        <xsl:value-of select="@ArrivalDateTime" />
      </xsl:attribute>
      <xsl:attribute name="FlightNumber">
        <xsl:value-of select="@FlightNumber" />
      </xsl:attribute>
      <xsl:attribute name="NumberInParty">
        <xsl:value-of select="@NumberInParty" />
      </xsl:attribute>
      <xsl:attribute name="ResBookDesigCode">
        <xsl:value-of select="@ResBookDesigCode" />
      </xsl:attribute>
      <DepartureAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="DepartureAirport/@LocationCode" />
        </xsl:attribute>
      </DepartureAirport>
      <ArrivalAirport>
        <xsl:attribute name="LocationCode">
          <xsl:value-of select="ArrivalAirport/@LocationCode" />
        </xsl:attribute>
      </ArrivalAirport>
      <OperatingAirline>
        <xsl:choose>
          <xsl:when test="OperatingAirline">
            <xsl:attribute name="Code">
              <xsl:value-of select="OperatingAirline/@Code" />
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="Code">
              <xsl:value-of select="MarketingAirline/@Code" />
            </xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>
      </OperatingAirline>
      <xsl:if test="Equipment">
        <Equipment>
          <xsl:attribute name="AirEquipType">
            <xsl:value-of select="Equipment/@AirEquipType" />
          </xsl:attribute>
        </Equipment>
      </xsl:if>
      <MarketingAirline>
        <xsl:attribute name="Code">
          <xsl:value-of select="MarketingAirline/@Code" />
        </xsl:attribute>
      </MarketingAirline>
      <xsl:if test="MarriageGrp!=''">
        <MarriageGrp>
          <xsl:attribute name="Ind">
            <xsl:value-of select="MarriageGrp"/>
          </xsl:attribute>
        </MarriageGrp>
      </xsl:if>
    </FlightSegment>
  </xsl:template>

  <xsl:template match="PTC_FareBreakdown">
    <PTC_FareBreakdown>
      <PassengerTypeQuantity>
        <xsl:attribute name="Code">
          <xsl:value-of select="PassengerTypeQuantity/@Code" />
        </xsl:attribute>
      </PassengerTypeQuantity>
      <xsl:apply-templates select="FareBasisCodes/FareBasisCode" />
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
                <xsl:value-of select="PassengerFare/BaseFare/@Amount" />
              </xsl:when>
              <xsl:otherwise>0.00</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PassengerFare/BaseFare/@CurrencyCode" />
          </xsl:attribute>
        </BaseFare>
        <EquivFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="PassengerFare/EquivFare /@Amount!=''">
                <xsl:value-of select="PassengerFare/EquivFare/@Amount" />
              </xsl:when>
              <xsl:otherwise>0.00</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PassengerFare/EquivFare/@CurrencyCode" />
          </xsl:attribute>
        </EquivFare>
        <Taxes>
          <xsl:apply-templates select="PassengerFare/Taxes/Tax" />
        </Taxes>
        <Fees>
          <xsl:apply-templates select="PassengerFare/Fees/Fee " />
        </Fees>
        <TotalFare>
          <xsl:attribute name="Amount">
            <xsl:choose>
              <xsl:when test="PassengerFare/TotalFare/@Amount!=''">
                <xsl:value-of select="PassengerFare/TotalFare/@Amount" />
              </xsl:when>
              <xsl:otherwise>0.00</xsl:otherwise>
            </xsl:choose>
          </xsl:attribute>
          <xsl:attribute name="CurrencyCode">
            <xsl:value-of select="PassengerFare/TotalFare/@CurrencyCode" />
          </xsl:attribute>
        </TotalFare>
      </PassengerFare>
    </PTC_FareBreakdown>
  </xsl:template>
  <xsl:template match="FareBasisCode">
    <FareBasisCode>
      <xsl:value-of select="." />
    </FareBasisCode>
  </xsl:template>
  <xsl:template match="Tax">
    <Tax>
      <xsl:attribute name="TaxCode">
        <xsl:value-of select="@TaxCode" />
      </xsl:attribute>
      <xsl:attribute name="Amount">
        <xsl:value-of select="@Amount" />
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="@CurrencyCode" />
      </xsl:attribute>
    </Tax>
  </xsl:template>
  <xsl:template match="Fee ">
    <Fee>
      <xsl:attribute name="Amount">
        <xsl:value-of select="@Amount" />
      </xsl:attribute>
      <xsl:attribute name="CurrencyCode">
        <xsl:value-of select="@CurrencyCode" />
      </xsl:attribute>
    </Fee>
  </xsl:template>
  <xsl:template match="Membership">
    <Membership>
      <xsl:attribute name="ProgramCode">
        <xsl:value-of select="@ProgramCode" />
      </xsl:attribute>
      <xsl:attribute name="BonusCode">
        <xsl:value-of select="@BonusCode" />
      </xsl:attribute>
    </Membership>
  </xsl:template>

  <xsl:template match="CustLoyalty" mode="ixplore">
    <xsl:param name="RefNumber"/>
    <CustLoyalty>
      <xsl:attribute name="RPH">
        <xsl:value-of select="$RefNumber" />
      </xsl:attribute>
      <xsl:attribute name="ProgramID">
        <xsl:value-of select="@ProgramID" />
      </xsl:attribute>
      <xsl:attribute name="MembershipID">
        <xsl:value-of select="@MembershipID" />
      </xsl:attribute>
      <xsl:attribute name="TravelerRefNumber">
        <xsl:value-of select="$RefNumber" />
        <xsl:text>.1</xsl:text>
      </xsl:attribute>
    </CustLoyalty>
  </xsl:template>

  <xsl:template match="CustLoyalty" mode="other">
    <xsl:param name="RefNumber"/>
    <CustLoyalty>
      <xsl:attribute name="RPH">
        <xsl:value-of select="$RefNumber" />
      </xsl:attribute>
      <xsl:attribute name="ProgramID">
        <xsl:value-of select="@ProgramID" />
      </xsl:attribute>
      <xsl:attribute name="MembershipID">
        <xsl:value-of select="@MembershipID" />
      </xsl:attribute>
      <xsl:attribute name="TravelerRefNumber">
        <xsl:value-of select="$RefNumber" />
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
        <xsl:value-of select="@PhoneNumber" />
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
        <xsl:value-of select="@PhoneNumber" />
      </xsl:attribute>
    </Telephone>
  </xsl:template>

  <xsl:template match="Address"  mode="billing">
    <!--ClientAddressRemark>
			<xsl:attribute name="Text">
				<xsl:value-of select="StreetNmbr" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="CityName" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="PostalCode" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="StateProv/@StateCode" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="CountryName/@Code" />
			</xsl:attribute>
		</ClientAddressRemark-->
    <Remark Type="Client Address">
      <Text>
        <xsl:value-of select="StreetNmbr" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CityName" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="PostalCode" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="StateProv/@StateCode" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CountryName/@Code" />
      </Text>
    </Remark>
  </xsl:template>

  <xsl:template match="DeliveryAddress">
    <!--DeliveryAddressRemark>
			<xsl:attribute name="Text">
				<xsl:value-of select="StreetNmbr" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="CityName" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="PostalCode" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="StateProv/@StateCode" />
				<xsl:text>,</xsl:text>
				<xsl:value-of select="CountryName/@Code" />
			</xsl:attribute>
		</DeliveryAddressRemark-->
    <Remark Type="Delivery Address">
      <Text>
        <xsl:value-of select="StreetNmbr" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CityName" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="PostalCode" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="StateProv/@StateCode" />
        <xsl:text>,</xsl:text>
        <xsl:value-of select="CountryName/@Code" />
      </Text>
    </Remark>
  </xsl:template>

  <xsl:template match="SeatRequest">
    <SeatPref>
      <xsl:attribute name="SeatPreference">
        <xsl:value-of select="@SeatPreference" />
      </xsl:attribute>
    </SeatPref>
  </xsl:template>

  <xsl:template match="PriceData">
    <OTA_AirPriceRQ  xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.13.1">
      <POS>
        <Source>
          <xsl:attribute name="PseudoCityCode">
            <xsl:value-of select="$PCC" />
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
          <xsl:if test="../AgencyData/Commission/@Amount!= '' or ../AgencyData/Commission/@Percent!='' or ../AgencyData/ServiceFee/@Amount!= ''">
            <Commission>
              <xsl:choose>
                <xsl:when test="../AgencyData/ServiceFee/@Amount!='0'">
                  <xsl:attribute name="Amount">
                    <xsl:value-of select="../AgencyData/ServiceFee/@Amount"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:when test="../AgencyData/Commission/@Amount!=''">
                  <xsl:attribute name="Amount">
                    <xsl:value-of select="../AgencyData/Commission/@Amount"/>
                  </xsl:attribute>
                </xsl:when>
                <xsl:otherwise>
                  <xsl:attribute name="Percentage">
                    <xsl:value-of select="../AgencyData/Commission/@Percent"/>
                  </xsl:attribute>
                </xsl:otherwise>
              </xsl:choose>
            </Commission>
          </xsl:if>
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
              <PrivateFare Ind="true" />
            </xsl:when>
            <xsl:otherwise>
              <PublicFare Ind="true" />
            </xsl:otherwise>
          </xsl:choose>
          <PriceRetention Default="true" />
        </TPA_Extensions>
      </TravelerInfoSummary>
    </OTA_AirPriceRQ>
  </xsl:template>

  <xsl:template name="month">
    <xsl:param name="month" />
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
