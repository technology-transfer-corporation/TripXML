<?xml version="1.0" ?> 
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_CarAvailRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 30 Jul 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_VehAvailRateRS Version="1.001">
			<xsl:apply-templates select="PoweredCar_SingleAvailabilityReply" />
			<xsl:apply-templates select="PoweredCar_MultiAvailabilityReply" />
			<xsl:apply-templates select="MessagesOnly_Reply/CAPI_Messages" />
		</OTA_VehAvailRateRS>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages">
		<Errors>
			<Error Type="Amadeus">
				<xsl:attribute name="Code"><xsl:value-of select="ErrorCode"/></xsl:attribute>
				<xsl:value-of select="Text"/>
			</Error>
		</Errors>
	</xsl:template>
	
	<xsl:template match="PoweredCar_SingleAvailabilityReply">
		<xsl:choose>
			<xsl:when test="errorSituation">
				<Errors>
					<Error Type="Amadeus">
						<xsl:attribute name="Code"><xsl:value-of select="errorSituation/applicationError/errorDetails/errorCode"/></xsl:attribute>
						<xsl:value-of select="errorSituation/errorFreeText/freeText"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<VehAvailRSCore>
					<xsl:apply-templates select="companyLocationInfo"/>
				</VehAvailRSCore>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="companyLocationInfo">
		<VehRentalCore>
			<xsl:apply-templates select="pickupDropoffTime"/>
			<xsl:apply-templates select="pickupDropoffLocs"/>
		</VehRentalCore>
		<VehVendorAvails>
			<VehVendorAvail>
				<xsl:apply-templates select="carCompanyData"/>
				<VehAvails>
					<xsl:apply-templates select="availabilityLine" mode="single"/>
				</VehAvails>
			</VehVendorAvail>
		</VehVendorAvails>
	</xsl:template>
	
	<xsl:template match="availabilityLine" mode="single">
		<VehAvail>
			<VehAvailCore>
				<xsl:attribute name="Status">
					<xsl:choose>
						<xsl:when test="rateStatus/statusCode='RAV'">Available</xsl:when>
						<xsl:otherwise>OnRequest</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:apply-templates select="vehicleInformation"/>
				<xsl:apply-templates select="rateDetailsInfo"/>
			</VehAvailCore>
			<!--VehAvailInfo>
				<xsl:apply-templates select="ruleInfo"/>
			</VehAvailInfo-->
		</VehAvail>
	</xsl:template>
	
	<xsl:template match="PoweredCar_MultiAvailabilityReply">
		<xsl:choose>
			<xsl:when test="errorInformation/applicationError/errorDetails/errorCode != '0' and errorInformation/applicationError/errorDetails/errorCategory!='WEC'">
				<Errors>
					<Error Type="Amadeus">
						<xsl:attribute name="Code"><xsl:value-of select="errorInformation/applicationError/errorDetails/errorCode"/></xsl:attribute>
						<xsl:value-of select="errorInformation/errorFreeText/freeText"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:when test="errorInformation/applicationError/errorDetails/errorCategory='EC'">
				<Errors>
					<Error Type="Amadeus">
						<xsl:attribute name="Code"><xsl:value-of select="errorInformation/applicationError/errorDetails/errorCode"/></xsl:attribute>
						<xsl:value-of select="errorInformation/errorFreeText/freeText"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<VehAvailRSCore>
					<xsl:apply-templates select="avlScreenHeader"/>
				</VehAvailRSCore>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="avlScreenHeader">
		<VehRentalCore>
			<xsl:apply-templates select="pickupDropoffTimes"/>
			<xsl:apply-templates select="pickupDropoffLocation"/>
		</VehRentalCore>
		<VehVendorAvails>
			<!--xsl:variable name="avline"><xsl:value-of select="count(availabilityLine)"/></xsl:variable>
			<xsl:variable name="rqc"><xsl:value-of select="count(availabilityLine/rateDetailsInfo/tariffInfo[1]/rateConvertedQualifier)"/></xsl:variable>
			<xsl:choose>
				<xsl:when test="$avline = $rqc">
					<xsl:apply-templates select="availabilityLine[rateDetailsInfo/tariffInfo/rateType='902']" mode="multi"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="availabilityLine[rateDetailsInfo/tariffInfo[2]][not(rateDetailsInfo/tariffInfo/rateConvertedQualifier)]" mode="multi"/>
				</xsl:otherwise>
			</xsl:choose-->
			<xsl:apply-templates select="availabilityLine" mode="multi"/>
		</VehVendorAvails>
	</xsl:template>
	
	<xsl:template match="availabilityLine" mode="multi">
		<VehVendorAvail>
			<xsl:apply-templates select="companyData"/>
			<VehAvails>
				<VehAvail>
					<VehAvailCore Status="Available">
						<xsl:apply-templates select="vehicleInformation"/>
						<xsl:apply-templates select="rateDetailsInfo"/>
					</VehAvailCore>
					<!--VehAvailInfo>
						<xsl:apply-templates select="ruleInfo"/>
					</VehAvailInfo-->
				</VehAvail>
			</VehAvails>
		</VehVendorAvail>
	</xsl:template>
	
	<xsl:template match="rateDetailsInfo">
		<RentalRate>
			<RateDistance>
				<xsl:attribute name="Unlimited">
					<xsl:choose>
						<xsl:when test="associatedCharges/amountQualifier='UNL'"><xsl:text>true</xsl:text></xsl:when>
						<xsl:otherwise><xsl:text>false</xsl:text></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="DistUnitName">
					<xsl:choose>
						<xsl:when test="associatedCharges/chargeType='7'"><xsl:text>Km</xsl:text></xsl:when>
						<xsl:when test="associatedCharges/chargeType='9'"><xsl:text>Km</xsl:text></xsl:when>
						<xsl:otherwise><xsl:text>Mile</xsl:text></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="VehiclePeriodUnitName">
					<xsl:choose>
						<xsl:when test="tariffInfo/rateType='1'"><xsl:text>Hour</xsl:text></xsl:when>
						<xsl:when test="tariffInfo/rateType='3'"><xsl:text>Day</xsl:text></xsl:when>
						<xsl:when test="tariffInfo/rateType='5'"><xsl:text>Weekend</xsl:text></xsl:when>
						<xsl:when test="tariffInfo/rateType='6'"><xsl:text>Week</xsl:text></xsl:when>
						<xsl:when test="tariffInfo/rateType='4'"><xsl:text>Month</xsl:text></xsl:when>
						<xsl:otherwise><xsl:text>RentalPeriod</xsl:text></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</RateDistance>
			<VehicleCharges>
				<VehicleCharge>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(tariffInfo[1]/rateAmount,'.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode"><xsl:value-of select="tariffInfo/rateCurrency"/></xsl:attribute>
					<xsl:attribute name="DecimalPlaces"><xsl:value-of select="string-length(substring-after(tariffInfo/rateAmount,'.'))"/></xsl:attribute>
					<xsl:attribute name="TaxInclusive">false</xsl:attribute>
					<xsl:choose>
						<xsl:when test="tariffInfo/rateAmountQualifier='901'">
							<xsl:attribute name="GuaranteedInd"><xsl:text>true</xsl:text></xsl:attribute>
						</xsl:when>
						<xsl:otherwise>
							<xsl:attribute name="GuaranteedInd"><xsl:text>false</xsl:text></xsl:attribute>
						</xsl:otherwise>
					</xsl:choose>
					<xsl:attribute name="Purpose"><xsl:text>1</xsl:text></xsl:attribute>
					<!--Calculation>
						<xsl:attribute name="UnitCharge">
							<xsl:value-of select="translate(tariffInfo[1]/rateAmount,'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="UnitName">
							<xsl:choose>
								<xsl:when test="tariffInfo/rateType='1'"><xsl:text>Hour</xsl:text></xsl:when>
								<xsl:when test="tariffInfo/rateType='3'"><xsl:text>Day</xsl:text></xsl:when>
								<xsl:when test="tariffInfo/rateType='5'"><xsl:text>Weekend</xsl:text></xsl:when>
								<xsl:when test="tariffInfo/rateType='6'"><xsl:text>Week</xsl:text></xsl:when>
								<xsl:when test="tariffInfo/rateType='4'"><xsl:text>Month</xsl:text></xsl:when>
								<xsl:otherwise><xsl:text>RentalPeriod</xsl:text></xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</Calculation-->
				</VehicleCharge>
				<xsl:apply-templates select="associatedCharges[amountQualifier!='UNL']"/>
			</VehicleCharges>
			<RateQualifier>
				<xsl:attribute name="RateCategory">
					<xsl:choose>
						<xsl:when test="extraRateTypeInfo/rateCategory='002'">Inclusive</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='006'">Con'vention</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='007'">Corporate</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='009'">Government</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='011'">Package</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='019'">Association</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='020'">Business</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='021'">Consortium</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='022'">Credential</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='023'">Industry</xsl:when>
						<xsl:when test="extraRateTypeInfo/rateCategory='024'">Standard</xsl:when>
						<xsl:otherwise>General</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="RateQualifier"><xsl:value-of select="../rateCodeInfo/fareCategories/fareType"/></xsl:attribute>
				<xsl:attribute name="VendorRateID"><xsl:value-of select="tariffInfo/rateIdentifier"/></xsl:attribute>
			</RateQualifier>
		</RentalRate>
		<TotalCharge>
			<xsl:attribute name="RateTotalAmount">
				<xsl:choose>
					<xsl:when test="tariffInfo[rateType = '906']/rateAmount != ''">
						<xsl:value-of select="translate(tariffInfo[rateType = '906']/rateAmount,'.','')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:variable name="start">
							<xsl:apply-templates select="../../pickupDropoffTimes/beginDateTime" mode="date"/>
						</xsl:variable>
						<xsl:variable name="end">
							<xsl:apply-templates select="../../pickupDropoffTimes/endDateTime" mode="date"/>
						</xsl:variable>
						<xsl:variable name="dc1" select="ttVB:FctDateDuration(string($start),string($end))"/>
						<xsl:variable name="dc">
							<xsl:choose>
								<xsl:when test="$dc1='0'">1</xsl:when>
								<xsl:otherwise><xsl:value-of select="$dc1"/></xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="amt"><xsl:value-of select="translate(tariffInfo[1]/rateAmount,'.','')"/></xsl:variable>
						<xsl:value-of select="$dc * $amt"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode"><xsl:value-of select="tariffInfo/rateCurrency"/></xsl:attribute>
		</TotalCharge>
	</xsl:template>
	
	<xsl:template match="associatedCharges">
		<VehicleCharge>
			<xsl:if test="chargeType = '2' or chargeType = '3' or chargeType = '4' or chargeType = '5'">
				<xsl:attribute name="Amount"><xsl:value-of select="translate(amount,'.','')"/></xsl:attribute>
			</xsl:if>
			<!--xsl:attribute name="TaxInclusive">false</xsl:attribute-->
			<xsl:attribute name="Description">
				<xsl:choose>
					<xsl:when test="chargeType = '108'">	Surcharge Policy</xsl:when>
					<xsl:when test="chargeType = '45'">Tax Policy</xsl:when>
					<xsl:when test="chargeType = 'COV'">Coverage Policy</xsl:when>
					<xsl:when test="chargeType = '2'">Extra Day</xsl:when>
					<xsl:when test="chargeType = '3'">Extra Week</xsl:when>
					<xsl:when test="chargeType = '4'">Extra Hour</xsl:when>
					<xsl:when test="chargeType = '5'">Extra Month</xsl:when>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="IncludedInRate">
				<xsl:choose>
					<xsl:when test="amountQualifier='3'">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="Purpose">
				<xsl:choose>
					<xsl:when test="chargeType = '108' or chargeType = '45' or chargeType = 'COV'">5</xsl:when>
					<xsl:when test="chargeType ='2'">10</xsl:when>
					<xsl:when test="chargeType ='3'">9</xsl:when>
					<xsl:when test="chargeType ='4'">11</xsl:when>
					<xsl:when test="chargeType ='5'">8</xsl:when>
					<xsl:otherwise>1</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
		</VehicleCharge>
	</xsl:template>
	
	<xsl:template match="vehicleInformation">
		<Vehicle>
			<xsl:attribute name="AirConditionInd">
				<xsl:choose>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,4,1) = 'R'">true</xsl:when>					
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="TransmissionType">
				<xsl:choose>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,3,1) = 'A'">Automatic</xsl:when>				
					<xsl:otherwise>Manual</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="VendorCarType"><xsl:value-of select="vehicleCharacteristic/vehicleRentalPrefType"/></xsl:attribute>
			<VehType>
				<xsl:attribute name="VehicleCategory">
				<xsl:choose>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'C'">2/4 Door Car</xsl:when>	
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'B'">2-Door Car</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'D'">4-Door Car</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'W'">Wagon</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'V'">Van</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'L'">Limousine</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'S'">Sport</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'T'">Convertible</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'F'">4-Wheel Drive</xsl:when>	
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'P'">Pickup</xsl:when>
					<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,2,1) = 'J'">All-Terrain</xsl:when>				
					<xsl:otherwise>Unavailable</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:value-of select="vehicleCharacteristic/vehicleRentalPrefType"/>
			</VehType>
			<VehClass>
				<xsl:attribute name="Size">
					<xsl:choose>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'M'">Mini</xsl:when>	
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'E'">Economy</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'C'">Compact</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'I'">Intermediate</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'S'">Standard</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'F'">Full-Size</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'P'">Premium</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'L'">Luxury</xsl:when>
						<xsl:when test="substring(vehicleCharacteristic/vehicleRentalPrefType,1,1) = 'X'">Special</xsl:when>							
						<xsl:otherwise>Unavailable</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</VehClass>
		</Vehicle>
	</xsl:template>
	
	<xsl:template match="ruleInfo">
		<PaymentRules>
			<xsl:apply-templates select="ruleDetails"/>
		</PaymentRules>
	</xsl:template>
	
	<xsl:template match="ruleDetails">
		<PaymentRule>
			<xsl:attribute name="RuleType"><xsl:value-of select="type"/></xsl:attribute>
		</PaymentRule>
	</xsl:template>
	
	<xsl:template match="companyData | carCompanyData">
		<Vendor>
			<xsl:attribute name="Code"><xsl:value-of select="companyCode"/></xsl:attribute>
			<xsl:attribute name="CodeContext"><xsl:value-of select="accessLevel"/></xsl:attribute>
		</Vendor>
	</xsl:template>
	
	<xsl:template match="pickupDropoffLocation | pickupDropoffLocs">
		<xsl:choose>
			<xsl:when test="locationType='176'">
				<PickUpLocation>
					<xsl:attribute name="LocationCode"><xsl:value-of select="locationDescription/name"/></xsl:attribute>
				</PickUpLocation>
			</xsl:when>
			<xsl:otherwise>
				<ReturnLocation>
					<xsl:attribute name="LocationCode"><xsl:value-of select="locationDescription/name"/></xsl:attribute>
				</ReturnLocation>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="pickupDropoffTimes | pickupDropoffTime">
		<xsl:attribute name="PickUpDateTime">
			<xsl:apply-templates select="beginDateTime" mode="datetime"/>
		</xsl:attribute>
		<xsl:attribute name="ReturnDateTime">
			<xsl:apply-templates select="endDateTime" mode="datetime"/>
		</xsl:attribute>
	</xsl:template>
	
	<xsl:template match="beginDateTime | endDateTime" mode="datetime">
		<xsl:value-of select="concat(format-number(year,'0000'),'-')"/>
		<xsl:value-of select="concat(format-number(month,'00'),'-')"/>
		<xsl:value-of select="concat(format-number(day,'00'),'T')"/>
		<xsl:value-of select="concat(format-number(hour,'00'),':')"/>
		<xsl:value-of select="concat(format-number(minutes,'00'),':00')"/>
	</xsl:template>
	
	<xsl:template match="beginDateTime | endDateTime" mode="date">
		<xsl:value-of select="concat(format-number(year,'0000'),'-')"/>
		<xsl:value-of select="concat(format-number(month,'00'),'-')"/>
		<xsl:value-of select="format-number(day,'00')"/>
	</xsl:template>

<!--**********************************************************************************************-->	
<msxsl:script language="VisualBasic" implements-prefix="ttVB">
<![CDATA[
Function FctDateDuration(byval p_startDate as string, byval p_endDate as string) as string
   	
    If (IsDate(p_startDate) And IsDate(p_endDate)) Then
        FctDateDuration = CStr(DateDiff("d", p_startDate, p_endDate)) 
    Else
        FctDateDuration = p_startDate & p_endDate
    End If

End Function
]]>
</msxsl:script>

	
</xsl:stylesheet>
