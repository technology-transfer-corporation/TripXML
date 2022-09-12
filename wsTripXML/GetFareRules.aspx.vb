Imports System
Imports System.Xml
Imports System.IO
Partial Public Class GetFareRules
    Inherits System.Web.UI.Page
    Public RPTDetails As Repeater
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Try
                txtPNR.Text = Request.QueryString("pnr")

                If txtPNR.Text.Trim().Length > 0 Then
                    processPNR(txtPNR.Text)
                    Response.Redirect("RulesResult.aspx", True)
                End If


            Catch ex As Exception

            End Try
        End If

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click

        If txtPNR.Text.Trim().Length > 0 Then
            processPNR(txtPNR.Text)
            Dim sb2 As StringBuilder = New StringBuilder()

            'sb2 = sb2.Append("<script type=""text/javascript"">")
            sb2 = sb2.Append("window.open('RulesResult.aspx', '', '');")
            'sb2 = sb2.Append("</script>")
            Page.ClientScript.RegisterStartupScript(GetType(String), "MyRules", sb2.ToString(), True)
        End If

    End Sub
    Private Sub processPNR(ByVal PNR As String)
        Dim req As String = ""
        Dim sb As StringBuilder = New StringBuilder()
        sb = sb.Append("<OTA_ReadRQ>")
        sb = sb.Append("<POS><Source PseudoCityCode=""")
        sb = sb.Append("ATL1S2157")
        sb = sb.Append("""><RequestorID Type=""21"" ID=""")
        sb = sb.Append("RussiaHouse")
        sb = sb.Append("""/> </Source><TPA_Extensions><Provider><Name>")
        sb = sb.Append("Amadeus")
        sb = sb.Append("</Name><System>")
        sb = sb.Append("Production")
        sb = sb.Append("</System><Userid>")
        sb = sb.Append("morqua")
        sb = sb.Append("</Userid> <Password>")
        sb = sb.Append("sm1rN0ff")
        sb = sb.Append("</Password></Provider></TPA_Extensions></POS><UniqueID ID=""")
        sb = sb.Append(PNR.Trim())
        sb = sb.Append("""  /></OTA_ReadRQ>")

        req = sb.ToString()
        sb = sb.Remove(0, sb.Length)

        Dim strResponse As String
        Dim PNRRead As wsTravelTalk.wsPNRRead_v03 = New wsTravelTalk.wsPNRRead_v03()
        strResponse = PNRRead.wmPNRReadXml(req)

        Dim oDoc As XmlDocument = Nothing
        Dim oRoot As XmlElement = Nothing

        Dim oNode As XmlNode = Nothing
        Dim oNode2 As XmlNode = Nothing

        Dim sbFlights As StringBuilder = New StringBuilder()
        Dim seats As Integer = 0
        Dim AdtCount As Integer = 0
        Dim CHDCount As Integer = 0
        Dim INFCount As Integer = 0

        Try

            oDoc = New XmlDocument
            oDoc.LoadXml(strResponse)
            oRoot = oDoc.DocumentElement


            For Each oNode In oRoot.SelectNodes("TravelItinerary/ItineraryInfo/ReservationItems/Item")
                For Each oNode2 In oNode.SelectNodes("Air")
                    sbFlights = sbFlights.Append("<FlightSegment DepartureDateTime=""").Append(oNode2.Attributes("DepartureDateTime").Value).Append(""" ArrivalDateTime=""").Append(oNode2.Attributes("ArrivalDateTime").Value).Append(""" FlightNumber=""").Append(oNode2.Attributes("FlightNumber").Value).Append(""" ResBookDesigCode=""").Append(oNode2.Attributes("ResBookDesigCode").Value).Append(""">")
                    sbFlights = sbFlights.Append("<DepartureAirport LocationCode=""").Append(oNode2.SelectSingleNode("DepartureAirport").Attributes("LocationCode").Value).Append(""" />")
                    sbFlights = sbFlights.Append("<ArrivalAirport LocationCode=""").Append(oNode2.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value).Append(""" />")
                    sbFlights = sbFlights.Append("<MarketingAirline Code=""").Append(oNode2.SelectSingleNode("MarketingAirline").Attributes("Code").Value).Append(""" />")
                    sbFlights = sbFlights.Append("</FlightSegment>")
                    seats = CType(oNode2.Attributes("NumberInParty").Value, Integer)
                Next

                'If Not oNode.SelectSingleNode("DepartureAirport") Is Nothing Then

                'End If
                'If Not oNode.SelectSingleNode("ArrivalAirport") Is Nothing Then
                '    ' oNode.SelectSingleNode("ArrivalAirport").InnerText = GetDecodeValue(ttAirports, oNode.SelectSingleNode("ArrivalAirport").Attributes("LocationCode").Value)
                'End If


            Next

            For Each oNode In oRoot.SelectNodes("TravelItinerary/CustomerInfos/CustomerInfo")

                If oNode.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes("NameType").Value = "ADT" Then
                    AdtCount = AdtCount + 1
                End If
                If oNode.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes("NameType").Value = "CHD" Then
                    CHDCount = CHDCount + 1
                End If
                If oNode.SelectSingleNode("Customer").SelectSingleNode("PersonName").Attributes("NameType").Value = "INF" Then
                    INFCount = INFCount + 1
                End If

            Next

            sb = sb.Append("<OTA_AirRulesRQ>")
            sb = sb.Append("<POS><Source PseudoCityCode=""")
            sb = sb.Append("ATL1S2157")
            sb = sb.Append("""><RequestorID Type=""21"" ID=""")
            sb = sb.Append("RussiaHouse")
            sb = sb.Append("""/> </Source><TPA_Extensions><Provider><Name>")
            sb = sb.Append("Amadeus")
            sb = sb.Append("</Name><System>")
            sb = sb.Append("Production")
            sb = sb.Append("</System><Userid>")
            sb = sb.Append("morqua")
            sb = sb.Append("</Userid> <Password>")
            sb = sb.Append("sm1rN0ff")
            sb = sb.Append("</Password></Provider></TPA_Extensions></POS>")
            sb = sb.Append("<AirItinerary>")
            sb = sb.Append("<OriginDestinationOptions>")
            sb = sb.Append("<OriginDestinationOption>")
            sb = sb.Append(sbFlights.ToString())
            sb = sb.Append("</OriginDestinationOption>")
            sb = sb.Append("</OriginDestinationOptions>")

            sb = sb.Append("</AirItinerary>")
            sb = sb.Append("<TravelerInfoSummary>")
            sb = sb.Append("<SeatsRequested>").Append(seats.ToString()).Append("</SeatsRequested>")
            sb = sb.Append("<AirTravelerAvail>")
            If AdtCount > 0 Then
                sb = sb.Append("<PassengerTypeQuantity Code=""ADT"" Quantity=""").Append(AdtCount.ToString()).Append(""" />")
            End If
            If CHDCount > 0 Then
                sb = sb.Append("<PassengerTypeQuantity Code=""CHD"" Quantity=""").Append(CHDCount.ToString()).Append(""" />")
            End If
            If INFCount > 0 Then
                sb = sb.Append("<PassengerTypeQuantity Code=""INF"" Quantity=""").Append(INFCount.ToString()).Append(""" />")
            End If
            sb = sb.Append("</AirTravelerAvail>")

            Try

                If Not (oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing") Is Nothing) Then

                    If Not (oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo") Is Nothing) Then
                        If Not (oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo").Attributes("PricingSource") Is Nothing) Then
                            sb = sb.Append("<PriceRequestInformation PricingSource=""").Append(oRoot.SelectSingleNode("TravelItinerary/ItineraryInfo/ReservationItems/ItemPricing/AirFareInfo").Attributes("PricingSource").Value).Append(""" />")
                        Else
                            sb = sb.Append("<PriceRequestInformation PricingSource=""Published"" />")
                        End If
                    Else
                        sb = sb.Append("<PriceRequestInformation PricingSource=""Published"" />")
                    End If
                Else
                    sb = sb.Append("<PriceRequestInformation PricingSource=""Published"" />")
                End If




            Catch ex As Exception

            End Try


            sb = sb.Append("</TravelerInfoSummary>")
            sb = sb.Append("</OTA_AirRulesRQ>")

            req = sb.ToString()
            sb = sb.Remove(0, sb.Length)

            Dim AirRule As wsTravelTalk.wsAirRules_v03 = New wsTravelTalk.wsAirRules_v03()
            strResponse = AirRule.wmAirRulesXml(req)

            'Dim reader As StreamReader = New StreamReader(Server.MapPath("AirRulesRS2.xml"))
            'strResponse = reader.ReadToEnd()

            oDoc = New XmlDocument
            oDoc.LoadXml(strResponse)
            oRoot = oDoc.DocumentElement

            Dim nodeList As XmlNodeList = oDoc.GetElementsByTagName("SubSection")
            'RPTHeader.DataSource = nodeList
            'RPTHeader.DataBind()
            Session("RulesNodes") = nodeList

            

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub RPTHeader_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles RPTHeader.ItemDataBound
        
        Dim node As XmlNode
        node = CType(e.Item.DataItem, XmlNode)

        Dim lbl As Label = CType(e.Item.FindControl("lblTitle"), Label)
        lbl.Text = node.Attributes("SubTitle").Value
        Dim ndList As XmlNodeList = node.SelectNodes("Paragraph")



        Dim tb As Table = CType(e.Item.FindControl("tb"), Table)
        For Each oNode In ndList

            Dim row As TableRow = New TableRow()
            Dim cell As TableCell = New TableCell()
            cell.Text = oNode.SelectSingleNode("Text").InnerText
            row.Cells.Add(cell)
            tb.Rows.Add(row)
        Next
        'RPTDetails = CType(e.Item.FindControl("RPTDetails"), Repeater)



        'RPTDetails.DataSource = ndList
        'RPTDetails.DataBind()





    End Sub

   






End Class