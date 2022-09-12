<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- 
================================================================== 
Galileo_QueueRS.xsl 															
================================================================== 
Date: 17 Feb 2020 - Kobelev - Added QueueProcessing_16/PNRBFRetrieve/QueueInfo
Date: 11 Mar 2019 - Kobelev			
Date: 02 Dec 2012 - Rastko													
================================================================== 
-->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:template match="/">
    <OTA_QueueRS Version="1.000">
      <xsl:apply-templates select="QueueProcessing_16/QueueCount"/>
      <xsl:apply-templates select="QueueProcessing_16/QueuePNRList/PNRList"/>
      <xsl:apply-templates select="PNRBFManagement_53/EndTransaction/EndTransactResponse[RecLoc !='']"/>
      <xsl:apply-templates select="PNRBFManagement_53[not(PNRBFRetrieve/ErrText)]/EndTransaction/EndTransactMessage[TypeInd ='E']"/>
      <xsl:apply-templates select="PNRBFManagement_53/PNRBFRetrieve/ErrText[Text !='']"/>
      <xsl:apply-templates select="PoweredQueue_CountTotalReply/errorReturn"/>
      <xsl:apply-templates select="PoweredQueue_ListReply/errorReturn"/>
      <xsl:apply-templates select="PoweredQueue_MoveItemReply/goodResponse" mode="move"/>
      <xsl:apply-templates select="PoweredQueue_MoveItemReply/errorReturn"/>
      <xsl:apply-templates select="PoweredQueue_RemoveItemReply/goodResponse" mode="remove"/>
      <xsl:apply-templates select="PoweredQueue_RemoveItemReply/errorReturn"/>
      <xsl:apply-templates select="QueueProcessing_16/QueueErrText"/>
      <xsl:apply-templates select="QueueProcessing_16/PNRBFRetrieve/QueueInfo" mode="remove"/>
      <xsl:apply-templates select="QueueProcessing_16/PNRBFRetrieve" mode="remove"/>
      <AltLangID>Galileo</AltLangID>
    </OTA_QueueRS>
  </xsl:template>

  <!-- Queue Count -->
  <xsl:template match="QueueCount">
    <CountQueue>
      <xsl:attribute name="PseudoCityCode">
        <xsl:value-of select="HeaderCount[1]/PCC"/>
      </xsl:attribute>
      <xsl:attribute name="TotalItems">
        <xsl:value-of select="sum(HeaderCount/TotPNRBFCnt)"/>
      </xsl:attribute>
      <Queues>
        <xsl:for-each select="HeaderCount">
          <Queue>
            <xsl:attribute name="Number">
              <xsl:value-of select="QNum"/>
            </xsl:attribute>
            <xsl:attribute name="Name">
              <xsl:value-of select="QTitle"/>
            </xsl:attribute>
            <xsl:attribute name="TotalQueueItems">
              <xsl:value-of select="TotPNRBFCnt"/>
            </xsl:attribute>
          </Queue>
        </xsl:for-each>
      </Queues>
    </CountQueue>
  </xsl:template>

  <!-- Queue List -->
  <xsl:template match="PNRList">
    <ListQueue>
      <!--xsl:attribute name="PseudoCityCode"><xsl:value-of select="queueView[1]/agent/originatorDetails/inHouseIdentification1"/></xsl:attribute-->
      <QueueCategory>
        <xsl:attribute name="QueueNumber">
          <xsl:value-of select="queueView[1]/queueNumber/queueDetails/number"/>
        </xsl:attribute>
        <!--xsl:attribute name="QueueName"></xsl:attribute-->
        <xsl:attribute name="CategoryNumber">
          <xsl:value-of select="queueView[1]/categoryDetails/subQueueInfoDetails/itemNumber"/>
        </xsl:attribute>
        <xsl:attribute name="TotalItems">
          <xsl:value-of select="count(RecLocAry/RecLoc)"/>
        </xsl:attribute>
        <QueueItems>
          <xsl:apply-templates select="RecLocAry/RecLoc"/>
        </QueueItems>
      </QueueCategory>
    </ListQueue>
  </xsl:template>

  <xsl:template match="PNRBFRetrieve" mode="remove">
    <xsl:if test="AirSeg">
      <ListQueue>
        <QueueCategory>
          <xsl:attribute name="QueueNumber">
            <xsl:value-of select="QueueInfo/QNum"/>
          </xsl:attribute>
          <!--xsl:attribute name="QueueName"></xsl:attribute-->
          <xsl:attribute name="CategoryNumber">
            <xsl:value-of select="QueueInfo/QCat"/>
          </xsl:attribute>
          <xsl:attribute name="TotalItems">
            <xsl:value-of select="count(RecLocAry/RecLoc)"/>
          </xsl:attribute>
          <QueueItems>
            <xsl:apply-templates select="."/>
          </QueueItems>
        </QueueCategory>
      </ListQueue>
    </xsl:if>
  </xsl:template>

  <!-- Queue List Queues -->
  <xsl:template match="RecLoc">
    <QueueItem>
      <xsl:if test="agent/originatorDetails/inHouseIdentification2 != ''">
        <xsl:attribute name="AgentCode">
          <xsl:value-of select="agent/originatorDetails/inHouseIdentification2"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="pnrdates[timeMode='700']/dateTime != ''">
        <xsl:attribute name="DateQueued">
          <xsl:apply-templates select="pnrdates[timeMode='700']/dateTime" mode="date"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="pnrdates[timeMode='701']/dateTime != ''">
        <xsl:attribute name="TicketDate">
          <xsl:apply-templates select="pnrdates[timeMode='701']/dateTime" mode="date"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="pnrdates[timeMode='711']/dateTime != ''">
        <xsl:attribute name="DateTimeCreated">
          <xsl:apply-templates select="pnrdates[timeMode='711']/dateTime" mode="date"/>
        </xsl:attribute>
      </xsl:if>
      <UniqueID Type="16">
        <xsl:attribute name="ID">
          <xsl:value-of select="RLoc"/>
        </xsl:attribute>
      </UniqueID>
      <TravelerName>
        <xsl:value-of select="Surname"/>
      </TravelerName>
      <xsl:if test="TravDt != ''">
        <Flight>
          <xsl:attribute name="DepartureDateTime">
            <xsl:value-of select="substring(TravDt,1,4)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(TravDt,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(TravDt,7,2)"/>
            <xsl:text>T00:00:00</xsl:text>
          </xsl:attribute>
          <!--xsl:attribute name="FlightNumber">
					<xsl:value-of select="segment/flightIdentification/flightNumber"/>
				</xsl:attribute>
				<DepartureAirport>
					<xsl:attribute name="LocationCode"><xsl:value-of select="segment/boardPointDetails/trueLocation"/></xsl:attribute>
				</DepartureAirport>
				<ArrivalAirport>
					<xsl:attribute name="LocationCode"><xsl:value-of select="segment/offpointDetails/trueLocation"/></xsl:attribute>
				</ArrivalAirport>
				<MarketingAirline>
					<xsl:attribute name="Code"><xsl:value-of select="segment/companyDetails/marketingCompany"/></xsl:attribute>
				</MarketingAirline-->
        </Flight>
      </xsl:if>
    </QueueItem>
  </xsl:template>

  <xsl:template match="PNRBFRetrieve">
    <QueueItem>
      <xsl:if test="agent/originatorDetails/inHouseIdentification2 != ''">
        <xsl:attribute name="AgentCode">
          <xsl:value-of select="GenPNRInfo/CreatingAgntDuty"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="DateQueued">
        <xsl:apply-templates select="GenPNRInfo/PNRBFPurgeDt" mode="date"/>
      </xsl:attribute>
      <xsl:if test="GenPNRInfo/TkNumExistInd != 'N'">
        <xsl:attribute name="TicketDate">
          <xsl:apply-templates select="GenPNRInfo/CurDtStamp" mode="date"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:attribute name="DateTimeCreated">
        <xsl:apply-templates select="GenPNRInfo/CreationDt" mode="date"/>
      </xsl:attribute>
      <UniqueID Type="16">
        <xsl:attribute name="ID">
          <xsl:value-of select="GenPNRInfo/RecLoc"/>
        </xsl:attribute>
      </UniqueID>
      <TravelerName>
        <xsl:value-of select="LNameInfo/LName"/>
      </TravelerName>
      <xsl:if test="AirSeg != ''">
        <Flight>
          <xsl:attribute name="DepartureDateTime">
            <xsl:value-of select="substring(AirSeg[1]/Dt,1,4)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(AirSeg[1]/Dt,5,2)"/>
            <xsl:text>-</xsl:text>
            <xsl:value-of select="substring(AirSeg[1]/Dt,7,2)"/>
            <xsl:text>T</xsl:text>
            <xsl:value-of select="substring(AirSeg[1]/StartTm,1,2)"/>
            <xsl:text>:</xsl:text>
            <xsl:value-of select="substring(AirSeg[1]/StartTm,3)"/>
            <xsl:text>:00</xsl:text>
          </xsl:attribute>
          <xsl:attribute name="FlightNumber">
            <xsl:value-of select="AirSeg[1]/FltNum"/>
          </xsl:attribute>
          <DepartureAirport>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="AirSeg[1]/StartAirp"/>
            </xsl:attribute>
          </DepartureAirport>
          <ArrivalAirport>
            <xsl:attribute name="LocationCode">
              <xsl:value-of select="AirSeg[1]/EndAirp"/>
            </xsl:attribute>
          </ArrivalAirport>
          <MarketingAirline>
            <xsl:attribute name="Code">
              <xsl:value-of select="AirSeg[1]/AirV"/>
            </xsl:attribute>
          </MarketingAirline>
        </Flight>
      </xsl:if>
    </QueueItem>
  </xsl:template>


  <xsl:template match="dateTime" mode="date">
    <xsl:choose>
      <xsl:when test="string-length(year) = 4">
        <xsl:value-of select="year"/>
      </xsl:when>
      <xsl:when test="string-length(year) = 2">
        <xsl:text>20</xsl:text>
        <xsl:value-of select="year"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:text>200</xsl:text>
        <xsl:value-of select="year"/>
      </xsl:otherwise>
    </xsl:choose>
    <xsl:text>-</xsl:text>
    <xsl:if test="string-length(month) = 1">
      <xsl:text>0</xsl:text>
    </xsl:if>
    <xsl:value-of select="month"/>
    <xsl:text>-</xsl:text>
    <xsl:if test="string-length(day) = 1">
      <xsl:text>0</xsl:text>
    </xsl:if>
    <xsl:value-of select="day"/>
    <xsl:if test="hour">
      <xsl:text>T</xsl:text>
      <xsl:if test="string-length(hour) = 1">
        <xsl:text>0</xsl:text>
      </xsl:if>
      <xsl:value-of select="hour"/>
      <xsl:text>:</xsl:text>
      <xsl:if test="string-length(minutes) = 1">
        <xsl:text>0</xsl:text>
      </xsl:if>
      <xsl:value-of select="minutes"/>
      <xsl:text>:00</xsl:text>
    </xsl:if>
  </xsl:template>

  <!-- Queue Move -->
  <xsl:template match="goodResponse" mode="move">
    <Move>
      <NumberOfPNRS>
      </NumberOfPNRS>
      <From>
      </From>
      <To>
      </To>
      <Text>OK processed</Text>
    </Move>
  </xsl:template>

  <!-- Queue Remove -->
  <xsl:template match="goodResponse" mode="remove">
    <Remove>
      <NumberOfPNRS>
      </NumberOfPNRS>
      <From>
      </From>
      <To>
      </To>
      <Text>OK processed</Text>
    </Remove>
  </xsl:template>

  <!-- Queue Remove -->
  <xsl:template match="QueueInfo" mode="remove">
    <Remove>
      <Text>OK processed</Text>
    </Remove>
  </xsl:template>

  <!-- errors -->
  <xsl:template match="ErrText | EndTransactMessage">
    <Errors>
      <Error Type="Galileo">
        <xsl:attribute name="Code">
          <xsl:value-of select="../ErrorCode"/>
        </xsl:attribute>
        <xsl:value-of select="Text"/>
      </Error>
    </Errors>
  </xsl:template>

  <!-- Queue Error -->
  <xsl:template match="QueueErrText">
    <Errors>
      <Error Type="Galileo">
        <xsl:value-of select="TextMsg/Txt"/>
      </Error>
    </Errors>
  </xsl:template>

  <!-- Queue Clean -->
  <!--xsl:template match="">
</xsl:template-->

  <!-- Queue Place -->
  <xsl:template match="EndTransactResponse">
    <PlaceQueue>
      <UniqueID Type="16">
        <xsl:attribute name="ID">
          <xsl:value-of select="RecLoc"/>
        </xsl:attribute>
      </UniqueID>
    </PlaceQueue>
  </xsl:template>

  <!-- Queue Exit -->
  <!--xsl:template match="">
</xsl:template-->

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
