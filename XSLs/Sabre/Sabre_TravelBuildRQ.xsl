<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Sabre_TravelBuildRQ.xsl 															       -->
	<!-- ================================================================== -->
	<!-- Date: 24 Jul 2012 - Rastko - mapped agency address							   -->
	<!-- Date: 23 Mar 2012 - Rastko - corrected ticketing time limit maping and added validating carrier   -->
	<!-- Date: 04 Jan 2011 - Rastko - acept both MI and M in SSR DOCS				       -->
	<!-- Date: 30 Dec 2010 - Rastko - changed SSR request to SSR requested			       -->
	<!-- Date: 18 Dec 2010 - Rastko - changed SSR request to be per pax and per segment	       -->
	<!-- Date: 08 Dec 2010 - Rastko - when CHD sent, send C09 to Sabre			       -->
	<!-- Date: 26 Sep 2010 - Rastko - fixed issue related TravelerRefNumber in CustLoyalty	       -->
	<!-- Date: 06 Sep 2010 - Rastko - fixed issue with SSR DOCS and INF			       -->
	<!-- Date: 03 Sep 2010 - Rastko - corrected seat and SSR processing			       -->
	<!-- Date: 01 Sep 2010 - Rastko - added mapping for seats							       -->
	<!-- Date: 31 Aug 2010 - Rastko - corrected marriage group element				       -->
	<!-- Date: 26 Aug 2010 - Rastko - added marriage element							       -->
	<!-- Date: 19 Aug 2010 - Rastko - added support for DOCS SSR					       -->
	<!-- Date: 30 Jul 2010 - Rastko - added commission mapping						       -->
	<!-- Date: 20 Apr 2010 - Rastko - enabled child birth date and passenger types		       -->
	<!-- Date: 13 May 2010 - Rastko - added check payment							       -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="PCC">
		<xsl:value-of select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
	</xsl:variable>
	<xsl:variable name="paxtype">
		<xsl:choose>
			<xsl:when test="$PCC = '9N60' or $PCC='EO8F'">Y</xsl:when>
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
		<xsl:if test="TPA_Extensions/PriceData">
			<Pricing>
				<xsl:apply-templates select="TPA_Extensions/PriceData"/>
			</Pricing>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address or OTA_AirBookRQ/Fulfillment/DeliveryAddress">
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
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
					<xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail">
						<xsl:choose>
							<xsl:when test="DirectBill/@DirectBill_ID='Check'">
								<FOPRemark Type="CHECK"/>
							</xsl:when>
							<xsl:otherwise>
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
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
				</AddRemarkRQ>
			</Remarks>
		</xsl:if>
		<xsl:if test=" OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks">
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
				<SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.4.1">
					<POS>
						<Source>
							<xsl:attribute name="PseudoCityCode">
								<xsl:value-of select="$PCC"/>
							</xsl:attribute>
						</Source>
					</POS>
					<xsl:for-each select="TPA_Extensions/PNRData/Traveler[substring(@PassengerTypeCode,1,1)='C']">
						<xsl:variable name="RPH">
							<xsl:value-of select="TravelerRefNumber/@RPH"/>
						</xsl:variable>
						<xsl:variable name="BD">
							<xsl:value-of select="@BirthDate"/>
						</xsl:variable>
						<xsl:for-each select="../../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
							<Service SSRCode="CHLD">
								<Airline>
									<xsl:attribute name="HostedCarrier">
										<xsl:choose>
											<xsl:when test="MarketingAirline/@Code='AA'">true</xsl:when>
											<xsl:otherwise>false</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</Airline>
								<TPA_Extensions>
									<Name>
										<xsl:attribute name="Number">
											<xsl:value-of select="$RPH"/>
											<xsl:text>.1</xsl:text>
										</xsl:attribute>
									</Name>
									<Segment>
										<xsl:attribute name="Number">
											<xsl:value-of select="@RPH"/>
										</xsl:attribute>
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
						<xsl:for-each select="../../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
							<Service SSRCode="INFT">
								<Airline>
									<xsl:attribute name="HostedCarrier">
										<xsl:choose>
											<xsl:when test="MarketingAirline/@Code='AA'">true</xsl:when>
											<xsl:otherwise>false</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</Airline>
								<TPA_Extensions>
									<Name>
										<xsl:attribute name="Number">1.1</xsl:attribute>
									</Name>
									<Segment>
										<xsl:attribute name="Number">
											<xsl:value-of select="@RPH"/>
										</xsl:attribute>
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
							</Service>
						</xsl:for-each>
					</xsl:for-each>
				</SpecialServiceRQ>
			</SpecialServicesCI>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest">
			<SpecialServicesSeat>
				<SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.4.1">
					<POS>
						<Source>
							<xsl:attribute name="PseudoCityCode">
								<xsl:value-of select="$PCC"/>
							</xsl:attribute>
						</Source>
					</POS>
					<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest">
						<xsl:call-template name="RPHPSeat">
							<xsl:with-param name="RPHP">
								<xsl:value-of select="@TravelerRefNumberRPHList"/>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:for-each>
				</SpecialServiceRQ>
			</SpecialServicesSeat>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest">
			<xsl:for-each select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest">
				<SpecialServicesSSR>
					<SpecialServiceRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.4.1">
						<POS>
							<Source>
								<xsl:attribute name="PseudoCityCode">
									<xsl:value-of select="$PCC"/>
								</xsl:attribute>
							</Source>
						</POS>
						<xsl:call-template name="RPHP">
							<xsl:with-param name="RPHP">
								<xsl:value-of select="@TravelerRefNumberRPHList"/>
							</xsl:with-param>
						</xsl:call-template>
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
										<xsl:otherwise>ORION</xsl:otherwise>
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
			<TravelItineraryReadRQ Version="2.0.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
				<!--<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>-->
				<MessagingDetails>
					<Transaction Code="PNR"/>
				</MessagingDetails>
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="UniqueID/@ID"/>
					</xsl:attribute>
					<!--TPA_Extensions>
				  <Transaction Code="WSR" /> 
		 		</TPA_Extensions-->
				</UniqueID>
			</TravelItineraryReadRQ>
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
												<xsl:when test="contains(Text,'-F-')">
													<xsl:value-of select="substring-before(Text,'-F-')"/>
													<xsl:value-of select="'-FI-'"/>
													<xsl:value-of select="substring-after(Text,'-F-')"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="Text"/>
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
				<Ticketing>
					<xsl:variable name="tktlimit">
						<xsl:choose>
							<xsl:when test="Ticketing/@TicketTimeLimit!=''">
								<xsl:value-of select="Ticketing/@TicketTimeLimit"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="../../OTA_AirBookRQ/Ticketing/@TicketTimeLimit"/>
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
							<xsl:attribute name="TicketTimeLimit">
								<xsl:value-of select="$tktlimit"/>
							</xsl:attribute>
							<xsl:attribute name="TicketingDate">
								<xsl:value-of select="$TktDate"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
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
		<!--
		<OTA_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode"><xsl:value-of select="$PCC"/></xsl:attribute>
				</Source>
			</POS>
			<AirItinerary>
				<xsl:attribute name="DirectionInd"><xsl:choose><xsl:when test="AirItinerary/@DirectionInd = 'Circle'">Return</xsl:when><xsl:when test="not(AirItinerary/@DirectionInd)"><xsl:choose><xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/DepartureAirport/@LocationCode = AirItinerary/OriginDestinationOptions/OriginDestinationOption[position()=last()]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode">Return</xsl:when><xsl:otherwise>Oneway</xsl:otherwise></xsl:choose></xsl:when><xsl:otherwise>Oneway</xsl:otherwise></xsl:choose></xsl:attribute>				
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
			<xsl:apply-templates select="FlightSegment" mode="new"/>
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
					<xsl:if test="@ValidatingAirlineCode!=''">
						<VendorPref>
							<xsl:attribute name="Code">
								<xsl:value-of select="@ValidatingAirlineCode"/>
							</xsl:attribute>
						</VendorPref>
					</xsl:if>
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
