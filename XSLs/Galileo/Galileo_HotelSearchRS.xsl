<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="HotelIndex_7_0" />
		<xsl:apply-templates select="HotelReferencePoint_5_0" />
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="HotelIndex_7_0">
		<OTA_HotelSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="HotelDescHeader/ResponseInd='E'">
					<Errors>
						<xsl:apply-templates select="HotelDescHeader/ErrQual" mode="desc" />
					</Errors>
				</xsl:when>
				<xsl:when test="HtlIndex/ResponseInd='E'">
					<Errors>
						<xsl:apply-templates select="HtlIndex/ErrQual" mode="index" />
					</Errors>
				</xsl:when>
				<xsl:when test="HtlIndex/ResponseInd='S'">
					<Success />
					<Warnings>
						<Warning Type="Traveltalk">REF POINT NOT FOUND - SIMILAR NAMES EXIST</Warning>
					</Warnings>
					<Areas>
						<xsl:apply-templates select="SimilarNames/SmlrNameAry/SmlrName" />
					</Areas>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<Properties>
						<xsl:apply-templates select="HtlIndexPropList/PropsRecordsAry/PropsRecords" />
					</Properties>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelSearchRS>
	</xsl:template>
	<xsl:template match="HotelReferencePoint_5_0">
		<OTA_HotelSearchRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="RefPtList/Type='E'">
					<Errors>
						<xsl:apply-templates select="RefPtList/ErrQual" mode="desc" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<Areas>
						<xsl:apply-templates select="RefPtListDetail/RefPtAry/RefPt" />
					</Areas>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelSearchRS>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropsRecords">
		<Property>
			<xsl:attribute name="ChainCode">
				<xsl:value-of select="Chain" />
			</xsl:attribute>
			<xsl:attribute name="HotelCode">
				<xsl:value-of select="RoomMasterID" />
			</xsl:attribute>
			<xsl:attribute name="HotelCityCode">
				<xsl:value-of select="../../../HtlIndex/PropListQual/City" />
			</xsl:attribute>
			<xsl:attribute name="HotelName">
				<xsl:value-of select="PropsName" />
			</xsl:attribute>
			<xsl:attribute name="HotelCodeContext">
				<xsl:choose>
					<xsl:when test="Locn= 'C'">City</xsl:when>
					<xsl:when test="Locn= 'A'">Airport</xsl:when>
					<xsl:when test="Locn= 'R'">Resort</xsl:when>
					<xsl:when test="Locn= 'S'">Suburban</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<Address>
				<AddressLine>
					<xsl:value-of select="ShortAddr" />
				</AddressLine>
				<xsl:if test="../../../HtlIndex/PropListQual/State !=''">
					<StateProv>
						<xsl:attribute name="StateCode">
							<xsl:value-of select="../../../HtlIndex/PropListQual/State" />
						</xsl:attribute>
					</StateProv>
				</xsl:if>
			</Address>
			<xsl:if test="AAARating !=''">
				<Award>
					<xsl:attribute name="Provider">AAA</xsl:attribute>
					<xsl:attribute name="Rating">
						<xsl:value-of select="AAARating" />
					</xsl:attribute>
				</Award>
			</xsl:if>
			<RelativePosition>
				<xsl:attribute name="Direction">
					<xsl:value-of select="Dir" />
				</xsl:attribute>
				<xsl:attribute name="Distance">
					<xsl:value-of select="Dist" />
				</xsl:attribute>
				<xsl:attribute name="DistanceUnitName">
					<xsl:choose>
						<xsl:when test="../../../HtlIndex/PropListQual/MileKiloSwitch = 'M'">Mile</xsl:when>
						<xsl:when test="../../../HtlIndex/PropListQual/MileKiloSwitch = 'K'">Km</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Name">
					<xsl:value-of select="../../../HtlIndex/PropListQual/RefPt" />
				</xsl:attribute>
				<xsl:if test="TransportationInd !=''">
					<Transportations>
						<Transportation>
							<xsl:attribute name="CodeDetail">
								<xsl:choose>
									<xsl:when test="TransportationInd = 'C'">Courtesy car</xsl:when>
									<xsl:when test="TransportationInd = 'L'">Limo</xsl:when>
									<xsl:when test="TransportationInd = 'O'">Check with hotel</xsl:when>
									<xsl:when test="TransportationInd = 'P'">Public transportation</xsl:when>
									<xsl:when test="TransportationInd = 'W'">Walking distance</xsl:when>
								</xsl:choose>
							</xsl:attribute>
						</Transportation>
					</Transportations>
				</xsl:if>
			</RelativePosition>
		</Property>
	</xsl:template>
	<xsl:template match="SmlrName">
		<Area>
			<AreaDescription>
				<Text>
					<xsl:value-of select="PropName" />
				</Text>
			</AreaDescription>
		</Area>
	</xsl:template>
	<xsl:template match="RefPt">
		<Area>
			<AreaDescription>
				<Text>
					<xsl:value-of select="Name" />
				</Text>
			</AreaDescription>
		</Area>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="ErrQual" mode="desc">
		<Error Type="Htl">
			<xsl:attribute name="Code">
				<xsl:value-of select="Err" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<xsl:template match="ErrQual" mode="index">
		<Error Type="Htl">
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrNum" />
			</xsl:attribute>
			<xsl:value-of select="ErrMsg" />
		</Error>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
</xsl:stylesheet>
