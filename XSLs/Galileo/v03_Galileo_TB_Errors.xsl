<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ******************************************************************************	-->
	<!-- Message	: Collect Errors and Warnings From Galileo Navitve Responses.		-->
	<!-- Author	: Rastko Ilic													-->
	<!-- Date		: 01 Feb 2010														-->
	<!-- ******************************************************************************	-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<!-- End Transaction Errors -->
		<xsl:apply-templates select="PNRBFEnd_7_0/EndTransaction/MsgBlkAry/MsgBlk" />
		<!-- End Transaction Errors Version 9 -->
		<xsl:apply-templates select="PNRBFEnd_9_0/EndTransactMessage" />
		<!-- Air Segment Error  -->
		<xsl:apply-templates select="PNRBFManagement_17/AirSegSell/AirSell/Status" />
		<xsl:apply-templates select="PNRBFReservationModify_5_0/AirSegSell/AirSellBlkAry/AirSellBlk/Status" />
		<xsl:apply-templates select="PNRBFManagement_17/AirSegSell/ErrText" />
		<xsl:apply-templates select="PNRBFManagement_17[CarSegSell/ErrorCode!='']/CarSegSellText/LineDescAry" />
		<xsl:apply-templates select="PNRBFManagement_17/HtlSegSell/HotelError" />
		<!-- Names, Telephone and Ticketing Time Limit Errors -->
		<xsl:apply-templates select="AgencyPNRBFBuildModify_6_0/PNRBFPrimaryBldChg" />
		<xsl:apply-templates select="PNRBFManagement_17/PNRBFPrimaryBldChg" />
		<!-- Hotel Segment Error  -->
		<xsl:apply-templates select="PNRBFReservationModify_5_0/HtlSegSell/TypeIndEFQual/ErrMsg" />
		<!-- Car Segment Error  -->
		<xsl:apply-templates select="PNRBFReservationModify_5_0/CarSegSell/TypeEWFQual/ErrMsg" />
		<!-- SSRs, OSIs Errors  -->
		<xsl:apply-templates select="AgencyPNRBFBuildModify_6_0/PNRBFSecondaryBldChg" />
		<!-- Fare Quote Errors  -->
		<xsl:apply-templates select="PNRBFManagement_17/FareInfo/FilingStatus[FareFiledOKInd='N']" />
		<xsl:apply-templates select="FareQuoteStorePrice_8_0/FareInfo/ErrText" />
	</xsl:template>
	<xsl:template match="MsgBlk">
		<Error>
			<xsl:choose>
				<xsl:when test="TypeInd='E'">
					<xsl:attribute name="Type">End Transaction</xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
					<xsl:attribute name="Type">
						<xsl:value-of select="TypeInd" />
					</xsl:attribute>
				</xsl:otherwise>
			</xsl:choose>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<xsl:template match="EndTransactMessage">
		<xsl:choose>
			<xsl:when test="TypeInd='Galileo'">
				<Error>
					<xsl:attribute name="Type">
						<xsl:value-of select="TypeInd" />
					</xsl:attribute>
					<xsl:value-of select="Text" />
				</Error>
			</xsl:when>
			<xsl:otherwise>
				<Warning>
					<xsl:value-of select="Text" />
				</Warning>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="Status">
		<xsl:if test="text()='UC'">
			<Error>
				<xsl:attribute name="Type">Galileo</xsl:attribute>
				<xsl:text>Unable to Confirm Flight. Status :</xsl:text>
				<xsl:value-of select="." />
				<xsl:text> Flight </xsl:text>
				<xsl:value-of select="concat(../Vnd,../FltNum)"/>
				<xsl:text> Class:</xsl:text>
				<xsl:value-of select="../Class"/>
			</Error>
		</xsl:if>
	</xsl:template>
	<xsl:template match="FilingStatus">
		<Error>
			<xsl:attribute name="Type">Galileo</xsl:attribute>
			<xsl:value-of select="../OutputMsg/Text" />
		</Error>
	</xsl:template>
	<xsl:template match="PNRBFPrimaryBldChg | PNRBFSecondaryBldChg">
		<xsl:if test="RecID='EROR'">
			<Error>
				<xsl:attribute name="Type">Galileo</xsl:attribute>
				<xsl:value-of select="Text" />
			</Error>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ErrMsg">
		<Error>
			<xsl:value-of select="." />
		</Error>
	</xsl:template>
	<xsl:template match="ErrText">
		<Error>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	<xsl:template match="LineDescAry">
		<xsl:for-each select="LineDescInfo">
			<Error><xsl:value-of select="Txt"/></Error>
		</xsl:for-each>
	</xsl:template>
	<xsl:template match="HotelError">
		<xsl:for-each select="ErrMsgAry/ErrMsg">
			<Error><xsl:value-of select="Msg"/></Error>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
