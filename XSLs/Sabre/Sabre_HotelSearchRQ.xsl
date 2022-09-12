<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelSearchRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_HotelSearchRQ">
		<OTA_HotelAvailRQ xmlns="http://www.opentravel.org/OTA/2002/08">
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
				<xsl:apply-templates select="Criteria/Criterion" />
			</AvailRequestSegments>
		</OTA_HotelAvailRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="Criterion">
		<!--there is no StayDateRange on OTA_HotelSearch schema -->
		<AvailRequestSegment>
			<StayDateRange>
				<xsl:attribute name="Start">
					<xsl:choose>
						<xsl:when test="StayDateRange/@Start  != ''">
							<xsl:value-of select="StayDateRange/@Start" />
						</xsl:when>
						<xsl:otherwise>2020-01-17</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="End">
					<xsl:choose>
						<xsl:when test="StayDateRange/@End != ''">
							<xsl:value-of select="StayDateRange/@End" />
						</xsl:when>
						<xsl:otherwise>2020-01-24</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</StayDateRange>
			<RateRange CurrencyCode="USD" />
			<RoomStayCandidates>
				<RoomStayCandidate Quantity="1">
					<!--note that @Count is hardcoded because there is not a @Count in the OTA msg -->
					<GuestCounts>
						<GuestCount>
							<xsl:attribute name="Count">1</xsl:attribute>
						</GuestCount>
					</GuestCounts>
				</RoomStayCandidate>
			</RoomStayCandidates>
			<HotelSearchCriteria>
				<Criterion>
					<CodeRef>
						<xsl:value-of select="HotelRef/@HotelCityCode" />
					</CodeRef>
					<xsl:if test="HotelRef/@ChainCode != ''">
						<HotelRef>
							<xsl:if test="HotelRef/@ChainCode != ''">
								<xsl:attribute name="ChainCode">
									<xsl:value-of select="HotelRef/@ChainCode" />
								</xsl:attribute>
							</xsl:if>
						</HotelRef>
					</xsl:if>
					<Radius>
						<xsl:attribute name="Distance">
							<xsl:choose>
								<xsl:when test="Radius/@Distance != ''">
									<xsl:value-of select="Radius/@Distance" />
								</xsl:when>
								<xsl:otherwise>10</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</Radius>
				</Criterion>
			</HotelSearchCriteria>
		</AvailRequestSegment>
		<TPA_Extensions>
			<MaxHotelsReturned>100</MaxHotelsReturned>
			<ReturnHotelMap>NO</ReturnHotelMap>
		</TPA_Extensions>
		<!--I don't know if we should add these TPA_Extensions, so for now I commented them out -->
		<!--TPA_Extensions>
			<MaxHotelsReturned>
				<xsl:value-of select="TPA_Extensions/MaxHotelsReturned"/>
			</MaxHotelsReturned>
				<ReturnHotelMap>
					<xsl:value-of select="TPA_Extensions/ReturnHotelMap"/>
				</ReturnHotelMap>
			</TPA_Extensions-->
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
