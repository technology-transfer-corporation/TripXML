<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- AmadeusWS_CruisePriceBookingRQ.xsl 	     								       -->
<!-- ================================================================== -->
<!-- Date: 19 Jul 2009 - Rastko												        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes"/>

<xsl:template match="OTA_CruisePriceBookingRQ">
	<Cruise_PriceBooking>
		<agentEnvironment>
			<agentTerminalId>
				<xsl:value-of select="('12345678')"/>
			</agentTerminalId>
		</agentEnvironment>
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
			<xsl:apply-templates select="SailingInfo/SelectedCategory/SelectedCabin/@CabinNumber"/>
			<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail[ContactInfo/@EmergencyFlag='false']" mode="contact"/>
		</sailingGroup>
		<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail/SelectedDining"/>
		<xsl:if test="ReservationInfo/GuestDetails/GuestDetail/SelectedInsurance/@InsuranceCode != ''">
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
		<xsl:if test="SailingInfo/SelectedSailing/@VendorCode = 'HAL' or SailingInfo/SelectedSailing/@VendorCode = 'WSC'">
			<xsl:for-each select="ReservationInfo/GuestDetails/GuestDetail">
				<packageGroup>
			            <otherPackages>
			                  <packageType>B</packageType>
			                  <packageDetails>
			                        <packageCode>000144</packageCode>
			                  </packageDetails>
			                  <packageStatus>AVL</packageStatus>
			            </otherPackages>
			            <packageAssociation>
			                  <travellerId>
			                        <lastName><xsl:value-of select="position()"/></lastName>
			                  </travellerId>
			            </packageAssociation>
			      </packageGroup>
			</xsl:for-each>
		</xsl:if>
		<xsl:apply-templates select="ReservationInfo/GuestDetails/GuestDetail" mode="pkg"/>
	</Cruise_PriceBooking>
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
				<xsl:value-of select="@CruisePackageCode"/>
			</packageCode>
		</packageDetails>
		<packageDateTime>
			<packageStartDate>
				<xsl:value-of select="substring(@StartDate,9,2)"/>
				<xsl:value-of select="substring(@StartDate,6,2)"/>
				<xsl:value-of select="substring(@StartDate,1,4)"/>
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

<xsl:template match="@CabinNumber">
	<cabinInfo>
		<cabinDetails>
			<cabinNbr>
				<xsl:value-of select="."/>
			</cabinNbr>						
		</cabinDetails>
		<xsl:apply-templates select="../@BedConfigurationCode"/>
	</cabinInfo>
</xsl:template>

<xsl:template match="@BedConfigurationCode">
	<bedDetails>
		<bedConfiguration>
			<xsl:value-of select="."/>
		</bedConfiguration>
	</bedDetails>
</xsl:template>

<xsl:template match="GuestDetail[ContactInfo/@EmergencyFlag='false']" mode="contact">
	<travellerGroup>
		<passengerInfo>
			<travellerId>
				<lastName>
					<xsl:value-of select="ContactInfo/PersonName/Surname"/>
				</lastName>
			</travellerId>
			<travellerDetails>
				<nameId>
					<xsl:value-of select="ContactInfo/PersonName/GivenName"/>
				</nameId>
				<referenceNbr>
					<xsl:choose>
						<xsl:when test="ContactInfo/@GuestRPH != ''"><xsl:value-of select="ContactInfo/@GuestRPH"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="position()"/></xsl:otherwise>
					</xsl:choose>
				</referenceNbr>
				<passengerTitle>
					<xsl:value-of select="ContactInfo/PersonName/NameTitle"/>
				</passengerTitle>
				<passengerGender>
					<xsl:value-of select="ContactInfo/@Gender"/>
				</passengerGender>
				<age>
					<xsl:value-of select="ContactInfo/@Age"/>
				</age>
			</travellerDetails>
		</passengerInfo>
		<modeOfTransportation>
			<transportationInfo>
				<modeOfTransport>
					<xsl:choose>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '29'">C</xsl:when>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '32'">A</xsl:when>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '33'">O</xsl:when>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '30'">F</xsl:when>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '31'">T</xsl:when>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '3'">B</xsl:when>
						<xsl:when test="ContactInfo/GuestTransportation/@TransportationMode = '21'">R</xsl:when>
						<xsl:otherwise><xsl:value-of select="ContactInfo/GuestTransportation/@TransportationMode"/></xsl:otherwise>
					</xsl:choose>
				</modeOfTransport>
				<motCity>
					<xsl:value-of select="ContactInfo/GuestTransportation/GatewayCity/@LocationCode"/>
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
		<xsl:apply-templates select="ContactInfo/GuestTransportation/GuestCity"/>
		<fareCode>
			<fareCodeId>
				<cruiseFareCode>
					<xsl:value-of select="SelectedFareCode/@FareCode"/>
				</cruiseFareCode>
			</fareCodeId>
		</fareCode>
		<xsl:apply-templates select="ContactInfo/@LoyaltyMembershipID"/>
	</travellerGroup>
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

<xsl:template match="SelectedDining">
	<diningGroup>
		<diningDetails>
			<diningIdentification>
				<diningLabel>
					<xsl:value-of select="@Sitting"/>
				</diningLabel>
				<xsl:if test="@Status != ''">
					<diningStatus>
						<xsl:choose>
							<xsl:when test="@Status = 'Available'">AVL</xsl:when>
							<xsl:when test="@Status = 'Unavailable'">CLO</xsl:when>
							<xsl:when test="@Status = 'OnRequest'">ONR</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="@Status"/>	
							</xsl:otherwise>
						</xsl:choose>	
					</diningStatus>
				</xsl:if>
			</diningIdentification>
			<xsl:if test="@SmokingCode != '' or @Language != '' or @DiningRoom != ''">
				<roomDetails>
					<xsl:apply-templates select="@SmokingCode"/>
					<xsl:apply-templates select="@Language"/>
					<xsl:apply-templates select="@DiningRoom"/>				
				</roomDetails>
			</xsl:if>
			<xsl:apply-templates select="@TableSize"/>
			<xsl:apply-templates select="@AgeCode"/>
		</diningDetails>
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
	</diningGroup>
</xsl:template>

<xsl:template match="@SmokingIndicator">
	<smokingIndicator>
		<xsl:value-of select="."/>
	</smokingIndicator>
</xsl:template>

<xsl:template match="@Language">
	<preferredIsoLanguage>
		<xsl:value-of select="."/>
	</preferredIsoLanguage>
</xsl:template>

<xsl:template match="@DiningRoom">
	<diningRoomId>
		<xsl:value-of select="."/>
	</diningRoomId>
</xsl:template>

<xsl:template match="@TableSize">
	<guestTableSize>
		<xsl:value-of select="."/>
	</guestTableSize>
</xsl:template>

<xsl:template match="@AgeCode">
	<diningGuestAge>
		<xsl:value-of select="."/>
	</diningGuestAge>
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
		<xsl:apply-templates select="PaymentCard/Address"/>
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
		<xsl:value-of select="substring(.,9,2)"/>
		<xsl:value-of select="substring(.,6,2)"/>
		<xsl:value-of select="substring(.,1,4)"/>
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
			<xsl:value-of select="../@ExpireDate"/>
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
				<xsl:value-of select="../CardHolderName"/>
			</lastName>
		</travellerId>
		<travellerDetails>
			<nameId>
				<xsl:value-of select="../CardHolderName"/>
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
	      </packageGroup>
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
