<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_ProfileCreateRQ.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 25 Aug 2010 - Rastko - mapping updates									-->
	<!-- Date: 11 Sep 2010 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:variable name="crsid">
		<xsl:choose>
			<xsl:when test="OTA_ProfileCreateRQ/POS/TPA_Extensions/Provider/Name='Apollo'">1V</xsl:when>
			<xsl:otherwise>1G</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_ProfileCreateRQ" />
	</xsl:template>
	
	<xsl:template match="OTA_ProfileCreateRQ">
		<ClientFile_2>
			<ClientFileMods>
				<ClientFileCreateHeaderMods>
					<FileInd>
						<xsl:choose>
							<xsl:when test="UniqueID[position()=2]/@ID!=''">P</xsl:when>
							<xsl:otherwise>B</xsl:otherwise>
						</xsl:choose>
					</FileInd>
					<DataInd>C</DataInd>
					<ResponseInd>D</ResponseInd>
					<NumLines>001	</NumLines>
					<CRSID>
						<xsl:value-of select="$crsid"/>
					</CRSID> 
					<PCC><xsl:value-of select="POS/Source/@PseudoCityCode"/></PCC> 
					<BusinessTitle><xsl:value-of select="UniqueID[1]/@ID"/></BusinessTitle> 
					<PersonalTitle>
						<xsl:value-of select="UniqueID[position()=2]/@ID"/>
					</PersonalTitle> 
				</ClientFileCreateHeaderMods>
				<xsl:for-each select="Profile/CompanyInfo/ContactPerson/PersonName[Surname!=''] | Profile/Customer/PersonName[Surname!='']">
					<ClientFileFixedLineData>
						<LineNum><xsl:value-of select="format-number(@RPH,'000')"/></LineNum>
						<MoveInd>Y</MoveInd>
						<ClientInd />
						<SecondaryInd />
						<TertiaryInd />
						<DataType>0001</DataType>
					</ClientFileFixedLineData>
					<ClientFileVariableLineData>
						<Data>
							<xsl:value-of select="Surname"/>
							<xsl:value-of select="concat('/',GivenName)"/>
						</Data>
					</ClientFileVariableLineData>
				</xsl:for-each>
				<xsl:for-each select="Profile/CompanyInfo/TelephoneInfo | Profile/Customer/Telephone">
					<ClientFileFixedLineData>
						<LineNum>
							<xsl:value-of select="format-number(@RPH,'000')"/>
						</LineNum>
						<MoveInd>
							<xsl:choose>
								<xsl:when test="@TransferAction='Mandatory'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</MoveInd>
						<ClientInd />
						<SecondaryInd />
						<TertiaryInd />
						<DataType>0002</DataType>
					</ClientFileFixedLineData>
					<ClientFileVariableLineData>
						<Data>
							<xsl:if test="@AreaCityCode!=''">
								<xsl:value-of select="concat(@AreaCityCode,'R/')"/>
							</xsl:if>
							<xsl:value-of select="@PhoneNumber"/>
						</Data>
					</ClientFileVariableLineData>
				</xsl:for-each>
				<xsl:for-each select="Profile/Customer/Email">
					<ClientFileFixedLineData>
						<LineNum>
							<xsl:value-of select="format-number(@RPH,'000')"/>
						</LineNum>
						<MoveInd>
							<xsl:choose>
								<xsl:when test="@TransferAction='Mandatory'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</MoveInd>
						<ClientInd />
						<SecondaryInd />
						<TertiaryInd />
						<DataType>0059</DataType>
					</ClientFileFixedLineData>
					<ClientFileVariableLineData>
						<Data>
							<xsl:value-of select="."/>
						</Data>
					</ClientFileVariableLineData>
				</xsl:for-each>
				<xsl:for-each select="Profile/Customer/Address[CityName!='']">
					<ClientFileFixedLineData>
						<LineNum>
							<xsl:value-of select="format-number(@RPH,'000')"/>
						</LineNum>
						<MoveInd>
							<xsl:choose>
								<xsl:when test="@TransferAction='Mandatory'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</MoveInd>
						<ClientInd />
						<SecondaryInd />
						<TertiaryInd />
						<DataType>
							<xsl:choose>
								<xsl:when test="@Type='Written'">0003</xsl:when>
								<xsl:otherwise>0004</xsl:otherwise>
							</xsl:choose>
						</DataType>
					</ClientFileFixedLineData>
					<ClientFileVariableLineData>
						<Data>
							<xsl:choose>
								<xsl:when test="$crsid='1V'">
									<xsl:value-of select="../PersonName/GivenName"/>
									<xsl:value-of select="concat(' ',../PersonName/Surname)"/>
									<xsl:value-of select="concat('@',StreetNmbr)"/>
									<xsl:value-of select="concat(' ',AddressLine)"/>
									<xsl:value-of select="concat('@',CityName)"/>
									<xsl:value-of select="concat(' ',StateProv/@StateCode)"/>
									<xsl:value-of select="concat(' Z/',PostalCode)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="../PersonName/GivenName"/>
									<xsl:value-of select="concat(' ',../PersonName/Surname)"/>
									<xsl:value-of select="concat('*',StreetNmbr)"/>
									<xsl:value-of select="concat(' ',AddressLine)"/>
									<xsl:value-of select="concat('*',CityName)"/>
									<xsl:value-of select="concat('*',StateProv/@StateCode)"/>
									<xsl:value-of select="concat('*P/',PostalCode)"/>
								</xsl:otherwise>
							</xsl:choose>
						</Data>
					</ClientFileVariableLineData>
				</xsl:for-each>
				<xsl:for-each select="Profile/CompanyInfo/PaymentForm[PaymentCard/@CardNumber!=''] | Profile/Customer/PaymentForm[PaymentCard/@CardNumber!='']">
					<ClientFileFixedLineData>
						<LineNum>
							<xsl:value-of select="format-number(@RPH,'000')"/>
						</LineNum>
						<MoveInd>
							<xsl:choose>
								<xsl:when test="@TransferAction='Mandatory'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</MoveInd>
						<ClientInd />
						<SecondaryInd />
						<TertiaryInd />
						<DataType>0021</DataType>
					</ClientFileFixedLineData>
					<ClientFileVariableLineData>
						<Data>
							<xsl:value-of select="PaymentCard/@CardCode"/>
							<xsl:value-of select="PaymentCard/@CardNumber"/>
							<xsl:value-of select="concat('/D',PaymentCard/@ExpireDate)"/>
						</Data>
					</ClientFileVariableLineData>
				</xsl:for-each>
			</ClientFileMods>
		</ClientFile_2>
	</xsl:template>
</xsl:stylesheet>
