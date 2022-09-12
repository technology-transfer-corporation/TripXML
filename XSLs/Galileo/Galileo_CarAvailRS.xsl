<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <!-- ================================================================== -->
  <!-- Galileo_CarAvailRS.xsl 															-->
  <!-- ================================================================== -->
  <!-- Date: 21 Aug 2010 - Rastko - corrected per car type mapping					-->
  <!-- Date: 09  Apr 2013 - Suraj - Changed to multiple vendors issue to change to response as per OTA	-->
  <!-- ================================================================== -->
  <xsl:output method="xml" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <OTA_VehAvailRateRS Version="1.001">
      <xsl:choose>
        <xsl:when test="CarStandardAvail_6_0/CarAvailDetail and not(CarStandardAvail_6_0/CarVErrors)">
          <xsl:choose>
            <xsl:when test="CarStandardAvail_6_0/CarAvailDetail/Signature='E'">
              <Errors>
                <xsl:apply-templates select="CarStandardAvail_6_0/CarAvailDetail/ErrQual"/>
              </Errors>
            </xsl:when>
            <xsl:when test="CarStandardAvail_6_0/CarAvailDetail/Signature='W'">
              <Success/>
              <Warnings Type="Galileo">
                <Warning>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="CarStandardAvail_6_0/CarAvailDetail/ErrQual/Err"/>
                  </xsl:attribute>
                  <xsl:value-of select="CarStandardAvail_6_0/CarAvailDetail/ErrorQual/Txt"/>
                </Warning>
              </Warnings>
              <xsl:apply-templates select="CarStandardAvail_6_0"/>
            </xsl:when>
            <xsl:otherwise>
              <Success/>
              <xsl:apply-templates select="CarStandardAvail_6_0"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <xsl:when test="CarStandardAvail_6_0/CarAvailDetail">
          <xsl:choose>
            <xsl:when test="CarStandardAvail_6_0/CarAvailDetail/Signature='W'">
              <Success/>
              <Warnings Type="Galileo">
                <Warning>
                  <xsl:attribute name="Code">
                    <xsl:value-of select="CarStandardAvail_6_0/CarAvailDetail/ErrQual/Err"/>
                  </xsl:attribute>
                  <xsl:value-of select="CarStandardAvail_6_0/CarAvailDetail/ErrorQual/Txt"/>
                </Warning>
              </Warnings>
            </xsl:when>
            <xsl:when test="CarStandardAvail_6_0/CarVErrors">
              <Success/>
              <Warnings>
                <xsl:apply-templates select="CarStandardAvail_6_0/CarVErrors/VErrAry/VErrInfo" mode="Warning"/>
              </Warnings>
            </xsl:when>
            <xsl:otherwise>
              <Success/>
            </xsl:otherwise>
          </xsl:choose>
          <xsl:apply-templates select="CarStandardAvail_6_0"/>
        </xsl:when>
        <xsl:otherwise>
          <Errors>
            <xsl:apply-templates select="CarStandardAvail_6_0/CarVErrors/VErrAry/VErrInfo" mode="error"/>
          </Errors>
        </xsl:otherwise>
      </xsl:choose>
    </OTA_VehAvailRateRS>
  </xsl:template>
  <xsl:template match="CarStandardAvail_6_0">
    <xsl:apply-templates select="CarAvailDetail"/>
  </xsl:template>
  <xsl:template match="CarAvailDetail">
    <VehAvailRSCore>
      <VehRentalCore>
        <xsl:attribute name="PickUpDateTime"/>
        <xsl:attribute name="ReturnDateTime"/>
        <PickUpLocation>
          <xsl:attribute name="LocationCode"/>
        </PickUpLocation>
        <ReturnLocation>
          <xsl:attribute name="LocationCode"/>
        </ReturnLocation>
      </VehRentalCore>
      <xsl:apply-templates select="DataQual"/>
      <!--<VehVendorAvails>
				<xsl:apply-templates select="DataQual"/>
			</VehVendorAvails>-->
    </VehAvailRSCore>
    <xsl:if test="DataQual/MoreCarsInd='Y'">
      <TPA_Extensions>
        <AddlCarAvail>
          <xsl:attribute name="AddAvailKey">
            <xsl:value-of select="DataQual/DBKey"/>
          </xsl:attribute>
        </AddlCarAvail>
      </TPA_Extensions>
    </xsl:if>
  </xsl:template>
  <xsl:template match="DataQual">
    <VehVendorAvails>
      <xsl:apply-templates select="CarDetailAry/CarDetail"/>
      <!--<VehVendorAvail>
				<Vendor>
					<xsl:attribute name="Code"><xsl:value-of select="CarDetailAry/CarDetail/Vnd"/></xsl:attribute>
					<xsl:attribute name="CodeContext"><xsl:value-of select="CarDetailAry/CarDetail/LinkInd"/></xsl:attribute>
				</Vendor>
				<VehAvails>
					<xsl:apply-templates select="CarDetailAry/CarDetail"/>
				</VehAvails>
			</VehVendorAvail>-->
    </VehVendorAvails>
  </xsl:template>
  <xsl:template match="CarDetail">
    <VehVendorAvail>
      <Vendor>
        <xsl:attribute name="Code">
          <xsl:value-of select="Vnd"/>
        </xsl:attribute>
        <xsl:attribute name="CodeContext">
          <xsl:value-of select="LinkInd"/>
        </xsl:attribute>
      </Vendor>
      <VehAvails>
        <VehAvail>

          <VehAvailCore>
            <xsl:attribute name="Status">
              <xsl:choose>
                <xsl:when test="AvailStatus = 'S'">Available</xsl:when>
                <xsl:when test="AvailStatus = 'N'">OnRequest</xsl:when>
                <xsl:otherwise>Unavailable</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <Vehicle>
              <xsl:attribute name="AirConditionInd">
                <xsl:choose>
                  <xsl:when test="substring(CarType,4,1) = 'R'">true</xsl:when>
                  <xsl:otherwise>false</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="TransmissionType">
                <xsl:choose>
                  <xsl:when test="substring(CarType,3,1) = 'A'">Automatic</xsl:when>
                  <xsl:otherwise>Manual</xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:attribute name="VendorCarType">
                <xsl:value-of select="CarType"/>
              </xsl:attribute>
              <VehType>
                <xsl:attribute name="VehicleCategory">
                  <xsl:choose>
                    <xsl:when test="substring(CarType,2,1) = 'C'">2/4 Door Car</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'B'">2-Door Car</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'D'">4-Door Car</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'W'">Wagon</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'V'">Van</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'L'">Limousine</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'S'">Sport</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'T'">Convertible</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'F'">4-Wheel Drive</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'P'">Pickup</xsl:when>
                    <xsl:when test="substring(CarType,2,1) = 'J'">All-Terrain</xsl:when>
                    <xsl:otherwise>Unavailable</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </VehType>
              <VehClass>
                <xsl:attribute name="Size">
                  <xsl:choose>
                    <xsl:when test="substring(CarType,1,1) = 'M'">Mini</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'E'">Economy</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'C'">Compact</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'I'">Intermediate</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'S'">Standard</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'F'">Full-Size</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'P'">Premium</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'L'">Luxury</xsl:when>
                    <xsl:when test="substring(CarType,1,1) = 'X'">Special</xsl:when>
                    <xsl:otherwise>Unavailable</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </VehClass>
              <xsl:if test="InclRateQual/CarTypeDesc != ''">
                <VehMakeModel>
                  <xsl:attribute name="Name">
                    <xsl:value-of select="InclRateQual/CarTypeDesc"/>
                  </xsl:attribute>
                </VehMakeModel>
              </xsl:if>
            </Vehicle>
            <RentalRate>
              <RateDistance>
                <xsl:choose>
                  <xsl:when test="Mile='UNL'">
                    <xsl:attribute name="Unlimited">true</xsl:attribute>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:attribute name="Unlimited">false</xsl:attribute>
                    <xsl:attribute name="Quantity">
                      <xsl:value-of select="Mile"/>
                    </xsl:attribute>
                  </xsl:otherwise>
                </xsl:choose>
                <xsl:attribute name="DistUnitName">
                  <xsl:choose>
                    <xsl:when test="../../../DataQual/DistUnit='M'">Mile</xsl:when>
                    <xsl:otherwise>Km</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="VehiclePeriodUnitName">
                  <xsl:choose>
                    <xsl:when test="RateType='H'">Hour</xsl:when>
                    <xsl:when test="RateType='D'">Day</xsl:when>
                    <xsl:when test="RateType='E'">Weekend</xsl:when>
                    <xsl:when test="RateType='W'">Week</xsl:when>
                    <xsl:when test="RateType='M'">Month</xsl:when>
                    <xsl:otherwise>RentalPeriod</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
              </RateDistance>
              <VehicleCharges>
                <VehicleCharge>
                  <xsl:attribute name="Amount">
                    <xsl:value-of select="Amt"/>
                  </xsl:attribute>
                  <xsl:attribute name="CurrencyCode">
                    <xsl:value-of select="../../../DataQual/Currency"/>
                  </xsl:attribute>
                  <xsl:attribute name="DecimalPlaces">
                    <xsl:value-of select="../../../DataQual/DecPos"/>
                  </xsl:attribute>
                  <xsl:attribute name="TaxInclusive">false</xsl:attribute>
                  <xsl:attribute name="GuaranteedInd">
                    <xsl:choose>
                      <xsl:when test="RateGuar='G'">true</xsl:when>
                      <xsl:otherwise>false</xsl:otherwise>
                    </xsl:choose>
                  </xsl:attribute>
                  <xsl:attribute name="Purpose">
                    <xsl:text>1</xsl:text>
                  </xsl:attribute>
                </VehicleCharge>
              </VehicleCharges>
              <RateQualifier>
                <xsl:attribute name="TravelPurpose">
                  <xsl:value-of select="YieldMgmt"/>
                </xsl:attribute>
                <xsl:attribute name="RateCategory">
                  <xsl:value-of select="RateCat"/>
                </xsl:attribute>
                <xsl:attribute name="RateQualifier">
                  <xsl:value-of select="Rate"/>
                </xsl:attribute>
                <xsl:attribute name="RatePeriod">
                  <xsl:choose>
                    <xsl:when test="RateType='H'">Hourly</xsl:when>
                    <xsl:when test="RateType='D'">Daily</xsl:when>
                    <xsl:when test="RateType='E'">WeekendDay</xsl:when>
                    <xsl:when test="RateType='W'">Weekly</xsl:when>
                    <xsl:when test="RateType='M'">Monthly</xsl:when>
                    <xsl:otherwise>Other</xsl:otherwise>
                  </xsl:choose>
                </xsl:attribute>
                <xsl:attribute name="VendorRateID">
                  <xsl:value-of select="RateDBKey"/>
                </xsl:attribute>
              </RateQualifier>
            </RentalRate>
            <TotalCharge>
              <xsl:attribute name="RateTotalAmount">
                <xsl:value-of select="BaseRateAmt"/>
              </xsl:attribute>
              <xsl:attribute name="CurrencyCode">
                <xsl:value-of select="../../../DataQual/Currency"/>
              </xsl:attribute>
            </TotalCharge>
            <!--	<xsl:if test="LocAffiliate != ''">
						<AlternateCarVendorCode><xsl:value-of select="LocAffiliate" /></AlternateCarVendorCode>
					</xsl:if>
					<Location>
						<CityCode><xsl:value-of select="City" /></CityCode>
						<Category><xsl:value-of select="LocnCat" /></Category>
						<Number><xsl:value-of select="LocnNum" /></Number>
					</Location>
					<ReferencePoint><xsl:value-of select="../../RefPt" /></ReferencePoint>
					<xsl:if test="Dist != ''">
						<Distance><xsl:value-of select="Dist" /></Distance>
					</xsl:if>
					<xsl:if test="contains('NESW',Dir)">
						<Direction>
							<xsl:value-of select="Dir" />
						</Direction>
					</xsl:if>
					<CarType><xsl:value-of select="CarType" /></CarType>
					<CarTypeDescription>!func:Decode(CarTypes,<xsl:value-of select="CarType"/>)</CarTypeDescription>
					<Rate>
						<xsl:attribute name="Type">
							<xsl:value-of select="RateType" />
						</xsl:attribute>
						<xsl:attribute name="Category">
							<xsl:value-of select="RateCat" />
						</xsl:attribute>
						<xsl:attribute name="Guarantee">
							<xsl:choose>
								<xsl:when test="RateGuar = 'G'">Y</xsl:when>
								<xsl:otherwise>N</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>
						<RateCode><xsl:value-of select="Rate" /></RateCode>
						<RateID><xsl:value-of select="RateDBKey" /></RateID>
						<Amount><xsl:value-of select="Amt" /></Amount>
						<MileKmRate><xsl:value-of select="MileRate" /></MileKmRate>
						<MileKmLimit><xsl:value-of select="Mile" /></MileKmLimit>
					</Rate>
					<xsl:if test="DropOffRestrictions = 'Y'">
						<ExtraCharges>
							<xsl:attribute name="Type">F</xsl:attribute>
						</ExtraCharges>
					</xsl:if>
					<xsl:if test="AdditionalCharges = 'Y'">
						<ExtraCharges>
							<xsl:attribute name="Type">U</xsl:attribute>
						</ExtraCharges>
					</xsl:if>
					<xsl:if test="AdvBookUnit != ''">
						<AdvanceBooking>
							<xsl:attribute name="Unit">
								<xsl:value-of select="AdvBookUnit" />
							</xsl:attribute>
							<xsl:value-of select="Tm" />
						</AdvanceBooking>
					</xsl:if>
					<xsl:if test="MinMaxUnit != ''">
						<MinimumBooking>
							<xsl:attribute name="Unit">
								<xsl:value-of select="MinMaxUnit" />
							</xsl:attribute>
							<xsl:value-of select="Min" />
						</MinimumBooking>
						<MaximumBooking>
							<xsl:attribute name="Unit">
								<xsl:value-of select="MinMaxUnit" />
							</xsl:attribute>
							<xsl:value-of select="Max" />
						</MaximumBooking>
					</xsl:if> -->
          </VehAvailCore>
        </VehAvail>
      </VehAvails>
    </VehVendorAvail>
  </xsl:template>
  <xsl:template match="VErrorInfo" mode="error">
    <Error Type="Galileo">
      <xsl:attribute name="Code">
        <xsl:value-of select="Num"/>
      </xsl:attribute>
      <xsl:value-of select="Msg"/>
    </Error>
  </xsl:template>
  <xsl:template match="VErrInfo" mode="Warning">
    <Warning Type="Galileo">
      <xsl:attribute name="Code">
        <xsl:value-of select="Num"/>
      </xsl:attribute>
      <xsl:if test="Vnd!=''">
        <xsl:value-of select="Vnd"/>
        <xsl:text> - </xsl:text>
      </xsl:if>
      <xsl:value-of select="Msg"/>
    </Warning>
  </xsl:template>
  <xsl:template match="ErrQual">
    <Error Type="Galileo">
      <xsl:attribute name="Code">
        <xsl:value-of select="Err"/>
      </xsl:attribute>
      <xsl:value-of select="Txt"/>
    </Error>
  </xsl:template>
</xsl:stylesheet>
