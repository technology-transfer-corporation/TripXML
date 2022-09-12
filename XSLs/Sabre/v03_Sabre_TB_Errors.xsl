<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" version="1.0">
  <!-- 
  ================================================================== 
  v03_Sabre_TB_Errors.xsl                           									                   
  ================================================================== 
  Date: 31 May 2021 - Kobelev - Upgrade to new GerReservationRS Method			      
  Date: 25 Feb 2014 - Rastko - catch add remark rs error as warning			      
  Date: 25 Feb 2014 - Rastko - return cryptic error response as is				      
  Date: 31 Jan 2014 - Rastko - added support for cryptic OK response in general
  Date: 30 Jan 2014 - Rastko - added support for cryptic OK response			      
  Date: 18 Jan 2014 - Rastko - added mapping for AirTicketRS and AddAccountingLineRS      
  Date: 17 Sep 2010 - Rastko - added mapping for addRemarks				                  
  Date: 20 Apr 2010 - Rastko - changed code to collect different status codes	      
  ================================================================== 
  -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="TravelItineraryAddInfoRS/Errors/Error" mode="error"/>
    <xsl:apply-templates select="OTA_AirBookRS/Errors/Error" mode="error"/>
    <xsl:apply-templates select="OTA_AirPriceRS/Errors/Error" mode="error"/>
    <xsl:apply-templates select="OTA_AirPriceRS/ApplicationResults/Error" mode="error"/>
    <xsl:apply-templates select="AddRemarkRS/Errors/Error" mode="warning"/>
    <xsl:apply-templates select="AddRemarkRS/ApplicationResults/Error" mode="warning"/>
    <xsl:apply-templates select="SabreCommandLLSRS" mode="error"/>
    <xsl:apply-templates select="AirTicketRS/Errors/Error" mode="warning"/>
    <xsl:apply-templates select="AddAccountingLineRS/Errors/Error" mode="warning"/>
    <xsl:apply-templates select="GetReservationRS/Errors/Error" mode="errorGR"/>
    <xsl:apply-templates select="TravelItineraryReadRS/ApplicationResults[@status='NotProcessed']/Error" mode="error"/>
    <xsl:apply-templates select="OTA_HotelResRS/ApplicationResults[@status='NotProcessed']/Error" mode="error"/>
  </xsl:template>

  <xsl:template match="Error" mode="errorGR">
    <Error>
      <xsl:attribute name="Type">Sabre</xsl:attribute>
      <xsl:value-of select="Message"/>
    </Error>
  </xsl:template>

  <xsl:template match="Error" mode="error">
    <Error>
      <xsl:attribute name="Type">Sabre</xsl:attribute>
      <xsl:value-of select="ErrorInfo/Message"/>
      <xsl:value-of select="SystemSpecificResults/Message"/>
    </Error>
  </xsl:template>

  <xsl:template match="Error" mode="warning">
    <Warning>
      <xsl:value-of select="ErrorInfo/Message"/>
      <xsl:value-of select="SystemSpecificResults/Message"/>
    </Warning>
  </xsl:template>

  <xsl:template match="text" mode="warning">
    <Warning>
      <xsl:attribute name="Type">Sabre</xsl:attribute>
      <xsl:value-of select="." />
    </Warning>
  </xsl:template>

  <xsl:template match="SabreCommandLLSRS" mode="error">
    <xsl:if test="not(contains(Response,'OK ')) and not(contains(Response,'ETR MESSAGE PROCESSED')) and not(contains(Response,'*'))">
      <Error>
        <xsl:attribute name="Type">Sabre</xsl:attribute>
        <!--xsl:choose>
					<xsl:when test="contains(Response,'INVLD') or contains(Response,'INVALID')"-->
        <xsl:value-of select="Response"/>
        <!--/xsl:when>
					<xsl:otherwise>
						<xsl:text>*IM AND CANCEL UNABLE SEGMENTS</xsl:text>
					</xsl:otherwise>
				</xsl:choose-->
      </Error>
      <xsl:call-template name="GetFlights">
        <xsl:with-param name="resp" select="Response"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template name="GetFlights">
    <xsl:param name="resp"/>
    <xsl:if test="contains($resp,' UC') or contains($resp,' LL') or contains($resp,' WL') or contains($resp,' HL')">
      <xsl:variable name="stCode">
        <xsl:variable name="STC">
          <xsl:element name="code">
            <xsl:attribute name="length">
              <xsl:value-of select="string-length(substring-before($resp,' UC'))"/>
            </xsl:attribute>
            <xsl:value-of select="' UC'"/>
          </xsl:element>
          <xsl:element name="code">
            <xsl:attribute name="length">
              <xsl:value-of select="string-length(substring-before($resp,' LL'))"/>
            </xsl:attribute>
            <xsl:value-of select="' LL'"/>
          </xsl:element>
          <xsl:element name="code">
            <xsl:attribute name="length">
              <xsl:value-of select="string-length(substring-before($resp,' WL'))"/>
            </xsl:attribute>
            <xsl:value-of select="' WL'"/>
          </xsl:element>
          <xsl:element name="code">
            <xsl:attribute name="length">
              <xsl:value-of select="string-length(substring-before($resp,' HL'))"/>
            </xsl:attribute>
            <xsl:value-of select="' HL'"/>
          </xsl:element>
        </xsl:variable>
        <xsl:apply-templates select="msxsl:node-set($STC)/code[@length>0]">
          <xsl:sort data-type="number" order="ascending" select="@length"/>
        </xsl:apply-templates>
      </xsl:variable>
      <xsl:variable name="flight">
        <xsl:value-of select="substring($resp,string-length(substring-before($resp,$stCode)) - 21)"/>
      </xsl:variable>
      <xsl:element name="Error">
        <xsl:attribute name="Type">Sabre</xsl:attribute>
        <xsl:text>Flight </xsl:text>
        <xsl:value-of select="substring($flight,1,2)"/>
        <xsl:value-of select="format-number(substring($flight,3,4),'0000')"/>
        <xsl:text> Class </xsl:text>
        <xsl:value-of select="substring($flight,7,1)"/>
        <xsl:variable name="month">
          <xsl:call-template name="month">
            <xsl:with-param name="month">
              <xsl:value-of select="substring($flight,11,3)"/>
            </xsl:with-param>
          </xsl:call-template>
        </xsl:variable>
        <xsl:variable name="day">
          <xsl:value-of select="substring($flight,9,2)"/>
        </xsl:variable>
        <xsl:text> Date </xsl:text>
        <xsl:variable name="cyear">
          <xsl:value-of select="substring(CurrentDate,7)"/>
        </xsl:variable>
        <xsl:variable name="cmonth">
          <xsl:value-of select="substring(CurrentDate,1,2)"/>
        </xsl:variable>
        <xsl:variable name="cday">
          <xsl:value-of select="substring(CurrentDate,4,2)"/>
        </xsl:variable>
        <xsl:choose>
          <xsl:when test="$month &lt; $cmonth">
            <xsl:value-of select="$cyear + 1"/>
          </xsl:when>
          <xsl:when test="$month=$cmonth and $day &lt; $cday">
            <xsl:value-of select="$cyear + 1"/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="$cyear"/>
          </xsl:otherwise>
        </xsl:choose>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="$month"/>
        <xsl:text>-</xsl:text>
        <xsl:value-of select="$day"/>
        <xsl:text> - </xsl:text>
        <xsl:choose>
          <xsl:when test="contains($stCode,'L')">Wait listed</xsl:when>
          <xsl:otherwise>Unable to sell</xsl:otherwise>
        </xsl:choose>
      </xsl:element>
      <xsl:call-template name="GetFlights">
        <xsl:with-param name="resp">
          <xsl:value-of select="substring-after($resp,$stCode)"/>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>

  <xsl:template match="code">
    <xsl:if test="position()=1">
      <xsl:value-of select="."/>
    </xsl:if>
  </xsl:template>

  <xsl:template name="month">
    <xsl:param name="month" />
    <xsl:choose>
      <xsl:when test="$month = 'JAN'">01</xsl:when>
      <xsl:when test="$month = 'FEB'">02</xsl:when>
      <xsl:when test="$month = 'MAR'">03</xsl:when>
      <xsl:when test="$month = 'APR'">04</xsl:when>
      <xsl:when test="$month = 'MAY'">05</xsl:when>
      <xsl:when test="$month = 'JUN'">06</xsl:when>
      <xsl:when test="$month = 'JUL'">07</xsl:when>
      <xsl:when test="$month = 'AUG'">08</xsl:when>
      <xsl:when test="$month = 'SEP'">09</xsl:when>
      <xsl:when test="$month = 'OCT'">10</xsl:when>
      <xsl:when test="$month = 'NOV'">11</xsl:when>
      <xsl:when test="$month = 'DEC'">12</xsl:when>
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>
