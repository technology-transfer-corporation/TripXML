<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:ttVB="urn:ttVB" exclude-result-prefixes="msxsl ttVB" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_DisplayTicketRQ.xsl												-->
<!-- ================================================================== -->
<!-- Date: 07 Aug 2012 - Rastko - new file												-->
<!-- ================================================================== -->

	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="TT_DisplayTicketRQ"/>
	</xsl:template>
	<xsl:template match="TT_DisplayTicketRQ">
		<DisplayTickets>
			<xsl:for-each select="Tickets/TicketNumber">
				<Ticket>
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
				</Ticket>
			</xsl:for-each>
		</DisplayTickets>
	</xsl:template>
</xsl:stylesheet>
