<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_CarInfoRS.xsl 																       -->
<!-- ================================================================== -->
<!-- Date: 25 Feb 2006 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CarDescription_5_0" />
	</xsl:template>
	<xsl:template match="CarDescription_5_0">
		<OTA_VehLocDetailRS Version="1.001">
			<xsl:if test="CarDesc/DataQual">
				<Success />
				<xsl:apply-templates select="CarDesc/DataQual/CityLocnQual" />
				<Vehicles>
					<xsl:apply-templates select="CarDescKeyword/TextAry/TextInfo[TextInd='0']/FormatedQual" />
				</Vehicles>
			</xsl:if>
			<xsl:if test="CarDesc/ErrQual">
				<Errors>
					<Error Type="Car">
						<xsl:attribute name="Code">
							<xsl:value-of select="CarDesc/ErrQual/ErrCode" />
						</xsl:attribute>
						<xsl:value-of select="CarDesc/ErrQual/Text" />
					</Error>
				</Errors>
			</xsl:if>
		</OTA_VehLocDetailRS>
	</xsl:template>
	<xsl:template match="CityLocnQual">
		<!--Vendor>
			<xsl:attribute name="Code">
				<xsl:value-of select="CarVendorCode"/>
			</xsl:attribute>	
		</Vendor-->
		<LocationDetail>
			<xsl:attribute name="AtAirport">
				<xsl:choose>
					<xsl:when test="LocnName='CITY'">
						<xsl:text>0</xsl:text>
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>1</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="LocnNum" />
			</xsl:attribute>
			<xsl:attribute name="Name">
				<xsl:value-of select="CityName" />
			</xsl:attribute>
			<Address>
				<StreetNmbr>
					<xsl:value-of select="LocnAddr" />
				</StreetNmbr>
				<CityName>
					<xsl:value-of select="CityName" />
				</CityName>
			</Address>
			<xsl:choose>
				<xsl:when test="contains(PhoneNum,'FAX')">
					<Telephone PhoneTechType="Phone">
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="substring-before(PhoneNum,'FAX')" />
						</xsl:attribute>
					</Telephone>
					<Telephone PhoneTechType="Fax">
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="substring-after(PhoneNum,'FAX')" />
						</xsl:attribute>
					</Telephone>
				</xsl:when>
				<xsl:otherwise>
					<Telephone PhoneTechType="Phone">
						<xsl:attribute name="PhoneNumber">
							<xsl:value-of select="PhoneNum" />
						</xsl:attribute>
					</Telephone>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:if test="BusinessHrs">
				<AdditionalInfo>
					<OperationSchedules>
						<OperationSchedule>
							<xsl:attribute name="Duration">
								<xsl:value-of select="BusinessHrs" />
							</xsl:attribute>
						</OperationSchedule>
					</OperationSchedules>
				</AdditionalInfo>
			</xsl:if>
		</LocationDetail>
	</xsl:template>
	<!-- <xsl:template match="TextInfo">
		<xsl:if test="TextInd=0">	
			<xsl:apply-templates select="FormatedQual"/>
		</xsl:if>
	</xsl:template> -->
	<xsl:template match="FormatedQual">
		<Vehicle>
			<xsl:attribute name="PassengerQuantity">
				<xsl:value-of select="format-number(PsgrCnt,'#')" />
			</xsl:attribute>
			<xsl:if test="BagCnt != ''">
				<xsl:attribute name="BaggageQuantity">
					<xsl:value-of select="format-number(BagCnt,'#')" />
				</xsl:attribute>
			</xsl:if>
			<VehType>
				<xsl:attribute name="VehicleCategory">
					<xsl:value-of select="CarType" />
				</xsl:attribute>
				<xsl:if test="DoorCount != ''">
					<xsl:attribute name="DoorCount">
						<xsl:value-of select="format-number(DoorCnt,'#')" />
					</xsl:attribute>
				</xsl:if>
			</VehType>
			<xsl:if test="Class !=''">
				<VehClass>
					<xsl:attribute name="Size">
						<xsl:value-of select="Class" />
					</xsl:attribute>
				</VehClass>
			</xsl:if>
			<VehMakeModel Name="{MakeModel}" />
		</Vehicle>
	</xsl:template>
</xsl:stylesheet>