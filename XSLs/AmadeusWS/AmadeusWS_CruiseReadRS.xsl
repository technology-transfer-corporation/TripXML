<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruiseReadRS.xsl 	     											       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Cruise_GetBookingDetailsReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseReadRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseReadRS>
</xsl:template>
	
<xsl:template match="CAPI_Messages">
	<Error>
		<xsl:attribute name="Code">
			<xsl:value-of select="ErrorCode" />
		</xsl:attribute>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="Text" />
	</Error>
</xsl:template>
<!-- Amadeus form of payment code to be added-->

<xsl:template match="Cruise_GetBookingDetailsReply">
	<OTA_CruiseReadRS>
		<xsl:attribute name="Version">1.000</xsl:attribute>
		<xsl:choose>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('2')">
				<xsl:element name="Errors">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="error"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('4')">
				<xsl:element name="Success"/>
				<xsl:element name="Warnings">
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="warning"/>
				</xsl:element>
			</xsl:when>
			<xsl:when test="not(advisoryMessage)">
				<xsl:element name="Success"/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="(not(advisoryMessage)) or (advisoryMessage/errorQualifierDescription/messageType=('4'))">
			<TravelItinerary>
				<ItineraryInfo>
					<ReservationItems>
						<Item>
							<Cruise>
								<SailingInfo>
									<SelectedSailing>
										<xsl:apply-templates select="sailingGroup/sailingDescription/sailingIndicators"/>
										<xsl:apply-templates select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNbr"/>
										<xsl:attribute name="Start">
											<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,5,4)"/>
											<xsl:text>-</xsl:text>	
											<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,3,2)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,1,2)"/>
											<xsl:text>T00:00:00</xsl:text>
										</xsl:attribute>
										<xsl:attribute name="Duration">
											<xsl:value-of select="sailingGroup/sailingDescription/sailingDateTime/sailingDuration"/>	
										</xsl:attribute>
										<xsl:attribute name="VendorCode">
											<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/cruiselineCode"/>	
										</xsl:attribute>
										<xsl:attribute name="VendorName"></xsl:attribute>
										<xsl:attribute name="ShipCode">
											<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/shipCode"/>	
										</xsl:attribute>
										<xsl:attribute name="ShipName"></xsl:attribute>
										<xsl:attribute name="Status">
											<xsl:choose>
												<xsl:when test="sailingGroup/sailingDescription/sailingDetails/sailingStatusCode = 'AVL'">Available</xsl:when>
												<xsl:when test="sailingGroup/sailingDescription/sailingDetails/sailingStatusCode = 'CLO'">Unavailable</xsl:when>
												<xsl:when test="sailingGroup/sailingDescription/sailingDetails/sailingStatusCode = 'ONR'">OnRequest</xsl:when>
												<xsl:when test="not(sailingGroup/sailingDescription/sailingDetails/sailingStatusCode)">Available</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="sailingGroup/sailingDescription/sailingDetails/sailingStatusCode"/>	
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</SelectedSailing>
									<xsl:apply-templates select="sailingGroup/inclusivePackage"/>
									<xsl:apply-templates select="sailingGroup/currencyInfo"/>
									<SelectedCategory>
										<xsl:attribute name="BerthedCategoryCode">
											<xsl:value-of select="sailingGroup/categoryInfo/categoryId/berthedCategory"/>	
										</xsl:attribute>
										<xsl:apply-templates select="sailingGroup/categoryInfo/categoryId/pricedCategory"/>
										<SelectedCabin>
											<xsl:attribute name="CabinNumber">
												<xsl:value-of select="sailingGroup/cabinInfo/cabinDetails/cabinNbr"/>
											</xsl:attribute>
											<xsl:apply-templates select="sailingGroup/cabinInfo/bedDetails/bedConfiguration[1]"/>
											<xsl:apply-templates select="sailingGroup/cabinInfo/bedDetails/bedConfiguration"/>
										</SelectedCabin>
									</SelectedCategory>
									<xsl:apply-templates select="modeOfTransportation"/>
									<xsl:apply-templates select="sailingGroup/sailingInformation"/>
								</SailingInfo>
								<ReservationInfo>
									<xsl:apply-templates select="bookingQualifier[partyQualifier=('1')]"/>
									<xsl:apply-templates select="reservationControlInfo"/>
									<ReservationDetails>
										<xsl:apply-templates select="sailingGroup/sailingQualifier"/>
										<xsl:apply-templates select="sailingGroup/travellerGroup"/>
										<xsl:apply-templates select="sailingGroup/travellerGroup/emergencyInfo"/>
										<xsl:apply-templates select="sailingGroup/travellerGroup/documentInfo"/>
										<xsl:apply-templates select="diningGroup"/>
										<xsl:apply-templates select="insuranceGroup"/>
										<xsl:if test="packageGroup">
											<SelectedPackages>
												<xsl:apply-templates select="packageGroup"/>
											</SelectedPackages>
										</xsl:if>
										<xsl:if test="transferGroup">
											<SelectedTransfers>
												<xsl:apply-templates select="transferGroup"/>
											</SelectedTransfers>
										</xsl:if>
										<xsl:apply-templates select="faxInfo[faxDetails/faxType != '']"/>
									</ReservationDetails>
									<xsl:if test="paymentGroup">
										<AcceptedPayments>
											<xsl:apply-templates select="paymentGroup/paymentInfo/creditCardInfo"/>
											<!--xsl:apply-templates select="paymentGroup/paymentInfo/paymentDetails"/-->
										</AcceptedPayments>
									</xsl:if>
									<xsl:if test="depositDateTime/dateTimeDescription[dateTimeQualifier != '170'] and depositDateTime/dateTimeDescription[dateTimeDetails != '']">
										<PaymentSchedule>
											<xsl:apply-templates select="depositDateTime"/>
										</PaymentSchedule>
									</xsl:if>
								</ReservationInfo>								
							</Cruise>
						</Item>					
					</ReservationItems>					
				</ItineraryInfo>
			</TravelItinerary>
		</xsl:if>
	</OTA_CruiseReadRS>
</xsl:template>

<xsl:template match="errorQualifierDescription" mode="error">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
		</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="errorQualifierDescription" mode="warning">
	<xsl:element name="Warning">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="sailingIndicators">
	<xsl:attribute name="ListOfSailingDescriptionCode">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="cruiseVoyageNbr">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="inclusivePackage">
	<InclusivePackageOption>
		<xsl:attribute name="CruisePackageCode">
			<xsl:value-of select="packageDetails/packageCode"/>	
		</xsl:attribute>
		<xsl:attribute name="InclusiveIndicator">true</xsl:attribute>
		<xsl:attribute name="StartDate">
			<xsl:value-of select="substring(packageDateTime/packageStartDate,5,4)"/>
			<xsl:text>-</xsl:text>	
			<xsl:value-of select="substring(packageDateTime/packageStartDate,3,2)"/>
			<xsl:text>-</xsl:text>
			<xsl:value-of select="substring(packageDateTime/packageStartDate,1,2)"/>
		</xsl:attribute>
	</InclusivePackageOption>
</xsl:template>

<xsl:template match="currencyInfo">
	<Currency>
		<xsl:attribute name="CurrencyCode">
			<xsl:value-of select="currencyList/currencyIsoCode"/>	
		</xsl:attribute>
	</Currency>
</xsl:template>

<xsl:template match="pricedCategory">
	<xsl:attribute name="PricedCategoryCode">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="bedConfiguration[1]">
	<CabinConfiguration>
		<xsl:attribute name="SelectedIndicator">
			<xsl:value-of select="('Y')"/>	
		</xsl:attribute>
		<xsl:attribute name="BedConfigurationCode">
			<xsl:value-of select="."/>	
		</xsl:attribute>
	</CabinConfiguration>
</xsl:template>

<xsl:template match="bedConfiguration">
	<CabinConfiguration>
		<xsl:attribute name="SelectedIndicator">
			<xsl:value-of select="('N')"/>	
		</xsl:attribute>
		<xsl:attribute name="BedConfigurationCode">
			<xsl:value-of select="."/>	
		</xsl:attribute>
	</CabinConfiguration>
</xsl:template>

<xsl:template match="modeOfTransportation">
	<Transportation>
		<xsl:attribute name="TransportationMode">
			<xsl:choose>
				<xsl:when test="transportationInfo/modeOfTransport ='C'">29</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='A'">32</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='O'">33</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='F'">30</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='T'">31</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='B'">3</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='R'">21</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="motStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="motStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="motStatus = 'WTL'">Waitlist</xsl:when>
				<xsl:when test="motStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:otherwise><xsl:value-of select="motStatus"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</Transportation>
</xsl:template>

<xsl:template match="sailingInformation">
	<xsl:element name="Information">
		<xsl:attribute name="Name">
			<xsl:value-of select="('SailingInfo')"/>	
		</xsl:attribute>
		<xsl:element name="Text">
			<xsl:value-of select="textDetails"/>	
		</xsl:element>
	</xsl:element>
</xsl:template>

<xsl:template match="bookingQualifier[partyQualifier=('1')]">
	<ReservationID>
		<xsl:attribute name="Type">CruiseLine</xsl:attribute>
		<xsl:attribute name="ID">
			<xsl:value-of select="itemDescription/value"/>
		</xsl:attribute>
		<xsl:attribute name="ID_Context">
			<xsl:value-of select="('ConfirmationNumber')"/>
		</xsl:attribute>
		<xsl:attribute name="StatusCode">
			<xsl:choose>
				<xsl:when test="itemDescription/status = 'ACT'">Active</xsl:when>
				<xsl:when test="itemDescription/status = 'CNF'">Confirmed</xsl:when>
				<xsl:when test="itemDescription/status = 'OFR'">Offered</xsl:when>
				<xsl:when test="itemDescription/status = 'PCN'">PendingConfirmation</xsl:when>
				<xsl:when test="itemDescription/status = 'CAN'">CancelledReinstatable</xsl:when>
				<xsl:when test="itemDescription/status = 'CAX'">CancelledNotReinstatable</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="itemDescription/status"/>	
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
	</ReservationID>
</xsl:template>

<xsl:template match="reservationControlInfo">
	<ReservationID>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="ID">
			<xsl:value-of select="reservationDetails/pnrRecordLocator"/>
		</xsl:attribute>
		<xsl:if test="reservationDetails/synchronisedDate != ''">
			<xsl:attribute name="SyncDateTime">
				<xsl:value-of select="reservationDetails/synchronisedDate"/>
				<xsl:value-of select="reservationDetails/synchronisedTime"/>
			</xsl:attribute>
		</xsl:if>
	</ReservationID>
</xsl:template>

<xsl:template match="sailingQualifier">
	<BookingCruiseProfiles>
		<xsl:attribute name="ProfileTypeIdentifier">
			<xsl:value-of select="profileType"/>
		</xsl:attribute>
		<xsl:apply-templates select="attributeInfo"/>
	</BookingCruiseProfiles>
</xsl:template>

<xsl:template match="attributeInfo">
	<CruiseProfile>
		<xsl:attribute name="Code">
			<xsl:value-of select="attributeType"/>
		</xsl:attribute>
		<xsl:apply-templates select="attribute"/>
	</CruiseProfile>
</xsl:template>

<xsl:template match="attribute">
	<xsl:attribute name="MaxQuantity">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="travellerGroup">
	<ContactInfo>
		<xsl:attribute name="EmergencyFlag">
			<xsl:value-of select="('false')"/>
		</xsl:attribute>
		<xsl:attribute name="RPH">
			<xsl:value-of select="passengerInfo/travellerDetails/referenceNbr"/>
		</xsl:attribute>
		<xsl:attribute name="Gender">
			<xsl:value-of select="passengerInfo/travellerDetails/passengerGender"/>
		</xsl:attribute>
		<xsl:attribute name="Age">
			<xsl:value-of select="passengerInfo/travellerDetails/age"/>
		</xsl:attribute>
		<xsl:apply-templates select="documentInfo/travellerDocumentDetails/nationality"/>
		<!--xsl:apply-templates select="paxQualifier[partyQualifier=('5')]"/>
		<xsl:apply-templates select="paxQualifier[partyQualifier=('13')]"/-->
		<xsl:apply-templates select="birthDate"/>
		<PersonName>
			<GivenName>
				<xsl:value-of select="passengerInfo/travellerDetails/nameId"/>
			</GivenName>
			<Surname>
				<xsl:value-of select="passengerInfo/travellerId/lastName"/>
			</Surname>
			<NameTitle>
				<xsl:value-of select="passengerInfo/travellerDetails/passengerTitle"/>
			</NameTitle>
		</PersonName>
		<xsl:apply-templates select="contactInfo[contactDetails/partyQualifierType=('P')]"/>
		<xsl:apply-templates select="paxAddress[addressQualifier/address=('PAX')]"/>
		<xsl:apply-templates select="paxAddress/locationDetails/emailAddress"/>
		<SelectedFareCode>
			<xsl:attribute name="FareCode">
				<xsl:value-of select="fareCode/fareCodeId/cruiseFareCode"/>
			</xsl:attribute>
			<xsl:apply-templates select="../passengerGroupId/passengerGroupInfoId/groupCode"/>
		</SelectedFareCode>
		<xsl:apply-templates select="selectedTransportationInfo"/>
		<xsl:apply-templates select="passengerQualifier"/>	
	</ContactInfo>
</xsl:template>

<xsl:template match="nationality">
	<xsl:attribute name="Nationality">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="paxQualifier[partyQualifier=('5')]">
	<xsl:attribute name="LoyaltyMembershipID">
		<xsl:value-of select="itemDescription/value"/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="paxQualifier[partyQualifier=('13')]">
	<xsl:attribute name="GuestOccupation">
		<xsl:value-of select="componentDetails/componentDescription"/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="birthDate">
	<xsl:if test="DateTimeDescription/DateTimeDetails != '00000000'">
		<xsl:attribute name="BirthDate">
			<xsl:value-of select="DateTimeDescription/DateTimeDetails"/>
		</xsl:attribute>
	</xsl:if>
</xsl:template>

<xsl:template match="contactInfo[contactDetails/partyQualifierType=('P')]">
	<Telephone>
		<xsl:attribute name="PhoneNumber">
			<xsl:value-of select="contactDetails/phoneNumber"/>
		</xsl:attribute>
	</Telephone>
</xsl:template>

<xsl:template match="paxAddress[addressQualifier/address=('PAX')]">
	<Address>
		<AddressLine>
			<xsl:value-of select="addressDetails/addressLine1"/>
		</AddressLine>
		<AddressLine>
			<xsl:value-of select="addressDetails/addressLine2"/>
		</AddressLine>
		<CityName>
			<xsl:value-of select="cityName"/>
		</CityName>
		<PostalCode>
			<xsl:value-of select="zipCode"/>
		</PostalCode>
		<StateProv>
			<xsl:value-of select="locationDetails/stateCode"/>
		</StateProv>
		<CountryName>
			<xsl:value-of select="countryIsoCode"/>
		</CountryName>
	</Address>
</xsl:template>

<xsl:template match="emailAddress">
	<Email>
		<xsl:value-of select="."/>
	</Email>
</xsl:template>

<xsl:template match="groupCode">
	<xsl:attribute name="GroupCode">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="selectedTransportationInfo">
	<GuestTransportation>
		<xsl:attribute name="TransportationMode">
			<xsl:choose>
				<xsl:when test="transportationInfo/modeOfTransport ='C'">29</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='A'">32</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='O'">33</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='F'">30</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='T'">31</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='B'">3</xsl:when>
				<xsl:when test="transportationInfo/modeOfTransport ='R'">21</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:if test="motStatus != ''">
			<xsl:attribute name="TransportationStatus">
				<xsl:choose>
					<xsl:when test="motStatus = 'AVL'">Available</xsl:when>
					<xsl:when test="motStatus = 'ONR'">OnRequest</xsl:when>
					<xsl:when test="motStatus = 'WTL'">Waitlist</xsl:when>
					<xsl:when test="motStatus = 'CLO'">Unavailable</xsl:when>
					<xsl:otherwise><xsl:value-of select="motStatus"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:if>
		<xsl:apply-templates select="../paxAddress[addressQualifier/address=('GST')]"/>
		<xsl:apply-templates select="transportationInfo/motCity"/>

	</GuestTransportation>
</xsl:template>

<xsl:template match="paxAddress[addressQualifier/address=('GST')]">
	<GuestCity>
		<xsl:value-of select="cityName"/>
	</GuestCity>
</xsl:template>

<xsl:template match="motCity">
	<GatewayCity>
		<xsl:attribute name="LocationCode">
			<xsl:value-of select="."/>
		</xsl:attribute>
	</GatewayCity>
</xsl:template>

<xsl:template match="passengerQualifier">
	<GuestCruiseProfiles>
		<xsl:attribute name="ProfileTypeIdentifier">
			<xsl:value-of select="profileType"/>
		</xsl:attribute>
		<xsl:apply-templates select="attributeInfo"/>
	</GuestCruiseProfiles>
</xsl:template>

<xsl:template match="emergencyInfo">
	<ContactInfo>
		<xsl:attribute name="EmergencyFlag">
			<xsl:value-of select="('Y')"/>
		</xsl:attribute>
		<PersonName>
			<GivenName>
				<xsl:value-of select="travellerDetails/nameId"/>
			</GivenName>
			<Surname>
				<xsl:value-of select="travellerId/lastName"/>
			</Surname>
			<NameTitle>
				<xsl:value-of select="travellerDetails/passengerTitle"/>
			</NameTitle>
		</PersonName>
		<xsl:apply-templates select="../contactInfo[contactDetails/partyQualifierType=('H')]"/>
		<xsl:apply-templates select="../paxAddress[addressQualifier/address=('HLP')]"/>
	</ContactInfo>
</xsl:template>

<xsl:template match="contactInfo[contactDetails/partyQualifierType=('H')]">
	<Telephone>
		<xsl:attribute name="PhoneNumber">
			<xsl:value-of select="contactDetails/phoneNumber"/>
		</xsl:attribute>
	</Telephone>
</xsl:template>

<xsl:template match="paxAddress[addressQualifier/address=('HLP')]">
	<Address>
		<AdressLine>
			<xsl:value-of select="addressDetails/addressLine1"/>
		</AdressLine>
		<AdressLine>
			<xsl:value-of select="addressDetails/addressLine2"/>
		</AdressLine>
		<CityName>
			<xsl:value-of select="cityName"/>
		</CityName>
		<PostalCode>
			<xsl:value-of select="zipCode"/>
		</PostalCode>
		<PostalCode>
			<xsl:value-of select="zipCode"/>
		</PostalCode>
		<StateProv>
			<xsl:value-of select="locationDetails/stateCode"/>
		</StateProv>
		<CountryName>
			<xsl:value-of select="countryIsoCode"/>
		</CountryName>
	</Address>
</xsl:template>

<xsl:template match="documentInfo">
	<TravelDocument>
		<xsl:attribute name="DocID">
			<xsl:value-of select="travellerDocumentDetails/documentNbr"/>
		</xsl:attribute>
		<xsl:attribute name="EffectiveDate">
			<xsl:value-of select="documentValidity/issueDate"/>
		</xsl:attribute>
		<xsl:attribute name="ExpireDate">
			<xsl:value-of select="documentValidity/expirationDate"/>
		</xsl:attribute>
		<xsl:if test="travellerDocumentDetails/issuanceCountry != ''">
			<xsl:attribute name="DocIssueCountry">
				<xsl:value-of select="travellerDocumentDetails/issuanceCountry"/>
			</xsl:attribute>
		</xsl:if>
		<DocHolderName>
			<xsl:value-of select="../passengerInfo/travellerDetails/passengerTitle"/>
			<xsl:text> </xsl:text>
			<xsl:value-of select="../passengerInfo/travellerDetails/nameId"/>
			<xsl:text> </xsl:text>
			<xsl:value-of select="../passengerInfo/travellerId/lastName"/>
		</DocHolderName>
	</TravelDocument>
</xsl:template>

<xsl:template match="diningGroup">
	<Dining>
		<xsl:attribute name="SelectedIndicator">
			<xsl:value-of select="diningDetails/diningIdentification/seatingQualifierId"/>
		</xsl:attribute>
		<xsl:apply-templates select="diningDetails/roomDetails/smokingIndicator"/>
		<xsl:apply-templates select="diningDetails/roomDetails/diningRoomId"/>
		<xsl:apply-templates select="diningDetails/guestTableSize"/>
		<xsl:apply-templates select="diningDetails/diningGuestAge"/>
		<xsl:apply-templates select="diningDetails/roomDetails/preferedIsoLanguage"/>
		<xsl:attribute name="Sitting">
			<xsl:value-of select="diningDetails/diningIdentification/diningLabel"/>
		</xsl:attribute>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="diningDetails/diningIdentification/diningStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="diningDetails/diningIdentification/diningStatus = 'CNF'">Confirmed</xsl:when>
				<xsl:when test="diningDetails/diningIdentification/diningStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="diningDetails/diningIdentification/diningStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="not(diningDetails/diningIdentification/diningStatus)">OnRequest</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="diningDetails/diningIdentification/diningStatus"/>	
				</xsl:otherwise>
			</xsl:choose>	
		</xsl:attribute>
	</Dining>
	<xsl:apply-templates select="diningAssociation"/>
</xsl:template>

<xsl:template match="smokingIndicator">
	<xsl:attribute name="SmokingCode">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="diningRoomId">
	<xsl:attribute name="DiningRoom">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="guestTableSize">
	<xsl:attribute name="TableSize">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="diningGuestAge">
	<xsl:attribute name="AgeCode">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="preferedIsoLanguage">
	<xsl:attribute name="Language">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="diningAssociation">
	<GuestAssociation>
		<xsl:attribute name="GuestRPH">
			<xsl:value-of select="travellerId/lastName"/>
		</xsl:attribute>
	</GuestAssociation>	
</xsl:template>

<xsl:template match="insuranceGroup">
	<Insurance>
		<xsl:attribute name="SelectedIndicator">
			<xsl:value-of select="insuranceInfo/insuranceList/defaultIndicator"/>
		</xsl:attribute>
		<xsl:attribute name="InsuranceCode">
			<xsl:value-of select="insuranceInfo/insuranceList/insuranceCode"/>
		</xsl:attribute>
		<xsl:apply-templates select="insuranceAssociation"/>
	</Insurance>
</xsl:template>

<xsl:template match="insuranceAssociation">
	<GuestAssociation>
		<xsl:attribute name="GuestRPH">
			<xsl:value-of select="travellerId/lastName"/>
		</xsl:attribute>
	</GuestAssociation>	
</xsl:template>

<xsl:template match="faxInfo">
	<FaxDocument>
		<xsl:attribute name="SelectedIndicator">
			<xsl:value-of select="faxDetails/faxQualifier"/>
		</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="faxDetails/faxType"/>
		</xsl:attribute>
	</FaxDocument>
</xsl:template>

<xsl:template match="creditCardInfo">
	<AcceptedPayment>
		<PaymentCard>
			<!--xsl:attribute name="FormOfPayment">
				<xsl:value-of select="('CCD')"/>
			</xsl:attribute-->
			<xsl:attribute name="CardCode">
					<xsl:value-of select="creditCardCode"/>
			</xsl:attribute>
		</PaymentCard>
	</AcceptedPayment>
</xsl:template>

<xsl:template match="paymentDetails">
	<AcceptedPayment>
		<xsl:choose>
			<xsl:when test="formOfPaymentCode = 'CCD'">
				<PaymentCard CardType="1"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'AGC'">
				<DirectBill DirectBill_ID="Agency Check"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'CAG'">
				<DirectBill DirectBill_ID="Credited Agency"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'WTR'">
				<DirectBill DirectBill_ID="Wire Transfer"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'BAG'">
				<DirectBill DirectBill_ID="Billing Agency"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'CHK'">
				<DirectBill DirectBill_ID="Check"/>
			</xsl:when>
			<xsl:when test="formOfPaymentCode = 'GRO'">
				<DirectBill DirectBill_ID="Giro"/>
			</xsl:when>
		</xsl:choose>
	</AcceptedPayment>
</xsl:template>

<xsl:template match="depositDateTime">
	<xsl:if test="dateTimeDescription/dateTimeQualifier != '170' and dateTimeDescription/dateTimeDetails != ''">
		<Payment>
			<xsl:attribute name="PaymentType">
				<xsl:value-of select="dateTimeDescription/dateTimeQualifier"/>
			</xsl:attribute>
			<xsl:attribute name="DueDate">
				<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,5,4)"/>	
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,3,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,1,2)"/>
				<xsl:if test="substring(dateTimeDescription/dateTimeDetails,9,2) != ''">
					<xsl:text>T</xsl:text>
					<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,9,2)"/>
					<xsl:text>:</xsl:text>
					<xsl:value-of select="substring(dateTimeDescription/dateTimeDetails,11,2)"/>
					<xsl:text>:00</xsl:text>
				</xsl:if>
			</xsl:attribute>
		</Payment>
	</xsl:if>
</xsl:template>

<xsl:template match="packageGroup">
	<SelectedPackage>
		<xsl:attribute name="PackageTypeCode">
			<xsl:choose>
				<xsl:when test="packageDescription/packageType = 'A'">Post</xsl:when>
				<xsl:when test="packageDescription/packageType = 'B'">Pre</xsl:when>
				<xsl:when test="packageDescription/packageType = 'S'">Shore</xsl:when>
				<xsl:when test="packageDescription/packageType = 'Z'">Bus</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="CruisePackageCode">
			<xsl:value-of select="packageDescription/packageDetails/packageCode"/>
		</xsl:attribute>
		<xsl:if test="packageDescription/packageDetails/packageDesc != ''">
			<xsl:attribute name="Description">
				<xsl:value-of select="packageDescription/packageDetails/packageDesc"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:attribute name="InclusiveIndicator">false</xsl:attribute>
		<xsl:if test="packageDescription/packageDateTime/packageStartDate != ''">
			<xsl:attribute name="StartDate">
				<xsl:value-of select="substring(packageDescription/packageDateTime/packageStartDate,1,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(packageDescription/packageDateTime/packageStartDate,3,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(packageDescription/packageDateTime/packageStartDate,5,4)"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="packageDescription/packageStatus = 'AVL'">Available</xsl:when>
				<xsl:when test="packageDescription/packageStatus = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="packageDescription/packageStatus = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="packageDescription/packageStatus = 'WTL'">Waitlist</xsl:when>
				<xsl:when test="not(packageDescription/packageStatus)">Available</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="packageDescription/packageStatus"/>	
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:if test="packageDescription/packageDateTime/packageDuration != ''">
			<xsl:attribute name="Duration">
				<xsl:value-of select="packageDescription/packageDateTime/packageDuration"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:choose>
			<xsl:when test="packageAssociation">
				<xsl:apply-templates select="packageAssociation"/>
			</xsl:when>
			<xsl:when test="qualifierNumberGroup">
				<xsl:if test="qualifierNumberGroup/otherPackageFeatures/nbrOfUnitsDetails/unitQualifier = 'RO'">
					<xsl:attribute name="HotelRoomRPH">
						<xsl:value-of select="qualifierNumberGroup/otherPackageFeatures/nbrOfUnitsDetails/unitValue"/>
					</xsl:attribute>
				</xsl:if>
				<xsl:apply-templates select="qualifierNumberGroup/roomAssociation"/>
			</xsl:when>
		</xsl:choose>
	</SelectedPackage>
</xsl:template>

<xsl:template match="transferGroup">
	<SelectedTransfer>
		<xsl:attribute name="TransferType">
			<xsl:choose>
				<xsl:when test="transferInfo/transferDetails/transferType = 'A'">Pre</xsl:when>
				<xsl:when test="transferInfo/transferDetails/transferType = 'D'">Post</xsl:when>
				<xsl:when test="transferInfo/transferDetails/transferType = 'R'">Round</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="TransferCode">
			<xsl:value-of select="transferInfo/transferDetails/transferCode"/>
		</xsl:attribute>
		<xsl:if test="transferInfo/transferDetails/transferDescription != ''">
			<xsl:attribute name="Description">
				<xsl:value-of select="transferInfo/transferDetails/transferDescription"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferInfo/dateTime/transferDate != ''">
			<xsl:attribute name="StartDateTime">
				<xsl:value-of select="substring(transferInfo/dateTime/transferDate,1,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(transferInfo/dateTime/transferDate,3,2)"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring(transferInfo/dateTime/transferDate,5,4)"/>
				<xsl:choose>
					<xsl:when test="transferInfo/dateTime/transferTime != ''">
						<xsl:text>T</xsl:text>
						<xsl:value-of select="substring(transferInfo/dateTime/transferTime,1,2)"/>
						<xsl:text>-</xsl:text>
						<xsl:value-of select="substring(transferInfo/dateTime/transferTime,3,2)"/>
						<xsl:text>:00</xsl:text>
					</xsl:when>
					<xsl:otherwise>T00:00:00</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferInfo[1]/transferLocation/boardOffPoints != ''">
			<xsl:attribute name="BoardPoint">
				<xsl:value-of select="transferInfo/transferLocation/boardOffPoints"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferInfo[position() = 2]/transferLocation/boardOffPoints != ''">
			<xsl:attribute name="OffPoint">
				<xsl:value-of select="transferInfo[position() = 2]/transferLocation/boardOffPoints"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:if test="transferAssociation">
			<xsl:apply-templates select="transferAssociation"/>
		</xsl:if>
	</SelectedTransfer>
</xsl:template>

<xsl:template match="packageAssociation | roomAssociation | transferAssociation">
	<GuestAssociation>
		<xsl:attribute name="GuestRPH">
			<xsl:value-of select="travellerId/lastName"/>
		</xsl:attribute>
	</GuestAssociation>	
</xsl:template>

</xsl:stylesheet>