<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
  Galileo_UpdateInsertRQ.xsl 														
  ================================================================== 
  Date: 01 Mar 2022 - Kobelev - Get Current PNR based on session or not.
  Date: 25 Feb 2022 - Kobelev - Isert Remarks fix.
  Date: 29 Jan 2021 - Kobelev - added Historical Remarks
  Date: 12 Nov 2020 - Kobelev - Invoice and Back Office Remarks
  Date: 25 Aug 2014 - Rastko - added support for ER option						 		
  Date: 20 Aug 2014 - Rastko - added support for OTA_UpdateSessionedRQ message	 		
  Date: 08 Dec 2012 - Rastko - added support for inserting loyalty info	 		  
  Date: 14 Feb 2010 - Rastko - added support for inserting telephone and email 		
  ================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="GDS">
		<xsl:value-of select="UpdateInsert/OTA_UpdateRQ/POS/TPA_Extensions/Provider/Name"/>
	</xsl:variable>
	<xsl:template match="/">
		<UpdateInsert>
			<MultiElements>
				<PNRBFManagement_53>
					<xsl:apply-templates select="UpdateInsert/OTA_UpdateRQ | UpdateInsert/OTA_UpdateSessionedRQ" mode="start" />
				</PNRBFManagement_53>
			</MultiElements>
			<ET>
				<PNRBFManagement_53>
					<EndTransactionMods>
						<EndTransactRequest>
							<ETInd>
								<xsl:choose>
									<xsl:when test="UpdateInsert/OTA_UpdateRQ">
										<xsl:value-of select="'E'"/>
									</xsl:when>
									<xsl:when test="UpdateInsert/OTA_UpdateSessionedRQ">
										<xsl:value-of select="'R'"/>
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</ETInd>
							<RcvdFrom>
								<xsl:choose>
									<xsl:when test="UpdateInsert/OTA_UpdateRQ/POS/Source/@AgentSine != ''">
										<xsl:value-of select="UpdateInsert/OTA_UpdateRQ/POS/Source/@AgentSine"/>
									</xsl:when>
									<xsl:when test="UpdateInsert/OTA_UpdateSessionedRQ/POS/Source/@AgentSine != ''">
										<xsl:value-of select="UpdateInsert/OTA_UpdateSessionedRQ/POS/Source/@AgentSine"/>
									</xsl:when>
									<xsl:otherwise>TRIPXML</xsl:otherwise>
								</xsl:choose>
							</RcvdFrom>
						</EndTransactRequest>
					</EndTransactionMods>
				</PNRBFManagement_53>
			</ET>
		</UpdateInsert>
	</xsl:template>

	<xsl:template match="OTA_UpdateRQ | UpdateInsert/OTA_UpdateSessionedRQ" mode="start">
		<xsl:apply-templates select="Position/Element/PNRData"/>
		<xsl:apply-templates select="Position/Element[@Operation='insert' and @Child='PNRData'][PNRData/Telephone]" mode="telephone"/>
		<!--xsl:apply-templates select="OTA_UpdateRQ/Position/Element[@Operation='insert' and @Child='Traveler']"/-->
		<xsl:if test="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment">
			<AirSegSellMods>
				<xsl:choose>
					<xsl:when test="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/@ActionCode = 'Other'">
						<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="Passive" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption" mode="Regular" />
					</xsl:otherwise>
				</xsl:choose>
			</AirSegSellMods>
		</xsl:if>
		<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation" />
		<xsl:apply-templates select="OTA_VehResRQ" />
		<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/UniqueRemarks"/>
		<xsl:if test="Position/Element[@Operation='insert' and (@Child='Remarks' or @Child='SpecialRemarks' or @Child='SpecialServiceRequests')] or Position/Element[@Operation='insert' and @Child='PNRData'][PNRData/Email]">
			<PNRBFRetrieveMods>
				<xsl:choose>
					<xsl:when test="ConversationID!=''"><CurrentPNR /></xsl:when>
					<xsl:otherwise>
						<PNRAddr>
							<FileAddr />
							<CodeCheck />
							<RecLoc>
								<xsl:value-of select="UniqueID/@ID" />
							</RecLoc>
						</PNRAddr>
					</xsl:otherwise>
				</xsl:choose>
			</PNRBFRetrieveMods>
			
			<PNRBFSecondaryBldChgMods>
				<ItemAry>
					<!-- create CHD OSI -->
					<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='CHD']" mode="osi" />
					<!-- SSR, OSI -->
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails"/>
					<!-- Remark, Special Remark -->
					<xsl:apply-templates select="Position/Element[@Operation='insert' and @Child='Remarks']"/>
					<xsl:apply-templates select="Position/Element[@Operation='insert' and @Child='SpecialRemarks']"/>
					<xsl:apply-templates select="Position/Element[@Operation='insert' and @Child='SpecialServiceRequests']"/>
					<!--  FOP Qual       -->
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail" />
					<!-- Delivery Address Qual  -->
					<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
					<!-- Email -->
					<xsl:apply-templates select="Position/Element/PNRData/Email"/>
					<!-- Ticketing remarks -->
					<xsl:apply-templates select="TPA_Extensions/PNRData/AccountingLine"/>
					<!-- accounting remarks -->
					<xsl:apply-templates select="TPA_Extensions/AgencyData/ServiceFee"/>
					<Item>
						<DataBlkInd>
							<xsl:text>E</xsl:text>
						</DataBlkInd>
						<EndQual>
							<EndMark>E</EndMark>
						</EndQual>
					</Item>
				</ItemAry>
			</PNRBFSecondaryBldChgMods>
			<xsl:if test="@RemarkType=''">
				<xsl:call-template name="FQDisplay" />
			</xsl:if>

		</xsl:if>
		<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests">
			<SeatSellMods>
				<ReqAry>
					<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SeatRequests" />
				</ReqAry>
			</SeatSellMods>
		</xsl:if>
		<xsl:if test="TPA_Extensions/PriceData">
			<xsl:apply-templates select="TPA_Extensions/PriceData"/>
		</xsl:if>
		<EndTransactionMods>
			<EndTransactRequest>
				<ETInd>
					<xsl:choose>
						<xsl:when test="TPA_Extensions/PNRData/Queue">Q</xsl:when>
						<xsl:otherwise>R</xsl:otherwise>
					</xsl:choose>
				</ETInd>
				<RcvdFrom>
					<xsl:choose>
						<xsl:when test="POS/Source/@AgentSine != ''">
							<xsl:value-of select="POS/Source/@AgentSine"/>
						</xsl:when>
						<xsl:otherwise>TRIPXML</xsl:otherwise>
					</xsl:choose>
				</RcvdFrom>
			</EndTransactRequest>
			<xsl:if test="TPA_Extensions/PNRData/Queue">
				<GlobalAccessInfo>
					<GlobAccessCRSAry>
						<GlobAccessCRS>
							<CRS>
								<xsl:choose>
									<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'">1V</xsl:when>
									<xsl:otherwise>1G</xsl:otherwise>
								</xsl:choose>
							</CRS>
							<PCC>
								<xsl:value-of select="TPA_Extensions/PNRData/Queue/@PseudoCityCode" />
							</PCC>
							<QNum>
								<xsl:value-of select="TPA_Extensions/PNRData/Queue/@QueueNumber" />
							</QNum>
							<xsl:if test="TPA_Extensions/PNRData/Queue/@QueueCategory">
								<QCat>
									<xsl:value-of select="TPA_Extensions/PNRData/Queue/@QueueCategory" />
								</QCat>
							</xsl:if>
						</GlobAccessCRS>
					</GlobAccessCRSAry>
				</GlobalAccessInfo>
			</xsl:if>
		</EndTransactionMods>
		<xsl:if test="TPA_Extensions/PriceData">
			<xsl:call-template name="FQDisplay" />
		</xsl:if>
	</xsl:template>

	<xsl:template name="FQDisplay">
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
		<FareRedisplayMods>
			<DisplayAction>
				<Action>D</Action>
			</DisplayAction>
			<FareNumInfo>
				<FareNumAry>
					<FareNum>2</FareNum>
				</FareNumAry>
			</FareNumInfo>
		</FareRedisplayMods>
		<FareRedisplayMods>
			<DisplayAction>
				<Action>D</Action>
			</DisplayAction>
			<FareNumInfo>
				<FareNumAry>
					<FareNum>3</FareNum>
				</FareNumAry>
			</FareNumInfo>
		</FareRedisplayMods>
		<FareRedisplayMods>
			<DisplayAction>
				<Action>D</Action>
			</DisplayAction>
			<FareNumInfo>
				<FareNumAry>
					<FareNum>4</FareNum>
				</FareNumAry>
			</FareNumInfo>
		</FareRedisplayMods>
	</xsl:template>

	<xsl:template match="OriginDestinationOption" mode="Regular">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:variable name="numfl">
			<xsl:value-of select="count(FlightSegment)"/>
		</xsl:variable>
		<xsl:apply-templates select="FlightSegment" mode="Regular">
			<xsl:with-param name="pos">
				<xsl:value-of select="$pos"/>
			</xsl:with-param>
			<xsl:with-param name="numfl">
				<xsl:value-of select="$numfl"/>
			</xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>

	<xsl:template match="FlightSegment" mode="Regular">
		<xsl:param name="pos"/>
		<xsl:param name="numfl"/>
		<AirSegSell>
			<Vnd>
				<xsl:value-of select="MarketingAirline/@Code" />
				<xsl:text><![CDATA[ ]]></xsl:text>
			</Vnd>
			<FltNum>
				<xsl:value-of select="@FlightNumber" />
			</FltNum>
			<OpSuf><![CDATA[ ]]></OpSuf>
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
				<xsl:choose>
					<xsl:when test="@ActionCode = 'Waitlist'">LL</xsl:when>
					<xsl:when test="@ActionCode = 'Other'">BK</xsl:when>
					<xsl:otherwise>NN</xsl:otherwise>
				</xsl:choose>
			</Status>
			<NumPsgrs>
				<xsl:value-of select="@NumberInParty" />
			</NumPsgrs>
			<StartTm>
				<xsl:value-of select="translate(substring(@DepartureDateTime,12,5),':','')"></xsl:value-of>
			</StartTm>
			<EndTm>
				<xsl:value-of select="translate(substring(@ArrivalDateTime,12,5),':','')"></xsl:value-of>
			</EndTm>
			<DtChg>00</DtChg>
			<StopoverIgnoreInd />
			<AvailDispType>G</AvailDispType>
			<VSpec />
			<AvailJrnyNum>
				<xsl:if test="$numfl > 1">
					<xsl:value-of select="format-number($pos,'00')"/>
				</xsl:if>
			</AvailJrnyNum>
		</AirSegSell>
	</xsl:template>

	<xsl:template match="FlightSegment" mode="Passive">
		<!--******************************************************************************************************-->
		<!--							Passive Segments								          -->
		<!--********************************************************************************************** *******-->
		<AirSegmentSell_6_0>
			<InsertSegAfterModes>
				<SegNum>
					<xsl:value-of select="position() - 1"/>
				</SegNum>
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
						<xsl:value-of select="ActionCode" />
					</Status>
					<NumPsgrs>
						<xsl:value-of select="NumberInParty" />
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
				<!--xsl:if test="../../../../../../OTA_TravelItineraryRQ/POS/Source/RequestorID/@ID">
					<OptFldID>
						<ID>BS</ID>
						<Contents>
							<xsl:value-of select="../../../../../../OTA_TravelItineraryRQ/POS/Source/RequestorID/@ID" />
						</Contents>
					</OptFldID>
				</xsl:if-->
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
		<xsl:choose>
			<xsl:when test="VehResRQCore/@Status='OnRequest'">
				<PassiveSegmentSellMods>
					<PassiveSegmentSellRequest>
						<PassiveSegType>CAR</PassiveSegType>
						<Vnd>
							<xsl:value-of select="VehResRQCore/VendorPref/@Code" />
						</Vnd>
						<Status>BK</Status>
						<NumItems>01</NumItems>
						<City>
							<xsl:value-of select="VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode"/>
						</City>
						<StartDt>
							<xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@PickUpDateTime),'-',''),1,8)" />
						</StartDt>
						<EndDt>
							<xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@ReturnDateTime),'-',''),1,8)" />
						</EndDt>
						<Type>
							<xsl:value-of select="VehResRQCore/VehPref/VehType/@VehicleCategory" />
						</Type>
						<DuePaidTextInd></DuePaidTextInd>
						<AmtDuePaid>
							<xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/CarRate/@Rate"/>.00
						</AmtDuePaid>
					</PassiveSegmentSellRequest>
				</PassiveSegmentSellMods>
			</xsl:when>
			<xsl:otherwise>
				<CarSegSellMods>
					<CarV>
						<xsl:value-of select="VehResRQCore/VendorPref/@Code" />
					</CarV>
					<CarType>
						<xsl:value-of select="VehResRQCore/VehPref/VehType/@VehicleCategory" />
					</CarType>
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
					<xsl:if test="VehResRQCore/VehRentalCore/ReturnLocation/@LocationCode != VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode">
						<DropLocn>
							<xsl:value-of select="VehResRQCore/VehRentalCore/ReturnLocation/@LocationCode" />
						</DropLocn>
					</xsl:if>
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
					<RateSource>
						<xsl:value-of select="VehResRQCore/VendorPref/@CodeContext" />
					</RateSource>
					<NumCars>
						<xsl:choose>
							<xsl:when test="VehResRQCore/TPA_Extensions/CarData/@NumCars != ''">
								<xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/@NumCars" />
							</xsl:when>
							<xsl:otherwise>1</xsl:otherwise>
						</xsl:choose>
					</NumCars>
					<RetRuleTxtInd>Y</RetRuleTxtInd>
					<!--*********************************************************************************-->
					<!--	**			Process Optional Fields                 	                        **-->
					<!--*********************************************************************************-->
					<xsl:if test="VehResRQCore/RateQualifier/@CorpDiscountNmbr or VehResRQInfo/RentalPaymentPref/PaymentCard or VehResRQInfo/		@SmokingAllowed='0' or ../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/Address or 		../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty">
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
			</xsl:otherwise>
		</xsl:choose>
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
	<!--******************************************************************************************************-->
	<!--							Seat Requests                 							    -->
	<!--******************************************************************************************************-->
	<xsl:template match="SeatRequests">
		<xsl:variable name="segs">
			<xsl:variable name="fseg">
				<xsl:value-of select="SeatRequest[1]/@FlightRefNumberRPHList"/>
			</xsl:variable>
			<xsl:apply-templates select="SeatRequest[1]">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="$fseg"/>
				</xsl:with-param>
				<xsl:with-param name="AllSegs"></xsl:with-param>
			</xsl:apply-templates>
		</xsl:variable>
		<xsl:call-template name="SeatPerSeg">
			<xsl:with-param name="SRPH">
				<xsl:value-of select="$segs"/>
			</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template match="SeatRequest">
		<xsl:param name="SRPH"/>
		<xsl:param name="AllSegs"/>
		<xsl:variable name="tallsegs">
			<xsl:call-template name="segs">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="$SRPH"/>
				</xsl:with-param>
				<xsl:with-param name="AllSegs">
					<xsl:value-of select="$AllSegs"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="fseg">
			<xsl:value-of select="following-sibling::SeatRequest[1]/@FlightRefNumberRPHList"/>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$fseg != ''">
				<xsl:apply-templates select="following-sibling::SeatRequest[1]">
					<xsl:with-param name="SRPH">
						<xsl:value-of select="$fseg"/>
					</xsl:with-param>
					<xsl:with-param name="AllSegs">
						<xsl:value-of select="concat($AllSegs,$tallsegs)"/>
					</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat($AllSegs,$tallsegs)"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="segs">
		<xsl:param name="SRPH"/>
		<xsl:param name="AllSegs"/>
		<xsl:if test="string-length($SRPH) != 0">
			<xsl:variable name="sRPH">
				<xsl:value-of select="substring($SRPH,1,1)"/>
			</xsl:variable>
			<xsl:variable name="tallsegs">
				<xsl:choose>
					<xsl:when test="contains($AllSegs,$sRPH)"></xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$sRPH"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:value-of select="$tallsegs"/>
			<xsl:call-template name="segs">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="substring($SRPH,2)"/>
				</xsl:with-param>
				<xsl:with-param name="AllSegs">
					<xsl:value-of select="$AllSegs"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="SeatPerSeg">
		<xsl:param name="SRPH"/>
		<xsl:if test="string-length($SRPH) != 0">
			<xsl:variable name="sRPH">
				<xsl:value-of select="substring($SRPH,1,1)"/>
			</xsl:variable>
			<ReqInfo>
				<ID>R</ID>
				<Num>
					<xsl:value-of select="position()" />
				</Num>
				<AirV>
					<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/MarketingAirline/	@Code" />
					<xsl:text><![CDATA[ ]]></xsl:text>
				</AirV>
				<FltNum>
					<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/@FlightNumber" />
				</FltNum>
				<OpSuf />
				<Dt>
					<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/	@DepartureDateTime,1,10),'-','')" />
				</Dt>
				<StartAirp>
					<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/DepartureAirport/	@LocationCode" />
					<xsl:text><![CDATA[  ]]></xsl:text>
				</StartAirp>
				<EndAirp>
					<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/ArrivalAirport/	@LocationCode" />
					<xsl:text><![CDATA[  ]]></xsl:text>
				</EndAirp>
				<BIC>
					<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/	@ResBookDesigCode" />
					<xsl:text><![CDATA[ ]]></xsl:text>
				</BIC>
				<xsl:if test="SeatRequest[contains(@FlightRefNumberRPHList,$sRPH)]/@SeatNumber !='' ">
					<ReqType>S</ReqType>
					<SeatQual>
						<SeatAry>
							<xsl:for-each select="SeatRequest[contains(@FlightRefNumberRPHList,$sRPH)][@SeatNumber != '']">
								<Seat>
									<xsl:variable name="seat">
										<xsl:if test="string-length(@SeatNumber) = 2">
											<xsl:text>0</xsl:text>
										</xsl:if>
										<xsl:value-of select="@SeatNumber"/>
									</xsl:variable>
									<xsl:text disable-output-escaping="yes">&lt;![CDATA[  </xsl:text>
									<xsl:value-of disable-output-escaping="yes" select="$seat" />
									<xsl:text disable-output-escaping="yes">]]&gt;</xsl:text>
								</Seat>
							</xsl:for-each>
						</SeatAry>
					</SeatQual>
				</xsl:if>
				<xsl:if test="SeatRequest[contains(@FlightRefNumberRPHList,$sRPH)]/@SeatPreference">
					<ReqType>G</ReqType>
					<AttribQual>
						<AttribAry>
							<xsl:for-each select="SeatRequest[contains(@FlightRefNumberRPHList,$sRPH)][@SeatPreference]">
								<xsl:variable name="TRPH">
									<xsl:value-of select="@TravelerRefNumberRPHList"/>
								</xsl:variable>
								<xsl:call-template name="SeatPerPax">
									<xsl:with-param name="TRPH">
										<xsl:value-of select="$TRPH"/>
									</xsl:with-param>
								</xsl:call-template>
							</xsl:for-each>
						</AttribAry>
					</AttribQual>
				</xsl:if>
			</ReqInfo>
			<xsl:call-template name="SeatPerSeg">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="substring($SRPH,2)"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="SeatPerPax">
		<xsl:param name="TRPH"/>
		<xsl:if test="string-length($TRPH) != 0">
			<Attrib>
				<xsl:value-of select="@SeatPreference" />
			</Attrib>
			<xsl:call-template name="SeatPerPax">
				<xsl:with-param name="TRPH">
					<xsl:value-of select="substring($TRPH,2)"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--			Process Names,Telephone, Customer Loyalty, ticketing	    	                -->
	<!-- ********************************************************************************************-->
	<xsl:template match="PNRData">
		<PNRBFPrimaryBldChgMods>
			<ItemAry>
				<xsl:apply-templates select="Traveler[PersonName]" mode="names" />
				<xsl:apply-templates select="Traveler" mode="ff" />
				<xsl:apply-templates select="Telephone" />
				<xsl:choose>
					<xsl:when test="Ticketing">
						<xsl:apply-templates select="Ticketing" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="../../OTA_AirBookRQ/Ticketing" />
					</xsl:otherwise>
				</xsl:choose>
				<Item>
					<DataBlkInd>E</DataBlkInd>
					<EndMarkQual>
						<EndMark>E</EndMark>
					</EndMarkQual>
				</Item>
			</ItemAry>
		</PNRBFPrimaryBldChgMods>
	</xsl:template>
	<!-- -->
	<xsl:template match="OTA_UpdateRQ/Position/Element[@Operation='insert' and @Child='PNRData'][PNRData/Telephone]" mode="telephone">
		<PNRBFPrimaryBldChgMods>
			<ItemAry>
				<xsl:apply-templates select="PNRData/Telephone" />
				<Item>
					<DataBlkInd>E</DataBlkInd>
					<EndMarkQual>
						<EndMark>E</EndMark>
					</EndMarkQual>
				</Item>
			</ItemAry>
		</PNRBFPrimaryBldChgMods>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--							Process special requests   	               			     -->
	<!-- ********************************************************************************************-->
	<xsl:template match="Position/Element[@Operation='insert' and @Child='Remarks'] | Position/Element[@Operation='insert' and @Child='SpecialRemarks'] | Position/Element[@Operation='insert' and @Child='SpecialServiceRequests']">
		<!-- OSI Qual  -->
		<xsl:apply-templates select="OtherServiceInformations/OtherServiceInformation" mode="osi"/>
		<!-- SSR Qual -->
		<xsl:apply-templates select="SpecialServiceRequests/SpecialServiceRequest" />
		<!-- GenRmkQual -->
		<xsl:apply-templates select="Remarks/Remark" />
		<!-- AssocRmkQual  -->
		<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Air']" mode="Air" />

		<xsl:choose>
			<xsl:when test="//POS/TPA_Extensions/Provider/Name='Galileo'">
				<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Invoice']" mode="Galileo" />
				<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Historical']" mode="GalileoH" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Invoice']" mode="Apollo" />
			</xsl:otherwise>
		</xsl:choose>
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
					<xsl:text>TAU/</xsl:text>
					<xsl:value-of select="substring(@TicketTimeLimit,9,2)" />
					<xsl:call-template name="month">
						<xsl:with-param name="month">
							<xsl:value-of select="substring(@TicketTimeLimit,6,2)" />
						</xsl:with-param>
					</xsl:call-template>
					<xsl:text>/</xsl:text>
					<xsl:value-of select="substring(@TicketTimeLimit,12,2)" />
					<xsl:value-of select="substring(@TicketTimeLimit,15,2)" />
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
						<xsl:value-of select="translate(PersonName/Surname,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
					</LName>
					<xsl:if test="@PassengerTypeCode='INF'">
						<NameType>I</NameType>
					</xsl:if>
					<LNameRmk>
						<xsl:if test="../../../POS/TPA_Extensions/Provider/Name = 'Apollo'">
							<xsl:choose>
								<xsl:when test="@PassengerTypeCode='ADT'">ADT</xsl:when>
								<xsl:when test="@PassengerTypeCode='CHD' and @BirthDate != ''">
									<xsl:value-of select="substring(@BirthDate,9,2)" />
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring(@BirthDate,6,2)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:value-of select="substring(@BirthDate,3,2)" />
								</xsl:when>
								<xsl:when test="@PassengerTypeCode='SRC' and Age != ''">
									SC<xsl:value-of select="Age" />
								</xsl:when>
								<xsl:when test="@PassengerTypeCode='YTH' and Age != ''">
									YC<xsl:value-of select="Age" />
								</xsl:when>
								<xsl:when test="@PassengerTypeCode='INF' and @BirthDate != ''">
									<xsl:value-of select="substring(@BirthDate,9,2)" />
									<xsl:call-template name="month">
										<xsl:with-param name="month">
											<xsl:value-of select="substring(@BirthDate,6,2)" />
										</xsl:with-param>
									</xsl:call-template>
									<xsl:value-of select="substring(@BirthDate,3,2)" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="@PassengerTypeCode" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</LNameRmk>
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
									<xsl:value-of select="translate(PersonName/GivenName,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
									<xsl:if test="PersonName/MiddleName != ''">
										<xsl:value-of select="string(' ')" />
										<xsl:value-of select="translate(PersonName/MiddleName,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
									</xsl:if>
									<xsl:if test="PersonName/NameTitle != ''">
										<xsl:value-of select="string(' ')" />
										<xsl:value-of select="translate(PersonName/NameTitle,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
									</xsl:if>
									<!--xsl:if test="../../../POS/TPA_Extensions/Provider/Name = 'Galileo'">
										<xsl:choose>
											<xsl:when test="@PassengerTypeCode='CHD'">
												<xsl:text>CH</xsl:text>
												<xsl:call-template name="bd">
													<xsl:with-param name="bd">
														<xsl:value-of select="@BirthDate" />
													</xsl:with-param>
												</xsl:call-template>
											</xsl:when>
											<xsl:when test="@PassengerTypeCode='INF'">
												<xsl:text>*</xsl:text>
												<xsl:call-template name="bd">
													<xsl:with-param name="bd">
														<xsl:value-of select="@BirthDate" />
													</xsl:with-param>
												</xsl:call-template>
											</xsl:when>
										</xsl:choose>
									</xsl:if-->
								</FName>
							</FNameItem>
						</FNameAry>
					</NameTypeQual>
				</AddChgNameRmkQual>
			</NameQual>
		</Item>
	</xsl:template>

	<xsl:template name="bd">
		<xsl:param name="bd" />
		<xsl:value-of select="substring($bd,9,2)"/>
		<xsl:choose>
			<xsl:when test="substring($bd,6,2) = '01'">JAN</xsl:when>
			<xsl:when test="substring($bd,6,2) = '02'">FEB</xsl:when>
			<xsl:when test="substring($bd,6,2) = '03'">MAR</xsl:when>
			<xsl:when test="substring($bd,6,2) = '04'">APR</xsl:when>
			<xsl:when test="substring($bd,6,2) = '05'">MAY</xsl:when>
			<xsl:when test="substring($bd,6,2) = '06'">JUN</xsl:when>
			<xsl:when test="substring($bd,6,2) = '07'">JUL</xsl:when>
			<xsl:when test="substring($bd,6,2) = '08'">AUG</xsl:when>
			<xsl:when test="substring($bd,6,2) = '09'">SEP</xsl:when>
			<xsl:when test="substring($bd,6,2) = '10'">OCT</xsl:when>
			<xsl:when test="substring($bd,6,2) = '11'">NOV</xsl:when>
			<xsl:when test="substring($bd,6,2) = '12'">DEC</xsl:when>
		</xsl:choose>
		<xsl:value-of select="substring($bd,3,2)"/>
	</xsl:template>

	<xsl:template match="Traveler" mode="ff">
		<xsl:variable name="pos">
			<xsl:value-of select="position()"/>
		</xsl:variable>
		<xsl:if test="CustLoyalty/@MembershipID != ''">
			<xsl:apply-templates select="CustLoyalty">
				<xsl:with-param name="pos">
					<xsl:value-of select="$pos"/>
				</xsl:with-param>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>

	<!-- ********************************************************************************************-->
	<!--						Frequent Customer Number			                            -->
	<!-- ********************************************************************************************-->
	<xsl:template match="CustLoyalty">
		<xsl:param name="pos"/>
		<Item>
			<DataBlkInd>
				<xsl:text><![CDATA[M ]]></xsl:text>
			</DataBlkInd>
			<FreqCustQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<LNameID>
						<xsl:value-of select="$pos" />
					</LNameID>
					<PsgrNum>1</PsgrNum>
					<AbsNameNum>
						<xsl:value-of select="$pos" />
					</AbsNameNum>
					<xsl:variable name="CLCode">
						<xsl:value-of select="@ProgramID" />
					</xsl:variable>
					<FreqCustNum>
						<xsl:value-of select="$CLCode" />
						<xsl:if test="string-length($CLCode) = '2'">
							<xsl:value-of select="string(' ')" />
						</xsl:if>
						<xsl:value-of select="string(' ')" />
						<xsl:value-of select="@MembershipID" />
					</FreqCustNum>
				</AddQual>
			</FreqCustQual>
		</Item>
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
					<xsl:choose>
						<xsl:when test="@AreaCityCode!=''">
							<City>
								<xsl:value-of select="@AreaCityCode" />
								<xsl:text><![CDATA[  ]]></xsl:text>
							</City>
						</xsl:when>
						<xsl:otherwise>
							<City>UNK </City>
						</xsl:otherwise>
					</xsl:choose>
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
						<xsl:if test="@CountryAccessCode != ''">
							<xsl:value-of select="@CountryAccessCode"/>
							<xsl:text>-</xsl:text>
						</xsl:if>
						<xsl:value-of select="translate(@PhoneNumber,'+','')" />
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
			<DataBlkInd><![CDATA[R ]]></DataBlkInd>
			<EmailQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Type>T</Type>
					<EmailData>
						<xsl:value-of select="."/>
					</EmailData>
				</AddQual>
			</EmailQual>
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
									<xsl:choose>
										<xsl:when test="PaymentCard/@CardCode = 'MC'">CA</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="PaymentCard/@CardCode" />
										</xsl:otherwise>
									</xsl:choose>
								</CC>
								<ExpDt>
									<xsl:value-of select="PaymentCard/@ExpireDate" />
								</ExpDt>
								<Acct>
									<xsl:value-of select="PaymentCard/@CardNumber" />
									<xsl:if test="PaymentCard/@SeriesCode != ''">
										<xsl:text>/I</xsl:text>
										<xsl:value-of select="PaymentCard/@SeriesCode" />
									</xsl:if>
								</Acct>
							</CCQual>
						</xsl:when>
						<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
							<TypeInd>1</TypeInd>
							<VarLenQual>
								<FOP>S</FOP>
							</VarLenQual>
						</xsl:when>
						<xsl:when test="DirectBill/@DirectBill_ID='Check'">
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
						<xsl:choose>
							<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'">
								<xsl:text><![CDATA[ Z/]]></xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text><![CDATA[ P/]]></xsl:text>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="PostalCode" />
						<xsl:if test="CountryName/@Code != '' or CountryName !=''">
							<xsl:text>@</xsl:text>
							<xsl:choose>
								<xsl:when test="CountryName != ''">
									<xsl:value-of select="CountryName" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CountryName/@Code" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
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
						<xsl:value-of select="substring(../../../TPA_Extensions/PNRData/Traveler[1]/PersonName/GivenName,1,1)"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="../../../TPA_Extensions/PNRData/Traveler[1]/PersonName/Surname"/>
						<xsl:text>@</xsl:text>
						<xsl:value-of select="substring(StreetNmbr,1,36)" />
						<xsl:text>@</xsl:text>
						<xsl:value-of select="CityName" />
						<xsl:text>@</xsl:text>
						<xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
							<xsl:choose>
								<xsl:when test="StateProv/@StateCode != ''">
									<xsl:value-of select="StateProv/@StateCode" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="StateProv" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'">
								<xsl:text><![CDATA[ Z/]]></xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text><![CDATA[ P/]]></xsl:text>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="PostalCode" />
						<xsl:if test="CountryName/@Code != '' or CountryName !=''">
							<xsl:text>@</xsl:text>
							<xsl:choose>
								<xsl:when test="CountryName != ''">
									<xsl:value-of select="CountryName" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CountryName/@Code" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
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
			<xsl:choose>
				<xsl:when test="FlightRefNumber/@RPH!=''">
					<xsl:variable name="SRPH" select="FlightRefNumber/@RPH" />
					<DataBlkInd><![CDATA[H ]]></DataBlkInd>
					<AssocRmkQual>
						<EditTypeInd>A</EditTypeInd>
						<AirQual>
							<StartDt>
								<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/					@DepartureDateTime,1,10),'-','')" />
							</StartDt>
							<AirV>
								<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/		@Code" />
								<xsl:text><![CDATA[ ]]></xsl:text>
							</AirV>
							<StartAirp>
								<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/DepartureAirport/		@LocationCode" />
								<xsl:text><![CDATA[  ]]></xsl:text>
							</StartAirp>
							<EndAirp>
								<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/ArrivalAirport/		@LocationCode" />
								<xsl:text><![CDATA[  ]]></xsl:text>
							</EndAirp>
							<FltNum>
								<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@FlightNumber" />
							</FltNum>
							<OpSuf><![CDATA[ ]]></OpSuf>
							<BIC>
								<xsl:value-of select="//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/		@ResBookDesigCode" />
								<xsl:text><![CDATA[ ]]></xsl:text>
							</BIC>
						</AirQual>
						<AssocRmkTypeQual>
							<EditTypeInd>A</EditTypeInd>
							<AddQual>
								<RmkAry>
									<RmkInfo>
										<Rmk>
											<xsl:value-of select="translate(.,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
										</Rmk>
									</RmkInfo>
								</RmkAry>
							</AddQual>
						</AssocRmkTypeQual>
					</AssocRmkQual>
				</xsl:when>
				<xsl:otherwise>
					<DataBlkInd><![CDATA[I ]]></DataBlkInd>
					<NonAssocRmkInd>
						<EditTypeInd>A</EditTypeInd>
						<AddQual>
							<Rmk>
								<xsl:value-of select="Text"/>
							</Rmk>
						</AddQual>
					</NonAssocRmkInd>
				</xsl:otherwise>
			</xsl:choose>
		</Item>
	</xsl:template>

	<xsl:template match="DeliveryAddress">
		<Item>
			<DataBlkInd><![CDATA[D ]]></DataBlkInd>
			<DeliveryAddrQual>
				<EditTypeInd>A</EditTypeInd>
				<AddChgQual>
					<DeliveryAddr>
						<xsl:value-of select="substring(../../../TPA_Extensions/PNRData/Traveler[1]/PersonName/GivenName,1,1)"/>
						<xsl:text> </xsl:text>
						<xsl:value-of select="../../../TPA_Extensions/PNRData/Traveler[1]/PersonName/Surname"/>
						<xsl:text>@</xsl:text>
						<xsl:value-of select="substring(StreetNmbr,1,36)" />
						<xsl:text>@</xsl:text>
						<xsl:value-of select="CityName" />
						<xsl:text>@</xsl:text>
						<xsl:if test="StateProv != '' or StateProv/@StateCode != ''">
							<xsl:choose>
								<xsl:when test="StateProv/@StateCode != ''">
									<xsl:value-of select="StateProv/@StateCode" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="StateProv" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
						<xsl:choose>
							<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'">
								<xsl:text><![CDATA[ Z/]]></xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text><![CDATA[ P/]]></xsl:text>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:value-of select="PostalCode" />
						<xsl:if test="CountryName/@Code != '' or CountryName !=''">
							<xsl:text>@</xsl:text>
							<xsl:choose>
								<xsl:when test="CountryName != ''">
									<xsl:value-of select="CountryName" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CountryName/@Code" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</DeliveryAddr>
				</AddChgQual>
			</DeliveryAddrQual>
		</Item>
	</xsl:template>

	<!-- ********************************************************************************************-->
	<!--							Ticketing Remarks                                                     -->
	<!-- ********************************************************************************************-->
	<xsl:template match="AccountingLine">
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
		<xsl:variable name="GDS">
			<xsl:choose>
				<xsl:when test="../../../../@Target='GAL'">Galileo</xsl:when>
				<xsl:otherwise>Apollo</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="remType">
			<xsl:choose>
				<xsl:when test="@RemarkType='Historical'">H</xsl:when>
				<xsl:when test="@RemarkType='Confidential'">C</xsl:when>
				<xsl:otherwise>F</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<Item>
			<DataBlkInd>G</DataBlkInd>
			<GenRmkQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<xsl:if test="$GDS = 'Galileo'">
						<Qual1>
							<xsl:value-of select="$remType"/>
						</Qual1>
						<!-- <xsl:value-of select="substring(.,1,1)"/> -->
						<Qual2>*</Qual2>
					</xsl:if>
					<Rmk>
						<xsl:value-of select="translate(.,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
					</Rmk>
				</AddQual>
			</GenRmkQual>
		</Item>
	</xsl:template>
	<!-- ********************************************************************************************-->
	<!--				Other Service Information (OSIs)                                                  -->
	<!-- ********************************************************************************************-->
	<xsl:template match="OtherServiceInformation" mode="osi">
		<Item>
			<xsl:variable name="SRPH" select="FlightRefNumber/@RPH" />
			<DataBlkInd><![CDATA[O ]]></DataBlkInd>
			<OSIQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<OSIV>
						<xsl:choose>
							<xsl:when test="Airline/@Code != ''">
								<xsl:value-of select="Airline/@Code"/>
							</xsl:when>
							<xsl:when test="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code!=''">
								<xsl:value-of select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/MarketingAirline/@Code" />
								<xsl:text><![CDATA[ ]]></xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>YY </xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</OSIV>
					<OSI>
						<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
					</OSI>
				</AddQual>
			</OSIQual>
		</Item>
	</xsl:template>

	<xsl:template match="Traveler" mode="osi">
		<Item>
			<DataBlkInd><![CDATA[O ]]></DataBlkInd>
			<OSIQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<OSIV>
						<!--xsl:value-of select="../../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions[1]/OriginDestinationOption[1]/FlightSegment[1]/MarketingAirline/@Code" />
						<xsl:text><![CDATA[ ]]></xsl:text-->
						<xsl:text>YY</xsl:text>
					</OSIV>
					<OSI>
						<xsl:text>1CHD </xsl:text>
						<xsl:value-of select="translate(PersonName/Surname,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
						<xsl:text>/</xsl:text>
						<xsl:value-of select="translate(PersonName/GivenName,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
						<xsl:text> DOB </xsl:text>
						<xsl:call-template name="bd">
							<xsl:with-param name="bd">
								<xsl:value-of select="@BirthDate" />
							</xsl:with-param>
						</xsl:call-template>
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
		<xsl:variable name="TRPH" select="@TravelerRefNumberRPHList" />
		<xsl:choose>
			<xsl:when test="not(@FlightRefNumberRPHList) and not(@TravelerRefNumberRPHList)">
				<Item>
					<DataBlkInd><![CDATA[S ]]></DataBlkInd>
					<SSRQual>
						<EditTypeInd>A</EditTypeInd>
						<AddQual>
							<SSRCode>
								<xsl:value-of select="@SSRCode" />
							</SSRCode>
							<FltNum></FltNum>
							<OpSuf><![CDATA[ ]]></OpSuf>
							<AirV>
								<xsl:value-of select="Airline/@Code" />
								<xsl:text><![CDATA[ ]]></xsl:text>
							</AirV>
							<Dt></Dt>
							<BIC></BIC>
							<StartAirp></StartAirp>
							<EndAirp></EndAirp>
							<xsl:if test="Text != ''">
								<Text>
									<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
								</Text>
							</xsl:if>
						</AddQual>
					</SSRQual>
				</Item>
			</xsl:when>
			<xsl:when test="not(@TravelerRefNumberRPHList)">
				<xsl:call-template name="SSRPerSeg">
					<xsl:with-param name="SRPH">
						<xsl:value-of select="$SRPH"/>
					</xsl:with-param>
					<xsl:with-param name="TRPH">0</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:when test="not(@FlightRefNumberRPHList)">
				<xsl:call-template name="SSRPerSeg">
					<xsl:with-param name="SRPH">0</xsl:with-param>
					<xsl:with-param name="TRPH">
						<xsl:value-of select="$TRPH"/>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="SSRPerSeg">
					<xsl:with-param name="SRPH">
						<xsl:value-of select="$SRPH"/>
					</xsl:with-param>
					<xsl:with-param name="TRPH">
						<xsl:value-of select="$TRPH"/>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="SSRPerSeg">
		<xsl:param name="SRPH"/>
		<xsl:param name="TRPH"/>
		<xsl:if test="$SRPH != ''">
			<xsl:call-template name="SSRPerPax">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="$SRPH"/>
				</xsl:with-param>
				<xsl:with-param name="TRPH">
					<xsl:value-of select="$TRPH"/>
				</xsl:with-param>
			</xsl:call-template>
			<xsl:call-template name="SSRPerSeg">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="substring($SRPH,2)"/>
				</xsl:with-param>
				<xsl:with-param name="TRPH">
					<xsl:value-of select="$TRPH"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template name="SSRPerPax">
		<xsl:param name="SRPH"/>
		<xsl:param name="TRPH"/>
		<xsl:if test="string-length($TRPH) != 0">
			<xsl:variable name="sRPH">
				<xsl:value-of select="substring($SRPH,1,1)"/>
			</xsl:variable>
			<xsl:variable name="tRPH">
				<xsl:value-of select="substring($TRPH,1,1)"/>
			</xsl:variable>
			<Item>
				<DataBlkInd><![CDATA[S ]]></DataBlkInd>
				<SSRQual>
					<EditTypeInd>A</EditTypeInd>
					<AddQual>
						<xsl:if test="$tRPH != '0'">
							<LNameNum>
								0<xsl:value-of select="$tRPH" />
							</LNameNum>
							<PsgrNum>01</PsgrNum>
							<AbsNameNum>
								0<xsl:value-of select="$tRPH" />
							</AbsNameNum>
						</xsl:if>
						<SSRCode>
							<xsl:value-of select="@SSRCode" />
						</SSRCode>
						<xsl:choose>
							<xsl:when test="$sRPH != '0'">
								<FltNum>
									<xsl:value-of select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/@FlightNumber" />
								</FltNum>
								<OpSuf><![CDATA[ ]]></OpSuf>
								<AirV>
									<xsl:value-of select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/MarketingAirline/@Code" />
									<xsl:text><![CDATA[ ]]></xsl:text>
								</AirV>
								<Dt>
									<xsl:value-of select="translate(substring(../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/@DepartureDateTime,1,10),'-','')" />
								</Dt>
								<BIC>
									<xsl:value-of select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/@ResBookDesigCode" />
									<xsl:text><![CDATA[ ]]></xsl:text>
								</BIC>
								<StartAirp>
									<xsl:value-of select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/DepartureAirport/@LocationCode" />
									<xsl:text><![CDATA[  ]]></xsl:text>
								</StartAirp>
								<EndAirp>
									<xsl:value-of select="../../../../AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/ArrivalAirport/@LocationCode" />
									<xsl:text><![CDATA[  ]]></xsl:text>
								</EndAirp>
							</xsl:when>
							<xsl:otherwise>
								<FltNum></FltNum>
								<OpSuf><![CDATA[ ]]></OpSuf>
								<AirV>
									<xsl:value-of select="Airline/@Code" />
									<xsl:text><![CDATA[ ]]></xsl:text>
								</AirV>
								<Dt></Dt>
								<BIC></BIC>
								<StartAirp></StartAirp>
								<EndAirp></EndAirp>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="Text != ''">
							<Text>
								<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
							</Text>
						</xsl:if>
					</AddQual>
				</SSRQual>
			</Item>
			<xsl:call-template name="SSRPerPax">
				<xsl:with-param name="SRPH">
					<xsl:value-of select="$SRPH"/>
				</xsl:with-param>
				<xsl:with-param name="TRPH">
					<xsl:value-of select="substring($TRPH,2)"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<!-- 
  ********************************************************************************************
  						Special Remarks    (Invoice)                                                    
  ********************************************************************************************
  -->
	<xsl:template match="SpecialRemark" mode="Galileo">
		<Item>
			<DataBlkInd>X</DataBlkInd>
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
						<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
					</Text>
				</AddQual>
			</DOCInvoiceQual>
		</Item>
	</xsl:template>

	<xsl:template match="SpecialRemark" mode="GalileoH">
		<Item>
			<DataBlkInd>G</DataBlkInd>
			<GenRmkQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Qual1>H</Qual1>
					<Rmk>
						<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
					</Rmk>
				</AddQual>
			</GenRmkQual>
		</Item>
	</xsl:template>

	<!-- ********************************************************************************************-->
	<!--						service fee (accounting line)                                          -->
	<!-- ********************************************************************************************-->
	<xsl:template match="ServiceFee">
		<Item>
			<DataBlkInd><![CDATA[X ]]></DataBlkInd>
			<DOCInvoiceQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					<Keyword>FT</Keyword>
					<Text>
						<xsl:text>SF-</xsl:text>
						<xsl:value-of select="@Amount" />
						<xsl:choose>
							<xsl:when test="../../../OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/DirectBill/@DirectBill_ID='Cash'">-CASH</xsl:when>
							<xsl:otherwise>-CC</xsl:otherwise>
						</xsl:choose>
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
						<!--xsl:value-of select="InvoiceKey" /-->
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
	<xsl:template match="PriceData">
		<StorePriceMods>
			<xsl:choose>
				<!--***********************************************************************-->
				<!--**                   Negotiated fares                                              -->
				<!--***********************************************************************-->
				<xsl:when test="@PriceType='Private'">
					<xsl:apply-templates select="NegoFares/PriceRequestInformation/NegotiatedFareCode" />
				</xsl:when>
				<xsl:otherwise>
					<!--***********************************************************************-->
					<!--**                     Published Fares                                             -->
					<!--***********************************************************************-->
					<!--xsl:apply-templates select="PublishedFares/FareRestrictPref" />
					<SegSelection>
						<ReqAirVPFs>N</ReqAirVPFs>
						<SegRangeAry>
							<SegRange>
								<StartSeg>00</StartSeg>
								<EndSeg>00</EndSeg>
								<FareType>N</FareType>
							</SegRange>
						</SegRangeAry>
					</SegSelection-->
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="../../OTA_AirBookRQ/Ticketing/@TicketType = 'Paper'">
				<DocumentSelect>
					<ETInd>P</ETInd>
					<!--ItinInd>I</ItinInd-->
				</DocumentSelect>
			</xsl:if>
			<!--***********************************************************************-->
			<!--**                      Passenger Info                                              -->
			<!--***********************************************************************-->
			<xsl:if test="../../../POS/TPA_Extensions/Provider/Name = 'Galileo'">
				<PlatingAirVMods>
					<PlatingAirV>
						<xsl:value-of 	select="../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/MarketingAirline/@Code"/>
					</PlatingAirV>
				</PlatingAirVMods>
			</xsl:if>
			<PsgrMods>
				<PsgrAry>
					<xsl:apply-templates select="../PNRData/Traveler" mode="PassengerInfo" />
				</PsgrAry>
			</PsgrMods>
			<!--DocProdFareType>
				<Type>
					<xsl:choose>
						<xsl:when test="@PriceType='Private'">
							<xsl:text>P</xsl:text>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text>N</xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</Type>
			</DocProdFareType-->
			<xsl:if test="../../POS/TPA_Extensions/Provider/Name = 'Galileo'">
				<PlatingAirVMod>
					<AirV>
						<xsl:value-of 	select="../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/MarketingAirline/@Code"/>
					</AirV>
				</PlatingAirVMod>
			</xsl:if>
			<xsl:apply-templates select="../AgencyData/Commission"/>
			<xsl:if test="../../POS/Source/@ISOCurrency != '' or ../../POS/Source/@ISOCountry != ''">
				<GenQuoteInfo>
					<SellCity>
						<xsl:value-of select="../../POS/Source/@ISOCountry"/>
					</SellCity>
					<AltCurrency>
						<xsl:value-of select="../../POS/Source/@ISOCurrency"/>
					</AltCurrency>
				</GenQuoteInfo>
			</xsl:if>
		</StorePriceMods>
	</xsl:template>

	<xsl:template match="Commission">
		<CommissionMod>
			<xsl:choose>
				<xsl:when test="@Percent != ''">
					<Percent>
						<xsl:value-of select="@Percent"/>
					</Percent>
				</xsl:when>
				<xsl:otherwise>
					<Amt>
						<xsl:value-of select="@Amount"/>
					</Amt>
				</xsl:otherwise>
			</xsl:choose>
		</CommissionMod>
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
					<xsl:when test="MinimumStay">Y</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</MinStay>
			<MaxStay>
				<xsl:choose>
					<xsl:when test="MaximumStay">Y</xsl:when>
					<xsl:otherwise>N</xsl:otherwise>
				</xsl:choose>
			</MaxStay>
			<AP>
				<xsl:choose>
					<xsl:when test="AdvResTicketing/AdvReservation">Y</xsl:when>
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
							<!--xsl:value-of select="@Code"/-->
						</Contract>
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
				0<xsl:value-of select="position()" />
			</LNameNum>
			<PsgrNum>01</PsgrNum>
			<AbsNameNum>
				0<xsl:value-of select="position()" />
			</AbsNameNum>
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

	<xsl:template match="UniqueRemarks">
		<PassiveSegmentSellMods>
			<xsl:apply-templates select="UniqueRemark"/>
		</PassiveSegmentSellMods>
	</xsl:template>

	<xsl:template match="UniqueRemark">
		<PassiveSegmentSellRequest>
			<PassiveSegType>
				<xsl:value-of select="@RemarkType"/>
			</PassiveSegType>
			<Vnd>
				<xsl:value-of select="substring(.,1,2)"/>
			</Vnd>
			<Status>
				<xsl:value-of select="substring(.,4,2)"/>
			</Status>
			<NumItems>
				<xsl:value-of select="substring(.,7,2)"/>
			</NumItems>
			<City>
				<xsl:value-of select="substring(.,10,3)"/>
			</City>
			<StartDt>
				<xsl:value-of select="translate(substring(.,14,10),'-','')"/>
			</StartDt>
			<EndDt />
			<Type></Type>
			<DuePaidTextInd></DuePaidTextInd>
			<AmtDuePaid></AmtDuePaid>
		</PassiveSegmentSellRequest>
		<xsl:if test="substring(.,24,1)='/'">
			<PassiveSegmentSellFreeformRequest>
				<PropAddrInd>CF</PropAddrInd>
				<Text>
					<xsl:value-of select="substring(.,25)"/>
				</Text>
			</PassiveSegmentSellFreeformRequest>
		</xsl:if>
	</xsl:template>

	<!-- 
***********************************************************************
**********  String Triming Functions **********************************
***********************************************************************
-->

	<xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

	<!-- Strips trailing whitespace characters from 'string' -->
	<xsl:template name="string-rtrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:variable name="length" select="string-length($string)" />

		<xsl:if test="$length &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, $length, 1))">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading whitespace characters from 'string' -->
	<xsl:template name="string-ltrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:if test="string-length($string) &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, 1, 1))">
					<xsl:call-template name="string-ltrim">
						<xsl:with-param name="string" select="substring($string, 2)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading and trailing whitespace characters from 'string' -->
	<xsl:template name="string-trim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />
		<xsl:call-template name="string-rtrim">
			<xsl:with-param name="string">
				<xsl:call-template name="string-ltrim">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="trim"   select="$trim" />
				</xsl:call-template>
			</xsl:with-param>
			<xsl:with-param name="trim"   select="$trim" />
		</xsl:call-template>
	</xsl:template>

</xsl:stylesheet>



