<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_CarRulesRQ.xsl																  -->
<!-- ================================================================== -->
<!-- Date: 20 Apr 2009 - Rastko															  -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	
	<xsl:template match="/">
		<PoweredCar_RateInformationFromAvailability>
			<xsl:apply-templates select="OTA_VehRateRuleRQ"/>
		</PoweredCar_RateInformationFromAvailability>
	</xsl:template>
	
	<xsl:template match="OTA_VehRateRuleRQ">
		<companyDetails>
			<travelSector>CAR</travelSector>
			<companyCode>
				<xsl:value-of select="RentalInfo/VehicleInfo/@Code"/> 
			</companyCode>
			 <accessLevel>
			 	<xsl:value-of select="RentalInfo/VehicleInfo/@CodeContext"/>
			 </accessLevel>
			<xsl:apply-templates select="POS/Source/RequestorID/CompanyName/@CompanyShortName"/>
			<xsl:apply-templates select="POS/Source/RequestorID/CompanyName/@CodeContext"/> <!-- used for access level, to be confirmed-->
		</companyDetails>
		<!--xsl:apply-templates select="RentalInfo/TPA_Extensions/RateSource"/--> <!-- could'nt find a matching element in the schema-->
		<rateSource>
			<selectionDetails>
				<option>P10</option>
			</selectionDetails>
		</rateSource>
		<pickupDropoffLocs>
			   <locationType>
				   <xsl:value-of select="('176')"/>
			   </locationType>
			   <locationDescription>
			   	<code>
			   		<xsl:value-of select="('1A')"/>
			   	</code>
			   	<name>
			   		<xsl:value-of select="RentalInfo/VehRentalCore/PickUpLocation/@LocationCode"/>
			   	</name>
			   </locationDescription> 		
		</pickupDropoffLocs>
		<pickupDropoffLocs>
			   <locationType>
				   <xsl:value-of select="('DOL')"/>
			   </locationType>
			   <locationDescription>
			   	<code>
			   		<xsl:value-of select="('1A')"/>
			   	</code>
			   	<name>
			   		<xsl:value-of select="RentalInfo/VehRentalCore/ReturnLocation/@LocationCode"/>
			   	</name>
			   </locationDescription> 		
		</pickupDropoffLocs>
		<pickupDropoffTimes>
	     		<businessSemantic>
		     		<xsl:value-of select="('PDA')"/>
	     		</businessSemantic>
	     		<beginDateTime>
		     		<year><xsl:value-of select="substring(RentalInfo/VehRentalCore/@PickUpDateTime,1,4)"/></year>
		     		<month><xsl:value-of select="substring(RentalInfo/VehRentalCore/@PickUpDateTime,6,2)"/></month>
		     		<day><xsl:value-of select="substring(RentalInfo/VehRentalCore/@PickUpDateTime,9,2)"/></day>
		     		<hour><xsl:value-of select="substring(RentalInfo/VehRentalCore/@PickUpDateTime,12,2)"/></hour>
		     		<minutes><xsl:value-of select="substring(RentalInfo/VehRentalCore/@PickUpDateTime,15,2)"/></minutes>
	     		</beginDateTime>
			<endDateTime>
		     		<year><xsl:value-of select="substring(RentalInfo/VehRentalCore/@ReturnDateTime,1,4)"/></year>
		     		<month><xsl:value-of select="substring(RentalInfo/VehRentalCore/@ReturnDateTime,6,2)"/></month>
		     		<day><xsl:value-of select="substring(RentalInfo/VehRentalCore/@ReturnDateTime,9,2)"/></day>
		     		<hour><xsl:value-of select="substring(RentalInfo/VehRentalCore/@ReturnDateTime,12,2)"/></hour>
		     		<minutes><xsl:value-of select="substring(RentalInfo/VehRentalCore/@ReturnDateTime,15,2)"/></minutes>
	     		</endDateTime>
		</pickupDropoffTimes>
		<rateInfo>
			<tariffInfo>
				<rateType><xsl:value-of select="RentalInfo/RateQualifier/@VendorRateID"/></rateType>
				<ratePlanIndicator>
					<xsl:choose>
						<xsl:when test="RentalInfo/RateQualifier/@RatePeriod = 'Daily'">DY</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RatePeriod = 'Weekly'">WY</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RatePeriod = 'Monthly'">MY</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RatePeriod = 'WeekendDay'">WD</xsl:when>
					</xsl:choose>
				</ratePlanIndicator>
			</tariffInfo>
			<rateInformation>
				<category>
					<xsl:choose>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Inclusive'">002</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Convention'">006</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Corporate'">007</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Government'">009</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Package'">011</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Association'">019</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Business'">020</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Consortium'">021</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Credential'">022</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Industry'">023</xsl:when>
						<xsl:when test="RentalInfo/RateQualifier/@RateCategory='Standard'">024</xsl:when>
						<xsl:otherwise>G</xsl:otherwise>
					</xsl:choose>
				</category>
			</rateInformation> 
		</rateInfo>
		<rateCodeInfo>
	     		<fareCategories>
	     			<fareType>
		     			<xsl:value-of select="RentalInfo/RateQualifier/@RateQualifier"/>
	     			</fareType>
	     		</fareCategories> 
		</rateCodeInfo>
		<vehicleInformation>
			<vehTypeOptionQualifier>VT</vehTypeOptionQualifier>
			<vehicleRentalNeedType>
				<vehicleTypeOwner>
					<xsl:value-of select="('ACR')"/>
				</vehicleTypeOwner>
				<vehicleRentalPrefType>
					<xsl:value-of select="RentalInfo/VehicleInfo/@VendorCarType"/>
				</vehicleRentalPrefType>
			</vehicleRentalNeedType>
		</vehicleInformation>
		<xsl:apply-templates select="RentalInfo/CustLoyalty"/>	
	</xsl:template>
	
	<!-- *********************************************************************************************************  -->
	
	<xsl:template match="CompanyShortName">
		<companyName>
			<xsl:value-of select="."/>
		</companyName>
	</xsl:template>

	<xsl:template match="CodeContext">
		<accessLevel>
			<xsl:value-of select="."/>
		</accessLevel>
	</xsl:template>

	<xsl:template match="RateSource">
		<rateSource>
			<selectionDetails>
				<option>
					<xsl:value-of select="."/>
				</option>
			</selectionDetails>
		</rateSource>
	</xsl:template>

	<xsl:template match="CustLoyalty">
		<customerInfo>
			<customerReferences>
				<referenceQualifier>
					<xsl:value-of select="('CD')"/>
				</referenceQualifier>
				<referenceNumber>
					<xsl:value-of select="@MembershipID"/>
				</referenceNumber>
			</customerReferences>
		</customerInfo>
	</xsl:template>
		
</xsl:stylesheet>
