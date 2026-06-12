<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_PNREndRQ.xsl 														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<PNREnd>
			<xsl:apply-templates select="OTA_PNREndRQ" />
		</PNREnd>
	</xsl:template>
	<xsl:template match="OTA_PNREndRQ">
		<Read>
			<PNRBFManagement_53>
				<PNRBFRetrieveMods>
					<PNRAddr>
						<FileAddr />
						<CodeCheck />
						<RecLoc>
							<xsl:value-of select="UniqueID/@ID" />
						</RecLoc>
					</PNRAddr>
				</PNRBFRetrieveMods>
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<FareNum>1</FareNum>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<FareNum>2</FareNum>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>
				<FareRedisplayMods>
					<DisplayAction>
						<Action>D</Action>
					</DisplayAction>
					<FareNumInfo>
						<FareNumAry>
							<FareNum>3</FareNum>
						</FareNumAry>
					</FareNumInfo>
				</FareRedisplayMods>
			</PNRBFManagement_53>
		</Read>
		<ET>
			<PNRBFManagement_53>
				<PNRBFRetrieveMods>
					<PNRAddr>
						<FileAddr />
						<CodeCheck />
						<RecLoc>
							<xsl:value-of select="UniqueID/@ID" />
						</RecLoc>
					</PNRAddr>
				</PNRBFRetrieveMods>
				<EndTransactionMods>
					<EndTransactRequest>
						<ETInd>E</ETInd>
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
	</xsl:template>
</xsl:stylesheet>
