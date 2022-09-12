<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <!-- ================================================================== -->
  <!-- AmadeusWS_QueueReadRQ.xsl 													       -->
  <!-- ================================================================== -->
  <!-- Date: 26 Jun 2014  - Rastko - added support for QueueMode_ProcessQueue		       -->
  <!-- Date: 05 Dec 2010  - Rastko - added Action = redisplay						       -->
  <!-- Date: 23 Jun 2009  - Rastko														       -->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes" />
  <xsl:variable name="username" select="OTA_QueueReadRQ/POS/TPA_Extensions/Provider/Userid"/>
  <xsl:variable name="system" select="OTA_QueueReadRQ/POS/TPA_Extensions/Provider/System"/>
  <xsl:template match="/">
    <xsl:apply-templates select="OTA_QueueReadRQ" />
  </xsl:template>

  <xsl:template match="OTA_QueueReadRQ">
    <xsl:apply-templates select="ItemOnQueue[@Action = 'NextRemove' or @Action = 'NextKeep']" mode="next"/>
    <xsl:apply-templates select="ItemOnQueue[@Action = 'Redisplay']" mode="redisplay"/>
    <xsl:apply-templates select="AccessQueue"/>
    <xsl:apply-templates select="ExitQueue"/>
  </xsl:template>

  <xsl:template match="ItemOnQueue" mode="next">
    <xsl:choose>
      <xsl:when test="$username='Downtown' and $system='Test' and $system='Test'">
        <QueueMode_ProcessQueue>
          <messageActionDetails>
            <messageFunctionDetails>
              <messageFunction>
                <xsl:choose>
                  <xsl:when test="@Action = 'NextRemove'">
                    <xsl:text>217</xsl:text>
                  </xsl:when>
                  <xsl:when test="@Action = 'NextKeep'">
                    <xsl:text>218</xsl:text>
                  </xsl:when>
                </xsl:choose>
              </messageFunction>
            </messageFunctionDetails>
          </messageActionDetails>
          <!--recordLocator>
						<reservation>
							<controlNumber>3GV4BG</controlNumber>
						</reservation>
					</recordLocator-->
          <queueInfoDetails>
            <selectionInfoDetails>
              <selectionDetails>
                <option>QP</option>
              </selectionDetails>
            </selectionInfoDetails>
          </queueInfoDetails>
        </QueueMode_ProcessQueue>
      </xsl:when>
      <xsl:otherwise>
        <Command_Cryptic>
          <messageAction>
            <messageFunctionDetails>
              <messageFunction>M</messageFunction>
            </messageFunctionDetails>
          </messageAction>
          <longTextString>
            <textStringDetails>
              <xsl:choose>
                <xsl:when test="@Action = 'NextRemove'">
                  <xsl:text>QN</xsl:text>
                </xsl:when>
                <xsl:when test="@Action = 'NextKeep'">
                  <xsl:text>IG</xsl:text>
                </xsl:when>
              </xsl:choose>
            </textStringDetails>
          </longTextString>
        </Command_Cryptic>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="ItemOnQueue" mode="redisplay">
    <PNR_Retrieve>
      <retrievalFacts>
        <retrieve>
          <type>1</type>
        </retrieve>
      </retrievalFacts>
    </PNR_Retrieve>
  </xsl:template>

  <xsl:template match="ExitQueue">
    <xsl:choose>
      <xsl:when test="$username='Downtown' and $system='Test'">
        <QueueMode_ProcessQueue>
          <messageActionDetails>
            <messageFunctionDetails>
              <messageFunction>216</messageFunction>
            </messageFunctionDetails>
          </messageActionDetails>
          <!--recordLocator>
						<reservation>
							<controlNumber>3GV4BG</controlNumber>
						</reservation>
					</recordLocator-->
          <queueInfoDetails>
            <selectionInfoDetails>
              <selectionDetails>
                <option>QP</option>
              </selectionDetails>
            </selectionInfoDetails>
          </queueInfoDetails>
        </QueueMode_ProcessQueue>
      </xsl:when>
      <xsl:otherwise>
        <Command_Cryptic>
          <messageAction>
            <messageFunctionDetails>
              <messageFunction>M</messageFunction>
            </messageFunctionDetails>
          </messageAction>
          <longTextString>
            <textStringDetails>
              <xsl:text>QI</xsl:text>
            </textStringDetails>
          </longTextString>
        </Command_Cryptic>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="AccessQueue">
    <xsl:choose>
      <xsl:when test="$username='Downtown' and $system='Test'">
        <QueueMode_ProcessQueue>
          <messageActionDetails>
            <messageFunctionDetails>
              <messageFunction>211</messageFunction>
            </messageFunctionDetails>
          </messageActionDetails>
          <queueInfoDetails>
            <selectionInfoDetails>
              <selectionDetails>
                <option>QP</option>
              </selectionDetails>
            </selectionInfoDetails>
            <queueGroup>
              <queueInfo>
                <queueDetails>
                  <number>
                    <xsl:value-of select="@Number"/>
                  </number>
                </queueDetails>
              </queueInfo>
              <xsl:if test="@Category != ''">
                <subQueueInfo>
                  <subQueueInfoDetails>
                    <identificationType>C</identificationType>
                    <itemNumber>
                      <xsl:value-of select="@Category"/>
                    </itemNumber>
                  </subQueueInfoDetails>
                </subQueueInfo>
              </xsl:if>
              <xsl:if test="@PseudoCityCode != ''">
                <targetOffice>
                  <internalIdDetails>
                    <inhouseId>
                      <xsl:value-of select="@PseudoCityCode"/>
                    </inhouseId>
                  </internalIdDetails>
                </targetOffice>
              </xsl:if>
            </queueGroup>
          </queueInfoDetails>
        </QueueMode_ProcessQueue>
      </xsl:when>
      <xsl:otherwise>
        <Command_Cryptic>
          <messageAction>
            <messageFunctionDetails>
              <messageFunction>M</messageFunction>
            </messageFunctionDetails>
          </messageAction>
          <longTextString>
            <textStringDetails>
              <xsl:text>QS</xsl:text>
              <xsl:if test="@PseudoCityCode != ''">
                <xsl:text>/</xsl:text>
                <xsl:value-of select="@PseudoCityCode"/>
              </xsl:if>
              <xsl:text>/</xsl:text>
              <xsl:value-of select="@Number"/>
              <xsl:if test="@Category != ''">
                <xsl:text>C</xsl:text>
                <xsl:value-of select="@Category"/>
              </xsl:if>
            </textStringDetails>
          </longTextString>
        </Command_Cryptic>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

</xsl:stylesheet>
