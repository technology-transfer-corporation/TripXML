<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
	AmadeusWS_IssueTicketRS.xsl 													
	================================================================== 
  Date: 03 Nov 2016 - Kobelev - corrected mapping of good result relative path vs. hardcoded
	Date: 01 Dec 2014 - Rastko - corrected mapping of good result for DocIssuance_IssueTicketReply		
	Date: 23 Mar 2012 - Shashin - added Error handle for timeout from amadeus 		
	Date: 12 Jan 2012 - Rastko - added mapping of DocIssuance_IssueTicket			
	Date: 16 Feb 2011 - Rastko - added mapping of record locator				
	Date: 15 Feb 2011 - Rastko - added mapping of issued ticket number			
	Date: 20 Dec 2009 - Rastko														
	================================================================== -->
	<xsl:output method="xml" omit-xml-declaration="yes" />
	<xsl:template match="/">
		<xsl:apply-templates select="PNR_Reply" mode="ok"/>
		<xsl:variable name="resp">
			<xsl:choose>
				<xsl:when test="Command_CrypticReply">
					<xsl:value-of select="Command_CrypticReply/longTextString/textStringDetails"/>
				</xsl:when>
				<xsl:when test="DocIssuance_IssueTicketReply">
					<xsl:choose>
						<xsl:when test="DocIssuance_IssueTicketReply/processingStatus/statusCode='O'">
							<xsl:value-of select="'OK '"/>
						</xsl:when>
						<xsl:when test="DocIssuance_IssueTicketReply/errorGroup/errorOrWarningCodeDetails/errorDetails/errorCode='OK'">
							<xsl:value-of select="'OK '"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="DocIssuance_IssueTicketReply/errorGroup/errorWarningDescription/freeText"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="MessagesOnly_Reply/CAPI_Messages/Text"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="contains($resp,'OK ') or contains($resp,'RP/')">
				<xsl:apply-templates select="MessagesOnly_Reply" mode="ok"/>
				<xsl:apply-templates select="Command_CrypticReply" mode="ok"/>
				<xsl:apply-templates select="DocIssuance_IssueTicketReply" mode="ok"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:apply-templates select="MessagesOnly_Reply" mode="error"/>
				<xsl:apply-templates select="Command_CrypticReply" mode="error"/>
				<xsl:apply-templates select="DocIssuance_IssueTicketReply" mode="error"/>
				<xsl:apply-templates select="Error" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="MessagesOnly_Reply | Command_CrypticReply | DocIssuance_IssueTicketReply|Error" mode="error">
		<TT_IssueTicketRS Version="1.001">
			<Errors>
				<xsl:apply-templates select="CAPI_Messages" />
				<xsl:apply-templates select="longTextString/textStringDetails" />
				<xsl:apply-templates select="errorGroup/errorWarningDescription/freeText" />
				<xsl:apply-templates select="Error" />
			</Errors>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="PNR_Reply | MessagesOnly_Reply | Command_CrypticReply | DocIssuance_IssueTicketReply" mode="ok">
		<TT_IssueTicketRS Version="1.001">
			<Success/>
      <xsl:choose>
        <xsl:when test="./pnrHeader">
          <xsl:apply-templates select="." />
        </xsl:when>
        <xsl:when test="./PNR_Reply/pnrHeader">
          <xsl:apply-templates select="./PNR_Reply" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:apply-templates select="PNR_Reply" />
        </xsl:otherwise>
      </xsl:choose>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="CAPI_Messages">
		<Error>
			<xsl:attribute name="Code">
				<xsl:value-of select="ErrorCode" />
			</xsl:attribute>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="Text" />
		</Error>
	</xsl:template>
	
	<xsl:template match="Error">
		<TT_IssueTicketRS Version="1.001">
			<Errors>
				<Error>
					<xsl:value-of select="." />
				</Error>
			</Errors>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="textStringDetails | freeText">	
		<Error>
			<xsl:attribute name="Type">Amadeus</xsl:attribute>
			<xsl:value-of select="." />
		</Error>
	</xsl:template>

  <xsl:template match="PNR_Reply">
    <UniqueID>
      <xsl:attribute name="ID">
        <xsl:value-of select="pnrHeader/reservationInfo/reservation/controlNumber"/>
      </xsl:attribute>
    </UniqueID>
    <TicketingControl>
      <Type>OK</Type>
    </TicketingControl>
    <xsl:for-each select="dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA'] | dataElementsMaster/dataElementsIndiv[elementManagementData/segmentName='FA']">
      <Ticket>
        <xsl:attribute name="Type">
          <xsl:choose>
            <xsl:when test="substring(substring-after(otherDataFreetext/longFreetext,'/'),1,1)='E'">Electronic</xsl:when>
            <xsl:otherwise>Paper</xsl:otherwise>
          </xsl:choose>
        </xsl:attribute>
        <xsl:attribute name="Number">
          <xsl:variable name="tn">
            <xsl:value-of select="substring-before(substring(otherDataFreetext/longFreetext,5),'/')"/>
          </xsl:variable>
          <xsl:value-of select="concat(substring-before($tn,'-'),substring-after($tn,'-'))"/>
        </xsl:attribute>
        <xsl:if test="Name!=''">
          <Name>
            <xsl:value-of select="Name"/>
          </Name>
        </xsl:if>
        <xsl:if test="Fare!=''">
          <Fare>
            <xsl:attribute name="Amount">
              <xsl:value-of select="normalize-space(Fare)"/>
            </xsl:attribute>
            <xsl:attribute name="Currency">
              <xsl:value-of select="Currency"/>
            </xsl:attribute>
          </Fare>
        </xsl:if>
      </Ticket>
    </xsl:for-each>
  </xsl:template>
	
</xsl:stylesheet>