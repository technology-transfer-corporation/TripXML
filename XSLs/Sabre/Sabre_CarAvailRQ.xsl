<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehAvailRateRQ" />
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="OTA_VehAvailRateRQ">
		<OTA_VehAvailRateRQ xmlns="http://www.opentravel.org/OTA/2002/11">
			<POS>
				<Source>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="POS/Source/@PseudoCityCode" />
					</xsl:attribute>
					<xsl:attribute name="ISOCountry">
						<xsl:choose>
							<xsl:when test="POS/Source/@ISOCountry != ''">
								<xsl:value-of select="POS/Source/@ISOCountry" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>US</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="ISOCurrency">
						<xsl:choose>
							<xsl:when test="POS/Source/@ISOCurrency != ''">
								<xsl:value-of select="POS/Source/@ISOCurrency" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>USD</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</Source>
			</POS>
			<VehAvailRQCore>
				<xsl:apply-templates select="VehAvailRQCore" />
			</VehAvailRQCore>
			<xsl:if test="VehAvailRQInfo/Customer/Primary/CustLoyalty/@MembershipID !=''">
				<VehAvailRQInfo>
					<Customer>
						<Primary>
							<xsl:apply-templates select="VehAvailRQInfo/Customer/Primary/CustLoyalty" />
						</Primary>
					</Customer>
				</VehAvailRQInfo>
			</xsl:if>
			<TPA_Extensions>
				<System>
					<xsl:value-of select="POS/TPA_Extensions/Provider/System" />
				</System>
				<PCC>
					<xsl:value-of select="POS/Source/@PseudoCityCode" />
				</PCC>
				<Userid>
					<xsl:value-of select="POS/TPA_Extensions/Provider/Userid" />
				</Userid>
				<Password>
					<xsl:value-of select="POS/TPA_Extensions/Provider/Password" />
				</Password>
				<SabreService>OTA_VehAvailRate</SabreService>
				<SabreAction>OTA_VehAvailRateRQ</SabreAction>
			</TPA_Extensions>
		</OTA_VehAvailRateRQ>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="VehAvailRQCore">
		<VehRentalCore>
			<xsl:attribute name="PickUpDateTime">
				<xsl:value-of select="VehRentalCore/@PickUpDateTime" />
			</xsl:attribute>
			<xsl:attribute name="ReturnDateTime">
				<xsl:value-of select="VehRentalCore/@ReturnDateTime" />
			</xsl:attribute>
			<PickUpLocation>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="VehRentalCore/PickUpLocation/@LocationCode" />
				</xsl:attribute>
			</PickUpLocation>
			<ReturnLocation>
				<xsl:attribute name="LocationCode">
					<xsl:value-of select="VehRentalCore/ReturnLocation/@LocationCode" />
				</xsl:attribute>
			</ReturnLocation>
		</VehRentalCore>
		<xsl:if test="VendorPrefs/VendorPref/@Code != ''">
			<VendorPrefs>
				<xsl:apply-templates select="VendorPrefs/VendorPref" />
			</VendorPrefs>
		</xsl:if>
		<VehiclePrefs>
			<xsl:choose>
				<xsl:when test="VehiclePrefs/VehiclePref/VehType/@VehicleCategory != ''">
					<xsl:apply-templates select="VehiclePrefs/VehiclePref" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>ECAR</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</VehiclePrefs>
		<xsl:if test="RateQualifier/@CorpDiscountNmbr != ''">
			<xsl:apply-templates select="RateQualifier" />
		</xsl:if>
		<xsl:if test="SpecialEquipPrefs/SpecialEquipPref/@EquipType != ''">
			<SpecialEquipPrefs>
				<xsl:apply-templates select="SpecialEquipPrefs/SpecialEquipPref" />
			</SpecialEquipPrefs>
		</xsl:if>
		<TPA_Extensions>
			<NumberOfCars>1</NumberOfCars>
		</TPA_Extensions>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="VendorPref">
		<VendorPref>
			<xsl:attribute name="Code">
				<xsl:value-of select="@Code" />
			</xsl:attribute>
		</VendorPref>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="VehiclePref">
		<VehiclePref>
			<xsl:attribute name="VehicleCategory">
				<xsl:value-of select="@VehicleCategory" />
			</xsl:attribute>
		</VehiclePref>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="RateQualifier">
		<RateQualifier>
			<xsl:attribute name="CorpDiscountNmbr">
				<xsl:value-of select="@CorpDiscountNmbr" />
			</xsl:attribute>
		</RateQualifier>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="SpecialEquipPref">
		<SpecialEquipPref>
			<xsl:attribute name="EquipType">
				<xsl:value-of select="@EquipType" />
			</xsl:attribute>
		</SpecialEquipPref>
	</xsl:template>
	<!--************************************************************************************************************	-->
	<xsl:template match="CustLoyalty">
		<CustLoyalty>
			<xsl:attribute name="MembershipID">
				<xsl:value-of select="@MembershipID" />
			</xsl:attribute>
		</CustLoyalty>
	</xsl:template>
	<!--************************************************************************************************************	-->
</xsl:stylesheet>