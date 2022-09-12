<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_VoidTicketRQ.xsl												-->
<!-- ================================================================== -->
<!-- Date: 06 May 2014 - Rastko - process tickets with or without a - in the ticket number	-->
<!-- Date: 06 Mar 2014 - Rastko - added support to new cancel document message		-->
<!-- Date: 10 Feb 2014 - Rastko - hard coded DTT OID in cryptic entry			-->
<!-- Date: 22 Jan 2014 - Rastko - implemented cryptic ticket void as temp solution 		-->
<!-- Date: 3 Feb 2012 - Shashin														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="username" select="TT_VoidTicketRQ/POS/TPA_Extensions/Provider/Userid"/>
	<xsl:variable name="pcc" select="TT_VoidTicketRQ/POS/Source/@PseudoCityCode"/>
	<xsl:template match="/">
		<xsl:apply-templates select="TT_VoidTicketRQ"/>
	</xsl:template>
	<xsl:template match="TT_VoidTicketRQ">
		<VoidTickets>
			<xsl:if test="$username!='OneTwoTrip'">
				<xsl:if test="UniqueID/@ID!=''">
					<PNR>
						<PNR_RetrieveByRecLoc>
							<sbrRecLoc>
								<reservation>
									<controlNumber>
										<xsl:value-of select="UniqueID/@ID" />
									</controlNumber>
								</reservation>
							</sbrRecLoc>
						</PNR_RetrieveByRecLoc>
					</PNR>
				</xsl:if>
			</xsl:if>
			<xsl:for-each select="Tickets/TicketNumber">
				<Ticket>
					<xsl:choose>
						<xsl:when test="$username='OneTwoTrip'">
							<RTT>
								<Ticket_ProcessETicket>
									<msgActionDetails>
										<messageFunctionDetails>
											<messageFunction>131</messageFunction>
										</messageFunctionDetails>
									</msgActionDetails>
									<ticketInfoGroup>
										<ticket>
											<documentDetails>
												<number>
													<xsl:value-of select="translate(.,'-','')"/>
												</number>
											</documentDetails>
										</ticket>
									</ticketInfoGroup>
								</Ticket_ProcessETicket>
							</RTT>
							<VDT>
								<Ticket_CancelDocument>
									<documentNumberDetails>
										<documentDetails>
											<number>
												<xsl:value-of select="substring(translate(.,'-',''),4)"/>
											</number>
										</documentDetails>
									</documentNumberDetails>
									<stockProviderDetails>
										<officeSettingsDetails>
											<marketIataCode>
												<xsl:choose>
													<xsl:when test="$pcc='MOWR228JQ'"><xsl:value-of select="'RU'"/></xsl:when>
													<xsl:when test="$pcc='MOWR222QD'"><xsl:value-of select="'RU'"/></xsl:when>
													<xsl:when test="$pcc='KIVU23313'"><xsl:value-of select="'UA'"/></xsl:when>
													<xsl:when test="$pcc='IEVU22311'"><xsl:value-of select="'UA'"/></xsl:when>
													<xsl:when test="$pcc='FMOL121BW'"><xsl:value-of select="'DE'"/></xsl:when>
													<xsl:when test="$pcc='BERL122L5'"><xsl:value-of select="'DE'"/></xsl:when>
													<xsl:when test="$pcc='NYC1S21GQ'"><xsl:value-of select="'US'"/></xsl:when>
												</xsl:choose>
											</marketIataCode>
										</officeSettingsDetails>
									</stockProviderDetails>
									<targetOfficeDetails>
										<originatorDetails>
											<inHouseIdentification2>
												<xsl:choose>
													<xsl:when test="$pcc='NYC1S21GQ'"><xsl:value-of select="'FLL1S212V'"/></xsl:when>
													<xsl:otherwise><xsl:value-of select="$pcc"/></xsl:otherwise>
												</xsl:choose>
											</inHouseIdentification2>
										</originatorDetails>
									</targetOfficeDetails>
								</Ticket_CancelDocument>
							</VDT>
						</xsl:when>
						<xsl:otherwise>
							<CMD>
								<Command_Cryptic>
									<messageAction>
										<messageFunctionDetails>
											<messageFunction>M</messageFunction>
										</messageFunctionDetails>
									</messageAction>
									<longTextString>
										<textStringDetails>
											<xsl:value-of select="'TRDC/LTKT'"/>
											<xsl:if test="../../POS/Source/@PseudoCityCode='NYC1S21GQ'">
												<xsl:value-of select="'/SOF-FLL1S212V'"/>
											</xsl:if>
										</textStringDetails>
									</longTextString>
								</Command_Cryptic>
							</CMD>
						</xsl:otherwise>
					</xsl:choose>
				</Ticket>
			</xsl:for-each>
		</VoidTickets>
	</xsl:template>
</xsl:stylesheet>
