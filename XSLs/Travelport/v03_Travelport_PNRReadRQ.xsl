<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
	v03_Travelport_PNRReadRQ.xsl 													
	================================================================== 
	Date: 15 Sep 2014 - Rastko																
	================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="OTA_ReadRQ/UniqueID/@ID != ''">
				<Request>
					<RetrieveReq>
						<UniversalRecordRetrieveReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                        xmlns:air_v50_0="http://www.travelport.com/schema/air_v50_0"
                                        xmlns:common="http://www.travelport.com/schema/common_v50_0"
                                        xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
                                        xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
                                        xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
                                        xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
                                        xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
							<xsl:attribute name="TargetBranch">
								<xsl:value-of select="OTA_ReadRQ/POS/Source/RequestorID/@Instance" />
							</xsl:attribute>

							<xsl:if test="OTA_ReadRQ/ConversationID">
								<xsl:attribute name="SessionKey">
									<xsl:value-of select="OTA_ReadRQ/ConversationID" />
								</xsl:attribute>
							</xsl:if>
							
							<common:BillingPointOfSaleInfo OriginApplication="UAPI"/>
							<ProviderReservationInfo>
								<xsl:attribute name="ProviderCode">
									<xsl:choose>
										<xsl:when test="OTA_ReadRQ/@Target='WSP'">
											<xsl:value-of select="'1P'"/>
										</xsl:when>
										<xsl:when test="OTA_ReadRQ/@Target='APL'">
											<xsl:value-of select="'1V'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'1G'"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:attribute>
								<xsl:attribute name="ProviderLocatorCode">
									<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID"/>
								</xsl:attribute>
							</ProviderReservationInfo>

							<!--
							<UniversalRecordLocatorCode>
								<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID"/>
							</UniversalRecordLocatorCode>
							-->

						</UniversalRecordRetrieveReq>
					</RetrieveReq>
					<SearchReq>
						<UniversalRecordSearchReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                      xmlns:air_v50_0="http://www.travelport.com/schema/air_v50_0"
                                      xmlns:common="http://www.travelport.com/schema/common_v50_0"
									  xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
                                      xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
                                      xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
                                      xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
                                      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
							
							<xsl:attribute name="TargetBranch">
								<xsl:value-of select="OTA_ReadRQ/POS/Source/RequestorID/@Instance" />
							</xsl:attribute>
							<xsl:attribute name="ProviderLocatorCode">
								<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID" />
							</xsl:attribute>

							<xsl:if test="OTA_ReadRQ/ConversationID">
								<xsl:attribute name="SessionKey">
									<xsl:value-of select="OTA_ReadRQ/ConversationID" />
								</xsl:attribute>
							</xsl:if>
							
							<xsl:attribute name="ProviderCode">
								<xsl:choose>
									<xsl:when test="OTA_ReadRQ/@Target='WSP'">
										<xsl:value-of select="'1P'"/>
									</xsl:when>
									<xsl:when test="OTA_ReadRQ/@Target='APL'">
										<xsl:value-of select="'1V'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'1G'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<common:BillingPointOfSaleInfo OriginApplication="UAPI" />
						</UniversalRecordSearchReq>
					</SearchReq>
					<ImportReq>
						<UniversalRecordImportReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                      xmlns:air_v50_0="http://www.travelport.com/schema/air_v50_0"
                                      xmlns:common="http://www.travelport.com/schema/common_v50_0"
                                      xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
                                      xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
                                      xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
                                      xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
                                      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
							
							<xsl:attribute name="TargetBranch">
								<xsl:value-of select="OTA_ReadRQ/POS/Source/RequestorID/@Instance" />
							</xsl:attribute>
							<xsl:attribute name="ProviderLocatorCode">
								<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID" />
							</xsl:attribute>
							<xsl:if test="OTA_ReadRQ/ConversationID">
								<xsl:attribute name="SessionKey">
									<xsl:value-of select="OTA_ReadRQ/ConversationID" />
								</xsl:attribute>
							</xsl:if>
														
							<xsl:attribute name="ProviderCode">
								<xsl:choose>
									<xsl:when test="OTA_ReadRQ/@Target='WSP'">
										<xsl:value-of select="'1P'"/>
									</xsl:when>
									<xsl:when test="OTA_ReadRQ/@Target='APL'">
										<xsl:value-of select="'1V'"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'1G'"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<common:BillingPointOfSaleInfo OriginApplication="UAPI" />
						</UniversalRecordImportReq>
					</ImportReq>
				</Request>
			</xsl:when>
			<xsl:when test="OTA_UpdateRQ/UniqueID/@ID != ''">
				<UniversalRecordRetrieveReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                    xmlns:air_v50_0="http://www.travelport.com/schema/air_v50_0"
                                    xmlns:common="http://www.travelport.com/schema/common_v50_0"
                                    xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
									xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
                                    xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
                                    xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
                                    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
					<xsl:attribute name="TargetBranch">
						<xsl:value-of select="OTA_UpdateRQ/POS/Source/RequestorID/@Instance" />
					</xsl:attribute>
					<xsl:if test="OTA_ReadRQ/ConversationID">
						<xsl:attribute name="SessionKey">
							<xsl:value-of select="OTA_ReadRQ/ConversationID" />
						</xsl:attribute>
					</xsl:if>
					
					<common:BillingPointOfSaleInfo OriginApplication="UAPI"/>
					<UniversalRecordLocatorCode>
						<xsl:value-of select="OTA_UpdateRQ/UniqueID/@ID"/>
					</UniversalRecordLocatorCode>
				</UniversalRecordRetrieveReq>
			</xsl:when>
			<xsl:when test="OTA_TravelModifyRQ/UniqueId/@ID != ''">
				<UniversalRecordRetrieveReq xmlns="http://www.travelport.com/schema/universal_v50_0"
                                    xmlns:air_v50_0="http://www.travelport.com/schema/air_v50_0"
                                    xmlns:common="http://www.travelport.com/schema/common_v50_0"
									xmlns:hotel="http://www.travelport.com/schema/hotel_v50_0"
                                    xmlns:passive="http://www.travelport.com/schema/passive_v50_0"
                                    xmlns:rail="http://www.travelport.com/schema/rail_v50_0"
                                    xmlns:vehicle="http://www.travelport.com/schema/vehicle_v50_0"
                                    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
					
					<xsl:attribute name="TargetBranch">
						<xsl:value-of select="OTA_TravelModifyRQ/POS/Source/RequestorID/@Instance" />
					</xsl:attribute>
					<xsl:if test="OTA_ReadRQ/ConversationID">
						<xsl:attribute name="SessionKey">
							<xsl:value-of select="OTA_ReadRQ/ConversationID" />
						</xsl:attribute>
					</xsl:if>
					
					<common:BillingPointOfSaleInfo OriginApplication="UAPI"/>
					<UniversalRecordLocatorCode>
						<xsl:value-of select="OTA_TravelModifyRQ/UniqueID/@ID"/>
					</UniversalRecordLocatorCode>
				</UniversalRecordRetrieveReq>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
