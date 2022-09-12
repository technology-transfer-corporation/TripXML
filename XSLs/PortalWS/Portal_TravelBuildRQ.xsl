<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Portal_TravelBuildRQ.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 23 Mar 2012 - Rastko - corrected tax and total calculation when multple stored fares	-->
	<!-- Date: 05 Nov 2011 - Rastko - support comma in markup value					-->
	<!-- Date: 23 Aug 2011 - Rastko - added mapping of SplitPNRNumber element			-->
	<!-- Date: 24 Jul 2011 - Rastko - corrected phone mapping	and pricing without pax type		-->
	<!-- Date: 24 Jun 2011 - Rastko - corrected arrival city mapping					-->
	<!-- Date: 13 Jun 2011 - Rastko - changed to use only PNR response				-->
	<!-- Date: 27 Jul 2009 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<Portal>
			<xsl:apply-templates select="Portal/OTA_TravelItineraryRS/TravelItinerary"/>
		</Portal>
	</xsl:template>
	<!-- ======================================================================= -->
	<xsl:template match="TravelItinerary">
		<xsl:for-each select="CustomerInfos/CustomerInfo">
			<SA>
				<SaveAccount>
					<customerAccount>
						<xsl:choose>
							<xsl:when test="Customer/PersonName/@NameType='CHD'">
								<isChild>true</isChild>
								<isYouth>false</isYouth>
								<isAdult>false</isAdult>
								<isSenior>false</isSenior>
								<isInfant>false</isInfant>
								<isSeatedInfant>false</isSeatedInfant>
							</xsl:when>
							<xsl:when test="Customer/PersonName/@NameType='YTH'">
								<isChild>false</isChild>
								<isYouth>true</isYouth>
								<isAdult>false</isAdult>
								<isSenior>false</isSenior>
								<isInfant>false</isInfant>
								<isSeatedInfant>false</isSeatedInfant>
							</xsl:when>
							<xsl:when test="Customer/PersonName/@NameType='SRC'">
								<isChild>false</isChild>
								<isYouth>false</isYouth>
								<isAdult>false</isAdult>
								<isSenior>true</isSenior>
								<isInfant>false</isInfant>
								<isSeatedInfant>false</isSeatedInfant>
							</xsl:when>
							<xsl:when test="Customer/PersonName/@NameType='INS'">
								<isChild>false</isChild>
								<isYouth>false</isYouth>
								<isAdult>false</isAdult>
								<isSenior>false</isSenior>
								<isInfant>false</isInfant>
								<isSeatedInfant>true</isSeatedInfant>
							</xsl:when>
							<xsl:when test="Customer/PersonName/@NameType='INF'">
								<isChild>false</isChild>
								<isYouth>false</isYouth>
								<isAdult>false</isAdult>
								<isSenior>false</isSenior>
								<isInfant>true</isInfant>
								<isSeatedInfant>false</isSeatedInfant>
							</xsl:when>
							<xsl:otherwise>
								<isChild>false</isChild>
								<isYouth>false</isYouth>
								<isAdult>true</isAdult>
								<isSenior>false</isSenior>
								<isInfant>false</isInfant>
								<isSeatedInfant>false</isSeatedInfant>
							</xsl:otherwise>
						</xsl:choose>
						<ID>0</ID>
						<ParentID>0</ParentID>
						<SiteID>0</SiteID>
						<Title>
							<xsl:value-of select="Customer/PersonName/NamePrefix"/>
						</Title>
						<FirstName>
							<xsl:value-of select="Customer/PersonName/GivenName"/>
						</FirstName>
						<MiddleName>
							<xsl:value-of select="Customer/PersonName/MiddleName"/>
						</MiddleName>
						<LastName>
							<xsl:value-of select="Customer/PersonName/Surname"/>
						</LastName>
						<Sex>
							<xsl:choose>
								<xsl:when test="contains(Customer/PersonName/GivenName,' MRS')">Female</xsl:when>
								<xsl:when test="contains(Customer/PersonName/GivenName,' MISS')">Female</xsl:when>
								<xsl:otherwise>Male</xsl:otherwise>
							</xsl:choose>
						</Sex>
						<DateOfBirth>
							<xsl:choose>
								<xsl:when test="Customer/@BirthDate!=''">
									<xsl:value-of select="Customer/@BirthDate"/>
								</xsl:when>
								<xsl:otherwise>0001-01-01T00:00:00.0000000-05:00</xsl:otherwise>
							</xsl:choose>
						</DateOfBirth>
						<Age>
							<xsl:choose>
								<xsl:when test="Customer/Age!=''">
									<xsl:value-of select="Customer/Age"/>
								</xsl:when>
								<xsl:otherwise>0</xsl:otherwise>
							</xsl:choose>
						</Age>
						<DocumentIssueDate>0001-01-01T00:00:00.0000000-05:00</DocumentIssueDate>
						<DocumentExpirationDate>0001-01-01T00:00:00.0000000-05:00</DocumentExpirationDate>
						<eMail>
							<xsl:value-of select="Customer/Email"/>
						</eMail>
						<PhoneOne>
							<xsl:value-of select="Customer/Telephone[@PhoneUseType='H'][1]/@PhoneNumber"/>
						</PhoneOne>
						<PhoneTwo>
							<xsl:value-of select="Customer/Telephone[@PhoneUseType='H'][position()=2]/@PhoneNumber"/>
						</PhoneTwo>
						<ReceiveNotification>false</ReceiveNotification>
						<LastLoginDate>0001-01-01T00:00:00.0000000-05:00</LastLoginDate>
						<Active>true</Active>
						<GuestRPH>0</GuestRPH>
						<EmergencyFlag>false</EmergencyFlag>
						<DiningTableSize>0</DiningTableSize>
						<InsertID>0</InsertID>
						<InsertDate>0001-01-01T00:00:00.0000000-05:00</InsertDate>
						<ModifiedID>0</ModifiedID>
						<ModifiedDate>0001-01-01T00:00:00.0000000-05:00</ModifiedDate>
					</customerAccount>
				</SaveAccount>
			</SA>
		</xsl:for-each>
		<xsl:if test="TravelCost/FormOfPayment/PaymentCard/@CardCode!=''">
			<SP>
				<SaveCustomerPaymentInfo>
					<pmtInfo>
						<xsl:variable name="CC" select="TravelCost/FormOfPayment/PaymentCard"/>
						<ID>0</ID>
						<Amount>0</Amount>
						<Active>false</Active>
						<CustomerID>715</CustomerID>
						<CreditCard>
							<ID><xsl:value-of select="$CC/@CardCode"/></ID>
							<PseudoCode><xsl:value-of select="$CC/@CardCode"/></PseudoCode>
							<CardTypeName>
								<xsl:choose>
									<xsl:when test="$CC/@CardCode='VI'">Visa</xsl:when>
									<xsl:when test="$CC/@CardCode='AX'">American Express</xsl:when>
									<xsl:when test="$CC/@CardCode='DN'">Diners</xsl:when>
									<xsl:when test="$CC/@CardCode='MC'">Master Card</xsl:when>
									<xsl:otherwise><xsl:value-of select="$CC/@CardCode"/></xsl:otherwise>
								</xsl:choose>
							</CardTypeName>	
							<xsl:variable name="rmk" select="ItineraryInfo/SpecialRequestDetails/Remarks"/>
							<VerificationCode>
								<xsl:value-of select="substring-after($rmk/Remark[contains(.,'*PR*** CC Verification Code:')],'Code: ')"/>
							</VerificationCode>
							<CardNumber><xsl:value-of select="$CC/@CardNumber"/></CardNumber>
							<EffectiveDate>0001-01-01T00:00:00.0000000-05:00</EffectiveDate>
							<ExpiryDate>
								<xsl:text>20</xsl:text>
								<xsl:value-of select="substring($CC/@ExpireDate,3)"/>
								<xsl:text>-</xsl:text>
								<xsl:value-of select="substring($CC/@ExpireDate,1,2)"/>
								<xsl:text>-28T00:00:00</xsl:text>
							</ExpiryDate>
							<CardHolder>
								<xsl:value-of select="substring-after($rmk/Remark[contains(.,'*PR*** Billing Name:')],'Name: ')"/>
							</CardHolder>
							<CardHolderPhone>
								<xsl:value-of select="substring-after($rmk/Remark[contains(.,'*PR*** Billing Phone:')],'Phone: ')"/>
							</CardHolderPhone>
							<ProcessingFeeAmount>0</ProcessingFeeAmount>
							<BankPhone>
								<xsl:value-of select="substring-after($rmk/Remark[contains(.,'*PR*** CC Bank Phone:')],'Phone: ')"/>
							</BankPhone>
							<BankName>
								<xsl:value-of select="substring-after($rmk/Remark[contains(.,'*PR*** CC Bank Name:')],'Name: ')"/>
							</BankName>
						</CreditCard>
						<InsUser>
							<ID>1</ID>
						</InsUser>
						<ModUser>
							<ID>1</ID>
						</ModUser>
						<InsDateTime>0001-01-01T00:00:00.0000000-05:00</InsDateTime>
						<ModDateTime>0001-01-01T00:00:00.0000000-05:00</ModDateTime>
						<BillingAddress>
							<CustomerAddressID>0</CustomerAddressID>
							<AddressLineOne><xsl:value-of select="$CC/Address/StreetNmbr"/></AddressLineOne>
							<AddressLineTwo/>
							<City>
								<Name><xsl:value-of select="$CC/Address/CityName"/></Name>
								<StateID><xsl:value-of select="$CC/Address/StateProv/@StateCode"/></StateID>
								<CountryID><xsl:value-of select="$CC/Address/CountryName/@Code"/></CountryID>
								<IsAirPort>false</IsAirPort>
								<IsSeaPort>false</IsSeaPort>
								<LanguageId>0</LanguageId>
								<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
								<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
							</City>
							<State>
								<ID><xsl:value-of select="$CC/Address/StateProv/@StateCode"/></ID>
								<CountryID><xsl:value-of select="$CC/Address/CountryName/@Code"/></CountryID>
							</State>
							<Country>
								<ID><xsl:value-of select="$CC/Address/CountryName/@Code"/></ID>
								<LanguageID>0</LanguageID>
							</Country>
							<ZipCode><xsl:value-of select="$CC/Address/PostalCode"/></ZipCode>
						</BillingAddress>
					</pmtInfo>
				</SaveCustomerPaymentInfo>
			</SP>
		</xsl:if>
		<CB>
			<!--CreateBookingInDB xmlns="http://odyssey.com/webservices/"-->
			<CreateBookingInDB>
				<booking>
					<TransactionID>0</TransactionID>
					<xsl:variable name="Omarkup2">
						<xsl:choose>
							<xsl:when test="ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'TOTAL MARKUP')]">
								<xsl:apply-templates select="ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'TOTAL MARKUP')][1]" mode="owner">
									<xsl:with-param name="tot">0</xsl:with-param>
								</xsl:apply-templates>
							</xsl:when>
							<xsl:when test="ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'TOTAL AMOUNT IN GDS CURRENCY')]">
								<xsl:variable name="morqua1">
									<xsl:value-of select="substring-before(substring-after(ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'TOTAL AMOUNT IN GDS CURRENCY')],'CURRENCY '),' USD')"/>
								</xsl:variable>
								<xsl:variable name="morqua">
									<xsl:choose>
										<xsl:when test="not(contains($morqua1,'.'))">
											<xsl:value-of select="concat($morqua1,'00')"/>
										</xsl:when>
										<xsl:when test="string-length(substring-after($morqua1,'.'))=1">
											<xsl:value-of select="concat(translate($morqua1,'.',''),'0')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate($morqua1,'.','')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:variable name="total">
									<xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/ItinTotalFare/TotalFare/@Amount"/>
								</xsl:variable>
								<xsl:variable name="dec"><xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:variable>
								<xsl:variable name="totnip">
									<xsl:value-of select="sum(ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown/PassengerTypeQuantity/@Quantity)"/>
								</xsl:variable>
								<xsl:variable name="mkp">
									<xsl:choose>
										<xsl:when test="$dec= 1"><xsl:value-of select="($morqua - $total) div 10"/></xsl:when>
										<xsl:when test="$dec= 2"><xsl:value-of select="($morqua - $total) div 100"/></xsl:when>
										<xsl:when test="$dec= 3"><xsl:value-of select="($morqua - $total) div 1000"/></xsl:when>
										<xsl:otherwise><xsl:value-of select="$morqua - $total"/></xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:value-of select="format-number($mkp div $totnip,'#.00')"/>
							</xsl:when>
							<xsl:when test="ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'TOTAL PASSENGER TICKETS')]">
								<xsl:variable name="jetcombo1">
									<xsl:value-of select="translate(substring-after(ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'TOTAL PASSENGER TICKETS')],'TICKETS USD'),' ','')"/>
								</xsl:variable>
								<xsl:variable name="jetcombo">
									<xsl:choose>
										<xsl:when test="not(contains($jetcombo1,'.'))">
											<xsl:value-of select="concat($jetcombo1,'00')"/>
										</xsl:when>
										<xsl:when test="string-length(substring-after($jetcombo1,'.'))=1">
											<xsl:value-of select="concat(translate($jetcombo1,'.',''),'0')"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="translate($jetcombo1,'.','')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:variable name="total">
									<xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/ItinTotalFare/TotalFare/@Amount"/>
								</xsl:variable>
								<xsl:variable name="dec"><xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:variable>
								<xsl:variable name="totnip">
									<xsl:value-of select="sum(ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown/PassengerTypeQuantity/@Quantity)"/>
								</xsl:variable>
								<xsl:variable name="mkp">
									<xsl:choose>
										<xsl:when test="$dec= 1"><xsl:value-of select="($jetcombo - $total) div 10"/></xsl:when>
										<xsl:when test="$dec= 2"><xsl:value-of select="($jetcombo - $total) div 100"/></xsl:when>
										<xsl:when test="$dec= 3"><xsl:value-of select="($jetcombo - $total) div 1000"/></xsl:when>
										<xsl:otherwise><xsl:value-of select="$jetcombo - $total"/></xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:value-of select="format-number($mkp div $totnip,'#.00')"/>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="Amarkup2">
						<xsl:choose>
							<xsl:when test="ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'Affiliate markup')]">
								<xsl:apply-templates select="ItineraryInfo/SpecialRequestDetails/Remarks/Remark[contains(.,'Affiliate markup')][1]" mode="affiliate">
									<xsl:with-param name="tot">0</xsl:with-param>
								</xsl:apply-templates>
							</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="pricesmain" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo"/>
					<xsl:variable name="decmain"><xsl:value-of select="$pricesmain/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:variable>
					<xsl:variable name="Omarkup">
						<xsl:choose>
							<xsl:when test="$Omarkup2='0' or $Omarkup2=''">0</xsl:when>
							<xsl:when test="$decmain = 1"><xsl:value-of select="format-number($Omarkup2 * 10,'0')"/></xsl:when>
							<xsl:when test="$decmain = 2"><xsl:value-of select="format-number($Omarkup2 * 100,'0')"/></xsl:when>
							<xsl:when test="$decmain = 3"><xsl:value-of select="format-number($Omarkup2 * 1000,'0')"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="$Omarkup2"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="Amarkup">
						<xsl:choose>
							<xsl:when test="$Amarkup2='0' or $Amarkup2=''">0</xsl:when>
							<xsl:when test="$decmain = 1"><xsl:value-of select="format-number($Amarkup2 * 10,'0')"/></xsl:when>
							<xsl:when test="$decmain = 2"><xsl:value-of select="format-number($Amarkup2 * 100,'0')"/></xsl:when>
							<xsl:when test="$decmain = 3"><xsl:value-of select="format-number($Amarkup2 * 1000,'0')"/></xsl:when>
							<xsl:otherwise><xsl:value-of select="$Amarkup2"/></xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="totnip">
						<xsl:value-of select="sum(ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown/PassengerTypeQuantity/@Quantity)"/>
					</xsl:variable>
					<xsl:variable name="markups"><xsl:value-of select="($Omarkup + $Amarkup) * $totnip"/></xsl:variable>
					<Customer>
						<isChild>false</isChild>
						<isYouth>false</isYouth>
						<isAdult>false</isAdult>
						<isSenior>false</isSenior>
						<isInfant>false</isInfant>
						<isSeatedInfant>false</isSeatedInfant>
						<ID>customerid</ID>
						<ParentID>0</ParentID>
						<SiteID>0</SiteID>
						<Title>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/PersonName/NamePrefix"/>
						</Title>
						<FirstName>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/PersonName/GivenName"/>
						</FirstName>
						<MiddleName>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/PersonName/MiddleName"/>
						</MiddleName>
						<LastName>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/PersonName/Surname"/>
						</LastName>
						<Sex>
							<xsl:choose>
								<xsl:when test="contains(CustomerInfos/CustomerInfo[1]/Customer/PersonName/GivenName,' MRS')">Female</xsl:when>
								<xsl:when test="contains(CustomerInfos/CustomerInfo[1]/Customer/PersonName/GivenName,' MISS')">Female</xsl:when>
								<xsl:otherwise>Male</xsl:otherwise>
							</xsl:choose>
						</Sex>
						<DateOfBirth>0001-01-01T00:00:00.0000000-05:00</DateOfBirth>
						<Age>0</Age>
						<DocumentIssueDate>0001-01-01T00:00:00.0000000-05:00</DocumentIssueDate>
						<DocumentExpirationDate>0001-01-01T00:00:00.0000000-05:00</DocumentExpirationDate>
						<eMail>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/Email"/>
						</eMail>
						<PhoneOne>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/Telephone[@PhoneUseType='H'][1]/@PhoneNumber"/>
						</PhoneOne>
						<PhoneTwo>
							<xsl:value-of select="CustomerInfos/CustomerInfo[1]/Customer/Telephone[@PhoneUseType='H'][position()=2]/@PhoneNumber"/>
						</PhoneTwo>
						<ReceiveNotification>false</ReceiveNotification>
						<LastLoginDate>0001-01-01T00:00:00.0000000-05:00</LastLoginDate>
						<Active>true</Active>
						<GuestRPH>0</GuestRPH>
						<EmergencyFlag>false</EmergencyFlag>
						<DiningTableSize>0</DiningTableSize>
						<InsertID>0</InsertID>
						<InsertDate>0001-01-01T00:00:00</InsertDate>
						<ModifiedID>0</ModifiedID>
						<ModifiedDate>0001-01-01T00:00:00</ModifiedDate>
					</Customer>
					<PNR><xsl:value-of select="ItineraryRef/@ID"/></PNR>
					<SelectedPaymentSchedules>
						<PaymentSchedule>
							<PaymentInfo>None</PaymentInfo>
							<PaymentNumber>0</PaymentNumber>
							<Amount>
								<xsl:variable name="prices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo"/>
								<xsl:variable name="dec"><xsl:value-of select="$prices/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:variable>
								<xsl:variable name="amt2"><xsl:value-of select="$prices/ItinTotalFare/TotalFare/@Amount"/></xsl:variable>
								<xsl:variable name="amt1"><xsl:value-of select="$amt2 + $markups"/></xsl:variable>
								<xsl:choose>
									<xsl:when test="$dec!='0'">
										<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
										<xsl:text>.</xsl:text>
										<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
									</xsl:when>
									<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
								</xsl:choose>
							</Amount>
							<Description>Full Payment</Description>
							<xsl:choose>
								<xsl:when test="ItineraryInfo/Ticketing/@TicketTimeLimit!=''">
									<AgencyDate>
										<xsl:value-of select="ItineraryInfo/Ticketing/@TicketTimeLimit"/>
									</AgencyDate>
									<PaidDate>0001-01-01T00:00:00.0000000-05:00</PaidDate>
									<SupplierDate>
										<xsl:value-of select="ItineraryInfo/Ticketing/@TicketTimeLimit"/>
									</SupplierDate>
								</xsl:when>
								<xsl:otherwise>
									<xsl:variable name="ttl">
										<xsl:text>20</xsl:text>
										<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,8,2)"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,6,2)"/>
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,4,2)"/>
										<xsl:text>T00:00:00</xsl:text>
									</xsl:variable>
									<AgencyDate>
										<xsl:value-of select="$ttl"/>
									</AgencyDate>
									<PaidDate>0001-01-01T00:00:00.0000000-05:00</PaidDate>
									<SupplierDate>
										<xsl:value-of select="$ttl"/>
									</SupplierDate>
								</xsl:otherwise>
							</xsl:choose>
							<SupplierID>0</SupplierID>
							<PaymentType>Air</PaymentType>
						</PaymentSchedule>
					</SelectedPaymentSchedules>
					<Status>
						<ID>6</ID>
						<Name>Held</Name>
					</Status>
					<Package>
						<MinInsidePrice2>0</MinInsidePrice2>
						<MinOutsidePrice2>0</MinOutsidePrice2>
						<MinBalconyPrice2>0</MinBalconyPrice2>
						<MinSuitePrice2>0</MinSuitePrice2>
						<SiteItemId>0</SiteItemId>
						<PackageID>0</PackageID>
						<SitePackageID>0</SitePackageID>
						<DepartureCity>
							<ID>
								<xsl:value-of select="ItineraryInfo/ReservationItems/Item[Air][1]/Air/DepartureAirport/@LocationCode"/>
							</ID>
							<Name>
								<xsl:value-of select="ItineraryInfo/ReservationItems/Item[Air][1]/Air/DepartureAirport"/>
							</Name>
							<IsAirPort>false</IsAirPort>
							<IsSeaPort>false</IsSeaPort>
							<LanguageId>0</LanguageId>
							<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
							<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
						</DepartureCity>
						<DepartureDateTime>
							<xsl:value-of select="ItineraryInfo/ReservationItems/Item[Air][1]/Air/@DepartureDateTime"/>
						</DepartureDateTime>
						<ArrivalCity>
							<ID>
								<xsl:value-of select="ItineraryInfo/ReservationItems/Item[Air][position()=last()]/Air/ArrivalAirport/@LocationCode"/>
							</ID>
							<Name>
								<xsl:value-of select="ItineraryInfo/ReservationItems/Item[Air][position()=last()]/Air/ArrivalAirport"/>
							</Name>
							<IsAirPort>false</IsAirPort>
							<IsSeaPort>false</IsSeaPort>
							<LanguageId>0</LanguageId>
							<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
							<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
						</ArrivalCity>
						<ArrivalDateTime>
							<xsl:value-of select="ItineraryInfo/ReservationItems/Item[Air][position()=last()]/Air/@ArrivalDateTime"/>
						</ArrivalDateTime>
						<Duration>0</Duration>
						<Currency>
							<ID>
								<xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
							</ID>
						</Currency>
						<FromCurrencyId>
							<xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/ItinTotalFare/BaseFare/@CurrencyCode"/>
						</FromCurrencyId>
						<ExchangeRate>1</ExchangeRate>
						<ExchageRateDateTime>2009-01-30T20:00:00.0000000-04:00</ExchageRateDateTime>
						<Price>0</Price>
						<BasePrice>0</BasePrice>
						<PortCharges>0</PortCharges>
						<TaxesAndFees>0</TaxesAndFees>
						<Taxes/>
						<AirAddOn>0</AirAddOn>
						<AddAdultFare>0</AddAdultFare>
						<AddChildFare>0</AddChildFare>
						<TotalCount>0</TotalCount>
						<AvailableCount>0</AvailableCount>
						<BkgStatus>
							<ID>6</ID>
							<Name>Held</Name>
						</BkgStatus>
						<Active>true</Active>
						<InsertDate>0001-01-01T00:00:00</InsertDate>
						<ModifiedDate>0001-01-01T00:00:00</ModifiedDate>
						<Air>
							<siteCommissions/>
							<internationalFlight>false</internationalFlight>
							<DiscountApplied>0</DiscountApplied>
							<PricingSource>
								<xsl:value-of select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/@PricingSource"/>
							</PricingSource>
							<ValidatingCarrier>
								<xsl:value-of select="ItineraryInfo/ReservationItems/Item[1]/Air/MarketingAirline/@Code"/>
							</ValidatingCarrier>
							<AirJourneyType>
								<ID>
									<xsl:choose>
										<xsl:when test="ItineraryInfo/ReservationItems/Item[Air][1]/DepartureAirport/@LocationCode = ItineraryInfo/ReservationItems/Item[Air][position()=last()]/ArrivalAirport/@LocationCode">41</xsl:when>
										<xsl:otherwise>40</xsl:otherwise>
									</xsl:choose>
								</ID>
								<SupplierID>0</SupplierID>
								<LanguageID>0</LanguageID>
								<xsl:variable name="direct">
									<xsl:choose>
										<xsl:when test="ItineraryInfo/ReservationItems/Item[Air][1]/DepartureAirport/@LocationCode = ItineraryInfo/ReservationItems/Item[Air][position()=last()]/ArrivalAirport/@LocationCode">Circle</xsl:when>
										<xsl:otherwise>One Way</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<PseudoCode>
									<xsl:value-of select="$direct"/>
								</PseudoCode>
								<Name>
									<xsl:choose>
										<xsl:when test="$direct='Circle'">Round Trip</xsl:when>
										<xsl:otherwise>One Way</xsl:otherwise>
									</xsl:choose>
								</Name>
								<Active>false</Active>
							</AirJourneyType>
							<Ticket>
								<TicketType>
									<xsl:value-of select="ItineraryInfo//Ticketing/@TicketType"/>
								</TicketType>
								<IsRefundable>false</IsRefundable>
								<xsl:choose>
									<xsl:when test="ItineraryInfo/Ticketing/@TicketTimeLimit!=''">
										<TicketTimeLimit>
											<xsl:value-of select="ItineraryInfo/Ticketing/@TicketTimeLimit"/>
										</TicketTimeLimit>
										<AgencyDate>
											<xsl:value-of select="ItineraryInfo/Ticketing/@TicketTimeLimit"/>
										</AgencyDate>
									</xsl:when>
									<xsl:otherwise>
										<xsl:variable name="ttl">
											<xsl:text>20</xsl:text>
											<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,8,2)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,6,2)"/>
											<xsl:text>-</xsl:text>
											<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,4,2)"/>
											<xsl:text>T00:00:00</xsl:text>
										</xsl:variable>
										<TicketTimeLimit>
											<xsl:value-of select="$ttl"/>
										</TicketTimeLimit>
										<AgencyDate>
											<xsl:value-of select="$ttl"/>
										</AgencyDate>
									</xsl:otherwise>
								</xsl:choose>
								<AutoTicket>false</AutoTicket>
							</Ticket>
							<PaperTicketRequested>
								<xsl:choose>
									<xsl:when test="ItineraryInfo/Ticketing/@TicketType='eTicket'">false</xsl:when>
									<xsl:otherwise>true</xsl:otherwise>
								</xsl:choose>
							</PaperTicketRequested>
							<SequenceNumber>9</SequenceNumber>
							<Provider>
								<xsl:text>Amadeus-</xsl:text>
								<xsl:value-of select="ItineraryRef/@ID_Context"/>
							</Provider>
							<xsl:variable name="prices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo"/>
							<xsl:variable name="dec"><xsl:value-of select="$prices/ItinTotalFare/BaseFare/@DecimalPlaces"/></xsl:variable>
							<AirPrices>
								<Price>
									<ID>7</ID>
									<Name>Air Fare</Name>
									<xsl:variable name="amt2"><xsl:value-of select="$prices/ItinTotalFare/BaseFare/@Amount"/></xsl:variable>
									<xsl:variable name="amt1"><xsl:value-of select="$amt2 + $markups"/></xsl:variable>
									<Amount>
										<xsl:choose>
											<xsl:when test="$dec!='0'">
												<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
											</xsl:when>
											<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
										</xsl:choose>
									</Amount>
									<AmountYield>0</AmountYield>
									<Priority>0</Priority>
									<RestrictedIndicator>true</RestrictedIndicator>
									<IncludesPortCharges>false</IncludesPortCharges>
									<IsCommission>false</IsCommission>
								</Price>
								<Price>
									<ID>23</ID>
									<Name>Total Due</Name>
									<xsl:variable name="amt2"><xsl:value-of select="$prices/ItinTotalFare/TotalFare/@Amount"/></xsl:variable>
									<xsl:variable name="amt1"><xsl:value-of select="$amt2 + $markups"/></xsl:variable>
									<Amount>
										<xsl:choose>
											<xsl:when test="$dec!='0'">
												<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
											</xsl:when>
											<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
										</xsl:choose>
									</Amount>
									<AmountYield>0</AmountYield>
									<Priority>0</Priority>
									<RestrictedIndicator>true</RestrictedIndicator>
									<IncludesPortCharges>false</IncludesPortCharges>
									<IsCommission>false</IsCommission>
								</Price>
								<Price>
									<ID>48</ID>
									<Name>Markup</Name>
									<Amount>
										<xsl:variable name="Omarkup1"><xsl:value-of select="$Omarkup * $totnip"/></xsl:variable>
										<xsl:choose>
											<xsl:when test="$Omarkup1='0'">0</xsl:when>
											<xsl:when test="contains($Omarkup1,'.')">
												<xsl:value-of select="format-number($Omarkup1,'0.00')"/>
											</xsl:when>
											<xsl:when test="$dec!='0'">
												<xsl:value-of select="substring($Omarkup1,1,string-length($Omarkup1) - $dec)"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="substring($Omarkup1, string-length($Omarkup1) - ($dec -1))"/>
											</xsl:when>
											<xsl:otherwise><xsl:value-of select="$Omarkup1"/></xsl:otherwise>
										</xsl:choose>
									</Amount>
									<AmountYield>0</AmountYield>
									<Priority>0</Priority>
									<RestrictedIndicator>false</RestrictedIndicator>
									<IncludesPortCharges>false</IncludesPortCharges>
									<IsCommission>false</IsCommission>
								</Price>
								<Price>
									<ID>49</ID>
									<Name>Agency/Affiliate Custom Markup</Name>
									<Amount>
										<xsl:variable name="Amarkup1"><xsl:value-of select="$Amarkup * $totnip"/></xsl:variable>
										<xsl:choose>
											<xsl:when test="$Amarkup1='0'">0</xsl:when>
											<xsl:when test="contains($Amarkup1,'.')">
												<xsl:value-of select="format-number($Amarkup1,'0.00')"/>
											</xsl:when>
											<xsl:when test="$dec!='0'">
												<xsl:value-of select="substring($Amarkup1,1,string-length($Amarkup1) - $dec)"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="substring($Amarkup1, string-length($Amarkup1) - ($dec -1))"/>
											</xsl:when>
											<xsl:otherwise><xsl:value-of select="$Amarkup1"/></xsl:otherwise>
										</xsl:choose>
									</Amount>
									<AmountYield>0</AmountYield>
									<Priority>0</Priority>
									<RestrictedIndicator>false</RestrictedIndicator>
									<IncludesPortCharges>false</IncludesPortCharges>
									<IsCommission>false</IsCommission>
								</Price>
								<Price>
									<ID>8</ID>
									<Name>Air Tax</Name>
									<PseudoCode>TotalTax</PseudoCode>
									<Amount>
										<xsl:choose>
											<xsl:when test="$dec!='0'">
												<xsl:value-of select="substring($prices/ItinTotalFare/Taxes/@Amount,1,string-length($prices/ItinTotalFare/Taxes/@Amount) - $dec)"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="substring($prices/ItinTotalFare/Taxes/@Amount, string-length($prices/ItinTotalFare/Taxes/@Amount) - ($dec -1))"/>
											</xsl:when>
											<xsl:otherwise><xsl:value-of select="$prices/ItinTotalFare/Taxes/@Amount"/></xsl:otherwise>
										</xsl:choose>
									</Amount>
									<AmountYield>0</AmountYield>
									<Priority>0</Priority>
									<RestrictedIndicator>true</RestrictedIndicator>
									<IncludesPortCharges>false</IncludesPortCharges>
									<IsCommission>false</IsCommission>
								</Price>
							</AirPrices>
							<xsl:variable name="ADTprices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='ADT']"/>
							<xsl:if test="$ADTprices!=''">
								<xsl:variable name="nip"><xsl:value-of select="sum($ADTprices/PassengerTypeQuantity/@Quantity)"/></xsl:variable>
								<AdultPrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0'">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt"><xsl:value-of select="sum($ADTprices/PassengerFare/TotalFare/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($ADTprices/PassengerFare/Taxes/Tax/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:for-each select="$ADTprices/PassengerFare/Taxes/Tax">
										<xsl:variable name="nip1"><xsl:value-of select="../../../PassengerTypeQuantity/@Quantity"/></xsl:variable>
										<Price>
											<ID>8</ID>
											<Name>Air Tax</Name>
											<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
											<Amount>
												<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
												<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip1"/></xsl:variable>
												<xsl:choose>
													<xsl:when test="$dec!='0'">
														<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
													</xsl:when>
													<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
												</xsl:choose>
											</Amount>
											<AmountYield>0</AmountYield>
											<Priority>0</Priority>
											<RestrictedIndicator>false</RestrictedIndicator>
											<IncludesPortCharges>false</IncludesPortCharges>
											<IsCommission>false</IsCommission>
										</Price>
									</xsl:for-each>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($ADTprices/PassengerFare/BaseFare/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</AdultPrices>
							</xsl:if>
							<xsl:variable name="CHDprices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='CHD']"/>
							<xsl:if test="$CHDprices!=''">
								<xsl:variable name="nip"><xsl:value-of select="$CHDprices/PassengerTypeQuantity/@Quantity"/></xsl:variable>
								<ChildPrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0'">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt"><xsl:value-of select="$CHDprices/PassengerFare/TotalFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($CHDprices/PassengerFare/Taxes/Tax/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:for-each select="$CHDprices/PassengerFare/Taxes/Tax">
										<Price>
											<ID>8</ID>
											<Name>Air Tax</Name>
											<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
											<Amount>
												<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
												<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
												<xsl:choose>
													<xsl:when test="$dec!='0'">
														<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
													</xsl:when>
													<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
												</xsl:choose>
											</Amount>
											<AmountYield>0</AmountYield>
											<Priority>0</Priority>
											<RestrictedIndicator>false</RestrictedIndicator>
											<IncludesPortCharges>false</IncludesPortCharges>
											<IsCommission>false</IsCommission>
										</Price>
									</xsl:for-each>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="$CHDprices/PassengerFare/BaseFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</ChildPrices>
							</xsl:if>
							<xsl:variable name="YTHprices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='YTH']"/>
							<xsl:if test="$YTHprices!=''">
								<xsl:variable name="nip"><xsl:value-of select="$YTHprices/PassengerTypeQuantity/@Quantity"/></xsl:variable>
								<YouthPrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0'">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt"><xsl:value-of select="$YTHprices/PassengerFare/TotalFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($YTHprices/PassengerFare/Taxes/Tax/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:for-each select="$YTHprices/PassengerFare/Taxes/Tax">
										<Price>
											<ID>8</ID>
											<Name>Air Tax</Name>
											<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
											<Amount>
												<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
												<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
												<xsl:choose>
													<xsl:when test="$dec!='0'">
														<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
													</xsl:when>
													<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
												</xsl:choose>
											</Amount>
											<AmountYield>0</AmountYield>
											<Priority>0</Priority>
											<RestrictedIndicator>false</RestrictedIndicator>
											<IncludesPortCharges>false</IncludesPortCharges>
											<IsCommission>false</IsCommission>
										</Price>
									</xsl:for-each>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="$YTHprices/PassengerFare/BaseFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</YouthPrices>
							</xsl:if>
							<xsl:variable name="INFprices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='INF']"/>
							<xsl:if test="$INFprices!=''">
								<xsl:variable name="nip"><xsl:value-of select="$INFprices/PassengerTypeQuantity/@Quantity"/></xsl:variable>
								<InfantPrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0'">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt"><xsl:value-of select="$INFprices/PassengerFare/TotalFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($INFprices/PassengerFare/Taxes/Tax/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$amt='0'">0.00</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:for-each select="$INFprices/PassengerFare/Taxes/Tax">
										<Price>
											<ID>8</ID>
											<Name>Air Tax</Name>
											<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
											<Amount>
												<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
												<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
												<xsl:choose>
													<xsl:when test="$dec!='0'">
														<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
													</xsl:when>
													<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
												</xsl:choose>
											</Amount>
											<AmountYield>0</AmountYield>
											<Priority>0</Priority>
											<RestrictedIndicator>false</RestrictedIndicator>
											<IncludesPortCharges>false</IncludesPortCharges>
											<IsCommission>false</IsCommission>
										</Price>
									</xsl:for-each>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="$INFprices/PassengerFare/BaseFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</InfantPrices>
							</xsl:if>
							<xsl:variable name="INSprices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='INS']"/>
							<xsl:if test="$INSprices!=''">
								<xsl:variable name="nip"><xsl:value-of select="$INSprices/PassengerTypeQuantity/@Quantity"/></xsl:variable>
								<SeatedInfantPrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0'">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt"><xsl:value-of select="$INSprices/PassengerFare/TotalFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($INSprices/PassengerFare/Taxes/Tax/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:for-each select="$INSprices/PassengerFare/Taxes/Tax">
										<Price>
											<ID>8</ID>
											<Name>Air Tax</Name>
											<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
											<Amount>
												<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
												<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
												<xsl:choose>
													<xsl:when test="$dec!='0'">
														<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
													</xsl:when>
													<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
												</xsl:choose>
											</Amount>
											<AmountYield>0</AmountYield>
											<Priority>0</Priority>
											<RestrictedIndicator>false</RestrictedIndicator>
											<IncludesPortCharges>false</IncludesPortCharges>
											<IsCommission>false</IsCommission>
										</Price>
									</xsl:for-each>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="$INSprices/PassengerFare/BaseFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</SeatedInfantPrices>
							</xsl:if>
							<xsl:variable name="SRCprices" select="ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code='SRC']"/>
							<xsl:if test="$SRCprices!=''">
								<xsl:variable name="nip"><xsl:value-of select="$SRCprices/PassengerTypeQuantity/@Quantity"/></xsl:variable>
								<SeniorPrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0'">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt"><xsl:value-of select="$SRCprices/PassengerFare/TotalFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="sum($SRCprices/PassengerFare/Taxes/Tax/@Amount)"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:for-each select="$SRCprices/PassengerFare/Taxes/Tax">
										<Price>
											<ID>8</ID>
											<Name>Air Tax</Name>
											<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
											<Amount>
												<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
												<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
												<xsl:choose>
													<xsl:when test="$dec!='0'">
														<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
														<xsl:text>.</xsl:text>
														<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
													</xsl:when>
													<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
												</xsl:choose>
											</Amount>
											<AmountYield>0</AmountYield>
											<Priority>0</Priority>
											<RestrictedIndicator>false</RestrictedIndicator>
											<IncludesPortCharges>false</IncludesPortCharges>
											<IsCommission>false</IsCommission>
										</Price>
									</xsl:for-each>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt"><xsl:value-of select="$SRCprices/PassengerFare/BaseFare/@Amount"/></xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="($amt div $nip) + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</SeniorPrices>
							</xsl:if>
							<xsl:variable name="flights" select="ItineraryInfo/ReservationItems"/>
							<Flights>
								<!--xsl:apply-templates select="OTA_AirBookRQ/AirItinerary/OriginDestinationOptions/OriginDestinationOption[1]">
									<xsl:with-param name="flights" select="$flights"/>
									<xsl:with-param name="pos">1</xsl:with-param>
								</xsl:apply-templates-->
								<xsl:apply-templates select="ItineraryInfo/ReservationItems"/>
							</Flights>
							<ClassTypeChanged>false</ClassTypeChanged>
							<CommissionPercent>0</CommissionPercent>
							<Promotions/>
							<SiteCommissions/>
							<TicketIssueDate>0001-01-01T00:00:00.0000000-05:00</TicketIssueDate>
							<PaymentSchedule>
								<PaymentSchedule>
									<PaymentNumber>0</PaymentNumber>
									<Amount>
										<xsl:choose>
											<xsl:when test="$dec!='0'">
												<xsl:value-of select="substring($prices/ItinTotalFare/TotalFare/@Amount,1,string-length($prices/ItinTotalFare/TotalFare/@Amount) - $dec)"/>
												<xsl:text>.</xsl:text>
												<xsl:value-of select="substring($prices/ItinTotalFare/TotalFare/@Amount, string-length($prices/ItinTotalFare/TotalFare/@Amount) - ($dec -1))"/>
											</xsl:when>
											<xsl:otherwise><xsl:value-of select="$prices/ItinTotalFare/TotalFare/@Amount"/></xsl:otherwise>
										</xsl:choose>
									</Amount>
									<Description>Full Payment</Description>
									<xsl:choose>
										<xsl:when test="ItineraryInfo/Ticketing/@TicketTimeLimit!=''">
											<AgencyDate>
												<xsl:value-of select="ItineraryInfo/Ticketing/@TicketTimeLimit"/>
											</AgencyDate>
											<PaidDate>0001-01-01T00:00:00.0000000-05:00</PaidDate>
											<SupplierDate>
												<xsl:value-of select="ItineraryInfo/Ticketing/@TicketTimeLimit"/>
											</SupplierDate>
										</xsl:when>
										<xsl:otherwise>
											<xsl:variable name="ttl">
												<xsl:text>20</xsl:text>
												<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,8,2)"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,6,2)"/>
												<xsl:text>-</xsl:text>
												<xsl:value-of select="substring(ItineraryInfo/Ticketing/TicketAdvisory,4,2)"/>
												<xsl:text>T00:00:00</xsl:text>
											</xsl:variable>
											<AgencyDate>
												<xsl:value-of select="$ttl"/>
											</AgencyDate>
											<PaidDate>0001-01-01T00:00:00.0000000-05:00</PaidDate>
											<SupplierDate>
												<xsl:value-of select="$ttl"/>
											</SupplierDate>
										</xsl:otherwise>
									</xsl:choose>
									<SupplierID>0</SupplierID>
									<PaymentType>Air</PaymentType>
								</PaymentSchedule>
							</PaymentSchedule>
							<DepartureCity>
								<ID>
									<xsl:value-of select="$flights/Item[1]/Air/DepartureAirport/@LocationCode"/>
								</ID>
								<Name>
									<xsl:value-of select="$flights/Item[1]/Air/DepartureAirport"/>
								</Name>
								<IsAirPort>false</IsAirPort>
								<IsSeaPort>false</IsSeaPort>
								<LanguageId>0</LanguageId>
								<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
								<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
							</DepartureCity>
							<DepartureDateTime>
								<xsl:value-of select="$flights/Item[1]/Air/@DepartureDateTime"/>
							</DepartureDateTime>
							<ArrivalCity>
								<ID>
									<xsl:value-of select="$flights/Item[Air][position()=last()]/Air/ArrivalAirport/@LocationCode"/>
								</ID>
								<Name>
									<xsl:value-of select="$flights/Item[Air][position()=last()]/Air/ArrivalAirport"/>
								</Name>
								<IsAirPort>false</IsAirPort>
								<IsSeaPort>false</IsSeaPort>
								<LanguageId>0</LanguageId>
								<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
								<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
							</ArrivalCity>
							<ArrivalDateTime>
								<xsl:value-of select="$flights/Item[Air][position()=last()]/Air/@ArrivalDateTime"/>
							</ArrivalDateTime>
							<Duration>0</Duration>
							<FromCity>
								<xsl:value-of select="$flights/Item[1]/Air/DepartureAirport/@LocationCode"/>
								</FromCity>
							<ToCity>
								<xsl:value-of select="$flights/Item[1]/Air/ArrivalAirport/@LocationCode"/>
							</ToCity>
							<FromCountry>US</FromCountry>
							<ToCountry>US</ToCountry>
						</Air>
						<MinInsidePrice>0</MinInsidePrice>
						<MinOutsidePrice>0</MinOutsidePrice>
						<MinBalconyPrice>0</MinBalconyPrice>
						<MinSuitePrice>0</MinSuitePrice>
						<MinInsidePriceYield>0</MinInsidePriceYield>
						<MinOutsidePriceYield>0</MinOutsidePriceYield>
						<MinBalconyPriceYield>0</MinBalconyPriceYield>
						<MinSuitePriceYield>0</MinSuitePriceYield>
						<PriceLastUpdated>0001-01-01T00:00:00.0000000-05:00</PriceLastUpdated>
						<TagData>0</TagData>
					</Package>
					<Passangers>
						<xsl:for-each select="CustomerInfos/CustomerInfo">
							<CustomerAccount>
								<isChild>false</isChild>
								<isYouth>false</isYouth>
								<isAdult>false</isAdult>
								<isSenior>false</isSenior>
								<isInfant>false</isInfant>
								<isSeatedInfant>false</isSeatedInfant>
								<ID>0</ID>
								<ParentID>0</ParentID>
								<SiteID>0</SiteID>
								<Title>
									<xsl:value-of select="Customer/PersonName/NamePrefix"/>
								</Title>
								<FirstName>
									<xsl:value-of select="Customer/PersonName/GivenName"/>
								</FirstName>
								<MiddleName>
									<xsl:value-of select="Customer/PersonName/MiddleName"/>
								</MiddleName>
								<LastName>
									<xsl:value-of select="Customer/PersonName/Surname"/>
								</LastName>
								<Sex>
									<xsl:choose>
										<xsl:when test="contains(Customer/PersonName/GivenName,' MRS')">Female</xsl:when>
										<xsl:when test="contains(Customer/PersonName/GivenName,' MISS')">Female</xsl:when>
										<xsl:otherwise>Male</xsl:otherwise>
									</xsl:choose>
								</Sex>
								<DateOfBirth>0001-01-01T00:00:00.0000000-05:00</DateOfBirth>
								<Age>0</Age>
								<DocumentIssueDate>0001-01-01T00:00:00.0000000-05:00</DocumentIssueDate>
								<DocumentExpirationDate>0001-01-01T00:00:00.0000000-05:00</DocumentExpirationDate>
								<eMail>
									<xsl:value-of select="Customer/Email"/>
								</eMail>
								<PhoneOne>
									<xsl:value-of select="Customer/Telephone[@PhoneUseType='H'][1]/@PhoneNumber"/>
								</PhoneOne>
								<PhoneTwo>
									<xsl:value-of select="Customer/Telephone[@PhoneUseType='H'][position()=2]/@PhoneNumber"/>
								</PhoneTwo>
								<Fax/>
								<Password/>
								<ReceiveNotification>false</ReceiveNotification>
								<ReceiveNotificationFormat>T</ReceiveNotificationFormat>
								<LastLoginDate>0001-01-01T00:00:00.0000000-05:00</LastLoginDate>
								<Active>true</Active>
								<GuestRPH>0</GuestRPH>
								<EmergencyFlag>false</EmergencyFlag>
								<xsl:variable name="paxtype"><xsl:value-of select="Customer/PersonName/@NameType"/></xsl:variable>
								<xsl:variable name="prices1" select="../../ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity/@Code=$paxtype]"/>
								<xsl:variable name="prices2" select="../../ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo/PTC_FareBreakdowns/PTC_FareBreakdown[PassengerTypeQuantity[not(@Code)]]"/>
								<xsl:variable name="nip">
									<xsl:choose>
										<xsl:when test="$paxtype=''">
											<xsl:value-of select="sum($prices2/PassengerTypeQuantity/@Quantity)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="sum($prices1/PassengerTypeQuantity/@Quantity)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:variable name="dec">
									<xsl:choose>
										<xsl:when test="$paxtype=''">
											<xsl:value-of select="$prices2/PassengerFare/BaseFare/@DecimalPlaces"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="$prices1/PassengerFare/BaseFare/@DecimalPlaces"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<CruisePrices>
									<Price>
										<ID>48</ID>
										<Name>Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Omarkup='0'">0</xsl:when>
												<xsl:when test="contains($Omarkup,'.')">
													<xsl:value-of select="format-number($Omarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Omarkup,1,string-length($Omarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Omarkup, string-length($Omarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Omarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>49</ID>
										<Name>Agency/Affiliate Custom Markup</Name>
										<Amount>
											<xsl:choose>
												<xsl:when test="$Amarkup='0' or $Amarkup=''">0</xsl:when>
												<xsl:when test="contains($Amarkup,'.')">
													<xsl:value-of select="format-number($Amarkup,'0.00')"/>
												</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($Amarkup,1,string-length($Amarkup) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($Amarkup, string-length($Amarkup) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$Amarkup"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>false</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>23</ID>
										<Name>Total Due</Name>
										<Amount>	
											<xsl:variable name="amt">
												<xsl:choose>
													<xsl:when test="$paxtype=''">
														<xsl:value-of select="sum($prices2/PassengerFare/TotalFare/@Amount)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="sum($prices1/PassengerFare/TotalFare/@Amount)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable  name="amt2"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:variable name="amt1"><xsl:value-of select="$amt2 + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<Price>
										<ID>8</ID>
										<Name>Air Tax</Name>
										<PseudoCode>TotalTax</PseudoCode>
										<Amount>
											<xsl:variable name="amt">
												<xsl:choose>
													<xsl:when test="$paxtype=''">
														<xsl:value-of select="sum($prices2/PassengerFare/Taxes/Tax/@Amount)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="sum($prices1/PassengerFare/Taxes/Tax/@Amount)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$amt='0'">0.00</xsl:when>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
									<xsl:choose>
										<xsl:when test="$paxtype=''">
											<xsl:for-each select="$prices2/PassengerFare/Taxes/Tax">
												<xsl:variable name="nip1">
													<xsl:value-of select="../../../PassengerTypeQuantity/@Quantity"/>
												</xsl:variable>
												<Price>
													<ID>8</ID>
													<Name>Air Tax</Name>
													<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
													<Amount>
														<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
														<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip1"/></xsl:variable>
														<xsl:choose>
															<xsl:when test="$dec!='0'">
																<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
															</xsl:when>
															<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
														</xsl:choose>
													</Amount>
													<AmountYield>0</AmountYield>
													<Priority>0</Priority>
													<RestrictedIndicator>false</RestrictedIndicator>
													<IncludesPortCharges>false</IncludesPortCharges>
													<IsCommission>false</IsCommission>
												</Price>
											</xsl:for-each>
										</xsl:when>
										<xsl:otherwise>
											<xsl:for-each select="$prices1/PassengerFare/Taxes/Tax">
												<xsl:variable name="nip1">
													<xsl:value-of select="../../../PassengerTypeQuantity/@Quantity"/>
												</xsl:variable>
												<Price>
													<ID>8</ID>
													<Name>Air Tax</Name>
													<PseudoCode><xsl:value-of select="@Code"/></PseudoCode>
													<Amount>
														<xsl:variable name="amt"><xsl:value-of select="@Amount"/></xsl:variable>
														<xsl:variable  name="amt1"><xsl:value-of select="$amt div $nip1"/></xsl:variable>
														<xsl:choose>
															<xsl:when test="$dec!='0'">
																<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
																<xsl:text>.</xsl:text>
																<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
															</xsl:when>
															<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
														</xsl:choose>
													</Amount>
													<AmountYield>0</AmountYield>
													<Priority>0</Priority>
													<RestrictedIndicator>false</RestrictedIndicator>
													<IncludesPortCharges>false</IncludesPortCharges>
													<IsCommission>false</IsCommission>
												</Price>
											</xsl:for-each>
										</xsl:otherwise>
									</xsl:choose>
									<Price>
										<ID>7</ID>
										<Name>Air Fare</Name>
										<Amount>
											<xsl:variable name="amt">
												<xsl:choose>
													<xsl:when test="$paxtype=''">
														<xsl:value-of select="sum($prices2/PassengerFare/BaseFare/@Amount)"/>
													</xsl:when>
													<xsl:otherwise>
														<xsl:value-of select="sum($prices1/PassengerFare/BaseFare/@Amount)"/>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:variable>
											<xsl:variable name="amt2"><xsl:value-of select="$amt div $nip"/></xsl:variable>
											<xsl:variable name="amt1"><xsl:value-of select="$amt2 + $Omarkup + $Amarkup"/></xsl:variable>
											<xsl:choose>
												<xsl:when test="$dec!='0'">
													<xsl:value-of select="substring($amt1,1,string-length($amt1) - $dec)"/>
													<xsl:text>.</xsl:text>
													<xsl:value-of select="substring($amt1, string-length($amt1) - ($dec -1))"/>
												</xsl:when>
												<xsl:otherwise><xsl:value-of select="$amt1"/></xsl:otherwise>
											</xsl:choose>
										</Amount>
										<AmountYield>0</AmountYield>
										<Priority>0</Priority>
										<RestrictedIndicator>true</RestrictedIndicator>
										<IncludesPortCharges>false</IncludesPortCharges>
										<IsCommission>false</IsCommission>
									</Price>
								</CruisePrices>
								<DiningTableSize>0</DiningTableSize>
								<InsertID>0</InsertID>
								<InsertDate>0001-01-01T00:00:00</InsertDate>
								<ModifiedID>0</ModifiedID>
								<ModifiedDate>0001-01-01T00:00:00</ModifiedDate>
							</CustomerAccount>
						</xsl:for-each>
					</Passangers>
					<PassangersAgeGroup>
						<Adults>
							<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType='ADT'])"/>
						</Adults>
						<Seniors>
							<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType='SRC'])"/>
						</Seniors>
						<Children>
							<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType='CHD'])"/>
						</Children>
						<Youth>
							<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType='YTH'])"/>
						</Youth>
						<InfantsInLap>
							<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType='INF'])"/>
						</InfantsInLap>
						<SeatedInfants>
							<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType='INS'])"/>
						</SeatedInfants>
					</PassangersAgeGroup>
					<GuestQty>
						<xsl:value-of select="count(CustomerInfos/CustomerInfo[Customer/PersonName/@NameType!='INF'])"/>
					</GuestQty>
					<ApplicationId>0</ApplicationId>
					<AgentContactId><xsl:value-of select="POS/Source/@AgentSine"/></AgentContactId>
					<AgentName><xsl:value-of select="POS/Source/@AgentSine"/></AgentName>
					<AgentContactNumber><xsl:value-of select="POS/Source/@AgentSine"/></AgentContactNumber>
					<User>
						<ID>0</ID>
					</User>
					<HoldExpirationTimer>0001-01-01T00:00:00.0000000-05:00</HoldExpirationTimer>
					<CreateDateTime>0001-01-01T00:00:00.0000000-05:00</CreateDateTime>
					<ModifiedDateTime>0001-01-01T00:00:00.0000000-05:00</ModifiedDateTime>
					<TableSize>0</TableSize>
					<RefSiteItemID>0</RefSiteItemID>
					<ReferrerID1> </ReferrerID1>
					<ExportDateTime>0001-01-01T00:00:00.0000000-05:00</ExportDateTime>
					<ConvBufferRate>1</ConvBufferRate>
					<xsl:if test="../../OTA_ReadRQ/SplitID/@ID!=''">
						<SplitPNRNumber><xsl:value-of select="../../OTA_ReadRQ/SplitID/@ID"/></SplitPNRNumber>
					</xsl:if>
				</booking>
			</CreateBookingInDB>
		</CB>
	</xsl:template>
	
	<xsl:template match="ReservationItems">
		<Flight>
			<Active>true</Active>
			<ProductType>
				<ID>2</ID>
				<Name>Air</Name>
			</ProductType>
			<FlightSegments>
				<!--xsl:apply-templates select="FlightSegment[1]">
					<xsl:with-param name="flights" select="$flights"/>
					<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
				</xsl:apply-templates-->
				<xsl:apply-templates select="Item[Air]"/>
			</FlightSegments>
			<TotalDuration>
				<Days>0</Days>
				<Hours>0</Hours>
				<Minutes>0</Minutes>
				<Seconds>0</Seconds>
			</TotalDuration>
		</Flight>
	</xsl:template>
	
	<xsl:template match="OriginDestinationOption">
		<xsl:param name="flights"/>
		<xsl:param name="pos"/>
		<Flight>
			<Active>true</Active>
			<ProductType>
				<ID>2</ID>
				<Name>Air</Name>
			</ProductType>
			<FlightSegments>
				<xsl:apply-templates select="FlightSegment[1]">
					<xsl:with-param name="flights" select="$flights"/>
					<xsl:with-param name="pos"><xsl:value-of select="$pos"/></xsl:with-param>
				</xsl:apply-templates>
			</FlightSegments>
			<TotalDuration>
				<Days>0</Days>
				<Hours>0</Hours>
				<Minutes>0</Minutes>
				<Seconds>0</Seconds>
			</TotalDuration>
		</Flight>
		<xsl:variable name="prepos"><xsl:value-of select="count(preceding-sibling::OriginDestinationOption/FlightSegment)"/></xsl:variable>
		<xsl:variable name="preposf"><xsl:value-of select="count(FlightSegment)"/></xsl:variable>
		<xsl:apply-templates select="following-sibling::OriginDestinationOption[1]">
			<xsl:with-param name="flights" select="$flights"/>
			<xsl:with-param name="pos"><xsl:value-of select="$prepos + $preposf + 1"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="Item[Air]">
		<FlightSegment>
			<Stops>0</Stops>
			<Rph>
				<xsl:value-of select="Air/@RPH"/>
			</Rph>
			<FlightSegmentNumber>
				<xsl:value-of select="Air/@FlightNumber"/>
			</FlightSegmentNumber>
			<FlightContext></FlightContext>
			<BookingClass/>
			<SelectedBookingClass>
				<ID>
					<xsl:value-of select="Air/@ResBookDesigCode"/>
				</ID>
				<ClassType></ClassType>
				<SelectedSeats/>
				<NumberofMinSeats>0</NumberofMinSeats>
				<Priority>0</Priority>
				<LanguageID>0</LanguageID>
				<Prices/>
			</SelectedBookingClass>
			<DiningProvided>false</DiningProvided>
			<ETicketAvailable>
				<xsl:choose>
					<xsl:when test="Air/@E_TicketEligibility='Eligible'">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</ETicketAvailable>
			<DepartureDateTime>
				<xsl:value-of select="Air/@DepartureDateTime"/>
			</DepartureDateTime>
			<ArrivalDateTime>
				<xsl:value-of select="Air/@ArrivalDateTime"/>
			</ArrivalDateTime>
			<ArrivalAirport>
				<ID>
					<xsl:value-of select="Air/ArrivalAirport/@LocationCode"/>
				</ID>
				<Name>
					<xsl:value-of select="Air/ArrivalAirport"/>
				</Name>
				<IsAirPort>false</IsAirPort>
				<IsSeaPort>false</IsSeaPort>
				<LanguageId>0</LanguageId>
				<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
				<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
			</ArrivalAirport>
			<DepartureAirport>
				<ID>
					<xsl:value-of select="Air/DepartureAirport/@LocationCode"/>
				</ID>
				<Name>
					<xsl:value-of select="Air/DepartureAirport"/>
				</Name>
				<IsAirPort>false</IsAirPort>
				<IsSeaPort>false</IsSeaPort>
				<LanguageId>0</LanguageId>
				<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
				<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
			</DepartureAirport>
			<AirportChange>false</AirportChange>
			<NumDayChange>0</NumDayChange>
			<ClassTypeChanged>false</ClassTypeChanged>
			<MarketingAirline>
				<ID>0</ID>
				<LanguageID>0</LanguageID>
				<PseudoCode>
					<xsl:value-of select="Air/MarketingAirline/@Code"/>
				</PseudoCode>
				<Name>
					<xsl:value-of select="Air/MarketingAirline/@Code"/>
				</Name>
				<Code>
					<xsl:value-of select="Air/MarketingAirline/@Code"/>
				</Code>
			</MarketingAirline>
			<OperatingAirline>
				<ID>0</ID>
				<LanguageID>0</LanguageID>
				<PseudoCode>
					<xsl:value-of select="Air/OperatingAirline/@Code"/>
				</PseudoCode>
				<Name>
					<xsl:value-of select="Air/OperatingAirline/@Code"/>
				</Name>
				<Code>
					<xsl:value-of select="Air/OperatingAirline/@Code"/>
				</Code>
			</OperatingAirline>
			<Airplane>
				<ID>0</ID>
				<LanguageID>0</LanguageID>
				<Active>false</Active>
				<PseudoCode>
					<xsl:value-of select="Air/Equipment/@AirEquipType"/>
				</PseudoCode>
				<Name>
					<xsl:value-of select="Air/Equipment"/>
				</Name>
			</Airplane>
			<AirlineRecLoc><xsl:value-of select="Air/TPA_Extensions/@ConfirmationNumber"/></AirlineRecLoc>
		</FlightSegment>
	</xsl:template>
	
	<xsl:template match="FlightSegment">
		<xsl:param name="flights"/>
		<xsl:param name="pos"/>
		<FlightSegment>
			<Stops>0</Stops>
			<Rph>
				<xsl:value-of select="$flights/Item[position()=$pos]/Air/@RPH"/>
			</Rph>
			<FlightSegmentNumber>
				<xsl:value-of select="@FlightNumber"/>
			</FlightSegmentNumber>
			<FlightContext></FlightContext>
			<BookingClass/>
			<SelectedBookingClass>
				<ID>
					<xsl:value-of select="@ResBookDesigCode"/>
				</ID>
				<ClassType></ClassType>
				<SelectedSeats/>
				<NumberofMinSeats>0</NumberofMinSeats>
				<Priority>0</Priority>
				<LanguageID>0</LanguageID>
				<Prices/>
			</SelectedBookingClass>
			<DiningProvided>false</DiningProvided>
			<ETicketAvailable>
				<xsl:choose>
					<xsl:when test="$flights/Item[position()=$pos]/Air/@E_TicketEligibility='Eligible'">true</xsl:when>
					<xsl:otherwise>false</xsl:otherwise>
				</xsl:choose>
			</ETicketAvailable>
			<DepartureDateTime>
				<xsl:value-of select="$flights/Item[position()=$pos]/Air/@DepartureDateTime"/>
			</DepartureDateTime>
			<ArrivalDateTime>
				<xsl:value-of select="$flights/Item[position()=$pos]/Air/@ArrivalDateTime"/>
			</ArrivalDateTime>
			<ArrivalAirport>
				<ID>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/ArrivalAirport/@LocationCode"/>
				</ID>
				<Name>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/ArrivalAirport/@LocationCode"/>
				</Name>
				<IsAirPort>false</IsAirPort>
				<IsSeaPort>false</IsSeaPort>
				<LanguageId>0</LanguageId>
				<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
				<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
			</ArrivalAirport>
			<DepartureAirport>
				<ID>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/DepartureAirport/@LocationCode"/>
				</ID>
				<Name>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/DepartureAirport/@LocationCode"/>
				</Name>
				<IsAirPort>false</IsAirPort>
				<IsSeaPort>false</IsSeaPort>
				<LanguageId>0</LanguageId>
				<DayLightSavingsTimeDifferenceFromGMT>0</DayLightSavingsTimeDifferenceFromGMT>
				<TimeDifferenceFromGMT>0</TimeDifferenceFromGMT>
			</DepartureAirport>
			<AirportChange>false</AirportChange>
			<NumDayChange>0</NumDayChange>
			<ClassTypeChanged>false</ClassTypeChanged>
			<MarketingAirline>
				<ID>0</ID>
				<LanguageID>0</LanguageID>
				<PseudoCode>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/MarketingAirline/@Code"/>
				</PseudoCode>
				<Name>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/MarketingAirline/@Code"/>
				</Name>
				<Code>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/MarketingAirline/@Code"/>
				</Code>
			</MarketingAirline>
			<OperatingAirline>
				<ID>0</ID>
				<LanguageID>0</LanguageID>
				<PseudoCode>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/OperatingAirline/@Code"/>
				</PseudoCode>
				<Name>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/OperatingAirline/@Code"/>
				</Name>
				<Code>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/OperatingAirline/@Code"/>
				</Code>
			</OperatingAirline>
			<Airplane>
				<ID>0</ID>
				<LanguageID>0</LanguageID>
				<Active>false</Active>
				<PseudoCode>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/Equipment/@AirEquipType"/>
				</PseudoCode>
				<Name>
					<xsl:value-of select="$flights/Item[position()=$pos]/Air/Equipment"/>
				</Name>
			</Airplane>
			<AirlineRecLoc><xsl:value-of select="$flights/Item[position()=$pos]/Air/TPA_Extensions/@ConfirmationNumber"/></AirlineRecLoc>
		</FlightSegment>
		<xsl:apply-templates select="following-sibling::FlightSegment[1]">
			<xsl:with-param name="flights" select="$flights"/>
			<xsl:with-param name="pos"><xsl:value-of select="$pos + 1"/></xsl:with-param>
		</xsl:apply-templates>
	</xsl:template>
	
	<xsl:template match="Remark" mode="owner">
		<xsl:param name="tot"/>
		<xsl:variable name="m"><xsl:value-of select="translate(substring-before(substring(.,8),' '),',','.')"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="following-sibling::Remark[contains(.,'TOTAL MARKUP')][1]">
				<xsl:apply-templates select="following-sibling::Remark[contains(.,'TOTAL MARKUP')][1]" mode="owner">
					<xsl:with-param name="tot"><xsl:value-of select="$tot + $m"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="$tot + $m"/></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template match="Remark" mode="affiliate">
		<xsl:param name="tot"/>
		<xsl:variable name="m"><xsl:value-of select="substring-before(substring(.,8),' ')"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="following-sibling::Remark[contains(.,'Affiliate markup')][1]">
				<xsl:apply-templates select="following-sibling::Remark[contains(.,'Affiliate markup')][1]" mode="affiliate">
					<xsl:with-param name="tot"><xsl:value-of select="$tot + $m"/></xsl:with-param>
				</xsl:apply-templates>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="$tot + $m"/></xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
