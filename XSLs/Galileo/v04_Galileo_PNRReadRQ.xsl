<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- v04_Galileo_PNRReadRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 11 oct 2010 - Rastko	 - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<PNRBFManagement_17>
			<PNRBFRetrieveMods>
				<PNRAddr>
					<FileAddr/>
					<CodeCheck/>
					<RecLoc>
						<xsl:choose>
							<xsl:when test="OTA_UpdateRQ/UniqueID/@ID != ''">
								<xsl:value-of select="OTA_UpdateRQ/UniqueID/@ID" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="OTA_ReadRQ/UniqueID/@ID" />
							</xsl:otherwise>
						</xsl:choose>
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
			<FareRedisplayMods>
				<DisplayAction>
					<Action>D</Action>
				</DisplayAction>
				<FareNumInfo>
					<FareNumAry>
						<FareNum>4</FareNum>
					</FareNumAry>
				</FareNumInfo>
			</FareRedisplayMods>
		</PNRBFManagement_17>
	</xsl:template>
</xsl:stylesheet>
