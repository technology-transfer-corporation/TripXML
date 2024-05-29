<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<!-- 
  ================================================================== 
	v04_Galileo_IssueMCORQ.xsl 													
  ================================================================== 
  Date: 19 Oct 2023 - Kobelev - Initial load.														
  ================================================================== 
  https://support.travelport.com/webhelp/GWS/Content/TRANSACTIONHELP/MiscellaneousChargeOrder_1_0/MiscellaneousChargeOrder_1_0.htm
-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="PCC">
		<xsl:value-of select="TT_IssueMCORQ/POS/Source/@PseudoCityCode"/>
	</xsl:variable>

	<xsl:template match="/">

		<xsl:choose>
			<xsl:when test="TT_IssueMCORQ">
				<TT_IssueMCORQ>
					<xsl:choose>
						<xsl:when test="TT_IssueMCORQ/PNR">
							<xsl:apply-templates select="TT_IssueMCORQ" />
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="TT_IssueMCORQ" />
						</xsl:otherwise>
					</xsl:choose>
				</TT_IssueMCORQ>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="DocProdFareManipulation_29" mode="exchMask" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="TT_IssueMCORQ">
		<PNRRead>
			<PNRBFManagement_53>
				<PNRBFRetrieveMods>
					<PNRAddr>
						<FileAddr/>
						<CodeCheck/>
						<RecLoc>
							<xsl:value-of select="UniqueID/@ID"/>
						</RecLoc>
					</PNRAddr>
				</PNRBFRetrieveMods>
				<!-- -->
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<xsl:choose>
								<xsl:when test="Ticketing/FareNumber!=''">
									<xsl:for-each select="Ticketing/FareNumber">
										<FareNum>
											<xsl:value-of select="."/>
										</FareNum>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<FareNum>1</FareNum>
								</xsl:otherwise>
							</xsl:choose>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>

			</PNRBFManagement_53>
		</PNRRead>

		<PNRCurrentRead>
			<PNRBFManagement_53>
				<PNRBFRetrieveMods>
					<CurrentPNR />
				</PNRBFRetrieveMods>
				<!-- -->
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<xsl:choose>
								<xsl:when test="Ticketing/FareNumber!=''">
									<xsl:for-each select="Ticketing/FareNumber">
										<FareNum>
											<xsl:value-of select="."/>
										</FareNum>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise>
									<FareNum>1</FareNum>
								</xsl:otherwise>
							</xsl:choose>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>

			</PNRBFManagement_53>
		</PNRCurrentRead>

		<ET>
			<PNRBFManagement_53>
				<EndTransactionMods>
					<EndTransactRequest>
						<ETInd>R</ETInd>
						<RcvdFrom>
							<xsl:choose>
								<xsl:when test="POS/Source/@AgentSine != ''">
									<xsl:value-of select="POS/Source/@AgentSine"/>
								</xsl:when>
								<xsl:otherwise>TRIPXML</xsl:otherwise>
							</xsl:choose>
						</RcvdFrom>
					</EndTransactRequest>
				</EndTransactionMods>
			</PNRBFManagement_53>
		</ET>

		<MCOS>
			<xsl:for-each select="MCOs/MCOMask" >
				<xsl:apply-templates select="." />
			</xsl:for-each>
		</MCOS>

		<GetTickets>
			<DocProdFareManipulation_29>
				<TicketingMods>
					<TicketingControl>
						<TransType>MO</TransType>
					</TicketingControl>
					<MCOIssue>
						<IssueInd>Y</IssueInd>
					</MCOIssue>
					<MCONumber>
						<Num />
					</MCONumber>
				</TicketingMods>
			</DocProdFareManipulation_29>
		</GetTickets>

		<ExchangeMCO>
			<DocProdFareManipulation_29>
				<TicketingMods>
					<ElectronicTicketFailed>
						<CancelInd />
						<IssuePaperTkInd>Y</IssuePaperTkInd>
					</ElectronicTicketFailed>
					<FareNumInfo>
						<FareNumAry>
							<FareNum/>
						</FareNumAry>
					</FareNumInfo>
					<ElectronicTicketFailed>
						<CancelInd/>
						<IssuePaperTkInd>Y</IssuePaperTkInd>
					</ElectronicTicketFailed>
					<AssocPsgrs>
						<PsgrAry>
							<Psgr/>
						</PsgrAry>
					</AssocPsgrs>
					<TicketingControl>
						<TransType>TK</TransType>
					</TicketingControl>
					<OtherFOP>
						<FOPID>13</FOPID>
						<Type>0</Type>
						<PmtCrncy>USD</PmtCrncy>
						<AddlDataIDAry>
							<AddlDataID>
								<ID>9</ID>
								<Dt/>
							</AddlDataID>
						</AddlDataIDAry>
					</OtherFOP>
				</TicketingMods>
			</DocProdFareManipulation_29>
		</ExchangeMCO>

	</xsl:template>

	<xsl:template match="DocProdFareManipulation_29" mode="exchMask">
		<DocProdFareManipulation_29>
			<TicketingMods>
				<xsl:copy-of select="Ticketing/FareNumInfo"/>
				<xsl:copy-of select="Ticketing/TicketingControl"/>
				<xsl:copy-of select="Ticketing/AssocPsgrs"/>
				<xsl:copy-of select="Ticketing/PassengerInfo"/>

				<xsl:copy-of select="Ticketing/ExcRefundTktCoupon"/>

				<xsl:copy-of select="Ticketing/ExchangeOldFareDataBase"/>
				<xsl:copy-of select="Ticketing/TaxInfo[TaxType='OLD']"/>
				<xsl:copy-of select="Ticketing/ExcOrigEchangedInfo"/>
				<xsl:copy-of select="Ticketing/ExcNewFareDataBase"/>
				<xsl:copy-of select="Ticketing/TaxInfo[TaxType='NEW']"/>
			</TicketingMods>
		</DocProdFareManipulation_29>

	</xsl:template>

	<xsl:template match="MCOMask">
		<MiscellaneousChargeOrder_1_0>
			<MCOProcessingMods>
				<MCOTicketData>
					<TktIssueInd>O</TktIssueInd>
				</MCOTicketData>
				<MCOIssue>
					<IssueInd>N</IssueInd>
				</MCOIssue>
				<MCOReasonCode>
					<ReasonCode>J</ReasonCode>
				</MCOReasonCode>
				<MCOMainData>
					<PsgrName>
						<xsl:value-of select="PassengerName"/>
					</PsgrName>
					<TourOperator>
						<xsl:value-of select="To"/>
					</TourOperator>
					<Location>
						<xsl:value-of select="AT" />
					</Location>
					<ValidFor>DEPOST FOR FUTURE TRANSPORTATION</ValidFor>
					<RelatedTktNum/>
					<Commission>0.00</Commission>
					<MCOAmt>
						<xsl:value-of select="Amount" />
					</MCOAmt>
					<Currency>
						<xsl:value-of select="CurrencyCode" />
					</Currency>
					<PlatingCarrier>
						<xsl:value-of select="TicketingAirlineCode" />
					</PlatingCarrier>
				</MCOMainData>
				<xsl:choose>
					<xsl:when test="CreditCard != ''">
						<xsl:apply-templates select="." mode="cc" />
					</xsl:when>
					<xsl:when test="Check = 'true' or Cheque = 'true'">
						<xsl:apply-templates select="." mode="check" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:apply-templates select="." mode="cash" />
					</xsl:otherwise>
				</xsl:choose>
			</MCOProcessingMods>
		</MiscellaneousChargeOrder_1_0>
	</xsl:template>

	<xsl:template match="MCOMask" mode="cc">
		<CreditCardFOP>
			<ID>6</ID>
			<Type>1</Type>
			<!--
			<Currency>
				<xsl:value-of select="CurrencyCode" />
			</Currency>
			<Amt>
				<xsl:value-of select="Amount" />
			</Amt>
			-->
			<ExpDt>
				<xsl:value-of select="Expiration" />
			</ExpDt>
			<!--
			<ApprovalInd>A</ApprovalInd>
			<AcceptOverride>Y</AcceptOverride>
			<ValidationBypassReq>N</ValidationBypassReq>
			<TransType/>			
			-->
			<Vnd>
				<xsl:value-of select="Vendor" />
			</Vnd>
			<Acct>
				<xsl:value-of select="CreditCard" />
			</Acct>
		</CreditCardFOP>
	</xsl:template>

	<xsl:template match="MCOMask" mode="check">
		<CheckFOP>
			<ID>
				<xsl:value-of select="Id" />
			</ID>
			<Type>2</Type>

			<!--
			<Currency>
				<xsl:value-of select="CurrencyCode" />
			</Currency>
			<Amt>
				<xsl:value-of select="Amount" />
			</Amt>
			-->
		</CheckFOP>
	</xsl:template>

	<xsl:template match="MCOMask" mode="cash">
		<OtherFOP>
			<FOPID>01</FOPID>
			<Type>2</Type>

			<!--
			<PmtCrncy>
				<xsl:value-of select="CurrencyCode" />
			</PmtCrncy>
			<Amt>
				<xsl:value-of select="Amount" />
			</Amt>
			-->

		</OtherFOP>
	</xsl:template>

	<!--
  ############################################################
  ## Template to tokenize strings                           ##
  ############################################################
-->
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
