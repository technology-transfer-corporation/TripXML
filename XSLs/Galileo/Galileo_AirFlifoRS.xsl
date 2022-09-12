<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_AirFlifoRS.xsl 																-->
	<!-- ================================================================== -->
	<!-- Date: 19 Jul 2005 - Bug TT64 - Rastko											-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="FlightInfo_6_0" />
	</xsl:template>
	<!--************************************************************************************************************-->
	<xsl:template match="FlightInfo_6_0">
		<OTA_AirFlifoRS>
			<xsl:choose>
				<xsl:when test="FltInfo/ItemAry/Item[DataBlkInd='F']/FltQual[VErr!='0']  or FltInfo/ErrorCode !='' or FltInfoMods/ErrorCode !=''">
					<Errors>
						<xsl:apply-templates select="FltInfo/ItemAry/Item/FltQual" mode="Error" />
						<xsl:apply-templates select="FltInfo/ErrorCode" mode="FltInfo" />
						<xsl:apply-templates select="FltInfoMods/ErrorCode" mode="FltInfoMods" />
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<xsl:choose>
						<xsl:when test="FltInfo/ItemAry/Item[DataBlkInd='F']/FltQual[VErr ='0']  and not(FltInfo/ItemAry/Item)">
							<Errors>
								<Error>
								<xsl:attribute name="Type">Flifo</xsl:attribute>
        							<xsl:attribute name="Code">
										<xsl:value-of select="GDS" />
									</xsl:attribute>GDS unable to process
        						</Error>
							</Errors>
						</xsl:when>
						<xsl:otherwise>
							<FlightInfoDetails>
								<xsl:apply-templates select="FltInfo/ItemAry/Item[DataBlkInd='C']" mode="Data" />
							</FlightInfoDetails>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_AirFlifoRS>
	</xsl:template>
	<!--************************************************************************************************************-->
	<!--  					Error  within FltInfo tags                 							      -->
	<!--***********************************************************************************************************  -->
	<xsl:template match="ErrorCode" mode="FltInfo">
		<Error>
			<xsl:attribute name="Type">Galileo</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="." />
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test=". = '0001'">Invalid data in request</xsl:when>
				<xsl:when test=". = '0002'">Transaction not allowed</xsl:when>
				<xsl:when test=". = '0003'">Invalid/passed date in request</xsl:when>
				<xsl:when test=". = '0004'">Flight not found</xsl:when>
				<xsl:when test=". = '0005'">Specified board point not found</xsl:when>
				<xsl:when test=". = '0006'">Specified off point not found</xsl:when>
				<xsl:when test=". = '0007'">Board/Off points missing from request</xsl:when>
				<xsl:otherwise>General system error</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>
	<!--************************************************************************************************************-->
	<!--  					Error  within FltInfo tags                 							      -->
	<!--***********************************************************************************************************  -->
	<xsl:template match="ErrorCode" mode="FltInfoMods">
		<Error>
			<xsl:attribute name="Type">Flifo</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="." />
			</xsl:attribute>
		</Error>
	</xsl:template>
	<!--************************************************************************************************************-->
	<!--  					Flight Information Response	- Error Path                                             -->
	<!--***********************************************************************************************************  -->
	<xsl:template match="FltQual" mode="Error">
		<Error>
			<xsl:attribute name="Type">Flifo</xsl:attribute>
			<xsl:attribute name="Code">
				<xsl:value-of select="VErr" />
			</xsl:attribute>
			<xsl:choose>
				<xsl:when test="VErr = 100">Invalid place of departure code</xsl:when>
				<xsl:when test="VErr = 101">Invalid place of destination code</xsl:when>
				<xsl:when test="VErr = 102">Invalid departure date</xsl:when>
				<xsl:when test="VErr = 114">Invalid flight number</xsl:when>
				<xsl:when test="VErr = 117">Schedule change in progress</xsl:when>
				<xsl:when test="VErr = 118">System unable to process</xsl:when>
				<xsl:when test="VErr = 155">Message function not supported</xsl:when>
				<xsl:when test="VErr = 303">Flight cancelled</xsl:when>
				<xsl:when test="VErr = 409">Invalid departure date</xsl:when>
				<xsl:when test="VErr = 410">Flight does not operate due to weather, mechanical or other conditions</xsl:when>
				<xsl:when test="VErr = 411">Flight does not operate on date requested</xsl:when>
				<xsl:when test="VErr = 412">Flight does not operate between requested cities</xsl:when>
				<xsl:when test="VErr = 413">Requested flight not active</xsl:when>
				<xsl:otherwise>Invalid Format</xsl:otherwise>
			</xsl:choose>
		</Error>
	</xsl:template>
	<!--************************************************************************************************************-->
	<!--  					Flight Information Response	Processing                                             -->
	<!--***********************************************************************************************************  -->
	<xsl:template match="Item" mode="Data">
		<xsl:choose>
			<xsl:when test="CityQual/StartEndInd = 'D'">
				<FlightLegInfo>
					<xsl:attribute name="FlightNumber">
						<xsl:apply-templates select="//FltInfo/ItemAry/Item/FltQual" mode="FltNum" />
					</xsl:attribute>
					<xsl:attribute name="FlightStatus">
						<xsl:choose>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='AD'">
								<xsl:text>Departed</xsl:text>
							</xsl:when>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='ED'">
								<xsl:text>Delayed</xsl:text>
							</xsl:when>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='DX'">
								<xsl:text>Departure Canceled</xsl:text>
							</xsl:when>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='LX'">
								<xsl:text>Landing canceled</xsl:text>
							</xsl:when>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='DV'">
								<xsl:text>Diverted</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>On Schedule</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
					<DepartureAirport>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="CityQual/City" />
						</xsl:attribute>
						<xsl:attribute name="CodeContext">IATA</xsl:attribute>
					</DepartureAirport>
					<ArrivalAirport>
						<xsl:attribute name="LocationCode">
							<xsl:value-of select="following-sibling::Item/CityQual/City" />
						</xsl:attribute>
						<xsl:attribute name="CodeContext">IATA</xsl:attribute>
						<xsl:choose>
							<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmQual='DV'">
								<xsl:attribute name="Diversion">1</xsl:attribute>
							</xsl:when>
							<xsl:otherwise>
								<xsl:attribute name="Diversion">0</xsl:attribute>
							</xsl:otherwise>
						</xsl:choose>
					</ArrivalAirport>
					<xsl:apply-templates select="//FltInfo/ItemAry/Item/FltQual" mode="AirCode" />
					<xsl:if test="CityQual/StartQual/Equip != ''">
						<Equipment>
							<xsl:attribute name="AirEquipType">
								<xsl:value-of select="CityQual/StartQual/Equip" />
							</xsl:attribute>
						</Equipment>
					</xsl:if>
					<Comment />
					<DepartureDateTime>
						<xsl:attribute name="Scheduled">
							<xsl:variable name="zeroes">0000</xsl:variable>
							<xsl:variable name="deptime">
								<xsl:value-of select="substring(string($zeroes),1,4-string-length(CityQual/StartQual/Tm))" />
								<xsl:value-of select="CityQual/StartQual/Tm" />
							</xsl:variable>
							<xsl:value-of select="substring(CityQual/StartQual/Dt,1,4)" />
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(CityQual/StartQual/Dt,5,2)" />
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(CityQual/StartQual/Dt,7,2)" />
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring($deptime,1,2)" />
							<xsl:text>:</xsl:text>
							<xsl:value-of select="substring($deptime,3,2)" />
							<xsl:text>:00</xsl:text>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='ED'">
								<xsl:attribute name="Estimated">
									<xsl:variable name="zeroes">0000</xsl:variable>
									<xsl:variable name="deptime">
										<xsl:choose>
											<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/Tm!='0'">
												<xsl:value-of select="substring(string($zeroes),1,4-string-length(CityQual/StartQual/TmQualAry/TmQualInfo/Tm))" />
												<xsl:value-of select="CityQual/StartQual/TmQualAry/TmQualInfo/Tm" />
											</xsl:when>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="substring(CityQual/StartQual/Dt,1,4)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(CityQual/StartQual/Dt,5,2)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(CityQual/StartQual/Dt,7,2)" />
									<xsl:text>T</xsl:text>
									<xsl:value-of select="substring($deptime,1,2)" />
									<xsl:text>:</xsl:text>
									<xsl:value-of select="substring($deptime,3,2)" />
									<xsl:text>:00</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='AD'">
								<xsl:attribute name="Actual">
									<xsl:variable name="zeroes">0000</xsl:variable>
									<xsl:variable name="deptime">
										<xsl:choose>
											<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/Tm!='0'">
												<xsl:value-of select="substring(string($zeroes),1,4-string-length(CityQual/StartQual/TmQualAry/TmQualInfo/Tm))" />
												<xsl:value-of select="CityQual/StartQual/TmQualAry/TmQualInfo/Tm" />
											</xsl:when>
										</xsl:choose>
									</xsl:variable>
									<xsl:value-of select="substring(CityQual/StartQual/Dt,1,4)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(CityQual/StartQual/Dt,5,2)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(CityQual/StartQual/Dt,7,2)" />
									<xsl:text>T</xsl:text>
									<xsl:value-of select="substring($deptime,1,2)" />
									<xsl:text>:</xsl:text>
									<xsl:value-of select="substring($deptime,3,2)" />
									<xsl:text>:00</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</DepartureDateTime>
					<ArrivalDateTime>
						<xsl:attribute name="Scheduled">
							<xsl:variable name="zeroes">0000</xsl:variable>
							<xsl:variable name="Arrtime">
								<xsl:value-of select="substring(string($zeroes),1,4-string-length(following-sibling::Item/CityQual/EndQual/Tm))" />
								<xsl:value-of select="following-sibling::Item/CityQual/EndQual/Tm" />
							</xsl:variable>
							<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,1,4)" />
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,5,2)" />
							<xsl:text>-</xsl:text>
							<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,7,2)" />
							<xsl:text>T</xsl:text>
							<xsl:value-of select="substring($Arrtime,1,2)" />
							<xsl:text>:</xsl:text>
							<xsl:value-of select="substring($Arrtime,3,2)" />
							<xsl:text>:00</xsl:text>
						</xsl:attribute>
						<xsl:choose>
							<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmQual='EB'">
								<xsl:attribute name="Estimated">
									<xsl:variable name="zeroes">0000</xsl:variable>
									<xsl:variable name="Arrtime">
										<xsl:value-of select="substring(string($zeroes),1,4-string-length(following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm))" />
										<xsl:value-of select="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm" />
									</xsl:variable>
									<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,1,4)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,5,2)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,7,2)" />
									<xsl:text>T</xsl:text>
									<xsl:value-of select="substring($Arrtime,1,2)" />
									<xsl:text>:</xsl:text>
									<xsl:value-of select="substring($Arrtime,3,2)" />
									<xsl:text>:00</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
						<xsl:choose>
							<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmQual='AA'">
								<xsl:attribute name="Actual">
									<xsl:variable name="zeroes">0000</xsl:variable>
									<xsl:variable name="Arrtime">
										<xsl:value-of select="substring(string($zeroes),1,4-string-length(following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm))" />
										<xsl:value-of select="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm" />
									</xsl:variable>
									<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,1,4)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,5,2)" />
									<xsl:text>-</xsl:text>
									<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,7,2)" />
									<xsl:text>T</xsl:text>
									<xsl:value-of select="substring($Arrtime,1,2)" />
									<xsl:text>:</xsl:text>
									<xsl:value-of select="substring($Arrtime,3,2)" />
									<xsl:text>:00</xsl:text>
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</ArrivalDateTime>
					<OperationTimes>
						<xsl:choose>
							<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmQual!='DX'">
								<!-- DEPARTURE -->
								<OperationTime>
									<xsl:attribute name="Time">
										<xsl:variable name="zeroes">0000</xsl:variable>
										<xsl:variable name="deptime">
											<xsl:choose>
												<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/Tm!='0'">
													<xsl:value-of select="substring(string($zeroes),1,4-string-length(CityQual/StartQual/TmQualAry/TmQualInfo/Tm))" />
													<xsl:value-of select="CityQual/StartQual/TmQualAry/TmQualInfo/Tm" />
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="substring(string($zeroes),1,4-string-length(CityQual/StartQual/Tm))" />
													<xsl:value-of select="CityQual/StartQual/Tm" />
												</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:value-of select="substring(CityQual/StartQual/Dt,1,4)" />
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(CityQual/StartQual/Dt,5,2)" />
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(CityQual/StartQual/Dt,7,2)" />
										<xsl:text>T</xsl:text>
										<xsl:value-of select="substring($deptime,1,2)" />
										<xsl:text>:</xsl:text>
										<xsl:value-of select="substring($deptime,3,2)" />
										<xsl:text>:00</xsl:text>
									</xsl:attribute>
									<xsl:attribute name="OperationType">Departure</xsl:attribute>
									<xsl:attribute name="TimeType">
										<xsl:choose>
											<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='AD'">
												<xsl:text>Actual</xsl:text>
											</xsl:when>
											<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/TmQual='ED'">
												<xsl:text>Estimated</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>Scheduled</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<ReasonCode>
										<xsl:if test="CityQual/StartQual/TmQualAry/TmQualInfo/Reason != ''">
											<xsl:attribute name="Code">
												<xsl:value-of select="CityQual/StartQual/TmQualAry/TmQualInfo/Reason" />
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="CodeContext">
											<xsl:choose>
												<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/VarianceInd='E'">
													<xsl:text>Early</xsl:text>
												</xsl:when>
												<xsl:when test="CityQual/StartQual/TmQualAry/TmQualInfo/VarianceInd='L'">
													<xsl:text>Late</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>Not applicable </xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:if test="CityQual/StartQual/TmQualAry/TmQualInfo/TmVariance != ''">
											<xsl:attribute name="Quantity">
												<xsl:value-of select="CityQual/StartQual/TmQualAry/TmQualInfo/TmVariance" />
											</xsl:attribute>
										</xsl:if>
									</ReasonCode>
								</OperationTime>
								<!-- ARRIVAL -->
								<OperationTime>
									<xsl:attribute name="Time">
										<xsl:variable name="zeroes">0000</xsl:variable>
										<xsl:variable name="Arrtime">
											<xsl:choose>
												<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm!='0'">
													<xsl:value-of select="substring(string($zeroes),1,4-string-length(following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm))" />
													<xsl:value-of select="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Tm" />
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="substring(string($zeroes),1,4-string-length(following-sibling::Item/CityQual/EndQual/Tm))" />
													<xsl:value-of select="following-sibling::Item/CityQual/EndQual/Tm" />
												</xsl:otherwise>
											</xsl:choose>
										</xsl:variable>
										<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,1,4)" />
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,5,2)" />
										<xsl:text>-</xsl:text>
										<xsl:value-of select="substring(following-sibling::Item/CityQual/EndQual/Dt,7,2)" />
										<xsl:text>T</xsl:text>
										<xsl:value-of select="substring($Arrtime,1,2)" />
										<xsl:text>:</xsl:text>
										<xsl:value-of select="substring($Arrtime,3,2)" />
										<xsl:text>:00</xsl:text>
									</xsl:attribute>
									<xsl:attribute name="OperationType">Arrival</xsl:attribute>
									<xsl:attribute name="TimeType">
										<xsl:choose>
											<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmQual='AA'">
												<xsl:text>Actual</xsl:text>
											</xsl:when>
											<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmQual='EB'">
												<xsl:text>Estimated</xsl:text>
											</xsl:when>
											<xsl:otherwise>
												<xsl:text>Scheduled</xsl:text>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:attribute>
									<ReasonCode>
										<xsl:if test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Reason != ''">
											<xsl:attribute name="Code">
												<xsl:value-of select="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/Reason" />
											</xsl:attribute>
										</xsl:if>
										<xsl:attribute name="CodeContext">
											<xsl:choose>
												<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/VarianceInd='E'">
													<xsl:text>Early</xsl:text>
												</xsl:when>
												<xsl:when test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/VarianceInd='L'">
													<xsl:text>Late</xsl:text>
												</xsl:when>
												<xsl:otherwise>
													<xsl:text>Not applicable </xsl:text>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
										<xsl:if test="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmVariance != ''">
											<xsl:attribute name="Quantity">
												<xsl:value-of select="following-sibling::Item/CityQual/EndQual/TmQualAry/TmQualInfo/TmVariance" />
											</xsl:attribute>
										</xsl:if>
									</ReasonCode>
								</OperationTime>
							</xsl:when>
							<xsl:otherwise>
								<OperationTime />
							</xsl:otherwise>
						</xsl:choose>
					</OperationTimes>
				</FlightLegInfo>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="FltQual" mode="FltNum">
		<xsl:value-of select="FltNum" />
	</xsl:template>
	<xsl:template match="FltQual" mode="AirCode">
		<MarketingAirline>
			<xsl:attribute name="Code">
				<xsl:value-of select="AirV" />
			</xsl:attribute>
		</MarketingAirline>
	</xsl:template>
	<!-- ======================================================================= -->
	<!-- Flight Information                                                      -->
	<!-- ======================================================================= -->
	<xsl:template match="Item" mode="X">
		<xsl:if test="FltTextQual/MsgAry/Msg/Text!=''">
			<MessageText>
				<xsl:value-of select="FltTextQual/MsgAry/Msg/Text" />
			</MessageText>
		</xsl:if>
	</xsl:template>
	<xsl:template match="CityQual" mode="equip">
		<xsl:if test="StartQual/Equip != ''">
			<xsl:apply-templates select="StartQual/Equip" />
		</xsl:if>
	</xsl:template>
	<xsl:template match="CityQual" mode="dep">
		<xsl:if test="StartQual">
			<Time>
				<xsl:apply-templates select="StartQual/Tm" />
			</Time>
			<xsl:apply-templates select="StartQual/Dt" />
			<xsl:apply-templates select="StartQual/Gate" />
		</xsl:if>
	</xsl:template>
	<xsl:template match="CityQual" mode="arr">
		<xsl:if test="EndQual">
			<Time>
				<xsl:apply-templates select="EndQual/Tm" />
			</Time>
			<xsl:apply-templates select="EndQual/Dt" />
			<xsl:apply-templates select="EndQual/Gate" />
		</xsl:if>
	</xsl:template>
	<xsl:template match="CityQual" mode="dreal">
		<xsl:apply-templates select="StartQual/TmQualAry/TmQualInfo" />
	</xsl:template>
	<xsl:template match="CityQual" mode="areal">
		<xsl:apply-templates select="EndQual/TmQualAry/TmQualInfo" />
	</xsl:template>
	<xsl:template match="CityTextQual" mode="dtxt">
		<CityInfo>
			<FromCity>
				<xsl:value-of select="City" />
			</FromCity>
			<xsl:if test="MsgAry/Msg/Text!=''">
				<Text>
					<xsl:value-of select="MsgAry/Msg/Text" />
				</Text>
			</xsl:if>
		</CityInfo>
	</xsl:template>
	<xsl:template match="CityTextQual" mode="atxt">
		<CityInfo>
			<ToCity>
				<xsl:value-of select="City" />
			</ToCity>
			<xsl:if test="MsgAry/Msg/Text!=''">
				<Text>
					<xsl:value-of select="MsgAry/Msg/Text" />
				</Text>
			</xsl:if>
		</CityInfo>
	</xsl:template>
</xsl:stylesheet>
