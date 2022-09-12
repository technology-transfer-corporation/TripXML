<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<!-- ================================================================== -->
<!-- Amadeus_CruiseCategoryAvailRS.xsl 	     									       -->
<!-- ================================================================== -->
<!-- Date: 15 Mar 2006 - Rastko											        		       -->
<!-- ================================================================== -->
<xsl:output method="xml" indent="yes" omit-xml-declaration="yes"/>

<xsl:template match="/">
	<xsl:apply-templates select="CruiseByPass_CategoryAvailabilityReply" />
	<xsl:apply-templates select="MessagesOnly_Reply" />
</xsl:template>

<xsl:template match="MessagesOnly_Reply">
	<OTA_CruiseCategoryAvailRS Version="1.000">
		<Errors>
			<xsl:apply-templates select="CAPI_Messages" />
		</Errors>
	</OTA_CruiseCategoryAvailRS>
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

<xsl:template match="CruiseByPass_CategoryAvailabilityReply">
	<OTA_CruiseCategoryAvailRS Version="1.000">
		<xsl:choose>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('2')">
				<Errors>
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="error"/>
				</Errors>
			</xsl:when>
			<xsl:when test="advisoryMessage/errorQualifierDescription/messageType=('4')">
				<Success/>
				<Warnings>
					<xsl:apply-templates select="advisoryMessage/errorQualifierDescription" mode="warning"/>
				</Warnings>
			</xsl:when>
			<xsl:when test="not(advisoryMessage)">
				<Success/>
			</xsl:when>
		</xsl:choose>
		<xsl:if test="(not(advisoryMessage)) or (advisoryMessage/errorQualifierDescription/messageType=('4'))">
			<SailingInfo>
				<SelectedSailing>
					<xsl:apply-templates select="sailingGroup/sailingDescription/sailingId/cruiseVoyageNumber"/>
					<xsl:attribute name="Start">
						<xsl:if test="sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate != ''">
							<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,5,4)"/>	
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(sailingGroup/sailingDescription/sailingDateTime/sailingDepartureDate,1,2)"/>
						</xsl:if>
					</xsl:attribute>
					<xsl:attribute name="Duration">
						<xsl:value-of select="sailingGroup/sailingDescription/sailingDateTime/sailingDuration"/>	
					</xsl:attribute>
					<xsl:attribute name="VendorCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/cruiselineCode"/>											</xsl:attribute>
					<xsl:attribute name="VendorName"></xsl:attribute>
					<xsl:attribute name="ShipCode">
						<xsl:value-of select="sailingGroup/sailingDescription/providerDetails/shipCode"/>	
					</xsl:attribute>
					<xsl:attribute name="ShipName"/>
				</SelectedSailing>
				<xsl:apply-templates select="sailingGroup/packageDescription"/>
				<Currency>
					<xsl:attribute name="CurrencyCode">
						<xsl:value-of select="sailingGroup/currencyInfo/currencyList/currencyIsoCode"/>
					</xsl:attribute>
				</Currency>
			</SailingInfo>
			<xsl:apply-templates select="sailingGroup/fareGroup"/>
			<xsl:if test="sailingGroup/attributeInfo">
				<Fee>
					<Taxes>
						<xsl:apply-templates select="sailingGroup/attributeInfo"/>
					</Taxes>
				</Fee>
			</xsl:if>
			<xsl:if test="sailingGroup/additionalPriceInfo">
				<Fee>
					<Taxes>
						<xsl:attribute name="Amount"><xsl:value-of select="sailingGroup/additionalPriceInfo/amountDetail/amount"/></xsl:attribute>
					</Taxes>
					<Description>
						<xsl:attribute name="Name"><xsl:value-of select="sailingGroup/additionalPriceInfo/amountDetail/amountQualifierCode"/></xsl:attribute>
					</Description>
				</Fee>
			</xsl:if>			
			<xsl:apply-templates select="sailingGroup/sailingInformation"/>
			<xsl:apply-templates select="sailingGroup/marketingMessage/advisoryMessageDetails/messageDescription"/>
		</xsl:if>
	</OTA_CruiseCategoryAvailRS>
</xsl:template>


<xsl:template match="errorQualifierDescription[messageType=('2')]" mode="error">
	<xsl:element name="Error">
		<xsl:attribute name="Type">Amadeus</xsl:attribute>
		<xsl:attribute name="Code">
			<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
		</xsl:attribute>
		<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
	</xsl:element>
</xsl:template>

<xsl:template match="errorQualifierDescription[messageType=('4')]" mode="warning">
	<Warning>
		<xsl:attribute name="Type">
			<xsl:value-of select="messageQualifier"/>
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="messageQualifier=('2')">
				<xsl:attribute name="ShortText">
					<xsl:value-of select="../advisoryMessageDetails/messageDescription"/>
				</xsl:attribute>
			</xsl:when>
			<xsl:when test="messageQualifier=('1')">
				<xsl:attribute name="Code">
					<xsl:value-of select="../advisoryMessageDetails/advisoryMessageNbr"/>
				</xsl:attribute>
			</xsl:when>
		</xsl:choose>	
	</Warning>
</xsl:template>

<xsl:template match="cruiseVoyageNumber">
	<xsl:attribute name="VoyageID">
		<xsl:value-of select="."/>
	</xsl:attribute>
</xsl:template>

<xsl:template match="sailingInformation">
	<Information>
		<xsl:attribute name="Name">
			<xsl:value-of select="('SailingInfo')"/>	
		</xsl:attribute>
		<Text>
			<xsl:value-of select="textDetails"/>	
		</Text>
	</Information>
</xsl:template>

<xsl:template match="packageDescription">
	<InclusivePackageOption>
		<xsl:attribute name="CruisePackageCode">
			<xsl:value-of select="packageDetails/packageCode"/>	
		</xsl:attribute>
		<xsl:attribute name="StartDate">
			<xsl:value-of select="packageDateTime/packageStartDate"/>	
		</xsl:attribute>
	</InclusivePackageOption>
</xsl:template>

<xsl:template match="messageDescription">
	<Information>
		<xsl:attribute name="Name">
			<xsl:value-of select="('MarketingMessage')"/>	
		</xsl:attribute>
		<Text>
			<xsl:value-of select="."/>	
		</Text>
	</Information>
</xsl:template>

<xsl:template match="attributeInfo">
	<Tax>
		<xsl:attribute name="Type">
			<xsl:value-of select="attributeType"/>	
		</xsl:attribute>
	</Tax>
</xsl:template>

<xsl:template match="fareGroup">
	<FareOption>
		<xsl:attribute name="FareCode">
			<xsl:value-of select="fareCode/fareCodeId/cruiseFareCode"/>	
		</xsl:attribute>
		<xsl:variable name="fc">
			<xsl:value-of select="fareCode/fareCodeId/cruiseFareCode"/>	
		</xsl:variable>
		<xsl:apply-templates select="passengerGroupId/passengerGroupInfoId/groupCode"/>
		<CategoryOptions>
			<xsl:apply-templates select="categoryGroup[priceInfo]">
				<xsl:with-param name="fc"><xsl:value-of select="$fc"/></xsl:with-param>
			</xsl:apply-templates>
		</CategoryOptions>
	</FareOption>
</xsl:template>

<xsl:template match="categoryGroup">
	<xsl:param name="fc"/>
	<CategoryOption>
		<xsl:choose>
			<xsl:when test="categoryInfo/fareCodeId != ''">
				<xsl:apply-templates select="categoryInfo/fareCodeId"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:attribute name="FareCode">
					<xsl:value-of select="$fc"/>	
				</xsl:attribute>
			</xsl:otherwise>
		</xsl:choose>
		<xsl:attribute name="Status">
			<xsl:choose>
				<xsl:when test="categoryInfo/statusOrGroupAllocation = 'AVL'">Available</xsl:when>
				<xsl:when test="categoryInfo/statusOrGroupAllocation = 'CLO'">Unavailable</xsl:when>
				<xsl:when test="categoryInfo/statusOrGroupAllocation = 'ONR'">OnRequest</xsl:when>
				<xsl:when test="categoryInfo/statusOrGroupAllocation = 'WTL'">OnRequest</xsl:when>
				<xsl:when test="substring(categoryInfo/statusOrGroupAllocation,1,1) = '0'">Available</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="categoryInfo/statusOrGroupAllocation"/>	
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:apply-templates select="categoryInfo/categoryId/heldIndicator"/>
		<xsl:apply-templates select="categoryInfo/categoryDetails/shipLocation"/>
		<xsl:apply-templates select="categoryInfo/categoryDetails/maxCabinOccupancy"/>
		<xsl:attribute name="PricedCategoryCode">
			<xsl:choose>
				<xsl:when test="categoryInfo/categoryId/pricedCategory != ''">
					<xsl:value-of select="categoryInfo/categoryId/pricedCategory"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="categoryInfo/categoryId/berthedCategory"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:attribute>
		<xsl:attribute name="BerthedCategoryCode">
			<xsl:value-of select="categoryInfo/categoryId/berthedCategory"/>
		</xsl:attribute>
		<xsl:if test="categoryInfo/indicators">
			<xsl:attribute name="ListOfCategoryQualifierCodes">
				<xsl:apply-templates select="categoryInfo/indicators"/>
			</xsl:attribute>
		</xsl:if>
		<xsl:apply-templates select="priceInfo"/>
	</CategoryOption>
</xsl:template>

<xsl:template match="heldIndicator">
	<xsl:attribute name="HeldIndicator">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="groupCode">
	<xsl:attribute name="GroupCode">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="shipLocation">
	<xsl:attribute name="CategoryLocation">
		<xsl:choose>
			<xsl:when test=". = 'O'">Outside</xsl:when>
			<xsl:when test=". = 'I'">Inside</xsl:when>
			<xsl:when test=". = 'B'">Both</xsl:when>
		</xsl:choose>
	</xsl:attribute>
</xsl:template>

<xsl:template match="maxCabinOccupancy">
	<xsl:attribute name="MaxOccupancy">
		<xsl:value-of select="."/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="indicators">
	<xsl:if test="position() > 1"><xsl:text> </xsl:text></xsl:if>
	<xsl:choose>
		<xsl:when test=". = 'K'">1</xsl:when>
		<xsl:when test=". = 'G'">2</xsl:when>
		<xsl:when test=". = 'S'">3</xsl:when>
		<xsl:when test=". = 'C'">5</xsl:when>
		<xsl:otherwise><xsl:value-of select="."/></xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template match="fareCodeId">
	<xsl:attribute name="FareCode">
		<xsl:value-of select="cruiseFareCode"/>	
	</xsl:attribute>
</xsl:template>

<xsl:template match="priceInfo">
	<PriceInfo>
		<xsl:attribute name="Amount">
			<xsl:value-of select="amountDetail/amount"/>	
		</xsl:attribute>
		<xsl:attribute name="AgeQualifyingCode">
			<xsl:choose>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'ADT'">10</xsl:when>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'INF'">7</xsl:when>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'CHD'">8</xsl:when>
				<xsl:when test="amountDetail/breakdownQualifierCode = 'SRC'">11</xsl:when>
				<xsl:when test="not(amountDetail/breakdownQualifierCode)">10</xsl:when>
				<xsl:otherwise><xsl:value-of select="amountDetail/breakdownQualifierCode"/></xsl:otherwise>
			</xsl:choose>	
		</xsl:attribute>
		<xsl:attribute name="BreakdownType">
			<xsl:value-of select="amountDetail/breakdownCode"/>	
		</xsl:attribute>
		<xsl:choose>
			<xsl:when test="amountDetail/priceDescription != ''">
				<xsl:apply-templates select="amountDetail/priceDescription"/>
			</xsl:when>
			<xsl:when test="amountDetail/amountQualifierCode = 'INF'">
				<PriceDescription>Informative price</PriceDescription>
			</xsl:when>
		</xsl:choose>
	</PriceInfo>
</xsl:template>

<xsl:template match="priceDescription">
	<PriceDescription>
		<xsl:value-of select="."/>	
	</PriceDescription>
</xsl:template>

</xsl:stylesheet>