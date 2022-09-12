<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- 
================================================================== 
Worldspan_QueueRQ.xsl 																							
================================================================== 
Date: 12 May 2016 - Kobelev - Queue Placment	with optional PCC
Date: 12 Apr 2016 - Kobelev - Queue Read	with optional PCC   			
Date: 26 Jan 2015 - Rastko - new file															
================================================================== 
-->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_QueueRQ" />
  </xsl:template>

  <xsl:template match="OTA_QueueRQ">
    <xsl:apply-templates select="ListQueue"/>
    <xsl:apply-templates select="CountQueue"/>
    <xsl:apply-templates select="RemoveQueue"/>
    <xsl:apply-templates select="BounceQueue"/>
    <xsl:apply-templates select="Move"/>
    <xsl:apply-templates select="PlaceQueue"/>
    <!--xsl:apply-templates select="Clean"/-->
  </xsl:template>

  <xsl:template match="ListQueue">
    <xsl:choose>
      <xsl:when test="@PseudoCityCode!=''">
        <xsl:value-of select="concat('QLD/',@PseudoCityCode,'/',@Number)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('QLD/',@Number)"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:if test="@Category!=''">
      <xsl:value-of select="concat('*C',@Category)"/>
    </xsl:if>
  </xsl:template>

  <xsl:template match="PlaceQueue">
    <xsl:value-of select="concat('*',UniqueID/@ID)"/>
    <xsl:value-of select="concat('QEP/', @PseudoCityCode, '/', @Number)"/>
    <xsl:if test="@Category!=''">
      <xsl:value-of select="concat('*C',@Category)"/>
    </xsl:if>
  </xsl:template>

  <!--
  <xsl:template match="Clean"></xsl:template>
  -->

  <xsl:template match="BounceQueue">
    <xsl:choose>
      <xsl:when test="@Action='R'">
        <Cryptic_GetScreen_Query>
          <Command>QN</Command>
        </Cryptic_GetScreen_Query>
      </xsl:when>
      <xsl:when test="@Action='I'">
        <Cryptic_GetScreen_Query>
          <Command>IG</Command>
        </Cryptic_GetScreen_Query>
      </xsl:when>
      <xsl:when test="@Action='E'">
        <Cryptic_GetScreen_Query>
          <Command>ER</Command>
        </Cryptic_GetScreen_Query>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="RemoveQueue">
    <xsl:value-of select="concat('*',UniqueID/@ID)"/>
    <xsl:choose>
      <xsl:when test="@PseudoCityCode = ''">
        <xsl:value-of select="concat('QRQ/',../POS/Source/@PseudoCityCode,'/',@Number,'*C',@Category)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="concat('QRQ/', @PseudoCityCode,'/',@Number,'*C',@Category)"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="Move">
    <Queue_MoveItem>
      <placementOption>
        <selectionDetails>
          <option>QBD</option>
        </selectionDetails>
      </placementOption>
      <targetDetails>
        <targetOffice>
          <sourceType>
            <sourceQualifier1>3</sourceQualifier1>
          </sourceType>
        </targetOffice>
        <queueNumber>
          <queueDetails>
            <number>
              <xsl:value-of select="FromQueue/@Number"/>
            </number>
          </queueDetails>
        </queueNumber>
        <categoryDetails>
          <subQueueInfoDetails>
            <identificationType>C</identificationType>
            <itemNumber>0</itemNumber>
          </subQueueInfoDetails>
        </categoryDetails>
        <placementDate>
          <timeMode>1</timeMode>
        </placementDate>
      </targetDetails>
      <targetDetails>
        <targetOffice>
          <sourceType>
            <sourceQualifier1>3</sourceQualifier1>
          </sourceType>
        </targetOffice>
        <queueNumber>
          <queueDetails>
            <number>
              <xsl:value-of select="ToQueue/@Number"/>
            </number>
          </queueDetails>
        </queueNumber>
        <categoryDetails>
          <subQueueInfoDetails>
            <identificationType>C</identificationType>
            <itemNumber>0</itemNumber>
          </subQueueInfoDetails>
        </categoryDetails>
        <placementDate>
          <timeMode>1</timeMode>
        </placementDate>
      </targetDetails>
      <numberOfPNRs>
        <quantityDetails>
          <numberOfUnit>
            <xsl:value-of select="@ItemsQuantity"/>
          </numberOfUnit>
        </quantityDetails>
      </numberOfPNRs>
    </Queue_MoveItem>
  </xsl:template>

  <xsl:template match="CountQueue">
    <Queue_CountTotal>
      <queueingOptions>
        <selectionDetails>
          <option>
            <xsl:choose>
              <xsl:when test="@Summary = 'true'">QTQ</xsl:when>
              <xsl:otherwise>QT</xsl:otherwise>
            </xsl:choose>
          </option>
        </selectionDetails>
      </queueingOptions>
      <xsl:if test="@PseudoCityCode != ''">
        <targetOffice>
          <sourceType>
            <sourceQualifier1>4</sourceQualifier1>
          </sourceType>
          <originatorDetails>
            <inHouseIdentification1>
              <xsl:value-of select="@PseudoCityCode"/>
            </inHouseIdentification1>
          </originatorDetails>
        </targetOffice>
      </xsl:if>
      <xsl:choose>
        <xsl:when test="@Number != ''">
          <QueueNumber>
            <queueDetails>
              <number>
                <xsl:value-of select="@Number"/>
              </number>
            </queueDetails>
          </QueueNumber>
        </xsl:when>
        <xsl:when test="@Name != ''">
          <categorySelection>
            <subQueueInfoDetails>
              <identificationType>N</identificationType>
              <itemDescription>
                <xsl:value-of select="@Name"/>
              </itemDescription>
            </subQueueInfoDetails>
          </categorySelection>
        </xsl:when>
      </xsl:choose>
    </Queue_CountTotal>
  </xsl:template>

</xsl:stylesheet>
