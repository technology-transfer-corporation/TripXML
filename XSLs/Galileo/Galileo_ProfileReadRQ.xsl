<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_ProfileReadRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 29 Aug 2010 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<ClientFile_2>
			<ClientFileMods>
				<xsl:choose>
					<xsl:when test="OTA_ProfileReadRQ/UniqueID/@ID='*'">
						<ClientFileListMods>
							<CRSID>
								<xsl:choose>
									<xsl:when test="OTA_ProfileReadRQ/POS/TPA_Extensions/Provider/Name='Apollo'">1V</xsl:when>
									<xsl:otherwise>1G</xsl:otherwise>
								</xsl:choose>
							</CRSID> 
							<PCC><xsl:value-of select="OTA_ProfileReadRQ/POS/Source/@PseudoCityCode"/></PCC>
							<BusinessTitle />
							<PersonalTitle />
							<PrefInd>B</PrefInd>
						</ClientFileListMods>
					</xsl:when>
					<xsl:when test="OTA_ProfileReadRQ/ReadRequests/ReadRequest/UniqueID/@ID='*'">
						<ClientFileListMods>
							<CRSID>
								<xsl:choose>
									<xsl:when test="OTA_ProfileReadRQ/POS/TPA_Extensions/Provider/Name='Apollo'">1V</xsl:when>
									<xsl:otherwise>1G</xsl:otherwise>
								</xsl:choose>
							</CRSID> 
							<PCC><xsl:value-of select="OTA_ProfileReadRQ/POS/Source/@PseudoCityCode"/></PCC>
							<BusinessTitle>
								<xsl:value-of select="OTA_ProfileReadRQ/UniqueID/@ID"/>
							</BusinessTitle>
							<PersonalTitle />
							<PrefInd>P</PrefInd>
						</ClientFileListMods>
					</xsl:when>
					<xsl:otherwise>
						<ClientFileDisplayMods>
							<CRSID>
								<xsl:choose>
									<xsl:when test="OTA_ProfileReadRQ/POS/TPA_Extensions/Provider/Name='Apollo'">1V</xsl:when>
									<xsl:otherwise>1G</xsl:otherwise>
								</xsl:choose>
							</CRSID> 
							<PCC><xsl:value-of select="OTA_ProfileReadRQ/POS/Source/@PseudoCityCode"/></PCC> 
							<BusinessTitle><xsl:value-of select="OTA_ProfileReadRQ/UniqueID/@ID"/></BusinessTitle> 
							<PersonalTitle>
								<xsl:value-of select="OTA_ProfileReadRQ/ReadRequests/ReadRequest/UniqueID/@ID"/>
							</PersonalTitle> 
							<FileInd>A</FileInd> 
							<MergeInd>N</MergeInd> 
						</ClientFileDisplayMods>
					</xsl:otherwise>
				</xsl:choose>
			</ClientFileMods>
		</ClientFile_2>
	</xsl:template>
</xsl:stylesheet>
