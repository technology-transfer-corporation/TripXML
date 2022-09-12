<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- 
================================================================== 
v03_Worldspan_PNRReadRS.xsl 					     								       
==================================================================
Date: 30 Aug 2022 - Samokhvalov - OperatingAirline Code fixes
Date: 30 Aug 2022 - Samokhvalov - Added TPA_Extensions/AgencyCommission.
Date: 22 Aug 2022 - Samokhvalov - PTC_Farebreakdown flight segments fixes.
Date: 18 Aug 2022 - Samokhvalov - Special Remarks CC(Controlling Carrier) reworked.
Date: 29 Apr 2022 - Kobelev - EMD Exchange and EMD Service Fee display fix.
Date: 16 Nov 2021 - Kobelev - Airline Decoding.
Date: 21 Oct 2021 - Kobelev - Change Controlling Carrier RemarkType from "Z" to "CC".
Date: 28 Sep 2021 - Kobelev - Passanger Date of Birth fix.
Date: 16 Aug 2021 - Kobelev - Controlling Carrier Identification.
Date: 27 Jul 2021 - Kobelev - Price Qoutes for each PTC will have in RPH reference  PTC. 
Date: 23 Jul 2021 - kobelev - Multiple Price Qoutes with different PTC. TR it belong.
Date: 26 Jul 2021 - Kobelev - Multiple Price Qoutes with different Markups for different
Date: 30 Apr 2021 - Samokhvalov - Tour Code remark fixes. Bug 1432
Date: 13 Apr 2021 - Samokhvalov - Tour Code remark fixes.
Date: 23 Mar 2021 - Kobelev - Main ItemPricing Price Quote type.
Date: 10 Mar 2021 - Kobelev - Conversation ID Handler.
Date: 10 Mar 2021 - Kobelev - TravelerRefNumberRPHList and Airline Code for SSRs.
Date: 08 Feb 2021 - Kobelev - Special Remarks display through the loop.
Date: 03 Sep 2020 - Kobelev - Creation Agent in CompanyName information.
Date: 25 Aug 2020 - Kobelev - CON type Special Remarks Added.
Date: 06 Jul 2020 - Kobelev - No number for Baggs allowed.
Date: 04 Jun 2020 - Kobelev - Total Base Fare and Total Eqv. Base Fare fix.
Date: 27 Jan 2020 - Kobelev - Fixing Endorsement Issue.
Date: 12 Dec 2019 - Kobelev - Added More information included Date of Issue to IssuedTickets
Date: 02 Dec 2019 - Kobelev - Fix OperatingAirline when Operating carrier has "FOR" and "BY" 
Date: 30 Oct 2019 - Kobelev - Fix idetifying type of PNR.
Date: 09 Oct 2019 - Kobelev - Build TPA_Extensions with respoct to a Passenger Type.
Date: 09 Oct 2019 - Kobelev - Fixed display of tickets when single passanger has more than one ticket for different segents. EX: 000123456789-790.
Date: 01 Oct 2019 - Kobelev - FlightRefNumberRPHList display fix for Pricing segments.
Date: 26 Sep 2019 - Kobelev - For each Item Pricing have Segment association (FlightRefNumberRPHList)
Date: 24 Sep 2019 - Kobelev - Fixed MCO vs. SPLIT MCO vs. Exchange display
Date: 21 Aug 2019 - Kobelev - Flight Number Formating
Date: 23 May 2019 - Kobelev - Start using AltLangID as GDS idetifier
Date: 17 Apr 2019 - Kobelev - New dates format for Passanger Birth Date. <CST_NME_INF>@10NOV90</CST_NME_INF> vs. <CST_NME_INF>900310</CST_NME_INF>
Date: 05 Apr 2019 - Kobelev - VOID display enhancement
Date: 28 Mar 2019 - Kobelev - VOID vs active ticket MSO based on DHV 
Date: 26 Mar 2019 - Kobelev - MCO and Void MCO from *DH and *DHV.
Date: 23 Nov 2018 - Kobelev - Tour Code from 4* better presintation.
Date: 10 Sep 2018 - Samokhvalov - Added FareType attribute to PassengerFare
Date: 07 Sep 2018 - Kobelev - Tour Code from 4* in regular PNR read.
Date: 14 Jun 2018 - Kobelev - CompanyName object with Creation Date Time added.
Date: 12 Feb 2018 - Samokhvalov Added Endorsment special remark
Date: 28 Dec 2017 - Samokhvalov TravelerRefNumberRPHList - Dots removed in Pax name
Date: 27 Dec 2017 - Samokhvalov CustomerInfo, date of birth ckeck for 'CNN' added
Date: 28 Nov 2017 - Samokhvalov PassangeFare corrected -> amount per 1 pax
Date: 19 Oct 2017 - Kobelev TIC_REC_PRC_QUO vs. PRC_QUO - Results of RePricing display.
Date: 15 May 2017 - Kobelev TicketAdvisory added logic.
Date: 20 Feb 2017 - Kobelev EquivFare as well as Dynamic DicimalPlaces presintation.
Date: 13 Jan 2017 - Kobelev In OperatingAirline trying to parse string to get Airline code.
Date: 09 Jan 2017 - Kobelev In OperatingAirline trying to parse string to get Airline code.
Date: 21 Dec 2016 - Kobelev OperatingAirline generates or not generates element.
Date: 12 Dec 2016 - Kobelev Added BSR Display.
Date: 09 Dec 2016 - Kobelev Added Foreigh Currency display.
Date: 08 Dec 2016 - Kobelev Added TravelerRefNumberRPHList to ItemPricing display
Date: 23 Nov 2016 - Kobelev Fixed Issue Ticket with travelers reference number when Pax name containes "."
Date: 21 Nov 2016 - Kobelev Fixed TOU_COD element TourCode. 
Date: 16 Nov 2016 - Kobelev Pax reference for each Issued ticket
Date: 16 Nov 2016 - Kobelev FOP in ost ticketing PNRs
Date: 02 Nov 2016 - Kobelev VOID for FareBasisCode. Related to ARNK.
Date: 01 Nov 2016 - Kobelev ARNK Fix. 
Date: 11 Oct 2016 - Kobelev Fixed TOU_COD element TourCode. 
Date: 11 Oct 2016 - Kobelev Fixed TIC_REC_PRC_QUO element. Always has to take only one.
Date: 10 Oct 2016 - Kobelev Added Married Segments flag
Date: 23 Sep 2016 - Kobelev Added VOID identifier to IssuedTickets
Date: 22 Sep 2016 - Kobelev Added TravelerRefNumberRPHList and FlightRefNumberRPHList to IssuedTickets
Date: 13 May 2016 - Kobelev Added TIC_REC_PRC_QUO 
Date: 23 Feb 2015 - Rastko														       
================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes" />

	<xsl:key name="trPTC" match="//DPW8/PNR_4_INF/Line/@TR" use="." />
	<xsl:key name="conCarr" match="//DPW8/AIR_SEG_INF/AIR_ITM/ARL_COD" use="." />

	<xsl:variable name="loop">
		<xsl:value-of select="count(//DPW8/PRC_INF/PRC_QUO/PTC_FAR_DTL) + 1"/>
	</xsl:variable>

	<xsl:template match="/">
		<xsl:apply-templates select="DPW8"/>
		<xsl:apply-templates select="XXW"/>
		<xsl:apply-templates select="BPW9"/>
	</xsl:template>

	<xsl:template match="XXW">
		<OTA_TravelItineraryRS Version="2.000">
			<Errors>
				<Error>
					<xsl:attribute name="Type">Worldspan</xsl:attribute>
					<xsl:attribute name="Code">
						<xsl:value-of select="ERROR/CODE"/>
					</xsl:attribute>
					<xsl:value-of select="ERROR/TEXT"/>
				</Error>
			</Errors>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<xsl:template match="BPW9">
		<OTA_TravelItineraryRS Version="2.000">
			<xsl:if test="not(PNR_RLOC) and TIME_PRICE_DIF != ''">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Worldspan</xsl:attribute>
						<xsl:choose>
							<xsl:when test="TIME_PRICE_DIF = 'T'">Time difference detected</xsl:when>
							<xsl:when test="TIME_PRICE_DIF = 'P'">Price difference detected</xsl:when>
							<xsl:when test="TIME_PRICE_DIF = 'B'">Time and Price difference detected</xsl:when>
							<xsl:when test="TIME_PRICE_DIF = 'V'">Price variance detected</xsl:when>
							<xsl:when test="TIME_PRICE_DIF = 'L'">Lower price detected</xsl:when>
						</xsl:choose>
					</Error>
				</Errors>
			</xsl:if>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<xsl:template match="DPW8">
		<OTA_TravelItineraryRS Version="v03" AltLangID="Worldspan">
			<xsl:choose>
				<xsl:when test="ERR">
					<Errors>
						<Error Type="Worldspan" Code="{//ERR/NUM}">
							<xsl:value-of select="//ERR/MSG_TXT"/>
						</Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<xsl:if test="WARNING_INFO/WRN_ITEM/WRN_TEXT != ''">
						<Warnings>
							<xsl:for-each select="WARNING_INFO/WRN_ITEM">
								<Warning Type="Worldspan">
									<xsl:value-of select="WRN_TEXT"/>
								</Warning>
							</xsl:for-each>
						</Warnings>
					</xsl:if>
					<TravelItinerary>
						<ItineraryRef>
							<xsl:attribute name="Type">PNR</xsl:attribute>
							<xsl:attribute name="ID">
								<xsl:value-of select="REC_LOC" />
							</xsl:attribute>
							<xsl:attribute name="ID_Context">
								<xsl:value-of select="PNR_INF/TEL_INF/TEL_NUM/SID"/>
							</xsl:attribute>
							<CompanyName>
								<!-- 13OCT2017 | 2017-3OC-13-->
								<xsl:variable name="CreationDate">
									<xsl:choose>
										<xsl:when test="string-length(CRE_DAT) > 7">
											<xsl:value-of select="substring(CRE_DAT,6,4)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(CRE_DAT,3,3)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(CRE_DAT,1,2)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(20, substring(CRE_DAT,6,2))"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(CRE_DAT,3,3)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(CRE_DAT,1,2)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:attribute name="Code">
									<xsl:value-of select="concat(PNR_INF/TEL_INF/TEL_NUM[1]/SID, '|', $CreationDate)"/>
								</xsl:attribute>
								<xsl:attribute name="CodeContext">IATACode</xsl:attribute>

								<xsl:value-of select="concat(PNR_HI_INF/Line[last()]/@PCC,'/', PNR_HI_INF/Line[last()]/@Agent)"/>
								<!--  
                <xsl:variable name="agent">
                  <xsl:value-of select="concat(ETR_INF/MIS_INF/MIS_TIC_INF[last()]/SID, '/', ETR_INF/MIS_INF/MIS_TIC_INF[last()]/TIC_AGT_ID, ' ')" />
                </xsl:variable>

                <xsl:call-template name="string-trim">
                  <xsl:with-param name="string" select="$agent" />
                </xsl:call-template>
                -->
							</CompanyName>
						</ItineraryRef>

						<CustomerInfos>
							<xsl:apply-templates select="PAX_INF/NME_ITM"/>
						</CustomerInfos>
						<ItineraryInfo>
							<ReservationItems>
								<xsl:apply-templates select="AIR_SEG_INF/AIR_ITM"/>
								<xsl:apply-templates select="TVL_SEG_INF/TVL_ITM"/>
								<xsl:variable name="pq" select="PRC_INF/TIC_REC_PRC_QUO"></xsl:variable>
								<xsl:variable name="tr" select="PNR_4_INF/Line[@ID]"></xsl:variable>

								<xsl:variable name="lTR">
									<xsl:for-each select="$tr/@TR[generate-id() = generate-id(key('trPTC',.)[1])]">
										<xsl:if test=". !=''">
											<xsl:value-of select="concat(., ',')"/>
										</xsl:if>
									</xsl:for-each>
								</xsl:variable>

								<xsl:variable name="elems">
									<xsl:call-template name="tokenizeString">
										<xsl:with-param name="list" select="$lTR"/>
										<xsl:with-param name="delimiter" select="','"/>
									</xsl:call-template>
								</xsl:variable>

								<!--
                <xsl:variable name="cPQ">
                  <xsl:choose>
                    <xsl:when test="count(msxsl:node-set($elems)/elem) = '2'">
                      <xsl:choose>
                        <xsl:when test="count($pq
                                [contains(
                                      concat(',', $lPTC), 
                                      concat(',', PTC_FAR_DTL/PTC, ',')
                                )]) > 1">
                          <xsl:value-of select="2"/>
                        </xsl:when>
                        <xsl:when test="count(tr) > 1">
                          <xsl:value-of select="count(tr)"/>                        
                        </xsl:when>
                        <xsl:otherwise>
                          <xsl:value-of select="1"/>
                        </xsl:otherwise>
                      </xsl:choose>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="1"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>
                -->

								<xsl:choose>
									<xsl:when test="PRC_INF/TIC_REC_PRC_QUO">
										<xsl:choose>
											<xsl:when test="PRC_INF/TIC_REC_PRC_QUO[DOC_INS]">
												<xsl:apply-templates select="PRC_INF/TIC_REC_PRC_QUO[DOC_INS]"  mode="ticketed"/>
											</xsl:when>
											<xsl:when test="count(msxsl:node-set($elems)/elem) > 1">
												<xsl:call-template name="JoinedPQ">
													<xsl:with-param name="PQs" select="PRC_INF/TIC_REC_PRC_QUO[contains(concat(',', $lTR), concat(',', TIC_REC_NUM, ','))]" />
												</xsl:call-template>
											</xsl:when>
											<xsl:otherwise>
												<xsl:apply-templates select="PRC_INF/TIC_REC_PRC_QUO[last()]"  mode="notticketed" />
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:apply-templates select="PRC_INF/PRC_QUO" />
									</xsl:otherwise>
								</xsl:choose>

							</ReservationItems>
							<xsl:apply-templates select="PNR_INF/TIC_INF"/>

							<xsl:if test="SSR_INF/SSR_ITM or RMK_INF">
								<SpecialRequestDetails>
									<xsl:if test="SSR_INF/SSR_ITM">
										<SpecialServiceRequests>
											<xsl:apply-templates select="SSR_INF/SSR_ITM" mode="SSR"/>
										</SpecialServiceRequests>
									</xsl:if>
									<xsl:if test="RMK_INF/GEN_RMK_INF/RMK_ITM or RMK_INF/UNQ_RMK_INF/RMK_ITM">
										<Remarks>
											<xsl:for-each select="RMK_INF/GEN_RMK_INF/RMK_ITM">
												<Remark RPH="{RMK_NUM}">
													<xsl:value-of select="RMK_TXT"/>
												</Remark>
											</xsl:for-each>
											<xsl:for-each select="RMK_INF/UNQ_RMK_INF/RMK_ITM">
												<Remark RPH="{RMK_NUM}">
													<xsl:choose>
														<xsl:when test="RMK_ALP='Z'">
															<xsl:attribute name="Category">
																<xsl:value-of select="'Historical'"/>
															</xsl:attribute>
														</xsl:when>
													</xsl:choose>
													<xsl:value-of select="RMK_TXT"/>
												</Remark>
											</xsl:for-each>
										</Remarks>
									</xsl:if>

									<SpecialRemarks>
										<xsl:choose>
											<xsl:when test="RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='IT'] 
                      or RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='ER'] 
                      or RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='CON'] 
                      or /DPW8/PNR_4_INF/Line[contains(., 'TDTC-')] or ETR_INF/TOU_COD">

												<xsl:for-each select="RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='IT' or RMK_TYP='ER' or RMK_TYP='CON']">
													<xsl:choose>
														<xsl:when test="RMK_TYP='IT'">
															<SpecialRemark>
																<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
																<Text>
																	<xsl:call-template name="string-trim">
																		<xsl:with-param name="string" select="RMK_TXT" />
																	</xsl:call-template>
																</Text>
															</SpecialRemark>
														</xsl:when>
														<xsl:when test="RMK_TYP='ER'">
															<SpecialRemark>
																<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
																<Text>
																	<xsl:call-template name="string-trim">
																		<xsl:with-param name="string" select="RMK_TXT" />
																	</xsl:call-template>
																</Text>
															</SpecialRemark>
														</xsl:when>
														<!--
                          <xsl:when test="ETR_INF/END_ADV">
                            <xsl:apply-templates select="ETR_INF" mode="Endorsements" />
                          </xsl:when>
                          -->
														<xsl:when test="RMK_TYP='CON'">
															<SpecialRemark>
																<xsl:attribute name="RemarkType">C</xsl:attribute>
																<Text>
																	<xsl:call-template name="string-trim">
																		<xsl:with-param name="string" select="RMK_TXT" />
																	</xsl:call-template>
																</Text>
															</SpecialRemark>
														</xsl:when>

													</xsl:choose>
												</xsl:for-each>
												<xsl:if test="ETR_INF/TOU_COD">
													<!--<SpecialRemarks>-->
													<!--xsl:apply-templates select="RMK_INF/UNQ_RMK_INF/RMK_ITM[RMK_ALP='Z']" mode="HistoryRemark"/-->
													<!--xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[contains(elementManagementData/segmentName,'RC')]" mode="ConfRemark"/>-->
													<!--<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FE']" mode="Endorsement"/-->
													<!--<xsl:apply-templates select="RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='IT']" mode="TourCode"/>-->
													<SpecialRemark>
														<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
														<Text>
															<xsl:value-of select="ETR_INF/TOU_COD"/>
														</Text>
													</SpecialRemark>
													<!--</SpecialRemarks>-->
												</xsl:if>
												<xsl:if test="not(ETR_INF/TOU_COD)">
													<!--<SpecialRemarks>-->
													<SpecialRemark>
														<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
														<Text>
															<xsl:if test="/DPW8/PNR_4_INF/Line[contains(., 'TDTC-')][1]">
																<xsl:variable name="tc" select="/DPW8/PNR_4_INF/Line[contains(., 'TDTC-')][1]"></xsl:variable>
																<xsl:choose>
																	<xsl:when test="contains($tc, '@')">
																		<xsl:value-of select="normalize-space(substring-before(substring-after($tc,'-'), '@'))"/>
																	</xsl:when>
																	<xsl:otherwise>
																		<xsl:value-of select="normalize-space(substring-after($tc, 'TDTC- '))"/>
																	</xsl:otherwise>
																</xsl:choose>
															</xsl:if>
														</Text>
													</SpecialRemark>
													<xsl:if test="ETR_INF/END_ADV">
														<xsl:apply-templates select="ETR_INF" mode="Endorsements" />
													</xsl:if>
													<!--</SpecialRemarks>-->
												</xsl:if>

											</xsl:when>
										</xsl:choose>
										<xsl:choose>
											<xsl:when test="PNR_4PR/Line">
												<xsl:for-each select="PNR_4PR/Line">
													<SpecialRemark>
														<xsl:attribute name="RPH">
															<xsl:value-of select="@CC"/>
														</xsl:attribute>
														<xsl:attribute name="RemarkType">CC</xsl:attribute>
														<FlightRefNumber>
															<xsl:attribute name="RPH">
																<xsl:value-of select="@Flights"/>
															</xsl:attribute>
														</FlightRefNumber>
														<Text>
															<xsl:value-of select="text()"/>
														</Text>
													</SpecialRemark>
												</xsl:for-each>
											</xsl:when>
											<xsl:otherwise>
												<xsl:variable name="cc" select="//AIR_SEG_INF/AIR_ITM" />
												<xsl:variable name="lCarr">
													<xsl:for-each select="$cc/ARL_COD[generate-id() = generate-id(key('conCarr',.)[1])]">
														<xsl:if test=". !=''">
															<xsl:value-of select="concat(., ',')"/>
														</xsl:if>
													</xsl:for-each>
												</xsl:variable>

												<xsl:variable name="elems">
													<xsl:call-template name="tokenizeString">
														<xsl:with-param name="list" select="$lCarr"/>
														<xsl:with-param name="delimiter" select="','"/>
													</xsl:call-template>
												</xsl:variable>

												<xsl:for-each select="msxsl:node-set($elems)/elem">

													<xsl:variable name="al">
														<xsl:value-of select="text()"/>
													</xsl:variable>

													<SpecialRemark>
														<xsl:attribute name="RPH">
															<xsl:call-template name="string-trim">
																<xsl:with-param name="string" select="$al" />
															</xsl:call-template>
														</xsl:attribute>
														<xsl:attribute name="RemarkType">CC</xsl:attribute>

														<FlightRefNumber>
															<xsl:attribute name="RPH">
																<xsl:for-each select="$cc[ARL_COD=$al]">
																	<xsl:variable name="rph">
																		<xsl:value-of select="SEG_NUM"/>
																	</xsl:variable>

																	<xsl:if test="position() > 1">
																		<xsl:text> </xsl:text>
																	</xsl:if>
																	<xsl:value-of select="$rph"/>
																</xsl:for-each>
															</xsl:attribute>
														</FlightRefNumber>

														<xsl:variable name="fltPath">
															<xsl:for-each select="$cc[ARL_COD=$al]">
																<xsl:variable name="port">
																	<xsl:value-of select="concat(DEP_ARP,ARR_ARP)"/>
																</xsl:variable>

																<xsl:if test="position() > 1">
																	<xsl:text> </xsl:text>
																</xsl:if>
																<xsl:value-of select="$port"/>
															</xsl:for-each>
														</xsl:variable>

														<Text>
															<xsl:call-template name="string-trim">
																<xsl:with-param name="string" select="concat($fltPath,' -/', $al)" />
															</xsl:call-template>
														</Text>
													</SpecialRemark>
												</xsl:for-each>
											</xsl:otherwise>
										</xsl:choose>
									</SpecialRemarks>
								</SpecialRequestDetails>
							</xsl:if>
							<xsl:if test="ETR_INF">
								<TPA_Extensions>
									<IssuedTickets>
										<xsl:apply-templates select="ETR_INF" mode="IssuedTicket" />
										<xsl:if test="//PNR_EMD_INF">
											<xsl:apply-templates select="//PNR_EMD_INF" mode="EMD" />
										</xsl:if>
									</IssuedTickets>
								</TPA_Extensions>
							</xsl:if>
						</ItineraryInfo>
						<TravelCost>
							<xsl:choose>
								<xsl:when test="PNR_INF/PMT_INF/PMT_COD">
									<xsl:apply-templates select="PNR_INF/PMT_INF" mode="IssuedTicket"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="ETR_INF[position()=1]/ETR_PMT_INF" mode="OTHER"/>
								</xsl:otherwise>
							</xsl:choose>
						</TravelCost>
						<TPA_Extensions>
							<xsl:if test="RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='CA' or RMK_TYP='DI']">
								<xsl:apply-templates select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FM']" mode="commission"/>
								<xsl:apply-templates select="RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='CA']" mode="accounting"/>
								<xsl:call-template name="DILine">
									<xsl:with-param name="DI">
										<xsl:value-of select="RMK_INF/SPE_RMK_INF/RMK_ITM[RMK_TYP='DI']/RMK_TXT"/>
									</xsl:with-param>
									<xsl:with-param name="rph">1</xsl:with-param>
								</xsl:call-template>
							</xsl:if>
							<xsl:if test="//PNR_DHT_INF/DOC_ITM/CM_INF">
								<xsl:apply-templates select="//PNR_DHT_INF/DOC_ITM" mode="agencyComm"/>
							</xsl:if>
						</TPA_Extensions>
					</TravelItinerary>
				</xsl:otherwise>
			</xsl:choose>

			<ConversationID>
				<xsl:choose>
					<xsl:when test="ConversationID!=''">
						<xsl:value-of select="ConversationID" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:text>NONE</xsl:text>
					</xsl:otherwise>
				</xsl:choose>
			</ConversationID>
		</OTA_TravelItineraryRS>
	</xsl:template>

	<xsl:template match="ETR_INF" mode="Endorsements">
		<xsl:variable name="tktNum" select="ETR_TIC_INF/TIC_NUM" />

		<SpecialRemark>
			<xsl:attribute name="RemarkType">Endorsement</xsl:attribute>
			<xsl:attribute name="RPH">
				<xsl:value-of select="$tktNum"/>
			</xsl:attribute>
			<Text>
				<xsl:for-each select="END_ADV">
					<xsl:call-template name="string-ltrim">
						<xsl:with-param name="string" select="concat(TXT, '/')" />
					</xsl:call-template>
				</xsl:for-each>
			</Text>
		</SpecialRemark>

	</xsl:template>

	<xsl:template match="ETR_INF" mode="IssuedTicket">
		<xsl:variable name="tktPax" select="PAX_NME" />
		<xsl:for-each select="ETR_TIC_INF">
			<xsl:variable name="tkt" select="TIC_NUM" />
			<xsl:variable name="tktInfo" select="concat(substring-after(substring-before(//PNR_DH_INF/Line[contains(@TicketNumber,$tkt)], '*'), ' '), ' I',substring-after(//PNR_DH_INF/Line[contains(@TicketNumber,$tkt)], ' I'))" />
			<xsl:variable name="tktType" select="CPN_INF[position()=1]/E_TIC_STA_COD/text()" />
			<xsl:variable name="tktStatus" select="CPN_INF[position()=1]/TIC_STA_COD/text()" />

			<IssuedTicket>
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="//PAX_INF/NME_ITM">
						<xsl:if test="translate(PAX_NME, '.','') = translate($tktPax, '.','')">
							<xsl:value-of select="NME_POS"/>
						</xsl:if>
					</xsl:for-each>
				</xsl:attribute>

				<xsl:variable name="segs">
					<xsl:for-each select="//SSR_INF/SSR_ITM[contains(SSR_TXT, $tkt)]">
						<xsl:choose>
							<xsl:when test="SEG_NUM">
								<xsl:call-template name="string-ltrim">
									<xsl:with-param name="string" select="concat(SEG_NUM, ' ')" />
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="string-ltrim">
									<xsl:with-param name="string" select="concat(translate(substring-before(substring-after(SSR_TXT,concat('.', $tkt)),'/'), 'C', ' '), ' ')" />
								</xsl:call-template>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</xsl:variable>

				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="translate($segs, '-', ' ')" />
					</xsl:call-template>
				</xsl:attribute>

				<xsl:choose>
					<xsl:when test="$tktType = 'V'">
						<xsl:value-of select="concat('TV ', $tkt, '-', $tktStatus, ' *VOID* ', '/', $tktInfo)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('TE ', $tkt, '-', $tktStatus, '/', $tktInfo)"/>
					</xsl:otherwise>
				</xsl:choose>
			</IssuedTicket>

			<xsl:if test="//MIS_INF[contains(ORI_TIC_NUM,$tkt)]">
				<ExchangeDocument>
					<xsl:attribute name="TravelerRefNumberRPHList">
						<xsl:for-each select="//PAX_INF/NME_ITM">
							<xsl:if test="translate(PAX_NME, '.','') = translate($tktPax, '.','')">
								<xsl:value-of select="NME_POS"/>
							</xsl:if>
						</xsl:for-each>
					</xsl:attribute>

					<xsl:variable name="segs">
						<xsl:for-each select="//SSR_INF/SSR_ITM[contains(SSR_TXT, $tkt)]">
							<xsl:choose>
								<xsl:when test="SEG_NUM">
									<xsl:call-template name="string-ltrim">
										<xsl:with-param name="string" select="concat(SEG_NUM, ' ')" />
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="string-ltrim">
										<xsl:with-param name="string" select="concat(translate(substring-before(substring-after(SSR_TXT,concat('.', $tkt)),'/'), 'C', ' '), ' ')" />
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:for-each>
					</xsl:variable>

					<xsl:attribute name="FlightRefNumberRPHList">
						<xsl:call-template name="string-rtrim">
							<xsl:with-param name="string" select="translate($segs, '-', ' ')" />
						</xsl:call-template>
					</xsl:attribute>

					<xsl:variable name="mcoN">
						<xsl:value-of select="$tkt"/>
						<!--<xsl:value-of select="MIS_INF/EXG_REI_INF/TIC_NUM"/>-->
					</xsl:variable>

					<xsl:variable name="mco">
						<xsl:choose>
							<xsl:when test="//MIS_INF/ORI_TIC_NUM">
								<xsl:value-of select="//MIS_INF/ORI_TIC_NUM"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="//MIS_INF/EXG_REI_INF/TIC_NUM"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="mcoFlag">
						<xsl:choose>
							<xsl:when test="//PNR_DH_INF/Line[@TicketNumber=concat('M',$mcoN)]">MCO</xsl:when>
							<xsl:otherwise>EX</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test="//PNR_DHV_INF/Line[@TicketNumber=concat('M',$mcoN)]">
							<xsl:value-of select="concat($mcoFlag,' ', substring($mco,1,3),'-',substring($mco,4,20), '/',substring($mco,24), '-*VOID*')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="concat($mcoFlag,' ', substring($mco,1,3),'-',substring($mco,4,20), '/',substring($mco,24), '-OK')"/>
						</xsl:otherwise>
					</xsl:choose>
				</ExchangeDocument>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PNR_EMD_INF" mode="EMD">

		<xsl:for-each select="Line">
			<xsl:variable name="tktPax" select="substring-before(.,' ')" />
			<xsl:variable name="tkt" select="@EMD" />
			<xsl:variable name="tktInfo" select="substring-after(.,' ')" />

			<IssuedTicket>
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:for-each select="//PAX_INF/NME_ITM">
						<xsl:if test="translate(PAX_NME, '.','') = translate($tktPax, '.','')">
							<xsl:value-of select="NME_POS"/>
						</xsl:if>
					</xsl:for-each>
				</xsl:attribute>

				<xsl:choose>
					<xsl:when test="starts-with($tktInfo, 'V ')">
						<xsl:value-of select="concat('EMD ', $tkt, ' *VOID* ', translate($tktInfo, 'V ', ''))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="concat('EMD ', $tkt, ' ', $tktInfo)"/>
					</xsl:otherwise>
				</xsl:choose>
			</IssuedTicket>

		</xsl:for-each>
	</xsl:template>

	<xsl:template name="DILine">
		<xsl:param name="DI"/>
		<xsl:param name="rph"/>
		<xsl:if test="string-length($DI) > 2">
			<xsl:variable name="DIone">
				<xsl:choose>
					<xsl:when test="contains(substring($DI,2),'#N')">
						<xsl:value-of select="concat('#',substring-before(substring($DI,2),'#N'))"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$DI"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<FuturePriceInfo RPH="{format-number($rph,'#0')}">
				<xsl:value-of select="$DIone"/>
			</FuturePriceInfo>
			<xsl:call-template name="DILine">
				<xsl:with-param name="DI">
					<xsl:value-of select="concat('#N',substring-after(substring($DI,2),'#N'))"/>
				</xsl:with-param>
				<xsl:with-param name="rph">
					<xsl:value-of select="$rph + 1"/>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

	<xsl:template match="RMK_ITM" mode="HistoryRemark">
		<SpecialRemark RemarkType="'H'" RPH="{RMK_NUM}">
			<xsl:value-of select="RMK_TXT"/>
		</SpecialRemark>
	</xsl:template>

	<xsl:template match="RMK_ITM" mode="TourCode">
		<SpecialRemark>
			<xsl:attribute name="RemarkType">TourCode</xsl:attribute>
			<Text>
				<xsl:value-of select="concat(RMK_TYP,' ',RMK_TXT)"/>
			</Text>
		</SpecialRemark>
	</xsl:template>

	<xsl:template match="RMK_ITM" mode="accounting">
		<AccountingLine>
			<xsl:value-of select="RMK_TXT"/>
		</AccountingLine>
	</xsl:template>

	<xsl:template match="DOC_ITM" mode="agencyComm">
		<AgencyCommission>
			<xsl:attribute name="Amount">
				<xsl:value-of select="translate(CM_INF/CM_AMT, translate(CM_INF/CM_AMT,'.0123456789',''),'')"/>
			</xsl:attribute>
			<xsl:variable name="tktPax" select="DOC_PAX_INF/PAX_NME" />
			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:for-each select="//PAX_INF/NME_ITM">
					<xsl:if test="translate(PAX_NME, '.','') = translate($tktPax, '.','')">
						<xsl:value-of select="NME_POS"/>
					</xsl:if>
				</xsl:for-each>
			</xsl:attribute>
		</AgencyCommission>
	</xsl:template>

	<xsl:template match="PRC_QUO">
		<ItemPricing>
			<AirFareInfo>
				<xsl:choose>
					<xsl:when test="PTC_FAR_DTL[1]/FAR_SHE_ORI[contains(text(), ' SR')]">
						<xsl:attribute name="PricingSource">Private</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="PricingSource">Published</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<ItinTotalFare>
					<!--
          <xsl:variable name="totbase">
					<xsl:variable name="amttot">
						<xsl:apply-templates select="FARE_INFO[1]" mode="basefare">
							<xsl:with-param name="total">0</xsl:with-param>
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:value-of select="substring-before($amttot,'/')" />
				</xsl:variable>
				<xsl:variable name="tottax">
					<xsl:variable name="amttot">
						<xsl:apply-templates select="FARE_INFO[1]" mode="TotalTax">
							<xsl:with-param name="total">0</xsl:with-param>
						</xsl:apply-templates>
					</xsl:variable>
					<xsl:value-of select="substring-before($amttot,'/')" />
				</xsl:variable>
          -->

					<BaseFare>
						<xsl:variable name="amount">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_AMT!=''">
									<xsl:value-of select="format-number(sum(TTL_BAS_FAR_AMT),'#.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(sum(EQV_BAS_FAR_AMT),'#.00')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:attribute name="Amount">
							<xsl:value-of select="translate($amount,'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:when test="EQV_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="EQV_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($amount,'.'))"/>
						</xsl:attribute>
					</BaseFare>

					<xsl:if test="EQV_BAS_FAR_AMT">
						<xsl:variable name="ptc" select="PTC"/>
						<xsl:variable name="ef">
							<xsl:apply-templates select="/DPW8/PNR_4_INF" mode="totalequiv" />
						</xsl:variable>
						<xsl:variable name="ptc_count">
							<xsl:apply-templates select="/DPW8/PNR_4_INF" mode="equiv_ptc_count" />
						</xsl:variable>
						<xsl:variable name="cur">
							<xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., 'TTL-')][1]" mode="equivcur" />
						</xsl:variable>
						<EquivFare>
							<xsl:attribute name="Amount">
								<xsl:choose>
									<xsl:when test="$ef != ''">
										<xsl:value-of select="translate($ef * $ptc_count,'.','')"/>
									</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:choose>
									<xsl:when test="$cur != ''">
										<xsl:value-of select="$cur"/>
									</xsl:when>
									<xsl:otherwise>USD</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="string-length(substring-after($ef,'.'))"/>
							</xsl:attribute>
						</EquivFare>
					</xsl:if>

					<Taxes>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(format-number(sum(..//TTL_TAX_AMT),'#.00'),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(TTL_TAX_AMT,'.'))"/>
						</xsl:attribute>
					</Taxes>

					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(format-number(sum(..//TTL_PRC_AMT),'#.00'),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(TTL_PRC_AMT,'.'))"/>
						</xsl:attribute>
					</TotalFare>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:apply-templates select="PTC_FAR_DTL" mode="Details" />
				</PTC_FareBreakdowns>
			</AirFareInfo>
		</ItemPricing>
	</xsl:template>

	<xsl:template match="TIC_REC_PRC_QUO" mode="ticketed">
		<ItemPricing>
			<AirFareInfo>
				<xsl:choose>
					<xsl:when test="PTC_FAR_DTL/FAR_SHE_ORI[contains(text(), ' SR')]">
						<xsl:attribute name="PricingSource">Private</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="PricingSource">Published</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<ItinTotalFare>
					<BaseFare>
						<xsl:variable name="amount">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_AMT!=''">
									<xsl:value-of select="TTL_BAS_FAR_AMT"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="EQV_BAS_FAR_AMT"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:attribute name="Amount">
							<xsl:value-of select="translate($amount,'.','')"/>
						</xsl:attribute>

						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:when test="EQV_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="EQV_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>

						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($amount,'.'))"/>
						</xsl:attribute>
					</BaseFare>
					<Taxes>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(TTL_TAX_AMT,'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(TTL_TAX_AMT,'.'))"/>
						</xsl:attribute>
					</Taxes>
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(TTL_PRC_AMT,'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(TTL_PRC_AMT,'.'))"/>
						</xsl:attribute>
					</TotalFare>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:for-each select="//PRC_INF/TIC_REC_PRC_QUO[DOC_INS]">
						<xsl:variable name="prc_cmd">
							<xsl:value-of select="PRC_QUO_CMD"/>
						</xsl:variable>
						<xsl:variable name="prc_txt">
							<xsl:value-of select="DOC_INS/DOC_INS_TXT"/>
						</xsl:variable>
						<xsl:variable name="tr_num">
							<xsl:value-of select="TIC_REC_NUM"/>
						</xsl:variable>
						<xsl:apply-templates select="PTC_FAR_DTL" mode="single">
							<xsl:with-param name="tr_num" select="$tr_num" />
						</xsl:apply-templates>
					</xsl:for-each>
				</PTC_FareBreakdowns>
			</AirFareInfo>
		</ItemPricing>
	</xsl:template>

	<xsl:template match="TIC_REC_PRC_QUO" mode="notticketed">
		<xsl:param name="coutnPQ" />
		<ItemPricing>
			<AirFareInfo>
				<ItinTotalFare>
					<BaseFare>
						<xsl:variable name="amount">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_AMT!=''">
									<xsl:value-of select="format-number(sum(..//TTL_BAS_FAR_AMT),'#.00')"/>
								</xsl:when>
								<xsl:when test="$coutnPQ > 1">
									<xsl:value-of select="sum(../TIC_REC_PRC_QUO/EQV_BAS_FAR_AMT)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="EQV_BAS_FAR_AMT"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:attribute name="Amount">
							<xsl:value-of select="translate($amount,'.','')"/>
						</xsl:attribute>

						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:when test="EQV_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="EQV_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>

						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($amount,'.'))"/>
						</xsl:attribute>
					</BaseFare>
					<Taxes>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="$coutnPQ > 1">
									<xsl:value-of select="translate(format-number(sum(../TIC_REC_PRC_QUO/TTL_TAX_AMT),'#.00'),'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(format-number(TTL_TAX_AMT,'#.00'),'.','')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(TTL_TAX_AMT,'.'))"/>
						</xsl:attribute>
					</Taxes>
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="$coutnPQ > 1">
									<xsl:value-of select="translate(format-number(sum(../TIC_REC_PRC_QUO/TTL_PRC_AMT),'#.00'),'.','')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="translate(format-number(TTL_PRC_AMT,'#.00'),'.','')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(TTL_PRC_AMT,'.'))"/>
						</xsl:attribute>
					</TotalFare>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:choose>
						<xsl:when test="$coutnPQ > 1">
							<xsl:for-each select="//PRC_INF/TIC_REC_PRC_QUO">
								<xsl:variable name="prc_cmd">
									<xsl:value-of select="PRC_QUO_CMD"/>
								</xsl:variable>
								<xsl:variable name="prc_txt">
									<xsl:value-of select="DOC_INS/DOC_INS_TXT"/>
								</xsl:variable>
								<xsl:apply-templates select="PTC_FAR_DTL" mode="single" />
							</xsl:for-each>
						</xsl:when>
						<xsl:otherwise>
							<xsl:for-each select=".">
								<xsl:variable name="prc_cmd">
									<xsl:value-of select="PRC_QUO_CMD"/>
								</xsl:variable>
								<xsl:variable name="prc_txt">
									<xsl:value-of select="DOC_INS/DOC_INS_TXT"/>
								</xsl:variable>

								<xsl:variable name="trN">
									<xsl:value-of select="TIC_REC_NUM"/>
								</xsl:variable>

								<xsl:apply-templates select="PTC_FAR_DTL" mode="single">
									<xsl:with-param name="tr_num" select="$trN"/>
								</xsl:apply-templates>
							</xsl:for-each>
						</xsl:otherwise>
					</xsl:choose>
				</PTC_FareBreakdowns>
			</AirFareInfo>
		</ItemPricing>
	</xsl:template>

	<xsl:template name="JoinedPQ">
		<xsl:param name="PQs" />
		<ItemPricing>
			<AirFareInfo>
				<ItinTotalFare>
					<BaseFare>
						<xsl:variable name="amount">
							<xsl:choose>
								<xsl:when test="$PQs[1]/TTL_BAS_FAR_AMT!=''">
									<xsl:value-of select="format-number(sum($PQs/TTL_BAS_FAR_AMT),'#.00')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="format-number(sum($PQs/EQV_BAS_FAR_AMT),'#.00')"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:attribute name="Amount">
							<xsl:value-of select="translate($amount,'.','')"/>
						</xsl:attribute>

						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="$PQs[last()]/TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="$PQs[last()]/TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:when test="$PQs[last()]/EQV_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="$PQs[last()]/EQV_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>

						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($amount,'.'))"/>
						</xsl:attribute>
					</BaseFare>
					<Taxes>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(format-number(sum($PQs/TTL_TAX_AMT),'#.00'),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="$PQs[last()]/TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="$PQs[last()]/TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($PQs[last()]/TTL_TAX_AMT,'.'))"/>
						</xsl:attribute>
					</Taxes>
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(format-number(sum($PQs/TTL_PRC_AMT),'#.00'),'.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="$PQs[last()]/TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="$PQs[last()]/TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($PQs[last()]/TTL_PRC_AMT,'.'))"/>
						</xsl:attribute>
					</TotalFare>
				</ItinTotalFare>
				<PTC_FareBreakdowns>
					<xsl:for-each select="msxsl:node-set($PQs)">
						<xsl:variable name="prc_cmd">
							<xsl:value-of select="PRC_QUO_CMD"/>
						</xsl:variable>
						<xsl:variable name="prc_txt">
							<xsl:value-of select="DOC_INS/DOC_INS_TXT"/>
						</xsl:variable>
						<xsl:variable name="tr_num">
							<xsl:value-of select="TIC_REC_NUM"/>
						</xsl:variable>
						<xsl:apply-templates select="PTC_FAR_DTL" mode="single">
							<xsl:with-param name="tr_num" select="$tr_num" />
						</xsl:apply-templates>
					</xsl:for-each>
				</PTC_FareBreakdowns>
			</AirFareInfo>
		</ItemPricing>
	</xsl:template>

	<xsl:template match="FARE_INFO" mode="basefare">
		<xsl:param name="total" />
		<xsl:variable name="totpax">
			<xsl:value-of select="NUMBER_OF_PEOPLE"/>
		</xsl:variable>
		<xsl:variable name="thistotal">
			<xsl:value-of select="translate(FARE,'.','') * $totpax" />
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::FARE_INFO[1]" mode="basefare">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>

	<xsl:template match="FARE_INFO" mode="TotalTax">
		<xsl:param name="total" />
		<xsl:variable name="totpax">
			<xsl:value-of select="NUMBER_OF_PEOPLE"/>
		</xsl:variable>
		<xsl:variable name="thistotal">
			<xsl:value-of select="translate(TAX,'.','') * $totpax" />
		</xsl:variable>
		<xsl:variable name="bigtotal">
			<xsl:value-of select="$total + $thistotal" />
		</xsl:variable>
		<xsl:apply-templates select="following-sibling::FARE_INFO[1]" mode="basefare">
			<xsl:with-param name="total">
				<xsl:value-of select="$bigtotal" />
			</xsl:with-param>
		</xsl:apply-templates>
		<xsl:value-of select="$bigtotal" />
		<xsl:text>/</xsl:text>
	</xsl:template>

	<xsl:template match="PTC_FAR_DTL" mode="Details">

		<xsl:variable name="base">
			<xsl:choose>
				<xsl:when test="BAS_FAR_AMT!=''">
					<xsl:value-of select="BAS_FAR_AMT"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="EQV_BAS_FAR_AMT"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tax">
			<xsl:value-of select="TAX_AMT" />
		</xsl:variable>
		<xsl:variable name="paxtype">
			<xsl:value-of select="PTC"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="count(../../../PAX_INF/NME_ITM[PTC=$paxtype])"/>
		</xsl:variable>
		<xsl:variable name="totbase">
			<xsl:choose>
				<xsl:when test="contains($base, '.')">
					<xsl:value-of select="format-number($base * $nip, '#0.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($base * $nip, '#0')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tottax">
			<xsl:choose>
				<xsl:when test="contains($tax, '.')">
					<xsl:value-of select="format-number($tax * $nip, '#0.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($tax * $nip, '#0')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="totfare">
			<xsl:choose>
				<xsl:when test="contains($base, '.')">
					<xsl:value-of select="format-number($totbase + $tottax, '#0.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($totbase + $tottax, '#0')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:for-each select="../../PRC_QUO">
			<xsl:variable name="pnrPax">
				<xsl:for-each select="//PAX_INF/NME_ITM[PTC=$paxtype]">
					<xsl:value-of select="concat(NME_POS, ' ')"/>
				</xsl:for-each>
			</xsl:variable>
			<PTC_FareBreakdown>
				<xsl:attribute name="RPH">
					<xsl:value-of select="position()"/>
				</xsl:attribute>
				<xsl:choose>
					<xsl:when test="PTC_FAR_DTL/FAR_SHE_ORI[contains(text(), ' SR')]">
						<xsl:attribute name="PricingSource">Private</xsl:attribute>
					</xsl:when>
					<xsl:otherwise>
						<xsl:attribute name="PricingSource">Published</xsl:attribute>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="$pnrPax" />
					</xsl:call-template>
				</xsl:attribute>

				<xsl:variable name="p" select="//PNR_4_INF/Line[1]/text()" />

				<xsl:variable name="segs">
					<xsl:choose>
						<xsl:when test="translate(substring-after(translate(string(../PRC_QUO_CMD),'#TR',''),'*S'),'/',' ')">
							<xsl:value-of select="translate(substring-after(translate(string(../PRC_QUO_CMD),'#TR',''),'*S'),'/',' ')"/>
						</xsl:when>
						<xsl:when test="contains($p,'*S')">
							<xsl:value-of select="translate(translate(substring-after($p,'*S'),'/',' '),'-',' ')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:for-each select="//AIR_SEG_INF/AIR_ITM">
								<xsl:call-template name="string-ltrim">
									<xsl:with-param name="string" select="concat(SEG_NUM, ' ')" />
								</xsl:call-template>
							</xsl:for-each>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:attribute name="FlightRefNumberRPHList">
					<xsl:choose>
						<xsl:when test="../SEGMENTS">
							<xsl:value-of select="../SEGMENTS"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:call-template name="string-rtrim">
								<xsl:with-param name="string" select="translate($segs, '-', ' ')" />
							</xsl:call-template>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>

				<PassengerTypeQuantity>
					<xsl:attribute name="Code">
						<xsl:choose>
							<!--
          <xsl:when test="PTC = 'CNN'">CHD</xsl:when>
          -->
							<xsl:when test="$paxtype = 'GGV'">GOV</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$paxtype" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="Quantity">
						<xsl:value-of select="$nip"/>
					</xsl:attribute>
				</PassengerTypeQuantity>
				<FareBasisCodes>
					<xsl:for-each select="PTC_FAR_DTL[PTC=$paxtype]/FAR_BAS_COD">
						<xsl:variable name="seg">
							<xsl:value-of select="position()" />
						</xsl:variable>
						<xsl:variable name="arnk">
							<xsl:value-of select="//AIR_SEG_INF/AIR_ITM[SEG_NUM=$seg]/FLI_NUM" />
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$arnk!='ARNK'">
								<FareBasisCode>
									<xsl:value-of select="." />
								</FareBasisCode>
							</xsl:when>
							<xsl:otherwise>
								<FareBasisCode>VOID</FareBasisCode>
								<FareBasisCode>
									<xsl:value-of select="." />
								</FareBasisCode>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:for-each>
				</FareBasisCodes>
				<PassengerFare>
					<xsl:attribute name="FareType">
						<xsl:value-of select="../FAR_TYP" />
					</xsl:attribute>
					<BaseFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate($base, '.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($base,'.'))"/>
						</xsl:attribute>
					</BaseFare>
					<xsl:if test="PTC_FAR_DTL/EQV_BAS_FAR_AMT">
						<xsl:variable name="ef">
							<xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equiv" />
						</xsl:variable>
						<xsl:variable name="cur">
							<xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equivcur" />
						</xsl:variable>
						<EquivFare>

							<xsl:attribute name="Amount">
								<xsl:choose>
									<xsl:when test="$ef != ''">
										<xsl:value-of select="format-number(translate($ef,'.',''), '#0')"/>
									</xsl:when>
									<xsl:otherwise>0</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="CurrencyCode">
								<xsl:choose>
									<xsl:when test="$cur != ''">
										<xsl:value-of select="$cur"/>
									</xsl:when>
									<xsl:otherwise>USD</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="DecimalPlaces">
								<xsl:value-of select="string-length(substring-after($ef,'.'))"/>
							</xsl:attribute>
						</EquivFare>
					</xsl:if>
					<Taxes>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate($tax, '.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($tax,'.'))"/>
						</xsl:attribute>
					</Taxes>
					<TotalFare>
						<xsl:attribute name="Amount">
							<xsl:value-of select="translate(format-number($base + $tax, '#0.00'), '.','')"/>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
									<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after(format-number($base + $tax, '#0.00'),'.'))"/>
						</xsl:attribute>
					</TotalFare>
				</PassengerFare>
				<TPA_Extensions>
					<xsl:if test="PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR!=''">
						<FareCalculation>
							<xsl:value-of select="PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR"/>
						</FareCalculation>
						<xsl:if test="PTC_FAR_DTL[PTC=$paxtype]/EQV_BAS_FAR_AMT">
							<xsl:variable name="cur">
								<xsl:apply-templates select="//PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equivcur" />
							</xsl:variable>
							<xsl:if test="//PNR_4_INF/Line[contains(., concat('1',$cur,'/'))][1]">
								<BSR>
									<xsl:value-of select="substring-after(substring-before(//PNR_4_INF/Line[contains(., concat('1',$cur,'/'))][1], 'USD'), '/')" />
								</BSR>
							</xsl:if>
						</xsl:if>

						<xsl:variable name="vc">
							<xsl:choose>
								<xsl:when test="contains(PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR, 'ROE')">
									<xsl:value-of select="substring(substring-after(substring-after(PTC_FAR_DTL/FAR_LDR,'ROE'),' '),1,2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="substring(substring-after(substring-after(PTC_FAR_DTL/FAR_LDR,'END'),' '),1,2)"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:if test="$vc != ''">
							<ValidatingAirlineCode>
								<xsl:value-of select="$vc"/>
							</ValidatingAirlineCode>
						</xsl:if>

						<xsl:if test="//MIS_INF/MIS_TIC_INF/BAG_ALL">
							<BagAllowance>
								<xsl:choose>
									<xsl:when test="contains(//MIS_INF/MIS_TIC_INF/BAG_ALL, 'PC')">
										<xsl:attribute name="Quantity">
											<xsl:choose>
												<xsl:when test="//MIS_INF/MIS_TIC_INF/BAG_ALL = 'PC'">
													<xsl:value-of select="0"/>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="substring-before(//MIS_INF/MIS_TIC_INF/BAG_ALL, 'PC')"/>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:attribute name="Type">
											<xsl:text>Piece</xsl:text>
										</xsl:attribute>
									</xsl:when>
									<xsl:otherwise>
										<xsl:attribute name="Weight">
											<xsl:value-of select="substring(//MIS_INF/MIS_TIC_INF/BAG_ALL, 1,string-length(//MIS_INF/MIS_TIC_INF/BAG_ALL) - 1)"/>
										</xsl:attribute>
										<xsl:attribute name="Type">
											<xsl:text>Weight</xsl:text>
										</xsl:attribute>
										<xsl:attribute name="Unit">
											<xsl:value-of select="substring(//MIS_INF/MIS_TIC_INF/BAG_ALL, string-length(//MIS_INF/MIS_TIC_INF/BAG_ALL))"/>
										</xsl:attribute>
									</xsl:otherwise>
								</xsl:choose>
								<!--
            <xsl:attribute name="ItinSeqNumber">
              <xsl:for-each select="../SegRelatedInfo">
                <xsl:if test="position() > 1">
                  <xsl:text> </xsl:text>
                </xsl:if>
                <xsl:value-of select="RelSegNum"/>
              </xsl:for-each>
            </xsl:attribute>
            -->
							</BagAllowance>
						</xsl:if>
					</xsl:if>
					<xsl:choose>
						<xsl:when test="//PNR_DHT_INF/DOC_ITM[DOC_PAX_INF/PTC=$paxtype]/DOC_PRC_INF/FAR_SHE_INF">
							<SupplementalInfo>
								<xsl:value-of select="//PNR_DHT_INF/DOC_ITM[DOC_PAX_INF/PTC=$paxtype]/DOC_PRC_INF/FAR_SHE_INF/FAR_SHE_TXT"/>
							</SupplementalInfo>
						</xsl:when>
						<xsl:otherwise>
							<xsl:if test="PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR!='' and PRC_QUO_CMD">
								<SupplementalInfo>
									<xsl:value-of select="PRC_QUO_CMD"/>
								</SupplementalInfo>
							</xsl:if>
						</xsl:otherwise>
					</xsl:choose>
				</TPA_Extensions>
			</PTC_FareBreakdown>
		</xsl:for-each>
	</xsl:template>

	<xsl:template match="PTC_FAR_DTL" mode="single">
		<xsl:param name="tr_num" />
		<xsl:variable name="base">
			<xsl:choose>
				<xsl:when test="BAS_FAR_AMT!=''">
					<xsl:value-of select="BAS_FAR_AMT"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="EQV_BAS_FAR_AMT"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tax">
			<xsl:value-of select="TAX_AMT" />
		</xsl:variable>
		<xsl:variable name="paxtype">
			<xsl:value-of select="PTC"/>
		</xsl:variable>
		<xsl:variable name="nip">
			<xsl:value-of select="count(../../../PAX_INF/NME_ITM[PTC=$paxtype])"/>
		</xsl:variable>
		<xsl:variable name="totbase">
			<xsl:choose>
				<xsl:when test="contains($base, '.')">
					<xsl:value-of select="format-number($base * $nip, '#0.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($base * $nip, '#0')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tottax">
			<xsl:choose>
				<xsl:when test="contains($tax, '.')">
					<xsl:value-of select="format-number($tax * $nip, '#0.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($tax * $nip, '#0')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="totfare">
			<xsl:choose>
				<xsl:when test="contains($base, '.')">
					<xsl:value-of select="format-number($totbase + $tottax, '#0.00')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="format-number($totbase + $tottax, '#0')"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="pnrPax">
			<xsl:for-each select="//PAX_INF/NME_ITM[PTC=$paxtype]">
				<xsl:value-of select="concat(NME_POS, ' ')"/>
			</xsl:for-each>
		</xsl:variable>

		<PTC_FareBreakdown>
			<xsl:attribute name="RPH">
				<xsl:choose>
					<xsl:when test="$tr_num!=''">
						<xsl:value-of select="$tr_num"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="position()"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="FAR_SHE_ORI[contains(text(), ' SR')]">
					<xsl:attribute name="PricingSource">Private</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="PricingSource">Published</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:attribute name="TravelerRefNumberRPHList">
				<xsl:call-template name="string-rtrim">
					<xsl:with-param name="string" select="$pnrPax" />
				</xsl:call-template>
			</xsl:attribute>
			<xsl:variable name="p" select="//PNR_4_INF/Line[1]/text()" />

			<xsl:variable name="segs">
				<xsl:choose>
					<xsl:when test="translate(substring-after(translate(string(../PRC_QUO_CMD),'#TR',''),'*S'),'/',' ')">
						<xsl:value-of select="translate(substring-after(translate(string(../PRC_QUO_CMD),'#TR',''),'*S'),'/',' ')"/>
					</xsl:when>
					<xsl:when test="contains($p,'*S')">
						<xsl:value-of select="translate(translate(substring-after($p,'*S'),'/',' '),'-',' ')"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:for-each select="//AIR_SEG_INF/AIR_ITM">
							<xsl:call-template name="string-ltrim">
								<xsl:with-param name="string" select="concat(SEG_NUM, ' ')" />
							</xsl:call-template>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:attribute name="FlightRefNumberRPHList">
				<xsl:choose>
					<xsl:when test="../../TIC_REC_PRC_QUO[1]/SEGMENTS">
						<xsl:value-of select="../../TIC_REC_PRC_QUO[1]/SEGMENTS"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="string-rtrim">
							<xsl:with-param name="string" select="translate($segs, '-', ' ')" />
						</xsl:call-template>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>

			<PassengerTypeQuantity>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="$paxtype = 'GGV'">GOV</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$paxtype" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="Quantity">
					<xsl:value-of select="$nip"/>
				</xsl:attribute>
			</PassengerTypeQuantity>
			<FareBasisCodes>
				<xsl:for-each select="FAR_BAS_COD">
					<xsl:variable name="seg">
						<xsl:value-of select="position()" />
					</xsl:variable>
					<xsl:variable name="arnk">
						<xsl:value-of select="//AIR_SEG_INF/AIR_ITM[SEG_NUM=$seg]/FLI_NUM" />
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$arnk!='ARNK'">
							<FareBasisCode>
								<xsl:value-of select="." />
							</FareBasisCode>
						</xsl:when>
						<xsl:otherwise>
							<FareBasisCode>VOID</FareBasisCode>
							<FareBasisCode>
								<xsl:value-of select="." />
							</FareBasisCode>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:for-each>
			</FareBasisCodes>
			<PassengerFare>
				<xsl:attribute name="FareType">
					<xsl:value-of select="//FAR_TYP" />
				</xsl:attribute>
				<BaseFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate($base, '.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
								<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="string-length(substring-after($base,'.'))"/>
					</xsl:attribute>
				</BaseFare>
				<xsl:if test="PTC_FAR_DTL/EQV_BAS_FAR_AMT">
					<xsl:variable name="ef">
						<xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equiv" />
					</xsl:variable>
					<xsl:variable name="cur">
						<xsl:apply-templates select="/DPW8/PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equivcur" />
					</xsl:variable>
					<EquivFare>

						<xsl:attribute name="Amount">
							<xsl:choose>
								<xsl:when test="$ef != ''">
									<xsl:value-of select="format-number(translate($ef,'.',''), '#0')"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:choose>
								<xsl:when test="$cur != ''">
									<xsl:value-of select="$cur"/>
								</xsl:when>
								<xsl:otherwise>USD</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="string-length(substring-after($ef,'.'))"/>
						</xsl:attribute>
					</EquivFare>
				</xsl:if>
				<Taxes>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate($tax, '.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
								<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="string-length(substring-after($tax,'.'))"/>
					</xsl:attribute>
				</Taxes>
				<TotalFare>
					<xsl:attribute name="Amount">
						<xsl:value-of select="translate(format-number($base + $tax, '#0.00'), '.','')"/>
					</xsl:attribute>
					<xsl:attribute name="CurrencyCode">
						<xsl:choose>
							<xsl:when test="TTL_BAS_FAR_CUR_COD != ''">
								<xsl:value-of select="TTL_BAS_FAR_CUR_COD"/>
							</xsl:when>
							<xsl:otherwise>USD</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="string-length(substring-after(format-number($base + $tax, '#0.00'),'.'))"/>
					</xsl:attribute>
				</TotalFare>
			</PassengerFare>
			<TPA_Extensions>
				<xsl:if test="FAR_LDR!=''">
					<FareCalculation>
						<xsl:value-of select="FAR_LDR"/>
					</FareCalculation>
					<xsl:if test="EQV_BAS_FAR_AMT">
						<xsl:variable name="cur">
							<xsl:apply-templates select="//PNR_4_INF/Line[contains(., concat($paxtype, ' '))]" mode="equivcur" />
						</xsl:variable>
						<xsl:if test="//PNR_4_INF/Line[contains(., concat('1',$cur,'/'))][1]">
							<BSR>
								<xsl:value-of select="substring-after(substring-before(//PNR_4_INF/Line[contains(., concat('1',$cur,'/'))][1], 'USD'), '/')" />
							</BSR>
						</xsl:if>
					</xsl:if>

					<xsl:variable name="vc">
						<xsl:choose>
							<xsl:when test="contains(FAR_LDR, 'ROE')">
								<xsl:value-of select="substring(substring-after(substring-after(FAR_LDR,'ROE'),' '),1,2)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="substring(substring-after(substring-after(FAR_LDR,'END'),' '),1,2)"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:if test="$vc != ''">
						<ValidatingAirlineCode>
							<xsl:value-of select="$vc"/>
						</ValidatingAirlineCode>
					</xsl:if>

					<!-- <xsl:if test="contains(../PRC_QUO_CMD,'-$P')"> -->
				</xsl:if>
				<xsl:choose>
					<xsl:when test="//PNR_DHT_INF/DOC_ITM[DOC_PAX_INF/PTC=$paxtype]/DOC_PRC_INF/FAR_SHE_INF">
						<SupplementalInfo>
							<xsl:value-of select="//PNR_DHT_INF/DOC_ITM[DOC_PAX_INF/PTC=$paxtype]/DOC_PRC_INF/FAR_SHE_INF/FAR_SHE_TXT"/>
						</SupplementalInfo>
					</xsl:when>
					<xsl:otherwise>
						<xsl:if test="FAR_LDR!='' and ../PRC_QUO_CMD">
							<SupplementalInfo>
								<xsl:value-of select="../PRC_QUO_CMD"/>
							</SupplementalInfo>
						</xsl:if>
					</xsl:otherwise>
				</xsl:choose>
			</TPA_Extensions>
		</PTC_FareBreakdown>

	</xsl:template>

	<xsl:template match="NME_ITM">
		<CustomerInfo>
			<xsl:attribute name="RPH">
				<xsl:value-of select="NME_POS"/>
			</xsl:attribute>
			<Customer>
				<xsl:variable name="paxName" select="translate(translate(translate(CST_NME_INF, 'DOB', ''), 'CNN', ''), '@','')" />

				<xsl:variable name="paxBD">
					<xsl:choose>
						<xsl:when test="not(string(number($paxName))='NaN')">
							<xsl:text>20</xsl:text>
							<xsl:value-of select="substring($paxName,1,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($paxName,3,2)"/>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($paxName,5,2)"/>
						</xsl:when>
						<xsl:when test="string(number($paxName))='NaN' and (string-length($paxName) = 7 or string-length($paxName) = 9)">
							<xsl:variable name="mo" select="substring($paxName,3,3)"/>
							<xsl:variable name="yr" select="substring($paxName,6)"/>

							<xsl:if test="string-length($yr) = 2">
								<xsl:choose>
									<xsl:when test="$yr>17">
										<xsl:text>19</xsl:text>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>20</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:if>

							<xsl:value-of select="$yr"/>

							<xsl:text>-</xsl:text>
							<xsl:choose>
								<xsl:when test="$mo = 'JAN'">01</xsl:when>
								<xsl:when test="$mo = 'FEB'">02</xsl:when>
								<xsl:when test="$mo = 'MAR'">03</xsl:when>
								<xsl:when test="$mo = 'APR'">04</xsl:when>
								<xsl:when test="$mo = 'MAY'">05</xsl:when>
								<xsl:when test="$mo = 'JUN'">06</xsl:when>
								<xsl:when test="$mo = 'JUL'">07</xsl:when>
								<xsl:when test="$mo = 'AUG'">08</xsl:when>
								<xsl:when test="$mo = 'SEP'">09</xsl:when>
								<xsl:when test="$mo = 'OCT'">10</xsl:when>
								<xsl:when test="$mo = 'NOV'">11</xsl:when>
								<xsl:when test="$mo = 'DEC'">12</xsl:when>
							</xsl:choose>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring($paxName,1,2)"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:text></xsl:text>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<xsl:if test="string-length($paxBD) > 0">
					<xsl:attribute name="BirthDate">
						<xsl:value-of select="$paxBD" />
					</xsl:attribute>
				</xsl:if>
				<PersonName>
					<xsl:attribute name="NameType">
						<xsl:value-of select="PTC"/>
					</xsl:attribute>
					<GivenName>
						<xsl:value-of select="substring-after(string(PAX_NME),'/')"/>
					</GivenName>
					<Surname>
						<xsl:value-of select="substring-before(string(PAX_NME),'/')"/>
					</Surname>
				</PersonName>
				<xsl:apply-templates select="../../PNR_INF/TEL_INF/TEL_NUM"/>
			</Customer>
		</CustomerInfo>
	</xsl:template>

	<xsl:template match="TEL_NUM">
		<Telephone>
			<xsl:attribute name="PhoneNumber">
				<xsl:value-of select="NUM"/>
			</xsl:attribute>
		</Telephone>
	</xsl:template>


	<!-- 
  ************************************************************** 
  FOP For Issued Tickets	   		                                    
  ************************************************************** 
  -->
	<xsl:template match="PMT_INF" mode="IssuedTicket">
		<FormOfPayment>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()" />
			</xsl:attribute>

			<xsl:if test="not(contains(/DPW8/PNR_DH_INF/Line[1],'NO DOC HISTORY')) or not(contains(/DPW8/PNR_DHV_INF/Line[1],'NO DOC HISTORY'))">

				<xsl:if test="not(contains(/DPW8/PNR_DH_INF/Line[1],'NO DOC HISTORY'))">
					<xsl:for-each select="/DPW8/PNR_DH_INF/Line[contains(text(), '*M')]">
						<xsl:if test="not(/DPW8/ETR_INF/MIS_INF/EXG_REI_INF[TIC_NUM = substring-after(@TicketNumber, 'M')]) and not(/DPW8/PNR_DHV_INF/Line[contains(text(), '*M')]/@TicketNumber = @TicketNumber)">
							<MiscChargeOrder>
								<xsl:attribute name="TicketNumber">
									<xsl:value-of  select="@TicketNumber" />
								</xsl:attribute>
								<xsl:value-of  select="." />
							</MiscChargeOrder>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>
				<xsl:if test="not(contains(/DPW8/PNR_DHV_INF/Line[1],'NO DOC HISTORY'))">
					<xsl:for-each select="/DPW8/PNR_DHV_INF/Line[contains(text(), '*M')]">
						<xsl:if test="not(/DPW8/ETR_INF/MIS_INF/EXG_REI_INF[TIC_NUM = substring-after(@TicketNumber, 'M')])">
							<MiscChargeOrder>
								<xsl:attribute name="TicketNumber">
									<xsl:value-of  select="@TicketNumber" />
								</xsl:attribute>
								<xsl:value-of  select="concat(., ' *VOID* ')" />
							</MiscChargeOrder>
						</xsl:if>
					</xsl:for-each>
				</xsl:if>

			</xsl:if>

			<xsl:choose>
				<xsl:when test="PMT_COD = 'CA'">
					<DirectBill>
						<xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
					</DirectBill>
				</xsl:when>
				<xsl:when test="PMT_COD = 'CK'">
					<DirectBill>
						<xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
					</DirectBill>
				</xsl:when>
				<xsl:when test="PMT_COD = 'CASH'">
					<DirectBill>
						<xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
					</DirectBill>
				</xsl:when>
				<xsl:when test="PMT_COD = 'CHECK'">
					<DirectBill>
						<xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
					</DirectBill>
				</xsl:when>
				<xsl:otherwise>
					<PaymentCard>
						<xsl:attribute name="CardCode">
							<xsl:choose>
								<xsl:when test="CC_TYP = 'CA'">MC</xsl:when>
								<xsl:when test="CC_TYP = 'DC'">DN</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CC_TYP"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CardNumber">
							<xsl:value-of select="CC_NUM"/>
						</xsl:attribute>
						<xsl:attribute name="ExpireDate">
							<xsl:value-of select="CC_EXP_DAT"/>
						</xsl:attribute>
					</PaymentCard>
					<TPA_Extensions>
						<xsl:attribute name="FOPType">CC</xsl:attribute>
					</TPA_Extensions>
				</xsl:otherwise>
			</xsl:choose>
		</FormOfPayment>
	</xsl:template>

	<!-- 
  ************************************************************** 
  FOP For Other cases	   		                                    
  ************************************************************** 
  -->
	<xsl:template match="ETR_PMT_INF" mode="OTHER">
		<FormOfPayment>
			<xsl:attribute name="RPH">
				<xsl:value-of select="position()" />
			</xsl:attribute>

			<xsl:if test="not(contains(/DPW8/PNR_DH_INF/Line[1],'NO DOC HISTORY'))">
				<xsl:for-each select="/DPW8/PNR_DH_INF/Line[contains(text(), '*M')]">
					<xsl:if test="not(/DPW8/ETR_INF/MIS_INF/EXG_REI_INF[TIC_NUM = substring-after(@TicketNumber, 'M')])">
						<MiscChargeOrder>
							<xsl:attribute name="TicketNumber">
								<xsl:value-of  select="@TicketNumber" />
							</xsl:attribute>
							<xsl:value-of  select="." />
						</MiscChargeOrder>
					</xsl:if>
				</xsl:for-each>
			</xsl:if>
			<xsl:if test="not(contains(/DPW8/PNR_DHV_INF/Line[1],'NO DOC HISTORY'))">
				<xsl:for-each select="/DPW8/PNR_DHV_INF/Line[contains(text(), '*M')]">
					<xsl:if test="not(/DPW8/ETR_INF/MIS_INF/EXG_REI_INF[TIC_NUM = substring-after(@TicketNumber, 'M')])">
						<MiscChargeOrder>
							<xsl:attribute name="TicketNumber">
								<xsl:value-of  select="@TicketNumber" />
							</xsl:attribute>
							<xsl:value-of  select="concat(., ' *VOID* ')" />
						</MiscChargeOrder>
					</xsl:if>
				</xsl:for-each>
			</xsl:if>

			<xsl:choose>
				<xsl:when test="PMT_COD = 'CA'">
					<DirectBill>
						<xsl:attribute name="DirectBill_ID">Cash</xsl:attribute>
					</DirectBill>
				</xsl:when>
				<xsl:when test="PMT_COD = 'CK'">
					<DirectBill>
						<xsl:attribute name="DirectBill_ID">Check</xsl:attribute>
					</DirectBill>
				</xsl:when>
				<xsl:otherwise>
					<PaymentCard>
						<xsl:attribute name="CardCode">
							<xsl:choose>
								<xsl:when test="CC_TYP = 'CA'">MC</xsl:when>
								<xsl:when test="CC_TYP = 'DC'">DN</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="CC_TYP"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="CardNumber">
							<xsl:value-of select="CC_NUM"/>
						</xsl:attribute>
						<xsl:attribute name="ExpireDate">
							<xsl:value-of select="CC_EXP_DAT"/>
						</xsl:attribute>
					</PaymentCard>
					<TPA_Extensions>
						<xsl:attribute name="FOPType">CC</xsl:attribute>
					</TPA_Extensions>
				</xsl:otherwise>
			</xsl:choose>
		</FormOfPayment>
	</xsl:template>

	<xsl:template match="TIC_INF">
		<Ticketing>
			<xsl:attribute name="TicketType">
				<xsl:choose>
					<xsl:when test="//AIR_SEG_INF/AIR_ITM/E_TIC_ELI='Y'">eTicket</xsl:when>
					<xsl:otherwise>Paper</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<TicketAdvisory>
				<xsl:choose>
					<xsl:when test=". ='T/'">
						<xsl:choose>
							<xsl:when test="../../PRC_INF/TIC_REC_PRC_QUO/DOC_INS/DOC_INS_TIC_FIE">
								<xsl:value-of select="../../PRC_INF/TIC_REC_PRC_QUO/DOC_INS/DOC_INS_TIC_FIE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="."/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="."/>
					</xsl:otherwise>
				</xsl:choose>

			</TicketAdvisory>
		</Ticketing>
	</xsl:template>

	<xsl:template match="Text">
		<Text>
			<xsl:value-of select="."/>
		</Text>
	</xsl:template>

	<xsl:template match="AIR_ITM">
		<Item>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:choose>
					<xsl:when test="SEG_NUM!=''">
						<xsl:value-of select="SEG_NUM" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="position()" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>

			<xsl:choose>
				<xsl:when test="FLI_NUM='ARNK'">
					<TPA_Extensions>
						<Arnk/>
					</TPA_Extensions>
				</xsl:when>
				<xsl:otherwise>
					<Air>
						<xsl:attribute name="RPH">
							<xsl:choose>
								<xsl:when test="SEG_NUM!=''">
									<xsl:value-of select="SEG_NUM" />
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="position()" />
								</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<xsl:attribute name="NumberInParty">
							<xsl:value-of select="../../PAX_INF/ITM_COU"/>
						</xsl:attribute>
						<xsl:attribute name="ResBookDesigCode">
							<xsl:value-of select="CLA_COD"/>
						</xsl:attribute>
						<xsl:attribute name="Status">
							<xsl:value-of select="SEG_STA"/>
						</xsl:attribute>
						<xsl:attribute name="DepartureDateTime">
							<xsl:value-of select="substring(FLI_DAT,6)"/>
							<xsl:text>-</xsl:text>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(FLI_DAT,3,3)"/>
								</xsl:with-param>
							</xsl:call-template>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(FLI_DAT,1,2)"/>
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring(DEP_TIM,1,2)"/>
							<xsl:text>:</xsl:text>
							<xsl:value-of select="substring(DEP_TIM,3,4)"/>
							<xsl:text>:00</xsl:text>
						</xsl:attribute>
						<xsl:attribute name="ArrivalDateTime">
							<xsl:value-of select="substring(ARR_DAT,6)"/>
							<xsl:text>-</xsl:text>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(ARR_DAT,3,3)"/>
								</xsl:with-param>
							</xsl:call-template>
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(ARR_DAT,1,2)"/>
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring(ARR_TIM,1,2)"/>
							<xsl:text>:</xsl:text>
							<xsl:value-of select="substring(ARR_TIM,3,4)"/>
							<xsl:text>:00</xsl:text>
						</xsl:attribute>
						<xsl:attribute name="FlightNumber">
							<!--<xsl:value-of select="FLI_NUM"/>-->
							<xsl:number value="FLI_NUM" format="0001"/>
						</xsl:attribute>
						<xsl:if test="E_TIC_ELI != ''">
							<xsl:attribute name="E_TicketEligibility">
								<xsl:choose>
									<xsl:when test="E_TIC_ELI = 'Y'">Eligible</xsl:when>
									<xsl:otherwise>NotEligible</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="NUM_STO != ''">
							<xsl:attribute name="StopQuantity">
								<xsl:value-of select="NUM_STO"/>
							</xsl:attribute>
						</xsl:if>
						<xsl:if test="FLI_DAY != ''">
							<xsl:attribute name="DepartureDay">
								<xsl:choose>
									<xsl:when test="FLI_DAY = 'Monday'">Mon</xsl:when>
									<xsl:when test="FLI_DAY = 'Tuesday'">Tue</xsl:when>
									<xsl:when test="FLI_DAY = 'Wednesday'">Wed</xsl:when>
									<xsl:when test="FLI_DAY = 'Thursday'">Thu</xsl:when>
									<xsl:when test="FLI_DAY = 'Friday'">Fri</xsl:when>
									<xsl:when test="FLI_DAY = 'Saturday'">Sat</xsl:when>
									<xsl:otherwise>Sun</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
						</xsl:if>
						<DepartureAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="DEP_ARP"/>
							</xsl:attribute>
							<xsl:if test="DEP_TER != '' and DEP_TER != '*'">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="DEP_TER"/>
								</xsl:attribute>
							</xsl:if>
						</DepartureAirport>
						<ArrivalAirport>
							<xsl:attribute name="LocationCode">
								<xsl:value-of select="ARR_ARP"/>
							</xsl:attribute>
							<xsl:if test="ARR_TER != '' and ARR_TER != '*'">
								<xsl:attribute name="Terminal">
									<xsl:value-of select="ARR_TER"/>
								</xsl:attribute>
							</xsl:if>
						</ArrivalAirport>
						<xsl:choose>
							<xsl:when test="ADD_FLI_SVC/COM/COD_SHA_INF">
								<OperatingAirline>
									<xsl:attribute name="Code">
										<xsl:apply-templates select="ADD_FLI_SVC/COM/COD_SHA_INF" mode="AirlineCode"/>
									</xsl:attribute>
									<xsl:choose>
										<xsl:when test ="contains(ADD_FLI_SVC/COM/COD_SHA_INF, ' FOR ')">
											<xsl:value-of select="substring-after(substring-before(translate(ADD_FLI_SVC/COM/COD_SHA_INF, '/',''), ' FOR '), ' BY ')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="substring-after(translate(ADD_FLI_SVC/COM/COD_SHA_INF, '/',''), ' BY ')"/>
										</xsl:otherwise>
									</xsl:choose>
								</OperatingAirline>
							</xsl:when>
							<xsl:otherwise>
								<OperatingAirline></OperatingAirline>
							</xsl:otherwise>
						</xsl:choose>
						<Equipment>
							<xsl:attribute name="AirEquipType">
								<xsl:value-of select="ADD_FLI_SVC/EQP_TYP"/>
							</xsl:attribute>
						</Equipment>
						<MarketingAirline>
							<xsl:attribute name="Code">
								<xsl:value-of select="ARL_COD"/>
							</xsl:attribute>
						</MarketingAirline>
						<xsl:if test="MAR_SEG_NUM">
							<MarriageGrp>
								<xsl:value-of select="MAR_SEG_NUM"/>
							</MarriageGrp>
						</xsl:if>
						<TPA_Extensions>
							<xsl:attribute name="ConfirmationNumber">
								<xsl:if test="DRC_RES_LOC!=''">
									<xsl:value-of select="substring(DRC_RES_LOC,6)"/>
								</xsl:if>
							</xsl:attribute>
						</TPA_Extensions>
					</Air>
				</xsl:otherwise>
			</xsl:choose>

		</Item>
	</xsl:template>

	<xsl:template match="TVL_ITM">
		<Item>
			<xsl:attribute name="Status">
				<xsl:value-of select="SEG_STA"/>
			</xsl:attribute>
			<xsl:attribute name="ItinSeqNumber">
				<xsl:choose>
					<xsl:when test="SEG_NUM!=''">
						<xsl:value-of select="SEG_NUM" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="position()" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<General>
				<xsl:attribute name="Start">
					<xsl:value-of select="concat(substring(STR_DAT,6),'-')"/>
					<xsl:call-template name="month">
						<xsl:with-param name="month">
							<xsl:value-of select="substring(STR_DAT,3,3)"/>
						</xsl:with-param>
					</xsl:call-template>
					<xsl:value-of select="concat('-',substring(STR_DAT,1,2))"/>
				</xsl:attribute>
				<Description>
					<xsl:value-of select="'Miscellaneous'"/>
					<xsl:if test="ASC_NME!=''">
						<xsl:text> - </xsl:text>
						<xsl:value-of select="ASC_NME"/>
					</xsl:if>
				</Description>
				<TPA_Extensions>
					<xsl:attribute name="Status">
						<xsl:value-of select="SEG_STA"/>
					</xsl:attribute>
					<xsl:attribute name="NumberInParty">
						<xsl:value-of select="NUM_ITM"/>
					</xsl:attribute>
					<Vendor>
						<xsl:attribute name="Code">
							<xsl:value-of select="VEN_COD"/>
						</xsl:attribute>
					</Vendor>
					<OriginCityCode>None</OriginCityCode>
				</TPA_Extensions>
			</General>
		</Item>
	</xsl:template>

	<xsl:template match="ADDRESS_INFO">
		<xsl:if test="BILLING_LINE_INFO!=''">
			<AddressGroup>
				<xsl:attribute name="Type">O</xsl:attribute>
				<xsl:if test="BILLING_LINE_INFO!=''">
					<xsl:attribute name="Use">B</xsl:attribute>
				</xsl:if>
				<ElementNumber>
					<xsl:value-of select="position()"/>
				</ElementNumber>
				<AddressText>
					<xsl:value-of select="BILLING_LINE_INFO[position()=1]"/>
					<xsl:value-of select="BILLING_LINE_INFO[position()=2]"/>
					<xsl:value-of select="BILLING_LINE_INFO[position()=3]"/>
					<xsl:value-of 	select="BILLING_LINE_INFO[position()=4]"/>
					<xsl:value-of select="BILLING_LINE_INFO[position()=5]"/>
					<xsl:value-of select="BILLING_LINE_INFO[position()=6]"/>
				</AddressText>
			</AddressGroup>
		</xsl:if>
		<xsl:if test="MAILING_LINE_INFO!=''">
			<AddressGroup>
				<xsl:attribute name="Type">O</xsl:attribute>
				<xsl:attribute name="Use">D</xsl:attribute>
				<ElementNumber>
					<xsl:value-of select="position()"/>
				</ElementNumber>
				<AddressText>
					<xsl:value-of select="MAILING_LINE_INFO[position()=1]"/>
					<xsl:value-of select="MAILING_LINE_INFO[position()=2]"/>
					<xsl:value-of select="MAILING_LINE_INFO[position()=3]"/>
					<xsl:value-of select="MAILING_LINE_INFO[position()=4]"/>
					<xsl:value-of select="MAILING_LINE_INFO[position()=5]"/>
					<xsl:value-of select="MAILING_LINE_INFO[position()=6]"/>
				</AddressText>
			</AddressGroup>
		</xsl:if>
	</xsl:template>

	<xsl:template match="HTL_ITEM">
		<HotelSegment>
			<ElementNumber>
				<xsl:value-of select="SEG_NUM"/>
			</ElementNumber>
			<TravelerElementNumber>!func:GetGlobal(HTravelerIDRef)</TravelerElementNumber>
			<Hotel>
				<CheckInDate>
					(<xsl:value-of select="concat(string(CITY_IN_OUT/IN_DATE/IN_DAY), string(CITY_IN_OUT/IN_DATE/IN_MONTH))"/>)
				</CheckInDate>
				<CheckOutDate>
					(<xsl:value-of select="concat(string(CITY_IN_OUT/OUT_DATE/OUT_DAY), string(CITY_IN_OUT/OUT_DATE/OUT_MONTH))"/>)
				</CheckOutDate>
				<NumberOfNights>
					!func:Math( SubDate , <xsl:value-of select="CITY_IN_OUT/OUT_DATE/OUT_DAY"/><xsl:value-of select="CITY_IN_OUT/OUT_DATE/OUT_MONTH"/>,<xsl:value-of select="CITY_IN_OUT/IN_DATE/IN_DAY"/><xsl:value-of select="CITY_IN_OUT/IN_DATE/IN_MONTH"/>, ddMMM, ddMMM)
				</NumberOfNights>
				<NumberOfPersons>
					<xsl:value-of select="NUMBER_OF_PERSONS"/>
				</NumberOfPersons>
				<ChainCode>
					<xsl:value-of select="HTL_CHAIN_CODE"/>
				</ChainCode>
				<ChainName>
					(Hotels,<xsl:value-of select="HTL_CHAIN_CODE"/>)
				</ChainName>
				<PropertyCode>
					<xsl:value-of select="HTL_PROP_CODE"/>
				</PropertyCode>
				<PropertyName>
					<xsl:value-of select="HTL_PROP_NAME"/>
				</PropertyName>
				<CityCode>
					<xsl:value-of select="CITY_IN_OUT/HTL_CITY_CODE"/>
				</CityCode>
				<CityName>
					(Airports, <xsl:value-of select="CITY_IN_OUT/HTL_CITY_CODE" />)
				</CityName>
				<CurrencyCode>
					<xsl:variable name="HtlDeci" select="substring-after(string(//Collects/Collect/HBW/ROOM_RATE),'.')"/>
					<xsl:variable name="HtlNoDeci" select="string-length($HtlDeci)"/>
					<xsl:attribute name="DecimalPlaces">
						<xsl:value-of select="$HtlNoDeci" />
					</xsl:attribute>
					<xsl:value-of select="RATE_CUR_CODE"/>
				</CurrencyCode>
			</Hotel>
			<Rooms>
				<BookingCode>
					<xsl:value-of select="ROOM_TYPE"/>
				</BookingCode>
				<RateCategory>!func:GetGlobal(RateCategory)</RateCategory>
				<RoomType>
					<xsl:value-of select="ROOM_TYPE"/>
				</RoomType>
				<RoomTypeDescription>
					(RoomTypes,<xsl:value-of select="ROOM_TYPE"/>)
				</RoomTypeDescription>
				<ActionCode>
					<xsl:value-of select="HTL_STATUS_ROOM/HTL_STATUS_CODE"/>
				</ActionCode>
				<NumberOfRooms>
					<xsl:value-of select="HTL_STATUS_ROOM/NUMBER_OF_ROOMS"/>
				</NumberOfRooms>
				<RateCode>!func:GetGlobal(RateCode)</RateCode>
				<xsl:choose>
					<xsl:when test="RATE_DESCRIPTION!=''">
						<RateCodeDescription>
							<xsl:value-of select="RATE_DESCRIPTION"/>
						</RateCodeDescription>
					</xsl:when>
					<xsl:otherwise>
						<RateCodeDescription>!func:GetGlobal(RateCode)</RateCodeDescription>
					</xsl:otherwise>
				</xsl:choose>
				<xsl:if test="//Collects/Collect/HBW/ROOM_RATE!=''">
					<RateAmount>
						<xsl:value-of select="translate(string(//Collects/Collect/HBW/ROOM_RATE),'.','')" />
					</RateAmount>
				</xsl:if>
				<xsl:choose>
					<xsl:when test="Rate_CHG_MAY_APPLY!=''">
						<RateChange>
							<xsl:value-of select="RATE-CHG-MAY-APPLY"/>
						</RateChange>
					</xsl:when>
					<xsl:otherwise>
						<RateChange>N</RateChange>
					</xsl:otherwise>
				</xsl:choose>
			</Rooms>
			<xsl:if test="EXTRA_CHARGES">
				<RoomOptions>
					<ExtraAdult>
						<xsl:value-of select="EXTRA_CHARGES/EA_AMOUNT"/>
					</ExtraAdult>
					<ExtraChild>
						<xsl:value-of select="EXTRA_CHARGES/EC_AMOUNT"/>
					</ExtraChild>
					<RollawayAdult>
						<xsl:value-of select="EXTRA_CHARGES/RA_AMOUNT"/>
					</RollawayAdult>
					<RollawayChild>
						<xsl:value-of select="EXTRA_CHARGES/RC_AMOUNT"/>
					</RollawayChild>
					<Crib>
						<xsl:value-of select="EXTRA_CHARGES/CR_AMOUNT"/>
					</Crib>
				</RoomOptions>
			</xsl:if>
			<xsl:if test="HTL_CONF_NBR!=''">
				<ConfirmationNumber>
					<xsl:value-of select="HTL_CONF_NBR"/>
				</ConfirmationNumber>
			</xsl:if>
			<xsl:if test="HTL_GUARANTEE_INFO!=''">
				<SupplementalInformation>
					<PaymentGuarantee>
						<CreditCard>
							<CCCode>
								<xsl:value-of select="substring(string(HTL_GUARANTEE_INFO),3,2)"/>
							</CCCode>
							<xsl:variable name="CCHotelOne">
								<xsl:value-of select="substring(string(HTL_GUARANTEE_INFO),5,35)"/>
							</xsl:variable>
							<xsl:variable name="CCHotelTwo">
								<xsl:value-of select="substring-before(string($CCHotelOne),'E')"/>
							</xsl:variable>
							<CCNumber>
								<xsl:value-of select="$CCHotelTwo"/>
							</CCNumber>
							<xsl:variable name="CCExpMonth">
								<xsl:value-of select="substring-after(string(HTL_GUARANTEE_INFO),'P')"/>
							</xsl:variable>
							<xsl:variable name="CCExpMonthTwo">
								<xsl:value-of select="substring-before(string($CCExpMonth),'-')"/>
							</xsl:variable>
							<xsl:variable name="CCExpYear">
								<xsl:value-of select="substring-after(string($CCExpMonth),'-')"/>
							</xsl:variable>
							<CCExpiration>
								<Month>
									<xsl:value-of select="$CCExpMonthTwo"/>
								</Month>
								<Year>
									<xsl:value-of select="$CCExpYear"/>
								</Year>
							</CCExpiration>
						</CreditCard>
					</PaymentGuarantee>
				</SupplementalInformation>
			</xsl:if>
		</HotelSegment>
	</xsl:template>

	<xsl:template match="PSG_ITEM" mode="TravelerElementNumber">
		<xsl:if test="CUSTOM_NAME_DATA!=''">
			<xsl:if test="CUSTOM_NAME_DATA!='*'">
				<TravelerElementNumber>
					<xsl:value-of select="CUSTOM_NAME_DATA"/>
				</TravelerElementNumber>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="CAR_ITEM">
		<CarSegment>
			<xsl:variable name="SegNum">
				<xsl:value-of select="SEG_NUM"/>
			</xsl:variable>
			<ElementNumber>
				<xsl:value-of select="SEG_NUM"/>
			</ElementNumber>
			<xsl:variable name="Passenger">
				<xsl:value-of select="RENTER_NAME"/>
			</xsl:variable>
			<TravelerElementNumber>!func:GetGlobal(TravelerIDRef)</TravelerElementNumber>
			<NumberOfCars>
				<xsl:value-of select="VENDOR_INFO/NUMBER_OF_CARS"/>
			</NumberOfCars>
			<PickUp>
				<AirportCode>
					<xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE"/>
				</AirportCode>
				<AirportName>
					(Airports, <xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE" />)
				</AirportName>
				<xsl:variable name="Date">
					<xsl:value-of select="concat(string(VENDOR_INFO/PICKUP_DATE/PICKUP_DAY), string (VENDOR_INFO/PICKUP_DATE/PICKUP_MONTH))"/>
				</xsl:variable>
				<Date>
					(<xsl:value-of select="$Date"/>)
				</Date>
				<xsl:variable name="PickUpTime">
					<xsl:value-of select="substring-after(string(ARR_FLIGHT_INFO),'-')"/>
				</xsl:variable>
				<Time>
					<xsl:value-of select="substring(string($PickUpTime),1,2)"/>:<xsl:value-of select="substring(string($PickUpTime),3,2)"/>
				</Time>
				<xsl:variable name="Flight">
					<xsl:value-of select="substring(string(ARR_FLIGHT_INFO),3,10)"/>
				</xsl:variable>
				<xsl:variable name="Flighttwo">
					<xsl:value-of select="substring-before(string($Flight),'-')"/>
				</xsl:variable>
				<FlightArrival>
					<AirlineCode>
						<xsl:value-of select="substring(string(ARR_FLIGHT_INFO),1,2)"/>
					</AirlineCode>
					<AirlineName>
						(Airlines, <xsl:value-of select="substring(string(ARR_FLIGHT_INFO),1,2)"/>)
					</AirlineName>
					<FlightNumber>
						<xsl:value-of select="$Flighttwo"/>
					</FlightNumber>
				</FlightArrival>
			</PickUp>
			<DropOff>
				<AirportCode>
					<xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE"/>
				</AirportCode>
				<AirportName>
					(Airports, <xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE" />)
				</AirportName>
				<Date>
					(<xsl:value-of select="concat(string(VENDOR_INFO/DROP_OFF_DATE/DROP_OFF_DAY), string(VENDOR_INFO/DROP_OFF_DATE/DROP_OFF_MONTH))"/>)
				</Date>
				<xsl:if test="DROP_OFF_TIME!=''">
					<Time>
						<xsl:value-of select="substring(string(DROP_OFF_TIME),1,2)"/>:<xsl:value-of select="substring(string(DROP_OFF_TIME),3,2)"/>
					</Time>
				</xsl:if>
			</DropOff>
			<CarData>
				<CarVendorCode>
					<xsl:value-of select="VENDOR_INFO/CAR_VENDOR_CODE"/>
				</CarVendorCode>
				<CarVendorName>
					(Cars,<xsl:value-of select="VENDOR_INFO/CAR_VENDOR_CODE"/>)
				</CarVendorName>
				<Location>
					<CityCode>
						<xsl:value-of select="VENDOR_INFO/PICKUP_CITY_CODE"/>
					</CityCode>
					<Category>
						<xsl:value-of select="LOCATION/LOCATION_CODE"/>
					</Category>
					<Number>
						<xsl:value-of select="LOCATION/LOCATION_CODE_EXT"/>
					</Number>
				</Location>
				<CarType>
					<xsl:value-of select="CAR_FLAGS/CAR_CLASS_CODE"/>
					<xsl:value-of select="CAR_FLAGS/CAR_TYPE_CODE"/>
					<xsl:value-of select="CAR_FLAGS/SHIFT_TYPE_CODE"/>
					<xsl:value-of select="CAR_FLAGS/AIR_COND_CODE"/>
				</CarType>
				<CarTypeDescription>
					(CarTypes,<xsl:value-of select="CAR_FLAGS/CAR_CLASS_CODE"/><xsl:value-of select="CAR_FLAGS/CAR_TYPE_CODE"/><xsl:value-of select="CAR_FLAGS/SHIFT_TYPE_CODE"/><xsl:value-of select="CAR_FLAGS/AIR_COND_CODE"/>)
				</CarTypeDescription>
				<ActionCode>
					<xsl:value-of select="VENDOR_INFO/CAR_STATUS_CODE"/>
				</ActionCode>
				<Rate>
					<xsl:apply-templates select="RATE_INFO/RATE_PLAN_CODE"/>
					<xsl:if test="RATE_INFO/RATE_CATEGORY">
						<xsl:attribute name="Category">
							<xsl:value-of select="RATE_INFO/RATE_CATEGORY"/>
						</xsl:attribute>
					</xsl:if>
					<RateCode>
						<xsl:value-of select="RATE_CODE"/>
					</RateCode>
					<RateAmount>
						<xsl:value-of select="translate(string(RATE_INFO/RATE_AMAOUNT),'.','')" />
					</RateAmount>
					<xsl:variable name="CarDeci" select="substring-after(string(RATE_INFO/RATE_AMAOUNT),'.')"/>
					<xsl:variable name="CarNoDeci" select="string-length($CarDeci)"/>
					<CurrencyCode>
						<xsl:attribute name="DecimalPlaces">
							<xsl:value-of select="$CarNoDeci" />
						</xsl:attribute>
						<xsl:value-of select="RATE_INFO/CURR_CODE"/>
					</CurrencyCode>
					<xsl:if test="RATE_INFO/MILEAGE_INFO/MILEAGE_CHARGE!='' or RATE_INFO/MILEAGE_INFO/FREE_MILEAGE!=''">
						<MileKmRate>
							<xsl:value-of select="RATE_INFO/MILEAGE_INFO/MILEAGE_CHARGE"/>
							<xsl:value-of select="RATE_INFO/MILEAGE_INFO/FREE_MILEAGE"/>
						</MileKmRate>
					</xsl:if>
				</Rate>
				<xsl:if test="RATE_INFO/EXTRA_DAY_RATE!='' or RATE_INFO/EXTRA_HOUR_RATE!=''">
					<ExtraCharges>
						<xsl:if test="RATE_INFO/EXTRA_DAY_RATE!=''">
							<xsl:attribute name="Type">D</xsl:attribute>
						</xsl:if>
						<ExtraChargesAmount>xsl:value-of select="RATE_INFO/EXTRA_DAY_RATE"/></ExtraChargesAmount>
						<MileKmLimit>
							<xsl:value-of select="RATE_INFO/EXTRA_DAY_MILEAGE/FREE_MILEAGE"/>
						</MileKmLimit>
						<xsl:if test="RATE_INFO/EXTRA_HOUR_RATE!=''">
							<xsl:attribute name="Type">H</xsl:attribute>
							<ExtraChargesAmount>
								<xsl:value-of select="RATE_INFO/EXTRA_HOUR_RATE"/>
							</ExtraChargesAmount>
							<MileKmLimit>
								<xsl:value-of select="RATE_INFO/EXTRA_HOUR_MILEAGE/FREE_MILEAGE"/>
							</MileKmLimit>
						</xsl:if>
						<xsl:if test="RATE_INFO/EXTRA_WEEK_RATE!=''">
							<xsl:attribute name="Type">H</xsl:attribute>
							<ExtraChargesAmount>
								<xsl:value-of select="RATE_INFO/EXTRA_WEEK_RATE"/>
							</ExtraChargesAmount>
						</xsl:if>
					</ExtraCharges>
				</xsl:if>
				<xsl:if test="SPECIAL_EQUIP_CODES!=''">
					<OptionalEquipment>
						<EquipmentType>
							<xsl:value-of select="SPECIAL_EQUIP_CODES"/>
						</EquipmentType>
					</OptionalEquipment>
				</xsl:if>
			</CarData>
			<xsl:if test="CONFIRMATION_NUM!=''">
				<ConfirmationNumber>
					<xsl:value-of select="CONFIRMATION_NUM"/>
				</ConfirmationNumber>
			</xsl:if>
			<SupplementalInformation>
				<BookingSource>
					<xsl:value-of select="BS_IATA_NUM"/>
				</BookingSource>
				<xsl:if test="CORP_DISCOUNT_NUM!=''">
					<CorporateDiscountNumber>
						<xsl:value-of select="CORP_DISCOUNT_NUM"/>
					</CorporateDiscountNumber>
				</xsl:if>
				<xsl:if test="GUARANTEE_INFO">
					<PaymentGuarantee>
						<CreditCard>
							<CCCode>
								<xsl:value-of select="substring(string(GUARANTEE_INFO),3,2)"/>
							</CCCode>
							<xsl:variable name="CCOne">
								<xsl:value-of select="substring(string(GUARANTEE_INFO),5,35)"/>
							</xsl:variable>
							<xsl:variable name="CCTwo">
								<xsl:value-of select="substring-before(string($CCOne),'E')"/>
							</xsl:variable>
							<CCNumber>
								<xsl:value-of select="$CCTwo"/>
							</CCNumber>
							<xsl:variable name="CCExpMonth">
								<xsl:value-of select="substring-after(string(GUARANTEE_INFO),'P')"/>
							</xsl:variable>
							<xsl:variable name="CCExpMonthTwo">
								<xsl:value-of select="substring-before(string($CCExpMonth),'-')"/>
							</xsl:variable>
							<xsl:variable name="CCExpYear">
								<xsl:value-of select="substring-after(string($CCExpMonth),'-')"/>
							</xsl:variable>
							<CCExpiration>
								<Month>
									<xsl:value-of select="$CCExpMonthTwo"/>
								</Month>
								<Year>
									<xsl:value-of select="$CCExpYear"/>
								</Year>
							</CCExpiration>
						</CreditCard>
					</PaymentGuarantee>
				</xsl:if>
				<xsl:if test="GUARANTEE_INFO"></xsl:if>
				<xsl:if test="SUPPLE_INFO!=''">
					<AdditionalInformation>
						<xsl:value-of select="SUPPLE_INFO"/>
					</AdditionalInformation>
				</xsl:if>
			</SupplementalInformation>
		</CarSegment>
	</xsl:template>

	<xsl:template match="RATE_PLAN_CODE">
		<xsl:attribute name="Type">
			<xsl:choose>
				<xsl:when test=".='DY'">D</xsl:when>
				<xsl:when test=".='WK'">W</xsl:when>
				<xsl:when test=".='MY'">M</xsl:when>
				<xsl:when test=".='WD'">E</xsl:when>
				<xsl:when test=".='HR'">H</xsl:when>
			</xsl:choose>
		</xsl:attribute>
	</xsl:template>

	<xsl:template match="AIR_ITEM" mode="SEAT">
		<AirlineCode>
			<xsl:value-of select="AIRLINE_CODE"/>
		</AirlineCode>
		<AirlineName>
			(Airlines,<xsl:value-of select="AIRLINE_CODE"/>)
		</AirlineName>
		<FlightNumber>
			<xsl:value-of select="FLIGHT_NUM"/>
		</FlightNumber>
		<Date>
			(<xsl:value-of select="concat(string(DEP_DATE/DEP_DAY), string(DEP_DATE/DEP_MONTH))"/>)
		</Date>
		<DepartureAirportCode>
			<xsl:value-of select="DEP_AIRPORT"/>
		</DepartureAirportCode>
		<DepartureAirportName>
			(Airlines,<xsl:value-of select="DEP_AIRPORT"/>)
		</DepartureAirportName>
		<ArrivalAirportCode>
			<xsl:value-of select="ARR_AIRPORT"/>
		</ArrivalAirportCode>
		<ArrivalAirportName>
			>(Airlines,<xsl:value-of select="ARR_AIRPORT"/>)
		</ArrivalAirportName>
		<ClassOfService>
			<xsl:value-of select="DEP_CLASS"/>
		</ClassOfService>
	</xsl:template>

	<xsl:template match="AIR_ITEM" mode="SEATTWO">
		<!-- SD Done -->
		<AirlineCode>
			<xsl:value-of select="AIRLINE_CODE"/>
		</AirlineCode>
		<AirlineName>
			(Airlines,<xsl:value-of select="AIRLINE_CODE"/>)
		</AirlineName>
		<FlightNumber>
			<xsl:value-of select="FLIGHT_NUM"/>
		</FlightNumber>
		<Date>
			(<xsl:value-of select="concat(string(DEP_DATE/DEP_DAY), string(DEP_DATE/DEP_MONTH))"/>)
		</Date>
		<DepartureAirportCode>
			<xsl:value-of select="DEP_AIRPORT"/>
		</DepartureAirportCode>
		<DepartureAirportName>
			(Airlines,<xsl:value-of select="DEP_AIRPORT"/>)
		</DepartureAirportName>
		<ArrivalAirportCode>
			<xsl:value-of select="ARR_AIRPORT"/>
		</ArrivalAirportCode>
		<ArrivalAirportName>
			>(Airlines,<xsl:value-of select="ARR_AIRPORT"/>)
		</ArrivalAirportName>
		<ClassOfService>
			<xsl:value-of select="DEP_CLASS"/>
		</ClassOfService>
	</xsl:template>

	<xsl:template match="OSI_INFO">
		<OtherServiceInformation>
			<ElementNumber>
				<xsl:value-of select="position()"/>
			</ElementNumber>
			<AirlineCode>
				<xsl:value-of select="substring(string(.),4,2)"/>
			</AirlineCode>
			<Text>
				<xsl:value-of select="substring(string(.),6,15)"/>
			</Text>
		</OtherServiceInformation>
	</xsl:template>

	<xsl:template match="SSR_TEXT" mode="SSRONE">
		<xsl:variable name="Airl">
			<xsl:value-of select="substring(string(.),8,2)"/>
		</xsl:variable>
		<xsl:variable name="Seat">
			<xsl:value-of select="substring(string(.),4,4)"/>
		</xsl:variable>
		<xsl:variable name="FlightNo">
			<xsl:value-of select="substring(string(translate(string(.),' ','')),19,4)"/>
		</xsl:variable>
		<xsl:variable name="Name">
			<xsl:value-of select="substring-after(string(.),'-')"/>
		</xsl:variable>
		<xsl:variable name="Nametwo">
			<xsl:value-of select="substring-before(string($Name),' .')"/>
		</xsl:variable>

		<xsl:if test="$Seat!='SEAT'">
			<SpecialServiceRequest>
				<ElementNumber>
					<xsl:value-of select="position()"/>
				</ElementNumber>
				<xsl:if test="$Seat!='SEAT' and $Seat!='FQTV'">
					<xsl:apply-templates select="//AIR_SEGMENT_INFO/AIR_ITEM[FLIGHT_NUM=$FlightNo]" mode="SegmentElementNumber"/>

					<xsl:choose>
						<xsl:when test="translate ($Nametwo, ' ','') != ''">
							<xsl:apply-templates select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Nametwo]" mode="Association"/>
						</xsl:when>
						<xsl:when test="translate ($Nametwo, ' ','') = ''">
							<xsl:apply-templates select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Name]" mode="Association"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="//PSGR_DATA/PSG_ITEM" mode="SEATTWO"/>
						</xsl:otherwise>
					</xsl:choose>

					<!-- It should be like this
          <xsl:if test="../NME_POS != ''">
            <xsl:attribute name="TravelerRefNumberRPHList">
              <xsl:value-of select="../NME_POS"/>
            </xsl:attribute>
          </xsl:if>
          -->

					<SSRCode>
						<xsl:value-of select="substring(string(.),4,4)"/>
					</SSRCode>
					<AirlineCode>
						<xsl:value-of select="$Airl"/>
					</AirlineCode>
					<AirlineName>
						(Airlines,<xsl:value-of select="$Airl"/>)
					</AirlineName>
					<Text>
						<xsl:value-of select="substring(string(.),1,100)"/>
					</Text>
				</xsl:if>
			</SpecialServiceRequest>
		</xsl:if>
	</xsl:template>

	<xsl:template match="AIR_ITEM" mode="SegmentElementNumber">
		<SegmentElementNumber>
			<xsl:value-of select="SEG_NUM"/>
		</SegmentElementNumber>
	</xsl:template>

	<xsl:template match="SSR_TEXT" mode="SSRTWO">
		<xsl:variable name="Seat">
			<xsl:value-of select="substring(string(.),4,4)"/>
		</xsl:variable>
		<xsl:variable name="Unconf">
			<xsl:value-of select="substring(string(.),10,2)"/>
		</xsl:variable>
		<xsl:variable name="FlightNo">
			<xsl:value-of select="substring(string(.),19,4)"/>
		</xsl:variable>
		<xsl:variable name="Airl">
			<xsl:value-of select="substring(string(.),8,2)"/>
		</xsl:variable>
		<xsl:variable name="Nametemp">
			<xsl:value-of select="substring-after(string(.),'-')"/>
		</xsl:variable>
		<xsl:variable name="Name">
			<xsl:value-of select="substring(string($Nametemp),3,99)"/>
		</xsl:variable>
		<xsl:variable name="Namethree">
			<xsl:value-of select="substring(string(.),36,100)"/>
		</xsl:variable>
		<xsl:variable name="Namefour">
			<xsl:value-of select="translate($Namethree,' ','')"/>
		</xsl:variable>
		<xsl:variable name="SegNum">
			<xsl:value-of select="//AIR_ITEM/SEG_NUM"/>
		</xsl:variable>

		<xsl:if test="$Seat='SEAT'">
			<xsl:if test="$Unconf='NN' or $Unconf='UC'">
				<SpecialServiceRequest>
					<ElementNumber>
						<xsl:value-of select="position()"/>
					</ElementNumber>
					<xsl:apply-templates select="//AIR_SEGMENT_INFO/AIR_ITEM[FLIGHT_NUM=$FlightNo]" mode="SegmentElementNumber"/>
					<xsl:choose>
						<xsl:when test="contains(string(.),'-')">
							<Association>
								<TravelerElementNumber>
									<xsl:value-of select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Name]/NAME_POSITION"/>
								</TravelerElementNumber>
							</Association>
						</xsl:when>
						<xsl:otherwise>
							<Association>
								<TravelerElementNumber>
									<xsl:value-of select="//PSGR_DATA/PSG_ITEM[LAST_FIRST_MIDDLE=$Namefour]/NAME_POSITION"/>
								</TravelerElementNumber>
							</Association>
						</xsl:otherwise>
					</xsl:choose>

					<!-- It should be like this
          <xsl:if test="../NME_POS != ''">
            <xsl:attribute name="TravelerRefNumberRPHList">
              <xsl:value-of select="../NME_POS"/>
            </xsl:attribute>
          </xsl:if>
          -->

					<SSRCode>
						<xsl:value-of select="substring(string(.),4,4)"/>
					</SSRCode>
					<AirlineCode>
						<xsl:value-of select="$Airl"/>
					</AirlineCode>
					<AirlineName>
						(Airlines,<xsl:value-of select="$Airl"/>)
					</AirlineName>
					<Text>
						<xsl:value-of select="substring(string(.),1,100)"/>
					</Text>
				</SpecialServiceRequest>
			</xsl:if>
		</xsl:if>
	</xsl:template>

	<xsl:template match="SET_ITEM">
		<Seat>
			<ElementNumber>
				<xsl:value-of select="position()"/>
			</ElementNumber>
			<SegmentElementNumber>
				<xsl:value-of select="SEG_NUM"/>
			</SegmentElementNumber>
			<SeatStatus>R</SeatStatus>
			<Assignment>
				<TravelerElementNumber>
					<xsl:value-of select="PSG_NUM"/>
				</TravelerElementNumber>
				<SeatLocation>
					<xsl:value-of select="SEAT_DETAIL/SEAT_NUM"/>
				</SeatLocation>
				<Characteristic>
					<xsl:value-of select="SEAT_DETAIL/SEAT_TYPE"/>
				</Characteristic>
			</Assignment>
		</Seat>
	</xsl:template>

	<xsl:template match="PSG_ITEM" mode="Assignment">
		<TravelerElementNumber>
			<xsl:value-of select="NAME_POSITION"/>
		</TravelerElementNumber>
	</xsl:template>

	<xsl:template match="PSG_ITEM" mode="Association">
		<Association>
			<TravelerElementNumber>
				<xsl:value-of select="NAME_POSITION"/>
			</TravelerElementNumber>
		</Association>
	</xsl:template>

	<xsl:template match="PSG_ITEM" mode="SEATTWO">
		<Association>
			<TravelerElementNumber>
				<xsl:value-of select="NAME_POSITION"/>
			</TravelerElementNumber>
		</Association>
	</xsl:template>

	<xsl:template match="RMK_ITEM" mode="ITINRMKS">
		<xsl:variable name="Rmk" select="substring(string(RMK_TEXT),1,2)"/>
		<xsl:if test="$Rmk='RM'">
			<xsl:variable name="Type" select="substring(string(RMK_TEXT),4,3)"/>
			<ItineraryRemarks>
				<xsl:if test="$Type='AIR'">
					<AirRemark>
						<ElementNumber>
							<xsl:value-of select="RMK_NUMBER"/>
						</ElementNumber>
						<SegmentElementNumber>
							<xsl:value-of select="substring(RMK_TEXT,string-length(RMK_TEXT),1)"/>
						</SegmentElementNumber>
						<Text>
							<xsl:value-of select="RMK_TEXT"/>
						</Text>
					</AirRemark>
				</xsl:if>
				<xsl:if test="$Type='CAR'">
					<CarRemark>
						<ElementNumber>
							<xsl:value-of select="RMK_NUMBER"/>
						</ElementNumber>
						<SegmentElementNumber>
							<xsl:value-of select="substring(RMK_TEXT,string-length(RMK_TEXT),1)"/>
						</SegmentElementNumber>
						<Text>
							<xsl:value-of select="RMK_TEXT"/>
						</Text>
					</CarRemark>
				</xsl:if>
				<xsl:if test="$Type='HTL'">
					<HotelRemark>
						<ElementNumber>
							<xsl:value-of select="RMK_NUMBER"/>
						</ElementNumber>
						<SegmentElementNumber>
							<xsl:value-of select="substring(RMK_TEXT,string-length(RMK_TEXT),1)"/>
						</SegmentElementNumber>
						<Text>
							<xsl:value-of select="RMK_TEXT"/>
						</Text>
					</HotelRemark>
				</xsl:if>
			</ItineraryRemarks>
		</xsl:if>
	</xsl:template>

	<xsl:template match="RMK_ITEM" mode="OTHER">
		<xsl:variable name="Rmk" select="substring(string(RMK_TEXT),1,1)"/>
		<xsl:variable name="Rmk2" select="substring(string(RMK_TEXT),1,2)"/>
		<xsl:variable name="Rmk3" select="substring(string(RMK_CATEGORY),1,2)"/>
		<xsl:if test="$Rmk!='/' and $Rmk2!='RM' and $Rmk3!='MT' and $Rmk3!='MN'">
			<GeneralRemark>
				<ElementNumber>
					<xsl:value-of select="RMK_NUMBER"/>
				</ElementNumber>
				<Text>
					<xsl:value-of select="RMK_TEXT"/>
				</Text>
			</GeneralRemark>
		</xsl:if>
	</xsl:template>

	<xsl:template match="RMK_ITEM" mode="TKTRMK">
		<xsl:variable name="Rmk3" select="substring(string(RMK_CATEGORY),1,2)"/>
		<xsl:if test="$Rmk3='MT'">
			<TktRmks>
				<ElemNo>
					<xsl:value-of select="RMK_NUMBER"/>
				</ElemNo>
				<Text>
					<xsl:value-of select="RMK_TEXT"/>
				</Text>
			</TktRmks>
		</xsl:if>
	</xsl:template>

	<xsl:template match="AIR_ITEM" mode="RLOC">
		<xsl:if test="DIRECT_RLOC!='' and not(preceding-sibling::AIR_ITEM/DIRECT_RLOC = current()/DIRECT_RLOC)">
			<VendorRecLocs>
				<VendorCode>
					<xsl:value-of select="AIRLINE_CODE"/>
				</VendorCode>
				<RecLoc>
					<xsl:value-of select="DIRECT_RLOC"/>
				</RecLoc>
			</VendorRecLocs>
		</xsl:if>
	</xsl:template>

	<xsl:template match="SSR_ITM" mode="SSR">
		<SpecialServiceRequest>
			<xsl:if test="NME_POS != ''">
				<xsl:attribute name="TravelerRefNumberRPHList">
					<xsl:value-of select="NME_POS"/>
				</xsl:attribute>
			</xsl:if>

			<xsl:variable name="paxrph">
				<xsl:value-of select="NME_POS"/>
			</xsl:variable>

			<xsl:attribute name="SSRCode">
				<xsl:value-of select="SSR_COD"/>
			</xsl:attribute>

			<xsl:variable name="paxtype">
				<xsl:value-of select="../../PAX_INF/NME_ITM[NME_POS=$paxrph]/PTC"/>
			</xsl:variable>

			<xsl:variable name="vc">
				<xsl:choose>
					<xsl:when test="contains(../../PRC_INF/PRC_QUO/PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR, 'ROE')">
						<xsl:value-of select="substring(substring-after(substring-after(../../PRC_INF/PRC_QUO/PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR,'ROE'),' '),1,2)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="substring(substring-after(substring-after(../../PRC_INF/PRC_QUO/PTC_FAR_DTL[PTC=$paxtype]/FAR_LDR,'END'),' '),1,2)"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<Airline>
				<xsl:attribute name="Code">
					<xsl:choose>
						<xsl:when test="SEG_NUM">
							<xsl:value-of select="../../AIR_SEG_INF/AIR_ITM[SEG_NUM = SEG_NUM]/ARL_COD"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$vc"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
			</Airline>
			<Text>
				<xsl:value-of select="concat(SSR_STA,SSR_TXT)"/>
			</Text>
		</SpecialServiceRequest>
	</xsl:template>

	<xsl:template name="strip-zeroes">
		<xsl:param name="in"/>
		<xsl:choose>
			<xsl:when test="substring($in, 1, 1) = '0' and string-length($in) > 1">
				<xsl:call-template name="strip-zeroes">
					<xsl:with-param name="in" select="substring($in, 2, string-length($in) - 1)"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$in"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="month">
		<xsl:param name="month" />
		<xsl:choose>
			<xsl:when test="$month = 'JAN'">01</xsl:when>
			<xsl:when test="$month = 'FEB'">02</xsl:when>
			<xsl:when test="$month = 'MAR'">03</xsl:when>
			<xsl:when test="$month = 'APR'">04</xsl:when>
			<xsl:when test="$month = 'MAY'">05</xsl:when>
			<xsl:when test="$month = 'JUN'">06</xsl:when>
			<xsl:when test="$month = 'JUL'">07</xsl:when>
			<xsl:when test="$month = 'AUG'">08</xsl:when>
			<xsl:when test="$month = 'SEP'">09</xsl:when>
			<xsl:when test="$month = 'OCT'">10</xsl:when>
			<xsl:when test="$month = 'NOV'">11</xsl:when>
			<xsl:when test="$month = 'DEC'">12</xsl:when>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="PNR_4_INF" mode="equiv_ptc_count">
		<!-- 
      ADT 01 TTL-USD   992.46  BF-EUR   452.00  TAX-  506.46 
      CHD 01 TTL-USD   870.46  BF-EUR   339.00  TAX-  506.46
    -->
		<xsl:variable name="ptc_count">
			<xsl:call-template name="get_ptc_count">
				<xsl:with-param name="lineindex" select="1"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:value-of select="$ptc_count"/>
	</xsl:template>

	<xsl:template match="PNR_4_INF" mode="totalequiv">
		<!-- 
      ADT 01 TTL-USD   992.46  BF-EUR   452.00  TAX-  506.46 
      CHD 01 TTL-USD   870.46  BF-EUR   339.00  TAX-  506.46
    -->
		<xsl:variable name="amount">
			<xsl:call-template name="addamount">
				<xsl:with-param name="totalamount" select="0"/>
				<xsl:with-param name="lineindex" select="1"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:value-of select="$amount"/>
	</xsl:template>

	<xsl:template match="Line" mode="equiv">
		<!-- ADT 01 TTL-USD   992.46  BF-EUR   452.00  TAX-  506.46 -->

		<xsl:variable name="amount">
			<!-- EUR   452.00 -->
			<xsl:call-template name="string-trim">
				<xsl:with-param name="string" select="substring-after(substring-after(substring-before(., 'TAX-'), 'BF-'), ' ')" />
			</xsl:call-template>
		</xsl:variable>
		<!--
    <xsl:value-of select="format-number(translate($amount,'.',''), '#0')"/>
    -->
		<xsl:value-of select="$amount"/>
	</xsl:template>

	<xsl:template match="Line" mode="equivcur">
		<!-- ADT 01 TTL-USD   992.46  BF-EUR   452.00  TAX-  506.46 -->
		<xsl:variable name="cur">
			<!-- EUR   452.00 -->
			<xsl:call-template name="string-trim">
				<xsl:with-param name="string" select="substring-before(substring-after(substring-before(., 'TAX-'), 'BF-'), ' ')" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:value-of select="$cur"/>
	</xsl:template>

	<xsl:template name="sumitup">
		<xsl:param name="fValue" />
		<xsl:param name="total" />
		<xsl:value-of select="format-number(fValue + $total,'#0')"/>
	</xsl:template>

	<xsl:template match="COD_SHA_INF" mode="AirlineCode">
		<!-- 
    We need to pars line like: 
    OPERATED BY AZUL AIRLINES AD 4493 
    OPERATED BY /AVIANCA
    OPERATED BY VIRGIN AUST INTL FOR VA SE ASIA
	
	OPERATED BY FINNAIR FOR EW DISCOVER 4Y 65
    Attempt to extract airline code from it.
    -->
		<xsl:variable name="elems">
			<xsl:call-template name="tokenizeString">
				<xsl:with-param name="list" select="translate(., '/', '')"/>
				<xsl:with-param name="delimiter" select="' '"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="AirlineCode" >
			<xsl:choose>
				<xsl:when test="contains(., ' BY ') and contains(., ' FOR ')">
					<xsl:variable name="lastElem">
						<xsl:value-of select="msxsl:node-set($elems)/elem[position()=(last())]"/>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="number($lastElem) = $lastElem">
							<xsl:value-of select="msxsl:node-set($elems)/elem[position()=(last() - 1)]"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="' '"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="count(msxsl:node-set($elems)/elem) > 3">
					<xsl:value-of select="msxsl:node-set($elems)/elem[position()=(last() - 1)]"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="' '"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:if test="string-length($AirlineCode)=2">
			<xsl:value-of select="$AirlineCode"/>
		</xsl:if>

		<xsl:if test="string-length($AirlineCode)!=2">
			<xsl:value-of select="''"/>
		</xsl:if>

	</xsl:template>

	<xsl:variable name="whitespace" select="'&#09;&#10;&#13; '" />

	<!-- Strips trailing whitespace characters from 'string' -->
	<xsl:template name="string-rtrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:variable name="length" select="string-length($string)" />

		<xsl:if test="$length &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, $length, 1))">
					<xsl:call-template name="string-rtrim">
						<xsl:with-param name="string" select="substring($string, 1, $length - 1)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading whitespace characters from 'string' -->
	<xsl:template name="string-ltrim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />

		<xsl:if test="string-length($string) &gt; 0">
			<xsl:choose>
				<xsl:when test="contains($trim, substring($string, 1, 1))">
					<xsl:call-template name="string-ltrim">
						<xsl:with-param name="string" select="substring($string, 2)" />
						<xsl:with-param name="trim"   select="$trim" />
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$string" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<!-- Strips leading and trailing whitespace characters from 'string' -->
	<xsl:template name="string-trim">
		<xsl:param name="string" />
		<xsl:param name="trim" select="$whitespace" />
		<xsl:call-template name="string-rtrim">
			<xsl:with-param name="string">
				<xsl:call-template name="string-ltrim">
					<xsl:with-param name="string" select="$string" />
					<xsl:with-param name="trim"   select="$trim" />
				</xsl:call-template>
			</xsl:with-param>
			<xsl:with-param name="trim"   select="$trim" />
		</xsl:call-template>
	</xsl:template>

	<!-- Examples of use trim function
  <ltrim>
    <xsl:call-template name="string-ltrim">
      <xsl:with-param name="string" select="'   &#10;   test  '" />
    </xsl:call-template>
  </ltrim>
  <rtrim>
    <xsl:call-template name="string-rtrim">
      <xsl:with-param name="string" select="'    &#10;    test  &#10;  '" />
    </xsl:call-template>
  </rtrim>
  <trim>
    <xsl:call-template name="string-trim">
      <xsl:with-param name="string" select="'    &#10;    test  &#10;  '" />
    </xsl:call-template>
  </trim>
  -->

	<xsl:template name="get_ptc_count">
		<xsl:param name="lineindex"/>
		<xsl:choose>
			<xsl:when test="Line[contains(., 'TTL-')][position()=$lineindex]">
				<xsl:variable name="ptc_count">
					<xsl:value-of select="substring(Line[contains(., 'TTL-')][position()=$lineindex],5,2)"/>
				</xsl:variable>
				<xsl:value-of select="$ptc_count"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="0"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>


	<xsl:template name="addamount">
		<xsl:param name="totalamount"/>
		<xsl:param name="lineindex"/>
		<xsl:choose>
			<xsl:when test="Line[contains(., 'TTL-')][position()=$lineindex]">
				<xsl:variable name="ef">
					<xsl:apply-templates select="Line[contains(., 'TTL-')][position()=$lineindex]" mode="equiv" />
				</xsl:variable>
				<xsl:call-template name="addamount">
					<xsl:with-param name="totalamount">
						<xsl:value-of select="$totalamount + $ef"/>
					</xsl:with-param>
					<xsl:with-param name="lineindex">
						<xsl:value-of select="$lineindex + 1"/>
					</xsl:with-param>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$totalamount"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--############################################################-->
	<!--## Template to tokenize strings                           ##-->
	<!--############################################################-->
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
	<!--
  ############################################################
  ## Template to tokenize strings based on the length       ##
  ############################################################
  -->
	<xsl:template name="wrap">
		<xsl:param name="text" select="."/>
		<xsl:param name="line-length" select="29"/>
		<xsl:param name="carry">
			<xsl:variable name="lengths">
				<xsl:for-each select="preceding-sibling::text()">
					<length>
						<xsl:value-of select="string-length()"/>
					</length>
				</xsl:for-each>
			</xsl:variable>
			<xsl:value-of select="sum(msxsl:node-set($lengths)/length) mod $line-length"/>
		</xsl:param>

		<xsl:value-of select="substring($text, 1, $line-length - $carry)"/>
		<xsl:text>/</xsl:text>
		<xsl:if test="$carry + string-length($text) > $line-length">
			<!-- recursive call -->
			<xsl:call-template name="wrap">
				<xsl:with-param name="text" select="substring($text, $line-length - $carry + 1)"/>
				<xsl:with-param name="carry" select="0"/>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>