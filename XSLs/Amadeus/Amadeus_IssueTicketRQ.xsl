<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Amadeus_IssueTicketRQ.xsl														-->
	<!-- ================================================================== -->
	<!-- Date:18 Mar 2009 - Rastko															-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<TT_IssueTicketRQ>
			<xsl:apply-templates select="TT_IssueTicketRQ" />
		</TT_IssueTicketRQ>
	</xsl:template>
	
	<xsl:template match="TT_IssueTicketRQ">
		<PNRRead>
			<PoweredPNR_Retrieve>
				<settings>
					<options>
						<optionCode>51</optionCode>
					</options>
				</settings>
				<retrievalFacts>
					<retrieve>
						<type>2</type>
						<office></office>
					</retrieve>
					<reservationOrProfileIdentifier>
						<reservation>
							<controlNumber>
								<xsl:value-of select="UniqueID/@ID" />
							</controlNumber>
						</reservation>
					</reservationOrProfileIdentifier>
				</retrievalFacts>
			</PoweredPNR_Retrieve>
		</PNRRead>
		<xsl:choose>
			<xsl:when test="Ticketing/@TicketType = 'eTicket' or Ticketing/@IssueMCO = 'true' or Ticketing/@IssueInvoice = 'true' or Ticketing/Notification/@ByEmail = 'true' or Ticketing/Notification/@ByFax = 'true'">
				<TicketCryptic>
					<xsl:choose>
						<xsl:when test="Ticketing/Notification/@ByEmail = 'true'">
							<Cryptic_GetScreen_Query>
								<Command>
									<xsl:text>TTP/ET/ITR-EMLA</xsl:text>
								</Command>
							</Cryptic_GetScreen_Query>
						</xsl:when>
						<xsl:when test="Ticketing/Notification/@ByFax = 'true'">
							<Cryptic_GetScreen_Query>
								<Command>
									<xsl:text>TTP/ET/ITR-FAXA</xsl:text>
								</Command>
							</Cryptic_GetScreen_Query>
						</xsl:when>
						<xsl:when test="Ticketing/@TicketType = 'eTicket' or Ticketing/@IssueMCO = 'true' or Ticketing/@IssueInvoice = 'true'">
							<Cryptic_GetScreen_Query>
								<Command>
									<xsl:text>TTP</xsl:text>
									<xsl:if test="Ticketing/@IssueInvoice = 'true'">/INV</xsl:if>
									<xsl:if test="Ticketing/@TicketType = 'eTicket'">/ET</xsl:if>
									<xsl:if test="Ticketing/@IssueMCO = 'true'">/TTM</xsl:if>
								</Command>
							</Cryptic_GetScreen_Query>
						</xsl:when>
					</xsl:choose>
				</TicketCryptic>
			</xsl:when>
			<xsl:otherwise>
				<Ticket>
					<DocPrt_IssueTicket_Query>
						<BackValidation/>
						<BackValidationOther/>
						<BackOfficeAIR/>
						<BoardingPass_OPTAT/>
						<ETktOverride/>
						<EtoPTktOverride/>
						<Exchange/>
						<FillerStrip_OPTAT/>
						<Invoice/>
						<Itinerary/>
						<MiniItinerary/>
						<NoBoardingPass_ATB/>
						<OpenReturn/>
						<PsgrSelect/>
						<PsgrTypeADT>T</PsgrTypeADT>
						<PsgrTypeINF/>
						<PastDateTSTOverride/>
						<RemovePNRchangeflag/>
						<PrinterDestination/>
						<RedisplayPNR>T</RedisplayPNR>
						<RepriceTST/>
						<SatelliteTktPrinter/>
						<SegmentSelect/>
						<StockRange/>
						<TSTselect/>
						<Validation/>
						<WithholdTaxes/>
						<WithholdSurcharges/>
						<TaxExemptFlag/>
						<SpecificTaxExempt/>
					</DocPrt_IssueTicket_Query>
				</Ticket>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
