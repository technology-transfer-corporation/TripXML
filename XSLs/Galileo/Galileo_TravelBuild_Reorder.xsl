<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<xsl:template match="/">
		<xsl:if test="AgencyPNRBFDisplay_7_0">
			<AgencyReorderItinerary_1_0>
				<ReorderItineraryMods>
					<ReorderSegs>
						<xsl:apply-templates select="AgencyPNRBFDisplay_7_0/PNRBFRetrieve" />
					</ReorderSegs>
				</ReorderItineraryMods>
			</AgencyReorderItinerary_1_0>
		</xsl:if>
		<xsl:if test="SegmentCancel_6_0">
			<SegmentCancel_6_0>
				<SegCancelMods>
					<CancelSegAry>
						<xsl:apply-templates select="SegmentCancel_6_0/SegCancelMods/CancelSegAry/CancelSeg/SegNum" mode="cancel">
							<xsl:sort data-type="text" select="." order="ascending" />
						</xsl:apply-templates>
					</CancelSegAry>
				</SegCancelMods>
			</SegmentCancel_6_0>
		</xsl:if>
	</xsl:template>
	<xsl:template match="PNRBFRetrieve">
		<xsl:apply-templates select="AirSeg/Dt | HtlSeg/StartDt | CarSeg/StartDt" mode="first">
			<xsl:sort data-type="text" select="." order="ascending" />
		</xsl:apply-templates>
		<SegNumAry>
			<xsl:apply-templates select="AirSeg/Dt | HtlSeg/StartDt | CarSeg/StartDt" mode="next">
				<xsl:sort data-type="text" select="." order="ascending" />
			</xsl:apply-templates>
		</SegNumAry>
	</xsl:template>
	<xsl:template match="Dt | StartDt" mode="first">
		<xsl:if test="position()=1">
			<AfterSegNum>
				<xsl:value-of select="../SegNum" />
			</AfterSegNum>
			<Spare />
		</xsl:if>
	</xsl:template>
	<xsl:template match="Dt | StartDt" mode="next">
		<xsl:if test="position()>1">
			<xsl:if test="//ARNK">
				<xsl:variable name="priorseg">
					<xsl:value-of select="../SegNum - 1" />
				</xsl:variable>
				<xsl:if test="$priorseg>0">
					<xsl:apply-templates select="../../ARNK[SegNum=$priorseg]" />
				</xsl:if>
			</xsl:if>
			<SegNum>
				<xsl:value-of select="../SegNum" />
			</SegNum>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ARNK">
		<SegNum>
			<xsl:value-of select="SegNum" />
		</SegNum>
	</xsl:template>
	<xsl:template match="SegNum" mode="cancel">
		<CancelSeg>
			<Tok>
				<xsl:value-of select="../Tok" />
			</Tok>
			<SegNum>
				<xsl:value-of select="." />
			</SegNum>
		</CancelSeg>
	</xsl:template>
</xsl:stylesheet>
