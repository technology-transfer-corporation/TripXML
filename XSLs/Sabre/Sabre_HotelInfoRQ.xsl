<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelDescriptiveInfoRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_HotelDescriptiveInfoRQ">
		<OTA_HotelAvailRQ xmlns="http://www.opentravel.org/OTA/2002/08">
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
				</Source>
			</POS>
			<AvailRequestSegments>
				<xsl:apply-templates select="HotelDescriptiveInfos/HotelDescriptiveInfo" />
			</AvailRequestSegments>
		</OTA_HotelAvailRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="HotelDescriptiveInfo">
		<AvailRequestSegment>
			<!--Note that there is no StayDateRange in OTA HotelDescription msg. -->
			<StayDateRange>
				<xsl:attribute name="Start">
					<xsl:value-of select="StayDateRange/@Start" />
				</xsl:attribute>
				<xsl:attribute name="End">
					<xsl:value-of select="StayDateRange/@End" />
				</xsl:attribute>
			</StayDateRange>
			<RateRange CurrencyCode="USD" />
			<RoomStayCandidates>
				<RoomStayCandidate>
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
					<Address />
					<HotelRef>
						<xsl:attribute name="HotelCode">
							<xsl:value-of select="@HotelCode" />
						</xsl:attribute>
					</HotelRef>
				</Criterion>
			</HotelSearchCriteria>
		</AvailRequestSegment>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>
