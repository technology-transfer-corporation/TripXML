<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruiseCreateBookingRQ.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 24 Nov 2007 - Rastko													        	       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruiseCreateBookingRQ">
	<CruiseByPass_CreateBooking>
		<agentEnvironment>
			<agentTerminalId>
				<xsl:value-of select="('12345678')"/>
			</agentTerminalId>
		</agentEnvironment>
		<contactInfo>
			<contactDetails>
				<partyQualifierType>
					<xsl:value-of select="('AGT')"/>
				</partyQualifierType>
				<phoneNumber>
					<xsl:value-of select="AgentInfo/@Contact"/>
				</phoneNumber>
			</contactDetails>
		</contactInfo>
		<xsl:apply-templates select="DeletedGuest"/>
		<bookingQualifier>
			<partyQualifier>
				<xsl:value-of select="('8')"/>
			</partyQualifier>
			<componentDetails>
				<componentQualifier>
					<xsl:value-of select="('10')"/>
				</componentQualifier> 
				<componentDescription>
					<xsl:value-of select="AgentInfo/@ContactID"/>
				</componentDescription>
			</componentDetails>
		</bookingQualifier>
		<nbrOfGuests>
			<nbrOfUnitsDetails>
				<unitValue>
					<xsl:choose>
						<xsl:when test="GuestCounts/GuestCount/@Quantity != ''">
							<xsl:value-of select="sum(GuestCounts/GuestCount/@Quantity)"/>
						</xsl:when>	
						<xsl:otherwise>
							<xsl:value-of select="count(ReservationInfo/GuestDetails/GuestDetail)"/>
						</xsl:otherwise>
					</xsl:choose>
				</unitValue>
				<unitQualifier>
					<xsl:value-of select="('NI')"/>
				</unitQualifier>
			</nbrOfUnitsDetails>
		</nbrOfGuests>
		<xsl:apply-templates select="SelectedFax"/>
		<sailingGroup>
			<sailingDescription>
				<providerDetails>
					<shipCode>
						<xsl:value-of select="SailingInfo/SelectedSailing/@ShipCode"/>
					</shipCode>
					<cruiselineCode>
						<xsl:value-of select="SailingInfo/SelectedSailing/@VendorCode"/>
					</cruiselineCode>
				</providerDetails>
				<sailingDateTime>
					<sailingDepartureDate>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,9,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,6,2)"/>
						<xsl:value-of select="substring(SailingInfo/SelectedSailing/@Start,1,4)"/>
					</sailingDepartureDate>
					<sailingDuration>
						<xsl:value-of select="SailingInfo/SelectedSailing/@Duration"/>
					</sailingDuration>
				</sailingDateTime>
				<xsl:apply-templates select="SailingInfo/SelectedSailing/@VoyageID"/>
				<arrivalAndDeparturePort>
					<portCode><xsl:value-of select="SailingInfo/DeparturePort/@LocationCode"/></portCode>
				</arrivalAndDeparturePort>
				<arrivalAndDeparturePort>
					<portCode><xsl:value-of select="SailingInfo/ArrivalPort/@LocationCode"/></portCode>
				</arrivalAndDeparturePort>
			</sailingDescription>
			<currencyInfo>
				<currencyList>
					<currencyQualifier>
						<xsl:value-of select="('5')"/>
					</currencyQualifier>
					<currencyIsoCode>
						<xsl:choose>
							<xsl:when test="SailingInfo/Currency/@CurrencyCode != ''">
								<xsl:value-of select="SailingInfo/Currency/@CurrencyCode"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</currencyIsoCode>
				</currencyList>
			</currencyInfo>
			<categoryInfo>
				<categoryId>
					<pricedCategory>
						<xsl:value-of select="SailingInfo/SelectedCategory/@PricedCategoryCode"/>
					</pricedCategory>
					<berthedCategory>
						<xsl:value-of select="SailingInfo/SelectedCategory/@BerthedCategoryCode"/>
					</berthedCategory>
				</categoryId>
			</categoryInfo>
			<xsl:apply-templates select="SailingInfo/InclusivePackageOption"/>
			<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail/SelectedFareCode/@GroupCode"/>
			<cabinInfo>
				<cabinDetails>
					<cabinNbr>
						<xsl:value-of select="SailingInfo/SelectedCategory/SelectedCabin/@CabinNumber"/>
					</cabinNbr>						
				</cabinDetails>
				<xsl:apply-templates select="SailingInfo/SelectedCategory/SelectedCabin/@BedConfigurationCode"/>
			</cabinInfo>
			<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail" mode="contact"/>
		</sailingGroup>
		<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining">
			<diningGroup>
				<diningDetails>
					<diningIdentification>
						<diningLabel>
							<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Sitting"/>
						</diningLabel>
						<diningStatus>
							<xsl:choose>
								<xsl:when test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Status = 'Available'">AVL</xsl:when>
								<xsl:when test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Status = 'Unavailable'">CLO</xsl:when>
								<xsl:when test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Status = 'OnRequest'">ONR</xsl:when>
								<xsl:when test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Status != ''"><xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Status"/></xsl:when>
								<xsl:otherwise>AVL</xsl:otherwise>
							</xsl:choose>	
						</diningStatus>
					</diningIdentification>
					<xsl:choose>
						<xsl:when test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@SmokingCode != '' or ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Language != '' or ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@DiningRoom != ''">
							<roomDetails>
								<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@SmokingCode != ''">
									<smokingIndicator>
										<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@SmokingCode"/>
									</smokingIndicator>
								</xsl:if>
								<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Language != ''">
									<preferredIsoLanguage>
										<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@Language"/>
									</preferredIsoLanguage>
								</xsl:if>
								<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@DiningRoom != ''">
									<diningRoomId>
										<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@DiningRoom"/>
									</diningRoomId>
								</xsl:if>											
							</roomDetails>
						</xsl:when>
					</xsl:choose>
					<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@TableSize != ''">
						<guestTableSize>
							<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@TableSize"/>
						</guestTableSize>
					</xsl:if>
					<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@AgeCode != ''">
						<diningGuestAge>
							<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining/@AgeCode"/>
						</diningGuestAge>
					</xsl:if>
				</diningDetails>
				<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining"/>
			</diningGroup>
		</xsl:if>
		<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedInsurance">
			<insuranceGroup>
				<insuranceInfo>
					<insuranceList>
						<insuranceCode>
							<xsl:value-of select="ReservationInfo/GuestDetails/GuestDetail/SelectedInsurance/@InsuranceCode"/>
						</insuranceCode>
					</insuranceList>
				</insuranceInfo>
				<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail/SelectedInsurance"/>
			</insuranceGroup>
		</xsl:if>
		<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail" mode="transfer"/>
		<xsl:apply-templates select="ReservationInfo/PaymentRequests/PaymentRequest"/>
		<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail[1]" mode="pkg"/>
		<xsl:if test="SailingInfo/SelectedSailing/@VendorCode = 'HAL' or SailingInfo/SelectedSailing/@VendorCode = 'WSC'">
			<specialServicesGroup>
				<specialServiceInfo>
					<specialServiceType>
						<itemCategory>S</itemCategory>
						<subCategory>UPG</subCategory>
					</specialServiceType>
					<specialServiceDetails>
						<specialServiceCode>Y</specialServiceCode>
						<specialServiceAsso>C</specialServiceAsso>
					</specialServiceDetails>
				</specialServiceInfo>
			</specialServicesGroup>
		</xsl:if>
	</CruiseByPass_CreateBooking>
</xsl:template>


<xsl:template match="DeletedGuest">
	<deletedGuestList>
		<travellerId>
			<lastName>
				<xsl:value-of select="@DeletedGuestReference"/>
			</lastName>
			<status>
				<xsl:value-of select="('D')"/>
			</status>
		</travellerId>
	</deletedGuestList>
</xsl:template>

<xsl:template match="SelectedFax">
	<faxInfo>
		<faxDetails>
			<faxType>
				<xsl:value-of select="@FaxType"/>
			</faxType>
		</faxDetails>
		<faxNbr>
			<xsl:value-of select="@PhoneNumber"/>
		</faxNbr>
	</faxInfo>
</xsl:template>

<xsl:template match="@VoyageID">
	<sailingId>
		<cruiseVoyageNbr>
			<xsl:value-of select="."/>
		</cruiseVoyageNbr>
	</sailingId>
</xsl:template>

<xsl:template match="InclusivePackageOption">
	<packageDescription>
		<packageType>
			<xsl:value-of select="('I')"/>
		</packageType>
		<packageDetails>
			<packageCode>
				<xsl:value-of select="@Code"/>
			</packageCode>
		</packageDetails>
		<packageDateTime>
			<packageStartDate>
				<xsl:value-of select="substring(@DepartureDate,9,2)"/>
				<xsl:value-of select="substring(@DepartureDate,6,2)"/>
				<xsl:value-of select="substring(@DepartureDate,1,4)"/>
			</packageStartDate>
		</packageDateTime>
	</packageDescription>
</xsl:template>

<xsl:template match="@GroupCode">
	<passengerGroupId>
		<passengerGroupInfoId>
			<groupCode>
				<xsl:value-of select="."/>
			</groupCode>
		</passengerGroupInfoId>
	</passengerGroupId>
</xsl:template>

<xsl:template match="@BedConfigurationCode">
	<bedDetails>
		<bedConfiguration>
			<xsl:value-of select="."/>
		</bedConfiguration>
	</bedDetails>
</xsl:template>

<xsl:template match="GuestDetail" mode="contact">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<travellerGroup>
		<xsl:apply-templates select="ContactInfo[@EmergencyFlag='false']">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="ContactInfo[@EmergencyFlag='true']">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
		</xsl:apply-templates>
		<xsl:apply-templates select="ContactInfo/@BirthDate"/>

		<xsl:if test="../../../SailingInfo/SelectedSailing/@VendorCode != 'RCC' and ../../../SailingInfo/SelectedSailing/@VendorCode != 'CEL'">
			<xsl:choose>
				<xsl:when test="not(ContactInfo/GuestTransportation/GuestCity/@LocationCode) or 	(ContactInfo/GuestTransportation/GuestCity/@LocationCode='')">
					<addressInfo>
						<addressQualifier>
							<address>
								<xsl:value-of select="('GST')"/>
							</address>
						</addressQualifier>
						<cityName>
							<xsl:value-of select="ContactInfo/GuestTransportation/GatewayCity/@LocationCode"/>
						</cityName>
					</addressInfo>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="ContactInfo/GuestTransportation/GuestCity"/>	
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:apply-templates select="ContactInfo/Address[../@EmergencyFlag='false']"/>
		<xsl:apply-templates select="ContactInfo/Address[../@EmergencyFlag='true']"/>
		<xsl:apply-templates select="ContactInfo/Telephone[../@EmergencyFlag='false']"/>
		<xsl:apply-templates select="ContactInfo/Telephone[../@EmergencyFlag='true']"/>
		<fareCode>
			<fareCodeId>
				<cruiseFareCode>
					<xsl:value-of select="SelectedFareCode/@FareCode"/>
				</cruiseFareCode>
			</fareCodeId>
		</fareCode>
		<xsl:apply-templates select="ContactInfo/@LoyaltyMembershipID"/>
		<xsl:apply-templates select="ContactInfo/@GuestOccupation"/>
		<xsl:apply-templates select="TravelDocument"/>
		<xsl:apply-templates select="AirDeviationRequests"/>
	</travellerGroup>
</xsl:template>

<xsl:template match="ContactInfo[@EmergencyFlag='false']">
	<xsl:param name="pos"/>
	<passengerInfo>
		<travellerId>
			<lastName>
				<xsl:value-of select="PersonName/Surname"/>
			</lastName>
			<status>
				<xsl:choose>
					<xsl:when test="../@GuestExistsIndicator = 'true'">E</xsl:when>
					<xsl:otherwise>A</xsl:otherwise>
				</xsl:choose>
			</status>
		</travellerId>
		<travellerDetails>
			<nameId>
				<xsl:value-of select="PersonName/GivenName"/>
			</nameId>
			<referenceNbr>
				<xsl:choose>
					<xsl:when test="@GuestRPH != ''"><xsl:value-of select="@GuestRPH"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="$pos"/></xsl:otherwise>
				</xsl:choose>
			</referenceNbr>
			<passengerTitle>
				<xsl:value-of select="PersonName/NameTitle"/>
			</passengerTitle>
			<passengerGender>
				<xsl:value-of select="@Gender"/>
			</passengerGender>
			<age>
				<xsl:value-of select="@Age"/>
			</age>
		</travellerDetails>
	</passengerInfo>
	<modeOfTransportation>
		<transportationInfo>
			<modeOfTransport>
				<xsl:choose>
					<xsl:when test="GuestTransportation/@TransportationMode = '29'">C</xsl:when>
					<xsl:when test="GuestTransportation/@TransportationMode = '32'">A</xsl:when>
					<xsl:when test="GuestTransportation/@TransportationMode = '33'">O</xsl:when>
					<xsl:when test="GuestTransportation/@TransportationMode = '30'">F</xsl:when>
					<xsl:when test="GuestTransportation/@TransportationMode = '31'">T</xsl:when>
					<xsl:when test="GuestTransportation/@TransportationMode = '3'">B</xsl:when>
					<xsl:when test="GuestTransportation/@TransportationMode = '21'">R</xsl:when>
					<xsl:otherwise><xsl:value-of select="GuestTransportation/@TransportationMode"/></xsl:otherwise>
				</xsl:choose>
			</modeOfTransport>
			<motCity>
				<xsl:value-of select="GuestTransportation/GatewayCity/@LocationCode"/>
			</motCity>
		</transportationInfo>
		<motStatus>
			<xsl:choose>
				<xsl:when test="GuestTransportation/@TransportationStatus = 'Available'">AVL</xsl:when>
				<xsl:when test="GuestTransportation/@TransportationStatus = 'Unavailable'">CLO</xsl:when>
				<xsl:when test="GuestTransportation/@TransportationStatus = 'OnRequest'">ONR</xsl:when>
				<xsl:when test="GuestTransportation/@TransportationStatus !=''"><xsl:value-of select="GuestTransportation/@TransportationStatus"/></xsl:when>
				<xsl:otherwise>AVL</xsl:otherwise>
			</xsl:choose>	
		</motStatus>
	</modeOfTransportation>
</xsl:template>

<xsl:template match="ContactInfo[@EmergencyFlag='true']">
	<emergencyInfo>
		<travellerId>
			<lastName>
				<xsl:value-of select="PersonName/Surname"/>
			</lastName>
		</travellerId>
		<travellerDetails>
			<nameId>
				<xsl:value-of select="PersonName/GivenName"/>
			</nameId>
		</travellerDetails>
	</emergencyInfo>
</xsl:template>

<xsl:template match="@BirthDate">
	<birthDate>
		<dateTimeDescription>
			<dateTimeQualifier>
				<xsl:value-of select="('BIR')"/>
			</dateTimeQualifier>
			<dateTimeDetails>
				<xsl:value-of select="substring(.,9,2)"/>
				<xsl:value-of select="substring(.,6,2)"/>
				<xsl:value-of select="substring(.,1,4)"/>
			</dateTimeDetails>
		</dateTimeDescription>
	</birthDate>
</xsl:template>

<xsl:template match="GuestCity">
	<addressInfo>
		<addressQualifier>
			<address>
				<xsl:value-of select="('GST')"/>
			</address>
		</addressQualifier>
		<cityName>
			<xsl:value-of select="@LocationCode"/>
		</cityName>
	</addressInfo>
</xsl:template>

<xsl:template match="Address[../@EmergencyFlag='false']">
	<addressInfo>
		<addressQualifier>
			<address>
				<xsl:value-of select="('PAX')"/>
			</address>
		</addressQualifier>
		<addressDetails>
			<addressLine1>
				<xsl:value-of select="concat(StreetNmbr,BldgRoom)"/>
			</addressLine1>
			<addressLine2>
				<xsl:value-of select="AddressLine"/>
			</addressLine2>
		</addressDetails>
		<cityName>
			<xsl:value-of select="CityName"/>
		</cityName>
		<zipCode>
			<xsl:value-of select="PostalCode"/>
		</zipCode>
		<countryIsoCode>
			<xsl:value-of select="CountryName/@Code"/>
		</countryIsoCode>
		<locationDetails>
			<stateCode>
				<xsl:value-of select="StateProv/@StateCode"/>
			</stateCode>
			<xsl:if test="../Email/@EmailType != ''">
				<emailAddress>
					<xsl:value-of select="../Email/@EmailType"/>
				</emailAddress>
			</xsl:if>
		</locationDetails>
	</addressInfo>
</xsl:template>

<xsl:template match="Address[../@EmergencyFlag='true']">
	<addressInfo>
		<addressQualifier>
			<address>
				<xsl:value-of select="('HLP')"/>
			</address>
		</addressQualifier>
		<addressDetails>
			<addressLine1>
				<xsl:value-of select="concat(StreetNmbr,BldgRoom)"/>
			</addressLine1>
			<addressLine2>
				<xsl:value-of select="AddressLine"/>
			</addressLine2>
		</addressDetails>
		<cityName>
			<xsl:value-of select="CityName"/>
		</cityName>
		<zipCode>
			<xsl:value-of select="PostalCode"/>
		</zipCode>
		<countryIsoCode>
			<xsl:value-of select="CountryName/@Code"/>
		</countryIsoCode>
		<locationDetails>
			<stateCode>
				<xsl:value-of select="StateProv/@StateCode"/>
			</stateCode>
		</locationDetails>
	</addressInfo>
</xsl:template>

<xsl:template match="ContactInfo/Telephone[../@EmergencyFlag='false']">
	<contactInfo>
		<contactDetails>
			<partyQualifierType>
				<xsl:value-of select="('P')"/>
			</partyQualifierType>
			<phoneNumber>
				<xsl:value-of select="@PhoneNumber"/>
			</phoneNumber>
		</contactDetails>
	</contactInfo>
</xsl:template>

<xsl:template match="ContactInfo/Telephone[../@EmergencyFlag='true']">
	<contactInfo>
		<contactDetails>
			<partyQualifierType>
				<xsl:value-of select="('H')"/>
			</partyQualifierType>
			<phoneNumber>
				<xsl:value-of select="@PhoneNumber"/>
			</phoneNumber>
		</contactDetails>
	</contactInfo>
</xsl:template>

<xsl:template match="@LoyaltyMembershipID">
	<paxQualifier>
		<partyQualifier>
			<xsl:value-of select="('5')"/>
		</partyQualifier>
		<itemDescription>
			<value>
				<xsl:value-of select="."/>
			</value>
		</itemDescription>
	</paxQualifier>
</xsl:template>

<xsl:template match="@GuestOccupation">
	<paxQualifier>
		<partyQualifier>
			<xsl:value-of select="('13')"/>
		</partyQualifier>
		<componentDetails>
			<componentQualifier>
				<xsl:value-of select="('10')"/>
			</componentQualifier>
			<componentDescription>
				<xsl:value-of select="."/>
			</componentDescription>
		</componentDetails>
	</paxQualifier>
</xsl:template>

<xsl:template match="TravelDocument">
	<documentInfo>
		<travellerDocumentDetails>
			<documentCode>
				<xsl:value-of select="('39')"/>
			</documentCode>
			<documentNbr>
				<xsl:value-of select="@DocID"/>
			</documentNbr>
			<issuanceCountry>
				<xsl:value-of select="@DocIssueCountry"/>
			</issuanceCountry>
			<nationality>
				<xsl:value-of select="../ContactInfo/@Nationality"/>
			</nationality>
			<birthcountry>
				<xsl:value-of select="../ContactInfo/@Nationality"/>
			</birthcountry>
		</travellerDocumentDetails>
		<documentValidity>
			<issueDate>
				<xsl:value-of select="substring(@EffectiveDate,9,2)"/>
				<xsl:value-of select="substring(@EffectiveDate,6,2)"/>
				<xsl:value-of select="substring(@EffectiveDate,1,4)"/>
			</issueDate>
			<expirationDate>
				<xsl:value-of select="substring(@ExpireDate,9,2)"/>
				<xsl:value-of select="substring(@ExpireDate,6,2)"/>
				<xsl:value-of select="substring(@ExpireDate,1,4)"/>
			</expirationDate>
		</documentValidity>
		<!--alienRegistration></alienRegistration-->
	</documentInfo>
</xsl:template>

<xsl:template match="AirDeviationRequests">
	<profileInfo>
		<profileType>
			<xsl:value-of select="('MAN')"/>
		</profileType>
		<attributeInfo>
			<attributeType>
				<xsl:value-of select="('24')"/>
			</attributeType>
		</attributeInfo>
	</profileInfo>
</xsl:template>

<xsl:template match="SelectedDining">
	<diningAssociation>
		<travellerId>
			<lastName>
				<xsl:choose>
					<xsl:when test="../ContactInfo/@GuestRPH != ''">
						<xsl:value-of select="../ContactInfo/@GuestRPH"/>	
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="position()"/></xsl:otherwise>
				</xsl:choose>				
			</lastName>
		</travellerId>
	</diningAssociation>
</xsl:template>

<xsl:template match="SelectedInsurance">
	<insuranceAssociation>
		<travellerId>
			<lastName>
				<xsl:choose>
					<xsl:when test="../ContactInfo/@GuestRPH">
						<xsl:value-of select="../ContactInfo/@GuestRPH"/>	
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="position()"/></xsl:otherwise>
				</xsl:choose>				
			</lastName>
		</travellerId>
	</insuranceAssociation>
</xsl:template>

<xsl:template match="PaymentRequest">
	<paymentGroup>
		<paymentInfo>
			<paymentDetails>
				<formOfPaymentCode>
					<xsl:choose>
						<xsl:when test="PaymentCard">CCD</xsl:when>
						<xsl:when test="BankAcct">CCD</xsl:when>
						<xsl:when test="DirectBill">CCD</xsl:when>
						<xsl:when test="Voucher">CCD</xsl:when>
						<xsl:when test="LoyaltyRedemption">CCD</xsl:when>
						<xsl:when test="MiscChargeOrder">CCD</xsl:when>
						<xsl:when test="Cash">CCD</xsl:when>
					</xsl:choose>
				</formOfPaymentCode>
				<paymentType>
					<xsl:value-of select="('F')"/>
				</paymentType>
				<xsl:apply-templates select="@ExtendedIndicator"/>
				<xsl:apply-templates select="PaymentAmount"/>
				<xsl:apply-templates select="@ReferenceNumber"/>
				<xsl:apply-templates select="PaymentCard/@EffectiveDate"/>
			</paymentDetails>
			<xsl:apply-templates select="PaymentCard/@CardNumber"/>
		</paymentInfo>
		<xsl:apply-templates select="PaymentCard/CardHolderName"/>
		<xsl:if test="../../../SailingInfo/SelectedSailing/@VendorCode != 'RCC' and ../../../SailingInfo/SelectedSailing/@VendorCode != 'CEL'">
			<xsl:apply-templates select="PaymentCard/Address"/>
		</xsl:if>
	</paymentGroup>
</xsl:template>

<xsl:template match="@ExtendedIndicator">
	<extendedPaymentIndicator>
		<xsl:value-of select="."/>
	</extendedPaymentIndicator>
</xsl:template>

<xsl:template match="PaymentAmount">
	<amount>
		<xsl:value-of select="@Amount"/>
	</amount>
	<currency>
		<xsl:value-of select="@CurrencyCode"/>
	</currency>
</xsl:template>

<xsl:template match="@ReferenceNumber">
	<referenceNbr>
		<xsl:value-of select="."/>
	</referenceNbr>
</xsl:template>

<xsl:template match="@EffectiveDate">
	<creditCardFromDate>
		<xsl:text>01</xsl:text>
		<xsl:value-of select="substring(.,1,2)"/>
		<xsl:text>20</xsl:text>
		<xsl:value-of select="substring(.,3,2)"/>
	</creditCardFromDate>
</xsl:template>

<xsl:template match="@CardNumber">
	<creditCardInfo>
		<creditCardCode>
			<xsl:value-of select="../@CardCode"/>
		</creditCardCode>
		<creditCardNbr>
			<xsl:value-of select="."/>
		</creditCardNbr>
		<creditCardExpiryDate>
			<xsl:value-of select="substring(../@ExpireDate,1,2)"/>
			<xsl:text>20</xsl:text>
			<xsl:value-of select="substring(../@ExpireDate,3,2)"/>
		</creditCardExpiryDate>
	</creditCardInfo>
</xsl:template>

<xsl:template match="CardHolderName">
	<paymentOriginator>
		<partyQualifier>
			<xsl:value-of select="('7')"/>
		</partyQualifier>
		<componentDetails>
			<componentQualifier>
				<xsl:value-of select="('10')"/>
			</componentQualifier>
			<componentDescription>
				<xsl:value-of select="."/>
			</componentDescription>
		</componentDetails>
	</paymentOriginator>
</xsl:template>

<xsl:template match="Address">
	<paymentOriginatorName>
		<travellerId>
			<lastName>
				<xsl:value-of select="substring-after(../CardHolderName,'/')"/>
			</lastName>
		</travellerId>
		<travellerDetails>
			<nameId>
				<xsl:value-of select="substring-before(../CardHolderName,'/')"/>
			</nameId>
		</travellerDetails>
	</paymentOriginatorName>
	<billingAddress>
		<addressDetails>
			<addressLine1>
				<xsl:value-of select="concat(StreetNmbr,BldgRoom)"/>
			</addressLine1>
			<addressLine2>
				<xsl:value-of select="AddressLine"/>
			</addressLine2>
		</addressDetails>
		<cityName>
			<xsl:value-of select="CityName"/>
		</cityName>
		<zipCode>
			<xsl:value-of select="PostalCode"/>
		</zipCode>
		<countryIsoCode>
			<xsl:value-of select="CountryName/@Code"/>
		</countryIsoCode>
		<locationDetails>
			<stateCode>
				<xsl:value-of select="StateProv/@StateCode"/>
			</stateCode>
		</locationDetails>
	</billingAddress>
</xsl:template>

<xsl:template match="GuestDetail" mode="pkg">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:apply-templates select="SelectedPackages/SelectedPackage" mode="pkg">
		<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
	</xsl:apply-templates>
</xsl:template>

<xsl:template match="GuestDetail" mode="transfer">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:apply-templates select="SelectedPackages/SelectedPackage" mode="transfer">
		<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
	</xsl:apply-templates>
</xsl:template>

<xsl:template match="SelectedPackage" mode="pkg">
	<xsl:param name="pos"/>
	<xsl:if test="@PackageTypeCode = 'Post' or @PackageTypeCode = 'Pre' or @PackageTypeCode = 'Shore' or @PackageTypeCode = 'Bus'">
		<packageGroup>
	            <otherPackages>
	                  <packageType>
	                  		<xsl:choose>
							<xsl:when test="@PackageTypeCode = 'Post'">A</xsl:when>
							<xsl:when test="@PackageTypeCode = 'Pre'">B</xsl:when>
							<xsl:when test="@PackageTypeCode = 'Shore'">S</xsl:when>
							<xsl:when test="@PackageTypeCode = 'Bus'">Z</xsl:when>
						</xsl:choose>
	                  </packageType>
	                  <packageDetails>
	                        <packageCode><xsl:value-of select="@CruisePackageCode"/></packageCode>
	                  </packageDetails>
	                  <packageStatus>AVL</packageStatus>
	            </otherPackages>
	            <packageAssociation>
	                  <travellerId>
	                        <lastName><xsl:value-of select="$pos"/></lastName>
	                  </travellerId>
	            </packageAssociation>
	             <xsl:variable name="PackageTypeCode"><xsl:value-of select="@PackageTypeCode"/></xsl:variable>
	            <xsl:variable name="CruisePackageCode"><xsl:value-of select="@CruisePackageCode"/></xsl:variable>
	            <xsl:apply-templates select="../../../GuestDetail[position() > 1]" mode="paxAssoc">
	            	<xsl:with-param name="PackageTypeCode"><xsl:value-of select="$PackageTypeCode"/></xsl:with-param>
	            	<xsl:with-param name="CruisePackageCode"><xsl:value-of select="$CruisePackageCode"/></xsl:with-param>
	            </xsl:apply-templates>
	      </packageGroup>
	</xsl:if>
</xsl:template>

<xsl:template match="GuestDetail" mode="paxAssoc">
	<xsl:param name="PackageTypeCode"/>
	<xsl:param name="CruisePackageCode"/>
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:apply-templates select="SelectedPackages/SelectedPackage" mode="paxAssoc">
		<xsl:with-param name="pos"><xsl:value-of select="$pos + 1"/></xsl:with-param>
		<xsl:with-param name="PackageTypeCode"><xsl:value-of select="$PackageTypeCode"/></xsl:with-param>
         	<xsl:with-param name="CruisePackageCode"><xsl:value-of select="$CruisePackageCode"/></xsl:with-param>
	</xsl:apply-templates>
</xsl:template>

<xsl:template match="SelectedPackage" mode="paxAssoc">
	<xsl:param name="pos"/>
	<xsl:param name="PackageTypeCode"/>
	<xsl:param name="CruisePackageCode"/>
	<xsl:if test="@PackageTypeCode = $PackageTypeCode and @CruisePackageCode = $CruisePackageCode">
		<packageAssociation>
                  <travellerId>
                        <lastName><xsl:value-of select="$pos"/></lastName>
                  </travellerId>
            </packageAssociation>
	</xsl:if>
</xsl:template>

<xsl:template match="SelectedPackage" mode="transfer">
	<xsl:param name="pos"/>
	<xsl:if test="@PackageTypeCode = 'PreTransfer' or @PackageTypeCode = 'PostTransfer' or @PackageTypeCode = 'RoundTransfer'">
		<transferGroup>
	            <transferInfo>
	                  <transferDetails>
	                  		<transferType>
		                  		<xsl:choose>
								<xsl:when test="@PackageTypeCode = 'PreTransfer'">D</xsl:when>
								<xsl:when test="@PackageTypeCode = 'PostTransfer'">A</xsl:when>
								<xsl:when test="@PackageTypeCode = 'RoundTransfer'">R</xsl:when>
							</xsl:choose>
	                 		 </transferType>
	                        <transferCode><xsl:value-of select="@CruisePackageCode"/></transferCode>
	                  </transferDetails>
	            </transferInfo>
	            <transferAssociation>
	                  <travellerId>
	                        <lastName><xsl:value-of select="$pos"/></lastName>
	                  </travellerId>
	            </transferAssociation>
	      </transferGroup>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>

