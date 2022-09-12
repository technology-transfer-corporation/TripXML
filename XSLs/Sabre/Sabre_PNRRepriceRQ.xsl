<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- 
   ================================================================== 
   Sabre_PNRRepriceRQ.xsl															
   ================================================================== 
   Date: 25 May 2022 - Samokhvalov - Fixed FareBasis (truncation to 8 chars removed).
   Date: 18 May 2022 - Kobelev - Fixed Ticket Designator in RePrice request (According to Irina, regardless if Discount is 0 still have to pass Ticket Desgnator as Discount).
   Date: 17 May 2022 - Kobelev - Added Tour Code to RePrice request.
   Date: 09 May 2022 - Samokhvalov - Grouped ItineraryOptions/SegmentSelect
   Date: 15 Feb 2022 - Kobelev - *ZZ + Ticket Designator and FareBases with Passanger association. Amount equivalent Sabre host command: WPQY/AD75/DA100.00‡N1.1-3.1
   Date: 15 Feb 2022 - Kobelev - *ZZ + Ticket Designator and FareBases. Amount equivalent Sabre host command: WPQY/AD75/DA100.00 
   Date: 14 Feb 2022 - Kobelev - Markup / Commssion with Branded Fare.
   Date: 02 Dec 2021 - Kobelev - Smart Pricing Capabilities moved in priority of execution.
   Date: 01 Dec 2021 - Kobelev - Smart Pricing Capabilities with Passanger Associations.
   Date: 30 Nov 2021 - Kobelev - Smart Pricing Capabilities.
   Date: 08 Jul 2021 - Kobelev - BrandedFares with Markup Fix.
   Date: 08 Jul 2021 - Kobelev - Group by Branded Fahttps://www.facebook.com/gaming/?ref=games_tabre fix.
   Date: 25 Jun 2021 - Babin - Added Branded Fares Joined price between different PCCs
   Date: 10 Jun 2021 - Kobelev - Order of Pricing Qualifiers fields fix
   Date: 06 Jun 2021 - Kobelev - Branded Fare with Ticket Designator fix
   Date: 02 Jun 2021 - Kobelev - Fixed Store Fares Indicator
   Date: 27 May 2021 - Kobelev - Fix Segment Reference for Branded Fares
   Date: 12 May 2021 - Babin - Added Passenger Type for Branded Fares
   Date: 13 Aug 2020 - Samokhvalov - Error fixes - CommandPricing@RPH must be combined with SegmentSelect@RPH
   Date: 17 Jul 2020 - Kobelev - Branded Fares and upgraded OTA_AirPriceRQ to 2.17.0
   Date: 29 Mar 2016 - Rastko - upgraded ReadRQ to version 3.6.0			
   Date: 14 Dec 2015 - Rastko - added entry for combinable fares		
   Date: 12 Apr 2014 - Rastko - corrected pricing command to send only one request		
   Date: 25 Feb 2014 - Rastko - implemented full support for ticket designator			
   Date: 19 Feb 2014 - Rastko - addes support for discount and ticket designator		
   Date: 13 Feb 2014 - Rastko - Moved PlusUp mapping after passenger type			
   Date: 12 Feb 2014 - Rastko - included tag Passengers so business logic can use it		
   Date: 08 Feb 2014 - Rastko - New file											
   ================================================================== 
   -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:key name="storedFareByFF" match="StoredFare" use="BrandedFares/FareFamily"/>
	<xsl:key name="storedFareByPTC" match="StoredFare" use="PassengerType/@Code"/>
	<xsl:key name="fbcBysf" match="FareSegments" use="AirSegments/text()"/>

	<xsl:key name="nameDistinct" match="AA[@Aattr = 'xyz1']/BB" use="@bAttr2"/>

	<xsl:template match="/">
		<OTA_PNRRepriceRQ>
			<xsl:apply-templates select="OTA_PNRRepriceRQ"/>
		</OTA_PNRRepriceRQ>
	</xsl:template>

	<xsl:template match="OTA_PNRRepriceRQ">
		<PNRRead>
			<TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
				<MessagingDetails>
					<SubjectAreas>
						<SubjectArea>FULL</SubjectArea>
					</SubjectAreas>
				</MessagingDetails>
				<UniqueID>
					<xsl:attribute name="ID">
						<xsl:value-of select="UniqueID/@ID" />
					</xsl:attribute>
				</UniqueID>
			</TravelItineraryReadRQ>
		</PNRRead>
		<PNRRedisplay>
			<TravelItineraryReadRQ Version="3.6.0" xmlns="http://services.sabre.com/res/tir/v3_6">
				<MessagingDetails>
					<SubjectAreas>
						<SubjectArea>FULL</SubjectArea>
					</SubjectAreas>
				</MessagingDetails>
			</TravelItineraryReadRQ>
		</PNRRedisplay>
		<Cryptic>
			<SabreCommandLLSRQ xmlns="http://webservices.sabre.com/sabreXML/2003/07" Version="2003A.TsabreXML1.6.1">
				<xsl:element name="Request">
					<xsl:attribute name="Output">SCREEN</xsl:attribute>
					<xsl:attribute name="MDRSubset">AD01</xsl:attribute>
					<xsl:attribute name="CDATA">true</xsl:attribute>
					<xsl:element name="HostCommand">
						<xsl:value-of select="'*PQS'" />
					</xsl:element>
				</xsl:element>
			</SabreCommandLLSRQ>
		</Cryptic>
		<Price>
			<xsl:choose>
				<!-- count(StoredFare/TicketDesignator)=count(StoredFare) and  -->
				<xsl:when test="count(StoredFare/FareSegments) = 0">
					<OTA_AirPriceRQ Version="2.17.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
						<PriceRequestInformation>
							<xsl:variable name="sf" select="@StoreFare"/>
							<xsl:attribute name="Retain">
								<xsl:value-of select="$sf"/>
							</xsl:attribute>
							<OptionalQualifiers>
								<xsl:if test="TourCode">
									<MiscQualifiers>
										<TourCode>
											<!-- Optional -->
											<!-- Repeat Factor=0 -->
											<!-- "Ind" is used to specify to suppress the fare amount on the ticket and replace with BT. -->
											<!-- This is not applicable to ARC subscribers. -->
											<!-- Equivalent Sabre host command: WPUB*TEST1212
											<SuppressFareReplaceWithBT Ind="true"/> -->
											<!-- Optional -->
											<!-- Repeat Factor=0 -->
											<!-- "Ind" is used to specify to suppress the fare amount on the ticket and replace with IT. -->
											<!-- Equivalent Sabre host command: WPUI*TEST1212
											<SuppressFareReplaceWithIT Ind="true"/> -->
											<!-- Optional -->
											<!-- Repeat Factor=0 -->
											<!-- "Ind" is used to specify to to suppress the IT in the tourcode box from printing. -->
											<!-- Equivalent Sabre host command: WPUN*TEST1212
											<SuppressIT Ind="true"/> -->
											<!-- Optional -->
											<!-- Repeat Factor=0 -->
											<!-- "Ind" is used to specify to specify to suppress IT from printing in the tour box on the ticket and to suppress      fare amounts from printing on the ticket. -->
											<!-- Equivalent Sabre host command: WPUX*TEST1212
											<SuppressITSupressFare Ind="true"/> -->
											<!-- Optional -->
											<!-- Repeat Factor=0 -->
											<!-- "Text" is used to specify tour code. -->
											<!-- Equivalent Sabre host command: WPUTEST1212 -->
											<Text>
												<xsl:value-of select="TourCode"/>
											</Text>
										</TourCode>
									</MiscQualifiers>
								</xsl:if>

								<PricingQualifiers>
									<xsl:choose>
										<xsl:when test="StoredFare[1]/BrandedFares">
											<xsl:apply-templates select="StoredFare[1]/BrandedFares" mode="FareFamily" />
										</xsl:when>
										<xsl:otherwise>
											<xsl:if test="StoredFare[1]/Discount/@Percent!='' or StoredFare[1]/TicketDesignator!=''">

												<xsl:apply-templates select="StoredFare[1]" mode="CommandPricing" />

												<ItineraryOptions>
													<xsl:call-template name="GetItineraryOptions"/>
													<!--<xsl:for-each select="FlightReference">
														<SegmentSelect Number="{@RPH}" RPH="{@RPH}"/>
													</xsl:for-each>-->
												</ItineraryOptions>

											</xsl:if>
										</xsl:otherwise>
									</xsl:choose>

									<xsl:if test="PassengerType and count(StoredFare)>1">
										<xsl:for-each select="PassengerType">
											<PassengerType>
												<xsl:attribute name="Quantity">
													<xsl:value-of select="@Quantity"/>
												</xsl:attribute>
												<xsl:attribute name="Code">
													<xsl:choose>
														<xsl:when test="@Code='CHD'">CNN</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="@Code"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:attribute>
											</PassengerType>
										</xsl:for-each>
									</xsl:if>

									<xsl:if test="StoredFare[1]/Markup/@Amount!=''">
										<PlusUp Amount="{Markup/@Amount}"/>
									</xsl:if>

								</PricingQualifiers>
							</OptionalQualifiers>
						</PriceRequestInformation>
					</OTA_AirPriceRQ>
				</xsl:when>
				<xsl:when test="StoredFare/FareSegments">
					<xsl:for-each select="StoredFare[generate-id() = generate-id(key('storedFareByPTC', PassengerType/@Code)[1])]">
						<xsl:apply-templates select="." mode="SmartPricingAll" />
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="count(StoredFare/FareSegments/AirSegments/@TicketDesignator) > 0 and count(StoredFare[FareSegments/AirSegments/@TicketDesignator])!=count(StoredFare)">

					<OTA_AirPriceRQ Version="2.17.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
						<PriceRequestInformation>
							<xsl:apply-templates select="." mode="td"/>
						</PriceRequestInformation>
					</OTA_AirPriceRQ>
					<OTA_AirPriceRQ Version="2.17.0" xmlns="http://webservices.sabre.com/sabreXML/2011/10">
						<PriceRequestInformation>
							<xsl:apply-templates select="." mode="simple"/>
						</PriceRequestInformation>
					</OTA_AirPriceRQ>

				</xsl:when>
				<xsl:when test="count(StoredFare/BrandedFares/FareFamily) > 1 and not(StoredFare/Markup)">

					<xsl:variable name="ffList" select="StoredFare"/>
					<xsl:variable name="sfflag" select="@StoreFare"/>

					<xsl:variable name="bfList">
						<xsl:call-template name="StoredFareGroup">
							<xsl:with-param name="list" select="$ffList"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:variable name="groupedSF">
						<xsl:call-template name="tokenizeString">
							<xsl:with-param name="list" select="$bfList"/>
							<xsl:with-param name="delimiter" select="','"/>
						</xsl:call-template>
					</xsl:variable>

					<xsl:for-each select="msxsl:node-set($groupedSF)/elem/node()[1]">
						<xsl:variable name="bfName" select="." />
						<xsl:call-template name="SFGrouping">
							<xsl:with-param name="paramSF" select="$ffList[BrandedFares/FareFamily/text()=$bfName]" />
							<xsl:with-param name="sf" select="$sfflag" />
						</xsl:call-template>
					</xsl:for-each>

				</xsl:when>
				<xsl:otherwise>
					<xsl:for-each select="StoredFare">
						<OTA_AirPriceRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.17.0">
							<PriceRequestInformation>
								<xsl:variable name="sf" select="../@StoreFare"/>
								<xsl:attribute name="Retain">
									<xsl:value-of select="$sf"/>
								</xsl:attribute>
								<OptionalQualifiers>
									<PricingQualifiers>
										<xsl:choose>
											<xsl:when test="BrandedFares">
												<xsl:apply-templates select="BrandedFares" mode="FareFamily" />
											</xsl:when>
											<xsl:when test="Discount/@Amount!='' or Discount/@Percent!='' or StoredFare/FareSegments/AirSegments/@TicketDesignator !=''">
												<CommandPricing>CP</CommandPricing>
											</xsl:when>
										</xsl:choose>
										<!--</xsl:otherwise>-->
										<NameSelect>NS</NameSelect>
										<!--<xsl:otherwise>-->
										<xsl:if test="PassengerType">
											<PassengerType>
												<xsl:attribute name="Quantity">
													<xsl:value-of select="PassengerType/@Quantity"/>
												</xsl:attribute>
												<xsl:attribute name="Code">
													<xsl:choose>
														<xsl:when test="PassengerType/@Code='CHD'">C09</xsl:when>
														<xsl:otherwise>
															<xsl:value-of select="PassengerType/@Code"/>
														</xsl:otherwise>
													</xsl:choose>
												</xsl:attribute>
											</PassengerType>
										</xsl:if>
										<xsl:if test="Markup/@Amount!=''">
											<PlusUp Amount="{Markup/@Amount}"/>
										</xsl:if>
									</PricingQualifiers>
								</OptionalQualifiers>
							</PriceRequestInformation>
						</OTA_AirPriceRQ>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
		</Price>
		<PriceCombined>
			<OTA_AirPriceRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.17.0">
				<PriceRequestInformation>
					<xsl:variable name="sf" select="@StoreFare"/>
					<xsl:attribute name="Retain">
						<xsl:value-of select="$sf"/>
					</xsl:attribute>
					<OptionalQualifiers>
						<PricingQualifiers>
							<xsl:choose>
								<xsl:when test="StoredFare[1]/BrandedFares">
									<xsl:apply-templates select="StoredFare[1]/BrandedFares" mode="FareFamily" >
										<xsl:with-param name="skipTD">1</xsl:with-param>
									</xsl:apply-templates>
								</xsl:when>
								<xsl:otherwise>
									<ItineraryOptions>
										<xsl:call-template name="GetItineraryOptions"/>										
									</ItineraryOptions>
								</xsl:otherwise>
							</xsl:choose>
							<NameSelect>NS</NameSelect>
							<xsl:if test="StoredFare[1]/PassengerType">
								<xsl:for-each select="StoredFare">
									<PassengerType>
										<xsl:attribute name="Quantity">
											<xsl:value-of select="PassengerType/@Quantity"/>
										</xsl:attribute>
										<xsl:attribute name="Code">
											<xsl:choose>
												<xsl:when test="PassengerType/@Code='CHD'">C09</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="PassengerType/@Code"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
									</PassengerType>
								</xsl:for-each>
							</xsl:if>
							<xsl:for-each select="StoredFare[1]/FareSegments/AirSegments[position()=1 or not(text()=preceding-sibling::AirSegments[1]/text())]">
								<!--<xsl:for-each select="StoredFare[1]/FareSegments/AirSegments">-->
								<xsl:if test=". != 'VOID'">
									<SpecificFare RPH="{@RPH}">
										<FareBasis>
											<xsl:variable name="fbc" select="." />
											<!--<xsl:variable name="fbc" select="substring(.,1,8)" />-->
											<xsl:value-of select="$fbc"/>
										</FareBasis>
									</SpecificFare>
								</xsl:if>
							</xsl:for-each>
							<xsl:if test="StoredFare[1]/Markup/@Amount!=''">
								<PlusUp Amount="{StoredFare[1]/Markup/@Amount}"/>
							</xsl:if>

						</PricingQualifiers>
					</OptionalQualifiers>
				</PriceRequestInformation>
			</OTA_AirPriceRQ>
		</PriceCombined>
		<xsl:if test="not(Ticketing/@EndTransact='false')">
			<ET>
				<EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.2">
					<EndTransaction Ind="true"/>
					<Source>
						<xsl:attribute name="ReceivedFrom">
							<xsl:choose>
								<xsl:when test="POS/Source/@AgentSine != ''">
									<xsl:value-of select="POS/Source/@AgentSine"/>
								</xsl:when>
								<xsl:otherwise>TRIPXML</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</Source>
				</EndTransactionRQ>
			</ET>
			<ReET>
				<EndTransactionRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.0.2">
					<EndTransaction Ind="true"/>
				</EndTransactionRQ>
			</ReET>
		</xsl:if>
	</xsl:template>

	<xsl:template match="OTA_PNRRepriceRQ" mode="td">

		<xsl:variable name="sf" select="@StoreFare"/>
		<xsl:attribute name="Retain">
			<xsl:value-of select="$sf"/>
		</xsl:attribute>
		<OptionalQualifiers>
			<PricingQualifiers>
				<xsl:choose>
					<xsl:when test="StoredFare[FareSegments/AirSegments/@TicketDesignator]/BrandedFares">
						<xsl:variable name="firstBrand" select="StoredFare[FareSegments/AirSegments/@TicketDesignator]/BrandedFares"></xsl:variable>
						<xsl:apply-templates select="$firstBrand[1]" mode="FareFamily" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:if test="StoredFare[FareSegments/AirSegments/@TicketDesignator]/Discount/@Percent!='' or StoredFare[FareSegments/AirSegments/@TicketDesignator]/TicketDesignator!=''">
							<xsl:apply-templates select="StoredFare[FareSegments/AirSegments/@TicketDesignator]" mode="CommandPricing"/>
							<ItineraryOptions>
								<xsl:call-template name="GetItineraryOptions"/>
								<!--<xsl:for-each select="FlightReference">
									<SegmentSelect Number="{@RPH}" RPH="{@RPH}"/>
								</xsl:for-each>-->
							</ItineraryOptions>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>

				<xsl:if test="StoredFare[FareSegments/AirSegments/@TicketDesignator]/PassengerType and count(StoredFare)>1">
					<NameSelect>NS</NameSelect>
					<xsl:for-each select="StoredFare[FareSegments/AirSegments/@TicketDesignator]/PassengerType">
						<PassengerType>
							<xsl:attribute name="Quantity">
								<xsl:value-of select="@Quantity"/>
							</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="@Code='CHD'">CNN</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</PassengerType>
					</xsl:for-each>
				</xsl:if>

				<xsl:if test="StoredFare[FareSegments/AirSegments/@TicketDesignator]/Markup/@Amount!=''">
					<PlusUp Amount="{Markup/@Amount}"/>
				</xsl:if>

			</PricingQualifiers>
		</OptionalQualifiers>

	</xsl:template>

	<xsl:template match="OTA_PNRRepriceRQ" mode="simple">

		<xsl:variable name="sf" select="@StoreFare"/>
		<xsl:attribute name="Retain">
			<xsl:value-of select="$sf"/>
		</xsl:attribute>
		<OptionalQualifiers>
			<PricingQualifiers>
				<xsl:choose>
					<xsl:when test="StoredFare[count(FareSegments/AirSegments/@TicketDesignator)=0]/BrandedFares">
						<xsl:apply-templates select="StoredFare[count(FareSegments/AirSegments/@TicketDesignator)=0]/BrandedFares" mode="FareFamily" />
					</xsl:when>
					<xsl:otherwise>

					</xsl:otherwise>
				</xsl:choose>

				<xsl:if test="StoredFare[count(FareSegments/AirSegments/@TicketDesignator)=0]/PassengerType">
					<NameSelect>NS</NameSelect>
					<xsl:for-each select="StoredFare[count(FareSegments/AirSegments/@TicketDesignator)=0]/PassengerType">
						<PassengerType>
							<xsl:attribute name="Quantity">
								<xsl:value-of select="@Quantity"/>
							</xsl:attribute>
							<xsl:attribute name="Code">
								<xsl:choose>
									<xsl:when test="@Code='CHD'">CNN</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="@Code"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</PassengerType>
					</xsl:for-each>
				</xsl:if>

				<xsl:if test="StoredFare[count(FareSegments/AirSegments/@TicketDesignator)=0]/Markup/@Amount!=''">
					<PlusUp Amount="{Markup/@Amount}"/>
				</xsl:if>

			</PricingQualifiers>
		</OptionalQualifiers>

	</xsl:template>

	<xsl:template name="SFGrouping">
		<xsl:param name="paramSF"/>
		<xsl:param name="sf"/>
		<OTA_AirPriceRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.17.0">
			<PriceRequestInformation>
				<xsl:attribute name="Retain">
					<xsl:value-of select="$sf"/>
				</xsl:attribute>
				<OptionalQualifiers>
					<PricingQualifiers>
						<!--<xsl:choose>-->
						<xsl:if test="$paramSF[1]/BrandedFares">
							<xsl:apply-templates select="$paramSF[1]/BrandedFares" mode="FareFamily" />
						</xsl:if>
						<!--<xsl:otherwise>-->
						<NameSelect>NS</NameSelect>
						<xsl:if test="$paramSF[1]/PassengerType">
							<xsl:for-each select="$paramSF">
								<PassengerType>
									<xsl:attribute name="Quantity">
										<xsl:value-of select="PassengerType/@Quantity"/>
									</xsl:attribute>
									<xsl:attribute name="Code">
										<xsl:choose>
											<xsl:when test="PassengerType/@Code='CHD'">C09</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="PassengerType/@Code"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
								</PassengerType>

							</xsl:for-each>
						</xsl:if>
						<xsl:if test="$paramSF[1]/Markup/@Amount!=''">
							<PlusUp Amount="{$paramSF[1]/Markup/@Amount}"/>
						</xsl:if>
						<!--
              </xsl:otherwise>
              </xsl:choose>
              <xsl:if test="StoredFare[1]/Markup/@Amount!=''">
                <PlusUp Amount="{StoredFare[1]/Markup/@Amount}"/>
              </xsl:if>
            -->
					</PricingQualifiers>
				</OptionalQualifiers>
			</PriceRequestInformation>
		</OTA_AirPriceRQ>
	</xsl:template>

	<!--
**********************************************
  Branded Fares
**********************************************
-->
	<xsl:template match="BrandedFares" mode="FareFamily">
		<xsl:param name="skipTD" select="0" />
		<xsl:for-each select="FareFamily">
			<Brand>
				<xsl:attribute name="RPH">
					<xsl:value-of select="@RPH"/>
				</xsl:attribute>
				<xsl:value-of select="."/>
			</Brand>
		</xsl:for-each>

		<xsl:if test="$skipTD != 1 and (../Discount/@Percent!='' or ../TicketDesignator!='')">
			<xsl:for-each select="FareFamily">
				<CommandPricing>
					<xsl:attribute name="RPH" >
						<xsl:value-of select="@RPH"/>
					</xsl:attribute>
					<xsl:if test="../../Discount/@Percent!=''">
						<Discount>
							<xsl:if test="../../Discount/@Percent!=''">
								<xsl:attribute name="Percent">
									<xsl:value-of select="../../Discount/@Percent"/>
								</xsl:attribute>
							</xsl:if>
							<xsl:if test="../../TicketDesignator!=''">
								<xsl:attribute name="AuthCode">
									<xsl:value-of select="../../TicketDesignator"/>
								</xsl:attribute>
							</xsl:if>
						</Discount>
					</xsl:if>
				</CommandPricing>
			</xsl:for-each>
		</xsl:if>

		<ItineraryOptions>
			<xsl:call-template name="GetItineraryOptions"/>
			<!--<xsl:for-each select="FareFamily">
				<SegmentSelect Number="{@RPH}" RPH="{@RPH}"/>
			</xsl:for-each>-->
		</ItineraryOptions>

	</xsl:template>

	<xsl:template match="StoredFare" mode="SmartPricingAll">
		<xsl:variable name="ptc" select="PassengerType/@Code" />

		<OTA_AirPriceRQ xmlns="http://webservices.sabre.com/sabreXML/2011/10" Version="2.17.0">
			<PriceRequestInformation>
				<xsl:variable name="sf" select="../@StoreFare"/>
				<xsl:attribute name="Retain">
					<xsl:value-of select="$sf"/>
				</xsl:attribute>
				<OptionalQualifiers>
					<xsl:if test="TourCode">
						<MiscQualifiers>
							<TourCode>
								<!-- Optional -->
								<!-- Repeat Factor=0 -->
								<!-- "Ind" is used to specify to suppress the fare amount on the ticket and replace with BT. -->
								<!-- This is not applicable to ARC subscribers. -->
								<!-- Equivalent Sabre host command: WPUB*TEST1212
											<SuppressFareReplaceWithBT Ind="true"/> -->
								<!-- Optional -->
								<!-- Repeat Factor=0 -->
								<!-- "Ind" is used to specify to suppress the fare amount on the ticket and replace with IT. -->
								<!-- Equivalent Sabre host command: WPUI*TEST1212
											<SuppressFareReplaceWithIT Ind="true"/> -->
								<!-- Optional -->
								<!-- Repeat Factor=0 -->
								<!-- "Ind" is used to specify to to suppress the IT in the tourcode box from printing. -->
								<!-- Equivalent Sabre host command: WPUN*TEST1212
											<SuppressIT Ind="true"/> -->
								<!-- Optional -->
								<!-- Repeat Factor=0 -->
								<!-- "Ind" is used to specify to specify to suppress IT from printing in the tour box on the ticket and to suppress      fare amounts from printing on the ticket. -->
								<!-- Equivalent Sabre host command: WPUX*TEST1212
											<SuppressITSupressFare Ind="true"/> -->
								<!-- Optional -->
								<!-- Repeat Factor=0 -->
								<!-- "Text" is used to specify tour code. -->
								<!-- Equivalent Sabre host command: WPUTEST1212 -->
								<Text>
									<xsl:value-of select="TourCode"/>
								</Text>
							</TourCode>
						</MiscQualifiers>
					</xsl:if>
					<PricingQualifiers>
						<xsl:if test="FareSegments">
							<xsl:apply-templates select="FareSegments" mode="SmartPricing" />
						</xsl:if>						
					</PricingQualifiers>
				</OptionalQualifiers>
			</PriceRequestInformation>
		</OTA_AirPriceRQ>
		
	</xsl:template>

	<!--<xsl:key name="keySegs" match="FB/text()" use="." />-->

	<xsl:template match="FareSegments" mode="SmartPricing">
		<xsl:variable name="ptc" select="../PassengerType" />

		<xsl:choose>
			<xsl:when test="AirSegments/@TicketDesignator!=''">
				<xsl:apply-templates select="../../StoredFare[PassengerType/@Code=$ptc/@Code]" mode="CommandPricing" />
				<ItineraryOptions>
					<xsl:call-template name="GetItineraryOptions"/>
					<!--<xsl:for-each select="../../FlightReference">
						<SegmentSelect Number="{@RPH}" RPH="{@RPH}"/>
					</xsl:for-each>-->
				</ItineraryOptions>
				<NameSelect>NS</NameSelect>
				<xsl:if test="../PassengerType">
					<PassengerType>
						<xsl:attribute name="Quantity">
							<xsl:value-of select="$ptc/@Quantity"/>
						</xsl:attribute>
						<xsl:attribute name="Code">
							<xsl:choose>
								<xsl:when test="$ptc/@Code='CHD'">C09</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$ptc/@Code"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</PassengerType>
					<xsl:if test="../Markup/@Amount!=''">
						<PlusUp Amount="{../Markup/@Amount}"/>
					</xsl:if>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<ItineraryOptions>
					<xsl:call-template name="GetItineraryOptions"/>
				</ItineraryOptions>
				<NameSelect>NS</NameSelect>
				<xsl:if test="../PassengerType">
					<PassengerType>
						<xsl:attribute name="Quantity">
							<xsl:value-of select="$ptc/@Quantity"/>
						</xsl:attribute>
						<xsl:attribute name="Code">
							<xsl:choose>
								<xsl:when test="$ptc/@Code='CHD'">C09</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$ptc/@Code"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
					</PassengerType>
					<xsl:if test="../Markup/@Amount!=''">
						<PlusUp Amount="{../Markup/@Amount}"/>
					</xsl:if>
				</xsl:if>
				<xsl:for-each select="AirSegments[position()=1 or not(text()=preceding-sibling::AirSegments[1]/text())]">
					<!--<xsl:for-each select="AirSegments">-->
					<SpecificFare RPH="{@RPH}">
						<FareBasis>
							<!--<xsl:variable name="fbc" select="substring(.,1,8)" />-->
							<xsl:value-of select="."/>
						</FareBasis>
					</SpecificFare>
				</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="GetItineraryOptions">
		<xsl:variable name="fbSegs">
			<xsl:choose>
				<xsl:when test="AirSegments">
					<xsl:for-each select="AirSegments[position()=1 or not(text()=preceding-sibling::AirSegments[1]/text())]">
						<xsl:variable name="fbCode" select="text()"/>
						<FB>
							<xsl:value-of select="@RPH" />
							<xsl:text>,</xsl:text>
							<xsl:for-each select="../AirSegments[text()=$fbCode and (text()=preceding-sibling::AirSegments[1]/text())]">
								<xsl:value-of select="@RPH" />
								<xsl:text>,</xsl:text>
							</xsl:for-each>
						</FB>
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="../FareSegments/AirSegments">
					<xsl:for-each select="../FareSegments/AirSegments[position()=1 or not(text()=preceding-sibling::AirSegments[1]/text())]">
						<xsl:variable name="fbCode" select="text()"/>
						<FB>
							<xsl:value-of select="@RPH" />
							<xsl:text>,</xsl:text>
							<xsl:for-each select="../FareSegments/AirSegments[text()=$fbCode and (text()=preceding-sibling::AirSegments[1]/text())]">
								<xsl:value-of select="@RPH" />
								<xsl:text>,</xsl:text>
							</xsl:for-each>
						</FB>
					</xsl:for-each>
				</xsl:when>
				<xsl:when test="StoredFare[1]/FareSegments/AirSegments">
					<xsl:for-each select="StoredFare[1]/FareSegments/AirSegments[position()=1 or not(text()=preceding-sibling::AirSegments[1]/text())]">
						<xsl:variable name="fbCode" select="text()"/>
						<FB>
							<xsl:value-of select="@RPH" />
							<xsl:text>,</xsl:text>
							<xsl:for-each select="StoredFare[1]/FareSegments/AirSegments[text()=$fbCode and (text()=preceding-sibling::AirSegments[1]/text())]">
								<xsl:value-of select="@RPH" />
								<xsl:text>,</xsl:text>
							</xsl:for-each>
						</FB>
					</xsl:for-each>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<!--<xsl:for-each select="msxsl:node-set($fbSegs)/FB/text()[generate-id() = generate-id(key('keySegs',.)[1])]">-->
		<xsl:for-each select="msxsl:node-set($fbSegs)/FB/text()">
			<SegmentSelect>
				<xsl:attribute name="Number">
					<xsl:value-of select="substring-before(.,',')"/>
				</xsl:attribute>

				<xsl:variable name="groupedSF">
					<xsl:call-template name="tokenizeString">
						<xsl:with-param name="list" select="."/>
						<xsl:with-param name="delimiter" select="','"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="contains(substring(.,1,string-length(.)-1),',')">
					<xsl:attribute name="EndNumber">
						<xsl:value-of select="msxsl:node-set($groupedSF)/elem[position()=last()]"/>
					</xsl:attribute>
				</xsl:if>

				<xsl:attribute name="RPH">
					<xsl:value-of select="substring-before(.,',')"/>
				</xsl:attribute>
			</SegmentSelect>
		</xsl:for-each>
		<!--
		<xsl:for-each select="AirSegments">
			<SegmentSelect Number="{@RPH}" RPH="{@RPH}"/>
		</xsl:for-each>
		-->
	</xsl:template>

	<xsl:template name="StoredFareGroup">
		<xsl:param name="list"/>
		<xsl:for-each select="$list[generate-id() = generate-id(key('storedFareByFF', BrandedFares/FareFamily)[1])]">
			<xsl:value-of select="concat(BrandedFares/FareFamily[1]/node()[1], ',')" />
			<!--
      <xsl:copy>
        <xsl:variable name="varPlan" select="key('storedFareByFF',  BrandedFares/FareFamily)" />
        <xsl:apply-templates select="$varPlan[1]/BrandedFare/FareFamily" />
        <channels>
          <xsl:for-each select="$varPlan">
            <xsl:apply-templates select="$varPlan[1]/BrandedFares/FareFamily[1]/node()[1]" />
          </xsl:for-each>
        </channels>
      </xsl:copy>
      -->
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="StoredFare" mode="CommandPricing">
		
		<xsl:variable name="discType">
			<xsl:choose>
				<xsl:when test="Discount/@Amount !=''">
					<xsl:text>A</xsl:text>
				</xsl:when>
				<xsl:otherwise>
					<xsl:text>P</xsl:text>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="disc">
			<xsl:choose>
				<xsl:when test="Discount/@Amount !=''">
					<xsl:value-of select="Discount/@Amount"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="Discount/@Percent"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="flt">
			<xsl:choose>
				<xsl:when test="../FlightReference">
					<xsl:copy-of select="../FlightReference" />
				</xsl:when>
				<xsl:when test="../FareSegments">
					<xsl:copy-of select="../FareSegments/AirSegments[text() != 'VOID']" />
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:for-each select="FareSegments/AirSegments[position()=1 or not(text()=preceding-sibling::AirSegments[1]/text())]">
			<!--<xsl:for-each select="FareSegments/AirSegments">-->
			<xsl:variable name="rph" select="@RPH" />
			<CommandPricing>
				<xsl:attribute name="RPH" >
					<xsl:value-of select="$rph"/>
				</xsl:attribute>
				<xsl:if test="$disc!=''"> <!-- and $disc!='0' -->
					<Discount>
						<xsl:choose>
							<xsl:when test="$discType ='P'">
								<xsl:attribute name="Percent">
									<xsl:value-of select="$disc"/>
								</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="Amount">
									<xsl:value-of select="$disc"/>
								</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
						<xsl:if test="@TicketDesignator!=''">
							<xsl:attribute name="AuthCode">
								<xsl:value-of select="@TicketDesignator"/>
							</xsl:attribute>
						</xsl:if>
					</Discount>
				</xsl:if>
				<xsl:if test="$flt">
					<FareBasis TicketDesignator="$td">
						<xsl:attribute name="Code">
							<xsl:if test="not(contains(.,'/'))">
								<xsl:value-of select="."/>
							</xsl:if>
							<xsl:if test="contains(.,'/')">
								<xsl:value-of select="substring-before(.,'/')"/>
							</xsl:if>
						</xsl:attribute>
						<xsl:attribute name="TicketDesignator">
							<xsl:value-of select="@TicketDesignator"/>
						</xsl:attribute>
					</FareBasis>
				</xsl:if>

			</CommandPricing>
		</xsl:for-each>

	</xsl:template>

	<xsl:template name="tokenizeString">
		<!--passed template parameter -->
		<xsl:param name="list"/>
		<xsl:param name="delimiter"/>
		<xsl:choose>
			<xsl:when test="contains($list, $delimiter)">
				<elem>
					<!-- get everything in front of the first delimiter -->
					<xsl:value-of select="substring-before($list,$delimiter)"/>
				</elem>
				<xsl:call-template name="tokenizeString">
					<!-- store anything left in another variable -->
					<xsl:with-param name="list" select="substring-after($list,$delimiter)"/>
					<xsl:with-param name="delimiter" select="$delimiter"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$list = ''">
						<xsl:text/>
					</xsl:when>
					<xsl:otherwise>
						<elem>
							<xsl:value-of select="$list"/>
						</elem>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

</xsl:stylesheet>

