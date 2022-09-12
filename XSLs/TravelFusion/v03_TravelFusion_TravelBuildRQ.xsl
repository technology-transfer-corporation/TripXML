<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- v03_TravelFusion_TravelBuildRQ.xsl												-->
	<!-- ================================================================== -->
	<!-- Date: 21 Oct 2013 - Rastko - send fake booking request only when System is test		-->
	<!-- Date: 21 Nov 2012 - Rastko - set passenger's title to Mrs if passenger gender is female	-->
	<!-- Date: 19 Nov 2012 - Rastko - set EnableFakeCardVerification to false			-->
	<!-- Date: 06 Nov 2012 - Rastko - commented out select flight as sent by booking engine	-->
	<!-- Date: 22 May 2012 - Rastko - added credit card types							-->
	<!-- Date: 30 Mar 2012 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="system" select="OTA_TravelItineraryRQ/POS/TPA_Extensions/Provider/System"/>
	<xsl:template match="/">
		<TravelBuild>
			<xsl:apply-templates select="OTA_TravelItineraryRQ"/>
		</TravelBuild>
	</xsl:template>
	<xsl:template match="OTA_TravelItineraryRQ">
		<!--SelectFlights>
			<CommandList>
				<ProcessDetails>
					<XmlLoginId>***XMLLogin***</XmlLoginId>
						<LoginId>***LoginID***</LoginId>
					<RoutingId><xsl:value-of select="substring-after(substring-before(OTA_AirBookRQ/@TransactionIdentifier,'-'),'RoutingId:')"/></RoutingId>
					<OutwardId><xsl:value-of select="substring-before(substring-after(OTA_AirBookRQ/@TransactionIdentifier,'OutwardId:'),'-')"/></OutwardId>
					<ReturnId><xsl:value-of select="substring-after(OTA_AirBookRQ/@TransactionIdentifier,'ReturnId:')"/></ReturnId>
					<HandoffParametersOnly>false</HandoffParametersOnly>
				</ProcessDetails>
			</CommandList>
		</SelectFlights-->
		<BookingDetails>
			<CommandList>
				<ProcessTerms>
					<XmlLoginId>***XMLLogin***</XmlLoginId>
					<LoginId>***LoginID***</LoginId>
					<RoutingId>
						<xsl:value-of select="substring-after(substring-before(OTA_AirBookRQ/@TransactionIdentifier,'-'),'RoutingId:')"/>
					</RoutingId>
					<BookingProfile>
						<!--CustomSupplierParameterList>
							<CustomSupplierParameter>
								<Name>SpeedyBoarding</Name>
								<Value>y</Value>
							</CustomSupplierParameter>
						</CustomSupplierParameterList-->
						<TravellerList>
							<xsl:for-each select="TPA_Extensions/PNRData/Traveler">
								<Traveller>
									<Age>
										<xsl:choose>
											<xsl:when test="@PassengerTypeCode='INF'">0</xsl:when>
											<xsl:when test="@PassengerTypeCode='CHD'">7</xsl:when>
											<xsl:otherwise>30</xsl:otherwise>
										</xsl:choose>
									</Age>
									<Name>
										<Title>
											<xsl:choose>
												<xsl:when test="PersonName/NamePrefix='MISS' or PersonName/NamePrefix='Miss'">Miss</xsl:when>
												<xsl:when test="PersonName/NamePrefix='MRS' or PersonName/NamePrefix='Mrs'">Mrs</xsl:when>
												<xsl:when test="@Gender='Female'">Mrs</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="'Mr'"/>
												</xsl:otherwise>
											</xsl:choose>
										</Title>
										<NamePartList>
											<NamePart>
												<xsl:value-of select="PersonName/GivenName"/>
											</NamePart>
											<xsl:if test="PersonName/MiddleName != ''">
												<NamePart>
													<xsl:value-of select="PersonName/MiddleName"/>
												</NamePart>
											</xsl:if>
											<NamePart>
												<xsl:value-of select="PersonName/Surname"/>
											</NamePart>
										</NamePartList>
									</Name>
									<CustomSupplierParameterList>
										<CustomSupplierParameter>
											<Name>DateOfBirth</Name>
											<Value>
												<xsl:value-of select="concat(substring(@BirthDate,9),'/')"/>
												<xsl:value-of select="concat(substring(@BirthDate,6,2),'/')"/>
												<xsl:value-of select="substring(@BirthDate,1,4)"/>
											</Value>
										</CustomSupplierParameter>
										<!--CustomSupplierParameter>
											<Name>NumberOfBags</Name>
											<Value>1</Value>
										</CustomSupplierParameter-->
									</CustomSupplierParameterList>
								</Traveller>
							</xsl:for-each>
						</TravellerList>
						<ContactDetails>
							<Name>
								<Title>
									<xsl:choose>
										<xsl:when test="TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='MISS' or TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='Miss'">Miss</xsl:when>
										<xsl:when test="TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='MRS' or TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='Mrs'">Mrs</xsl:when>
										<xsl:when test="TPA_Extensions/PNRData/Traveler[1]/@Gender='Female'">Mrs</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'Mr'"/>
										</xsl:otherwise>
									</xsl:choose>
								</Title>
								<NamePartList>
									<NamePart>
										<xsl:value-of select="TPA_Extensions/PNRData/Traveler[1]/PersonName/GivenName"/>
									</NamePart>
									<xsl:if test="TPA_Extensions/PNRData/Traveler[1]/PersonName/MiddleName != ''">
										<NamePart>
											<xsl:value-of select="TPA_Extensions/PNRData/Traveler[1]/PersonName/MiddleName"/>
										</NamePart>
									</xsl:if>
									<NamePart>
										<xsl:value-of select="TPA_Extensions/PNRData/Traveler[1]/PersonName/Surname"/>
									</NamePart>
								</NamePartList>
							</Name>
							<Address>
								<Company/>
								<Flat/>
								<BuildingName/>
								<BuildingNumber/>
								<Street>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/StreetNmbr"/>
								</Street>
								<Locality/>
								<City>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/CityName"/>
								</City>
								<Province>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/StateProv/@StateCode"/>
								</Province>
								<Postcode>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/PostalCode"/>
								</Postcode>
								<CountryCode>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/CountryName/@Code"/>
								</CountryCode>
							</Address>
							<HomePhone>
								<InternationalCode>
									<xsl:value-of select="TPA_Extensions/PNRData/Telephone/@CountryAccessCode"/>
								</InternationalCode>
								<AreaCode>305</AreaCode>
								<Number>
									<xsl:value-of select="translate(TPA_Extensions/PNRData/Telephone/@PhoneNumber,'-','')"/>
								</Number>
								<Extension/>
							</HomePhone>
							<Email>
								<xsl:value-of select="TPA_Extensions/PNRData/Email"/>
							</Email>
						</ContactDetails>
						<BillingDetails>
							<Name>
								<Title>
									<xsl:choose>
										<xsl:when test="TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='MISS' or TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='Miss'">Miss</xsl:when>
										<xsl:when test="TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='MRS' or TPA_Extensions/PNRData/Traveler[1]/PersonName/NamePrefix='Mrs'">Mrs</xsl:when>
										<xsl:when test="TPA_Extensions/PNRData/Traveler[1]/@Gender='Female'">Mrs</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="'Mr'"/>
										</xsl:otherwise>
									</xsl:choose>
								</Title>
								<NamePartList>
									<NamePart>
										<xsl:value-of select="substring-before(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/CardHolderName,' ')"/>
									</NamePart>
									<NamePart>
										<xsl:value-of select="substring-after(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/CardHolderName,' ')"/>
									</NamePart>
								</NamePartList>
							</Name>
							<Address>
								<Company/>
								<Flat/>
								<BuildingName/>
								<BuildingNumber/>
								<Street>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/StreetNmbr"/>
								</Street>
								<Locality/>
								<City>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/CityName"/>
								</City>
								<Province>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/StateProv/@StateCode"/>
								</Province>
								<Postcode>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/PostalCode"/>
								</Postcode>
								<CountryCode>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/Address/CountryName/@Code"/>
								</CountryCode>
							</Address>
							<CreditCard>
								<Company/>
								<NameOnCard>
									<NamePartList>
										<NamePart>
											<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/CardHolderName"/>
										</NamePart>
									</NamePartList>
								</NameOnCard>
								<Number>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
								</Number>
								<SecurityCode>
									<xsl:value-of select="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@SeriesCode"/>
								</SecurityCode>
								<ExpiryDate>
									<xsl:value-of select="concat(substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,1,2),'/')"/>
									<xsl:value-of select="substring(OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate,3,2)"/>
								</ExpiryDate>
								<StartDate>04/12</StartDate>
								<CardType>
									<xsl:choose>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='VI'">Visa Credit</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MC'">MasterCard</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='AX'">American Express</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='VE'">Visa Electron</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='TP'">Air Plus</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='DN'">Diners Club</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MD'">MasterCard Debit</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MP'">MasterCard Prepaid</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='VT'">Visa Delta</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='VD'">Visa Debit</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='CN'">Connect</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='SO'">Solo</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='MO'">Maestro</xsl:when>
										<xsl:when test="OTA_AirBookRQ/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode='SW'">Switch</xsl:when>
									</xsl:choose>
								</CardType>
								<IssueNumber>0</IssueNumber>
							</CreditCard>
						</BillingDetails>
					</BookingProfile>
				</ProcessTerms>
			</CommandList>
		</BookingDetails>
		<CreateBooking>
			<CommandList>
				<StartBooking>
					<XmlLoginId>***XMLLogin***</XmlLoginId>
					<LoginId>***LoginID***</LoginId>
					<TFBookingReference>***TFBookingReference***</TFBookingReference>
					<ExpectedPrice>
						<Amount>
							<xsl:value-of select="substring(OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='T']/Text,4)"/>
						</Amount>
						<Currency>
							<xsl:value-of select="substring(OTA_AirBookRQ/TravelerInfo/SpecialReqDetails/SpecialRemarks/SpecialRemark[@RemarkType='T']/Text,1,3)"/>
						</Currency>
					</ExpectedPrice>
					<xsl:if test="$system='Test'">
						<FakeBooking>
							<EnableFakeBooking>true</EnableFakeBooking>
							<FakeBookingSimulatedDelaySeconds>15</FakeBookingSimulatedDelaySeconds>
							<FakeBookingStatus>Succeeded</FakeBookingStatus>
							<EnableFakeCardVerification>false</EnableFakeCardVerification>
						</FakeBooking>
					</xsl:if>
				</StartBooking>
			</CommandList>
		</CreateBooking>
		<GetBooking>
			<CommandList>
				<CheckBooking>
					<XmlLoginId>***XMLLogin***</XmlLoginId>
					<LoginId>***LoginID***</LoginId>
					<TFBookingReference>***TFBookingReference***</TFBookingReference>
				</CheckBooking>
			</CommandList>
		</GetBooking>
		<RetrieveBooking>
			<CommandList>
				<GetBookingDetails>
					<XmlLoginId>***XMLLogin***</XmlLoginId>
					<LoginId>***LoginID***</LoginId>
					<TFBookingReference>***TFBookingReference***</TFBookingReference>
				</GetBookingDetails>
			</CommandList>
		</RetrieveBooking>
	</xsl:template>
</xsl:stylesheet>
