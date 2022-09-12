<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_PNRCancelRQ.xsl 														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_CancelRQ" />
	</xsl:template>
	<xsl:template match="OTA_CancelRQ">
		<PNRBFManagement_17>
			<PNRBFRetrieveMods>
				<PNRAddr>
					<FileAddr/>
					<CodeCheck/>
					<RecLoc>
						<xsl:value-of select="UniqueID/@ID"/>
					</RecLoc>
				</PNRAddr>
			</PNRBFRetrieveMods>
			<SegCancelMods>
				<CancelSegAry>
					<CancelSeg>
						<Tok>01</Tok> 
						<SegNum>FF</SegNum> 
					</CancelSeg>
				</CancelSegAry>
			</SegCancelMods>				
			<EndTransactionMods>
				<EndTransactRequest>
					<ETInd>E</ETInd>
					<RcvdFrom>
						<xsl:choose>
							<xsl:when test="POS/Source/@AgentSine != ''"><xsl:value-of select="POS/Source/@AgentSine"/></xsl:when>
							<xsl:otherwise>TRIPXML</xsl:otherwise>
						</xsl:choose>
					</RcvdFrom>
				</EndTransactRequest>
			</EndTransactionMods>
		</PNRBFManagement_17>
	</xsl:template>
</xsl:stylesheet>
