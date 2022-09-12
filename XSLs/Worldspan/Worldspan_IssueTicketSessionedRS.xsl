<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
	<!-- 
  ================================================================== 
	Worldspan_IssueTicketSessionedRS.xsl												
	==================================================================
  Date: 08 Jun 2021 - Kobelev - Added Conversation ID Support.	
  Date: 06 Apr 2017 - Kobelev - Added new process for DI display.	
  Date: 06 Apr 2017 - Kobelev - Added Invoice Number field when ticket information returned in the screen.	
  Date: 04 Apr 2017 - Kobelev - Added Invoice Number field.	
  Date: 26 Sep 2016 - Kobelev - Ticketing Fix payment type restrictions.												
	Date: 21 Apr 2014 - Rastko  - New file												
	================================================================== 
  -->
	<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<xsl:apply-templates select="OTA_AirDemandTicketRS"/>
    <xsl:choose>
      <xsl:when test="contains(Screen/Line[position()=2],'DI@')">
        <xsl:apply-templates select="Screen" mode="DI"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:apply-templates select="Screen"/>    
      </xsl:otherwise>
    </xsl:choose>
		
	</xsl:template>
  
	<xsl:template match="OTA_AirDemandTicketRS">
		<TT_IssueTicketRS Version="1.001">
			<xsl:choose>
				<xsl:when test="Warnings">
					<Errors>
						<Error><xsl:value-of select="Warnings/Warning"/></Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<UniqueID>
						<xsl:attribute name="ID">
							<xsl:value-of select="BookingReferenceID/@ID"/>
						</xsl:attribute>
					</UniqueID>
					<TicketingControl>
						<Type>OK</Type>
					</TicketingControl>
					<!--<xsl:for-each select="TicketItemInfo[not(@PaymentType)]">-->
          <xsl:for-each select="TicketItemInfo">
						<Ticket>
							<xsl:attribute name="Type">
								<xsl:choose>
									<xsl:when test="@Type='eTicket'">Electronic</xsl:when>
									<xsl:otherwise>Paper</xsl:otherwise>
								</xsl:choose>
							</xsl:attribute>
							<xsl:attribute name="Number">
								<xsl:value-of select="@TicketNumber"/>
							</xsl:attribute>
              <xsl:if test="@InvoiceNumber">
                <xsl:attribute name="InvoiceNumber">
                  <xsl:value-of select="@InvoiceNumber"/>
                </xsl:attribute>
              </xsl:if>
							<xsl:if test="PassengerName">
								<Name>
									<xsl:value-of select="PassengerName/GivenName"/>
									<xsl:value-of select="concat(' ',PassengerName/Surname)"/>
								</Name>
							</xsl:if>
						</Ticket>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
			 <xsl:choose>
      <xsl:when test="ConversationID">
        <ConversationID>
          <xsl:value-of select="ConversationID"/>
        </ConversationID>
      </xsl:when>
      <xsl:otherwise>
        <ConversationID>NONE</ConversationID>
      </xsl:otherwise>      
    </xsl:choose>
		</TT_IssueTicketRS>
	</xsl:template>
	
	<xsl:template match="Screen">
		<TT_IssueTicketRS Version="1.001">
			<xsl:choose>
				<xsl:when test="count(Line)!= 4">
					<Errors>
						<Error><xsl:value-of select="Line[1]"/></Error>
					</Errors>
				</xsl:when>
				<xsl:otherwise>
					<Success/>
					<UniqueID>
						<xsl:attribute name="ID">
							<xsl:value-of select="substring-before(substring-after(Line[position()=3],'*'),'(')"/>
						</xsl:attribute>
					</UniqueID>
					<TicketingControl>
						<Type>OK</Type>
					</TicketingControl>
					<xsl:for-each select="Line[position()>1 and starts-with(.,'  ')]">
            <!--
            0         1         2         3         4         5         6
            0123456789012345678901234567890123456789012345678901234567890123456
                    715.96A    E2357979075781        0.00 TIPY/AGUL  00635724
            -->
						<Ticket>
							<xsl:attribute name="Type">Electronic</xsl:attribute>
							<xsl:attribute name="Number">
								<xsl:value-of select="substring(.,20,13)"/>
							</xsl:attribute>
              <xsl:attribute name="InvoiceNumber">
                <xsl:value-of select="substring(.,57,8)"/>
              </xsl:attribute>
						</Ticket>
					</xsl:for-each>
				</xsl:otherwise>
			</xsl:choose>
			<ConversationID>None</ConversationID>
		</TT_IssueTicketRS>
	</xsl:template>

<!--
 <Screen>
  <Line>&gt;4-DI@1#N1.1/2.1#K5</Line>
  <Line>          T/06APR0934  1P/0G3/RO*E5557979075804-805 I635734 *I N</Line>
  <Line>          1.1/2.1</Line>
  <Line>&gt;</Line>
</Screen>            
            
            0         1         2         3         4         5         6
            0123456789012345678901234567890123456789012345678901234567890123456
                      T/06APR0934  1P/0G3/RO*E5557979075804-805 I635734 *I N
-->
  <xsl:template match="Screen" mode="DI">
    <TT_IssueTicketRS Version="1.001">
      <xsl:choose>
        <xsl:when test="contains(., 'T/')">
          <Success/>
          <UniqueID>
            <xsl:attribute name="ID">
              <xsl:value-of select="Line[position()=1]"/>
            </xsl:attribute>
          </UniqueID>
          <TicketingControl>
            <Type>OK</Type>
          </TicketingControl>
          <Ticket>
            <xsl:attribute name="Type">Electronic</xsl:attribute>
            <xsl:attribute name="Number">
              <xsl:value-of select="substring-before(substring-after(Line[position()>1 and contains(.,'T/')],'RO*E'), ' I')"/>
            </xsl:attribute>
            <xsl:attribute name="InvoiceNumber">
              <xsl:value-of select="substring-before(substring-after(Line[position()>1 and contains(.,'T/')],' I'), ' *I')"/>
            </xsl:attribute>
            <xsl:choose>
              <xsl:when test="count(Line) = 5">
                <xsl:attribute name="Passengers">
                  <xsl:value-of select="translate(Line[position()=4], ' ','')"/>
                </xsl:attribute>    
              </xsl:when>
              <xsl:when test="count(Line) = 4">
                <xsl:attribute name="Passengers">
                  <xsl:value-of select="translate(translate(substring-after(Line[position()=3], '*I N'), ' ',''), '&#xA;','')"/>
                </xsl:attribute>
              </xsl:when>
              <xsl:otherwise>
              </xsl:otherwise>
            </xsl:choose>
          </Ticket>
        </xsl:when>
        <xsl:otherwise>
          <Errors>
            <Error>
              <xsl:value-of select="Line[1]"/>
            </Error>
          </Errors>
        </xsl:otherwise>
      </xsl:choose>
      <ConversationID>None</ConversationID>
    </TT_IssueTicketRS>
  </xsl:template>
</xsl:stylesheet>
