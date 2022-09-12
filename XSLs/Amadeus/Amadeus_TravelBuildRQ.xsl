<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_TravelBuildRQ.xsl 														       -->
	<!-- ================================================================== -->
	<!-- Date: 30 Oct 2009 - Rastko														       -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<!--xsl:variable name="bl" select="document('../../Tables/BL/alliancetravel_BL.xml')"/-->
	<xsl:variable name="pcc" select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
	<xsl:variable name="pricer">
		<xsl:choose>
			<xsl:when test="($pcc!='FMN1S28AA' and $pcc!='LAX1S21VW' and $pcc!='HOUG32101' and $pcc!='HOUG3210A' and $pcc!='MSPG32102' and $pcc!='MSPG32103' and $pcc!='MSPG32100' and $pcc!='DCA1S2182' and $pcc!='DCA1S2117' and $pcc!='LONU123LF' and $pcc!='LAXGO3100' and $pcc!='YTOGO3100' and $pcc!='BUF1S210L' and $pcc!='ATL1S21D9' and $pcc!='ATL1S21DG' and $pcc!='AVL1S2101' and $pcc!='CHI1S211S' and $pcc!='MKE1S2142' and $pcc!='DCA1S219D' and $pcc!='MCO1S213N') and OTA_TravelItineraryRQ/OTA_AirBookRQ">MP</xsl:when>
			<xsl:otherwise>VP</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="passengers">
		<xsl:for-each select="OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler[@PassengerTypeCode != 'INF' and @PassengerTypeCode != 'ITF']">
			<xsl:sort order="ascending" data-type="text" select="PersonName/Surname"/>
			<Traveler>
				<xsl:attribute name="PassengerTypeCode"><xsl:value-of select="@PassengerTypeCode"/></xsl:attribute>
				<PersonName>
					<NamePrefix>
						<xsl:value-of select="PersonName/NamePrefix"/>
					</NamePrefix>
					<GivenName>
						<xsl:value-of select="PersonName/GivenName"/>
					</GivenName>
					<Surname>
						<xsl:value-of select="PersonName/Surname"/>
					</Surname>
					<NameTitle>
						<xsl:value-of select="PersonName/NameTitle"/>
					</NameTitle>
				</PersonName>
				<TravelerRefNumber>
					<xsl:attribute name="RPH"><xsl:value-of select="position()"/></xsl:attribute>
				</TravelerRefNumber>
			</Traveler>
		</xsl:for-each>
		<xsl:for-each select="OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF']">
			<xsl:copy-of select="."/>
		</xsl:for-each>
	</xsl:variable>
	<xsl:template match="/">
		<TravelBuild>
			<xsl:apply-templates select="OTA_TravelItineraryRQ"/>
		</TravelBuild>
	</xsl:template>
	<!-- ************************************************************************************************************-->
	<xsl:template match="OTA_TravelItineraryRQ">
		<xsl:if test="$pricer='MP'">
			<MasterPricer>
				<PoweredLowestFare_SellFromRecommendation>
					<messageActionDetails>
						<messageFunctionDetails>
							<messageFunction>183</messageFunction>
							<additionalMessageFunction>M1</additionalMessageFunction>
						</messageFunctionDetails>
					</messageActionDetails>
					<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="MP"/>
				</PoweredLowestFare_SellFromRecommendation>
			</MasterPricer>
		</xsl:if>
		<MultiElements>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<xsl:variable name="inf">
					<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'])"/>
				</xsl:variable>
				<xsl:variable name="chd">
					<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'CHD' or @PassengerTypeCode = 'INS' or @PassengerTypeCode = 'INN' or @PassengerTypeCode = 'ITS'])"/>
				</xsl:variable>
				<xsl:variable name="adt">
					<xsl:variable name="all">
						<xsl:value-of select="count(TPA_Extensions/PNRData/Traveler)"/>
					</xsl:variable>
					<xsl:value-of select="$all - $inf - $chd"/>
				</xsl:variable>
				<!--xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[(@PassengerTypeCode != 'INF' and @PassengerTypeCode != 'ITF' and @PassengerTypeCode != 'INS' and @PassengerTypeCode != 'CHD' and @PassengerTypeCode != 'ITS' and @PassengerTypeCode != 'INN') or not(@PassengerTypeCode)]" mode="nameadt">
					<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
					<xsl:with-param name="inf"><xsl:value-of select="$inf"/></xsl:with-param>
				</xsl:apply-templates>
				<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode = 'INS' or @PassengerTypeCode = 'CHD' or @PassengerTypeCode = 'INN' or @PassengerTypeCode = 'ITS']" mode="namechd">
					<xsl:with-param name="adt"><xsl:value-of select="$adt"/></xsl:with-param>
				</xsl:apply-templates-->
				<xsl:apply-templates select="msxsl:node-set($passengers)/Traveler[(@PassengerTypeCode != 'INF' and @PassengerTypeCode != 'ITF' and @PassengerTypeCode != 'INS' and @PassengerTypeCode != 'CHD' and @PassengerTypeCode != 'ITS' and @PassengerTypeCode != 'INN') or not(@PassengerTypeCode)]" mode="nameadt">
					<xsl:with-param name="adt">
						<xsl:value-of select="$adt"/>
					</xsl:with-param>
					<xsl:with-param name="inf">
						<xsl:value-of select="$inf"/>
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:apply-templates select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode = 'INS' or @PassengerTypeCode = 'CHD' or @PassengerTypeCode = 'INN' or @PassengerTypeCode = 'ITS']" mode="namechd">
					<xsl:with-param name="adt">
						<xsl:value-of select="$adt"/>
					</xsl:with-param>
				</xsl:apply-templates>
				<xsl:if test="$pricer='VP'">
					<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="VP"/>
				</xsl:if>
				<xsl:if test="OTA_AirBookRQ/MiscellaneousSegments/Segment">
					<originDestinationDetails>
						<originDestination />
						<xsl:apply-templates select="OTA_AirBookRQ/MiscellaneousSegments/Segment" mode="Seg"/>
					</originDestinationDetails>
				</xsl:if>				<dataElementsMaster>
					<marker1>1</marker1>
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
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='C']" mode="rc"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType!='C']" mode="rmk"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address" mode="billing"/>
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest[(@SeatPreference!='' and @SeatPreference!='0') or @SeatNumber!='']"/>
					<xsl:apply-templates select="TPA_Extensions/AgencyData/Commission"/>
					<xsl:apply-templates select="TPA_Extensions/PNRData/AccountingLine"/>
					<xsl:choose>
						<xsl:when test="TPA_Extensions/PNRData/Queue">
							<xsl:apply-templates select="TPA_Extensions/PNRData/Queue"/>
						</xsl:when>
						<xsl:when test="OTA_AirBookRQ/Queue">
							<xsl:apply-templates select="OTA_AirBookRQ/Queue"/>
						</xsl:when>
					</xsl:choose>
					<xsl:variable name="paxpos">
						<xsl:for-each select="TPA_Extensions/PNRData/Traveler">
							<xsl:sort order="ascending" data-type="text" select="PersonName/Surname"/>
							<xsl:value-of select="TravelerRefNumber/@RPH"/>
						</xsl:for-each>
					</xsl:variable>
					<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '']">
						<xsl:with-param name="paxpos" select="$paxpos"/>
					</xsl:apply-templates>
				</dataElementsMaster>
			</PoweredPNR_AddMultiElements>
		</MultiElements>
		<xsl:if test="OTA_AirBookRQ">
			<MCT>
				<Cryptic_GetScreen_Query>
					<Command>DMI</Command>
				</Cryptic_GetScreen_Query>
			</MCT>
			<ValuePricer>
				<Cryptic_GetScreen_Query>
					<Command>
						<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions" mode="VPCryptic"/>
					</Command>
				</Cryptic_GetScreen_Query>
			</ValuePricer>
			<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest[@SSRCode!=''] or OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation or TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '']">
				<SSRs>
					<PoweredPNR_AddMultiElements>
						<pnrActions>
							<optionCode>0</optionCode>
						</pnrActions>
						<dataElementsMaster>
							<marker1>1</marker1>
							<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest[@SSRCode!='']"/>
							<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation"/>
							<xsl:variable name="paxpos">
								<xsl:for-each select="TPA_Extensions/PNRData/Traveler">
									<xsl:sort order="ascending" data-type="text" select="PersonName/Surname"/>
									<xsl:value-of select="TravelerRefNumber/@RPH"/>
								</xsl:for-each>
							</xsl:variable>
							<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler/CustLoyalty[@ProgramID != '']">
								<xsl:with-param name="paxpos" select="$paxpos"/>
							</xsl:apply-templates>
						</dataElementsMaster>
					</PoweredPNR_AddMultiElements>
				</SSRs>
			</xsl:if>
		</xsl:if>
		<Cars>
			<xsl:apply-templates select="OTA_VehResRQ/VehResRQCore"/>
		</Cars>
		<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation"/>
		<Pricing>
			<xsl:apply-templates select="TPA_Extensions/PriceData"/>
		</Pricing>
		<StorePrice>
			<PoweredTicket_CreateTSTFromPricing>
				<!--xsl:variable name="ptypes">
					<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode!='INF'][1]" mode="storeprice">
						<xsl:with-param name="pt"></xsl:with-param>
					</xsl:apply-templates>
				</xsl:variable>
				<xsl:call-template name="psaList">
					<xsl:with-param name="ptypes"><xsl:value-of select="$ptypes"/></xsl:with-param>
					<xsl:with-param name="tst">1</xsl:with-param>
				</xsl:call-template-->
			</PoweredTicket_CreateTSTFromPricing>
		</StorePrice>
		<ET>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>10</optionCode>
				</pnrActions>
			</PoweredPNR_AddMultiElements>
		</ET>
		<EPAY>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>10</optionCode>
				</pnrActions>
				<dataElementsMaster>
					<marker1>1</marker1>
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
			</PoweredPNR_AddMultiElements>
		</EPAY>
	</xsl:template>
	<xsl:template name="psaList">
		<xsl:param name="ptypes"/>
		<xsl:param name="tst"/>
		<xsl:if test="string-length($ptypes) > 0">
			<psaList>
				<itemReference>
					<referenceType>TST</referenceType>
					<uniqueReference>
						<xsl:value-of select="$tst"/>
					</uniqueReference>
				</itemReference>
			</psaList>
			<xsl:call-template name="psaList">
				<xsl:with-param name="ptypes">
					<xsl:value-of select="substring($ptypes,4)"/>
				</xsl:with-param>
				<xsl:with-param name="tst">
					<xsl:value-of select="$tst + 1"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
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
			<xsl:if test="$pos &lt; $inf or $pos = $inf">
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
						<type>
							<xsl:value-of select="following-sibling::Traveler[@PassengerTypeCode='INF' or @PassengerTypeCode='ITF'][position() = $pos]/@PassengerTypeCode"/>
						</type>
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
		</travellerInfo>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group - Value Pricer						 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationOptions" mode="VPCryptic">
		<xsl:text>FXC</xsl:text>
		<!--xsl:value-of select="OriginDestinationOption[1]/FlightSegment[1]/@NumberInParty"/-->
		<xsl:for-each select="OriginDestinationOption">
			<xsl:if test="position()=1">
				<xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode"/>
			</xsl:if>
			<xsl:if test="position() &gt; 1 and FlightSegment[1]/DepartureAirport/@LocationCode != preceding-sibling::OriginDestinationOption[1]/FlightSegment[position()=last()]/ArrivalAirport/@LocationCode">
				<xsl:text>--</xsl:text>
				<xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode"/>
			</xsl:if>
			<xsl:text>/D</xsl:text>
			<xsl:value-of select="substring(FlightSegment[1]/@DepartureDateTime,9,2)"/>
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(FlightSegment[1]/@DepartureDateTime,6,2)"/>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
		</xsl:for-each>
		<xsl:text>//R,UP</xsl:text>
		<!--xsl:text>/PAX</xsl:text>
		<xsl:for-each select="../../../TPA_Extensions/PNRData/Traveler[@PassengerTypeCode!='INF']">
			<xsl:text>/1/R</xsl:text>
			<xsl:choose>
				<xsl:when test="@PassengerTypeCode = 'CHD'">CH</xsl:when>
				<xsl:when test="substring(@PassengerTypeCode,1,1)='C'">CH</xsl:when>
				<xsl:otherwise><xsl:value-of select="@PassengerTypeCode"/></xsl:otherwise>
			</xsl:choose>
		</xsl:for-each>
		<xsl:variable name="infcount">
			<xsl:value-of select="count(../../../TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='INF'])"/>
		</xsl:variable>
		<xsl:if test="$infcount &gt; 0">
			<xsl:text>/INF/</xsl:text>
			<xsl:value-of select="$infcount"/>
		</xsl:if-->
	</xsl:template>
	<xsl:template match="OriginDestinationOption" mode="VP">
		<originDestinationDetails>
			<originDestination>
				<origin>
					<xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode"/>
				</origin>
				<destination>
					<xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode"/>
				</destination>
			</originDestination>
			<xsl:apply-templates select="FlightSegment" mode="VP"/>
		</originDestinationDetails>
	</xsl:template>
	<xsl:template match="FlightSegment" mode="VP">
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
		<itineraryInfo>
			<elementManagementItinerary>
				<reference>
					<qualifier>SR</qualifier>
					<number>
						<xsl:value-of select="@RPH"/>
					</number>
				</reference>
				<segmentName>AIR</segmentName>
			</elementManagementItinerary>
			<airAuxItinerary>
				<travelProduct>
					<product>
						<depDate>
							<xsl:value-of select="substring(string(@DepartureDateTime),9,2)"/>
							<xsl:value-of select="substring(string(@DepartureDateTime),6,2)"/>
							<xsl:value-of select="substring(string(@DepartureDateTime),3,2)"/>
						</depDate>
					</product>
					<boardpointDetail>
						<cityCode>
							<xsl:value-of select="DepartureAirport/@LocationCode"/>
						</cityCode>
					</boardpointDetail>
					<offpointDetail>
						<cityCode>
							<xsl:value-of select="ArrivalAirport/@LocationCode"/>
						</cityCode>
					</offpointDetail>
					<company>
						<identification>
							<xsl:value-of select="MarketingAirline/@Code"/>
						</identification>
					</company>
					<productDetails>
						<identification>
							<xsl:value-of select="@FlightNumber"/>
						</identification>
						<classOfService>
							<xsl:value-of select="@ResBookDesigCode"/>
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
						<xsl:value-of select="@NumberInParty"/>
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
	</xsl:template>
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
			<!--message>
				<messageFunctionDetails>
					<messageFunction>183</messageFunction>
				</messageFunctionDetails>
			</message-->
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
							<xsl:when test="@SSRCode = 'DOCS'">HK</xsl:when>
							<xsl:otherwise>NN</xsl:otherwise>
						</xsl:choose>
					</status>
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
					<companyId>
						<xsl:value-of select="Airline/@Code"/>
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
						<xsl:when test="$Type='S' and $pricer='MP'">T</xsl:when>
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
							<xsl:value-of select="."/>
						</freetext>
					</remarks>
				</miscellaneousRemark>
			</dataElementsIndiv>
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
							<passengerType>
								<xsl:choose>
									<xsl:when test="TravelerRefNumber/@RPH!=''">
										<xsl:variable name="rph">
											<xsl:value-of select="TravelerRefNumber/@RPH"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">INF</xsl:when>
											<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='ITF'">INF</xsl:when>
											<xsl:otherwise>PAX</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>PAX</xsl:otherwise>
								</xsl:choose>
							</passengerType>
							<officeId/>
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
							<passengerType>
								<xsl:choose>
									<xsl:when test="TravelerRefNumber/@RPH!=''">
										<xsl:variable name="rph">
											<xsl:value-of select="TravelerRefNumber/@RPH"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='INF'">INF</xsl:when>
											<xsl:when test="../../../../../PNRData/Traveler[TravelerRefNumber/@RPH=$rph]/@PassengerTypeCode='ITF'">INF</xsl:when>
											<xsl:otherwise>PAX</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>PAX</xsl:otherwise>
								</xsl:choose>
							</passengerType>
							<freeFormatTour>
								<indicator>FF</indicator>
								<freetext>
									<xsl:value-of select="Text"/>
								</freetext>
							</freeFormatTour>
						</tourCode>
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
			</dataElementsIndiv>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		      Form of payment		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="PaymentDetails">
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
							<xsl:when test="PaymentCard/@CardCode = 'DI'">DC</xsl:when>
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
								<xsl:value-of select="StreetNmbr"/>
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
					<officeId>
						<xsl:value-of select="@PseudoCityCode"/>
					</officeId>
					<date/>
					<queue>
						<xsl:value-of select="@QueueNumber"/>
					</queue>
					<category>
						<xsl:value-of select="@QueueCategory"/>
					</category>
					<freetext/>
				</optionDetail>
			</optionElement>
		</dataElementsIndiv>
	</xsl:template>
	<!-- *********************************************************************** -->
	<!--         		     Frequent Flyer Information		 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="CustLoyalty">
		<xsl:param name="paxpos"/>
		<xsl:variable name="rph">
			<xsl:value-of select="../TravelerRefNumber/@RPH"/>
		</xsl:variable>
		<xsl:variable name="pos">
			<xsl:value-of select="substring-before($paxpos,$rph)"/>
		</xsl:variable>
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>SSR</segmentName>
			</elementManagementData>
			<serviceRequest>
				<ssr>
					<type>FQTV</type>
					<companyId>
						<xsl:value-of select="@ProgramID"/>
					</companyId>
					<indicator>P01</indicator>
				</ssr>
			</serviceRequest>
			<frequentTravellerData>
				<frequentTraveller>
					<companyId>
						<xsl:value-of select="@ProgramID"/>
					</companyId>
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
						<number>
							<xsl:value-of select="string-length($pos) + 1"/>
						</number>
					</reference>
				</referenceForDataElement>
			</xsl:if>
		</dataElementsIndiv>
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
						<xsl:otherwise>TRAVELTALK</xsl:otherwise>
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
		<PoweredCar_Sell>
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
						<rateType>
							<xsl:value-of select="RateQualifier/@RateCategory"/>
						</rateType>
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
		</PoweredCar_Sell>
	</xsl:template>
	<xsl:template match="HotelReservation">
		<Hotels>
			<xsl:if test="RoomStays/RoomStay/BasicPropertyInfo/@HotelCodeContext!='SA' and RoomStays/RoomStay/BasicPropertyInfo/@HotelCodeContext!='IA' and 	//POS/TPA_Extensions/Provider/System != 'Test'">
				<HotelAvail>
					<PoweredHotel_SingleAvailability>
						<scrollingInformation>
							<displayRequest>050</displayRequest>
							<maxNumberItems>30</maxNumberItems>
						</scrollingInformation>
						<xsl:apply-templates select="RoomStays/RoomStay" mode="avail"/>
					</PoweredHotel_SingleAvailability>
				</HotelAvail>
			</xsl:if>
			<HotelSell>
				<PoweredHotel_Sell>
					<productInformation>
						<xsl:apply-templates select="RoomStays/RoomStay" mode="sell"/>
					</productInformation>
				</PoweredHotel_Sell>
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
					<xsl:when test="BasicPropertyInfo/@HotelCodeContext='SA' or BasicPropertyInfo/@HotelCodeContext='IA' or ../../../../../POS/TPA_Extensions/Provider/System = 'Test'">
						<roomRateIdentifier>
							<roomType>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),1,3)"/>
							</roomType>
							<!--ratePlanCode>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),4,3)" />
							</ratePlanCode-->
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
		<!--xsl:if test="@SourceOfBusiness!=''">
			<bookingSource>
			     <originIdentification>
			     	<originatorId><xsl:value-of select="@SourceOfBusiness"/></originatorId>
			     </originIdentification> 
			</bookingSource>
		</xsl:if-->
		<relatedProduct>
			<quantity>
				<xsl:value-of select="RoomRates/RoomRate/@NumberOfUnits"/>
			</quantity>
		</relatedProduct>
		<selectionInformation>
			<selectionDetails>
				<option>P10</option>
			</selectionDetails>
		</selectionInformation>
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
								<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode"/>
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
								<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode"/>
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
		<referenceForSegment>
			<xsl:apply-templates select="../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler[1]" mode="hotel"/>
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
	<xsl:template match="Traveler" mode="hotel">
		<referenceDetails>
			<type>PT</type>
			<value>
				<xsl:value-of select="TravelerRefNumber/@RPH + 1"/>
			</value>
		</referenceDetails>
	</xsl:template>
	<xsl:template match="PriceData">
		<xsl:choose>
			<xsl:when test="PassengerTypeQuantity[@Code = 'IIT' or @Code = 'ITF' or @Code = 'INN'] and @PriceType='Private'">
				<Cryptic_GetScreen_Query>
					<Command>
						<xsl:text>FXP/</xsl:text>
						<xsl:for-each select="PassengerTypeQuantity">
							<xsl:if test="position() > 1">
								<xsl:text>//</xsl:text>
							</xsl:if>
							<xsl:text>P</xsl:text>
							<xsl:choose>
								<xsl:when test="@Code = 'IIT'">
									<!--xsl:for-each select="../../PNRData/Traveler[@PassengerTypeCode='ADT']">
										<xsl:if test="position() > 1"><xsl:text>,</xsl:text></xsl:if>
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:for-each-->
									<xsl:for-each select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode='ADT']">
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
										<!--xsl:value-of select="position()"/-->
										<xsl:variable name="pos">
											<xsl:value-of select="position()"/>
										</xsl:variable>
										<xsl:value-of select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode='ADT'][position()=$pos]/TravelerRefNumber/@RPH"/>
									</xsl:for-each>
									<xsl:text>/INF/R</xsl:text>
									<xsl:value-of select="@Code"/>
								</xsl:when>
								<xsl:when test="@Code = 'INN'">
									<!--xsl:for-each select="../../PNRData/Traveler[@PassengerTypeCode='CHD']">
										<xsl:if test="position() > 1"><xsl:text>,</xsl:text></xsl:if>
										<xsl:value-of select="TravelerRefNumber/@RPH"/>
									</xsl:for-each-->
									<xsl:for-each select="msxsl:node-set($passengers)/Traveler[@PassengerTypeCode='CHD']">
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
					</Command>
				</Cryptic_GetScreen_Query>
			</xsl:when>
			<xsl:otherwise>
				<PoweredFare_PricePNRWithBookingClass>
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
														<xsl:otherwise>
															<xsl:value-of select="."/>
														</xsl:otherwise>
													</xsl:choose>
												</attributeDescription>
											</attributeDetails>
										</xsl:for-each>
									</xsl:when>
									<!--xsl:when test="../../POS/Source/RequestorID/@ID = $bl/BL/@RequestorID and ../../POS/Source/@PseudoCityCode = $bl/BL/Message/ProviderBL/@PCC and ../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]/FlightSegment[1]/MarketingAirline/@Code = $bl/BL/Message/ProviderBL/Preferences/PrivateFare/Airline/@Code">
										<attributeDetails>
											<attributeType>RW</attributeType>
											<attributeDescription><xsl:value-of select="$bl/BL/Message/ProviderBL/Preferences/PrivateFare/@Code"/></attributeDescription>
										</attributeDetails>
									</xsl:when-->
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
							</xsl:otherwise>
						</xsl:choose>
					</overrideInformation>
					<xsl:if test="../PNRData/Ticketing/TicketAdvisory != '' and ../PNRData/Ticketing/TicketAdvisory != 'TL' and ../PNRData/Ticketing/TicketAdvisory != 'XL'">
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
					<xsl:if test="@ValidatingAirlineCode != ''">
						<validatingCarrier>
							<carrierInformation>
								<carrierCode>
									<xsl:value-of select="@ValidatingAirlineCode"/>
								</carrierCode>
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
					<!--discountInformation>          
						<penDisInformation>
							 <infoQualifier>701</infoQualifier>
							  <penDisData>
							  	  <discountCode>INN</discountCode> 
							  </penDisData>
						</penDisInformation>
						 <referenceQualifier>
							<refDetails>
								<refQualifier>P</refQualifier>
								<refNumber>17</refNumber>
							</refDetails> 
						 </referenceQualifier>
					</discountInformation-->
				</PoweredFare_PricePNRWithBookingClass>
			</xsl:otherwise>
		</xsl:choose>
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
							<xsl:value-of select="substring(string(@Date),9,2)"/>
							<xsl:value-of select="substring(string(@Date),6,2)"/>
							<xsl:value-of select="substring(string(@Date),3,2)"/>
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
						<xsl:value-of select="Text"/>
					</longFreetext>
				</freetextItinerary>
			</airAuxItinerary>
		</itineraryInfo>
	</xsl:template>
</xsl:stylesheet>
