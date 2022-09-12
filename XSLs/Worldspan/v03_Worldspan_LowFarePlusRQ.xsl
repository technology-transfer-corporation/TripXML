<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<!-- ================================================================== -->
<!-- Worldspan_LowFarePlusRQ.xsl 													        -->
<!-- ================================================================== -->
<!-- Date: 18 Feb 2010 - Rastko					        -->
<!-- ================================================================== -->
<xsl:output method="xml" omit-xml-declaration="yes" />
<xsl:template match="/">
	<PSC5>
		<xsl:apply-templates select="OTA_AirLowFareSearchPlusRQ"/>
	</PSC5>
</xsl:template>

<!-- **************************************************************** -->
<xsl:template match="OTA_AirLowFareSearchPlusRQ">
      <xsl:if test="OriginDestinationInformation[position()=1]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=last()]/DestinationLocation/@LocationCode">
		<xsl:variable name="num"><xsl:value-of select="count(OriginDestinationInformation)"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="$num=2">
				<xsl:if test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
					<OPT>F</OPT>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$num=3">
				<xsl:if test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
					<xsl:if test="OriginDestinationInformation[position()=3]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=2]/DestinationLocation/@LocationCode">
						<OPT>F</OPT>
					</xsl:if>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
					<xsl:if test="OriginDestinationInformation[position()=3]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=2]/DestinationLocation/@LocationCode">
						<xsl:if test="OriginDestinationInformation[position()=4]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=3]/DestinationLocation/@LocationCode">
							<OPT>F</OPT>
						</xsl:if>
					</xsl:if>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:if>
	<xsl:apply-templates select="TravelerInfoSummary/PriceRequestInformation"/>
	<OPT>O</OPT>
	<OPT>H</OPT>
	<POI_ORI>
		<CIT><xsl:value-of select="OriginDestinationInformation[position()=1]/OriginLocation/@LocationCode" /></CIT>
		<CIT_IND>Y</CIT_IND>
	</POI_ORI>
	<xsl:apply-templates select="POS/Source"/>  
	<xsl:if test="OriginDestinationInformation[position()=1]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=last()]/DestinationLocation/@LocationCode">    
		<xsl:variable name="num"><xsl:value-of select="count(OriginDestinationInformation)"/></xsl:variable>
		<xsl:choose>
			<xsl:when test="$num=2">
				<xsl:if test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
					<REG_RET_DAT>
						<xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,9,2)"/>
						<xsl:call-template name="month">
							<xsl:with-param name="month">
								<xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,6,2)" />
							</xsl:with-param>
						</xsl:call-template>
					</REG_RET_DAT>
				</xsl:if>
			</xsl:when>
			<xsl:when test="$num=3">
				<xsl:if test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
					<xsl:if test="OriginDestinationInformation[position()=3]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=2]/DestinationLocation/@LocationCode">
						<REG_RET_DAT>
							<xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,9,2)"/>
							<xsl:call-template name="month">
								<xsl:with-param name="month">
									<xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,6,2)" />
								</xsl:with-param>
							</xsl:call-template>
						</REG_RET_DAT>
					</xsl:if>
				</xsl:if>
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
					<xsl:if test="OriginDestinationInformation[position()=3]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=2]/DestinationLocation/@LocationCode">
						<xsl:if test="OriginDestinationInformation[position()=4]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=3]/DestinationLocation/@LocationCode">
							<REG_RET_DAT>
								<xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,9,2)"/>
								<xsl:call-template name="month">
									<xsl:with-param name="month">
										<xsl:value-of select="substring(OriginDestinationInformation[position()=last()]/DepartureDateTime,6,2)" />
									</xsl:with-param>
								</xsl:call-template>
							</REG_RET_DAT>
						</xsl:if>
					</xsl:if>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:if>
	<xsl:choose>
		<xsl:when test="OriginDestinationInformation[1]/DepartureDateTime/@WindowBefore != ''">
			<WIN><xsl:value-of select="OriginDestinationInformation[1]/DepartureDateTime/@WindowBefore"/></WIN>
		</xsl:when>
		<xsl:when test="OriginDestinationInformation[1]/DepartureDateTime/@WindowAfter != ''">
			<WIN><xsl:value-of select="OriginDestinationInformation[1]/DepartureDateTime/@WindowAfter"/></WIN>
		</xsl:when>
	</xsl:choose>
	<NUM_ALT>100</NUM_ALT>
	<xsl:if test="TravelPreferences/VendorPref/@Code !='' ">	
		<xsl:apply-templates select="TravelPreferences"/> 
	</xsl:if>
	<xsl:if test="OriginDestinationInformation[position()=1]/ConnectionLocations/ConnectionLocation/@PreferLevel = 'Unacceptable'">
		<EXC_CIT>
			<xsl:apply-templates select="OriginDestinationInformation[position()=1]/ConnectionLocations/ConnectionLocation[@PreferLevel = 'Unacceptable']"/>
		</EXC_CIT>
	</xsl:if>
	<xsl:apply-templates select="TravelerInfoSummary/AirTravelerAvail/PassengerTypeQuantity" />
	<xsl:choose>
		<xsl:when test="OriginDestinationInformation[position()=1]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=last()]/DestinationLocation/@LocationCode"> 
			<xsl:variable name="num"><xsl:value-of select="count(OriginDestinationInformation)"/></xsl:variable>
			<xsl:choose>
				<xsl:when test="$num=2">
					<xsl:choose>
						<xsl:when test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
							<xsl:apply-templates select="OriginDestinationInformation[position()=1]" mode="Data"/>	
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>	
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$num=3">
					<xsl:choose>
						<xsl:when test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
							<xsl:choose>
								<xsl:when test="OriginDestinationInformation[position()=3]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=2]/DestinationLocation/@LocationCode">
									<xsl:apply-templates select="OriginDestinationInformation[position() &lt; last()]" mode="Data"/>	
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>	
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>	
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
				<xsl:when test="$num=4">
					<xsl:choose>
						<xsl:when test="OriginDestinationInformation[position()=2]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=1]/DestinationLocation/@LocationCode">
							<xsl:choose>
								<xsl:when test="OriginDestinationInformation[position()=3]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=2]/DestinationLocation/@LocationCode">
									<xsl:choose>
										<xsl:when test="OriginDestinationInformation[position()=4]/OriginLocation/@LocationCode = OriginDestinationInformation[position()=3]/DestinationLocation/@LocationCode">
											<xsl:apply-templates select="OriginDestinationInformation[position() &lt; last()]" mode="Data"/>	
										</xsl:when>
										<xsl:otherwise>
											<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>	
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>	
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>	
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>
			</xsl:choose>
	 	</xsl:when>
		<xsl:otherwise>  
			<xsl:apply-templates select="OriginDestinationInformation" mode="Oneway"/>
		</xsl:otherwise>
	</xsl:choose>
	<xsl:apply-templates select="TravelerInfoSummary/PriceRequestInformation/NegotiatedFareCode"/>  
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="PriceRequestInformation">
	<xsl:if test="@PricingSource = 'Published'">
		<OPT>C</OPT>
		<xsl:if test="../../TravelPreferences and not(../../TravelPreferences/FareRestrictPref/VoluntaryChanges/Penalty)">          
			<OPT>B</OPT> 
		</xsl:if>
		<xsl:if test="../../TravelPreferences and not(../../TravelPreferences/FareRestrictPref/AdvResTicketing/AdvReservation)">
			<OPT>A</OPT> 
		</xsl:if>		
	</xsl:if>
	<xsl:if test="@PricingSource = 'Private'"> 
		<OPT>V</OPT>
	</xsl:if>
	<xsl:if test="@PricingSource = 'Both'"> 
		<OPT>U</OPT>
		<xsl:if test="../../TravelPreferences and not(../../TravelPreferences/FareRestrictPref/VoluntaryChanges/Penalty)">          
			<OPT>B</OPT> 
		</xsl:if>
		<xsl:if test="../../TravelPreferences and not(../../TravelPreferences/FareRestrictPref/AdvResTicketing/AdvReservation)">
			<OPT>A</OPT> 
		</xsl:if>
	</xsl:if>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="Source">
	<xsl:if test="@ISOCurrency != ''">
		<ISO_CUR_COD><xsl:value-of select="@ISOCurrency" /></ISO_CUR_COD>
	</xsl:if>
	<xsl:if test="@ISOCountry != ''">
		<ISO_CTY_SAL><xsl:value-of select="@ISOCountry" /></ISO_CTY_SAL>
	</xsl:if>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="TravelPreferences">
	<xsl:if test="VendorPref/@Code">
		<ARL_INF>
			<xsl:choose>
				<xsl:when test="VendorPref/@PreferLevel = 'Preferred'"><ARL_OPT>A</ARL_OPT></xsl:when>
				<xsl:otherwise><ARL_OPT>B</ARL_OPT></xsl:otherwise>
			</xsl:choose>
			<xsl:choose>
				<xsl:when test="VendorPref/@PreferLevel != 'Unacceptable'">
					<xsl:apply-templates select="VendorPref[@PreferLevel != 'Unacceptable']"/>
				</xsl:when>
				<xsl:when test="VendorPref/@PreferLevel = 'Unacceptable'">
					<EXC_ARL>
						<xsl:apply-templates select="VendorPref[@PreferLevel = 'Unacceptable']"/>
					</EXC_ARL>
				</xsl:when>
				<xsl:otherwise>
					<xsl:apply-templates select="VendorPref"/>
				</xsl:otherwise>
			</xsl:choose>
		</ARL_INF>
	</xsl:if>
</xsl:template>

<xsl:template match="VendorPref">
	<ARL_COD><xsl:value-of select="@Code" /></ARL_COD>
</xsl:template>

<xsl:template match="PassengerTypeQuantity">
	<PTC_INF>
		<NUM_PAX><xsl:value-of select="@Quantity"/></NUM_PAX>
		<PTC>
			<xsl:choose>
				<xsl:when test="@Code = 'CHD'">CNN</xsl:when>
				<xsl:otherwise><xsl:value-of select="@Code"/></xsl:otherwise>
			</xsl:choose>
		</PTC>	
	</PTC_INF>
</xsl:template>

<xsl:template match="OriginDestinationInformation" mode="Data">
	<xsl:variable name="posnext"><xsl:value-of select="position()+1"/></xsl:variable>
	<DES_INF>
		<xsl:if test="ConnectionLocations/ConnectionLocation">
			<CON_CIT_COD>
				<xsl:apply-templates select="ConnectionLocations/ConnectionLocation"/>
			</CON_CIT_COD>
		</xsl:if>
		<DEP_DAT>
			<xsl:value-of select="substring(DepartureDateTime,9,2)"/>
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(DepartureDateTime,6,2)" />
				</xsl:with-param>
			</xsl:call-template>
		</DEP_DAT>
		<xsl:choose>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='First'">
				<CAB_CLA>F</CAB_CLA> 
			</xsl:when>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='Business'">
				<CAB_CLA>C</CAB_CLA> 
			</xsl:when>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='Economy'">
				<CAB_CLA>Y</CAB_CLA> 
			</xsl:when>
		</xsl:choose>
		<xsl:if test="DepartureDateTime != '' and substring(DepartureDateTime,12,5) != '00:00'">
			<DEP_TIM><xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" /></DEP_TIM>  
		</xsl:if>
		<xsl:if test="ArrivalDateTime != '' and substring(ArrivalDateTime,12,5) != '00:00'">
			<ARR_TIM><xsl:value-of select="translate(substring(ArrivalDateTime,12,5),':','')" /></ARR_TIM> 
		</xsl:if>
		<POI_DES>
			<CIT><xsl:value-of select="DestinationLocation/@LocationCode" /></CIT>
			<CIT_IND>Y</CIT_IND>
		</POI_DES>
		<xsl:variable name="pos" select="position()"/>
		<xsl:if test="$pos &lt; count(../OriginDestinationInformation)">
			<STO_TYP>1</STO_TYP> 	
		</xsl:if>	
	</DES_INF>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="OriginDestinationInformation" mode="Oneway">
	<xsl:variable name="posnext"><xsl:value-of select="position()+1"/></xsl:variable>
	<DES_INF>
		<xsl:apply-templates select="ConnectionLocations/ConnectionLocation"/>
		<DEP_DAT>
			<xsl:value-of select="substring(DepartureDateTime,9,2)"/>
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(DepartureDateTime,6,2)" />
				</xsl:with-param>
			</xsl:call-template>
		</DEP_DAT>
		<xsl:choose>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='First'">
				<CAB_CLA>F</CAB_CLA> 
			</xsl:when>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='Business'">
				<CAB_CLA>C</CAB_CLA> 
			</xsl:when>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='Economy'">
				<CAB_CLA>Y</CAB_CLA> 
			</xsl:when>
		</xsl:choose>
		<xsl:if test="DepartureDateTime != '' and substring(DepartureDateTime,12,5) != '00:00'">
			<DEP_TIM><xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" /></DEP_TIM>  
		</xsl:if>
		<xsl:if test="ArrivalDateTime != '' and substring(ArrivalDateTime,12,5) != '00:00'">
			<ARR_TIM><xsl:value-of select="translate(substring(ArrivalDateTime,12,5),':','')" /></ARR_TIM> 
		</xsl:if>
		<POI_DES>
			<CIT><xsl:value-of select="DestinationLocation/@LocationCode" /></CIT>
			<CIT_IND>Y</CIT_IND>
		</POI_DES>
		<xsl:variable name="pos" select="position()"/>
		<xsl:if test="$pos &lt; count(../OriginDestinationInformation)">
			<STO_TYP>1</STO_TYP> 	
		</xsl:if>	
	</DES_INF>
	<xsl:if test="../OriginDestinationInformation[position()=$posnext]/OriginLocation/@LocationCode != DestinationLocation/@LocationCode">   
		<DES_INF>
			<DEP_DAT>
				<xsl:value-of select="substring(../OriginDestinationInformation[position()=$posnext]/DepartureDateTime,9,2)"/>
				<xsl:call-template name="month">
					<xsl:with-param name="month">
						<xsl:value-of select="substring(../OriginDestinationInformation[position()=$posnext]/DepartureDateTime,6,2)" />
					</xsl:with-param>
				</xsl:call-template>
			</DEP_DAT>	
			<POI_DES>
				<CIT><xsl:value-of select="../OriginDestinationInformation[position()=$posnext]/OriginLocation/@LocationCode" /></CIT>
				<CIT_IND>Y</CIT_IND>
			</POI_DES>
			<STO_TYP>2</STO_TYP> 
 		</DES_INF>
	</xsl:if>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="OriginDestinationInformation" mode="Last">
	<DES_INF>
		<xsl:apply-templates select="ConnectionLocations/ConnectionLocation"/>
		<DEP_DAT>
			<xsl:value-of select="substring(DepartureDateTime,9,2)"/>
			<xsl:call-template name="month">
				<xsl:with-param name="month">
					<xsl:value-of select="substring(DepartureDateTime,6,2)" />
				</xsl:with-param>
			</xsl:call-template>
		</DEP_DAT>
		<xsl:choose>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='First'">
				<CAB_CLA>F</CAB_CLA> 
			</xsl:when>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='Business'">
				<CAB_CLA>C</CAB_CLA> 
			</xsl:when>
			<xsl:when test="../TravelPreferences/CabinPref/@Cabin ='Economy'">
				<CAB_CLA>Y</CAB_CLA> 
			</xsl:when>
		</xsl:choose>
		<xsl:if test="DepartureDateTime != '' and substring(DepartureDateTime,12,5) != '00:00'">
			<DEP_TIM><xsl:value-of select="translate(substring(DepartureDateTime,12,5),':','')" /></DEP_TIM>  
		</xsl:if>
		<xsl:if test="ArrivalDateTime != '' and substring(ArrivalDateTime,12,5) != '00:00'">
			<ARR_TIM><xsl:value-of select="translate(substring(ArrivalDateTime,12,5),':','')" /></ARR_TIM> 
		</xsl:if>
		<POI_DES>
			<CIT><xsl:value-of select="DestinationLocation/@LocationCode" /></CIT>
			<CIT_IND>Y</CIT_IND>
		</POI_DES>
		<xsl:variable name="pos" select="position()"/>
		<xsl:if test="$pos &lt; count(../OriginDestinationInformation)">
			<STO_TYP>1</STO_TYP> 	
		</xsl:if>	
	</DES_INF>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="ConnectionLocation">
	<xsl:if test="@PreferLevel != 'Unacceptable' or not (@PreferLevel)">
		<CON_CIT_COD><CIT><xsl:value-of select="@LocationCode" /></CIT></CON_CIT_COD>
	</xsl:if>
</xsl:template>
<!-- **************************************************************** -->
<xsl:template match="NegotiatedFareCode">  
	<SRC_INF>
        	<SRC_NUM><xsl:value-of select="@SecondaryCode"/></SRC_NUM>  
	</SRC_INF>
</xsl:template>

<xsl:template name="month">
		<xsl:param name="month" />
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