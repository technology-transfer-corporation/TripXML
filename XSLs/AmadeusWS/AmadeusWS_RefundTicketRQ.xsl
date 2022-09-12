<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_RefundTicketRQ.xsl												-->
<!-- ================================================================== -->
<!-- Date: 24 Jul 2013 - Rastko															-->
<!-- ================================================================== -->

	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="TT_RefundTicketRQ"/>
	</xsl:template>
	<xsl:template match="TT_RefundTicketRQ">
		<Ticket>
			<Cryptic>
				<Command_Cryptic>
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<textStringDetails>
							<xsl:value-of select="'TRF'" />
							<xsl:value-of select="concat(Ticket/TicketNumber,'/FULL/CP')"/>
							<xsl:choose>
								<xsl:when test="Ticket/Penalty/@DecimalPlaces='2'">
									<xsl:value-of select="substring(Ticket/Penalty/@Amount,1,string-length(Ticket/Penalty/@Amount) - 3)"/>
									<xsl:value-of select="string('.')"/>
									<xsl:value-of select="substring(Ticket/Penalty/@Amount,string-length(Ticket/Penalty/@Amount) - 2)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="Ticket/Penalty/@Amount"/>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:value-of select="'A'" />
						</textStringDetails>
					</longTextString>
				</Command_Cryptic>
			</Cryptic>
			<Init>
				<DocRefund_InitRefund>
				    <ticketNumberGroup>
				        <documentNumberDetails>
				            <documentDetails>
				                <number>
				                	<xsl:value-of select="translate(Ticket/TicketNumber,'-','')"/>
				                </number>
				            </documentDetails>
				        </documentNumberDetails>
				    </ticketNumberGroup>
				</DocRefund_InitRefund>
			</Init>
			<Update>
				<DocRefund_UpdateRefund>
					<userIdentification>
						<originIdentification>
							<originatorId>
								<xsl:value-of select="IATACode"/>
							</originatorId>
						</originIdentification>
					</userIdentification>
					<dateTimeInformation>
						<businessSemantic>710</businessSemantic>
						<dateTime>
							<year>
								<xsl:value-of select="substring(Ticket/TicketNumber/@DateOfIssue,7)"/>
							</year>
							<month>
								<xsl:value-of select="substring(Ticket/TicketNumber/@DateOfIssue,4,2)"/>
							</month>
							<day>
								<xsl:value-of select="substring(Ticket/TicketNumber/@DateOfIssue,1,2)"/>
							</day>
						</dateTime>
					</dateTimeInformation>
					<dateTimeInformation>
						<businessSemantic>DR</businessSemantic>
						<dateTime>
							<year>
								<xsl:value-of select="substring(Ticket/TicketNumber/@DateOfRefund,7)"/>
							</year>
							<month>
								<xsl:value-of select="substring(Ticket/TicketNumber/@DateOfRefund,4,2)"/>
							</month>
							<day>
								<xsl:value-of select="substring(Ticket/TicketNumber/@DateOfRefund,1,2)"/>
							</day>
						</dateTime>
					</dateTimeInformation>
					<ticket>
						<ticketInformation>
							<documentDetails>
								<number>
									<xsl:value-of select="translate(Ticket/TicketNumber,'-','')"/>
								</number>
							</documentDetails>
						</ticketInformation>
						<!--ticketGroup>
							<couponInformationDetails>
								<couponDetails>
									<cpnNumber>1</cpnNumber>
									<cpnStatus>RF</cpnStatus>
								</couponDetails>
							</couponInformationDetails>
						</ticketGroup-->
					</ticket>
					<monetaryInformation>
						<monetaryDetails>
							<typeQualifier>RFT</typeQualifier>
							<amount>
								<xsl:choose>
									<xsl:when test="Ticket/Refund/@DecimalPlaces='2'">
										<xsl:value-of select="substring(Ticket/Refund/@Amount,1,string-length(Ticket/Refund/@Amount) - 3)"/>
										<xsl:value-of select="string('.')"/>
										<xsl:value-of select="substring(Ticket/Refund/@Amount,string-length(Ticket/Refund/@Amount) - 2)"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="Ticket/Refund/@Amount"/>
									</xsl:otherwise>
								</xsl:choose>
							</amount>
							<currency>
								<xsl:value-of select="Ticket/Refund/@Currency"/>
							</currency>
						</monetaryDetails>
						<xsl:if test="Ticket/Penalty/@Amount!=''">
							<otherMonetaryDetails>
								<typeQualifier>CP</typeQualifier>
								<amount>
									<xsl:choose>
										<xsl:when test="Ticket/Penalty/@DecimalPlaces='2'">
											<xsl:value-of select="substring(Ticket/Penalty/@Amount,1,string-length(Ticket/Penalty/@Amount) - 3)"/>
											<xsl:value-of select="string('.')"/>
											<xsl:value-of select="substring(Ticket/Penalty/@Amount,string-length(Ticket/Penalty/@Amount) - 2)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="Ticket/Penalty/@Amount"/>
										</xsl:otherwise>
									</xsl:choose>
								</amount>
								<currency>
									<xsl:value-of select="Ticket/Penalty/@Currency"/>
								</currency>
							</otherMonetaryDetails>
						</xsl:if>
						<!--otherMonetaryDetails>
							<typeQualifier>RFU</typeQualifier>
							<amount>0.00</amount>
							<currency>CHF</currency>
						</otherMonetaryDetails>
						<otherMonetaryDetails>
							<typeQualifier>FRF</typeQualifier>
							<amount>102.00</amount>
							<currency>CHF</currency>
						</otherMonetaryDetails>
						<otherMonetaryDetails>
							<typeQualifier>B</typeQualifier>
							<amount>102.00</amount>
							<currency>CHF</currency>
						</otherMonetaryDetails>
						<otherMonetaryDetails>
							<typeQualifier>TXT</typeQualifier>
							<amount>177.50</amount>
							<currency>CHF</currency>
						</otherMonetaryDetails-->
					</monetaryInformation>
				</DocRefund_UpdateRefund>
			</Update>
			<Process>
				<DocRefund_ProcessRefund>
					<actionDetails>
						<statusDetails>
							<indicator>IRN</indicator>
						</statusDetails>
					</actionDetails>
				</DocRefund_ProcessRefund>
			</Process>
		</Ticket>
	</xsl:template>
</xsl:stylesheet>
