<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!--
  ================================================================== 
	AmadeusWS_SalesReportRQ.xsl 												
	================================================================== 
	Date: 29 Aug 2013 - Rastko - added support for dates 							
	Date: 18 Oct 2012 - Rastko - new file												
	================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="SalesReportRQ" />
	</xsl:template>
	<xsl:template match="SalesReportRQ">
		<Report>
			<Sales>
				<Command_Cryptic>
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<textStringDetails>
							<xsl:choose>
								<xsl:when test="Entry='DailyReport'">
									<xsl:value-of select="'TJD'"/>
									<xsl:choose>
										<xsl:when test="ReportDate!=''">
											<xsl:value-of select="concat('/D-',substring(ReportDate,9))"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring(ReportDate,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
										</xsl:when>
										<xsl:when test="ReportDateRange/@Start!='' and ReportDateRange/@End!=''">
											<xsl:value-of select="concat('/D-',substring(ReportDateRange/@Start,9))"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring(ReportDateRange/@Start,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
											<xsl:value-of select="substring(ReportDateRange/@End,9)"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring(ReportDateRange/@End,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="Entry='QueryReport'">
									<xsl:value-of select="'TJQ'"/>
									<xsl:choose>
										<xsl:when test="ReportDate!=''">
											<xsl:value-of select="concat('/D-',substring(ReportDate,9))"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring(ReportDate,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
										</xsl:when>
										<xsl:when test="ReportDateRange/@Start!='' and ReportDateRange/@End!=''">
											<xsl:value-of select="concat('/D-',substring(ReportDateRange/@Start,9))"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring(ReportDateRange/@Start,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
											<xsl:value-of select="substring(ReportDateRange/@End,9)"/>
											<xsl:call-template name="month">
												<xsl:with-param name="month">
													<xsl:value-of select="substring(ReportDateRange/@End,6,2)"/>
												</xsl:with-param>
											</xsl:call-template>
										</xsl:when>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'IG'"/>
								</xsl:otherwise>
							</xsl:choose>
						</textStringDetails>
					</longTextString>
				</Command_Cryptic>
			</Sales>
			<MD>
				<Command_Cryptic>
					<messageAction>
						<messageFunctionDetails>
							<messageFunction>M</messageFunction>
						</messageFunctionDetails>
					</messageAction>
					<longTextString>
						<textStringDetails>MD</textStringDetails>
					</longTextString>
				</Command_Cryptic>
			</MD>
		</Report>
	</xsl:template>
	
	<xsl:template name="month">
		<xsl:param name="month"/>
		<xsl:choose>
			<xsl:when test="$month = '01'">JAN</xsl:when>
			<xsl:when test="$month = '02'">FEB</xsl:when>
			<xsl:when test="$month = '03'">MAR</xsl:when>
			<xsl:when test="$month = '04'">APR</xsl:when>
			<xsl:when test="$month = '05'">MAY</xsl:when>
			<xsl:when test="$month = '06'">JUN</xsl:when>
			<xsl:when test="$month = '07'">JUL</xsl:when>
			<xsl:when test="$month = '08'">AUG</xsl:when>
			<xsl:when test="$month = '09'">SEP</xsl:when>
			<xsl:when test="$month = '10'">OCT</xsl:when>
			<xsl:when test="$month = '11'">NOV</xsl:when>
			<xsl:when test="$month = '12'">DEC</xsl:when>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
