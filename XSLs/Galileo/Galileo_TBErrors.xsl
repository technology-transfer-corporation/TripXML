<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ******************************************************************************	-->
	<!-- Message	: Collect Errors and Warnings From Galileos Navitve Responses.		-->
	<!-- Input		: Galileos Navitve Responses.										-->
	<!-- Author		: Alexis Consuegra													-->
	<!-- Date		: Sep 23 2004 .														-->
	<!-- ******************************************************************************	-->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<!-- End Transaction Errors -->
		<xsl:apply-templates select="PNRBFEnd_7_0/EndTransaction/MsgBlkAry/MsgBlk" />
		<!-- End Transaction Errors Version 9 -->
		<xsl:apply-templates select="PNRBFEnd_9_0/EndTransactMessage" />
		<!-- Air Segment Error  -->
		<xsl:apply-templates select="PNRBFReservationModify_5_0/AirSegSell/AirSellBlkAry/AirSellBlk/Status" />
		<!-- Names, Telephone and Ticketing Time Limit Errors -->
		<xsl:apply-templates select="AgencyPNRBFBuildModify_6_0/PNRBFPrimaryBldChg" />
		<!-- Hotel Segment Error  -->
		<xsl:apply-templates select="PNRBFReservationModify_5_0/HtlSegSell/TypeIndEFQual/ErrMsg" />
		<!-- Car Segment Error  -->
		<xsl:apply-templates select="PNRBFReservationModify_5_0/CarSegSell/TypeEWFQual/ErrMsg" />
		<!-- SSRs, OSIs Errors  -->
		<xsl:apply-templates select="AgencyPNRBFBuildModify_6_0/PNRBFSecondaryBldChg" />
		<!-- Fare Quote Errors  -->
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
			<xsl:when test="TypeInd='E'">
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
		<xsl:if test="text()='UC' or text()='NN'">
			<Error>
				<xsl:attribute name="Type">E</xsl:attribute>
				<xsl:text>Unable to Confirm Flight. Status :</xsl:text>
				<xsl:value-of select="." />
			</Error>
		</xsl:if>
	</xsl:template>
	<xsl:template match="PNRBFPrimaryBldChg | PNRBFSecondaryBldChg">
		<xsl:if test="RecID='EROR'">
			<Error>
				<xsl:attribute name="Type">E</xsl:attribute>
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
</xsl:stylesheet>
