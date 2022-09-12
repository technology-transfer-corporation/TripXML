<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_VehLocDetailRS" />
		<xsl:if test="ErrorRS/TPA_Extensions/ErrorInfo">
			<OTA_VehLocDetailRS>
				<xsl:attribute name="Version">1.001</xsl:attribute>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Sabre</xsl:attribute>
						<xsl:attribute name="Code">E</xsl:attribute>
						<xsl:text>INVALID INPUT FILE</xsl:text>
					</Error>
				</Errors>
			</OTA_VehLocDetailRS>
		</xsl:if>
	</xsl:template>
	<!-- ************************************************************** -->
	<xsl:template match="OTA_VehLocDetailRS">
		<OTA_VehLocDetailRS>
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
				<xsl:when test="not(LocationDetail/@Code) and not(Errors/Error)">
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
					<xsl:if test="Vendor/@Code != ''">
						<Vendor>
							<Code>
								<xsl:value-of select="Vendor/@Code" />
							</Code>
						</Vendor>
					</xsl:if>
					<xsl:apply-templates select="LocationDetail" />
					<TPA_Extensions>
						<AdditionalServices>
							<xsl:value-of select="TPA_Extensions/AdditionalServices" />
						</AdditionalServices>
						<Age>
							<AgeRequirements>
								<xsl:value-of select="TPA_Extensions/Age/AgeRequirements" />
							</AgeRequirements>
						</Age>
						<Drop>
							<DropOff>
								<xsl:value-of select="TPA_Extensions/Drop/DropOff" />
							</DropOff>
						</Drop>
						<Fuel>
							<FuelInformation>
								<xsl:value-of select="TPA_Extensions/Fuel/FuelInformation" />
							</FuelInformation>
						</Fuel>
						<GeographicRestrictions>
							<xsl:value-of select="TPA_Extensions/GeographicRestrictions" />
						</GeographicRestrictions>
						<Makes>
							<CarModelsText>
								<xsl:value-of select="TPA_Extensions/Makes/CarModelsText" />
							</CarModelsText>
							<xsl:apply-templates select="TPA_Extensions/Makes/MakesOfCars" />
							<Warn>
								<xsl:value-of select="TPA_Extensions/Makes/Warn" />
							</Warn>
						</Makes>
						<License>
							<xsl:value-of select="TPA_Extensions/License" />
						</License>
						<Surcharges>
							<AgeSurcharges>
								<xsl:value-of select="TPA_Extensions/Surcharges/AgeSurcharges" />
							</AgeSurcharges>
						</Surcharges>
						<Taxes>
							<TaxInformation>
								<xsl:value-of select="TPA_Extensions/Taxes/TaxInformation" />
							</TaxInformation>
						</Taxes>
						<Insurance>
							<InsuranceInformation>
								<xsl:value-of select="TPA_Extensions/Insurance/InsuranceInformation" />
							</InsuranceInformation>
							<PAI>
								<xsl:value-of select="TPA_Extensions/PAI" />
							</PAI>
						</Insurance>
						<Waiver>
							<WaiverInformation>
								<LossDamageWaiver>
									<xsl:value-of select="TPA_Extensions/Waiver/WaiverInformation/LossDamageWaiver" />
								</LossDamageWaiver>
							</WaiverInformation>
						</Waiver>
					</TPA_Extensions>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_VehLocDetailRS>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="LocationDetail">
		<LocationDetail>
			<xsl:attribute name="AtAirport">
				<xsl:value-of select="@AtAirport" />
			</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="@Code" />
			</xsl:attribute>
			<Address>
				<AddressLine>
					<xsl:value-of select="Address/AddressLine[position() = '1']" />
				</AddressLine>
				<xsl:if test="Address/AddressLine[position() = '2'] != ''">
					<AddressLine>
						<xsl:value-of select="Address/AddressLine[position() = '2']" />
					</AddressLine>
				</xsl:if>
				<CityName>
					<xsl:value-of select="Address/CityName" />
				</CityName>
				<PostalCode>
					<xsl:value-of select="Address/PostalCode" />
				</PostalCode>
				<StateProv>
					<xsl:attribute name="StateCode">
						<xsl:value-of select="Address/StateProv/@StateCode" />
					</xsl:attribute>
				</StateProv>
				<CountryName>
					<xsl:attribute name="Code">
						<xsl:value-of select="Address/CountryName/@Code" />
					</xsl:attribute>
				</CountryName>
			</Address>
			<xsl:apply-templates select="Telephone" />
			<AdditionalInfo>
				<VehRentLocInfos>
					<xsl:apply-templates select="AdditionalInfo/VehRentLocInfo" />
				</VehRentLocInfos>
				<OperationSchedules>
					<OperationSchedule>
						<OperationTimes>
							<xsl:apply-templates select="AdditionalInfo/OperationSchedules/OperationSchedule/OperationTimes/OperationTime" />
						</OperationTimes>
					</OperationSchedule>
				</OperationSchedules>
			</AdditionalInfo>
		</LocationDetail>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="VehRentLocInfo">
		<VehRentLocInfo>
			<xsl:attribute name="Type">
				<xsl:value-of select="substring(string(@InfoType),1,1)" />
			</xsl:attribute>
			<SubSection>
				<Paragraph>
					<Text>
						<xsl:value-of select="SubSection/Paragraph/Text" />
					</Text>
				</Paragraph>
			</SubSection>
		</VehRentLocInfo>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="Telephone">
		<xsl:if test="@PhoneNumber != ''">
			<Telephone>
				<xsl:attribute name="PhoneNumber">
					<xsl:value-of select="@PhoneNumber" />
				</xsl:attribute>
				<xsl:attribute name="PhoneUseType">
					<xsl:value-of select="@PhoneUseType" />
				</xsl:attribute>
			</Telephone>
		</xsl:if>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="OperationTime">
		<OperationTime>
			<xsl:attribute name="Duration">
				<xsl:choose>
					<xsl:when test="Duration != ''">
						<xsl:value-of select="@Duration" />
					</xsl:when>
					<xsl:otherwise>P0Y0M0DT<xsl:value-of select="substring(string(@End),1,2)" />H<xsl:value-of select="substring(string(@End),4,2)" />M</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="@Mon != ''">
					<xsl:attribute name="Mon">
						<xsl:value-of select="@Mon" />
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@Tue != ''">
					<xsl:attribute name="Tue">
						<xsl:value-of select="@Tue" />
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@Weds != ''">
					<xsl:attribute name="Weds">
						<xsl:value-of select="@Weds" />
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@Thur != ''">
					<xsl:attribute name="Thur">
						<xsl:value-of select="@Thur" />
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@Fri != ''">
					<xsl:attribute name="Fri">
						<xsl:value-of select="@Fri" />
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@Sat != ''">
					<xsl:attribute name="Sat">
						<xsl:value-of select="@Sat" />
					</xsl:attribute>
				</xsl:when>
				<xsl:when test="@Sun != ''">
					<xsl:attribute name="Sun">
						<xsl:value-of select="@Sun" />
					</xsl:attribute>
				</xsl:when>
			</xsl:choose>
		</OperationTime>
	</xsl:template>
	<!--*************************************************************-->
	<xsl:template match="MakesOfCars">
		<MakesOfCars>
			<xsl:if test="@CarExample != ''">
				<xsl:attribute name="CarExample">
					<xsl:value-of select="@CarExample" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@CarGroup != ''">
				<xsl:attribute name="CarGroup">
					<xsl:value-of select="@CarGroup" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@CarType != ''">
				<xsl:attribute name="CarType">
					<xsl:value-of select="@CarType" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@NumberOfBags != ''">
				<xsl:attribute name="NumberOfBags">
					<xsl:value-of select="@NumberOfBags" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@NumberOfDoors != ''">
				<xsl:attribute name="NumberOfDoors">
					<xsl:value-of select="@NumberOfDoors" />
				</xsl:attribute>
			</xsl:if>
			<xsl:if test="@PassengerCapacity != ''">
				<xsl:attribute name="PassengerCapacity">
					<xsl:value-of select="@PassengerCapacity" />
				</xsl:attribute>
			</xsl:if>
		</MakesOfCars>
	</xsl:template>
	<!--*************************************************************-->
</xsl:stylesheet>
