<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehAvailRateRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_VehAvailRateRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_VehAvailRateRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_VehAvailRateRS">
		<OTA_VehAvailRateRS>
			<xsl:attribute name="Version">1.001</xsl:attribute>
			<xsl:choose>
				<xsl:when test="Errors/Error != ''">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="Errors/Error/@Code != ''">
										<xsl:value-of select="Errors/Error/@Code" />
									</xsl:when>
									<xsl:otherwise>E</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:value-of select="Errors/Error" />
						</Error>
					</Errors>
				</xsl:when>
				<xsl:when test="not(VehAvailRSCore/VehRentalCore) and not(Errors/Error)">
					<Errors>
						<Error>
							<xsl:attribute name="Type">Sabre</xsl:attribute>
							<xsl:attribute name="Code">E</xsl:attribute>
							<xsl:text>INVALID INPUT FILE</xsl:text>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success></Success>
					<xsl:apply-templates select="VehAvailRSCore" />
				</xsl:otherwise>
			</xsl:choose>
		</OTA_VehAvailRateRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="VehAvailRSCore">
		<VehAvailRSCore>
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
			<VehVendorAvails>
				<xsl:apply-templates select="VehVendorAvails/VehVendorAvail" />
			</VehVendorAvails>
		</VehAvailRSCore>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="VehVendorAvail">
		<VehVendorAvail>
			<Vendor>
				<xsl:attribute name="Code">
					<xsl:value-of select="Vendor/@Code" />
				</xsl:attribute>
				<xsl:value-of select="Vendor" />
			</Vendor>
			<VehAvails>
				<xsl:apply-templates select="VehAvails/VehAvail" />
			</VehAvails>
			<xsl:if test="VehAvails/VehAvail/VehAvailCore/TPA_Extensions/Rules/HeaderInfo/Header/Item != ''">
				<Info>
					<VendorMessages>
						<VendorMessage InfoType="5">
							<SubSection>
								<Paragraph>
									<Text>
										<xsl:value-of select="VehAvails/VehAvail/VehAvailCore/TPA_Extensions/Rules/HeaderInfo/Header/Item" />
									</Text>
								</Paragraph>
							</SubSection>
						</VendorMessage>
					</VendorMessages>
				</Info>
			</xsl:if>
		</VehVendorAvail>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="VehAvail">
		<VehAvail>
			<VehAvailCore>
				<xsl:attribute name="Status">Available</xsl:attribute>
				<Vehicle>
					<xsl:attribute name="AirConditionInd"><xsl:value-of select="VehAvailCore/Vehicle/@AirConditionInd"/></xsl:attribute>
					<xsl:attribute name="TransmissionType">
						<xsl:value-of select="VehAvailCore/Vehicle/@TransmissionType" />
					</xsl:attribute>
					<VehType>
						<xsl:attribute name="VehicleCategory">
							<xsl:value-of select="VehAvailCore/Vehicle/VehType/@VehicleCategory" />
						</xsl:attribute>
					</VehType>
				</Vehicle>
				<RentalRate>
					<RateDistance>
						<xsl:attribute name="DistUnitName">
							<xsl:value-of select="VehAvailCore/RentalRate/RateDistance/@DistUnitName" />
						</xsl:attribute>
						<xsl:attribute name="Unlimited"><xsl:value-of select="VehAvailCore/RentalRate/RateDistance/@Unlimited"/></xsl:attribute>
						<xsl:attribute name="VehiclePeriodUnitName">
							<xsl:value-of select="VehAvailCore/RentalRate/RateDistance/@VehiclePeriodUnitName" />
						</xsl:attribute>
					</RateDistance>
					<VehicleCharges>
						<VehicleCharge>
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@Amount,'.','')" />
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:value-of select="VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@CurrencyCode" />
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@DecimalPlaces" />
							</xsl:attribute>
							<xsl:attribute name="GuaranteedInd"><xsl:value-of select="VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@GuaranteedInd"/></xsl:attribute>
							<xsl:attribute name="Purpose">
								<xsl:value-of select="VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@Purpose" />
							</xsl:attribute>
							<xsl:attribute name="TaxInclusive"><xsl:value-of select="VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@TaxInclusive"/>
</xsl:attribute>
							<xsl:if test="VehAvailCore/TPA_Extensions/Rules/DetailPriceCalculation/Item/TaxFeeValue != ''">
								<TaxAmounts>
									<xsl:apply-templates select="VehAvailCore/TPA_Extensions/Rules/DetailPriceCalculation/Item" mode="tax"/>
								</TaxAmounts>
							</xsl:if>
						</VehicleCharge>
					</VehicleCharges>
					<RateQualifier>
						<xsl:attribute name="RateQualifier">
							<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/RateCodeInfo/Item[1]/Text" />
						</xsl:attribute>
						<xsl:attribute name="RatePeriod">
							<xsl:value-of select="VehAvailCore/RentalRate/RateQualifier/@RatePeriod" />
						</xsl:attribute>
					</RateQualifier>
				</RentalRate>
				<xsl:if test="VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/CarTotalPrice != ''">
					<TotalCharge>
						<xsl:attribute name="RateTotalAmount">
							<xsl:value-of select="translate(VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/CarTotalPrice,'.','')" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="VehAvailCore/RentalRate/VehicleCharges/VehicleCharge/@CurrencyCode" />
						</xsl:attribute>
					</TotalCharge>
				</xsl:if>
				<xsl:if test="VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/ExtraDayRate != '' or VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/ExtraHourRate != ''">
					<Fees>
						<xsl:if test="VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/ExtraDayRate != ''">
							<Fee>
								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/ExtraDayRate,'.','')" />
								</xsl:attribute>
								<xsl:attribute name="TaxInclusive">0</xsl:attribute>
								<xsl:attribute name="Description">Additional Day Rate</xsl:attribute>
								<xsl:attribute name="Purpose">10</xsl:attribute>
							</Fee>
						</xsl:if>
						<xsl:if test="VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/ExtraHourRate != ''">
							<Fee>
								<xsl:attribute name="Amount">
									<xsl:value-of select="translate(VehAvailCore/TPA_Extensions/Rules/RateInfo/Item/ExtraHourRate,'.','')" />
								</xsl:attribute>
								<xsl:attribute name="TaxInclusive">0</xsl:attribute>
								<xsl:attribute name="Description">Additional Hour Rate</xsl:attribute>
								<xsl:attribute name="Purpose">11</xsl:attribute>
							</Fee>
						</xsl:if>
					</Fees>
				</xsl:if>
				<!--TPA_Extensions>
								<Rules>
									<MiscRateInfo>
										<xsl:apply-templates select="VehAvailCore/TPA_Extensions/Rules/MiscRateInfo/RuleText/Item" mode="Rule"/>														</MiscRateInfo>
									<BasicRulesInfo>
										<OvernightRequiredIndicator>
											<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/BasicRulesInfo/OvernightRequiredIndicator"/>															</OvernightRequiredIndicator>
										<MaximumRentalPeriod>
											<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/BasicRulesInfo/MaximumRentalPeriod"/>																</MaximumRentalPeriod>										
										<ReturnByTime>
											<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/BasicRulesInfo/ReturnByTime"/>	
										</ReturnByTime>
										<PickupAfterTime>
											<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/BasicRulesInfo/PickupAfterTime"/>																</PickupAfterTime>
										<MinimumRentalDays>
											<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/BasicRulesInfo/MinimumRentalDays"/>																</MinimumRentalDays>
										<PickupByTime>
											<xsl:value-of select="VehAvailCore/TPA_Extensions/Rules/BasicRulesInfo/PickupByTime"/>	
										</PickupByTime>
									</BasicRulesInfo>
								</Rules>
							</TPA_Extensions-->
			</VehAvailCore>
		</VehAvail>
	</xsl:template>
	<!-- **************************************************************** -->
	<xsl:template match="Item" mode="Rule">
		<RuleText>
			<Item>
				<xsl:value-of select="." />
			</Item>
		</RuleText>
	</xsl:template>
	<!-- **************************************************************** -->
	<xsl:template match="Item" mode="tax">
		<xsl:param name="cur" />
		<TaxAmount>
			<xsl:attribute name="Total">
				<xsl:value-of select="translate(TaxFeeValue,'.','')" />
			</xsl:attribute>
			<xsl:attribute name="Description">
				<xsl:value-of select="translate(TaxFeeText,'','')" />
			</xsl:attribute>
		</TaxAmount>
	</xsl:template>
	<!-- **************************************************************** -->
</xsl:stylesheet>
