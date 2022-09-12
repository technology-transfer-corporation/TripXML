<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
	<!-- ================================================================== -->
	<!-- Galileo_ProfileReadRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 25 Aug 2010 - Rastko - mapping updates									-->
	<!-- Date: 19 Aug 2010 - Rastko - mapping updates									-->
	<!-- Date: 29 Aug 2010 - Rastko - new file												-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_ProfileReadRS Version="1.0">
			<xsl:apply-templates select="ClientFile_2" />
		</OTA_ProfileReadRS>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFile_2">
		<xsl:choose>
			<xsl:when test="ClientFile/ErrText">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Galileo</xsl:attribute>
						<xsl:value-of select="ClientFile/ErrText/Text" />
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success />
				<Profiles>
					<xsl:apply-templates select="ClientFile/ClientFileTypeDisplayHeader" mode="header"/>
					<xsl:apply-templates select="ClientFile/ClientFileSingleTitleList"/>
				</Profiles>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileTypeDisplayHeader" mode="header">
		<xsl:variable name="profileType"><xsl:value-of select="FileInd"/></xsl:variable>
		<ProfileInfo>
			<UniqueID>
				<xsl:attribute name="Type">21</xsl:attribute>
				<xsl:attribute name="ID">
					<xsl:choose>
						<xsl:when test="PersonalTitle!=''">
							<xsl:value-of select="PersonalTitle"/>
						</xsl:when>
						<xsl:when test="BusinessTitle!=''">
							<xsl:value-of select="BusinessTitle"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="AgncyDesc"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:if test="PersonalTitle=''">
					<xsl:choose>
						<xsl:when test="BusinessTitle!=''">
							<xsl:choose>
								<xsl:when test="following-sibling::ClientFileFixedLineData[1]/DataType='31'">
									<CompanyName>
										<xsl:value-of select="following-sibling::ClientFileFixedLineData[1]/following-sibling::ClientFileVariableLineData[1]/Data"/>
									</CompanyName>
								</xsl:when>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<CompanyName>
								<xsl:value-of select="AgncyDesc"/>
							</CompanyName>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</UniqueID>
			<Profile>
				<xsl:attribute name="ProfileType">
					<xsl:choose>
						<xsl:when test="FileInd='M'">
							<xsl:value-of select="'Travel Agent'"/>
						</xsl:when>
						<xsl:when test="FileInd='B'">
							<xsl:value-of select="'Corporation'"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="'Customer'"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="CreateDateTime">
					<xsl:value-of select="substring(CreationDt,1,4)"/>
					<xsl:value-of select="concat('-',substring(CreationDt,5,2))"/>
					<xsl:value-of select="concat('-',substring(CreationDt,7),'T00:00:00')"/>
				</xsl:attribute>
				<xsl:attribute name="RPH">
					<xsl:value-of select="position()"/>
				</xsl:attribute>
				<xsl:choose>
					<xsl:when test="FileInd='P'">
						<Customer>
							<xsl:apply-templates select="following-sibling::*[DataType=1][1]" mode="name">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=4][1]" mode="pdeliveryinfo">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=3][1]" mode="written">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=2][1]" mode="phone">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=21][1]" mode="fop">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
						</Customer>
						<xsl:variable name="remarks">
							<Remarks>
								<xsl:apply-templates select="following-sibling::*[DataType=10][1]" mode="remark">
									<xsl:with-param name="profileType" select="$profileType"/>
								</xsl:apply-templates>
							</Remarks>
						</xsl:variable>
						<xsl:if test="msxsl:node-set($remarks)/Remarks/Remark">
							<xsl:copy-of select="msxsl:node-set($remarks)"/>
						</xsl:if>
						<xsl:variable name="comments">
							<Comments>
								<Comment>
									<xsl:apply-templates select="following-sibling::*[DataType=31][1]" mode="comment">
										<xsl:with-param name="profileType" select="$profileType"/>
									</xsl:apply-templates>
								</Comment>
							</Comments>
						</xsl:variable>
						<xsl:if test="msxsl:node-set($comments)/Comments/Comment!=''">
							<xsl:copy-of select="msxsl:node-set($comments)"/>
						</xsl:if>
					</xsl:when>
					<xsl:otherwise>
						<CompanyInfo>
							<xsl:choose>
								<xsl:when test="following-sibling::ClientFileFixedLineData[1]/DataType='31'">
									<CompanyName>
										<xsl:value-of select="following-sibling::ClientFileFixedLineData[1]/following-sibling::ClientFileVariableLineData[1]/Data"/>
									</CompanyName>
								</xsl:when>
							</xsl:choose>	
							<xsl:apply-templates select="following-sibling::*[DataType=4][1]" mode="deliveryinfo">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=2][1]" mode="phoneinfo">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=6][1]" mode="osi">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
							<xsl:apply-templates select="following-sibling::*[DataType=21][1]" mode="fop">
								<xsl:with-param name="profileType" select="$profileType"/>
							</xsl:apply-templates>
						</CompanyInfo>
						<xsl:variable name="remarks">
							<Remarks>
								<xsl:apply-templates select="following-sibling::*[DataType=10][1]" mode="remark">
									<xsl:with-param name="profileType" select="$profileType"/>
								</xsl:apply-templates>
							</Remarks>
						</xsl:variable>
						<xsl:if test="msxsl:node-set($remarks)/Remarks/Remark">
							<xsl:copy-of select="msxsl:node-set($remarks)"/>
						</xsl:if>
						<xsl:variable name="comments">
							<Comments>
								<Comment>
									<xsl:apply-templates select="following-sibling::*[DataType=31][1]" mode="comment">
										<xsl:with-param name="profileType" select="$profileType"/>
									</xsl:apply-templates>
								</Comment>
							</Comments>
						</xsl:variable>
						<xsl:if test="msxsl:node-set($comments)/Comments/Comment!=''">
							<xsl:copy-of select="msxsl:node-set($comments)"/>
						</xsl:if>
						<xsl:apply-templates select="following-sibling::*[DataType=25][1]" mode="rula">
							<xsl:with-param name="profileType" select="$profileType"/>
						</xsl:apply-templates>
					</xsl:otherwise>
				</xsl:choose>
			</Profile>
		</ProfileInfo>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="phoneinfo">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=2">
			<TelephoneInfo>
				<xsl:attribute name="PhoneNumber">
					<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'P.')"/>
				</xsl:attribute>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
			</TelephoneInfo>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="phoneinfo">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="deliveryinfo">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=4">
			<AddressInfo>
				<xsl:attribute name="Type">Delivery</xsl:attribute>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:element name="AddressLine">
					<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'D.')"/>
				</xsl:element >
			</AddressInfo>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="deliveryinfo">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="pdeliveryinfo">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=4">
			<Address>
				<xsl:attribute name="Type">Delivery</xsl:attribute>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:element name="AddressLine">
					<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'D.')"/>
				</xsl:element >
			</Address>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="deliveryinfo">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="written">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=3">
			<Address>
				<xsl:attribute name="Type">Written</xsl:attribute>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:element name="AddressLine">
					<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'W.')"/>
				</xsl:element >
			</Address>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="deliveryinfo">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="osi">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=6">
			<OtherServiceInformation>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:element name="Airline">
					<xsl:attribute name="Code">
						<xsl:value-of select="substring(following-sibling::ClientFileVariableLineData[1]/Data,4,2)"/>
					</xsl:attribute>
				</xsl:element >
				<xsl:element name="Text">
					<xsl:value-of select="substring(following-sibling::ClientFileVariableLineData[1]/Data,6)"/>
				</xsl:element>
			</OtherServiceInformation>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="osi">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="fop">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=21">
			<PaymentForm>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<PaymentCard>
					<xsl:variable name="card">
						<xsl:value-of select="following-sibling::ClientFileVariableLineData[1]/Data"/>
					</xsl:variable>
					<xsl:attribute name="CardCode">
						<xsl:value-of select="substring($card,3,2)"/>
					</xsl:attribute>
					<xsl:attribute name="CardNumber">
						<xsl:value-of select="substring-before(substring($card,5),'/D')"/>
					</xsl:attribute>
					<xsl:if test="contains($card,'/CVV')">
						<xsl:attribute name="SeriesCode">
							<xsl:value-of select="substring-after($card,'/CVV ')"/>
						</xsl:attribute>
					</xsl:if>
					<xsl:attribute name="ExpireDate">
						<xsl:choose>
							<xsl:when test="contains(substring-after($card,'/D'),'/')">
								<xsl:value-of select="substring-before(substring-after($card,'/D'),'/')"/>
							</xsl:when>
							<xsl:otherwise><xsl:value-of select="substring-after($card,'/D')"/></xsl:otherwise>
						</xsl:choose>
					</xsl:attribute>
				</PaymentCard>
			</PaymentForm>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="fop">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="comment">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=31">
			<xsl:element name="Text">
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="following-sibling::ClientFileVariableLineData[1]/Data"/>
			</xsl:element>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="comment">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="remark">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=10">
			<xsl:element name="Remark">
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'NP.')"/>
			</xsl:element>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="remark">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="phone">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=2">
			<Telephone>
				<xsl:attribute name="PhoneNumber">
					<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'P.')"/>
				</xsl:attribute>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
			</Telephone>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="phone">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="rula">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=25">
			<TPA_Extensions>
				<xsl:element name="CustomCheckRule">
					<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
					<xsl:attribute name="TransferAction">
						<xsl:choose>
							<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
							<xsl:when test="MoveInd='N'">Never</xsl:when>
						</xsl:choose>
					</xsl:attribute>
					<xsl:value-of select="following-sibling::ClientFileVariableLineData[1]/Data"/>
				</xsl:element>
			</TPA_Extensions>
		</xsl:if>
		<xsl:if test="following-sibling::*[position()=2][name()='ClientFileFixedLineData']">
			<xsl:apply-templates select="following-sibling::*[position()=2]" mode="phone">
				<xsl:with-param name="profileType" select="$profileType"/>
			</xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileFixedLineData" mode="name">
		<xsl:param name="profileType"/>
		<xsl:if test="preceding-sibling::ClientFileTypeDisplayHeader[1]/FileInd = $profileType and DataType=1">
			<PersonName>
				<xsl:attribute name="RPH"><xsl:value-of select="LineNum"/></xsl:attribute>
				<xsl:attribute name="TransferAction">
					<xsl:choose>
						<xsl:when test="MoveInd='Y'">Mandatory</xsl:when>
						<xsl:when test="MoveInd='N'">Never</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<GivenName>
					<xsl:value-of select="substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'/')"/>
				</GivenName>
				<Surname>
					<xsl:value-of select="substring-before(substring-after(following-sibling::ClientFileVariableLineData[1]/Data,'N.1'),'/')"/>
				</Surname>
			</PersonName>
		</xsl:if>
	</xsl:template>
	<!-- -->
	<xsl:template match="ClientFileSingleTitleList">
		<ProfileInfo>
			<UniqueID>
				<xsl:attribute name="Type">21</xsl:attribute>
				<xsl:attribute name="ID">
					<xsl:value-of select="Title"/>
				</xsl:attribute>
			</UniqueID>
			<Profile>
				<xsl:attribute name="ProfileType">
					<xsl:choose>
						<xsl:when test="FileInd='M'">
							<xsl:value-of select="'Travel Agent'"/>
						</xsl:when>
						<xsl:when test="FileInd='B'">
							<xsl:value-of select="'Corporation'"/>
						</xsl:when>
						<xsl:otherwise><xsl:value-of select="'Customer'"/></xsl:otherwise>
					</xsl:choose>
				</xsl:attribute>
				<xsl:attribute name="RPH">
					<xsl:value-of select="position()"/>
				</xsl:attribute>
			</Profile>
		</ProfileInfo>
	</xsl:template>
</xsl:stylesheet>
