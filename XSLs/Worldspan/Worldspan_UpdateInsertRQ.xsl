<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- 
================================================================== 
Worldspan_UpdateInsertRQ.xsl 													       
================================================================== 
Date: 27 Jul 2021 - Kobelev - Markup has to be associated with correct TR number.
Date: 16 Feb 2010 - Rastko - added insert FOP									    
================================================================== 
-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:variable name="pcc" select="UpdateInsert/OTA_UpdateRQ/POS/Source/@PseudoCityCode"/>
	<xsl:template match="/">
		<xsl:apply-templates select="UpdateInsert/OTA_UpdateRQ"/>
		<xsl:apply-templates select="UpdateInsert/OTA_UpdateSessionedRQ"/>
	</xsl:template>
	<xsl:template match="OTA_UpdateRQ | OTA_UpdateSessionedRQ">
		<UpdateInsert>
			<xsl:if test="Position/Element[@Operation='insert' and @Child='Remarks'] or Position/Element[@Operation='insert' and @Child='SpecialRemarks'] or Position/Element[@Operation='insert' and @Child='Fulfillment']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
				<TTRMC>
					<RMC2>
						<MSG_VERSION>2</MSG_VERSION>
						<xsl:variable name="rmks">
							<xsl:value-of select="count(Position/Element[@Operation='insert' and @Child='Remarks']/Remarks/Remark)"/>
						</xsl:variable>
						<xsl:variable name="sprmks">
							<xsl:value-of select="count(Position/Element[@Operation='insert' and @Child='SpecialRemarks']/SpecialRemarks/SpecialRemark)"/>
						</xsl:variable>
						<xsl:variable name="ccs"><xsl:value-of select="count(/Position/Element[@Operation='insert' and @Child='Fulfillment']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard)"/></xsl:variable>
						<VARIABLE_COUNT><xsl:value-of select="$rmks + $ccs + $sprmks"/></VARIABLE_COUNT>
						<PNR_RLOC><xsl:value-of select="UniqueID/@ID"/></PNR_RLOC>
						<END_OPTION>E</END_OPTION>
						<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>
						<xsl:for-each select="Position/Element[@Operation='insert' and @Child='Remarks']/Remarks/Remark">
							<REMARK>
								<REMARK_LINE>
									<xsl:value-of select="."/>
								</REMARK_LINE>
							</REMARK>
						</xsl:for-each>
						<xsl:for-each select="Position/Element[@Operation='insert' and @Child='SpecialRemarks']/SpecialRemarks/SpecialRemark">
							<REMARK>
								<REMARK_LINE>
									<xsl:choose>
										<xsl:when test="@RemarkType='Historical'">
											<xsl:value-of select="concat('.Z',Text)"/>
										</xsl:when>
										<xsl:when test="@RemarkType='TourCode'">
											<xsl:value-of select="concat('-',Text)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="."/>
										</xsl:otherwise>
									</xsl:choose>
								</REMARK_LINE>
							</REMARK>
						</xsl:for-each>
						<xsl:for-each select="Position/Element[@Operation='insert' and @Child='Fulfillment']/Fulfillment/PaymentDetails/PaymentDetail/PaymentCard">
							<REMARK>
								<REMARK_LINE>
									<xsl:text>$CC</xsl:text>
									<xsl:value-of select="@CardCode"/>
									<xsl:value-of select="@CardNumber"/>
									<xsl:text>N</xsl:text>
									<xsl:value-of select="@ExpireDate"/>
								</REMARK_LINE>
							</REMARK>
						</xsl:for-each>
					</RMC2>
				</TTRMC>
			</xsl:if>
			<xsl:if test="Position/Element[@Operation='insert' and @Child='PNRData']">
				<TTUPC>
					<xsl:for-each select="Position/Element[@Operation='insert' and @Child='PNRData']/PNRData/FuturePriceInfo">
						<UPC7>
							<MSG_VERSION>7</MSG_VERSION>
							<VARIABLE_COUNT>1</VARIABLE_COUNT>
							<PNR_RLOC><xsl:value-of select="../../../../UniqueID/@ID"/></PNR_RLOC>
							<END_OPTION>E</END_OPTION>
							<OVERRIDE_CUST_REF>Y</OVERRIDE_CUST_REF>
							<DOC_INSTRUCT_REMARK>
								<xsl:value-of select="'4-DI'"/>
								<xsl:variable name="sf"><xsl:value-of select="@StoredFareRef"/></xsl:variable>
								<xsl:variable name="paxt">
									<xsl:choose>
										<xsl:when test="../../../../DPW8/PRC_INF/PRC_QUO">
											<xsl:value-of select="../../../../DPW8/PRC_INF/PRC_QUO/PTC_FAR_DTL[position()=$sf]/PTC"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="../../../../DPW8/PRC_INF/TIC_REC_PRC_QUO[1]/PTC_FAR_DTL[position()=$sf]/PTC"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:variable>
								<xsl:value-of select="'#N'"/>
								<!--xsl:for-each select="../../../../DPW8/PAX_INF/NME_ITM[PTC=$paxt]">
									<xsl:if test="position() > 1"><xsl:value-of select="'/'"/></xsl:if>
									<xsl:value-of select="NME_POS"/>
								</xsl:for-each-->
								<xsl:for-each select="TravelerRPH">
									<xsl:if test="position() > 1"><xsl:value-of select="'/'"/></xsl:if>
									<xsl:value-of select="."/>
								</xsl:for-each>
								<xsl:if test="Markup">
									<xsl:value-of select="concat('#TR',$sf)"/>
								</xsl:if>
								<xsl:if test="Commission">
									<xsl:variable name="apos">#</xsl:variable>
									<xsl:variable name="comm">
										<xsl:choose>
											<xsl:when test="Commission/@Amount!=''">
												<xsl:value-of select="string(Commission/@Amount)"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="string(Commission/@Percent)"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:variable name="comm1">
										<xsl:choose>
											<xsl:when test="$comm='0'"><xsl:value-of select="'0'"/></xsl:when>
											<xsl:when test="Commission/@DecimalPlaces='2'">
												<xsl:value-of select="substring($comm,1,string-length($comm) - 2)"/>
												<xsl:value-of select="'.'"/>
												<xsl:value-of select="substring($comm,string-length($comm) - 1)"/>
											</xsl:when>
                      <xsl:when test="Commission/@DecimalPlaces='0'">
                        <xsl:value-of select="$comm"/>
                      </xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat($comm,'.00')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:variable>
									<xsl:choose>
										<xsl:when test="Commission/@Amount!=''">
											<xsl:value-of select="concat($apos,'K$',$comm1)"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat($apos,'K',$comm1)"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:if>
								<!--xsl:if test="Commission">
									<xsl:value-of select="'-OK'"/>
								</xsl:if-->
							</DOC_INSTRUCT_REMARK>
						</UPC7>
					</xsl:for-each>
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
		</UpdateInsert>
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
