<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_CarRulesRS.xsl														  -->
<!-- ================================================================== -->
<!-- Date: 28 Sep 2012 - Rastko - corrected mapping issues						  -->
<!-- Date: 29 Jun 2009 - Rastko														  -->
<!-- ================================================================== -->
<xsl:output omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="Car_RateInformationFromAvailabilityReply"/>
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_VehRateRuleRS>
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_VehRateRuleRS>
</xsl:template>
	
<xsl:template match="CAPI_Messages">
	<Error>
		<xsl:attribute name="Code">
			<xsl:value-of select="ErrorCode" />
		</xsl:attribute>
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:value-of select="Text" />
	</Error>
</xsl:template>

<xsl:template match="Car_RateInformationFromAvailabilityReply">
	<OTA_VehRateRuleRS>
		<xsl:choose>
			<xsl:when test="rateDetails">
				<Success/>
				<xsl:if test="errorWarning">
					<Warnings>
						<xsl:apply-templates select="errorWarning" mode="warning"/>
					</Warnings>
				</xsl:if>
				<xsl:apply-templates select="rateDetails"/>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<xsl:apply-templates select="errorWarning" mode="error"/>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>			
	</OTA_VehRateRuleRS>		
</xsl:template>	

<xsl:template match="errorWarning" mode="warning">
	<Warning Type="Amadeus">
		<xsl:attribute name="Code"><xsl:value-of select="applicationError/errorDetails/errorCode"/></xsl:attribute>
		<xsl:value-of select="errorFreeText/freeText"/>
	</Warning>	
</xsl:template>

<xsl:template match="errorWarning" mode="error">
	<Error Type="Amadeus">
		<xsl:value-of select="errorFreeText/freeText"/>
	</Error>
</xsl:template>

<xsl:template match="rateDetails">
	<VehRentalCore>
		<xsl:attribute name="CompanyShortName"><xsl:value-of select="companyIdentification/companyName"/></xsl:attribute>
		<xsl:attribute name="Code"><xsl:value-of select="companyIdentification/companyCode"/></xsl:attribute>
		<xsl:attribute name="CodeContext"><xsl:value-of select="companyIdentification/accessLevel"/></xsl:attribute>
	</VehRentalCore>
	<xsl:apply-templates select="vehicleInfoGroup"/>
	<xsl:apply-templates select="rateDetail[tariffInfo/amountType='RP']" mode="base"/>
	<xsl:apply-templates select="rateDetail[tariffInfo/amountType='904']" mode="total"/>
	<xsl:if test="otherRulesGroup">
		<RateRules>
			<xsl:apply-templates select="otherRulesGroup"/>
		</RateRules>
	</xsl:if>
	<xsl:if test="taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/type='013' and taxSurchargeCoverageInfo/chargeDetails/amount !='']">
		<PricedEquips>
			<xsl:for-each select="taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/type='013' and taxSurchargeCoverageInfo/chargeDetails/amount !='']">
				<PricedEquip>
					<Equipment>
						<xsl:attribute name="EquipType"><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/comment"/></xsl:attribute>
					</Equipment>
					<Charge>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(taxSurchargeCoverageInfo/chargeDetails/amount,'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="taxSurchargeCoverageInfo/tariffInfo/currency"/>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:choose>
								<xsl:when test="contains(taxSurchargeCoverageInfo/chargeDetails/amount,'.')">2</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="IncludedInRate">
							<xsl:choose>
								<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
									<xsl:value-of select="'false'"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="Description">
							<xsl:choose>
								<xsl:when test="additionalInfo/freeText!=''"><xsl:value-of select="additionalInfo/freeText"/></xsl:when>
								<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/comment!=''"><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/comment"/></xsl:when>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
							<Calculation>
								<xsl:attribute name="UnitName">
									<xsl:choose>
										<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
										<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
										<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
										<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
										<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
										<xsl:otherwise><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Calculation>
						</xsl:if>
					</Charge>
				</PricedEquip>
			</xsl:for-each>
		</PricedEquips>
	</xsl:if>
	<xsl:if test="taxCovSurchargeGroup[(taxSurchargeCoverageInfo/chargeDetails/type='108' or taxSurchargeCoverageInfo/chargeDetails/type='045') and taxSurchargeCoverageInfo/chargeDetails/amount !='']">
		<Fees>
			<xsl:for-each select="taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/type='108' and taxSurchargeCoverageInfo/chargeDetails/amount !='']">
				<Fee>
					<xsl:choose>
						<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='012'">
							<xsl:attribute name="Percent">
								<xsl:value-of select="translate(taxSurchargeCoverageInfo/chargeDetails/amount,'.','')"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(taxSurchargeCoverageInfo/chargeDetails/amount,'.','')"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="taxSurchargeCoverageInfo/tariffInfo/currency"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="contains(taxSurchargeCoverageInfo/chargeDetails/amount,'.')">2</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="IncludedInRate">
						<xsl:choose>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
								<xsl:value-of select="'false'"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Description">
						<xsl:choose>
							<xsl:when test="additionalInfo/freeText!=''"><xsl:value-of select="additionalInfo/freeText"/></xsl:when>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/comment!=''"><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/comment"/></xsl:when>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Purpose">Surcharge</xsl:attribute>
					<xsl:if test="taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
						<Calculation>
							<xsl:attribute name="UnitName">
								<xsl:choose>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
									<xsl:otherwise><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</Calculation>
					</xsl:if>
				</Fee>			
			</xsl:for-each>
			<xsl:for-each select="taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/type='045' and taxSurchargeCoverageInfo/chargeDetails/amount !='']">
				<Fee>
					<xsl:choose>
						<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='012'">
							<xsl:attribute name="Percent">
								<xsl:value-of select="translate(taxSurchargeCoverageInfo/chargeDetails/amount,'.','')"/>
							</xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="Amount">
								<xsl:value-of select="translate(taxSurchargeCoverageInfo/chargeDetails/amount,'.','')"/>
							</xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="taxSurchargeCoverageInfo/tariffInfo/currency"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="contains(taxSurchargeCoverageInfo/chargeDetails/amount,'.')">2</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="IncludedInRate">
						<xsl:choose>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
								<xsl:value-of select="'false'"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Description">
						<xsl:choose>
							<xsl:when test="additionalInfo/freeText!=''"><xsl:value-of select="additionalInfo/freeText"/></xsl:when>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/comment!=''"><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/comment"/></xsl:when>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Purpose">Tax</xsl:attribute>
					<xsl:if test="taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
						<Calculation>
							<xsl:attribute name="UnitName">
								<xsl:choose>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
									<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
									<xsl:otherwise><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</Calculation>
					</xsl:if>
				</Fee>			
			</xsl:for-each>
		</Fees>
	</xsl:if>
	<xsl:if test="taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/type='COV' and taxSurchargeCoverageInfo/chargeDetails/amount != '']">
		<PricedCoverages>
			<xsl:apply-templates select="taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/type='COV' and taxSurchargeCoverageInfo/chargeDetails/amount !='']"/>
		</PricedCoverages>
	</xsl:if>
	<xsl:apply-templates select="pickupDropoffLocation"/>
	<xsl:if test="mktText">
		<VendorMessages>
			<xsl:apply-templates select="mktText"/>
		</VendorMessages>
	</xsl:if>
</xsl:template>

<xsl:template match="vehicleInfoGroup">
	<Vehicle>
		<xsl:attribute name="VendorCarType"><xsl:value-of select="vehicleDetails/vehicleCharacteristic/vehicleRentalPrefType"/></xsl:attribute>
		<xsl:apply-templates select="vehicleDetails/carModel"/>
		<xsl:if test="additionalInfo/freeText">
			<Description>
				<xsl:apply-templates select="additionalInfo/freeText" mode="VehInfo"/>
			</Description>
		</xsl:if>
	</Vehicle>
</xsl:template>

<xsl:template match="vehicleInfo[qualifier='NOD']">
	<xsl:attribute name="DoorCount"><xsl:value-of select="value"/></xsl:attribute>
</xsl:template>

<xsl:template match="carModel">
	<VehMakeModel>
		<xsl:attribute name="Name"><xsl:value-of select="."/></xsl:attribute>
	</VehMakeModel>
</xsl:template>

<xsl:template match="freeText" mode="VehInfo">
	<xsl:value-of select="."/>
</xsl:template>

<xsl:template match="rateDetail" mode="base">
	<RentalRate>
		<RateDistance>
			<xsl:attribute name="Unlimited">
				<xsl:choose>
					<xsl:when test="chargeDetails[type='034' or type='033']"><xsl:value-of select="'true'"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="'false'"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</RateDistance>
		<VehicleCharges>
			<VehicleCharge>
				<xsl:attribute name="Amount"><xsl:value-of select="translate(tariffInfo/amount,'.','')"/></xsl:attribute>
				<xsl:attribute name="CurrencyCode"><xsl:value-of select="tariffInfo/currency"/></xsl:attribute>
				<xsl:attribute name="DecimalPlaces">
					<xsl:choose>
						<xsl:when test="contains(tariffInfo/amount,'.')">2</xsl:when>
						<xsl:otherwise>0</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Purpose">Base</xsl:attribute>
				<xsl:choose>
					<xsl:when test="tariffInfo/rateChangeIndicator='GUA'">
						<xsl:attribute name="GuaranteedInd"><xsl:value-of select="('true')"/></xsl:attribute>
					</xsl:when>
					<xsl:when test="tariffInfo/rateChangeIndicator='QUO'">
						<xsl:attribute name="GuaranteedInd"><xsl:value-of select="('false')"/></xsl:attribute>
					</xsl:when>
				</xsl:choose>
				<Calculation>
					<xsl:attribute name="UnitName">
						<xsl:choose>
							<xsl:when test="tariffInfo/ratePlanIndicator='DY'">Per day</xsl:when>
							<xsl:when test="tariffInfo/ratePlanIndicator='WY'">Per week</xsl:when>
							<xsl:when test="tariffInfo/ratePlanIndicator='MY'">Per month</xsl:when>
							<xsl:when test="tariffInfo/ratePlanIndicator='3'">Per rental</xsl:when>
							<xsl:otherwise><xsl:value-of select="tariffInfo/ratePlanIndicator"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</Calculation>
			</VehicleCharge>			
		</VehicleCharges>
		<RateQualifier>
			<xsl:attribute name="RateQualifier"><xsl:value-of select="../rateCodeGroup/rateCode/fareCategories/fareType"/></xsl:attribute>
			<xsl:if test="../rateCodeGroup/additionalInfo/freeText!=''">
				<PromoDesc><xsl:value-of select="../rateCodeGroup/additionalInfo/freeText"/></PromoDesc>
			</xsl:if>
			<xsl:apply-templates select="../remarks"/>
		</RateQualifier>
	</RentalRate>
	<xsl:for-each select="chargeDetails[type='108']">
		<xsl:variable name="src"><xsl:value-of select="comment"/></xsl:variable>
		<xsl:if test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType!='012'">
			<RentalRate>
				<VehicleCharges>
					<VehicleCharge>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="amount!=''"><xsl:value-of select="translate(amount,'.','')"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="../tariffInfo/currency"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:choose>
								<xsl:when test="contains(amount,'.')">2</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="IncludedInRate">
							<xsl:choose>
								<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
									<xsl:value-of select="'false'"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="Description"><xsl:value-of select="comment"/></xsl:attribute>
						<xsl:attribute name="Purpose">Surcharge</xsl:attribute>
						<xsl:if test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
							<Calculation>
								<xsl:attribute name="UnitName">
									<xsl:choose>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
										<xsl:otherwise><xsl:value-of select="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Calculation>
						</xsl:if>
					</VehicleCharge>
				</VehicleCharges>
			</RentalRate>
		</xsl:if>		
	</xsl:for-each>
	<xsl:for-each select="chargeDetails[type='108']">
		<xsl:variable name="src"><xsl:value-of select="comment"/></xsl:variable>
		<xsl:if test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='012'">
			<RentalRate>
				<VehicleCharges>
					<VehicleCharge>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="amount!=''"><xsl:value-of select="translate(amount,'.','')"/></xsl:when>
								<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="../tariffInfo/currency"/></xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:choose>
								<xsl:when test="contains(amount,'.')">2</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="IncludedInRate">
							<xsl:choose>
								<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
									<xsl:value-of select="'false'"/>
								</xsl:when>
								<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="Description"><xsl:value-of select="comment"/></xsl:attribute>
						<xsl:attribute name="Purpose">Surcharge</xsl:attribute>
						<xsl:if test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
							<Calculation>
								<xsl:attribute name="UnitName">
									<xsl:choose>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
										<xsl:otherwise><xsl:value-of select="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Calculation>
						</xsl:if>
					</VehicleCharge>
				</VehicleCharges>
			</RentalRate>
		</xsl:if>	
	</xsl:for-each>
	<xsl:for-each select="chargeDetails[type='COV']">
		<xsl:variable name="src"><xsl:value-of select="comment"/></xsl:variable>
		<RentalRate>
			<VehicleCharges>
				<VehicleCharge>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="amount!=''"><xsl:value-of select="translate(amount,'.','')"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="../tariffInfo/currency"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="contains(amount,'.')">2</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="IncludedInRate">
						<xsl:choose>
							<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
								<xsl:value-of select="'false'"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Description"><xsl:value-of select="comment"/></xsl:attribute>
					<xsl:attribute name="Purpose">Coverage</xsl:attribute>
					<xsl:if test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
							<Calculation>
								<xsl:attribute name="UnitName">
									<xsl:choose>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
										<xsl:otherwise><xsl:value-of select="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Calculation>
						</xsl:if>
				</VehicleCharge>
			</VehicleCharges>
		</RentalRate>
	</xsl:for-each>
	<xsl:for-each select="chargeDetails[type='045']">
		<xsl:variable name="src"><xsl:value-of select="comment"/></xsl:variable>
		<RentalRate>
			<VehicleCharges>
				<VehicleCharge>
					<xsl:attribute name="Amount">
						<xsl:choose>
							<xsl:when test="amount!=''"><xsl:value-of select="translate(amount,'.','')"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="'0'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="../tariffInfo/currency"/>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="contains(amount,'.')">2</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="IncludedInRate">
						<xsl:choose>
							<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
								<xsl:value-of select="'false'"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Description"><xsl:value-of select="comment"/></xsl:attribute>
					<xsl:attribute name="Purpose">Tax</xsl:attribute>
					<xsl:if test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
							<Calculation>
								<xsl:attribute name="UnitName">
									<xsl:choose>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
										<xsl:when test="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
										<xsl:otherwise><xsl:value-of select="../../taxCovSurchargeGroup[taxSurchargeCoverageInfo/chargeDetails/comment=$src]/taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
							</Calculation>
						</xsl:if>
				</VehicleCharge>
			</VehicleCharges>
		</RentalRate>			
	</xsl:for-each>
	<xsl:if test="chargeDetails[type='008']">
		<RentalRate>
			<RateDistance>
				<xsl:choose>
					<xsl:when test="chargeDetails[type='79']">
						<xsl:attribute name="Unlimited">
							<xsl:choose>
								<xsl:when test="chargeDetails[type='79']/description='UNL'">true</xsl:when>
								<xsl:otherwise>false</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="chargeDetails[type='79']/amount">
							<xsl:attribute name="Quantity"><xsl:value-of select="chargeDetails[type='79']/amount"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="DistUnitName">Mile</xsl:attribute>
					</xsl:when>
					<xsl:when test="chargeDetails[type='80']">
						<xsl:attribute name="Unlimited">
							<xsl:choose>
								<xsl:when test="chargeDetails[type='80']/description='UNL'">true</xsl:when>
								<xsl:otherwise>false</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="chargeDetails[type='80']/amount">
							<xsl:attribute name="Quantity"><xsl:value-of select="chargeDetails[type='80']/amount"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="DistUnitName">Km</xsl:attribute>
					</xsl:when>
				</xsl:choose>
			</RateDistance>
			<VehicleCharges>
				<VehicleCharge>
					<xsl:attribute name="Amount"><xsl:value-of select="translate(chargeDetails[type='008']/amount,'.','')"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="tariffInfo/currency"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="contains(chargeDetails[type='008']/amount,'.')">2</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="IncludedInRate"><xsl:value-of select="'false'"/></xsl:attribute>
					<xsl:attribute name="Description">EXTRA DAY</xsl:attribute>
					<xsl:attribute name="Purpose">Extra</xsl:attribute>
				</VehicleCharge>			
			</VehicleCharges>
		</RentalRate>
	</xsl:if>
	<xsl:if test="chargeDetails[type='009']">
		<RentalRate>
			<RateDistance>
				<xsl:choose>
					<xsl:when test="chargeDetails[type='81']">
						<xsl:attribute name="Unlimited">
							<xsl:choose>
								<xsl:when test="chargeDetails[type='081']/description='UNL'">true</xsl:when>
								<xsl:otherwise>false</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="chargeDetails[type='81']/amount">
							<xsl:attribute name="Quantity"><xsl:value-of select="chargeDetails[type='81']/amount"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="DistUnitName">Mile</xsl:attribute>
					</xsl:when>
					<xsl:when test="chargeDetails[type='82']">
						<xsl:attribute name="Unlimited">
							<xsl:choose>
								<xsl:when test="chargeDetails[type='82']/description='UNL'">true</xsl:when>
								<xsl:otherwise>false</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:if test="chargeDetails[type='82']/amount">
							<xsl:attribute name="Quantity"><xsl:value-of select="chargeDetails[type='82']/amount"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="DistUnitName">Km</xsl:attribute>
					</xsl:when>
				</xsl:choose>
			</RateDistance>
			<VehicleCharges>
				<VehicleCharge>
					<xsl:attribute name="Amount"><xsl:value-of select="translate(chargeDetails[type='009']/amount,'.','')"/></xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="tariffInfo/currency"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:choose>
							<xsl:when test="contains(chargeDetails[type='008']/amount,'.')">2</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="IncludedInRate"><xsl:value-of select="'false'"/></xsl:attribute>
					<xsl:attribute name="Description">EXTRA HOUR</xsl:attribute>
					<xsl:attribute name="Purpose">Extra</xsl:attribute>
				</VehicleCharge>			
			</VehicleCharges>
		</RentalRate>
	</xsl:if>
</xsl:template>

<xsl:template match="rateDetail" mode="total">
	<TotalCharge>
		<xsl:attribute name="RateTotalAmount"><xsl:value-of select="translate(tariffInfo/amount,'.','')"/></xsl:attribute>
		<xsl:attribute name="CurrencyCode"><xsl:value-of select="tariffInfo/currency"/></xsl:attribute>
		<xsl:attribute name="DecimalPlaces">
			<xsl:choose>
				<xsl:when test="contains(tariffInfo/amount,'.')">2</xsl:when>
				<xsl:otherwise>0</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="Purpose">Total</xsl:attribute>
	</TotalCharge>
</xsl:template>

<xsl:template match="chargeDetails">
	<TaxAmount>
		<xsl:attribute name="Amount"><xsl:value-of select="amount"/></xsl:attribute>
		<xsl:attribute name="TaxCode"><xsl:value-of select="type"/></xsl:attribute>
		<xsl:attribute name="Description"><xsl:value-of select="concat(description, '-', comment)"/></xsl:attribute>		
	</TaxAmount>
</xsl:template>

<xsl:template match="remarks">
	<RateComments>
		<xsl:apply-templates select="freeText" mode="comments"/>
		<xsl:apply-templates select="../rateCodeGroup/additionalInfo/freeText" mode="comments"/>
	</RateComments>
</xsl:template>

<xsl:template match="freeText" mode="comments">
	<RateComment>
		<xsl:value-of select="."/>
	</RateComment>
</xsl:template>

<xsl:template match="otherRulesGroup">
	<xsl:choose>
		<xsl:when test="otherRules/ruleDetails/type=('ADB')">
			<AdvanceBooking>
				<xsl:attribute name="RequiredInd"><xsl:value-of select="('true')"/></xsl:attribute>
			</AdvanceBooking>		
		</xsl:when>
		<xsl:when test="otherRules/ruleDetails/type=('GUA')">
			<RateGuarantee>
				<Description>
					<xsl:value-of select="otherRules/ruleText/freeText"/>
				</Description>
			</RateGuarantee>		
		</xsl:when>
		<xsl:when test="otherRules/ruleDetails/type=('DEP')">
			<RateDeposit>
				<xsl:attribute name="DepositRequiredInd"><xsl:value-of select="('true')"/></xsl:attribute>
			</RateDeposit>		
		</xsl:when>
	</xsl:choose>
</xsl:template>

<xsl:template match="taxCovSurchargeGroup">
	<PricedCoverage>
		<xsl:attribute name="Required">
			<xsl:choose>
				<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/description='OPT'">false</xsl:when>
				<xsl:otherwise>true</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<Coverage>
			<xsl:attribute name="CoverageType">
				<xsl:choose>
					<xsl:when test="contains(taxSurchargeCoverageInfo/chargeDetails/comment,' - ')">
						<xsl:value-of select="substring-before(taxSurchargeCoverageInfo/chargeDetails/comment,' -')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/comment"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<Details>
				<xsl:attribute name="CoverageTextType">Supplement</xsl:attribute>
			</Details>
		</Coverage>
		<Charge>
			<xsl:attribute name="Amount"><xsl:value-of select="translate(taxSurchargeCoverageInfo/chargeDetails/amount,'.','')"/></xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/currency"/></xsl:attribute>
			<xsl:attribute name="DecimalPlaces">
				<xsl:choose>
					<xsl:when test="contains(taxSurchargeCoverageInfo/chargeDetails/amount,'.')">2</xsl:when>
					<xsl:otherwise>0</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="IncludedInRate">
				<xsl:choose>
					<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/description='OPT'">
						<xsl:value-of select="'false'"/>
					</xsl:when>
					<xsl:otherwise><xsl:value-of select="'true'"/></xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Description">
				<xsl:choose>
					<xsl:when test="contains(taxSurchargeCoverageInfo/chargeDetails/comment,' - ')">
						<xsl:value-of select="substring-after(taxSurchargeCoverageInfo/chargeDetails/comment,'- ')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/comment"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>	
			<xsl:attribute name="Purpose"><xsl:value-of select="Coverage"/></xsl:attribute>
			<xsl:if test="taxSurchargeCoverageInfo/chargeDetails/periodType!=''">
				<Calculation>
					<xsl:attribute name="UnitName">
						<xsl:choose>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='001'">Per day</xsl:when>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='002'">Per week</xsl:when>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='003'">Per month</xsl:when>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='004'">Per rental</xsl:when>
							<xsl:when test="taxSurchargeCoverageInfo/chargeDetails/periodType='012'">Percentage</xsl:when>
							<xsl:otherwise><xsl:value-of select="taxSurchargeCoverageInfo/chargeDetails/periodType"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</Calculation>
			</xsl:if>
		</Charge>
	</PricedCoverage>
</xsl:template>

<xsl:template match="pickupDropoffLocation">
	<LocationDetails>
		<xsl:attribute name="Code"><xsl:value-of select="locationCode/locationDescription/name"/></xsl:attribute>
		<xsl:choose>
			<xsl:when test="locationCode/locationType=('176')">
				<xsl:attribute name="CodeContext">
					<xsl:value-of select="('PickUp')"/>
				</xsl:attribute>
				<xsl:attribute name="DropOffIndicator">
					<xsl:choose>
						<xsl:when test="following-sibling::pickupDropoffLocation"><xsl:value-of select="('false')"/></xsl:when>
						<xsl:otherwise><xsl:value-of select="('true')"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="locationCode/locationType=('DOL')">
				<xsl:attribute name="CodeContext">
					<xsl:value-of select="('Return')"/>
				</xsl:attribute>
				<xsl:attribute name="DropOffIndicator">
					<xsl:value-of select="('true')"/>
				</xsl:attribute>
			</xsl:when>
		</xsl:choose>
		<xsl:apply-templates select="address"/>
		<xsl:apply-templates select="phone"/>
		<xsl:if test="openingHours/beginDateTime">
			<AdditionalInfo>
				<OperationSchedules>
					<xsl:attribute name="Start">
						<xsl:choose>
							<xsl:when test="openingHours/beginDateTime/hour='0'">
								<xsl:text>12:</xsl:text>
								<xsl:value-of select="format-number(openingHours/beginDateTime/minutes,'00')"/>
								<xsl:text> AM</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(openingHours/beginDateTime/hour,'00')"/>
								<xsl:text>:</xsl:text>
								<xsl:value-of select="format-number(openingHours/beginDateTime/minutes,'00')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="End">
						<xsl:choose>
							<xsl:when test="openingHours/endDateTime/hour='24'">
								<xsl:text>12:</xsl:text>
								<xsl:value-of select="format-number(openingHours/endDateTime/minutes,'00')"/>
								<xsl:text> AM</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(openingHours/endDateTime/hour,'00')"/>
								<xsl:text>:</xsl:text>
								<xsl:value-of select="format-number(openingHours/endDateTime/minutes,'00')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</OperationSchedules>
			</AdditionalInfo>
		</xsl:if>
	</LocationDetails>
</xsl:template>

<xsl:template match="address">
	<Address>
		<AddressLine>
			<xsl:value-of select="addressDetails/line1"/>
		</AddressLine>
		<xsl:if test="addressDetails/line2">
			<AddressLine>
				<xsl:value-of select="addressDetails/line2"/>
			</AddressLine>
		</xsl:if>
		<CityName>
			<xsl:value-of select="city"/>
		</CityName>
		<PostalCode>
			<xsl:value-of select="zipCode"/>
		</PostalCode>
		<StateProv>
			<xsl:value-of select="regionDetails/code"/>
		</StateProv>
		<CountryName>
			<xsl:value-of select="countryCode"/>
		</CountryName>
	</Address>
</xsl:template>

<xsl:template match="phone">
	<Telephone>
		<xsl:attribute name="PhoneUseType">
			<xsl:choose>
				<xsl:when test="phoneOrEmailType='PHO'">Phone</xsl:when>
				<xsl:when test="phoneOrEmailType='FAX'">Fax</xsl:when>
				<xsl:otherwise><xsl:value-of select="phoneOrEmailType"/></xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="PhoneNumber"><xsl:value-of select="telephoneNumber/telephoneNumber"/></xsl:attribute>
	</Telephone>
</xsl:template>

<xsl:template match="mktText">
	<VendorMessage>
		<SubSection>
			<xsl:apply-templates select="freeText" mode="mkt"/>
		</SubSection>			
	</VendorMessage>
</xsl:template>

<xsl:template match="freeText" mode="mkt">
	<Paragraph>
		<Text>
			<xsl:value-of select="."/>
		</Text>	
	</Paragraph>
</xsl:template>

</xsl:stylesheet>

