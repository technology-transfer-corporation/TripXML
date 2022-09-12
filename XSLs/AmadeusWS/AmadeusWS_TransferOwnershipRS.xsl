<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_TransferOwnershipRS.xsl 		  	       						           -->
	<!-- ================================================================== -->
	<!-- Date: 08 Sep 2011 - Rastko - add mapping of PNR_Reply					            -->
	<!-- Date: 06 Apr 2010 - Rastko													            -->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<OTA_TransferOwnershipRS>
			<xsl:attribute name="Version">1.0</xsl:attribute>
			<xsl:choose>
				<xsl:when test="MessagesOnly_Reply != ''">
					<xsl:apply-templates select="MessagesOnly_Reply"/>
				</xsl:when>
				<xsl:when test="Errors">
					<xsl:apply-templates select="Errors"/>
				</xsl:when>
				<xsl:when test="Command_CrypticReply">
					<xsl:apply-templates select="Command_CrypticReply"/>
				</xsl:when>
				<xsl:when test="PNR_Reply">
					<xsl:apply-templates select="PNR_Reply"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="PoweredPNR_TransferOwnershipReply"/>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_TransferOwnershipRS>
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply">
		<Errors>
			<Error>
				<xsl:attribute name="Type">Amadeus</xsl:attribute>
				<xsl:attribute name="Code"><xsl:value-of select="CAPI_Messages/ErrorCode"/></xsl:attribute>
				<xsl:value-of select="CAPI_Messages/Text"/>
			</Error>
		</Errors>
	</xsl:template>
	<xsl:template match="Errors">
		<Errors>
			<xsl:for-each select="Error">
				<Error>
					<xsl:attribute name="Type">Amadeus</xsl:attribute>
					<xsl:attribute name="Code"><xsl:value-of select="@Code"/></xsl:attribute>
					<xsl:value-of select="."/>
				</Error>
			</xsl:for-each>
		</Errors>
	</xsl:template>
	<xsl:template match="PoweredPNR_TransferOwnershipReply">
		<xsl:choose>
			<xsl:when test="officeIdentification/officeError">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Amadeus</xsl:attribute>
						<xsl:attribute name="Code"><xsl:value-of select="officeIdentification/officeError/errorNumber/errorDetails/errorCode"/></xsl:attribute>
						<xsl:value-of select="officeIdentification/officeError/errorFreeText/freeText"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<UniqueID>
					<xsl:attribute name="ID"><xsl:value-of select="recordLocator/reservation/controlNumber"/></xsl:attribute>
				</UniqueID>
				<NewOwner>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="officeIdentification/officeIdentificator/originatorDetails/inHouseIdentification1"/>
					</xsl:attribute>
				</NewOwner>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Command_CrypticReply">
		<xsl:choose>
			<xsl:when test="contains(longTextString/textStringDetails,'END OF TRANSACTION COMPLETE')">
				<Success/>
				<UniqueID>
					<xsl:attribute name="ID"><xsl:value-of select="substring(substring-after(longTextString/textStringDetails,' -'),1,6)"/></xsl:attribute>
				</UniqueID>
				<NewOwner>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="OTA_TransferOwnershipRQ/NewOwner/@PseudoCityCode"/>
					</xsl:attribute>
				</NewOwner>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<Error>
						<xsl:attribute name="Type">Amadeus</xsl:attribute>
						<xsl:value-of select="substring-after(longTextString/textStringDetails,'/')"/>
					</Error>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="PNR_Reply">
		<xsl:choose>
			<xsl:when test="generalErrorInfo[messageErrorInformation/errorDetail/qualifier='EC'][messageErrorText/freetextDetail/subjectQualifier='3']/messageErrorText/text!= ''">
				<Errors>
					<Error>
						<xsl:attribute name="Type">Amadeus</xsl:attribute>
						<xsl:value-of select="generalErrorInfo[messageErrorInformation/errorDetail/qualifier='EC'][messageErrorText/freetextDetail/subjectQualifier='3']/messageErrorText/text"/>
					</Error>
				</Errors>
			</xsl:when>
			<xsl:otherwise>
				<Success/>
				<UniqueID>
					<xsl:attribute name="ID"><xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber"/></xsl:attribute>
				</UniqueID>
				<NewOwner>
					<xsl:attribute name="PseudoCityCode">
						<xsl:value-of select="OTA_TransferOwnershipRQ/NewOwner/@PseudoCityCode"/>
					</xsl:attribute>
				</NewOwner>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
