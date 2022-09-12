<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_HotelInfoRS.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 25 Feb 2006 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="HotelDescription_5_0" />
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="HotelDescription_5_0">
		<OTA_HotelDescriptiveInfoRS>
			<xsl:attribute name="Version">1.000</xsl:attribute>
			<xsl:choose>
				<xsl:when test="HotelDescHeader/ResponseInd='E'">
					<Errors>
						<xsl:apply-templates select="HotelDescHeader/ErrQual" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success />
					<HotelDescriptiveContents>
						<HotelDescriptiveContent LanguageCode="en-us">
							<xsl:apply-templates select="HotelDescHeader/PropKeywordQual |  HotelDescHeader/PropGenericQual | HotelDescHeader/ChainQual"
								mode="Contents" />
							<HotelInfo>
								<xsl:apply-templates select="HotelDescHeader/PropKeywordQual |  HotelDescHeader/PropGenericQual" mode="HotelInfo" />
								<xsl:if test="HotelDescKeywordText !=''">
									<Services>
										<Service>
											<Features>
												<xsl:apply-templates select="HotelDescKeywordText/TextItemAry/TextItem[LineType='K']" mode="K" />
											</Features>
										</Service>
									</Services>
								</xsl:if>
							</HotelInfo>
							<xsl:apply-templates select="HotelDescHeader/PropKeywordQual |  HotelDescHeader/PropGenericQual" mode="Features" />
							<!--Need to come back and do CheckInTime and CheckOutTIme -->
						</HotelDescriptiveContent>
					</HotelDescriptiveContents>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_HotelDescriptiveInfoRS>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="TextItem" mode="K">
		<Feature>
			<xsl:attribute name="CodeDetail">
				<xsl:value-of select="Text" />
			</xsl:attribute>
			<Description>
				<xsl:apply-templates select="../../../HotelDescKeywordText" mode="alltext" />
			</Description>
		</Feature>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="HotelDescKeywordText" mode="alltext">
		<!--xsl:attribute name="CodeDetail"><xsl:value-of select="TextItemAry/TextItem[LineType='K']/Text"/></xsl:attribute-->
		<xsl:choose>
			<xsl:when test="position()='1'">
				<xsl:apply-templates select="TextItemAry/TextItem[LineType='K']/following-sibling::TextItem" mode="NextLine" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="TextItemAry/TextItem" mode="NextLine" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="ErrQual">
		<Error Type="Htl">
			<xsl:attribute name="Code">
				<xsl:value-of select="Err" />
			</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropKeywordQual" mode="Features">
		<xsl:if test="AddrLineAry/AddrLine !=''">
			<ContactInfos>
				<ContactInfo>
					<Addresses>
						<Address>
							<AddressLine>
								<!--Come back and break this down into State ZiP etc.-->
								<xsl:apply-templates select="AddrLineAry/AddrLine" />
							</AddressLine>
						</Address>
					</Addresses>
					<Phones>
						<Phone>
							<xsl:attribute name="PhoneNumber">
								<xsl:value-of select="Phone" />
							</xsl:attribute>
							<xsl:attribute name="PhoneUseType">PHN</xsl:attribute>
						</Phone>
						<xsl:if test="FaxNum !=''">
							<Phone>
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="FaxNum" />
								</xsl:attribute>
								<xsl:attribute name="PhoneUseType">FAX</xsl:attribute>
							</Phone>
						</xsl:if>
					</Phones>
				</ContactInfo>
			</ContactInfos>
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropGenericQual" mode="Features">
		<xsl:if test="AddrLineAry/AddrLine !=''">
			<ContactInfos>
				<ContactInfo>
					<Addresses>
						<Address>
							<AddressLine>
								<xsl:apply-templates select="AddrLineAry/AddrLine" />
							</AddressLine>
						</Address>
					</Addresses>
					<Phones>
						<Phone>
							<xsl:attribute name="PhoneNumber">
								<xsl:value-of select="Phone" />
							</xsl:attribute>
							<xsl:attribute name="PhoneUseType">PHN</xsl:attribute>
						</Phone>
						<xsl:if test="FaxNum !=''">
							<Phone>
								<xsl:attribute name="PhoneNumber">
									<xsl:value-of select="FaxNum" />
								</xsl:attribute>
								<xsl:attribute name="PhoneUseType">FAX</xsl:attribute>
							</Phone>
						</xsl:if>
					</Phones>
				</ContactInfo>
			</ContactInfos>
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropKeywordQual" mode="HotelInfo">
		<xsl:if test="DirToAirp or DistToAirport != ''">
			<RelativePositions>
				<RelativePosition>
					<xsl:attribute name="Direction">
						<xsl:value-of select="DirToAirp" />
					</xsl:attribute>
					<xsl:attribute name="Distance">
						<xsl:value-of select="DistToAirp" />
						<xsl:value-of select="DistUnits" />
					</xsl:attribute>
					<xsl:attribute name="Name">Airport</xsl:attribute>
				</RelativePosition>
			</RelativePositions>
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropGenericQual" mode="HotelInfo">
		<xsl:if test="DirToAirp or DistToAirport != ''">
			<RelativePositions>
				<RelativePosition>
					<xsl:attribute name="Direction">
						<xsl:value-of select="DirToAirp" />
					</xsl:attribute>
					<xsl:attribute name="Distance">
						<xsl:value-of select="DistToAirp" />
						<xsl:value-of select="DistUnits" />
					</xsl:attribute>
					<xsl:attribute name="Name">Airport</xsl:attribute>
				</RelativePosition>
			</RelativePositions>
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="ChainQual" mode="Contents">
		<xsl:attribute name="ChainCode">
			<xsl:value-of select="Chain" />
		</xsl:attribute>
		<xsl:attribute name="ChainName">
			<xsl:value-of select="Name" />
		</xsl:attribute>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropKeywordQual" mode="Contents">
		<xsl:attribute name="ChainCode">
			<xsl:value-of select="Chain" />
		</xsl:attribute>
		<xsl:attribute name="HotelCode">
			<xsl:value-of select="PropID" />
		</xsl:attribute>
		<xsl:attribute name="HotelName">
			<xsl:value-of select="Name" />
		</xsl:attribute>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="PropGenericQual" mode="Contents">
		<xsl:attribute name="ChainCode">
			<xsl:value-of select="Chain" />
		</xsl:attribute>
		<xsl:attribute name="HotelCode">
			<xsl:value-of select="PropID" />
		</xsl:attribute>
		<xsl:attribute name="HotelName">
			<xsl:value-of select="Name" />
		</xsl:attribute>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="AddrLine">
		<xsl:value-of select="translate(string(.),'&#164;',' ')" />
		<xsl:if test="position()!=last()">
			<xsl:text>,</xsl:text>
		</xsl:if>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
	<xsl:template match="TextItem" mode="NextLine">
		<Text>
			<xsl:value-of select="translate(Text,'&#164;',' ')" />
		</Text>
	</xsl:template>
	<!-- ***********************************************************************************************************-->
</xsl:stylesheet>
