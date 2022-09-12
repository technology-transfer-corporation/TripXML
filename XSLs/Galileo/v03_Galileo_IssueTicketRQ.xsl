<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
  <!-- 
  ================================================================== 
	v04_Galileo_IssueTicketRQ.xsl 													
	================================================================== 
  Date: 04 Nov 2020 - Kobelev - MIR Printer Status Change.
  Date: 28 Oct 2020 - Kobelev - Printer Status Change Cryptically.
  Date: 27 Oct 2020 - Kobelev - Ticketing and Invoice Printer assignement.
  Date: 27 Oct 2020 - Kobelev - Service Update. Fix FOP and Commission issues.	
	Date: 15 Sep 2009 - Rastko														
	================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="PCC">
    <xsl:value-of select="TT_IssueTicketRQ/POS/Source/@PseudoCityCode"/>
  </xsl:variable>
  <xsl:template match="/">
    <TT_IssueTicketRQ>
      <xsl:apply-templates select="TT_IssueTicketRQ" />
    </TT_IssueTicketRQ>
  </xsl:template>
  <!-- ************************************************************************************************************-->
  <xsl:template match="TT_IssueTicketRQ">

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

    <xsl:if test="Ticketing/@DesignatePrinter='true'">
      <CheckPRT>
        <TicketPrinterLinkage_1_0>
          <LinkageDisplayMods>
            <xsl:if test="Ticketing/TicketingPrinter!=''">
              <PseudoCity>
                <PCC>
                  <xsl:value-of select="$PCC"/>
                </PCC>
              </PseudoCity>
            </xsl:if>
          </LinkageDisplayMods>
        </TicketPrinterLinkage_1_0>
      </CheckPRT>

      <SetPRT>
        <TicketPrinterLinkage_1_0>
          <LinkageUpdateMods>
            <xsl:if test="Ticketing/TicketingPrinter!=''">
              <PrinterParameters>
                <LNIATA>
                  <xsl:value-of select="Ticketing/TicketingPrinter"/>
                </LNIATA>
                <Type>
                  <xsl:value-of select="'T'"/>
                </Type>
              </PrinterParameters>
            </xsl:if>
            <xsl:if test="Ticketing/InvoicePrinter!=''">
              <PrinterParameters>
                <LNIATA>
                  <xsl:value-of select="Ticketing/InvoicePrinter"/>
                </LNIATA>
                <Type>
                  <xsl:value-of select="'I'"/>
                </Type>
              </PrinterParameters>
            </xsl:if>
            <xsl:if test="Ticketing/OtherPrinter!=''">
              <PrinterParameters>
                <LNIATA>
                  <xsl:value-of select="Ticketing/OtherPrinter"/>
                </LNIATA>
                <Type>
                  <xsl:value-of select="'A'"/>
                </Type>
              </PrinterParameters>
            </xsl:if>
          </LinkageUpdateMods>
        </TicketPrinterLinkage_1_0>
      </SetPRT>

      <CrypticSetPRTStatus>
        <xsl:value-of select="concat('HMOM',Ticketing/TicketingPrinter,'-U')" />
      </CrypticSetPRTStatus>
    </xsl:if>

    <!--
    <VerifyATFQ>
			<PNRBFManagement_53>
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
			</PNRBFManagement_53>
		</VerifyATFQ>
    -->

    <!--
    <xsl:for-each select="Ticketing/FareNumber">
      <CrypticTMU>
        <xsl:text>TMU</xsl:text>
        <xsl:value-of select="."/>
        <xsl:text>F</xsl:text>
        <xsl:choose>
          <xsl:when test="../../Fulfillment/PaymentDetails/PaymentDetail/DirectBill">
            <xsl:choose>
              <xsl:when test="../../Fulfillment/PaymentDetails/PaymentDetail/DirectBill/@DirectBill_ID = 'Check'">
                <xsl:text>CK</xsl:text>
              </xsl:when>
              <xsl:otherwise>
                <xsl:text>S</xsl:text>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:when>
          <xsl:when test="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
            <xsl:value-of select="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
            <xsl:text>*D</xsl:text>
            <xsl:value-of select="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate"/>
            <xsl:text>*E00*A</xsl:text>
            <xsl:value-of select="../../Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ConfirmationNumber"/>
          </xsl:when>
        </xsl:choose>

        <xsl:if test="../OrderNumber != ''">
          <xsl:text>*C</xsl:text>
          <xsl:value-of select="../OrderNumber"/>
        </xsl:if>

        <xsl:choose>
          <xsl:when test="../Commission/@Percent=0 and ../Commission/@Amount=0">
            <xsl:text>/Z</xsl:text>
            <xsl:value-of select="../Commission/@Percent"/>
          </xsl:when>
          <xsl:when test="../Commission/@Amount > 0">
            <xsl:text>/ZA</xsl:text>
            <xsl:value-of select="../Commission/@Amount"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>/Z</xsl:text>
            <xsl:value-of select="../Commission/@Percent"/>
          </xsl:otherwise>

        </xsl:choose>

        -->
    <!--xsl:text>/ET</xsl:text-->
    <!--
      </CrypticTMU>
    </xsl:for-each>
    -->

    <xsl:if test="Ticketing/BookingPCC != ''">
      <CrypticRULA>RULA/BANK</CrypticRULA>
    </xsl:if>

    <!--xsl:if test="Ticketing/FareNumber!='' or Ticketing/BookingPCC != ''"-->

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

    <xsl:for-each select="Ticketing/FareNumber">
      <CrypticFF>
        <xsl:text>*FF</xsl:text>
        <xsl:value-of select="."/>
      </CrypticFF>
    </xsl:for-each>

    <Ticket>
      <!--
      <xsl:choose>
				<xsl:when test="POS/Source/@PseudoCityCode='7EV1'">TKP</xsl:when>
				<xsl:otherwise>TKPDTDAD</xsl:otherwise>
			</xsl:choose>
      -->
      <DocProdFareManipulation_29>
        <TicketingMods>
          <DocumentSelect>
            <!--<TkOnlyInd>Y</TkOnlyInd>-->
            <ETInd>
              <xsl:choose>
                <xsl:when test="Ticketing/@TicketType = 'Paper'">P</xsl:when>
                <xsl:when test="Ticketing/@TicketType = 'eTicket'">Y</xsl:when>
              </xsl:choose>
            </ETInd>
          </DocumentSelect>
          <ElectronicTicketFailed>
            <CancelInd>Y</CancelInd>
            <IssuePaperTkInd></IssuePaperTkInd>
            <IssuePaperTkToSTP></IssuePaperTkToSTP>
            <STPlocation></STPlocation>
          </ElectronicTicketFailed>
          <FareNumInfo>
            <FareNumAry>
              <xsl:for-each select="Ticketing/FareNumber">
                <FareNum>
                  <xsl:value-of select="."/>
                </FareNum>
              </xsl:for-each>
            </FareNumAry>
          </FareNumInfo>
          <!--xsl:apply-templates select="Fulfillment/PaymentDetails/PaymentDetail" mode="DocProd"/-->
          <TicketingControl>
            <TransType>TK</TransType>
          </TicketingControl>

          <!-- FOP -->
          <xsl:if test="Fulfillment/PaymentDetails/PaymentDetail/DirectBill">
            <xsl:choose>
              <xsl:when test="Fulfillment/PaymentDetails/PaymentDetail/DirectBill">
                <CheckFOP>
                  <ID>02</ID>
                  <Type>2</Type>
                  <Amt />
                </CheckFOP>
              </xsl:when>
              <xsl:when test="Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
                <CreditCardFOP>
                  <ID>6</ID>
                  <Type>1</Type>
                  <Currency>USD</Currency>
                  <Amt>0</Amt>
                  <ExpDt>
                    <xsl:value-of select="concat(substring-before(Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate, '/'),substring-after(Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@ExpireDate, '/20'))"/>
                  </ExpDt>
                  <TransType>2</TransType>
                  <ApprovalInd/>
                  <AcceptOverride/>
                  <ValidationBypassReq/>
                  <Vnd>
                    <xsl:value-of select="Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardCode"/>
                  </Vnd>
                  <Acct>
                    <xsl:value-of select="Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@CardNumber"/>
                  </Acct>
                  <xsl:if test="Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@EffectiveDate">
                    <AdditionalInfoAry>
                      <AdditionalInfo>
                        <ID>1</ID>
                        <Dt>
                          <xsl:value-of select="Fulfillment/PaymentDetails/PaymentDetail/PaymentCard/@EffectiveDate"/>
                        </Dt>
                      </AdditionalInfo>
                    </AdditionalInfoAry>
                  </xsl:if>
                </CreditCardFOP>
              </xsl:when>
            </xsl:choose>
          </xsl:if>

          <xsl:if test="Ticketing/Commission">
            <!--          -->
            <CommissionMod>
              <xsl:choose>
                <xsl:when test="Ticketing/Commission/@Amount">
                  <Amt>
                    <xsl:value-of select="Ticketing/Commission/@Amount"/>
                  </Amt>
                </xsl:when>
                <xsl:otherwise>
                  <Percent>
                    <xsl:number value="Ticketing/Commission/@Percent" format="01"/>
                  </Percent>
                </xsl:otherwise>
              </xsl:choose>
            </CommissionMod>
          </xsl:if>
          <xsl:if test="Ticketing/@TravelerRefNumberRPHList != ''">
            <AssocPsgrs>
              <PsgrAry>
                <xsl:call-template name="SSRPerPax">
                  <xsl:with-param name="TRPH">
                    <xsl:value-of select="Ticketing/@TravelerRefNumberRPHList"/>
                  </xsl:with-param>
                </xsl:call-template>
              </PsgrAry>
            </AssocPsgrs>
          </xsl:if>
        </TicketingMods>
      </DocProdFareManipulation_29>
    </Ticket>

    <GetTickets>
      <DocProdFareManipulation_29>
        <TicketNumbersMods/>
      </DocProdFareManipulation_29>
    </GetTickets>
  </xsl:template>

  <xsl:template match="PaymentDetail" mode="DocProd">
    <CreditCardFOP>
      <ID>6</ID>
      <Type>1</Type>
      <Currency/>
      <Amt>0</Amt>
      <ExpDt>
        <xsl:value-of select="PaymentCard/@ExpireDate" />
      </ExpDt>
      <TransType/>
      <ApprovalInd/>
      <AcceptOverride/>
      <ValidationBypassReq>N</ValidationBypassReq>
      <Vnd>
        <xsl:value-of select="PaymentCard/@CardCode" />
      </Vnd>
      <Acct>
        <xsl:value-of select="PaymentCard/@CardNumber" />
      </Acct>
    </CreditCardFOP>
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
                          <CC>
                            <xsl:value-of select="PaymentCard/@CardCode" />
                          </CC>
                          <ExpDt>
                            <xsl:value-of select="PaymentCard/@ExpireDate" />
                          </ExpDt>
                          <Acct>
                            <xsl:value-of select="PaymentCard/@CardNumber" />
                          </Acct>
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
    <xsl:text>TKP</xsl:text>
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
    <!-- 1,2 -->
    <xsl:if test="string-length($TRPH) != 0">

      <xsl:variable name="elems">
        <xsl:call-template name="tokenizeString">
          <xsl:with-param name="list" select="$TRPH"/>
          <xsl:with-param name="delimiter" select="','"/>
        </xsl:call-template>
      </xsl:variable>

      <xsl:for-each select="msxsl:node-set($elems)/elem">
        <xsl:variable name="tRPH">
          <xsl:value-of select="."/>
        </xsl:variable>
        <Psgr>
          <LNameNum>0<xsl:value-of select="$tRPH" /></LNameNum>
          <PsgrNum>0<xsl:value-of select="$tRPH" /></PsgrNum>
          <AbsNameNum>0<xsl:value-of select="$tRPH" /></AbsNameNum>
        </Psgr>
      </xsl:for-each>


      <!--
      <xsl:call-template name="SSRPerPax">
        <xsl:with-param name="TRPH">
          <xsl:value-of select="substring($TRPH,2)"/>
        </xsl:with-param>
      </xsl:call-template>
      -->

    </xsl:if>
  </xsl:template>

  <xsl:template name="SSRPerSeg">
    <xsl:param name="SRPH"/>
    <xsl:if test="string-length($SRPH) != 0">
      <xsl:variable name="sRPH">
        <xsl:value-of select="substring($SRPH,1,1)"/>
      </xsl:variable>
      <SegNum>
        0<xsl:value-of select="$sRPH" />
      </SegNum>
      <xsl:call-template name="SSRPerSeg">
        <xsl:with-param name="SRPH">
          <xsl:value-of select="substring($SRPH,2)"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
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