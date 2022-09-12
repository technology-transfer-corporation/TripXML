<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
	<!-- Amadeus_CurConvRS.xsl 															-->
	<!-- ================================================================== -->
	<!-- Date: 27 Sep 2009 - Rastko - added conversion via BSR and ICH				-->
	<!-- Date: 23 Sep 2009 - Rastko - added error handling								-->
	<!-- Date: 21 Sep 2009 - Rastko - added ICH USED process						-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_CurConvRS>
			<xsl:choose>
				<xsl:when test="contains(Cryptic_GetScreen_Reply/CAPI_Screen/Response,'BSR') or contains(Cryptic_GetScreen_Reply/CAPI_Screen/Response,'USED')">
					<Success />
					<xsl:apply-templates select="Cryptic_GetScreen_Reply" />
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error Type="Amadeus">
							<xsl:value-of select="Cryptic_GetScreen_Reply/CAPI_Screen/Response" />
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_CurConvRS>
	</xsl:template>
	<xsl:template match="Cryptic_GetScreen_Reply">
		<Conversion>
			<xsl:apply-templates select="CAPI_Screen" />
		</Conversion>
	</xsl:template>
	<xsl:template match="CAPI_Screen">
		<xsl:variable name="euro">
			<xsl:value-of select="string('ATS BEF FIM FRF DEM GRD IEP ITL LUF NLG PTE ESP')" />
		</xsl:variable>
		<xsl:variable name="cur1">
			<xsl:value-of select="substring-before(Response,'/')" />
		</xsl:variable>
		<xsl:variable name="curL">
			<xsl:value-of select="string-length($cur1)" />
		</xsl:variable>
		<xsl:variable name="cf">
			<xsl:value-of select="substring($cur1,$curL - 2,3)" />
		</xsl:variable>
		<xsl:variable name="ct">
			<xsl:value-of select="substring(substring-after(Response,'/'),1,3)" />
		</xsl:variable>
		<xsl:variable name="fa">
			<xsl:choose>
				<xsl:when test="$curL=6">1</xsl:when>
				<xsl:when test="$curL=7">
					<xsl:choose>
						<xsl:when test="substring($cur1,4,1)=' '">1</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="substring($cur1,4,1)" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring($cur1,4,$curL - 6)" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<From>
			<xsl:attribute name="Amount">
				<xsl:value-of select="$fa" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="$cf" />
			</xsl:attribute>
		</From> 
		<xsl:variable name="c1">
			<xsl:choose>
				<xsl:when test="contains($euro, $cf)">
					<xsl:text>EMU USED 1 EUR = </xsl:text>
				</xsl:when>
				<xsl:when test="contains(Response, 'BSR USED')">
					<xsl:text>BSR USED 1 </xsl:text>
					<xsl:value-of select="$cf" />
					<xsl:text> = </xsl:text>
				</xsl:when>
				<xsl:when test="contains(Response, 'ICH USED')">
					<xsl:text>ICH USED 1 </xsl:text>
					<xsl:value-of select="$cf" />
					<xsl:text> = </xsl:text>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="am1">
			<xsl:value-of select="substring-before(substring-after(Response, $c1),' ')" />
		</xsl:variable>
		<xsl:variable name="amc">
			<xsl:choose>
				<xsl:when test="contains($euro, $ct) and contains($euro, $cf)">
					<xsl:variable name="c3">
						<xsl:value-of select="substring-after(Response, 'EMU USED 1 EUR = ')" />
					</xsl:variable>
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after($c3, 'EMU USED 1 EUR = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am2 div $am1" />
				</xsl:when>
				<xsl:when test="contains($euro, $ct)">
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after(Response, 'EMU USED 1 EUR = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am1 * $am2" />
				</xsl:when>
				<xsl:when test="contains($euro, $cf)">
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after(Response, 'BSR USED 1 EUR = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am2 div $am1" />
				</xsl:when>
				<xsl:when test="contains(Response, 'BSR USED') and contains(Response, 'ICH USED')">
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after(Response, 'ICH USED 1 USD = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am2 * $am1" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$am1" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="tad">
			<xsl:value-of select="$amc * $fa" />
		</xsl:variable>
		<xsl:variable name="BeforeDecimal">
			<xsl:value-of select="substring-before($tad,'.')" />
		</xsl:variable>
		<xsl:variable name="AfterDecimal">
			<xsl:value-of select="substring-after($tad,'.')" />
		</xsl:variable>
		<xsl:variable name="AfterDecimal3">
			<xsl:value-of select="substring($AfterDecimal,1,1)" />
		</xsl:variable>
		<xsl:variable name="amcd">
			<xsl:choose>
				<xsl:when test="$BeforeDecimal = ''">
					<xsl:value-of select="$tad * 10" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$BeforeDecimal" />
					<xsl:value-of select="$AfterDecimal3" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="xxx">
			<xsl:value-of select="5" />
		</xsl:variable>
		<xsl:variable name="rnd">
			<xsl:value-of select="$amcd + $xxx" />
		</xsl:variable>
		<xsl:variable name="ta">
			<xsl:value-of select="substring($rnd,1,string-length($rnd)-1)" />
		</xsl:variable>
		<To>
			<xsl:attribute name="Amount">
				<xsl:choose>
					<xsl:when test="$ta = ''">0</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$fa * $amc" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="$ct" />
			</xsl:attribute>
		</To>
		<xsl:variable name="BeforeDec">
			<xsl:value-of select="substring-before($amc,'.')" />
		</xsl:variable>
		<xsl:variable name="AfterDec">
			<xsl:value-of select="substring-after($amc,'.')" />
		</xsl:variable>
		<xsl:variable name="AfterDec3">
			<xsl:value-of select="substring($AfterDec,1,4)" />
		</xsl:variable>
		<xsl:variable name="cr">
			<xsl:value-of select="$BeforeDec" />
			<xsl:value-of select="$AfterDec3" />
		</xsl:variable>
		<xsl:variable name="rndcr">
			<xsl:value-of select="$cr + $xxx" />
		</xsl:variable>
		<xsl:variable name="tacr">
			<xsl:value-of select="substring($rndcr,1,string-length($rndcr)-1)" />
		</xsl:variable>
		<ConversionRate>
			<xsl:value-of select="$amc" />
		</ConversionRate>
		<Rounding>
			<xsl:value-of select="string-length(substring-after($amc,'.'))" />
		</Rounding>
	</xsl:template>
</xsl:stylesheet>
