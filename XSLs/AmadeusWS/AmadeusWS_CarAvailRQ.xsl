<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CarAvailRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 25 Aug 2012 - Rastko - added discount parameters						-->
	<!-- Date: 29 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehAvailRateRQ"/>
	</xsl:template>
	<xsl:template match="OTA_VehAvailRateRQ">
		<xsl:choose>
			<xsl:when test="count(VehAvailRQCore/VendorPrefs/VendorPref)='1'">
				<Car_SingleAvailability>
					<xsl:apply-templates select="VehAvailRQCore" mode="single"/>
				</Car_SingleAvailability>
			</xsl:when>
			<xsl:otherwise>
				<Car_MultiAvailability>
					<xsl:apply-templates select="VehAvailRQCore" mode="multi"/>
				</Car_MultiAvailability>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="VehAvailRQCore" mode="single">
		<xsl:apply-templates select="VendorPrefs/VendorPref" mode="single"/>
		<pickupDropoffLocs>
			<xsl:apply-templates select="VehRentalCore/PickUpLocation"/>
		</pickupDropoffLocs>
		<pickupDropoffLocs>
			<xsl:apply-templates select="VehRentalCore/ReturnLocation"/>
		</pickupDropoffLocs>
		<xsl:apply-templates select="VehRentalCore"/>
		<xsl:if test="../POS/Source/@ISOCurrency!='' or TPA_Extensions/VehOptions/UnlimitedMilesInd='true'">
			<rateinfo>
				<xsl:if test="../POS/Source/@ISOCurrency!=''">
					<tariffInfo>
						<currency><xsl:value-of select="../POS/Source/@ISOCurrency"/></currency>
					</tariffInfo>
				</xsl:if>
				<xsl:if test="TPA_Extensions/VehOptions/UnlimitedMilesInd='true'">
					<chargeDetails>
						<type>UNL</type>
					</chargeDetails>
				</xsl:if>
			</rateinfo>
		</xsl:if>
		<xsl:if test="VehPrefs/VehPref/VehType/@VehicleCategory != ''">
			<xsl:apply-templates select="VehPrefs" mode="single"/>
		</xsl:if>
	</xsl:template>
	<xsl:template match="VehAvailRQCore" mode="multi">
		<xsl:apply-templates select="VendorPrefs/VendorPref" mode="multi"/>
		<pickupDropoffLocation>
			<xsl:apply-templates select="VehRentalCore/PickUpLocation"/>
		</pickupDropoffLocation>
		<pickupDropoffLocation>
			<xsl:apply-templates select="VehRentalCore/ReturnLocation"/>
		</pickupDropoffLocation>
		<xsl:apply-templates select="VehRentalCore"/>
		<xsl:if test="../POS/Source/@ISOCurrency!='' or TPA_Extensions/VehOptions/UnlimitedMilesInd='true'">
			<rateinfo>
				<xsl:if test="../POS/Source/@ISOCurrency!=''">
					<tariffInfo>
						<currency><xsl:value-of select="../POS/Source/@ISOCurrency"/></currency>
					</tariffInfo>
				</xsl:if>
				<xsl:if test="TPA_Extensions/VehOptions/UnlimitedMilesInd='true'">
					<chargeDetails>
						<type>UNL</type>
					</chargeDetails>
				</xsl:if>
			</rateinfo>
		</xsl:if>
		<xsl:if test="VehPrefs/VehPref/VehType/@VehicleCategory != ''">
			<xsl:apply-templates select="VehPrefs" mode="multi"/>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="VehPrefs" mode="single">
		<vehicleTypeInfo>
			<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
			<vehicleRentalNeedType>
				<vehicleTypeOwner>ACR</vehicleTypeOwner>
				<xsl:apply-templates select="VehPref"/>
			</vehicleRentalNeedType>
		</vehicleTypeInfo>
	</xsl:template>
	
	<xsl:template match="VehPrefs" mode="multi">
		<vehiculeType>
			<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
			<vehicleRentalNeedType>
				<vehicleTypeOwner>ACR</vehicleTypeOwner>
				<xsl:apply-templates select="VehPref"/>
			</vehicleRentalNeedType>
		</vehiculeType>
	</xsl:template>
	
	<xsl:template match="VehPref">
		<vehicleRentalPrefType>
			<xsl:value-of select="VehType/@VehicleCategory"/>
		</vehicleRentalPrefType>
	</xsl:template>
	<xsl:template match="VehRentalCore">
		<pickupDropoffTimes>
			<beginDateTime>
				<year>
					<xsl:value-of select="substring(@PickUpDateTime,1,4)"/>
				</year>
				<month>
					<xsl:value-of select="substring(@PickUpDateTime,6,2)"/>
				</month>
				<day>
					<xsl:value-of select="substring(@PickUpDateTime,9,2)"/>
				</day>
				<hour>
					<xsl:value-of select="substring(@PickUpDateTime,12,2)"/>
				</hour>
				<minutes>
					<xsl:value-of select="substring(@PickUpDateTime,15,2)"/>
				</minutes>
			</beginDateTime>
			<endDateTime>
				<year>
					<xsl:value-of select="substring(@ReturnDateTime,1,4)"/>
				</year>
				<month>
					<xsl:value-of select="substring(@ReturnDateTime,6,2)"/>
				</month>
				<day>
					<xsl:value-of select="substring(@ReturnDateTime,9,2)"/>
				</day>
				<hour>
					<xsl:value-of select="substring(@ReturnDateTime,12,2)"/>
				</hour>
				<minutes>
					<xsl:value-of select="substring(@ReturnDateTime,15,2)"/>
				</minutes>
			</endDateTime>
		</pickupDropoffTimes>
	</xsl:template>
	<xsl:template match="VendorPref" mode="single">
		<providerDetails>
			<companyDetails>
				<travelSector>CAR</travelSector>
				<companyCode>
					<xsl:value-of select="@Code"/>
				</companyCode>
			</companyDetails>
			<xsl:if test="../../RateQualifier/@CorpDiscountNmbr!='' or ../../RateQualifier/@PromotionCode!='' or ../../RateQualifier/@RateQualifier!=''">
				<customerInfo>
						<xsl:choose>
							<xsl:when test="../../RateQualifier/@CorpDiscountNmbr!='' and ../../RateQualifier/@PromotionCode!='' and ../../RateQualifier/@RateQualifier!=''">
								<customerReferences>
									<referenceQualifier>1</referenceQualifier>
									<referenceNumber><xsl:value-of select="../../RateQualifier/@RateQualifier"/></referenceNumber>
								</customerReferences>
								<otherCustomerRef>
									<referenceQualifier>CD</referenceQualifier>
									<referenceNumber><xsl:value-of select="../../RateQualifier/@CorpDiscountNmbr"/></referenceNumber>
								</otherCustomerRef>
								<otherCustomerRef>
									<referenceQualifier>PC</referenceQualifier>
									<referenceNumber><xsl:value-of select="../../RateQualifier/@PromotionCode"/></referenceNumber>
								</otherCustomerRef>
							</xsl:when>
							<xsl:when test="(../../RateQualifier/@CorpDiscountNmbr!='' and ../../RateQualifier/@PromotionCode!='') or (../../RateQualifier/@CorpDiscountNmbr!='' and ../../RateQualifier/@RateQualifier!='') or (../../RateQualifier/@PromotionCode!='' and ../../RateQualifier/@RateQualifier!='')">
								<xsl:choose>
									<xsl:when test="../../RateQualifier/@CorpDiscountNmbr!='' and ../../RateQualifier/@PromotionCode!=''">
										<customerReferences>
											<referenceQualifier>CD</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@CorpDiscountNmbr"/></referenceNumber>
										</customerReferences>
										<otherCustomerRef>
											<referenceQualifier>PC</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@PromotionCode"/></referenceNumber>
										</otherCustomerRef>
									</xsl:when>
									<xsl:when test="../../RateQualifier/@CorpDiscountNmbr!='' and ../../RateQualifier/@RateQualifier!=''">
										<customerReferences>
											<referenceQualifier>CD</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@CorpDiscountNmbr"/></referenceNumber>
										</customerReferences>
										<otherCustomerRef>
											<referenceQualifier>1</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@RateQualifier"/></referenceNumber>
										</otherCustomerRef>
									</xsl:when>
									<xsl:otherwise>
										<customerReferences>
											<referenceQualifier>PC</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@PromotionCode"/></referenceNumber>
										</customerReferences>
										<otherCustomerRef>
											<referenceQualifier>1</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@RateQualifier"/></referenceNumber>
										</otherCustomerRef>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="../../RateQualifier/@CorpDiscountNmbr!=''">
										<customerReferences>
											<referenceQualifier>CD</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@CorpDiscountNmbr"/></referenceNumber>
										</customerReferences>
									</xsl:when>
									<xsl:when test="../../RateQualifier/@PromotionCode!=''">
										<customerReferences>
											<referenceQualifier>PC</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@PromotionCode"/></referenceNumber>
										</customerReferences>
									</xsl:when>
									<xsl:otherwise>
										<customerReferences>
											<referenceQualifier>1</referenceQualifier>
											<referenceNumber><xsl:value-of select="../../RateQualifier/@RateQualifier"/></referenceNumber>
										</customerReferences>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
				</customerInfo>
			</xsl:if>
		</providerDetails>
	</xsl:template>
	<xsl:template match="VendorPref" mode="multi">
		<carCompany>
			<companyDetails>
				<travelSector>CAR</travelSector>
				<companyCode>
					<xsl:value-of select="@Code"/>
				</companyCode>
			</companyDetails>
		</carCompany>
	</xsl:template>
	<xsl:template match="PickUpLocation">
		<locationType>176</locationType>
		<locationDescription>
			<code>
				<xsl:text>1A</xsl:text>
			</code>
			<name>
				<xsl:value-of select="@LocationCode"/>
			</name>
		</locationDescription>
	</xsl:template>
	<xsl:template match="ReturnLocation">
		<locationType>DOL</locationType>
		<locationDescription>
			<code>
				<xsl:text>1A</xsl:text>
			</code>
			<name>
				<xsl:value-of select="@LocationCode"/>
			</name>
		</locationDescription>
	</xsl:template>
</xsl:stylesheet>
