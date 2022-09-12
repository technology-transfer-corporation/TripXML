<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- v04_Sabre_TravelBuildRQ.xsl 															       -->
	<!-- ================================================================== -->
	<!-- Date: 21 APR 2010 - Shashin														       -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="PCC">
		<xsl:value-of select="OTA_TravelItineraryRQ/POS/Source/@PseudoCityCode"/>
	</xsl:variable>
	<xsl:variable name="paxtype">
		<xsl:choose>
			<xsl:when test="$PCC = 'B66B' or $PCC = 'B67B' or $PCC = 'B68B'">Y</xsl:when>
			<xsl:otherwise>N</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:template match="/">
		<TravelBuild>
			<xsl:apply-templates select="OTA_TravelItineraryRQ"/>
		</TravelBuild>
	</xsl:template>
	<xsl:template match="OTA_TravelItineraryRQ">
		<!--********************PassengerDetails************************-->
		<PassengerDetails>
			<PassengerDetailsRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="OTA_2007A.TsabreXML1.2.1">
				<xsl:apply-templates select="TPA_Extensions/PNRData" mode="ixplore"/>
				<TravelerInfo>
					<xsl:for-each select="Traveler">
						<PersonName>
							<xsl:attribute name="TravelerRefNumber"><xsl:value-of select="TravelerRefNumber/@RPH"/><GivenName><xsl:value-of select="PersonName/GivenName"/><xsl:if test="PersonName/NamePrefix != ''"><xsl:text/><xsl:value-of select="PersonName/NamePrefix"/></xsl:if></GivenName><Surname><xsl:value-of select="PersonName/Surname"/></Surname><xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'"><Infant Ind="true"/></xsl:if></xsl:attribute>
						</PersonName>
					</xsl:for-each>
					<Telephone>
						<xsl:attribute name="PhoneLocationType"><xsl:choose><xsl:when test="@PhoneLocationType='Home'">H</xsl:when><xsl:when test="@PhoneLocationType='Work'">W</xsl:when><xsl:when test="@PhoneLocationType='Mobile'">M</xsl:when><xsl:when test="@PhoneLocationType='Fax'">F</xsl:when><xsl:when test="@PhoneLocationType='Business'">B</xsl:when></xsl:choose></xsl:attribute>
						<xsl:if test="@AreaCityCode!=''">
							<xsl:attribute name="AreaCityCode"><xsl:value-of select="@AreaCityCode"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="PhoneNumber"><xsl:value-of select="@PhoneNumber"/></xsl:attribute>
					</Telephone>
					<Telephone>
						<xsl:attribute name="PhoneUseType"><xsl:choose><xsl:when test="@PhoneLocationType='Home'">H</xsl:when><xsl:when test="@PhoneLocationType='Work'">W</xsl:when><xsl:when test="@PhoneLocationType='Mobile'">M</xsl:when><xsl:when test="@PhoneLocationType='Fax'">F</xsl:when><xsl:when test="@PhoneLocationType='Business'">B</xsl:when></xsl:choose></xsl:attribute>
						<xsl:if test="@AreaCityCode!=''">
							<xsl:attribute name="AreaCityCode"><xsl:value-of select="@AreaCityCode"/></xsl:attribute>
						</xsl:if>
						<xsl:attribute name="PhoneNumber"><xsl:value-of select="@PhoneNumber"/></xsl:attribute>
					</Telephone>
					<Email>
						<xsl:attribute name="EmailAddress"><xsl:value-of select="Email"/></xsl:attribute>
					</Email>
					<Address>
					</Address>
					<xsl:for-each select="Traveler">
						<xsl:apply-templates select="CustLoyalty" mode="ixplore">
							<!--
							<xsl:with-param name="RefNumber">
								<xsl:value-of select="$RefNumber"/>
							</xsl:with-param>
							-->
						</xsl:apply-templates>
					</xsl:for-each>
					<xsl:for-each select="Traveler">
						<xsl:if test="@PassengerTypeCode != '' and $paxtype='Y'">
							<PassengerType>
								<xsl:attribute name="Code"><xsl:value-of select="@PassengerTypeCode"/></xsl:attribute>
								<xsl:attribute name="NameNumber"><xsl:value-of select="TravelerRefNumber/@RPH"/><xsl:choose><xsl:when test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'"><xsl:text>.1</xsl:text></xsl:when><xsl:otherwise><xsl:text>.1</xsl:text></xsl:otherwise></xsl:choose></xsl:attribute>
							</PassengerType>
						</xsl:if>
					</xsl:for-each>
					<Ticketing>
						<xsl:attribute name="TicketType">7TAW</xsl:attribute>
						<xsl:attribute name="TicketTimeLimit"><xsl:value-of select="Ticketing/@TicketTimeLimit"/></xsl:attribute>
						<xsl:if test="Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!=''">
							<xsl:choose>
								<xsl:when test="Queue/@QueueNumber!=''">
									<xsl:attribute name="PseudoCityCode"><xsl:value-of select="Queue/@PseudoCityCode"/></xsl:attribute>
									<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/></xsl:attribute>
								</xsl:when>
								<xsl:otherwise>
									<xsl:attribute name="PseudoCityCode"><xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/></xsl:attribute>
									<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/></xsl:attribute>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:if>
					</Ticketing>
				</TravelerInfo>
				<Remarks>
					<PaymentDetails>
						<PaymentDetail Type="CC">
							<PaymentCard>
								<xsl:attribute name="CardCode"><xsl:choose><xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MC'">CA</xsl:when><xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='DS'">DI</xsl:when><xsl:otherwise><xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/></xsl:otherwise></xsl:choose></xsl:attribute>
								<xsl:attribute name="CardNumber"><xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/></xsl:attribute>
								<xsl:attribute name="ExpireDate"><xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate"/></xsl:attribute>
							</PaymentCard>
						</PaymentDetail>
					</PaymentDetails>
				</Remarks>
				<SeatRequest>
				</SeatRequest>
				<PriceQuoteInfo>
				</PriceQuoteInfo>
				<EndTransactionRQ>
					<UpdatedBy>
						<TPA_Extensions>
							<Access>
								<AccessPerson>
									<GivenName>
										<xsl:choose>
											<xsl:when test="POS/Source/@AgentSine != ''">
												<xsl:value-of select="POS/Source/@AgentSine"/>
											</xsl:when>
											<xsl:otherwise>TRIPXML</xsl:otherwise>
										</xsl:choose>
									</GivenName>
								</AccessPerson>
							</Access>
						</TPA_Extensions>
					</UpdatedBy>
					<EndTransaction Ind="true">
						<SendEmail Ind="true"/>
					</EndTransaction>
				</EndTransactionRQ>
				<Queue>
					<QPlace>
						<xsl:attribute name="QueueNumber"><xsl:value-of select="OTA_AirBookRQ/Queue/@QueueNumber"/></xsl:attribute>
						<xsl:attribute name="SystemCode"><xsl:value-of select="OTA_AirBookRQ/Queue/@QueueCategory"/></xsl:attribute>
						<xsl:attribute name="PseudoCityCode"><xsl:value-of select="OTA_AirBookRQ/Queue/@PseudoCityCode"/></xsl:attribute>
					</QPlace>
				</Queue>
			</PassengerDetailsRQ>
		</PassengerDetails>
		<!--*******************Enhanced AirBook**************************-->
		<Enhanced_AirBook>
			<Enhanced_AirBookRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" TimeStamp="2001-12-17T09:30:47-05:00" Version="OTA_2007A.TsabreXML1.2.1">
				<POS>
					<Source>
						<xsl:choose>
							<xsl:when test="POS/Source/@AgentSin !=''">
								<xsl:attribute name="AgentSine"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<xsl:attribute name="AirlineVendorID"><xsl:value-of select="Airline/@Code"/></xsl:attribute>
						<xsl:attribute name="PseudoCityCode"><xsl:value-of select="$PCC"/></xsl:attribute>
						
					</Source>
				</POS>
				<AirBookRQ>
					<AirItinerary>
						<HaltOnError Ind="true"/>
						<RedisplayReservation WaitInterval="5000" NumAttempts="2" HaltOnUCStatus="true"/>
						<OriginDestinationOptions>
							<xsl:apply-templates select="AirItinerary/OriginDestinationOptions/OriginDestinationOption"/>
						</OriginDestinationOptions>
					</AirItinerary>
				</AirBookRQ>
				<AirPriceRQ>
					<HaltOnError Ind="true"/>
					<PriceRequestInformation>
						<xsl:attribute name="CurrencyCode"><xsl:value-of select="PassengerFare/BaseFare/@CurrencyCode"/></xsl:attribute>
					</PriceRequestInformation>
				</AirPriceRQ>
				<IgnoreAfter>
					<HaltOnError Ind="true"/>
					<IgnoreTransaction Ind="true"/>
				</IgnoreAfter>
			</Enhanced_AirBookRQ>
		</Enhanced_AirBook>
		<!-- ******************************************** -->
	</xsl:template>
	<xsl:template match="PNRData" mode="ixplore">
		<POS>
			<Source>
				<xsl:attribute name="AgentSine"><xsl:choose><xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:when><xsl:otherwise>TRIPXML</xsl:otherwise></xsl:choose></xsl:attribute>
				<xsl:attribute name="AirlineVendorID"><xsl:choose><xsl:when test="OperatingAirline"><xsl:value-of select="OperatingAirline/@Code"/></xsl:when><xsl:otherwise><xsl:value-of select="MarketingAirline/@Code"/></xsl:otherwise></xsl:choose></xsl:attribute>
				<xsl:attribute name="PseudoCityCode"><xsl:value-of select="$PCC"/></xsl:attribute>
			</Source>
		</POS>
	</xsl:template>
	<xsl:template match="PNRData" mode="other">
		<TravelItineraryAddInfoRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.2.1">
			<POS>
				<Source>
					<xsl:attribute name="AgentSine"><xsl:choose><xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:when><xsl:otherwise>TRIPXML</xsl:otherwise></xsl:choose></xsl:attribute>
					<xsl:attribute name="AirlineVendorID"><xsl:choose><xsl:when test="OperatingAirline"><xsl:attribute name="Code"><xsl:value-of select="OperatingAirline/@Code"/></xsl:attribute></xsl:when><xsl:otherwise><xsl:attribute name="Code"><xsl:value-of select="MarketingAirline/@Code"/></xsl:attribute></xsl:otherwise></xsl:choose></xsl:attribute>
					<xsl:attribute name="PseudoCityCode"><xsl:value-of select="$PCC"/></xsl:attribute>
				</Source>
			</POS>
			<CustomerInfo>
				<xsl:apply-templates select="Traveler"/>
				<xsl:apply-templates select="Telephone" mode="other"/>
				<!--xsl:if test="$RefNumber = '1'"-->
				<Email>
					<xsl:attribute name="EmailAddress"><xsl:value-of select="TPA_Extensions/PNRData/Email"/></xsl:attribute>
					<!--xsl:attribute name="NameNumber">
							<xsl:value-of select="$RefNumber" />
							<xsl:text>.1</xsl:text>
						</xsl:attribute-->
				</Email>
				<!--/xsl:if-->
				<xsl:for-each select="Traveler">
					<xsl:variable name="RefNumber">
						<xsl:choose>
							<xsl:when test="TravelerRefNumber/@RPH != ''">
								<xsl:value-of select="TravelerRefNumber/@RPH"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="position()"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:apply-templates select="CustLoyalty" mode="other">
						<xsl:with-param name="RefNumber">
							<xsl:value-of select="$RefNumber"/>
						</xsl:with-param>
					</xsl:apply-templates>
				</xsl:for-each>
				<xsl:if test="AccountingLine!=''">
					<CustomerIdentifier>
						<xsl:attribute name="Identifier"><xsl:value-of select="AccountingLine"/></xsl:attribute>
					</CustomerIdentifier>
				</xsl:if>
				<xsl:for-each select="Traveler">
					<xsl:variable name="refnum">
						<xsl:choose>
							<xsl:when test="TravelerRefNumber/@RPH != ''">
								<xsl:value-of select="TravelerRefNumber/@RPH"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="position()"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:if test="@PassengerTypeCode != '' and $paxtype='Y'">
						<PassengerType>
							<xsl:attribute name="Code"><xsl:value-of select="@PassengerTypeCode"/></xsl:attribute>
							<xsl:attribute name="NameNumber"><xsl:value-of select="$refnum"/><xsl:choose><xsl:when test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'"><xsl:text>.1</xsl:text></xsl:when><xsl:otherwise><xsl:text>.1</xsl:text></xsl:otherwise></xsl:choose></xsl:attribute>
						</PassengerType>
					</xsl:if>
				</xsl:for-each>
			</CustomerInfo>
			<AgencyInfo>
				<Ticketing>
					<xsl:attribute name="TicketingDate"><xsl:choose><xsl:when test="Ticketing/@TicketTimeLimit!=''"><xsl:value-of select="substring(Ticketing/@TicketTimeLimit,1,10)"/></xsl:when><xsl:otherwise><xsl:value-of select="substring(../../OTA_AirBookRQ/Ticketing/@TicketTimeLimit,1,10)"/></xsl:otherwise></xsl:choose></xsl:attribute>
					<xsl:if test="Queue/@QueueNumber!='' or ../../OTA_AirBookRQ/Queue/@QueueNumber!=''">
						<xsl:choose>
							<xsl:when test="Queue/@QueueNumber!=''">
								<xsl:attribute name="PseudoCityCode"><xsl:value-of select="Queue/@PseudoCityCode"/></xsl:attribute>
								<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(Queue/@QueueNumber,'000')"/></xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="PseudoCityCode"><xsl:value-of select="../../OTA_AirBookRQ/Queue/@PseudoCityCode"/></xsl:attribute>
								<xsl:attribute name="QueueNumber"><xsl:value-of select="format-number(../../OTA_AirBookRQ/Queue/@QueueNumber,'000')"/></xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:if>
				</Ticketing>
			</AgencyInfo>
		</TravelItineraryAddInfoRQ>
	</xsl:template>
	<xsl:template match="Traveler">
		<xsl:variable name="RefNumber">
			<xsl:choose>
				<xsl:when test="TravelerRefNumber/@RPH != ''">
					<xsl:value-of select="TravelerRefNumber/@RPH"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="position()"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<PersonName>
			<xsl:attribute name="RPH"><xsl:value-of select="$RefNumber"/></xsl:attribute>
			<GivenName>
				<xsl:value-of select="PersonName/GivenName"/>
				<xsl:if test="PersonName/NamePrefix != ''">
					<xsl:text> </xsl:text>
					<xsl:value-of select="PersonName/NamePrefix"/>
				</xsl:if>
			</GivenName>
			<Surname>
				<xsl:value-of select="PersonName/Surname"/>
			</Surname>
			<xsl:if test="@PassengerTypeCode = 'INF' or @PassengerTypeCode = 'ITF'">
				<Infant Ind="true"/>
			</xsl:if>
		</PersonName>
	</xsl:template>
</xsl:stylesheet>
<!-- Stylus Studio meta-information - (c) 2004-2009. Progress Software Corporation. All rights reserved.

<metaInformation>
	<scenarios>
		<scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="..\..\..\Documents and Settings\suchin\Desktop\tr_buildRQ.xml" htmlbaseurl="" outputurl="" processortype="saxon8" useresolver="yes" profilemode="0"
		          profiledepth="" profilelength="" urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no"
		          validator="internal" customvalidator="">
			<advancedProp name="sInitialMode" value=""/>
			<advancedProp name="bXsltOneIsOkay" value="true"/>
			<advancedProp name="bSchemaAware" value="true"/>
			<advancedProp name="bXml11" value="false"/>
			<advancedProp name="iValidation" value="0"/>
			<advancedProp name="bExtensions" value="true"/>
			<advancedProp name="iWhitespace" value="0"/>
			<advancedProp name="sInitialTemplate" value=""/>
			<advancedProp name="bTinyTree" value="true"/>
			<advancedProp name="bWarnings" value="true"/>
			<advancedProp name="bUseDTD" value="false"/>
			<advancedProp name="iErrorHandling" value="fatal"/>
		</scenario>
	</scenarios>
	<MapperMetaTag>
		<MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no"/>
		<MapperBlockPosition></MapperBlockPosition>
		<TemplateContext></TemplateContext>
		<MapperFilter side="source"></MapperFilter>
	</MapperMetaTag>
</metaInformation>
-->
