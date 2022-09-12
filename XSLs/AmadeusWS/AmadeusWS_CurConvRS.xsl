<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- ================================================================== -->
	<!-- AmadeusWS_CurConvRS.xsl 														-->
	<!-- ================================================================== -->
	<!-- Date: 22 May 2012 - Rastko - changed mapping due to Amadeus screen changes		-->
	<!-- Date: 27 Sep 2009 - Rastko - added conversion via BSR and ICH				-->
	<!-- Date: 23 Sep 2009 - Rastko - added error handling								-->
	<!-- Date: 21 Sep 2009 - Rastko - added ICH USED process						-->
	<!-- Date: 13 Jun 2009 - Rastko														-->
	<!-- ================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<OTA_CurConvRS>
			<xsl:choose>
				<xsl:when test="contains(Command_CrypticReply/longTextString/textStringDetails,'BSR') or contains(Cryptic_GetScreen_Reply/longTextString/textStringDetails,'USED')">
					<Success />
					<xsl:apply-templates select="Command_CrypticReply" />
				</xsl:when>
				<xsl:otherwise>
					<Errors>
						<Error Type="Amadeus">
							<xsl:value-of select="Command_CrypticReply/longTextString/textStringDetails" />
						</Error>
					</Errors>
				</xsl:otherwise>
			</xsl:choose>
		</OTA_CurConvRS>
	</xsl:template>
	<xsl:template match="Command_CrypticReply">
		<Conversion>
			<xsl:apply-templates select="longTextString" />
		</Conversion>
	</xsl:template>
	
	<xsl:template match="longTextString">
		<xsl:variable name="textStringDetails"><xsl:value-of select="substring(textStringDetails,4)"/></xsl:variable>
		<xsl:variable name="cur1">
			<xsl:value-of select="substring-before($textStringDetails,'/')"/>
		</xsl:variable>
		<xsl:variable name="curL">
			<xsl:value-of select="string-length($cur1)" />
		</xsl:variable>
		<xsl:variable name="cf">
			<xsl:value-of select="substring($cur1,$curL - 2)" />
		</xsl:variable>
		<xsl:variable name="fa">
			<xsl:value-of select="substring($cur1,1,$curL - 3)" />
		</xsl:variable>
		<From>
			<xsl:attribute name="Amount">
				<xsl:value-of select="$fa" />
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="$cf" />
			</xsl:attribute>
		</From>
		<To>
			<xsl:variable name="am1"><xsl:value-of select="substring-before(substring-after($textStringDetails,'ROUNDED AS FARES&#xD;'),' -')"/></xsl:variable>
			<xsl:attribute name="Amount">
				<xsl:value-of select="substring-after($am1,' ')"/>
			</xsl:attribute>
			<xsl:attribute name="CurrencyCode">
				<xsl:value-of select="substring($am1,1,3)" />
			</xsl:attribute>
		</To>
		<ConversionRate>
			<xsl:variable name="cr"><xsl:value-of select="substring-before(substring-after(substring-after($textStringDetails,'BSR USED '),'= '),' ')"/></xsl:variable>
			<xsl:value-of select="$cr" />
		</ConversionRate>
		<Rounding>
			<xsl:variable name="rnd">
				<xsl:variable name="rnd1">
					<xsl:value-of select="substring-after($textStringDetails,'ROUNDING OF OTHER CHARGES ')"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="substring($rnd1,1,2)='UP'"><xsl:value-of select="substring-before(substring-after($rnd1,'UP TO '), ' ')"/></xsl:when>
					<xsl:otherwise><xsl:value-of select="substring-before(substring-after($rnd1,'TO NEAREST '), ' ')"/></xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:value-of select="$rnd" />
		</Rounding>
	</xsl:template>
    
	<!--xsl:template match="longTextString">
		<xsl:variable name="textStringDetails"><xsl:value-of select="substring(textStringDetails,4)"/></xsl:variable>
		<xsl:variable name="euro">
			<xsl:value-of select="string('ATS BEF FIM FRF DEM GRD IEP ITL LUF NLG PTE ESP')" />
		</xsl:variable>
		<xsl:variable name="cur1">
			<xsl:value-of select="substring-before($textStringDetails,'/')"/>
		</xsl:variable>
		<xsl:variable name="curL">
			<xsl:value-of select="string-length($cur1)" />
		</xsl:variable>
		<xsl:variable name="cf">
			<xsl:value-of select="substring($cur1,$curL - 2,3)" />
		</xsl:variable>
		<xsl:variable name="ct">
			<xsl:value-of select="substring(substring-after($textStringDetails,'/'),1,3)" />
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
				<xsl:when test="contains($textStringDetails, 'BSR USED')">
					<xsl:text>BSR USED 1 </xsl:text>
					<xsl:value-of select="$cf" />
					<xsl:text> = </xsl:text>
				</xsl:when>
				<xsl:when test="contains($textStringDetails, 'ICH USED')">
					<xsl:text>ICH USED 1 </xsl:text>
					<xsl:value-of select="$cf" />
					<xsl:text> = </xsl:text>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="am1">
			<xsl:value-of select="substring-before(substring-after($textStringDetails, $c1),' ')" />
		</xsl:variable>
		<xsl:variable name="amc">
			<xsl:choose>
				<xsl:when test="contains($euro, $ct) and contains($euro, $cf)">
					<xsl:variable name="c3">
						<xsl:value-of select="substring-after($textStringDetails, 'EMU USED 1 EUR = ')" />
					</xsl:variable>
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after($c3, 'EMU USED 1 EUR = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am2 div $am1" />
				</xsl:when>
				<xsl:when test="contains($euro, $ct)">
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after($textStringDetails, 'EMU USED 1 EUR = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am1 * $am2" />
				</xsl:when>
				<xsl:when test="contains($euro, $cf)">
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after($textStringDetails, 'BSR USED 1 EUR = '),' ')" />
					</xsl:variable>
					<xsl:value-of select="$am2 div $am1" />
				</xsl:when>
				<xsl:when test="contains($textStringDetails, 'BSR USED') and contains($textStringDetails, 'ICH USED')">
					<xsl:variable name="am2">
						<xsl:value-of select="substring-before(substring-after($textStringDetails, 'ICH USED 1 USD = '),' ')" />
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
	</xsl:template-->
</xsl:stylesheet>
