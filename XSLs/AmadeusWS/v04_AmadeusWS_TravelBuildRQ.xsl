<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- ================================================================== -->
	<!-- v04_AmadeusWS_TravelBuildRQ.xsl 												       -->
	<!-- ================================================================== -->
	<!-- Date: 27 Jan 2014 - Rastko - corrected pricing time limit override process			-->
	<!-- Date:19 Oct 2013 - Rastko - added attribute SpecialInstruction				-->
	<!-- Date: 24 Apr 2013 - Suraj -  Un commented lines 2288-2294 (SourceOfBusiness)	 -->
	<!-- Date: 18 Apr 2013 - Rastko - corrected Mobile phone to send -M to Amadeus		 -->
	<!-- Date: 05 Apr 2013 - Suraj Added the CategorizedRemark -->
	<!-- Date: 22 Feb 2013 - Rastko - added support for supplemental info element in hotel sell	 -->
	<!-- Date: 08 Jan 2013 - Rastko - added support for multple pricing options			 -->
	<!-- Date: 09 Nov 2012 - Rastko - added phone formatting option					 -->
	<!-- Date: 13 Sep 2012 - Rastko - changed deposit code in hotel sell				 -->
	<!-- Date: 30 Aug 2012 - Rastko - removed test system check in hotel sell message		 -->
	<!-- Date: 10 Aug 2012 - Rastko - corrected missing Pricing tag for pricing booking		 -->
	<!-- Date: 08 Aug 2012 - Rastko -changed SSR FOID status to HK				 -->
	<!-- Date: 29 May 2012 - Shashin -changed SSR DOCA status to HK				 -->		
	<!-- Date: 28 Mar 2012 - Shashin - TicketAdvisory change to check 'OK'			 -->
	<!-- Date: 19 Mar 2012 - Rastko - added RU segment automatic addition for ATL1S2157 OID	 -->
	<!-- Date: 15 Mar 2012 - Rastko - changed pax association calculation in pricing cryptic entry	 -->
	<!-- Date: 03 Feb 2012 - Rastko - corrected FOP processing (CC was not processed)	       -->
	<!-- Date: 18 Jan 2012 - Shashin - EMD modification                                      		       -->
	<!-- Date: 21 Nov 2011 - Rastko - changed validating airline process to create FV element from it -->
	<!-- Date: 20 Oct 2011 - Rastko - corrected INF RPH mapping in remarks			       -->
	<!-- Date: 20 Oct 2011 - Rastko - corrected INF RPH mapping for SSR DOCS		       -->
	<!-- Date: 09 Jun 2011 - Shashin - mapped adult birth date when present			       -->
	<!-- Date: 03 Jun 2011 - Rastko - do not send validating carrier for Avianca			       -->
	<!-- Date: 24 May 2011 - Rastko - send FQTV if id and number present in request		       -->
	<!-- Date: 31 Jan 2011 - Rastko - do not send validating carrier for Air Mauritius		       -->
	<!-- Date: 05 Nov 2010 - Rastko - used cryptic entry to create SSR FQTV			       -->
	<!-- Date: 12 Oct 2010 - Rastko - added SecondaryCode for corporate fares pricing		       -->
	<!-- Date: 05 Oct 2010 - Rastko - new file												       -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="pcc" select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
	<xsl:variable name="username" select="OTA_TravelItineraryRQ/POS/TPA_Extensions/Provider/Userid"/>
	<xsl:template match="/">
		<TravelBuild>
			<xsl:apply-templates select="OTA_TravelItineraryRQ"/>
		</TravelBuild>
	</xsl:template>
	<!-- ************************************************************************************************************-->
	<xsl:template match="OTA_TravelItineraryRQ">
		<MasterPricer>
			<Air_SellFromRecommendation>
				<messageActionDetails>
					<messageFunctionDetails>
						<messageFunction>183</messageFunction>
						<additionalMessageFunction>M1</additionalMessageFunction>
					</messageFunctionDetails>
				</messageActionDetails>
				<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="MP"/>
			</Air_SellFromRecommendation>
		</MasterPricer>
		<MultiElements>
			<PNR_AddMultiElements>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<xsl:variable name="inf">
					<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'])"/>
				</xsl:variable>
				<xsl:variable name="chd">
					<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'CHD' or @PassengerTypeCode = 'INS' or @PassengerTypeCode = 'ITS' or @PassengerTypeCode = 'INN'])"/>
				</xsl:variable>
				<xsl:variable name="adt">
					<xsl:variable name="all">
						<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler)"/>
					</xsl:variable>
					<xsl:value-of select="$all - $inf - $chd"/>
				</xsl:variable>
				<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[(@PassengerTypeCode != 'INF' and @PassengerTypeCode != 'IN' and @PassengerTypeCode != 'INS' and @PassengerTypeCode != 'CHD' and @PassengerTypeCode != 'ITS' and @PassengerTypeCode != 'INN') or not(@PassengerTypeCode)]" mode="nameadt">
					<xsl:with-param name="adt">
						<xsl:value-of select="$adt"/>
					</xsl:with-param>
					<xsl:with-param name="inf">
						<xsl:value-of select="$inf"/>
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INS' or @PassengerTypeCode = 'CHD' or @PassengerTypeCode = 'ITF' or @PassengerTypeCode = 'INN']" mode="namechd">
					<xsl:with-param name="adt">
						<xsl:value-of select="$adt"/>
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:if test="OTA_AirBookRQ/MiscellaneousSegments/Segment or $pcc='ATL1S2157'">
					<originDestinationDetails>
						<originDestination />
						<xsl:choose>
							<xsl:when test="OTA_AirBookRQ/MiscellaneousSegments/Segment">
								<xsl:apply-templates select="OTA_AirBookRQ/MiscellaneousSegments/Segment" mode="Seg"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="createMisc"/>
							</xsl:otherwise>
						</xsl:choose>
					</originDestinationDetails>
				</xsl:if>				
				<dataElementsMaster>
					<marker1/>
					<xsl:apply-templates select="TPA_Extensions/PNRData/Telephone"/>
					<xsl:choose>
						<xsl:when test="OTA_AirBookRQ/Ticketing">
							<xsl:apply-templates select="OTA_AirBookRQ/Ticketing"/>
						</xsl:when>
						<xsl:when test="TPA_Extensions/PNRData/Ticketing">
							<xsl:apply-templates select="TPA_Extensions/PNRData/Ticketing"/>
						</xsl:when>
					</xsl:choose>
					<xsl:apply-templates select="POS/Source"/>
					<xsl:apply-templates select="TPA_Extensions/PNRData/Email"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest[@SSRCode!='']"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/CategorizedRemarks/CategorizedRemark"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='C']" mode="rc"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType!='C']" mode="rmk"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address" mode="billing"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/DirectBill/Address" mode="billing"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest[(@SeatPreference!='' and @SeatPreference!='0') or @SeatNumber!='']"/>
					<xsl:apply-templates select="TPA_Extensions/AgencyData/Commission"/>
					<xsl:apply-templates select="TPA_Extensions/PNRData/AccountingLine"/>
					<xsl:if test="TPA_Extensions/PriceData/@ValidatingAirlineCode != '' and $pcc!='MRUMK0101' and $pcc!='BOGAV08AZ' and count(TPA_Extensions/PriceData) =1">
						<dataElementsIndiv>
							<elementManagementData>
								<segmentName>FV</segmentName>
							</elementManagementData>
							<ticketingCarrier>
								<carrier>
									<airlineCode>
										<xsl:value-of select="TPA_Extensions/PriceData/@ValidatingAirlineCode"/>
									</airlineCode>
								</carrier>
							</ticketingCarrier>
						</dataElementsIndiv>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="TPA_Extensions/PNRData/Queue">
							<xsl:apply-templates select="TPA_Extensions/PNRData/Queue"/>
						</xsl:when>
						<xsl:when test="OTA_AirBookRQ/Queue">
							<xsl:apply-templates select="OTA_AirBookRQ/Queue"/>
						</xsl:when>
					</xsl:choose>
					<!--xsl:apply-templates select="TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '']" /-->
				</dataElementsMaster>
			</PNR_AddMultiElements>
		</MultiElements>
		<xsl:if test="TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '' and @MembershipID!='']">
			<FQTV>
				<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '' and @MembershipID!='']"/>
			</FQTV>
		</xsl:if>
		<xsl:if test="TPA_Extensions/PNRData/MCO">
			<MCO>
				<xsl:apply-templates select="TPA_Extensions/PNRData/MCO"/>
			</MCO>
		</xsl:if>
		<xsl:if test="OTA_AirBookRQ">
			<MCT>
				<Command_Cryptic>
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<textStringDetails>DMI</textStringDetails>
					</longTextString>
				</Command_Cryptic>
			</MCT>
		</xsl:if>
		<xsl:apply-templates select="OTA_VehResRQ/VehResRQCore"/>
		<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation"/>
		<xsl:apply-templates select="TPA_Extensions/PriceData"/>
		<StorePrice>
			<Ticket_CreateTSTFromPricing>
			</Ticket_CreateTSTFromPricing>
		</StorePrice>
		<xsl:if test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/MiscChargeOrder[@OriginalPaymentForm= 'MSCC']">
			<Exchange>
				<EXCH>
					<Command_Cryptic>
						<messageAction>
							<messageFunctionDetails>
								<messageFunction>M</messageFunction>
							</messageFunctionDetails>
						</messageAction>
						<longTextString>
							<textStringDetails>
								<xsl:text>TTK/EXCH/Tnn</xsl:text>
							</textStringDetails>
						</longTextString>
					</Command_Cryptic>
				</EXCH>
				<MSCC>
					<Command_Cryptic>
						<messageAction>
							<messageFunctionDetails>
								<messageFunction>M</messageFunction>
							</messageFunctionDetails>
						</messageAction>
						<longTextString>
							<textStringDetails>
								<xsl:text>FPO/MSCC</xsl:text>
							</textStringDetails>
						</longTextString>
					</Command_Cryptic>
				</MSCC>
			</Exchange>
		</xsl:if>
		<ET>
			<PNR_AddMultiElements>
				<pnrActions>
					<optionCode>10</optionCode>
					<optionCode>30</optionCode>
				</pnrActions>
			</PNR_AddMultiElements>
		</ET>
		<EPAY>
			<PNR_AddMultiElements>
				<pnrActions>
					<optionCode>10</optionCode>
				</pnrActions>
				<dataElementsMaster>
					<marker1/>
					<dataElementsIndiv>
						<elementManagementData>
							<segmentName>SSR</segmentName>
						</elementManagementData>
						<serviceRequest>
							<ssr>
								<type>EPAY</type>
								<status>NN</status>
								<quantity>
									<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler)"/>
								</quantity>
								<companyId>XX</companyId>
								<freetext>
									<xsl:text>CC/</xsl:text>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
									<xsl:text>/EXP</xsl:text>
									<xsl:value-of select="substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,1,2)"/>
									<xsl:text> </xsl:text>
									<xsl:value-of select="substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,3,2)"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="translate(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/CardHolderName,'/',' ')"/>
								</freetext>
							</ssr>
						</serviceRequest>
					</dataElementsIndiv>
				</dataElementsMaster>
			</PNR_AddMultiElements>
		</EPAY>
	</xsl:template>
	<xsl:template match="Traveler" mode="storeprice">
		<xsl:param name="pt"/>
		<xsl:variable name="pt1">
			<xsl:value-of select="$pt"/>
			<xsl:if test="not(contains($pt,@PassengerTypeCode))">
				<xsl:value-of select="@PassengerTypeCode"/>
			</xsl:if>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="following-sibling::Traveler[1]">
				<xsl:apply-templates select="following-sibling::Traveler[@PassengerTypeCode!='INF' or @PassengerTypeCode!='ITF'][1]" mode="storeprice">
					<xsl:with-param name="pt">
						<xsl:value-of select="$pt1"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$pt1"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Name Elements 							 -->
	<!---********************************************************************-->
	<xsl:template match="Traveler" mode="nameadt">
		<xsl:param name="adt"/>
		<xsl:param name="inf"/>
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<travellerInfo>
			<elementManagementPassenger>
				<reference>
					<qualifier>PR</qualifier>
					<number>
						<xsl:choose>
							<xsl:when test="TravelerRefNumber/@RPH != ''">
								<xsl:value-of select="TravelerRefNumber/@RPH"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$pos"/>
							</xsl:otherwise>
						</xsl:choose>
					</number>
				</reference>
				<segmentName>NM</segmentName>
			</elementManagementPassenger>
			<passengerData>
				<travellerInformation>
					<traveller>
						<surname>
							<xsl:value-of select="PersonName/Surname"/>
						</surname>
						<quantity>
							<xsl:choose>
								<xsl:when test="$pos > $inf">1</xsl:when>
								<xsl:otherwise>2</xsl:otherwise>
							</xsl:choose>
						</quantity>
					</traveller>
					<passenger>
						<firstName>
							<xsl:value-of select="PersonName/GivenName"/>
							<xsl:if test="PersonName/MiddleName !=''">
								<xsl:value-of select="concat(' ',PersonName/MiddleName)"/>
							</xsl:if>
							<xsl:if test="PersonName/NamePrefix !=''">
								<xsl:value-of select="concat(' ',PersonName/NamePrefix)"/>
							</xsl:if>
						</firstName>
						<type>
							<xsl:choose>
								<xsl:when test="@PassengerTypeCode='SRC'">YCD</xsl:when>
								<xsl:when test="@PassengerTypeCode='SCR'">YCD</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@PassengerTypeCode"/>
								</xsl:otherwise>
							</xsl:choose>
						</type>
						<xsl:if test="$pos &lt; $inf or $pos = $inf">
							<infantIndicator>3</infantIndicator>
						</xsl:if>
					</passenger>
				</travellerInformation>
				<xsl:if test="@BirthDate!=''">
					<dateOfBirth>
						<dateAndTimeDetails>
							<qualifier>706</qualifier>
							<date>
								<xsl:value-of select="substring(@BirthDate,9,2)"/>
								<xsl:value-of select="substring(@BirthDate,6,2)"/>
								<xsl:value-of select="substring(@BirthDate,1,4)"/>
							</date>
						</dateAndTimeDetails>
					</dateOfBirth>
				</xsl:if>
			</passengerData>
			<xsl:if test="$pos &lt; $inf or $pos = $inf">
				<passengerData>
					<travellerInformation>
						<traveller>
							<surname>
								<xsl:value-of select="following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/PersonName/Surname"/>
							</surname>
						</traveller>
						<passenger>
							<firstName>
								<xsl:value-of select="following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/PersonName/GivenName"/>
							</firstName>
							<!--type><xsl:value-of select="following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/@PassengerTypeCode"/></type-->
							<type>INF</type>
						</passenger>
					</travellerInformation>
					<xsl:if test="following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/@BirthDate!=''">
						<dateOfBirth>
							<dateAndTimeDetails>
								<qualifier>706</qualifier>
								<date>
									<xsl:value-of select="substring(following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/@BirthDate,9,2)"/>
									<xsl:value-of select="substring(following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/@BirthDate,6,2)"/>
									<xsl:value-of select="substring(following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/@BirthDate,1,4)"/>
								</date>
							</dateAndTimeDetails>
						</dateOfBirth>
					</xsl:if>
				</passengerData>
			</xsl:if>
		</travellerInfo>
	</xsl:template>
	<xsl:template match="Traveler" mode="namechd">
		<xsl:param name="adt"/>
		<xsl:variable name="pos1">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="pos">
			<xsl:value-of select="$pos1 + $adt"/>
		</xsl:variable>
		<travellerInfo>
			<elementManagementPassenger>
				<reference>
					<qualifier>PR</qualifier>
					<number>
						<xsl:choose>
							<xsl:when test="TravelerRefNumber/@RPH != ''">
								<xsl:value-of select="TravelerRefNumber/@RPH"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$pos"/>
							</xsl:otherwise>
						</xsl:choose>
					</number>
				</reference>
				<segmentName>NM</segmentName>
			</elementManagementPassenger>
			<passengerData>
				<travellerInformation>
					<traveller>
						<surname>
							<xsl:value-of select="PersonName/Surname"/>
						</surname>
						<quantity>1</quantity>
					</traveller>
					<passenger>
						<firstName>
							<xsl:value-of select="PersonName/GivenName"/>
						</firstName>
						<type>
							<xsl:choose>
								<xsl:when test="@PassengerTypeCode = 'CH'">CHD</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@PassengerTypeCode"/>
								</xsl:otherwise>
							</xsl:choose>
						</type>
					</passenger>
				</travellerInformation>
				<xsl:if test="@BirthDate!='' and @PassengerTypeCode!='INS'">
					<dateOfBirth>
						<dateAndTimeDetails>
							<qualifier>706</qualifier>
							<date>
								<xsl:value-of select="substring(@BirthDate,9,2)"/>
								<xsl:value-of select="substring(@BirthDate,6,2)"/>
								<xsl:value-of select="substring(@BirthDate,1,4)"/>
							</date>
						</dateAndTimeDetails>
					</dateOfBirth>
				</xsl:if>
			</passengerData>
		</travellerInfo>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group - 					 -->
	<!---********************************************************************-->
	<xsl:template match="FlightSegment" mode="MP">
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(@DepartureDateTime,'T')"/>
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)"/>
		</xsl:variable>
		<xsl:variable name="ArrTime">
			<xsl:value-of select="substring-after(@ArrivalDateTime,'T')"/>
		</xsl:variable>
		<xsl:variable name="ArrTime2">
			<xsl:value-of select="substring(string($ArrTime),1,5)"/>
		</xsl:variable>
		<segmentInformation>
			<travelProductInformation>
				<flightDate>
					<departureDate>
						<xsl:value-of select="substring(string(@DepartureDateTime),9,2)"/>
						<xsl:value-of select="substring(string(@DepartureDateTime),6,2)"/>
						<xsl:value-of select="substring(string(@DepartureDateTime),3,2)"/>
					</departureDate>
				</flightDate>
				<boardPointDetails>
					<trueLocationId>
						<xsl:value-of select="DepartureAirport/@LocationCode"/>
					</trueLocationId>
				</boardPointDetails>
				<offpointDetails>
					<trueLocationId>
						<xsl:value-of select="ArrivalAirport/@LocationCode"/>
					</trueLocationId>
				</offpointDetails>
				<companyDetails>
					<marketingCompany>
						<xsl:value-of select="MarketingAirline/@Code"/>
					</marketingCompany>
				</companyDetails>
				<flightIdentification>
					<flightNumber>
						<xsl:value-of select="@FlightNumber"/>
					</flightNumber>
					<bookingClass>
						<xsl:value-of select="@ResBookDesigCode"/>
					</bookingClass>
				</flightIdentification>
				<xsl:if test="@FlightContext!=''">
					<flightTypeDetails>
						<flightIndicator>
							<xsl:value-of select="@FlightContext"/>
						</flightIndicator>
					</flightTypeDetails>
				</xsl:if>
			</travelProductInformation>
			<relatedproductInformation>
				<quantity>
					<xsl:value-of select="@NumberInParty"/>
				</quantity>
				<statusCode>NN</statusCode>
			</relatedproductInformation>
		</segmentInformation>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group - Master Pricer						 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationOption" mode="MP">
		<itineraryDetails>
			<originDestinationDetails>
				<origin>
					<xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode"/>
				</origin>
				<destination>
					<xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
				</destination>
			</originDestinationDetails>
			<message>
				<messageFunctionDetails>
					<messageFunction>183</messageFunction>
				</messageFunctionDetails>
			</message>
			<xsl:apply-templates select="FlightSegment" mode="MP"/>
		</itineraryDetails>
	</xsl:template>
	<!---**********************************************************************************-->
	<!--     Data Elements 					                    	       -->
	<!---**********************************************************************************-->
	<!--         		        Phone fields				 	       -->
	<!---**********************************************************************************-->
	<xsl:template match="Telephone">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>AP</segmentName>
			</elementManagementData>
			<xsl:choose>
				<xsl:when test="@FormattedInd='true'">
					<freetextData>
						<freetextDetail>
							<subjectQualifier>3</subjectQualifier>
						</freetextDetail>
						<xsl:if test="@PhoneNumber!=''">
							<longFreetext>
								<xsl:choose>
									<xsl:when test="@AreaCityCode!=''">
										<xsl:value-of select="concat(@AreaCityCode,' ',@CountryAccessCode,@PhoneNumber)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat(@CountryAccessCode,@PhoneNumber)"/>
									</xsl:otherwise>
								</xsl:choose>
								<xsl:choose>
									<xsl:when test="@PhoneLocationType='Business'">
										<xsl:value-of select="'-B'"/>
									</xsl:when>
									<xsl:when test="@PhoneLocationType='Mobile'">
										<xsl:value-of select="'-M'"/>
									</xsl:when>
									<xsl:when test="@PhoneLocationType='Home'">
										<xsl:value-of select="'-H'"/>
									</xsl:when>
									<xsl:when test="@PhoneLocationType='Agency'">
										<xsl:value-of select="'-A'"/>
									</xsl:when>
									<xsl:when test="@PhoneLocationType='Fax'">
										<xsl:value-of select="'-F'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'-O'"/>
									</xsl:otherwise>
								</xsl:choose>
							</longFreetext>
						</xsl:if>
					</freetextData>
				</xsl:when>
				<xsl:otherwise>
					<freetextData>
						<freetextDetail>
							<subjectQualifier>3</subjectQualifier>
							<xsl:choose>
								<xsl:when test="@PhoneLocationType='Business'">
									<type>3</type>
								</xsl:when>
								<xsl:when test="@PhoneLocationType='Home'">
									<type>4</type>
								</xsl:when>
								<xsl:when test="@PhoneLocationType='Agency'">
									<type>6</type>
								</xsl:when>
								<xsl:when test="@PhoneLocationType='Mobile'">
									<type>7</type>
								</xsl:when>
								<xsl:when test="@PhoneLocationType='Fax'">
									<type>P01</type>
								</xsl:when>
								<xsl:otherwise>
									<type>5</type>
								</xsl:otherwise>
							</xsl:choose>
						</freetextDetail>
						<longFreetext>
							<xsl:value-of select="@AreaCityCode"/>
							<xsl:value-of select="@PhoneNumber"/>
						</longFreetext>
					</freetextData>
				</xsl:otherwise>
			</xsl:choose>
		</dataElementsIndiv>
	</xsl:template>
	<!---**********************************************************************************-->
	<!--     Data Elements 					                    	       -->
	<!---**********************************************************************************-->
	<!--         		        email fields				 	       -->
	<!---**********************************************************************************-->
	<xsl:template match="Email">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>AP</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
					<type>P02</type>
				</freetextDetail>
				<longFreetext>
					<xsl:value-of select="."/>
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       Ticketing Fields			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Ticketing">
		<xsl:variable name="TktTime">
			<xsl:value-of select="substring-after(@TicketTimeLimit,'T')"/>
		</xsl:variable>
		<xsl:variable name="TktTime2">
			<xsl:value-of select="substring(string($TktTime),1,5)"/>
		</xsl:variable>
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>TK</segmentName>
			</elementManagementData>
			<ticketElement>
				<ticket>
					<xsl:choose>
						<xsl:when test="TicketAdvisory = 'OK'">
							<indicator>OK</indicator>
						</xsl:when>
						<xsl:when test="@TicketTimeLimit">
							<indicator>
								<xsl:choose>
									<xsl:when test="TicketAdvisory = 'XL'">XL</xsl:when>
									<xsl:when test="TicketAdvisory = 'TL'">TL</xsl:when>
									<xsl:otherwise>TL</xsl:otherwise>
								</xsl:choose>
							</indicator>
							<date>
								<xsl:value-of select="substring(string(@TicketTimeLimit),9,2)"/>
								<xsl:value-of select="substring(string(@TicketTimeLimit),6,2)"/>
								<xsl:value-of select="substring(string(@TicketTimeLimit),3,2)"/>
							</date>
							<xsl:choose>
								<xsl:when test="$TktTime2 = '0000'">
									<time>
										<xsl:text>2400</xsl:text>
									</time>
								</xsl:when>
								<xsl:otherwise>
									<time>
										<xsl:value-of select="translate(string($TktTime2),':','')"/>
									</time>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
					</xsl:choose>
				</ticket>
			</ticketElement>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       SSR			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="SpecialServiceRequest">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>SSR</segmentName>
			</elementManagementData>
			<serviceRequest>
				<ssr>
					<type>
						<xsl:value-of select="@SSRCode"/>
					</type>
					<status>
						<xsl:choose>
							<xsl:when test="@SSRCode = 'DOCS' or @SSRCode = 'DOCO' or @SSRCode = 'DOCA' or @SSRCode = 'FOID' or @SSRCode = 'CLID'">HK</xsl:when>
							<xsl:otherwise>NN</xsl:otherwise>
						</xsl:choose>
					</status>
					<xsl:if test="@SSRCode!='CLID'">
						<quantity>
							<xsl:choose>
								<xsl:when test="@TravelerRefNumberRPHList!=''">
									<xsl:value-of select="string-length(@TravelerRefNumberRPHList)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="count(../../../../../TPA_Extensions/PNRData/Traveler)"/>
								</xsl:otherwise>
							</xsl:choose>
						</quantity>
					</xsl:if>
					<companyId>
						<xsl:choose>
							<xsl:when test="Airline/@Code!=''">
								<xsl:value-of select="Airline/@Code"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'YY'"/>
							</xsl:otherwise>
						</xsl:choose>
					</companyId>
					<xsl:if test="Text!=''">
						<freetext>
							<xsl:value-of select="Text"/>
						</freetext>
					</xsl:if>
				</ssr>
			</serviceRequest>
			<xsl:if test="@TravelerRefNumberRPHList != '' or @FlightRefNumberRPHList != ''">
				<referenceForDataElement>
					<xsl:if test="@TravelerRefNumberRPHList != ''">
						<xsl:variable name="rph">
							<xsl:value-of select="@TravelerRefNumberRPHList"/>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="string-length(@TravelerRefNumberRPHList)=1 and ../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">
								<xsl:variable name="infrph">
									<xsl:for-each select=" ../../../../../TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='INF']">
										<xsl:if test="TravelerRefNumber/@RPH=$rph">
											<xsl:value-of select="position()"/>
										</xsl:if>
									</xsl:for-each>
								</xsl:variable>
								<xsl:call-template name="RPH">
									<xsl:with-param name="RPH">
										<xsl:value-of select="$infrph"/>
									</xsl:with-param>
									<xsl:with-param name="Type">
										<xsl:text>P</xsl:text>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="RPH">
									<xsl:with-param name="RPH">
										<xsl:value-of select="@TravelerRefNumberRPHList"/>
									</xsl:with-param>
									<xsl:with-param name="Type">
										<xsl:text>P</xsl:text>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
					<xsl:if test="@FlightRefNumberRPHList != '' and Airline/@Code!='YY'">
						<xsl:call-template name="RPH">
							<xsl:with-param name="RPH">
								<xsl:value-of select="@FlightRefNumberRPHList"/>
							</xsl:with-param>
							<xsl:with-param name="Type">
								<xsl:text>S</xsl:text>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</referenceForDataElement>
			</xsl:if>
		</dataElementsIndiv>
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
					<xsl:choose>
						<xsl:when test="$Type='S'">T</xsl:when>
						<xsl:otherwise>R</xsl:otherwise>
					</xsl:choose>
				</qualifier>
				<number>
					<xsl:value-of select="$tRPH"/>
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
	<!-- *********************************************************************** -->
	<!--         		       Seat request			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="SeatRequest">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>STR</segmentName>
			</elementManagementData>
			<seatGroup>
				<seatRequest>
					<xsl:if test="@SeatNumber = '' and @SeatPreference=''">
						<seat>
							<type>RQST</type>
						</seat>
					</xsl:if>
					<xsl:if test="@SeatNumber != '' or @SeatPreference != ''">
						<special>
							<xsl:if test="@SeatNumber != ''">
								<data>
									<xsl:value-of select="@SeatNumber"/>
								</data>
							</xsl:if>
							<xsl:if test="@SeatPreference != ''">
								<seatType>
									<xsl:value-of select="@SeatPreference"/>
								</seatType>
							</xsl:if>
						</special>
					</xsl:if>
				</seatRequest>
			</seatGroup>
			<xsl:if test="@TravelerRefNumberRPHList != '' or @FlightRefNumberRPHList != ''">
				<referenceForDataElement>
					<xsl:if test="@TravelerRefNumberRPHList != ''">
						<xsl:call-template name="RPH">
							<xsl:with-param name="RPH">
								<xsl:value-of select="@TravelerRefNumberRPHList"/>
							</xsl:with-param>
							<xsl:with-param name="Type">
								<xsl:text>P</xsl:text>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
					<xsl:if test="@FlightRefNumberRPHList != ''">
						<xsl:call-template name="RPH">
							<xsl:with-param name="RPH">
								<xsl:value-of select="@FlightRefNumberRPHList"/>
							</xsl:with-param>
							<xsl:with-param name="Type">
								<xsl:text>S</xsl:text>
							</xsl:with-param>
						</xsl:call-template>
					</xsl:if>
				</referenceForDataElement>
			</xsl:if>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       OSI			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="OtherServiceInformation">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>OS</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
					<type>28</type>
					<companyId>
						<xsl:value-of select="Airline/@Code"/>
					</companyId>
				</freetextDetail>
				<longFreetext>
					<xsl:value-of select="Text"/>
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       General remark			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Remark">
		<xsl:if test=". != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>RM</segmentName>
				</elementManagementData>
				<miscellaneousRemark>
					<remarks>
						<type>RM</type>
						<freetext>
							<xsl:choose>
								<xsl:when test="contains(.,'MORQUA')">
									<xsl:value-of select="substring-before(.,'MORQUA')"/>
									<xsl:value-of select="'AWAD'"/>
									<xsl:value-of select="substring-after(.,'MORQUA')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="."/>
								</xsl:otherwise>
							</xsl:choose>
						</freetext>
					</remarks>
				</miscellaneousRemark>
			</dataElementsIndiv>
			<xsl:if test="$username='morqua' and contains(.,'Total Markup')">
				<xsl:variable name="paxnum">
					<xsl:value-of select="count(../../../../../TPA_Extensions/PNRData/Traveler)"/>
				</xsl:variable>
				<xsl:variable name="markup">
					<xsl:value-of select="substring-before(substring-after(.,' '),' ')"/>
				</xsl:variable>
				<xsl:variable name="perPaxMarkup">
					<xsl:value-of select="$markup div $paxnum"/>
				</xsl:variable>
				<xsl:for-each select="../../../../../TPA_Extensions/PNRData/Traveler">
					<dataElementsIndiv>
						<elementManagementData>
							<segmentName>RM</segmentName>
						</elementManagementData>
						<miscellaneousRemark>
							<remarks>
								<type>RM</type>
								<freetext>
									<xsl:value-of select="concat('*THR** ',format-number($perPaxMarkup,'0.00'),' MARKUP PER PERSON WAS APPLIED.')"/>
								</freetext>
							</remarks>
						</miscellaneousRemark>
						<referenceForDataElement>
							<xsl:choose>
								<xsl:when test="@PassengerTypeCode='INF'">
									<xsl:variable name="rph">
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:variable>
									<xsl:variable name="infrph">
										<xsl:for-each select=" ../Traveler[@PassengerTypeCode='INF']">
											<xsl:if test="TravelerRefNumber/@RPH=$rph">
												<xsl:value-of select="position()"/>
											</xsl:if>
										</xsl:for-each>
									</xsl:variable>
									<xsl:call-template name="RPH">
										<xsl:with-param name="RPH">
											<xsl:value-of select="$infrph"/>
										</xsl:with-param>
										<xsl:with-param name="Type">
											<xsl:text>P</xsl:text>
										</xsl:with-param>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="RPH">
										<xsl:with-param name="RPH">
											<xsl:value-of select="TravelerRefNumber/@RPH"/>
										</xsl:with-param>
										<xsl:with-param name="Type">
											<xsl:text>P</xsl:text>
										</xsl:with-param>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</referenceForDataElement>
					</dataElementsIndiv>
				</xsl:for-each>
			</xsl:if>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       Categorized remark			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="CategorizedRemark ">
		<xsl:if test=". != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>RM</segmentName>
				</elementManagementData>
				<miscellaneousRemark>
					<remarks>
						<type>RM</type>
						<xsl:if test="@Category!= ''">
							<category>
								<xsl:value-of select="@Category"/>
							</category>
						</xsl:if>
						<freetext>
							<xsl:value-of select="."/>
							<!--<xsl:choose>
								<xsl:when test="contains(.,'MORQUA')">
									<xsl:value-of select="substring-before(.,'MORQUA')"/>
									<xsl:value-of select="'AWAD'"/>
									<xsl:value-of select="substring-after(.,'MORQUA')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="."/>
								</xsl:otherwise>
							</xsl:choose>-->
						</freetext>
					</remarks>
				</miscellaneousRemark>
			</dataElementsIndiv>
			<!--<xsl:if test="$username='morqua' and contains(.,'Total Markup')">
				<xsl:variable name="paxnum">
					<xsl:value-of select="count(../../../../../TPA_Extensions/PNRData/Traveler)"/>
				</xsl:variable>
				<xsl:variable name="markup">
					<xsl:value-of select="substring-before(substring-after(.,' '),' ')"/>
				</xsl:variable>
				<xsl:variable name="perPaxMarkup">
					<xsl:value-of select="$markup div $paxnum"/>
				</xsl:variable>
				<xsl:for-each select="../../../../../TPA_Extensions/PNRData/Traveler">
					<dataElementsIndiv>
						<elementManagementData>
							<segmentName>RM</segmentName>
						</elementManagementData>
						<miscellaneousRemark>
							<remarks>
								<type>RM</type>
								<freetext>
									<xsl:value-of select="concat('*THR** ',format-number($perPaxMarkup,'0.00'),' MARKUP PER PERSON WAS APPLIED.')"/>
								</freetext>
							</remarks>
						</miscellaneousRemark>
						<referenceForDataElement>
							<xsl:choose>
								<xsl:when test="@PassengerTypeCode='INF'">
									<xsl:variable name="rph">
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:variable>
									<xsl:variable name="infrph">
										<xsl:for-each select=" ../Traveler[@PassengerTypeCode='INF']">
											<xsl:if test="TravelerRefNumber/@RPH=$rph">
												<xsl:value-of select="position()"/>
											</xsl:if>
										</xsl:for-each>
									</xsl:variable>
									<xsl:call-template name="RPH">
										<xsl:with-param name="RPH">
											<xsl:value-of select="$infrph"/>
										</xsl:with-param>
										<xsl:with-param name="Type">
											<xsl:text>P</xsl:text>
										</xsl:with-param>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="RPH">
										<xsl:with-param name="RPH">
											<xsl:value-of select="TravelerRefNumber/@RPH"/>
										</xsl:with-param>
										<xsl:with-param name="Type">
											<xsl:text>P</xsl:text>
										</xsl:with-param>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</referenceForDataElement>
					</dataElementsIndiv>
				</xsl:for-each>
			</xsl:if>-->
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       Special confidential remark		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="SpecialRemark" mode="rc">
		<xsl:if test=". != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>RC</segmentName>
				</elementManagementData>
				<miscellaneousRemark>
					<remarks>
						<type>RC</type>
						<freetext>
							<xsl:choose>
								<xsl:when test="contains(Text,'//')">
									<xsl:value-of select="substring-after(Text,'//')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Text"/>
								</xsl:otherwise>
							</xsl:choose>
						</freetext>
						<xsl:if test="contains(Text,'//')">
							<xsl:variable name="offid">
								<xsl:value-of select="substring-before(Text,'//')"/>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="contains($offid,',')">
									<officeId>
										<xsl:value-of select="substring-before($offid,',')"/>
									</officeId>
									<xsl:variable name="offid1">
										<xsl:value-of select="substring-after($offid,',')"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="contains($offid1,',')">
											<officeId>
												<xsl:value-of select="substring-before($offid1,',')"/>
											</officeId>
											<officeId>
												<xsl:value-of select="substring-after($offid1,',')"/>
											</officeId>
										</xsl:when>
										<xsl:otherwise>
											<officeId>
												<xsl:value-of select="$offid1"/>
											</officeId>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<officeId>
										<xsl:value-of select="$offid"/>
									</officeId>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</remarks>
				</miscellaneousRemark>
				<xsl:if test="@TravelerRefNumber != '' or @FlightRefNumber != ''">
					<referenceForDataElement>
						<xsl:if test="@TravelerRefNumber != ''">
							<xsl:call-template name="RPH">
								<xsl:with-param name="RPH">
									<xsl:value-of select="@TravelerRefNumber"/>
								</xsl:with-param>
								<xsl:with-param name="Type">
									<xsl:text>P</xsl:text>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
						<xsl:if test="@FlightRefNumber != ''">
							<xsl:call-template name="RPH">
								<xsl:with-param name="RPH">
									<xsl:value-of select="@FlightRefNumber"/>
								</xsl:with-param>
								<xsl:with-param name="Type">
									<xsl:text>S</xsl:text>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
					</referenceForDataElement>
				</xsl:if>
			</dataElementsIndiv>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		       Special remark			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="SpecialRemark" mode="rmk">
		<xsl:if test=". != ''">
			<dataElementsIndiv>
				<xsl:choose>
					<xsl:when test="@RemarkType = 'Endorsement'">
						<elementManagementData>
							<segmentName>FE</segmentName>
						</elementManagementData>
						<fareElement>
							<generalIndicator>E</generalIndicator>
							<xsl:if test="TravelerRefNumber/@RPH!=''">
								<passengerType>
									<xsl:variable name="rph">
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">INF</xsl:when>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='ITF'">INF</xsl:when>
										<xsl:otherwise>PAX</xsl:otherwise>
									</xsl:choose>
								</passengerType>
							</xsl:if>
							<freetextLong>
								<xsl:value-of select="Text"/>
							</freetextLong>
						</fareElement>
					</xsl:when>
					<xsl:when test="@RemarkType = 'TourCode'">
						<elementManagementData>
							<segmentName>FT</segmentName>
						</elementManagementData>
						<tourCode>
							<xsl:if test="TravelerRefNumber/@RPH!=''">
								<passengerType>
									<xsl:variable name="rph">
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">INF</xsl:when>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='ITF'">INF</xsl:when>
										<xsl:otherwise>PAX</xsl:otherwise>
									</xsl:choose>
								</passengerType>
							</xsl:if>
							<freeFormatTour>
								<indicator>FF</indicator>
								<freetext>
									<xsl:value-of select="Text"/>
								</freetext>
							</freeFormatTour>
						</tourCode>
					</xsl:when>
					<xsl:when test="@RemarkType = 'Cryptic'">
						<elementManagementData>
							<segmentName>RM</segmentName>
						</elementManagementData>
						<miscellaneousRemark>
							<remarks>
								<type>RM</type>
								<category>*</category>
								<freetext>
									<xsl:value-of select="Text"/>
								</freetext>
							</remarks>
						</miscellaneousRemark>
					</xsl:when>
					<xsl:when test="@RemarkType = 'ValidatingCarrier'">
						<elementManagementData>
							<segmentName>FV</segmentName>
						</elementManagementData>
						<ticketingCarrier>
							<xsl:if test="TravelerRefNumber/@RPH!=''">
								<passengerType>
									<xsl:variable name="rph">
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">INF</xsl:when>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='ITF'">INF</xsl:when>
										<xsl:otherwise>PAX</xsl:otherwise>
									</xsl:choose>
								</passengerType>
							</xsl:if>
							<carrier>
								<airlineCode>
									<xsl:value-of select="Text"/>
								</airlineCode>
							</carrier>
						</ticketingCarrier>
					</xsl:when>
					<xsl:when test="@RemarkType = 'ManualDocument' or @RemarkType = 'AutomatedTicket' or @RemarkType = 'ElectronicTicket' or @RemarkType = 'ManualTicket'">
						<elementManagementData>
							<segmentName>
								<xsl:choose>
									<xsl:when test="@RemarkType = 'ManualDocument'">FH</xsl:when>
									<xsl:when test="@RemarkType = 'AutomatedTicket'">FHA</xsl:when>
									<xsl:when test="@RemarkType = 'ElectronicTicket'">FHE</xsl:when>
									<xsl:when test="@RemarkType = 'ManualTicket'">FHM</xsl:when>
								</xsl:choose>
							</segmentName>
						</elementManagementData>
						<manualFareDocument>
							<xsl:if test="TravelerRefNumber/@RPH!=''">
								<passengerType>
									<xsl:variable name="rph">
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">INF</xsl:when>
										<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='ITF'">INF</xsl:when>
										<xsl:otherwise>PAX</xsl:otherwise>
									</xsl:choose>
								</passengerType>
							</xsl:if>
							<document>
								<companyId>
									<xsl:value-of select="substring-before(Text,'-')"/>
								</companyId>
								<ticketNumber>
									<xsl:value-of select="substring-after(Text,'-')"/>
								</ticketNumber>
							</document>
						</manualFareDocument>
					</xsl:when>
					<xsl:otherwise>
						<elementManagementData>
							<segmentName>RI</segmentName>
						</elementManagementData>
						<miscellaneousRemark>
							<remarks>
								<type>RI</type>
								<category>
									<xsl:value-of select="@RemarkType"/>
								</category>
								<freetext>
									<xsl:value-of select="Text"/>
								</freetext>
							</remarks>
						</miscellaneousRemark>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="TravelerRefNumber/@RPH != '' or FlightRefNumber/@RPH != ''">
					<referenceForDataElement>
						<!--xsl:if test="TravelerRefNumber/@RPH != ''">
						<xsl:call-template name="RPH">
							<xsl:with-param name="RPH"><xsl:value-of select="TravelerRefNumber/@RPH"/></xsl:with-param>
							<xsl:with-param name="Type"><xsl:text>P</xsl:text></xsl:with-param>
						</xsl:call-template>
					</xsl:if-->
						<xsl:for-each select="TravelerRefNumber">
							<reference>
								<qualifier>PR</qualifier>
								<number>
									<xsl:value-of select="@RPH"/>
								</number>
							</reference>
						</xsl:for-each>
						<xsl:if test="FlightRefNumber/@RPH != ''">
							<xsl:call-template name="RPH">
								<xsl:with-param name="RPH">
									<xsl:value-of select="FlightRefNumber/@RPH"/>
								</xsl:with-param>
								<xsl:with-param name="Type">
									<xsl:text>S</xsl:text>
								</xsl:with-param>
							</xsl:call-template>
						</xsl:if>
					</referenceForDataElement>
				</xsl:if>
			</dataElementsIndiv>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		      Form of payment		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match = "PaymentDetails">
			 <dataElementsIndiv> 
				<elementManagementData>
					<segmentName>FP</segmentName> 	
				</elementManagementData>                                  
		 		<formOfPayment>	 		
		 			<xsl:apply-templates select="PaymentDetail"/>	
		 		</formOfPayment>
			</dataElementsIndiv>
	</xsl:template>
	<!-- -->
	<xsl:template match="PaymentDetail">
		<fop>
			<xsl:choose>
				<xsl:when test="PaymentCard">
					<identification>CC</identification>
					<xsl:if test="PaymentAmount/@Amount != ''">
						<amount>
							<xsl:choose>
								<xsl:when test="PaymentAmount/@DecimalPlaces !=''">
									<xsl:variable name="dec">
										<xsl:value-of select="PaymentAmount/@DecimalPlaces"/>
									</xsl:variable>
									<xsl:value-of select="substring(PaymentAmount/@Amount,1,string-length(PaymentAmount/@Amount) - $dec)"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="substring(PaymentAmount/@Amount,string-length(PaymentAmount/@Amount) - ($dec - 1))"/>
								</xsl:when>
								<xsl:when test="contains(PaymentAmount/@Amount,'.')">
									<xsl:value-of select="format-number(PaymentAmount/@Amount,'0.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="PaymentAmount/@Amount"/>
								</xsl:otherwise>
							</xsl:choose>
						</amount>
					</xsl:if>
					<creditCardCode>
						<xsl:choose>
							<xsl:when test="PaymentCard/@CardCode = 'MC'">CA</xsl:when>
							<xsl:when test="PaymentCard/@CardCode = 'DN'">DC</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="PaymentCard/@CardCode"/>
							</xsl:otherwise>
						</xsl:choose>
					</creditCardCode>
					<accountNumber>
						<xsl:value-of select="PaymentCard/@CardNumber"/>
					</accountNumber>
					<expiryDate>
						<xsl:value-of select="PaymentCard/@ExpireDate"/>
					</expiryDate>
					<xsl:if test="TPA_Extensions/@ConfirmationNumber!=''">
						<approvalCode>
							<xsl:choose>
								<xsl:when test="substring(TPA_Extensions/@ConfirmationNumber,1,1) = 'N'">
									<xsl:value-of select="substring(TPA_Extensions/@ConfirmationNumber,2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="TPA_Extensions/@ConfirmationNumber"/>
								</xsl:otherwise>
							</xsl:choose>
						</approvalCode>
					</xsl:if>
					<xsl:if test="PaymentAmount/@CurrencyCode != ''">
						<currencyCode>
							<xsl:value-of select="PaymentAmount/@CurrencyCode"/>
						</currencyCode>
					</xsl:if>
				</xsl:when>
				<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
					<identification>CA</identification>
					<xsl:if test="PaymentAmount/@Amount != ''">
						<amount>
							<xsl:choose>
								<xsl:when test="PaymentAmount/@DecimalPlaces !=''">
									<xsl:variable name="dec">
										<xsl:value-of select="PaymentAmount/@DecimalPlaces"/>
									</xsl:variable>
									<xsl:value-of select="substring(PaymentAmount/@Amount,1,string-length(PaymentAmount/@Amount) - $dec)"/>
									<xsl:text>.</xsl:text>
									<xsl:value-of select="substring(PaymentAmount/@Amount,string-length(PaymentAmount/@Amount) - ($dec - 1))"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="PaymentAmount/@Amount"/>
								</xsl:otherwise>
							</xsl:choose>
						</amount>
					</xsl:if>
					<xsl:if test="PaymentAmount/@CurrencyCode != ''">
						<currencyCode>
							<xsl:value-of select="PaymentAmount/@CurrencyCode"/>
						</currencyCode>
					</xsl:if>
				</xsl:when>
				<xsl:when test="DirectBill/@DirectBill_ID='Check'">
					<identification>CK</identification>
				</xsl:when>
				<xsl:when test="MiscChargeOrder/@OriginalPaymentForm != ''">
                    <identification>MS</identification>
		            <freetext>
		                <xsl:value-of select="MiscChargeOrder/@OriginalPaymentForm"/>
		            </freetext> 	
                </xsl:when>
				<xsl:otherwise>
					<identification>MS</identification>
				</xsl:otherwise>
			</xsl:choose>
		</fop>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		      Billing address					 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Address" mode="billing">
		<xsl:choose>
			<xsl:when test="AddressLine != '' and StreetNmbr=''">
				<dataElementsIndiv>
					<elementManagementData>
						<segmentName>ABU</segmentName>
					</elementManagementData>
					<freetextData>
						<freetextDetail>
							<subjectQualifier>3</subjectQualifier>
						</freetextDetail>
						<longFreetext>
							<xsl:value-of select="AddressLine"/>
						</longFreetext>
					</freetextData>
				</dataElementsIndiv>
			</xsl:when>
			<xsl:when test="@FormattedInd = 'true'">
				<dataElementsIndiv>
					<elementManagementData>
						<segmentName>AB</segmentName>
					</elementManagementData>
					<structuredAddress>
						<informationType>2</informationType>
						<address>
							<optionA1>A1</optionA1>
							<optionTextA1>
								<xsl:value-of select="StreetNmbr"/>
							</optionTextA1>
						</address>
						<optionalData>
							<option>CI</option>
							<optionText>
								<xsl:value-of select="CityName"/>
							</optionText>
						</optionalData>
						<xsl:if test="PostalCode!=''">
							<optionalData>
								<option>ZP</option>
								<optionText>
									<xsl:value-of select="PostalCode"/>
								</optionText>
							</optionalData>
						</xsl:if>
						<xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
							<optionalData>
								<option>ST</option>
								<optionText>
									<xsl:choose>
										<xsl:when test="StateProv/@StateCode != ''">
											<xsl:value-of select="StateProv/@StateCode"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="StateProv"/>
										</xsl:otherwise>
									</xsl:choose>
								</optionText>
							</optionalData>
						</xsl:if>
						<optionalData>
							<option>CO</option>
							<optionText>
								<xsl:choose>
									<xsl:when test="CountryName != ''">
										<xsl:value-of select="CountryName"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="CountryName/@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</optionText>
						</optionalData>
					</structuredAddress>
				</dataElementsIndiv>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="." mode="pax"/>
				<xsl:apply-templates select="." mode="street"/>
				<xsl:apply-templates select="." mode="city"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Address" mode="pax">
		<xsl:if test="../CardHolderName != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>ABU</segmentName>
				</elementManagementData>
				<freetextData>
					<freetextDetail>
						<subjectQualifier>3</subjectQualifier>
					</freetextDetail>
					<longFreetext>
						<xsl:value-of select="../CardHolderName"/>
					</longFreetext>
				</freetextData>
			</dataElementsIndiv>
		</xsl:if>
	</xsl:template>
	<xsl:template match="Address" mode="street">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>ABU</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
				</freetextDetail>
				<longFreetext>
					<xsl:value-of select="StreetNmbr"/>
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<xsl:template match="Address" mode="city">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>ABU</segmentName>
			</elementManagementData>
			<freetextData>
				<freetextDetail>
					<subjectQualifier>3</subjectQualifier>
				</freetextDetail>
				<longFreetext>
					<xsl:value-of select="CityName"/>
					<xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
						<xsl:text> </xsl:text>
						<xsl:choose>
							<xsl:when test="StateProv/@StateCode != ''">
								<xsl:value-of select="StateProv/@StateCode"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="StateProv"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
					<xsl:if test="PostalCode!=''">
						<xsl:text> </xsl:text>
						<xsl:value-of select="PostalCode"/>
					</xsl:if>
					<xsl:if test="CountryName != '' or CountryName/@Code != ''">
						<xsl:text> </xsl:text>
						<xsl:choose>
							<xsl:when test="CountryName != ''">
								<xsl:value-of select="CountryName"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="CountryName/@Code"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		      Delivery address					 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="DeliveryAddress">
		<xsl:choose>
			<xsl:when test="AddressLine != ''">
				<dataElementsIndiv>
					<elementManagementData>
						<segmentName>AMU</segmentName>
					</elementManagementData>
					<freetextData>
						<freetextDetail>
							<subjectQualifier>3</subjectQualifier>
						</freetextDetail>
						<longFreetext>
							<xsl:value-of select="AddressLine"/>
						</longFreetext>
					</freetextData>
				</dataElementsIndiv>
			</xsl:when>
			<xsl:otherwise>
				<dataElementsIndiv>
					<elementManagementData>
						<segmentName>AM</segmentName>
					</elementManagementData>
					<structuredAddress>
						<informationType>P25</informationType>
						<address>
							<optionA1>A1</optionA1>
							<optionTextA1>
								<xsl:value-of select="translate(StreetNmbr,'/',' ')"/>
							</optionTextA1>
						</address>
						<xsl:if test="../Name != ''">
							<optionalData>
								<option>NA</option>
								<optionText>
									<xsl:value-of select="../Name"/>
								</optionText>
							</optionalData>
						</xsl:if>
						<xsl:if test="BldgRoom != ''">
							<optionalData>
								<option>A2</option>
								<optionText>
									<xsl:value-of select="BldgRoom"/>
								</optionText>
							</optionalData>
						</xsl:if>
						<optionalData>
							<option>CI</option>
							<optionText>
								<xsl:value-of select="CityName"/>
							</optionText>
						</optionalData>
						<xsl:if test="PostalCode!=''">
							<optionalData>
								<option>ZP</option>
								<optionText>
									<xsl:value-of select="PostalCode"/>
								</optionText>
							</optionalData>
						</xsl:if>
						<xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
							<optionalData>
								<option>ST</option>
								<optionText>
									<xsl:choose>
										<xsl:when test="StateProv/@StateCode != ''">
											<xsl:value-of select="StateProv/@StateCode"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="StateProv"/>
										</xsl:otherwise>
									</xsl:choose>
								</optionText>
							</optionalData>
						</xsl:if>
						<xsl:if test="CountryName != '' or CountryName/@Code != ''">
							<optionalData>
								<option>CO</option>
								<optionText>
									<xsl:choose>
										<xsl:when test="CountryName != ''">
											<xsl:value-of select="CountryName"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="CountryName/@Code"/>
										</xsl:otherwise>
									</xsl:choose>
								</optionText>
							</optionalData>
						</xsl:if>
					</structuredAddress>
				</dataElementsIndiv>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		     Agency commission fee		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Commission">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>FM</segmentName>
			</elementManagementData>
			<commission>
				<commissionInfo>
					<xsl:choose>
						<xsl:when test="@Percent != ''">
							<percentage>
								<xsl:value-of select="@Percent"/>
							</percentage>
						</xsl:when>
						<xsl:otherwise>
							<amount>
								<xsl:choose>
									<xsl:when test="contains(@Amount,'.')">
										<xsl:value-of select="format-number(@Amount,'#.00')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@Amount"/>
									</xsl:otherwise>
								</xsl:choose>
							</amount>
						</xsl:otherwise>
					</xsl:choose>
				</commissionInfo>
			</commission>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		     Accointing Information		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="AccountingLine">
		<xsl:if test=". != ''">
			<dataElementsIndiv>
				<elementManagementData>
					<segmentName>AI</segmentName>
				</elementManagementData>
				<accounting>
					<account>
						<number>
							<xsl:value-of select="translate(string(.),'-','')"/>
						</number>
					</account>
				</accounting>
			</dataElementsIndiv>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		     Queue Information		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Queue">
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>OP</segmentName>
			</elementManagementData>
			<optionElement>
				<optionDetail>
					<xsl:if test="@PseudoCityCode!=''">
						<officeId>
							<xsl:value-of select="@PseudoCityCode"/>
						</officeId>
					</xsl:if>
					<queue>
						<xsl:value-of select="@QueueNumber"/>
					</queue>
					<category>
						<xsl:value-of select="@QueueCategory"/>
					</category>
				</optionDetail>
			</optionElement>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		     Frequent Flyer Information		 	  			  -->
	<!-- *********************************************************************** -->
	<!--xsl:template match="CustLoyalty">
	<dataElementsIndiv> 
		<elementManagementData>
			<segmentName>SSR</segmentName> 	
		</elementManagementData>      
		<serviceRequest>
			<ssr>
				<type>FQTV</type>
				<companyId><xsl:value-of select="@ProgramID"/></companyId>
				<indicator>P01</indicator>
			</ssr>
		</serviceRequest>                            
 		<frequentTravellerData>
 			<frequentTraveller>
 				<companyId><xsl:value-of select="@ProgramID"/></companyId>
 				<membershipNumber>
 					<xsl:value-of select="@ProgramID"/>
 					<xsl:value-of select="@MembershipID"/>
 				</membershipNumber>
 			</frequentTraveller>
 		</frequentTravellerData>
 		<xsl:if test="../TravelerRefNumber/@RPH != ''">
	 		<referenceForDataElement>
	 			<reference>
	 				<qualifier>PR</qualifier>
	 				<number><xsl:value-of select="../TravelerRefNumber/@RPH"/></number>
	 			</reference>
	 		</referenceForDataElement>
	 	</xsl:if>
	</dataElementsIndiv>
</xsl:template-->
	<!-- -->
	<xsl:template match="CustLoyalty">
		<Command_Cryptic>
			<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
			</messageAction>
			<longTextString>
				<textStringDetails>
					<xsl:text>FFN</xsl:text>
					<xsl:value-of select="@ProgramID"/>
					<xsl:text>-</xsl:text>
					<xsl:value-of select="@MembershipID"/>
					<xsl:text>/P</xsl:text>
					<xsl:value-of select="../TravelerRefNumber/@RPH"/>
				</textStringDetails>
			</longTextString>
		</Command_Cryptic>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		     Received From				     -->
	<!-- *********************************************************************** -->
	<xsl:template match="Source">
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
						<xsl:when test="@AgentSine != ''">
							<xsl:value-of select="@AgentSine"/>
						</xsl:when>
						<xsl:otherwise>TRIPXML</xsl:otherwise>
					</xsl:choose>
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<xsl:template match="POS" mode="Open">
		<SessionCreateRQ>
			<xsl:attribute name="Version"><xsl:text>1.01</xsl:text></xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode"><xsl:value-of select="Source/@PseudoCityCode"/></xsl:attribute>
				</Source>
			</POS>
		</SessionCreateRQ>
	</xsl:template>
	<xsl:template match="POS" mode="Close">
		<SessionCloseRQ>
			<xsl:attribute name="Version"><xsl:text>1.01</xsl:text></xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode"><xsl:value-of select="Source/@PseudoCityCode"/></xsl:attribute>
				</Source>
			</POS>
		</SessionCloseRQ>
	</xsl:template>
	<xsl:template match="VehResRQCore">
		<Cars>
			<CarAvail>
				<Car_SingleAvailability>
					<providerDetails>
						<companyDetails>
							<travelSector>CAR</travelSector>
							<companyCode>
								<xsl:value-of select="VendorPref/@Code"/>
							</companyCode>
						</companyDetails>
					</providerDetails>
					<pickupDropoffLocs>
						<locationType>176</locationType>
						<locationDescription>
							<code>
								<xsl:text>1A</xsl:text>
							</code>
							<name>
								<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode"/>
							</name>
						</locationDescription>
					</pickupDropoffLocs>
					<pickupDropoffLocs>
						<locationType>DOL</locationType>
						<locationDescription>
							<code>
								<xsl:text>1A</xsl:text>
							</code>
							<name>
								<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode"/>
							</name>
						</locationDescription>
					</pickupDropoffLocs>
					<xsl:apply-templates select="VehRentalCore"/>
					<pickupDropoffTimes>
						<beginDateTime>
							<year>
								<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,1,4)"/>
							</year>
							<month>
								<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,6,2)"/>
							</month>
							<day>
								<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,9,2)"/>
							</day>
							<hour>
								<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,12,2)"/>
							</hour>
							<minutes>
								<xsl:value-of select="substring(VehRentalCore/@PickUpDateTime,15,2)"/>
							</minutes>
						</beginDateTime>
						<endDateTime>
							<year>
								<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,1,4)"/>
							</year>
							<month>
								<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,6,2)"/>
							</month>
							<day>
								<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,9,2)"/>
							</day>
							<hour>
								<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,12,2)"/>
							</hour>
							<minutes>
								<xsl:value-of select="substring(VehRentalCore/@ReturnDateTime,15,2)"/>
							</minutes>
						</endDateTime>
					</pickupDropoffTimes>
					<xsl:if test="../POS/Source/@ISOCurrency!=''">
						<rateinfo>
							<tariffInfo>
								<currency>
									<xsl:value-of select="../POS/Source/@ISOCurrency"/>
								</currency>
							</tariffInfo>
						</rateinfo>
					</xsl:if>
					<xsl:if test="VehPref/VehType/@VehicleCategory != ''">
						<vehicleTypeInfo>
							<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
							<vehicleRentalNeedType>
								<vehicleTypeOwner>ACR</vehicleTypeOwner>
								<vehicleRentalPrefType>
									<xsl:value-of select="VehPref/VehType/@VehicleCategory"/>
								</vehicleRentalPrefType>
							</vehicleRentalNeedType>
						</vehicleTypeInfo>
					</xsl:if>
				</Car_SingleAvailability>
			</CarAvail>
			<CarSell>
				<Car_Sell>
					<pnrInfo>
						<paxTattooNbr>
							<referenceDetails>
								<type>PT</type>
								<value>X</value>
								<!-- real value will be added by business logic -->
							</referenceDetails>
						</paxTattooNbr>
					</pnrInfo>
					<sellData>
						<companyIdentification>
							<travelSector>CAR</travelSector>
							<companyCode>
								<xsl:value-of select="VendorPref/@Code"/>
							</companyCode>
							<xsl:if test="VendorPref/@CompanyShortName !=''">
								<companyName>
									<xsl:value-of select="VendorPref/@CompanyShortName"/>
								</companyName>
							</xsl:if>
							<xsl:if test="VendorPref/@CodeContext !=''">
								<accessLevel>
									<xsl:choose>
										<xsl:when test="VendorPref/@CodeContext = 'Complete Access'">CA</xsl:when>
										<xsl:when test="VendorPref/@CodeContext = 'Complete Access Plus'">CP</xsl:when>
										<xsl:when test="VendorPref/@CodeContext = 'Standard Access'">SA</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="VendorPref/@CodeContext"/>
										</xsl:otherwise>
									</xsl:choose>
								</accessLevel>
							</xsl:if>
						</companyIdentification>
						<sellFromAvailabilitylGroup/>
						<!--validityPeriod>
								<quantityDetails>
									<qualifier>DLD</qualifier>
									<value>5</value>
								</quantityDetails>
							</validityPeriod>
							<validityTokens>
								<referenceDetails>
									<type>EED</type>
									<value>489E61C994B3B9C7</value>
								</referenceDetails>
								<referenceDetails>
									<type>EFD</type>
									<value>527A502A497325A7B60F8489790652A9</value>
								</referenceDetails>
							</validityTokens>
						</sellFromAvailabilitylGroup-->
						<locationInfo>
							<locationType>176</locationType>
							<locationDescription>
								<code>1A</code>
								<name>
									<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode"/>
								</name>
							</locationDescription>
						</locationInfo>
						<locationInfo>
							<locationType>DOL</locationType>
							<locationDescription>
								<code>1A</code>
								<name>
									<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode"/>
								</name>
							</locationDescription>
						</locationInfo>
						<pickupDropoffTimes>
							<beginDateTime>
								<year>
									<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),1,4)"/>
								</year>
								<month>
									<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),6,2)"/>
								</month>
								<day>
									<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),9,2)"/>
								</day>
								<hour>
									<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),12,2)"/>
								</hour>
								<minutes>
									<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),15,2)"/>
								</minutes>
							</beginDateTime>
							<endDateTime>
								<year>
									<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),1,4)"/>
								</year>
								<month>
									<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),6,2)"/>
								</month>
								<day>
									<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),9,2)"/>
								</day>
								<hour>
									<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),12,2)"/>
								</hour>
								<minutes>
									<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),15,2)"/>
								</minutes>
							</endDateTime>
						</pickupDropoffTimes>
						<vehicleInformation>
							<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
							<vehicleRentalNeedType>
								<vehicleTypeOwner>ACR</vehicleTypeOwner>
								<vehicleRentalPrefType>
									<xsl:value-of select="VehPref/VehType/@VehicleCategory"/>
								</vehicleRentalPrefType>
							</vehicleRentalNeedType>
						</vehicleInformation>
						<xsl:if test="../VehResRQInfo/ArrivalDetails">
							<arrivalInfo>
								<inboundCarrierDetails>
									<carrier>
										<xsl:value-of select="../VehResRQInfo/ArrivalDetails/MarketingCompany/@Code"/>
									</carrier>
								</inboundCarrierDetails>
								<inboundFlightDetails>
									<flightNumber>
										<xsl:value-of select="../VehResRQInfo/ArrivalDetails/@Number"/>
									</flightNumber>
								</inboundFlightDetails>
								<inboundArrivalDate>
									<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),1,4)"/>
									<xsl:value-of select="substring(string	(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),6,2)"/>
									<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),9,2)"/>
									<xsl:value-of select="substring(string	(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),12,2)"/>
									<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),15,2)"/>
								</inboundArrivalDate>
							</arrivalInfo>
						</xsl:if>
						<rateCodeInfo>
							<fareCategories>
								<fareType>
									<xsl:value-of select="RateQualifier/@RateQualifier"/>
								</fareType>
							</fareCategories>
						</rateCodeInfo>
						<xsl:if test="RateQualifier/@CorpDiscountNmbr != ''">
							<customerInfo>
								<customerReferences>
									<referenceQualifier>CD</referenceQualifier>
									<referenceNumber>
										<xsl:value-of select="RateQualifier/@CorpDiscountNmbr"/>
									</referenceNumber>
								</customerReferences>
							</customerInfo>
						</xsl:if>
						<rateInfo>
							<tariffInfo>
								<rateType/>
								<!--xsl:value-of select="RateQualifier/@VendorRateID" />
								</rateType-->
								<xsl:if test="RateQualifier/@RatePeriod != ''">
									<ratePlanIndicator>
										<xsl:choose>
											<xsl:when test="RateQualifier/@RatePeriod = 'Daily'">DY</xsl:when>
											<xsl:when test="RateQualifier/@RatePeriod = 'Monthly'">MY</xsl:when>
											<xsl:when test="RateQualifier/@RatePeriod = 'Weekend'">WD</xsl:when>
											<xsl:when test="RateQualifier/@RatePeriod = 'Weekly'">WY</xsl:when>
										</xsl:choose>
									</ratePlanIndicator>
								</xsl:if>
							</tariffInfo>
						</rateInfo>
						<xsl:if test="../VehResRQInfo/RentalPaymentPref !=''">
							<payment>
								<formOfPayment>
									<type>
										<xsl:choose>
											<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode != ''">CC</xsl:when>
											<xsl:otherwise>CA</xsl:otherwise>
										</xsl:choose>
									</type>
									<vendorCode>
										<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode"/>
									</vendorCode>
									<creditCardNumber>
										<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardNumber"/>
									</creditCardNumber>
									<expiryDate>
										<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@ExpireDate"/>
									</expiryDate>
									<extendedPayment>
										<xsl:choose>
											<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode != ''">GUA</xsl:when>
											<xsl:otherwise>FOP</xsl:otherwise>
										</xsl:choose>
									</extendedPayment>
								</formOfPayment>
								<xsl:if test="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode  != ''">
									<otherFormOfPayment>
										<type>
											<xsl:choose>
												<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode != ''">CC</xsl:when>
												<xsl:otherwise>CA</xsl:otherwise>
											</xsl:choose>
										</type>
										<vendorCode>
											<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode"/>
										</vendorCode>
										<creditCardNumber>
											<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardNumber"/>
										</creditCardNumber>
										<expiryDate>
											<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@ExpireDate"/>
										</expiryDate>
										<extendedPayment>
											<xsl:choose>
												<xsl:when test="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode != ''">GUA</xsl:when>
												<xsl:otherwise>FOP</xsl:otherwise>
											</xsl:choose>
										</extendedPayment>
									</otherFormOfPayment>
								</xsl:if>
							</payment>
						</xsl:if>
					</sellData>
				</Car_Sell>
			</CarSell>
		</Cars>
	</xsl:template>
	<xsl:template match="HotelReservation">
		<Hotels>
			<xsl:if test="RoomStays/RoomStay/BasicPropertyInfo/@HotelCodeContext!='SA' and RoomStays/RoomStay/BasicPropertyInfo/@HotelCodeContext!='IA'">
				<HotelAvail>
					<Hotel_SingleAvailability>
						<scrollingInformation>
							<displayRequest>050</displayRequest>
							<maxNumberItems>30</maxNumberItems>
						</scrollingInformation>
						<xsl:apply-templates select="RoomStays/RoomStay" mode="avail"/>
					</Hotel_SingleAvailability>
				</HotelAvail>
			</xsl:if>
			<HotelSell>
				<Hotel_Sell>
					<productInformation>
						<xsl:apply-templates select="RoomStays/RoomStay" mode="sell"/>
					</productInformation>
				</Hotel_Sell>
			</HotelSell>
		</Hotels>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<xsl:template match="RoomStay" mode="sell">
		<hotelPropertyInfo>
			<hotelReference>
				<chainCode>
					<xsl:value-of select="BasicPropertyInfo/@ChainCode"/>
				</chainCode>
				<cityCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCityCode"/>
				</cityCode>
				<hotelCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCode"/>
				</hotelCode>
			</hotelReference>
		</hotelPropertyInfo>
		<requestedDates>
			<businessSemantic>CHK</businessSemantic>
			<timeMode>LT</timeMode>
			<beginDateTime>
				<year>
					<xsl:value-of select="substring(string(TimeSpan/@Start),1,4)"/>
				</year>
				<month>
					<xsl:value-of select="substring(string(TimeSpan/@Start),6,2)"/>
				</month>
				<day>
					<xsl:value-of select="substring(string(TimeSpan/@Start),9,2)"/>
				</day>
			</beginDateTime>
			<endDateTime>
				<year>
					<xsl:value-of select="substring(string(TimeSpan/@End),1,4)"/>
				</year>
				<month>
					<xsl:value-of select="substring(string(TimeSpan/@End),6,2)"/>
				</month>
				<day>
					<xsl:value-of select="substring(string(TimeSpan/@End),9,2)"/>
				</day>
			</endDateTime>
		</requestedDates>
		<roomRateDetails>
			<roomInformation>
				<xsl:choose>
					<xsl:when test="BasicPropertyInfo/@HotelCodeContext='SA' or BasicPropertyInfo/@HotelCodeContext='IA'">
						<roomRateIdentifier>
							<roomType>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),1,3)"/>
							</roomType>
							<ratePlanCode>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),4,3)"/>
							</ratePlanCode>
						</roomRateIdentifier>
					</xsl:when>
					<xsl:otherwise>
						<bookingCode>
							<xsl:value-of select="RoomRates/RoomRate/@BookingCode"/>
						</bookingCode>
					</xsl:otherwise>
				</xsl:choose>
				<guestCountDetails>
					<numberOfUnit>
						<xsl:value-of select="GuestCounts/GuestCount/@Count"/>
					</numberOfUnit>
					<unitQualifier>A</unitQualifier>
				</guestCountDetails>
			</roomInformation>
			<xsl:if test="RoomTypes/RoomType/TPA_Extensions/HotelData/ExtraAdult !='' or RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayAdult !='' or 	RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayChild !='' or RoomTypes/RoomType/TPA_Extensions/HotelData/Crib !=''"/>
		</roomRateDetails>
		<xsl:if test="@SourceOfBusiness!='' and Guarantee/@GuaranteeType != 'BS'">
			<bookingSource>
			     <originIdentification>
			     	<originatorId><xsl:value-of select="@SourceOfBusiness"/></originatorId>
			     </originIdentification> 
			</bookingSource>
		</xsl:if>
		<!--relatedProduct>
			<quantity>
				<xsl:value-of select="RoomRates/RoomRate/@NumberOfUnits"/>
			</quantity>
		</relatedProduct>
		<selectionInformation>
			<selectionDetails>
				<option>P10</option>
			</selectionDetails>
		</selectionInformation-->
		<xsl:if test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!= '' or DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard!='' or (@SourceOfBusiness != '' and Guarantee/@GuaranteeType = 'BS')">
			<guaranteeOrDeposit>
				<xsl:choose>
					<xsl:when test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!= ''">
						<paymentInfo>
							<paymentDetails>
								<formOfPaymentCode>1</formOfPaymentCode>
								<paymentType>1</paymentType>
								<serviceToPay>3</serviceToPay>
							</paymentDetails>
						</paymentInfo>
						<creditCardInfo>
							<formOfPayment>
								<type>CC</type>
								<vendorCode>
									<xsl:variable name="cc">
										<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="$cc='VD'">VI</xsl:when>
										<xsl:when test="$cc='VE'">VI</xsl:when>
										<xsl:when test="$cc='VT'">VI</xsl:when>
										<xsl:when test="$cc='MC'">CA</xsl:when>
										<xsl:when test="$cc='MD'">CA</xsl:when>
										<xsl:when test="$cc='MP'">CA</xsl:when>
										<xsl:when test="$cc='DN'">DC</xsl:when>
										<xsl:otherwise><xsl:value-of select="$cc"/></xsl:otherwise>
									</xsl:choose>
								</vendorCode>
								<creditCardNumber>
									<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardNumber"/>
								</creditCardNumber>
								<expiryDate>
									<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate"/>
								</expiryDate>
							</formOfPayment>
						</creditCardInfo>
					</xsl:when>
					<xsl:when test="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard!=''">
						<paymentInfo>
							<paymentDetails>
								<formOfPaymentCode>1</formOfPaymentCode>
								<paymentType>2</paymentType>
								<serviceToPay>3</serviceToPay>
							</paymentDetails>
						</paymentInfo>
						<creditCardInfo>
							<formOfPayment>
								<type>CC</type>
								<vendorCode>
									<xsl:variable name="cc">
										<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode"/>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="$cc='VD'">VI</xsl:when>
										<xsl:when test="$cc='VE'">VI</xsl:when>
										<xsl:when test="$cc='VT'">VI</xsl:when>
										<xsl:when test="$cc='MC'">CA</xsl:when>
										<xsl:when test="$cc='MD'">CA</xsl:when>
										<xsl:when test="$cc='MP'">CA</xsl:when>
										<xsl:when test="$cc='DN'">DC</xsl:when>
										<xsl:otherwise><xsl:value-of select="$cc"/></xsl:otherwise>
									</xsl:choose>
								</vendorCode>
								<creditCardNumber>
									<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardNumber"/>
								</creditCardNumber>
								<expiryDate>
									<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@ExpireDate"/>
								</expiryDate>
							</formOfPayment>
						</creditCardInfo>
					</xsl:when>
					<xsl:when test="@SourceOfBusiness != '' and Guarantee/@GuaranteeType = 'BS'">
						<paymentInfo>
							<paymentDetails>
								<formOfPaymentCode>10</formOfPaymentCode>
								<paymentType>1</paymentType>
								<serviceToPay>3</serviceToPay>
								<referenceNumber>
									<xsl:value-of select="@SourceOfBusiness"/>
								</referenceNumber>
							</paymentDetails>
						</paymentInfo>
					</xsl:when>
				</xsl:choose>
			</guaranteeOrDeposit>
		</xsl:if>
		<xsl:if test="SpecialRequests/SpecialRequest[@RequestCode='SI']">
			<textOptions>
				<remarkDetails>
					<type>HSI</type>
					<freetext><xsl:value-of select="SpecialRequests/SpecialRequest[@RequestCode='SI']/Text"/></freetext>
					<businessFunction>3</businessFunction>
					<language>EN</language>
					<source>M</source>
					<encoding>1</encoding>
				</remarkDetails>
			</textOptions>
		</xsl:if>
		<referenceForSegment>
			<referenceDetails>
				<type>PT</type>
				<value>
					<xsl:choose>
						<xsl:when test="ResGuestRPHs/ResGuestRPH/@RPH!=''">
							<xsl:variable name="rph">
								<xsl:value-of select="ResGuestRPHs/ResGuestRPH/@RPH"/>
							</xsl:variable>
							<xsl:value-of select="$rph + 1"/>
						</xsl:when>
						<xsl:otherwise>X</xsl:otherwise>
					</xsl:choose>
				</value>
			</referenceDetails>
		</referenceForSegment>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<xsl:template match="RoomStay" mode="avail">
		<hotelPropertySelection>
			<propertyInformation>
				<chainCode>
					<xsl:value-of select="BasicPropertyInfo/@ChainCode"/>
				</chainCode>
				<cityCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCityCode"/>
				</cityCode>
				<propertyCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCode"/>
				</propertyCode>
			</propertyInformation>
			<checkInDate>
				<xsl:value-of select="substring(string(TimeSpan/@Start),9,2)"/>
				<xsl:value-of select="substring(string(TimeSpan/@Start),6,2)"/>
				<xsl:value-of select="substring(string(TimeSpan/@Start),3,2)"/>
			</checkInDate>
			<checkOutDate>
				<xsl:value-of select="substring(string(TimeSpan/@End),9,2)"/>
				<xsl:value-of select="substring(string(TimeSpan/@End),6,2)"/>
				<xsl:value-of select="substring(string(TimeSpan/@End),3,2)"/>
			</checkOutDate>
			<roomDetails>
				<availabilityStatus>A</availabilityStatus>
				<xsl:if test="GuestCounts/GuestCount/@Count != ''">
					<occupancy>
						<xsl:value-of select="GuestCounts/GuestCount/@Count"/>
					</occupancy>
				</xsl:if>
				<!--xsl:if test="RoomStayCandidates/RoomStayCandidate/@RoomType !=''">
					<roomType>
						<xsl:value-of select="RoomStayCandidates/RoomStayCandidate/@RoomType" />
					</roomType>
				</xsl:if-->
			</roomDetails>
			<!--xsl:if test="//@RequestedCurrency !=' '">
				<rateDetails>
					<xsl:if test="//@RequestedCurrency !=''">
						<currency>
							<xsl:value-of select="@RequestedCurrency" />
						</currency>
					</xsl:if>
				</rateDetails>
			</xsl:if-->
			<xsl:if test="RoomRates/RoomRate/@RatePlanID != ''">
				<negotiatedRateSelection>
					<rateCode>
						<xsl:value-of select="RoomRates/RoomRate/@RatePlanID"/>
					</rateCode>
				</negotiatedRateSelection>
			</xsl:if>
			<!--xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate" mode="RatePlanCode" /-->
		</hotelPropertySelection>
	</xsl:template>
	<!-- -->
	<xsl:template match="PriceData">
		<Pricing>
			<xsl:choose>
				<xsl:when test="@PricingInstruction!=''">
					<Command_Cryptic>
						<messageAction>
							<messageFunctionDetails>
								<messageFunction>M</messageFunction>
							</messageFunctionDetails>
						</messageAction>
						<longTextString>
							<textStringDetails>
								<xsl:text>FXP/ET/R,I</xsl:text>
								<xsl:value-of select="@PricingInstruction"/>
							</textStringDetails>
						</longTextString>
					</Command_Cryptic>
				</xsl:when>
				<xsl:when test="PassengerTypeQuantity[@Code = 'IIT' or @Code = 'ITF' or @Code = 'INN' or @Code = 'I06'] and @PriceType='Private'">
					<Command_Cryptic>
						<messageAction>
							<messageFunctionDetails>
								<messageFunction>M</messageFunction>
							</messageFunctionDetails>
						</messageAction>
						<longTextString>
							<textStringDetails>
								<xsl:variable name="pax">
									<xsl:apply-templates select="../PNRData/Traveler[@PassengerTypeCode!='INF']" mode="sortbysurname">
										<xsl:sort data-type="text" order="ascending" select="PersonName/Surname"/>
									</xsl:apply-templates>
								</xsl:variable>
								<xsl:text>FXP/</xsl:text>
								<xsl:for-each select="PassengerTypeQuantity">
									<xsl:if test="position() > 1">
										<xsl:text>//</xsl:text>
									</xsl:if>
									<xsl:text>P</xsl:text>
									<xsl:choose>
										<xsl:when test="@Code = 'IIT'">
											<xsl:for-each select="msxsl:node-set($pax)/Traveler[@PassengerTypeCode='ADT']">
												<xsl:if test="position() > 1">
													<xsl:text>,</xsl:text>
												</xsl:if>
												<xsl:value-of select="TravelerRefNumber/@RPH"/>
											</xsl:for-each>
											<xsl:text>/PAX/R</xsl:text>
											<xsl:value-of select="@Code"/>
											<xsl:text>,U</xsl:text>
										</xsl:when>
										<xsl:when test="@Code = 'ITF'">
											<xsl:for-each select="../../PNRData/Traveler[@PassengerTypeCode='INF']">
												<xsl:if test="position() > 1">
													<xsl:text>,</xsl:text>
												</xsl:if>
												<xsl:variable name="posinf">
													<xsl:value-of select="position()"/>
												</xsl:variable>
												<xsl:value-of select="msxsl:node-set($pax)/Traveler[@PassengerTypeCode='ADT'][position()=$posinf]/TravelerRefNumber/@RPH"/>
											</xsl:for-each>
											<xsl:text>/INF/R</xsl:text>
											<xsl:value-of select="@Code"/>
										</xsl:when>
										<xsl:when test="@Code = 'INN' or @Code = 'I06'">
											<xsl:for-each select="msxsl:node-set($pax)/Traveler[@PassengerTypeCode='CHD']">
												<xsl:if test="position() > 1">
													<xsl:text>,</xsl:text>
												</xsl:if>
												<xsl:value-of select="TravelerRefNumber/@RPH"/>
											</xsl:for-each>
											<xsl:text>/R</xsl:text>
											<xsl:value-of select="@Code"/>
										</xsl:when>
									</xsl:choose>
								</xsl:for-each>
							</textStringDetails>
						</longTextString>
					</Command_Cryptic>
				</xsl:when>
				<xsl:otherwise>
					<Fare_PricePNRWithBookingClass>
						<xsl:if test="@FlightRefNumberRPHList!=''">
							<paxSegReference>
								<xsl:call-template name="GetSegNum">
									<xsl:with-param name="segs"><xsl:value-of select="@FlightRefNumberRPHList"/></xsl:with-param>
								</xsl:call-template>
							</paxSegReference>
						</xsl:if>
						<overrideInformation>
							<attributeDetails>
								<attributeType>RLO</attributeType>
							</attributeDetails>
							<xsl:choose>
								<xsl:when test="@PriceType='Private'">
									<xsl:choose>
										<xsl:when test="NegoFares/PriceRequestInformation/NegotiatedFareCode and NegoFares/PriceRequestInformation/NegotiatedFareCode!='JCB'">
											<xsl:for-each select="NegoFares/PriceRequestInformation/NegotiatedFareCode">
												<attributeDetails>
													<attributeType>RW</attributeType>
													<attributeDescription>
														<xsl:choose>
															<xsl:when test="@Code!=''">
																<xsl:value-of select="@Code"/>
															</xsl:when>
															<xsl:when test="@SecondaryCode!=''">
																<xsl:value-of select="@SecondaryCode"/>
															</xsl:when>
															<xsl:otherwise>
																<xsl:value-of select="."/>
															</xsl:otherwise>
														</xsl:choose>
													</attributeDescription>
												</attributeDetails>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<attributeDetails>
												<attributeType>RU</attributeType>
											</attributeDetails>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<attributeDetails>
										<attributeType>RP</attributeType>
									</attributeDetails>
									<xsl:if test="not(PublishedFares)">
										<attributeDetails>
											<attributeType>RU</attributeType>
										</attributeDetails>
									</xsl:if>
								</xsl:otherwise>
							</xsl:choose>
						</overrideInformation>
						<xsl:if test="../PNRData/Ticketing/TicketAdvisory != '' and substring(../PNRData/Ticketing/TicketAdvisory,1,2) != 'TL' and substring(../PNRData/Ticketing/TicketAdvisory,1,2) != 'XL' and substring(../PNRData/Ticketing/TicketAdvisory,1,2) != 'OK'">
							<dateOverride>
								<businessSemantic>DO</businessSemantic>
								<dateTime>
									<year>
										<xsl:value-of select="substring(../PNRData/Ticketing/TicketAdvisory,1,4)"/>
									</year>
									<month>
										<xsl:value-of select="substring(../PNRData/Ticketing/TicketAdvisory,6,2)"/>
									</month>
									<day>
										<xsl:value-of select="substring(../PNRData/Ticketing/TicketAdvisory,9,2)"/>
									</day>
								</dateTime>
							</dateOverride>
						</xsl:if>
						<xsl:if test="@FlightRefNumberRPHList!=''">
							<validatingCarrier>
								<carrierInformation>
									<carrierCode><xsl:value-of select="@ValidatingAirlineCode"/></carrierCode>
								</carrierInformation>
							</validatingCarrier>
						</xsl:if>
						<xsl:if test="../../POS/Source/@ISOCurrency != ''">
							<currencyOverride>
								<firstRateDetail>
									<currencyCode>
										<xsl:value-of select="../../POS/Source/@ISOCurrency"/>
									</currencyCode>
								</firstRateDetail>
							</currencyOverride>
						</xsl:if>
					</Fare_PricePNRWithBookingClass>
				</xsl:otherwise>
			</xsl:choose>
		</Pricing>
	</xsl:template>
	<xsl:template match="Traveler" mode="sortbysurname">
		<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
		<Traveler>
			<xsl:attribute name="PassengerTypeCode"><xsl:value-of select="@PassengerTypeCode"/></xsl:attribute>
			<TravelerRefNumber>
				<xsl:attribute name="RPH"><xsl:value-of select="$pos"/></xsl:attribute>
			</TravelerRefNumber>
		</Traveler> 			
	</xsl:template>
	<xsl:template name="GetSegNum">
		<xsl:param name="segs"/>
		<xsl:if test="$segs != ''">
			<xsl:variable name="seg">
				<xsl:value-of select="concat($segs,' ')"/>
			</xsl:variable>
			<refDetails>
				<refQualifier>S</refQualifier>
				<refNumber>
					<xsl:value-of select="substring-before($seg,' ')"/>
				</refNumber>
			</refDetails>
			<xsl:call-template name="GetSegNum">
				<xsl:with-param name="segs">
					<xsl:value-of select="substring-after($segs,' ')"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<xsl:template match="MCO">
		<PNR_CreateTSM>
			<msg>
				<messageFunctionDetails>
					<businessFunction>47</businessFunction>
				</messageFunctionDetails>
			</msg>
			<xsl:variable name="pax">
				<xsl:value-of select="TravelerRefNumber/@RPH"/>
			</xsl:variable>
			<mcoData>
				<paxTattoo>
					<otherPaxDetails>
						<type>
							<xsl:choose>
								<xsl:when test="../Traveler[TravelerRefNumber/@RPH = $pax]/@PassengerTypeCode = 'INF'">IN</xsl:when>
								<xsl:otherwise>A</xsl:otherwise>
							</xsl:choose>
						</type>
						<uniqueCustomerIdentifier>
							<xsl:value-of select="TravelerRefNumber/@RPH"/>
						</uniqueCustomerIdentifier>
					</otherPaxDetails>
				</paxTattoo>
				<totalFare>
					<monetaryDetails>
						<typeQualifier>B</typeQualifier>
						<amount>
							<xsl:variable name="amt">
								<xsl:variable name="dec">
									<xsl:value-of select="MCOFare/@DecimalPlaces"/>
								</xsl:variable>
								<xsl:value-of select="substring(MCOFare/@Amount,1,string-length(MCOFare/@Amount) - $dec)"/>
								<xsl:value-of select="substring(MCOFare/@Amount,string-length(MCOFare/@Amount) - ($dec - 1),2)"/>
							</xsl:variable>
							<xsl:value-of select="substring($amt,1,string-length($amt) - 2)"/>
							<xsl:text>.</xsl:text>
							<xsl:value-of select="substring($amt,string-length($amt) - 1)"/>
						</amount>
						<currency>
							<xsl:value-of select="MCOFare/@CurrencyCode"/>
						</currency>
					</monetaryDetails>
				</totalFare>
				<genInfo>
					<productDateTimeDetails>
						<departureDate>
							<xsl:value-of select="substring(DepartureDate,9,2)"/>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(DepartureDate,6,2)"/>
								</xsl:with-param>
							</xsl:call-template>
						</departureDate>
					</productDateTimeDetails>
				</genInfo>
				<freeTextInfo>
					<freeTextQualification>
						<textSubjectQualifier>3</textSubjectQualifier>
						<informationType>13</informationType>
					</freeTextQualification>
					<freeText>
						<xsl:value-of select="Type/Text"/>
					</freeText>
				</freeTextInfo>
				<mcoDocData>
					<tktNumber>
						<documentDetails>
							<type>
								<xsl:value-of select="Type/@Code"/>
							</type>
						</documentDetails>
					</tktNumber>
				</mcoDocData>
			</mcoData>
		</PNR_CreateTSM>
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
	
	<!-- ********************************OTH Segment ************************************************************************ -->
	<xsl:template match="Segment" mode="Seg">
		<itineraryInfo>
			<elementManagementItinerary>
				<reference> 
					<qualifier>SR</qualifier>
					<number>
						<xsl:value-of select="position()"/>
					</number>
				</reference>
				<segmentName>
					<xsl:choose>
						<xsl:when test="@Type='OTH'">RU</xsl:when>
						<xsl:otherwise>RU</xsl:otherwise>
					</xsl:choose>
				</segmentName>
			</elementManagementItinerary>
			<airAuxItinerary>
				<travelProduct>
					<product>
						<depDate>
							<xsl:value-of select="substring(string(@Date),9,2)" />
							<xsl:value-of select="substring(string(@Date),6,2)" />
							<xsl:value-of select="substring(string(@Date),3,2)" />
						</depDate>
					</product>
					<boardpointDetail>
						<cityCode>
							<xsl:value-of select="@LocationCode"/>
						</cityCode>
					</boardpointDetail>
					<company>
						<identification>1A</identification>
					</company>
				</travelProduct>
				<messageAction>
					<business>
						<function>32</function>
					</business>
				</messageAction>
				<relatedProduct>
					<quantity>
						<xsl:value-of select="@NumberInParty"/>
					</quantity>
					<status>HK</status>
				</relatedProduct>
				<freetextItinerary>
					<longFreetext>
						<xsl:value-of select="Text" />
					</longFreetext>
				</freetextItinerary>
			</airAuxItinerary>
		</itineraryInfo>
	</xsl:template>	

	<xsl:template name="createMisc">
		<itineraryInfo>
			<elementManagementItinerary>
				<reference> 
					<qualifier>SR</qualifier>
					<number>1</number>
				</reference>
				<segmentName>RU</segmentName>
			</elementManagementItinerary>
			<airAuxItinerary>
				<travelProduct>
					<product>
						<depDate><xsl:value-of select="'HOLDDATE'"/></depDate>
					</product>
					<boardpointDetail>
						<cityCode>ATL</cityCode>
					</boardpointDetail>
					<company>
						<identification>1A</identification>
					</company>
				</travelProduct>
				<messageAction>
					<business>
						<function>32</function>
					</business>
				</messageAction>
				<relatedProduct>
					<quantity>1</quantity>
					<status>HK</status>
				</relatedProduct>
				<freetextItinerary>
					<longFreetext><xsl:value-of select="'HOLD FOR NOTES'"/></longFreetext>
				</freetextItinerary>
			</airAuxItinerary>
			<referenceForSegment>
				<reference>
					<qualifier>PR</qualifier>
					<number>1</number>
				</reference>
			</referenceForSegment>
		</itineraryInfo>
	</xsl:template>
</xsl:stylesheet>
