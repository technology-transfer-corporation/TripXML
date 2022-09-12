<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelAvailRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_HotelAvailRQ">
		<xsl:choose>
			<!--This is the RoomRate message when requesting prices for a specific Hotel Code -->
			<xsl:when test="AvailRequestSegments/AvailRequestSegment/HotelSearchCriteria/Criterion/HotelRef/@HotelCode != ''">
				<OTA_HotelAvailRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
					<POS>
						<Source>
							<xsl:attribute name="PseudoCityCode">
								<xsl:value-of select="POS/Source/@PseudoCityCode" />
							</xsl:attribute>
						</Source>
					</POS>
					<xsl:element name="AvailRequestSegments">
						<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="RoomRate" />
					</xsl:element>
				</OTA_HotelAvailRQ>
			</xsl:when>
			<!--This is the HotelAvail message when NOT requesting prices for a specific Hotel Code -->
			<xsl:otherwise>
				<OTA_HotelAvailRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.5.1">
					<POS>
						<Source>
							<xsl:attribute name="PseudoCityCode">
								<xsl:value-of select="POS/Source/@PseudoCityCode" />
							</xsl:attribute>
							<xsl:if test="POS/Source/@ISOCountry != ''">
								<xsl:attribute name="ISOCountry">
									<xsl:value-of select="POS/Source/@ISOCountry" />
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="POS/Source/@ISOCurrency != ''">
								<xsl:attribute name="ISOCurrency">
									<xsl:value-of select="POS/Source/@ISOCurrency" />
								</xsl:attribute>
							</xsl:if>
						</Source>
					</POS>
					<AvailRequestSegments>
						<xsl:apply-templates select="AvailRequestSegments/AvailRequestSegment" mode="HotelAvail" />
					</AvailRequestSegments>
				</OTA_HotelAvailRQ>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="AvailRequestSegment" mode="RoomRate">
		<xsl:element name="AvailRequestSegment">
			<StayDateRange>
				<xsl:attribute name="Start">
					<xsl:value-of select="StayDateRange/@Start" />
				</xsl:attribute>
				<xsl:attribute name="End">
					<xsl:value-of select="StayDateRange/@End" />
				</xsl:attribute>
			</StayDateRange>
			<RateRange>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="RateRange/@CurrencyCode !=''">
							<xsl:value-of select="RateRange/@CurrencyCode" />
						</xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<!--xsl:if test="RateRange/@MinRate !=''">		
					<xsl:attribute name="MinRate">
						<xsl:value-of select="RateRange/@MinRate" />
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="RateRange/@MaxRate !=''">		
					<xsl:attribute name="MaxRate">
						<xsl:value-of select="RateRange/@MaxRate" />
					</xsl:attribute>
				</xsl:if-->
			</RateRange>
			<xsl:if test="RatePlanCandidates">
				<RatePlanCandidates>
					<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate" mode="RoomRate" />
				</RatePlanCandidates>
			</xsl:if>
			<RoomStayCandidates>
				<xsl:apply-templates select="RoomStayCandidates/RoomStayCandidate" mode="RoomRate" />
			</RoomStayCandidates>
			<HotelSearchCriteria>
				<Criterion>
					<!--CodeRef>
					<xsl:value-of select="HotelSearchCriteria/Criterion/CodeRef/@LocationCode" />
				</CodeRef-->
					<!--Address /-->
					<HotelRef>
						<!--xsl:attribute name="ChainCode">
							<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode" />
						</xsl:attribute-->
						<xsl:attribute name="HotelCode">
							<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCode" />
						</xsl:attribute>
					</HotelRef>
					<!--xsl:if test="Radius">
					<Radius>
						<xsl:attribute name="Distance">
							<xsl:value-of select="HotelSearchCriteria/Criterion/@Distance" />
						</xsl:attribute>
					</Radius>
				</xsl:if-->
				</Criterion>
			</HotelSearchCriteria>
		</xsl:element>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="RatePlanCandidate" mode="RoomRate">
		<RatePlanCandidate>
			<xsl:attribute name="RatePlanCode">
				<xsl:value-of select="@RatePlanCode" />
			</xsl:attribute>
		</RatePlanCandidate>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="RoomStayCandidate" mode="RoomRate">
		<RoomStayCandidate>
			<!--xsl:attribute name="Quantity">
					<xsl:value-of select="GuestCounts/GuestCount/@Count" />
			</xsl:attribute-->
			<GuestCounts>
				<GuestCount>
					<xsl:attribute name="Count">
						<xsl:value-of select="GuestCounts/GuestCount/@Count" />
					</xsl:attribute>
				</GuestCount>
			</GuestCounts>
		</RoomStayCandidate>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="AvailRequestSegment" mode="HotelAvail">
		<AvailRequestSegment>
			<StayDateRange>
				<xsl:attribute name="Start">
					<xsl:value-of select="StayDateRange/@Start" />
				</xsl:attribute>
				<xsl:attribute name="End">
					<xsl:value-of select="StayDateRange/@End" />
				</xsl:attribute>
			</StayDateRange>
			<RateRange>
				<xsl:attribute name="CurrencyCode">
					<xsl:choose>
						<xsl:when test="RateRange/@CurrencyCode !=''">
							<xsl:value-of select="RateRange/@CurrencyCode" />
						</xsl:when>
						<xsl:otherwise>USD</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:if test="RateRange/@MinRate != ''">
					<xsl:attribute name="MinRate">
						<xsl:value-of select="RateRange/@MinRate" />
					</xsl:attribute>
				</xsl:if>
				<xsl:if test="RateRange/@MaxRate != ''">
					<xsl:attribute name="MaxRate">
						<xsl:value-of select="RateRange/@MaxRate" />
					</xsl:attribute>
				</xsl:if>
			</RateRange>
			<xsl:if test="RatePlanCandidates">
				<RatePlanCandidates>
					<xsl:apply-templates select="RatePlanCandidates/RatePlanCandidate" mode="HotelAvail" />
				</RatePlanCandidates>
			</xsl:if>
			<RoomStayCandidates>
				<xsl:apply-templates select="RoomStayCandidates/RoomStayCandidate" mode="HotelAvail" />
			</RoomStayCandidates>
			<HotelSearchCriteria>
				<Criterion>
					<HotelRef>
						<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@ChainCode !=''">
							<xsl:attribute name="ChainCode">
								<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@ChainCode" />
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode!=''">
							<xsl:attribute name="HotelCityCode">
								<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCityCode" />
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelCode !=''">
							<xsl:attribute name="HotelCode">
								<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelCode" />
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="HotelSearchCriteria/Criterion/HotelRef/@HotelName !=''">
							<xsl:attribute name="HotelName">
								<xsl:value-of select="HotelSearchCriteria/Criterion/HotelRef/@HotelName" />
							</xsl:attribute>
						</xsl:if>
					</HotelRef>
					<xsl:if test="HotelSearchCriteria/Criterion/Radius/@Distance">
						<Radius>
							<xsl:attribute name="Distance">
								<xsl:value-of select="HotelSearchCriteria/Criterion/Radius/@Distance" />
							</xsl:attribute>
						</Radius>
					</xsl:if>
				</Criterion>
			</HotelSearchCriteria>
			<!--TPA_Extensions>
				<MaxHotelsReturned>
					<xsl:choose>
						<xsl:when test="../../@MaxResponses!=''">
							<xsl:choose>
								<xsl:when test="../../@MaxResponses &lt; 101"><xsl:value-of select="../../@MaxResponses"/></xsl:when>
								<xsl:otherwise>50</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>50</xsl:otherwise>
					</xsl:choose>
				</MaxHotelsReturned>
				<ReturnHotelMap>
					<xsl:choose>
						<xsl:when test="TPA_Extensions/ReturnHotelMap != ''">
							<xsl:value-of select="TPA_Extensions/ReturnHotelMap" />
						</xsl:when>
						<xsl:otherwise>NO</xsl:otherwise>
					</xsl:choose>
				</ReturnHotelMap>
			</TPA_Extensions-->
		</AvailRequestSegment>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="RatePlanCandidate" mode="HotelAvail">
		<RatePlanCandidate>
			<xsl:attribute name="RatePlanCode">
				<xsl:value-of select="@RatePlanCode" />
			</xsl:attribute>
		</RatePlanCandidate>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="RoomStayCandidate" mode="HotelAvail">
		<RoomStayCandidate>
			<xsl:if test="@RoomType !=''">
				<xsl:attribute name="RoomType">
					<xsl:value-of select="@RoomType" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@RoomTypeCode !=''">
				<xsl:attribute name="RoomTypeCode">
					<xsl:value-of select="@RoomTypeCode" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@RoomID !=''">
				<xsl:attribute name="RoomID">
					<xsl:value-of select="@RoomID" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@InvBlockCode !=''">
				<xsl:attribute name="InvBlockCode">
					<xsl:value-of select="@InvBlockCode" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@PromotionCode !=''">
				<xsl:attribute name="PromotionCode">
					<xsl:value-of select="@PromotionCode" />
				</xsl:attribute>
			</xsl:if>
			<xsl:attribute name="Quantity">
				<xsl:choose>
					<xsl:when test="@Quantity !=''">
						<xsl:value-of select="@Quantity" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="GuestCounts/GuestCount/@Count" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<GuestCounts>
				<GuestCount>
					<xsl:attribute name="Count">
						<xsl:value-of select="GuestCounts/GuestCount/@Count" />
					</xsl:attribute>
				</GuestCount>
			</GuestCounts>
		</RoomStayCandidate>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
