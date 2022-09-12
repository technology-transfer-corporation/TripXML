<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_TravelBuildRQ.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 13 Dec 2012 - Laleen - Moved the 'AirSegSellMods' tag to the top within 'PNRBFManagement'      -->
<!-- Date: 30 Aug 2012 - Rastko - in SSR FQTV send pax last name / first name 		-->
<!-- Date: 24 Aug 2012 - Rastko - do not take pax name from input in SSR FQTV 		-->
<!-- Date: 23 Aug 2012 - Rastko - added pax name to SSR FQTV request			-->
<!-- Date: 20 Feb 2011 - Rastko - added agency IATA number as guarantee		-->
<!-- Date: 18 Feb 2011 - Rastko - corrected mapping of SSR DOCS				-->
<!-- Date: 18 Oct 2010 - Rastko - added passenger ID in car segment				-->
<!-- Date: 24 Sep 2010 - Rastko - removed mapping of YieldMgmtNum as done in business logic	-->
<!-- Date: 21 Jul 2010 - Rastko - hard coded PCC process in Rules				-->
<!-- Date: 14 Feb 2009 - Rastko - added support for due/paid/text					-->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:variable name="GDS"><xsl:value-of select="OTA_TravelItineraryRQ/POS/TPA_Extensions/Provider/Name"/></xsl:variable>
<xsl:variable name="PCC"><xsl:value-of select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/></xsl:variable>
<xsl:template match="/">
	<TravelBuild>
		<xsl:apply-templates select="OTA_TravelItineraryRQ" mode="start" />
	</TravelBuild>
</xsl:template>

<xsl:template match="OTA_TravelItineraryRQ" mode="start">
	<xsl:if test="OTA_VehResRQ">
		<CarAvail>
			<CarStandardAvail_6_0>
				<CarAvailMods>
					<StartDt>
						<xsl:value-of select="substring(translate(string(OTA_VehResRQ/VehResRQCore/VehRentalCore/@PickUpDateTime),'-',''),1,8)" />
					</StartDt>
					<StartTm>
						<xsl:value-of select="substring(translate(string(OTA_VehResRQ/VehResRQCore/VehRentalCore/@PickUpDateTime),':',''),12,4)" />
					</StartTm>
					<Pt>
						<xsl:value-of select="OTA_VehResRQ/VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode" />
					</Pt>
					<EndDt>
						<xsl:value-of select="substring(translate(string(OTA_VehResRQ/VehResRQCore/VehRentalCore/@ReturnDateTime),'-',''),1,8)" />
					</EndDt>
					<EndTm>
						<xsl:value-of select="substring(translate(string(OTA_VehResRQ/VehResRQCore/VehRentalCore/@ReturnDateTime),':',''),12,4)" />
					</EndTm>
					<CarV1><xsl:value-of select="OTA_VehResRQ/VehResRQCore/VendorPref/@Code" /></CarV1>
					<CarType1>
						<xsl:value-of select="OTA_VehResRQ/VehResRQCore/VehPref/VehType/@VehicleCategory" />
					</CarType1>
					<Currency/>
				</CarAvailMods>
			</CarStandardAvail_6_0>
		</CarAvail>
	</xsl:if>
	<PNRBF>
		<PNRBFManagement_17>
      <!--xsl:apply-templates select="TPA_Extensions/PNRData"/-->
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

      <xsl:apply-templates select="TPA_Extensions/PNRData"/>
      
			<xsl:apply-templates select="OTA_HotelResRQ/HotelReservations/HotelReservation" />
			<xsl:apply-templates select="OTA_VehResRQ" />
			<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/UniqueRemarks[UniqueRemark/@RemarkType!='DUE' and UniqueRemark/@RemarkType!='PAID' and UniqueRemark/@RemarkType!='TEXT']"/>
			<xsl:if test="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails or OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail or OTA_AirBookRQ/Fulfillment/DeliveryAddress or TPA_Extensions/PNRData/AccountingLine or TPA_Extensions/PNRData/Traveler/@PassengerTypeCode = 'CHD' or TPA_Extensions/AgencyData/ServiceFee/@Amount !=''">
				<PNRBFSecondaryBldChgMods>
					<ItemAry>
						<!-- create CHD OSI -->
						<xsl:apply-templates select="TPA_Extensions/PNRData/Traveler[@PassengerTypeCode='CHD']" mode="osi" />
						<!-- SSR, OSI, Remark, Unique Remark, Special Remark -->
						<xsl:apply-templates select="OTA_AirBookRQ/TravelerInfo/SpecialReqDetails"/>
						<!--  FOP Qual       -->
						<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail" />
						<!-- Delivery Address Qual  -->
						<xsl:apply-templates select="OTA_AirBookRQ/Fulfillment/DeliveryAddress"/>
						<!-- Email -->
						<xsl:apply-templates select="TPA_Extensions/PNRData/Email"/>
						<!-- Ticketing remarks -->
						<xsl:apply-templates select="TPA_Extensions/PNRData/AccountingLine"/>
						<!-- accounting remarks -->
						<xsl:apply-templates select="TPA_Extensions/AgencyData/ServiceFee"/>
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
			<!--EndTransactionMods>
				<EndTransactRequest>
					<ETInd>
						<xsl:choose>
							<xsl:when test="TPA_Extensions/PNRData/Queue">Q</xsl:when>
							<xsl:otherwise>R</xsl:otherwise>
						</xsl:choose>
					</ETInd>
					<RcvdFrom>
						<xsl:choose>
							<xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:when>
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
			</xsl:if-->
		</PNRBFManagement_17>
	</PNRBF>
	<ET>
		<PNRBFManagement_17>
			<EndTransactionMods>
				<EndTransactRequest>
					<ETInd>
						<xsl:choose>
							<xsl:when test="TPA_Extensions/PNRData/Queue">Q</xsl:when>
							<xsl:otherwise>E</xsl:otherwise>
						</xsl:choose>
					</ETInd>
					<RcvdFrom>
						<xsl:choose>
							<xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:when>
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
			</xsl:if>
		</PNRBFManagement_17>
	</ET>
	<xsl:if test="TPA_Extensions/AgencyData/EndRules/@RuleType!=''">
		<Rules>
			<PNRBFCustomCheck_1>
				<CustomCheckRuleMods>
					<xsl:for-each select="TPA_Extensions/AgencyData/EndRules">
						<CCRuleData>
							<CRSID>
								<xsl:choose>
									<xsl:when test="$GDS='Galileo'">1G</xsl:when>
									<xsl:otherwise>1V</xsl:otherwise>
								</xsl:choose></CRSID> 
							<ReqType><xsl:value-of select="@RuleType"/></ReqType> 
							<RuleDispMode> </RuleDispMode> 
							<PCC>
								<xsl:choose>
									<xsl:when test="$PCC='M7Q'">
										<xsl:value-of select="'74NA'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$PCC"/>
									</xsl:otherwise>
								</xsl:choose>
							</PCC> 
							<RuleName><xsl:value-of select="@RuleName"/></RuleName> 
							<RuleNum></RuleNum> 
							<PCC2 /> 
							<RuleName2 /> 
							<RuleNum2 /> 
						</CCRuleData>
						<CCRuleText>
							<RuleTxt><xsl:value-of select="."/></RuleTxt>
						</CCRuleText>
					</xsl:for-each>
				</CustomCheckRuleMods>
			</PNRBFCustomCheck_1>
		</Rules>
		<ETR>
			<PNRBFManagement_17>
				<PNRBFSecondaryBldChgMods>
					<ItemAry>
						<Item>
							<DataBlkInd><![CDATA[G ]]></DataBlkInd>
							<GenRmkQual>
								<EditTypeInd>A</EditTypeInd>
								<AddQual>
									<Rmk>DBL ET CCR</Rmk>
								</AddQual>
							</GenRmkQual>
						</Item>
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
								<xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:when>
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
				</xsl:if>
			</PNRBFManagement_17>
		</ETR>
	</xsl:if>
</xsl:template>

<xsl:template match="OriginDestinationOption" mode="Regular">
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:variable name="numfl"><xsl:value-of select="count(FlightSegment)"/></xsl:variable>
	<xsl:apply-templates select="FlightSegment" mode="Regular">
		<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
		<xsl:with-param name="numfl"><xsl:value-of select="$numfl"/></xsl:with-param>
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
		<StartTm><xsl:value-of select="translate(substring(@DepartureDateTime,12,5),':','')"></xsl:value-of></StartTm>
		<EndTm><xsl:value-of select="translate(substring(@ArrivalDateTime,12,5),':','')"></xsl:value-of></EndTm>
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
			<SegNum><xsl:value-of select="position() - 1"/></SegNum>
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
	<HotelSell>
		<SegNum>00</SegNum>
		<SellStatusInd>SS</SellStatusInd>
		<StartDt>
			<xsl:value-of select="substring(translate(string(TimeSpan/@Start),'-',''),1,8)" />
		</StartDt>
		<EndDt>
			<xsl:value-of select="substring(translate(string(TimeSpan/@End),'-',''),1,8)" />
		</EndDt>
		<NumNights>
			<xsl:value-of select="format-number(TimeSpan/@Duration,'00')"/>
		</NumNights>
		<PrimaryCity>
			<xsl:value-of select="BasicPropertyInfo/@HotelCityCode"/>
		</PrimaryCity>
		<Chain>
			<xsl:value-of select="BasicPropertyInfo/@ChainCode" />
		</Chain>
		<RoomMasterID>
			<xsl:value-of select="BasicPropertyInfo/@HotelCode" />
		</RoomMasterID>
		<BIC>
			<xsl:value-of select="RoomRates/RoomRate/@BookingCode" />
		</BIC>
		<NumAdults>
			<xsl:value-of select="GuestCounts/GuestCount/@Count" />
		</NumAdults>
		<NumRooms>
			<xsl:value-of select="RoomRates/RoomRate/@NumberOfUnits" />
		</NumRooms>
		<!--RateAccess1>
			<xsl:text><![CDATA[ ]]></xsl:text>
		</RateAccess1>
		<RateAccess2>
			<xsl:text><![CDATA[ ]]></xsl:text>
		</RateAccess2>
		<RateAccess3>
			<xsl:text><![CDATA[ ]]></xsl:text>
		</RateAccess3-->
		<!--NumExtraAdults>
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
		</NumChildRollaway-->
		<!--SellSource>
			<xsl:value-of select="@SourceOfBusiness" />
		</SellSource>
		<RetRuleTxtInd>
			<xsl:text>Y</xsl:text>
		</RetRuleTxtInd-->
	</HotelSell>
	<!--*********************************************************************************-->
	<!--	**			Process Optional Fields                 	                        **-->
	<!--*********************************************************************************-->
	<xsl:if test="ResGuestRPHs/ResGuestRPH/@RPH!='' or Guarantee or DepositPayments or SpecialRequests or ../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/Address or ../../../../../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty">
		<HotelOptionalData>
			<FldAry>
				<!--*********************************************************************************-->
				<!--	**			Process Guarantee  Information   	                        **-->
				<!--*********************************************************************************-->
				<xsl:choose>
					<xsl:when test="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard!=''">
						<Fld>
							<ID>GT</ID>
							<Contents>
								<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardCode" />
								<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@CardNumber" />
								<xsl:text>EXP</xsl:text>
								<xsl:value-of select="Guarantee/GuaranteesAccepted/GuaranteeAccepted/PaymentCard/@ExpireDate" />
							</Contents>
						</Fld>
					</xsl:when>
					<xsl:when test="Guarantee/GuaranteeDescription/Text">
						<Fld>
							<ID>GT</ID>
							<Contents>
								<xsl:value-of select="Guarantee/GuaranteeDescription/Text" />
							</Contents>
						</Fld>
					</xsl:when>
				</xsl:choose>
				<!--*********************************************************************************-->
				<!--	**			Process Deposit Information   	                                 **-->
				<!--*********************************************************************************-->
				<xsl:if test="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode!=''">
					<Fld>
						<ID>DP</ID>
						<Contents>
							<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardCode" />
							<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@CardNumber" />
							<xsl:text>EXP</xsl:text>
							<xsl:value-of select="DepositPayments/RequiredPayment/AcceptedPayments/AcceptedPayment/PaymentCard/@ExpireDate" />
						</Contents>
					</Fld>
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
			</FldAry>
		</HotelOptionalData>
	</xsl:if>
</xsl:template>
<!--*********************************************************************************-->
<!--	**			Name Associations for Hotels       			                  **-->
<!--*********************************************************************************-->
<xsl:template match="Traveler" mode="hotel">
	<Fld>
		<ID>NF</ID>
		<Contents>
			<xsl:value-of select="PersonName/GivenName" />
		</Contents>
	</Fld>
	<Fld>
		<ID>NL</ID>
		<Contents>
			<xsl:value-of select="PersonName/Surname" />
		</Contents>
	</Fld>
</xsl:template>
<!--*********************************************************************************-->
<!--	**		 	            Address                			                         **-->
<!--*********************************************************************************-->
<xsl:template match="Address">
	<Fld>
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
	</Fld>
</xsl:template>
<!--*********************************************************************************-->
<!--	**		 	           CustLoyalty       			                               **-->
<!--*********************************************************************************-->
<xsl:template match="CustLoyalty" mode="hotel">
	<Fld>
		<ID>FG</ID>
		<Contents>
			<xsl:value-of select="@ProgramID" />
			<xsl:value-of select="@MembershipID" />
		</Contents>
	</Fld>
</xsl:template>
<!--************************************************************************-->
<!--	Add Hotel  Remaining SupplementalInformation		    -->
<!--************************************************************************-->
<xsl:template match="SpecialRequest">
	<!-- Supplenetal information -->
	<xsl:if test="@RequestCode='SI'">
		<Fld>
			<ID>SI</ID>
			<Contents>
				<xsl:value-of select="Text" />
			</Contents>
		</Fld>
	</xsl:if>
	<!-- Overrite Corporate Code  -->
	<xsl:if test="@RequestCode='RT'">
		<Fld>
			<ID>RT</ID>
			<Contents>
				<xsl:value-of select="Text" />
			</Contents>
		</Fld>
	</xsl:if>
	<!-- Tour Number  -->
	<xsl:if test="@RequestCode='TN'">
		<Fld>
			<ID>TN</ID>
			<Contents>
				<xsl:value-of select="Text" />
			</Contents>
		</Fld>
	</xsl:if>
	<!-- Room Location -->
	<xsl:if test="@RequestCode='RL'">
		<Fld>
			<ID>RL</ID>
			<Contents>
				<xsl:value-of select="Text" />
			</Contents>
		</Fld>
	</xsl:if>
	<!-- Meal Plan-->
	<xsl:if test="@RequestCode='MP'">
		<Fld>
			<ID>MP</ID>
			<Contents>
				<xsl:value-of select="Text" />
			</Contents>
		</Fld>
	</xsl:if>
	<!-- Corporate Discount -->
	<xsl:if test="@RequestCode='CD'">
		<Fld>
			<ID>CD</ID>
			<Contents>
				<xsl:value-of select="Text" />
			</Contents>
		</Fld>
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
				  <Vnd><xsl:value-of select="VehResRQCore/VendorPref/@Code" /></Vnd> 
				  <Status>BK</Status> 
				  <NumItems>01</NumItems> 
				  <City><xsl:value-of select="VehResRQCore/VehRentalCore/PickUpLocation/@LocationCode"/></City> 
				  <StartDt><xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@PickUpDateTime),'-',''),1,8)" /></StartDt> 
				  <EndDt><xsl:value-of select="substring(translate(string(VehResRQCore/VehRentalCore/@ReturnDateTime),'-',''),1,8)" /></EndDt>
				  <Type><xsl:value-of select="VehResRQCore/VehPref/VehType/@VehicleCategory" /></Type> 
				  <DuePaidTextInd></DuePaidTextInd> 
				  <AmtDuePaid><xsl:value-of select="VehResRQCore/TPA_Extensions/CarData/CarRate/@Rate"/>.00</AmtDuePaid> 
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
				<YieldMgmtNum>
					<!--xsl:value-of select="VehResRQCore/RateQualifier/@TravelPurpose" /-->
				</YieldMgmtNum>
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
				<RefDBKey>
						<xsl:value-of select="VehResRQCore/RateQualifier/@VendorRateID"/>
				</RefDBKey>
				<PrevRefAvail>L</PrevRefAvail>
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
				<xsl:if test="VehResRQCore/RateQualifier/@CorpDiscountNmbr or VehResRQInfo/RentalPaymentPref/PaymentCard or VehResRQInfo/	@SmokingAllowed='0' or ../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/Address or 	../../OTA_TravelItineraryRQ/TPA_Extensions/PNRData/Traveler/CustLoyalty">
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
						<!--	**			Process corporate discount number                 	            **-->
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
						<!--	**			Process passenger ID                      	            **-->
						<!--*********************************************************************************-->
						<xsl:if test="VehResRQCore/RateQualifier/@PromotionCode!=''">
							<OptFldID>
								<ID>ID</ID>
								<Contents>
									<xsl:value-of select="VehResRQCore/RateQualifier/@PromotionCode" />
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
		<xsl:variable name="fseg"><xsl:value-of select="SeatRequest[1]/@FlightRefNumberRPHList"/></xsl:variable>
		<xsl:apply-templates select="SeatRequest[1]">
			<xsl:with-param name="SRPH"><xsl:value-of select="$fseg"/></xsl:with-param>
			<xsl:with-param name="AllSegs"></xsl:with-param>
		</xsl:apply-templates>
	</xsl:variable>
	<xsl:call-template name="SeatPerSeg">
		<xsl:with-param name="SRPH"><xsl:value-of select="$segs"/></xsl:with-param>
	</xsl:call-template>
</xsl:template>

<xsl:template match="SeatRequest">
	<xsl:param name="SRPH"/>
	<xsl:param name="AllSegs"/>
	<xsl:variable name="tallsegs">
		<xsl:call-template name="segs">
			<xsl:with-param name="SRPH"><xsl:value-of select="$SRPH"/></xsl:with-param>
			<xsl:with-param name="AllSegs"><xsl:value-of select="$AllSegs"/></xsl:with-param>
		</xsl:call-template>
	</xsl:variable>
	<xsl:variable name="fseg"><xsl:value-of select="following-sibling::SeatRequest[1]/@FlightRefNumberRPHList"/></xsl:variable>
	<xsl:choose>
		<xsl:when test="$fseg != ''">
			<xsl:apply-templates select="following-sibling::SeatRequest[1]">
				<xsl:with-param name="SRPH"><xsl:value-of select="$fseg"/></xsl:with-param>
				<xsl:with-param name="AllSegs"><xsl:value-of select="concat($AllSegs,$tallsegs)"/></xsl:with-param>
			</xsl:apply-templates>
		</xsl:when>
		<xsl:otherwise><xsl:value-of select="concat($AllSegs,$tallsegs)"/></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="segs">
	<xsl:param name="SRPH"/>
	<xsl:param name="AllSegs"/>
	<xsl:if test="string-length($SRPH) != 0">
		<xsl:variable name="sRPH"><xsl:value-of select="substring($SRPH,1,1)"/></xsl:variable>
		<xsl:variable name="tallsegs">
			<xsl:choose>
				<xsl:when test="contains($AllSegs,$sRPH)"></xsl:when>
				<xsl:otherwise><xsl:value-of select="$sRPH"/></xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="$tallsegs"/>
		<xsl:call-template name="segs">
			<xsl:with-param name="SRPH"><xsl:value-of select="substring($SRPH,2)"/></xsl:with-param>
			<xsl:with-param name="AllSegs"><xsl:value-of select="$AllSegs"/></xsl:with-param>
		</xsl:call-template>
	</xsl:if>
</xsl:template>

<xsl:template name="SeatPerSeg">
	<xsl:param name="SRPH"/>
	<xsl:if test="string-length($SRPH) != 0">
		<xsl:variable name="sRPH"><xsl:value-of select="substring($SRPH,1,1)"/></xsl:variable>
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
				<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $sRPH]/@DepartureDateTime,1,10),'-','')" />
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
							<xsl:variable name="TRPH"><xsl:value-of select="@TravelerRefNumberRPHList"/></xsl:variable>
							<xsl:call-template name="SeatPerPax">
								<xsl:with-param name="TRPH"><xsl:value-of select="$TRPH"/></xsl:with-param>
							</xsl:call-template>
						</xsl:for-each>
					</AttribAry>
				</AttribQual>
			</xsl:if>
		</ReqInfo>
		<xsl:call-template name="SeatPerSeg">
			<xsl:with-param name="SRPH"><xsl:value-of select="substring($SRPH,2)"/></xsl:with-param>
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
			<xsl:with-param name="TRPH"><xsl:value-of select="substring($TRPH,2)"/></xsl:with-param>
		</xsl:call-template>
	</xsl:if>
</xsl:template>
<!-- ********************************************************************************************-->
<!--			Process Names,Telephone, Customer Loyalty, ticketing	    	                -->
<!-- ********************************************************************************************-->
<xsl:template match="PNRData">
	<PNRBFPrimaryBldChgMods>
		<ItemAry>
			<xsl:apply-templates select="Traveler" mode="names" />
			<xsl:apply-templates select="Traveler" mode="ff" />
			<xsl:apply-templates select="Telephone" />
			<xsl:choose>
				<xsl:when test="Ticketing"><xsl:apply-templates select="Ticketing" /></xsl:when>
				<xsl:otherwise><xsl:apply-templates select="../../OTA_AirBookRQ/Ticketing" /></xsl:otherwise>
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
<!-- ********************************************************************************************-->
<!--							Process Air Itinerary     	               			     -->
<!-- ********************************************************************************************-->
<xsl:template match="SpecialReqDetails">
	<!-- OSI Qual  -->
	<xsl:apply-templates select="OtherServiceInformations/OtherServiceInformation" mode="osi"/>
	<!-- SSR Qual -->
	<xsl:apply-templates select="SpecialServiceRequests/SpecialServiceRequest" />
	<!-- GenRmkQual -->
	<xsl:apply-templates select="Remarks/Remark" />
	<!-- Due/paid/text -->
	<xsl:apply-templates select="UniqueRemarks/UniqueRemark[@RemarkType='DUE' or @RemarkType='PAID' or @RemarkType='TEXT']" mode="dpt"/>
	<!-- AssocRmkQual  -->
	<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Air']" mode="Air" />
	<xsl:choose>
		<xsl:when test="//POS/TPA_Extensions/Provider/Name='Galileo'">
			<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Invoice']" mode="Galileo" />
		</xsl:when>
		<xsl:otherwise>
			<xsl:apply-templates select="SpecialRemarks/SpecialRemark[@RemarkType='Invoice']" 	mode="Apollo" />
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>
<!-- ********************************************************************************************-->
<!--								Unique Remark				                         	   -->
<!-- ********************************************************************************************-->
<xsl:template match="UniqueRemark" mode="dpt">
	<Item>
		<DataBlkInd>
			<xsl:text><![CDATA[Y ]]></xsl:text>
		</DataBlkInd>
		<DuePaidTextQual>
			<EditTypeInd>A</EditTypeInd>
			<AddQual>
				<Type><xsl:value-of select="concat('A',substring-before(.,'/'))"/></Type>
				<InDt><xsl:value-of select="translate(substring-before(substring-after(.,'/'),'/'),'-','')"/></InDt>
				<DuePaidTextInd><xsl:value-of select="substring(@RemarkType,1,1)"/></DuePaidTextInd>
				<Text><xsl:value-of select="substring-after(substring-after(.,'/'),'/')"/></Text>
			</AddQual>
		</DuePaidTextQual>
	</Item>
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
							<xsl:when test="@PassengerTypeCode='SRC' and Age != ''">SC<xsl:value-of select="Age" /></xsl:when>
							<xsl:when test="@PassengerTypeCode='YTH' and Age != ''">YC<xsl:value-of select="Age" /></xsl:when>
							<xsl:when test="@PassengerTypeCode='INF' and @BirthDate != ''">
								<xsl:value-of select="substring(@BirthDate,9,2)" />
								<xsl:call-template name="month">
									<xsl:with-param name="month">
										<xsl:value-of select="substring(@BirthDate,6,2)" />
									</xsl:with-param>
								</xsl:call-template>
								<xsl:value-of select="substring(@BirthDate,3,2)" />
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="@PassengerTypeCode" /></xsl:otherwise>
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
											<xsl:text>*CH</xsl:text>
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
							<xsl:if test="../../../POS/TPA_Extensions/Provider/Name = 'Galileo'">
								<xsl:if test="@PassengerTypeCode='CHD' or @PassengerTypeCode='INF'">
									<FNameRmk>
										<xsl:if test="@PassengerTypeCode='CHD'">
											<xsl:value-of select="@PassengerTypeCode"/>
										</xsl:if>
										<xsl:call-template name="bd">
											<xsl:with-param name="bd">
												<xsl:value-of select="@BirthDate" />
											</xsl:with-param>
										</xsl:call-template>
									</FNameRmk>
								</xsl:if>
							</xsl:if>
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
	<xsl:variable name="pos"><xsl:value-of select="position()"/></xsl:variable>
	<xsl:if test="CustLoyalty/@MembershipID != ''">
		<xsl:apply-templates select="CustLoyalty">
			<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
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
				<LNameID><xsl:value-of select="$pos" /></LNameID>
				<PsgrNum>1</PsgrNum>
				<AbsNameNum><xsl:value-of select="$pos" /></AbsNameNum>
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
					<xsl:otherwise><City>UNK </City></xsl:otherwise>
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
						<xsl:when test="@PhoneLocationType='Hotel'">
							<xsl:text>H</xsl:text>
						</xsl:when>
						<xsl:when test="@PhoneLocationType='Mobile'">
							<xsl:text>M</xsl:text>
						</xsl:when>
						<xsl:otherwise>N</xsl:otherwise>
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
					<EmailData><xsl:value-of select="."/></EmailData>
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
									<xsl:otherwise><xsl:value-of select="PaymentCard/@CardCode" /></xsl:otherwise>
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
						<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'"><xsl:text><![CDATA[ Z/]]></xsl:text></xsl:when>
						<xsl:otherwise><xsl:text><![CDATA[ P/]]></xsl:text></xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="PostalCode" />
					<xsl:if test="CountryName/@Code != '' or CountryName !=''">
						<xsl:text>@</xsl:text>
						<xsl:choose>
							<xsl:when test="CountryName != ''"><xsl:value-of select="CountryName" /></xsl:when>
							<xsl:otherwise><xsl:value-of select="CountryName/@Code" /></xsl:otherwise>
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
							<xsl:when test="StateProv/@StateCode != ''"><xsl:value-of select="StateProv/@StateCode" /></xsl:when>
							<xsl:otherwise><xsl:value-of select="StateProv" /></xsl:otherwise>
						</xsl:choose>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="POS/TPA_Extensions/Provider/Name = 'Apollo'"><xsl:text><![CDATA[ Z/]]></xsl:text></xsl:when>
						<xsl:otherwise><xsl:text><![CDATA[ P/]]></xsl:text></xsl:otherwise>
					</xsl:choose>
					<xsl:value-of select="PostalCode" />
					<xsl:if test="CountryName/@Code != '' or CountryName !=''">
						<xsl:text>@</xsl:text>
						<xsl:choose>
							<xsl:when test="CountryName != ''"><xsl:value-of select="CountryName" /></xsl:when>
							<xsl:otherwise><xsl:value-of select="CountryName/@Code" /></xsl:otherwise>
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
							<xsl:value-of select="translate(substring(//AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment[@RPH = $SRPH]/@DepartureDateTime,1,10),'-','')" />
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
	<Item>
		<DataBlkInd><![CDATA[G ]]></DataBlkInd>
		<GenRmkQual>
			<EditTypeInd>A</EditTypeInd>
			<AddQual>
				<xsl:choose>
					<xsl:when test="$GDS = 'Galileo' and substring(.,2,1) = '*'">
						<Qual1><xsl:value-of select="substring(.,1,1)"/></Qual1>
						<Qual2>*</Qual2>
						<Rmk>
							<xsl:variable name="rmk">
								<xsl:value-of select="substring-after(translate(.,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ'),'*')" />
							</xsl:variable>
							<xsl:variable name="rmk1">
								<xsl:choose>
									<xsl:when test="contains($rmk,'@')">
										<xsl:value-of select="substring-before($rmk,'@')"/>
										<xsl:text>*40</xsl:text>
										<xsl:value-of select="substring-after($rmk,'@')"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="$rmk"/></xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:value-of select="$rmk1"/>
						</Rmk>
					</xsl:when>
					<xsl:otherwise>
						<Rmk>
							<xsl:variable name="rmk">
								<xsl:value-of select="translate(.,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
							</xsl:variable>
							<xsl:variable name="rmk1">
								<xsl:choose>
									<xsl:when test="contains($rmk,'@')">
										<xsl:value-of select="substring-before($rmk,'@')"/>
										<xsl:text>*40</xsl:text>
										<xsl:value-of select="substring-after($rmk,'@')"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="$rmk"/></xsl:otherwise>
								</xsl:choose>
							</xsl:variable>
							<xsl:value-of select="$rmk1"/>
						</Rmk>
					</xsl:otherwise>
				</xsl:choose>
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
						<xsl:when test="Airline/@Code != ''"><xsl:value-of select="Airline/@Code"/></xsl:when>
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
						<SSRCode><xsl:value-of select="@SSRCode" /></SSRCode>
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
								<xsl:variable name="txt">
									<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
								</xsl:variable>
								<xsl:choose>
									<xsl:when test="@SSRCode='DOCS'">
										<xsl:value-of select="translate($txt,'-','/')"/>
									</xsl:when>
									<xsl:when test="@SSRCode='FQTV' and contains($txt,'-')">
										<xsl:value-of select="substring-before($txt,'-')"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="$txt"/></xsl:otherwise>
								</xsl:choose>
								<xsl:if test="@SSRCode='FQTV'">
									<xsl:variable name="paxname">
										<xsl:value-of select="concat('-',../../../../../TPA_Extensions/PNRData/Traveler[1]/PersonName/Surname,'/',../../../../../TPA_Extensions/PNRData/Traveler[1]/PersonName/GivenName)"/>
									</xsl:variable>	
									<xsl:value-of select="translate($paxname,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />		
								</xsl:if>
							</Text>
						</xsl:if>
					</AddQual>
				</SSRQual>
			</Item>
		</xsl:when>
		<xsl:when test="not(@TravelerRefNumberRPHList)">
			<xsl:call-template name="SSRPerSeg">
				<xsl:with-param name="SRPH"><xsl:value-of select="$SRPH"/></xsl:with-param>
				<xsl:with-param name="TRPH">0</xsl:with-param>
			</xsl:call-template>
		</xsl:when>
		<xsl:when test="not(@FlightRefNumberRPHList)">
			<xsl:call-template name="SSRPerSeg">
				<xsl:with-param name="SRPH">0</xsl:with-param>
				<xsl:with-param name="TRPH"><xsl:value-of select="$TRPH"/></xsl:with-param>
			</xsl:call-template>
		</xsl:when>
		<xsl:otherwise>
			<xsl:call-template name="SSRPerSeg">
				<xsl:with-param name="SRPH"><xsl:value-of select="$SRPH"/></xsl:with-param>
				<xsl:with-param name="TRPH"><xsl:value-of select="$TRPH"/></xsl:with-param>
			</xsl:call-template>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="SSRPerSeg">
	<xsl:param name="SRPH"/>
	<xsl:param name="TRPH"/>
	<xsl:if test="$SRPH != ''">
		<xsl:call-template name="SSRPerPax">
			<xsl:with-param name="SRPH"><xsl:value-of select="$SRPH"/></xsl:with-param>
			<xsl:with-param name="TRPH"><xsl:value-of select="$TRPH"/></xsl:with-param>
		</xsl:call-template>
		<xsl:call-template name="SSRPerSeg">
			<xsl:with-param name="SRPH"><xsl:value-of select="substring($SRPH,2)"/></xsl:with-param>
			<xsl:with-param name="TRPH"><xsl:value-of select="$TRPH"/></xsl:with-param>
		</xsl:call-template>
	</xsl:if>	
</xsl:template>

<xsl:template name="SSRPerPax">
	<xsl:param name="SRPH"/>
	<xsl:param name="TRPH"/>
	<xsl:if test="string-length($TRPH) != 0">
		<xsl:variable name="sRPH"><xsl:value-of select="substring($SRPH,1,1)"/></xsl:variable>
		<xsl:variable name="tRPH"><xsl:value-of select="substring($TRPH,1,1)"/></xsl:variable>
		<Item>
			<DataBlkInd><![CDATA[S ]]></DataBlkInd>
			<SSRQual>
				<EditTypeInd>A</EditTypeInd>
				<AddQual>
					 <xsl:if test="$tRPH != '0'">
						 <LNameNum>0<xsl:value-of select="$tRPH" /></LNameNum>
						 <PsgrNum>01</PsgrNum>
						<AbsNameNum>0<xsl:value-of select="$tRPH" /></AbsNameNum>
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
							<xsl:variable name="txt">
								<xsl:value-of select="translate(Text,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="@SSRCode='DOCS'">
									<xsl:value-of select="translate($txt,'-','/')"/>
								</xsl:when>
								<xsl:when test="@SSRCode='FQTV' and contains($txt,'-')">
									<xsl:value-of select="substring-before($txt,'-')"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="$txt"/></xsl:otherwise>
							</xsl:choose>
							<xsl:if test="@SSRCode='FQTV'">
								<xsl:variable name="paxname">
										<xsl:value-of select="concat('-',../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$tRPH]/PersonName/Surname,'/',../../../../../TPA_Extensions/PNRData/Traveler[TravelerRefNumber/@RPH=$tRPH]/PersonName/GivenName)"/>
								</xsl:variable>
								<xsl:value-of select="translate($paxname,'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />	
							</xsl:if>
						</Text>
					</xsl:if>
				</AddQual>
			</SSRQual>
		</Item>
		<xsl:call-template name="SSRPerPax">
			<xsl:with-param name="SRPH"><xsl:value-of select="$SRPH"/></xsl:with-param>
			<xsl:with-param name="TRPH"><xsl:value-of select="substring($TRPH,2)"/></xsl:with-param>
		</xsl:call-template>
	</xsl:if>
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
					<xsl:value-of select="substring-before(Text,'-')" />
				</Keyword>
				<Text>
					<xsl:value-of select="translate(substring-after(Text,'-'),'abcdefghijklmnopqrstuvwxyz','ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
				</Text>
			</AddQual>
		</DOCInvoiceQual>
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
			<xsl:when test="PassengerTypeQuantity/@Code!=''">
				<xsl:apply-templates select="PassengerTypeQuantity[1]" mode="PaxType">
					<xsl:with-param name="counter">1</xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="../PNRData/Traveler" mode="PaxType" />
			</xsl:otherwise>
		</xsl:choose>
		<!--***********************************************************************-->
		<!--**                      Passenger Info                                              -->
		<!--***********************************************************************-->
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
		<xsl:if test="../../OTA_AirBookRQ/Ticketing/@TicketType = 'Paper'">
			<DocumentSelect>
				<ETInd>P</ETInd> 
				<!--ItinInd>I</ItinInd--> 
			</DocumentSelect>
		</xsl:if>
		<AssocPsgrs>
			<PsgrAry>
				<xsl:apply-templates select="../PNRData/Traveler" mode="PassengerInfo" />
			</PsgrAry>
		</AssocPsgrs>
		<PassengerType>
			<PsgrAry>
				<xsl:choose>
					<xsl:when test="PassengerTypeQuantity/@Code!=''">
						<xsl:apply-templates select="PassengerTypeQuantity[1]" mode="PassengerType">
							<xsl:with-param name="counter">1</xsl:with-param>
						</xsl:apply-templates>
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="../PNRData/Traveler" mode="PassengerType" />
					</xsl:otherwise>
				</xsl:choose>
			</PsgrAry>
		</PassengerType>
		<xsl:if test="../../POS/TPA_Extensions/Provider/Name = 'Galileo'">
			<PlatingAirVMods>
				<PlatingAirV><xsl:value-of 	select="../../OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption/FlightSegment/MarketingAirline/@Code"/> </PlatingAirV>
			</PlatingAirVMods>
		</xsl:if>
		<xsl:if test="../../POS/Source/@ISOCurrency != '' or ../../POS/Source/@ISOCountry != ''">
			<GenQuoteInfo>
				<SellCity><xsl:value-of select="../../POS/Source/@ISOCountry"/></SellCity>
				<AltCurrency><xsl:value-of select="../../POS/Source/@ISOCurrency"/></AltCurrency>
			</GenQuoteInfo> 
		</xsl:if>
		<xsl:choose>
			<!--***********************************************************************-->
			<!--**                   Negotiated fares                                              -->
			<!--***********************************************************************-->
			<xsl:when test="@PriceType='Private'">
					<xsl:choose>
						<xsl:when test="NegoFares/PriceRequestInformation/NegotiatedFareCode">
							<xsl:apply-templates select="NegoFares/PriceRequestInformation/NegotiatedFareCode"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="." mode="private"/>
						</xsl:otherwise>
					</xsl:choose>
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
	</StorePriceMods>
</xsl:template>

<xsl:template match="Commission">
	<CommissionMod>
		<xsl:choose>
			<xsl:when test="@Percent != ''"><Percent><xsl:value-of select="@Percent"/></Percent></xsl:when>
			<xsl:otherwise><Amt><xsl:value-of select="@Amount"/></Amt></xsl:otherwise>
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
						<!--xsl:value-of select="@Code"/--></Contract>
					<PublishedFaresInd>N</PublishedFaresInd>
					<Type>A</Type>
				</PFQual>
			</SegRange>
		</SegRangeAry>
	</SegSelection>
</xsl:template>
<!-- -->
<xsl:template match="PriceData" mode="private">
	<SegSelection>
		<ReqAirVPFs>Y</ReqAirVPFs>
		<SegRangeAry>
			<SegRange>
				<StartSeg>00</StartSeg>
				<EndSeg>00</EndSeg>
				<FareType>P</FareType>
				<PFQual>
					<CRSInd>
						<xsl:choose>
							<xsl:when test="../../POS/TPA_Extensions/Provider/Name = 'Galileo'">1G</xsl:when>
							<xsl:otherwise>1V</xsl:otherwise>
						</xsl:choose>
					</CRSInd>
					<PCC>
						<xsl:value-of select="../../POS/Source/@PseudoCityCode" />
					</PCC>
					<Contract/>
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
		<LNameNum>0<xsl:value-of select="position()" /></LNameNum>
		<PsgrNum>01</PsgrNum>
		<AbsNameNum>0<xsl:value-of select="position()" /></AbsNameNum>
	</Psgr>
</xsl:template>
<xsl:template match="Traveler" mode="PassengerType">
	<Psgr>
		<LNameNum>0<xsl:value-of select="position()" /></LNameNum>
		<PsgrNum>01</PsgrNum>
		<AbsNameNum>0<xsl:value-of select="position()" /></AbsNameNum>
		<PTC>
			<xsl:choose>
				<xsl:when test="@PassengerTypeCode='CHD'">
					<xsl:choose>
						<xsl:when test="@Age!=''">
							<xsl:text>C</xsl:text>
							<xsl:value-of select="format-number(@Age,'00')"/>
						</xsl:when>
						<xsl:otherwise>CNN</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="@PassengerTypeCode='SRC'">
					<xsl:choose>
						<xsl:when test="@Age!=''">
							<xsl:text>S</xsl:text>
							<xsl:value-of select="@Age"/>
						</xsl:when>
						<xsl:otherwise>SNN</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise><xsl:value-of select="@PassengerTypeCode"/></xsl:otherwise>
			</xsl:choose>
		</PTC>
		<Age> 
			<xsl:if test="@Age!=''">
				<xsl:value-of select="format-number(@Age,'00')"/>
			</xsl:if>
		</Age>
		<PricePTCOnly>N</PricePTCOnly>
	</Psgr>
</xsl:template>
<xsl:template match="Traveler" mode="PaxType">
	<PICOptMod>
		<PIC>
			<xsl:choose>
				<xsl:when test="@PassengerTypeCode='CHD'">
					<xsl:choose>
						<xsl:when test="@Age!=''">
							<xsl:text>C</xsl:text>
							<xsl:value-of select="format-number(@Age,'00')"/>
						</xsl:when>
						<xsl:otherwise>CNN</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="@PassengerTypeCode='SRC'">
					<xsl:choose>
						<xsl:when test="@Age!=''">
							<xsl:text>S</xsl:text>
							<xsl:value-of select="@Age"/>
						</xsl:when>
						<xsl:otherwise>SNN</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise><xsl:value-of select="@PassengerTypeCode"/></xsl:otherwise>
			</xsl:choose>
		</PIC>
	</PICOptMod>
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
<!-- -->
<xsl:template match="PassengerTypeQuantity" mode="PaxType">
	<xsl:param name="counter" />
	<xsl:call-template name="create_each_pax_type">
		<xsl:with-param name="typeQ">
			<xsl:value-of select="@Quantity" />
		</xsl:with-param>
		<xsl:with-param name="counter">
			<xsl:value-of select="$counter" />
		</xsl:with-param>
	</xsl:call-template>
	<xsl:apply-templates select="following-sibling::PassengerTypeQuantity[1]" mode="PaxType">
		<xsl:with-param name="counter">
			<xsl:value-of select="$counter + @Quantity" />
		</xsl:with-param>
	</xsl:apply-templates>
</xsl:template>
<xsl:template name="create_each_pax_type">
	<xsl:param name="typeQ" />
	<xsl:param name="counter" />
	<xsl:if test="$typeQ != 0">
		<PICOptMod>
			<PIC>
				<xsl:value-of select="@Code"/>
			</PIC>
		</PICOptMod>
		<xsl:call-template name="create_each_pax_type">
			<xsl:with-param name="typeQ">
				<xsl:value-of select="$typeQ - 1" />
			</xsl:with-param>
			<xsl:with-param name="counter">
				<xsl:value-of select="$counter + 1" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:if>
</xsl:template>
<!-- -->
<xsl:template match="PassengerTypeQuantity" mode="PassengerType">
	<xsl:param name="counter" />
	<xsl:call-template name="create_each_passenger_type">
		<xsl:with-param name="typeQ">
			<xsl:value-of select="@Quantity" />
		</xsl:with-param>
		<xsl:with-param name="counter">
			<xsl:value-of select="$counter" />
		</xsl:with-param>
	</xsl:call-template>
	<xsl:apply-templates select="following-sibling::PassengerTypeQuantity[1]" mode="PassengerType">
		<xsl:with-param name="counter">
			<xsl:value-of select="$counter + @Quantity" />
		</xsl:with-param>
	</xsl:apply-templates>
</xsl:template>
<xsl:template name="create_each_passenger_type">
	<xsl:param name="typeQ" />
	<xsl:param name="counter" />
	<xsl:if test="$typeQ != 0">
		<Psgr>
			<LNameNum>0<xsl:value-of select="$counter" /></LNameNum>
			<PsgrNum>01</PsgrNum>
			<AbsNameNum>0<xsl:value-of select="$counter" /></AbsNameNum>
			<PTC>
				<xsl:value-of select="@Code"/>
			</PTC>
			<PricePTCOnly>N</PricePTCOnly>
		</Psgr>
		<xsl:call-template name="create_each_passenger_type">
			<xsl:with-param name="typeQ">
				<xsl:value-of select="$typeQ - 1" />
			</xsl:with-param>
			<xsl:with-param name="counter">
				<xsl:value-of select="$counter + 1" />
			</xsl:with-param>
		</xsl:call-template>
	</xsl:if>
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
		<xsl:apply-templates select="UniqueRemark" mode="UniqueRemark"/>
	</PassiveSegmentSellMods>
</xsl:template>

<xsl:template match="UniqueRemark" mode="UniqueRemark">
	<PassiveSegmentSellRequest>
		<PassiveSegType><xsl:value-of select="@RemarkType"/></PassiveSegType>
		<Vnd><xsl:value-of select="substring(.,1,2)"/></Vnd>
		<Status><xsl:value-of select="substring(.,4,2)"/></Status>
		<NumItems><xsl:value-of select="substring(.,7,2)"/></NumItems>
		<City><xsl:value-of select="substring(.,10,3)"/></City>
		<StartDt><xsl:value-of select="translate(substring(.,14,10),'-','')"/></StartDt>
		<EndDt />
		<Type></Type>
		<DuePaidTextInd></DuePaidTextInd>
		<AmtDuePaid></AmtDuePaid>
	</PassiveSegmentSellRequest>
	<xsl:if test="substring(.,24,1)='/'">
		<PassiveSegmentSellFreeformRequest>
			<PropAddrInd>CF</PropAddrInd>
			<Text><xsl:value-of select="substring(.,25)"/></Text>
		</PassiveSegmentSellFreeformRequest>
	</xsl:if>
</xsl:template>

</xsl:stylesheet>



