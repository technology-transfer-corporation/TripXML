<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Worldspan_IssueTicketSessionedRQ.xsl												-->
	<!-- ================================================================== -->
	<!-- Date: 07 Apr 2014 - Rastko - New file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="TT_IssueTicketRQ"/>
	</xsl:template>
	<xsl:template match="TT_IssueTicketRQ">
		<OTA_AirDemandTicketRQ Version="1" xmlns="http://www.opentravel.org/OTA/2003/05">
			<POS>
				<Source/>
			</POS>
			<DemandTicketDetail ReturnAllTktNbrsInd="true">
				<MessageFunction Function="ET"/>
				<!-- <MessageFunction Function="ItineraryInvoice"/> -->
        <MessageFunction Function="Interface"/>
				<BookingReferenceID Type="14" ID="{UniqueID/@ID}"/>
				<!--PaymentInfo PaymentType="7"/-->
				<xsl:if test="Ticketing/FutureTicket">
					<DocumentInstructions>
						<xsl:for-each select="Ticketing/FutureTicket/Number">
							<DocumentInstruction Number="{.}"/>
						</xsl:for-each>
					</DocumentInstructions>
				</xsl:if>
			</DemandTicketDetail>
		</OTA_AirDemandTicketRQ>
	</xsl:template>
</xsl:stylesheet>
