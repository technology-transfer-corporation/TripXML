<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- AmadeusWS_TransferOwnershipRQ.xsl 						    					       -->
<!-- ================================================================== -->
<!-- Date: 06 Apr 2010 - Rastko														       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="OTA_TransferOwnershipRQ/POS/Source/@PseudoCityCode='MEXMX28BK'">
				<xsl:apply-templates select="OTA_TransferOwnershipRQ" mode="Banamex"/>
			</xsl:when>
			<xsl:when test="OTA_TransferOwnershipRQ/POS/Source/@PseudoCityCode='IAH1S3100'">
				<xsl:apply-templates select="OTA_TransferOwnershipRQ" mode="Banamex"/>
			</xsl:when>
			<xsl:when test="count(OTA_TransferOwnershipRQ/NewOwner) = 1">
				<xsl:apply-templates select="OTA_TransferOwnershipRQ" mode="xml"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="OTA_TransferOwnershipRQ" mode="cryptic"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="OTA_TransferOwnershipRQ" mode="xml">
		<PoweredPNR_TransferOwnership>		
			<recordLocator>
				<reservation>
					<controlNumber>
						<xsl:value-of select="UniqueID/@ID" />
					</controlNumber>
				</reservation>
			</recordLocator>
			<!--propagatioAction>
				<actionRequestCode>NPR</actionRequestCode>
			</propagatioAction-->
			<officeIdentification>
				<officeDetail>
					<originatorDetails>
						<inHouseIdentification1>
							<xsl:value-of select="NewOwner/@PseudoCityCode" />
						</inHouseIdentification1>
					</originatorDetails>
				</officeDetail>
				<xsl:if test="NewOwner/Change/@ActionCode != ''">
					<xsl:for-each select="NewOwner/Change">
						<specificChanges>
							<actionRequestCode>
								<xsl:choose>
									<xsl:when test="@ActionCode = 'TicketingOffice'">TO</xsl:when>
									<xsl:when test="@ActionCode = 'QueuingOffice'">QO</xsl:when>
									<xsl:otherwise>OQ</xsl:otherwise>
								</xsl:choose>
							</actionRequestCode>
						</specificChanges>
					</xsl:for-each>
				</xsl:if>
			</officeIdentification>	
		</PoweredPNR_TransferOwnership>	
	</xsl:template>	
	<xsl:template match="OTA_TransferOwnershipRQ" mode="cryptic">
		<Cryptic>
			<Command_Cryptic>
				<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<textStringDetails>
						<xsl:text>RP/</xsl:text>
						<xsl:value-of select="NewOwner[1]/@PseudoCityCode" />
						<xsl:if test="NewOwner[1]/Change/@ActionCode != ''">
							<xsl:text>/</xsl:text>
							<xsl:for-each select="NewOwner[1]/Change">
								<xsl:choose>
									<xsl:when test="@ActionCode = 'TicketingOffice'">TK</xsl:when>
									<xsl:when test="@ActionCode = 'QueuingOffice'">OP</xsl:when>
								</xsl:choose>
							</xsl:for-each>
						</xsl:if>
					</textStringDetails>
				</longTextString>
			</Command_Cryptic>
			<Command_Cryptic>
				<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<textStringDetails>
						<xsl:text>RFXX/</xsl:text>
						<xsl:value-of select="NewOwner[position()=2]/@PseudoCityCode" />
					</textStringDetails>
				</longTextString>
			</Command_Cryptic>
		</Cryptic>
	</xsl:template>
	<!-- -->
	<xsl:template match="OTA_TransferOwnershipRQ" mode="Banamex">
		<Cryptic>
			<Command_Cryptic>
				<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<textStringDetails>
						<xsl:text>RP/</xsl:text>
						<xsl:value-of select="NewOwner/@PseudoCityCode" />
						<xsl:text>/ALL</xsl:text>
					</textStringDetails>
				</longTextString>
			</Command_Cryptic>
			<Command_Cryptic>
				<messageAction>
				<messageFunctionDetails>
					<messageFunction>M</messageFunction>
				</messageFunctionDetails>
				</messageAction>
				<longTextString>
					<textStringDetails>
						<xsl:text>RF</xsl:text>
						<xsl:choose>
							<xsl:when test="POS/Source/@AgentSine!=''">
								<xsl:value-of select="POS/Source/@AgentSine"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'TRIPXML'"/>
							</xsl:otherwise>
						</xsl:choose>
					</textStringDetails>
				</longTextString>
			</Command_Cryptic>
		</Cryptic>
	</xsl:template>
</xsl:stylesheet>

