<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_HotelSearchRQ" />
	</xsl:template>

	<xsl:template match="OTA_HotelSearchRQ">
		<xsl:choose>
			<xsl:when test="Criteria/Criterion/RefPoint = 'ALL'">
				<HotelReferencePoint_5_0>
					<RefPtListMods>
						<City>
							<xsl:value-of select="Criteria/Criterion/HotelRef/@HotelCityCode" />
						</City>
					</RefPtListMods>
				</HotelReferencePoint_5_0>
			</xsl:when>
			<xsl:otherwise>
				<HotelIndex_7_0>
					<HtlIndexMods>
						<xsl:apply-templates select="Criteria/Criterion" />
					</HtlIndexMods>
				</HotelIndex_7_0>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="Criterion">
		<StartDt></StartDt>
		<EndDt></EndDt>
		<City>
			<xsl:value-of select="HotelRef/@HotelCityCode" />
			<xsl:value-of select="string('  ')" />
		</City>
		<xsl:if test="RefPoint !=''">
			<RefPt>
				<xsl:value-of select="RefPoint" />
			</RefPt>
		</xsl:if>
		<NoRelaxInd>Y</NoRelaxInd>
		<Currency>
			<xsl:choose>
				<xsl:when test="../../POS/Source/@ISOCurrency !=''">
					<xsl:value-of select="../../POS/Source/@ISOCurrency" />
				</xsl:when>
				<xsl:otherwise>USD</xsl:otherwise>
			</xsl:choose>
		</Currency>
		<MoreInd>N</MoreInd>
		<SlotIDAry>
			<xsl:if test="HotelAmenity/@Code !=''">
				<SlotID>
					<ID>A </ID>
					<Priority>1</Priority>
					<Data>
						<xsl:choose>
							<xsl:when test="HotelAmenity/@Code = 'Air Conditioning'">01</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Babysitting'">03</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Barber'">05</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Beauty Shop'">06</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Rental Car Desk'">09</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Concierge'">14</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Entertainment'">21</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Family Plan'">22</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Golf'">27</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Handicap'">28</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Health Club'">29</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Kitchen'">31</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Laundry'">32</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Meeting Room'">36</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Minibar'">37</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Movies'">38</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Non Smoking Rooms'">40</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Parking'">41</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Pets'">43</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Pool'">45</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Indoor Pool'">46</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Outdoor Pool'">47</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Restaurant'">50</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Room Service'">51</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Sauna'">56</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Secretarial'">57</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Tennis Court'">63</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Cable TV'">66</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'WC'">69</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Wet Bar'">70</xsl:when>
							<xsl:when test="HotelAmenity/@Code = 'Fire Safety'">80</xsl:when>
						</xsl:choose>
					</Data>
				</SlotID>
			</xsl:if>
			<xsl:if test="Award/@Provider ='AAA'">
				<SlotID>
					<ID>E </ID>
					<Priority>1</Priority>
					<Data>
						<xsl:value-of select="Award/@Rating" />
					</Data>
				</SlotID>
			</xsl:if>
			<xsl:if test="Radius/@Distance !=''">
				<xsl:variable name="DirectionLength">
					<xsl:value-of select="string-length(Radius/@Direction)" />
				</xsl:variable>
				<SlotID>
					<ID>D </ID>
					<Priority>1</Priority>
					<xsl:variable name="zero">00</xsl:variable>
					<Data>001<xsl:value-of select="Radius/@DistanceMeasure" /><xsl:choose>
							<xsl:when test="Radius/@Direction = '' or  $DirectionLength !='2'">NW</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Radius/@Direction" />
							</xsl:otherwise>
						</xsl:choose><xsl:value-of select="string(' ')" /><xsl:value-of select="substring($zero,1,3-string-length(Radius/@Distance))" />
							<xsl:value-of select="Radius/@Distance" /></Data>
					<!--Note: Due to testing done on 1/7/04, a single character direction fails when sending to Galileo, and 2 blanks fails, therefore NW is hard coded. Also the '-' (less than sign) can't be used -->
				</SlotID>
			</xsl:if>
			<xsl:if test="HotelRef/@HotelName !=''">
				<SlotID>
					<ID>N </ID>
					<Priority>1</Priority>
					<Data>
						<xsl:value-of select="HotelRef/@HotelName" />
					</Data>
				</SlotID>
			</xsl:if>
			<xsl:if test="HotelRef/@ChainCode !=''">
				<SlotID>
					<ID>Z </ID>
					<Priority>1</Priority>
					<Data>
						<xsl:value-of select="HotelRef/@ChainCode" />
					</Data>
				</SlotID>
			</xsl:if>
		</SlotIDAry>
	</xsl:template>
	
</xsl:stylesheet>