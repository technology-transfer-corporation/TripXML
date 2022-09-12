<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_HotelAvailRQ.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 20 Feb 2011 - Rastko - changed priority in hotel chain code				-->
	<!-- Date: 29 May 2008 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelAvailRQ"/>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="OTA_HotelAvailRQ">
		<xsl:choose>
			<xsl:when test="AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion/HotelRef/@HotelCode != ''">
				<HotelCompleteAvailability_9_0_2>
					<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="Single"/>
				</HotelCompleteAvailability_9_0_2>
			</xsl:when>
			<xsl:otherwise>
				<HotelAvailability_11_0_2>
					<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="Multiple"/>
				</HotelAvailability_11_0_2>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="AvailRequestSegment" mode="Single">
		<HotelCompleteAvailabilityMods>
			<HotelInsideAvailability>
				<ArrivalDt>
					<xsl:value-of select="translate(string(StayDateRange/@Start),'-','')"/>
				</ArrivalDt>
				<DepartureDt>
					<xsl:value-of select="translate(string(StayDateRange/@End),'-','')"/>
				</DepartureDt>
				<NumNights><xsl:value-of select="StayDateRange/@Duration"/></NumNights>
				<xsl:apply-templates select="RoomStayCandidates/RoomStayCandidate" mode="Count"/>
				<Vnd><xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode"/></Vnd>
				<RMNum><xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCode"/></RMNum>
				<AltAvailInd>N</AltAvailInd>
				<RMOnlyInd>N</RMOnlyInd>
				<NoRelaxInd>M</NoRelaxInd>
				<xsl:if test="../../POS/Source/@ISOCurrency!=''">
					<AltCrncy>
						<xsl:value-of select="../../POS/Source/@ISOCurrency"/>
					</AltCrncy>
				</xsl:if>
			</HotelInsideAvailability>
			<xsl:if test="@MoreDataEchoToken!= ''">
				<HotelInsideAvailMore>
					<VKeys>
						<xsl:value-of select="substring(string(@MoreDataEchoToken),2)"/>
					</VKeys>
				</HotelInsideAvailMore>
			</xsl:if>
			<xsl:if test="RoomStayCandidates/RoomStayCandidate/@RoomType!='' or RatePlanCandidates/RatePlanCandidate/@RatePlanType!='' or RatePlanCandidates/RatePlanCandidate/@RatePlanID!='' or RateRange/@MinRate!=''">
				<HotelSlotData>
					<SlotIDAry>
						<xsl:apply-templates select="RoomStayCandidates/RoomStayCandidate[@RoomType!='']" mode="RoomType"/>
						<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate[@RatePlanType!='']" mode="RatePlanType"/>
						<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate[@RatePlanID!='']" mode="RatePlanIDS"/>
						<xsl:if test="RateRange[@MinRate!='']">
							<xsl:variable name="zero">00000000</xsl:variable>
							<SlotID>
								<ID>R</ID>
								<Priority>01</Priority>
								<Data>
									<xsl:value-of select="substring($zero,1,9-string-length(RateRange/@MinRate))"/>
									<xsl:value-of select="RateRange/@MinRate"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring($zero,1,9-string-length(RateRange/@MaxRate))"/>
									<xsl:value-of select="RateRange/@MaxRate"/>
								</Data>
							</SlotID>
						</xsl:if>
					</SlotIDAry>
				</HotelSlotData>
			</xsl:if>
		</HotelCompleteAvailabilityMods>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="RoomStayCandidate" mode="Count">
		<NumAdults>
			<xsl:value-of select="sum(GuestCounts/GuestCount/@Count)"/>
		</NumAdults>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="RoomStayCandidate" mode="RoomType">
		<SlotID>
			<ID>B</ID>
			<Priority>01</Priority>
			<Data>
				<xsl:value-of select="substring(string(@RoomType),1,3)"/>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="RatePlanCandidate" mode="RatePlanType">
		<SlotID>
			<ID>C</ID>
			<Priority>01</Priority>
			<Data>
				<xsl:value-of select="@RatePlanType"/>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="RatePlanCandidate" mode="RatePlanIDS">
		<SlotID>
			<ID>M</ID>
			<Priority>01</Priority>
			<Data>
				<xsl:value-of select="@RatePlanID"/>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<!--The next set of template matches are for the Multi Availability message -->
	<!-- **********************************************************************************************************-->
	<xsl:template match="AvailRequestSegment" mode="Multiple">
		<AvailabilityRequestMods>
			<HotelRequestHeader>
				<StartDt>
					<xsl:value-of select="translate(string(StayDateRange/@Start),'-','')"/>
				</StartDt>
				<EndDt>
					<xsl:value-of select="translate(string(StayDateRange/@End),'-','')"/>
				</EndDt>
				<City>
					<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode"/>
				</City>
				<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="RefPoint"/>
				<NoRelaxInd>Y</NoRelaxInd>
				<xsl:if test="../../POS/Source/@ISOCurrency != ''">
					<DesiredAltCrncy><xsl:value-of select="../../POS/Source/@ISOCurrency"/></DesiredAltCrncy>
				</xsl:if>
			</HotelRequestHeader>
			<HotelRequest>
				<ReqInd>C</ReqInd>
				<NumNights><xsl:value-of select="StayDateRange/@Duration"/></NumNights>
				<xsl:apply-templates select="RoomStayCandidates/RoomStayCandidate" mode="CountM"/>
			</HotelRequest>		
			<xsl:if test="@MoreDataEchoToken !=''">
				<HotelGetMore>
					<StartDBKey>
						<xsl:value-of select="substring-after(substring-before(string(@MoreDataEchoToken),'B'),'A')"/>
					</StartDBKey>
					<EndDBKey>
						<xsl:value-of select="substring-after(substring-before(string(@MoreDataEchoToken),'C'),'B')"/>
					</EndDBKey>
					<RefPtDBKey>
						<xsl:value-of select="substring-after(substring-before(string(@MoreDataEchoToken),'D'),'C')"/>
					</RefPtDBKey>
					<BitMap>
						<xsl:value-of select="substring-after(substring-before(string(@MoreDataEchoToken),'E'),'D')"/>
					</BitMap>
					<TotNumProps>
						<xsl:value-of select="substring-before(string(@MoreDataEchoToken),'A')"/>
					</TotNumProps>
					<SubtotalNumPropsRet>
						<xsl:value-of select="substring-after(substring-before(string(@MoreDataEchoToken),'F'),'E')"/>
					</SubtotalNumPropsRet>
					<MoreInd>Y</MoreInd>
					<DBPropTok>
						<xsl:value-of select="substring-after(substring-before(string(@MoreDataEchoToken),'G'),'F')"/>
					</DBPropTok>
					<DBPropKey>
						<xsl:value-of select="substring-after(string(@MoreDataEchoToken),'G')"/>
					</DBPropKey>
				</HotelGetMore>
			</xsl:if>
			<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelCode!=''">
				<IDNumAry>
					<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="PropCode"/>
				</IDNumAry>
			</xsl:if>
			<xsl:if test="HotelSearchCriteria/Criterion/HotelAmenity/@Code !='' or RoomStayCandidates/RoomStayCandidate/@RoomType!='' or RatePlanCandidates/RatePlanCandidate/@RatePlanCode!='' or RatePlanCandidates/RatePlanCandidate/@RatePlanID!='' or HotelSearchCriteria/Criterion/Radius/@Distance!='' or HotelSearchCriteria/Criterion/Radius/@Direction!=''or HotelSearchCriteria/Criterion/HotelRef/@HotelName!='' or HotelSearchCriteria/Criterion/CodeRef/@LocationCode!='' or RateRange/@MinRate!='' or RateRange/@MaxRate!='' or HotelSearchCriteria/Criterion/HotelRef/@ChainCode!=''">
				<HotelSlotData>
					<SlotIDAry>
						<xsl:apply-templates select="HotelSearchCriteria/Criterion/HotelAmenity"/>
						<xsl:apply-templates select="RoomStayCandidates/RoomStayCandidate" mode="RoomType"/>
						<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate[@RatePlanCode != '']" mode="RatePlanCode"/>
						<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate[@RatePlanID != '']" mode="RatePlanIDM"/>
						<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="Radius"/>
						<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="LocationCode"/>
						<!--Come back and do RateAccess -->
						<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="HotelName"/>
						<!--Come back and do PropertyType -->
						<xsl:apply-templates select="RateRange"/>
						<!--Come back and do Transportation -->
						<xsl:apply-templates select="HotelSearchCriteria/Criterion" mode="ChainCode"/>
					</SlotIDAry>
				</HotelSlotData>
			</xsl:if>
		</AvailabilityRequestMods>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="RoomStayCandidate" mode="CountM">
		<NumPersons>
			<xsl:value-of select="sum(GuestCounts/GuestCount/@Count)"/>
		</NumPersons>
	</xsl:template>
	<!-- **********************************************************************************************************-->
	<xsl:template match="Criterion" mode="RefPoint">
		<RefPt>
			<xsl:value-of select="RefPoint"/>
		</RefPt>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="AlternateCurrencyCode">
		<DesiredAltCurrency>
			<xsl:value-of select="."/>
		</DesiredAltCurrency>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="Criterion" mode="PropCode">
		<IDNum>
			<Vnd><xsl:value-of select="HotelRef/@ChainCode"/></Vnd>
			<Num><xsl:value-of select="HotelRef/@HotelCode"/></Num>			
		</IDNum>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="HotelAmenity">
		<SlotID>
			<ID>A</ID>
			<Priority>01</Priority>
			<!--Come back to this and put a conversion of standard HotelAmenity Codes according to OTA and convert to Galileo standard codes -->
			<Data>
				<xsl:choose>
					<xsl:when test="@Code = 'Air'">01</xsl:when>
					<xsl:when test="@Code = 'Babysitting'">03</xsl:when>
					<xsl:when test="@Code = 'Barber'">05</xsl:when>
					<xsl:when test="@Code = 'Beauty'">06</xsl:when>
					<xsl:when test="@Code = 'Rental Car Desk'">09</xsl:when>
					<xsl:when test="@Code = 'Concierge'">14</xsl:when>
					<xsl:when test="@Code = 'Entertainment'">21</xsl:when>
					<xsl:when test="@Code = 'Family Plan'">22</xsl:when>
					<xsl:when test="@Code = 'Golf'">27</xsl:when>
					<xsl:when test="@Code = 'Handicap'">28</xsl:when>
					<xsl:when test="@Code = 'Health Club'">29</xsl:when>
					<xsl:when test="@Code = 'Kitchen'">31</xsl:when>
					<xsl:when test="@Code = 'Laundry'">32</xsl:when>
					<xsl:when test="@Code = 'Meeting Room'">36</xsl:when>
					<xsl:when test="@Code = 'Minibar'">37</xsl:when>
					<xsl:when test="@Code = 'Non Smoking'">40</xsl:when>
					<xsl:when test="@Code = 'Parking'">41</xsl:when>
					<xsl:when test="@Code = 'Pets'">43</xsl:when>
					<xsl:when test="@Code = 'Pool'">45</xsl:when>
					<xsl:when test="@Code = 'Indoor Pool'">46</xsl:when>
					<xsl:when test="@Code = 'Outdoor Pool'">47</xsl:when>
					<xsl:when test="@Code = 'Restaurant'">50</xsl:when>
					<xsl:when test="@Code = 'Room Service'">51</xsl:when>
					<xsl:when test="@Code = 'Sauna'">56</xsl:when>
					<xsl:when test="@Code = 'Secretarial'">57</xsl:when>
					<xsl:when test="@Code = 'Tennis Court'">63</xsl:when>
					<xsl:when test="@Code = 'Cable TV'">66</xsl:when>
					<xsl:when test="@Code = 'WC'">69</xsl:when>
					<xsl:when test="@Code = 'Wet Bar'">70</xsl:when>
				</xsl:choose>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="RoomStayCandidate" mode="RoomType">
		<xsl:if test="@RoomType !=''">
			<SlotID>
				<ID>B</ID>
				<Priority>01</Priority>
				<Data>
					<xsl:value-of select="@RoomType"/>
				</Data>
			</SlotID>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="RatePlanCandidate" mode="RatePlanCode">
		<SlotID>
			<ID>ID</ID>
			<Priority>01</Priority>
			<Data>
				<xsl:value-of select="@RatePlanCode"/>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="RatePlanCandidate" mode="RatePlanIDM">
		<SlotID>
			<ID>M</ID>
			<Priority>01</Priority>
			<Data>
				<xsl:value-of select="@RatePlanID"/>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="Criterion" mode="Radius">
		<xsl:if test="Radius/@Distance !=''">
			<SlotID>
				<ID>D</ID>
				<Priority>01</Priority>
				<Data>
					<xsl:value-of select="string(' 000')"/>
					<xsl:value-of select="Radius/@DistanceMeasure"/>
					<xsl:choose>
						<xsl:when test="string-length(string(Radius/@Direction))='1'">
							<xsl:value-of select="Radius/@Direction"/>
							<xsl:value-of select="string(' ')"/>
						</xsl:when>
						<xsl:when test="string-length(string(Radius/@Direction))='2'">
							<xsl:value-of select="Radius/@Direction"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="string('  ')"/>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:text>-</xsl:text>
					<xsl:variable name="zero">000</xsl:variable>
					<xsl:value-of select="substring($zero,1,3-string-length(Radius/@Distance))"/>
					<xsl:value-of select="Radius/@Distance"/>
				</Data>
			</SlotID>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="Criterion" mode="LocationCode">
		<xsl:if test="CodeRef/@LocationCode!=''">
			<SlotID>
				<ID>L</ID>
				<Priority>01</Priority>
				<!--Come back to this and put a conversion of standard HoteLocationCodes according to OTA and convert to Galileo 	standard codes -->
				<Data>
					<xsl:value-of select="CodeRef/@LocationCode"/>
				</Data>
			</SlotID>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="Criterion" mode="HotelName">
		<xsl:if test="HotelRef/@HotelName!=''">
			<SlotID>
				<ID>N</ID>
				<Priority>01</Priority>
				<Data>
					<xsl:value-of select="substring(string(HotelRef/@HotelName),1,12)"/>
				</Data>
			</SlotID>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="RateRange">
		<xsl:variable name="zero">00000000</xsl:variable>
		<SlotID>
			<ID>R</ID>
			<Priority>01</Priority>
			<Data>
				<xsl:value-of select="substring($zero,1,9-string-length(@MinRate))"/>
				<xsl:value-of select="@MinRate"/>
				<xsl:text>-</xsl:text>
				<xsl:value-of select="substring($zero,1,9-string-length(@MaxRate))"/>
				<xsl:value-of select="@MaxRate"/>
			</Data>
		</SlotID>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
	<xsl:template match="Criterion" mode="ChainCode">
		<xsl:if test="HotelRef/@ChainCode!=''">
			<SlotID>
				<ID>Z</ID>
				<Priority>
					<xsl:choose>
						<xsl:when test="../../RatePlanCandidates/RatePlanCandidate/@RatePlanID!=''">02</xsl:when>
						<xsl:otherwise>01</xsl:otherwise>
					</xsl:choose>
				</Priority>
				<Data>
					<xsl:value-of select="HotelRef/@ChainCode"/>
				</Data>
			</SlotID>
		</xsl:if>
	</xsl:template>
	<!-- *********************************************************************************************************** -->
</xsl:stylesheet>
