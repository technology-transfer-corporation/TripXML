<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Amadeus_TransferOwnershipRQ.xsl 						       					       -->
<!-- ================================================================== -->
<!-- Date: 18 Mar 2009																	       -->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:choose>
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
			<Cryptic_GetScreen_Query>
				<Command>
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
				</Command>
			</Cryptic_GetScreen_Query>
			<Cryptic_GetScreen_Query>
				<Command>
					<xsl:text>RFXX/</xsl:text>
					<xsl:value-of select="NewOwner[position()=2]/@PseudoCityCode" />
				</Command>
			</Cryptic_GetScreen_Query>
		</Cryptic>
	</xsl:template>
</xsl:stylesheet>

