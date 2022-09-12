<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_IssueTicketRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 03 Dec 2012 - Rastko - tested code specific to Nexus customer			-->
	<!-- Date: 06 Aug 2012 - Rastko - upgraded to same level as version 3 of message		-->
	<!-- Date: 31 Jan 2011 - Rastko - add option MIR to printing						-->
	<!-- Date: 21 May 2010 - Rastko - suppress invoice from printing					-->
	<!-- Date: 31 Mar 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="PCC"><xsl:value-of select="TT_IssueTicketRQ/POS/Source/@PseudoCityCode"/></xsl:variable>
	
  <!-- 'User variable is not defined in local xsl'-->
  <xsl:variable name="User"><xsl:value-of select="TT_IssueTicketRQ/POS/Source/RequestorID/@ID"/></xsl:variable>
	<!--=============================================-->
  
  
  <xsl:template match="/">
		<TT_IssueTicketRQ>
			<xsl:apply-templates select="TT_IssueTicketRQ" />
		</TT_IssueTicketRQ>
	</xsl:template>
	<!-- ************************************************************************************************************-->
	<xsl:template match="TT_IssueTicketRQ">
		<CheckPRT>
			<TicketPrinterLinkage_1_0>
				<LinkageDisplayMods>
					<PseudoCity>
						<PCC></PCC>
					</PseudoCity>
				</LinkageDisplayMods>
			</TicketPrinterLinkage_1_0>
		</CheckPRT>
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
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<xsl:choose>
								<xsl:when test="Ticketing/FareNumber!=''">
									<xsl:for-each select="Ticketing/FareNumber">
										<FareNum><xsl:value-of select="."/></FareNum>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise><FareNum>1</FareNum></xsl:otherwise>
							</xsl:choose>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>
			</PNRBFManagement_53>
		</PNRRead>
    
    <PNRCurrentRead>
			<PNRBFManagement_53>
				<PNRBFRetrieveMods><CurrentPNR /></PNRBFRetrieveMods>
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<xsl:choose>
								<xsl:when test="Ticketing/FareNumber!=''">
									<xsl:for-each select="Ticketing/FareNumber">
										<FareNum><xsl:value-of select="."/></FareNum>
									</xsl:for-each>
								</xsl:when>
								<xsl:otherwise><FareNum>1</FareNum></xsl:otherwise>
							</xsl:choose>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>
			</PNRBFManagement_53>
		</PNRCurrentRead>
    
		<!--xsl:for-each select="Ticketing/FareNumber">
			<CrypticTMU>
				<xsl:text>TMU</xsl:text>
				<xsl:value-of select="."/>
				<xsl:text>Z0</xsl:text>
			</CrypticTMU>
		</xsl:for-each-->
    
		<!--VerifyATFQ>
			<PNRBFManagement_17>
				<FareQuoteRepriceMods>
					<ItemAry>
						<Item>
							<BlkInd>A </BlkInd>
							<SpecificQual>
								<RelFare>0</RelFare>
							</SpecificQual>
						</Item>
					</ItemAry>
				</FareQuoteRepriceMods>
				<FareQuoteVerifyMods>
					<ItemAry>
						<Item>
							<BlkInd>A </BlkInd>
							<SpecificQual>
								<RelFare>0</RelFare>
							</SpecificQual>
						</Item>
					</ItemAry>
				</FareQuoteVerifyMods>
			</PNRBFManagement_17>
		</VerifyATFQ-->
    
    <!-- below given if condition was not in the local xsl. But content was there--> 
		<xsl:if test="$User = 'Nexus'">
    <!--=========================================================================-->  
			
      <!--
      <xsl:for-each select="Ticketing/FareNumber">
				<CrypticTMU>
					<xsl:text>TMU</xsl:text>
					<xsl:value-of select="."/>
					<xsl:text>F</xsl:text>
					<xsl:value-of select="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
					<xsl:text>*D</xsl:text>
					<xsl:value-of select="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate"/>
					<xsl:text>*E00*A</xsl:text>
					<xsl:value-of select="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ConfirmationNumber"/>
					<xsl:if test="../OrderNumber != ''">
						<xsl:text>*C</xsl:text>
						<xsl:value-of select="../OrderNumber"/>
					</xsl:if>
				</CrypticTMU>
			</xsl:for-each>
      -->
      
			<xsl:if test="Ticketing/BookingPCC != ''">
				<CrypticRULA>RULA/BANK</CrypticRULA>
			</xsl:if>
		</xsl:if>
		<!--xsl:if test="Ticketing/FareNumber!='' or Ticketing/BookingPCC != ''"-->
			<ET>			
				<PNRBFManagement_53>
          <!--EndTransactionMods block has been completedly commented out-->          
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
          <!--============================================================--> 
            
					</EndTransactionMods>					
					<!--FareRedisplayMods>
						<DisplayAction>
							<Action>D</Action>
						</DisplayAction>
						<FareNumInfo>
							<FareNumAry>
								<xsl:for-each select="Ticketing/FareNumber">
									<FareNum><xsl:value-of select="."/></FareNum>
								</xsl:for-each>
							</FareNumAry>
						</FareNumInfo>
					</FareRedisplayMods-->
				</PNRBFManagement_53>
			</ET>
		<!--/xsl:if-->
    
    <!--below given if condition was not there in the local xsl-->
		<xsl:if test="$User = 'Nexus'">
    <!--=======================================================-->
			<xsl:for-each select="Ticketing/FareNumber">
				<CrypticFF>
					<xsl:text>*FF</xsl:text>
					<xsl:value-of select="."/>
				</CrypticFF>
			</xsl:for-each>
		</xsl:if>
    
    <!--=======================================================-->
		<Ticket>
			<DocProdFareManipulation_29>        
        <!--below given entire TicketingMods block was commented in local xsl-->        
				<TicketingMods>
					<DocumentSelect>
						<TkOnlyInd>Y</TkOnlyInd>
						<ETInd>
							<xsl:choose>
								<xsl:when test="Ticketing/@TicketType = 'Paper'">P</xsl:when>
								<xsl:when test="Ticketing/@TicketType = 'eTicket'">Y</xsl:when>
							</xsl:choose>
						</ETInd>
						<xsl:if test="Ticketing/@IssueInvoice = 'true'">
							<MIRInd>N</MIRInd>
						</xsl:if>
					</DocumentSelect>
					<ElectronicTicketFailed>
						<CancelInd>Y</CancelInd>
						<!--
            <IssuePaperTkInd></IssuePaperTkInd>
						<IssuePaperTkToSTP></IssuePaperTkToSTP>
            -->
						<STPlocation></STPlocation>
					</ElectronicTicketFailed>
          
          <!--the below given if condition was not given in local xsl file. Content was there-->
					<xsl:if test="$User = 'Nexus'">
						<FareNumInfo>
							<FareNumAry>
								<xsl:for-each select="Ticketing/FareNumber">
									<FareNum><xsl:value-of select="."/></FareNum>
								</xsl:for-each>
							</FareNumAry>
						</FareNumInfo>
					</xsl:if>
					<TicketingControl>
						<TransType>TK</TransType>
					</TicketingControl>
				</TicketingMods>        
       <!--==================================================================================--> 			
      </DocProdFareManipulation_29>
		</Ticket>
		<GetTickets>
			<DocProdFareManipulation_29>
				<TicketNumbersMods/>
			</DocProdFareManipulation_29>
		</GetTickets>
	</xsl:template>
	
	<xsl:template match="PaymentDetail">
		<xsl:if test="PaymentCard/@CardNumber != '' or PaymentCard/@DirectBill/@DirectBill_ID != ''">
			<ChangeFOP>
				<PNRBFManagement_53>
					<PNRBFSecondaryBldChgMods>
						<ItemAry>
							<Item>
								<DataBlkInd>F </DataBlkInd>
								<FOPQual>
									<EditTypeInd>D</EditTypeInd>
									<DelQual>0</DelQual>
								</FOPQual>
							</Item>
							<Item>
								<DataBlkInd>F </DataBlkInd>
								<FOPQual>
									<EditTypeInd>A</EditTypeInd>
									<AddChgQual>
										<xsl:choose>
											<xsl:when test="PaymentCard">
												<TypeInd>2</TypeInd>
												<CCQual>
													<CC><xsl:value-of select="PaymentCard/@CardCode" /></CC>
													<ExpDt><xsl:value-of select="PaymentCard/@ExpireDate" /></ExpDt>
													<Acct><xsl:value-of select="PaymentCard/@CardNumber" /></Acct>
												</CCQual>
											</xsl:when>
											<xsl:when test="DirectBill/@DirectBill_ID='Cash'">
												<TypeInd>1</TypeInd>
												<VarLenQual>
													<FOP>S</FOP>
												</VarLenQual>
											</xsl:when>
											<xsl:when test="DirectBill/@DirectBill_ID='Check'">
												<TypeInd>1</TypeInd>
												<VarLenQual>
													<FOP>CK</FOP>
												</VarLenQual>
											</xsl:when>
										</xsl:choose>
									</AddChgQual>
								</FOPQual>
							</Item>
							<Item>
								<DataBlkInd>E </DataBlkInd>
								<EndQual>
									<EndMark>E</EndMark>
								</EndQual>
							</Item>
						</ItemAry>
					</PNRBFSecondaryBldChgMods>
					<EndTransactionMods>
						<EndTransactRequest>
							<ETInd>R</ETInd>
							<RcvdFrom>
								<xsl:choose>
									<xsl:when test="//TT_IssueTicketRQ/POS/Source/@AgentSine != ''">
										<xsl:value-of select="POS/Source/@AgentSine"/>
									</xsl:when>
									<xsl:otherwise>TRIPXML</xsl:otherwise>
								</xsl:choose>
							</RcvdFrom>
						</EndTransactRequest>
					</EndTransactionMods>
				</PNRBFManagement_53>
			</ChangeFOP>
		</xsl:if>
	</xsl:template>
	
	<xsl:template match="Ticketing">
		<xsl:text>TKPDTDAD</xsl:text>
		<!--xsl:if test="@TicketType = 'eTicket'"><xsl:text>ET</xsl:text></xsl:if-->
		<!--DocProdFareManipulation_4_0>
			<xsl:if test="@TravelerRefNumberRPHList != '' or @FlightRefNumberRPHList != ''">
				<ManualFareUpdateSaveMods>
					<xsl:if test="@TravelerRefNumberRPHList != ''">
						<AssocPsgrs>
							<PsgrAry>
								<xsl:call-template name="SSRPerPax">
									<xsl:with-param name="TRPH"><xsl:value-of select="@TravelerRefNumberRPHList"/></xsl:with-param>
								</xsl:call-template>
							</PsgrAry>
						</AssocPsgrs>
					</xsl:if>
					<xsl:if test="@FlightRefNumberRPHList != ''">
						<AssocSegs>
							<SegNumAry>
								<xsl:call-template name="SSRPerSeg">
									<xsl:with-param name="SRPH"><xsl:value-of select="@FlightRefNumberRPHList"/></xsl:with-param>
								</xsl:call-template>
							</SegNumAry>
						</AssocSegs>
					</xsl:if>
				</ManualFareUpdateSaveMods>
			</xsl:if>
			<TicketingMods>
				<ElectronicTicketFailed>
					<CancelInd>Y</CancelInd>
					<IssuePaperTkInd></IssuePaperTkInd>
					<IssuePaperTkToSTP></IssuePaperTkToSTP>
					<STPlocation></STPlocation>
				</ElectronicTicketFailed>
				<FareNumInfo>
					<FareNumAry>
						<FareNum>1</FareNum>
					</FareNumAry>
				</FareNumInfo-->
    
				<!--xsl:if test="FareNumber != ''">
					<FareNumInfo>
						<FareNumAry>
							<xsl:for-each select="FareNumber">
								<FareNum><xsl:value-of select="."/></FareNum>
							</xsl:for-each>
						</FareNumAry>
					</FareNumInfo>
				</xsl:if-->
    
				<!--xsl:if test="OtherPrinter != ''">
					<PrintTableOverride><xsl:value-of select="OtherPrinter"/></PrintTableOverride>
				</xsl:if>
				<TicketingControl>
					<TransType>TK</TransType>
				</TicketingControl>
			</TicketingMods>
		</DocProdFareManipulation_4_0-->
	</xsl:template>
	
	<xsl:template name="SSRPerPax">
		<xsl:param name="TRPH"/>
		<xsl:if test="string-length($TRPH) != 0">
			<xsl:variable name="tRPH"><xsl:value-of select="substring($TRPH,1,1)"/></xsl:variable>
			<Psgr>
				<LNameNum>0<xsl:value-of select="$tRPH" /></LNameNum>
				<PsgrNum>01</PsgrNum>
				<AbsNameNum>0<xsl:value-of select="$tRPH" /></AbsNameNum>
			</Psgr>
			<xsl:call-template name="SSRPerPax">
				<xsl:with-param name="TRPH"><xsl:value-of select="substring($TRPH,2)"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
	
	<xsl:template name="SSRPerSeg">
		<xsl:param name="SRPH"/>
		<xsl:if test="string-length($SRPH) != 0">
			<xsl:variable name="sRPH"><xsl:value-of select="substring($SRPH,1,1)"/></xsl:variable>
			<SegNum>0<xsl:value-of select="$sRPH" /></SegNum> 
			<xsl:call-template name="SSRPerSeg">
				<xsl:with-param name="SRPH"><xsl:value-of select="substring($SRPH,2)"/></xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>

</xsl:stylesheet>
