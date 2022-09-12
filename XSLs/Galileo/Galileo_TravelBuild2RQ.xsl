<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ******************************************************************************	-->
	<!-- Message	: Travel Build Request												-->
	<!-- Input		: OTA_TravelItineraryRQ												-->
	<!-- Author		: Alexis Consuegra													-->
	<!-- Date		: Sep 20 2004 . Combine All Travel Build Requests					-->
	<!-- ******************************************************************************	-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNRBuildRQ>
			<xsl:apply-templates select="OTA_TravelItineraryRQ" />
		</PNRBuildRQ>
	</xsl:template>
	<xsl:template match="OTA_TravelItineraryRQ">
		<OTA_AirBookRQ>
			<xsl:apply-templates select="OTA_AirBookRQ" mode="air" />
		</OTA_AirBookRQ>
		<OTA_NamesRQ>
			<xsl:apply-templates select="." mode="names" />
		</OTA_NamesRQ>
		<OTA_AirSeatRQ>
			<xsl:apply-templates select="OTA_AirBookRQ" mode="seat" />
		</OTA_AirSeatRQ>
		<OTA_CarRQ>
			<xsl:apply-templates select="OTA_VehResRQ" />
		</OTA_CarRQ>
		<OTA_HotelRQ>
			<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations" />
		</OTA_HotelRQ>
		<OTA_CurrPNRRead>
			<AgencyPNRBFDisplay_7_0>
				<PNRBFRetrieveMods>
					<CurrentPNR />
				</PNRBFRetrieveMods>
			</AgencyPNRBFDisplay_7_0>
		</OTA_CurrPNRRead>
		<OTA_PriceRQ>
			<xsl:apply-templates select="OTA_AirBookRQ" mode="price" />
		</OTA_PriceRQ>
		<OTA_EndTRQ>
			<xsl:apply-templates select="." mode="et" />
		</OTA_EndTRQ>
		<OTA_ReadRQ>
			<AgencyPNRBFDisplay_7_0>
				<PNRBFRetrieveMods>
					<PNRAddr>
						<RecLoc></RecLoc>
					</PNRAddr>
				</PNRBFRetrieveMods>
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<FareNum>1</FareNum>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>
			</AgencyPNRBFDisplay_7_0>
		</OTA_ReadRQ>
		<OTA_PriceVerify>
			<FareQuoteVerify_6_0>
				<FareQuoteVerifyMods>
					<ItemAry>
						<Item>
							<BlkInd>A </BlkInd>
							<SpecificQual>
								<RelFare>0</RelFare>
							</SpecificQual>
						</Item>
					</ItemAry>
				</FareQuoteVerifyMods>
			</FareQuoteVerify_6_0>
		</OTA_PriceVerify>
	</xsl:template>
	<!--  *****************  -->
	<!--  ****** Air ******  -->
	<!--  *****************  -->
	<xsl:template match="OTA_AirBookRQ" mode="air">
		<xsl:if test="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
			<PNRBFReservationModify_5_0>
				<AirSegSellMods>
					<OutputMsg>Y</OutputMsg>
					<SellBlkAry>
						<xsl:choose>
							<xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/@ActionCode ='OK'">
								<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"
									mode="Regular" />
							</xsl:when>
							<xsl:when test="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[not(@ActionCode)]">
								<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"
									mode="Regular" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment"
									mode="Passive" />
							</xsl:otherwise>
						</xsl:choose>
					</SellBlkAry>
				</AirSegSellMods>
			</PNRBFReservationModify_5_0>
		</xsl:if>
	</xsl:template>
	<xsl:template match="FlightSegment" mode="Regular">
		<xsl:choose>
			<!--************************************************************************************************************-->
			<!--							Open Segments									          -->
			<!--************************************************************************************************************-->
			<xsl:when test="@Type='Open'">
				<SellBlk>
					<ReqNum>
						<xsl:value-of select="position()" />
					</ReqNum>
					<AirV>
						<xsl:value-of select="MarketingAirline/@Code" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</AirV>
					<FltNum>
						<xsl:value-of select="@Type" />
					</FltNum>
					<OptSuf><![CDATA[ ]]></OptSuf>
					<BIC>
						<xsl:value-of select="@ResBookDesigCode" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</BIC>
					<Dt>
						<xsl:value-of select="substring(@DepartureDateTime,1,4)" />
						<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
						<xsl:value-of select="substring(@DepartureDateTime,9,2)" />
					</Dt>
					<StartAirp>
						<xsl:value-of select="DepartureAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</StartAirp>
					<EndAirp>
						<xsl:value-of select="ArrivalAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</EndAirp>
					<ReqType>
						<xsl:value-of select="../../../ActionCode" />
					</ReqType>
					<SeatCnt>
						<xsl:value-of select="../../../@NumberInParty" />
					</SeatCnt>
					<Filler>0</Filler>
					<LinkAirVInd>0</LinkAirVInd>
				</SellBlk>
			</xsl:when>
			<!--************************************************************************************************************-->
			<!--							Regular Segments								          -->
			<!-- ======================================================================== -->
			<xsl:otherwise>
				<SellBlk>
					<ReqNum>
						<xsl:value-of select="position()" />
					</ReqNum>
					<AirV>
						<xsl:value-of select="MarketingAirline/@Code" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</AirV>
					<FltNum>
						<xsl:value-of select="@FlightNumber" />
					</FltNum>
					<OptSuf><![CDATA[ ]]></OptSuf>
					<BIC>
						<xsl:value-of select="@ResBookDesigCode" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</BIC>
					<Dt>
						<xsl:value-of select="substring(@DepartureDateTime,1,4)" />
						<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
						<xsl:value-of select="substring(@DepartureDateTime,9,2)" />
					</Dt>
					<StartAirp>
						<xsl:value-of select="DepartureAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</StartAirp>
					<EndAirp>
						<xsl:value-of select="ArrivalAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</EndAirp>
					<ReqType>NN</ReqType>
					<SeatCnt>
						<xsl:value-of select="@NumberInParty" />
					</SeatCnt>
					<LinkAirVInd>0</LinkAirVInd>
					<xsl:choose>
						<xsl:when test="position()=last()">
							<ConxInd>N</ConxInd>
						</xsl:when>
						<xsl:otherwise>
							<ConxInd>Y</ConxInd>
						</xsl:otherwise>
					</xsl:choose>
				</SellBlk>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="FlightSegment" mode="Passive">
		<!--******************************************************************************************************-->
		<!--							Passive Segments								          -->
		<!--********************************************************************************************** *******-->
		<AirSegmentSell_6_0>
			<InsertSegAfterModes>
				<SegNum>xsl:value-of select="position() - 1"/></SegNum>
			</InsertSegAfterModes>
			<AirSegMods>
				<AirSegSell>
					<Vnd>
						<xsl:value-of select="MarketingAirline/@Code" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</Vnd>
					<FltNum>
						<xsl:value-of select="@FlightNumber" />
					</FltNum>
					<OptSuf><![CDATA[ ]]></OptSuf>
					<Class>
						<xsl:value-of select="@ResBookDesigCode" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</Class>
					<StartDt>
						<xsl:value-of select="substring(@DepartureDateTime,1,4)" />
						<xsl:value-of select="substring(@DepartureDateTime,6,2)" />
						<xsl:value-of select="substring(@DepartureDateTime,9,2)" />
					</StartDt>
					<StartAirp>
						<xsl:value-of select="DepartureAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</StartAirp>
					<EndAirp>
						<xsl:value-of select="ArrivalAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</EndAirp>
					<Status>
						<xsl:value-of select="../../../ActionCode" />
					</Status>
					<NumPsgrs>
						<xsl:value-of select="../../../@NumberInParty" />
					</NumPsgrs>
					<StartTm>
						<xsl:value-of select="substring(@DepartureDateTime,12,2)" />
						<xsl:value-of select="substring(@DepartureDateTime,15,2)" />
					</StartTm>
					<EndTm>
						<xsl:value-of select="substring(@ArrivalDateTime,12,2)" />
						<xsl:value-of select="substring(@ArrivalDateTime,15,2)" />
					</EndTm>
					<AvailDispType>M</AvailDispType>
				</AirSegSell>
			</AirSegMods>
		</AirSegmentSell_6_0>
	</xsl:template>
	<!--  *****************  -->
	<!--  ***** Hotel *****  -->
	<!--  *****************  -->
	<xsl:template match="HotelReservations">
		<PNRBFReservationModify_5_0>
			<xsl:apply-templates select="HotelReservation" />
		</PNRBFReservationModify_5_0>
	</xsl:template>
	<!--*********************************************************************************-->
	<!--	**						Hotel Sell segments				          **-->
	<!--*********************************************************************************-->
	<xsl:template match="HotelReservation">
		<HtlSegSellMods>
			<xsl:apply-templates select="RoomStays/RoomStay" />
		</HtlSegSellMods>
	</xsl:template>
	<!--*********************************************************************************-->
	<!--	**			Process Hotel Reservation Info 			                  **-->
	<!--*********************************************************************************-->
	<xsl:template match="RoomStay">
		<StartDt>
			<xsl:value-of select="substring(translate(string(TimeSpan/@Start),'-',''),1,8)" />
		</StartDt>
		<EndDt>
			<xsl:value-of select="substring(translate(string(TimeSpan/@End),'-',''),1,8)" />
		</EndDt>
		<NumPersons>
			<xsl:value-of select="GuestCounts/GuestCount/@Count" />
		</NumPersons>
		<NumRooms>
			<xsl:value-of select="RoomRates/RoomRate/@NumberOfUnits" />
		</NumRooms>
		<Chain>
			<xsl:value-of select="BasicPropertyInfo/@ChainCode" />
		</Chain>
		<RoomMasterID>
			<xsl:value-of select="BasicPropertyInfo/@HotelCode" />
		</RoomMasterID>
		<BIC>
			<xsl:value-of select="RoomRates/RoomRate/@BookingCode" />
		</BIC>
		<RateAccess1>
			<xsl:text><![CDATA[ ]]></xsl:text>
		</RateAccess1>
		<RateAccess2>
			<xsl:text><![CDATA[ ]]></xsl:text>
		</RateAccess2>
		<RateAccess3>
			<xsl:text><![CDATA[ ]]></xsl:text>
		</RateAccess3>
		<NumExtraAdults>
			<xsl:value-of select="RoomTypes/RoomType/TPA_Extensions/HotelData/ExtraAdult" />
		</NumExtraAdults>
		<NumExtraChildren>
			<xsl:value-of select="RoomTypes/RoomType/TPA_Extensions/HotelData/ExtraChild" />
		</NumExtraChildren>
		<NumCrib>
			<xsl:value-of select="RoomTypes/RoomType/TPA_Extensions/HotelData/Crib" />
		</NumCrib>
		<NumAdultRollaway>
			<xsl:value-of select="RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayAdult" />
		</NumAdultRollaway>
		<NumChildRollaway>
			<xsl:value-of select="RoomTypes/RoomType/TPA_Extensions/HotelData/RollawayChild" />
		</NumChildRollaway>
		<SellSource>
			<xsl:value-of select="@SourceOfBusiness" />
		</SellSource>
		<RetRuleTxtInd>
			<xsl:text>Y</xsl:text>
		</RetRuleTxtInd>
		<!--*********************************************************************************-->
		<!--	**			Process Optional Fields                 	                        **-->
		<!--*********************************************************************************-->
		<xsl:if test="ResGuestRPHs/ResGuestRPH/@RPH!='' or Guarantee or DepositPayments or SpecialRequests or ../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/Address or ../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty">
			<OptFldIDAry>
				<!--*********************************************************************************-->
				<!--	**			Process Guarantee  Information   	                        **-->
				<!--*********************************************************************************-->
				<xsl:if test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!=''">
					<OptFldID>
						<ID>GT</ID>
						<Contents>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode" />
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardNumber" />
							<xsl:text>EXP</xsl:text>
							<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate" />
						</Contents>
					</OptFldID>
				</xsl:if>
				<!--*********************************************************************************-->
				<!--	**			Process Deposit Information   	                                 **-->
				<!--*********************************************************************************-->
				<xsl:if test="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode!=''">
					<OptFldID>
						<ID>DP</ID>
						<Contents>
							<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode" />
							<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardNumber" />
							<xsl:text>EXP</xsl:text>
							<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@ExpireDate" />
						</Contents>
					</OptFldID>
				</xsl:if>
				<!--*********************************************************************************-->
				<!--	**			Process Special Requests  Information   	                  **-->
				<!--*********************************************************************************-->
				<xsl:if test="SpecialRequests/SpecialRequest">
					<xsl:apply-templates select="SpecialRequests/SpecialRequest" />
				</xsl:if>
				<!--*********************************************************************************-->
				<!--	**			Process Name Associations                      	            **-->
				<!--*********************************************************************************-->
				<xsl:if test="ResGuestRPHs/ResGuestRPH/@RPH!=''">
					<xsl:for-each select="ResGuestRPHs/ResGuestRPH">
						<xsl:variable name="RAN" select="@RPH" />
						<xsl:apply-templates select="../../../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$RAN]"
							mode="hotel" />
					</xsl:for-each>
				</xsl:if>
				<!--*********************************************************************************-->
				<!--	**			Process Address/CustLotalty	                   	            **-->
				<!--*********************************************************************************-->
				<xsl:apply-templates select="../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Address" />
				<xsl:apply-templates select="../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty"
					mode="hotel" />
				<!--*********************************************************************************-->
				<!--	**			Process Booking Source                      	                  **-->
				<!--*********************************************************************************-->
				<xsl:if test="../../../../../../OTA_TravelItineraryRQ/POS/Source/RequestorID/@ID">
					<OptFldID>
						<ID>BS</ID>
						<Contents>
							<xsl:value-of select="../../../../../../OTA_TravelItineraryRQ/POS/Source/RequestorID/@ID" />
						</Contents>
					</OptFldID>
				</xsl:if>
			</OptFldIDAry>
		</xsl:if>
	</xsl:template>
	<!--*********************************************************************************-->
	<!--	**			Name Associations for Hotels       			                  **-->
	<!--*********************************************************************************-->
	<xsl:template match="Traveler" mode="hotel">
		<OptFldID>
			<ID>NF</ID>
			<Contents>
				<xsl:value-of select="PersonName/GivenName" />
			</Contents>
		</OptFldID>
		<OptFldID>
			<ID>NL</ID>
			<Contents>
				<xsl:value-of select="PersonName/Surname" />
			</Contents>
		</OptFldID>
	</xsl:template>
	<!--*********************************************************************************-->
	<!--	**		 	            Address                			                         **-->
	<!--*********************************************************************************-->
	<xsl:template match="Address">
		<OptFldID>
			<ID>AD</ID>
			<Contents>
				<xsl:value-of select="StreetNmbr" />
				<xsl:text>$</xsl:text>
				<xsl:value-of select="CityName" />
				<xsl:text>$</xsl:text>
				<xsl:value-of select="StateProv/@StateCode" />
				<xsl:text>$</xsl:text>
				<xsl:value-of select="CountryName/@Code" />
				<xsl:text>$</xsl:text>
				<xsl:value-of select="PostalCode" />
			</Contents>
		</OptFldID>
	</xsl:template>
	<!--*********************************************************************************-->
	<!--	**		 	           CustLoyalty       			                               **-->
	<!--*********************************************************************************-->
	<xsl:template match="CustLoyalty" mode="hotel">
		<OptFldID>
			<ID>FG</ID>
			<Contents>
				<xsl:value-of select="@ProgramID" />
				<xsl:value-of select="@MembershipID" />
			</Contents>
		</OptFldID>
	</xsl:template>
	<!--************************************************************************-->
	<!--	Add Hotel  Remaining SupplementalInformation		    -->
	<!--************************************************************************-->
	<xsl:template match="SpecialRequest">
		<!-- Supplenetal information -->
		<xsl:if test="@RequestCode='SI'">
			<OptFldID>
				<ID>SI</ID>
				<Contents>
					<xsl:value-of select="Text" />
				</Contents>
			</OptFldID>
		</xsl:if>
		<!-- Overrite Corporate Code  -->
		<xsl:if test="@RequestCode='RT'">
			<OptFldID>
				<ID>RT</ID>
				<Contents>
					<xsl:value-of select="Text" />
				</Contents>
			</OptFldID>
		</xsl:if>
		<!-- Tour Number  -->
		<xsl:if test="@RequestCode='TN'">
			<OptFldID>
				<ID>TN</ID>
				<Contents>
					<xsl:value-of select="Text" />
				</Contents>
			</OptFldID>
		</xsl:if>
		<!-- Room Location -->
		<xsl:if test="@RequestCode='RL'">
			<OptFldID>
				<ID>RL</ID>
				<Contents>
					<xsl:value-of select="Text" />
				</Contents>
			</OptFldID>
		</xsl:if>
		<!-- Meal Plan-->
		<xsl:if test="@RequestCode='MP'">
			<OptFldID>
				<ID>MP</ID>
				<Contents>
					<xsl:value-of select="Text" />
				</Contents>
			</OptFldID>
		</xsl:if>
		<!-- Corporate Discount -->
		<xsl:if test="@RequestCode='CD'">
			<OptFldID>
				<ID>CD</ID>
				<Contents>
					<xsl:value-of select="Text" />
				</Contents>
			</OptFldID>
		</xsl:if>
	</xsl:template>
	<!--  *****************  -->
	<!--  ****** Car ******  -->
	<!--  *****************  -->
	<!--***********************************************************************************-->
	<!--				Car Segment Processing       						  -->
	<!--***********************************************************************************-->
	<xsl:template match="OTA_VehResRQ">
		<PNRBFReservationModify_5_0>
			<CarSegSellMods>
				<StartDt>
					<xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@PickUpDateTime),'-',''),1,8)" />
				</StartDt>
				<StartTm>
					<xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@PickUpDateTime),':',''),12,4)" />
				</StartTm>
				<StartAirV>
					<xsl:value-of select="VehResRQInfo/ArrivalDetails/MarketingCompany/@Code" />
					<xsl:text><![CDATA[ ]]></xsl:text>
				</StartAirV>
				<StartFltNum>
					<xsl:value-of select="VehResRQInfo/ArrivalDetails/@Number" />
				</StartFltNum>
				<Airp>
					<xsl:value-of select="VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode" />
					<xsl:text><![CDATA[  ]]></xsl:text>
				</Airp>
				<LocnCat>
					<xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/CarLocation/@Category" />
				</LocnCat>
				<LocnNum>
					<xsl:if test="string-length(VehResRQCore/TPA_Extensions/CarData/CarLocation/@Number) = '2'">
						<xsl:text>0</xsl:text>
					</xsl:if>
					<xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/CarLocation/@Number" />
				</LocnNum>
				<EndDt>
					<xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@ReturnDateTime),'-',''),1,8)" />
				</EndDt>
				<EndTm>
					<xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@ReturnDateTime),':',''),12,4)" />
				</EndTm>
				<xsl:if test="VehResRQCore/VehRentalCore/ReturnLocation/@LocationCode!= ''">
					<DropLocn>
						<xsl:value-of select="VehResRQCore/VehRentalCore/ReturnLocation/@LocationCode" />
					</DropLocn>
				</xsl:if>
				<CarV>
					<xsl:value-of select="VehResRQCore/VendorPref/@Code" />
				</CarV>
				<CarType>
					<xsl:value-of select="VehResRQCore/VehPref/VehType/@VehicleCategory" />
				</CarType>
				<RateType>
					<xsl:choose>
						<xsl:when test="VehResRQCore/RateQualifier/@RatePeriod='Weekly'">
							<xsl:text>W</xsl:text>
						</xsl:when>
						<xsl:when test="VehResRQCore/RateQualifier/@RatePeriod='Monthly'">
							<xsl:text>M</xsl:text>
						</xsl:when>
						<xsl:when test="VehResRQCore/RateQualifier/@RatePeriod='  WeekendDay'">
							<xsl:text>E</xsl:text>
						</xsl:when>
						<xsl:otherwise>D</xsl:otherwise>
					</xsl:choose>
				</RateType>
				<RateCat>
					<xsl:value-of select="VehResRQCore/RateQualifier/@RateCategory" />
				</RateCat>
				<Rate>
					<xsl:value-of select="VehResRQCore/RateQualifier/@RateQualifier" />
				</Rate>
				<Currency>
					<xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/CarRate/@Currency" />
				</Currency>
				<NumCars>
					<xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/@NumCars" />
				</NumCars>
				<RetRuleTxtInd>Y</RetRuleTxtInd>
				<!--*********************************************************************************-->
				<!--	**			Process Optional Fields                 	                        **-->
				<!--*********************************************************************************-->
				<xsl:if test="VehResRQCore/RateQualifier/@CorpDiscountNmbr or VehResRQInfo/RentalPaymentPref/PaymentCard or VehResRQInfo/@SmokingAllowed='0' or ../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/Address or ../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty">
					<OptFldIDAry>
						<!--*********************************************************************************-->
						<!--	**			Process Guarantee  Information   	                        **-->
						<!--*********************************************************************************-->
						<xsl:if test="VehResRQInfo/RentalPaymentPref/PaymentCard">
							<OptFldID>
								<ID>GT</ID>
								<Contents>
									<xsl:value-of select="VehResRQInfo/RentalPaymentPref/PaymentCard/@CardCode" />
									<xsl:value-of select="VehResRQInfo/RentalPaymentPref/PaymentCard/@CardNumber" />
									<xsl:text>EXP</xsl:text>
									<xsl:value-of select="VehResRQInfo/RentalPaymentPref/PaymentCard/@ExpireDate" />
								</Contents>
							</OptFldID>
						</xsl:if>
						<!--*********************************************************************************-->
						<!--	**			Process Special Requests  Information   	                  **-->
						<!--*********************************************************************************-->
						<xsl:if test="VehResRQInfo/@SmokingAllowed='0'">
							<OptFldID>
								<ID>SI</ID>
								<Contents>NONSMOKING</Contents>
							</OptFldID>
						</xsl:if>
						<!--*********************************************************************************-->
						<!--	**			Process Name Associations                      	            **-->
						<!--*********************************************************************************-->
						<xsl:if test="VehResRQCore/RateQualifier/@CorpDiscountNmbr!=''">
							<OptFldID>
								<ID>CD</ID>
								<Contents>
									<xsl:value-of select="VehResRQCore/RateQualifier/@CorpDiscountNmbr" />
								</Contents>
							</OptFldID>
						</xsl:if>
						<!--*********************************************************************************-->
						<!--	**			Process Address/CustLotalty	                   	            **-->
						<!--*********************************************************************************-->
						<xsl:apply-templates select="../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Address" />
						<xsl:apply-templates select="../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty"
							mode="car" />
						<!--*********************************************************************************-->
						<!--	**			Process Booking Source                      	                  **-->
						<!--*********************************************************************************-->
						<xsl:if test="../../OTA_TravelItineraryRQ/POS/Source/RequestorID/@ID">
							<OptFldID>
								<ID>BS</ID>
								<Contents>
									<xsl:value-of select="../../OTA_TravelItineraryRQ/POS/Source/RequestorID/@ID" />
								</Contents>
							</OptFldID>
						</xsl:if>
					</OptFldIDAry>
				</xsl:if>
			</CarSegSellMods>
		</PNRBFReservationModify_5_0>
	</xsl:template>
	<!--*********************************************************************************-->
	<!--	**		 	           CustLoyalty       			                               **-->
	<!--*********************************************************************************-->
	<xsl:template match="CustLoyalty" mode="car">
		<OptFldID>
			<ID>FT</ID>
			<Contents>
				<xsl:value-of select="@ProgramID" />
				<xsl:value-of select="@MembershipID" />
			</Contents>
		</OptFldID>
	</xsl:template>
	<!--  *****************  -->
	<!--  ***** Seats *****  -->
	<!--  *****************  -->
	<xsl:template match="OTA_AirBookRQ" mode="seat">
		<xsl:if test="TravelerInfo/SpecialReqDetails/SeatRequests">
			<PNRBFReservationModify_5_0>
				<SeatSellMods>
					<ReqAry>
						<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/SeatRequests/SeatRequest" />
					</ReqAry>
				</SeatSellMods>
			</PNRBFReservationModify_5_0>
		</xsl:if>
	</xsl:template>
	<!--******************************************************************************************************-->
	<!--							Seat Requests                 							    -->
	<!--******************************************************************************************************-->
	<xsl:template match="SeatRequest">
		<xsl:variable name="SRPH" select="@FlightRefNumberRPHList" />
		<ReqInfo>
			<ID>R</ID>
			<Num>
				<xsl:value-of select="position()" />
			</Num>
			<AirV>
				<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code" />
				<xsl:text><![CDATA[ ]]></xsl:text>
			</AirV>
			<FltNum>
				<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@FlightNumber" />
			</FltNum>
			<OpSuf />
			<Dt>
				<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@DepartureDateTime,1,10),'-','')" />
			</Dt>
			<StartAirp>
				<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/DepartureAirport/@LocationCode" />
				<xsl:text><![CDATA[  ]]></xsl:text>
			</StartAirp>
			<EndAirp>
				<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/ArrivalAirport/@LocationCode" />
				<xsl:text><![CDATA[  ]]></xsl:text>
			</EndAirp>
			<BIC>
				<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@ResBookDesigCode" />
				<xsl:text><![CDATA[ ]]></xsl:text>
			</BIC>
			<xsl:choose>
				<xsl:when test="@SeatPreference ='None'">
					<ReqType>S</ReqType>
					<SeatQual>
						<SeatAry>
							<xsl:variable name="number">
								<xsl:choose>
									<xsl:when test="string-length(string(.))=2">
										<xsl:text>0</xsl:text>
										<xsl:value-of select="@SeatNumber" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@SeatNumber" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<Seat>
								<xsl:text disable-output-escaping="yes">&lt;![CDATA[  </xsl:text>
								<xsl:value-of disable-output-escaping="yes" select="$number" />
								<xsl:text disable-output-escaping="yes">]]&gt;</xsl:text>
							</Seat>
						</SeatAry>
					</SeatQual>
				</xsl:when>
				<xsl:otherwise>
					<ReqType>G</ReqType>
					<AttribQual>
						<AttribAry>
							<Attrib>
								<xsl:value-of select="@SeatPreference" />
							</Attrib>
						</AttribAry>
					</AttribQual>
				</xsl:otherwise>
			</xsl:choose>
		</ReqInfo>
	</xsl:template>
	<!--  *****************  -->
	<!--  ***** Names *****  -->
	<!--  *****************  -->
	<xsl:template match="OTA_TravelItineraryRQ" mode="names">
		<AgencyPNRBFBuildModify_6_0>
			<PNRBFPrimaryBldChgMods>
				<ItemAry>
					<xsl:apply-templates select="TPA_Extensions/PNRData" />
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address"
						mode="Billing" />
					<Item>
						<DataBlkInd>E</DataBlkInd>
						<EndMarkQual>
							<EndMark>E</EndMark>
						</EndMarkQual>
					</Item>
				</ItemAry>
			</PNRBFPrimaryBldChgMods>
			<xsl:apply-templates select="OTA_AirBookRQ" mode="names" />
		</AgencyPNRBFBuildModify_6_0>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--			Process Names,Telephone, Email, Customer Loyalty	    	                -->
	<!-- ********************************************************************************************-->
	<xsl:template match="PNRData">
		<xsl:apply-templates select="Traveler" mode="names" />
		<xsl:apply-templates select="Telephone" />
		<xsl:apply-templates select="Email" />
		<xsl:apply-templates select="Ticketing" />
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Process Air Itinerary     	               			     -->
	<!-- ********************************************************************************************-->
	<xsl:template match="OTA_AirBookRQ" mode="names">
		<PNRBFSecondaryBldChgMods>
			<ItemAry>
				<!--  FOP Qual       -->
				<xsl:apply-templates select="Fulfillment/PaymentDetails/PaymentDetail" />
				<!-- Delivery Address Qual  -->
				<xsl:apply-templates select="Fulfillment/DeliveryAddress" />
				<!-- OSI Qual  -->
				<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/OtherServiceInformations/OtherServiceInformation" />
				<!-- SSR Qual -->
				<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/SpecialServiceRequests/SpecialServiceRequest" />
				<!-- TkRmkQual  -->
				<xsl:apply-templates select="Ticketing/TicketAdvisory" />
				<!-- GenRmkQual -->
				<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/Remarks/Remark" />
				<!-- AssocRmkQual  -->
				<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='Air']"
					mode="Air" />
				<xsl:choose>
					<xsl:when test="//POS/BookingChannel/CompanyName/@Code='1G'">
						<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='Invoice']"
							mode="Galileo" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='Invoice']"
							mode="Apollo" />
					</xsl:otherwise>
				</xsl:choose>
				<Item>
					<DataBlkInd>
						<xsl:text><![CDATA[E ]]></xsl:text>
					</DataBlkInd>
					<EndQual>
						<EndMark>E</EndMark>
					</EndQual>
				</Item>
			</ItemAry>
		</PNRBFSecondaryBldChgMods>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--								Ticketing					                         	   -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Ticketing">
		<Item>
			<DataBlkInd>
				<xsl:text><![CDATA[T ]]></xsl:text>
			</DataBlkInd>
			<TkQual>
				<Tk>
					<xsl:text>TL</xsl:text>
					<xsl:value-of select="substring(@TicketTimeLimit,12,2)" />
					<xsl:value-of select="substring(@TicketTimeLimit,15,2)" />
					<xsl:text>/</xsl:text>
					<xsl:value-of select="substring(@TicketTimeLimit,9,2)" />
					<xsl:call-template name="month">
						<xsl:with-param name="month">
							<xsl:value-of select="substring(@TicketTimeLimit,6,2)" />
						</xsl:with-param>
					</xsl:call-template>
				</Tk>
			</TkQual>
		</Item>
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
	<!-- ********************************************************************************************-->
	<!--								Passenger Name			                             -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Traveler" mode="names">
		<Item>
			<DataBlkInd>
				<xsl:text><![CDATA[N ]]></xsl:text>
			</DataBlkInd>
			<NameQual>
				<EditTypeInd>A</EditTypeInd>
				<EditTypeIndAppliesTo />
				<AddChgNameRmkQual>
					<LNameID>
						<xsl:value-of select="position()" />
					</LNameID>
					<LName>
						<xsl:value-of select="PersonName/Surname" />
					</LName>
					<xsl:choose>
						<xsl:when test="@PassengerTypeCode='ADT'">
							<LNameRmk>ADT</LNameRmk>
						</xsl:when>
						<xsl:when test="@PassengerTypeCode='CHD' and @Age != ''">
							<LNameRmk>C<xsl:value-of select="@Age" /></LNameRmk>
						</xsl:when>
						<xsl:when test="@PassengerTypeCode='SRC' and @Age != ''">
							<LNameRmk>SC<xsl:value-of select="@Age" /></LNameRmk>
						</xsl:when>
						<xsl:when test="@PassengerTypeCode='YTH' and @Age != ''">
							<LNameRmk>YC<xsl:value-of select="@Age" /></LNameRmk>
						</xsl:when>
						<xsl:when test="@PassengerTypeCode='INF'">
							<NameType>I</NameType>
							<LNameRmk>INF</LNameRmk>
						</xsl:when>
						<xsl:otherwise>
							<LNameRmk>
								<xsl:value-of select="@PassengerTypeCode" />
							</LNameRmk>
						</xsl:otherwise>
					</xsl:choose>
					<NameTypeQual>
						<FNameAry>
							<FNameItem>
								<PsgrNum>
									<xsl:value-of select="position()" />
								</PsgrNum>
								<AbsNameNum>
									<xsl:value-of select="position()" />
								</AbsNameNum>
								<FName>
									<xsl:value-of select="PersonName/GivenName" />
									<xsl:value-of select="string(' ')" />
									<xsl:value-of select="PersonName/MiddleName" />
									<xsl:value-of select="string(' ')" />
									<xsl:value-of select="PersonName/NameTitle" />
								</FName>
							</FNameItem>
						</FNameAry>
					</NameTypeQual>
				</AddChgNameRmkQual>
			</NameQual>
		</Item>
		<!-- ********************************************************************************************-->
		<!--						Frequent Customer Number			                            -->
		<!-- ********************************************************************************************-->
		<xsl:if test="CustLoyalty">
			<Item>
				<DataBlkInd>M</DataBlkInd>
				<FreqCustQual>
					<EditTypeInd>A</EditTypeInd>
					<AddQual>
						<FreqCustNum>
							<xsl:variable name="CLCode">
								<xsl:value-of select="CustLoyalty/@ProgramID" />
							</xsl:variable>
							<xsl:if test="string-length($CLCode)='2'">
								<xsl:value-of select="string(' ')" />
							</xsl:if>
							<xsl:value-of select="$CLCode" />
							<xsl:value-of select="string(' ')" />
							<xsl:value-of select="CustLoyalty/@MembershipID" />
						</FreqCustNum>
					</AddQual>
				</FreqCustQual>
			</Item>
		</xsl:if>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--								Telephone						                      -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Telephone">
		<Item>
			<DataBlkInd><![CDATA[P ]]></DataBlkInd>
			<PhoneQual>
				<EditTypeInd>A</EditTypeInd>
				<AddPhoneQual>
					<xsl:if test="@AreaCityCode!=''">
						<City>
							<xsl:value-of select="@AreaCityCode" />
							<xsl:text><![CDATA[  ]]></xsl:text>
						</City>
					</xsl:if>
					<Type>
						<xsl:choose>
							<xsl:when test="@PhoneLocationType='Home'">
								<xsl:text>R</xsl:text>
							</xsl:when>
							<xsl:when test="@PhoneLocationType='Business'">
								<xsl:text>B</xsl:text>
							</xsl:when>
							<xsl:when test="@PhoneLocationType='Agency'">
								<xsl:text>A</xsl:text>
							</xsl:when>
							<xsl:when test="@PhoneLocationType='Fax'">
								<xsl:text>F</xsl:text>
							</xsl:when>
							<xsl:otherwise>X</xsl:otherwise>
						</xsl:choose>
					</Type>
					<PhoneNumber>
						<xsl:value-of select="@PhoneNumber" />
					</PhoneNumber>
				</AddPhoneQual>
			</PhoneQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--								Email						                            -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Email">
		<Item>
			<DataBlkInd><![CDATA[P ]]></DataBlkInd>
			<PhoneQual>
				<EditTypeInd>A</EditTypeInd>
				<AddPhoneQual>
					<City>
						<xsl:text>XXX<![CDATA[  ]]></xsl:text>
					</City>
					<Type>E</Type>
					<xsl:variable name="email">
						<xsl:choose>
							<xsl:when test="contains(string(.),'_')">
								<xsl:value-of select="substring-before(string(.),'_')"/>
								<xsl:text>--</xsl:text>
								<xsl:value-of select="substring-after(string(.),'_')"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="email1">
						<xsl:choose>
							<xsl:when test="contains($email,'@')">
								<xsl:value-of select="substring-before($email,'@')"/>
								<xsl:text>//</xsl:text>
								<xsl:value-of select="substring-after($email,'@')"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<PhoneNumber><xsl:value-of select="$email1"/></PhoneNumber>
				</AddPhoneQual>
			</PhoneQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Fulfillment (FOP)                                                         -->
	<!-- ********************************************************************************************-->
	<xsl:template match="PaymentDetail">
		<Item>
			<DataBlkInd><![CDATA[F ]]></DataBlkInd>
			<FOPQual>
				<EditTypeInd>A</EditTypeInd>
				<AddChgQual>
					<xsl:choose>
						<xsl:when test="PaymentCard">
							<TypeInd>2</TypeInd>
							<CCQual>
								<CC>
									<xsl:value-of select="PaymentCard/@CardCode" />
								</CC>
								<ExpDt>
									<xsl:value-of select="PaymentCard/@ExpireDate" />
								</ExpDt>
								<Acct>
									<xsl:value-of select="PaymentCard/@CardNumber" />
								</Acct>
							</CCQual>
						</xsl:when>
						<xsl:when test="@Type='CASH'">
							<TypeInd>1</TypeInd>
							<VarLenQual>
								<FOP>S</FOP>
							</VarLenQual>
						</xsl:when>
						<xsl:when test="@Type='CHECK'">
							<TypeInd>1</TypeInd>
							<VarLenQual>
								<FOP>CK</FOP>
							</VarLenQual>
						</xsl:when>
					</xsl:choose>
				</AddChgQual>
			</FOPQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Billing Address                                                          -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Address" mode="Billing">
		<Item>
			<DataBlkInd><![CDATA[W ]]></DataBlkInd>
			<AddrQual>
				<EditTypeInd>A</EditTypeInd>
				<AddChgAddr>
					<Addr>
						<xsl:if test="../CardHolderName!=''">
							<xsl:value-of select="../CardHolderName" />
							<xsl:text>@</xsl:text>
						</xsl:if>
						<xsl:value-of select="StreetNmbr" />
						<xsl:text>@</xsl:text>
						<xsl:value-of select="CityName" />
						<xsl:if test="StateProv">
							<xsl:text>@</xsl:text>
							<xsl:value-of select="StateProv" />
						</xsl:if>
						<xsl:text><![CDATA[ Z/]]></xsl:text>
						<xsl:value-of select="PostalCode" />
						<xsl:text>@</xsl:text>
						<xsl:value-of select="CountryName" />
					</Addr>
				</AddChgAddr>
			</AddrQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Delivery Address                                                      -->
	<!-- ********************************************************************************************-->
	<xsl:template match="DeliveryAddress">
		<Item>
			<DataBlkInd><![CDATA[D ]]></DataBlkInd>
			<DeliveryAddrQual>
				<EditTypeInd>A</EditTypeInd>
				<AddChgQual>
					<DeliveryAddr>
						<xsl:value-of select="StreetNmbr" />
						<xsl:text>@</xsl:text>
						<xsl:value-of select="CityName" />
						<xsl:if test="StateProv">
							<xsl:text>@</xsl:text>
							<xsl:value-of select="StateProv" />
						</xsl:if>
						<xsl:text><![CDATA[ Z/]]></xsl:text>
						<xsl:value-of select="PostalCode" />
						<xsl:text>@</xsl:text>
						<xsl:value-of select="CountryName" />
					</DeliveryAddr>
				</AddChgQual>
			</DeliveryAddrQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--						Special Remarks    (Air)                                                   -->
	<!-- ********************************************************************************************-->
	<xsl:template match="SpecialRemark" mode="Air">
		<Item>
			<xsl:variable name="SRPH" select="FlightRefNumber/@RPH" />
			<DataBlkInd><![CDATA[H ]]></DataBlkInd>
			<AssocRmkQual>
				<EditTypeInd>A</EditTypeInd>
				<AirQual>
					<StartDt>
						<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/			@DepartureDateTime,1,10),'-','')" />
					</StartDt>
					<AirV>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</AirV>
					<StartAirp>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/DepartureAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</StartAirp>
					<EndAirp>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/ArrivalAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</EndAirp>
					<FltNum>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@FlightNumber" />
					</FltNum>
					<OpSuf><![CDATA[ ]]></OpSuf>
					<BIC>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@ResBookDesigCode" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</BIC>
				</AirQual>
				<AssocRmkTypeQual>
					<EditTypeInd>A</EditTypeInd>
					<AddQual>
						<RmkAry>
							<RmkInfo>
								<Rmk>
									<xsl:value-of select="Text" />
								</Rmk>
							</RmkInfo>
						</RmkAry>
					</AddQual>
				</AssocRmkTypeQual>
			</AssocRmkQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Ticketing Remarks                                                     -->
	<!-- ********************************************************************************************-->
	<xsl:template match="TicketAdvisory">
		<Item>
			<DataBlkInd><![CDATA[K ]]></DataBlkInd>
			<TkRmkQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Rmk>
						<xsl:value-of select="." />
					</Rmk>
				</AddQual>
			</TkRmkQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Remarks (General)                                                    -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Remark">
		<Item>
			<DataBlkInd><![CDATA[G ]]></DataBlkInd>
			<GenRmkQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Rmk>
						<xsl:value-of select="." />
					</Rmk>
				</AddQual>
			</GenRmkQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--				Other Service Information (OSIs)                                                  -->
	<!-- ********************************************************************************************-->
	<xsl:template match="OtherServiceInformation">
		<Item>
			<xsl:variable name="SRPH" select="FlightRefNumber/@RPH" />
			<DataBlkInd><![CDATA[O ]]></DataBlkInd>
			<OSIQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<OSIV>
						<xsl:choose>
							<xsl:when test="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code!=''">
								<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code" />
								<xsl:text><![CDATA[ ]]></xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>YY </xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</OSIV>
					<OSI>
						<xsl:value-of select="Text" />
					</OSI>
				</AddQual>
			</OSIQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--					Special Service Requests (SSRs)                                          -->
	<!-- ********************************************************************************************-->
	<xsl:template match="SpecialServiceRequest">
		<xsl:variable name="SRPH" select="@FlightRefNumberRPHList" />
		<Item>
			<DataBlkInd><![CDATA[S ]]></DataBlkInd>
			<SSRQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<xsl:if test="@TravelerRefNumberRPHList!=''">
						<LNameNum>0<xsl:value-of select="@TravelerRefNumberRPHList" />
								</LNameNum>
						<PsgrNum>01</PsgrNum>
						<AbsNameNum>0</AbsNameNum>
					</xsl:if>
					<SSRCode>
						<xsl:value-of select="@SSRCode" />
					</SSRCode>
					<FltNum>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@FlightNumber" />
					</FltNum>
					<OpSuf><![CDATA[ ]]></OpSuf>
					<AirV>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</AirV>
					<Dt>
						<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@DepartureDateTime,1,10),'-','')" />
					</Dt>
					<BIC>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@ResBookDesigCode" />
						<xsl:text><![CDATA[ ]]></xsl:text>
					</BIC>
					<StartAirp>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/DepartureAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</StartAirp>
					<EndAirp>
						<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/ArrivalAirport/@LocationCode" />
						<xsl:text><![CDATA[  ]]></xsl:text>
					</EndAirp>
					<xsl:if test="Text != ''">
						<Text><xsl:value-of select="Text"/></Text>
					</xsl:if>
				</AddQual>
			</SSRQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--						Special Remarks    (Invoicel)                                            -->
	<!-- ********************************************************************************************-->
	<xsl:template match="SpecialRemark" mode="Galileo">
		<Item>
			<DataBlkInd><![CDATA[X ]]></DataBlkInd>
			<DOCInvoiceQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Keyword>
						<xsl:choose>
							<xsl:when test="Keyword='DYO'">DYO</xsl:when>
							<xsl:when test="Keyword='BBA'">CA</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Keyword" />
							</xsl:otherwise>
						</xsl:choose>
					</Keyword>
					<Text>
						<xsl:value-of select="Text" />
					</Text>
				</AddQual>
			</DOCInvoiceQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--						Special Remarks    (Invoicel)                                            -->
	<!-- ********************************************************************************************-->
	<xsl:template match="SpecialRemark" mode="Apollo">
		<Item>
			<DataBlkInd><![CDATA[K ]]></DataBlkInd>
			<TkRmkQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Rmk>
						<xsl:value-of select="InvoiceKey" />
						<xsl:value-of select="Text" />
					</Rmk>
				</AddQual>
			</TkRmkQual>
		</Item>
	</xsl:template>
	<!--  *****************  -->
	<!--  ***** Price *****  -->
	<!--  *****************  -->
	<!--***********************************************************************-->
	<!--**              	Main Store Price	                                            **-->
	<!--***********************************************************************-->
	<xsl:template match="OTA_AirBookRQ" mode="price">
		<xsl:if test="../TPA_Extensions/PriceData">
			<FareQuoteStorePrice_8_0>
				<StorePriceMods>
					<xsl:choose>
						<!--***********************************************************************-->
						<!--**                   Negotiated fares                                              -->
						<!--***********************************************************************-->
						<xsl:when test="../TPA_Extensions/PriceData/@PriceType='Private'">
							<xsl:apply-templates select="../TPA_Extensions/PriceData/NegoFares/PriceRequestInformation/NegotiatedFareCode" />
						</xsl:when>
						<xsl:otherwise>
							<!--***********************************************************************-->
							<!--**                     Published Fares                                             -->
							<!--***********************************************************************-->
							<xsl:apply-templates select="../TPA_Extensions/PriceData/PublishedFares/FareRestrictPref" />
							<SegSelection>
								<ReqAirVPFs>N</ReqAirVPFs>
								<SegRangeAry>
									<SegRange>
										<StartSeg>00</StartSeg>
										<EndSeg>00</EndSeg>
										<FareType>N</FareType>
									</SegRange>
								</SegRangeAry>
							</SegSelection>
						</xsl:otherwise>
					</xsl:choose>
					<!--***********************************************************************-->
					<!--**                      Passenger Info                                              -->
					<!--***********************************************************************-->
					<PsgrMods>
						<PsgrAry>
							<xsl:apply-templates select="../TPA_Extensions/PNRData/Traveler" mode="PassengerInfo" />
						</PsgrAry>
					</PsgrMods>
					<DocProdFareType>
						<Type>
							<xsl:choose>
								<xsl:when test="../TPA_Extensions/PriceData/@PriceType='Private'">
									<xsl:text>P</xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>N</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</Type>
					</DocProdFareType>
				</StorePriceMods>
			</FareQuoteStorePrice_8_0>
		</xsl:if>
	</xsl:template>
	<!--***********************************************************************-->
	<!--**                       Set Fare Qualifiers                                       -->
	<!--***********************************************************************-->
	<xsl:template match="FareRestrictPref">
		<GenFarePrefs>
			<xsl:choose>
				<xsl:when test="VoluntaryChanges/Penalty[not(@PenaltyType)]">
					<Pen>
						<xsl:value-of select="string('  ')" />
					</Pen>
				</xsl:when>
				<xsl:when test="VoluntaryChanges/Penalty/@PenaltyType = 'NonRef'">
					<Pen>99</Pen>
				</xsl:when>
				<xsl:when test="VoluntaryChanges/Penalty/@PenaltyType = 'Ref'">
					<Pen>00</Pen>
				</xsl:when>
				<xsl:otherwise>
					<Pen>00</Pen>
				</xsl:otherwise>
			</xsl:choose>
			<MinStay>
				<xsl:choose>
					<xsl:when test="MinimumStay">1</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</MinStay>
			<MaxStay>
				<xsl:choose>
					<xsl:when test="MaximumStay">1</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</MaxStay>
			<AP>
				<xsl:choose>
					<xsl:when test="AdvResTicketing/AdvReservation">1</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</AP>
		</GenFarePrefs>
	</xsl:template>
	<!--***********************************************************************-->
	<!--**                       Negotiated Fares                                           -->
	<!--***********************************************************************-->
	<xsl:template match="NegotiatedFareCode">
		<SegSelection>
			<ReqAirVPFs>Y</ReqAirVPFs>
			<SegRangeAry>
				<SegRange>
					<StartSeg>00</StartSeg>
					<EndSeg>00</EndSeg>
					<FareType>P</FareType>
					<PFQual>
						<CRSInd>
							<xsl:value-of select="@SupplierCode" />
						</CRSInd>
						<PCC>
							<xsl:value-of select="../../../../../POS/Source/@PseudoCityCode" />
						</PCC>
						<xsl:if test="@SecondaryCode != ''">
							<Acct>
								<xsl:value-of select="@SecondaryCode" />
							</Acct>
						</xsl:if>
						<Contract>
							<!--xsl:value-of select="@Code"/--></Contract>
						<PublishedFaresInd>N</PublishedFaresInd>
						<Type>A</Type>
					</PFQual>
				</SegRange>
			</SegRangeAry>
		</SegSelection>
	</xsl:template>
	<!--***********************************************************************-->
	<!--**                    Passenger Info                                                 -->
	<!--***********************************************************************-->
	<xsl:template match="Traveler" mode="PassengerInfo">
		<Psgr>
			<LNameNum>
				<xsl:value-of select="position()" />
			</LNameNum>
			<PsgrNum>1</PsgrNum>
			<AbsNameNum>1</AbsNameNum>
			<PIC>
				<xsl:value-of select="@PassengerTypeCode" />
			</PIC>
		</Psgr>
	</xsl:template>
	<xsl:template match="Traveler" mode="Association">
		<Psgr>
			<LNameNum>
				<xsl:value-of select="position()" />
			</LNameNum>
			<PsgrNum>1</PsgrNum>
			<AbsNameNum>1</AbsNameNum>
		</Psgr>
	</xsl:template>
	<!--  *****************  -->
	<!--  ***** End T *****  -->
	<!--  *****************  -->
	<xsl:template match="OTA_TravelItineraryRQ" mode="et">
		<PNRBFEnd_7_0>
			<EndTransactionMods>
				<xsl:choose>
					<xsl:when test="OTA_AirBookRQ/Queue">
						<TypeInd>Q</TypeInd>
						<RcvdFrom>
							<xsl:value-of select="POS/Source/@AgentSine" />
						</RcvdFrom>
						<ChgQEPQual>
							<PCC>
								<xsl:value-of select="OTA_AirBookRQ/Queue/@PseudoCityCode" />
							</PCC>
							<PCCQNum>
								<xsl:value-of select="OTA_AirBookRQ/Queue/@QueueNumber" />
							</PCCQNum>
							<xsl:if test="OTA_AirBookRQ/Queue/@QueueCategory">
								<QCat>
									<xsl:value-of select="OTA_AirBookRQ/Queue/@QueueCategory" />
								</QCat>
							</xsl:if>
						</ChgQEPQual>
					</xsl:when>
					<xsl:otherwise>
						<TypeInd>E</TypeInd>
						<RcvdFrom>
							<xsl:value-of select="POS/Source/@AgentSine" />
						</RcvdFrom>
					</xsl:otherwise>
				</xsl:choose>
			</EndTransactionMods>
		</PNRBFEnd_7_0>
	</xsl:template>
</xsl:stylesheet>
