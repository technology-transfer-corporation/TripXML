<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<TravelBuild>
			<xsl:apply-templates select="OTA_TravelItineraryRQ" />
		</TravelBuild>
	</xsl:template>
	<!-- ************************************************************************************************************-->
	<xsl:template match="OTA_TravelItineraryRQ">
		<Session>
			<xsl:apply-templates select="POS" mode="Open" />
		</Session>
		<AddMultiElementsMandatory>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler" mode="name" />
				<dataElementsMaster>
					<marker1>1</marker1>
					<xsl:apply-templates select="TPA_Extensions/PNRData/Telephone" />
					<xsl:choose>
						<xsl:when test="OTA_AirBookRQ/Ticketing">
							<xsl:apply-templates select="OTA_AirBookRQ/Ticketing" />
						</xsl:when>
						<xsl:when test="TPA_Extensions/PNRData/Ticketing">
							<xsl:apply-templates select="TPA_Extensions/PNRData/Ticketing" />
						</xsl:when>
					</xsl:choose>
					<xsl:apply-templates select="POS/Source" />
				</dataElementsMaster>
			</PoweredPNR_AddMultiElements>
		</AddMultiElementsMandatory>
		<AddMultiElementsAirSegments>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>0</optionCode>
				</pnrActions>
				<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption" />
			</PoweredPNR_AddMultiElements>
		</AddMultiElementsAirSegments>
		<AddMultiElementsOther>
			<xsl:if test="TPA_Extensions/PNRData/Email or OTA_AirBookRQ/TravelerInfo/SpecialReqDetails">
				<PoweredPNR_AddMultiElements>
					<pnrActions>
						<optionCode>0</optionCode>
					</pnrActions>
					<dataElementsMaster>
						<marker1>1</marker1>
						<xsl:apply-templates select="TPA_Extensions/PNRData/Email" />
						<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest"/>
						<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation"/>
						<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/Remarks/Remark"/>	
					</dataElementsMaster>
				</PoweredPNR_AddMultiElements>
			</xsl:if>
		</AddMultiElementsOther>
		<Cars>
			<xsl:apply-templates select="OTA_VehResRQ/VehResRQCore" />
		</Cars>
		<Hotels>
			<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation" />
		</Hotels>
		<Pricing>
			<xsl:apply-templates select="TPA_Extensions/PriceData"/>
		</Pricing>
		<ET>
			<PoweredPNR_AddMultiElements>
				<pnrActions>
					<optionCode>11</optionCode>
				</pnrActions>
			</PoweredPNR_AddMultiElements>
		</ET>
		<EndSession>
			<xsl:apply-templates select="POS" mode="Close" />
		</EndSession>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Name Elements 							 -->
	<!---********************************************************************-->
	<xsl:template match="Traveler" mode="name">
		<xsl:if test="@PassengerTypeCode!='INF'">
			<travellerInfo>
				<elementManagementPassenger>
					<reference>
						<qualifier>PR</qualifier>
						<number>
							<xsl:value-of select="TravelerRefNumber/@RPH" />
						</number>
					</reference>
					<segmentName>NM</segmentName>
				</elementManagementPassenger>
				<travellerInformation>
					<traveller>
						<surname>
							<xsl:value-of select="PersonName/Surname" />
						</surname>
						<quantity>
							<xsl:choose>
								<xsl:when test="following-sibling::Traveler[1]/@PassengerTypeCode='INF'">2</xsl:when>
								<xsl:otherwise>1</xsl:otherwise>
							</xsl:choose>
						</quantity>
					</traveller>
					<passenger>
						<firstName>
							<xsl:value-of select="PersonName/GivenName" />
						</firstName>
						<type>
							<xsl:value-of select="@PassengerTypeCode" />
						</type>
						<xsl:if test="following-sibling::Traveler[1]/@PassengerTypeCode='INF'">
							<infantIndicator>2</infantIndicator>
						</xsl:if>
					</passenger>
					<xsl:if test="following-sibling::Traveler[1]/@PassengerTypeCode='INF'">
						<passenger>
							<firstName>
								<xsl:value-of select="following-sibling::Traveler[1]/PersonName/GivenName" />
							</firstName>
							<type>INF</type>
						</passenger>
					</xsl:if>
				</travellerInformation>
			</travellerInfo>
		</xsl:if>
	</xsl:template>
	<!--*********************************************************************-->
	<!--     Air Sell Group 						 -->
	<!---********************************************************************-->
	<xsl:template match="OriginDestinationOption">
		<originDestinationDetails>
			<originDestination>
				<origin><xsl:value-of select="FlightSegment[1]/DepartureAirport/@LocationCode" /></origin>
				<destination><xsl:value-of select="FlightSegment[position()=last()]/ArrivalAirport/@LocationCode" /></destination>
			</originDestination>
			<xsl:apply-templates select="FlightSegment" />
		</originDestinationDetails>
	</xsl:template>
	
	<xsl:template match="FlightSegment">
		<xsl:variable name="DepTime">
			<xsl:value-of select="substring-after(@DepartureDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="DepTime2">
			<xsl:value-of select="substring(string($DepTime),1,5)" />
		</xsl:variable>
		<xsl:variable name="ArrTime">
			<xsl:value-of select="substring-after(@ArrivalDateTime,'T')" />
		</xsl:variable>
		<xsl:variable name="ArrTime2">
			<xsl:value-of select="substring(string($ArrTime),1,5)" />
		</xsl:variable>
		<itineraryInfo>
			<elementManagementItinerary>
				<segmentName>AIR</segmentName>
			</elementManagementItinerary>
			<airAuxItinerary>
				<travelProduct>
					<product>
						<depDate>
							<xsl:value-of select="substring(string(@DepartureDateTime),9,2)" />
							<xsl:value-of select="substring(string(@DepartureDateTime),6,2)" />
							<xsl:value-of select="substring(string(@DepartureDateTime),3,2)" />
						</depDate>
						<depTime>
							<xsl:value-of select="translate(string($DepTime2),':','')" />
						</depTime>
						<arrDate>
							<xsl:value-of select="substring(string(@ArrivalDateTime),9,2)" />
							<xsl:value-of select="substring(string(@ArrivalDateTime),6,2)" />
							<xsl:value-of select="substring(string(@ArrivalDateTime),3,2)" />
						</arrDate>
						<arrTime>
							<xsl:value-of select="translate(string($ArrTime2),':','')" />
						</arrTime>
					</product>
					<boardpointDetail>
						<cityCode>
							<xsl:value-of select="DepartureAirport/@LocationCode" />
						</cityCode>
					</boardpointDetail>
					<offpointDetail>
						<cityCode>
							<xsl:value-of select="ArrivalAirport/@LocationCode" />
						</cityCode>
					</offpointDetail>
					<company>
						<identification>
							<xsl:value-of select="MarketingAirline/@Code" />
						</identification>
					</company>
					<productDetails>
						<identification>
							<xsl:value-of select="@FlightNumber" />
						</identification>
						<classOfService>
							<xsl:value-of select="@ResBookDesigCode" />
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
						<xsl:value-of select="@NumberInParty" />
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
					<xsl:value-of select="@AreaCityCode" />
					<xsl:value-of select="@PhoneNumber" />
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
					<xsl:value-of select="." />
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>

	<!-- *********************************************************************** -->
	<!--         		       Ticketing Fields			 	  			  -->
	<!-- *********************************************************************** -->
	<xsl:template match="Ticketing">
		<xsl:variable name="TktTime">
			<xsl:value-of select="substring-after(@TicketTimeLimit,'T')" />
		</xsl:variable>
		<xsl:variable name="TktTime2">
			<xsl:value-of select="substring(string($TktTime),1,5)" />
		</xsl:variable>
		<dataElementsIndiv>
			<elementManagementData>
				<segmentName>TK</segmentName>
			</elementManagementData>
			<ticketElement>
				<ticket>
					<xsl:choose>
						<xsl:when test ="TicketAdvisory ='OK'">
							<indicator>OK</indicator>
						</xsl:when>
						<xsl:when test="@TicketTimeLimit">
							<indicator>TL</indicator>
							<date>
								<xsl:value-of select="substring(string(@TicketTimeLimit),9,2)" />
								<xsl:value-of select="substring(string(@TicketTimeLimit),6,2)" />
								<xsl:value-of select="substring(string(@TicketTimeLimit),3,2)" />
							</date>
							<xsl:choose>
								<xsl:when test="$TktTime2 = '0000'">
									<time>
										<xsl:text>2400</xsl:text>
									</time>
								</xsl:when>
								<xsl:otherwise>
									<time>
										<xsl:value-of select="translate(string($TktTime2),':','')" />
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
<xsl:template match = "SpecialServiceRequest" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>SSR</segmentName> 	
		</elementManagementData>                                  
 		<serviceRequest>
			<ssr>
				<type><xsl:value-of select="@SSRCode"/></type>
				<status>NN</status>
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
				<companyId><xsl:value-of select="Airline/@Code"/></companyId>
				<xsl:if test="Text!=''">
					<freetext><xsl:value-of select="Text"/></freetext>
				</xsl:if>
			</ssr>
		</serviceRequest>               
	</dataElementsIndiv>
</xsl:template>
<!-- *********************************************************************** -->
<!--         		       OSI			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "OtherServiceInformation" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>OS</segmentName> 	
		</elementManagementData>                                  
 		<freetextData>   
			<freetextDetail>
				<subjectQualifier>3</subjectQualifier>
				<type>28</type>
				<companyId><xsl:value-of select="Airline/@Code"/></companyId>
			</freetextDetail>
			<longFreetext><xsl:value-of select="Text"/></longFreetext>
		</freetextData>               
	</dataElementsIndiv>
</xsl:template>
<!-- *********************************************************************** -->
<!--         		       General remark			 	  			  -->
<!-- *********************************************************************** --> 
<xsl:template match = "Remark" >
	 <dataElementsIndiv> 
		<elementManagementData>
			<segmentName>RM</segmentName> 	
		</elementManagementData>                                  
 		<miscellaneousRemark>
	   		<remarks>
	   			<type>RM</type>
	   	   		<freetext><xsl:value-of select="."/></freetext> 
	   		</remarks>
	   </miscellaneousRemark>              
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
					<xsl:value-of select="@PseudoCityCode" />
				</longFreetext>
			</freetextData>
		</dataElementsIndiv>
	</xsl:template>
	<xsl:template match="POS" mode="Open">
		<SessionCreateRQ>
			<xsl:attribute name="Version">
				<xsl:text>1.01</xsl:text>
			</xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
		</SessionCreateRQ>
	</xsl:template>
	<xsl:template match="POS" mode="Close">
		<SessionCloseRQ>
			<xsl:attribute name="Version">
				<xsl:text>1.01</xsl:text>
			</xsl:attribute>
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="Source/@PseudoCityCode" />
					</xsl:attribute>
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
						<value>2</value>
					</referenceDetails>
				</paxTattooNbr>
			</pnrInfo>
			<sellData>
				<companyIdentification>
					<travelSector>CAR</travelSector>
					<companyCode>
						<xsl:value-of select="VendorPref/@Code" />
					</companyCode>
					<xsl:if test="VendorPref/@CompanyShortName !=''">
						<companyName>
							<xsl:value-of select="VendorPref/@CompanyShortName" />
						</companyName>
					</xsl:if>
					<xsl:if test="VendorPref/@CodeContext !=''">
						<accessLevel>
							<xsl:choose>
								<xsl:when test="VendorPref/@CodeContext = 'Complete Access'">CA</xsl:when>
								<xsl:when test="VendorPref/@CodeContext = 'Complete Access Plus'">CP</xsl:when>
								<xsl:when test="VendorPref/@CodeContext = 'Standard Access'">SA</xsl:when>
							</xsl:choose>
						</accessLevel>
					</xsl:if>
				</companyIdentification>
				<locationInfo>
					<locationType>176</locationType>
					<locationDescription>
						<code>1A</code>
						<name>
							<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode" />
						</name>
					</locationDescription>
				</locationInfo>
				<locationInfo>
					<locationType>DOL</locationType>
					<locationDescription>
						<code>1A</code>
						<name>
							<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode" />
						</name>
					</locationDescription>
				</locationInfo>
				<pickupDropoffTimes>
					<beginDateTime>
						<year>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),1,4)" />
						</year>
						<month>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),6,2)" />
						</month>
						<day>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),9,2)" />
						</day>
						<hour>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),12,2)" />
						</hour>
						<minutes>
							<xsl:value-of select="substring(string(VehRentalCore/@PickUpDateTime),15,2)" />
						</minutes>
					</beginDateTime>
					<endDateTime>
						<year>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),1,4)" />
						</year>
						<month>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),6,2)" />
						</month>
						<day>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),9,2)" />
						</day>
						<hour>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),12,2)" />
						</hour>
						<minutes>
							<xsl:value-of select="substring(string(VehRentalCore/@ReturnDateTime),15,2)" />
						</minutes>
					</endDateTime>
				</pickupDropoffTimes>
				<vehicleInformation>
					<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
					<vehicleRentalNeedType>
						<vehicleTypeOwner>ACR</vehicleTypeOwner>
						<vehicleRentalPrefType>
							<xsl:value-of select="VehPref/VehType/@VehicleCategory" />
						</vehicleRentalPrefType>
					</vehicleRentalNeedType>
				</vehicleInformation>
				<xsl:if test="../VehResRQInfo/ArrivalDetails">
					<arrivalInfo>
						<inboundCarrierDetails>
							<carrier>
								<xsl:value-of select="../VehResRQInfo/ArrivalDetails/MarketingCompany/@Code" />
							</carrier>
						</inboundCarrierDetails>
						<inboundFlightDetails>
							<flightNumber>
								<xsl:value-of select="../VehResRQInfo/ArrivalDetails/@Number" />
							</flightNumber>
						</inboundFlightDetails>
						<inboundArrivalDate>
							<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),1,4)" />
							<xsl:value-of select="substring(string	(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),6,2)" />
							<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),9,2)" />
							<xsl:value-of select="substring(string	(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),12,2)" />
							<xsl:value-of select="substring(string(../VehResRQInfo/ArrivalDetails/@ArrivalDateTime),15,2)" />
						</inboundArrivalDate>
					</arrivalInfo>
				</xsl:if>
				<rateCodeInfo>
					<fareCategories>
						<fareType>
							<xsl:value-of select="RateQualifier/@RateQualifier" />
						</fareType>
					</fareCategories>
				</rateCodeInfo>
				<xsl:if test="RateQualifier/@CorpDiscountNmbr != ''">
					<customerInfo>
						<customerReferences>
							<referenceQualifier>CD</referenceQualifier>
							<referenceNumber>
								<xsl:value-of select="RateQualifier/@CorpDiscountNmbr" />
							</referenceNumber>
						</customerReferences>
					</customerInfo>
				</xsl:if>
				<rateInfo>
					<tariffInfo>
						<rateType>
							<xsl:value-of select="RateQualifier/@RateCategory" />
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
								<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardCode" />
							</vendorCode>
							<creditCardNumber>
								<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@CardNumber" />
							</creditCardNumber>
							<expiryDate>
								<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '1']/PaymentCard/@ExpireDate" />
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
									<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardCode" />
								</vendorCode>
								<creditCardNumber>
									<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@CardNumber" />
								</creditCardNumber>
								<expiryDate>
									<xsl:value-of select="../VehResRQInfo/RentalPaymentPref[position() = '2']/PaymentCard/@ExpireDate" />
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
		<PoweredHotel_Sell>
			<productInformation>
				<xsl:apply-templates select="RoomStays/RoomStay" />
			</productInformation>
		</PoweredHotel_Sell>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<xsl:template match="RoomStay">
		<hotelPropertyInfo>
			<hotelReference>
				<chainCode>
					<xsl:value-of select="BasicPropertyInfo/@ChainCode" />
				</chainCode>
				<cityCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCityCode" />
				</cityCode>
				<hotelCode>
					<xsl:value-of select="BasicPropertyInfo/@HotelCode" />
				</hotelCode>
			</hotelReference>
		</hotelPropertyInfo>
		<requestedDates>
			<businessSemantic>CHK</businessSemantic>
			<timeMode>LT</timeMode>
			<beginDateTime>
				<year>
					<xsl:value-of select="substring(string(TimeSpan/@Start),1,4)" />
				</year>
				<month>
					<xsl:value-of select="substring(string(TimeSpan/@Start),6,2)" />
				</month>
				<day>
					<xsl:value-of select="substring(string(TimeSpan/@Start),9,2)" />
				</day>
			</beginDateTime>
			<endDateTime>
				<year>
					<xsl:value-of select="substring(string(TimeSpan/@End),1,4)" />
				</year>
				<month>
					<xsl:value-of select="substring(string(TimeSpan/@End),6,2)" />
				</month>
				<day>
					<xsl:value-of select="substring(string(TimeSpan/@End),9,2)" />
				</day>
			</endDateTime>
		</requestedDates>
		<roomRateDetails>
			<roomInformation>
				<xsl:choose>
					<xsl:when test="BasicPropertyInfo/@HotelCodeContext='SA' or BasicPropertyInfo/@HotelCodeContext='CA'">
						<roomRateIdentifier>
							<roomType>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),1,3)" />
							</roomType>
							<ratePlanCode>
								<xsl:value-of select="substring(string(RoomRates/RoomRate/@BookingCode),4,3)" />
							</ratePlanCode>
						</roomRateIdentifier>
					</xsl:when>
					<xsl:otherwise>
						<bookingCode>
							<xsl:value-of select="RoomRates/RoomRate/@BookingCode" />
						</bookingCode>
					</xsl:otherwise>
				</xsl:choose>
				<guestCountDetails>
					<numberOfUnit>
						<xsl:value-of select="GuestCounts/GuestCount/@Count" />
					</numberOfUnit>
					<unitQualifier>A</unitQualifier>
				</guestCountDetails>
			</roomInformation>
			<xsl:if test="RoomTypes/RoomType/TPA_Extensions/HotelData/ExtraAdult !='' or RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayAdult !='' or 	RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayChild !='' or RoomTypes/RoomType/TPA_Extensions/HotelData/Crib !=''"></xsl:if>
		</roomRateDetails>
		<relatedProduct>
			<quantity>
				<xsl:value-of select="RoomRates/RoomRate/@NumberOfUnits" />
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
				</xsl:when>
				<xsl:when test="DepositPayments/RequiredPayment/AcceptedPayment/PaymentCard!=''">
					<paymentInfo>
						<paymentDetails>
							<formOfPaymentCode>ADV</formOfPaymentCode>
							<paymentType>2</paymentType>
							<serviceToPay>3</serviceToPay>
						</paymentDetails>
					</paymentInfo>
				</xsl:when>
			</xsl:choose>
			<xsl:if test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!= ''">
				<creditCardInfo>
					<formOfPayment>
						<type>CC</type>
						<vendorCode>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode" />
						</vendorCode>
						<creditCardNumber>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardNumber" />
						</creditCardNumber>
						<expiryDate>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate" />
						</expiryDate>
					</formOfPayment>
				</creditCardInfo>
			</xsl:if>
		</guaranteeOrDeposit>
		<referenceForSegment>
			<xsl:apply-templates select="../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler[1]"
				mode="hotel" />
		</referenceForSegment>
	</xsl:template>
	<!--***********************************************************************************************************-->
	<xsl:template match="Traveler" mode="hotel">
		<referenceDetails>
			<type>PT</type>
			<value>
				<xsl:value-of select="TravelerRefNumber/@RPH + 1" />
			</value>
		</referenceDetails>
	</xsl:template>
	
	<xsl:template match="PriceData">
		<FarePlus_PriceItinerary_Query>
			<TSTFlag>T</TSTFlag>
			<LowestFareOption>T</LowestFareOption>
			<xsl:choose>
				<xsl:when test="@PriceType = 'Private'">
					<CAPI_FXXPsgrPlus>
						<PassengerNumber>
							<xsl:for-each select="../PNRData/Traveler">
								<xsl:value-of select="position()"/>
								<xsl:if test="position() != last()">
									<xsl:text>,</xsl:text>
								</xsl:if>
							</xsl:for-each>
						</PassengerNumber>
						<PTC1><xsl:value-of select="NegoFares/PriceRequestInformation/NegotiatedFareCode"/></PTC1>
					</CAPI_FXXPsgrPlus>
					<CAPI_TicketingIndicatorsPlus>
						<PricingTicketingIndicator>RU</PricingTicketingIndicator>
						<PricingTicketingIndicator>JCB</PricingTicketingIndicator>
					</CAPI_TicketingIndicatorsPlus>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="not(PublishedFares/FareRestrictPref)">NR</xsl:when>
						<xsl:otherwise>
							<xsl:if test="PublishedFares/FareRestrictPref/MinimumStay">
								<OtherRestrictions>NMN</OtherRestrictions>               <!--  No minimum stay -->
							</xsl:if>
						    <xsl:if test="PublishedFares/FareRestrictPref/MaximumStay">
							<OtherRestrictions>NMX</OtherRestrictions>               <!--  No maximum stay -->
						    </xsl:if>
						    <xsl:if test="PublishedFares/FareRestrictPref/AdvResTicketing/AdvReservation">
							<OtherRestrictions>NAP</OtherRestrictions>               <!--  No advance purchase Fares -->
						    </xsl:if>
						    <xsl:if test="PublishedFares/FareRestrictPref/VoluntaryChanges/Penalty">
							<OtherRestrictions>NPE</OtherRestrictions>               <!--  No penalties Fares -->
						    </xsl:if>
						</xsl:otherwise>
					</xsl:choose>
					<SellingCity></SellingCity>
					<TicketingCity></TicketingCity>
					<!-- ************************************************************************************************* -->
					<!-- Private (Nego) or Published (Normal) fares requested					       -->	
					<!-- ************************************************************************************************* -->
				    <CAPI_TicketingIndicatorsPlus>
					    <PricingTicketingIndicator>RP</PricingTicketingIndicator>	
				    </CAPI_TicketingIndicatorsPlus>
				</xsl:otherwise>
			</xsl:choose>					
		</FarePlus_PriceItinerary_Query>
	</xsl:template>

</xsl:stylesheet>