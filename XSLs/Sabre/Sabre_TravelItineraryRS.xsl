<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_TravelItineraryRS Version="2.000">
			<Success />
			<xsl:apply-templates select="OTA_TravelItineraryRS" />
		</OTA_TravelItineraryRS>
	</xsl:template>
	<xsl:template match="OTA_TravelItineraryRS">
		<xsl:choose>
			<xsl:when test="TravelItinerary">
				<TravelItinerary>
					<ItineraryRef>
						<xsl:attribute name="Type">PNR</xsl:attribute>
						<xsl:attribute name="Id">
							<xsl:value-of select="TravelItinerary/ItineraryRef/@Id" />
						</xsl:attribute>
					</ItineraryRef>
					<xsl:apply-templates select="TravelItinerary" />
				</TravelItinerary>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<xsl:apply-templates select="Errors/Error" />
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			TravelItinerary detail Information                                             -->
	<!--************************************************************************************-->
	<xsl:template match="TravelItinerary">
		<!--******************************************************-->
		<!--				Names                                      -->
		<!--******************************************************-->
		<CustomerInfos>
			<xsl:apply-templates select="CustomerInfos/CustomerInfo" />
		</CustomerInfos>
		<!--******************************************************-->
		<!--			Air Itinerary                                      -->
		<!--******************************************************-->
		<ItineraryInfo>
			<ReservationItems>
				<Item>
					<xsl:if test="ItineraryInfo/ReservationItems/Item/Air">
						<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Air" mode="Air" />
					</xsl:if>
					<xsl:if test="ItineraryInfo/ReservationItems/Item/Vehicle">
						<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Vehicle" mode="Car" />
					</xsl:if>
					<xsl:if test="ItineraryInfo/ReservationItems/Item/Hotel">
						<xsl:apply-templates select="ItineraryInfo/ReservationItems/Item/Hotel" mode="Hotel" />
					</xsl:if>
				</Item>
				<xsl:if test="ItineraryInfo/ReservationItems/ItemPricing">
					<ItemPricing>
						<xsl:apply-templates select="ItineraryInfo/ReservationItems/ItemPricing" mode="Pricing" />
					</ItemPricing>
				</xsl:if>
			</ReservationItems>
			<xsl:apply-templates select="ItineraryInfo/Ticketing" mode="Ticketing" />
			<xsl:if test="ItineraryInfo/SpecialServices">
				<SpecialServices>
					<xsl:apply-templates select="ItineraryInfo/SpecialServices" />
				</SpecialServices>
			</xsl:if>
		</ItineraryInfo>
		<!--******************************************************-->
		<!--			Form of Payment                               -->
		<!--******************************************************-->
		<TravelCost>
			<xsl:if test="TravelCost/FormOfPayment">
				<FormOfPayment>
					<xsl:apply-templates select="TravelCost/FormOfPayment" />
				</FormOfPayment>
			</xsl:if>
		</TravelCost>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			PNR Retrieve Errors                                           	                 -->
	<!--************************************************************************************-->
	<xsl:template match="Err">
		<Error>
			<xsl:attribute name="Type">General</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<!--************************************************************************************-->
	<!--						 Passenger Information         		                        -->
	<!--************************************************************************************-->
	<xsl:template match="CustomerInfo">
		<CustomerInfo>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<Customer>
				<PersonName>
					<xsl:attribute name="NameType">
						<xsl:value-of select="substring(string(Remarks),23,3)" />
					</xsl:attribute>
					<xsl:if test="NamePrefix!=''">
						<NamePrefix>
							<xsl:value-of select="Customer/PersonName/NamePrefix" />
						</NamePrefix>
					</xsl:if>
					<GivenName>
						<xsl:value-of select="Customer/PersonName/GivenName" />
					</GivenName>
					<Surname>
						<xsl:value-of select="Customer/PersonName/Surname" />
					</Surname>
				</PersonName>
				<xsl:apply-templates select="Customer/Telephone" />
				<xsl:apply-templates select="Customer/Email" />
				<xsl:apply-templates select="Customer/Address" />
			</Customer>
		</CustomerInfo>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			      Air Itinerary Section				                              	    -->
	<!--************************************************************************************-->
	<xsl:template match="Air" mode="Air">
		<Air>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()" />
			</xsl:attribute>
			<xsl:variable name="PaxCount">
				<xsl:value-of select="count(../../../../CustomerInfos/CustomerInfo)" />
			</xsl:variable>
			<xsl:attribute name="NumberInParty">
				<xsl:value-of select="$PaxCount" />
			</xsl:attribute>
			<!--<xsl:attribute name="ResBookDesigCode"><xsl:value-of select="BIC"/></xsl:attribute>
			<xsl:attribute name="ActionCode"><xsl:value-of select="Status"/></xsl:attribute> -->
			<xsl:attribute name="DepartureDateTime">
				<xsl:value-of select="@DepartureDateTime" />
			</xsl:attribute>
			<xsl:attribute name="ArrivalDateTime">
				<xsl:value-of select="@ArrivalDateTime" />
			</xsl:attribute>
			<xsl:attribute name="FlightNumber">
				<xsl:value-of select="@FlightNumber" />
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
			<Equipment>
				<xsl:attribute name="AirEquipType">
					<xsl:value-of select="Equipment/@AirEquipType" />
				</xsl:attribute>
			</Equipment>
			<MarketingAirline>
				<xsl:attribute name="Code">
					<xsl:value-of select="MarketingAirline/@Code" />
				</xsl:attribute>
			</MarketingAirline>
			<!--<TPA_Extensions>
				<xsl:attribute name="DateChange"><xsl:value-of select="DayChg"/></xsl:attribute>
				<xsl:choose>
					<xsl:when test="FlownInd = 'Y'">
						<xsl:attribute name="FlownIndicator">1</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="FlownIndicator">0</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
			</TPA_Extensions>-->
		</Air>
	</xsl:template>
	<!--*****************************************************************-->
	<!--			Car Segs						   		                                 -->
	<!--*****************************************************************-->
	<xsl:template match="Vehicle" mode="Car">
		<Vehicle>
			<ConfID>
				<xsl:attribute name="ID">
					<xsl:value-of select="ConfID/@ID" />
				</xsl:attribute>
			</ConfID>
			<Vendor>
				<xsl:attribute name="Code">
					<xsl:value-of select="Vendor/@Code" />
				</xsl:attribute>
			</Vendor>
			<VehRentalCore>
				<xsl:attribute name="PickUpDateTime">
					<xsl:value-of select="VehRentalCore/@PickUpDateTime" />
				</xsl:attribute>
				<xsl:attribute name="ReturnDateTime">
					<xsl:value-of select="VehRentalCore/@ReturnDateTime" />
				</xsl:attribute>
				<PickUpLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode" />
					</xsl:attribute>
				</PickUpLocation>
				<ReturnLocation>
					<xsl:attribute name="LocationCode">
						<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode" />
					</xsl:attribute>
				</ReturnLocation>
			</VehRentalCore>
			<Veh>
				<xsl:attribute name="AirConditionInd">
					<xsl:value-of select="Vehicle/@AirConditionInd" />
				</xsl:attribute>
				<xsl:attribute name="TransmissionType">
					<xsl:value-of select="Vehicle/@TransmissionType" />
				</xsl:attribute>
				<VehType>
					<xsl:attribute name="VehicleCategory">
						<xsl:value-of select="Vehicle/VehType/@VehicleCategory" />
					</xsl:attribute>
				</VehType>
			</Veh>
			<RentalRate>
				<RateDistance>
					<xsl:attribute name="Unlimited">
						<xsl:choose>
							<xsl:when test="RentalRate/RateDistance/@Unlimited='true'">1</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DistUnitName">
						<xsl:value-of select="RentalRate/RateDistance/@DistUnitName" />
					</xsl:attribute>
					<xsl:attribute name="VehiclePeriodUnitName">
						<xsl:value-of select="RentalRate/RateDistance/@ VehiclePeriodUnitName" />
					</xsl:attribute>
				</RateDistance>
				<VehicleCharges>
					<VehicleCharge>
						<xsl:attribute name="Amount">
							<xsl:value-of select="RentalRate/VehicleCharges/VehicleCharge/@Amount" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="RentalRate/VehicleCharges/VehicleCharge/@CurrencyCode" />
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="RentalRate/VehicleCharges/VehicleCharge/@DecimalPlaces" />
						</xsl:attribute>
						<xsl:attribute name="TaxInclusive">
							<xsl:choose>
								<xsl:when test="RentalRate/VehicleCharges/VehicleCharge/@TaxInclusive='true'">1</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="GuaranteedInd">
							<xsl:choose>
								<xsl:when test="RentalRate/VehicleCharges/VehicleCharge/@GuaranteedInd='true'">1</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</VehicleCharge>
				</VehicleCharges>
			</RentalRate>
			<TotalCharge>
				<xsl:attribute name="RateTotalAmount">
					<xsl:value-of select="TotalCharge/@RateTotalAmount" />
				</xsl:attribute>
				<xsl:attribute name="EstimatedTotalAmount">
					<xsl:value-of select="TotalCharge/@EstimatedTotalAmount" />
				</xsl:attribute>
				<xsl:attribute name="CurrencyCode">
					<xsl:value-of select="TotalCharge/@CurrencyCode" />
				</xsl:attribute>
			</TotalCharge>
		</Vehicle>
	</xsl:template>
	<!--*****************************************************************-->
	<!--			Hotel Segs								    -->
	<!--*****************************************************************-->
	<xsl:template match="Hotel" mode="Hotel">
		<Hotel>
			<Reservation>
				<RoomTypes>
					<RoomType>
						<xsl:attribute name="RoomTypeCode">
							<xsl:value-of select="Reservation/RoomTypes/RoomType/@RoomTypeCode" />
						</xsl:attribute>
						<xsl:attribute name="NumberOfUnits">
							<xsl:value-of select="Reservation/RoomTypes/RoomType/@NumberOfUnits" />
						</xsl:attribute>
					</RoomType>
				</RoomTypes>
				<RoomRates>
					<RoomRate>
						<Rates>
							<Rate>
								<Base>
									<xsl:attribute name="AmountBeforeTax">
										<xsl:value-of select="Reservation/RoomRates/RoomRate/Rate/Base/@AmountBeforeTax" />
									</xsl:attribute>
									<xsl:attribute name="CurrencyCode">
										<xsl:value-of select="Reservation/RoomRates/RoomRate/Rate/Base/@CurrencyCode" />
									</xsl:attribute>
								</Base>
							</Rate>
						</Rates>
					</RoomRate>
				</RoomRates>
				<GuestCounts>
					<GuestCount>
						<xsl:attribute name="AgeQualifyingCode">
							<xsl:value-of select="Reservation/GuestCounts/GuestCount/AgeQualifyingCode" />
						</xsl:attribute>
						<xsl:attribute name="Count">
							<xsl:value-of select="Reservation/GuestCounts/GuestCount/Count" />
						</xsl:attribute>
					</GuestCount>
				</GuestCounts>
				<TimeSpan>
					<xsl:attribute name="Start">
						<xsl:value-of select="Reservation/TimeSpan/@Start" />
					</xsl:attribute>
					<xsl:attribute name="Duration">
						<xsl:value-of select="Reservation/TimeSpan/@Duration" />
					</xsl:attribute>
					<xsl:attribute name="End">
						<xsl:value-of select="Reservation/TimeSpan/@End" />
					</xsl:attribute>
				</TimeSpan>
				<BasicPropertyInfo>
					<xsl:attribute name="ChainCode">
						<xsl:value-of select="Reservation/BasicPropertyInfo/@ChainCode" />
					</xsl:attribute>
					<xsl:attribute name="HotelCityCode">
						<xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCityCode" />
					</xsl:attribute>
					<xsl:attribute name="HotelCode">
						<xsl:value-of select="Reservation/BasicPropertyInfo/@HotelCode" />
					</xsl:attribute>
					<xsl:attribute name="HotelName">
						<xsl:value-of select="Reservation/BasicPropertyInfo/@HotelName" />
					</xsl:attribute>
				</BasicPropertyInfo>
			</Reservation>
			<TPA_Extensions>
				<xsl:attribute name="ConfirmationNumber">
					<xsl:value-of select="TPA_Extensions/@ConfirmationNumber" />
				</xsl:attribute>
			</TPA_Extensions>
		</Hotel>
	</xsl:template>
	<!--************************************************************************************-->
	<!--					Calculate Total FareTotals	 	      			           -->
	<!--***********************************************************************************-->
	<xsl:template match="ItemPricing">
		<AirFareInfo>
			<ItinTotalFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/BaseFare/@Amount" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/BaseFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</BaseFare>
				<EquivFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/EquivFare/@Amount" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/EquivFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</EquivFare>
				<Taxes>
					<xsl:attribute name="Amount">
						<xsl:value-of select="sum(AirFareInfo/ItinTotalFare/Taxes/Tax/@Amount)" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/Taxes/Tax/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
					<xsl:apply-templates select="AirFareInfo/ItinTotalFare/Taxes/Tax" mode="TotalFare" />
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/TotalFare/@Amount" />
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="AirFareInfo/ItinTotalFare/TotalFare/@CurrencyCode" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
				</TotalFare>
			</ItinTotalFare>
			<PTC_FareBreakdowns>
				<xsl:apply-templates select="PTC_FareInfo" />
			</PTC_FareBreakdowns>
		</AirFareInfo>
	</xsl:template>
	<!--************************************************************************************-->
	<!--					Individual Tax element 	 	      			                -->
	<!--***********************************************************************************-->
	<xsl:template match="Tax" mode="TotalFare">
		<Tax>
			<xsl:attribute name="Amount">
				<xsl:value-of select="@Amount" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="@CurrencyCode" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
		</Tax>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Calculate Fare Totals per Passenger Type	 	                 -->
	<!--************************************************************************************-->
	<xsl:template match="PTC_FareInfo">
		<PTC_FareBreakdown>
			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:value-of select="PTC_FareBreakdown/PassengerTypeQuantity/@Code" />
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:apply-templates select="FareBasisCode" />
			</FareBasisCodes>
			<PassengerFare>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/BaseFare/@Amount" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/BaseFareCurrency/@CurrencyCode" />
					</xsl:attribute>
				</BaseFare>
				<xsl:choose>
					<xsl:when test="PTC_FareBreakdown/PassengerFare/EquivAmt != '0'">
						<EquivFare>
							<xsl:attribute name="Amount">
								<xsl:value-of select="PTC_FareBreakdown/PassengerFare/EquivAmt/@Amount" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="2" />
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="PTC_FareBreakdown/PassengerFare/EquivAmt/@CurrencyCode" />
							</xsl:attribute>
						</EquivFare>
					</xsl:when>
				</xsl:choose>
				<Taxes>
					<xsl:apply-templates select="PTC_FareBreakdown/PassengerFare//Taxes/Tax" mode="PTC" />
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@Amount" />
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="PTC_FareBreakdown/PassengerFare/TotalFare/@CurrencyCode" />
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
	<!--************************************************************************************-->
	<!--					Individual Tax element 	 	      			                -->
	<!--***********************************************************************************-->
	<xsl:template match="Tax" mode="PTC">
		<Tax>
			<xsl:attribute name="Amount">
				<xsl:value-of select="@Amount" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="@CurrencyCode" />
			</xsl:attribute>
			<xsl:attribute name="DecimalPlaces">2</xsl:attribute>
		</Tax>
	</xsl:template>
	<!--************************************************************************************-->
	<!-- 						Telephone									    -->
	<!--************************************************************************************-->
	<xsl:template match="Telephone">
		<Telephone>
			<xsl:attribute name="PhoneUseType">
				<xsl:value-of select="@PhoneUseType" />
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
	<!--************************************************************************************-->
	<!--		EmailAddress  Processing									    -->
	<!--************************************************************************************-->
	<xsl:template match="Email">
		<Email>
			<xsl:value-of select="." />
		</Email>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Address/Delivery Addres information						    -->
	<!--************************************************************************************-->
	<xsl:template match="Address">
		<Address>
			<xsl:attribute name="Type">
				<xsl:value-of select="@Type" />
			</xsl:attribute>
			<xsl:attribute name="UseType">Mailing</xsl:attribute>
			<StreetNmbr>
				<xsl:value-of select="AddressLine[1]" />
				<xsl:value-of select="AddressLine[2]" />
			</StreetNmbr>
			<CityName>
				<xsl:value-of select="AddressLine[3]" />
			</CityName>
			<PostalCode />
			<StateProv />
			<CountryName>
				<xsl:attribute name="Code">
					<xsl:value-of select="CountryName/@Code" />
				</xsl:attribute>
			</CountryName>
		</Address>
	</xsl:template>
	<!-- ***********************************************************-->
	<!--  				Ticketing info      			     -->
	<!-- ********************************************************** -->
	<xsl:template match="Ticketing">
		<Ticketing>
			<xsl:attribute name="TicketType">
				<xsl:value-of select="@TicketType" />
			</xsl:attribute>
		</Ticketing>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Form of Payment       						                       -->
	<!--************************************************************************************-->
	<xsl:template match="FormOfPayment">
		<xsl:attribute name="RPH">
			<xsl:value-of select="position()" />
		</xsl:attribute>
		<PaymentCard>
			<xsl:attribute name="CardCode">
				<xsl:value-of select="PaymentCard/@CardType" />
			</xsl:attribute>
			<xsl:attribute name="CardNumber">
				<xsl:value-of select="PaymentCard/@CardNumber" />
			</xsl:attribute>
			<xsl:attribute name="ExpireDate">
				<xsl:value-of select="PaymentCard/@ExpireDate" />
			</xsl:attribute>
		</PaymentCard>
		<TPA_Extensions>
			<xsl:attribute name="FOPType">CC</xsl:attribute>
		</TPA_Extensions>
	</xsl:template>
	<!--************************************************************************************-->
	<!--			Special Service Request (SSR) Processing				    -->
	<!--************************************************************************************-->
	<xsl:template match="SpecialServices">
		<SpecialServiceRequest>
			<xsl:attribute name="SSRCode">
				<xsl:value-of select="Service/@SSRCode" />
			</xsl:attribute>
			<Text>
				<xsl:value-of select="Service/Text" />
			</Text>
		</SpecialServiceRequest>
	</xsl:template>
</xsl:stylesheet>
