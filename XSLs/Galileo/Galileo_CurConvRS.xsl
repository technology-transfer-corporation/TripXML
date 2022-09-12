<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Galileo_CurConvRS.xsl 															-->
<!-- ================================================================== -->
<!-- Date: 14 Feb 2007 - Rastko														-->
<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="CurrencyConversion_6_0" />
	</xsl:template>
	<xsl:template match="CurrencyConversion_6_0">
		<OTA_CurConvRS>
			<xsl:apply-templates select="CurrencyConversion" />
		</OTA_CurConvRS>
	</xsl:template>
	<xsl:template match="CurrencyConversion">
		<xsl:apply-templates select="TypeErrQual" />
		<xsl:apply-templates select="TypeConvQual/PairStatusAry/PairStatus" />
	</xsl:template>
	<xsl:template match="TypeErrQual">
		<Errors>
			<Error Type="Galileo" Code="{ErrNum}">
				<xsl:value-of select="ErrMsg" />
			</Error>
		</Errors>
	</xsl:template>
	<xsl:template match="PairStatus">
		<xsl:choose>
			<xsl:when test="PairStatusInd = '0'">
				<xsl:variable name="zeros">0000000000</xsl:variable>
				<xsl:variable name="dec">
					<xsl:value-of select="DecPos2" />
				</xsl:variable>
				<Success />
				<Conversion>
					<From>
						<xsl:attribute name="Amount">
							<xsl:variable name="deca">
								<xsl:value-of select="DecPos1" />
							</xsl:variable>
							<xsl:text>1.</xsl:text>
							<xsl:value-of select="substring($zeros,1,$deca)" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Currency1" />
						</xsl:attribute>
					</From>
					<xsl:variable name="convrate1">
						<xsl:variable name="conv1">
							<xsl:value-of select="ConvInd1" />
						</xsl:variable>
						<xsl:variable name="conv2">
							<xsl:value-of select="ConvInd2" />
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="Currency3 = ''">
								<xsl:choose>
									<xsl:when test="Calc1TypeInd = 'M'">
										<xsl:value-of select="ConvInd2 div 1000" />
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$conv2 div $conv1" />
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$conv2 div $conv1" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="BeforeDecimal">
						<xsl:value-of select="substring-before($convrate1,'.')" />
					</xsl:variable>
					<xsl:variable name="AfterDecimal">
						<xsl:value-of select="substring-after($convrate1,'.')" />
					</xsl:variable>
					<xsl:variable name="AfterDecimal3">
						<xsl:choose>
							<xsl:when test="string-length($AfterDecimal) > 5">
								<xsl:value-of select="substring(substring($AfterDecimal,1,6) + 5,1,5)" />
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$AfterDecimal" />
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="convrate">
						<xsl:choose>
							<xsl:when test="$BeforeDecimal=''">
								<xsl:value-of select="$convrate1" />
							</xsl:when>
							<xsl:otherwise>
							<xsl:value-of select="$BeforeDecimal" />.<xsl:value-of select="$AfterDecimal3" />
						</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<To>
						<xsl:attribute name="Amount">
							<xsl:value-of select="$convrate" />
						</xsl:attribute>
						<xsl:attribute name="CurrencyCode">
							<xsl:value-of select="Currency2" />
						</xsl:attribute>
					</To>
					<ConversionRate>
						<xsl:value-of select="$convrate" />
					</ConversionRate>
					<Rounding>
						<xsl:choose>
							<xsl:when test="string-length($AfterDecimal3) = 5">0.000005</xsl:when>
							<xsl:otherwise>0</xsl:otherwise>
						</xsl:choose>
					</Rounding>
				</Conversion>
			</xsl:when>
			<xsl:otherwise>
				<Errors>
					<Error Type="Galileo" Code="{PairStatusInd}">
						<xsl:choose>
							<xsl:when test="PairStatusInd = '1'">
								<xsl:text>BASE CURRENCY NOT FOUND</xsl:text>
							</xsl:when>
							<xsl:when test="PairStatusInd = '2'">
								<xsl:text>BASE CURRENCY RATE NOT IN USE</xsl:text>
							</xsl:when>
							<xsl:when test="PairStatusInd = '3'">
								<xsl:text>SECOND CURRENCY NOT FOUND</xsl:text>
							</xsl:when>
							<xsl:when test="PairStatusInd = '4'">
								<xsl:text>SECOND CURRENCY RATE NOT IN USE</xsl:text>
							</xsl:when>
							<xsl:when test="PairStatusInd = '5'">
								<xsl:text>TRIANGULATION CURRENCY 3 NOT FOUND</xsl:text>
							</xsl:when>
							<xsl:when test="PairStatusInd = '6'">
								<xsl:text>TRIANGULATION CURRENCY 3 RATE NOT IN USE</xsl:text>
							</xsl:when>
							<xsl:otherwise>
								<xsl:text>UNKNOWN GALILEO ERROR</xsl:text>
							</xsl:otherwise>
						</xsl:choose>
					</Error>
				</Errors>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
