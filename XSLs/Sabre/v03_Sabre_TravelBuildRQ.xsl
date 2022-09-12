<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
  v03_Sabre_TravelBuildRQ.xsl 														       
  ================================================================== 
  Date: 25 Aug 2022 - Kobelev - Updated to lastest version serveral templates	
  Date: 02 Oct 2017 - Rastko - added support for CarResGroupRQ tag		
  Date: 17 Aug 2017 - Rastko - corrected hotel booking process to call avail and then sell	
  Date: 16 Jun 2017 - Rastko - added support for exception process for 6XUF pcc		
  Date: 20 Dec 2015 - Rastko - added support for exception process for 6XUF pcc		
  Date: 18 Dec 2015 - Rastko - change to car version 2.0.1		
  Date: 26 Feb 2015 - Rastko - added F1E2 to pccs with passenger types					   
  Date: 24 Jul 2012 - Rastko - mapped agency address							   
  Date: 23 Mar 2012 - Rastko - corrected ticketing time limit maping and added validating carrier   
  Date: 22 Mar 2012 - Kasun - include cash and check form of payments			
  Date: 24 Oct 2011 - Rastko - corrected remarks processing					      
  Date: 04 Oct 2011 - Shashin - change remark section to spit remarks,address and payments       
  Date: 04 Jan 2011 - Rastko - accept both MI and M in SSR DOCS			       
  Date: 30 Dec 2010 - Rastko - changed SSR request to SSR requested			   
  Date: 18 Dec 2010 - Rastko - changed SSR request to be per pax and per segment	       
  Date: 08 Dec 2010 - Rastko - when CHD sent, send C09 to Sabre			       
  Date: 26 Sep 2010 - Rastko - fixed issue related TravelerRefNumber in CustLoyalty	       
  Date: 06 Sep 2010 - Rastko - fixed issue with SSR DOCS and INF			       
  Date: 03 Sep 2010 - Rastko - corrected seat and SSR processing			       
  Date: 01 Sep 2010 - Rastko - added mapping for seats							       
  Date: 31 Aug 2010 - Rastko - corrected marriage group element				   
  Date: 26 Aug 2010 - Rastko - added marriage element							       
  Date: 19 Aug 2010 - Rastko - added support for DOCS SSR					       
  Date: 30 Jul 2010 - Rastko - added commission mapping						       
  Date: 20 Apr 2010 - Rastko - enabled child birth date and passenger types		       
  Date: 13 May 2010 - Rastko - added check payment							       
  ================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="PCC">
		<xsl:value-of select="translate(OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
	</xsl:variable>
	<xsl:variable name="VA">
		<xsl:value-of select="OTA_TravelItineraryRQ/TPA_Extensions/PriceData/@ValidatingAirlineCode"/>
	</xsl:variable>
	<xsl:variable name="paxtype">
		<xsl:choose>
			<xsl:when test="$PCC = '9N60' or $PCC = 'Z5PF'  or $PCC = 'EO8F' or $PCC = 'F1E2'">Y</xsl:when>
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
		<!--xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or OTA_AirBookRQ/Fulfillment/DeliveryAddress"-->
		<Remarks>
			<xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail">
				<AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" ReturnHostCommand="true" Version="2.1.1">
					<RemarkInfo>
						<xsl:for-each select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail">
							<xsl:choose>
								<xsl:when test="DirectBill/@DirectBill_ID='Check'">
									<FOP_Remark Type="CHECK"/>
								</xsl:when>
								<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
									<FOP_Remark Type="CASH"/>
								</xsl:when>
								<xsl:otherwise>
									<FOP_Remark>
										<CC_Info>
											<PaymentCard>
												<xsl:if test="PaymentCard/@SeriesCode!=''">
													<xsl:attribute name="AirlineCode">
														<xsl:value-of select="../../../../TPA_Extensions/PriceData/@ValidatingAirlineCode"/>
													</xsl:attribute>
													<xsl:attribute name="CardSecurityCode">
														<xsl:value-of select="PaymentCard/@SeriesCode"/>
													</xsl:attribute>
												</xsl:if>
												<xsl:attribute name="Code">
													<xsl:choose>
														<xsl:when test="PaymentCard/@CardCode='MC'">
															<xsl:value-of select="'CA'"/>
														</xsl:when>
														<xsl:when test="PaymentCard/@CardCode='DS'">
															<xsl:value-of select="'DI'"/>
														</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="PaymentCard/@CardCode"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:attribute>
												<xsl:attribute name="ExpireDate">
													<xsl:value-of select="concat('20',substring(PaymentCard/@ExpireDate,3),'-',substring(PaymentCard/@ExpireDate,1,2))"/>
												</xsl:attribute>
												<xsl:attribute name="Number">
													<xsl:value-of select="PaymentCard/@CardNumber"/>
												</xsl:attribute>
											</PaymentCard>
										</CC_Info>
									</FOP_Remark>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</RemarkInfo>
				</AddRemarkRQ>
			</xsl:if>
			<AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" ReturnHostCommand="true" Version="2.1.1">
				<RemarkInfo>
					<Remark Type="Historical">
						<xsl:variable name="pcode">
							<xsl:choose>
								<xsl:when test="not(OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@ActionCode!=''])">PNR</xsl:when>
								<xsl:when test="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@ActionCode!='Passive']">PNR</xsl:when>
								<xsl:when test="OTA_HotelResRQ/HotelReservations/HotelReservation">PNR</xsl:when>
								<xsl:when test="OTA_VehResRQ/VehResRQCore">PNR</xsl:when>
								<xsl:otherwise>SCF</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<Text>
							<xsl:value-of select="concat('ASD/TL/',$pcode,'/Techtrans booking')"/>
						</Text>
					</Remark>
					<xsl:if test="$VA='TK' or $VA='HU' or $VA='QF' or $VA='GF'">
						<Remark Type="General">
							<Text>COM*** CLAIM 7</Text>
						</Remark>
						<Remark Type="General">
							<Text>COM*** AGENT 0</Text>
						</Remark>
					</xsl:if>
					<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks">
						<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark">
							<Remark Type="General">
								<Text>
									<xsl:variable name="vartxt">
										<xsl:value-of select="translate(translate(translate(translate(translate(translate(.,'*#',''),':',''),'@','AT'),';',','),'+',''),'?','')"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="string-length($vartxt) > 69">
											<xsl:value-of select="substring($vartxt,1,69)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$vartxt"/>
										</xsl:otherwise>
									</xsl:choose>
								</Text>
							</Remark>
						</xsl:for-each>
					</xsl:if>
					<xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or OTA_AirBookRQ/Fulfillment/DeliveryAddress">
						<xsl:for-each select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address">
							<xsl:if test="../CardHolderName!=''">
								<Remark Type="Client Address">
									<Text>
										<xsl:value-of select="../CardHolderName"/>
									</Text>
								</Remark>
							</xsl:if>
							<Remark Type="Client Address">
								<Text>
									<xsl:choose>
										<xsl:when test="AddressLine!=''">
											<xsl:value-of select="translate(AddressLine,'*#:@;+','')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate(StreetNmbr,'*#:@;+','')"/>
										</xsl:otherwise>
									</xsl:choose>
								</Text>
							</Remark>
							<Remark Type="Client Address">
								<Text>
									<xsl:value-of select="CityName"/>
									<xsl:if test="StateProv/@StateCode!=''">
										<xsl:value-of select="concat(' ',StateProv/@StateCode)"/>
									</xsl:if>
								</Text>
							</Remark>
							<xsl:if test="PostalCode!=''">
								<Remark Type="Client Address">
									<Text>
										<xsl:value-of select="PostalCode"/>
									</Text>
								</Remark>
							</xsl:if>
							<xsl:if test="CountryName/@Code!=''">
								<Remark Type="Client Address">
									<Text>
										<xsl:value-of select="CountryName/@Code"/>
									</Text>
								</Remark>
							</xsl:if>
						</xsl:for-each>
						<xsl:for-each select="OTA_AirBookRQ/Fulfillment/DeliveryAddress">
							<Remark Type="Delivery Address">
								<Text>
									<xsl:choose>
										<xsl:when test="AddressLine!=''">
											<xsl:value-of select="AddressLine"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="StreetNmbr"/>
										</xsl:otherwise>
									</xsl:choose>
								</Text>
							</Remark>
							<Remark Type="Delivery Address">
								<Text>
									<xsl:value-of select="CityName"/>
									<xsl:if test="StateProv/@StateCode!=''">
										<xsl:value-of select="concat(' ',StateProv/@StateCode)"/>
									</xsl:if>
								</Text>
							</Remark>
							<xsl:if test="PostalCode!=''">
								<Remark Type="Delivery Address">
									<Text>
										<xsl:value-of select="PostalCode"/>
									</Text>
								</Remark>
							</xsl:if>
							<xsl:if test="CountryName/@Code!=''">
								<Remark Type="Delivery Address">
									<Text>
										<xsl:value-of select="CountryName/@Code"/>
									</Text>
								</Remark>
							</xsl:if>
						</xsl:for-each>
					</xsl:if>
				</RemarkInfo>
			</AddRemarkRQ>
		</Remarks>
		<!--/xsl:if-->
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='HostEntry']">
			<HostEntry>
				<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='HostEntry']">
					<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
						<xsl:element name="Request">
							<xsl:attribute name="Output">SCREEN</xsl:attribute>
							<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
							<xsl:attribute name="CDATA">true</xsl:attribute>
							<xsl:element name="HostCommand">
								<xsl:value-of select="Text" />
							</xsl:element>
						</xsl:element>
					</SabreCommandLLSRQ>
				</xsl:for-each>
			</HostEntry>
		</xsl:if>
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
		<xsl:if test="OTA_VehResRQ or CarResGroupRQ/OTA_VehResRQ">
			<CarAvail>
				<xsl:apply-templates select="OTA_VehResRQ/VehResRQCore" mode="carAvail"/>
				<xsl:apply-templates select="CarResGroupRQ/OTA_VehResRQ/VehResRQCore"  mode="carAvail"/>
			</CarAvail>
			<CarBook>
				<xsl:apply-templates select="OTA_VehResRQ/VehResRQCore"/>
				<xsl:apply-templates select="CarResGroupRQ/OTA_VehResRQ/VehResRQCore"/>
			</CarBook>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/MiscellaneousSegments/Segment">
			<MiscellaneousSegments>
				<xsl:apply-templates select="OTA_AirBookRQ/MiscellaneousSegments/Segment" mode="Seg"/>
			</MiscellaneousSegments>
		</xsl:if>
		<xsl:if test="TPA_Extensions/PriceData">
			<Pricing>
				<xsl:apply-templates select="TPA_Extensions/PriceData"/>
			</Pricing>
		</xsl:if>
		<!--xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or OTA_AirBookRQ/Fulfillment/DeliveryAddress">
			<Remarks>
				<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks">
					<AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
						<POS>
							<xsl:value-of select="$PCC"/>
						</POS>
						<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark">
							<BasicRemark>
								<xsl:attribute name="Text"><xsl:value-of select="translate(translate(translate(.,'*',' '),':',''),'@','AT')"/></xsl:attribute>
							</BasicRemark>
						</xsl:for-each>
					</AddRemarkRQ>
				</xsl:if>
				<xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or OTA_AirBookRQ/Fulfillment/DeliveryAddress">
					<AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
						<POS>
							<xsl:value-of select="$PCC"/>
						</POS>
						<xsl:for-each select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address">
							<ClientAddressRemark>
								<xsl:attribute name="Text"><xsl:value-of select="StreetNmbr"/><xsl:text>,</xsl:text><xsl:value-of select="CityName"/><xsl:text>,</xsl:text><xsl:value-of select="PostalCode"/><xsl:text>,</xsl:text><xsl:value-of select="StateProv/@StateCode"/><xsl:text>,</xsl:text><xsl:value-of select="CountryName/@Code"/></xsl:attribute>
							</ClientAddressRemark>
						</xsl:for-each>
						<xsl:for-each select="OTA_AirBookRQ/Fulfillment/DeliveryAddress">
							<DeliveryAddressRemark>
								<xsl:attribute name="Text"><xsl:value-of select="StreetNmbr"/><xsl:text>,</xsl:text><xsl:value-of select="CityName"/><xsl:text>,</xsl:text><xsl:value-of select="PostalCode"/><xsl:text>,</xsl:text><xsl:value-of select="StateProv/@StateCode"/><xsl:text>,</xsl:text><xsl:value-of select="CountryName/@Code"/></xsl:attribute>
							</DeliveryAddressRemark>
						</xsl:for-each>
					</AddRemarkRQ>
				</xsl:if>
				<xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail">
					<AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
						<POS>
							<xsl:value-of select="$PCC"/>
						</POS>
						<xsl:for-each select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail">
							<xsl:choose>
								<xsl:when test="DirectBill/@DirectBill_ID='Check'">
									<FOPRemark Type="CHECK"/>
								</xsl:when>
								<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
									<FOPRemark Type="CASH"/>
								</xsl:when>
								<xsl:otherwise>
									<FOPRemark>
										<CCInfo>
											<CreditCardVendor>
												<xsl:attribute name="Code"><xsl:choose><xsl:when test="PaymentCard/@CardCode='MC'"><xsl:value-of select="'CA'"/></xsl:when><xsl:when test="PaymentCard/@CardCode='DS'"><xsl:value-of select="'DI'"/></xsl:when><xsl:otherwise><xsl:value-of select="PaymentCard/@CardCode"/></xsl:otherwise></xsl:choose></xsl:attribute>
											</CreditCardVendor>
											<CreditCardNumber>
												<xsl:attribute name="Code"><xsl:value-of select="PaymentCard/@CardNumber"/></xsl:attribute>
											</CreditCardNumber>
											<CreditCardExpiration>
												<xsl:attribute name="ExpireDate"><xsl:value-of select="PaymentCard/@ExpireDate"/></xsl:attribute>
											</CreditCardExpiration>
										</CCInfo>
									</FOPRemark>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</AddRemarkRQ>
				</xsl:if>
			</Remarks>
		</xsl:if-->
		<xsl:if test=" OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks[@RemarkType='C']">
			<SpecialRemarks>
				<AddRemarkRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.0.1">
					<POS>
						<xsl:value-of select="$PCC"/>
					</POS>
					<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark">
						<xsl:if test="@RemarkType='C'">
							<HistoricalRemark>
								<xsl:attribute name="Text">
									<xsl:value-of select="."/>
								</xsl:attribute>
							</HistoricalRemark>
						</xsl:if>
					</xsl:for-each>
				</AddRemarkRQ>
			</SpecialRemarks>
		</xsl:if>
		<xsl:if test="TPA_Extensions/PNRData/Traveler[substring(@PassengerTypeCode,1,1)='C' or @PassengerTypeCode='INF']">
			<SpecialServicesCI>
				<SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.2.1">
					<SpecialServiceInfo>
						<xsl:for-each select="TPA_Extensions/PNRData/Traveler[substring(@PassengerTypeCode,1,1)='C']">
							<xsl:variable name="RPH">
								<xsl:value-of select="TravelerRefNumber/@RPH"/>
							</xsl:variable>
							<xsl:variable name="BD">
								<xsl:value-of select="@BirthDate"/>
							</xsl:variable>
							<xsl:for-each 	select="../../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
								<!--Service SSRCode="CHLD">
										<Airline>
											<xsl:attribute name="HostedCarrier">
												<xsl:choose>
													<xsl:when test="MarketingAirline/@Code='AA'">true</xsl:when>
													<xsl:when test="MarketingAirline/@Code='WY'">true</xsl:when>
													<xsl:otherwise>false</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</Airline>
										<TPA_Extensions>
											<Name>
												<xsl:attribute name="Number"><xsl:value-of select="$RPH"/><xsl:text>.1</xsl:text></xsl:attribute>
											</Name>
											<Segment>
												<xsl:attribute name="Number"><xsl:value-of select="@RPH"/></xsl:attribute>
											</Segment>
										</TPA_Extensions>
										<Text>
											<xsl:value-of select="substring($BD,9)"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring($BD,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
											<xsl:value-of select="substring($BD,3,2)"/>
										</Text>
									</Service-->

								<Service SSR_Code="CHLD">
									<xsl:attribute name="SegmentNumber">
										<xsl:value-of select="@RPH"/>
									</xsl:attribute>
									<PersonName>
										<xsl:attribute name="NameNumber">
											<xsl:choose>
												<xsl:when test="../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$RPH]/				@PassengerTypeCode='CHD'">
													<xsl:value-of select="$RPH"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$RPH"/>
												</xsl:otherwise>
											</xsl:choose>
											<xsl:text>.1</xsl:text>
										</xsl:attribute>
									</PersonName>
									<Text>
										<xsl:value-of select="substring($BD,9)"/>
										<xsl:call-template name="month">
											<xsl:with-param name="month">
												<xsl:value-of select="substring($BD,6,2)"/>
											</xsl:with-param>
										</xsl:call-template>
										<xsl:value-of select="substring($BD,3,2)"/>
									</Text>
									<VendorPrefs>
										<Airline>
											<xsl:attribute name="Hosted">
												<xsl:choose>
													<xsl:when test="MarketingAirline/@Code='AA'">true</xsl:when>
													<xsl:when test="MarketingAirline/@Code='WY'">true</xsl:when>
													<xsl:otherwise>false</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</Airline>
									</VendorPrefs>
								</Service>
							</xsl:for-each>
						</xsl:for-each>
						<xsl:for-each select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='INF']">
							<xsl:variable name="BD">
								<xsl:value-of select="@BirthDate"/>
							</xsl:variable>
							<xsl:variable name="Surname">
								<xsl:value-of select="PersonName/Surname"/>
							</xsl:variable>
							<xsl:variable name="GivenName">
								<xsl:value-of select="PersonName/GivenName"/>
							</xsl:variable>
							<xsl:variable name="NamePrefix">
								<xsl:value-of select="PersonName/NamePrefix"/>
							</xsl:variable>
							<xsl:for-each 	select="../../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
								<!--Service SSRCode="INFT">
										<Airline>
											<xsl:attribute name="HostedCarrier">
												<xsl:choose>
													<xsl:when test="MarketingAirline/@Code='AA'">true</xsl:when>
													<xsl:when test="MarketingAirline/@Code='WY'">true</xsl:when>
													<xsl:otherwise>false</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</Airline>
										<TPA_Extensions>
											<Name>
												<xsl:attribute name="Number">
													<xsl:value-of select="'1.1'"/>
												</xsl:attribute>
											</Name>
											<Segment>
												<xsl:attribute name="Number"><xsl:value-of select="@RPH"/></xsl:attribute>
											</Segment>
										</TPA_Extensions>
										<Text>
											<xsl:value-of select="$Surname"/>
											<xsl:text>/</xsl:text>
											<xsl:value-of select="$GivenName"/>
											<xsl:if test="$NamePrefix != ''">
												<xsl:value-of select="string(' ')"/>
												<xsl:value-of select="$NamePrefix"/>
											</xsl:if>
											<xsl:text>/</xsl:text>
											<xsl:value-of select="substring($BD,9)"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring($BD,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
											<xsl:value-of select="substring($BD,3,2)"/>
										</Text>
									</Service-->
								<xsl:variable name="AirlineCompany">
									<xsl:value-of select="MarketingAirline/@Code"/>
								</xsl:variable>
								<Service SSR_Code="INFT">
									<xsl:attribute name="SegmentNumber">
										<xsl:value-of select="@RPH"/>
									</xsl:attribute>
									<PersonName>
										<xsl:attribute name="NameNumber">
											<xsl:value-of select="'1.1'"/>
										</xsl:attribute>
									</PersonName>
									<Text>

										<xsl:choose>
											<xsl:when test="$AirlineCompany='AA'">4INFT/</xsl:when>
											<xsl:otherwise>3INFT/</xsl:otherwise>
										</xsl:choose>
										<xsl:value-of select="$Surname"/>
										<xsl:text>/</xsl:text>
										<xsl:value-of select="$GivenName"/>
										<xsl:if test="$NamePrefix != ''">
											<xsl:value-of select="string(' ')"/>
											<xsl:value-of select="$NamePrefix"/>
										</xsl:if>
										<xsl:text>/</xsl:text>
										<xsl:value-of select="substring($BD,9)"/>
										<xsl:call-template name="month">
											<xsl:with-param name="month">
												<xsl:value-of select="substring($BD,6,2)"/>
											</xsl:with-param>
										</xsl:call-template>
										<xsl:value-of select="substring($BD,3,2)"/>-1.1
										<!--<xsl:text>-1.1</xsl:text>-->
									</Text>
									<VendorPrefs>
										<Airline>
											<xsl:attribute name="Hosted">
												<xsl:choose>
													<xsl:when test="MarketingAirline/@Code='AA'">true</xsl:when>
													<xsl:when test="MarketingAirline/@Code='WY'">true</xsl:when>
													<xsl:otherwise>false</xsl:otherwise>
												</xsl:choose>
											</xsl:attribute>
										</Airline>
									</VendorPrefs>
								</Service>
							</xsl:for-each>
						</xsl:for-each>
					</SpecialServiceInfo>
				</SpecialServiceRQ>
			</SpecialServicesCI>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest">
			<SpecialServicesSeat>
				<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest">
					<xsl:call-template name="RPHPSeat">
						<xsl:with-param name="RPHP">
							<xsl:value-of select="@TravelerRefNumberRPHList"/>
						</xsl:with-param>
					</xsl:call-template>
				</xsl:for-each>
			</SpecialServicesSeat>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest">
			<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest">
				<SpecialServicesSSR>
					<SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.2.1">
						<SpecialServiceInfo>
							<xsl:call-template name="RPHP">
								<xsl:with-param name="RPHP">
									<xsl:value-of select="@TravelerRefNumberRPHList"/>
								</xsl:with-param>
							</xsl:call-template>
						</SpecialServiceInfo>
					</SpecialServiceRQ>
				</SpecialServicesSSR>
			</xsl:for-each>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation">
			<SpecialServicesOSI>
				<SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.4.1">
					<POS>
						<Source>
							<xsl:attribute name="PseudoCityCode">
								<xsl:value-of select="$PCC"/>
							</xsl:attribute>
						</Source>
					</POS>
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
				</SpecialServiceRQ>
			</SpecialServicesOSI>
		</xsl:if>
		<xsl:if test="TPA_Extensions/PNRData/Queue/@QueueNumber!='' and $PCC!='6XUF'">
			<Queue>
				<QueuePlaceRQ ReturnHostCommand="false" Version="2.0.2" xmlns="http://webservices.sabre.com/sabreXML/2011/10" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
					<QueueInfo>
						<QueueIdentifier>
							<xsl:choose>
								<xsl:when test="TPA_Extensions/PNRData/Queue/@QueueNumber != ''">
									<xsl:attribute name="Number">
										<xsl:value-of select="TPA_Extensions/PNRData/Queue/@QueueNumber"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:when test="TPA_Extensions/PNRData/Queue/@QueueName != ''">
									<xsl:attribute name="Name">
										<xsl:value-of select="TPA_Extensions/PNRData/Queue/@QueueName"/>
									</xsl:attribute>
								</xsl:when>
							</xsl:choose>
							<xsl:choose>
								<xsl:when test="TPA_Extensions/PNRData/Queue/@Category!=''">
									<xsl:attribute name="PrefatoryInstructionCode">
										<xsl:value-of select="TPA_Extensions/PNRData/Queue/@Category"/>
									</xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="PrefatoryInstructionCode">
										<xsl:value-of select="'0'"/>
									</xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:if test="TPA_Extensions/PNRData/Queue/@PseudoCityCode != ''">
								<xsl:attribute name="PseudoCityCode">
									<xsl:value-of select="TPA_Extensions/PNRData/Queue/@PseudoCityCode"/>
								</xsl:attribute>
							</xsl:if>
						</QueueIdentifier>
						<UniqueID/>
					</QueueInfo>
				</QueuePlaceRQ>
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
										<xsl:otherwise>TXML</xsl:otherwise>
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
				<Locator>
					<xsl:value-of select="UniqueID/@ID" />
				</Locator>
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
	<!-- -->
	<xsl:template name="RPHP">
		<xsl:param name="RPHP"/>
		<xsl:if test="string-length($RPHP) != 0">
			<xsl:variable name="paxref">
				<xsl:value-of select="substring($RPHP,1,1)"/>
			</xsl:variable>
			<xsl:variable name="segs">
				<xsl:choose>
					<xsl:when test="@FlightRefNumberRPHList!=''">
						<xsl:value-of select="@FlightRefNumberRPHList"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:for-each select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
							<xsl:value-of select="@RPH"/>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:call-template name="RPHS">
				<xsl:with-param name="RPHS">
					<xsl:value-of select="$segs"/>
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
			<xsl:choose>
				<xsl:when test="@SSRCode='DOCS' and substring(Text,1,1)='P'">
					<AdvancePassenger>
						<xsl:attribute name="SegmentNumber">
							<xsl:value-of select="$tRPH"/>
						</xsl:attribute>
						<Document Type="P">
							<xsl:attribute name="ExpirationDate">
								<xsl:variable name="expd">
									<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
								</xsl:variable>
								<xsl:value-of select="concat('20',substring($expd,6),'-')"/>
								<xsl:call-template name="monthLong">
									<xsl:with-param name="month">
										<xsl:value-of select="substring($expd,3,3)"/>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:value-of select="concat('-',substring($expd,1,2))"/>
							</xsl:attribute>
							<xsl:attribute name="Number">
								<xsl:value-of select="substring-before(substring-after(substring-after(Text,'-'),'-'),'-')"/>
							</xsl:attribute>
							<IssueCountry>
								<xsl:value-of select="substring-before(substring-after(Text,'-'),'-')"/>
							</IssueCountry>
							<NationalityCountry>
								<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-')"/>
							</NationalityCountry>
						</Document>
						<PersonName>
							<xsl:attribute name="DateOfBirth">
								<xsl:variable name="dob">
									<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-')"/>
								</xsl:variable>
								<xsl:variable name="doby">
									<xsl:value-of select="substring($dob,6,1)"/>
								</xsl:variable>
								<xsl:variable name="dobyc">
									<xsl:choose>
										<xsl:when test="$doby='0' or $doby='1' or $doby='2'">20</xsl:when>
										<xsl:otherwise>19</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:value-of select="concat($dobyc,substring($dob,6),'-')"/>
								<xsl:call-template name="monthLong">
									<xsl:with-param name="month">
										<xsl:value-of select="substring($dob,3,3)"/>
									</xsl:with-param>
								</xsl:call-template>
								<xsl:value-of select="concat('-',substring($dob,1,2))"/>
							</xsl:attribute>
							<xsl:attribute name="Gender">
								<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),1,1)"/>
							</xsl:attribute>
							<xsl:attribute name="NameNumber">
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
							<xsl:variable name="givenn">
								<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
							</xsl:variable>
							<GivenName>
								<xsl:choose>
									<xsl:when test="contains($givenn,'-')">
										<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$givenn"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
							</GivenName>
							<xsl:if test="contains($givenn,'-')">
								<MiddleName>
									<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
								</MiddleName>
							</xsl:if>
							<Surname>
								<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
							</Surname>
						</PersonName>
						<VendorPrefs>
							<Airline>
								<xsl:attribute name="Hosted">
									<xsl:choose>
										<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='AA'">true</xsl:when>
										<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='WY'">true</xsl:when>
										<xsl:otherwise>false</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Airline>
						</VendorPrefs>
					</AdvancePassenger>
				</xsl:when>
				<xsl:when test="@SSRCode='DOCO' or @SSRCode='DOCS'">
					<SecureFlight>
						<xsl:attribute name="SegmentNumber">
							<xsl:value-of select="$tRPH"/>
						</xsl:attribute>
						<xsl:if test="substring(Text,2,1)='K'">
							<KnownTravelerNumber>
								<xsl:value-of select="substring-before(substring(Text,4),'-')"/>
							</KnownTravelerNumber>
						</xsl:if>
						<PersonName>
							<xsl:if test="@SSRCode='DOCS'">
								<xsl:attribute name="DateOfBirth">
									<xsl:variable name="dob">
										<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-')"/>
									</xsl:variable>
									<xsl:variable name="doby">
										<xsl:value-of select="substring($dob,6,1)"/>
									</xsl:variable>
									<xsl:variable name="dobyc">
										<xsl:choose>
											<xsl:when test="$doby='0' or $doby='1' or $doby='2'">20</xsl:when>
											<xsl:otherwise>19</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="concat($dobyc,substring($dob,6),'-')"/>
									<xsl:call-template name="monthLong">
										<xsl:with-param name="month">
											<xsl:value-of select="substring($dob,3,3)"/>
										</xsl:with-param>
									</xsl:call-template>
									<xsl:value-of select="concat('-',substring($dob,1,2))"/>
								</xsl:attribute>
								<xsl:attribute name="Gender">
									<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),1,1)"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="NameNumber">
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
							<xsl:if test="@SSRCode='DOCS'">
								<xsl:variable name="givenn">
									<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
								</xsl:variable>
								<GivenName>
									<xsl:choose>
										<xsl:when test="contains($givenn,'-')">
											<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$givenn"/>
										</xsl:otherwise>
									</xsl:choose>
									<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
									<xsl:if test="contains($givenn,'-')">
										<xsl:value-of select="concat(' ',substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'))"/>
									</xsl:if>
								</GivenName>
								<!--xsl:if test="contains($givenn,'-')">
									<MiddleName>
										<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
									</MiddleName>
								</xsl:if-->
								<Surname>
									<xsl:value-of select="substring-before(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(substring-after(Text,'-'),'-'),'-'),'-'),'-'),'-'),'-'),'-')"/>
								</Surname>
							</xsl:if>
						</PersonName>
						<xsl:if test="substring(Text,2,1)='R'">
							<RedressNumber>
								<xsl:value-of select="substring-before(substring(Text,4),'-')"/>
							</RedressNumber>
						</xsl:if>
						<VendorPrefs>
							<Airline>
								<xsl:attribute name="Hosted">
									<xsl:choose>
										<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='AA'">true</xsl:when>
										<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='WY'">true</xsl:when>
										<xsl:otherwise>false</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Airline>
						</VendorPrefs>
					</SecureFlight>
				</xsl:when>
				<xsl:otherwise>
					<Service SSR_Code="{@SSRCode}">
						<xsl:attribute name="SegmentNumber">
							<xsl:value-of select="$tRPH"/>
						</xsl:attribute>
						<PersonName>
							<xsl:attribute name="NameNumber">
								<xsl:choose>
									<xsl:when test="../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$paxref]/@PassengerTypeCode='INF'">
										<xsl:value-of select="$paxref"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$paxref"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:text>.1</xsl:text>
							</xsl:attribute>
						</PersonName>
						<xsl:if test="Text!=''">
							<Text>
								<xsl:value-of select="Text"/>
							</Text>
						</xsl:if>
						<VendorPrefs>
							<Airline>
								<xsl:attribute name="Hosted">
									<xsl:choose>
										<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='AA'">true</xsl:when>
										<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH=$tRPH]/MarketingAirline/@Code='WY'">true</xsl:when>
										<xsl:otherwise>false</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Airline>
						</VendorPrefs>
					</Service>
				</xsl:otherwise>
			</xsl:choose>
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
			<AirSeatRQ Version="2.0.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
				<Seats>
					<Seat>
						<xsl:attribute name="NameNumber">
							<xsl:value-of select="$paxref"/>
							<xsl:text>.1</xsl:text>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="@SeatNumber!=''">
								<xsl:attribute name="Number">
									<xsl:value-of select="@SeatNumber"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="Preference">
									<xsl:choose>
										<xsl:when test="@SeatPreference='W'">NW</xsl:when>
										<xsl:when test="@SeatPreference='A'">NA</xsl:when>
										<xsl:otherwise>AN</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:attribute name="SegmentNumber">
							<xsl:value-of select="$tRPH"/>
						</xsl:attribute>
					</Seat>
				</Seats>
			</AirSeatRQ>
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
								<xsl:choose>
									<xsl:when test="@PassengerTypeCode='CHD'">C09</xsl:when>
									<xsl:when test="@PassengerTypeCode='JCB'">ADT</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@PassengerTypeCode"/>
									</xsl:otherwise>
								</xsl:choose>
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
				<xsl:if test="Address[@Type='Agency']/StreetNmbr!=''">
					<Address>
						<xsl:if test="Address[@Type='Agency']/AddressLine!=''">
							<TPA_Extensions>
								<AgencyName>
									<xsl:value-of select="Address[@Type='Agency']/AddressLine"/>
								</AgencyName>
							</TPA_Extensions>
						</xsl:if>
						<StreetNmbr>
							<xsl:value-of select="Address[@Type='Agency']/StreetNmbr"/>
						</StreetNmbr>
						<CityName>
							<xsl:value-of select="Address[@Type='Agency']/CityName"/>
						</CityName>
						<PostalCode>
							<xsl:value-of select="Address[@Type='Agency']/PostalCode"/>
						</PostalCode>
						<StateCountyProv>
							<xsl:attribute name="StateCode">
								<xsl:value-of select="Address[@Type='Agency']/StateProv/@StateCode"/>
							</xsl:attribute>
						</StateCountyProv>
						<CountryName>
							<xsl:attribute name="Code">
								<xsl:value-of select="Address[@Type='Agency']/CountryName/@Code"/>
							</xsl:attribute>
						</CountryName>
					</Address>
				</xsl:if>
				<Ticketing>
					<xsl:variable name="tktlimit">
						<xsl:choose>
							<xsl:when test="Ticketing/@TicketTimeLimit!=''">
								<xsl:call-template name="if-midnight-fix-date">
									<xsl:with-param name="date" select="Ticketing/@TicketTimeLimit" />
								</xsl:call-template>
								<!--<xsl:value-of select="Ticketing/@TicketTimeLimit"/>-->
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="if-midnight-fix-date">
									<xsl:with-param name="date" select="../../OTA_AirBookRQ/Ticketing/@TicketTimeLimit" />
								</xsl:call-template>
								<!--<xsl:value-of select="../../OTA_AirBookRQ/Ticketing/@TicketTimeLimit"/>-->
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="TktDate">
						<xsl:value-of select="substring($tktlimit,1,10)"/>
					</xsl:variable>
					<xsl:variable name="TktTime">
						<xsl:value-of select="substring-after($tktlimit,'T')"/>
					</xsl:variable>
					<xsl:variable name="TktTime2">
						<xsl:value-of select="substring(string($TktTime),1,5)"/>
					</xsl:variable>
					<xsl:variable name="TktAdvisory">
						<xsl:choose>
							<xsl:when test="Ticketing/TicketAdvisory='XL'">XL</xsl:when>
							<xsl:when test="Ticketing/TicketAdvisory= 'TL'">TL</xsl:when>
							<xsl:otherwise>TL</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$TktAdvisory='XL'">
							<xsl:attribute name="TicketType">
								<xsl:variable name="TktHour">
									<xsl:value-of select="substring(string($TktTime2),1,2)"/>
								</xsl:variable>
								<xsl:text>8</xsl:text>
								<xsl:choose>
									<xsl:when test="number($TktHour)&lt;12">
										<xsl:choose>
											<xsl:when test="starts-with(string($TktHour),'0') ">
												<xsl:value-of select="substring(string($TktHour),2,1)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="string($TktHour)"/>
											</xsl:otherwise>
										</xsl:choose>
										<xsl:text>A/</xsl:text>
									</xsl:when>
									<xsl:when test="number($TktHour)=12">
										<xsl:value-of select="number($TktHour)"/>
										<xsl:text>N/</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="number($TktHour)-12"/>
										<xsl:text>P/</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:value-of select="substring(string($TktDate),9,2)"/>
								<xsl:call-template name="month">
									<xsl:with-param name="month">
										<xsl:value-of select="substring($TktDate,6,2)"/>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="TicketType">7TAW</xsl:attribute>
							<xsl:if test="$tktlimit!=''">
								<xsl:attribute name="TicketTimeLimit">
									<xsl:value-of select="$tktlimit"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:attribute name="TicketingDate">
								<xsl:value-of select="$TktDate"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<!--xsl:if test="(Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!='') and $PCC!='6XUF'">
						<xsl:choose>
							<xsl:when test="Queue/@QueueNumber!=''">
								<xsl:attribute name="PseudoCityCode"><xsl:value-of select="Queue/@PseudoCityCode"/></xsl:attribute>
								<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/></xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="PseudoCityCode"><xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/></xsl:attribute>
								<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/></xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if-->
				</Ticketing>
			</AgencyInfo>
		</TravelItineraryAddInfoRQ>
	</xsl:template>
	<xsl:template match="PNRData" mode="other">
		<xsl:if test="AccountingLine!=''">
			<TravelItineraryAddInfoRQ Version="2.2.1" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
				<CustomerInfo>
					<CustomerIdentifier>
						<xsl:value-of select="AccountingLine"/>
					</CustomerIdentifier>
				</CustomerInfo>
			</TravelItineraryAddInfoRQ>
		</xsl:if>
		<TravelItineraryAddInfoRQ Version="2.2.1" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<AgencyInfo>
				<xsl:if test="Address[@Type='Agency']/StreetNmbr!=''">
					<Address>
						<AddressLine>
							<xsl:choose>
								<xsl:when test="Address[@Type='Agency']/AddressLine!=''">
									<xsl:value-of select="Address[@Type='Agency']/AddressLine"/>
								</xsl:when>
								<xsl:otherwise>AGENCY ADDRESS</xsl:otherwise>
							</xsl:choose>
						</AddressLine>
						<CityName>
							<xsl:value-of select="Address[@Type='Agency']/CityName"/>
						</CityName>
						<CountryCode>
							<xsl:value-of select="Address[@Type='Agency']/CountryName/@Code"/>
						</CountryCode>
						<PostalCode>
							<xsl:value-of select="Address[@Type='Agency']/PostalCode"/>
						</PostalCode>
						<StateCountyProv>
							<xsl:attribute name="StateCode">
								<xsl:value-of select="Address[@Type='Agency']/StateProv/@StateCode"/>
							</xsl:attribute>
						</StateCountyProv>
						<StreetNmbr>
							<xsl:value-of select="translate(Address[@Type='Agency']/StreetNmbr,'#','')"/>
						</StreetNmbr>
					</Address>
				</xsl:if>
				<Ticketing>
					<xsl:variable name="tktlimit">
						<xsl:choose>
							<xsl:when test="Ticketing/@TicketTimeLimit!=''">
								<xsl:call-template name="if-midnight-fix-date">
									<xsl:with-param name="date" select="concat(substring(Ticketing/@TicketTimeLimit,6,8),':00')" />
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="if-midnight-fix-date">
									<xsl:with-param name="date" select="concat(substring(../../OTA_AirBookRQ/Ticketing/@TicketTimeLimit,6,8),':00')" />
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:attribute name="TicketType">7TAW</xsl:attribute>
					<xsl:if test="$tktlimit!=''">
						<xsl:attribute name="TicketTimeLimit">
							<xsl:value-of select="$tktlimit"/>
						</xsl:attribute>
					</xsl:if>
					<!--xsl:if test="(Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!='') and $PCC!='6XUF'">
						<xsl:choose>
							<xsl:when test="Queue/@QueueNumber!=''">
								<xsl:attribute name="PseudoCityCode"><xsl:value-of select="Queue/@PseudoCityCode"/></xsl:attribute>
								<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/></xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="PseudoCityCode"><xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/></xsl:attribute>
								<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/></xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if-->
				</Ticketing>
			</AgencyInfo>
			<CustomerInfo>
				<ContactNumbers>
					<xsl:apply-templates select="Telephone" mode="other" />
				</ContactNumbers>
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
				<Email>
					<xsl:attribute name="Address">
						<xsl:value-of select="Email"/>
					</xsl:attribute>
				</Email>
				<xsl:apply-templates select="Traveler"/>
				<!--xsl:for-each select="Traveler">
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
							<xsl:attribute name="Code"><xsl:value-of select="@PassengerTypeCode"/></xsl:attribute>
							<xsl:attribute name="NameNumber"><xsl:value-of select="$refnum"/><xsl:choose><xsl:when test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'"><xsl:text>.1</xsl:text></xsl:when><xsl:otherwise><xsl:text>.1</xsl:text></xsl:otherwise></xsl:choose></xsl:attribute>
						</PassengerType>
					</xsl:if>
				</xsl:for-each-->
			</CustomerInfo>
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
			<xsl:if test="@PassengerTypeCode != ''">
				<xsl:attribute name="PassengerType">
					<xsl:choose>
						<xsl:when test="@PassengerTypeCode='CHD'">C09</xsl:when>
						<xsl:when test="@PassengerTypeCode='JCB'">ADT</xsl:when>
						<xsl:when test="@PassengerTypeCode='ITF'">INF</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="@PassengerTypeCode"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="NameNumber">
					<xsl:value-of select="concat(TravelerRefNumber/@RPH,'.1')"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
				<xsl:attribute name="Infant">true</xsl:attribute>
			</xsl:if>
			<GivenName>
				<xsl:value-of select="PersonName/GivenName"/>
				<xsl:if test="PersonName/MiddleName != ''">
					<xsl:text> </xsl:text>
					<xsl:value-of select="PersonName/MiddleName"/>
				</xsl:if>
				<xsl:if test="PersonName/NamePrefix != ''">
					<xsl:text> </xsl:text>
					<xsl:value-of select="PersonName/NamePrefix"/>
				</xsl:if>
			</GivenName>
			<Surname>
				<xsl:value-of select="PersonName/Surname"/>
			</Surname>
		</PersonName>
	</xsl:template>
	<!--************************************************************-->
	<!--  			 Air Itinerary                                       -->
	<!--************************************************************-->
	<xsl:template match="OTA_AirBookRQ">
		<!--
		<OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.2.0">
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
		-->
		<OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.2.0">
			<OriginDestinationInformation>
				<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment" mode="new"/>
			</OriginDestinationInformation>
		</OTA_AirBookRQ>
	</xsl:template>
	<!--************************************************************-->
	<!--  			           Hotel                                       -->
	<!--************************************************************-->
	<xsl:template match="HotelReservation">
		<HotelAvail>
			<HotelPropertyDescriptionRQ Version="2.3.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
				<AvailRequestSegment>
					<GuestCounts Count="{RoomStays/RoomStay/GuestCounts/GuestCount/@Count}"/>
					<HotelSearchCriteria>
						<Criterion>
							<HotelRef HotelCode="{RoomStays/RoomStay/BasicPropertyInfo/@HotelCode}"/>
						</Criterion>
					</HotelSearchCriteria>
					<TimeSpan Start="{substring(RoomStays/RoomStay/TimeSpan/@Start,1,10)}" End="{substring(RoomStays/RoomStay/TimeSpan/@End,1,10)}"/>
				</AvailRequestSegment>
			</HotelPropertyDescriptionRQ>
		</HotelAvail>
		<HotelRes>
			<OTA_HotelResRQ ReturnHostCommand="true" Version="2.2.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
				<Hotel>
					<BasicPropertyInfo RPH="{RoomStays/RoomStay/RoomRates/RoomRate/@BookingCode}"/>
					<!--Customer NameNumber="1.1"/-->
					<Guarantee>
						<xsl:attribute name="Type">
							<xsl:choose>
								<xsl:when test="RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode!=''">G</xsl:when>
								<xsl:when test="RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode!=''">GDPST</xsl:when>
								<xsl:otherwise>GAGT</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode!=''">
								<CC_Info>
									<PaymentCard>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode='MC'">
													<xsl:value-of select="'CA'"/>
												</xsl:when>
												<xsl:when test="RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode='DS'">
													<xsl:value-of select="'DI'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:attribute name="ExpireDate">
											<xsl:value-of select="concat('20',substring(RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate,3,2),'-')"/>
											<xsl:value-of select="substring(RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate,1,2)"/>
										</xsl:attribute>
										<xsl:attribute name="Number">
											<xsl:value-of select="RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardNumber"/>
										</xsl:attribute>
									</PaymentCard>
									<PersonName>
										<Surname>
											<xsl:value-of select="substring-after(RoomStays/RoomStay/Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/CardHolderName,' ')"/>
										</Surname>
									</PersonName>
								</CC_Info>
							</xsl:when>
							<xsl:when test="RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode!=''">
								<CC_Info>
									<PaymentCard>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode='MC'">
													<xsl:value-of select="'CA'"/>
												</xsl:when>
												<xsl:when test="RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode='DS'">
													<xsl:value-of select="'DI'"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:attribute name="ExpireDate">
											<xsl:value-of select="concat('20',substring(RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@ExpireDate,3,2),'-')"/>
											<xsl:value-of select="substring(RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@ExpireDate,1,2)"/>
										</xsl:attribute>
										<xsl:attribute name="Number">
											<xsl:value-of select="RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardNumber"/>
										</xsl:attribute>
									</PaymentCard>
									<PersonName>
										<Surname>
											<xsl:value-of select="substring-after(RoomStays/RoomStay/DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/CardHolderName,' ')"/>
										</Surname>
									</PersonName>
								</CC_Info>
							</xsl:when>
						</xsl:choose>
					</Guarantee>
					<!--GuestCounts Count="{RoomStays/RoomStay/GuestCounts/GuestCount/@Count}"/-->
					<RoomType NumberOfUnits="{RoomStays/RoomStay/RoomRates/RoomRate/@NumberOfUnits}"/>
					<!--RoomType NumberOfUnits="{RoomStays/RoomStay/RoomRates/RoomRate/@NumberOfUnits}" RoomTypeCode="{RoomStays/RoomStay/RoomRates/RoomRate/@BookingCode}"/>
					<TimeSpan End="{substring(RoomStays/RoomStay/TimeSpan/@End,6,11)}" Start="{substring(RoomStays/RoomStay/TimeSpan/@Start,6,11)}"/-->
				</Hotel>
			</OTA_HotelResRQ>
		</HotelRes>
	</xsl:template>
	<!--************************************************************-->
	<!--  			               Car                                      -->
	<!--************************************************************-->
	<xsl:template match="VehResRQCore">
		<OTA_VehResRQ Version="2.1.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<VehResRQCore>
				<!--<xsl:if test="RateQualifier/@RateCategory!=''">
					<RateQualifier>
						<xsl:attribute name="RateCode"><xsl:value-of select="RateQualifier/@RateCategory"/></xsl:attribute>
						<xsl:if test="RateQualifier/@CorpDiscountNmbr != ''">
							<Corporate>
								<ID><xsl:value-of select="RateQualifier/@CorpDiscountNmbr"/></ID>
							</Corporate>
						</xsl:if>
						<xsl:if test="Customer/Primary/CustLoyalty">
							<xsl:copy-of select="Customer/Primary/CustLoyalty"/>
						</xsl:if>
					</RateQualifier>
				</xsl:if>
				<VehPrefs>
					<VehPref>
						<xsl:if test="VehPref/@AirConditionInd!=''">
							<xsl:attribute name="AirConditionInd"><xsl:value-of select="VehPref/@AirConditionInd"/></xsl:attribute>
						</xsl:if>
						<xsl:if test="VehPref/@TransmissionType!=''">
							<xsl:attribute name="TransmissionType"><xsl:value-of select="VehPref/@TransmissionType"/></xsl:attribute>
						</xsl:if>
						<VehType>
							<xsl:value-of select="VehPref/VehType/@VehicleCategory"/>
						</VehType>
					</VehPref>
				</VehPrefs>-->
				<xsl:if test="SpecialEquipPrefs/SpecialEquipPref">
					<SpecialEquipPrefs>
						<xsl:for-each select="SpecialEquipPrefs/SpecialEquipPref">
							<xsl:if test="@EquipType != ''">
								<SpecialEquipPref>
									<xsl:attribute name="EquipType">
										<xsl:value-of select="@EquipType"/>
									</xsl:attribute>
								</SpecialEquipPref>
							</xsl:if>
						</xsl:for-each>
					</SpecialEquipPrefs>
				</xsl:if>
				<VehRentalCore RPH="1">
					<!--<xsl:attribute name="PickUpDateTime"><xsl:value-of select="VehRentalCore/@PickUpDateTime"/></xsl:attribute>
					<xsl:attribute name="ReturnDateTime"><xsl:value-of select="VehRentalCore/@ReturnDateTime"/></xsl:attribute>
					<xsl:attribute name="Quantity"><xsl:value-of select="TPA_Extensions/CarData/@NumCars"/></xsl:attribute>
					<PickUpLocation>
						<xsl:attribute name="LocationCode"><xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode"/></xsl:attribute>
					</PickUpLocation>
					<xsl:if test="VehRentalCore/PickUpLocation/@LocationCode != VehRentalCore/ReturnLocation /@LocationCode">
						<ReturnLocation>
							<xsl:attribute name="LocationCode"><xsl:value-of select="VehRentalCore/ReturnLocation /@LocationCode"/></xsl:attribute>
						</ReturnLocation>
					</xsl:if>-->
				</VehRentalCore>
				<!--<VendorPrefs>
      		<VendorPref>
						<xsl:attribute name="Code"><xsl:value-of select="VendorPref/@Code"/></xsl:attribute>
					</VendorPref>
    		</VendorPrefs>-->
			</VehResRQCore>
		</OTA_VehResRQ>
	</xsl:template>

	<xsl:template match="VehResRQCore" mode="carAvail">

		<OTA_VehAvailRateRQ Version="2.4.1" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<VehAvailRQCore QueryType="Shop">
				<xsl:if test="RateQualifier/@RateCategory!=''">
					<RateQualifier>
						<xsl:attribute name="RateCode">
							<xsl:value-of select="RateQualifier/@RateCategory"/>
						</xsl:attribute>
						<xsl:if test="RateQualifier/@CorpDiscountNmbr != ''">
							<Corporate>
								<ID>
									<xsl:value-of select="RateQualifier/@CorpDiscountNmbr"/>
								</ID>
							</Corporate>
						</xsl:if>
						<xsl:if test="Customer/Primary/CustLoyalty">
							<xsl:copy-of select="Customer/Primary/CustLoyalty"/>
						</xsl:if>
					</RateQualifier>
				</xsl:if>
				<VehPrefs>
					<VehPref>
						<xsl:if test="VehPref/@AirConditionInd!=''">
							<xsl:attribute name="AirConditionInd">
								<xsl:value-of select="VehPref/@AirConditionInd"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="VehPref/@TransmissionType!=''">
							<xsl:attribute name="TransmissionType">
								<xsl:value-of select="VehPref/@TransmissionType"/>
							</xsl:attribute>
						</xsl:if>
						<VehType>
							<xsl:value-of select="VehPref/VehType/@VehicleCategory"/>
						</VehType>
					</VehPref>
				</VehPrefs>
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
					<xsl:if test="VehRentalCore/PickUpLocation/@LocationCode != VehRentalCore/ReturnLocation /@LocationCode">
						<ReturnLocation>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="VehRentalCore/ReturnLocation /@LocationCode"/>
							</xsl:attribute>
						</ReturnLocation>
					</xsl:if>
				</VehRentalCore>
				<VendorPrefs>
					<VendorPref>
						<xsl:attribute name="Code">
							<xsl:value-of select="VendorPref/@Code"/>
						</xsl:attribute>
					</VendorPref>
				</VendorPrefs>
			</VehAvailRQCore>
		</OTA_VehAvailRateRQ>
	</xsl:template>
	<xsl:template match="OriginDestinationOption">
		<OriginDestinationOption>
			<xsl:apply-templates select="FlightSegment"/>
		</OriginDestinationOption>
	</xsl:template>
	<xsl:template match="FlightSegment" mode="old">
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
			<xsl:if test="MarriageGrp!=''">
				<MarriageGrp>
					<xsl:attribute name="Ind">
						<xsl:value-of select="MarriageGrp"/>
					</xsl:attribute>
				</MarriageGrp>
			</xsl:if>
		</FlightSegment>
	</xsl:template>
	<xsl:template match="FlightSegment" mode="new">
		<FlightSegment>
			<xsl:attribute name="Status">NN</xsl:attribute>
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
			<DestinationLocation>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="ArrivalAirport/@LocationCode"/>
				</xsl:attribute>
			</DestinationLocation>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="MarketingAirline/@Code"/>
				</xsl:attribute>
				<xsl:attribute name="FlightNumber">
					<xsl:value-of select="@FlightNumber"/>
				</xsl:attribute>
			</MarketingAirline>
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

			<xsl:if test="MarriageGrp!=''">
				<MarriageGrp>
					<xsl:attribute name="Ind">
						<xsl:value-of select="MarriageGrp"/>
					</xsl:attribute>
				</MarriageGrp>
			</xsl:if>
			<OriginLocation>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="DepartureAirport/@LocationCode"/>
				</xsl:attribute>
			</OriginLocation>
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
		<xsl:if test="@MembershipID!=''">
			<CustLoyalty>
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
			<xsl:variable name="pID">
				<xsl:value-of select="@ProgramID"/>
			</xsl:variable>
			<xsl:variable name="mID">
				<xsl:value-of select="@MembershipID"/>
			</xsl:variable>
			<xsl:variable name="NN">
				<xsl:value-of select="$RefNumber"/>
				<xsl:text>.1</xsl:text>
			</xsl:variable>
			<xsl:for-each select="../../../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
				<xsl:if test="MarketingAirline/@Code!=$pID">
					<CustLoyalty>
						<xsl:attribute name="ProgramID">
							<xsl:value-of select="$pID"/>
						</xsl:attribute>
						<xsl:attribute name="MembershipID">
							<xsl:value-of select="$mID"/>
						</xsl:attribute>
						<xsl:attribute name="NameNumber">
							<xsl:value-of select="$NN"/>
						</xsl:attribute>
						<xsl:attribute name="TravelingCarrierCode">
							<xsl:value-of select="MarketingAirline/@Code"/>
						</xsl:attribute>
					</CustLoyalty>
				</xsl:if>
			</xsl:for-each>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Telephone" mode="other">
		<ContactNumber>
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
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="@AreaCityCode"/>
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Phone">
				<xsl:value-of select="@PhoneNumber"/>
			</xsl:attribute>
			<xsl:attribute name="NameNumber">
				<xsl:value-of select="concat(../Traveler/TravelerRefNumber/@RPH,'.1')"/>
			</xsl:attribute>
		</ContactNumber>
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
				<xsl:value-of select="@PhoneNumber"/>
			</xsl:attribute>
		</Telephone>
	</xsl:template>
	<xsl:template match="SeatRequest">
		<SeatPref>
			<xsl:attribute name="SeatPreference">
				<xsl:value-of select="@SeatPreference"/>
			</xsl:attribute>
		</SeatPref>
	</xsl:template>
	<xsl:template match="PriceData">
		<!-- https://files.developer.sabre.com/drc/servicedoc/OTA_AirPriceLLSRQ_v2.17.0_Design.xml -->
		<OTA_AirPriceRQ Version="2.17.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
			<PriceRequestInformation Retain="true">
				<OptionalQualifiers>
					<xsl:if test="@PricingInstruction!=''">
						<MiscQualifiers>
							<HemisphereCode>
								<xsl:value-of select="substring(substring-after(@PricingInstruction,'H'),1,1)"/>
							</HemisphereCode>
							<JourneyCode>
								<xsl:value-of select="substring(substring-after(@PricingInstruction,'J'),1,1)"/>
							</JourneyCode>
						</MiscQualifiers>
					</xsl:if>
					<PricingQualifiers>
						<xsl:if test="../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/@FareFamily!=''">
							<xsl:for-each select="../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
								<Brand>
									<xsl:attribute name="RPH">
										<xsl:value-of select="position()"/>
									</xsl:attribute>
									<xsl:value-of select="substring-before(substring-after(@FareFamily,'-'),'-')"/>
								</Brand>
							</xsl:for-each>
						</xsl:if>
						<ItineraryOptions>
							<xsl:for-each select="../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
								<SegmentSelect Number="{position()}" RPH="{position()}"/>
							</xsl:for-each>
						</ItineraryOptions>
						
							<xsl:for-each select="../../OTA_AirBookRQ/AirItineraryPricingInfo/PTC_FareBreakdowns/PTC_FareBreakdown/FareBasisCodes/FareBasisCode">
								<xsl:variable name="pos" select="position()" />
								<SpecificFare RPH="position()">
									<FareBasis>
										<xsl:value-of select="text()"/>
									</FareBasis>
								</SpecificFare>
							</xsl:for-each>
							
						<xsl:choose>
							<xsl:when test="PassengerTypeQuantity/@Code!=''">
								<xsl:for-each select="PassengerTypeQuantity">
									<PassengerType>
										<xsl:attribute name="Quantity">
											<xsl:value-of select="@Quantity"/>
										</xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="@CodeContext!=''">
													<xsl:value-of select="@CodeContext"/>
												</xsl:when>
												<xsl:when test="@Code='CHD'">C09</xsl:when>
												<xsl:when test="@Code!=''">
													<xsl:value-of select="@Code"/>
												</xsl:when>
												<xsl:otherwise>ADT</xsl:otherwise>
											</xsl:choose>
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
												<xsl:when test="@CodeContext='JCB'">JCB</xsl:when>
												<xsl:when test="@CodeContext='JNN'">JNN</xsl:when>
												<xsl:when test="@CodeContext='JNF'">JNF</xsl:when>
												<xsl:when test="../../PriceData/@PriceType='Private' and (@PassengerTypeCode='ADT' or not(@PassengerTypeCode)) and not(../../PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/@Code)">ADT</xsl:when>
												<xsl:when test="../../PriceData/@PriceType='Private' and @PassengerTypeCode='CHD' and not(../../PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/@Code)">JNN</xsl:when>
												<xsl:when test="../../PriceData/@PriceType='Private' and @PassengerTypeCode='INF' and not(../../PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/@Code)">JNF</xsl:when>
												<xsl:when test="../../PriceData/@PriceType='Private' and ../../PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode/@SecondaryCode='JCB'">JCB</xsl:when>
												<xsl:when test="@PassengerTypeCode='CHD'">C09</xsl:when>
												<xsl:when test="@PassengerTypeCode!=''">
													<xsl:value-of select="@PassengerTypeCode"/>
												</xsl:when>
												<xsl:otherwise>ADT</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PassengerType>
								</xsl:for-each>
							</xsl:otherwise>
						</xsl:choose>
					</PricingQualifiers>
				</OptionalQualifiers>
			</PriceRequestInformation>
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

	<xsl:template name="monthLong">
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
					<xsl:choose>
						<xsl:when test="@Vendor!=''">
							<xsl:value-of select="@Vendor"/>
						</xsl:when>
						<xsl:otherwise>1S</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Date">
					<xsl:value-of select="@Date"/>
				</xsl:attribute>
				<MiscLocation>
					<xsl:attribute name="LocationCode">
						MIA<!--xsl:value-of select="@LocationCode"/-->
					</xsl:attribute>
					<xsl:attribute name="CodeContext">IATA</xsl:attribute>
				</MiscLocation>
				<Text>
					<xsl:choose>
						<xsl:when test="Text!=''">
							<xsl:value-of select="Text"/>
						</xsl:when>
						<xsl:otherwise>MISCELLANEOUS</xsl:otherwise>
					</xsl:choose>
				</Text>
			</Segment>
		</MiscSegmentSellRQ>
	</xsl:template>
	<xsl:template name="if-midnight-fix-date">
		<xsl:param name="date" />
		<xsl:choose>
			<xsl:when test="$date =':00'" >
				<xsl:value-of select="''" />
			</xsl:when>
			<xsl:when test="substring($date,7,2)='00'" >
				<xsl:value-of select="concat(concat(substring($date,0,7),'23'), substring($date,9,3))" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$date" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
