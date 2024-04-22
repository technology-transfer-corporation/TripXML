<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="xml" omit-xml-declaration="yes"/>

	<xsl:template match="/">
		
			<xsl:choose>
				<xsl:when test="not(contains(SabreCommandLLSRS/Response, 'DPTR      ARVL    MEALS'))">
					<xsl:call-template name="bad"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="good"/>
				</xsl:otherwise>
			</xsl:choose>

		
	</xsl:template>

	<xsl:template name="good">
		<OTA_ShowMileageRS Version="1.0">
			<Success/>
			<xsl:choose>
				<xsl:when test="MileageRS">
					<xsl:call-template name="show"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="gen"/>
				</xsl:otherwise>
			</xsl:choose>

			<xsl:if test="ConversationID != ''">
				<ConversationID>
					<xsl:value-of select="SabreCommandLLSRS/ConversationID"/>
				</ConversationID>
			</xsl:if>
		</OTA_ShowMileageRS>
	</xsl:template>

	<xsl:template name="bad">
		<OTA_ShowMileageRS Version="1.0">
			<Errors>
				<Error>
					<xsl:value-of select="SabreCommandLLSRS/Response"/>
				</Error>
			</Errors>
			<xsl:if test="ConversationID != ''">
				<ConversationID>
					<xsl:value-of select="ConversationID"/>
				</ConversationID>
			</xsl:if>
		</OTA_ShowMileageRS>
	</xsl:template>

	<xsl:template name="show">
		<FromCity>
			<xsl:value-of select="MileageRS/OriginInfo/OriginLocation/@LocationCode"/>
		</FromCity>
		<xsl:apply-templates select="MileageRS/LineNumber"/>
	</xsl:template>

	<xsl:template name="gen">
		<xsl:variable name="lines">
			<xsl:call-template name="split">
				<!-- store anything left in another variable -->
				<xsl:with-param name="text" select="SabreCommandLLSRS/Response/text()"/>
			</xsl:call-template>
		</xsl:variable>
		<Remarks>
			<xsl:for-each select="msxsl:node-set($lines)[1]/node()">
				<Remark>
					<xsl:value-of select="."/>
				</Remark>
			</xsl:for-each>
		</Remarks>
	</xsl:template>

	<xsl:template match="LineNumber">
		<ToCity>
			<xsl:attribute name="Mileage">
				<xsl:value-of select="TicketedPointMileage/@Code"/>
			</xsl:attribute>
			<xsl:attribute name="AccumulativeMileage">
				<xsl:value-of select="CumulativeTicketedPointMileage/@Code"/>
			</xsl:attribute>
			<xsl:value-of select="DestinationLocation/@LocationCode"/>
		</ToCity>
		<xsl:if test="position() = last()">
			<TotalMileage>
				<xsl:value-of select="CumulativeTicketedPointMileage/@Code"/>
			</TotalMileage>
		</xsl:if>
	</xsl:template>

	<!--############################################################-->
	<!--## Template to tokenize strings                           ##-->
	<!--############################################################-->
	<xsl:template name="tokenizeString">
		<!--passed template parameter -->
		<xsl:param name="list"/>
		<xsl:param name="delimiter"/>
		<xsl:choose>
			<xsl:when test="contains($list, $delimiter)">
				<elem>
					<!-- get everything in front of the first delimiter -->
					<xsl:value-of select="substring-before($list,$delimiter)"/>
				</elem>
				<xsl:call-template name="tokenizeString">
					<!-- store anything left in another variable -->
					<xsl:with-param name="list" select="substring-after($list,$delimiter)"/>
					<xsl:with-param name="delimiter" select="$delimiter"/>
				</xsl:call-template>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$list = ''">
						<xsl:text/>
					</xsl:when>
					<xsl:otherwise>
						<elem>
							<xsl:value-of select="$list"/>
						</elem>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="split">
		<xsl:param name="text" select="."/>
		<xsl:if test="string-length($text) > 0">
			<xsl:variable name="output-text">
				<xsl:value-of select="normalize-space(substring-before(concat($text, '&#xA;'), '&#xA;'))"/>
			</xsl:variable>
			<xsl:choose>
				<xsl:when test="string-length($output-text) &lt; 63">
					<xsl:if test="normalize-space($output-text) != ''">
						<Remark>
							<xsl:value-of select="$output-text"/>
						</Remark>
					</xsl:if>

					<xsl:call-template name="split">
						<xsl:with-param name="text" select="substring-after($text, '&#xA;')"/>
					</xsl:call-template>
				</xsl:when>
				<xsl:otherwise>
					<xsl:if test="normalize-space($output-text) != ''">
						<Remark SEGMENT="1">
							<xsl:value-of select="substring($output-text, 1, 63)"/>
						</Remark>
					</xsl:if>
					<xsl:call-template name="split">
						<xsl:with-param name="text" select="substring($text, 63 )"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>




