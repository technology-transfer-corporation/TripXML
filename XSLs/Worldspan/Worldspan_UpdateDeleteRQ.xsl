<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_UpdateDeleteRQ.xsl 													       -->
<!-- ================================================================== -->
<!-- Date: 06 Apr 2014 - Rastko - new file									       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="pcc" select="UpdateDelete/OTA_UpdateRQ/POS/Source/@PseudoCityCode"/>
	<xsl:template match="/">
		<xsl:apply-templates select="UpdateDelete/OTA_UpdateRQ"/>
		<xsl:apply-templates select="UpdateDelete/OTA_UpdateSessionedRQ"/>
	</xsl:template>
	<xsl:template match="OTA_UpdateRQ | OTA_UpdateSessionedRQ">
		<Updatedelete>
			<xsl:if test="Position/Element[@Operation='delete' and @Child='Remarks'] or Position/Element[@Operation='delete' and @Child='SpecialRemarks'] or Position/Element[@Operation='delete' and @Child='Fulfillment']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard or Position/Element[@Operation='delete' and @Child='TPA_Extensions']">
				<TTUPC>
					<UPC7>
						<MSG_VERSION>7</MSG_VERSION>
						<xsl:variable name="rmks">
							<xsl:value-of select="count(Position/Element[@Operation='delete' and @Child='Remarks']/Remarks/Remark)"/>
						</xsl:variable>
						<xsl:variable name="tktinst">
							<xsl:value-of select="count(Position/Element[@Operation='delete' and @Child='TPA_Extensions']/TPA_Extensions)"/>
						</xsl:variable>
						<xsl:variable name="sprmks">
							<xsl:value-of select="count(Position/Element[@Operation='delete' and @Child='SpecialRemarks']/SpecialRemarks/SpecialRemark)"/>
						</xsl:variable>
						<xsl:variable name="ccs">
							<xsl:value-of select="count(/Position/Element[@Operation='delete' and @Child='Fulfillment']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard)"/>
						</xsl:variable>
						<VARIABLE_COUNT><xsl:value-of select="$rmks + $ccs + $sprmks + $tktinst"/></VARIABLE_COUNT>
						<PNR_RLOC><xsl:value-of select="UniqueID/@ID"/></PNR_RLOC>
						<END_OPTION>E</END_OPTION>
						<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>
						<xsl:if test="Position/Element[@Operation='delete' and @Child='Remarks']">
							<REMARKS>
								<xsl:for-each select="Position/Element[@Operation='delete' and @Child='Remarks']/Remarks/Remark">
									<xsl:sort order="descending" data-type="number" select="@RPH"/>
										<REMARK_LINE>
											<xsl:value-of select="concat('5',@RPH,'@')"/>
										</REMARK_LINE>
									</xsl:for-each>
							</REMARKS>
						</xsl:if>
						<!--xsl:for-each select="Position/Element[@Operation='delete' and @Child='SpecialRemarks']/SpecialRemarks/SpecialRemark">
							<REMARK>
								<REMARK_LINE>
									<xsl:choose>
										<xsl:when test="@RemarkType='Historical'">
											<xsl:value-of select="concat('.Z',Text)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="."/>
										</xsl:otherwise>
									</xsl:choose>
								</REMARK_LINE>
							</REMARK>
						</xsl:for-each>
						<xsl:for-each select="Position/Element[@Operation='delete' and @Child='Fulfillment']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
							<REMARK>
								<REMARK_LINE>
									<xsl:text>$CC</xsl:text>
									<xsl:value-of select="@CardCode"/>
									<xsl:value-of select="@CardNumber"/>
									<xsl:text>N</xsl:text>
									<xsl:value-of select="@ExpireDate"/>
								</REMARK_LINE>
							</REMARK>
						</xsl:for-each-->
						<xsl:if test="Position/Element[@Operation='delete' and @Child='TPA_Extensions']">
							<DOC_INSTRUCT_REMARK>
								<xsl:value-of select="'4-DI@'"/>
								<xsl:value-of select="Position/Element[@Operation='delete' and @Child='TPA_Extensions']/TPA_Extensions/FuturePriceInfo/@RPH"/>
							</DOC_INSTRUCT_REMARK>
						</xsl:if>
					</UPC7>
				</TTUPC>
			</xsl:if>
			<TTDPC>
				<DPC8>
					<MSG_VERSION>8</MSG_VERSION>
					<REC_LOC><xsl:value-of select="UniqueID/@ID"/></REC_LOC>
					<ETR_INF>Y</ETR_INF> 
		  			<ALL_PNR_INF>Y</ALL_PNR_INF> 
		  			<PRC_INF>Y</PRC_INF>
				</DPC8>
			</TTDPC>
		</Updatedelete>
	</xsl:template>
	
	<xsl:template match="Element" mode="rmks">
		<TTRMC>
			<RMC2>
				<MSG_VERSION>2</MSG_VERSION>
				<xsl:variable name="rmks"><xsl:value-of select="count(Remarks/Remark)"/></xsl:variable>
				<xsl:variable name="ccs"><xsl:value-of select="count(Fulfillment/PaymentDetails/PaymentDetail/PaymentCard)"/></xsl:variable>
				<VARIABLE_COUNT><xsl:value-of select="$rmks + $ccs"/></VARIABLE_COUNT>
				<PNR_RLOC><xsl:value-of select="//OTA_UpdateRQ/UniqueID/@ID"/></PNR_RLOC>
				<END_OPTION>E</END_OPTION>
				<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>
				<xsl:for-each select="Remarks/Remark">
					<REMARK>
						<REMARK_LINE>
							<xsl:value-of select="."/>
						</REMARK_LINE>
					</REMARK>
				</xsl:for-each>
				<xsl:for-each select="Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
					<REMARK>
						<REMARK_LINE>
							<xsl:text>CC</xsl:text>
							<xsl:value-of select="@CardCode"/>
							<xsl:value-of select="@CardNumber"/>
							<xsl:text>N</xsl:text>
							<xsl:value-of select="@ExpireDate"/>
						</REMARK_LINE>
					</REMARK>
				</xsl:for-each>
			</RMC2>
		</TTRMC>
	</xsl:template>
	
</xsl:stylesheet>
